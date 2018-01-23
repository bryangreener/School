<?php

//listing 13.6 Checking session existence

include_once("ShoppingCart.class.php"); //file not provided.
session_start();
// always check for existence of session object before accessing it
if ( !isset($_SESSION["Cart"]) ) {
  //session variables can be strings, arrays, or objects, but
  // smaller is better
  $_SESSION["Cart"] = new ShoppingCart();
}
$cart = $_SESSION["Cart"];
?>