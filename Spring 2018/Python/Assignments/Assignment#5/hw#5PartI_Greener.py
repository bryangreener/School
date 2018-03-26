# Name:     Bryan Greener
# Date:     2018-03-25
# Homework: #5

import unittest
from functools import reduce
# This class handles all logic and is called by testBookshop class.
class bookshop:
    def m1(orderList):
        return(list(map(lambda x: [x[0]] + list(map(lambda y: tuple([y[0], y[1]*y[2] if y[1]*y[2] >= 100 else y[1]*y[2] + 10]), x[1:])), orderList)))
    
    def m2(orderList):
        return(list(map(lambda x: tuple([x[0], x[1][0]]), list(map(lambda x: [x[0]] + sorted(list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), key=lambda y: [y[1]]), orderList)))))

    def m3(orderList):
        return(list(map(lambda x: tuple([x[0], x[1][0]]), list(map(lambda x: [x[0]] + sorted(list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), key=lambda y: [y[1]], reverse=True), orderList)))))

    def m4(orderList):
        return(list(map(lambda x: tuple([x[0], round(sum([item[1] for item in x[1:]]), 2)]), list(map(lambda x: [x[0]] + list(map(lambda y: tuple([y[0], y[1]*y[2]]), x[1:])), orderList)))))

    def m5(self):
        t0 = list(map(lambda x: tuple([x[0],x[1]*x[2]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist]))
        t1 = [tuple(reduce(lambda x,y: x, ([y[0]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]
        t2 = [val for t in t1 for val in t]
        t3 = list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]))
        t4 = sorted(list(zip(t2, t3)), key=lambda x: x[1], reverse=True)
        return(list(t4[0]))
        
    def m6(orderList):
        t0 = list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist]))
        t1 = [tuple(reduce(lambda x,y: x, ([y[0]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]
        t2 = [val for t in t1 for val in t]
        t3 = list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]))
        t4 = sorted(list(zip(t2, t3)), key=lambda x: x[1], reverse=True)
        return(list(t4[0]))

    def m7(orderList):
        return(sorted(list(map(lambda x: tuple([x[0]] + list(map(sum, [list(map(lambda y: y[1], x[1:]))]))),orderList)), key=lambda k: int(k[1]), reverse=True))
        
    def m8(orderList):
        return(sum(reduce(lambda x,y: x+y, ([y[1]] for y in [t for sublist in list(map(lambda z: z[1:], orderList)) for t in sublist]))))
        
    def m9(orderList):
        
        t0 = list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist]))
        t1 = [tuple(reduce(lambda x,y: x, ([y[0]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]
        t2 = [val for t in t1 for val in t]
        t3 = list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in t0 if y[0]==x))) for x in set(y[0] for y in t0)]))
        t4 = sorted(list(zip(t2, t3)), key=lambda x: int(x[1]))
        return([t4[len(t4)-1][0],t4[0][0]])
        
        # I can also write this in one line however you get the line below which is 2,222 characters long and is evil.
        #return([sorted(list(zip([val for t in [tuple(reduce(lambda x,y: x, ([y[0]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))] for val in t], list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))])))), key=lambda x: int(x[1]))[len(sorted(list(zip([val for t in [tuple(reduce(lambda x,y: x, ([y[0]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))] for val in t], list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))])))), key=lambda x: int(x[1])))-1][0],sorted(list(zip([val for t in [tuple(reduce(lambda x,y: x, ([y[0]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))] for val in t], list(map(sum, [tuple(reduce(lambda x,y: x+y, ([y[1]] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])) if y[0]==x))) for x in set(y[0] for y in list(map(lambda x: tuple([x[0],x[1]]), [t for sublist in list(map(lambda x: x[1:], orderList)) for t in sublist])))])))), key=lambda x: int(x[1]))[0][0]])
        
    def m10(orderList):
        return(list(map(lambda x: len(x), orderList)))

class testBookshop(unittest.TestCase):
    def testM1(self):
        self.assertEqual(bookshop.m1(orderList), [  [1, ("5464", 49.96), ("8274",233.82), ("9744", 404.55)],
                                                    [2, ("5464", 99.91), ("9744", 404.55)],
                                                    [3, ("5464", 99.91), ("88112", 274.89)],
                                                    [4, ("8732", 93.93), ("7733", 208.89), ("88112", 199.75)]])
    def testM2(self):
        self.assertEqual(bookshop.m2(orderList), [(1,"5464"), (2,"5464"), (3,"5464"), (4,"8732")])
    def testM3(self):
        self.assertEqual(bookshop.m3(orderList), [(1,"9744"), (2,"9744"), (3,"88112"), (4,"7733")])
    def testM4(self):
        self.assertEqual(bookshop.m4(orderList), [(1, 678.33), (2, 494.46), (3, 364.8), (4, 492.57)])
    def testM5(self):
        self.assertEqual(bookshop.m5(orderList), ["9744", 809.1])
    def testM6(self):
        self.assertEqual(bookshop.m6(orderList), ["5464", 22])
    def testM7(self):
        self.assertEqual(bookshop.m7(orderList), [(1, 31), (4, 23), (3, 20), (2, 18)])
    def testM8(self):
        self.assertEqual(bookshop.m8(orderList), 92)
    def testM9(self):
        self.assertEqual(bookshop.m9(orderList), ["5464", "8732"])
    def testM10(self):
        self.assertEqual(bookshop.m10(orderList), [4, 3, 3, 4])

# Global list for easier referencing.
orderList = [ [1, ("5464", 4, 9.99), ("8274",18,12.99), ("9744", 9, 44.95)],
               [2, ("5464", 9, 9.99), ("9744", 9, 44.95)],
               [3, ("5464", 9, 9.99), ("88112", 11, 24.99)],
               [4, ("8732", 7, 11.99), ("7733", 11,18.99), ("88112", 5, 39.95)] ]

# Begin unit tests.
if __name__ == '__main__':
    unittest.main()
