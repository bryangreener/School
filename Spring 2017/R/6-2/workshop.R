#6.68
u=8.9
sigma=2.5
xbar=10.2
n=6

# Test H0: U= 8.9, Ha: U>8.9
z=(xbar-u)/(sigma/sqrt(n))
z
pnorm(-abs(z))

#6.71
mpg=c(5,6.5,-0.6,1.7,3.7,4.5,8,2.2,4.9,3,
4.4,0.1,3,1.1,1.1,5,2.1,3.7,-0.6,-4.2)
n=20
sigma=3
u=0
z=(mean(mpg)-u)/(sigma/sqrt(n))
z
pvalue=2*pnorm(-abs(z))
pvalue











