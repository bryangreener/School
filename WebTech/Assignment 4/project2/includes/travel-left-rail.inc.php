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
              <?php
                foreach($continents as $continent)
                {
                  echo "<li><a href=''>$continent</a></li>";
                }
              ?>

           </ul>
         </div>  <!-- end continents panel -->  
         <div class="panel panel-info">
           <div class="panel-heading">Popular Countries</div>
           <ul class="list-group">  
              <?php
                foreach($countries as $country)
                {
                  echo "<li><a href=''>$country[0]</a></li>";
                }
              ?>
  
           </ul>
         </div>  <!-- end countries panel -->    