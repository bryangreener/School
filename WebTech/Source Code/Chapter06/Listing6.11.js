var greetingBox = document.getElementById('example1');
greetingBox.addEventListener('click', function (){alert('Good Morning')});
greetingBox.addEventListener('mouseout', function(){ alert('Goodbye')});
// IE 8
//greetingBox.attachEvent('click', alert('Good Morning'));
