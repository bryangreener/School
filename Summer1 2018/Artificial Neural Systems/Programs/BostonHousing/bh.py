# -*- coding: utf-8 -*-
"""
Created on Wed May  9 14:55:57 2018

@author: Bryan
"""

import numpy as np
import pandas as pd
import random
from sklearn import preprocessing

#df = pd.read_csv("./BostonHousing.csv", header=None)
#data = np.array([df[1:]], dtype=float)
data = np.genfromtxt('./BostonHousing.csv', delimiter=',')[1:]
data = np.multiply((1.0/data.max(axis=0)), data)

class Network():
    def __init__(self):
        self.weights = [np.random.randn(13,1)]
        self.a = [np.random.randn(1,1)]
        
    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))

    def forward(self, x):
        z = np.dot(x, self.weights[0])
        self.a[0] = self.sigmoid(z)
        return self.a[0]
        
    def train(self, trainData, testData, epochs):
        for e in range(epochs):
            random.shuffle(trainData)
            for d in trainData:
                yhat = self.forward(np.array([d[:-1]]))
                self.weights[0] = self.weights[0] - \
                    np. multiply((d[-1] - yhat), self.a[0])
                self.weights[0] = self.weights[0]/np.amax(self.weights[0],axis=0)
            
            result = 0
            for d in testData:
                if self.forward(d[:-1]) == 1: 
                    result += 1
            print("Epoch {0}: {1}/106".format(e, result))

net = Network()
net.train(data[:400], data[401:], 100)

               
            
        