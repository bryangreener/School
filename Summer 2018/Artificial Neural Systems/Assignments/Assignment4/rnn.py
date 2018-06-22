# -*- coding: utf-8 -*-
"""
Created on Sun Jun 10 13:32:58 2018

@author: Bryan
"""

from __future__ import print_function, division
import numpy as np
import tensorflow as tf
import matplotlib.pyplot as plt

numEpochs = 1
seriesLength = 50000
backpropLength = 15
stateSize = 4
numClasses = 2
batchSize = 5
numBatches = seriesLength // batchSize // backpropLength

batchXph = tf.placeholder(tf.float32, [batchSize, backpropLength])
batchYph = tf.placeholder(tf.int32, [batchSize, backpropLength])
initState = tf.placeholder(tf.float32, [batchSize, stateSize])
w1 = tf.Variable(np.random.rand(stateSize+1, stateSize), dtype=tf.float32)
b1 = tf.Variable(np.zeros((1,stateSize)), dtype=tf.float32)
w2 = tf.Variable(np.random.rand(stateSize, numClasses), dtype=tf.float32)
b2 = tf.Variable(np.zeros((1, numClasses)), dtype=tf.float32)

#unstack columns
inputSeries = tf.unstack(batchXph, axis=1)
labelSeries = tf.unstack(batchYph, axis=1)

#forward pass
currentState = initState
stateSeries = []
for currentInput in inputSeries:
    currentInput = tf.reshape(currentInput, [batchSize, 1])
    concat = tf.concat(1, [currentInput, currentState])
    nextState = tf.tanh(tf.matmul(concat, w1) + b1)
    stateSeries.append(nextState)
    currentState = nextState

#calculate loss
logitSeries = [tf.matmul(state, w2) + b2 for state in stateSeries]
predictionSeries = [tf.nn.softmax(logits) for logits in logitSeries]
losses = [tf.nn.sparse_softmax_cross_entropy_with_logits(logits, labels) for
          logits, labels in zip(logitSeries, labelSeries)]
totalLoss = tf.reduce_mean(losses)
trainStep = tf.train.AdagradOptimizer(0.3).minimize(totalLoss)

with tf.Session() as sess:
    sess.run(tf.initialize_all_variables())
    lossList = []
    for epochIdx in range(numEpochs):
        x,y = GenerateData()
        _currentState = np.zeros((batchSize, stateSize))
        print("New data, epoch", epochIdx)
        for batchIdx in range(numBatches):
            startIdx = batchIdx * backpropLength
            endIdx = startIdx + backpropLength
            batchX = x[:, startIdx, endIdx]
            batchY = y[:, startIdx, endIdx]
            _totalLoss, _trainStep, _currentState, _predictionSeries = sess.run(
                    [totalLoss, trainStep, currentState, predictionSeries],
                feed_dict={
                        batchXph:batchX,
                        batchYph:batchY,
                        initState:_currentState})
            lossList.append(_totalLoss)
            if batchIdx % 100 == 0:
                print("Step", batchIdx, "Loss", _totalLoss)
            
