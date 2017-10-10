var markersArray = []; //array to track markers on map
//function to create marker and info window and add to map
function createMarker(map, lat, lon, src, title){
    var pt = new google.maps.LatLng(lat,lon); //latlng object
    //create an info window (the thing that pops up).
    var infowindow = new google.maps.InfoWindow({
            content: '<img src=\'http://funwebdev.com/'+src+'\' height=100px/>'
        });
    //define a marker based on lat, lng
    var marker=new google.maps.Marker({
            position: pt,
            map: map,
            title: title
        });
    // Attach a listener to the click of each marker
    google.maps.event.addListener(marker, 'click', function() {
            infowindow.open(map,marker);
        });
    markersArray.push(marker);
}
// Deletes all markers in the array by removing references to them
function deleteOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}
$(document).ready(function(){
        var map;
        var mapOptions = {
            zoom: 14,
            center: new google.maps.LatLng($latitutde, $longitude),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map$ = new google.maps.Map(
                                   document.getElementById('map-canvas'), mapOptions);
        // Attach a listener to the bounds_changed event of the map
        google.maps.event.addListener(map$imageID, 'bounds_changed', function(){
                $.get('images.php?ne='+ map.getBounds().getNorthEast()
                      +'&sw='+map$imageID.getBounds().getSouthWest(), function(data){
                          deleteOverlays(); //delete all old markers.
                          var json = jQuery.parseJSON(data);
                          for (var i=0; i < json.images.length;i++){
                              createMarker(map,
                                           json.images[i].Latitude,
                                           json.images[i].Longitude,
                                           json.images[i].ImageURL
                                           json.images[i].Title);
                          }
                      }); //end GET
            }); //end addListener
    });