
//listing 15.23 Use of animate() with a step function to do CSS3 rotation

$(this).animate(
                // parameter one: Plain Object with CSS options.
                {opacity:"show","fontSize":"120%","marginRight":"100px"},
                // parameter 2: Plain Object with other options including a
                // step function
                {step: function(now, fx) {
                        // if the method was called for the margin property
                        if (fx.prop=="marginRight") {
                            var angle=(now/100)*360; //percentage of a full circle
                            // Multiple rotation methods to work in multiple browsers
                            $(this).css("transform","rotate("+angle+"deg)");
                            $(this).css("-webkit-transform","rotate("+angle+"deg)");
                            $(this).css("-ms-transform","rotate("+angle+"deg)");
                        }
                    },
                        duration:5000, "easing":"linear"
                        }
);