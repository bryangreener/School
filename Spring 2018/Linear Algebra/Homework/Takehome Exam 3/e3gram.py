import numpy as np

M=np.array([[1,2],[1,1]])
R=np.array([[1],[1]])
for i in range(11):
    print(R)
    R = np.dot(M,R)
    