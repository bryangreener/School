import random

def Bubble(inputList):
    for index,value in enumerate(inputList):
        try:
            if inputList[index+1] < value:
                inputList[index] = inputList[index+1]
                inputList[index+1] = value
                Bubble(inputList)
        except IndexError:
            pass
    return inputList

def Insertion(inputList, index):
    if index <= 1:
        return
    Insertion(inputList, index-1)
    last = inputList[index-1]
    i = index - 2
    while (i >= 0 and inputList[i] > last):
        inputList[i+1] = inputList[i]
        i = i - 1
    inputList[i+1] = last
    
def QuickSort(inputList, low, high):
    if low < high:
        pi = Partition(inputList, low, high)
        QuickSort(inputList, low, pi - 1)
        QuickSort(inputList, pi + 1, high)
    return
def Partition(inputList, low, high):
    pivot = inputList[high]
    i = low - 1
    for j in range(low, high):
        if inputList[j] <= pivot:
            i = i+1
            inputList[i],inputList[j] = inputList[j],inputList[i]
    inputList[i+1],inputList[high] = inputList[high],inputList[i+1]
    return(i+1)
    
iterations = [1000]
values = []
tempnums = []
for i in iterations:
    for j in range(0,i):
        tempnums.append(random.randint(1,i))
    values.append(tempnums[:])
    del tempnums[:]
    
result = []
for i in values:
    result = Bubble(i)
    for j in result:
        print(j)
    #Insertion(i, len(i))
    #QuickSort(0, len(i))
