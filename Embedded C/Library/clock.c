/********************************************************************/
// HC12 Library:  clock
// Processor:     MC9S12XDP512
// Author:        Jonathan Legit 
// Details:       clock functions
//                                
// Date:          Feb. 27, 2025
// Revision History :
//  Feb. 27 2024      - Initial build
//  March 6, 2025     - Setup of setup clock speed functions


/********************************************************************/
// Library includes
/********************************************************************/
#include <hidef.h>      /* common defines and macros */
#include "derivative.h" /* derivative-specific definitions */
#include "clock.h"

/********************************************************************/
// Global Variables
/********************************************************************/
#define DEF_BUS_CLOCK 8000000
static unsigned long ClockSpeed = DEF_BUS_CLOCK;

/********************************************************************/
// Library Functions
/********************************************************************/
void Clock_Set2MHZ(void)
{
  // PLLCLK = 2 x OSCCLK x ([SYNR + 1] / [REFDV + 1])
  // BUS  CLK =  PLLCLK / 2
  // Goal is to set Bus Clock to 20[MHz], therefore PLLCLK to 40[MHz]
  // We set SYNR = 4, REFDV = 3
  // CLKSEL_PLLSEL_MASK = 0b10000000;
  // Disable system clock as pll
  CLKSEL &= ~CLKSEL_PLLSEL_MASK;  
  // We want SYNR to have a value of 0 decimal.
  // SYNR = 0b00000000;
  // SYNR = 0x00;
  SYNR = 0;
  // We want REFDV to have a value of 7 decimal.
  // REFDV = 0b00000111;
  // REFDV = 0x07;
  REFDV = 7;
  // Our formula is PLLCLK = (2 * 16MHz * (4+1))/(3+1)
  // PLLCLK = (32MHz * 5)/4 = 40MHz.
  // BUSCLK = PLLCLK/2 = 40MHz/2 = 20MHz.
  // PLL ON and AUTO
  PLLCTL = PLLCTL_PLLON_MASK | PLLCTL_AUTO_MASK;
  // Wait for PLL to lock - ensures the clock source is stable first before setting the clock source
  while(!(CRGFLG & CRGFLG_LOCK_MASK));
  // System clock is derived from PLLCLK (Bus Clock = PLLCLK / 2).
  // Setting PLL as the clock source
  CLKSEL |= CLKSEL_PLLSEL_MASK;      
  ClockSpeed = 2000000;
}

void Clock_Set8MHZ(void)
{
  //8MHz is the default clock speed
  CLKSEL &= ~CLKSEL_PLLSEL_MASK;  //disable system clock as pll
  ClockSpeed = DEF_BUS_CLOCK;
}

void Clock_Set20MHZ(void)
{
  // PLLCLK = 2 x OSCCLK x ([SYNR + 1] / [REFDV + 1])
  // BUS  CLK =  PLLCLK / 2
  // Goal is to set Bus Clock to 20[MHz], therefore PLLCLK to 40[MHz]
  // We set SYNR = 4, REFDV = 3
  // CLKSEL_PLLSEL_MASK = 0b10000000;
  // Disable system clock as pll
  CLKSEL &= ~CLKSEL_PLLSEL_MASK;  
  // We want SYNR to have a value of 4 decimal.
  // SYNR = 0b00000100;
  // SYNR = 0x04;
  SYNR = SYNR_SYN2_MASK;
  // We want REFDV to have a value of 3 decimal.
  // REFDV = 0b00000011;
  // REFDV = 0x03;
  REFDV = REFDV_REFDV1_MASK | REFDV_REFDV0_MASK;
  // Our formula is PLLCLK = (2 * 16MHz * (4+1))/(3+1)
  // PLLCLK = (32MHz * 5)/4 = 40MHz.
  // BUSCLK = PLLCLK/2 = 40MHz/2 = 20MHz.
  // PLL ON and AUTO
  PLLCTL = PLLCTL_PLLON_MASK | PLLCTL_AUTO_MASK;
  // Wait for PLL to lock - ensures the clock source is stable first before setting the clock source
  while(!(CRGFLG & CRGFLG_LOCK_MASK));
  // System clock is derived from PLLCLK (Bus Clock = PLLCLK / 2).
  // Setting PLL as the clock source
  CLKSEL |= CLKSEL_PLLSEL_MASK;      
  ClockSpeed = 20000000;
}

void Clock_Set34MHZ(void)
{
  // PLLCLK = 2 x OSCCLK x ([SYNR + 1] / [REFDV + 1])
  // BUS  CLK =  PLLCLK / 2
  // Goal is to set Bus Clock to 20[MHz], therefore PLLCLK to 40[MHz]
  // We set SYNR = 4, REFDV = 3
  // CLKSEL_PLLSEL_MASK = 0b10000000;
  // Disable system clock as pll
  CLKSEL &= ~CLKSEL_PLLSEL_MASK;  
  // We want SYNR to have a value of 0 decimal.
  // SYNR = 0b00000000;
  // SYNR = 0x00;
  SYNR = 16;
  // We want REFDV to have a value of 7 decimal.
  // REFDV = 0b00000111;
  // REFDV = 0x07;
  REFDV = 7;
  // Our formula is PLLCLK = (2 * 16MHz * (4+1))/(3+1)
  // PLLCLK = (32MHz * 5)/4 = 40MHz.
  // BUSCLK = PLLCLK/2 = 40MHz/2 = 20MHz.
  // PLL ON and AUTO
  PLLCTL = PLLCTL_PLLON_MASK | PLLCTL_AUTO_MASK;
  // Wait for PLL to lock - ensures the clock source is stable first before setting the clock source
  while(!(CRGFLG & CRGFLG_LOCK_MASK));
  // System clock is derived from PLLCLK (Bus Clock = PLLCLK / 2).
  // Setting PLL as the clock source
  CLKSEL |= CLKSEL_PLLSEL_MASK;      
  ClockSpeed = 34000000;
}

void Clock_Set40MHZ(void)
{
  // PLLCLK = 2 x OSCCLK x ([SYNR + 1] / [REFDV + 1])
  // BUS  CLK =  PLLCLK / 2
  // Goal is to set Bus Clock to 20[MHz], therefore PLLCLK to 40[MHz]
  // We set SYNR = 4, REFDV = 3
  // CLKSEL_PLLSEL_MASK = 0b10000000;
  // Disable system clock as pll
  CLKSEL &= ~CLKSEL_PLLSEL_MASK;  
  // We want SYNR to have a value of 4 decimal.
  // SYNR = 0b00000100;
  // SYNR = 0x04;
  SYNR = 4;
  // We want REFDV to have a value of 1 decimal.
  // REFDV = 0b00000001;
  // REFDV = 0x01;
  REFDV = 1;
  // Our formula is PLLCLK = (2 * 16MHz * (4+1))/(3+1)
  // PLLCLK = (32MHz * 5)/4 = 40MHz.
  // BUSCLK = PLLCLK/2 = 40MHz/2 = 20MHz.
  // PLL ON and AUTO
  PLLCTL = PLLCTL_PLLON_MASK | PLLCTL_AUTO_MASK;
  // Wait for PLL to lock - ensures the clock source is stable first before setting the clock source - ensures the clock source is stable first before setting the clock source
  while(!(CRGFLG & CRGFLG_LOCK_MASK));
  // System clock is derived from PLLCLK (Bus Clock = PLLCLK / 2).
  // Setting PLL as the clock source
  CLKSEL |= CLKSEL_PLLSEL_MASK;      
  ClockSpeed = 40000000;
}
void Clock_EnableOutput()
{
   // Activate clock output (NECLK) so we can measure with
   // our Analog Discovery on Pin 39 (PE4) (22.3.2.9 pg830).
   // In order to activate the clock output we have to make
   // (NECLK) = 0, but the ECLKCTL_NECLK_MASK = 1, so we
   // have to negate it, then AND with ECLKCTL.
    ECLKCTL &= ~ECLKCTL_NECLK_MASK;
    // Divide the bus clock by 4 and present on Pin 39 (PE4)
    ECLKCTL |= ECLKCTL_EDIV1_MASK | ECLKCTL_EDIV0_MASK; 
}

unsigned long Clock_GetBusSpeed(void){
  return ClockSpeed;
}

/********************************************************************/
// Another group of library functions
/********************************************************************/


/********************************************************************/
// Another title placeholder
/********************************************************************/


/********************************************************************/
// Interrupt Service Routines Template
/********************************************************************/
