<?php
/*
   Handles database access for the Genre table. 

 */
class GenreDB 
{  

    public function findForArtWork($artWorkId)
    {
        $pdo = DatabaseHelper::setConnectionInfo(array(DBCONNECTION, DBUSER, DBPASS));
        $sql = "SELECT Genres.GenreID as GenreID, GenreName, Era, Description, Link FROM Genres INNER JOIN ArtWorkGenres ON Genres.GenreID = ArtWorkGenres.GenreID WHERE ArtWorkGenres.ArtWorkID=?";
        $statement = DatabaseHelper::runQuery($pdo, $sql, Array($artWorkId));
        return $statement;
        
    }

}

?>