#9.28


# Data from 1991 General Society Survey on Gender and Political
# party affiliation from
# Agresti, Introduction to Categorical Data Analysis
#
#          Dem  Ind  Rep
#    F      279  73  225
#    M      165  47  191
#
#

# Enter the data into a 2 x 3 matrix

data = c(279,73,225,165,47,191)
data
datm = matrix(data,nrow=2,ncol=3,byrow=TRUE)
datm

# Label Row and COlumn names
rownames(datm)=c('Full','Part')
colnames(datm)=c('Male','Female')
datm

# Marginal distribution for male and female
col1=sum(datm[,1])
col2=sum(datm[,2])
total=sum(datm)
vec1=c(col1,col2)/total
names(vec1)=c('Male',Female')
barplot(vec1)

# PART B conditional distribution of status for male/female
con.male=datm[,1]/col1
con.female=datm[,2]/col2
con=cbind(con.male,con.female)
barplot(con,beside=TRUE,ylab="proportion")






# Compute row totals so we can get the condition distributions by gender
row1 = sum(datm[1,])
row2 = sum(datm[2,])
row1
row2

# Here are the conditional party distributions for females and for males

propf = datm[1,]/row1
propm = datm[2,]/row2

propf
propm

# add labels
names(propf)=c("D","I","R")
names(propm)=c("D","I","R")

# Simple graphs
barplot(propf, main="F")
barplot(propm, main="M")

# Is there a significant association between party and gender?
# Let's test for independece of these two variables:

datm
#H_0: No association between status and gender
#H_a: THere is an association between sattus and gender
chisq.test(datm,correct=F) # Need to add the correct=F

result = chisq.test(datm)
result
# Reject H_0 because pvalue < 0.05
# There is an association between gender and status


attributes(result)

result$expected     # expected table

residuals(result)   # Pearson residuals = (observed - Expected)/sqrt(Expected)

# If cell counts are two small we can simulate from the independence model to
# geta  small sample significance test:

chisq.test(datm,simulate.p.value=TRUE)


#9.49
# Goodness of fit test
# H_0: p1=0.43, p2=0.35, p3=0.15, p4=0.07
# H_a: At least one of these is different

counts = c(79,83,26,12)
counts

# test the null hypothesis that the cell probabilities all equal 1/6
resultd = chisq.test(counts, p=c(0.43,0.35,0.15,0.07))

resultd
# Since pvalue is greater than 0.05 we fail to reject H_0

resultd$expected

resultd$observed

resultd$residual

#10.22,23,24,26
data=read.csv("ibi.csv")
attach(data)
boxplot(IBI)
# Data is skewed to the left
summary(IBI)

hist(Area)
# Data is skewed to the right

plot(Area,IBI)
# Roughly linear and positive.
# H_0: There is no association between two variables. Slope is 0
# H_a: There is an association. Slope != 0

fit=lm(IBI~Area) # IBI is Response Area is Independent
summary(fit)
# b0 is 52.92. Intercept Estimate
# b1 is 0.46. Area Estimate
#IBI=52.92+0.46Area
# We reject the null hypothesis because p value is 0.001.
residuals=residuals(fit)
plot(Area,residuals)
# There is no pattern and most values are scattered around 0
# For the forest variable, just replace all Area with Forest.
# For 10.24, since pvalue for forest is greater than 0.05, we want to
# use area since its pvalue is 0.001. Compare IBI with Area
# Can also compare response variables using R-value. Higher value is better.

# Chapter 11 Multiple Linear Regression
fit=lm(IBI~Area+Forest)
summary(fit)
# IBI=40.62+0.569Area+0.233Forest Multiple Linear Equation
# According to this summary, both Area and Forest are statitically significant
# in this dataset.
# Area and Forest are both associated with IBI in the multiple model because
# both pvalues are smaller than 0.05. 0.00004 and 0.0015
# Predict IBI when Area=10 and Forest=20

newdata = data.frame(Area=10,Forest=20)
newdata   # see what the new data looks like
predict(fit, newdata, se.fit=TRUE, interval="confidence")
# From lower to upper is 95% confidence interval.
predict(fit, newdata, se.fit=TRUE, interval="prediction")
# Lower to upper is 95% prediction interval.
detach(data)



# Chapter 12
data2=read.csv("kudzu.csv")
attach(data2)
boxplot(BMD~Treatment)
# Control and low dose are very similar but high dose is different.
fit=lm(BMD~Treatment) # BMD is response. Treatment is interesting
anova(fit)
# H_0: All means are same
# H_a: Some of them are different
# After the Anova test, we reject H_0 because pvalue is smaller than 0.05

pairwise.t.test(BMD,Treatment,p.adjust="bonferroni")

# Based on this test, we conclude that there are differences between high
# dose and control. There are also differences between high and low dose
# because the corresponding pvalues are 0.0107 and 0.0022.




