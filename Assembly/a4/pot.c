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

note_t *head = NULL;
note_t *current = NULL;

unsigned int *pointer = 0;

int main(void) {
	cio_printf("TEST1");
	WDTCTL  = WDTPW | WDTHOLD;
	BCSCTL1 = CALBC1_1MHZ;
	DCOCTL  = CALDCO_1MHZ;

	// MORE INITIALIZATION
	pointer = malloc(sizeof(unsigned int));
	//TA0CTL = TASSEL_2 | MC_1 | ID_3;


	head = (note_t*)malloc(sizeof(note_t));
	(*head).pitch = 0;
	(*head).next = current;

	P1DIR = BIT4 | BIT6;
	P1REN = BIT3;
	P1OUT |= BIT3;
	P1IE |= BIT3;
	P1IES |= BIT3;
	P1IFG &= ~BIT3;


	__bis_SR_register(LPM0_bits | GIE);
	__eint();
	for (;;) {
		cio_printf("TEST\n\r");
		TA0CTL = TASSEL_2 | MC_1 | ID_3;
		initialize_dtc(4, pointer);
		TA0CCR0 = 10000000;
		//TA0CCR0 = 1000000/(*pointer)/2;
		P1SEL = BIT6;
		TA0CCTL1 = OUTMOD_4;
	}
}

void initialize_dtc(unsigned int channel, unsigned int *pointer) {
  ADC10CTL0 &= ~ENC;                        // Disable ADC before configuration.
  ADC10CTL0 = ADC10ON;                      // Turn ADC on in single line before configuration.
  while(ADC10CTL1 & ADC10BUSY);             // Make sure the ADC is not running per 22.2.7
  ADC10DTC0 = ADC10CT;                      // Repeat conversion.
  ADC10DTC1 = 1;                            // Only one conversion at a time.
  ADC10SA = (unsigned int) pointer;         // Put results at specified place in memory.
  ADC10CTL0 |= ADC10SHT_3 | SREF_0 | REFON | MSC; // 64 clock ticks, Use Reference, Reference on
                                            // ADC On, Multi-Sample Conversion, Interrupts enabled.
  ADC10CTL1 = channel | ADC10SSEL_3 | ADC10DIV_7 | CONSEQ_2; // Set channel, Use SMCLK,
                                            // 1/8 Divider, Repeat single channel.
  ADC10AE0  = 1 << (channel >> 12);         // Analog Enable P1.<channel>
  ADC10CTL0 |= ENC;                         // Enable conversion.
  ADC10CTL0 |= ADC10SC;                     // Start conversion
}

#pragma vector=TIMER1_A0_VECTOR
__interrupt void timer(void) {
	// USED FOR DOUBLE-PRESS
}

#pragma vector=PORT1_VECTOR
__interrupt void button(void) {
	// ADD A NEW NOTE TO THE LIST AND LINK IT
	initialize_dtc(4, pointer);
	note_t *newNode = malloc(sizeof(note_t));
	(*newNode).pitch = pointer;
	(*newNode).next = NULL;
	(*current).next = newNode;
	current = (*current).next;
	
}

