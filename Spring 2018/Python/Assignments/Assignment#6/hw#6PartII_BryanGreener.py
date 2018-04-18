# Name: Bryan Greener
# Date: 2018-04-17
# Homework: 6 part 2

import sys
import csv

def MemoizeReset(f):
    cache = {} # Dict acting as cache
    def MR(*args, **kwargs): # Main helper func. Accumulates count and caches
        val = str(args) + str(kwargs)
        if val not in cache:
            cache[val] = f(*args, **kwargs)
            MR.count += 1
        return cache[val]
    MR.__name__ = f.__name__
    MR.count = 0
    return MR

@MemoizeReset # Decorator for func above
def lev(a, b):
    if a == "":
        return len(b)
    if b == "":
        return len(a)
    if a[-1] == b[-1]:
        cost = 0
    else:
        cost = 1
    output = min([lev(a[:-1], b)+1,
                  lev(a, b[:-1])+1,
                  lev(a[:-1], b[:-1]) + cost])
    return output

# If no input arg specified
if len(sys.argv) < 2:
    print("No file specified. Exiting")
    exit()
else: #input arg specified. save to filename
    filename = sys.argv[1]
try:
    with open(filename, 'r') as file:
        reader = csv.reader(file, delimiter=',')
        for row in reader:
            count = lev.count
            lev.count = 0 #reset counter in decorator
            print("%s,%s,%d,%s" % (row[0].strip(), 
                                   row[1].strip(), 
                                   lev(row[0].strip(),row[1].strip()),
                                   str(count)))
except OSError as err: # Invalid input file name
    print("OS Error: {0}".format(err))
    exit()
except (ValueError,IndexError) as err: #Invalid input file content
    print("Input Error: {0}".format(err))
    exit()