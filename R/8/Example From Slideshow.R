# EXAMPLE
# Your instructor claims 50% of the beads in a container are red. A random sample
# of 251 beads is selected, of which 107 are red. Calculate and interpret a 90%
# confidence interval for the proportion of red beads in the container. Use your
# interval to comment on this claim.
n=251
x=107
phat=x/n
statistic=phat
zstar=qnorm(.9+(1-.9)/2)
se=sqrt(phat*(1-phat))/n
moe=zstar*se
c(statistic-moe,statistic+moe)
# This finds 90% confidence interval for the
# porportion of red beads.