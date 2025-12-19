/********************************************************************/
// HC12 Library:  Library Name Goes Here
// Processor:     MC9S12XDP512
// Author:        Your Name Here
// Details:       What is this library for?
//                                
// Date:          Current Date
// Revision History :
//  September 28 2024 - Initial build


/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */

#include "atd.h"

/********************************************************************/
// Global Variables
/********************************************************************/
static void (*pCallback)(void) = NULL;

/********************************************************************/
// Library Functions
/********************************************************************/
// will not be configuring external trigger options at this time
void ATD_Config(ATD_ConfigOpt PowerON, ATD_ConfigOpt FastFlag, ATD_ConfigOpt WaitMode, ATD_ConfigOpt Interrupt){
    ATD0CTL2 |= PowerON | FastFlag | WaitMode | Interrupt; //(5.3.2.3)
}
void ATD_SetWrap(ATD_ChannelSelect Ch){
    ATD0CTL0 |= Ch; // (5.3.2.1) (Table 5-2)
}
// will not be configuring Result Register FIFO mode
void ATD_SetNumConversions(ATD_NumConversion NumConversions, ATD_FreezeMode FrzMode){
    ATD0CTL3 |= NumConversions | FrzMode; // (5.3.2.4) (Table 5-8)
}
void ATD_SetScanBehaviour(ATD_DataFormat Format, ATD_SamplingMode Scan, ATD_SamplingMode Mult, ATD_ChannelSelect StartCh){
    ATD0CTL5 |= Format | Scan | Mult | StartCh; // (5.3.2.6)
}
void ATD_SetClock(ATD_Resolution Resolution, ATD_Prescaler Prescaler, ATD_SampleTime SampleTime){
    ATD0CTL4 |= Resolution | Prescaler | SampleTime; // (5.3.2.5) (Table 5-11) (Table 5-12)
}
void ATD_SetCallback(void (*pFunction)(void)){
    pCallback = pFunction;
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
interrupt VectorNumber_Vatd0 void AtoD_ISR(void){ //AtoD 0 Interrupt Service Routine
    // Reading the results register will clear the Sequence Complete flag when Fast Flag Clear is enabled (5.3.2.7)
    // Clearing the Sequence Complete Flag will also clear the Sequence Complete Interrupt Flag (5.3.2.3)
    if(pCallback)
        pCallback();
}