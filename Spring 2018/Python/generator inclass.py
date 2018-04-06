'''
x = [1,2,3]
d = iter(x)
while d:
    try:
        print(next(d))
    except StopIteration:
        break
        
def gen():
    #the yield function is what makes a method a generator
    yield 1
    # the return of a generator returns the stopiteration error
    # generator can have multiple yield statements

# main
#d = gen()
#next(d)
#next(d) # this will call the stop iteration error

def fib(n):
    a,b,count = 0,1,0
    while True:
        if count> n:
            return
        yield a
        a,b = b, a+b
        count += 1
        
f = fib(5)
next(f)
next(f)
next(f)
next(f)
next(f)

def gen():
    yield 1
    #raise StopIteration(5)
    return 5

def co():
    print("Coroutine is started")
    x = yield
    print("Coroutine has received a value: ", x)
    
d = co()
next(d)
d.send("OOF")
# will break with stop iteration when sent

'''

def gen(obj):
    index = 0
    while True:
        if index> len(obj):
            index = 0
            try:
                message = yield obj[index]
            except Exception:
                print("index: ", index)
        if message != None:
            index=0 if message < 0 else message
        else:
            index += 1

d = gen("CS2000 Python Programming")
next(d)
next(d)
d.send(7) #gives 7th item in passed in message
d.send(8) #gives 8th item in passed in message
next(d) #due to previous send, this returns 9th item in message
d.send(30) #returns C since index gets sent to 0