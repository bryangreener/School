#include <msp430.h>
#include <stdlib.h>
#include <libemb/serial/serial.h>
#include <libemb/conio/conio.h>
#include "dtc.h"

//void initialize_dtc(unsigned int channel, unsigned int *pointer);

typedef struct _note_t {
	int pitch;
	struct _note_t *next;
} note_t;

note_t *HEAD = NULL;
note_t *curr = NULL;

unsigned int *pointer = 0;

int main(void) {
	cio_printf("TEST1");
	WDTCTL  = WDTPW | WDTHOLD;
	BCSCTL1 = CALBC1_1MHZ;
	DCOCTL  = CALDCO_1MHZ;

	// MORE INITIALIZATION
	HEAD = malloc(sizeof(note_t));
	HEAD->pitch = 0;
	HEAD->next = NULL;
	curr = HEAD;

	initialize_dtc(INCH_4, &TA0CCR0);

	P1DIR = BIT4 | BIT6;
	P1REN = BIT3;
	P1OUT |= BIT3;
	P1IE |= BIT3;
	P1IES |= BIT3;
	P1IFG &= ~BIT3;

	__eint();
	for (;;) {
		TA0CTL = TASSEL_2 | MC_1 | ID_3;
		P1SEL = BIT6;
		TA0CCTL1 = OUTMOD_4;
	}
}

#pragma vector=TIMER1_A0_VECTOR
__interrupt void timer(void) {
	// USED FOR DOUBLE-PRESS
}

#pragma vector=PORT1_VECTOR
__interrupt void button(void) {
	// ADD A NEW NOTE TO THE LIST AND LINK IT
	curr->next = malloc(sizeof(note_t));
	curr->pitch = TA0CCR0;
	curr = curr->next;
	curr->next = NULL;
	curr->pitch = 0;
	//start time to last 1/4 sec when button pressed once.
	// if made to endof timer with single button press, add node
	// else call method
}

