
//listing 15.13 jQuery to asynchronously get a URL and outputs when the response arrives

$.get("/vote.php?option=C", function(data,textStatus,jsXHR) {
	if (textStatus=="success") {
	    console.log("success! response is:" + data);
	}
	else {
	    console.log("There was an error code"+jsXHR.status);
	}
	console.log("all done");
});