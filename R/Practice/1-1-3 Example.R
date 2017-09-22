# Prussian horsekick data set
k=c(0,1,2,3,4)		#set k vector
x=c(109,65,22,3,1)	#set x vector
p=x/sum(x)			#relative frequencies
cat("Print P ",p)

r=sum(k*p)			#mean
v=sum(x*(k-r)^2)/199	#variance
print(r)
print(v)
f=dpois(k,r)
print(cbind(k,p,f))