<?php
// class definition must be before session_start
include_once("classes/ShoppingCartItem.class.php");
include_once("classes/ShoppingCart.class.php");

session_start();

$_SESSION["ShoppingCart"] = null;

header('Location: ' . $_SERVER['HTTP_REFERER']);
?>