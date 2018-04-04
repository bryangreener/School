class my_enumerate:
    def __init__(self, data):
        self.data = data
        self.i = 0
    def __iter__(self):
        return self
    def next(self):
        if self.i < len(self.data):
            i = self.i
            self.i += 1
            return i, self.data[i]
        else:
            raise StopIteration()
            
x = my_enumerate(['a','b','c'])
''' Run by typing x.next() in console until error is thrown '''