/********************************************************************/
// HC12 Library:  Periodic Interrupt Timer
// Processor:     MC9S12XDP512
// Author:        Jonathan Le
// Details:       
//                                
// Date:          Sept. 19, 2025
// Revision History :
//  Sept. 19, 2025 - Initial build
//  Oct. 14, 2025 - changes:
//                  - need to use force load when changing timing
//                  - will need to always manually reset the flag if something changes
//                  - need to clear and then turn the flag back on
//                  - overhead for running all the processes - subtract the time added for the overhead
/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "clock.h"
#include "pit.h"

/********************************************************************/
// Global Variables
/********************************************************************/
static void (*pCallback_Ch0)(void) = NULL; // channel 0 callback
static void (*pCallback_Ch1)(void) = NULL; // channel 1 callback
static void (*pCallback_Ch2)(void) = NULL; // channel 2 callback
static void (*pCallback_Ch3)(void) = NULL; // channel 3 callback
// unsigned long currentClock = 0;

/********************************************************************/
// Library Functions
/********************************************************************/
void PIT_Set1msDelay(PIT_Channel ch){
    unsigned int timer16 = (Clock_GetBusSpeed()/1000) - 1; // -1 to account for 0 start 

    if(!(PITCE & ch)) // checking if channel is active (13.3.0.3)
        PIT_InitChannel(ch, PIT_MT1, PIT_IDIS);

    // calculation: (<current_clockspeed>/1000000 *) - 1 will be the required ticks for us
    PITMTLD1 = 0; // Load micro timer 1 (13.3.0.7)

    switch (ch) //Load the 16-bit counter (13.3.0.8) 
    {
        case PIT_CH0:
            PITLD0 = timer16;
            break;
        case PIT_CH1:
            PITLD1 = timer16;
            break;
        case PIT_CH2:
            PITLD2 = timer16;
            break;
        case PIT_CH3:
            PITLD3 = timer16;
            break;
    }

    PITCFLMT |= PITCFLMT_PITE_MASK; //Force load the micro timer (13.3.0.1)
    PITFLT |= PITFLT_PFLT_MASK; //Force load the 16-bit timer (13.3.0.2)

    if(!(PITINTE & ch)){ // if interrupt is not active for the channel, use the blocking delay (13.3.0.5)
        while (!(PITTF & ch)); // Wait until timer expires
        PITTF = ch; // Clear interrupt flag (13.3.0.6)
    }
}

void PIT_Sleep(PIT_Channel ch, unsigned int ms){
    // calculation: (<current_clockspeed>/1000 * ms) will be the required ticks for ms -> x - 1 after  
    unsigned long reqTicks = (Clock_GetBusSpeed()/1000) * ms;
    unsigned int mtTicks = 0;
    unsigned int chTicks = 0;

    // continue checking microtimer and 16-bit timer combinations until no remainders and 16-bit channel value <= 65536
    while(!chTicks){
        mtTicks++;
        // no decimal points, so check mod equal to 0
        if(!(reqTicks % mtTicks)){
            // check if the division produces a number within 16-bit max limit (65536)
            if(reqTicks / mtTicks <= 65536){
                chTicks = reqTicks / mtTicks; // store division result at 16-bit channel ticks (exit condition for loop)
            }
        }
    }

    PITMTLD1 = mtTicks - 1; // Load micro timer 1 (13.3.0.7)

    switch (ch) //Load the 16-bit counter (13.3.0.8) 
    {
        case PIT_CH0:
            PITLD0 = chTicks - 1;
            break;
        case PIT_CH1:
            PITLD1 = chTicks - 1;
            break;
        case PIT_CH2:
            PITLD2 = chTicks - 1;
            break;
        case PIT_CH3:
            PITLD3 = chTicks - 1;
            break;
    }

    PITCFLMT |= PITCFLMT_PITE_MASK; //Force load the micro timer (13.3.0.1)
    PITFLT |= PITFLT_PFLT_MASK; //Force load the 16-bit timer (13.3.0.2)

    if(!(PITINTE & ch)){ // if interrupt is not active for the channel, use the blocking delay (13.3.0.5)
        while (!(PITTF & ch)); // Wait until timer expires
        PITTF = ch; // Clear interrupt flag (13.3.0.6)
    }
}

// initalizes the micro-timer channel, PIT channel, and interrupts of the PIT module
void PIT_InitChannel(PIT_Channel ch, PIT_MicroTimer mt, PIT_Interrupt ie){
    PITCE |= ch; // Enable PIT channel specified (13.3.0.3)
    PITMUX = (mt == 0) ? PITMUX & ~ch : PITMUX | ch; // Connect PIT channel to micro timer specified (13.3.0.4)
    PITINTE = (ie == 0) ? PITINTE & ~ch : PITINTE | ch; // Connect interrupt on PIT channel specified (13.3.0.5)
}
void PIT_SetTiming_Custom(PIT_Channel ch, PIT_MicroTimer mt, unsigned char timer8, unsigned int timer16){
    // checking which channel is being specified for the micro-timer (13.3.0.4)
    if((PITMUX & ch) & mt)
        PITMTLD1 = timer8 - 1; // Load micro timer 1; 0 counts as a cycle (hence -1) (13.3.0.7)
    if(!((PITMUX & ch) & mt))
        PITMTLD0 = timer8 - 1; // Load micro timer 0; 0 counts as a cycle (hence -1) (13.3.0.7)
    
    // Configure specified PIT channel counter
    if(PITCE & ch){ // checking if the channel is initialized first (13.3.0.3)
        switch(ch){ //Load the 16-bit counter (13.3.0.8)  
            case PIT_CH0:
                PITLD0 = timer16 - 1;
            break;
            case PIT_CH1:
                PITLD1 = timer16 - 1;
            break;
            case PIT_CH2:
                PITLD2 = timer16 - 1;
            break;
            case PIT_CH3:
                PITLD3 = timer16 - 1;
            break;
        }  
    }
}
void PIT_AssignFunction(PIT_Channel ch, void (*pFunction)(void)){
    // Configure specified PIT channel counter
    if(PITCE & ch){ // checking if the channel is initialized first (13.3.0.3)
        switch(ch){ //Load the 16-bit counter (13.3.0.8)  
            case PIT_CH0:
                pCallback_Ch0 = pFunction;
            break;
            case PIT_CH1:
                pCallback_Ch1 = pFunction;
            break;
            case PIT_CH2:
                pCallback_Ch2 = pFunction;
            break;
            case PIT_CH3:
                pCallback_Ch3 = pFunction;
            break;
        }  
    }
}
void PIT_Start(void){
    PITCFLMT |= PITCFLMT_PITE_MASK; // Enable the PIT module (13.3.0.1)
}
void PIT_Delay_ms(PIT_Channel ch, PIT_MicroTimer mt, unsigned int ms){
    // calculation: (<current_clockspeed>/1000 * ms) will be the required ticks for ms -> x - 1 after  
    // unsigned long used as 40,000,000 is the max ticks (40MHz), where unsigned long max is 4,294,967,295
    unsigned long reqTicks = (Clock_GetBusSpeed()/1000) * ms;
    unsigned int mtTicks = 0;
    unsigned int chTicks = 0;

    // continue checking microtimer and 16-bit timer combinations until no remainders and 16-bit channel value <= 65536
    while(!chTicks){
        mtTicks++;
        // no decimal points, so check mod equal to 0
        if(!(reqTicks % mtTicks)){
            // check if the division produces a number within 16-bit max limit (65536)
            if(reqTicks / mtTicks <= 65536){
                chTicks = reqTicks / mtTicks; // store division result at 16-bit channel ticks (exit condition for loop)
            }
        }
    }

    if(!mt)
        PITMTLD0 = mtTicks - 1; // Load micro timer 0 (13.3.0.7)
    if(mt)
        PITMTLD1 = mtTicks - 1; // Load micro timer 1 (13.3.0.7)

    switch (ch) //Load the 16-bit counter (13.3.0.8) 
    {
        case PIT_CH0:
            PITLD0 = chTicks - 1;
            break;
        case PIT_CH1:
            PITLD1 = chTicks - 1;
            break;
        case PIT_CH2:
            PITLD2 = chTicks - 1;
            break;
        case PIT_CH3:
            PITLD3 = chTicks - 1;
            break;
    }

    PITCFLMT |= PITCFLMT_PITE_MASK; //Force load the micro timer (13.3.0.1)
    PITFLT |= PITFLT_PFLT_MASK; //Force load the 16-bit timer (13.3.0.2)

    if(!(PITINTE & ch)){ // if interrupt is not active for the channel, use the blocking delay (13.3.0.5)
        while (!(PITTF & ch)); // Wait until timer expires
        PITTF = ch; // Clear interrupt flag (13.3.0.6)
    }
}
void PIT_Delay_us(PIT_Channel ch, PIT_MicroTimer mt, unsigned int us){

    // calculation: (<current_clockspeed>/1000000 * us) - 1 will be the required ticks for us
    // uint is used as the max timer value is 65536, which is equal to the size of an unsigned int
    unsigned int timer16 = ((Clock_GetBusSpeed()/1000000) * us) - 1; // -1 to account for 0 start 
    // all of the microsecond range can be handled by the 16-bit timer, so set the specified mt to 0
    if(!mt)
        PITMTLD0 = 0; // Load micro timer 0 (13.3.0.7)
    if(mt)
        PITMTLD1 = 0; // Load micro timer 1 (13.3.0.7)

    // the 16 bit timer can handle the entire microsecond range for this microboard (with a max clock of 40 MHz vs 65536 for 16 bit)
    switch (ch) //Load the 16-bit counter (13.3.0.8) 
    {
        case PIT_CH0:
            PITLD0 = timer16;
            break;
        case PIT_CH1:
            PITLD1 = timer16;
            break;
        case PIT_CH2:
            PITLD2 = timer16;
            break;
        case PIT_CH3:
            PITLD3 = timer16;
            break;
    }

    PITCFLMT |= PITCFLMT_PITE_MASK; //Force load the micro timer (13.3.0.1)
    PITFLT |= PITFLT_PFLT_MASK; //Force load the 16-bit timer (13.3.0.2)

    if(!(PITINTE & ch)){ // if interrupt is not active for the channel, use the blocking delay (13.3.0.5)
        PITTF = ch; // Clear interrupt flag (13.3.0.6)
        while (!(PITTF & ch)); // Wait until timer expires
    }
}

/********************************************************************/
// Library Functions
/********************************************************************/


/********************************************************************/
// Another group of library functions
/********************************************************************/


/********************************************************************/
// Another title placeholder
/********************************************************************/


/********************************************************************/
// Interrupt Service Routines Template
/********************************************************************/
interrupt VectorNumber_Vpit0 void Vpit0_ISR(void){
    PITTF = PIT_CH0; // clearing flag (13.3.0.6)
    if(pCallback_Ch0) // check if there is a function within the pointer
        pCallback_Ch0();
}
interrupt VectorNumber_Vpit1 void Vpit1_ISR(void){
    PITTF = PIT_CH1; // clearing flag (13.3.0.6)
    if(pCallback_Ch1) // check if there is a function within the pointer
        pCallback_Ch1();
}
interrupt VectorNumber_Vpit2 void Vpit2_ISR(void){
    PITTF = PIT_CH2; // clearing flag (13.3.0.6)
    if(pCallback_Ch2) // check if there is a function within the pointer
        pCallback_Ch2();
}
interrupt VectorNumber_Vpit3 void Vpit3_ISR(void){
    PITTF = PIT_CH3; // clearing flag (13.3.0.6)
    if(pCallback_Ch3) // check if there is a function within the pointer
        pCallback_Ch3();
}