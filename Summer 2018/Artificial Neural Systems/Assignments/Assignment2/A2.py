# -*- coding: utf-8 -*-
"""
Created on Sun May 20 10:03:36 2018

@author: Bryan
"""

import numpy as np
import pandas as pd
import csv
import random

class Network():
    def __init__(self, layers):
        self.layers = layers
        self.w = [np.random.randn(y,x) for x,y in zip(layers[:-1],layers[1:])]
        self.a = [np.random.randn(x,1) for x in self.layers]
        self.z = [np.random.randn(x,1) for x in self.layers]

    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))

    def sigmoidPrime(self, z):
        return np.exp(-z)/((1.0 + np.exp(-z))**2)

    def forward(self, x):
        self.a[0] = x
        for i in range(1, len(self.layers)):
            self.a[i] = self.sigmoid(np.dot(self.a[i-1], self.w[i-1].T))
        return self.a[-1]

    def backward(self, x, y, eta):
        #### Calculate error of last layer
            self.z[-1] = self.sigmoid(np.dot(self.a[-2], self.w[-1].T))
            delta = np.multiply(-(y-self.a[-1], self.sigmoidPrime(self.z[-1])))
            self.w[-1] = self.w[-1] - eta * delta
            # Normalize
            self.w[-1] = self.w[-1] / self.w[-1].max(axis=0)
            
        #### Send errors backward
            for i in range(len(self.layers) - 2, 0, -1):
                self.z[i] = self.sigmoid(np.dot(self.a[i-1], self.w[i-1].T))
                delta = np.multiply(-(y-self.a[-1]),
                                    self.sigmoidPrime(self.z[i-1]))
                dw = np.divide(np.dot(self.a[i-1].T, delta) + self.w[i].T, 
                               x.shape[0])
                self.w[i] = self.w[i] - eta * dw
                # Normalize
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
            print("Epoch {0},{1}  {2}% accuracy".Format(epoch, epochs,
                  (accuracy/testData.count)*100))
            
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

