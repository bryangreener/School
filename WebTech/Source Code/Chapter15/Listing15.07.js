
//listing 15.7 Using the listener technique in jQuery with on and off methods

$(document).ready(function(){
	$(":file").on("change",alertFileName); // add listener
});

// handler function using this
function alertFileName() {
    console.log("The file selected is: "+this.value);
}