<?php
try{
	$connString = "mysql:host=localhost:dbname=NAMEHERE";
	$user="testusr";
	$pass="password";
	$pdo = new PDO($connnString,$user,$pass);
	$pdo->setAttribute(PDO::ATTR_ERRMODE,PDO::ERRMODE_EXCEPTION);

	$sql = "select statement goes here";
	$result = $pdo->query($sql);

	while($row = $result->fetch()){
		echo $row['ID'] ." - " . $row['CategoryName'] . </br>
	}
}


Other connection values (procedural)
	$host = "localhost";
	$database = "dbname";
	$user = "username"
	$pass = "pw";

	$connection = mysqli_connect($host, $user, $pass, $database);

MORE GENERIC (object oriented)
	$connectionString = "mysql:host=localhost;dbname=dbnamehere";
	$user = "un";
	$pass = "pw";
	$pdo = new PDO($connectionString, $user, $pass);

ERROR HANDLING
	Using mysqli 
		$error = mysqli_connect_error();
		if($error != null)
		{
			$output = "error info here" . $error;
			exit($output);
		}

	Using PDO
		try{

		$connString = "enter string here"
		$pdo = new PDO("info here");
		}
		catch{}

EXECUTE query
	Using mysqli
		$sql = "query string";
		$result = mysqli_query($connection, $sql);
	Using PDO
		$sql = "query string";
		$result = $pdo->query($sql);

all this code is in powerpoint btw 
	PDO query without return 
		$sql = "query string";
		$count = $pdo->exec($sql);
		echo "<p>Updated " . $count . " rows</p>";

INTEGRATE USER DATA 
	$from = $_POST['old'];
	$to = $_POST['new'];
	$sql = "UPDATE Categories SET CategoryName='$to' WHERE CategoryName='$from'";
	$count = $pdo->exec($sql);
This is bad practice as it is open to sql injection attacks 
Sanitizing User Input 
	$from = $pdo->quote($from);
	$to = $pdo->quote($to);
	$sql = "UPDATE Categories SET CategoryName=$to WHERE CategoryName=$from";
	$count = $pdo->exec($sql);

Prepared Statements 
	MYSQLi 
	$id = $_GET['id']; // Retrieve param value from query string

	// Construct paramertized query -- notice ? param
	$sql = "SELECT Title, CopyrightYear FORM Books WHERE ID=?";

	// Create prepared statement
	if($statement = mysqli_prepare($connection,$sql)
	{
		// Bind params s: string, b: blob, i: int, etc.
		mysqli_stmt_bindm($statement, 'i', $id;
		// Execute query
		mysqli_stmt_execute($statement);
	}

	PDO 
	// get params valuye from query string
	$id = $_GET['id'];
	//METHOD 1 using question mark placeholders
	$sql = "SELECT Title, CopyrightYear FROM Books WHERE ID=?";
	$statement = $pdo->prepare($sql);
	$statement->bindValue(1,$id);
	$statement->execute();

	//METHOD 2 using named parameters
	$sql="SELECT Title,CopyrightYEar FROM Books WHERE ID= :id";
	$statement=$pdo->prepare($sql);
	$statement->bindValue(':id', $id);
	$statement->execute();

PROCESS QUERY RESULTS 
	MYSQLi (not prepared statements)
	$sql = "SELECT * FROM Categories ORDER BY CategoryName";
	// run query
	if($result = mysqli_query($connection,$sql)){
		while($row = mysqli_fetch_assoc($result))
		{
			echo $row['ID'] . " - " . $row['CategoryName'];
			echo "<br/>";
		}
	}

	PDO 
	$sql = "SELCT * ...."
	$result = $pdo->query($sql);
	while($row = $result->fetch()){
		echo "blah blah";
	}

	MYSQLi (using prepared statements)
	$sql = "blah";
	if($statement = mysqli_prepare($connection,$sql)){
		mysqli_stmt_bindm($statement, 'i', $id);
		mysqli_stmt_execute($statement);
		// bind result vars
		mysqli_stmt_bind_result($statement,$title,$year);
		// loop through data
		while(mysqli_stmt_fetch($statement)){
			echo "blah";
		}
	}

CLOSING CONNECTION 
	MYSQLi
	$connection = mysqli_connect("connect info");
	// release memory used by result set. This is necessary if you are going to run another query on this connection.
	mysqli_free_result($result);
	// close db connection
	mysqli_close($connection);

	PDO 
	$pdo = new PDO($connString, $user, $pass);
	// Closes connection and frees resources uses by PDO object.
	$pdo = null;

TRANSACTIONS 
	MYSQLi 
	$connection = mysqli_connect("blah");
	// Set autocommit to off. If autocmmit is on, then mysql will commit each command after it is executed.
	mysqli_autocommit($connection,FALSE);
	// insert values here
	$result1 = mysqli_query($connection,"query string");
	$result2 = "more";
	if($result1 && $result2)
	{
		// commit transaction
		mysqli_commit($connection);
	}
	else
	{
		// rollback transaction
		mysqli_rollback($connection);
	}

	PDO
	$pdo = new PDO($connString, $user, $pass);
	// turn on exception so that exception is thrown if error occurs
	$pdo->setAttribute(PDO::ATTR_ERRMODE,PDO::ERRMODE_EXCEPTION);

	try{
		$pdo_beginTransaction();

		// set of queries here. if any fails, exception is thrown and query is rolled back.
		$pdo->query("querystring");
		$pdo->query("another one");
		// if we made it here it means that no exception was thrown which means no query has failed so we can commit the transaction
		$pdo->commit();
	} catch(Exception $e){
		// we must rollback transaction since error occurred with insert
		$pdo->rollback();
	}

