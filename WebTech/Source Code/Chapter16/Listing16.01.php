<?php

//Listing 16.1 PHP functions to insert and select a record with plaintext storage

//Insert the user with the password
function insertUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password", "Login");
  $sql = "INSERT INTO Users(Username,Password) VALUES('$username','$password')");
mysqli_query($link, $sql); //execute the query
}
//Check if the credentials match a user in the system
function validateUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password","Login");
  $sql = "SELECT UserID FROM Users WHERE Username='$username' AND Password='$password'";
  $result = mysqli_query($link, $sql); //execute the query
  if($row = mysqli_fetch_assoc($result)){
    return true; //record found, return true.
  }
  return false; //record not found matching credentials, return false
}

?>