<?php

/*
  Return a string containing XML for book
*/
function createXMLforBooks($bookResults, $dbAdapter) {
  // first set up the XML writer
  $writer = new XMLWriter();
  $writer->openMemory();
  $writer->startDocument('1.0','UTF-8');
  $writer->setIndent(true);
  // create the root element
  $writer->startElement("books");
  // now loop through each book object in our collection and
  // write the appropriate XML for it
  foreach ($bookResults as $book) {
    writeSingleBookXML($writer, $book, $dbAdapter);
  }
  // close root element
  $writer->endElement();
  // finish up writer
  $writer->endDocument();
  // return a string representation of the XML writer
  return $writer->outputMemory(true);
}
/*
  Writes XML for a single book
*/
function writeSingleBookXML($writer, $book, $dbAdapter) {
  $writer->startElement("book");
  $writer->writeAttribute("id", $book->ID);
  // write XML for the ISBN numbers
  writeIsbnsXML($writer, $book);
  $writer->startElement("title");
  $writer->text(htmlentities($book->Title));
  $writer->endElement();
  // write XML for the authors
  writeAuthorXML($writer, $book, $dbAdapter);
  $writer->startElement("category");
  $writer->text($book->Category);
  $writer->endElement();
  $writer->startElement("subcategory");
  $writer->text($book->Subcategory);
  $writer->endElement();
  $writer->startElement("year");
  $writer->text($book->CopyrightYear);
  $writer->endElement();
  $writer->startElement("imprint");
  $writer->text($book->Imprint);
  $writer->endElement();
  $writer->startElement("pagecount");
  $writer->text($book->PageCountsEditorialEst);
  $writer->endElement();
  $writer->startElement("description");
  $writer->text(htmlentities($book->Description));
  $writer->endElement();
  $writer->endElement();
}
/*
  Using the XML Writer, add information for a book’s ISBN numbers
*/
function writeIsbnsXML($writer, $book) {
  $writer->startElement("isbns");
  $writer->startElement("isbn10");
  $writer->text($book->ISBN10);
  $writer->endElement();
  $writer->startElement("isbn13");
  $writer->text($book->ISBN13);
  $writer->endElement();
  $writer->endElement();
}
/*
  writes XML for a book’s authors
*/
function writeAuthorXML($writer, $book, $dbAdapter) {
  // will implement this shortly
}

?>