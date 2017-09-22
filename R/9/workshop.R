data=rbind(c(2719,2991),c(535,680))
# GENERATE MARGINAL DISTRIBUTION
total=sum(data)
#row1=sum(data[1,])
#row2=sum(data[2,])
col1=sum(data[,1])
col2=sum(data[,2])
#prop.margin.row=c(row1/total,row2/total)
prop.margin.col=c(col1/total,col2/total)
names(prop.margin.col)=c("Male","Female")
#barplot(prop.margin.row)
#x11()
barplot(prop.margin.col)
prop.margin.col

# GENERATE CONDITIONAL DISTRIBUTION
prop1=data[,1]/col1
prop2=data[,2]/col2
names(prop1)=c("Full-Time","Part-Time")
names(prop2)=c("Full-Time","Part-Time")
barplot(prop1)
barplot(prop2)

# FIND EXPECTED VALUES FOR ROW 1 CELLS

expected1=(sum(data[1,])*sum(data[,1]))/total
expected1
expected2=(sum(data[1,])*sum(data[,2]))/total
expected2

# CALCULATE P-VALUE

chisq.test(data)

# 9.49
# CALCULATE CHISQ TEST
counts=c(79,83,36,12)
result=chisq.test(counts,p=c(.43,.35,.15,.07))
result