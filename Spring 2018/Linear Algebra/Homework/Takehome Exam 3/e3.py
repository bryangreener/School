import numpy as np
from fractions import Fraction
'''
M = np.array([[1,2],[1,1]])
A=1
B=1
r = np.array([[1],[1]])
for i in range(10):
    r = np.dot(M,r)
    print(r)
    print('---')
'''

A=np.matrix([[2,3,4,5],[3,4,5,6],[4,5,6,7],[5,6,7,8]])
U, s, V = np.linalg.svd(A)
print(U,s,V)
print(np.allclose(A, U * np.diag(s) * V))
print(np.round([np.dot(U[:,i-1].A1, U[:,i].A1) for i in range(1,len(U))]))
print(np.round(np.sum((np.multiply(U,U)),0)))
print(np.allclose(U.T * U, np.identity(len(U))))