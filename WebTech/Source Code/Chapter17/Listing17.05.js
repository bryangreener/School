if (window.XMLHttpRequest) {
    // code for IE7+, Firefox, Chrome, Opera, Safari
    xmlhttp=new XMLHttpRequest();
}
else {
    // code for old versions of IE (optional you might just decide to
    // ignore these)
    xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
}
// load the external XML file
xmlhttp.open("GET","art.xml",false);
xmlhttp.send();
xmlDoc=xmlhttp.responseXML;
// now extract a node list of all <painting> elements
paintings = xmlDoc.getElementsByTagName("painting");
if (paintings) {
    // loop through each painting element
    for (var i = 0; i < paintings.length; i++)
        {
            // display its id attribute
            alert("id="+paintings[i].getAttribute("id"));
            // find its <title> element
            title = paintings[i].getElementsByTagName("title");
            if (title) {
                // display the text content of the <title> element
                alert("title="+title[0].textContent);
            }
        }
}
