
//listing 15.2 Die pseudo-class with an internally defined method

function Die(col) {
    this.color=col;
    this.faces=[1,2,3,4,5,6];
    // define method randomRoll as an anonymous function
    this.randomRoll = function() {
	var randNum = Math.floor((Math.random() * this.faces.length)+ 1);
	return this.faces[randNum-1];
    };
}