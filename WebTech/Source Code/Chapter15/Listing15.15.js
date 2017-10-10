
//listing 15.15 A raw AJAX method code to make a post

$.ajax({ url: "vote.php",
	    data: $("#voteForm").serialize(),
	    async: true,
	    type: post
});