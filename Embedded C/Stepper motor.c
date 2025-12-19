/********************************************************************
*HC12 Program:    Lab 7 - CMPE 2150
*Processor:       MC9S12XDP512
*Xtal Speed:      20 MHz
*Author:          Jonathan L
*Date:            2025
*
*Details: Full rotation each for fwd/rev of Wave, Full, Half step modes
*
********************************************************************/

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
/********************************************************************
*		Library includes
********************************************************************/


/********************************************************************
*		Prototypes
********************************************************************/
void FullStepForward(void);   //four Full Steps
void FullStepBackward(void);  //four Full Steps
void HalfStepForward(void);   //eight Half Steps
void HalfStepBackward(void);  //eight Half Steps
void rtiCallback(void);
/********************************************************************
*		Variables
********************************************************************/
  static volatile struct Flags{
    unsigned int fullStep:1;
    unsigned int forward:1;
  } flag;
  char PTAHold;
  int ForCount;
  unsigned int TickCount;
  static void (*pFunction)(void) = NULL;

/********************************************************************
*		Lookups
********************************************************************/
const char Sequence[8]={
            0b00010000, 0b00110000, 0b00100000, 0b01100000,
            0b01000000, 0b11000000, 0b10000000, 0b10010000
            };


void main(void) 	// main entry point
{
  _DISABLE_COP();
  EnableInterrupts

  /****Use PLL to increase bus speed to 20 MHz; disable for legacy****/

  // SYNR = 4;                // 2.3.2.1,2 div 4/3 becomes 5/4 (1.25) * 16Mhz * 2 = 40MHz, /2 = 20MHz
  // REFDV = 3;
  // CLKSEL_PSTP = 1;         // 2.3.2.6 (pseudo stop, clock runs in stop)
  // PLLCTL = 0b11111111;     // 2.3.2.7 (monitor + fast wakeup, AUTO mode)

  // while (!CRGFLG_LOCK);    //wait for PLL to lock
  // CLKSEL_PLLSEL = 1;       // 2.3.2.6 now that we are locked, use PLLCLK/2 for bus (20MHz)

/********************************************************************
*		Initializations
********************************************************************/
  Clock_Set20MHZ();
  RTI_InitCallback(&rtiCallback);
  SWL_Init();               // switches initialized

  DDRA|=0b11110000;         //PTA4 - 7 as outputs

  TSCR1 |= 0b10000000 | ECT_PRECISIONTIMER_EN;      //enable timer module with precision timer
  PTPSR = 200-1; //Set precision pre-scaler to 200 for 10us tick at 20MHz (7.3.2.25)
  TSCR2 |= 0b00000011;      //set tick to 400 ns
  TIOS  |= 0b00000001;      //IOS0 to output compare for tick clock
  TCTL2 |= 0b00000001;	    //set PT0 to toggle mode
  TCTL2 &= 0b11111101;      //...continued	
  TFLG1  = 0b00000001;      //clear flag

  TickCount = 250;         //2.5 ms / 10us = 250 ticks
  TC0=TCNT+TickCount;       //first timer interval
  pFunction = FullStepForward;
  for (;;)                  //endless program loop
  {
/********************************************************************
*		Main Program Code
********************************************************************/
//run a full rotation of each
    pFunction(); // function pointer run everyloop, but different conditions will change which function is being pointed to                         
  }
}
/********************************************************************
*		Functions
********************************************************************/
void rtiCallback(void){
  static SwState up, down, center, right, left = Idle; // states of the buttons

  // checking switch status
  Sw_State(&up, SWL_UP);
  Sw_State(&down, SWL_DOWN);
  Sw_State(&center, SWL_CTR);
  Sw_State(&left, SWL_LEFT);
  Sw_State(&right, SWL_RIGHT);

  if(up == Pressed){
    TickCount = 250; // 2.5ms
  }
  if(down == Pressed){
    TickCount = 25000; // 0.25s
  }
  if(center == Pressed){
    flag.fullStep = !flag.fullStep;
    // full step and foward
    if(flag.fullStep && flag.forward)
      pFunction = FullStepForward;
    // full step and backwards
    if(flag.fullStep && !flag.forward)
      pFunction = FullStepBackward;
    // half step and forwards
    if(!flag.fullStep && flag.forward)
      pFunction = HalfStepForward;
    // half step and backwards
    if(!flag.fullStep && !flag.forward)
      pFunction = HalfStepBackward;
  }
  if(right == Pressed){
    flag.forward = 1;
    pFunction = (flag.fullStep) ? FullStepForward : HalfStepForward;
  }
  if(left == Pressed){
    flag.forward = 0;
    pFunction = (flag.fullStep) ? FullStepBackward : HalfStepBackward;
  }
}

void FullStepForward(void){
  char Counter;
  for(Counter = 1;Counter<8; Counter += 2){
    PTAHold=PORTA&0b00001111;               //don't mess up lower nibble
    PORTA=PTAHold|Sequence[Counter];
    while(!TFLG1);                          //blocking timer wait
    TFLG1=0b00000001;                       //clear flag
    TC0+=TickCount;
  }
}

void FullStepBackward(void){
  char Counter;
  for(Counter = 7;Counter>0; Counter-=2){
    PTAHold=PORTA&0b00001111;               //don't mess up lower nibble
    PORTA=PTAHold|Sequence[Counter];
    while(!TFLG1);                          //blocking timer wait
    TFLG1=0b00000001;                       //clear flag
    TC0+=TickCount;
  }
}

void HalfStepForward(void){
  char Counter;
  for(Counter = 0;Counter<8; Counter++){
    PTAHold=PORTA&0b00001111;               //don't mess up lower nibble
    PORTA=PTAHold|Sequence[Counter];
    while(!TFLG1);                          //blocking timer wait
    TFLG1=0b00000001;                       //clear flag
    TC0+=TickCount;
  }
}

void HalfStepBackward(void){
  char Counter;
  for(Counter = 7;Counter>=0; Counter--){
    PTAHold=PORTA&0b00001111;               //don't mess up lower nibble
    PORTA=PTAHold|Sequence[Counter];
    while(!TFLG1);                          //blocking timer wait
    TFLG1=0b00000001;                       //clear flag
    TC0+=TickCount;
  }
}



/********************************************************************
*		Interrupt Service Routines
********************************************************************/



/*******************************************************************/