<?php

//Listing 14.10 Retrieving and saving data using a domain object and a gateway

// use artist gateway to retrieve a specific artist
$gate = new ArtistTableGateway($dbAdapter);
$artist = $gate->findByKey($id);
echo $artist->LastName . ', ' . $artist->FirstName;
//. . .
// make a change to domain object
$artist->LastName = 'Picasso';
// then use gateway to save it
$gate->update($artist);

?>