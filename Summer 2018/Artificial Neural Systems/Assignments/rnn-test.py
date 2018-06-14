from __future__ import print_function
import tensorflow as tf
from tensorflow.contrib import rnn
import numpy as np
import collections
import random
import time

def elapsed(sec):
	if sec < 60:
		return str(sec) + " sec"
	elif sec < (60*60):
		return str(sec/60) + " min"
	else:
		return str(sec/(60*60)) + " hr"

def ReadData(fname):
	with open(fname) as f:
		content = f.readlines()
	content = [x.strip() for x in content]
	content = [word for i in range(len(content)) for word in content[i].split()]
	content = np.array(content)
	return content

def BuildDataset(words):
	count = collections.Counter(words).most_common()
	dictionary = dict()
	for word, _ in count:
		dictionary[word] = len(dictionary)
	reverseDictionary = dict(zip(dictionary.values(), dictionary.keys()))
	return dictionary, reverseDictionary

def RNN(x, weights, biases):
	x = tf.reshape(x, [-1, n_input])
	x = tf.split(x, n_input, 1)
	lstm = rnn.BasicLSTMCell(n_hidden)
	outputs, states = rnn.static_rnn(lstm, x, dtype=tf.float32)
	return tf.matmul(outputs[-1], weights['out']) + biases['out']

#time start
startTime = time.time()
#logging output
logsPath = '/tmp/tensorflow/rnn_words'
writer = tf.summary.FileWriter(logsPath)
# text file with words for training
trainingFile = 'rev.txt'
trainingData = ReadData(trainingFile)
print('Loaded training data...')
# build dataset
dictionary, reverseDictionary = BuildDataset(trainingData)
#initialize variables
vocabSize = len(dictionary)
eta = 0.01
iters = 5000
displayStep = 1000
n_input = 3
# number of hidden units in RNN cell
n_hidden = 5000
# tf graph input
x = tf.placeholder('float', [None, n_input, 1])
y = tf.placeholder('float', [None, vocabSize])
# weights and biases for rnn output node
weights = {'out': tf.Variable(tf.random_normal([n_hidden, vocabSize]))}
biases = {'out': tf.Variable(tf.random_normal([vocabSize]))}

pred = RNN(x, weights, biases)
# loss and optimizer
cost = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(logits=pred, labels=y))
optimizer = tf.train.RMSPropOptimizer(learning_rate=eta).minimize(cost)
#model eval
correctPred = tf.equal(tf.argmax(pred, 1), tf.argmax(y, 1))
accuracy = tf.reduce_mean(tf.cast(correctPred, tf.float32))
#initialize variables
init = tf.global_variables_initializer()

with tf.Session() as sess:
	sess.run(init)
	step = 0
	offset = random.randint(0, n_input+1)
	endOffset = n_input + 1
	accTotal = 0
	lossTotal = 0
	writer.add_graph(sess.graph)
	while step < iters:
		if offset > (len(trainingData)-endOffset):
			offset = random.randint(0, n_input + 1)
		symbolsInKeys = [ [dictionary[ str(trainingData[i])]] for i in range(offset, offset+n_input)]
		symbolsInKeys = np.reshape(np.array(symbolsInKeys), [-1, n_input, 1])
		symbolsOutOnehot = np.zeros([vocabSize], dtype=float)
		symbolsOutOnehot[dictionary[str(trainingData[offset+n_input])]] = 1.0
		symbolsOutOnehot = np.reshape(symbolsOutOnehot, [1,-1])
		_, acc, loss, onehotPred = sess.run([optimizer, accuracy, cost, pred], feed_dict={x: symbolsInKeys, y: symbolsOutOnehot})
		lossTotal += loss
		accTotal += acc
		if(step+1) % displayStep == 0:
			print("Iter= " + str(step+1) + ", Average Loss= " +
					"{:.6f}".format(lossTotal/displayStep) + ", Average Accuracy= " +
					"{:.2f}%".format(100*accTotal/displayStep))
			accTotal = 0
			lossTotal = 0
			symbolsIn = [trainingData[i] for i in range(offset, offset + n_input)]
			symbolsOut = trainingData[offset + n_input]
			symbolsOutPred = reverseDictionary[int(tf.argmax(onehotPred, 1).eval())]
			print("%s - [%s] vs [%s]" % (symbolsIn, symbolsOut, symbolsOutPred))
		step += 1
		offset += (n_input + 1)
	print("Optimization Finishes.")
	print("Elapsed time: ", elapsed(time.time() - startTime))
	print("Run on command line.")
	print("\ttensorboard --logdir=%s" % (logsPath))
	print("Point your web browser to: http://localhost:6006/")
	while True:
		prompt = "%s words: " % n_input
		sentence = input(prompt)
		sentence = sentence.strip()
		words = sentence.split(' ')
		if len(words) != n_input:
			continue
		try:
			symbolsInKeys = [dictionary[str(words[i])] for i in range(len(words))]
			for i in range(32):
				keys = np.reshape(np.array(symbolsInKeys), [-1, n_input, 1])
				onehotPred = sess.run(pred, feed_dict={x: keys})
				onehotPredIndex = int(tf.argmax(onehotPred, 1).eval())
				sentence = "%s %s" % (sentence, reverseDictionary[onehotPredIndex])
				symbolsInKeys = symbolsInKeys[1:]
				symbolsInKeys.append(onehotPredIndex)
			print(sentence)
		except:
			print("Word not in dictionary")