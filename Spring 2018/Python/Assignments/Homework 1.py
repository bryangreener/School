# Name: Bryan Greener
# Date: 2018-01-21
# Homework: 1

# PART 1
string1 = input("Enter string 1:")
string2 = input("Enter string 2:")
if string1 in string2:
    print("True")
else: 
    print("False")

# PART 2
pal = input("Enter string:")
revPal = pal[::-1]
if pal == revPal:
    print("True")
else:
    print("False")

# PART 3
num1 = int(input("Enter number 1:"))
num2 = int(input("Enter number 2:"))
print(num1 + num2)

# PART 4
bnum = abs(int(input("Enter number:")))
print("{0:b}".format(bnum))

# PART 5
string1 = input("Enter string 1:")
string2 = input("Enter string 2:")
print(string1 in string2)

