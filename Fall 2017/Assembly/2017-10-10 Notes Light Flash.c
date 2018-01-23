#include <msp430.h>
int main(void)
{
	WDTCTL = WDTPW | WDTHOLD;
	BCSCTL1 = CALBC1_1MHZ;
	DCOCTL = CALDCO_1MHZ;

	P1DIR = BIT0;	//for pull up resistor use p1 for pull down use p2
	P1REN = BIT3;
	P1OUT = BIT3;
	P1IE = BIT3;	
	P1IES = BIT3;	//edge select (high to low)
	P1IFG &= ~BIT3;	//clears bit3 in register
	
	_BIS_SR(LPM3_bits | GIE);	//microcontroller goes to sleep and listens for interrupts

	return 0;
}

#pragma vectoe=PORT1_VECTOR
__interupt void button(void)
{
	P1OUT ^= BIT0;

	while(!(P1IN & BIT3));	//polling p1 vs bit 3
	__delay_cycles(32000);	//helps prevent flashing of input
	P1IFG &= ~BIT3;			//clear interrupt flag
}

/*
locate msp430g2553.h
open with vim
search for interrupt vectors
*/