<?php

//Listing 11.13 Executing a query that doesn't return data (mysqli)
$sql = "UPDATE Categories SET CategoryName='Web' WHERE CategoryName='Business'";
if ( mysqli_query($connection, $sql) ) {
  $count = mysqli_affected_rows($connection);
  echo "<p>Updated " . $count . " rows</p>";
}

?>