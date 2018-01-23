<?php

//Listing 12.2 Procedural approach to error handling

$connection = mysqli_connect(DBHOST, DBUSER, DBPASS, DBNAME);
$error = mysqli_connect_error();
if ($error != null) {
  // handle the error
  //...
}

?>