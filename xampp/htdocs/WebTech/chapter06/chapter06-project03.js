var images = document.querySelectorAll(".artThumb");
var imgLoc = new Array();
for(var i = 0; i < images.length; i + 1)
{
    imgLoc[i] = images.src.substr(18);
}

    $(document).ready(function ()
    {
        $('span').on('mouseenter', function () {
            $('#test').show();
            $(this).css({
                "text-decoration": "underline"
            });
        }).on('mouseleave', function() {
            $('#test').hide();
            $(this).css({
                "text-decoration": ''
            });
        });
    });
    /*for(var j = 0; j < imgLoc.length; j + 1)
    {
        if(x.substr(18) == imgLoc[j])
        {
            img.src = "images/art/" + imgLoc[j];
            break;
        }
    }
    img.className = "bigImg";
    
    s.appendChild(img);
    document.body.appendChild(s);*/