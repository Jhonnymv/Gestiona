$(document).ready(function () {
    dataSlides();

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



    function dataSlides() {
        $.ajax({
            type: "GET",
            url: "/Configuracion/ConsultarSlides",
            traditional: true,
            success: (response) => {
                if (response.success)
                {
                    /*TRAER DATOS*/
                    let data = JSON.parse(response.data);
                    /*CREAR SLIDERS*/
                    let indicators = '';
                    let inner_carousel = '';
                    for (let i = 0; i < data.length; i++) {
                        indicators += `<button type="button" data-bs-target="#mc-home-carousel" data-bs-slide-to="${i}" aria-label="Slide ${i}"></button>`;
                        inner_carousel += `<div class="carousel-item">
                <div>
                    <img src="${data[i].Imagen}" class="d-block w-100" alt="...">
                </div>
                <div class="carousel-caption d-none d-md-block">
                    <h3>${data[i].Titulo}</h3>
                    <p>${data[i].Contenido}</p>
                </div>
                <div class="landing-accesories">
                    <div class="cover-div">
    
                    </div>
                    <div class="bottom-bar">
    
                    </div>
                </div>
            </div>`;
                    }
                    $('#mc-home-carousel .carousel-indicators').append(indicators);
                    $('#mc-home-carousel .carousel-inner').append(inner_carousel);
                    $('.carousel-indicators button').first().addClass('active');
                    $('.carousel-item').first().addClass('active');
                    $('.carousel-indicators').first().attr('aria-current', 'true');
                } else {
                    console.log("Ocurrió un problema: " + response.message);
                }
            }
        });
    }
    //var data = [
    //    { 'titulo': 'Primer Slide', 'contenido': 'Contenido Primer Slide', 'imagen': '/img/landing-page-img.png' },
    //    { 'titulo': 'Segundo Slide', 'contenido': 'Contenido Segundo Slide', 'imagen': '/img/menu-bg.png' },
    //    { 'titulo': 'Tercer Slide', 'contenido': 'Contenido Tercer Slide', 'imagen': '/img/back-inspection.png' },
    //    { 'titulo': 'Cuarto Slide', 'contenido': 'Contenido Cuarto Slide', 'imagen': '/img/back-inspection.png' }
    //];

});