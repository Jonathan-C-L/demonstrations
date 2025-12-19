/*
  Enhanced Capture Timer (ECT) Header File
  FILE: ect.h
  Created on: Nov. 16st, 2025, by Jonathan Le
*/
/**************************************************
ECT Pins:
TCx     Port Pin    Micro Pin     
TC0     PT0         9      
TC1     PT1         10      
TC2     PT2         11      
TC3     PT3         12      
TC4     PT4         15      
TC5     PT5         16      
TC6     PT6         17      
TC7     PT7         18      
**************************************************/
// timer will count based on the value in the TSCR2 register (prescaler)
/**************************************************
Prescaler Selection: (7.3.2.11) (Table 7-15)
PR2     PR1     PR0     Prescale Factor
0       0       0       1
0       0       1       2
0       1       0       4
0       1       1       8
1       0       0       16
1       0       1       32
1       1       0       64
1       1       1       128
**************************************************/
// To set prescale timing, '|=' enum to TSCR2
// Or individually '=' 1 or 0 to TSCR2_PRx
// If precision timer is enabled TRSCR1_PRNT, can set timing by assigning to PTPSR register (0 counts so -1)
typedef enum ECT_PrescaleTypedef{
    ECT_PreS1 = 0,
    ECT_PreS2 = 1U,
    ECT_PreS4 = 2U,
    ECT_PreS8 = 1U | 2U,
    ECT_PreS16 = 4U,
    ECT_PreS32 = 1U | 4U,
    ECT_PreS64 = 2U | 4U,
    ECT_PreS128 = 1U | 2U | 4U
} ECT_Prescale;
// To enable timer overflow interrupt, '|=' enum with TSCR2 
// Or individually '=' 1 or 0 to TSCR2_TOI
typedef enum ECT_TimerRollOverInterruptTypedef{
    ECT_RO_IEN = 128U,
    ECT_RO_IDIS = 0
} ECT_TimerRollOverInterrupt;
// To enable channel 7 timer reset, '|=' enum with TSCR2
// Or individually '=' 1 or 0 to TSCR2_TCRE
typedef enum ECT_TimerCounterResetTypedef{
    ECT_TReset_EN = 8U,
    ECT_TReset_DIS = 0
} ECT_TimerCounterReset;
/******************** General channel select to be used for TIOS (7.3.2.1) or OC7M (7.3.2.3) or TIE (7.3.2.10) ********************/
// To enable channels as outputs, '|=' enum to TIOS or individually '=' 1 or 0 to TIOS_IOSx
// To connect channels to a channel 7 compare event, '|=' enum with OC7M or individually '=' 1 or  0 to OC7M_OC7Mx
// If the specific channel for the OC7M is set high, a 1 or 0 can be sent to the specified channel on a channel 7 compare
//  -> 0 is the default, but to send a 1, '|=' enum with OC7D
// To enable the Timer Interrupt, '|=' enum to TIE or individually '=' 1 or 0 to TIE_CxI
typedef enum ECT_EnableOutputTypedef{
    ECT_CH0 = 1U,
    ECT_CH1 = 2U,
    ECT_CH2 = 4U,
    ECT_CH3 = 8U,
    ECT_CH4 = 16U,
    ECT_CH5 = 32U,
    ECT_CH6 = 64U,
    ECT_CH7 = 128U
} ECT_EnableOutput;
/*****************************************
Compare Result Output Action - TCTL1/TCTL2 (7.3.2.8) (Table 7-10)
OMx     OLx     Action
0       0       Timer Disconnected from pin
0       1       Toggle OCx output line
1       0       Clear OCx output line to zero
1       1       Set OCx output line to one
*****************************************/
// To set output action, '|=' enum to TCTL1 
// Or individually '=' 1 or 0 to TCTL1_OMx/TCTL1_OLx
typedef enum ECT_CompareAction4to7Typedef{
    ECT_CH4to7_DC = 0,
    ECT_CH4_TOG = 1U,
    ECT_CH4_CLR = 2U,
    ECT_CH4_SET = 1U | 2U,

    ECT_CH5_TOG = 4U,
    ECT_CH5_CLR = 8U,
    ECT_CH5_SET = 4U | 8U,

    ECT_CH6_TOG = 16U,
    ECT_CH6_CLR = 32U,
    ECT_CH6_SET = 16U | 32U,

    ECT_CH7_TOG = 64U,
    ECT_CH7_CLR = 128U,
    ECT_CH7_SET = 64U | 128U,
} ECT_CompareAction4to7;
// To set output action, '|=' enum to TCTL2 
// Or individually '=' 1 or 0 to TCTL2_OMx/TCTL2_OLx
typedef enum ECT_CompareAction0to3Typedef{
    ECT_CH0to3_DC = 0,
    ECT_CH0_TOG = 1U,
    ECT_CH0_CLR = 2U,
    ECT_CH0_SET = 1U | 2U,

    ECT_CH1_TOG = 4U,
    ECT_CH1_CLR = 8U,
    ECT_CH1_SET = 4U | 8U,

    ECT_CH2_TOG = 16U,
    ECT_CH2_CLR = 32U,
    ECT_CH2_SET = 16U | 32U,

    ECT_CH3_TOG = 64U,
    ECT_CH3_CLR = 128U,
    ECT_CH3_SET = 64U | 128U,
} ECT_CompareAction0to3;
// To set timer controls like start, fast flag, wait, freeze, and precision timing, '|=' enum to TSCR1
// Or individually '=' 1 or 0 to TSCR1_bits
typedef enum ECT_TimerControlTypedef{
    ECT_TimerStart = 128U,
    ECT_FFC = 16U,
    ECT_WAIT = 64U,
    ECT_FRZ = 32U,
    ECT_PRECISIONTIMER_EN = 8U
} ECT_TimerControl;
 /*****************************************
  Edge Detector Circuit Configuration - TCTL3 (Ch1, 3, 5, 7)/TCTL4 (Ch0, 2, 4, 6) (7.3.2.9) (Table 7-12)
  EDGxB  EDGxA
    0      0    Capture disabled
    0      1    Capture on rising edges only
    1      0    Capture on falling edges only
    1      1    Capture on any edge (rising or falling)
  *****************************************/




// General bits to use with ECT registers if needed
typedef enum ECT_BitsTypedef{
    ECT_HIGH = 1,
    ECT_LOW = 0
} ECT_Bits;