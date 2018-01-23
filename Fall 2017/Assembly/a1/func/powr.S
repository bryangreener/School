	.file "powr.S"
	.text
.global powr
powr:				;most of this is copied from mult.S
	clr r8			;only changes will be documented
	clr r9
	clr r10
	clr r11
	clr r12
	mov r14,r13
	mov r15,r14

	cmp #0,r13
	jge b
	add #1,r11
	mov r13,r10
	call #convert
	mov r10,r13
	clr r10
b:
	cmp #0,r14
	jge c
	add #1,r11
	mov r14,r10
	call #convert
	mov r10,r14
	clr r10
c:
	clr r15
	add r13,r8		;add r13 to r8 and r9 as temp value storage
	add r13,r9
	cmp #0,r14		;if r14 is 0, go to zero function
	jeq zero
	call #multLoop	;call outer multiply loop
	cmp #1,r11
	jeq d
	ret
d:
	mov r15,r10
	call #convert
	mov r10,r15
	ret

multLoop:
	dec r14			;loop counter. decrement r14
	cmp #0,r14		;if r14=0, end
	jeq end
	call #multiply	;while r14!=0, call multiply
end:
	mov r9,r15		;move r9 to r15 for output
	ret

multiply:
	cmp #0,r8		;if r8=0, break loop
	jeq break
	dec r8			;loop counter. decrement r8
	add r9,r15		;add current r9 to r15
	jmp multiply	;return to loop
break:
	clr r8			;clear temp registers before returning
	clr r9
	add r13,r8		;add input a to r8
	mov r15,r9		;move accumuted result to r9
	clr r15			;clear r15 in case mov didnt do it
	jmp multLoop	;back to multloop
	
convert:
	xor #0xFFFF,r10
	add #1,r10
	ret

zero:
	clr r15
	add #1,r15
	ret
