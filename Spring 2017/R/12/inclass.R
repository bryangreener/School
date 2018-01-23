# Example: One-way ANOVA model (4 treatments)

# Just copy and paste these commands into the R command
# window to see the results.

# Analysis of data on coagulation times 
# for blood drawn from 24 animals
# randomly allocated to four diets.
# See Box, Hunter & Hunter, Ch 4, p.134.

# "scan" the response data into a vector
coag <- scan()
62 60 63 59 63 59
63 67 71 64 65 66
68 66 71 67 68 68
56 62 60 61 63 64

### H0: mua=mub=muc=mud=0
### Ha: At least two of these are different


# Create the "factor" vector of treatments.
# Make sure the order matches the order
# of the responses.
diet <- factor(rep(x=c("A","B","C","D"), times=c(6,6,6,6)))

# display the data
data.frame(diet, coag)

boxplot(coag~diet)  # box plot comparing diets

# useful function - split data in first argument by factor in second argument
split(coag,diet)

# apply sample means function to split data
sapply(split(coag,diet),"mean")

# within group sample standard deviations
sapply(split(coag,diet),"sd")

# Conditions look ok for pooling - the largest sd is smaller than
# twice the smallest sd

# Here's a quick way to the get the ANOVA table using the 
# lm function and anova function
model1 = lm(coag ~ diet) 
###diet is the interesting treatment. coag is outcome
anova(model1)
###since F is large and P is smaller than 0.05, reject the null hypothesis


# Multiple comparisons of means

# Determine which pairs of means differ using pariwise t statistic
pairwise.t.test(coag, diet, p.adjust="bonferroni")
### This gives a 3x3 table where the values are the Pvalues comparing x to y
### mua x mud fails to reject H0
### muc x mub fails to reject H0
### All others reject H0



# Simultaneous confidence intervals for differences between means
results = aov(coag ~ diet)
TukeyHSD(results, conf.level = 0.95) 

