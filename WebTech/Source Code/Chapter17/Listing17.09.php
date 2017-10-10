<?php
$filename ="Listing17.01.xml";
$art = simplexml_load_file($filename);
$titles = $art->xpath('/art/painting/title');
foreach ($titles as $t) {
  echo $t . '<br/>';
}
$names = $art->xpath('/art/painting[year>1800]/artist/name');
foreach ($names as $n) {
  echo $n . '<br/>';
}

?>