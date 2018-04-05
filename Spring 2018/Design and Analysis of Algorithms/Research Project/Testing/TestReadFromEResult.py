import ast
import matplotlib.pyplot as plt
import numpy as np
file = open('NNOutput2018-04-05.txt', 'r')
mainlist = []
for line in file:
    mainlist.append(line)
temp = []
for i in mainlist:
    temp.append(ast.literal_eval(i))
it = 0
layerlist=['784-10-10','784-10-10-10','784-10-10-10-10',
             '784-15-10','784-15-15-10','784-15-15-15-10',
             '784-20-10','784-20-20-10','784-20-20-20-10',
             '784-25-10','784-25-25-10','784-25-25-25-10',
             '784-30-10','784-30-30-10','784-30-30-30-10',
             '784-50-10','784-50-50-10','784-50-50-50-10']
learnrates=['1.0','2.0','3.0','4.0','5.0','6.0','7.0','8.0','9.0','10.0']

for i in range(len(layerlist)):
    for j in temp[i][0]:
        fig = plt.figure()    
        for k in j:
            plt.plot(np.array(k[:30]))
        plt.title("Design: {0} -- Learn Rate: {1}".format(layerlist[i], learnrates[it]))
        plt.legend(['5', '10', '15', '20', '25', '30', '35', '40', '45', '50'], loc='lower right')
        plt.grid(1)
        plt.show()
        fig.savefig("./Graphs/{0}_{1}.png".format(layerlist[i], learnrates[it]))
        plt.close(fig)
        it += 1
    it = 0
        
        

