# Name: Bryan Greener
# Date: 2018-04-17
# Homework: 6 part 2

def MemoizeReset(f):
    mem = {}
    def MR(*args, **kwargs):
        MR.calls += 1
        key = str(args) + str(kwargs)
        if key not in mem:
            mem[key] = f(*args, **kwargs)
        return mem[key]
    MR.calls = 0
    MR.__name__ = f.__name__
    return MR

@MemoizeReset
def lev(a, b):
    if a == "":
        return len(b)
    if b == "":
        return len(a)
    if a[-1] == b[-1]:
        cost = 0
    else:
        cost = 1
    res = min([lev(a[:-1], b)+1,
               lev(a, b[:-1])+1,
               lev(a[:-1], b[:-1]) + cost])
    return res

import csv
with open('wordfile.txt', 'r') as file:
    reader = csv.reader(file, delimiter=',')
    for row in reader:
        print("%s,%s,%d,%s" % 
              (row[0].strip(), row[1].strip(), 
               lev(row[0].strip(),row[1].strip()),str(lev.calls)))