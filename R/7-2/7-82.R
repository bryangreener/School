data=read.csv("ewtreediameter.csv")
attach(data)
fix(data)
dbh
groupnum

# A
boxplot(dbh ~ groupnum)

# B
qqnorm(dbh[groupnum=="1"])
x11()
qqnorm(dbh[groupnum=="2"])

# C
# Find mean of group 1 and group 2
x1=dbh[groupnum=="1"]
x2=dbh[groupnum=="2"]

m1 = mean(x1)
s1 = sd(x1)
m2 = mean(x2)
s2 = sd(x2)
n1 = length(x1)
n2 = length(x2)
g1 = "East"
g2 = "West"
data.frame(g1,n1,m1,s1)
data.frame(g2,n2,m2,s2)

# D, E
t.test(x1,x2)









