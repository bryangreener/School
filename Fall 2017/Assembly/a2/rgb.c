#include <msp430.h>
#include <libemb/serial/serial.h>
#include <libemb/conio/conio.h>

// useful global variables or
// data structures?
// ADDED value to reference index in colors in interrupt
int value;
// ADDED colors array of char arrays for use in output
char colors[8][7] = {"OFF","RED","GREEN","YELLOW","BLUE","PURPLE","CYAN","WHITE"};

int main(void)
{
	// stop watchdog timer
	WDTCTL  = WDTPW | WDTHOLD;
	// run at 1Mhz
	BCSCTL1 = CALBC1_1MHZ;
	DCOCTL  = CALDCO_1MHZ;

	// all P2 pins as outputs
	P2DIR = -1;
	// all P2 pins off
	P2OUT = 0;
	// is more set up needed here?
	// ADDED the following lines to enable interrupts when pressing button P1.3
	P1REN = BIT3;
	P1OUT |= BIT3;
	P1IE |= BIT3;
	P1IES |= BIT3;
	P1IFG &= ~BIT3;

	// communicate at 9600 baud
	serial_init(9600);

	// general interrupt enable
	__bis_SR_register(GIE);

	for(;;) {
		// cycle through the colors in here
		// ADDED This loop will count from 0 to 7 and output the current
		// index to P2 which enables the needed pins for each color.
		for(int i = 0; i < 8; i++)
		{
			P2OUT = i;
			value = i;	// Value used in interrupt to point to colors index
			__delay_cycles(1000000);	// Delay for 1 second
		}
		P2OUT ^= 1;
	}

	// never do this
	return 0;
}

#pragma vector=PORT1_VECTOR
__interrupt void print_color(void)
{
	// print out the color in here
	cio_printf("%s\n\r",colors[value]);	// ADDED output the word in colors array at index of value
	// button debounce
	while (!(BIT3 & P1IN));
	__delay_cycles(32000);
	// clear interrupt flag for P1.3
	P1IFG &= ~BIT3;
}

