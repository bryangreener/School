import numpy as np
import random
import time
from mnist import MNIST

def loadData():
    mndata = MNIST('../mnist-data')
    mndata.gz = True
    trainData = mndata.load_training()
    testData = mndata.load_testing()
    return trainData, testData
def loadDataWrapper():
    trd, ted = loadData()
    trainInputs = [np.reshape(x,(784,1)) for x in trd[0]]
    trainInputs = np.multiply((1.0/255.0),np.array(trainInputs))
    trainResults = [vectorizedResult(y) for y in trd[1]]
    trainData = list(zip(trainInputs, trainResults))
    testInputs = [np.reshape(x,(784,1)) for x in ted[0]]
    testInputs = np.multiply((1.0/255.0),np.array(testInputs))
    testData = list(zip(testInputs,ted[1]))
    return trainData,testData
def vectorizedResult(i):
    e = np.zeros((10,1))
    e[i] = 1.0
    return e

def testWrapper(func, args):
        return func(args)

def sigmoid(z):
    return 1.0/(1.0+np.exp(-z))
def sigmoidPrime(z):
    return(sigmoid(z)*(1-sigmoid(z)))

eps = 1e-8

class Network():
    def __init__(self, layers):
        #### Initialize weight and bias arrays with random values
        self.numLayers = len(layers)
        self.layers = layers
        self.biases = [np.random.randn(y,1) for y in layers[1:]]
        self.weights = [np.random.randn(y,x) for x,y in zip(
                layers[:-1],layers[1:])]
        #### Arrays used for rolling cost accumulation
        self.eRho =  np.array([0.02,0.08,0.3,0.6])
        self.ePast = [[np.array([np.zeros(y)]).T for x in range(4)] \
            for y in layers[1:]]
        self.grads = [[np.array([np.zeros(y)]).T for x in range(4)] \
            for y in layers[1:]]
    
    def __del__(self):
        print("Deleted Network")
    
    def FeedForward(self,a):
        for b,w in zip(self.biases,self.weights):
            a = sigmoid(np.dot(w,a)+b)
        return a
    
    def Backprop(self, x, y):
    #### Initialize delta vectors for bias/weight with zeros.
        deltaB = [np.zeros(b.shape) for b in self.biases]
        deltaW = [np.zeros(w.shape) for w in self.weights]
    #### Feed Forward z^l=w^l a^(l-1) + b^l and a^l=sig(z^l)
        activation = x
        activations = [x]
        zs = []
        for b,w in zip(self.biases,self.weights):
            z = np.dot(w,activation)+b
            zs.append(z)
            activation = sigmoid(z)
            activations.append(activation)
    #### Calculate Error E^l=delta_a C * sigprime(z^L)
        gradient= activations[-1]-y
        self.updateErrors(gradient, self.numLayers-2)
        delta = gradient*sigmoidPrime(zs[-1])
        deltaB[-1] = delta
        deltaW[-1] = np.dot(delta,activations[-2].T)
    #### Backpropagate Error
        for i in reversed(range(0,self.numLayers-2)):
    #### Calculate E^l=((w^(l+1)^T E^(l+1)) * sigprime(z^l))
            delta = np.dot(self.weights[i+1].T,delta)*sigmoidPrime(zs[i])
    #### Calculate dC/dw_jk^l = a_k^(l-1) E_j^l and dC/db_j^l = E_j^l
            deltaB[i] = delta
            deltaW[i] = np.dot(activations[i+1].T,delta)
            self.updateErrors(deltaW[i], i)
        return(deltaB,deltaW)

    def RMSProp(self, trainData, epochs, miniBatchSize, alpha, testData=None):
        n = len(trainData)
    #### Repeat train process for number of epochs specified
        for i in range(1, epochs+1):
            random.shuffle(trainData)
            miniBatches = [trainData[x:x+miniBatchSize] \
                           for x in range(0,n,miniBatchSize)]
    #### Calculate minibatch gradients
            for miniBatch in miniBatches:
                self.getMiniBatchGradient(miniBatch, alpha)
    
    #### If testData is passed in, forward pass and compare results then print
            if testData:
                nTest = len(testData)
                tempEval = self.evaluate(testData)
                print("Epoch {0}: {1} / {2}".format(i, tempEval, nTest))
            else:
                print("Epoch {0} complete".format(i))
    
    def updateErrors(self, gradient, layer):
        self.ePast[:-1] = self.ePast[1:];
        self.ePast[layer][-1] = np.multiply(0.9,self.ePast[layer][-1]) + \
            np.multiply(0.1,gradient**2)
        self.ePast[layer][-1] = np.array([np.multiply(np.array( \
                  self.ePast[layer]).T[0],self.eRho).T.sum(axis=0)]).T
                
        self.grads[:-1] = self.grads[1:];
        self.grads[layer][-1] = gradient
        self.grads[layer][-1] = np.array([np.multiply(np.array( \
                  self.grads[layer]).T,self.eRho).T.sum(axis=0)]).T
        
    def evaluate(self, testData):
        res = [(np.argmax(self.FeedForward(x)),y) for x,y in testData]
        return sum(int(x==y) for x,y in res)
    
    def getMiniBatchGradient(self, miniBatch, alpha):
        nBiases = [np.zeros(b.shape) for b in self.biases]
        nWeights = [np.zeros(w.shape) for w in self.weights]

        for x,y in miniBatch:
            dnBiases,dnWeights = self.Backprop(x,y)

            nBiases = [nb+dnb for nb,dnb in zip(nBiases,dnBiases)]
            nWeights = [nw+dnw for nw,dnw in zip(nWeights,dnWeights)]
        self.weights = [w-np.multiply(alpha,g[-1])/np.sqrt(e[-1]+eps) \
                        for w,g,e in zip(self.weights,self.grads,self.ePast)]
        self.biases = [b-(alpha/len(miniBatch))*nb
                       for b, nb in zip(self.biases, nBiases)]
        
            
        
        
trainData, testData = loadDataWrapper()
net = Network([784,35,15,20,10])
net.RMSProp(trainData[:5000], 30, 15, 0.001, testData[:500])