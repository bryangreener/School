<?php
  try{
    $connStr = "mysql:host=localhost;dbname=travels";
    $un = "root";
    $pw = "";
    $pdo = new PDO($connStr,$un,$pw);
  }catch(PDOException $e){
    die($e->getMessage());
  }
  $continents = [];
  $countries = [];
  $cities = [];
  $imgLocations = [];

  if(isset($_GET['city']) && isset($_GET['country']))
  {
    if($_GET['city'] == 0 || $_GET['country'] == "ZZZ")
    {
      if($_GET['city'] == 0 && $_GET['country'] != "ZZZ")
      {
        $query = "SELECT 'Path'
            FROM travelimage
            JOIN travelimagedetails ON travelimagedetails.ImageID = travelimage.ImageID
            WHERE CountryCodeISO = :country
            ORDER BY ImageID";
        $stmt = $pdo->prepare($query);
        $stmt->bindValue(':country', $_GET['country']);
        $result = $stmt->execute();
      }
      else if($_GET['country'] == "ZZZ" && $_GET['city'] != 0)
      {
        $query = "SELECT 'Path'
            FROM travelimage
            JOIN travelimagedetails ON travelimagedetails.ImageID = travelimage.ImageID
            WHERE CityCode = :city
            ORDER BY ImageID";
        $stmt = $pdo->prepare($query);
        $stmt->bindValue(':city', $_GET['city']);
        $result = $stmt->execute();
      }
    }
    else
    {
      $query = "SELECT 'Path'
            FROM travelimage
            JOIN travelimagedetails ON travelimagedetails.ImageID = travelimage.ImageID
            WHERE CityCode = :city AND CountryCodeISO = :country
            ORDER BY ImageID";
      $stmt = $pdo->prepare($query);
      $stmt->bindValue(':city', $_GET['city']);
      $stmt->bindValue(':country', $_GET['country']);
      $result = $stmt->execute();
    }
  }

  if(!empty($result))
  {
    while($row = $result->fetch())
    {
      $imgLocations[] = $row;
    }
  }

  $query = "SELECT ContinentName FROM geocontinents ORDER BY ContinentName ASC";
  $result = $pdo->query($query);
  while($row = $result->fetch())
  {
    array_push($continents,$row['ContinentName']);
  }

  $query = "SELECT CountryName, CountryCodeISO 
            FROM geocountries 
              JOIN travelImagedetails ON geocountries.ISO=travelImagedetails.CountryCodeISO 
            WHERE CountryCodeISO IS NOT NULL 
            ORDER BY CountryName ASC";
  $result = $pdo->query($query);
  while($row = $result->fetch())
  {
    $countries[] = $row;
  }

  $query = "SELECT AsciiName, CityCode
            FROM travelImagedetails 
              JOIN geocities ON travelImagedetails.CityCode = geocities.GeoNameID
            WHERE CityCode IS NOT NULL
            ORDER BY AsciiName ASC";
  $result = $pdo->query($query);
  while($row = $result->fetch())
  {
    $cities[] = $row;
  }

  $continents = array_unique($continents);
  $countries = array_unique($countries, SORT_REGULAR);
  $cities = array_unique($cities, SORT_REGULAR);
?>

<!DOCTYPE html>
<html lang="en">
<head>
   <title>Travel Template</title>
   <?php include 'includes/travel-head.inc.php'; ?>
</head>
<body>

<?php include 'includes/travel-header.inc.php'; ?>
   
<div class="container">  <!-- start main content container -->
   <div class="row">  <!-- start main content row -->
      <div class="col-md-3">  <!-- start left navigation rail column -->
         <?php include 'includes/travel-left-rail.inc.php'; ?>
      </div>  <!-- end left navigation rail --> 
      
      <div class="col-md-9">  <!-- start main content column -->
         <ol class="breadcrumb">
           <li><a href="#">Home</a></li>
           <li><a href="#">Browse</a></li>
           <li class="active">Images</li>
         </ol>          
    
         <div class="well well-sm">
            <form class="form-inline" role="form" method="get" action="<?php echo $_SERVER['PHP_SELF']; ?>">
              <div class="form-group" >
                <select class="form-control" name="city">
                  <option value="0">Filter by City</option>
                  <?php
                    foreach($cities as $city)
                    {
                      echo "<option value=$city[1]>$city[0]</option>";
                    }
                  ?>
                </select>
              </div>
              <div class="form-group">
                <select class="form-control" name="country">
                  <option value="ZZZ">Filter by Country</option>
                  <?php
                    foreach($countries as $country)
                    {
                      echo "<option value=$country[1]>$country[0]</option>";
                    }
                  ?>
                </select>
              </div>  
              <button type="submit" class="btn btn-primary">Filter</button>
            </form>         
         </div>      <!-- end filter well -->
         
         <div class="well">
            <div class="row">
                <!-- display image thumbnails code here -->
                <?php
                  foreach($imgLocations as $img)
                  {
                    echo "$img[0]";
                  }
                ?>
            </div>
         </div>   <!-- end images well -->

      </div>  <!-- end main content column -->
   </div>  <!-- end main content row -->
</div>   <!-- end main content container -->
   
<?php include 'includes/travel-footer.inc.php'; ?>   

   
   
 <!-- Bootstrap core JavaScript
 ================================================== -->
 <!-- Placed at the end of the document so the pages load faster -->
 <script src="bootstrap3_travelTheme/assets/js/jquery.js"></script>
 <script src="bootstrap3_travelTheme/dist/js/bootstrap.min.js"></script>
 <script src="bootstrap3_travelTheme/assets/js/holder.js"></script>
</body>
</html>