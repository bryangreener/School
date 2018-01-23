<?php

//Listing 11.14 Executing a query that doesn't return data (PDO)
$sql = "UPDATE Categories SET CategoryName='Web' WHERE CategoryName='Business'";
$count = $pdo->exec($sql);
echo "<p>Updated " . $count . " rows</p>";

?>