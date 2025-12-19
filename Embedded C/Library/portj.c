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

#include "portj.h"

/********************************************************************/
// Global Variables
/********************************************************************/
static void (*pCallback_J0)(void) = 0; // J0 callback
static void (*pCallback_J1)(void) = 0; // J1 callback

/********************************************************************/
// Library Functions
/********************************************************************/
void PortJ_InitInputButton(PortJ_Button bt, PortJ_Pull pull, PortJ_Edge edge, PortJ_Interrupt enable){
    DDRJ &= ~bt; // Port J set as inputs (0) (22.3.2.56)

    if(bt == PortJ_J0){
        PERJ_PERJ0 = pull; // set pull (22.3.2.58)
        PPSJ_PPSJ0 = edge; // polarity (22.3.2.59) 
        PIEJ_PIEJ0 = enable; // interrupt (22.3.2.60)
    }
    if(bt == PortJ_J1){
        PERJ_PERJ1 = pull; // set pull (22.3.2.58)
        PPSJ_PPSJ1 = edge; // polarity (22.3.2.59)
        PIEJ_PIEJ1 = enable; // interrupt (22.3.2.60)
    }
    
}
void PortJ_AddButtonCallback(PortJ_Button bt, void (*pFunction)(void)){
    if(bt == PortJ_J0){
        pCallback_J0 = pFunction;
    }
    if(bt == PortJ_J1){
        pCallback_J1 = pFunction;
    }
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
interrupt VectorNumber_Vportj void PortJ_ISR(){
  if(PIFJ & PIFJ_PIFJ0_MASK){
    PIFJ = PIFJ_PIFJ0_MASK; // clearing the flag
    if(pCallback_J0)
        pCallback_J0();
  }
  if(PIFJ & PIFJ_PIFJ1_MASK){
    PIFJ = PIFJ_PIFJ1_MASK; // clearing the flag
    if(pCallback_J1)
        pCallback_J1();
  }
}