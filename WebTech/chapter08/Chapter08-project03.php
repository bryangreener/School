<!DOCTYPE html>
<html lang="en">
  <head>

     <!-- Google fonts used in this theme  -->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans+Condensed:700' rel='stylesheet' type='text/css'>
   <link href='http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,400,800,600,700,300' rel='stylesheet' type='text/css'>  

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="../../assets/ico/favicon.png">

    <title>Travel Template</title>
    
   

    <!-- Bootstrap core CSS -->
    <link href="bootstrap3_travelTheme/dist/css/bootstrap.min.css" rel="stylesheet">


    <!-- Custom styles for this template -->
    <link href="bootstrap3_travelTheme/theme.css" rel="stylesheet">  

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="bootstrap3_travelTheme/assets/js/html5shiv.js"></script>
      <script src="bootstrap3_travelTheme/assets/js/respond.min.js"></script>
    <![endif]-->
  </head>

  <body>

<header>
   <div id="topHeaderRow">
      <div class="container">
         <div class="pull-right">
            <ul class="list-inline">
              <li><a href="#"><span class="glyphicon glyphicon-log-out"></span> Logout</a></li>
              <li><a href="#"><span class="glyphicon glyphicon-user"></span> Profile</a></li>
              <li><a href="#"><span class="glyphicon glyphicon-star"></span> Favorites</a></li>
            </ul>         
         </div>
      </div>
   </div>
   
    <div class="navbar navbar-default ">
      <div class="container">
         <nav>
           <div class="navbar-header">
             <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
             </button>
             <a class="navbar-brand" href="#">Share Your Travels</a>
           </div>
           <div class="navbar-collapse collapse">
             <ul class="nav navbar-nav">
               <li><a href="#">Home</a></li>
               <li><a href="#about">About</a></li>
               <li><a href="#contact">Contact</a></li>
               <li class="dropdown">
                 <a href="#" class="dropdown-toggle" data-toggle="dropdown">Browse <b class="caret"></b></a>
                 <ul class="dropdown-menu">
                   <li><a href="#">Posts</a></li>
                   <li><a href="#">Images</a></li>
                   <li class="divider"></li>
                   <li><a href="#">Countries</a></li>
                   <li><a href="#">Users</a></li>                
                 </ul>
               </li>
             </ul>
           </div><!--/.nav-collapse -->
        </nav>
      </div>
    </div>
</header>
   
<div class="container">  <!-- start main content container -->
   <div class="row">  <!-- start main content row -->
      <div class="col-md-3">  <!-- start left navigation rail column -->

         <div class="panel panel-default">
           <div class="panel-heading">Search</div>
           <div class="panel-body">
             <form>
               <div class="input-group">
                  <input type="text" class="form-control" placeholder="search ...">
                  <span class="input-group-btn">
                    <button class="btn btn-warning" type="button"><span class="glyphicon glyphicon-search"></span>          
                    </button>
                  </span>
               </div>  
             </form>
           </div>
         </div>  <!-- end search panel -->       
      
         <div class="panel panel-info">
           <div class="panel-heading">Continents</div>
           <ul class="list-group">               
              <li class="list-group-item"><a href="#">Asia</a></li>
              <li class="list-group-item"><a href="#">Africa</a></li>
              <li class="list-group-item"><a href="#">Europe</a></li>
              <li class="list-group-item"><a href="#">North America</a></li>
              <li class="list-group-item"><a href="#">South America</a></li>    
           </ul>
         </div>  <!-- end continents panel -->  
         <div class="panel panel-info">
           <div class="panel-heading">Popular Countries</div>
           <ul class="list-group">               
              <li class="list-group-item"><a href="#">Bahamas</a></li>
              <li class="list-group-item active">Canada</li>
              <li class="list-group-item"><a href="#">Germany</a></li>
              <li class="list-group-item"><a href="#">Ghana</a></li>
              <li class="list-group-item"><a href="#">Greece</a></li>
              <li class="list-group-item"><a href="#">Hungary</a></li>
              <li class="list-group-item"><a href="#">Italy</a></li>
              <li class="list-group-item"><a href="#">Spain</a></li>
              <li class="list-group-item"><a href="#">United Kingdom</a></li>
              <li class="list-group-item"><a href="#">United States</a></li>    
           </ul>
         </div>  <!-- end countries panel -->       
      </div>  <!-- end left navigation rail --> 
      
      <div class="col-md-9">  <!-- start main content column -->
         <ol class="breadcrumb">
           <li><a href="#">Home</a></li>
           <li><a href="#">Browse</a></li>
           <li class="active">Posts</li>
         </ol>          
      
         <div class="jumbotron" id="postJumbo">
           <h1>Posts</h1>
           <p>Read other travellers' posts ... or create your own.</p>
           <p><a class="btn btn-warning btn-lg">Learn more &raquo;</a></p>
         </div>        
      
         <!-- start post summaries -->
         <div class="postlist">
         
           <!-- replace each of these rows with a function call -->
           <div class="row">
               <div class="col-md-2"> 
                  <a href="post.php?id=1" class=""><img src="images/travel/6592294487.jpg" alt="Calgary in the Snow" class="img-thumbnail"/></a>
               </div>
               <div class="col-md-10"> 
                  <h2>Calgary in the Snow</h2>
                  <div class="details">
                    Posted by <a href="user.php?id=2">Leonie K&#246;hler</a>
                    <span class="pull-right">12/4/2011</span>
                  </div>
                  <p class="excerpt">
                  In December of 2011, I was lucky/unfortunate enough to have the opportunity to fly to chilly Calgary in Western Canada for a week-long conference. 
                  </p>
                  <p><a href="post.php?id=1" class="btn btn-primary btn-sm">Read more</a></p>
               </div>
           </div>
           <hr/>
           
           <div class="row">
               <div class="col-md-2"> 
                  <a href="post.php?id=3" class=""><img src="images/travel/6114904363.jpg" alt="Mountain Climbing" class="img-thumbnail"/></a>
               </div>
               <div class="col-md-10"> 
                  <h2>Mountain Climbing</h2>
                  <div class="details">
                    Posted by <a href="user.php?id=5">Frantisek  Wichterlov&#225;</a>
                    <span class="pull-right">9/3/2011</span>
                  </div>
                  <p class="excerpt">
                  The highlight of our last trip to Canada was a climb up Sulpher Mountain just outside of Banff. I'd like to say that it was so difficult that we required crampons, pitons, and screamers but that would be stretching the truth for sure.
                  </p>
                  <p><a href="post.php?id=3" class="btn btn-primary btn-sm">Read more</a></p>
               </div>
           </div>
           <hr/>

           <div class="row">
               <div class="col-md-2"> 
                  <a href="post.php?id=9" class=""><img src="images/travel/5856697109.jpg" alt="Nova Scotia" class="img-thumbnail"/></a>
               </div>
               <div class="col-md-10"> 
                  <h2>Nova Scotia</h2>
                  <div class="details">
                    Posted by <a href="user.php?id=13">Edward Francis</a> 
                    <span class="pull-right">3/19/2012</span>
                  </div>
                  <p class="excerpt">
                  The steamer Mongolia, belonging to the Peninsular and Oriental Company, built of iron, of two thousand eight hundred tons burden, and five hundred horse-power, was due at eleven o'clock a.m. on Wednesday, the 9th of October, at Suez. 
                  </p>
                  <p><a href="post.php?id=9" class="btn btn-primary btn-sm">Read more</a></p>
               </div>
           </div>          
         </div>
        
      <!-- replace pagination with function call -->
      <ul class="pagination">
        <li class="disabled"><a href="#">&laquo;</a></li>
        <li><a href="#">1</a></li>
        <li><a href="#">2</a></li>
        <li><a href="#">3</a></li>
        <li class="active"><a href="#">4</a></li>
        <li><a href="#">5</a></li>
        <li><a href="#">6</a></li>
        <li><a href="#">7</a></li>
        <li><a href="#">8</a></li>
        <li><a href="#">9</a></li>
        <li><a href="#">&raquo;</a></li>
      </ul>
      
      </div>  <!-- end main content column -->
   </div>  <!-- end main content row -->
</div>   <!-- end main content container -->
   
   
<footer>
   <div class="container">
      <div class="row">
        <div class="col-md-4">
            <h4>Destinations</h4>
            <ul class="nav nav-pills nav-stacked">
                 <li><a href="#">Asia</a></li>
                 <li><a href="#">Africa</a></li>
                 <li><a href="#">Europe</a></li>
                 <li><a href="#">North America</a></li>
                 <li><a href="#">South America</a></li>
               </ul>  
        </div>
        <div class="col-md-4">
            <h4>Links</h4>
            <nav>
               <ul class="nav nav-pills nav-stacked">
                 <li><a href="#">Home</a></li>
                 <li><a href="#">About</a></li>
                 <li><a href="#">Browse</a></li>
                 <li><a href="#">Search</a></li>
               </ul>            
            </nav>               
        </div>
        <div class="col-md-4">
            <h4>Connect</h4>
            <hr/>
            <div class="row">
               <div class="col-md-6">
                  <button type="button" class="btn btn-primary">Facebook</button>
               </div>
               <div class="col-md-6">
                  <button type="button" class="btn btn-info">Twitter</button>
               </div>
            </div>
            <hr/>
            Phone: 333-444-5555</br>
            Email: support@shareyourtravels.com
        </div>
      </div>
   </div>
   <div class="container">
      <hr/>
      <div class="row">
         <div class="col-md-6">
            <p><small>&copy; 2014 - Share Your Travels</small></p>
         </div>
         <div class="col-md-6">
            <p class="pull-right"><small>A case study for <a href="http://www.fundwebdev.com">Fundamentals of Web Development</a><small></p>
         </div>            
      </div>
   </div>
</footer>
   
   
 <!-- Bootstrap core JavaScript
 ================================================== -->
 <!-- Placed at the end of the document so the pages load faster -->
 <script src="bootstrap3_travelTheme/assets/js/jquery.js"></script>
 <script src="bootstrap3_travelTheme/dist/js/bootstrap.min.js"></script>
 <script src="bootstrap3_travelTheme/assets/js/holder.js"></script>
</body>
</html>