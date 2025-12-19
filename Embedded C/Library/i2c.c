//IIC0 Library Files
//Processor:  MC9S12XDP512
//Crystal:  16 MHz
//by carlos Estay
//December 2020

#include <hidef.h>
#include "derivative.h"
#include "i2c.h"
#include "pit.h"
/*

- START CONDITION -> Drive SDA LOW when SCL is HIGH (Want to Transfer Data)
- STOP CONDITION  -> Drive SDA HIGH when SCL is HIGH (Want to release the BUS)

- DATA -> 8 bits of information, can be: Control code, address, or data
  Data on SDA line is only valid when SCL is HIGH

- ACK/NACK -> The device can ack the tyransfer of each byte by bringing the SDA line
  LOW during the 9th clock pulse of SCL

- Addressing -> Can be 7-BIT or 10-BIT
*/

/*
According to the schematics

NAME  |  PORT  |  PIN
--------------------
SDA   |  PJ6   | 99
SCL   |  PJ7   | 98

*/

//------------------------------------------------------------Init Function
void I2C0_Init(void)
{
    //Attempt to take over the BUS
    int i;
    IIC0_IBCR &= ~IIC0_IBCR_IBEN_MASK;    //Disable Bus
    DDRJ |= DDRP_DDRP7_MASK;            //Manually set SCL as output

    for ( i = 0; i < 20; i++)
    {
        PIT_Delay_us(PIT_CH3, PIT_MT0, 1); //Replace this with your Delay Microseconds, timer based ideally
        DDRJ ^= DDRP_DDRP7_MASK;    //Toggle SCL every 5[us] -> 10[us] period -> 100[Khz]
    }
    DDRJ &= ~DDRP_DDRP7_MASK;        //Release SCL(PJ7)
    PIT_Delay_us(PIT_CH3, PIT_MT0, 20);   //A delay for the micro to release PJ7 properly before activating IIC0
    //Set clock to 100[khz](divide by 80), SDA Hold = 20clks, SCL Hold Start = 32clks, SCL hold Stop = 42clks
    IIC0_IBFD = 0x47;     
    IIC0_IBCR = 0;  //Reset Register
    IIC0_IBCR |= IIC0_IBCR_IBEN_MASK;   //Enable BUS, has to be done before any modiufication to IBCR
    IIC0_IBCR &= ~(IIC0_IBCR_IBIE_MASK | IIC0_IBCR_IBSWAI_MASK);  //no interrupts, normal WAI  
}

//------------------------------------------------------Init with Bus Speed
void I2C0_InitBus(enum I2C_BusRate bRate)
{
    //Attempt to take over the BUS
    int i;
    IIC0_IBCR &= ~IIC0_IBCR_IBEN_MASK;    //Disable Bus
    DDRJ |= DDRP_DDRP7_MASK;            //Manually set SCL as output

    for ( i = 0; i < 20; i++)
    {
       PIT_Delay_us(PIT_CH3, PIT_MT0, 1);//Replace this with your Delay Microseconds, timer based ideally
        DDRJ ^= DDRP_DDRP7_MASK;    //Toggle SCL every 5[us] -> 10[us] period -> 100[Khz]
    }
    DDRJ &= ~DDRP_DDRP7_MASK;        //Release SCL(PJ7)
    PIT_Delay_us(PIT_CH3, PIT_MT0, 20);    //A delay for the micro to release PJ7 properly before activating IIC0
    //Set clock to 100[khz](divide by 80), SDA Hold = 20clks, SCL Hold Start = 32clks, SCL hold Stop = 42clks
    IIC0_IBFD = (char)bRate;     
    IIC0_IBCR = 0;  //Reset Register
    IIC0_IBCR |= IIC0_IBCR_IBEN_MASK;   //Enable BUS, has to be done before any modiufication to IBCR
    IIC0_IBCR &= ~(IIC0_IBCR_IBIE_MASK | IIC0_IBCR_IBSWAI_MASK);  //no interrupts, normal WAI  
}


//---------------------------------------------------------------Bus Busy
int I2C0_WaitForBus()
{
    int count = WAIT_COUNT_MAX;

    while(--count > 0)
    {
        if(!(IIC0_IBSR & IIC0_IBSR_IBB_MASK)) //Check busy flag in status register, 1 if busy
        {//If not busy
            return 0;
        }
    }
    return -1;
}
//--------------------------------------------------------------READ/WRITE From/To Address
int I2C0_Addr(char Address, char IsRead, char WaitForBus)
{

    if (WaitForBus)
    {      
      if (I2C0_WaitForBus()) return -1;   //bus is busy -- deal with it in main.c     
    }  

    /*
    Take over the BUS:
    - Upon Reset, the MS/SL bit is cleared
    - When this bit is changed from 0 to 1, a START is generated
    - When this bit is changed from 1 to 0, a STOP is generate

    To send An address, we need to generate a START and transmit first, address is always transmit
    */

    IIC0_IBCR |= IIC0_IBCR_MS_SL_MASK | IIC0_IBCR_TX_RX_MASK;   //Master mode, start TX


    if(IsRead == IIC0_WRITE) //Operation is a Write
    {
        IIC0_IBDR = (Address << 1) & 0xFE; //Send slave address shifted by 1 and WRITE
    }
    else if(IsRead == IIC0_READ)//Operation is a READ
    {
        IIC0_IBDR = (Address << 1) | 0x01; //Send slave address shifted by 1 and READ
    }
    
    //Check Datasheet 9.7.1.3
    while(!(IIC0_IBSR & IIC0_IBSR_IBIF_MASK));    //Wait for Byte transfer complete

    IIC0_IBSR |= IIC0_IBSR_IBIF_MASK;   //Clear flag

    //Check for ACK 
    if(!(IIC0_IBSR & IIC0_IBSR_RXAK_MASK))
    {// 0 -  Acknowledge Received
        return 0;  
    }
    else
    {// 1 -  NO Acknowledge Received
        IIC0_IBCR &= ~IIC0_IBCR_MS_SL_MASK; //Stop
    }
    return -1;
}
//----------------------------------------------------------------------Write Byte
int I2C0_WriteByte(char Val, char IssueStop)
{
    int count = WAIT_COUNT_MAX;

    //Write byte to send
    IIC0_IBDR = Val;

    //Wait for flag on Timeout
    while((!(IIC0_IBSR & IIC0_IBSR_IBIF_MASK)) && (--count > 0));

    // did we timeout?
    if (count <= 0)
        return -1;
    //No timeout, 
    if(IssueStop)
    {
        IIC0_IBCR &= ~IIC0_IBCR_MS_SL_MASK; //Stop
    }
    if(IIC0_IBSR & IIC0_IBSR_IBIF_MASK)
    {
        IIC0_IBSR |= IIC0_IBSR_IBIF_MASK;   //Clear flag if active
    }
    
    //Write sucessful
    return 0;
}
//---------------------------------------------------------------------Read Byte
int I2C0_ReadByte(char *buff, char IssueAck, char IssueStop)
{
    char junk;    //needed for first fake-read of register

    // //Setup as receiver
    // IIC0_IBCR &= ~IIC0_IBCR_TX_RX_MASK;

    if(IssueAck)
    {
        IIC0_IBCR &= ~ IIC0_IBCR_TXAK_MASK; //Ack ON
    }
    else
    {
        IIC0_IBCR |= IIC0_IBCR_TXAK_MASK; //Ack OFF
    }
    
    //Setup as receiver
    IIC0_IBCR &= ~IIC0_IBCR_TX_RX_MASK;

    //Start read process
    junk = IIC0_IBDR; 

    while(!(IIC0_IBSR & IIC0_IBSR_IBIF_MASK));  //wait for flag to be SET

    //issue stop, if desired
    if (IssueStop)
    {
        IIC0_IBCR &= ~IIC0_IBCR_MS_SL_MASK;
    }

    IIC0_IBSR |= IIC0_IBSR_IBIF_MASK; //Clear flag

    
    // get the actual byte
    *buff = IIC0_IBDR;  

    return 0; //Good Condition

}
//-----------------------------------------------------------Repeat Start
/*
This is to send another START if no STOP has been previously sent.
Every START has to end with a STOP at some point, otherwise you have to
use this function (repeat start).
A STOP can be sent at the end of the last byte you are transmiting or
receiving by setting the IssueStop variable as IIC0_STOP (1)
*/
void I2C0_RepeatStart(void)
{
    IIC0_IBCR |= IIC0_IBCR_RSTA_MASK;
}
//-----------------------------------------------------------Write vontage in DAC
int I2C0_WriteDAC(unsigned int value, char dac)
{
/*
    DAC INPUT WORD (LTC2633-12)
    *****1ST DATA BYTE*****   ******2ND DATA BYTE******    ***3RD DATA BYTE***
    C3 C2 C1 C0 A3 A2 A1 A0   D11 D10 D9 D8 D7 D6 D5 D4    D3 D2 D1 D0 X X X X

    DAC FIRST BYTE**************
    For writing into the DAC we'll use command 0b0010 = 0x2 =>
    Write to Input Register n, Update (Power-Up) All
    This will be the most significant nibble. The following (least significant)
    nibble will be the Address code, which is according to the table:

    A3 A2 A1 A0
    0 0 0 0 DAC A
    0 0 0 1 DAC B
    1 1 1 1 All DACs

    DAC SECOND and THIRD BYTES*******************
    The following bytes contain the number to write, which is 12-bits
    left justified, which means the number to send has to be shifted to 
    the left by 4 and the 4 least significant bits are don't care.
*/
    // How I think this is working:
    // *casting a char will truncate the most significant byte
    // 1111 1111 1111 1111
    // MSB = (1111 1111 1111 1111 >> 8) => 0000 0000 [1111 1111]
    // LSB = (1111 1111 1111 1111) => 1111 1111 [1111 1111]

    unsigned int shiftedBytes = value << 4;
    int result;
    result = I2C0_Addr(0x10, IIC0_WRITE, IIC0_WAIT); //Address the DAC
    result = I2C0_WriteByte(dac, IIC0_NOSTOP); //Command it to turn on and update channel A
    result = I2C0_WriteByte(shiftedBytes >> 8, IIC0_NOSTOP); //Send the MSB byte of the new value
    result = I2C0_WriteByte(shiftedBytes, IIC0_STOP); //Send the LSB 4-bits of the value and send a stop
    return result;
}
//------------------------------------------------------------Write 8-bit register w/Addr
int I2C0_RegWrite(char addr, char reg, char data, char issueStop)
{
    int result;
    result = I2C0_Addr(addr, IIC0_WRITE, IIC0_WAIT);
    result = I2C0_WriteByte(reg, IIC0_NOSTOP);
    result = I2C0_WriteByte(data, issueStop);
    return result;
}
//------------------------------------------------------------Read 8-bit register w/Addr
int I2C0_RegRead(char *buff, char addr, char reg)
{
    int result;
    result = I2C0_Addr(addr, IIC0_WRITE, IIC0_WAIT);
    result = I2C0_WriteByte(reg, IIC0_NOSTOP);
    I2C0_RepeatStart();
    //We cannot use our I2C0_Addr() function as is after a restart
    //We will do it manually writing into the register and waiting/clearing flag
    IIC0_IBDR = (addr<<1) | 0x1; //Addr w/READ operation
    while(!(IIC0_IBSR & IIC0_IBSR_IBIF_MASK));  //wait for flag to be SET
    IIC0_IBSR |= IIC0_IBSR_IBIF_MASK; //Clear flag
    result = I2C0_ReadByte(buff, IIC0_NACK, IIC0_STOP);//Read Byte, issue STOP at the end
    return result;
}




