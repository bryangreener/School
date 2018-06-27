# http://adventuresinmachinelearning.com/keras-lstm-tutorial/
from __future__ import print_function
import os
import collections
import tensorflow as tf
from keras.models import Sequential, load_model
from keras.layers import Dense, Activation, Embedding, Flatten, Dropout
from keras.layers import TimeDistributed, Reshape, Lambda, LSTM
from keras.optimizers import RMSprop, Adam, SGD
from keras import backend as K
from keras.utils import to_categorical
from keras.callbacks import ModelCheckpoint
import numpy as np
import argparse
import pdb

### DATA PREPROCESSING ###
def ReadWords(file):
    with tf.gfile.GFile(file, "r") as f:
        return f.read().replace('\n', '<eos>').split()
def BuildVocab(file):
    data = ReadWords(file)
    counter = collections.Counter(data)
    countPairs = sorted(counter.items(), key=lambda x: (-x[1], x[0]))
    words, _ = list(zip(*countPairs))
    wordToId = dict(zip(words, range(len(words))))
    return wordToId
def FileToWordIds(file, wordToId):
    data = ReadWords(file)
    return [wordToId[word] for word in data if word in wordToId]
def LoadData():
    trainPath = os.path.join(dataPath, 'ptb.train.txt')
    validPath = os.path.join(dataPath, 'ptb.valid.txt')
    testPath  = os.path.join(dataPath, 'ptb.test.txt')
    wordToId = BuildVocab(trainPath)
    trainData = FileToWordIds(trainPath, wordToId)
    validData = FileToWordIds(validPath, wordToId)
    testData  = FileToWordIds(testPath,  wordToId)
    vocab = len(wordToId)
    reversedDictionary = dict(zip(wordToId.values(), wordToId.keys()))
    print(trainData[:5])
    print(wordToId)
    print(vocab)
    print(" ".join([reversedDictionary[x] for x in trainData[:10]]))
    return trainData, validData, testData, vocab, reversedDictionary

### LSTM DATA GENERATORS ###
class KerasBatchGenerator(object):
    def __init__(self, data, numSteps, batchSize, vocab, skipStep=5):
        self.data = data
        self.numSteps = numSteps
        self.batchSize = batchSize
        self.vocab = vocab
        # track progress of batches through dataset
        self.currentIdx = 0
        # number of words skipped before next batch is skimmed
        self.skipStep = skipStep
    def Generate(self):
        # create output arrays
        x = np.zeros((self.batchSize, self.numSteps))
        y = np.zeros((self.batchSize, self.numSteps, self.vocab))
        while True:
            for i in range(self.batchSize):
                if self.currentIdx + self.numSteps >= len(self.data):
                    self.currentIdx = 0
                x[i, :] = self.data[self.currentIdx:self.currentIdx + self.numSteps]
                tempY = self.data[self.currentIdx + 1: self.currentIdx + self.numSteps + 1]
                y[i, :, :] = to_categorical(tempY, num_classes=self.vocab)
                self.currentIdx += self.skipStep
            yield x, y

dataPath = 'C:\\Users\\aabgreener\\Desktop\\lstmdata\\data'

#parser = argparse.ArgumentParser()
#parser.add_argument('run_opt', type=int, default=1, help='An integer: 1 to train, 2 to test')
#parser.add_argument('--dataPath', type=str, default=dataPath, help='Full path of train data.')
#args = parser.parse_args()
#if args.dataPath:
#    dataPath = args.dataPath

numSteps = 30
numEpochs = 50
batchSize = 20
hiddenSize = 500
useDropout = True

trainData, validData, testData, vocab, reversedDictionary = LoadData()
trainDataGenerator = KerasBatchGenerator(trainData, numSteps, batchSize, vocab,
                                         skipStep=numSteps)
validDataGenerator = KerasBatchGenerator(validData, numSteps, batchSize, vocab,
                                         skipStep=numSteps)

model = Sequential()
model.add(Embedding(vocab, hiddenSize, input_length=numSteps))
model.add(LSTM(hiddenSize, return_sequences=True))
model.add(LSTM(hiddenSize, return_sequences=True))
if useDropout:
    model.add(Dropout(0.5))
model.add(TimeDistributed(Dense(vocab)))
model.add(Activation('softmax'))
model.compile(loss='categorical_crossentropy', optimizer='adam', 
              metrics=['categorical_accuracy'])
checkpointer = ModelCheckpoint(filepath=dataPath + '/model-{epoch:02d}.hdf5',
                               verbose=1)
model.fit_generator(trainDataGenerator.Generate(),
                    len(trainData)//(batchSize*numSteps), numEpochs,
                    validation_data=validDataGenerator.Generate(),
                    validation_steps=len(validData)//(batchSize*numSteps), 
                    callbacks=[checkpointer])

model = load_model(dataPath + "\model-40.hdf5")
dummyIters = 40
exampleTrainingGenerator = KerasBatchGenerator(trainData, numSteps, 1, 
                                               vocab, skipStep=1)
print("Training data")
for i in range(dummyIters):
    dummy = next(exampleTrainingGenerator.Generate())
numPredict = 10
truePrintOut = "Actual words: "
predPrintOut = "Predicted words: "
for i in range(numPredict):
    data = next(exampleTrainingGenerator.Generate())
    prediction = model.predict(data[0])
    predictWord = np.argmax(prediction[:, numSteps-1, :])
    truePrintOut += reversedDictionary[trainData[numSteps + dummyIters + i]]
    predPrintOut += reversedDictionary[predictWord] + " "
print(truePrintOut)
print(predPrintOut)