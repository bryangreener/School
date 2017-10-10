<?php

$filename = "Listing17.01.xml";
// create and open the reader
$reader = new XMLReader();
$reader->open($filename);
// loop through the XML file
while($reader->read()) {
  $nodeName = $reader->name;
  if ($reader->nodeType == XMLREADER::ELEMENT
      && $nodeName =='painting') {
    // create a SimpleXML object from the current painting node
    $doc = new DOMDocument('1.0', 'UTF-8');
    $painting = simplexml_import_dom($doc->importNode($reader->expand(),true));
    // now have a single painting as an object so can output it
    echo '<h2>' . $painting->title . '</h2>';
    echo '<p>By ' . $painting->artist->name . '</p>';
  }
}

?>