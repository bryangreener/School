document.addEventListener("DOMContentLoaded", function(event)
{
var ck_name = /^[A-Za-z0-9 ]{1,20}$/;
var ck_email = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
var ck_password = /^[A-Za-z0-9!@#$%^&*()_]{4,20}$/;

var elements = document.querySelectorAll('input');

var btn = document.querySelectorAll('#submitButton');
console.log(btn);
NodeList.prototype.addEventListener = function(event, func){
    this.forEach(function(content, item){
        content.addEventListener(event, func);
    });
}

btn.addEventListener('click', function(event){
    event.preventDefault();
    var first   = elements[1].value;
    var last    = elements[2].value;
    var email   = elements[3].value;
    var p1      = elements[4].value;
    var p2      = elements[5].value;
    var chk     = elements[6].value;
    var errors = 0;

    if(!ck_name.test(first)){
        elements[1].value = "Invalid First Name";
        elements[1].style.backgroundColor = "LightPink";
        errors++;
    }
    if(!ck_name.test(last)){
        elements[2].value = "Invalid Last Name";
        elements[2].style.backgroundColor = "LightPink";
        errors++;
    }
    if(!ck_email.test(email)){
        elements[3].value = "Invalid Email";
        elements[3].style.backgroundColor = "LightPink";
        errors++;
    }
    if(!ck_password.test(p1)){
        elements[4].style.backgroundColor = "LightPink";
        alert("Password must be between 4 and 20 ASCII characters")
        errors++;
    }
    if(p1 != p2)
    {
        elements[4].style.backgroundColor = "LightPink";
        elements[5].style.backgroundColor = "LightPink";
        alert("Passwords Do Not Match")
        errors++;
    }
    if(!elements[6].checked)
    {
        elements[6].parentElement.style.backgroundColor = "LightPink";
        alert("Must Agree to Privacy Policy");
        errors++;
    }
    if(errors > 0)
    {
        errors = 0;
        return false;
    }
    document.getElementById('mainForm').submit();
    return true;
})
});

