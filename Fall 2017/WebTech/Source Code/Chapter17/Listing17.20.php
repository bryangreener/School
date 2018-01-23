<?php
//require_once('includes/setup.inc.php');
require_once('Listing17.21.php');
// array to be used for query string validation and extraction
$acceptedCriteria = array('imprint','category','subcategory');
// parallel array to be used for constructing appropriate SQL
// criteria
$whereClause = array('Imprint=?','CategoryName=?','SubcategoryName=?');
// Tell the browser to expect XML rather than HTML
// NOTE: comment this line out when debugging
header('Content-type: text/xml');
// check query string parameters and either output XML or error
// message (in XML)
if ( isCorrectQueryStringInfo($acceptedCriteria) ) {
  outputXML($dbAdapter, $acceptedCriteria, $whereClause);
}
else {
  echo '<errorResult>Error: incorrect query string values</errorResult>';
}
?>