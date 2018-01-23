<?php
function constructFlickrSearchRequest($search)
{
  $serviceDomain = 'http://api.flickr.com/services/rest/?';
  $method = 'method=flickr.photos.search';
  $api_key = 'api_key=' . 'your Flickr api key here';
  $searchFor = 'tags=' . $search;
  $format = 'format=rest';
  // only 12 results for now
  $options = 'per_page=12';
  // due to copyright, we will use only the author’s Flickr images
  $options .= '&user_id=31790027%40N04';
  return $serviceDomain . $method . '&' . $api_key .'&'. $searchFor . '&' . $format . '&' . $options;
}
?>