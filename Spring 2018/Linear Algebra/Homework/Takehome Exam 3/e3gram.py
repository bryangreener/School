import numpy as np
from sympy import *
'''
M=np.array([[1,2],[1,1]])
R=np.array([[1],[1]])
for i in range(11):
    print(R)
    R = np.dot(M,R)
'''
'''
def powerIter(A, n):
    b_k = np.random.rand(A.shape[0],1)
    for _ in range(n):
        b_k1 = np.dot(A, b_k)
        b_k1_norm = np.linalg.norm(b_k1)
        b_k = b_k1 / b_k1_norm
    return b_k
'''
    
M = np.matrix([[6,-4,1],[-4,6,-1],[1,-1,11]])


u = []
v = []
u.append(np.array([[1],[1],[1]]))
maxeigen = 0
for i in range(10000):
    v.append(M.dot(u[i]))
    u1 = v[i]
    u.append(v[i]/np.max(v[i]))
    u2 = u[i+1]
    maxeigen = np.max(v[i])
print(maxeigen)

E = np.linalg.eigvals(M)
print([Matrix((np.identity(3)*round(e)) - M).nullspace() for e in E])