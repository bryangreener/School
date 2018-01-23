<?php

//Listing 16.2 PHP functions to insert and select a record using password hashing

//Insert the user with the password being hashed by MD5 first.
function insertUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password","Login");
  $sql = "INSERT INTO Users(Username,Password) VALUES('$username',MD5('$password'))";
  mysqli_query($link, $sql); //execute the query
}
//Check if the credentials match a user in the system with MD5 hash
function validateUser($username,$password){
  $link = mysqli_connect("localhost", "my_user", "my_password", "Login");
  $sql = "SELECT UserID FROM Users WHERE Username='$username' AND Password=MD5('$password')";
  $result = mysqli_query($link, $sql); //execute the query
  if($row = mysqli_fetch_assoc($result)){
    return true; //record found, return true.
  }
  return false; //record not found matching credentials, return false
}

?>