
//listing 15.6 jQuery code to listen for file inputs changing, all inside the document’s ready event

$(document).ready(function(){
	//set up listeners on the change event for the file items.
	$("input[type=file]").change(function(){
		console.log("The file to upload is "+ this.value);
	});
});