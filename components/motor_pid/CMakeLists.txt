idf_component_register(
    SRCS "motor_pid.c"
    INCLUDE_DIRS "include"
    REQUIRES driver esp_driver_gpio esp_driver_ledc freertos esp_timer esp_driver_pcnt nvs_flash
)