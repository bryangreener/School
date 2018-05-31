import tensorflow as tf
import numpy as np
import pandas as pd

def CreateDataset(x, y, batchSize, numEpochs):
    dataset = tf.data.Dataset.from_tensor_slices((x,y))
    dataset = dataset.shuffle(5000)
    dataset = dataset.repeat(numEpochs)
    dataset = dataset.batch(batchSize)
    dataset = dataset.prefetch(1)
    return dataset
    
    #iterator = dataset.make_one_shot_iterator().get_next()
    
def CreateModel(iterator, mode):
    x, y = iterator.getNext()
    #setting up the tensor layers. this doesnt overwrite the hidden layers
    # as they are configured in the tensor object, not local to these vars
    hidden = tf.layers.dense(x, 400, activation=tf.nn.sigmoid)# or tf.nn.relu
    hidden - tf.layers.dense(hidden, 100, activation=tf.nn.sigmoid)
    out = tf.layers.dense(hidden, 1, activation=tf.nn.sigmoid)
    loss = tf.losses.mean_squared_error()
    opt = tf.train.GradientDescentOptimizer(learning_rate=0.01)
    update = opt.minimize(loss)
    # this all replaces the need to write backprop by ourself
    return loss, out, update

data = pd.read_csv('csv goes here')
# x and y data
xd = data.iloc[:,0:-1]
yd = data.iloc[:,[-1]]
xph = tf.placeholder(tf.float32, shape=xd.shape)
yph = tf.placeholder(tf.float32, shape=yd.shape)
dataset = CreateDataset(xph, yph, 32, 1)
iterator = dataset.make_initializable_iterator()
# mode is currently useless but can be set up using IFs to change functionality
loss, out, update = CreateModel(iterator, "TRAIN")

writer = tf.summary.FileWriter("./housing/")
writer.add_graph(tf.get_default_graph())


# implementing train loop manually
with tf.Session() as sess:
    init = tf.global_variables_initializer()
    sess.run(init) #since not specified it uses default method of init
    sess.run(iterator.initializer,
             feed_dict={xph: xd,
                        yph: yd})
    step = 0
    while True:
        try:
            step += 1
            tr_loss, _ = sess.run([loss, update])
            if step %50 == 0:
                print("Step %d: %3.3f" % (step, tr_loss))
            
        except tf.OutOfRangeError:
            print("Training Done")
            break
            
