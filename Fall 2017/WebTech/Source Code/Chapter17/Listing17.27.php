<!DOCTYPE html>
<html>
<head>
<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

</head>
<body>
<?php
   function getGoogleMap($imageID, $latitude, $longitude) {
return "<script type='text/javascript'>
$(document).ready(function() {
  var map$imageID;
  var mapOptions = {
    zoom: 14,
    center: new google.maps.LatLng($latitude, $longitude),
    mapTypeId: google.maps.MapTypeId.ROADMAP
  };
  map$imageID = new google.maps.Map(
  document.getElementById
  ('map-canvas$imageID'), mapOptions);
  });
</script>
<div style='width: 400px; height: 400px;' class='map-canvas' id='map-canvas$imageID'></div>";
 }
   echo getGoogleMap(1, 51.011179,-114.132866);
?>
</body>
</html>