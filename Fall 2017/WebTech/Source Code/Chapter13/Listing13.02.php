<?php

//listing 13.2 Reading a cookie <-visit Listing13.01.php to set the cookie.

if( !isset($_COOKIE['Username']) ) {
  //no valid cookie found
}
else {
  echo "The username retrieved from the cookie is:";
  echo $_COOKIE['Username'];
}
?>