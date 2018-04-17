import ast
import matplotlib.pyplot as plt
import numpy as np
file = open('TotalLong.txt', 'r')
mainlist = []
for line in file:
    mainlist.append(line)
temp = []
for i in mainlist:
    temp.append(ast.literal_eval(i))
it = 0
layerlist=['784-35-15-20-10']
learnrates=['0.075']
batchlist=['15']

fig = plt.figure()
plt.plot(np.array(temp[0][0][0][0][100:500])/100)
plt.plot(np.array(temp[1][0][0][0][100:500])/100)
plt.title("Design: 784-35-15-20-10 -- Learn Rate: 0.075")
plt.legend(['RMSprop', 'SGD'], loc='lower right')
plt.grid(1)
plt.xlabel("Iterations (Epochs)")
plt.ylabel("Accuracy (%)")
plt.xlim((100,400))
plt.show()
fig.savefig("./RMSvsSGD2.png")
plt.close(fig)

'''
#### PLOT EPOCHS
for i in range(len(layerlist)):
    for j in temp[i][0]:
        fig = plt.figure()    
        for k in j:
            plt.plot(np.array(k[:30]))
        plt.title("Design: {0} -- Learn Rate: {1}".format(layerlist[i], learnrates[it]))
        plt.legend(['RMSprop', 'SGD'], loc='lower right')
        plt.grid(1)
        plt.show()
        fig.savefig(".{0}_{1}.png".format(layerlist[i], learnrates[it]))
        plt.close(fig)
        it += 1
    it = 0


#### PLOT TIMES
times = []
batches = [15]
for i in range(len(layerlist)):
    tt = []
    for j in temp[i][0]:
        temptime = []
        for k in j:
            temptime.append(k[-1])
        tt.append(zip(batches,temptime))
    times.append(tt)

for i in range(len(times)):
    fig = plt.figure()
    for j in times[i]:
        x,y = zip(*j)
        plt.plot(x,y)
    plt.title("Design: {0}".format(layerlist[i]))
    plt.legend(['0.075'], loc='lower right')
    plt.grid(1)
    plt.show()
    fig.savefig(".TIME_{0}.png".format(layerlist[i]))
    plt.close(fig)
'''