data=read.csv("final.csv")
attach(data)
names(data)
fix(data)

# 1
fit1a=lm(Sales~Price)
summary(fit1a)
fit1a
predict(fit1a, se.fit=TRUE, interval="confidence")
newdata = data.frame(Price=150)
newdata
predict(fit1a, newdata, se.fit=TRUE, interval="confidence")
t.test(Price,Sales)

# 2
fit2a=lm(Sales~Price+Advertising+Population)
summary(fit2a)
newdata2 = data.frame(Price=150,Advertising=5,Population=270)
newdata2
predict(fit2a, newdata2, se.fit=TRUE, interval="confidence")

# 3
fit3a=lm(Sales~ShelveLoc)
anova(fit3a)
x1=Sales[ShelveLoc=="Bad"]
x2=Sales[ShelveLoc=="Medium"]
x3=Sales[ShelveLoc=="Good"]
com3=cbind(x1,x2,x3)
boxplot(x1,x2,x3,beside=TRUE,xlab="Bad,Medium,Good")
anova(fit3a)
pairwise.t.test(Sales,ShelveLoc,p.adjust="bonferroni")

#4
table(US,Income)
chisq.test(table(US,Income))
























