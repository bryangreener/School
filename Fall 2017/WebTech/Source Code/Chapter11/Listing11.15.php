<?php

//Listing 11.15 Integrating user input into a query (first attempt)
$from = $_POST['old'];
$to = $_POST['new'];
$sql = "UPDATE Categories SET CategoryName='$to' WHERE CategoryName='$from'";
$count = $pdo->exec($sql);

?>