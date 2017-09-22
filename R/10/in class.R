
# Illustrate linear regression analysis with the
# "cars" data on speed and stopping distance.
# These data are preloaded in the R package.
# Copy commands into R to see how they work.

# information about the data:
?cars

# view the data
cars

# variables in the dataset
names(cars)

# attach the data
attach(cars)

# scatterplot with least squares line
plot(speed, dist)
abline(lm(dist ~ speed))

# fit the linear model and save as an object

LinearModel.1 = lm(dist ~ speed, data=cars)

# see the model summary including coeeficient estimates,
# standard errors, t-statistics etc.
summary(LinearModel.1) # ADDITION: test std is B0, Error is B1

# test if slope is 0 or not
H_0: beta1=0 vs H_a: beta1 !=0
b1=3.9324
seb1=0.4155
t=b1/seb1
t
n=50
df=n-2
1-pt(t,df) # pvalue
#reject null because pvalue is less than 0.05

# get the residuals and fitted values

resid = residuals(LinearModel.1)
pred = fitted.values(LinearModel.1)

# plot residuals versus speed
x11()
plot(speed, resid)
abline(h=0)

# plot residuals versus fitted values
x11()
plot(pred, resid)
abline(h=0)

# confidence interval for the mean stopping distance 
# for speed=60

newspeed = data.frame(speed=60)
newspeed   # see what the new data looks like
predict(LinearModel.1, newspeed, se.fit=TRUE, interval="confidence")

# prediction interval for new observation at speed=60
predict(LinearModel.1, newspeed, se.fit=TRUE, interval="prediction")

# note that the prediction interval is wider than the 
# confidence interval for the mean.

# get the analysis of variance statistics:
anova(LinearModel.1)

# detach the data

detach(cars)










