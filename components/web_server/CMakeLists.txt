idf_component_register(
    SRCS "web_server.c"
    INCLUDE_DIRS "include"
    REQUIRES nvs_flash esp_wifi esp_netif esp_http_server motor_pid freertos cjson
)