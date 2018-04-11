import numpy as np
M = np.array([[1,2],[1,1]])
A=1
B=1
r = np.array([[1],[1]])
for i in range(10):
    r = np.dot(M,r)
    print(r)
    print('---')