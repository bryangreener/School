<?php

class Artist implements Serializable {
  //some parts borroed from earlier chapters.
  const EARLIEST_DATE = 'January 1, 1200';
  private static $artistCount = 0;
  private $firstName;
  private $lastName;
  private $birthDate;
  private $deathDate;
  private $birthCity;  
  private $artworks;  

  // Implement the Serializable interface methods
  public function serialize() {
    // use the built-in PHP serialize function
    return serialize(
		     array("earliest" =>self::$earliestDate,
			   "first" => $this->firstName,
			   "last" => $this->lastName,
			   "bdate" => $this->birthDate,
			   "ddate" => $this->deathDate,
			   "bcity" => $this->birthCity,
			   "works" => $this->artworks
			   )
		     );
  }
  public function unserialize($data) {
    // use the built-in PHP unserialize function
    $data = unserialize($data);
    self::$earliestDate = $data['earliest'];
    $this->firstName = $data['first'];
    $this->lastName = $data['last'];
    $this->birthDate = $data['bdate'];
    $this->deathDate = $data['ddate'];
    $this->birthCity = $data['bcity'];
    $this->artworks = $data['works'];
  }
  //...
}

?>