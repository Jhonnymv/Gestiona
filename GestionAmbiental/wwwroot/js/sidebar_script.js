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
        let selectedolditemcomp = $(".components .mainlist a span:contains(" + selectedolditem + ")");
        selectedolditemcomp.each(function (index, element) {
            if (element.textContent.length === selectedolditem.length) {
                $(this).closest('.mainlist').addClass("actived");
                $(this).closest('.mainlist a').removeClass("collapsed");
                $(this).closest('.mainlist a').attr("aria-expanded", "true");
                $(this).closest('.mainlist a').next().addClass("show");
            }
        });

        var oldsublsit = localStorage.getItem('sublist');
        if (oldsublsit !== null) {
            let sublistoption = $(".components .mainlist a span:contains(" + selectedolditem + ")").closest(".mainlist a").next().children("li").children("a:contains('" + oldsublsit + "')");
            sublistoption.each(function (index, element) {
                if (element.textContent.length === oldsublsit.length) {
                    $(this).addClass("list-active");
                }
            });
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
