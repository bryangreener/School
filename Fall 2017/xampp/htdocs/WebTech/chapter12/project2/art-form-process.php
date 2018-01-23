<?php
try{
$server = "localhost";
$un = "root";
$pw = "";
$dbname = "chapter12project02db";
$pdo = new PDO("mysql:host=$server;dbname=$dbname", $un, $pw);
}catch(PDOException $e){
  print "ERROR" . $e->getMessage() . "<br/>";
  die();
}

if(isset($_POST['first']) && isset($_POST['last']) && isset($_POST['email']) && isset($_POST['password1']))
{
	$fn = $_POST['first'];
	$ln = $_POST['last'];
	$em = $_POST['email'];
	$pw = $_POST['password1'];

}

$pdo->beginTransaction();
try{
	$sql = "INSERT INTO users (first,last,email,pass) VALUES(:first,:last,:email,:pass);";

	$query = $pdo->prepare($sql);
	$query->bindParam(':first', $fn);
	$query->bindParam(':last', $ln);
	$query->bindParam(':email', $em);
	$query->bindParam(':pass', $pw);
	$query->execute();

	$pdo->commit();
	echo 'GOT HERE';
	header('Location: chapter12-project02.php');
}catch(Exception $e)
{
	echo '<script>console.log("ROLLING BACK QUERY")</script>';
    echo $e->getMessage();
    $pdo->rollBack();
}
?>
