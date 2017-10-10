<?php

//listing 20.4 Portion of a PHP email harvesting scraper

foreach($aTags as $link){
  $mailpos=strpos($link->getAttribute('href'),"mailto:");
  if($mailpos !== false){
    echo substr($link->getAttribute('href'),$mailpos+7)."<br>";
  }
}

?>