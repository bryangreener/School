# 1
# Assign xbar and sigma
xbar=8
sigma=3
# a)
# Find probability that value is 6<x<9
pnorm(9,8,3)-pnorm(6,8,3)
# b)
# Generate sample data with significant number of values
sample=rnorm(10000000,8,3)
# Find .95 percentile of sample data
quantile(sample,.95)
# c)
# Create sample data with 20 values
srs=rnorm(20,8,3)
# Calculate Z value
z=((mean(srs)-8)/(sigma/sqrt(20)))
#---------------------------------
# 2
# Assign standard values
mu0=8
sigma=0.024
n=16
xbar=8.10
# a)
# Perform T Test for CI=95%
zstar.95=qnorm(.95+(1-.95)/2) #critical value
moe.95=zstar.95*sigma/sqrt(n) #margin of error
CI.95=c(xbar-moe.95,xbar+moe.95)
CI.95
# b)
# Calculate Test Statistic and pValue
z=(xbar-mu0)/(sigma/sqrt(n)) 
pvalue=2*pnorm(z)
pvalue
# c)
# Calculate number of samples needed to fit moe=0.005
zstar.95=qnorm(0.95+(1-0.95)/2)
((zstar.95*sigma)/0.005)^2
#---------------------------------
# 3
# Create array
data=c(108.6,93.9,94.2,99.9,99.5,94.3,103.1,108.0,102.6,110.7,107.3,95.4)
n=length(data)
# a)
# Assign values for basic computation
xbar=mean(data)
mu0=100
sigma=sd(data)
# Calculate Test Statistic
z=(xbar-mu0)/(sigma/sqrt(n))
# T Test given no CI, testing Alt hypothesis 
t.test(data,mu=100,alternative="greater")# verify values
# b)
# T Test given CI=90%
t.test(data,mu=100,conf.level=0.9)
#---------------------------------
# 4
# a)
# Import CSV, check headers, and attach
data4=read.csv("seventhgrade.csv")
names(data4)
attach(data4)
# Gender 1=male 2=female
# Assign IQ values for each group to separate arrays
male=c(IQ[Gender==1])
female=c(IQ[Gender==2])
# T Test given no CI
t.test(female,male)
# b)
# T Test given CI=80%
t.test(female,male,conf.level=0.8)
# Detach dataset
detach(data4)
































