<?php

include_once("Listing14.04.php");

//Listing 14.5 Example subclasses

class ArtistTableGateway extends TableDataGateway
{
  //. . .
  protected function getSelectStatement()
  {
    return " SELECT ArtistID,FirstName,LastName,Nationality FROM Artists";
  }
  protected function getPrimaryKeyName() {
    return "AuthorID";
  }
}

class ArtWorkTableGateway extends TableDataGateway
{
  //. . .
  protected function getSelectStatement()
  {
    return "SELECT ArtWorkID,Title,Description, . . . FROM ArtWorks";
  }
  protected function getPrimaryKeyName() {
    return "ArtWorkID";
  }
}

?>