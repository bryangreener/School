'''
Code and information source:
    https://github.com/stephencwelch/Neural-Networks-Demystified/blob/master/partSix.py
'''

import numpy as np
from scipy import optimize # For BFGS Gradient Descent optimization
from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
from matplotlib import cm
from matplotlib.ticker import LinearLocator
    
class NeuralNetwork(object):
    Lambda = 0.00001
    def __init__(self, inputLayerSize,
                 hiddenLayer1Size, hiddenLayer2Size, outputLayerSize,
                 W1=[], W2=[], W3=[]):
        self.inputLayerSize = inputLayerSize
        self.outputLayerSize = outputLayerSize
        self.hiddenLayer1Size = hiddenLayer1Size
        self.hiddenLayer2Size = hiddenLayer2Size
        
        if not W1: # If no weight list passed in, randomize
            self.W1 = np.random.randn(self.inputLayerSize,
                                      self.hiddenLayer1Size)
            self.W2 = np.random.randn(self.hiddenLayer1Size,
                                      self.hiddenLayer2Size)
            self.W3 = np.random.randn(self.hiddenLayer2Size,
                                      self.outputLayerSize)
        else: # If passing in weight lists, set them
            self.W1 = W1
            self.W2 = W2
            self.W3 = W3
            
    def forward(self, X):
        #X is input layer, Z2 is hidden layer, W1 are weights from X to Z
        # a2 = f(z2) a2 is second activation layer at z2.
        #z3 = a2w2 z3 is activition of third layer
        #repeated for a3 and z4
        #yhat = f(z4) yhat is final estimate output
        self.z2 = np.dot(X, self.W1)
        self.a2 = self.sigmoid(self.z2)
        
        self.z3 = np.dot(self.a2, self.W2)
        self.a3 = self.sigmoid(self.z3)
        
        self.z4 = np.dot(self.a3, self.W3)
        yhat = self.sigmoid(self.z4)
        return yhat

    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))
    
    def sigmoidPrime(self, z):
        return np.exp(-z)/((1.0+np.exp(-z))**2)
    
    #unoptimized cost function J = sum((1/2)(y-yhat))
    #dJ/d(W2) = (sum((1/2)(y-yhat))) / d(W2)
    #We perform the chain rule to expand on this derivative
    def costFunction(self, X, y):
       #compute cost for given X,y use weights already stored in class
       self.yhat = self.forward(X)
       # The next line helps prevent overfitting model by making larger
       # magnitudes of weights cost more.
       # It is split up to make function clearer
       s0 = 0.5*sum((y-self.yhat)**2)/X.shape[0]
       s1 = sum(sum(self.W1**2))
       s2 = sum(sum(self.W2**2))
       s3 = sum(sum(self.W3**2))
       J = s0+(self.Lambda/2)*(s1+s2+s3)
       return J
   
    def costFunctionPrime(self, X, y):
        self.yhat = self.forward(X)
        
        delta4 = np.multiply(-(y-self.yhat), self.sigmoidPrime(self.z4))
        
        dJdW3 = (np.dot(self.a3.T, delta4) + self.Lambda*self.W3)/X.shape[0]
        delta3 = np.dot(delta4, self.W3.T)*self.sigmoidPrime(self.z3)
       
        dJdW2 = (np.dot(self.a2.T, delta3) + self.Lambda*self.W2)/X.shape[0]
        delta2 = np.dot(delta3, self.W2.T)*self.sigmoidPrime(self.z2)
        
        dJdW1 = (np.dot(X.T, delta2) + self.Lambda*self.W1)/X.shape[0]
        return dJdW1, dJdW2, dJdW3
    
    def getParams(self):
        params = np.concatenate((self.W1.ravel(), 
                                 self.W2.ravel(),
                                 self.W3.ravel()))
        return params
    
    def setParams(self, params):
        W1_start = 0
        W1_end = self.inputLayerSize * self.hiddenLayer1Size
        self.W1 = np.reshape(params[W1_start:W1_end], 
                             (self.inputLayerSize, self.hiddenLayer1Size))
        
        W2_end = W1_end + self.hiddenLayer1Size * self.hiddenLayer2Size
        self.W2 = np.reshape(params[W1_end:W2_end],
                             (self.hiddenLayer1Size, self.hiddenLayer2Size))
        
        W3_end = W2_end + self.hiddenLayer2Size * self.outputLayerSize
        self.W3 = np.reshape(params[W2_end:W3_end],
                             (self.hiddenLayer2Size, self.outputLayerSize))
        
    def computeGradients(self, X, y):
        dJdW1, dJdW2, dJdW3 = self.costFunctionPrime(X, y)
        return np.concatenate((dJdW1.ravel(), dJdW2.ravel(), dJdW3.ravel()))

#Used for testing only. Not used in Network Training
def computeNumericalGradients(N, X, y):
    paramsInitial = N.getParams()
    numgrad = np.zeros(paramsInitial.shape)
    perturb = np.zeros(paramsInitial.shape)
    e = 1e-4
    for p in range(len(paramsInitial)):
        perturb[p] = e
        N.setParams(paramsInitial+perturb)
        loss2 = N.costFunction(X, y)
        N.setParams(paramsInitial - perturb)
        loss1 = N.costFunction(X, y)
        # Compute numerical gradient
        numgrad[p] = (loss2-loss1) / (2*e)
        # Return value changed back to zero
        perturb[p]
    # return params to original values
    N.setParams(paramsInitial)
    return numgrad
            
# test errors with norm(grad-numgrad)/norm(grad+numgrad)
# Error should be <= 1.0e-8
    
class trainer(object):
    def __init__(self, N):
        self.N = N
        
    def costFunctionWrapper(self, params, X, y):
        self.N.setParams(params)
        cost = self.N.costFunction(X, y)
        grad = self.N.computeGradients(X, y)
        return cost, grad
    
    #Callback lets us track cost function value as we train network
    def callbackF(self, params):
        self.N.setParams(params)
        self.J.append(self.N.costFunction(self.X, self.y))
        self.testJ.append(self.N.costFunction(self.testX, self.testY))
        
    def train(self, trainX, trainY, testX, testY):
        #Make interval variable for callback function
        self.X = trainX
        self.y = trainY
        self.testX = testX
        self.testY = testY
        
        #Make empty list to store costs:
        self.J = []
        self.testJ = []
        
        # Set initial parameters
        params0 = self.N.getParams()
        
        options = {'maxiter':200, 'disp':True }
        ''' We set jac=True (jacobian parameter) true since we compute
        gradient in our neural network class.'''
        _res = optimize.minimize(self.costFunctionWrapper, params0, 
                                 jac=True, method='BFGS', 
                                 args=(trainX, trainY), options=options, 
                                 callback=self.callbackF)
        #After net is trained, replace random params with trained params
        self.N.setParams(_res.x)
        self.optimizationResults = _res 

#### Import MNIST Data
from mnist import MNIST
mndata = MNIST('./mnist-data')
mndata.gz = True

#### Load data from MNIST db into variables
trainX, trainY = mndata.load_training()
testX, testY = mndata.load_testing()

trainX = np.array(np.multiply((1.0/255.0), trainX), dtype=float)
trainY = np.array(list(map(lambda x: [x], trainY)), dtype=float)
testX = np.array(np.multiply((1.0/255.0), testX), dtype=float)
testY = np.array(list(map(lambda x: [x], testY)), dtype=float)

#### Normalize Data
#trainX = trainX/np.amax(trainX, axis=0)
trainY = trainY/max(trainY)
#testX = testX/np.amax(testX, axis=0)
testY = testY/max(testY)

#### Initialize Network
NN = NeuralNetwork(784, 16, 16, 10)
T = trainer(NN)
T.train(trainX[:100], trainY[:100], testX[:10], testY[:10])

#### Plot cost vs iterations
plt.plot(T.J)
plt.plot(T.testJ)
plt.grid(1)
plt.xlabel('Iterations')
plt.ylabel('Cost')
plt.show()





























