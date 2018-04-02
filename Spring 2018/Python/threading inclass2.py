import threading
import random
import time
def listAppend(size, outList):
    del outList[:]
    for i in range(size):
        outList.append(random.randint(0,size))
    print(threading.currentThread().getName())
    print(outList)

if __name__ == "__main__":
    size = 10
    threads = 100000
    jobs = []
    for i in range(threads):
          jobs.append(threading.Thread(target=listAppend, args=(size,[])))
    for x in jobs:
          x.start()
    for x in jobs:
          x.join()
    print("Complete")
