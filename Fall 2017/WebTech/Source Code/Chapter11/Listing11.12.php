<?php

//Listing 11.12 Executing a SELECT query (pdo)

$sql = "SELECT * FROM Categories ORDER BY CategoryName";
// returns a PDOStatement object
$result = $pdo->query($sql);

?>