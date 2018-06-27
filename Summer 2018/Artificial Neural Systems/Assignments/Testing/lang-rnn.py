import tensorflow as tf

''' PSEUDOCODE
words_in_dataset = tf.placeholder(tf.float32, [time_steps, batch_size, num_features])
lstm = tf.contrib.rnn.BasicLSTMCell(lstm_size)
hidden_state = tf.zeros([batch_size, lstm.state_size])
current_state = tf.zeros([batch_size, lstm.state_size])
state = hidden_state, current_state
probabilities = []
loss = 0.0
for current_batch_of_words in words_in_dataset:
    output, state = lstm(current_batch_of_words, state)
    logits = tf.matmul(output, softmax_w) + softmax_b
    probabilities.append(tf.nn.softmax(logits))
    loss += loss_function(probabilities, target_words) '''
    
''' TRUNCATED BACKPROP
words = tf.placeholder(tf.int32, [batch_size, num_steps])
lstm = tf.contrib.rnn.BasicLSTMCell(lstm_size)
initial_state = state = tf.zeros([batch_size, lstm.state_size)
for i in range(num_steps):
    output, state = lstm(words[:, i], state)
final_state = state '''

''' IMPLEMENT ITERATIONA OVER DATASET
numpy_state = inital_state.eval()
total_loss = 0.0
for current_batch_of_words in words_in_dataset:
    numpy_state, current_loss = session.run([final_state, loss],
        feed_dict={initial_state: numpy_state, words: current_batch_of_words})
    total_loss += current_loss '''
    
''' STACKING LSTMs
def lstm_cell():
    return tf.contrib.rnn.BasicLSTMCell(lstm_size)

stacked_lstm = tf.contrib.rnn.MultiRNNCell(
        [lstm_cell() for _ in range(number_of_layers)])
initial_state = state = stacked_lstm.zero_state(batch_size, tf.float32)
for i in range(num_steps):
    output, state = stacked_lstm(words[:, i], state)
final_state = state '''
