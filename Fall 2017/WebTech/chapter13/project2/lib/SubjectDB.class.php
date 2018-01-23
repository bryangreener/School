<?php
/*
   Handles database access for the Subject table. 

 */
class SubjectDB 
{  

    public function findForArtWork($artWorkId)
    {
        $pdo = DatabaseHelper::setConnectionInfo(array(DBCONNECTION, DBUSER, DBPASS));
         $sql = "SELECT Subjects.SubjectId As SubjectId, SubjectName FROM Subjects INNER JOIN ArtWorkSubjects ON Subjects.SubjectId = ArtWorkSubjects.SubjectID WHERE ArtWorkSubjects.ArtWorkID=?";

        $statement = DatabaseHelper::runQuery($pdo, $sql, Array($artWorkId));
        return $statement;
        
    }

}

?>