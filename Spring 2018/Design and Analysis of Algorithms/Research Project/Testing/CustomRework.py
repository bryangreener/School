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

eps = 1e-6

class Network():
    def __init__(self, layers):
        #### Initialize weight and bias arrays with random values
        self.numLayers = len(layers)
        self.layers = layers
        self.biases = [np.random.randn(y,1) for y in layers[1:]]
        self.weights = [np.random.randn(y,x) for x,y in zip(
                layers[:-1],layers[1:])]

        self.errors = [np.zeros(y.shape) for y in self.weights]
        self.zs = [np.zeros(y.shape) for y in self.weights]
        
        self.cWeights = [np.zeros(y) for y in layers[1:]]
        self.cBiases = [np.zeros(y) for y in layers[1:]]
    def __del__(self):
        print("Deleted Network")
    
    def FeedForward(self,a):
        for b,w in zip(self.biases,self.weights):
            a = sigmoid(np.dot(w,a)+b)
        return a
    
    def BackpropOld(self, x, y):
    #### Initialize delta vectors for bias/weight with zeros.
        deltaB = [np.zeros(b.shape) for b in self.biases]
        deltaW = [np.zeros(w.shape) for w in self.weights]
    #### Rolling average vector for RMS Prop
        
    #### Feed Forward z^l=w^l a^(l-1) + b^l and a^l=sig(z^l)
        activation = x
        activations = [x]
        zs = []
        for b,w in zip(self.biases,self.weights):
            z = np.dot(w,activation)+b
            zs.append(z)
            activation = sigmoid(z)
            activations.append(activation)
            
    #### Update rolling mean for RMS Prop
        #self.updateCache((activations[-1]-y), -1)
        
    #### Calculate Error E^l=delta_a C * sigprime(z^L)
        delta = (activations[-1]-y)*sigmoidPrime(zs[-1])
        deltaB[-1] = delta
        deltaW[-1] = np.dot(delta,activations[-2].T)
    #### Backpropagate Error
        for i in reversed(range(0,self.numLayers-2)):
    #### Calculate E^l=((w^(l+1)^T E^(l+1)) * sigprime(z^l))
            delta = np.dot(self.weights[i+1].T,delta)*sigmoidPrime(zs[i])
    #### Calculate dC/dw_jk^l = a_k^(l-1) E_j^l and dC/db_j^l = E_j^l
            deltaB[i] = delta
            deltaW[i] = np.dot(activations[i+1].T,delta)
    #### Update cache for RMS Prop
            #self.updateCache(deltaW[i], i)
        return(deltaB,deltaW)

    def Momentum(self, trainData, epochs, miniBatchSize, 
                 alpha, gamma, testData=None):
        velocity = [np.zeros(w.shape) for w in self.weights]
        miniBatches = [trainData[x:x+miniBatchSize] \
                           for x in range(0,len(trainData),miniBatchSize)]
        
        for i in range(1, epochs + 1):
            random.shuffle(trainData)
            idx = np.random.randint(0, len(miniBatches))
            nablaW = self.miniBatch(miniBatches[idx], alpha)
            
            for layer in range(len(nablaW)):
                velocity[layer] = np.multiply(gamma,velocity[layer]) + \
                    np.multiply(alpha,nablaW[layer])
                self.weights[layer] += np.multiply(-alpha/ \
                            len(miniBatches[idx]),velocity[layer])
            
    #### If testData is passed in, forward pass and compare results then print
            if testData:
                nTest = len(testData)
                tempEval = self.evaluate(testData)
                print("Epoch {0}: {1} / {2}".format(i, tempEval, nTest))
            else:
                print("Epoch {0} complete".format(i))
            

    def RMSProp(self, trainData, epochs, miniBatchSize, alpha, testData=None):
        n = len(trainData)
        decay = 0.9
        cacheW = [np.zeros(w.shape) for w in self.weights]
        cacheB = [np.zeros(b.shape) for b in self.biases]
        miniBatches = [trainData[x:x+miniBatchSize] \
                           for x in range(0,n,miniBatchSize)]
    #### Repeat train process for number of epochs specified
        for h in range(1, epochs+1):
            idx = np.random.randint(0,len(miniBatches))
            nablaW, nablaB = self.miniBatch(miniBatches[idx], alpha)
            k = 0
            for i in nablaW:
                l = 0
                for j in i:
                    cacheW[k][l] = decay * cacheW[k][l] + \
                        (1.0 - decay) * (i[l]**2)
                    self.weights[k][l] += alpha * i[l] / \
                        (np.sqrt(cacheW[k][l]) + eps)
                    l += 1
                k += 1
            k = 0
            for i in nablaB:
                l = 0
                for j in i:
                    cacheB[k][l] = decay * cacheB[k][l] + \
                        (1.0 - decay) * (i[l]**2)
                    self.biases[k][l] += alpha * i[l] / \
                        (np.sqrt(cacheB[k][l]) + eps)
                    l += 1
                k += 1
                    
    #### If testData is passed in, forward pass and compare results then print
            if testData:
                nTest = len(testData)
                tempEval = self.evaluate(testData)
                print("Epoch {0}: {1} / {2}".format(h, tempEval, nTest))
            else:
                print("Epoch {0} complete".format(h))
                
    def miniBatch(self, mb, alpha):
        for x,y in mb:
            activation = x
            activations = [x]
            
            for w,b in zip(self.weights,self.biases):
                z = np.dot(w,activation)+b
                self.zs.append(z)
                activation = sigmoid(z)
                activations.append(activation)
            self.errors[-1] = (activations[-1]-y)*sigmoidPrime(self.zs[-1])
        return self.Backprop()
    
    def Backprop(self):
        ret = []
        for i in reversed(range(0,self.numLayers - 2)):
            ret.append(np.dot(self.weights[i+1].T,self.errors[i+1]))
            self.errors[i] += np.dot(self.weights[i+1].T, 
                       self.errors[i+1]) * sigmoidPrime(self.zs[i])
        return ret
            
    def evaluate(self, testData):
        res = [(np.argmax(self.FeedForward(x)),y) for x,y in testData]
        return sum(int(x==y) for x,y in res)
        
trainData, testData = loadDataWrapper()
net = Network([784,35,10])
#net.RMSProp(trainData[:1000], 1000, 15, 1.0, testData[:100])
net.Momentum(trainData[:60000], 1000, 15, 1.0, 0.9, testData[:1000])