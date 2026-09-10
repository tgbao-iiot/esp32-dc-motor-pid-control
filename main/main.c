#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "motor_pid.h"
#include "web_server.h"

void app_main(void)
{
    // 1. Khởi tạo toàn bộ ngoại vi động cơ & encoder
    motor_init();

    // 2. Chạy luồng điều khiển PID (Gán cho Core 0)
    xTaskCreatePinnedToCore(pid_control_task, "pid_control_task", 4096, NULL, 5, NULL, 0);

    // 3. Chạy luồng phát Wi-Fi AP và Web Server (Gán cho Core 1)
    xTaskCreatePinnedToCore(wifi_web_task, "wifi_web_task", 8192, NULL, 3, NULL, 1);
}