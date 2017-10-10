<?php

//listing 20.3 PHP scraper script to extract all the hyperlinks and anchor text

$DOM = new DOMDocument();
$DOM->loadHTML($HTMLDOCUMENT);
$aTags = $DOM->getElementsByTagName("a");
foreach($aTags as $link){
  echo $link->getAttribute('href')." - ".$link->nodeValue."<br>";
}

?>