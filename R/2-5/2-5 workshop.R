# create array from data
data=c(3388,389,5238,1164,1703,1699,762,2045)

# change array into matrix then display end result
datamatrix=matrix(data,nrow=4,ncol=2,byrow=TRUE)
colnames(datamatrix)=c('Full-time','Part-time')
rownames(datamatrix)=c('15-19','20-24','25-34','35 and over')
datamatrix

# part a
# displaying value from matrix
datamatrix[1,1]

# part b
row1=sum(datamatrix[1,])
row2=sum(datamatrix[2,])
row3=sum(datamatrix[3,])
row4=sum(datamatrix[4,])

col1=sum(datamatrix[,1])
col2=sum(datamatrix[,2])

# generate and display joint distribution
total=sum(data)
prop.joint=datamatrix/total
prop.joint


# part c and d
# generate and display marginal distribution for rows and columns
prop.margin.row=c(row1/total,row2/total,row3/total,row4/total)
names(prop.margin.row)=c('15-19','20-24','25-34','35 and over')
prop.margin.col=c(col1/total,col2/total)
names(prop.margin.col)=c('Full-time','Part-time')
barplot(prop.margin.row)
x11()
barplot(prop.margin.col)

# 2.126

# generate conditional distribution
prop1=datamatrix[1,]/row1
prop2=datamatrix[2,]/row1
prop3=datamatrix[3,]/row1
prop4=datamatrix[4,]/row1

names(prop1)=c('Full-time','Part-time')
names(prop2)=c('Full-time','Part-time')
names(prop3)=c('Full-time','Part-time')
names(prop4)=c('Full-time','Part-time')

x11()
barplot(prop1,main="15-19")
x11()
barplot(prop2,main="20-24")
x11()
barplot(prop3,main="25-34")
x11()
barplot(prop4,main="35+")

table=cbind(prop1,prop2,prop3,prop4)
colnames(table)=c('15-19','20-24','25-34','35+')

x11()
barplot(table,beside=TRUE,col=heat.colors(2),ylab="proportion")
legend("topright",c('Full-Time','Part-Time'),cex=1.5,bty="n",fill=heat.colors(2));

prop.table(datamatrix)
table1=prop.table(datamatrix,margin=1)
table2=prop.table(datamatrix,margin=2)
x11()
barplot(table1,beside=TRUE,col=heat.colors(4),ylab="proportion")
legend("topright",c('15-19','20-24','25-34','35+'),cex=1.5,bty="n",fill=heat.colors(4));