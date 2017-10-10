<?php

require_once('includes/setup.inc.php');
require_once('includes/funcFindTitles.inc.php');
// Tell the browser to expect JSON rather than HTML
header('Content-type: application/json');
if ( isCorrectQueryStringInfo() ) {
  outputJSON($dbAdapter);
}
else {
  // put error message in JSON format
  echo '{"error": {"message":"Incorrect query string values"}}';
}
function outputJSON($dbAdapter) {
  // get query string values and set up search criteria
  $whereClause = 'Title Like ?';
  $look = $_GET['term'] . '%';
  // get the data from the database
  $bookGate = new BookTableGateway($dbAdapter);
  $results = $bookGate->findByFromJoins($whereClause, Array($look) );
  // output the JSON for the retrieved book data
  echo json_encode($results);
  $dbAdapter->closeConnection();
}

?>