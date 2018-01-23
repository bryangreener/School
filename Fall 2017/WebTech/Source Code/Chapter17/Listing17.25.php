<?php

class Book extends DomainObject implements JsonSerializable
{
  //  ...
  /*
    This method is called by the json_encode() function that is
    part of PHP
  */
  public function jsonSerialize() {
    return [
            'id' => $this->ID,
            'value' => $this->Title
            ];
  }
}