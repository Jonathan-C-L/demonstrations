/********************************************************************/
// HC12 Program:  ICA12 - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       Using I2C, send output a voltage into the DAC chip embedded on the board              
// Date:          Dec. 7, 2025

/* Debugging tip:
  - use the protocol analyzer to check if the proper information is being transmitted
  - I tried playing around with it and watched Diligent's tutorial video but couldn't figure out how to use it... :(
  - I figured out how to use the protocol analyzer on this board! I forgot about the SCL and SDA pins...
*/

/* Question 4:
  - When switching the DAC between an adjustment of 100 to 10 or vise versa, the output signal would take a
    fairly long time to update. However, looking through the datasheet for the LTC2633, the chip seems to support
    support standard-mode (100kHz or 10us) or fast-mode (400kHz or 2.5us) for receiving information
  - This would mean it is possible to write to the dac much faster than the 200us setting this assignment calls for
*/

/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "clock.h"
#include "lcd.h"
#include "rti.h"
#include "sci.h"
#include "segs.h"
#include "sw_led.h"
#include "pit.h"
#include "atd.h"
#include "ect.h"
#include "i2c.h"

//Other system includes or your includes go here
#include <stdlib.h>
#include <stdio.h>


/********************************************************************/
//Defines
/********************************************************************/

/********************************************************************/
// Local Prototypes
/********************************************************************/
void pitCallback(void);
/********************************************************************/
// Global Variables
/********************************************************************/
// global flags to be trigged in main or interrupts
volatile struct _Flag{
  unsigned int update:1;
  unsigned int testMode:1;
} flag;

/********************************************************************/
// Constants
/********************************************************************/

/********************************************************************/
// Main Entry
/********************************************************************/
void main(void)
{
  //Any main local variables must be declared here
  unsigned int dacValue = 0;
  volatile char dacIndex = 0;
  volatile unsigned char adjustment = 100;

  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
Clock_Set20MHZ();
PIT_InitChannel(PIT_CH3, PIT_MT0, PIT_IDIS); // required for I2C lib
PIT_InitChannel(PIT_CH1, PIT_MT1, PIT_IEN); // using this interrupt to generate a 200us delay
PIT_Delay_us(PIT_CH1, PIT_MT1, 3);
PIT_AssignFunction(PIT_CH1, &pitCallback);
PIT_Start();
I2C0_Init();
SWL_Init();

/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    static SwState upState, downState, centerState, rightState, leftState;
    const int dacTest[4] = {0x0000, 0x0FFF, 1000, 2000}; // const as the test values will not be changed at runtime

    // change between testing and rising ramps
    Sw_State(&centerState, SWL_CTR);

    if(centerState == Pressed){
      flag.testMode ^= flag.testMode; // toggle the mode
    }

    if(flag.testMode){
      // up increments index, down decrements index
      Sw_State(&upState, SWL_UP);
      Sw_State(&downState, SWL_DOWN);

      if(upState == Pressed)
        (dacIndex < 4) ? dacTest[++dacIndex] : dacTest[4];
      if(downState == Pressed)
        (dacIndex > 0) ?  dacTest[--dacIndex] : 0;
      
      if(flag.update){
        flag.update = 0;    
        /*
        Expected values:
        0x0000 = 0 => 0 (ish) mV  | Measured: 1.2 mV
        0x0FFF = 4095 => 4095 mV  | Measured: 4095 mV
        1000 => 1000 mV           | Measured: 1005 mV           
        2000 = 2000 mV            | Measured: 2002 mV
        */ 

        // for a 12-bit number, 4095 is the max unsigned value 
        I2C0_WriteDAC(dacTest[dacIndex], DAC_A);
        I2C0_WriteDAC(dacTest[dacIndex], DAC_B);
      }
    }
    else{
      // right will switch to 100 adjustment value and left will switch to 10 adjustment value
      Sw_State(&leftState, SWL_LEFT);
      Sw_State(&rightState, SWL_RIGHT);
      if(leftState == Pressed)
        adjustment = 10; 
      // an adjustment of 10 causes the ramps to have "steps" of 10 mV, so the ramps look fairly smooth
      // however, this will extend the period out to 145ms because it takes more 200us ticks to reach the 4095/0 values
      if(rightState == Pressed)
        adjustment = 100; 
      // an adjustment of 100 value causes the ramps to have "steps" of 100 mV, leading to a rough appearance to the ramps 
      // the period for an adjustment of 100 would be 15ms
      
      if(flag.update){
        flag.update = 0;

        dacValue += adjustment;
        I2C0_WriteDAC(dacValue, DAC_A); // rising ramp
        I2C0_WriteDAC(4095 - dacValue, DAC_B); // falling ramp
      }
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/
void pitCallback(void){
  flag.update = 1;
}
/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
