$(document).ready(function (){
    $(".input-login-mc input").val("");


    $(".input-login-mc input").focusout(function(){
        if($(this).val() !== ""){
            $(this).addClass("has-content");
        }
        else{
            $(this).removeClass("has-content");
        }
    });
});