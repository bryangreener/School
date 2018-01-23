# Multiple linear regression example - UN demographic data on 
# gdp, percentage urban population and fertility 

# First make sure working directory is set to the folder containing the 
# data file "UN2.csv"

UNdata = read.csv("UN2.csv")
attach(UNdata)

# display first five rows
UNdata[1:5,]

# notice that the first column is the country name, which we do not use in the analysis.
# for convenience we use these as row labels and drop the 4th column
row.names(UNdata) = UNdata[,"Locality"]
UNdata = UNdata[,-1]

UNdata[1:5,]

# Look at some Summary statistics and graphs

# summary stats for each variable
summary(UNdata)

# correlation matrix for the variables
cor(UNdata)

# scatterplot matrix for the variables (defualt for a data matrix)
plot(UNdata)




# How does fertility relate to affluence and urbanization?
# Consider the multiple linear regression of 
#   log-fertility on log-gdp and percentage urban

model1 = lm(logFertility ~ Purban + logPPgdp, data=UNdata)

summary(model1)

# Based on the results, several questions:

# 1) What hypothesis does the F-statistic test, and what does the result mean?

# 2) Are the coefficients of the explanatory variables statistically significant?

# 3) What are the effects of the explanatory variables on the response?

# sums of squares for anova
# note that the model sum-of-squares = sum of "Sum sq" for Purban and logPPgdp

anova(model1)


# Check the model: residual plot

fitted = fitted.values(model1)
resid = residuals(model1)
x11()
plot(fitted,resid,xlab="Yhat",ylab="Y - Yhat")
abline(h=0)

newspeed=data.frame(Purban=0.5,logPPgdp=1)
newspeed
predict(model1,newspeed,se.fit=TRUE,interval="confidence")
# Suppose we want a 95% confidence interval for the effect of percent urban.
# How to compute it?


# Suppose we want to know how much Purban (urbanization measure) contributed to
# the reduction in fertility after gdp is taken into account. The we can fit the model
# with Purban as the last variable and look at the sequential anova:

model2 = lm(logFertility ~ logPPgdp + Purban, data=UNdata)

summary(model2)

anova(model2)

# We see that Purban has a p-value of 0.063, and accounts for a relatively small
# portion of the variation in logFertility.

detach(UNdata)
