
//listing 17.6 XML processing using jQuery

art = '<?xml version="1.0" encoding="ISO-8859-1"?>';
art += '<art><painting id="290"><title>Balcony … </art>';
// use jQuery parseXML() function to create the DOM object
xmlDoc = $.parseXML( art );
// convert DOM object to jQuery object
$xml = $( xmlDoc );
// find all the painting elements
$paintings = $xml.find( "painting" );
// loop through each painting element
$paintings.each(function() {
        // display its id
        alert($(this).attr("id"));
        // find the title element within the current painting element
        $title = $(this).find( "title" );
        // and display its content
        alert( $title.text() );
});