height=read.table("heights.txt",header=T,sep=" ")
attach(height)
names(height)
plot(Mheight,Dheight)

cor(Mheight,Dheight)

xbar=mean(Mheight)
ybar=mean(Dheight)
sx=sd(Mheight)
sy=sd(Dheight)
r=cor(Mheight,Dheight)
r
slope=r*sy/sx
intercept=ybar-slope*xbar
slope
intercept #=29.917

#least square regression line is yhat=29.917+0.5Mheight
#What is expected Dheight when Mheight is 65"
29.917+0.54*65

fit=lm(Dheight ~ Mheight) #response var first when using lm function
summary(fit)
abline(fit, col='red')
fit=lm(Mheight ~ Dheight)
abline(fit, col='green')
residuals(fit)


