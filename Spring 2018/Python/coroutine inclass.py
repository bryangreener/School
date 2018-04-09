def gen():
    


iter_obj = gen()
next(iter_obj)
for val in [3,10,22,7,15,26,70]:
    print(iter_obj.send(val))