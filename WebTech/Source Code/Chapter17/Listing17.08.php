<?php
$filename = 'Listing17.01.xml';
if (file_exists($filename)) {
  $art = simplexml_load_file($filename);
  // access a single element
  $painting = $art->painting[0];
  echo '<h2>' . $painting->title . '</h2>';
  echo '<p>By ' . $painting->artist->name . '</p>';
  // display id attribute
  echo '<p>id=' . $painting["id"] . '</p>';
  // loop through all the paintings
  echo "<ul>";
  foreach ($art->painting as $p)
    {
      echo '<li>' . $p->title . '</li>';
    }
  echo '</ul>';
} else {
  exit('Failed to open ' . $filename);
}
?>