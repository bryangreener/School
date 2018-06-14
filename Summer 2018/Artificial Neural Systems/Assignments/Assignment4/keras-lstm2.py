#https://machinelearningmastery.com/text-generation-lstm-recurrent-neural-networks-python-keras/
from __future__ import print_function
import keras
from keras.layers import Dense, Dropout, LSTM, TimeDistributed
from keras.models import Sequential, load_model
from keras.callbacks import ModelCheckpoint
import numpy as np
from keras.utils import np_utils

filename = "txts\\Rev.txt"
raw_text = (open(filename).read()).lower()
chars = sorted(list(set(raw_text)))
chars_to_int = dict((c, i) for i, c in enumerate(chars))
n_chars = len(raw_text)
n_vocab = len(chars)
print("Total characters:", n_chars)
print("Total vocabulary:", n_vocab)

seq_length = 100
dataX, dataY = [], []
for i in range(0, n_chars - seq_length, 1):
    seq_in = raw_text[i:i+seq_length]
    seq_out = raw_text[i+seq_length]
    dataX.append([chars_to_int[char] for char in seq_in])
    dataY.append(chars_to_int[seq_out])
n_patterns = len(dataX)
print("Total patterns:", n_patterns)

# reshape x to be [samples, timesteps, features]
x = np.reshape(dataX, (n_patterns, seq_length, 1))
x = x / float(n_vocab)
#onehot encode output variable
y = np_utils.to_categorical(dataY)
#define lstm model
model = Sequential()
model.add(LSTM(256, input_shape=(x.shape[1], x.shape[2]), 
               return_sequences=True))
model.add(Dropout(0.2))
#uncomment the following two lines to improve accuracy
model.add(LSTM(256))
model.add(Dropout(0.2))
model.add(TimeDistributed(Dense(y.shape[1], activation='softmax'))) #added
model.compile(loss='categorical_crossentropy', optimizer='adam')

# define checkpoint
filepath = "weights-improvement-{epoch:02d}-{loss:.4f}-big.hdf5"
checkpoint = ModelCheckpoint(filepath, monitor='loss', 
                             verbose=1, save_best_only=True)
callbacks_list = [checkpoint]

model.fit(x, y, epochs=20, batch_size=128, callbacks=callbacks_list)


#load netrwork weights
filename = "weights-improvement-19-1.9435.hdf5"
model.load_weights(filename)
model.compile(loss='categorical_crossentropy', optimizer='adam')
int_to_char = dict((i, c) for i, c in enumerate(chars))
# pick random seed
start = np.random.randint(0, len(dataX)-2)
pattern = dataX[start]
print("Seed:")
print("\"", ''.join([int_to_char[value] for value in pattern]), "\"")
# generate chars
for i in range(1000):
    x = np.reshape(pattern, (1, len(pattern), 1))
    x = x / float(n_vocab)
    prediction = model.predict(x, verbose=0)
    index = np.argmax(prediction)
    result = int_to_char[index]
    seq_in = [int_to_char[value] for value in pattern]
    print(result)
    pattern.append(index)
    pattern = pattern[1:len(pattern)]
print("\nDone.")