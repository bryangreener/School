import numpy as np
import random
import math

class Network():
    def __init__(self, inSize, outSize, eta):
        # Initialize weight array with input size
        self.weights = np.random.randn(inSize, outSize)
        self.eta = eta
    
    # Sigmoid function used to bring values between 0 and 1.
    def sigmoid(self, z):
        return 1.0/(1.0+np.exp(-z))
    
    # ReLU function for testing.
    def relu(self, z):
        return z * (z > 0.0)
    
    def train(self, data, epochs):
        random.shuffle(data)
        size = math.floor(data.shape[0] * 0.8) #80% of data used for training
        trainData = np.array([data[:size]])  # 80%
        testData = np.array([data[size+1:]]) # 20%
        
        for e in range(epochs):
            # Shuffle training data for consistency
            random.shuffle(trainData)
            for d in trainData[0]:
                # Feed forward
                yhat = self.sigmoid(np.dot(d[:-1], self.weights))
                # Calculate error
                cost = d[-1] - yhat
                # Update weights for next time step
                self.weights += self.eta * np.multiply(cost, np.array([d[:-1]]).T)
                
            correct = 0
            for d in testData[0]:
                yhat = np.dot(d[:-1], self.weights)
                if (yhat > mean and d[-1] > mean) or \
                    (yhat < mean and d[-1] < mean):
                    correct += 1
            print("Epoch: {0}  {1}/{2}".format(e, correct, testData[0].shape[0]))


# Read csv as strings excluding header
inputData = np.genfromtxt('./bh.csv', delimiter=',', dtype="|U5")[1:]
# Remove invalid chars that prevent float conversion
for row in range(inputData.shape[0]):
    for item in range(inputData.shape[1]):
        inputData[row][item] = inputData[row][item].replace('"','')

# Convert to floats
data = inputData.astype(float)
# Normalize data and find mean value of result column
dNorm = data / data.max(axis=0)
mean = dNorm.mean(axis=0)[-1]

# net takes input/output layer sizes and learning rate hyperparameter.
# use 0.01 for learn rate.
net = Network(13, 1, 0.01)
net.train(dNorm, 100000)
