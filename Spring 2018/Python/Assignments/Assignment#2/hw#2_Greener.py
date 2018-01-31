# Name: Bryan Greener
# Date: 2018-01-29
# Homework: 2

# Open file, read contents into variable, then close file.
f = open("charactermask.txt", encoding="utf8")
contents = list(f.read())
f.close()
# File read error
if contents == "":
    print("FILE READ ERROR. CONTENTS EMPTY")
# String used as pseudo regex
ascii = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
# Replace all non alphabet chars with space
for x in range(0,len(contents)):
    if contents[x] not in ascii:
        contents[x] = ' ';
    contents[x] = contents[x].lower()
contents = "".join(contents)
# Split at spaces
word = contents.split()
# Initialize a dictionary to store words
wd = {}
# For each word
for x in range(0,len(word)):
    # If word exists increment, otherwise add to dict
    if len(word[x]) > 1:
        if word[x] in wd.keys():
            wd[word[x]] += 1
        else:
            wd[word[x]] = 1
# Lists used to store most and least used words. Unique counter
most = [""]*10
least = [""]*10
unique = 0
# For each item in dict
for key, value in wd.items():
    # from index 0 to 9
    for i in range(0,10):
        # If no item in index, add key
        if most[i] == "":
            most[i] = key
            break
        # Else if current key is larger than key at index in list
        elif value > wd[most[i]]:
            for y in range(9, i-1, -1):
                most[y] = most[y-1]
            most[i] = key
            break
    for i in range(0,10):
        # Repeat for least[] list
        if value > 1:
            if least[i] == "":
                least[i] = key
                break
            elif value <= wd[least[i]]:
                for y in range(9, i-1, -1):
                    least[y] = least[y-1]
                least[i] = key
                break
    if value == 1:
        unique += 1
# Print out results
print("-----------------------------------")
print("# Most Frequent Words:")
print("WORD".ljust(20), "COUNT")
print("___________________________________")
for x in range(0, 10):
    print(most[x].ljust(20), wd.get(most[x]))
print("-----------------------------------")
print("# Least Frequent Words:")
print("WORD".ljust(20), "COUNT")
print("___________________________________")
for x in range(0, 10):
    print(least[x].ljust(20), wd.get(least[x]))
print("-----------------------------------")
print("Number of Unique Words:\n%s" % unique)
