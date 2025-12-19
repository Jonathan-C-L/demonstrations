/*
  Periodic Interrupt Timer (PIT) Header File
  FILE: pit.h
  Created on: October 24th, 2022, by Carlos Estay
  Edited Oct. 20th, 2023 by Jonathan Le
*/

typedef enum PIT_ChannelTyepedef_
{
    PIT_CH0 = 0b00000001,
    PIT_CH1 = 0b00000010,
    PIT_CH2 = 0b00000100,
    PIT_CH3 = 0b00001000
}PIT_Channel;

typedef enum PIT_MicroTimerTyepedef_
{
    PIT_MT1 = 1,
    PIT_MT0 = 0
}PIT_MicroTimer;

typedef enum PIT_InterruptTyepedef_
{
    PIT_IEN = 1,
    PIT_IDIS = 0
}PIT_Interrupt;

/// @brief A blocking delay function in milliseconds
///        Fixed to microtimer 1.
/// @param ch The channel to use with the delay
/// @param ms The number of milliseconds to delay
void PIT_Sleep(PIT_Channel ch, unsigned int ms);

/// @brief Configures the channel to a 1[ms] event, fix connection to micro-timer1 
/// @param ch The channel to be configured
void PIT_Set1msDelay(PIT_Channel ch);

/// @brief Configures a channel (ch0 - ch3)
/// @param ch The channel in question (PIT_CH0 - PIT_CH3)
/// @param mt The micro-timer to be connected to (MT1 or MT0(default))
/// @param ie Enables or disables interrupt for the channel
void PIT_InitChannel(PIT_Channel ch, PIT_MicroTimer mt, PIT_Interrupt ie);

/// @brief Configures a specified microtimer and 16-bit timer channel to any custom timing (ticks must be calculated ahead of time)
///        The function will handle the (tick - 1) operations* 
///        Remember to check the current clock rate** 
/// @param ch The 16-bit timer to be configured
/// @param mt The 8-bit microtimer to be configured
/// @param timer8 The ticks for the 8-bit microtimer
/// @param timer16 The ticks for the 16-bit microtimer
void PIT_SetTiming_Custom(PIT_Channel ch, PIT_MicroTimer mt, unsigned char timer8, unsigned int timer16);

/// @brief PIT_Delay_us() sets a milisecond blocking delay using the PIT module (CAN ONLY DO miliseconds*)
/// @param ch 0 to 3 16-bit timers
/// @param mt 0 to 1 8-bit microtimers
/// @param us Specified milisecond delay
void PIT_Delay_ms(PIT_Channel ch, PIT_MicroTimer mt, unsigned int ms);

/// @brief PIT_Start() enables the PIT module to have the PIT timers start (MUST BE DONE AFTER PIT CHANNEL INITS*)
void PIT_Start(void);

/// @brief PIT_Delay_us() sets a microsecond blocking delay using the PIT module (CAN ONLY DO MICROSECONDS*)
/// @param ch 0 to 3 16-bit timers
/// @param mt 0 to 1 8-bit microtimers
/// @param us Specified microsecond delay
void PIT_Delay_us(PIT_Channel ch, PIT_MicroTimer mt, unsigned int us);

/// @brief PIT_AssignFunction() assigns the pointer to the specified address for the specified channel
///        Interrupts MUST be enabled, otherwise nothing will happen*
/// @param ch 0 to 3 16-bit timer channels
void PIT_AssignFunction(PIT_Channel ch, void (*pFunction)(void));