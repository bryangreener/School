import csv
import numpy as np
l = []
newList = []
with open('testfile.txt', 'r') as f:
    reader = csv.reader(f)
    l = list(reader)
for i in l:
    newList.append(i)
newList = newList[::2]
nl = []
for i in newList:
   nl.append(i.pop(0))
nnl = []
for i in nl:
    listt = [x.strip() for x in i[1:len(i)-1].split(',')]
    nnl.append(listt)
for i in nnl:
    del i[0:len(i)-900]
'''
Now at list of lists with 900 elements each
'''
test = []
for i in nnl:
    tempList = []
    ttl = []
    j = [i[x:x+100] for x in range(0, len(i), 100)]
    for k in j:
        l = [k[x:x+10] for x in range(0, len(k), 10)]
        ttl.append(l)
        
    tempList.append(ttl)
tempList = np.array(tempList)
'''
Now at a list of 9 (eventually 15) lists with 10 lists with 10 elements each
Outermost is entire collection
Next in are lists with different number and value of layers
Next in are lists of different learning rates
Innermost are lists of different batch sizes

LAYER1
    LEARN1
        BATCH1
        BATCH2
        ...
        BATCH10
    LEARN2
        ...
    ...
    LEARN10
LAYER2
...
LAYER10
'''
t= 5