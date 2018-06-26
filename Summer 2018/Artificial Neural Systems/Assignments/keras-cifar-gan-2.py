#READ THIS
#https://medium.com/@utk.is.here/keep-calm-and-train-a-gan-pitfalls-and-tips-on-training-generative-adversarial-networks-edd529764aa9
from keras.models import Sequential
from keras.layers import Dense, Reshape, Flatten, Activation, MaxPooling2D, Dropout
from keras.layers import BatchNormalization, UpSampling2D, Conv2D, Conv2DTranspose, LeakyReLU
from keras.optimizers import Adam, SGD
from keras.datasets import cifar10
import numpy as np
from PIL import Image
import math

def Generator():
    '''
    model = Sequential()
    model.add(Dense(input_dim=100, output_dim=1024))
    model.add(Activation('tanh'))
    model.add(Dense(128*8*8))
    model.add(BatchNormalization())
    model.add(Activation('tanh'))
    model.add(Reshape((128, 8, 8), input_shape=(128*8*8,))
    model.add(UpSampling2D(size=(2,2)))
    model.add(Conv2D(64, 5, 5, padding='same'))
    model.add(Activation('tanh'))
    model.add(UpSampling2D(size=(2,2)))
    model.add(Conv2D(3, 5, 5, padding='same'))
    model.add(Activation('tanh'))
    return model
    '''
    generator = Sequential()
    dropout = 0.4
    depth = 64+64+64+64
    dim = 8
    
    generator.add(Dense(dim*dim*depth, input_dim=100))
    generator.add(BatchNormalization(momentum=0.9))
    generator.add(Reshape((dim, dim, depth)))
    generator.add(Dropout(dropout))
    generator.add(UpSampling2D())
    generator.add(Conv2DTranspose(int(depth/2), 5, padding='same', 
                                  activation='relu',
                                  kernel_initializer='glorot_normal',
                                  bias_initializer='Zeros'))
    generator.add(BatchNormalization(momentum=0.9))
    #generator.add(Activation('relu'))
    generator.add(UpSampling2D())
    generator.add(Conv2DTranspose(int(depth/4), 5, padding='same', 
                                  activation='relu',
                                  kernel_initializer='glorot_normal',
                                  bias_initializer='Zeros'))
    generator.add(BatchNormalization(momentum=0.9))
    #generator.add(Activation('relu'))
    generator.add(Conv2DTranspose(int(depth/8), 5, padding='same', 
                                  activation='relu',
                                  kernel_initializer='glorot_normal',
                                  bias_initializer='Zeros'))
    generator.add(BatchNormalization(momentum=0.9))
    #generator.add(Activation('relu'))
    generator.add(Conv2DTranspose(3, 5, padding='same', activation='tanh',
                                  kernel_initializer='glorot_normal',
                                  bias_initializer='Zeros'))
    return generator
    '''
    model = Sequential()
    model.add(Dense(256*8*8, 
                    input_dim=100,
                    kernel_initializer='glorot_normal', 
                    bias_initializer='Zeros'))
    model.add(Reshape((8, 8, 256)))
    model.add(Conv2DTranspose(256/2, kernel_size=5, padding='same',
                              activation='relu', 
                              kernel_initializer='glorot_normal', 
                              bias_initializer='Zeros'))
    model.add(BatchNormalization())
    model.add(Conv2DTranspose(256/4, kernel_size=5, padding='same',
                              activation='relu',
                              kernel_initializer='glorot_normal',
                              bias_initializer='Zeros'))
    model.add(BatchNormalization())
    model.add(Conv2DTranspose(256/8, kernel_size=5, padding='same',
                              activation='relu',
                              kernel_initializer='glorot_normal',
                              bias_initializer='Zeros'))
    model.add(BatchNormalization())
    model.add(Conv2DTranspose(3, kernel_size=5, padding='same',
                              activation='tanh',
                              kernel_initializer='glorot_normal',
                              bias_initializer='Zeros'))
    return model
    '''
def Discriminator():
    '''
    model = Sequential()
    model.add(Conv2D(32, kernel_size=(3,3), strides=(1,1), padding='same',
                 activation='tanh', input_shape=(3, 32, 32)))
    model.add(Conv2D(32, (3,3), padding='same', activation='tanh'))
    model.add(MaxPooling2D(pool_size=(2,2), dim_ordering='th'))
    model.add(Conv2D(64, (3,3), padding='same', activation='tanh'))
    model.add(Conv2D(64, (3,3), padding='same', activation='tanh'))
    model.add(MaxPooling2D(pool_size=(2,2), dim_ordering='th'))
    model.add(Conv2D(128, (3,3), padding='same', activation='tanh'))
    model.add(Conv2D(128, (3,3), padding='same', activation='tanh'))
    model.add(MaxPooling2D(pool_size=(2,2), dim_ordering='th'))
    model.add(Flatten())
    model.add(Dense(1024, activation='tanh'))
    model.add(Dense(1, activation='sigmoid'))
    return model
    '''
    discriminator = Sequential()
    depth = 64
    #dropout = 0.4
    input_shape = (32, 32, 3)
    discriminator.add(Conv2D(depth*1, 5, strides=1, input_shape=input_shape,
                             padding='same', activation='tanh'))
    discriminator.add(MaxPooling2D())
    #discriminator.add(LeakyReLU(alpha=0.2))
    #discriminator.add(Dropout(dropout))
    discriminator.add(Conv2D(depth*2, 5, strides=1, padding='same', activation='tanh'))
    discriminator.add(MaxPooling2D())
    #discriminator.add(LeakyReLU(alpha=0.2))
    #discriminator.add(Dropout(dropout))
    discriminator.add(Conv2D(depth*4, 5, strides=1, padding='same', activation='tanh'))
    discriminator.add(MaxPooling2D())
    #discriminator.add(LeakyReLU(alpha=0.2))
    #discriminator.add(Dropout(dropout))
    discriminator.add(Conv2D(depth*4, 5, strides=1, padding='same', activation='tanh'))
    discriminator.add(MaxPooling2D())
    #discriminator.add(LeakyReLU(alpha=0.2))
    #discriminator.add(Dropout(dropout))
    discriminator.add(Flatten())
    discriminator.add(Dense(1024, activation='tanh'))
    discriminator.add(Dense(1, activation='sigmoid'))
    return discriminator
    
def Adversary(g, d):
    model = Sequential()
    model.add(g)
    d.trainable = False
    model.add(d)
    return model

def CombineImages(generated_images):
    num = generated_images.shape[0]
    width = int(math.sqrt(num))
    height = int(math.ceil(float(num)/width))
    generated_images = generated_images.reshape((generated_images.shape[0], 32, 32, 3))
    shape = generated_images.shape[1:3]
    image = np.zeros((height*shape[0], width*shape[1], 3), dtype=generated_images.dtype)
    for index, img in enumerate(generated_images):
        i = int(index/width)
        j = index % width
        image[i * shape[0]: (i+1) * shape[0], j * shape[1]: (j+1) * shape[1], 
              0] = img[:, :, 0]
        image[i * shape[0]: (i+1) * shape[0], j * shape[1]: (j+1) * shape[1], 
              1] = img[:, :, 1]
        image[i * shape[0]: (i+1) * shape[0], j * shape[1]: (j+1) * shape[1], 
              2] = img[:, :, 2]
    return image

def Train(batch_size):
    (x_train, _),(_,_) = cifar10.load_data()
    x_train = (x_train.astype(np.float32) - 127.5)/127.5
    #x_train = x_train.reshape((x_train.shape[0], 3) + x_train.shape[1:3])
    d = Discriminator()
    g = Generator()
    a = Adversary(g, d)
    d_optimizer = Adam(lr=0.0002, beta_1=0.5)
    g_optimizer = Adam(lr=0.0002, beta_1=0.5)
    g.compile(loss='binary_crossentropy', optimizer=g_optimizer)
    a.compile(loss='binary_crossentropy', optimizer=g_optimizer)
    d.trainable = True
    d.compile(loss='binary_crossentropy', optimizer=d_optimizer)
    
    noise = np.zeros((batch_size, 100))
    
    for epoch in range(1000):
        print("Epoch", epoch)
        for index in range(int(x_train.shape[0]/batch_size)):
            for i in range(batch_size):
                noise[i, :] = np.random.uniform(-1, 1, 100)
            image_batch = x_train[index * batch_size: (index+1) * batch_size]
            generated_images = g.predict(noise, verbose=0)
            if index % 5 == 0:
                image = CombineImages(generated_images)
                image = image*127.5 + 127.5
                Image.fromarray(image.astype(np.uint8)).save(str(epoch)+"_"+str(index)+".png")
            x = np.concatenate((image_batch, generated_images))
            y = [1] * batch_size + [0] * batch_size
            d_loss = d.train_on_batch(x, y)
            print("Batch %d d_loss: %f" % (index, d_loss))
            for i in range(batch_size):
                noise[i, :] = np.random.uniform(-1, 1, 100)
            d.trainable = False
            g_loss = a.train_on_batch(noise, [1] * batch_size)
            d.trainable = True
            print("Batch %d g_loss : %f" % (index, g_loss))

if __name__ == "__main__":
    Train(batch_size=16)
