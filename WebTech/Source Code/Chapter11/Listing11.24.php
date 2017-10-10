<?php

//Listing 11.24 Letting an object populate itself from a result set

class Book {
  public $id;
  public $title;
  public $copyrightYear;
  public $description;
  function __construct($record)
  {
    // the references to the field names in associative array must
    // match the case in the table
    $this->id = $record['ID'];
    $this->title = $record['Title'];
    $this->copyrightYear = $record['CopyrightYear'];
    $this->description = $record['Description'];
  }
}
//...
// in some other page or class
$statement->execute();
// using the Book class
$b = new Book($statement->fetch());
echo 'ID: ' . $b->id . '<br/>';
echo 'Title: ' . $b->title . '<br/>';
echo 'Copyright Year: ' . $b->copyrightYear . '<br/>';

?>