import ast
import matplotlib.pyplot as plt

file = open('outfile5.txt', 'r')
mainlist = []
for line in file:
    mainlist.append(line)
temp = []
for i in mainlist:
    temp.append(ast.literal_eval(i))
it = 0
learnrates=[1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0]
for i in temp[2][0]:
    fig = plt.figure()    
    for j in i:
        plt.plot(np.array(j[:30]))
    plt.title("Learn Rate {0}".format(learnrates[it]))
    plt.legend([5,10,15,20,25,30,35,40,45,50], loc='lower right')
    plt.grid(1)
    plt.show()
    it += 1


#'5','10','15','20','25','30','35','40','45','50'
