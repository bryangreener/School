import threading
def main_thread():
    global x
    x = 0
    t1 = threading.Thread(target=thread_job)
    t2 = threading.Thread(target=thread_job)
    t1.start()
    t2.start()
    