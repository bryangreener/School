# Source: https://github.com/adventuresinML/adventures-in-ml-code
from __future__ import print_function
import keras
from keras.datasets import mnist
from keras.layers import Dense, Flatten
from keras.layers import Conv2D, MaxPooling2D
from keras.models import Sequential
import matplotlib.pyplot as plt

class AccuracyHistory(keras.callbacks.Callback):
    def on_train_begin(self, logs={}):
        self.acc = []
    def on_epoch_end(self, batch, logs={}):
        self.acc.append(logs.get('acc'))

#initialize variables for network
batch_size = 128
num_classes = 10
epochs = 10
#input image dimensions
img_x, img_y = 28,28
#load mnist dataset
(x_train, y_train), (x_test, y_test) = mnist.load_data()
#reshape data into 4d tensor (sampleNumber, xImgSize, yImgSize, numChannels)
x_train = x_train.reshape(x_train.shape[0], img_x, img_y, 1)
x_test = x_test.reshape(x_test.shape[0], img_x, img_y, 1)
input_shape = (img_x, img_y, 1)
# convert data to right type
x_train = x_train.astype('float32')
x_test = x_test.astype('float32')
# normalize between 0 and 1
x_train /= 255
x_test /= 255
print('x_train shape:', x_train.shape)
print(x_train.shape[0], 'train samples')
print(x_test.shape[0], 'test samples')
# convert vectors to binary class matrices for use in categorical_crossentropy
y_train = keras.utils.to_categorical(y_train, num_classes)
y_test = keras.utils.to_categorical(y_test, num_classes)

# Set up keras model
model = Sequential()
# 1st Conv Layer taking input image in
model.add(Conv2D(32, kernel_size=(5,5), strides=(1,1), 
                 activation='relu', input_shape=input_shape))
# 1st Max Pooling Layer
model.add(MaxPooling2D(pool_size=(2,2), strides=(2,2)))
# 2nd Conv Layer
model.add(Conv2D(64, (5,5), activation='relu'))
# 2nd Max Pooling layer
model.add(MaxPooling2D(pool_size=(2,2)))
# Flatten to 1 layer of nodes
model.add(Flatten())
# Fully connected layer using ReLU
model.add(Dense(1000, activation='relu'))
# Fully connected layer to outputs using Softmax
model.add(Dense(num_classes, activation='softmax'))
# Define loss function of network
model.compile(loss=keras.losses.categorical_crossentropy,
              optimizer=keras.optimizers.Adam(),
              metrics=['accuracy'])
# callback used in fit
history = AccuracyHistory()
# Train network
model.fit(x_train, y_train,
          batch_size=batch_size,
          epochs=epochs,
          verbose=1,
          validation_data=(x_test,y_test),
          callbacks=[history])
# Test and Output results
score = model.evaluate(x_test, y_test, verbose=0)
print("Test loss:", score[0])
print("Test accuracy:", score[1])
plt.plot(range(1, 11), history.acc)
plt.xlabel('Epochs')
plt.ylabel('Accuracy')
plt.show()
