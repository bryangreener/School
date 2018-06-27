# -*- coding: utf-8 -*-
"""
Created on Thu Jun 14 08:22:37 2018

@author: aabgreener
"""

from __future__ import print_function
import keras
from keras.layers import Dense, Flatten, Dropout, Conv2D, MaxPooling2D
from keras.models import Sequential, load_model
from keras.datasets import cifar100
from keras.constraints import maxnorm
from keras.callbacks import ModelCheckpoint

numClasses = 100
batchSize = 32
epochs = 10

(x_train, y_train), (x_test, y_test) = cifar100.load_data()

y_train = keras.utils.to_categorical(y_train, numClasses)
y_test = keras.utils.to_categorical(y_test, numClasses)
x_train = x_train.astype('float32')
x_test = x_test.astype('float32')
x_train /= 255
x_test /= 255

model = Sequential()
model.add(Conv2D(64, kernel_size=(5,5), strides=(1,1), padding='same',
                 activation='relu', input_shape=x_train.shape[1:]))
model.add(MaxPooling2D(pool_size=(3,3)))

model.add(Conv2D(64, (5,5), padding='same', activation='relu'))
model.add(MaxPooling2D(pool_size=(3,3)))

model.add(Conv2D(64, (5,5), padding='same', activation='relu'))
model.add(MaxPooling2D(pool_size=(3,3)))

model.add(Flatten())
model.add(Dense(1024, activation='relu', kernel_constraint=maxnorm(3)))
model.add(Dense(numClasses, activation='softmax'))

model.compile(loss=keras.losses.categorical_crossentropy,
              optimizer=keras.optimizers.Adam(),
              metrics=['accuracy'])

model.fit(x_train, y_train, batch_size=batchSize, epochs=epochs,
          verbose=1, validation_data=(x_test,y_test))
score = model.evaluate(x_test, y_test, verbose=0)
print("Total loss:", score[0])
print("Test accuracy:", score[1])