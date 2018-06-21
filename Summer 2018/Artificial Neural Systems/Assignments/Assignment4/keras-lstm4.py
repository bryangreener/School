from __future__ import print_function
import keras
from keras.layers import Dense, Dropout, LSTM
from keras.models import Sequential, load_model
from keras.callbacks import ModelCheckpoint
from keras.utils import np_utils
import numpy as np
import matplotlib.pyplot as plt

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

    def CreateModel(self, x, y, numHidden, dropAmount, resumeFile=''):
        if resumeFile:
            model = load_model(resumeFile)   
        else:
            model = Sequential()
            model.add(LSTM(numHidden, input_shape=(x.shape[1], x.shape[2]),
                           return_sequences=True))
            model.add(Dropout(dropAmount))
            model.add(LSTM(numHidden, return_sequences=True))
            model.add(Dropout(dropAmount))
            model.add(LSTM(numHidden, return_sequences=True))
            model.add(Dropout(dropAmount))
            model.add(Dense(y.shape[1], activation='softmax'))
            model.compile(loss='categorical_crossentropy', optimizer='adam')
        self.model = model

    def Checkpointer(self):
        file = "lstm4-small-{epoch:02d}-{loss:.4f}.hdf5"
        return [ModelCheckpoint(file,
                                monitor='loss',
                                verbose=1,
                                save_best_only=True)]

    def Train(self, x, y, numEpochs, batchSize):
        checkpoint = self.Checkpointer()
        history = self.model.fit(x,
                                 y,
                                 epochs=numEpochs,
                                 batch_size=batchSize,
                                 shuffle=False,
                                 callbacks=checkpoint)
        
        plt.plot(history.history['loss'])
        plt.xlabel('Epochs')
        plt.ylabel('Loss')
        plt.show()

    def Generate(self, x, y, numToPrint):
        start = np.random.randint(0, len(x)-2)
        pattern = x[start]
        output = ''.join([self.ixToChar[np.argmax(v)] for v in pattern])
        print("Seed: ", output)
        
        for i in range(numToPrint):
            prediction = self.model.predict(np.array([pattern]), verbose=0)
            idx = np.argmax(prediction)
            output += self.ixToChar[idx]
            #seqIn = [self.ixToChar[v] for v in pattern]
            pattern = np.concatenate((pattern, prediction))
            pattern = pattern[1:len(pattern)]
        print("New String:", output)

if __name__ == '__main__':
    file = 'txts\\Rev.txt'
    epochs = 50
    batchSize = 128
    seqLength = 100
    numToPrint = 50
    numHidden = 256
    dropAmount = 0.2
    #resumeFile = "lstm4-13-0.8142.hdf5"
    lstm = RNN(batchSize, epochs)
    x, y = lstm.Read(file, seqLength)
    lstm.CreateModel(x, y, numHidden, dropAmount)
    lstm.Train(x, y, epochs, batchSize)
    lstm.Generate(x, y, numToPrint)

    
