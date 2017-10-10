<?php

//Listing 11.30 Solution to search results page problem

function getSearchFor()
{
  $value = "";
  if (isset($_GET[SEARCHBOX])) {
    $value = $_GET[SEARCHBOX];
  }
  return $value;
}

?>