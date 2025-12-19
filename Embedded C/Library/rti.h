//real time interrupt library
//Jonathan Le
//March 6, 2025
extern volatile unsigned long rtiMasterCount;

void RTI_Init(void);
void RTI_Delay_ms(unsigned long delay);
void RTI_NonblockingDelay(unsigned long delay);

void RTI_InitCallback(void (*pCallback)(void));
void RTI_InitCallback_ms(void (*pCallback)(void), unsigned long);