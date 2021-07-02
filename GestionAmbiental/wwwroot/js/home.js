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

    $('#loginModal').on('shown.bs.modal', function () {
        $('input:text:visible:first').focus();
    })

    $('.login-button button').click(function () {
        if ($("#User").val() === "") {
            $("#error_message").css('display' , 'block');
            $("#error_message").text('Ingrese un Usuario');
        }

        else if ($("#Password").val() === "") {
            $("#error_message").css('display', 'block');
            $("#error_message").text('Ingrese su Contraseña');
        }

        else {
            $("#error_message").css('display', 'none');
            $('.login-button button span').css('display', 'none');
            $(this).html('');
            $(this).fadeIn('slow').append(`<div class="spinner">
                                          <div class="rect1"></div>
                                          <div class="rect2"></div>
                                          <div class="rect3"></div>
                                          <div class="rect4"></div>
                                          <div class="rect5"></div>
                                        </div>`
            );
        }
    });

    $('#loginModal').on('hidden.bs.modal', function () {
        $('.spinner').remove();
        $('.login-button button span').css('display', 'block');
    });

    $('#loginModal').on('keypress', function (e) {
        if (e.which === 13) {
            $('.login-button button').click();
        }
    });
});