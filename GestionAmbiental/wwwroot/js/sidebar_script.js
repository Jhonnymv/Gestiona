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
    compSidebarList()
});

$("#sidebar .components .mainlist ul li a").click(function () {
    $('.list-active').removeClass("list-active");
    $(this).addClass("list-active");
    var id = $(this).closest('.mainlist').children('a').children('span');
    localStorage.setItem("selectedolditem", $(id).text());
    localStorage.setItem("sublist", $(this).text());
    compSidebarList()
});

$("#sidebar .components .mainlist").click(function () {
    if ($(this).children('a').next().length === 0) {
        $('.list-active').removeClass("list-active");
        var id = $(this).children('a').children('span');
        localStorage.setItem("selectedolditem", $(id).text());
        compSidebarList()
    }
});

function compSidebarList() {
    var selectedolditem = localStorage.getItem('selectedolditem');
    if (selectedolditem !== null) {
        var oldsublsit = localStorage.getItem('sublist');
        if (oldsublsit !== null) {
            let searchSublist = $(".components .mainlist a span:contains('" + selectedolditem + "')").closest(".mainlist a").next().children("li").children("a:contains('" + oldsublsit + "')");
            if (searchSublist) {
                $(".components .mainlist a span:contains('" + selectedolditem + "')").closest('.mainlist').addClass("actived");
                $(".components .mainlist a span:contains('" + selectedolditem + "')").closest('.mainlist a').removeClass("collapsed");
                $(".components .mainlist a span:contains('" + selectedolditem + "')").closest('.mainlist a').attr("aria-expanded", "true");
                $(".components .mainlist a span:contains('" + selectedolditem + "')").closest('.mainlist a').next().addClass("show");
                searchSublist.addClass("list-active");
            }
            else if (searchSublist === 0) {
                $(".components .mainlist a span:contains('" + selectedolditem + "')").closest('.mainlist').addClass("actived");
            }
        }
    }
}


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
