/********************************************************************/
// HC12 Library:  real time interrupt
// Processor:     MC9S12XDP512
// Author:        Jonathan Le
// Details:       clock functions
//                                
// Date:          March 6, 2025
// Revision History :
//  March 6, 2025 - Initial build


/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "rti.h"
#include "sw_led.h"

/********************************************************************/
// Global Variables
/********************************************************************/
volatile unsigned long rtiMasterCount = 0;
volatile unsigned int rtiInterval = 0;
volatile unsigned int timeoutCounter = 0;
static void (*pCallback)(void) = NULL; 
/********************************************************************/
// Library Functions
/********************************************************************/
void RTI_Init(void)
{
  // RTICTL - Decimal, divider 2000, mod8 counter,
  // results in an exact 1ms tick, independent of
  // buss speed, or code execution speed.
  RTICTL = 0b10010111;
  // CRGINT_RTIE_MASK = 0b10000000
  // Enable real time Interrupt
  CRGINT |= CRGINT_RTIE_MASK; 
}

void RTI_Delay_ms(unsigned long delay)
{
  unsigned long timeout = rtiMasterCount + delay;
  while(rtiMasterCount - timeout > 0);
}

void RTI_InitCallback(void (*pFunction)(void)){
  RTI_Init();  // initializing rti
  pCallback = pFunction;  // assigning the callback from main.c to the pointer
  rtiInterval = 0; // setting the delay (0 to only run callback once)
}

void RTI_InitCallback_ms(void (*pFunction)(void), unsigned long interval){
  RTI_Init();  // initializing rti
  pCallback = pFunction;  // assigning the callback from main.c to the pointer
  rtiInterval = interval;  // setting the delay (>0 delay is the interval for callback call)
}

/********************************************************************/
// Another group of library functions
/********************************************************************/


/********************************************************************/
// Another title placeholder
/********************************************************************/


/********************************************************************/
// Interrupt Service Routines Template
/********************************************************************/
interrupt VectorNumber_Vrti void Vrti_ISR(void)
{
  // Clear flag so interrupt can happen again.
  CRGFLG = CRGFLG_RTIF_MASK;
  // Perform some actions here...
  rtiMasterCount++; // every 1ms

  // Ensure function pointer is assigned before running callback
  if(pCallback){  
    // RTIInitCallback()
    if(rtiInterval ==  0)  
      pCallback();
      // RTIInitCallback_ms()
      else{  
      if(timeoutCounter++ >= rtiInterval){
        pCallback();
        timeoutCounter = 0; // reset timeout counter
      }
    }
  }
}

