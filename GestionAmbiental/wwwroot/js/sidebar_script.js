$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
    compSizeScreen();
});

$(window).resize(function(){
    compSizeScreen()
});

//COMP ACTIVE LIST
$(document).ready(function () {
    $("#sidebar .components .mainlist").click(function () {
        var id = $(this);

        // $(".actived").removeClass("actived");
        // $(id).addClass("actived");
        localStorage.setItem("selectedolditem", $(id).text());
    });

    var selectedolditem = localStorage.getItem('selectedolditem');

    if (selectedolditem !== null) {

        $(".components .mainlist:contains('" + selectedolditem + "')").addClass("actived");
    }
});


//COMP WINDOW SIZE
function compSizeScreen() {
    var ventana_ancho = $(window).width();
    if (window.matchMedia('(max-width: 768px)').matches) {
        if (!$('#sidebar').hasClass('active')) {
            $('#content').css('margin-left', '0px');
            $('#content').css('width', ventana_ancho);
        } else {
            $('#content').css('margin-left', '80px');
            $('#content').css('width', ventana_ancho - 80);
        }
    } else {
        if (!$('#sidebar').hasClass('active')) {
            $('#content').css('margin-left', '250px');
            $('#content').css('width', ventana_ancho - 250);
        } else {
            $('#content').css('margin-left', '100px');
            $('#content').css('width', ventana_ancho - 100);
        }
    }
}

$('#sidebarCollapse').click(function () {
    var ventana_ancho = $(window).width();
    if (window.matchMedia('(max-width: 768px)').matches) {
        if (!$('#sidebar').hasClass('active')) {
            $('#content').css('margin-left', '80px');
            $('#content').css('width', ventana_ancho - 80);
        } else {
            $('#content').css('margin-left', '0px');
            $('#content').css('width', ventana_ancho);
        }
    } else {
        if (!$('#sidebar').hasClass('active')) {
            $('#content').css('margin-left', '100px');
            $('#content').css('width', ventana_ancho - 100);
        } else {
            $('#content').css('margin-left', '250px');
            $('#content').css('width', ventana_ancho - 250);
        }
    }
})
