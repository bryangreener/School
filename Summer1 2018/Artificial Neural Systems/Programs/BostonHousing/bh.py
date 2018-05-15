import numpy as np
import random


class Network():
    def __init__(self):
        self.weights = np.random.randn(13,1)
        
    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))
    
    def relu(self, z):
        return z * (z > 0.0)
    
    def train(self, data, epochs):
        random.shuffle(data)
        trainData = np.array([data[:400]])
        testData = np.array([data[401:]])
        
        for e in range(epochs):
            random.shuffle(trainData)
            for d in trainData[0]:
                yhat = self.sigmoid(np.dot(d[:-1], self.weights))
                cost = d[-1] - yhat
                self.weights += np.multiply(cost, np.array([d[:-1]]).T)
                
            correct = 0
            for d in testData[0]:
                yhat = np.dot(d[:-1], self.weights)
                if (yhat > mean and d[-1] > mean) or \
                    (yhat < mean and d[-1] < mean):
                    correct += 1
            print("Epoch: {0}  {1}/{2}".format(e, correct, testData[0].shape[0]))


inputData = np.genfromtxt('./bh.csv', delimiter=',', dtype="|U5")[1:]
for row in range(inputData.shape[0]):
    for item in range(inputData.shape[1]):
        inputData[row][item] = inputData[row][item].replace('"','')
data = inputData.astype(float)
dNorm = data / data.max(axis=0)
mean = dNorm.mean(axis=0)[-1]

net = Network()
net.train(dNorm, 10000)
