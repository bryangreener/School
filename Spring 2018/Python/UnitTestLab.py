import math2
import unittest

class TestMath2(unittest.TestCase):
    def testAdd(self):
        self.assertEqual(math2.add(1,2), 3)
    def testMultiply(self):
        self.assertEqual(math2.multiply(1,2), 2.0)
    def testDivide(self):
        self.assertEqual(math2.divide(2,2), 1.0)

if __name__ == "__main__":
    unittest.main()
