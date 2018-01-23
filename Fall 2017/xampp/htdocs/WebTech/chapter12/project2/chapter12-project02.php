

<!DOCTYPE html>
<html lang="en">
<head>
 <meta charset="utf-8">
 <meta name="viewport" content="width=device-width, initial-scale=1.0">
 <title>Chapter 12</title>

 <!-- Bootstrap core CSS -->
 <link href="bootstrap3_defaultTheme/dist/css/bootstrap.css" rel="stylesheet">

 <!-- Custom styles for this template -->
 <link href="chapter12-project02.css" rel="stylesheet">

 <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
 <!--[if lt IE 9]>
   <script src="bootstrap3_defaultTheme/assets/js/html5shiv.js"></script>
   <script src="bootstrap3_defaultTheme/assets/js/respond.min.js"></script>
 <![endif]-->
</head>

<body>

<script src="validate.js" type="text/javascript"><script>
<script>
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
    return true;
})
});
</script>


<?php include 'art-header.inc.php' ?>

<div class="container">


   
   <div class="row">
      <div class="col-md-3">
      
<div class="panel panel-default">
  <div class="panel-heading">Account</div>
  <div class="panel-body">

  <ul class="nav nav-pills nav-stacked">
  <li><a href="#">Login</a></li>
  <li class="active"><a href="#">Register</a></li>

</ul>  
  
  
  </div>
</div>      
        
      </div>
      <div class="col-md-9">
      
      
<form role="form" class="form-horizontal" action="art-form-process.php" id="mainForm" method="post">
   <div class="page-header">
      <h2>Register Account</h2>
      <p>If you already have an account with us, please login at the login page.</p>   
   </div>


     <div class="form-group">
       <label for="first" class="col-md-3 control-label">First Name</label>
       <div class="col-md-9">
       <input type="text" class="form-control" name="first" >
       </div>
     </div>
     <div class="form-group">
       <label for="last" class="col-md-3 control-label">Last Name</label>
       <div class="col-md-9">
       <input type="text" class="form-control" name="last" >
       </div>
     </div>
     <div class="form-group">
       <label for="email" class="col-md-3 control-label">Email</label>
       <div class="col-md-9">
       <input type="email" class="form-control error" name="email">
       </div>
     </div>        


     <div class="form-group">
       <label for="password1" class="col-md-3 control-label">Password</label>
       <div class="col-md-9">
       <input type="password" class="form-control" name="password1">
       </div>
     </div>
     <div class="form-group">
       <label for="password2" class="col-md-3 control-label">Password Confirm</label>
       <div class="col-md-9">
       <input type="password" class="form-control" name="password2">
       </div>
     </div>
  <div class="form-group">
    <div class="col-md-offset-3 col-md-9">
      <div class="checkbox">
        <label>
          <input type="checkbox" name="privacy" > I agree to the <a href="#">privacy policy</a>
        </label>
      </div>
    </div>
  </div>     


  <div class="form-group">
    <div class="col-md-offset-3 col-md-9">
      <button type="submit" id="submitButton" class="btn btn-primary">Register</button>
    </div>
  </div>
   
   </form>  
</div>  
</div> 

</div>  <!-- end container -->

<?php include 'art-footer.inc.php' ?>

 <!-- Bootstrap core JavaScript
 ================================================== -->
 <!-- Placed at the end of the document so the pages load faster -->
 <script src="bootstrap3_defaultTheme/assets/js/jquery.js"></script>
 <script src="bootstrap3_defaultTheme/dist/js/bootstrap.min.js"></script>    
</script>

</body>
</html>
