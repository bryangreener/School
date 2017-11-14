/*
	Made By: Bryan Greener
	Class: CS2230
	Assignment: a0
	---
	IMPORTANT:
	Must run using the gcc -lm switch for math functions to work
	---
	Please let me know if I am not following any specific conventions
	for C that can cause me trouble in the future.

	I'd like to not form bad habits right off the bat.
	
	Thank you.
*/

// Imports needed for libraries used
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

// Defining limits for long
#define LONG_MAX 0x7FFFFFFFL

// Declaring functions used later on.
// isdigit checks if a char is a digit or not
// Manually created bool enum
// log2 is log base 2 of x
int isdigit(int c);
typedef enum {false,true} bool;
double log2(double x);

// Initializing methods ( c calls them functions )
char convert(int input);
void flip(char* input, long vOut, int b);
bool validate(char vIn[100], char bIn[100]);
void ui(int input);
void shift(long v, int b);
void mod(long v, int b);

// Main function
// Handles user input and calculation of new base
void main(void)
{
	// Create char arrays and other variables
	// vIn is the decimal value in,
	// bIn is the base in,
	// v is value, b is base
	// valid is used to validate input
	char vIn[100] = "", bIn[100] = "";
	long v;
	int b;
	bool valid = false;
	// Loop used to validate input
	do 
	{
		// Ask for user input and save in vIn and bIn
		// ui function just to make things clean in this method
		ui(1);
		fgets(vIn,100,stdin);
		ui(2);
		fgets(bIn,100,stdin);

		// Call validate function, passing in value and base char arrays
		valid = validate(vIn, bIn);

	}	while(valid == false); // Only break when input is valid

	// Line break
	ui(3);

	// Convert value and base to long and int 
	v = atol(vIn);
	b = atoi(bIn);

	// Call mod and divide function
	mod(v, b);

	// If the base is a power of 2...
	if(b != 0 && ((b & (b - 1)) == 0))
	{
		// Call mask and shift function
		shift(v, b);
	}
}

// Function that converts from decimal to any base using a mod and divide algorithm
void mod(long v, int b)
{
	// Char array (result) filled with empty values
	char res[32] = "";
	// Int i used for indexing
	int i = 0;
	// vOut saving value of v for future use
	long vOut = v;

	// Special case for when 0 is entered
	if (v == 0)
	{
		res[i] = convert(v);
	}

	// Loop used to perform base conversion formula
	while (v > 0)
	{
		// Call convert function and store in result at index i
		// The following two lines are the algorithm used to convert from dec to base n
		res[i++] = convert(v % b);
		v /= b;
	}

	ui(5);
	// Call flip function which reverses the string and outputs result
	flip(res, vOut, b);
}

// Function that converts from decimal to bases that are powers of 2
// using bit masking and shifting
void shift(long v, int b)
{
	char res[32] = "";
	int i = 0;
	long vOut = v;

	if (v == 0)
	{
		res[i] = convert(v);
	}

	while (v > 0)
	{
		// Bit mask and shift algorithm.
		res[i++] = convert(v & (b -1));
		v = v >> (long)log2(b);
	}

	ui(6);

	flip(res, vOut, b);
}

// Converts decimal value to corresponding character.
char convert(int input)
{
	// String used as key to decimal-to-char conversion
	const char *ascii = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";
	// Save output as the character in ascii at index of input
	return ascii[input];
}

// Function used to reverse a string
void flip(char* input, long vOut, int b)
{
	// Create variables and char array
	char temp[32] = "";
	// l saves the length of the actual data in the input array
	int l = strnlen(input, 32);
	// j copies l to be used later without breaking the for loop
	int j = l;

	// Loop while index is less than the length of the input array
	for (int i = 0; i < l; i ++)
	{
		// Working from the end of the temp array and the beginning of input array,
		// assign value from input to temp.
		temp[--j] = input[i];
	}

	// Send temp array back to main
	printf("%ld IN BASE %i = %s\n", vOut, b, temp);
}

// Function used to validate input
bool validate(char vIn[100], char bIn[100])
{
	// Create and initialize variables
	long v;
	int b;
	bool valid = false;

	// Setting lengths for value and base counters
	int vl = strnlen(vIn, 100) - 1;
	int bl = strnlen(bIn, 100) - 1;

	// Loop through each item in the valueIn array
	for (int i = 0; i < vl; i++)
	{		
		// isdigit returns 0 only if the specified character is NOT a digit
		if(isdigit(vIn[i]) == 0)
		{
			// Print invalid message
			ui(4);

			// Return invalid for main class
			return valid = false;
			break;
		}
	}

	// Loop through each item in baseIn array
	for (int j = 0; j < bl; j++)
	{
		// see previous for loop notes
		if(isdigit(bIn[j]) == 0)
		{
			ui(4);
			return valid = false;
			break;
		}
	}

	// convert vIn and bIn to long and int
	v = atol(vIn);
	b = atoi(bIn);
	
	// Verify that inputs are within accepted values
	if((v <= LONG_MAX) && (v >= 0) && (b > 1) && (b < 65))
	{
		// Send back to main to break input validation while loop
		return valid = true;
	}
}

// UI function used to make code look better in other parts of program
// and makes it easier to print output.
void ui(int input)
{
	switch(input)
	{
		case 1:
			printf("Enter Decimal Value:\n");
			break;
		case 2:
			printf("Enter New Base:\n");
			break;
		case 3:
			printf("=============================\n");
			break;
		case 4:
			printf("\n\n     !! INVALID INPUT !!\n");
			printf(" ONLY CHARACTERS 0-9 ALLOWED\n");
			printf("Min decimal input:  0\n");
			printf("Max decimal input: %ld\n", LONG_MAX);
			printf("Min base input:     1\n");
			printf("Max base input:    64\n");
			printf("=============================\n\n");
			break;
		case 5:
			printf("Using MOD and DIVIDE\n");
			break;
		case 6:
			printf("Using MASK and SHIFT\n");
			break;
	}
		
}
	