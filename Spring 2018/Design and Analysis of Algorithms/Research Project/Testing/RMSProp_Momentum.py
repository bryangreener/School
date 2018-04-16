import numpy as np
import random
import time
import matplotlib.pyplot as plt
from scipy import optimize
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
    return np.exp(-z)/((1.0 + np.exp(-z))**2)

class Network():
    def __init__(self, layers):
        #### Initialize weight and bias arrays with random values
        self.numLayers = len(layers)
        self.layers = layers
        self.weights = [np.random.randn(y,x) for x,y in zip(
                layers[:-1],layers[1:])]
        self.Lambda = 1e-4
        self.a = [np.zeros(y.shape) for y in self.weights]
        self.z = [np.zeros(y.shape) for y in self.weights]

    def __del__(self):
        print("Deleted Network")
    '''
    def CostFunction(self, x, y):
        self.yhat = self.FeedForward(x)
        temp = []
        for i in range(self.numLayers-1):
            temp += sum(sum(self.weights[i]**2))
        J = 0.5*sum((y-self.yhat)**2)/x.shape[0] + \
            np.multiply((self.Lambda/2),temp)
        return J
    '''
    def CostFunctionPrime(self, x, y): # Effectively backprop
        self.yhat = self.FeedForward(x)
        dW = []
        delta = np.multiply(-(y-self.yhat), sigmoidPrime(self.z[-1]))
        for i in reversed(range(1, self.numLayers - 1)):
            dW.append(np.divide(np.dot(self.a[i-1].T, delta) + \
                                np.multiply(self.Lambda, \
                                self.weights[i].T),x.shape[0]))
            delta = np.multiply(np.dot(delta, self.weights[i]), \
                                sigmoidPrime(self.z[i-1]))
        dW.append(np.divide(np.dot(x.T, delta) + np.multiply(self.Lambda, \
                  self.weights[0].T),x.shape[0]))
        return dW # List of gradients
    
    def FeedForward(self,x):
        # Z is offset by 1 to the left
        self.z[0] = np.dot(x, self.weights[0].T)
        self.a[0] = sigmoid(self.z[0])
        for i in range(1, self.numLayers-1):
            self.z[i] = np.dot(self.a[i-1], self.weights[i].T)
            self.a[i] = sigmoid(self.z[i])
        return self.a[-1]
        # Last activation in a[] is the yhat value final est output
    
    '''
    def Backprop(self, xs, hs, errs):
        deltas = [np.zeros(w.shape) for w in self.weights]
        #### Gradient from output layer to prev layer
        deltas[-1] = [np.dot(h[-1],errs) for h in hs]
        dh = np.dot(errs,self.weights[-1].T)
        dh[hs[:][-1] <= 0] = 0
        deltas[-2] = np.dot(self.weights[-2].T,dh)
        #### Gradients right to left of hidden layers
        for i in reversed(range(1, self.numLayers-2)):
            dh[hs[i] <= 0] = 0
            deltas[i] = 1
        dh = np.dot(errs,self.weights[0].T)
        dh[hs <= 0] = 0
        deltas[0] = np.dot(xs.T, dh)
        self.weights = deltas
    '''
    
    def miniBatch(self, mb):
        x,y = [],[]
        for a,b in mb:
            x.append(a.ravel())
            y.append(b.ravel())
        gradients = self.CostFunctionPrime(np.array(x),np.array(y))
        return gradients
    
    def PartitionBatches(self, trainData, miniBatchSize):
        return [trainData[x:x+miniBatchSize] \
                for x in range(0,len(trainData),miniBatchSize)]
    '''
    def Momentum(self, trainData, epochs, miniBatchSize, 
                 alpha, gamma, testData=None):
        velocity = [np.zeros(w.shape) for w in self.weights]
        for i in range(1, epochs + 1):
            random.shuffle(trainData)
            miniBatches = self.PartitionBatches(trainData, miniBatchSize)
            for j in miniBatches:
                gradients = self.miniBatch(j)
                gradients = gradients[::-1]
                for layer in range(len(gradients)):
                    velocity[layer] = np.multiply(gamma, velocity[layer]) + \
                        np.multiply(alpha, gradients[layer].T)
                    self.weights[layer] += velocity[layer]
            self.RunTests(i, testData)
    '''
    def RMSprop(self, trainData, epochs, miniBatchSize,
                alpha=0.075, gamma=0.9, beta=5.0, testData=None):
        #### Cache = r_t, Velocity = v_t
        cache = [np.zeros(w.shape) for w in self.weights]
        velocity = [np.zeros(w.shape) for w in self.weights]
        # Beta < 1
        for i in range(1,epochs + 1):
            random.shuffle(trainData)
            miniBatches = self.PartitionBatches(trainData, miniBatchSize)
            for j in miniBatches:
                gradients = self.miniBatch(j)
                gradients = gradients[::-1]
                #http://climin.readthedocs.io/en/latest/rmsprop.html
                for k in range(len(gradients)):
                    tempWeights = self.weights[k]
                    velocity[k] = np.multiply(gamma,velocity[k])+np.multiply(alpha,gradients[k].T)
                    self.weights[k] = self.weights[k] - np.multiply(beta,velocity[k])
                    cache[k] = np.multiply((1-gamma),gradients[k].T**2)+np.multiply(gamma, cache[k])
                    cache[k] = cache[k]/np.amax(cache[k], axis=0)
                    self.weights[k] = tempWeights - (np.multiply(beta,velocity[k]) + np.multiply(np.divide(alpha,(np.sqrt(cache[k]))), gradients[k].T))
                                       
                    #cache[k] = np.multiply((1-gamma),gradients[k].T**2)+np.multiply(gamma,cache[k])
                    #This line fixes it all
                    #cache[k] = cache[k]/np.amax(cache[k], axis=0)
                    #self.weights[k] = self.weights[k] - \
                    #    np.multiply((alpha/np.sqrt(cache[k])),gradients[k].T)
                    
                    #cache[k] = np.multiply(gamma, cache[k]) + \
                    #    np.multiply((1-gamma), (gradients[k].T**2))
                    #self.weights[k] += np.divide( \
                    #        np.multiply(alpha, gradients[k].T), \
                    #        (np.sqrt(cache[k]) + eps))

            self.RunTests(i, testData)
                    
    def RunTests(self, epoch, testData=None):
        if testData:
            nTest = len(np.array(testData))
            tempEval = self.evaluate(testData)
            epochResults.append(tempEval)
            print("Epoch {0}: {1} / {2}".format(epoch, tempEval, nTest))
        else:
            print("Epoch {0} complete".format(epoch))
                
    def evaluate(self, testData):
        res = [(np.argmax(self.FeedForward(x.T)),y) for x,y in testData]
        return sum(int(x==y) for x,y in res)
'''
    def GetParams(self):
        return self.weights
            
    def SetParams(self, params):
        self.weights = params
        
    def ComputeGradients(self, x, y):
        return self.CostFunctionPrime(x,y)
'''
'''
class Trainer(object):
    def __init__(self, N):
        self.N = N
        
    def CostFunctionWrapper(self, params, x, y):
        self.N.SetParams(params)
        cost = self.N.CostFunction(x,y)
        grad = self.N.ComputeGradients(x,y)
        return cost,grad
    
    def Callback(self, params):
        self.N.SetParams(params)
        self.J.append(self.N.CostFunction(self.trainX, self.trainY))
        self.TJ.append(self.N.CostFunction(self.testX, self.testY))
    
    def Train(self, trainData, testData):
        trainX,trainY,testX,testY = [],[],[],[]
        for a,b in trainData:
            trainX.append(a.ravel())
            trainY.append(b.ravel())
        self.trainX = np.array(trainX)
        self.trainY = np.array(trainY)
        for a,b in testData:
            testX.append(a.ravel())
            testY.append(b)
        self.testX = np.array(testX)
        self.testY = np.array(testY)
        
        self.J = []
        self.TJ = []
        params0 = self.N.GetParams()
        
        options = {'maxiter':200, 'disp':True }
        res = optimize.minimize(self.CostFunctionWrapper, params0, 
                                jac=True, method='BFGS',
                                args=(self.trainX.T,self.trainY.T), 
                                options=options, callback=self.Callback)
        self.N.setParams(res.x)
        self.optimizationResults = res
'''

epochResults = []
learnList = [0.075]
batchList = [15]
layerList = [[784,35,15,20,10]]

trainData, testData = loadDataWrapper()
#net = Network([784,35,15,20,10])
#net.RMSprop(trainData[:5000], 10000, 15, beta=6.0, testData=testData[:1000])

# ~96% accurate at 60k train. ~95% accurate at 5k train 1k test
#net = Network([784,35,15,20,10])
#net.RMSprop(trainData[:60000], 1000, 15, 0.075, 0.9, testData[:10000])

for i in layerList:
    totalTrainingResults = []
    totalLearnResults = []
    for j in learnList:
        totalBatchResults = []
        for k in batchList:
            toAverage = []
            for l in range(0,1):
                print("Layers: %i x %i, Learn Rate: %f, MiniBatch Size: %i, "
                      "Iteration: %i" % (len(i)-2, i[1], j, k, l+1))
                start = time.time()
                net = testWrapper(Network, i)
                net.RMSprop(trainData[:60000], 500, k, j, testData=testData[:10000])
                stop = time.time()
                epochResults.append((stop-start)*1000.0)
                toAverage.append(list(epochResults))
                del epochResults[:]
                del net
            #toAverage = toAverage
            totalBatchResults.append(list(np.mean(toAverage, axis=0)))
            print("Time for 1000 Epochs (ms): %f" % (totalBatchResults[-1][-1]))
        totalLearnResults.append(totalBatchResults)
    totalTrainingResults.append(totalLearnResults)
    # Output to file
    with open("RMSLong.txt", "a+") as output:
        output.write(str(totalTrainingResults) + '\n')