	.file "mod.S"
	.text
.global mod
mod:				;the majority of this code is copied from divd.S
	clr r10			;only changes will be documented
	clr r11
	clr r13
	mov r14,r13
	mov r15,r14
	clr r15	

	cmp #0,r14
	jeq end

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
	call #divide
	cmp #1,r11
	jeq d
	ret
d:
	mov r15,r10
	call #convert
	mov r10,r15
	ret
	
divide:
	clr r12
comp:
	cmp r13,r12
	jl sub			;if r12(0) < r13, subtract loop
	jeq end			;if r13 = r12(0), end
	jge remainder	;if r13 < r12(0), remainder loop

sub:
	sub r14,r13		;subtract r14 from r13
	jmp comp

remainder:
	mov r13,r10		;convert r13 sign
	call #convert
	mov r10,r13
	sub r13,r14		;subtract r13 from r14
	mov r14,r15
	ret				;return result

convert:
	xor #0xFFFF,r10
	add #1,r10
	ret

end:
	ret
