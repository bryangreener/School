import threading
import time
def sleeper():
    print(threading.currentThread().getName())
    time.sleep(5)
    print(threading.currentThread().getName())
    return

threads = 5
jobs = []
for i in range(threads):
    thread = threading.Thread(target=sleeper)
    jobs.append(thread)
for x in jobs:
    x.start()
for x in jobs:
    x.join()
