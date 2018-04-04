class reverse_iter:
    def __init__(self, data):
        self.data = data
        self.index = len(data) - 1
    def __iter__(self):
        return self
    def next(self):
        if self.index >= 0:
            index = self.index
            self.index -= 1
            return self.data[index]
        else:
            raise StopIteration()

x = reverse_iter([1,2,3,4])
''' Run by typing x.next() in console until error is thrown '''