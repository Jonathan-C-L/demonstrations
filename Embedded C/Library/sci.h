// Serial Communications Interface
// Jonathan Le
// 2025-03-13

#define BUFFER_SIZE 64

volatile static unsigned char rxBuffer[128]; //Create a buffer for receiving data
volatile static unsigned int rxIndex = 0; //Create an index for accessing the rxBuffer
volatile static unsigned char txBuffer[128]; //Create a buffer for transmitting data
volatile static unsigned int txIndex = 0; //Create an index for accessing the txBuffer

volatile static struct __FlagBits //Create a structure of bitfields for flags. 
{
    unsigned int sciReceiving:1; //Flag to indicate that the SCI is receiving data and hasn't timed out yet
    unsigned int dataReady:1;  //Flag to indicate that rxBuffer is ready to process
    unsigned int dataSending:1;
} sci_flags;

int SCI_Init(unsigned long ulBaudRate);
void sci0_txByte (char txByte);
void sci0_txStr (char* strIndex);
unsigned char sci0_rxByte(char* pRxByte);
// void ClearBuffers(void);
// void ClearTxBuffer(void);
// void ClearRxBuffer(void);


/************************Variable Inits***************************/
/*
char rxByte = 0;
char rxBuffer[BUFFER_SIZE];
int rxIndex = 0;
char txBuffer[BUFFER_SIZE];
int txIndex = 0;
int endOfMessageReceived = 0; //for rti function
*/
