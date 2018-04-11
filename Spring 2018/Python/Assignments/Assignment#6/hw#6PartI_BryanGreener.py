# Name: Bryan Greener
# Date: 2018-04-11
# Homework : #6 Part 1

import csv

def csvopen(file):
    with open(file, "r") as entries:
        for entry in csv.reader(entries):
            yield entry

rownum = 0 #iterator for row number
for i in csvopen('people.csv'):
    if rownum != 0: # If 0, skip header row
        first, last, age  = i[0], i[1], int(i[2])
        if(age%2):
            print('{0}: {1}, {2}'.format(rownum, last, first))
        else: #Have to use last[1] since there is a space in the CSV
            print('{0}{1} is number {2}'.format(first[0], last[1], rownum))
    rownum += 1