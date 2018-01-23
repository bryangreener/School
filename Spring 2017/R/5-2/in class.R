#Chi Squared Distribution
# n=1000000, k=1 (samples = 1000000, population mean = 1)
X=rchisq(1000000, 1)
hist(X)
#Variance = 2k. Standard dev = 2
sd(X)^2

xmeans=NULL                    #intialize vector to store results from repeated sampling
for(i in 1:10000){               #run the sampling 100 times and save the results
xmeans=c(zmeans,mean(sample(X,100)))
}
mean(xmeans)
sd(zmeans)
x11()
hist(zmeans)













