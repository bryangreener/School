document.getElementById("loginForm").onsubmit = function(e){
  var pass = document.getElementById("password").value;
  if(pass==""){
    alert ("enter a password");
    e.preventDefault();
  }
}
  
