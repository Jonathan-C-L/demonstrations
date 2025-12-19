/********************************************************************/
// HC12 Program:  ICA - Embedded Applications
// Processor:     MC9S12XDP512
// Bus Speed:     20 MHz
// Author:        Jonathan Le
// Details:       SCI - Packets and time syncing               
// Date:          Oct. 13, 2025

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


//Other system includes or your includes go here
#include <stdlib.h>
#include <stdio.h>


/********************************************************************/
//Defines
/********************************************************************/
#define SCI_TIMEOUT 5
enum Header{
  None = 0,
  UnixTime = 0xA1,
  LocalTime = 0xA2,
  RGB = 0xA4,
  Servo = 0xA4,
  TextMsg = 0xB1,
  Counter = 0xB2
} Header;
volatile struct _flag{
    unsigned int overrunMessage:1; // Flag to clear lcd
    unsigned int timeoutMessage:1; //Flag to indicate that the displayed time should be updated
    unsigned int loadingMessage:1; //Flag to indicate that the displayed time should be updated
    unsigned int clearLcd:1;
    unsigned int haltedMessage:1;
    unsigned int updateDisplays:1;
} flags;
/********************************************************************/
// Local Prototypes
/********************************************************************/
void ToggleRedLED(void);
/********************************************************************/
// Global Variables
/********************************************************************/
SwState upState, downState, centerState, rightState, leftState;
unsigned char displayType = LocalTime;
unsigned int sciTimeoutCounter = 0; //Counter to track the SCI timeout
unsigned int numBytes = 0;
unsigned long unixTime = 0;
const char* month[] = {"Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"};
const char* day[] = {"Sun", "Mon", "Tues", "Wed", "Thurs", "Fri", "Sat"}; 

typedef struct DateTimeStruct_Typedef
{
  uint Year;
  uint Month;
  uint MonthDay;
  uint Day;
  byte Hours;
  byte Minutes;
  byte Seconds;
  byte HuSeconds; //hundreths of seconds (10ms)
} DateTimeStruct;

DateTimeStruct dateTime;

/********************************************************************/
// Constants
/********************************************************************/

/********************************************************************/
// Main Entry
/********************************************************************/
void main(void)
{
  //Any main local variables must be declared here
  unsigned char buffer[32];
  unsigned char bufferTime[32];
  // main entry point
  _DISABLE_COP();
  EnableInterrupts;
  
/********************************************************************/
  // one-time initializations
/********************************************************************/
Clock_Set20MHZ();
RTI_Init();
SWL_Init();
RTI_InitCallback_ms(&ToggleRedLED, 10);
SCI_Init(115200);
Segs_Init();
lcd_Init();
/********************************************************************/
  // main program loop
/********************************************************************/

  for (;;)
  {
    Sw_State(&leftState, SWL_LEFT); // unix time
    Sw_State(&rightState, SWL_RIGHT); // local time

    if(leftState == Pressed){
      displayType = LocalTime; // change to local time
    }
    if(rightState == Pressed){
      displayType = UnixTime; // change to unix time
    }

    // if information has been full processed - flag set off in the RTI interrupt
    if(sci_flags.dataReady){
      lcd_Clear(); 
      switch (rxBuffer[0]){
        case LocalTime:
          // must use address of as I'm starting at the second position
          memcpy(&dateTime, &rxBuffer[1], sizeof(dateTime)); //Move Rx'd data into dateTime structure
          sprintf(buffer, "%s", "Synced: Local Time");
          displayType = LocalTime;
          sprintf(bufferTime, "%s %s %d, %d", day[dateTime.Day-1], month[dateTime.Month-1], dateTime.MonthDay, dateTime.Year);
          lcd_StringXY(0, 0, bufferTime);
          break;
        case UnixTime:
          memcpy(&unixTime, &rxBuffer[1], 4);
          sprintf(buffer, "%s", "Synced: Unix Time");
          displayType = UnixTime;
          break;
        default:
          sprintf(buffer, "%s", "Invalid Packet");
          break;
      }
      lcd_StringXY(0, 2, buffer); 

      sprintf(buffer, "Rx: %02d bytes rcvd.", numBytes);
      lcd_StringXY(0, 3, buffer);
      //clear buffer
      memset(rxBuffer, 0, rxIndex);
      rxIndex = 0;
      sci_flags.dataReady = 0;
    }
    // display LCD and 7-segs
    if(flags.updateDisplays){
      flags.updateDisplays = 0; //Clear the update flag
      if(displayType == LocalTime){
        sprintf(bufferTime, "%02d:%02d:%02d  ", dateTime.Hours, dateTime.Minutes, dateTime.Seconds);
      }
      if(displayType == UnixTime){
        sprintf(bufferTime, "%010lu", unixTime); // totalSeconds is unsigned long, use %lu*
      }
      lcd_StringXY(0, 1, bufferTime);
      Segs_16D(dateTime.HuSeconds, Segs_LineTop);
      Segs_16D(unixTime, Segs_LineBottom); // for verification of totalSeconds
    }
  }                   
}

/********************************************************************/
// Functions
/********************************************************************/
void ToggleRedLED(void){
  SWL_TOG(SWL_RED);
  // incrementing the DateTime struct
  if(++(dateTime.HuSeconds) > 99){
    dateTime.HuSeconds = 0;
    if(++(dateTime.Seconds) > 59){
      dateTime.Seconds = 0;
      if(++(dateTime.Minutes) > 59){
        dateTime.Minutes = 0;
        if(++(dateTime.Hours) > 23){
          dateTime.Hours = 0;
        }
      }
    }
  }
  
  flags.updateDisplays = 1; // callback is is 10ms delay

  // unixtime increment
  if(!(rtiMasterCount%100))
    unixTime++; // storing the number of seconds elapsed
  if(sci_flags.sciReceiving){ //If the SCI timeout should be running
    if(sciTimeoutCounter++ >= SCI_TIMEOUT){ //Check if the counter is 5ms and then increment it
      sci_flags.sciReceiving = 0; //If the timer has been running for 5 miliseconds, clear the flag
      sci_flags.dataReady = 1; //Signal that the rx buffer is ready to process

      // a struct is being received, so there is no need to null terminate the rxBuffer
      // rxBuffer[rxIndex] = '\0'; //Null terminate the received string
      numBytes = rxIndex; // last moment before the count of bytes is reset
      rxIndex = 0; //Reset the rxIndex
    }
  }
}
/********************************************************************/
// Interrupt Service Routines
/********************************************************************/
interrupt VectorNumber_Vsci0 void SCI0_ISR(void){
  if(SCI0SR1 & SCI0SR1_RDRF_MASK){ //Checking the Received Data Ready Flag will clear the flag automatically
    sci_flags.sciReceiving = 1; //Start running the SCI Timeout timer
    sciTimeoutCounter = 0; //Reset the timeout counter every time a byte is received

    rxBuffer[rxIndex++] = SCI0DRL; //Read the Rx'd byte
      
  }

  if((SCI0SR1 & SCI0SR1_TDRE_MASK) && SCI0CR2_TIE){ //If the Transmit Data register is empty, and the Transmitter interrupt is enabled
    if (txBuffer[txIndex]){ //If the character in the txBuffer isn't a \0
      SCI0DRL = txBuffer[txIndex++]; //Transmit the data and increment the txIndex
    }
    else{ // If the character was \0, the string in the buffer has been sent
      SCI0CR2_TIE = 0; //Disable the transmitter interrupt
      txIndex = 0; //Reset the txIndex
    }
  }
}