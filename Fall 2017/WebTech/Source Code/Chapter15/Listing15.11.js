
//listing 15.11 Using wrap() with a callback to create a unique div for every matched element

$(".contact").wrap(function(){
	return "<div class='galleryLink' title='Visit " + $(this).html()+
	    "'></div>";
});