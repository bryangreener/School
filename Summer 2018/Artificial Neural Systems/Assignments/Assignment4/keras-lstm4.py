from __future__ import print_function
from keras.layers import Dense, Dropout, LSTM
from keras.models import Sequential, load_model
from keras.callbacks import ModelCheckpoint
from keras.utils import np_utils
import numpy as np

class RNN:
    def __init__(self, batchSize, epochs):
        self.batchSize = batchSize
        self.epochs = epochs

    def Read(self, file, seqLength):
        data = open(file, 'r').read()
        chars = list(set(data))
        nData = len(data)
        self.nVocab = len(chars)
        charToIx = { ch:i for i, ch in enumerate(chars) }
        self.ixToChar = { i:ch for i, ch in enumerate(chars) }
        x, y = [], []
        for i in range(0, nData - seqLength, 1):
            x.append([charToIx[c] for c in data[i:i+seqLength]])
            #y.append([charToIx[c] for c in data[i+1:i+seqLength+1]])
            y.append(charToIx[data[i+seqLength]])
        x = np_utils.to_categorical(x)
        y = np_utils.to_categorical(y)
        return x, y

    def CreateModel(self, x, y, numHidden, dropAmount):
        model = Sequential()
        model.add(LSTM(numHidden, input_shape=(x.shape[1], x.shape[2]),
                       return_sequences=True))
        model.add(Dropout(dropAmount))
        model.add(LSTM(numHidden))
        model.add(Dropout(dropAmount))
        model.add(Dense(y.shape[1], activation='softmax'))
        model.compile(loss='categorical_crossentropy', optimizer='adam')
        self.model = model

    def Checkpointer(self):
        file = "lstm4-{epoch:02d}-{loss:.4f}.hdf5"
        return [ModelCheckpoint(file,
                                monitor='loss',
                                verbose=1,
                                save_best_only=True)]

    def Train(self, x, y, numEpochs, batchSize, resumeFile=None):
        if resumeFile:
            self.model.load_weights(resumeFile)
            self.model.compile(loss='categorical_crossentropy',
                               optimizer='adam')
        self.model.fit(x,
                       y,
                       epochs=numEpochs,
                       batch_size=batchSize,
                       callbacks=self.Checkpointer())

    def Generate(self, x, y, numToPrint, resumeFile=None):
        if resumeFile:
            self.model.load_weights(resumeFile)
            self.model.compile(loss='categorical_crossentropy',
                               optimizer='adam')
        start = np.random.randint(0, len(x)-2)
        pattern = x[start]
        print("Seed: {}", ("\"",
                          ''.join([self.ixToChar[v] for v in pattern]),
                          "\""))
        for i in range(numToPrint):
            prediction = self.model.predict(x, verbose=0)
            idx = np.argmax(prediction)
            result = self.ixToChar[idx]
            #seqIn = [self.ixToChar[v] for v in pattern]
            print(result)
            pattern.append(idx)
            pattern = pattern[1:len(pattern)]

if __name__ == '__main__':
    file = 'txts\\Rev.txt'
    epochs = 25
    batchSize = 32
    seqLength = 100
    numToPrint = 50
    numHidden = 128
    dropAmount = 0.2
    lstm = RNN(batchSize, epochs)
    x, y = lstm.Read(file, seqLength)
    lstm.CreateModel(x, y, numHidden, dropAmount)
    lstm.Train(x, y, epochs, batchSize)
    lstm.Generate(x, y, numToPrint)