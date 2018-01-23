	.file "divd.S"
	.text
.global divd
divd:
	;clear registers and move inputs over for r15 to be used as output
	clr r10
	clr r11
	clr r13
	mov r14,r13
	mov r15,r14
	clr r15			;register reserved for results

	cmp #0,r14		;if trying to divide by zero, return 0
	jeq end

	cmp #0,r13		;same process as multiply function. see those notes
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
	call #divide	;when reached, call divide function
	cmp #1,r11		;check if only one input was negative
	jeq d 			;if so, jump to d otherwise return r15 as is
	ret
d:
	mov r15,r10		;convert r15 from positive to negative
	call #convert
	mov r10,r15
	ret
	
divide:
	clr r12			;clr r12 to prevent weird memory issues
comp:
	cmp r13,r12		;compare r13 to r12 which is zeroed out
	jl sub			;if 13 > 0, jump to sub loop
	ret 			;if 13 <= 0, return to call
sub:
	sub r14,r13		;subtract r14 from r13
	cmp r12,r13		;compare r12 (0) to r13
	jl comp 		;if 13 < 0 jump to comp.
	inc r15			;increment result
	jmp comp  		;jump back to comp
	
end:
	ret

convert:			;2s comp convert function
	xor #0xFFFF,r10
	add #1,r10
	ret
