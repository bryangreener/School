# 5.18
# 	Given population mean and standard deviation
mu=19.2
sigma=5.1
# a
n=1 # We only have one observation in sample data
# 	Find probability that this observation is greater than 23
#	P(xbar>23)
1-pnorm(23,mu,sigma)
# b 
muxbar=mu
sigmaxbar=sigma/sqrt(25)
# c
#	P(xbar>23)
1-pnorm(23,muxbar,sigmaxbar)
#-----------------------------
# 5.73
#	n=? 
p=.7555 #Probability of success
#	a
n=12 # 12 observed vehicles
# 	Find probability that more than half carry only one person P(X>=7)
#	Same as 1-P(X<=6)=1-pbinom(6,n,p)
1-pbinom(6,n,p)
#	b
n=80 # Given 80 observes vehicles
#	P(X>=41)=1-P(X<=40)
1-pbinom(40,n,p)
#-----------------------------
# 5.76
p=3/4
#	a
#	P(X=9) Proability that 9 of 12 are success
n=12
dbinom(9,n,p)
#	b
n=120
# 	Find the mean of X. n*p (mean of average)
n*p
#	Expected number of successes among 120 is 90
#	c
#	P(X>=80) = 1-P(X<=79)
1-pbinom(79,n,p)
#-----------------------------
# 6.11
#	Sample mean is 50
xbar=50
#	Std Dev is assumed to be 7
sigma=7 # Population std dev
#	a
#	Compute 95% confidence interval when n=10,20,40,80
#	statistic +- critical value * sigma/sqrt(n)
#	critical value is the same as zstar
#	statistic is the same as xbar
zstar=qnorm(0.95+(1-0.95)/2)
n=10
n=20
n=40
n=80
c(xbar-zstar*sigma/sqrt(n),xbar+zstar*sigma/sqrt(n))
#----------------------------
# 6.12
n=49
xbar=70
sigma=14

C=0.8 # Confidence Level
zstar=qnorm(C+(1-C)/2)
moe=zstar*sigma/sqrt(n) # Margin of error
c(xbar-moe, xbar+moe)
#	In order to have more confidence, you need to have a higher
#	confidence level to capture more values.
#----------------------------
# 6.68
#	H_0: mu=8.9 vs H_a: mu>8.9
#	One Sided
sigma=2.5
n=6
xbar=10.2
z=(xbar-8.9)/(sigma/sqrt(n)) # I guess we dont need parenthesis there...
1-pnorm(z)
#	Fail to reject H0 because pvalue is greater than 0.05
#----------------------------
# 7.28
#	b
#	H_0: mu = 3421.7 vs H_a: mu < 3421.7
#	Compute test statistic
s=987 # Sample std dev
xbar=3077
n=114
#	b
t=(xbar-3421.7)/(s/sqrt(n))
df=n-1
pt(t,df)
#	Reject the null hypothesis since it is < 0.05
#	c
tstar=qt(0.975,df)
moe=tstar*(s/sqrt(n))
c(xbar-moe,xbar+moe)
#----------------------------
# 7.36
data=read.csv("pickcount.csv")
attach(data)
names(data)
hist(PickCount)
qqnorm(PickCount)
mean(PickCount)
sd(PickCount)
t.test(PickCount,conf.level=0.9) # Need conf.level to change from 95%
#	H_0: mu=950 vs H_a: mu<950
t.test(PickCount,mu=950,alternative="less")
#	Reject null (H_0) because pvalue is less than 0.05
detach(data)
#----------------------------
# 7.82
data=read.csv("ewtreediameter.csv")
attach(data)
names(data)
summary(data)
boxplot(dbh ~ ew)
qqnorm(dbh[ew=="e"])
qqnorm(dbh[ew=="w"])
#	H_0: mu_e = mu_w vs H_a: mu_e != mu_w
t.test(dbh[ew=="e"], dbh[ew=="w"])
#	pvalue is less than 0.05 so reject H_0 (nul hypothesis)
detach(data)
























































































































