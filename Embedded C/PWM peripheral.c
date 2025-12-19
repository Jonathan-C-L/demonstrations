/********************************************************************/
// HC12 Program:  ICA10 - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       PWM 16-bit on the lcd. 
//                Note: A bit confused at what the control scheme was supposed to be
//                CONTROLS:
//                - Left and right changes options
//                - Up and down only changes percentage values (Adjustment and Duty %)
//                - Center button affects the toggle menu items (Channel 7 ON/OFF, Polarity, and Reset)
//                - Reset is the 4th option (denoted with a '*') and pressing the center button 
// Date:          Nov. 30, 2025
// Revision History :
//  Nov. 30, 2025



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
typedef enum MenuOptionsTypedef{
  Adjustment,
  Duty,
  Polarity,
  Reset
} MenuOptions;
/********************************************************************/
// Local Prototypes
/********************************************************************/

/********************************************************************/
// Global Variables
/********************************************************************/
static volatile struct _flag{
    unsigned int updateDisplays:1;
} flags;
/********************************************************************/
// Constants
/********************************************************************/

/********************************************************************/
// Main Entry
/********************************************************************/
void main(void)
{
  //Any main local variables must be declared here
  // initializing the array here to assign it to register memory addresses to it later
  static unsigned int* menu[4]; 
  unsigned char buffer[12];
  static unsigned int adjust = 100; // not sure why, but having a unsigned char typing always started the adjustment at 256
  const unsigned int reset[3] = {100, 500, 128};

  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
Clock_Set20MHZ();
RTI_Init();
SWL_Init();
Segs_Init();
lcd_Init();
lcd_DispControl(1, 1); 

// static displays on lcd
lcd_StringXY(0, 0, "Adjust:       10.0%");
lcd_StringXY(0, 1, "Duty:         50.0%");
lcd_StringXY(0, 2, "Polarity:     Pos");
lcd_StringXY(0, 3, "PWM Ch:       ON  *");
lcd_AddrXY(19, 0);
/********** PWM Configuration ***********/
PWMCTL_CON67 = 1; // Enabling merged 6 and 7 channels into a single 16-bit channel  (8.3.2.6)
// ClockSB = ClockB/(2*ScaleB)
PWMPRCLK |= PWMPRCLK_PCKB1_MASK; // Set prescaler B to 4 (PCKB1), Clock A set to 20MHz/4 = 5MHz (8.3.2.4)
PWMPOL_PPOL7 = 1; // Channel 7 set to positive polarity (8.3.2.2)

// not using clockSB because the range for the period and the duty cycle can be reached with just the prescale value
PWMPER67 = 1000; // 200us/200ns = 1000 ticks (8.3.2.13)
PWMDTY67 = 500; // 50% duty -> (500/1000)*100 (8.3.2.14) 

menu[Adjustment] = &adjust; // for altering adjust sizes
menu[Duty] = &PWMDTY67; // for changing duty cycle
menu[Polarity] = &PWMPOL; // cannot use address of bitfield for polarity, must ^= to toggle
menu[Reset] = reset; // for resetting to defaults

Segs_16D(*menu[Polarity], Segs_LineBottom);
Segs_16D(*menu[Duty], Segs_LineTop); // displaying duty cycle tick value in the top segs

PWME_PWME7 = 1; //Enable the 16-bit channel 67 (pin 109) (8.3.2.1)
/********************************************************************/
  // main program loop
/********************************************************************/
  for (;;)
  {
    static SwState upState, downState, centerState, rightState, leftState;
    static unsigned char index = 0; // for selecting options

    // checking switch status
    Sw_State(&upState, SWL_UP);
    Sw_State(&downState, SWL_DOWN);
    Sw_State(&centerState, SWL_CTR);
    Sw_State(&leftState, SWL_LEFT);
    Sw_State(&rightState, SWL_RIGHT);

    /**************************************************** Left/Right Buttons *******************************************************/
    if(upState == Pressed){
      if(index == Adjustment) // Adjustment is menu option [0]
        *menu[index] = (*menu[index] < 100) ? *menu[index] * 10 : 100; // adjustments are multiples of 10, max 100
      if(index == Duty){ // Duty is menu option [1]
        // because the rate of change can differ, must account for possibility of adjustment value being different when incrementing duty cycle
        *menu[index] = ((*menu[index] + *menu[Adjustment]) < PWMPER67) ? *menu[index] +  *menu[Adjustment] : PWMPER67; 
        Segs_16D(*menu[index], Segs_LineTop);
      }
        
      // display changed for adjustment value
      sprintf(buffer, "%.1f%% ", (float)*menu[index]/10);
      lcd_StringXY(14, index, buffer);
      lcd_AddrXY(19, index); // move cursor to indicate the menu option
    }
    if(downState == Pressed){
      if(index == Adjustment) // Adjustment is menu option [0]
        *menu[index] = (*menu[index] > 1) ? *menu[index] / 10 : 1; // adjustments are multiples of 10, min 1
      if(index == Duty){ // Duty is menu option [1]
        // because the rate of change can differ, must account for possibility of adjustment value being different when decrementing duty cycle
        *menu[index] = ((*menu[index] - *menu[Adjustment]) > 1) ? *menu[index] - *menu[Adjustment] : 1; 
        Segs_16D(*menu[index], Segs_LineTop);
      }
      
      // display changed for adjustment value
      sprintf(buffer, "%.1f%% ", (float)*menu[index]/10);
      lcd_StringXY(14, index, buffer);
      lcd_AddrXY(19, index); // move cursor to indicate the menu option
    }
    /**************************************************** Center Button Toggles *******************************************************/
    if(centerState == Pressed){
      if(index == Polarity){ // if the menu option is Polarity[2]
        PWMPOL_PPOL7 = !PWMPOL_PPOL7; // toggles polarity (starts in positive polarity)  
        if(PWMPOL_PPOL7)
          lcd_StringXY(14, 2, "Pos");
        else
          lcd_StringXY(14, 2, "Neg");
      }
      else if(index == Reset){ // if the menu option is Reset[3], then the center button will toggle the reset
        int i;
        *menu[Adjustment] = menu[Reset][Adjustment];
        *menu[Duty] = menu[Reset][Duty];
        // *menu[Polarity] |= menu[Reset][Polarity]; //-> this doesn't work for some reason... the frequency tanks to 10Hz with this...
        PWMPOL_PPOL7 = 1; // this works so will leave it like this for now
        Segs_16D(*menu[Polarity], Segs_LineBottom);
        Segs_16D(*menu[Duty], Segs_LineTop);
        for(i = 0; i < 4; i++){
          if (i < 2){
            sprintf(buffer, "%.1f%% ", (float)*menu[i]/10);
            lcd_StringXY(14, i, buffer);
          }
          if (i == Polarity)
            lcd_StringXY(14, 2, "Pos");
          if (i == Reset)
            lcd_StringXY(14, 3, "ON  *");
        }
      }
      else{
        PWME_PWME7 = !PWME_PWME7; // Enabling merged 6 and 7 channels into a single 16-bit channel  (8.3.2.6)
        if(PWME_PWME7)
          lcd_StringXY(14, 3, "ON  *");
        else
          lcd_StringXY(14, 3, "OFF *");
      }
      lcd_AddrXY(19, index); // move cursor to indicate the menu option
    }
    /**************************************************** Left/Right Buttons  *******************************************************/
    if(leftState == Pressed){
      index = (index > 0) ? --index : 0; // min position of lcd is 0
      lcd_AddrXY(19, index); // move cursor to indicate the menu option
    }
    if(rightState == Pressed){
      index = (index < 3) ? ++index : 3; // max position of lcd is 3
      lcd_AddrXY(19, index); // move cursor to indicate the menu option
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/

/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
