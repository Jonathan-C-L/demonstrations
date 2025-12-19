/********************************************************************/
// HC12 Library:  Serial Communications Interface
// Processor:     MC9S12XDP512
// Author:        Jonathan Le
// Details:       Serial Communications Interface
//                PS0 (RXD0) is PIN 89
//                PS1 (TXD0) is PIN 90                
// Date:          March 13, 2025
// Revision History :
//  March 13, 2025 - Initial build


/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "sci.h"
#include "clock.h"

/********************************************************************/
// Global Variables
/********************************************************************/

/********************************************************************/
// Library Functions
/********************************************************************/
// Read a byte, non-blocking,
// returns 1 if byte read, 0 if not.
unsigned char sci0_rxByte(char* pRxByte)
{
    if(SCI0SR1 & SCI0SR1_RDRF_MASK)
    {
        *pRxByte = SCI0DRL;
        // Character received.
        return 1;
    }
    // Character NOT received.
    return 0;
}

// Send a null-terminated string over SCI.
void sci0_txStr (char* strIndex)
{
    while(*strIndex != 0)
    {
        sci0_txByte(*strIndex++);
    }
}

int SCI_Init(unsigned long ulBaudRate){
    unsigned long ulBaudDivider;
    // No need to modify this register
    SCI0CR1 = 0;
    // Enable Transmit and Receive and Rx Interrupt on the control register 2
    // The control register (TE and RE) must be activated first before the Baud rate is calculated
    SCI0CR2 = SCI0CR2_TE_MASK | SCI0CR2_RE_MASK | SCI0CR2_RIE_MASK;
    // Baud rate divider = BusSpeed/16/Baud Rate (11.3.2.1)
    // Multiply by 10, add five, and then divide by 10 to implement rounding without floats
    ulBaudDivider = (((Clock_GetBusSpeed()/16)*10)/ulBaudRate+5)/10;
    // Set the 12 bit Baud rate setting (11.3.2.1) by assigning the calculated ulBaudDivider into the Baud rate register
    SCI0BD = (int)ulBaudDivider;
    return (int)ulBaudDivider;
}

// Send a character byte over SCI.
void sci0_txByte (char byte)
{
    // BLOCKING 1 byte transmission
    // Wait till transmit data register is empty by waiting until the TDRE in the Status register is 1 (the data register is empty).
    while(!(SCI0SR1_TDRE));
    // Send character by assigning information in 'byte' into the Data register.
    SCI0DRL = byte;

    //NON-BLOCKING 1 byte transmission
    //   if(SCI0SR1_TDRE){
    //     SCI0DRL = btye;
    //   }
}


/********************************************************************/
// Another group of library functions
/********************************************************************/
/*
//clear both transmit and receive buffers
void ClearBuffers(void){
  // Clear rxBuffer with 0's for next received message.
  for(rxIndex=0; rxIndex<BUFFER_SIZE; rxIndex++){
    rxBuffer[rxIndex] = 0;
  }
  // Reset rxIndex to 0 for next received message.
  rxIndex = 0;
  // Clear txBuffer with 0's for next transmitted message.
  for(txIndex=0; txIndex<BUFFER_SIZE; txIndex++){
    txBuffer[txIndex] = 0;
  }
  // Reset txIndex to 0 for next transmitted message.
  txIndex = 0;
}

//clear only transmit buffer
void ClearTxBuffer(void){
  // Clear txBuffer with 0's for next transmitted message.
  for(txIndex=0; txIndex<BUFFER_SIZE; txIndex++){
    txBuffer[txIndex] = 0;
  }
  // Reset txIndex to 0 for next transmitted message.
  txIndex = 0;
}

//clear only receive buffer
void ClearRxBuffer(void){
    // Clear rxBuffer with 0's for next received message.
    for(rxIndex=0; rxIndex<BUFFER_SIZE; rxIndex++){
      rxBuffer[rxIndex] = 0;
    }
    // Reset rxIndex to 0 for next received message.
    rxIndex = 0;
}




*/
/********************************************************************/
// Another title placeholder
/********************************************************************/


/********************************************************************/
// Interrupt Service Routines Template
/********************************************************************/


// interrupt VectorNumber_Vsci0 void SCI0_ISR(void){
//   if(SCI0SR1 & SCI0SR1_RDRF_MASK){ //Checking the Received Data Ready Flag will clear the flag automatically (11.3.2.7)
//     volatile char rxData; //A temporary variable to store the data in the Rx register
//     rxData = SCI0DRL; //Read the Rx'd byte
//     if (rxData == '\r'){ //If the transmitted character was the carriage return
//       flags.dataReady = 1; // Signal that the Rx Buffer is ready to be processed
//       rxBuffer[rxIndex++] = '\0'; //Null terminate the string in the Rx Buffer
//       rxIndex = 0; //Reset the rxIndex to 0
//     }
    
//     else{
//       rxBuffer[rxIndex++] = rxData; //If the Rx'd character wasn't a carriage return, add it to the rxBuffer and increment the index
//     }
//   }

//   if((SCI0SR1 & SCI0SR1_TDRE_MASK) && SCI0CR2_TIE){ //If the Transmit Data register is empty, and the Transmitter interrupt is enabled
//     if (txBuffer[txIndex]){ //If the character in the txBuffer isn't a \0
//       SCI0DRL = txBuffer[txIndex++]; //Transmit the data and increment the txIndex
//     }
//     else{ // If the character was \0, the string in the buffer has been sent
//       SCI0CR2_TIE = 0; //Disable the transmitter interrupt
//       txIndex = 0; //Reset the txIndex
//     }
//   }
// }


/*

//RECEIVING ONLY
interrupt VectorNumber_Vsci0 void Vsci0_ISR(void)
{
  SWL_TOG(SWL_YELLOW);
  if(sci0_rxByte(&rxByte))
  {
    if(rxByte == '\r'){
      endOfMessageReceived = 1;
      SWL_TOG(SWL_RED);
    }
    else if(rxIndex < BUFFER_SIZE)
      // Add received byte to buffer array.
      rxBuffer[rxIndex++] = rxByte;
  }
}

//RECEIVING AND SENDING
interrupt VectorNumber_Vsci0 void Vsci0_ISR(void)
{
  if(sci0_rxByte(&rxByte))
  {
    // Messsage is ended when
    // CR or '\r' is sent from
    // Tera-Term.
    if(rxByte == '\r'){
      endOfMessageReceived = 1;
    }
    else if(rxByte == '\n'){
      // Do Nothing but make sure that
      // Tera-Term sends both a CR and LF
      // when Enter is pressed.
    }
    else if(rxIndex < BUFFER_SIZE){
      // Add received byte to buffer array.
      rxBuffer[rxIndex++] = rxByte;
    }
  }
}

*/