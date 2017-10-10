var hourOfDay=10; // var to hold hour of day, set it later...
var greeting; // var to hold the greeting message.
if (hourOfDay > 4 && hourOfDay < 12){
// if statement with condition
  greeting = "Good Morning";
}
else if (hourOfDay >= 12 && hourOfDay < 20){
// optional else if
  greeting = "Good Afternoon";
}
else{ // optional else branch
  greeting = "Good Evening";
}

//Not in official Listing.
document.write(greeting);
