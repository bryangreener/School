#Name: Bryan Greener
#Date: 2018-04-02
#Homework: 5 Part 2

import random
import time
# Incremental Bubble Sort
def Bubble(inputList):
    for i in range(len(inputList)):
        for j in range(0, len(inputList) - i - 1):
            if inputList[j] > inputList[j+1]:
                inputList[j], inputList[j+1] = inputList[j+1], inputList[j]
    return inputList
# Incremental Insertion Sort
def Insertion(inputList):
    for i in range(1, len(inputList)):
        k = inputList[i]
        j = i-1
        while j >= 0 and k < inputList[j]:
            inputList[j+1] = inputList[j]
            j = j-1
        inputList[j+1] = k
    return inputList
# Recursive Quicksort (Limited to 1000 items max)
def QuickSort(inputList, low, high):
    if low < high:
        pivot = Partition(inputList, low, high)
        QuickSort(inputList, low, pivot - 1)
        QuickSort(inputList, pivot + 1, high)
    return inputList
# Helper method called by quicksort for partitioning input array
def Partition(inputList, low, high):
    pivot = inputList[high]
    tempIndex = low - 1
    for i in range(low, high):
        if inputList[i] <= pivot:
            tempIndex = tempIndex + 1
            inputList[tempIndex], inputList[i] = inputList[i], inputList[tempIndex]
    inputList[tempIndex + 1], inputList[high] = inputList[high], inputList[tempIndex + 1]
    return(tempIndex+1)
        
# Number of items for each iteration of sort
iterations = [50, 100, 500, 1000, 5000]

values = [] # Will store randomly generated values
tempnums = [] # Temp stores random nums. Prevents duplicate problems
for i in iterations:
    for j in range(0,i):
        tempnums.append(random.randint(1,i))
    values.append(tempnums[:])
    del tempnums[:]
    
# Arrays storing runtime results for each sort iteration
bTime = []
iTime = []
qTime = []

for i in values:
    start = time.time()
    r1 = Bubble(i)
    stop = time.time()
    bTime.append((stop - start) * 1000)

for i in values:
    start = time.time()
    r2 = Insertion(i)
    stop = time.time()
    iTime.append((stop - start) * 1000)

for i in values[:4]:
    start = time.time()
    r3 = QuickSort(i, 0, len(i) - 1) 
    stop = time.time()
    qTime.append((stop - start) * 1000)

print("BubbleSort Runtimes: ", bTime)
print("InsertSort Runtimes: ", iTime)
print("Quick Sort Runtimes: ", qTime)