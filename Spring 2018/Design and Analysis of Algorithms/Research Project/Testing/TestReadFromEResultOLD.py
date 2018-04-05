import ast
import matplotlib.pyplot as plt
import numpy as np
<<<<<<< HEAD:Spring 2018/Design and Analysis of Algorithms/Research Project/TestReadFromEResult.py
file = open('outfile5.txt', 'r')
=======
file = open('NewSigmoidTest.txt', 'r')
>>>>>>> 9852377bfc801fc9f4ebfa094710aae005d371ac:Spring 2018/Design and Analysis of Algorithms/Research Project/Testing/TestReadFromEResult.py
mainlist = []
for line in file:
    mainlist.append(line)
temp = []
for i in mainlist:
    temp.append(ast.literal_eval(i))
it = 0
learnrates=[0.5,1.5,2.5]
for i in temp[0][0]:
    fig = plt.figure()    
    for j in i:
        plt.plot(np.array(j[:30]))
    plt.title("Learn Rate {0}".format(learnrates[it]))
    plt.legend([15,30,50], loc='lower right')
    plt.grid(1)
    plt.show()
    it += 1

