data=read.csv("ibi.csv")
attach(data)
names(data)
fit1=lm(IBI~Forest)
summary(fit1)

fit2=lm(IBI~Area)
summary(fit2)
detach(data)












#ignore everything below.
fix(data)
summary(data)
attach(data)
# a
summary(Area)
summary(IBI)
# b
plot(IBI,Area)
abline(lm(Area ~ IBI))
# c
LinearModel.1 = lm(IBI~Area)
summary(LinearModel.1)
# d
# H0: beta1=0 Ha: beta1!=0
# e
anova(LinearModel.1)


# f
resid = residuals(LinearModel.1)
pred = fitted.values(LinearModel.1)

# plot residuals versus
x11()
plot(Area,resid)
abline(h=0)

# plot residuals versus fitted values
x11()
plot(pred, resid)
abline(h=0)

# 10.23
# a
fix(data)
summary(Forest)
summary(IBI)
# b
plot(IBI,Forest)
abline(lm(Forest ~ IBI))
# c
LinearModel.1 = lm(Forest ~ IBI)
summary(LinearModel.1)
# d
# H0: beta1=0 Ha: beta1!=0
# e
anova(LinearModel.1)


# f
resid = residuals(LinearModel.1)
pred = fitted.values(LinearModel.1)

# plot residuals versus
x11()
plot(Forest,resid)
abline(h=0)

# plot residuals versus fitted values
x11()
plot(pred, resid)
abline(h=0)

# 10.26
newarea = data.frame(Area=60)
newarea   # see what the new data looks like
predict(LinearModel.1, newarea, se.fit=TRUE, interval="confidence",conf.level=.6)

# prediction interval for new observation at speed=60
predict(LinearModel.1, newarea, se.fit=TRUE, interval="prediction")
anova(LinearModel.1)

























