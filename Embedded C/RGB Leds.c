/********************************************************************/
// HC12 Program:  ICA9 - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       Pulse Width Modulation with 8-bit channel with the RGB leds on the microboard             
// Date:          Nov. 30, 2025
// Revision History :
//  Nov. 30, 2025 - Project started



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
// #include <stdlib.h>
#include <stdio.h>
// #include <string.h>


/********************************************************************/
//Defines
/********************************************************************/

/********************************************************************/
// Local Prototypes
/********************************************************************/
void rtiCallback50ms(void);
void updateRGB(void);
/********************************************************************/
// Global Variables
/********************************************************************/
// globals because it is accessed in main and in the rti callback
unsigned int dutyCyclePartA = 25; 
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
  // these will be used for the inital duty cycle values so they are placed here
  static unsigned char* RGBDutyCycles[3]; // cannot put the rgb values into the 2d array below because of the const keyword making the array below immutable
  unsigned char buffer[23]; // putting buffer here because it is used for displaying the default values

  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
Clock_Set20MHZ();
RTI_InitCallback_ms(&rtiCallback50ms, 50);
SWL_Init();

// must display any static items on the lcd right after the init
lcd_Init();
lcd_Clear();
lcd_DispControl(0,0);
// Print static string to the LCD
lcd_StringXY(0, 3, "Duty Cycle:");
lcd_StringXY(0, 0, "R:");
lcd_StringXY(6, 0, "G:");
lcd_StringXY(12, 0, "B:");
lcd_StringXY(0, 1, "Edit:");
lcd_StringXY(0, 2, "Step:"); 

/******* PWM Config *******/
// Part A
PWMPOL_PPOL3 = 1; //Set channel 3 to start the cycle high and go low when duty count is reached (8.3.2.2)
PWMPRCLK |= PWMPRCLK_PCKB1_MASK; // Set prescaler B to 4 (PCKB1), Clock B set to 200ns (8.3.2.4)
PWMCLK_PCLK3 = 1; //Use Clock SB (Scaled Clock B) for channel 3. This allows a clock slower than 156.25 kHz (8.3.2.3)
PWMSCLB = 25; //Clock SB = Clock B / (2 * PWMSCLB), to get a 100 kHz clock with a 10 us tick, scaleB must be 25 (8.3.2.10)
             // This will allow an 8-bit PWM to create a 130.7 Hz to  16 kHz period
PWMPER3 = 100; // Period of 100 ticks (~1ms period) - just makes setting duty cycle easier (8.3.2.13)
PWMDTY3 = dutyCyclePartA; // Setting duty cycle, starting at 25% ((25/100)*100) (8.3.2.14) 
PWME_PWME3 = 1; // Enable PWM channel 3 (LCD display on pin 1) (8.3.2.1)

// Part B
PWMPOL |= PWMPOL_PPOL0_MASK | PWMPOL_PPOL1_MASK | PWMPOL_PPOL4_MASK; // Set channel 0 (pin 4), 1 (pin 3), 4 (pin 112) to start the cycle high and go low when duty count is reached (8.3.2.2)
PWMPRCLK |= PWMPRCLK_PCKA2_MASK ; // Set prescaler A to 16 -> Clock A set to 20MHz/16 = 1.25MHz or 800ns
// don't need to use scaleA as the output frequency is relatively high already

// at 800ns per tick, 255 would roughly be 4.9kHz output frequency (close enough?)
PWMPER0 = 255;
PWMPER1 = 255;
PWMPER4 = 255;
// assigning registers to array -> adding the addresses so the addresses can be dereferenced to be assigned values
RGBDutyCycles[0] = &PWMDTY4;
RGBDutyCycles[1] = &PWMDTY1;
RGBDutyCycles[2] = &PWMDTY0;
*RGBDutyCycles[0] = *RGBDutyCycles[1] = *RGBDutyCycles[2] = 1; // start off white at low brightness 
PWME |= PWME_PWME0_MASK | PWME_PWME1_MASK | PWME_PWME4_MASK; // Enable PWM channel 0, 1, 4 (8.3.2.1)

// just here to set the default selection
lcd_StringXY(6, 1, "Red"); 
sprintf(buffer, "%03u", *RGBDutyCycles[0]);
lcd_StringXY(2, 0, buffer);
sprintf(buffer, "%03u", *RGBDutyCycles[1]);
lcd_StringXY(8, 0, buffer);
sprintf(buffer, "%03u", *RGBDutyCycles[2]);
lcd_StringXY(14, 0, buffer);
lcd_StringXY(5, 2, "01"); // this is for step
/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    // static keyword persists the variable, so only gets initialized once at the top of this block
    static SwState upState, downState, centerState, rightState, leftState = Idle;
    static char index = 0; // index for the RGB values in the array
    static unsigned char step = 1; // inital step is by 1 for the led PWM
    const char RGB[3][6] = {{'R', 'e', 'd', ' ', ' ', '\0'}, {'G', 'r', 'e', 'e', 'n', '\0'}, {'B', 'l', 'u', 'e', ' ', '\0'}};
    
    // checking switch states
    Sw_State(&upState, SWL_UP);
    Sw_State(&downState, SWL_DOWN);
    Sw_State(&centerState, SWL_CTR);
    Sw_State(&leftState, SWL_LEFT);
    Sw_State(&rightState, SWL_RIGHT);

    // Part A
    if(upState == Pressed || downState == Pressed || centerState == Pressed || leftState == Pressed || rightState == Pressed)
      PWMDTY3 = dutyCyclePartA = 100; // setting duty cycle to 100% when any switch is pressed (8.3.2.14) 

    // Part B
    if(upState == Pressed){
      // need to account for changes in step, so check if an overflow would occur first, then clamp if overflow would happen
      *RGBDutyCycles[index] = ((*RGBDutyCycles[index] + step) < 255) ? *RGBDutyCycles[index] + step : 255;
      updateRGB();
    }
    if(downState == Pressed){
      // need to account for changes in step, so check if an underflow would occur first, then clamp if underflow would happen
      *RGBDutyCycles[index] = ((*RGBDutyCycles[index] - step) > 1) ? *RGBDutyCycles[index] - step : 1;
      updateRGB();
    }
    if(centerState == Pressed){
      step = (step == 20) ? 1 : 20; // toggle between a step of 1 or 20
      sprintf(buffer, "%02u", step);
      lcd_StringXY(5, 2, buffer);
    }
    if(leftState == Pressed){
      index = (index > 0) ? --index : sizeof(index)+1; // decrement index and wraps
      lcd_StringXY(6, 1, RGB[index]); // display on lcd
    }
    if(rightState == Pressed){
      index = (index < 2) ? ++index : 0; // increment index and wraps
      lcd_StringXY(6, 1, RGB[index]); // display on lcd
    }
    if(flags.updateDisplays){
      flags.updateDisplays = 0;
      sprintf(buffer, "%02u%%", dutyCyclePartA); // %% to display the percent sign
      lcd_StringXY(11, 3, buffer); // displaying the duty cycle
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/
void rtiCallback50ms(void){
  PWMDTY3 = (dutyCyclePartA > 25) ? --dutyCyclePartA : 25; // every 50ms decrement the duty cycle by 1% (8.3.2.14) 
  flags.updateDisplays = 1;
}
void updateRGB(void){
  unsigned char lcdBuffer[5];
  sprintf(lcdBuffer, "%03u", PWMDTY4);
  lcd_StringXY(2, 0, lcdBuffer);
  sprintf(lcdBuffer, "%03u", PWMDTY1);
  lcd_StringXY(8, 0, lcdBuffer);
  sprintf(lcdBuffer, "%03u", PWMDTY0);
  lcd_StringXY(14, 0, lcdBuffer);
}
/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
