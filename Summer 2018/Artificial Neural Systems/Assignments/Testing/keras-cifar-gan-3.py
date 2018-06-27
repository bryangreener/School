#READ THIS
#https://medium.com/@utk.is.here/keep-calm-and-train-a-gan-pitfalls-and-tips-on-training-generative-adversarial-networks-edd529764aa9
import keras
from keras.models import Sequential
from keras.layers import Dense, Reshape, Flatten, Activation, MaxPooling2D, Dropout
from keras.layers import BatchNormalization, UpSampling2D, Conv2D, Conv2DTranspose, LeakyReLU
from keras.optimizers import Adam, SGD
from keras.datasets import cifar10
import numpy as np
from PIL import Image
import matplotlib.pyplot as plt
import math

def Generator():
    generator = Sequential()
    depth = 1024
    dim = 2
    # shape based on page 4 of: https://arxiv.org/pdf/1511.06434.pdf
    # 1x4096
    generator.add(Dense(dim*dim*depth, input_dim=100))
    generator.add(BatchNormalization(momentum=0.9))
    # 2x2x1024
    generator.add(Reshape((dim, dim, depth)))
    # 4x4x512
    generator.add(Conv2DTranspose(int(depth/2), 5, strides=2, padding='same'))
    generator.add(BatchNormalization(momentum=0.9))
    generator.add(LeakyReLU(alpha=0.2))
    generator.add(Dropout(0.5))
    # 8x8x256
    generator.add(Conv2DTranspose(int(depth/4), 5, strides=2, padding='same'))
    generator.add(BatchNormalization(momentum=0.9))
    generator.add(LeakyReLU(alpha=0.2))
    generator.add(Dropout(0.5))
    # 16x16x128
    generator.add(Conv2DTranspose(int(depth/8), 5, strides=2, padding='same'))
    generator.add(BatchNormalization(momentum=0.9))
    generator.add(LeakyReLU(alpha=0.2))
    generator.add(Dropout(0.5))
    # 32x32x3
    generator.add(Conv2DTranspose(3, 5, strides=2, padding='same', activation='tanh'))
    generator.summary()
    return generator

def Discriminator():
    discriminator = Sequential()
    depth = 128
    input_shape = (32, 32, 3)
    
    discriminator.add(Conv2D(depth*1, 5, strides=2, 
                             input_shape=input_shape, padding='same'))
    discriminator.add(LeakyReLU(alpha=0.2))
    
    discriminator.add(Conv2D(depth*2, 5, strides=2, padding='same'))
    discriminator.add(LeakyReLU(alpha=0.2))
    
    discriminator.add(Conv2D(depth*4, 5, strides=2, padding='same'))
    discriminator.add(LeakyReLU(alpha=0.2))
    
    discriminator.add(Flatten())
    discriminator.add(Dense(1024))
    discriminator.add(LeakyReLU(alpha=0.1))

    discriminator.add(Dense(1))
    discriminator.summary()
    return discriminator
    
def Adversary(g, d):
    model = Sequential()
    model.add(g)
    d.trainable = False
    model.add(d)
    return model

def CombineImages(generated_images, epoch, batch, batch_size):
    plt.ioff()
    dim = int(math.sqrt(batch_size))
    fig, axs = plt.subplots(dim,dim, squeeze=False)
    count = 0
    for i in range(dim):
        for j in range(dim):
            img = keras.preprocessing.image.array_to_img(generated_images[count], scale=True)
            axs[i,j].imshow(img)
            axs[i,j].axis('off')
            count += 1
    #plt.show()
    plt.savefig('GAN_Images\{0}_{1}'.format(epoch, batch))
    plt.close()

def Train(batch_size):
    (x_train, y_train),(_,_) = cifar10.load_data()
    x_train = x_train[y_train[:,0]==7] #horses only
    x_train = (x_train - 127.5)/127.5
    d = Discriminator()
    g = Generator()
    a = Adversary(g, d)
    optimizer = Adam(0.0002, 0.5)
    g.compile(loss='binary_crossentropy', optimizer=optimizer)
    a.compile(loss='binary_crossentropy', optimizer=optimizer)
    d.trainable = True
    d.compile(loss='binary_crossentropy', optimizer=optimizer)
    
    noise = np.zeros((batch_size, 100))
    continuous_noise = np.zeros((batch_size, 100))
    for i in range(batch_size):
        continuous_noise[i, :] = np.random.uniform(-1, 1, 100)
        
    for epoch in range(1000):
        print("Epoch", epoch)
        for index in range(int(x_train.shape[0]/batch_size)):
            for i in range(batch_size):
                noise[i, :] = np.random.uniform(-1, 1, 100)
            image_batch = x_train[index * batch_size: (index+1) * batch_size]
            generated_images = g.predict(noise)
            if index % 10 == 0:
                CombineImages(g.predict(continuous_noise), epoch, index, batch_size)
            x = np.concatenate((image_batch, generated_images))
            y = [1] * batch_size + [0] * batch_size
            d_loss = d.train_on_batch(x, y)
            for i in range(batch_size):
                noise[i, :] = np.random.uniform(-1, 1, 100)
            d.trainable = False
            g_loss = a.train_on_batch(noise, [1] * batch_size)
            d.trainable = True
            print("Batch %d d_loss : %f g_loss : %f" % (index, d_loss, g_loss))

if __name__ == "__main__":
    Train(batch_size=4)
