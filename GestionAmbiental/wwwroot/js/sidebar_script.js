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
    $("#sidebar .components .mainlist ul li a").click(function () {
        $('.list-active').removeClass("list-active");
        $(this).addClass("list-active");
        var id = $(this).closest('.mainlist');
        localStorage.setItem("selectedolditem", $(id).text());
        localStorage.setItem("sublist", $(this).text());
    });

    var selectedolditem = localStorage.getItem('selectedolditem');
    if (selectedolditem !== null) {
        $(".components .mainlist:contains('" + selectedolditem + "')").addClass("actived");
        $(".components .mainlist:contains('" + selectedolditem + "') a").removeClass("collapsed");
        $(".components .mainlist:contains('" + selectedolditem + "') a").attr("aria-expanded", "true");
        $(".components .mainlist:contains('" + selectedolditem + "') ul").addClass("show");
    }

    var oldsublsit = localStorage.getItem('sublist');
    console.log(oldsublsit)
    if (oldsublsit !== null) {
        $(".components .mainlist:contains('" + selectedolditem + "') ul li a:contains('" + oldsublsit + "')").addClass("list-active");
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
