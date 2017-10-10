
//listing 15.3 The Die pseudo-class using the prototype object to define methods

// Start Die Class
function Die(col) {
    this.color=col;
    this.faces=[1,2,3,4,5,6];
}
Die.prototype.randomRoll = function() {
    var randNum = Math.floor((Math.random() * this.faces.length) + 1);
    return this.faces[randNum-1];
};
// End Die Class