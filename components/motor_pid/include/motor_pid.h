#ifndef MOTOR_PID_H
#define MOTOR_PID_H

#include <stdint.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"

// Các biến giao tiếp với Web Server
typedef struct {
    float kp;
    float ki;
    float kd;
    int32_t target_rpm;
} pid_config_t;

typedef struct {
    int32_t current_rpm;
    int32_t target_rpm;
    uint8_t pwm_output;
} motor_status_t;


// Các API public
void pid_set_config(float kp, float ki, float kd, int32_t target_rpm);
void pid_get_config(pid_config_t *config);
void pid_get_status(motor_status_t *status);

void motor_init(void);
void pid_control_task(void *pvParameters);

#endif