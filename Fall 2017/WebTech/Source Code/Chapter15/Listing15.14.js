
//listing 15.14 Modular jQuery code using the jqXHR object

var jqxhr = $.get("/vote.php?option=C");
jqxhr.done(function(data) { console.log(data);});
jqxhr.fail(function(jqXHR) { console.log("Error: "+jqXHR.status); });
jqxhr.always(function() { console.log("all done"); });