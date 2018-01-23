
//listing 15.16 Adding headers to an AJAX post in jQuery

$.ajax({ url: "vote.php",
	    data: $("#voteForm").serialize(),
	    async: true,
	    type: post,
	    headers: {"User-Agent" : "Homebrew JavaScript Vote Engine agent",
		"Referer": "http://funwebdev.com"
		}
});