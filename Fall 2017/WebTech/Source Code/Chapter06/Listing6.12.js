function displayTheDate() {
var d = new Date();
alert ("You clicked this on "+ d.toString());
}
var element = document.getElementById('example1');
element.onclick = displayTheDate;
// or using the other approach
element.addEventListener('click',displayTheDate);
