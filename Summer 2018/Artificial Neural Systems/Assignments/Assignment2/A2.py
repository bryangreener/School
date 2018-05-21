# -*- coding: utf-8 -*-
"""
Created on Sun May 20 10:03:36 2018
@author: Bryan
"""

import numpy as np
import pandas as pd
import random

class Network():
    def __init__(self, layers):
        self.layers = layers
        self.w = [np.random.randn(x,y) for x,y in zip(layers[:-1],layers[1:])]
        self.a = [np.random.randn(1,x) for x in self.layers]
        self.z = [np.random.randn(1,x) for x in self.layers]

    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))

    def sigmoidPrime(self, z):
        return np.exp(-z)/((1.0 + np.exp(-z))**2)

    def forward(self, x):
        # Preload input layer activation with inputs
        self.a[0] = self.sigmoid(x)
        for i in range(1, len(self.layers)):
            self.z[i] = np.dot(self.a[i-1], self.w[i-1])
            self.a[i] = self.sigmoid(self.z[i])
        return self.a[-1]

    def backward(self, x, y, eta):
        lam = 1e-4
        self.a[0] = x #update activation at first layer with inputs
        #### Calculate partial derivatives of cost (cost prime)
        for i in range(len(self.layers)-2, 0, -1):
            if i == len(self.layers)-2: #y-yhat for last layer
                delta = np.dot((self.a[-1]-y), self.sigmoidPrime(self.z[-1]))
            else: 
                delta = np.dot(delta,self.w[i+1].T)*self.sigmoidPrime(self.z[i+1])
            dw = np.divide((np.dot(delta.T,self.a[i]).T + lam * self.w[i]), 
                           (x.shape[0]))
            self.w[i] = self.w[i] - eta * dw
            self.w[i] = self.w[i] / self.w[i].max(axis=0)
        
    def train(self, trainData, testData, epochs, eta):
        for epoch in range(epochs):
            random.shuffle(trainData)
            for t in trainData:
                t = t.reshape(-1, 1)
                self.forward(t[:-1].T)
                self.backward(t[:-1].T, t[-1].T, eta)
            
            accuracy = 0.0
            for t in testData:
                accuracy += (t[-1] - self.forward(t[:-1]))
            print("Epoch {0}/{1}  {2}% accuracy".format(epoch, epochs,
                  (accuracy/testData.shape[0])*100))
            
def category(df):
    for col in list(df.select_dtypes(exclude=[np.number])):
        df[col] = df[col].astype('category')
    catCols = df.select_dtypes(['category']).columns
    df[catCols] = df[catCols].apply(lambda x: x.cat.codes)
    return df
            
            
df = category(pd.read_csv('./other_housing.csv').fillna(value=0))
inputData = df.values
inputData = inputData / inputData.max(axis=0)
# 75/25 split of data
trainData = inputData[:round(inputData.shape[0] * 0.75)]
testData = inputData[round(inputData.shape[0] * 0.75 + 1):]

net = Network([inputData.shape[1]-1, 10, 20, 1])
net.train(trainData, testData, 1000, 0.01)
