#6.11
xbar=50
sigma=7
n=10
n=20
n=40
n=80
zstar = qnorm(.975,0,1)
moe=zstar*sigma/sqrt(n)
CI=c(xbar-moe,xbar+moe)
CI


#6.12
xbar=70
sigma=14
n=49
confidence=.80
confidence=.90
confidence=.95
confidence=.99
zstar=qnorm(confidence+(1-confidence)/2,0,1)
moe=zstar*sigma/sqrt(n)
CI=c(xbar-moe,xbar+moe)
CI

#6.28c
mpg = c(41.5,50.7,36.6,37.3,34.2,45.0,48.0,43.2,47.7,42.2,
43.2,44.6,48.4,46.4,46.8,39.2,37.3,43.5,44.3,43.3)
sigma=3.5
xbar=mean(mpg)
n=length(mpg)
zstar.95=qnorm(.95+(1-.95)/2)
moe.95=zstar.95*sigma/sqrt(n)
CI.95=c(xbar-moe.95,xbar+moe.95)
CI.95










