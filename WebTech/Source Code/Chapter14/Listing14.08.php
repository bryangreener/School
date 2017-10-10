<?php

//Listing 14.8 Example __set() magic method

class DomainObject {
  //. . .
  public function __set($name, $value) {
    $mutator = 'set' . ucfirst($name);
    // if mutator method is defined than call it
    if (method_exists($this, $mutator) &&
	is_callable(array($this, $mutator))) {
      $this->$mutator($value);
    }
    else {
      $this->$name = $value;
    }
  }
}

?>