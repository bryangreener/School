# Example 4:

# First download "heights.txt". 
# From the R file menu select "Change dir.." to change the R working directory 
# to the folder that contains "heights.txt"


heights=read.table("heights.txt",head=TRUE) #read in data

names(heights) #check names of variables

heights         #look at entire dataset
heights[1:15,]  # look at first 15 rows of dataset


heights$Mheight    #look at only variable "Mheight"
heights$Dheight    #look at only variable "Dheight"

attach(heights) #allow direct access to variable data

Mheight    #look at only variable "Mheight"
Dheight    #look at only variable "Dheight"

hist(Mheight)                          #histogram for "Mheight" variable, R decides bin size
hist(Mheight, breaks=c(20,40,60,80)) #histogram for "Mheight" variable, specify bin cut points
hist(Mheight, breaks=c(50,54,58,62,66,70,74))  # different bins
hist(Mheight, breaks=20)               #histogram for "Mheight" variable, 20 bins of equal length
hist(Mheight)                          #histogram for "Mheight" variable, R decides on bin size

mean(Mheight)      #return mean of "Mheight"
sqrt(var(Mheight)) #return standard deviation of "Mheight"
sd(Mheight)        #return standard deviation of "Mheight"


summary(Mheight) #return five number summary of "Mheight" (also mean)

fivenum(Mheight) #return five number summary of "Mheight"


###identify outliers###
heightNumber=rownames(heights)         #stores ID number of heights
heightNumber[Mheight > 63.9 + 1.5*IQR(Mheight)] #ID numbers with Mheights that are (high) outliers

heightNumber[Mheight < 60.8 - 1.5*IQR(Mheight)] #ID numbers with Mheights that are (low) outliers


heights[194,] #look at entry for suspected outlier

heights[6,] #look at entry for suspected outlier

# These are not really outliers, they are just extreme values in a large dataset

boxplot(Mheight) #create a boxplot for "Mheight"

stem(Mheight) # create stem and leaf plot (histograms are better for large datasets!

detach(heights) #remove direct access to variable data