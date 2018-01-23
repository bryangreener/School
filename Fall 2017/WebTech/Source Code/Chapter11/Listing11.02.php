<?php

//Listing 11.2 Excerpt from a config.inc.php file for a phpMyAdmin installation

$cfg['Servers'][$i]['host'] = 'localhost';
$cfg['Servers'][$i]['controluser'] = 'DBUsername';
$cfg['Servers'][$i]['controlpass'] = 'DBPassword';
$cfg['Servers'][$i]['extension'] = 'mysqli';
// use the mysqli extension

?>