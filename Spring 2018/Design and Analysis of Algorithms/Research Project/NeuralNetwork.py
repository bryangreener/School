# =========================================================================
# Feed Forward Neural Network with Backprop, RMSprop, and Nesterov Momentum
# Author:   Bryan Greener
# Date:     March-April 2018
# Class:    Design and Analysis of Algorithms
#
# This program is written as a research project for the class above.
#
# Included in the containing directory is a research paper which has
#   a list of references and intellectual/programming sources which were
#   used during the creation of this program. Many of the equations and
#   formulae used in this program are cited in the aforementioned report.
#
# The code in this program is the product of taking the concepts and base
#   structure exhibited in multiple sources and using the knowledge gained
#   from analyzing these sources and applying them to create a program
#   which suited my needs.
#
# The functions used to load MNIST data were provided in order to load
#   these data sets as they are saved in a custom format which cannot
#   be read from otherwise.
# MNIST Data Loader Source (multiple lines): 
# https://github.com/mnielsen/
#         neural-networks-and-deep-learning/blob/master/src/mnist_loader.py
# =========================================================================
import numpy as np
import random
import time
from mnist import MNIST # Custom import for loading MNIST data.

#### Functions to load MNIST Data
#    I added the lines noted below in order to normalize each
#    data set. This is easier done in these functions instead of
#    handling it later on.
def loadData():
    mndata = MNIST('./mnist-data')
    mndata.gz = True
    trainData = mndata.load_training()
    testData = mndata.load_testing()
    return trainData, testData
def loadDataWrapper():
    trd, ted = loadData()
    trainInputs = [np.reshape(x,(784,1)) for x in trd[0]]
    trainInputs = np.multiply((1.0/255.0),np.array(trainInputs)) #ADDED
    trainResults = [vectorizedResult(y) for y in trd[1]]
    trainData = list(zip(trainInputs, trainResults))
    testInputs = [np.reshape(x,(784,1)) for x in ted[0]]
    testInputs = np.multiply((1.0/255.0),np.array(testInputs)) #ADDED
    testData = list(zip(testInputs,ted[1]))
    return trainData,testData
def vectorizedResult(i):
    e = np.zeros((10,1))
    e[i] = 1.0
    return e
# ================================================================

#### Wrapper for passing in various training parameters
def testWrapper(func, args):
        return func(args)
# ================================================================

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

    #### Method that deletes the network object.
    #    Used during testing loops to prevent problems with
    #    residual data from prev object.
    def __del__(self):
        print("Deleted Network")

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
        for a,b in mb:
            x.append(a.ravel())
            y.append(b.ravel())
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
            self.RunTests(i, testData)
    
    #### Method that passes in each set of testing data to forward propagate.
    #    Compares observed and expected results then outputs number of
    #    correct observations.
    def RunTests(self, epoch, testData=None):
        #### If no test data is passed in then dont try to forward prop.
        if testData:
            # Number of testing inputs.
            nTest = len(np.array(testData))
            tempEval = self.evaluate(testData)
            # List used outside of class to output results to file.
            epochResults.append(tempEval)
            print("Epoch {0}: {1} / {2}".format(epoch, tempEval, nTest))
        #### If no test data passed in, just print that epoch finished.
        else:
            print("Epoch {0} complete".format(epoch))
    
    #### Helper method used by RunTEsts that sends all training data inputs
    #    to be forward propagated. Saves yhat returns in matrix.
    def evaluate(self, testData):
        res = [(np.argmax(self.FeedForward(x.T)),y) for x,y in testData]
        #### Send back the number of correct observations.
        return sum(int(x==y) for x,y in res)
# ================================================================


#### List storing results from each epoch as doubles.
epochResults = []

#### Hyperparameters for network.
#    Each list can be propagated with multiple network parameters.
##   learnList is a list of doubles that specify the learning rates of the
#      network. Lower values are typically better.
##   batchList is a list of integers that specify the number of items
#      per miniBatch. Values around 10-30 are typically better. 
##   layerList is a list of lists containing the number of nodes at each layer.
#      Each item in this list is a different layer. Different amounts of layers
#      are accepted.
##   trainSize is the number of training set items to send in to network.
#      Max value is 60,000.
##   testSize is the number of testing set items to send in to the network.
#      Max value is 10,000.
##   numEpochs is the number of full training iterations that the network
#    will perform.
#      No real best value for this as it depends on the behavior of
#      the network. For default network parameters, 100 epochs
#      is more than enough to max out accuracy and get good log data.
learnList = [0.075]
batchList = [15]
layerList = [[784,35,15,20,10]]
trainSize = 60000;
testSize = 10000;
numEpochs = 500;

#### Call function that reads in MNIST database objects into two lists.
trainData, testData = loadDataWrapper()

#### Below are some commented out examples of training the network using
#    default parameters to RMSprop method. These don't output logs.
#    ~97% accurate at 60k train 10k test. 
#    ~95% accurate at 5k train 1k test
#
#net = Network([784,35,15,20,10])
#net.RMSprop(trainData[:60000], testData = testData[:10000])
#net.RMSprop(trainData[:5000], testData = testData[:1000])

#### Training loops that iterate over each hyperparameter list item
#    then output the epoch results (acc/time) list to a txt file.
#    This needs to be updated to include gamma and beta values.
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
                start = time.time() # TIMER START
                net = testWrapper(Network, i)
                net.RMSprop(trainData[:trainSize], numEpochs, k, j, \
                            testData=testData[:testSize])
                stop = time.time()  # TIMER STOP
                # Append runtime (*1000 to convert to ms) to results.
                epochResults.append((stop-start)*1000.0)
                toAverage.append(list(epochResults))
                del epochResults[:] # Resolve ghosting data.
                del net # Resolve ghosting data.
            totalBatchResults.append(list(np.mean(toAverage, axis=0)))
            print("Time for 1000 Epochs (ms): %f" % \
                  (totalBatchResults[-1][-1]))
        totalLearnResults.append(totalBatchResults)
    totalTrainingResults.append(totalLearnResults)
    # Output to file
    with open("NNLog.txt", "a+") as output:
        output.write(str(totalTrainingResults) + '\n')