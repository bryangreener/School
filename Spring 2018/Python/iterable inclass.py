def iterable(obj):
    try:
        iter(obj)
        return True
    except TypeError:
        return False
    
for element in [1, ['a', 'b', 'c'], {'a':1,'b':2}, "string", 1.0035, (1,2,3)]:
    print(element, "Iterable? ", iterable(element))
    
class xrange:
    def __init__(self, n):
        self.i = 0
        self.n = n
    
    def __iter__(self):
        return self
    def next(self):
        if self.i < self.n:
            i = self.i
            self.i += 1
            return i
        else:
            raise StopIteration()
            
x = xrange(5)

import itertools as it
for x,y in it.izip(['a','b','c'],[1,2,3]):
    print(x,y)