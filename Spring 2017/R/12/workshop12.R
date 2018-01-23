data=read.csv("kudzu.csv")
fix(data)
attach(data

# A
boxplot(BMD~Treatment,data)

# B
sapply(split(BMD,Treatment),"mean")
sapply(split(BMD,Treatment),"sd")
group1=BMD[Group==1]
group2=BMD[Group==2]
group3=BMD[Group==3]
qqnorm(group1,main="Control")
qqnorm(group2,main="Low Dose")
qqnorm(group3,main="High Dose")

# C
model=lm(BMD~Treatment)
anova(model)

# D
pairwise.t.test(BMD, Treatment, p.adjust="bonferroni")

detach(data)










