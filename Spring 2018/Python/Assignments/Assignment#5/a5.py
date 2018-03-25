import unittest
from functools import reduce
from operator import add
class bookshop:
    def m1(orderList):
        return(list(map(lambda x: [x[0]] + list(map(lambda y: tuple([y[0], y[1]*y[2] if y[1]*y[2] >= 100 else y[1]*y[2] + 10]), x[1:])), orderList)))
    
    def m2(orderList):
        return(list(map(lambda x: tuple([x[0], x[1][0]]), list(map(lambda x: [x[0]] + sorted(list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), key=lambda y: [y[1]]), orderList)))))

    def m3(orderList):
        return(list(map(lambda x: tuple([x[0], x[1][0]]), list(map(lambda x: [x[0]] + sorted(list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), key=lambda y: [y[1]], reverse=True), orderList)))))

    def m4(orderList):
        #list(map(lambda x: [x[0]] + list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), orderList))
        return(list(map(lambda x: tuple([x[0], round(sum([item[1] for item in x[1:]]), 2)]), list(map(lambda x: [x[0]] + list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), orderList)))))

    def m5(self):
        temp = list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist]))
        print(temp)
        t2 = [tuple(reduce(lambda x,y: x, ([y[0]] for y in temp if y[0]==x))) for x in set(y[0] for y in temp)]
        t3 = list(reduce(lambda x,y: x+y, list(map(lambda z: z, # LEFT OFF HERE [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in temp if y[0]==x))) for x in set(y[0] for y in temp)]))))
        print(t2)
        print(t3)
        #return([tuple(reduce(lambda x,y: x+y, ([y[0]] for y in temp if y[0]==x))) for x in set(y[0] for y in temp)])
        
    #def m6(orderList):
        #print(list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))
        #return([(a, sum([y for (x,y) in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if x == a]) for a in list(map(lambda z: tuple([z[0],z[1]]), [t for sublist in list(map(lambda z: z[1:], orderList)) for t in sublist])))])
        

class testBookshop(unittest.TestCase):
    def tm1(self):
        self.assertEqual(bookshop.m1(orderList), [ [1, ("5464", 49.96), ("8274",233.82), ("9744", 404.55)],
               [2, ("5464", 99.91), ("9744", 404.55)],
               [3, ("5464", 99.91), ("88112", 274.89)],
               [4, ("8732", 93.93), ("7733", 208.89), ("88112", 199.75)] ])
    def tm2(self):
        self.assertEqual(bookshop.m2(orderList), [ (1, "5464"), (2, "5464"), (3, "5464"), (4, "8732") ])
    def tm3(self):
        self.assertEqual(bookshop.m2(orderList), [ (1, "9744"), (2, "9744"), (3, "88112"), (4, "7733") ])

orderList = [ [1, ("5464", 4, 9.99), ("8274",18,12.99), ("9744", 9, 44.95)],
               [2, ("5464", 9, 9.99), ("9744", 9, 44.95)],
               [3, ("5464", 9, 9.99), ("88112", 11, 24.99)],
               [4, ("8732", 7, 11.99), ("7733", 11,18.99), ("88112", 5, 39.95)] ]

print("\nM1\n")
print(bookshop.m1(orderList))
print("\nM2\n")
print(bookshop.m2(orderList))
print("\nM3\n")
print(bookshop.m3(orderList))
print("\nM4\n")
print(bookshop.m4(orderList))
print("\nM5\n")
print(bookshop.m5(orderList))
print("\nM6\n")
#print(bookshop.m6(orderList))
print("\nM7\n")

print("\nM8\n")

print("\nM9\n")

print("\nM10\n")


if __name__ == "__main__":
    unittest.main()
