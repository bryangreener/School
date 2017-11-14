#include <msp430.h>

// Written by Bryan Greener
// ...for the second time...


int mode = 1;						// Global int used to switch between modes.
int main(void)
{
    WDTCTL  = WDTPW | WDTHOLD;
    BCSCTL1 = CALBC1_1MHZ;
    DCOCTL  = CALDCO_1MHZ;

    P1DIR = BIT0;					// Initially load program only using P1.0 as output

    P1REN = BIT3;					// P1.3 button interrupt setup
    P1OUT |= BIT3;
    P1IE |= BIT3;
    P1IES |= BIT3;
    P1IFG &= ~BIT3;

    __bis_SR_register(GIE);
    for (;;) {
        P1OUT ^= BIT0;				// Flip P1.0 between on and off every 0.5s
        __delay_cycles(500000);
    }

    return 0;
}

#pragma vector=TIMER1_A0_VECTOR
__interrupt void blink_green (void)
{
    P1OUT ^= BIT1;					// Flip P1.1 between on and off when interrupt called.
}

#pragma vector=PORT1_VECTOR
__interrupt void button (void)
{
    switch(mode)
    {
        case 1:	// Going from Red only flash to Red/Green flash
			P1DIR = BIT0 | BIT1;				// Set new pins for output
			TA1CTL   = TASSEL_2 | MC_1 | ID_3;	// Enable TA1 timer
    		TA1CCTL0 = CCIE;					// Set TA1 to interrupt enable mode
    		TA1CCR0  = 41667;					// Green flash every 0.33s
            mode++;								// increment current mode
            break;
        case 2:
            P1DIR = BIT1 | BIT6;				// New outputs

			P1SEL = BIT6;						// P1.6 used in TA0 for toggling
    		TA0CTL   = TASSEL_2 | MC_1 | ID_3;	// Enable TA0 timer
    		TA0CCR0  = 31250;					// Blue flash every 0.25s
    		TA0CCTL1 = OUTMOD_4;				// Toggle mode
            mode++;
            break;
        case 3:
			P1DIR = BIT6;						// Only P1.6 allowed now.

			TA1CTL = TASSEL_2 | MC_0 | ID_3;	// Disable TA1 timer
			TA1CCTL0 &= ~BIT4;					// Disable TA1 timer just in case...
			TA0CCR0 = 62500;					// Blue flash every 0.5s
            mode++;
            break;
        case 4:
			P1DIR = BIT0;						// Only P1.0 now

			TA0CTL = TASSEL_2 | MC_0 | ID_3;	// Disable TA0 timer
			TA0CCTL1 &= ~(BIT7 | BIT6 | BIT5);	// Disable OUTMOD for TA0 just in case...
            mode = 1;							// Set mode back to red only flash (mode 1)
            break;
        default:
            mode = 1;	// Shouldn't ever reach this line.
            break;
    }

    while (!(BIT3 & P1IN));
    __delay_cycles(32000);
    P1IFG &= ~BIT3;
}