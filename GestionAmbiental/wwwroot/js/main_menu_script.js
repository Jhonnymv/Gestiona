const colors = ['#CD3728', '#00BCE4', '#E6298D', '#C1D82F'];
$('.menu_item').mouseenter(function (){
    const rand = Math.floor(Math.random() * 4);

    $(this).find('svg path').attr('style', 'fill: '+colors[rand]+'');
    $(this).find('span').attr('style', 'color: '+colors[rand]+'');

    $(this).mouseleave(function (){
        $(this).find('svg path').attr('style', 'fill: #2D649B');
        $(this).find('span').attr('style', 'color: #2D649B');
    });
});

$(document).ready(function () {
    compIndicator();
    compScroll();
    localStorage.removeItem('selectedolditem');
    localStorage.removeItem('sublist');

    $('.modal-menu-option').click(function () {
        localStorage.removeItem('selectedolditem');
        localStorage.removeItem('sublist');

        let title = $(this).offsetParent().offsetParent().children().children().children("h3").children("span").text();
        let subMenu = $(this).children(".text-modal-menu-option").children("span").text();
        console.log(title);
        console.log(subMenu);

        localStorage.setItem("selectedolditem", title);
        localStorage.setItem("sublist", subMenu);
    });
});

$(window).resize(function () {
    compIndicator();
    compScroll();
});

$(window).scroll(function () {
    compScroll();
});

function compIndicator() {
    if ($(document).height() > $(window).height() + 70) {
        $('#indicator').show();
    }
    else {
        $('#indicator').hide();
    }
}

function compScroll() {
    if ($(window).scrollTop() + $(window).height() > $(document).height()) {
        $('#indicator').hide();
    }
    else {
        $('#indicator').show();
    }
}

$('#indicator').click(function () {
    $(document).scrollTop($(document).height());
});