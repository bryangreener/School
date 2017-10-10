<?php

$filename = 'Listing17.01.xml';
if (file_exists($filename)) {
  // create and open the reader
  $reader = new XMLReader();
  $reader->open($filename);
  // loop through the XML file
  while ( $reader->read() ) {
    $nodeName = $reader->name;
    // since all sorts of different XML nodes we must check
    // node type
    if ($reader->nodeType == XMLREADER::ELEMENT
        && $nodeName == 'painting') {
      $id = $reader->getAttribute('id');
      echo '<p>id=' . $id . '</p>';
    }
    if ($reader->nodeType == XMLREADER::ELEMENT
        && $nodeName =='title') {
      // read the next node to get at the text node
      $reader->read();
      echo '<p>' . $reader->value . '</p>';
    }
  }
} else {
  exit('Failed to open ' . $filename);
}

?>