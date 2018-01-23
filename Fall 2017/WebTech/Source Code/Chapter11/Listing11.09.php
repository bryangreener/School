<?php
//LISTING 11.9 Handling connection errors with PDO
try {
  $connString = "mysql:host=localhost;dbname=bookcrm";
  $user = "DBUSER";
  $pass = "DBPASS";
  $pdo = new PDO($connString,$user,$pass);
  //...
  }
catch (PDOException $e) {
  die( $e->getMessage() );
}

?>