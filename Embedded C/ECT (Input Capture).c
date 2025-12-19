/********************************************************************/
// HC12 Program:  CMPE 2250 - Embedded System Applications (ICA 8)
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       Assignment going into depth on ECT input capture with interrupts               
// Date:          Nov. 22, 2025
// Revision History :
//  Nov. 22, 2025 - Project started



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

//Other system includes or your includes go here
#include <stdlib.h>
#include <stdio.h>


/********************************************************************/
//Defines
/********************************************************************/

/********************************************************************/
// Local Prototypes
/********************************************************************/
void rtiCallback(void);
/********************************************************************/
// Global Variables
/********************************************************************/
// SwState upState, downState, centerState, rightState, leftState;
unsigned long captureTicks = 0;
unsigned long lastCaptureTicks = 0;
unsigned char updateSegs = 1; // segs display flag
unsigned char updateLcd = 1; // lcd display flag
unsigned long frequency = 0;
unsigned int mCounter = 50;
/********************************************************************/
// Constants
/********************************************************************/

/********************************************************************/
// Main Entry
/********************************************************************/
void main(void)
{
  //Any main local variables must be declared here

  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
Clock_Set20MHZ();
RTI_InitCallback_ms(&rtiCallback, 100);
Segs_Init();
lcd_Init();

TSCR1 |= ECT_PRECISIONTIMER_EN; // enabling precision timer (7.3.2.6)
PTPSR = 20-1; //Enable precision timers 20 for 1us tick at 20MHz. Must be enabled to use PTMCPSR for modulus divider. (7.3.2.6) (7.3.2.25)

// ECT channels are inputs by default, but to be explicit, set each bit to 0
TIOS_IOS0 = 0; // Channel 0 as input (Pin 9) (7.3.2.1)
TIOS_IOS7 = 0; // Channel 7 as input (Pin 18) (7.3.2.1)
TCTL4 |= TCTL4_EDG0A_MASK; // Channel 0 capture on rising edges only (7.3.2.9)

TIE |= ECT_CH0; // enabling interrupt on Channel 0 (7.3.2.10)
//************ Pulse Accumulator Setup ************//
ICSYS |= ICSYS_BUFEN_MASK | ICSYS_LATQ_MASK; //Enable input capture buffers and latch mode (7.3.2.24)
                                              // This allows the pulse accumulator values to be latched into holding registers
                                              // when the mod counter reaches 0
PTMCPSR = 200-1; //Set modulus down counter precision pre-scaler to 199 (7.3.2.26)
                  // 20MHz Clock / 200 = 100KHz = 10us tick

MCCTL |= MCCTL_MCZI_MASK | MCCTL_MODMC_MASK; //Enable modulus underflow interrupt and enable modulus mode (7.3.2.19)
                                            // MODMC must be enabled for the timer to automatically restart
MCCNT = mCounter; //Preload the mod counter the mod counter with a value of 50000 for a 0.5s period. (7.3.2.30)

// Falling or rising edge detection is set up just as it was for Input Capture
/*****************************************
Edge Detector Circuit Configuration - TCTL3/TCTL4 (Table 7-12)
EDGxB  EDGxA
  0      0    Capture disabled
  0      1    Capture on rising edges only
  1      0    Capture on falling edges only
  1      1    Capture on any edge (rising or falling)
*****************************************/
TCTL3 |=  TCTL3_EDG7A_MASK; //Capture rising edges on channel 7 (7.3.2.9)

PACTL |= PACTL_PAEN_MASK | PACTL_PEDGE_MASK; //Enable 16-bit Pulse Accumulator A and count on rising edges (7.3.2.15)
                                            // Pulse accumulator A's input is on IC7 (Pin 18) (7.3.2.15)
                                            // When using pulse accumulator A, the edge control is set in PACTL instead of TCTL4
MCCTL_MCEN = 1; //Enable the modulus counter (7.3.2.19)
MCCNT = mCounter; //Set the load value of the mod counter to 50000 for a 0.5s period. (7.3.2.30)
                 // The mod counter must be running to update the timer load register (7.3.2.30)
  
TSCR1 |= ECT_TimerStart; // Enable main timer (7.3.2.6)

/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    unsigned char lcdBuffer[21];
    if(updateSegs){ // display every 100ms to prevent strobing of segs
      updateSegs = 0; // reset flag
      Segs_16D(captureTicks, Segs_LineBottom);
      Segs_16D(frequency, Segs_LineTop);
    }
    if(updateLcd){
      updateLcd = 0;
      // if-else structure not accounting for measurements below Hz
      if(mCounter > 100){
        sprintf(lcdBuffer, "Freq: %04lu[Hz]  ", frequency);
        lcd_StringXY(0, 0, lcdBuffer);
      }
      else{
        sprintf(lcdBuffer, "Freq: %02lu[kHz] ", frequency); 
        lcd_StringXY(0, 0, lcdBuffer);
      }
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/
void rtiCallback(void){
    updateSegs = 1;  
}
/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
interrupt VectorNumber_Vtimch0 void ECT0_ISR(void){
  captureTicks = (TC0 - lastCaptureTicks); // calculate the period
  lastCaptureTicks = TC0; // save the capture for next substraction
  TFLG1 = TFLG1_C0F_MASK; // no fast flag clear, must manually clear
}
interrupt VectorNumber_Vtimmdcu void Mod_Counter_ISR(void){
  MCFLG = MCFLG_MCZF_MASK; //Clear interrupt flag
  // Need to multiply frequency by 2 because the PA32H value polls at half of the min (1s) and max (10us) frequencies 
  frequency = PA32H; // Getting Pulse Accumulator A (PACN3 and PACN2) -> shared with pin 7 (7.3.2.15)
  // Issue: when frequency is not divisible by 2, the frequency will bounce between the even numbers adjacent to the odd number
  // If not divisible by, take the value 1 below 
  if(frequency % 2)
    frequency = (frequency * 2) - 1;
  else // else, just use the hold value * 2
    frequency *= 2;
  if(frequency <= 1)
    mCounter = 50000; // 0.5ms/10us = 50000 ticks mod counter
  else if (frequency > 1000)
    mCounter = 50; // 500us/10us = 50 ticks mod counter
  MCCNT = mCounter; //Set the load value of the mod counter (7.3.2.30)
  
  updateLcd = 1;
}
/*
Part 6:
Make a demonstration changing different frequencies on the AD2 function generator, using ranges
from 1[Hz] to 100[Khz]. Are all the values displaying properly on the 7-segment display and LCD? If
not, can you explain why? Could we implement any solution to the problem?
  - There are no problems with the displays when the AD2 is generating at 1[Hz]
  - At 100[kHz], the lcd and segs will display numbers that are not close to the 100kHz value
    -> The likely reason for this is that 100k is beyond the 65,535 of the 16-bit pulse accumulator,
    -> and the overflow would cause the values to be different from the expected 100kHz number
  - Fix: Change the mod counter value to adjust the polling rate for frequency so that the hold value in PA32H
         will fall within the 65,535 maximum of the 16-bit register. 
    -> For my solution, I switched between mod counter values that will poll at half of the min (1s) and max values (10us) because
       getting 1s from the pre-scale and mod counter values requires changing the clock speed (was trying to maintain 20MHz*).
    -> When the frequency is greater than 1k, switch the mod counter to poll every 50 ticks (500us), then multiply by 2
    -> When the frequency is less than 1 (less than 1kHz), switch the mod counter to poll every 50000 ticks (0.5s), then multiply by 2 
*/