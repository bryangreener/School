<?php

include_once("Listing17.14.php");
$request = constructFlickrSearchRequest('Athens');
echo '<p><small>' . $request . '</small></p>';
$http = curl_init($request);
// Set curl options
curl_setopt($http, CURLOPT_HEADER, false);
curl_setopt($http, CURLOPT_RETURNTRANSFER, true);
// Make the request
$response = curl_exec($http);
// get the status code
$status_code = curl_getinfo($http, CURLINFO_HTTP_CODE);
// Close the curl session
curl_close($http);
if ($status_code == 200) {
  // create simpleXML object by loading string
  $xml = simplexml_load_string($response);
  // iterate through each <photo> element
  foreach ($xml->photos->photo as $p)
    {
      // construct URLs for image and for link
      $pageURL = "http://www.flickr.com/photos/" . $p['owner'] . "/" .$p['id'];
      $imgURL = "http://farm" .$p["farm"] . ".staticflickr.com/"   . $p["server"] . "/" . $p["id"] . "_" . $p["secret"] . "_q.jpg";
      // output links and image tags
      echo "<a href='" . $pageURL . "'>";
      echo "<img src='" . $imgURL . "' />";
      echo "</a>"; }
}
else {
  die("Your call to web service failed -- code=" . $status_code);
}