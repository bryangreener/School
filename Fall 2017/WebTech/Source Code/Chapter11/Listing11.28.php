<?php
//Listing 11.28 Alternate list of links example
?>
<ul>
<?php
$result = getResults(); // some function that returns the result set
while ($row = $result->fetch()) {
?>
<li>
<a href="l ist.php?category=<?php echo $row['ID']; ?>">
    <?php echo $row['CategoryName']; ?>
</a>
</li>
    <?php } ?>
</ul>
