/********************************************************************/
// HC12 Program:  ICA7 - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       Using output compare to measure signals, then using those measured signals to vary the duty cycle of the output
//                from the microboard              
// Date:          Nov. 12, 2025
// Revision History :
//  Nov. 12, 2025 - project started
//  Nov. 16, 2025 - project completed

/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "clock.h"
#include "sw_led.h"
#include "ect.h"

//Other system includes or your includes go here
#include <stdlib.h>
#include <stdio.h>


/********************************************************************/
//Defines
/********************************************************************/

/********************************************************************/
// Local Prototypes
/********************************************************************/

/********************************************************************/
// Global Variables
/********************************************************************/
SwState upState, downState, rightState, leftState;

/********************************************************************/
// Constants
/********************************************************************/

/********************************************************************/
// Main Entry
/********************************************************************/
void main(void)
{
  //Any main local variables must be declared here
  unsigned long dutyCycle = 50; // dutyCycle is a ulong to prevent value overflow problems for TC0 assignment

  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
  Clock_Set20MHZ();
  SWL_Init();

   //Configure ECT Output Compare
  TIOS |= TIOS_IOS7_MASK | TIOS_IOS0_MASK; //Set Channel 0 (Pin 9) and Channel 7 (7.3.2.1) as output compare - normally set as inputs
  // TSCR1 |= TSCR1_TFFCA_MASK; //Enable Fast Flag Clear (7.3.2.6) This register can also be used to enable precision timers and Stop in freeze mode.

  /*****************************************
  Compare Result Output Action - TCTL1/TCTL2 (Table 7-10)
  OMx   OLx
  0     0  Timer Disconnected from pin
  0     1  Toggle OCx output line
  1     0  Clear OCx output lize to zero
  1     1  Set OCx output line to one
  *****************************************/
  TCTL1 |= ECT_CH7_TOG; //Channel 7 configured to toggle (7.3.2.8) (Table 7-10) 
  TCTL2 |= ECT_CH0_CLR; //Channel 0 configured to clear (7.3.2.8) (Table 7-10)

  // OC7M_OC7M0 = 1; //Successful output compare on channel 7 will change output of channel 0 (7.3.2.3)
  OC7M |= ECT_CH0;
  OC7D |= ECT_CH0; //Successful compare on channel 7 will set channel 0 to high (7.3.2.4)
  // OC7D &= ~OC7M_OC7M0_MASK; // will set channel 0 to low on successful compare on channel 7 (7.3.2.4)

  // TSCR2_TCRE = 1; //Successful output compare on channel 7 will reset TCNT  (7.3.2.11) 

  // for enum to work, must '|=' the enum 
  TSCR2 |= ECT_PreS32 | ECT_TReset_EN; //Set clock pre-scaler to 16 (7.3.2.11) (Table 7-15)
                       // With a 20MHz clock, this will create an 800ns tick counter. 20MHz/16 = 1.25MHz = 800ns Period

  /**
   * Part B Explanation:
   * - The prescaler value determines the speed the output compare ticks (800 ns in this case)
   * - The ticks assigned to the specific TCx compare register will go high when the tick value is reached by the 16-bit counter
   * - Channel 7 activation will reset the counter
   * - This means channel 7 will determine the period for all other channels (5 ms in this case)
   * - That means the period for both channels will be the same, but the offset will be the difference between the timings 
   * - Turning on the C7M and C7D registers will connect channel 7 and channel 0, activating channel 0 on a channel 7 output compare event
   * - This means that when channel 7 is being checked (going high), channel 0 will go high (then low on it's tick counter is reached), 
   *   and then when channel 7 is checked again (going low), channel 0 will go high again, repeating the cycle
   */               
  TC7 = 6250; //5ms /800ns = 6250 ticks for Channel 7 (7.3.2.14)
  TC0 = (TC7 * dutyCycle)/100; //2.5ms / 800ns = 3125 ticks (7.3.2.14)

  TSCR1 |= ECT_TimerStart; //Start ECT timer (7.3.2.6) (Table 7-7)

/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    // checking button states
    Sw_State(&leftState, SWL_LEFT);
    Sw_State(&rightState, SWL_RIGHT);
    Sw_State(&upState, SWL_UP);
    Sw_State(&downState, SWL_DOWN);

    if(leftState == Pressed){
      TC7 = (TC7 < 12500) ? TC7 + 125 : 12500; // 100us/800ns = 125 ticks for channel 7 | 10ms/800ns = 12,500 max ticks (7.3.2.14)
      TC0 = (TC7 * dutyCycle)/100; // compensate for change in duty cycle as period changes
    }
    if(rightState == Pressed){
      TC7 = (TC7 > 625) ? TC7 - 125 : 625; // 100us/800ns = 125 ticks for channel 7 | 500us/800ns = 625 min ticks (7.3.2.14)
      TC0 = (TC7 * dutyCycle)/100; // compensate for change in duty cycle as period changes
    }
    if(upState == Pressed){
      dutyCycle = (dutyCycle < 95) ? dutyCycle + 5 : 95;
      TC0 = (TC7 * dutyCycle)/100; // re-assign channel 0 tick value to show the duty cycle change
    }
    if(downState == Pressed){
      dutyCycle = (dutyCycle > 5) ? dutyCycle - 5 : 5;
      TC0 = (TC7 * dutyCycle)/100; // re-assign channel 0 tick value to show the duty cycle change
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/

/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
