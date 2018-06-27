from __future__ import print_function
import keras
from keras.datasets import cifar10
from keras.layers import Input, Dense, Reshape, Flatten, Dropout
from keras.layers import BatchNormalization, Activation, MaxPooling2D
from keras.layers.advanced_activations import LeakyReLU
from keras.layers.convolutional import UpSampling2D, Conv2D
from keras.models import Sequential, Model, load_model
from keras.callbacks import ModelCheckpoint
from keras.constraints import maxnorm
from keras.optimizers import Adam
import matplotlib.pyplot as plt
import numpy as np

class AccuracyHistory(keras.callbacks.Callback):
    def on_train_begin(self, logs={}):
        self.acc = []
    def on_epoch_end(self, batch, logs={}):
        ModelCheckpoint(filepath=modelPath + '\model-{epoch:02d}.hdf5')
        self.acc.append(logs.get('acc'))

class GAN():
    def __init__(self):
        self.shape = (32, 32, 3)
        self.batchSize = 32
        self.numClasses = 10
        self.epochs = 100
        self.latentDim = 100
        
        self.disc = self.Discriminator()
        self.disc.compile(loss='binary_crossentropy',
                                   optimizer=Adam(),
                                   metrics=['accuracy'])
        self.gen = self.Generator()
        z = Input(shape=(100,))
        img = self.gen(z)
        self.disc.trainable = False
        validity = self.disc(img)
        self.combined = Model(z, validity)
        history = AccuracyHistory()
        self.combined.compile(loss='binary_crossentropy', 
                              optimizer=keras.optimizers.Adam(),
                              callback=[history])
    
    def Generator(self):
        gen = Sequential()
        gen.add(Dense(256, input_dim=self.latentDim))
        gen.add(LeakyReLU(alpha=0.2))
        gen.add(BatchNormalization(momentum=0.8))
        gen.add(Dense(512))
        gen.add(LeakyReLU(alpha=0.2))
        gen.add(BatchNormalization(momentum=0.8))
        gen.add(Dense(1024))
        gen.add(LeakyReLU(alpha=0.2))
        gen.add(BatchNormalization(momentum=0.8))
        gen.add(Dense(np.prod(self.shape), activation='tanh'))
        gen.add(Reshape(self.shape))
        gen.summary()
        
        noise = Input(shape=(self.latentDim,))
        img = gen(noise)
        
        return Model(noise, img)
        
    def Discriminator(self):
        disc = Sequential()
        disc.add(Conv2D(32, kernel_size=(3,3), strides=(1,1), padding='same',
                         activation='relu', input_shape=self.shape))
        disc.add(Dropout(0.2))
        disc.add(Conv2D(32, (3,3), padding='same', activation='relu'))
        disc.add(MaxPooling2D(pool_size=(2,2)))
        disc.add(Conv2D(64, (3,3), padding='same', activation='relu'))
        disc.add(Dropout(0.2))
        disc.add(Conv2D(64, (3,3), padding='same', activation='relu'))
        disc.add(MaxPooling2D(pool_size=(2,2)))
        disc.add(Conv2D(128, (3,3), padding='same', activation='relu'))
        disc.add(Dropout(0.2))
        disc.add(Conv2D(128, (3,3), padding='same', activation='relu'))
        disc.add(MaxPooling2D(pool_size=(2,2)))
        disc.add(Flatten())
        disc.add(Dropout(0.2))
        disc.add(Dense(1024, activation='relu', kernel_constraint=maxnorm(3)))
        disc.add(Dropout(0.2))
        disc.add(Dense(self.numClasses, activation='softmax'))
        disc.summary()
        
        img = Input(shape=self.shape)
        validity = disc(img)
        
        return Model(img, validity)
    
    def Train(self):
        sampleInterval = 200
        (x_train, _), (_, _) = cifar10.load_data()
        x_train = x_train / 127.5 - 1.
        x_train = np.expand_dims(x_train, axis=3)
        
        valid = np.ones((self.batchSize, 1))
        fake = np.zeros((self.batchSize, 1))
        
        for epoch in range(self.epochs):
            idx = np.random.randint(0, x_train.shape[0], self.batchSize)
            imgs = x_train[idx]
            noise = np.random.normal(0, 1, (self.batchSize, 100))
            genImgs = self.gen.predict(noise)
            # Train discriminator
            dLossReal = self.disc.train_on_batch(imgs, valid)
            dLossFake = self.disc.train_on_batch(genImgs, fake)
            dLoss = 0.5 * np.add(dLossReal, dLossFake)
            # Train generator
            noise = np.random.normal(0, 1, (self.batchSize, 100))
            gLoss = self.combined.train_on_batch(noise, valid)
        
            print("%d [D loss: %f, acc.: %.2f%%] [G loss: %f]" % (
                    epoch, dLoss[0], 100*dLoss[1], gLoss))
            if epoch % sampleInterval == 0:
                self.SampleImages(epoch)
                
        def SampleImages(self, epoch):
            r, c = 5, 5
            noise = np.random.normal(0, 1, (r * c, 100))
            genImages = self.gen.predict(noise)
            genImages = 0.5 * genImages + 0.5
            fig, axs = plt.subplots(r, c)
            cnt = 0
            for i in range(r):
                for j in range(c):
                    axs[i,j].imshow(genImages[cnt, :, :, 0], cmap='gray')
                    axs[i,j].axis('off')
                    cnt += 1
            fig.savefig('images/%d.png' % epoch)
            plt.close()

if __name__ == '__main__':
    modelPath = 'C:\\users\\aabgreener\\desktop\\keras-cifar-gan'
    gan = GAN()
    gan.Train()