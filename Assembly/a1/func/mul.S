	.file "mul.S"
	.text
.global mul
mul:
	;clearing registers for later use
	clr r10
	clr r11
	clr r12
	;move inputs over to free up r15 for output accumulation
	mov r14,r13
	mov r15,r14

	cmp #0,r13		;check if r13 is negative
	jge b 			;if positive, jump
	add #1,r11		;counter used to check signs later on
	mov r13,r10		;store r13 in r10 for convert function
	call #convert	;calls convert function at end of code
	mov r10,r13		;move back to r13 and clear r10 for future use
	clr r10
b:
	cmp #0,r14		;same process as above repeated for r14 input
	jge c
	add #1,r11
	mov r14,r10
	call #convert
	mov r10,r14
	clr r10
c:
	clr r15			;clear r15 to prevent weird issues where mov didnt clear it
	call #multiply	;call multiply function
	cmp #1,r11		;if only one input is negative, jump to d to convert output to negative
	jeq d
	ret 			;otherwise, return r15 output
d:
	mov r15,r10		;convert r15 from positive to negative then output
	call #convert
	mov r10,r15
	ret

multiply:
	cmp #0,r13		;compare r13 to 0
	jeq break		;if r13=0, call break which returns to call #multiply
	add r14,r15		;otherwise, accumulate r15 with contents of r14
	dec r13			;decrement r13 (this controls the loop)
	jmp multiply	;repeat
break:
	ret
	
convert:			;this just uses 2s comp to convert from negative to positive
	xor #0xFFFF,r10	;invert all bits
	add #1,r10		;add 1
	ret

zero:
	clr r15			;if either input is 0, set r15 to 0
	ret

