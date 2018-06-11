from __future__ import print_function
from __future__ import division
import numpy as np
import tensorflow as tf
import matplotlib.pyplot as plt


# Generate training data
def GenerateData():
	x = np.array(np.random.choice(2, seriesLength, p=[0.5,0.5]))
	y = np.roll(x, echoStep)
	y[0:echoStep] = 0
	x = x.reshape((batchSize, -1))
	y = y.reshape((batchSize, -1))
	return(x,y)

def Plot(lossList, predictionsSeries, batchX, batchY):
	plt.subplot(2, 3, 1)
	plt.cla()
	plt.plot(lossList)
	for batchSeriesIdx in range(5):
		oneHotOutputSeries = np.array(predictionsSeries)[:, batchSeriesIdx, :]
		singleOutputSeries = np.array([(1 if out[0] < 0.5 else 0) for out in oneHotOutputSeries])
		plt.subplot(2,3,batchSeriesIdx+2)
		plt.cla()
		plt.axis([0, truncBackpropLength, 0, 2])
		leftOffset = range(truncBackpropLength)
		plt.bar(leftOffset, batchX[batchSeriesIdx, :], width=1, color="blue")
		plt.bar(leftOffset, batchY[batchSeriesIdx, :] * 0.5, width=1, color="red")
		plt.bar(leftOffset, singleOutputSeries * 0.3, width=1, color="green")
	plt.draw()
	plt.pause(0.0001)

epochs = 100
seriesLength = 50000
truncBackpropLength = 15
stateSize = 4
numClasses = 2
echoStep = 3
batchSize = 5
numBatches = seriesLength // batchSize // truncBackpropLength

batchXPlaceholder = tf.placeholder(tf.float32, [batchSize, truncBackpropLength])
batchYPlaceholder = tf.placeholder(tf.int32, [batchSize, truncBackpropLength])
initState = tf.placeholder(tf.float32, [batchSize, stateSize])

W = tf.Variable(np.random.rand(stateSize+1, stateSize), dtype=tf.float32)
b = tf.Variable(np.zeros((1, stateSize)), dtype=tf.float32)
W2 = tf.Variable(np.random.rand(stateSize, numClasses), dtype=tf.float32)
b2 = tf.Variable(np.zeros((1, numClasses)), dtype=tf.float32)

#unpack columns
inputsSeries = tf.unstack(batchXPlaceholder, axis=1)
labelsSeries = tf.unstack(batchYPlaceholder, axis=1)

#forward pass
currentState = initState
statesSeries = []
for currentInput in inputsSeries:
	currentInput = tf.reshape(currentInput, [batchSize, 1])
	inputAndStateConcatenated = tf.concat(1, [currentInput, currentState])
	nextState = tf.tan(tf.matmul(inputAndStateConcatenated, W) + b)
	statesSeries.append(nextState)
	currentState = nextState

logitsSeries = [tf.matmul(state, W2) + b2 for state in statesSeries]
predictionsSeries = [tf.nn.softmax(logits) for logits in logitsSeries]
losses = [tf.nn.sparse_softmax_cross_entropy_with_logits(logits, labels) for logits,labels in zip(logitsSeries,labelsSeries)]
totalLoss = tf.reduce_mean(losses)
trainStep = tf.train.AdagradOptimizer(0.3).minimize(totalLoss)

with tf.Session as sess:
	sess.run(tf.initialize_all_variables())
	plt.ion()
	plt.figure()
	plt.show()
	lossList = []
	for epochIdx in range(epochs):
		x,y = GenerateData()
		_currentState = np.zeros((batchSize, stateSize))
		print("New data, epoch", epochIdx)
		for batchIdx in range(numBatches):
			startIdx = batchIdx * truncBackpropLength
			endIdx = startIdx + truncBackpropLength
			batchX = x[:, startIdx, endIdx]
			batchY = y[:, startIdx, endIdx]
			_totalLoss, _trainStep, _currentState, _predictionsSeries = sess.run(
				[totalLoss, trainStep, currentState, predictionsSeries],
				feed_dict={
					batchXPlaceholder:batchX,
					batchYPlaceholder:batchY,
					initState:_currentState
				})
			lossList.append(_totalLoss)
			if batchIdx%100 == 0:
				print("Step", batchIdx, "Loss", _totalLoss)
				plt.plot(lossList, _predictionsSeries, batchX, batchY)

plt.ioff()
plt.show()
