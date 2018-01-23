<?php

//From Listing 10.1
class Artist {
  public $firstName;
  public $lastName;
  public $birthDate;
  public $birthCity;
  public $deathDate;
}

//Listing 10.2
$picasso = new Artist();
$dali = new Artist();
$picasso->firstName = "Pablo";
$picasso->lastName = "Picasso";
$picasso->birthCity = "Malaga";
$picasso->birthDate = "October 25 1881";
$picasso->deathDate = "April 8 1973";

?>