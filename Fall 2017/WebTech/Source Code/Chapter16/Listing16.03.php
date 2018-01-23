<?php

//Listing 16.3 PHP functions to insert and select a record using password hashing and salting

function generateRandomSalt(){
  return base64_encode(mcrypt_create_iv(12), MCRYPT_DEV_URANDOM));
}
//Insert the user with the password salt generated, stored, and
//password hashed
function insertUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password",
                         "Login");
  $salt = generateRandomSalt();
  $sql = "INSERT INTO Users(Username,Password,Salt)
VALUES('$username',MD5('$password$salt'), '$salt')");
mysqli_query($link, $sql); //execute the query
}
//Check if the credentials match a user in the system with MD5 hash
//using salt
function validateUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password",
                         "Login");
  $sql = "SELECT Salt FROM Users WHERE Username='$username'";
  $result = mysqli_query($link, $sql); //execute the query
  if($row = mysqli_fetch_assoc($result)){
    //username exists, build second query with salt
    $salt = $row['Salt'];
    $saltSql = "SELECT UserID FROM Users WHERE Username='$username' AND Password=MD5('$password$salt')";
    $finalResult = mysqli_query($link, $saltSql);
    if($finalrow = mysqli_fetch_assoc($finalResult))
      return true; //record found, return true.
  }
  return false; //record not found matching credentials, return false
}

?>