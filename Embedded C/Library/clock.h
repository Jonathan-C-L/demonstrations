// Clock library
// Jonathan Le - Feb. 27, 2025

typedef enum ClockDivTypedef
{
    ClockOutDiv1 = 0b00000000,
    ClockOutDiv2 = 0b00000001,
    ClockOutDiv3 = 0b00000010,
    ClockOutDiv4 = 0b00000011
} ClockOutDiv;

void Clock_Set2MHZ(void);
void Clock_Set8MHZ(void);
void Clock_Set20MHZ(void);
void Clock_Set34MHZ(void);
void Clock_Set40MHZ(void);
void Clock_EnableOutput(void);
unsigned long Clock_GetBusSpeed(void);
