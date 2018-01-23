<?php
/*
  Algorithm for outputting the XML for the books
*/
function outputXML($dbAdapter, $acceptedCriteria, $whereClause) {
  // get query string values and set up search criteria
  $criteria = $_GET['criteria'];
  $look = $_GET['look'];
  $index = array_search($criteria, $acceptedCriteria);
  // get the data from the database
  $bookGate = new BookTableGateway($dbAdapter);
  $results = $bookGate->findByFromJoins( $whereClause[$index],
                                         Array($look) );
  // output the XML for the retrieved book data
  echo createXMLforBooks($results, $dbAdapter);
  $dbAdapter->closeConnection();
}
/*
  Checks if valid query string information was passed in GET or POST
*/
function isCorrectQueryStringInfo($acceptedCriteria) {
  if ( isCriteraPresent($acceptedCriteria) && isLookPresent()) {
    return true;
  }
  return false;
}
/*
  Checks for query string info that specifies which criteria to use
*/
function isCriteraPresent($acceptedCriteria) {
  if ($_SERVER['REQUEST_METHOD'] == 'GET'
      && isset($_GET['criteria'])) {
    // now check criteria values are correct
    if ( in_array($_GET['criteria'],$acceptedCriteria) )
      return true;
    else
      return false;
  }
  return false;
}
/*
  Checks for query string info that specifies which criteria to use
*/
function isLookPresent() {
  if ($_SERVER['REQUEST_METHOD'] == 'GET' && !empty($_GET['look']))
    return true;
  return false;
}
/*
  Return a string containing XML for book
*/
function createXMLforBooks($bookResults, $dbAdapter) {
  // will implement this function shortly
}

?>