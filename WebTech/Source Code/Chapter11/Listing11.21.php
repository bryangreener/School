<?php

//Listing 11.21 Looping through the result set (mysqli—using prepared statements)

$sql = "SELECT Title, CopyrightYear FROM Books WHERE ID=?";
if ($statement = mysqli_prepare($connection, $sql)) {
  mysqli_stmt_bindm($statement, 'i', $id);
  mysqli_stmt_execute($statement);
  // bind result variables
  mysqli_stmt_bind_result($statement, $title, $year);
  // loop through the data
  while (mysqli_stmt_fetch($statement)) {
    echo $title . '-' . $year . '<br/>';
  }
}

?>