
//listing 21.1 Including Facebook JS API and creating a FB object to enable plugins with jQuery

$(document).ready(function() {
	$.ajaxSetup({ cache: true });
	$.getScript('//connect.facebook.net/en_UK/all.js', function(){
		FB.init({appId: App_ID,//<-- Change this to a valid ID (register with facebook) to make this code work!!!!!!!
			    //channelUrl: "http://funwebdev.com" <--Optional.
			    status:true, //status: check fb login
			    xfbml:true //parse for FB plugins
			    });
		$('#loginbutton,#feedbutton').removeAttr('disabled');
		FB.getLoginStatus(updateStatusCallback);
	});
});