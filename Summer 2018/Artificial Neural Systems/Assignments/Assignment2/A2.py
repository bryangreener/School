# -*- coding: utf-8 -*-
"""
Created on Sun May 20 10:03:36 2018
@author: Bryan
"""

import numpy as np
import pandas as pd
import random
from sklearn.metrics import mean_squared_error as mse
from math import sqrt

class Network():
    def __init__(self, layers):
        self.layers = layers
        self.w = [np.random.randn(x,y) for x,y in zip(layers[:-1],layers[1:])]
        self.a = [np.zeros((1,x.shape[1])) for x in self.w]
        self.z = [np.zeros((1,x.shape[1])) for x in self.w]

    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))

    def sigmoidPrime(self, z):
        return np.exp(-z)/((1.0 + np.exp(-z))**2)

    def forward(self, x):
        self.z[0] = x.dot(self.w[0])
        self.a[0] = self.sigmoid(self.z[0])
        for i in range(1, len(self.layers) - 1):
            self.z[i] = self.a[i-1].dot(self.w[i])
            self.a[i] = self.sigmoid(self.z[i])
        return self.a[-1]

    def backward(self, x, y, eta):
        lam = 1e-4
        delta = -(y-self.a[-1]) * self.sigmoidPrime(self.z[-1])
        for i in range(len(self.layers)-2, 0, -1):
            dw = (self.a[i-1].T.dot(delta) + lam * self.w[i]) / x.shape[0]
            delta = delta.dot(self.w[i].T) * self.sigmoidPrime(self.z[i-1])
            self.w[i] = self.w[i] - (eta * dw)
        dw = (x.T.dot(delta) + lam * self.w[0]) / x.shape[0]
        self.w[0] = self.w[0] - (eta * dw)
        
    def train(self, trainData, testData, epochs, eta):
        for epoch in range(epochs):
            random.shuffle(trainData)
            for t in trainData:
                t = t.reshape(-1, 1)
                self.forward(t[:-1].T)
                self.backward(t[:-1].T, t[-1].T, eta)
            
            self.test(testData, epoch, epochs)

    def test(self, testData, e, es):
        rms = sqrt(mse([[t[-1]*norm] for t in testData],
                       [[self.forward(t[:-1])[0] * norm] for t in testData]))
        #print("Epoch {0: >4}/{1} rmse: ${2: >10.2f}".format(e + 1, es, rms))
    
    def newTestFunc(self, testData):
        return [self.forward(t)[0] * norm for t in testData]

#### Convert categorical variables to one-hots
def category(df):
    for col in list(df.select_dtypes(exclude=[np.number])):
        temp = pd.get_dummies(df[col])
        for t in temp:
            df.insert(df.columns.get_loc(col), str(col)+" "+str(t), temp[t])
        df = df.drop([col], axis=1)
    return df

def compare(a, b):
    for header in a:
        if header not in b.columns:
            a = a.drop([header], axis=1)
    for header in b:
        if header not in a.columns:
            b = b.drop([header], axis=1)
    return a,b
            
dfa = category(pd.read_csv('./other_housing.csv').fillna(value=0))
dfb = category(pd.read_csv('./other_housing_test.csv').fillna(value=0))

df, newdf = compare(dfa.iloc[:,:-1], dfb.iloc[:,1:])
df.insert(0, dfa.iloc[:,-1].name, dfa.iloc[:,-1])
newdf.insert(0, dfb.iloc[:,0].name, dfb.iloc[:,0])

inputData = df.values
newTest = newdf.iloc[:,1:].values


# Normalize Data
norm = np.linalg.norm(inputData)
inputData = inputData / np.linalg.norm(inputData)

# Shuffle before splitting data
random.shuffle(inputData)

# 75/25 split of data
trainData = inputData[round(inputData.shape[0] * 0.75):]
testData = inputData[round(inputData.shape[0] * 0.25 + 1):]

net = Network([inputData.shape[1]-1, 30, 10, 1])
net.train(trainData, testData, 500, 0.01)

newTest = newTest / np.linalg.norm(inputData)
newTestRes = net.newTestFunc(newTest)
final = np.array([newdf.iloc[:,0].values, newTestRes])
blah = pd.DataFrame(np.column_stack([newdf.iloc[:,0].values, newTestRes]), columns=['Order', 'PredictedSalesPrice_FirstLast'])
print("Complete")
blah.to_csv('results.csv')
