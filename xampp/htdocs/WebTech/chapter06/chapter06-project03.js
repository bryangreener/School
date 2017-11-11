document.addEventListener("DOMContentLoaded", function(event)
{
    var span = document.createElement("span");
    var img = document.createElement("img");
    var container;
    var elements = document.querySelectorAll(".artThumb");
    NodeList.prototype.addEventListener = function(event, func){
        this.forEach(function(content, item){
            content.addEventListener(event, func);
        });
    }
    

    elements.addEventListener("mouseover", function(event){
        var src = event.currentTarget.getAttribute('src');
        img.setAttribute("src", src);
        img.setAttribute("class", "bigImg");
        img.setAttribute("width", "10%");
        img.setAttribute("height", "15%");
        span.appendChild(img);
        span.setAttribute("id", "imgspan");
        container = event.currentTarget.parentNode.appendChild(span);
    });
    elements.addEventListener("mouseout", function(event){
        var node = document.getElementById("imgspan");
        if (node.parentNode) {
          node.parentNode.removeChild(node);
        }
    })
});