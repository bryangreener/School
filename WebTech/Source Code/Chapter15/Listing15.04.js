
//listing 15.4 Adding a method named countChars to the String class

String.prototype.countChars = function (c) {
    var count=0;
    for (var i=0;i<this.length;i++) {
	if (this.charAt(i) == c)
	    count++;
    }
    return count;
}
