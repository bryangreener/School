<?php
// first define api key constants – you will replace these values
define("BING_API_KEY",'[your api key here]');
define("GEONAMES_API_USERNAME", '[your username here]');
//
// Constructs the URL to retrieve lat/long for a real-world
// address. It is passed a customer object
//
function constructBingSearchRequest($customer)
{
  $serviceDomain = 'http://dev.virtualearth.net/REST/v1/Locations?';
  $api_key = 'key=' . BING_API_KEY;
  $query = 'query=' . urlencode($customer->address) . ','
    .urlencode($customer->city) . ',' . $customer->region . ','
    .$customer->country;
  return $serviceDomain . $api_key . '&' . $query;
}
//
// Constructs the URL to retrieve nearby amenities to a location
//
function constructGeoNameSearchRequest($lat, $long)
{
  $serviceDomain = 'http://api.geonames.org/findNearbyPOIsOSMJSON?';
  $api_key = 'username=' . GEONAMES_API_USERNAME;
  $query = 'lat=' . $lat . '&lng=' . $long;
  return $serviceDomain . $api_key . '&' . $query;
}
//
// Constructs the URL for static map with main location and amenities
//
function constructBingMapRequest($zoom, $width, $length, $lat,
                                 $long, $amenities)
{
  $serviceDomain = 'http://dev.virtualearth.net/REST/v1/Imagery/
Map/Road/';
  $api_key = 'key=' . BING_API_KEY;
  $request = $serviceDomain . $lat . ',' . $long . '/' . $zoom;
  $request .= '?mapSize=' . $width . ',' . $length . '&' . $api_key;
  $request .= '&pp=' . $lat . ',' . $long . ';66';
  foreach ($amenities as $amenity)
    {
      $request .= '&pp=' . $amenity->lat . ',' . $amenity->lng . ';34';
    }
  return $request;
}
//
// Invokes/requests a web service and returns its response.
// For simplicity's sake, if problem with service it simply dies.
// For real-world site, would need better error handling.
//
function invokeWebService($request)
{
  $http = curl_init($request);
  curl_setopt($http, CURLOPT_HEADER, false);
  curl_setopt($http, CURLOPT_RETURNTRANSFER, true);
  $response = curl_exec($http);
  $status_code = curl_getinfo($http, CURLINFO_HTTP_CODE);
  curl_close($http);
  if ($status_code == 200) {
    return $response;
  }
  else {
    die("Your call to web service failed -- code=" . $status_code);
  }
}
//
// Code that implements algorithm from Figure 17.12. Notice that it
// returns the populated image tag for the map image
//
function getCustomerMapImage($customer)
{
  // call web service
  $request = constructBingSearchRequest($customer);
  $response = invokeWebService($request);
  // now decode JSON and extract latitude and longitude
  $json = json_decode($response);
  if (json_last_error() == JSON_ERROR_NONE) {
    $lat = $json->resourceSets[0]->resources[0]->point
      ->coordinates[0];
    $long = $json->resourceSets[0]->resources[0]->point
      ->coordinates[1];
    // with this lat/long, get list of amenities
    $request = constructGeoNameSearchRequest($lat, $long);
    $response = invokeWebService($request);
    $json = json_decode($response);
    if (json_last_error() == JSON_ERROR_NONE) {
      // now get map image with location and amenity markers
      $mapImageURL = constructBingMapRequest(16, 600, 400, $lat,
                                             $long, $json->poi);
      $img = '<img src="' . $mapImageURL . '" alt="map here" />';
      return $img;
    }
  }
}
// Somewhere in your page, you will have to get the customer object
$customer = getCustomer();
// And then somewhere on the page there will be this call, which
// displays the map image.
echo getCustomerMapImage($customer);
?>