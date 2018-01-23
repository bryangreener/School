<?php

//Listing 11.20 Looping through the result set (mysqli—not prepared statements)

$sql = "select * from Categories order by CategoryName";
// run the query
if ($result = mysqli_query($connection, $sql)) {
  // fetch a record from result set into an associative array
  while($row = mysqli_fetch_assoc($result))
    {
      // the keys match the field names from the table
      echo $row['ID'] . " - " . $row['CategoryName'] ;
      echo "<br/>";
    }
}

?>