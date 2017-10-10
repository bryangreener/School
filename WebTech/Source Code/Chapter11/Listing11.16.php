<?php

//Listing 11.16 Sanitizing user input before use in an SQL query
$from = $pdo->quote($from);
$to = $pdo->quote($to);
$sql = "UPDATE Categories SET CategoryName=$to WHERE CategoryName=$from";
$count = $pdo->exec($sql);

?>