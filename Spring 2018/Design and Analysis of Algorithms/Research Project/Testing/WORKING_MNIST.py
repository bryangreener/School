import random
import numpy as np


alpha = 255
class NeuralNetwork(object): 
    def __init__(self, sizes):
        self.numLayers = len(sizes)
        self.sizes = sizes
        self.delta_past = [[np.array([np.zeros(y)]).T for x in range(4)] for y in sizes[1:]]
        self.delta_rho = [0.05,0.15,0.3,0.5]
        self.biases = [np.random.randn(y, 1) for y in sizes[1:]]
        self.weights = [np.random.randn(y, x) for x, y in zip(sizes[:-1], sizes[1:])]
    def __del__(self):
        print("Deleted NeuralNetwork object")
    
    def forward(self, a):
        for b, w in zip(self.biases, self.weights):
            a = sigmoid(np.dot(w, a) + b)
        return a
    #Stochastic gradient descent
    def SGD(self, training_data, epochs, mini_batch_size, eta, test_data=None):
        """Train the neural network using mini-batch stochastic
        gradient descent.  The "training_data" is a list of tuples
        "(x, y)" representing the training inputs and the desired
        outputs.  The other non-optional parameters are
        self-explanatory.  If "test_data" is provided then the
        network will be evaluated against the test data after each
        epoch, and partial progress printed out.  This is useful for
        tracking progress, but slows things down substantially."""
        # Alpha hyperparameter added for modified sigmoid function
        self.alhpa = alpha
        if test_data: n_test = sum([1 for _ in test_data]) #len(test_data)
        n = sum([1 for _ in training_data]) #len(training_data)
        for j in range(epochs):
            random.shuffle(training_data)
            mini_batches = [
                    training_data[k:k+mini_batch_size]
                    for k in range(0, n, mini_batch_size)]
            for mini_batch in mini_batches:
                self.update_mini_batch(mini_batch, eta)
            if test_data:
                tempEval = self.evaluate(test_data)
                epochResults.append(tempEval)
                print("Epoch {0}: {1} / {2}".format(j+1, tempEval, n_test))
            else:
                print("Epoch {0} complete".format(j))
                
    def update_mini_batch(self, mini_batch, eta):
        """Update the network's weights and biases by applying
        gradient descent using backpropagation to a single mini batch.
        The "mini_batch" is a list of tuples "(x, y)", and "eta"
        is the learning rate."""
        nabla_b = [np.zeros(b.shape) for b in self.biases]
        nabla_w = [np.zeros(w.shape) for w in self.weights]
        for x, y in mini_batch:
            delta_nabla_b, delta_nabla_w = self.backprop(x, y)
            nabla_b = [nb+dnb for nb, dnb in zip(nabla_b, delta_nabla_b)]
            nabla_w = [nw+dnw for nw, dnw in zip(nabla_w, delta_nabla_w)]
        self.weights = [w-(eta/len(mini_batch))*nw
                        for w, nw in zip(self.weights, nabla_w)]
        self.biases = [b-(eta/len(mini_batch))*nb
                       for b, nb in zip(self.biases, nabla_b)]

    def backprop(self, x, y):
        nabla_b = [np.zeros(b.shape) for b in self.biases]
        nabla_w = [np.zeros(w.shape) for w in self.weights]
        
        activation = x
        activations = [x]
        zs = []
        for b, w in zip(self.biases, self.weights):
            z = np.dot(w, activation) + b
            zs.append(z)
            activation = sigmoid(z)
            activations.append(activation)
            
        #backward pass
        delta = self.cost_derivative(activations[-1],y)*sigmoid_prime(zs[-1])
        #delta = np.array([self.push_delta(delta, -1)]).T
        
        nabla_b[-1] = delta
        #nabla_b[-1] = delta[0]
        nabla_w[-1] = np.dot(delta, activations[-2].T)
        #nabla_w[-1] = np.dot(delta[0], activations[-2].T)
        for l in range(2, self.numLayers):
            z = zs[-l]
            sp = sigmoid_prime(z)
            delta = np.dot(self.weights[-l+1].T, delta) * sp
            #delta = np.dot(self.weights[-l+1].T, delta[0]) * sp
            #delta = np.array([self.push_delta(delta, 4-l)]).T
            nabla_b[-l] = delta
            #nabla_b[-l] = delta[0]
            nabla_w[-l] = np.dot(delta, activations[-l-1].T)
            #nabla_w[-l] = np.dot(delta[0], activations[-l-1].T)
        return(nabla_b, nabla_w)
    
    def evaluate(self, test_data):
        test_results = [(np.argmax(self.forward(x)), y)
                        for (x,y) in test_data ]
        return sum(int(x==y) for (x,y) in test_results)
    
    def cost_derivative(self, output_activations, y):
        return(output_activations-y)
    # Store most recent 4 delta values.
    # Multiply by sclar then return to overwrite passed in delta.
    def push_delta(self, delta, layer):
        if layer == -5: layer = 0
        self.delta_past[layer][:-1] = self.delta_past[layer][1:]; 
        self.delta_past[layer][-1] = delta
        td = np.array([np.multiply(np.array(
                self.delta_past[layer]).T[0],self.delta_rho).T.sum(axis=0)]).T
        self.delta_past[layer][-1] = td
        return(td)
            
            
        
        
#### Modifying Sigmoid with alpha slope parameter
    # This alpha param should be set to the max value of any input.
    # In this case, use 255
def sigmoid(z):
    return 1.0/(1.0+np.exp(-z))
    #return 1.0/(1.0+np.exp(-alpha*(lambda x: (1.0/x) if x >= 1 else x, z)))
def sigmoid_prime(z):
    return(sigmoid(z)*(1-sigmoid(z)))
    
    
# MAIN

from mnist import MNIST
def load_data():
    mndata = MNIST('../mnist-data')
    mndata.gz = True
    training_data = mndata.load_training()
    test_data = mndata.load_testing()
    return (training_data, test_data)
def load_data_wrapper():
    tr_d, te_d = load_data()
    training_inputs = [np.reshape(x, (784, 1)) for x in tr_d[0]]
    training_inputs = np.multiply((1.0/255.0), np.array(training_inputs))
    training_results = [vectorized_result(y) for y in tr_d[1]]
    training_data = list(zip(training_inputs, training_results))
    test_inputs = [np.reshape(x, (784, 1)) for x in te_d[0]]
    test_inputs = np.multiply((1.0/255.0), np.array(test_inputs))
    test_data = list(zip(test_inputs, te_d[1]))
    return (training_data, test_data)
def vectorized_result(j):
    """Return a 10-dimensional unit vector with a 1.0 in the jth
    position and zeroes elsewhere.  This is used to convert a digit
    (0...9) into a corresponding desired output from the neural
    network."""
    e = np.zeros((10, 1))
    e[j] = 1.0
    return e

        
#### MAIN
training_data, test_data = load_data_wrapper()
training_data = list(training_data)

#### Lists for gathering training data
'''
Base training inputs:
    Layers: 784, 10, 10
    SGD Inputs: 30, 10, 3.0
Variance:
    Layers: 784, *, ..., *, 10
    SGD Inputs: 30, *, *
Input and output layers must always be 784 and 10 due to the format
of our data.
Number of epochs will stay static at 30 since it gives a good
amount of time for the training to actually take effect.
'''


#net = NeuralNetwork([784, 25, 15, 10])
#net.SGD(training_data[:1000], 100, 15, 2, test_data=list(test_data)[:100])


def testWrapper(func, args):
    return func(args)
import time
#import csv


learnList = [1.0]
batchList = [15]
layerList = [[784,35,15,20,10]]
#### Initialize network and iterate through input combos
#net = NeuralNetwork([784, 16, 10])
epochResults = []
for i in layerList:
    totalTrainingResults = []
    totalLearnResults = []
    for j in learnList:
        totalBatchResults = []
        for k in batchList:
            toAverage = []
            for l in range(0,5):
                print("Layers: %i x %i, Learn Rate: %f, MiniBatch Size: %i, "
                      "Iteration: %i" % (len(i)-2, i[1], j, k, l+1))
                start = time.time()
                net = testWrapper(NeuralNetwork, i)
                net.SGD(training_data[:5000], 1000, k, j, test_data=test_data[:1000])
                stop = time.time()
                epochResults.append((stop-start)*1000.0)
                toAverage.append(list(epochResults))
                del epochResults[:]
                del net
            #toAverage = toAverage
            totalBatchResults.append(list(np.mean(toAverage, axis=0)))
            print("Time for 10 Epochs (ms): %f" % (totalBatchResults[-1][-1]))
        totalLearnResults.append(totalBatchResults)
    totalTrainingResults.append(totalLearnResults)
    # Output to file
    with open("NewSigmoidTest.txt", "a+") as output:
        output.write(str(totalTrainingResults) + '\n')


















