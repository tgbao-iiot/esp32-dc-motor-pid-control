#include "motor_pid.h"
#include "driver/gpio.h"
#include "driver/ledc.h"
#include "driver/pulse_cnt.h"
#include "esp_log.h"
#include <math.h>
#include <stdlib.h>
#include "nvs_flash.h"
#include "nvs.h"

static const char *TAG = "MOTOR_PID";

#define IN1_GPIO GPIO_NUM_22
#define IN2_GPIO GPIO_NUM_23
#define ENA_PWM_GPIO GPIO_NUM_21
#define ENCODER_A_PIN GPIO_NUM_18
#define ENCODER_B_PIN GPIO_NUM_19
#define PULSES_PER_REV 330
#define NVS_NAMESPACE "pid_store"

static pid_config_t g_pid_config = {
    .kp = 0.0f,
    .ki = 0.0f,
    .kd = 0.0f,
    .target_rpm = 0
};

static motor_status_t g_motor_status = {
    .current_rpm = 0,
    .target_rpm = 0,
    .pwm_output = 0
};

static pcnt_unit_handle_t pcnt_unit_handle = NULL;
static pcnt_channel_handle_t pcnt_chan_handle = NULL;
static SemaphoreHandle_t g_pid_mutex = NULL;

// Hàm lưu đọc NVS
static esp_err_t save_pid_to_nvs(pid_config_t *config)
{
    nvs_handle_t nvs_handle;
    esp_err_t err = nvs_open(NVS_NAMESPACE, NVS_READWRITE, &nvs_handle);
    if(err != ESP_OK)
    {
        ESP_LOGE(TAG, "Loi mo NVS: %s", esp_err_to_name(err));
        return err;
    }

    err = nvs_set_blob(nvs_handle, "pid_config", config, sizeof(pid_config_t));
    if (err == ESP_OK) {
        err = nvs_commit(nvs_handle); 
        ESP_LOGI(TAG, "Luu cau hinh PID vao NVS thanh cong!");
    } else {
        ESP_LOGE(TAG, "Loi ghi blob NVS: %s", esp_err_to_name(err));
    }

    nvs_close(nvs_handle);
    return err;
}

static esp_err_t load_pid_from_nvs(pid_config_t *config)
{
    nvs_handle_t nvs_handle;
    esp_err_t err = nvs_open(NVS_NAMESPACE, NVS_READWRITE, &nvs_handle);
    if(err != ESP_OK)
    {
        ESP_LOGE(TAG, "Loi mo NVS: %s", esp_err_to_name(err));
        return err;
    }

    size_t required_size = sizeof(pid_config_t);
    err = nvs_get_blob(nvs_handle, "pid_config", config, &required_size);
    if (err == ESP_OK) {
        ESP_LOGI(TAG, "Load PID tu NVS: Kp=%.3f, Ki=%.3f, Kd=%.3f, Target=%" PRIi32,
                 config->kp, config->ki, config->kd, config->target_rpm);
    } else {
        ESP_LOGW(TAG, "Loi doc blob NVS: %s", esp_err_to_name(err));
    }

    nvs_close(nvs_handle);
    return err;
}

// Bộ lọc IIR 
// y[n] = alpha * x[n] + (1 - alpha) * y[n-1]
static float iir_filter(float input_rpm) {
    static float filtered_rpm = 0;
    float alpha = 0.05f;
    filtered_rpm = alpha * input_rpm + (1.0f - alpha) * filtered_rpm;
    return filtered_rpm;
}

void pid_set_config(float kp, float ki, float kd, int32_t target_rpm) {
    if (g_pid_mutex != NULL && xSemaphoreTake(g_pid_mutex, pdMS_TO_TICKS(50)) == pdTRUE) {
        g_pid_config.kp = kp;
        g_pid_config.ki = ki;
        g_pid_config.kd = kd;
        g_pid_config.target_rpm = target_rpm;

        save_pid_to_nvs(&g_pid_config);

        xSemaphoreGive(g_pid_mutex);
    }
}

void pid_get_config(pid_config_t *config) {
    if (g_pid_mutex != NULL && xSemaphoreTake(g_pid_mutex, pdMS_TO_TICKS(50)) == pdTRUE) {
        *config = g_pid_config;
        xSemaphoreGive(g_pid_mutex);
    }
}

void pid_get_status(motor_status_t *status) {
    if (g_pid_mutex != NULL && xSemaphoreTake(g_pid_mutex, pdMS_TO_TICKS(50)) == pdTRUE) {
        *status = g_motor_status;
        xSemaphoreGive(g_pid_mutex);
    }
}

// Điều khiển động cơ
static void motor_set(int direction, uint8_t speed) {
    if (direction) {
        gpio_set_level(IN1_GPIO, 1);
        gpio_set_level(IN2_GPIO, 0);
    } else {
        gpio_set_level(IN1_GPIO, 0);
        gpio_set_level(IN2_GPIO, 1);
    }

    ledc_set_duty(LEDC_LOW_SPEED_MODE, LEDC_CHANNEL_0, speed);
    ledc_update_duty(LEDC_LOW_SPEED_MODE, LEDC_CHANNEL_0);
}

// Task PID 
void pid_control_task(void *pvParameters) {
    static int32_t prev_target_rpm = 0;
    static float prev_error = 0;
    static float integral = 0;
    static float derivative = 0;

    TickType_t last_wake_time = xTaskGetTickCount();

    while (1) {
        vTaskDelayUntil(&last_wake_time, pdMS_TO_TICKS(10)); 

        static pid_config_t local_config;
        
        if (xSemaphoreTake(g_pid_mutex, pdMS_TO_TICKS(2)) == pdTRUE) {
            local_config = g_pid_config;
            xSemaphoreGive(g_pid_mutex);
        }

        // Reset PID khi đổi chiều
        if ((prev_target_rpm > 0 && local_config.target_rpm < 0) || (prev_target_rpm < 0 && local_config.target_rpm > 0)) {
            integral = 0;
            prev_error = 0;
            derivative = 0;
        }
        if (abs(local_config.target_rpm - prev_target_rpm) > 50) {
            integral = 0;
            derivative = 0;
        }
        prev_target_rpm = local_config.target_rpm;

        // Đọc số xung
        int pulse_count_10ms = 0;
        pcnt_unit_get_count(pcnt_unit_handle, &pulse_count_10ms);
        pcnt_unit_clear_count(pcnt_unit_handle);

        // Tính toán RPM
        float revs = (float)pulse_count_10ms / PULSES_PER_REV;
        float rpm_cacl = revs * 60.0f / 0.01f; 

        // Lọc RPM
        float current_rpm = iir_filter(rpm_cacl);

        // Tính toán PID
        // Sai số
        float error = (float)local_config.target_rpm - current_rpm;

        // Khâu tích phân
        integral += error * 0.01f;
        if (integral > 500.0f) integral = 500.0f;
        if (integral < -500.0f) integral = -500.0f;

        // Khâu vi phân
        derivative = (error - prev_error) / 0.01f;
        if (derivative > 500.0f) derivative = 500.0f;
        if (derivative < -500.0f) derivative = -500.0f;

        prev_error = error;

        float output = local_config.kp * error + local_config.ki * integral + local_config.kd * derivative;

        // Xử lí PWM
        float abs_output = fabsf(output);
        if (output > 255.0f) output = 255.0f;
        uint8_t pwm_output = (uint8_t)abs_output;

        // Xuất tín hiệu động cơ
        if (local_config.target_rpm > 0) {
            motor_set(1, pwm_output);
        } else if (local_config.target_rpm < 0) {
            motor_set(0, pwm_output);
        } else {
            motor_set(0, 0);
            integral = 0;
            prev_error = 0;
        }

        ESP_LOGI(TAG, "Speed: %.2f | Target: %" PRIi32 " | PWM: %d", current_rpm, local_config.target_rpm, pwm_output);
        if (xSemaphoreTake(g_pid_mutex, pdMS_TO_TICKS(2)) == pdTRUE) {
            g_motor_status.current_rpm = (int32_t)current_rpm;
            g_motor_status.target_rpm = local_config.target_rpm;
            g_motor_status.pwm_output = pwm_output;
            xSemaphoreGive(g_pid_mutex);
        }
    }
}

// Khởi tạo phần cứng
void motor_init(void) {
    g_pid_mutex = xSemaphoreCreateMutex();
    if (g_pid_mutex == NULL) {
        ESP_LOGE(TAG, "Loi tao Mutex!");
        return;
    }

    pid_config_t loaded_cfg;
    if (load_pid_from_nvs(&loaded_cfg) == ESP_OK) {
        g_pid_config = loaded_cfg;
    } else {
        ESP_LOGI(TAG, "Dung cau hinh PID mac dinh (0.0)");
    }
    
    gpio_set_direction(IN1_GPIO, GPIO_MODE_OUTPUT);
    gpio_set_direction(IN2_GPIO, GPIO_MODE_OUTPUT);

    // Cấu hình PWM LEDC
    ledc_timer_config_t pwm_timer = {
        .duty_resolution = LEDC_TIMER_8_BIT,
        .freq_hz = 5000,
        .speed_mode = LEDC_LOW_SPEED_MODE,
        .timer_num = LEDC_TIMER_0,
        .clk_cfg = LEDC_AUTO_CLK
    };
    ledc_timer_config(&pwm_timer);

    ledc_channel_config_t pwm_channel = {
        .channel = LEDC_CHANNEL_0,
        .duty = 0,
        .gpio_num = ENA_PWM_GPIO,
        .speed_mode = LEDC_LOW_SPEED_MODE,
        .timer_sel = LEDC_TIMER_0
    };
    ledc_channel_config(&pwm_channel);

    //Cấu hình PCNT
    pcnt_unit_config_t pcnt_config = {
        .high_limit = 32767, //giới hạn bộ đếm
        .low_limit = -32768,
    };
    ESP_ERROR_CHECK(pcnt_new_unit(&pcnt_config ,&pcnt_unit_handle));

    pcnt_glitch_filter_config_t filter_config = {
        .max_glitch_ns = 1000,
    };
    ESP_ERROR_CHECK(pcnt_unit_set_glitch_filter(pcnt_unit_handle, &filter_config));

    pcnt_chan_config_t chan_config = {
        .edge_gpio_num = ENCODER_A_PIN,
        .level_gpio_num = ENCODER_B_PIN,
    };

    ESP_ERROR_CHECK(pcnt_new_channel(pcnt_unit_handle, &chan_config, &pcnt_chan_handle));

    gpio_pullup_en(ENCODER_A_PIN);
    gpio_pullup_en(ENCODER_B_PIN);

    ESP_ERROR_CHECK(pcnt_channel_set_edge_action(pcnt_chan_handle, PCNT_CHANNEL_EDGE_ACTION_INCREASE, PCNT_CHANNEL_EDGE_ACTION_HOLD));
    ESP_ERROR_CHECK(pcnt_channel_set_level_action(pcnt_chan_handle, PCNT_CHANNEL_LEVEL_ACTION_KEEP, PCNT_CHANNEL_LEVEL_ACTION_INVERSE));

    ESP_ERROR_CHECK(pcnt_unit_enable(pcnt_unit_handle));
    ESP_ERROR_CHECK(pcnt_unit_clear_count(pcnt_unit_handle));
    ESP_ERROR_CHECK(pcnt_unit_start(pcnt_unit_handle));
    ESP_LOGI(TAG, "Khoi tao Motor PWM va PCNT Encoder thanh cong!");
}