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
    def __init__(self):
        self.inputLayerSize = 2
        self.outputLayerSize = 1
        self.hiddenLayerSize = 16
        
        self.W1 = np.random.randn(self.inputLayerSize, self.hiddenLayerSize)
        self.W2 = np.random.randn(self.hiddenLayerSize, self.outputLayerSize)
    def forward(self, X):
        #X is input layer, Z2 is hidden layer, W1 are weights from X to Z
        # a2 = f(z2) a2 is second activation layer at z2.
        #z3 = a2w2 z3 is activition of third layer
        #yhat = f(z3) yhat is final estimate output
        self.z2 = np.dot(X, self.W1)
        self.a2 = self.sigmoid(self.z2)
        self.z3 = np.dot(self.a2, self.W2)
        yhat = self.sigmoid(self.z3)
        return yhat

    def sigmoid(self, z):
        return 1/(1+np.exp(-z))
    
    def sigmoidPrime(self, z):
        return np.exp(-z)/((1+np.exp(-z))**2)
#unoptimized cost function J = sum((1/2)(y-yhat))
#dJ/d(W2) = (sum((1/2)(y-yhat))) / d(W2)
#We perform the chain rule to expand on this derivative
    def costFunction(self, X, y):
       #compute cost for given X,y use weights already stored in class
       self.yhat = self.forward(X)
       # The next line helps prevent overfitting model by making larger
       # magnitudes of weights cost more.
       #J = 0.5*sum((y-self.yhat)**2)/X.shape[0] + (self.Lambda/2)*(sum(self.W1**2)+sum(self.W2**2))
       J = 0.5*sum((y-self.yhat)**2)/X.shape[0] + (self.Lambda/2)*(sum(sum(self.W1**2))+sum(sum(self.W2**2)))
       #J = (0.5*np.sum((y - self.yhat)**2) + (self.Lambda/2) * (np.sum(self.W1**2) + np.sum(self.W2**2))) /X.shape[0]
       return J
   
    def costFunctionPrime(self, X, y):
        self.yhat = self.forward(X)
        delta3 = np.multiply(-(y-self.yhat), self.sigmoidPrime(self.z3))
        ''' Add gradient of regularization term: 
            Lambda is a hyperparameter. Higher values of lambda
            incur higher penalties for models of higher complexity. '''
        dJdW2 = (np.dot(self.a2.T, delta3) + self.Lambda*self.W2)/X.shape[0]
        
        delta2 = np.dot(delta3, self.W2.T)*self.sigmoidPrime(self.z2)
        ''' Add gradient of regularization term: '''
        dJdW1 = (np.dot(X.T, delta2) + self.Lambda*self.W1)/X.shape[0]
        return dJdW1, dJdW2
    
    def getParams(self):
        params = np.concatenate((self.W1.ravel(), self.W2.ravel()))
        return params
    def setParams(self, params):
        W1_start = 0
        W1_end = self.hiddenLayerSize * self.inputLayerSize
        self.W1 = np.reshape(params[W1_start:W1_end], 
                             (self.inputLayerSize, self.hiddenLayerSize))
        # Add more layers to net by repeating this process for number of layers
        W2_end = W1_end + self.hiddenLayerSize * self.outputLayerSize
        self.W2 = np.reshape(params[W1_end:W2_end],
                             (self.hiddenLayerSize, self.outputLayerSize))
    def computeGradients(self, X, y):
        dJdW1, dJdW2 = self.costFunctionPrime(X, y)
        return np.concatenate((dJdW1.ravel(), dJdW2.ravel()))
    
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
        
        options = {'mixiter':200, 'disp':True }
        ''' We set jac=True (jacobian parameter) true since we compute
        gradient in our neural network class.'''
        _res = optimize.minimize(self.costFunctionWrapper, params0, 
                                 jac=True, method='BFGS', 
                                 args=(trainX, trainY), options=options, 
                                 callback=self.callbackF)
        #After net is trained, replace random params with trained params
        self.N.setParams(_res.x)
        self.optimizationResults = _res
    
    '''
    For testing, do as follows:
        NN = NeuralNetwork()
        T = trainer(NN)
        T.tain(X,y)
        plot(T.J)
        grid(1)
        ylabel('Cost')
        xlabel('Iterations')
    This plots a graph of the cost in terms of iterations through training.
    We should get a monotonically decreating graph.
    Number of function evaluations to find a solution should be <= 100
    
    Next
        NN.costFunctionPrime(X, y)
    This prints our matrices. We should get very small values ( < 1.0e-6 )
    
    To verify predictions:
        NN.forward(X)
    This will give the values of forward prop.
    Then type
        y
    This gives our estimates. These two matices should be very close.
    '''
    
    
    
def tests():
    # Test network nfor various combinations of sleep/study
    hoursSleep = np.linspace(0, 10, 100)
    hoursStudy = np.linspace(0, 5, 100)
    
    #Normalize data( same way training data was normalized)
    hoursSleepNorm = hoursSleep/10.
    hoursStudyNorm = hoursStudy/5.
    
    #Create 2-d versions of input for plotting
    a, b = np.meshgrid(hoursSleepNorm, hoursStudyNorm)
    
    #Join into a single input matrix
    allInputs = np.zeros((a.size, 2))
    allInputs[:, 0] = a.ravel()
    allInputs[:, 1] = b.ravel()
    
    allOutputs = NN.forward(allInputs)

    #Make contour plot
    yy = np.dot(hoursStudy.reshape(100, 1), np.ones((1,100)))
    xx = np.dot(hoursSleep.reshape(100,1), np.ones((1, 100))).T
    CS = plt.contour(xx, yy, 100*allOutputs.reshape(100,100))
    plt.clabel(CS, inline=1, fontsize=10)
    plt.xlabel('Var1')
    plt.ylabel('Var2')    
    #Make 3d plot
    #matplotlib qt
    fig = plt.figure()
    ax = fig.gca(projection = '3d')
    surf = ax.plot_surface(xx, yy, 100*allOutputs.reshape(100,100),cmap=cm.jet)
    ax.set_xlabel('Var1')
    ax.set_ylabel('Var2')
    ax.set_zlabel('Freq')
    ax.mouse_init()
    plt.show()
    
''' MAIN '''

NN = NeuralNetwork()
# X= (hours sleep, hours study), y = score
#Training Data
import csv
crimData = csv.reader(open('crimtab.csv', 'rt'), delimiter=",")
co1, co2, co3, co4, co5, co6 = [], [], [], [], [], []
for row in crimData:
    co1.append(row[0])
    co2.append(row[1])
    co3.append(row[2])
    #co4.append(row[3])
    #co5.append(row[4])
    #co6.append(row[5])
    
co1.pop(0)
co2.pop(0)
co3.pop(0)
#co4.pop(0)
#co5.pop(0)
#co6.pop(0)

c1 = [float(i) for i in co1[:799]]
tc1 = [float(i) for i in co1[800:]]
c2 = [float(i) for i in co2[:799]]
tc2 = [float(i) for i in co2[800:]]
c3 = [float(i) for i in co3[:799]]
tc3 = [float(i) for i in co3[800:]]
'''
c4 = [float(i) for i in co4[:1249]]
tc4 = [float(i) for i in co4[1250:]]
c5 = [float(i) for i in co5[:1249]]
tc5 = [float(i) for i in co5[1250:]]
c6 = [float(i) for i in co6[:1249]]
tc6 = [float(i) for i in co6[1250:]]
'''
#Training Data
trainX = np.array(list(zip(c1,c2)), dtype=float)
trainY = np.array(list(map(lambda x: [x], c3)), dtype=float)
#trainX = np.array(([1,1],[8,1],[10,2],[6,3],[8,5],[7.5,4],[8,10],[8,8],[10,100]), dtype=float)
#trainY = np.array(([50],[65],[70],[70],[80],[80],[95],[90],[5]), dtype=float)

#Testing Data
testX = np.array(list(zip(tc1,tc2)), dtype=float)
testY = np.array(list(map(lambda x: [x], tc3)), dtype=float)
#testX = np.array(([4,5.5],[4.5,1],[9,2.5],[6,2]),dtype=float)
#testY = np.array(([70],[89],[75],[82]),dtype=float)

#Normalize
trainX = trainX/np.amax(trainX, axis=0)
trainY = trainY/max(c3)
testX = testX/np.amax(testX, axis=0)
testY = testY/max(tc3)

#Train NN with new data
NN = NeuralNetwork()
T = trainer(NN)
T.train(trainX, trainY, testX, testY)

#Plot costs during training:
#You can see where overfitting begins by observing the T.testJ line

plt.plot(T.J)
plt.plot(T.testJ)
plt.grid(1)
plt.xlabel('Iterations')
plt.ylabel('Cost')
plt.show()


# Test network nfor various combinations of sleep/study
testa = np.linspace(0, 10, 100)
testb = np.linspace(0, 5, 100)
    
#Normalize data( same way training data was normalized)
testan = testa/10.
testbn = testb/5.

#Create 2-d versions of input for plotting
#testa = np.array([x[0] for x in testX])
#testb = np.array([x[1] for x in testX])
a, b = np.meshgrid(testan, testbn)
    
#Join into a single input matrix
allInputs = np.zeros((a.size, 2))
allInputs[:, 0] = a.ravel()
allInputs[:, 1] = b.ravel()
    
allOutputs = NN.forward(allInputs)

#Make contour plot
fig = plt.figure()
yy = np.dot(testa.reshape(100, 1), np.ones((1,100)))
xx = np.dot(testb.reshape(100, 1), np.ones((1, 100))).T
CS = plt.contour(xx, yy, 100*allOutputs.reshape(100,100))

plt.clabel(CS, inline=1, fontsize=10)
plt.xlabel('Var1')
plt.ylabel('Var2')
plt.show()    
    #Make 3d plot
    #matplotlib qt
fig = plt.figure()
ax = fig.gca(projection = '3d')
surf = ax.plot_surface(xx, yy, 100*allOutputs.reshape(100,100),cmap=cm.jet)
ax.set_xlabel('Var1')
ax.set_ylabel('Var2')
ax.set_zlabel('Freq')
ax.mouse_init()
plt.show()

'''  
# Make sure nothing broke. Do numerical gradient checking
numgrad = computeNumericalGradient(NN, X, y)
grad = NN.computeGradients(X, y)

#Should be less than 1.0e-8
norm(grad-numgrad)/norm(grad+numgrad)

T = trainer(NN)
T.train(trainX, trainY, testX, testY)

tests()

#Visualize costs during training
# The two lines should be very close
plot(T.J)
plot(T.testJ)
grid(1)
xlabel('Iterations')
ylabel('Cost')
legend('Training','Testing')
# To further reduce overfitting we can increase lambda even further.
'''



'''
#plot projections of new data
fig = figure(0, (8,3))
subplot(1,2,1)
scatter(X[:,0], y)
grid(1)
xlabel('Hours Sleeping')
ylabel('Hours Studying')

subplot(1,2,2)
scatter(X[:,1], y)
grid(1)
xlabel('Hours Studying')
ylabel('Test Score')
#normalize
X = X/np.amax(X, axis = 0)
y = y/100 #max test score is 100

#Train network with new data
T = trainer(NN)
T.train(X,y)

#Plot cost during training
plot(T.J)
grid(1)
xlabel('Iterations')
ylabel('Cost')

tests()
#   IMPORTANT: These results will appear to be overfitting.
#   We need to test if that is the case or not.
#   
#   To do this, we split our data into two sets: training and testing 
    
#Training Data
trainX = np.array(([3,5],[5,1],[10,2],[6,1.5]), dtype=float)
trainY = np.array(([75],[82],[93],[70]), dtype=float)

#Testing Data
testX = np.array(([4,5.5],[4.5,1],[9,2.5],[6,2]),dtype=float)
testY = np.array(([70],[89],[75]),dtype=float)

#Normalize
trainX = trainX/np.amax(trainX, axis=0)
trainY = trainY/100.
testX = testX/np.amax(testX, axis=0)
testY = testY/100.

#Train NN with new data
NN = NeuralNetwork()
T = trainer(NN)
T.train(tainX, trainY, testX, testY)

#Plot costs during training:
#You can see where overfitting begins by observing the T.testJ line
plot(T.J)
plot(T.testJ)
grid(1)
xlabel('Iterations')
ylabel('Cost')

#   Good ratio is 10x number of examples as degrees of freedom in model
#   Since our model uses 9 weights, we need 90 observations.
#   Otherwise we can use regularization.
'''
    
    
    
    