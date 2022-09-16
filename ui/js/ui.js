$(document).ready(function () {
   
    initControlInput();
});

function initControlInput() {
    $(".input").addClass("input--default");

    $(".input-right__body, .input .date").focus(function(e) { 
        if ( $(this).parent().hasClass("input--default") ) {
            $(this).val("");
        }
        $(this).parent().removeClass("input--default");
        $(this).parent().removeClass("input--done");
        $(this).parent().removeClass("input--error");
        $(this).parent().addClass("input--focus");
    });
    $(".input-right__body, .input .date").blur(function(e) { 
        $(this).parent().removeClass("input--focus");
        if ($(this).val() == "") {
            $(this).parent().addClass("input--default");
        } 
        else {
            $(this).parent().addClass("input--done");
        }
    });

    $(".input-left__body").focus(function(e) { 
        if ($(this).parent().hasClass("input--default") ) {
            $(this).val("");
        }
        $(this).parent().removeClass("input--default");
        $(this).parent().removeClass("input--done");
        $(this).parent().removeClass("input--error");
        $(this).parent().addClass("input--focus");
        $(this).parent().children(".input-left__icon").show();
    });
    $(".input-left__body").blur(function(e) { 
        $(this).parent().removeClass("input--focus");
        if ($(this).val() == "") {
            $(this).parent().addClass("input--default");
            $(this).parent().children(".input-left__icon").hide();
        } 
        else {
            $(this).parent().addClass("input--done");
        }
    });
}