#Name: Bryan Greener
#Date:2018-02-14
#Homework:3

# FUNCTION BLOCK
def check_hex(user_input): #Validate hex input
    try:
        user_input = int(user_input, 16)
        user_input = hex(user_input)
        return True
    except ValueError:
        return False
def hex_operation(input_list, operation): #Operate on hex (passed as strings)
    output_list = []
    output_list.append(hex(input_list[0])[2:])
    result = input_list[0]
    print("  %08x".upper() % result)
    for i in range(1,len(input_list)):
        output_list.append(hex(input_list[i])[2:])
        if(operation == 1):
            print("| %08x".upper() % input_list[i])
            result |= input_list[i]
        elif(operation == 2):
            print("& %08x".upper() % input_list[i])
            result &= input_list[i]
        else:
            print("^ %08x".upper() % input_list[i])
            result ^= input_list[i]
    print("= %08x".upper() % result)
    return output_list
def hex_binary(input_list, operation):
    result = int(input_list[0], 16)
    print(" ", '  '.join([str('{0:032b}'.format(result))[i:i+8] for i in range(0, len(str('{0:032b}'.format(result))), 8)]))
    for i in range(1,len(input_list)):
        if(operation == 1):
            print("|", '  '.join([str('{0:032b}'.format(int(input_list[i], 16)))[j:j+8] for j in range(0, len(str('{0:032b}'.format(int(input_list[i], 16)))), 8)]))
            result |= int(input_list[i], 16)
        elif(operation == 2):
            print("&", '  '.join([str('{0:032b}'.format(int(input_list[i], 16)))[j:j+8] for j in range(0, len(str('{0:032b}'.format(int(input_list[i], 16)))), 8)]))
            result &= int(input_list[i], 16)
        else:
            print("^", '  '.join([str('{0:032b}'.format(int(input_list[i], 16)))[j:j+8] for j in range(0, len(str('{0:032b}'.format(int(input_list[i], 16)))), 8)]))
            result ^= int(input_list[i], 16)
    print("=", '  '.join([str('{0:032b}'.format(result))[i:i+8] for i in range(0, len(str('{0:032b}'.format(result))), 8)]))

# MAIN
while(True): # keeps program looping
    operation = 0
    num_integers = 0
    while(operation == 0):
        user_input = input("\nEnter Operation: ")
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

    # GET NUM INTEGERS
    while(num_integers < 2):
        user_input = input("Enter number of integers: ")
        try:
            num_integers = int(user_input)
            if(num_integers < 2):
                num_integers = 0;
                print("Please enter a number greater than 1")
        except ValueError:
            print("Please enter a number greater than 1")


    int_list = [] #list of strings containing hex values
    hex_list = []
    valid_hex = False
    for i in range(1,num_integers+1): #prompt for multiple hex inputs
        user_input = input("Enter integer %d: " % i)
        valid_hex = check_hex(user_input)
        while(valid_hex == False):
            print("Please enter an 8-digit hexadecimal integer")
            user_input = input("Enter integer %d: " % i)
            valid_hex = check_hex(user_input) #bool to break while()
        hexval = int(user_input, 16)
        int_list.append(hexval) #valid hex (as string) saved to list

    print("\nHexadecimal operation:")
    hex_list = hex_operation(int_list, operation) #perform hex operations on hex ints
    print("\nBinary operation:")
    hex_binary(hex_list, operation) #perform bin operation on hex strings
