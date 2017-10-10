<?php

//Listing 11.23 Populating an object from a result set (PDO)

$id = $_GET['id'];
$sql = "SELECT id, title, copyrightYear, description FROM Books WHERE id= ?";
$statement = $pdo->prepare($sql);
$statement->bindValue(1, $id);
$statement->execute();
$b = $statement->fetchObject('Book');
echo 'ID: ' . $b->id . '<br/>';
echo 'Title: ' . $b->title . '<br/>';
echo 'Year: ' . $b->copyrightYear . '<br/>';
echo 'Description: ' . $b->description . '<br/>';

?>