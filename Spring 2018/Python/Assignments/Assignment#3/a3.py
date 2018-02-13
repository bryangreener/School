def check_hex(user_input):
    try:
        user_input = hex(user_input)
        return True
    except ValueError:
        return False
def hex_operation():
    return True
def hex_binary():
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

input_dict = {}
valid_hex = False
for i in range(1,num_integers+1):
    user_input = input("Enter integer %d:" % i)
    valid_hex = check_hex(user_input)
    while(valid_hex == False):
        print("Please enter an 8-digit hexadecimal integer")
        user_input = input("Enter integer %d:" % i)
        valid_hex = check_hex(user_input)
        
    input_dict = {i: user_input}
    print(input_dict[i])
