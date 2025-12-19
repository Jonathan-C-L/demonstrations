/********************************************************************/
// HC12 Program:  ICA6 - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       Basic AtoD Control               
// Date:          Nov. 7, 2025

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

//Other system includes or your includes go here
// #include <stdlib.h>
#include <stdio.h> // required for this project
#include <string.h> // required for memory manipulation of strings

/********************************************************************/
//Defines
/********************************************************************/
// flags to represent the state of processes/registers
volatile struct _flag{
  unsigned int startConversion:1;
  unsigned int updateDisplay:1;
  unsigned int multEnable:1;
} flag;
/********************************************************************/
// Local Prototypes
/********************************************************************/
void rtiCallback(void);
void atdCallback(void);
/********************************************************************/
// Global Variables
/********************************************************************/
SwState centerState;
unsigned int resultSteps[3]; //Buffer for holding 3 conversion results

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
  RTI_InitCallback(&rtiCallback);
  lcd_Init();
  SWL_Init();
  Segs_Init();
  lcd_DispControl(0,0); //Disable the cursor and blinking

  //Configure ADC

  // Power on the ADC Module, Enable Fast Flag Clear, Interrupt enabled, Power in wait mode (5.3.2.3)
  ATD_Config(ATD_ON, ATD_FFC, ATD_PD_WAIT, ATD_IEN);


  // Wrapping around AN6 (5.3.2.1) (Table 5-2)
  ATD_SetWrap(ATD_CH6);

  // Perform sequence of four and finish conversion during freeze (5.3.2.4) (Table 5-8)
  // An integer variable and bitwise operations could be used to parameterize the sequence length.  
  ATD_SetNumConversions(ATD_4CONV, ATD_FRZ_FIN);

  // Divide clock by 20 for 1MHz AtoD clock
  // 10-bit resolution, Hold sample for 4 A/D conversion clock periods, set AtoD clock prescaler to 20 
  //(5.3.2.5) (Table 5-11) (Table 5-12)
  ATD_SetClock(ATD_10bit, ATD_PreS20, ATD_ST4);

  ATD_SetScanBehaviour(ATD_RJ_U, ATD_SCAN, ATD_MULT, ATD_CH4); //Right-justify data, Sample multiple channels, Begin on channel 4
                                                               //All writes to will ATD0CTL5 begin conversion (5.3.2.6)
  flag.multEnable = 1; // start with the green led on as the ATD starts with MULT enabled
  ATD_SetCallback(&atdCallback);
/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    unsigned int counter;
    char stringBuffer[21]; //Buffer for holding LCD strings. 20 Charaters + 1 null termination character

    if(flag.multEnable)
      SWL_ON(SWL_GREEN);
    else
      SWL_OFF(SWL_GREEN);
    if(flag.startConversion){
      flag.startConversion = 0; // reset flag

      // Part A:
      // SWL_ON(SWL_YELLOW);
      // ATD0CTL5 = ATD0CTL5_DJM_MASK | ATD0CTL5_MULT_MASK | 4; //Right-justify data, Sample multiple channels, Begin on channel 4
      //                                                        //All writes to will ATD0CTL5 begin conversion (5.3.2.6)
      // while(!(ATD0STAT0 & ATD0STAT0_SCF_MASK)); // Wait for Sequence Complete flag to go high (5.3.2.7)
      // SWL_OFF(SWL_YELLOW);

      if(flag.updateDisplay){
        flag.updateDisplay = 0; // reset flag
        
        Segs_16D(resultSteps[0], Segs_LineTop);
        for(counter = 0; counter < 3; counter++){ // Use a for loop to create a string with each result and display them on the LCD
          sprintf(stringBuffer, "%u:%04u steps~ %04umV", counter + 4, resultSteps[counter], resultSteps[counter]*5);
          lcd_StringXY(0, counter, stringBuffer);
        }
      }
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/
void rtiCallback(void){
  flag.startConversion = 1;
  Sw_State(&centerState, SWL_CTR); // checking if the center button was pressed
  if(centerState == Pressed){
    // Clearing and setting the MULT channel in the ATD0CTL5 register to the opposite of the current MULT state (5.3.2.6) (Table 5-13)
    ATD0CTL5 ^= ATD0CTL5_MULT_MASK;
    flag.multEnable = !flag.multEnable;
  }
  if(!(rtiMasterCount%500)){ // 500 so number changes could be viewed a bit easier
    flag.updateDisplay = 1; // Flag to change the lcd display
    SWL_TOG(SWL_RED);
  }
}
void atdCallback(void){
  memcpy(resultSteps, &ATD0DR0, sizeof(resultSteps)); //Copy the first 4 AtoD data registers to the resultSteps array. The `sizeof()` function allows the number of
                                                      // bytes being copied to automatically adjust if the size of the resultSteps array is modified
                                                      // Reading the results register will clear the Sequence Complete flag when Fast Flag Clear is enabled (5.3.2.7)
                                                      // Clearing the Sequence Complete Flag will also clear the Sequence Complete Interrupt Flag (5.3.2.3)
  SWL_TOG(SWL_YELLOW); //Toggle the red LED every time a sequence is complete. This will allow direct measurement of the conversion time with an oscilloscope
}
/********************************************************************/
// Interrupt Service Routines
/********************************************************************/