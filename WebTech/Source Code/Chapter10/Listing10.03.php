<?php

//From Listing 10.1
class Artist {
  public $firstName;
  public $lastName;
  public $birthDate;
  public $birthCity;
  public $deathDate;

  //Listing 10.3
  function __construct($firstName, $lastName, $city, $birth, $death=null) {
    $this->firstName = $firstName;
    $this->lastName = $lastName;
    $this->birthCity = $city;
    $this->birthDate = $birth;
    $this->deathDate = $death;
  }
}

//Replacement for  Listing 10.2
$picasso = new Artist("Pablo","Picasso","Malaga","Oct 25,1881","Apr 8,1973");
$dali = new Artist("Salvador","Dali","Figures","May 11 1904","Jan 23 1989");


?>