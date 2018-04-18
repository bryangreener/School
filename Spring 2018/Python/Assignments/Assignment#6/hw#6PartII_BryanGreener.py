# Name: Bryan Greener
# Date: 2018-04-17
# Homework: 6 part 2

import sys

def MemoizeReset(f):
    cache = {} # Dict acting as cache
    def MR(*args, **kwargs): # Main helper func. Accumulates count and caches
        val = str(args) + str(kwargs)
        if val not in cache:
            MemoizeReset.count += 1
            cache[val] = f(*args, **kwargs)
        return cache[val]
    MR.count = 0
    MR.__name__ = f.__name__
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

import csv
if sys.argv[1] == "-f" and len(sys.argv) > 2:
    filename = sys.argv[2]
else:
    print("No file specified. Exiting")
    exit()
try:
    with open(filename, 'r') as file:
        reader = csv.reader(file, delimiter=',')
        for row in reader:
            print("%s,%s,%d,%s" % (row[0].strip(), 
                                   row[1].strip(), 
                                   lev(row[0].strip(),row[1].strip()),
                                   str(lev.count)))
except OSError as err:
    print("OS Error: {0}".format(err))
    exit()
except NameError as err:
    print("Type Error: {0}".format(err))
    exit()