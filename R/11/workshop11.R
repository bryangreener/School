data=read.csv("IBI.csv")
attach(data)
summary(data)
cor(data)
model1=lm(IBI~Area+Forest,data=data)
summary(model1)

newvalue=data.frame(Area=50,Forest=10)
newvalue
predict(model1,newvalue,se.fit=TRUE,interval="prediction" pred.level=0.95)

anova(model1)
detach(data)
















