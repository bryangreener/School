<?php

//Listing 11.11 Executing a SELECT query (mysqli)

$sql = "SELECT * FROM Categories ORDER BY CategoryName";
// returns a mysqli_result object
$result = mysqli_query($connection, $sql);

?>
