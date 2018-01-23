<?php

/*
Using the XML Writer, add information for a book’s authors
*/
function writeAuthorXML($writer, $book, $dbAdapter) {
  // retrieve authors for the current book
  $authorGate = new AuthorTableGateway($dbAdapter);
  $authorResults = $authorGate->getForBookId($book->ID);
  // now write <authors> collection
  $writer->startElement("authors");
  // loop through each author in the collection and output it
  foreach ($authorResults as $author) {
    writeSingleAuthorXML($writer, $author);
  }
  $writer->endElement();
}
/*
Writes XML for a single author
*/
function writeSingleAuthorXML($writer, $author) {
  $writer->startElement("author");
  $writer->startElement("lastname");
  $writer->text($author->LastName);
  $writer->endElement();
  $writer->startElement("firstname");
  $writer->text($author->FirstName);
  $writer->endElement();
  $writer->startElement("institution");
  $writer->text($author->Institution);
  $writer->endElement();
  $writer->endElement();
}

?>