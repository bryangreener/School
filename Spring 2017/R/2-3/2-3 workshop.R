#read file
beers = read.csv("beer.csv")
attach(beers)
names(beers)
# set variables for the reg line formula
xbar=mean(PercentAlcohol)
xbar
ybar=mean(Carbohydrates)
ybar
sx=sd(PercentAlcohol)
sx
sy=sd(Carbohydrates)
sy
r=cor(PercentAlcohol,Carbohydrates)
r
#calculate regression line
slope=r*sy/sx
intercept=ybar-slope*xbar
#yhat=ybar-slope*xbar

#plot data and display reg line
plot(PercentAlcohol,Carbohydrates)
fit=lm(Carbohydrates ~ PercentAlcohol)
abline(fit,col='green')

#remove outlier
which(PercentAlcohol==min(PercentAlcohol))
PercentAlcohol[57]
beerOutlier=beers[-57,]
detach(beers)

#attach new data set
attach(beerOutlier)

plot(PercentAlcohol,Carbohydrates)
cor(PercentAlcohol,Carbohydrates)

#set variables for reg line calc
xbar=mean(PercentAlcohol)
xbar
ybar=mean(Carbohydrates)
ybar
sx=sd(PercentAlcohol)
sx
sy=sd(Carbohydrates)
sy
r=cor(PercentAlcohol,Carbohydrates)
r

#calculate regression line
slope=r*sy/sx
intercept=ybar-slope*xbar

#add reg line to new plot
fit=lm(Carbohydrates ~ PercentAlcohol)
abline(fit,col='blue')

detach(beerOutlier)