def check_hex(user_input):
    try:
        user_input = int(user_input, 16)
        user_input = hex(user_input)
        return True
    except ValueError:
        return False
def hex_operation(input_list, operation):
    result = input_list[0]
    for i in range(1,len(input_list)):
        if(operation == 1):
            result |= input_list[i]
        elif(operation == 2):
            result &= input_list[i]
        else:
            result ^= input_list[i]
    return result
def hex_binary(input_list, operation):
    return True
operation = 0
num_integers = 0
while(operation == 0):
    user_input = input("Enter Operation:")
    if(user_input == '|'):
        operation = 1
    elif(user_input == '&'):
        operation = 2
    elif(user_input =='^'):
        operation = 3
    elif(user_input == 'q'):
        exit(0)
    else:
        print("Please enter |, &, ^, or q")

while(num_integers < 2):
    user_input = input("Enter number of integers:")
    try:
        num_integers = int(user_input)
        if(num_integers < 2):
            num_integers = 0;
            print("Please enter a number greater than 1")
    except ValueError:
        print("Please enter a number greater than 1")

input_list = []
valid_hex = False
for i in range(0,num_integers):
    user_input = input("Enter integer %d:" % i)
    valid_hex = check_hex(user_input)
    while(valid_hex == False):
        print("Please enter an 8-digit hexadecimal integer")
        user_input = input("Enter integer %d:" % i)
        valid_hex = check_hex(user_input)
    hex_in = int(user_input, 16)
    input_list.append(hex_in)
    print(i, input_list[i])
result = hex_operation(input_list, operation)
# Hex formatted output
print("%08x".upper() % result)
result = hex_operation(input_list, operation)
print("{0:032b}".format(result))
