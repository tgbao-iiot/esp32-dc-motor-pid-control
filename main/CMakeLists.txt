idf_component_register(
    SRCS "main.c"
    INCLUDE_DIRS "."
    REQUIRES motor_pid web_server freertos
)