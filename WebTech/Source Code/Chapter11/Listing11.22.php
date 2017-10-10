<?php

//Listing 11.22 Looping through the result set (PDO)

$sql = "select * from Categories order by CategoryName";
$result = $pdo->query($sql);
while ( $row = $result->fetch() ) {
  echo $row['ID'] . " - " . $row['CategoryName'] . "<br/>";
}

?>