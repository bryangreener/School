from __future__ import print_function
import keras
import matplotlib.pyplot as plt
from keras.datasets import cifar10
from keras.layers import Dense, Flatten, Conv2D, MaxPooling2D, Dropout
from keras.models import Sequential, load_model
from keras.constraints import maxnorm
#from keras_sequential_ascii import sequential_model_to_ascii_printout
from keras.callbacks import ModelCheckpoint

class AccuracyHistory(keras.callbacks.Callback):
    def on_train_begin(self, logs={}):
        self.acc = []
    def on_epoch_end(self, batch, logs={}):
        ModelCheckpoint(filepath=modelPath + '\model-{epoch:02d}.hdf5')
        self.acc.append(logs.get('acc'))

modelPath = 'C:\\users\\aabgreener\\desktop'
(x_train, y_train), (x_test, y_test) = cifar10.load_data()
batchSize = 32
numClasses = 10
epochs = 100

#convert and preprocess images
# output variables are defined as a vector of ints from 0 to 1 for each class
y_train = keras.utils.to_categorical(y_train, numClasses)
y_test = keras.utils.to_categorical(y_test, numClasses)
x_train = x_train.astype('float32')
x_test = x_test.astype('float32')
x_train /= 255
x_test /= 255


model = Sequential()

model.add(Conv2D(32, kernel_size=(3,3), strides=(1,1), padding='same',
                 activation='relu', input_shape=x_train.shape[1:]))
model.add(Dropout(0.2))

model.add(Conv2D(32, (3,3), padding='same', activation='relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(64, (3,3), padding='same', activation='relu'))
model.add(Dropout(0.2))

model.add(Conv2D(64, (3,3), padding='same', activation='relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(128, (3,3), padding='same', activation='relu'))
model.add(Dropout(0.2))

model.add(Conv2D(128, (3,3), padding='same', activation='relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Flatten())
model.add(Dropout(0.2))
model.add(Dense(1024, activation='relu', kernel_constraint=maxnorm(3)))
model.add(Dropout(0.2))
model.add(Dense(numClasses, activation='softmax'))

model.compile(loss=keras.losses.categorical_crossentropy,
              optimizer=keras.optimizers.Adam(),
              metrics=['accuracy'])

history=AccuracyHistory()

cnn_n = model.fit(x_train, y_train,
          batch_size=batchSize,
          epochs=epochs,
          verbose=1,
          validation_data=(x_test, y_test),
          callbacks=[history])

model = load_model(modelPath + '\model-10.hdf5')

#sequential_model_to_ascii_printout(cnn_n)

score = model.evaluate(x_test, y_test, verbose=0)
print("Test loss:", score[0])
print("Test accuracy:", score[1])
plt.plot(range(1,11), history.acc)
plt.xlabel('Epochs')
plt.ylabel('Accuracy')
plt.show()

import numpy as np
from sklearn.metrics import classification_report, confusion_matrix
YPred = cnn_n.predict(x_test, verbose=2)
yPred = np.argmax(YPred, axis=1)
for ix in range(10):
    print(ix, confusion_matrix(np.argmax(y_test, axis=1), yPred)[ix].sum())
cm = confusion_matrix(np.argmax(y_test, axis=1), yPred)
print(cm)
import seabord as sn
import pandas as pd
df_cm = pd.Dataframe(cm, range(10), range(10))
plt.figure(figsize=(10,7))
sn.set(font_scale=1.4)
sn.heatmap(df_cm, annot=True, annot_kws={"size": 12})
plt.show