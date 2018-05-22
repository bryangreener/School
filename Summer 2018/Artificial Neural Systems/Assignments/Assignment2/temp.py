# -*- coding: utf-8 -*-
"""
Created on Tue May 22 12:44:28 2018

@author: aabgreener
"""

import numpy as np
import pandas as pd
import random
from sklearn.metrics import mean_squared_error as mse
from math import sqrt

#### Sigmoid helper functions
def sigmoid(z):
    return 1.0/(1.0+np.exp(-z))
def sigmoidPrime(z):
    return np.exp(-z)/((1.0 + np.exp(-z))**2)
# ================================================================
    
#### Main class containing functions for train/test network
class Network():
    def __init__(self, layers):
        #### Init weights matrix with random values in std dev
        self.numLayers = len(layers)
        self.layers = layers
        self.weights = [np.random.randn(y,x) for x,y in zip(
                layers[:-1],layers[1:])]
        self.Lambda = 1e-4
        #### Init activation/Z matrices with zeros same shape
        #    as weights matrix.
        self.a = [np.zeros(y.shape) for y in self.weights]
        self.z = [np.zeros(y.shape) for y in self.weights]

    #### Method acting as backpropagation step.
    #    This is f'(dC/dw)
    def CostFunctionPrime(self, x, y):
        self.yhat = self.FeedForward(x)
        dW = []
        #### Calculate delta C gradient.
        delta = np.multiply(-(y-self.yhat), sigmoidPrime(self.z[-1]))
        #### Propagate this gradient backward through network.
        for i in reversed(range(1, self.numLayers - 1)):
            #### Update delta Weight list with dot product of
            #    scaled weights and activations. 
            dW.append(np.divide(np.dot(self.a[i-1].T, delta) + \
                                np.multiply(self.Lambda, \
                                self.weights[i].T),x.shape[0]))
            #### Update delta C gradient with these delta Weights
            delta = np.multiply(np.dot(delta, self.weights[i]), \
                                sigmoidPrime(self.z[i-1]))
        #### Update delta Weight for first layer. 
        dW.append(np.divide(np.dot(x.T, delta) + np.multiply(self.Lambda, \
                  self.weights[0].T),x.shape[0]))
        return dW # List of gradients
    
    #### Forward propagation through network by passing in X value matrices
    #    in train/test lists.
    def FeedForward(self,x):
        #### Calculate Z then Activations for input layer.
        #    Z is offset by 1 to the left
        self.z[0] = np.dot(x, self.weights[0].T)
        self.a[0] = sigmoid(self.z[0])
        #### Continue this process for remaining layers to the right.
        for i in range(1, self.numLayers-1):
            self.z[i] = np.dot(self.a[i-1], self.weights[i].T)
            self.a[i] = sigmoid(self.z[i])
        #### Last activation in a[] is the yhat value final est output
        return self.a[-1]
    
    #### Handler for minibatches. Send minibatch through costfunctionprime
    #    (backpropagation) then return the gradients.
    def miniBatch(self, mb):
        x,y = [],[]
        for i in mb:
            x.append(i[:-1].ravel())
            y.append(i[-1].ravel())
        gradients = self.CostFunctionPrime(np.array(x),np.array(y))
        return gradients
    
    #### Helper to split training data into minibatches.
    def PartitionBatches(self, trainData, miniBatchSize):
        return [trainData[x:x+miniBatchSize] \
                for x in range(0,len(trainData),miniBatchSize)]

    #### Root Mean Squared Propagation
    #    This function handles splitting input data sets
    #    and sending them to appropriate functions for
    #    forward/backward propagation.
    #### After testing, best current training hyperparameters
    #    are miniBatchSize = 15, alpha=0.075, gamma = 0.9, and beta = 0.50.
    #    These have allowed us to get 97.5% accuracy on a training
    #    set of 60,000 images and testing set of 10,000 images.
    def RMSprop(self, trainData, epochs=30, miniBatchSize=15,
                alpha=0.075, gamma=0.9, beta=5.0, testData=None):
        #### Cache = r_t, Velocity = v_t
        #    Initialize with zeros in shape of weights matrix.
        cache = [np.zeros(w.shape) for w in self.weights]
        velocity = [np.zeros(w.shape) for w in self.weights]
        
        #### Iterate over each epoch, shuffling data each time to
        #    prevent overfitting. An epoch can be seen as a single
        #    training iteration over the whole training set.
        for i in range(1,epochs + 1):
            random.shuffle(trainData)
            # Partition minibatches from training data.
            miniBatches = self.PartitionBatches(trainData, miniBatchSize)
            #### Iterate over each miniBatchand calculate gradients for each.
            for j in miniBatches:
                gradients = self.miniBatch(j)
                # Reverse gradients array for following calculations.
                gradients = gradients[::-1]
                #### Iterate over number of gradients returned.
                for k in range(len(gradients)):
                    # Save current weights before completing half-step.
                    tempWeights = self.weights[k]
                    # v_l = gamma v_l + alpha transpose(g_l)
                    velocity[k] = np.multiply(gamma,velocity[k]) + \
                        np.multiply(alpha,gradients[k].T)
                    # w_l = w_l - beta v_l
                    self.weights[k] = self.weights[k] - \
                        np.multiply(beta,velocity[k])
                    # r_l = (1-gamma) transpose(g_l)^2 + gamma r_l
                    cache[k] = np.multiply((1-gamma),gradients[k].T**2) + \
                        np.multiply(gamma, cache[k])
                    # Normalize r_l
                    cache[k] = cache[k]/np.amax(cache[k], axis=0)
                    # w_l = tempW_l - beta v_l + (alpha g_l^T) / (sqrt(r_l))
                    self.weights[k] = tempWeights - \
                        (np.multiply(beta,velocity[k]) + \
                         np.multiply(np.divide(alpha,(np.sqrt(cache[k]))), \
                                     gradients[k].T))
            #### For each epoch, forward prop testing data using RunTests()
            #    i is current epoch number.
            self.test(testData, i, epochs)
    
    #### Method that passes in each set of testing data to forward propagate.
    #    Compares observed and expected results then outputs number of
    #    correct observations.
    def test(self, testData, e, es):
        rms = sqrt(mse([[t[-1]*norm] for t in testData],
                       [[self.FeedForward(t[:-1])[0] * norm] for t in testData]))
        print("Epoch {0: >4}/{1} rmse: ${2: >10.2f}".format(e + 1, es, rms))

# ================================================================


#### List storing results from each epoch as doubles.
epochResults = []

#### Convert categorical variables to one-hots
def category(df):
    for col in list(df.select_dtypes(exclude=[np.number])):
        temp = pd.get_dummies(df[col])
        for t in temp:
            df.insert(df.columns.get_loc(col), str(col)+" "+str(t), temp[t])
        df = df.drop([col], axis=1)
    return df
            
            
df = category(pd.read_csv('./other_housing.csv').fillna(value=0))
inputData = df.values
# Normalize Data
norm = np.linalg.norm(inputData)
inputData = inputData / np.linalg.norm(inputData)

# Shuffle before splitting data
random.shuffle(inputData)

# 75/25 split of data
trainData = inputData[:round(inputData.shape[0] * 0.90)]
testData = inputData[round(inputData.shape[0] * 0.10 + 1):]

net = Network([inputData.shape[1]-1, 30, 10, 1])
net.RMSprop(trainData, epochs=1000, testData=testData)
