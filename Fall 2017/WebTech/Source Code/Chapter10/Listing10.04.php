<?php

//From Listing 10.1
class Artist {
  public $firstName;
  public $lastName;
  public $birthDate;
  public $birthCity;
  public $deathDate;

  //From Listing 10.3
  function __construct($firstName, $lastName, $city, $birth, $death=null) {
    $this->firstName = $firstName;
    $this->lastName = $lastName;
    $this->birthCity = $city;
    $this->birthDate = $birth;
    $this->deathDate = $death;
  }

  //Listing 10.4
  public function outputAsTable() {
    $table = "<table>";
    $table .= "<tr><th colspan='2'>";
    $table .= $this->firstName . " " . $this->lastName;
    $table .= "</th></tr>";
    $table .= "<tr><td>Birth:</td>";
    $table .= "<td>" . $this->birthDate;
    $table .= "(" . $this->birthCity . ")</td></tr>";
    $table .= "<tr><td>Death:</td>";
    $table .= "<td>" . $this->deathDate . "</td></tr>";
    $table .= "</table>";
    return $table;
  }

}

//code to demonstrate useage of the class
$picasso = new Artist("Pablo","Picasso","Malaga","Oct 25,1881","Apr 8,1973");
$dali = new Artist("Salvador","Dali","Figures","May 11 1904","Jan 23 1989");
echo $picasso->outputAsTable();
echo $dali->outputAsTable();


?>