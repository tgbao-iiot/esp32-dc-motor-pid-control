#include "web_server.h"
#include "motor_pid.h"

#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "esp_wifi.h"
#include "esp_event.h"
#include "esp_netif.h"
#include "esp_log.h"
#include "nvs_flash.h"
#include "esp_http_server.h"
#include "cJSON.h"

static const char *TAG = "WEB_SERVER";

/* ================= CÁC HÀM XỬ LÝ HTTP (HANDLERS) ================= */
static esp_err_t get_config_handler(httpd_req_t *req) {
    pid_config_t config;
    pid_get_config(&config); 

    char resp[32];
    if (strstr(req->uri, "/kp")) {
        snprintf(resp, sizeof(resp), "%.3f", config.kp);
    } else if (strstr(req->uri, "/ki")) {
        snprintf(resp, sizeof(resp), "%.3f", config.ki);
    } else if (strstr(req->uri, "/kd")) {
        snprintf(resp, sizeof(resp), "%.3f", config.kd);
    } else {
        httpd_resp_send_err(req, HTTPD_404_NOT_FOUND, "Not found");
        return ESP_FAIL;
    }

    httpd_resp_send(req, resp, HTTPD_RESP_USE_STRLEN);
    return ESP_OK;
}

static esp_err_t get_status_handler(httpd_req_t *req) {
    motor_status_t status;
    pid_get_status(&status);

    char resp[32];
    if (strstr(req->uri, "/rpm")) {
        snprintf(resp, sizeof(resp), "%" PRIi32, status.current_rpm);
    } else if (strstr(req->uri, "/target")) {
        snprintf(resp, sizeof(resp), "%" PRIi32, status.target_rpm);
    } else if (strstr(req->uri, "/pwm")) {
        snprintf(resp, sizeof(resp), "%d", status.pwm_output);
    } else {
        httpd_resp_send_err(req, HTTPD_404_NOT_FOUND, "Not found");
        return ESP_FAIL;
    }

    httpd_resp_send(req, resp, HTTPD_RESP_USE_STRLEN);
    return ESP_OK;
}

static esp_err_t root_handler(httpd_req_t *req) {
    httpd_resp_send(req, "ESP32 Ready", HTTPD_RESP_USE_STRLEN);
    return ESP_OK;
}

static esp_err_t post_config_handler(httpd_req_t *req)
{
    char buf[256];
    int ret, remaining = req->content_len;
    if(remaining >= sizeof(buf))
    {
        httpd_resp_send_err(req, HTTPD_500_INTERNAL_SERVER_ERROR, "Payload too large");
        return ESP_FAIL;
    }

    ret = httpd_req_recv(req, buf, remaining);
    if (ret <= 0) {
        return ESP_FAIL;
    }
    buf[ret] = '\0';

    cJSON *json = cJSON_Parse(buf);
    if (json == NULL) {
        httpd_resp_send_err(req, HTTPD_400_BAD_REQUEST, "Invalid JSON Format");
        return ESP_FAIL;
    }

    cJSON *kp = cJSON_GetObjectItem(json, "kp");
    cJSON *ki = cJSON_GetObjectItem(json, "ki");
    cJSON *kd = cJSON_GetObjectItem(json, "kd");
    cJSON *target = cJSON_GetObjectItem(json, "target");

    if(cJSON_IsNumber(kp) && cJSON_IsNumber(ki) && cJSON_IsNumber(kd) && cJSON_IsNumber(target))
    {
        pid_set_config((float)kp->valuedouble,
                       (float)ki->valuedouble,
                       (float)kd->valuedouble, 
                       (int32_t)target->valueint);

        ESP_LOGI(TAG, "Cap nhat POST JSON: Kp=%.2f, Ki=%.2f, Kd=%.2f, Target=%d", kp->valuedouble, ki->valuedouble, kd->valuedouble, target->valueint);

        httpd_resp_send(req, "OK", HTTPD_RESP_USE_STRLEN);
    } else {
        httpd_resp_send_err(req, HTTPD_400_BAD_REQUEST, "Missing or Invalid JSON Fields");
    }
    cJSON_Delete(json);
    return ESP_OK;
}
/* ================= KHỞI TẠO WEB SERVER ================= */

static httpd_handle_t start_webserver(void) {
    httpd_config_t config = HTTPD_DEFAULT_CONFIG();
    httpd_handle_t server = NULL;

    if (httpd_start(&server, &config) == ESP_OK) {

        httpd_uri_t root_uri = { .uri = "/", .method = HTTP_GET, .handler = root_handler };
        httpd_register_uri_handler(server, &root_uri);

        httpd_uri_t rpm_uri = { .uri = "/rpm", .method = HTTP_GET, .handler = get_status_handler };
        httpd_register_uri_handler(server, &rpm_uri);

        httpd_uri_t target_uri = { .uri = "/target", .method = HTTP_GET, .handler = get_status_handler };
        httpd_register_uri_handler(server, &target_uri);

        httpd_uri_t pwm_uri = { .uri = "/pwm", .method = HTTP_GET, .handler = get_status_handler };
        httpd_register_uri_handler(server, &pwm_uri);

        httpd_uri_t kp_uri = { .uri = "/kp", .method = HTTP_GET, .handler = get_config_handler };
        httpd_register_uri_handler(server, &kp_uri);

        httpd_uri_t ki_uri = { .uri = "/ki", .method = HTTP_GET, .handler = get_config_handler };
        httpd_register_uri_handler(server, &ki_uri);

        httpd_uri_t kd_uri = { .uri = "/kd", .method = HTTP_GET, .handler = get_config_handler };
        httpd_register_uri_handler(server, &kd_uri);

        httpd_uri_t post_config_uri = { .uri = "/api/config", .method   = HTTP_POST, .handler  = post_config_handler };
        httpd_register_uri_handler(server, &post_config_uri);
    }
    return server;   
}

/* ================= KHỞI TẠO WI-FI ================= */

static void wifi_init_softap(void) {
    ESP_ERROR_CHECK(esp_netif_init());
    ESP_ERROR_CHECK(esp_event_loop_create_default());
    esp_netif_create_default_wifi_ap();

    wifi_init_config_t cfg = WIFI_INIT_CONFIG_DEFAULT();
    ESP_ERROR_CHECK(esp_wifi_init(&cfg));

    wifi_config_t ap_config = {
        .ap = {
            .ssid = "ESP32_WIFI",
            .ssid_len = strlen("ESP32_WIFI"),
            .password = "12345678",
            .channel = 1,
            .max_connection = 4,
            .authmode = WIFI_AUTH_WPA_WPA2_PSK
        },
    };

    ESP_ERROR_CHECK(esp_wifi_set_mode(WIFI_MODE_AP));
    ESP_ERROR_CHECK(esp_wifi_set_config(WIFI_IF_AP, &ap_config));
    ESP_ERROR_CHECK(esp_wifi_start());
    ESP_LOGI(TAG, "WiFi AP started. SSID:%s", ap_config.ap.ssid);
}

/* ================= TASK CHÍNH ================= */

void wifi_web_task(void *pvParameters) {
    esp_err_t ret = nvs_flash_init();
    if (ret == ESP_ERR_NVS_NO_FREE_PAGES || ret == ESP_ERR_NVS_NEW_VERSION_FOUND) {
        ESP_ERROR_CHECK(nvs_flash_erase());
        ret = nvs_flash_init();
    }
    ESP_ERROR_CHECK(ret);

    wifi_init_softap();
    start_webserver();
     
    ESP_LOGI(TAG, "Web server started successfully");
    vTaskDelete(NULL); 
}