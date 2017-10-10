
//listing 15.18 Hidden iFrame technique to upload files

$(document).ready(function() {
	// set up listener when the file changes
	$(":file").on("change",uploadFile);
	// hide the submit buttons
	$("input[type=submit]").css("display","none");
});
// function called when the file being chosen changes
function uploadFile () {
    // create a hidden iframe
    var hidName = "hiddenIFrame";
    $("#fileUpload").append("<iframe id='"+hidName+"' name='"+hidName+"'style='display:none' src='#' ></iframe>");
    // set form’s target to iframe
    $("#fileUpload").prop("target",hidName);
    // submit the form, now that an image is in it.
    $("#fileUpload").submit();
    // Now register the load event of the iframe to give feedback
    $('#'+hidName).load(function() {
	    var link = $(this).contents().find('body')[0].innerHTML;
	    // add an image dynamically to the page from the file just uploaded
	    $("#fileUpload").append("<img src='"+link+"' />");
	});
}