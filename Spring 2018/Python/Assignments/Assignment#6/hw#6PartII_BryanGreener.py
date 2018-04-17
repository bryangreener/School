def lev(a, len_a, b, len_b):
    @MemoizeReset
    cost = 0
    if len_a == 0: return len_b
    if len_b == 0: return len_a
    if a[len_a - 1] == b[len_b - 1]:
        cost = 0
    else:
        cost = 1
    
    return min(lev(a, len_a - 1, b, len_b) + 1,
               lev(a, len_a, b, len_b - 1) + 1,
               lev(a, len_a - 1, b, len_b - 1) + cost)
    
class MemoizeReset(object):
    def __init__(self, f):
        self.count = 0
        self.f = f
        self.cache = {}
    def __call__(self, *args):
        try:
            self.count += 1
            return self.cache[args]
        except KeyError:
            value = self.f(*args)
            self.cache[args] = value
            return value
        except TypeError:
            return self.f(*args)
    def __repr__(self):
        return self.f.__doc__
    def __get__(self, obj, objtype):
        fn = functools.partial(self.__call__, obj)
        fn.reset = self._reset
        return fn
    def _reset(self):
        self.cache = {}
        
        

    
    

