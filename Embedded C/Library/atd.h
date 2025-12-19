/*
  Analog to Digital (ATD) Header File
  FILE: atd.h
  Created on: Nov. 1st, 2025, by Jonathan Le
*/
/****************************************
ATD0 Channel    Label       PIN No.
AN7             PAD07       81
AN6             PAD06       79
AN5             PAD05       77
AN4             PAD04       75
AN3             PAD03       73
AN2             PAD02       71
AN1             PAD01       69
AN0             PAD00       67
****************************************/
// To configure settings for ATD, '|=' with ATD0CTL2
typedef enum ATD_ConfigOptTypedef{
    ATD_ON = 128U,
    ATD_FFC = 64U,
    ATD_PD_WAIT = 32U,
    ATD_EXT_TRIGLE_HIGH = 16U,
    ATD_EXT_TRIGP_HIGH = 8U,
    ATD_EXT_TRIG_EN = 4U,
    ATD_IEN = 2U,
    ATD_IDIS = 0,
    ATD_CONFIG_INACTIVE = 0
} ATD_ConfigOpt;
// To set the number of conversions per sequence, '|=' with ATD0CTL3
typedef enum ATD_NumConversionTypedef{
    ATD_8CONV = 64U,
    ATD_7CONV = 32U | 16U | 8U,
    ATD_6CONV = 32U | 16U,
    ATD_5CONV = 32U | 8U,
    ATD_4CONV = 32U,
    ATD_3CONV = 16U | 8U,
    ATD_2CONV = 16U,
    ATD_1CONV = 8U,
    ATD_FIFO = 4U
} ATD_NumConversion;
// To set the Freeze Mode behaviour, '|=' with ATD0CTL3
typedef enum ATD_FreezeModeTypedef{
    ATD_FRZ_CONT = 0,
    ATD_FRZ_RES = 1U,
    ATD_FRZ_FIN = 2U,
    ATD_FRZ_STOP = 1U | 2U
} ATD_FreezeMode;
// To set the format of the conversions, '|=' with ATDOCTL5
typedef enum ATD_DataFormatTypedef{
    ATD_LJ_U = 0,
    ATD_LJ_S = 64U,
    ATD_RJ_U = 128U
} ATD_DataFormat;
// To set the continuous scanning and/or multichannel sampling, '|=' with ATD0CTL5
typedef enum ATD_SamplingModeTypedef{
    ATD_SCAN = 32U,
    ATD_MULT = 16U,
    ATD_NOSCAN = 0,
    ATD_SINGLE = 0,
    ATD_MODE_INACTIVE = 0
} ATD_SamplingMode;
// To choose a channel for wrapping (ATD0CTL0) and starting (ATD0CTL5), '|=' with enum on the specific register
typedef enum ATD_ChannelSelectTypedef{
    ATD_CH0 = 0,
    ATD_CH1 = 1,
    ATD_CH2 = 2,
    ATD_CH3 = 3,
    ATD_CH4 = 4,
    ATD_CH5 = 5,
    ATD_CH6 = 6,
    ATD_CH7 = 7
} ATD_ChannelSelect;
// To set the sampling resolution, '|=' with ATD0CTL4
typedef enum ATD_ResolutionTypedef{
    ATD_8bit = 128U,
    ATD_10bit = 0
} ATD_Resolution;
// To set number of conversions per clock period, '|=' with ATD0CTL4
typedef enum ATD_SampleTimeTypedef{
    ATD_ST16 = 64U | 32U,
    ATD_ST8 = 64U,
    ATD_ST4 = 32U,
    ATD_ST2 = 0
} ATD_SampleTime;
// only have up to 20, but can add more later if needed
// To set prescale value, '|=' with ATD0CTL4
typedef enum ATD_PrescalerTypedef{
    ATD_PreS2 = 0,
    ATD_PreS4 = 1,
    ATD_PreS6 = 2,
    ATD_PreS8 = 3,
    ATD_PreS10 = 4,
    ATD_PreS12 = 5,
    ATD_PreS14 = 6,
    ATD_PreS16 = 7,
    ATD_PreS18 = 8,
    ATD_PreS20 = 9
} ATD_Prescaler;

/// @brief Configures the ATD options for the ATD Control Register 2 (5.3.2.3)
/// @param PowerON if PowerON is enabled, it will turn on the ATD, else power off (5.3.2.3)
/// @param FastFlag if FastFlag is enabled, it will turn enable the fast flag clear, else manually read CCF in ATDSTAT1 (5.3.2.3)
/// @param WaitMode if WaitMode is enabled, it will power down the ATD in wait mode, else ATD continues in wait mode (5.3.2.3)
/// @param Interrupt if Interrupt is enabled, it will turn on the ATD interrupt, else interrupts disabled (5.3.2.3)
/// @param SeqComplete if SeqComplete is enabled, it will turn on the ATD interrupt flag when an ATD sequence is complete (other options will remain) (5.3.2.3)
void ATD_Config(ATD_ConfigOpt PowerON, ATD_ConfigOpt FastFlag, ATD_ConfigOpt WaitMode, ATD_ConfigOpt Interrupt);

/// @brief Sets the AD conversion clock frequency, resolution, and number of conversions per period of the ATD clock
/// @param Resolution 8-bit or 10-bit resolution (the higher the more detail)
/// @param Prescaler value that will keep the conversion frequency between 2MHz and 500kHz (5.3.2.5) (Table 5-11) (Table 5-12)
/// @param SampleTime number of samples per clock period
void ATD_SetClock(ATD_Resolution Resolution, ATD_Prescaler Prescaler, ATD_SampleTime SampleTime);

/// @brief Sets the channel to wrap the conversions at (ensure mult is enabled)
/// @param Ch the specific channel to wrap at (5.3.2.1) (Table 5-2)
void ATD_SetWrap(ATD_ChannelSelect Ch);

/// @brief Sets the number of conversions to perform for each sequence and the behaviour when conversions are frozen (5.3.2.4) (Table 5-8)
/// @param NumConversions the number of conversions per sequence
/// @param FrzMode behaviour of the conversion in freeze mode
void ATD_SetNumConversions(ATD_NumConversion NumConversions, ATD_FreezeMode FrzMode);

/// @brief Sets how if the conversions will automatically reset, multi/single channel, and the starting channel for the conversions
/// @param Format right or left justified for the resulting data
/// @param Scan continuous vs single conversion sequence
/// @param Mult multi-channel or single channel sampling
/// @param StartCh starting channel for the conversions
void ATD_SetScanBehaviour(ATD_DataFormat Format, ATD_SamplingMode Scan, ATD_SamplingMode Mult, ATD_ChannelSelect StartCh);

/// @brief Will assign a callback function to the function pointer and executes the callback in the interrupt**
/// @param pFunction The address of the callback function
void ATD_SetCallback(void (*pFunction)(void));