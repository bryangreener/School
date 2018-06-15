#https://machinelearningmastery.com/text-generation-lstm-recurrent-neural-networks-python-keras/
from __future__ import print_function
import keras
from keras.layers import Dense, Dropout, LSTM, TimeDistributed
from keras.models import Sequential, load_model
from keras.callbacks import ModelCheckpoint
import numpy as np
from keras.utils import np_utils

def PrepareData(file):
    raw = open(file).read().lower()
    chars = sorted(list(set(raw)))
    charsToInt = dict((c, i) for i, c in enumerate(chars))
    nChars = len(raw)
    seqLength = 100
    x, y = [], []
    for i in range(0, nChars - seqLength, 1):
        seqIn = raw[i:i+seqLength]
        seqOut = raw[i+seqLength]
        x.append([charsToInt[char] for char in seqIn])
        y.append(charsToInt[seqOut])
    newX = np_utils.to_categorical(x)
    newY = np_utils.to_categorical(y)
    return newX, newY, chars

def Checkpointer(model):
    file = "lstm-{epoch:02d}-{loss:.4f}.hdf5"
    return [ModelCheckpoint(file,monitor='loss',verbose=1,save_best_only=True)]

def Train(x, y, epochs, batchSize, resume=True):
    model = Sequential()
    model.add(LSTM(128, 
                   input_shape=(x.shape[1], x.shape[2]), 
                   return_sequences=True))
    model.add(Dropout(0.2))
    model.add(LSTM(128))
    model.add(Dropout(0.2))
    model.add(Dense(y.shape[1], activation='softmax'))
    model.compile(loss='categorical_crossentropy', optimizer='adam')
    
    checkpoint = Checkpointer(model)
    
    model.fit(x, y, epochs=epochs, batch_size=batchSize, callbacks=checkpoint)
    
    return model

def Generate(model, file, chars, numToPrint):
    model.load_weights(file)
    model.compile(loss='categorical_crossentropy', optimizer='adam')
    intToChar = dict((i, c) for i, c in enumerate(chars))
    start = np.random.randint(0, len(x)-2)
    pattern = x[start]
    print("Seed:", "\"", ''.join([intToChar[v] for v in pattern]), "\"")
    for i in range(numToPrint):
        tempX = np_utils.to_categorical(pattern)
        prediction = model.predict(tempX, verbose=0)
        index = np.argmax(prediction)
        result = intToChar[index]
        seqIn = [intToChar[v] for v in pattern]
        print(result)
        pattern.append(index)
        pattern = pattern[1:len(pattern)]
    
    
    
if __name__ == '__main__':
    file = "txts\\Rev.txt"
    epochs = 5
    batchSize = 32
    numToPrint = 50
    x, y, chars = PrepareData(file)
    model = Train(x, y, epochs, batchSize)
    Generate(model, file, chars)
    
