$(document).ready(function () {
    let r1 = 20;
    let g1 = 27;
    let b1 = 89;
    inspectionStyle(r1, g1, b1);
    $("#inspectionColorPicked").html('rgb ('+r1+', '+g1+', '+b1+')');
    let inspectionColor = rgbToHex(r1, g1, b1);
    $("#inspectionColorPicker").val(inspectionColor);
});

const rgbToHex = (r, g, b) => '#' + [r, g, b].map(x => {
    const hex = x.toString(16)
    return hex.length === 1 ? '0' + hex : hex
}).join('')

function inspectionStyle (r, g, b){
    $('.inspection-opacity-color').css('background-color', 'rgba('+r+', '+g+', '+b+', 0.7)');
    $('.table-responsive.inspection-color table tbody tr td').css({"color":'rgb('+r+', '+g+', '+b+')', "background-color":'rgba('+r+', '+g+', '+b+', 0.2)'});
    $('.action-container.inspection-color').css("border-color", 'rgb('+r+', '+g+', '+b+')');
    $('.inspection .image-header-container').css('background-image', 'url("img/back-inspection.png")');

    //PAGINATION STYLE
    $('.pagination-container.inspection-color .pagination .page-item .page-link').attr('style', 'color : rgb('+r+', '+g+', '+b+') !important');
    $('.pagination-container.inspection-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb('+r+', '+g+', '+b+') !important');
    $('.pagination-container.inspection-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb('+r+', '+g+', '+b+') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.inspection-color').css({"border-color": 'rgb('+r+', '+g+', '+b+')', "color":'rgb('+r+', '+g+', '+b+')'});
    $('.btn-special-action-outline.inspection-color').mouseenter(function (){
        $(this).attr('style', 'background-color: rgb('+r+', '+g+', '+b+') !important; color: #ffffff !important');
        $(this).mouseleave(function (){
            $(this).attr('style', 'background-color: transparent; color: rgb('+r+', '+g+', '+b+'); border-color: rgb('+r+', '+g+', '+b+')');
        });
    });

    //LANDING PAGE
    $('div.landing-page').css('background-image', 'url("img/landing-page-img.png")');

    //MENU PAGE
    $('.menu_img_bg').css('background-image', 'url("img/menu-bg.png")');

    //MODAL LOGIN
    $('div.modal .login-image-background').css('background-image', 'url("img/landing-page-img.png")');
}

$("#inspectionColorPicker").change(function() {
    var hex = $(this).val();
    const hexToRgb = hex =>
        hex.replace(/^#?([a-f\d])([a-f\d])([a-f\d])$/i
            ,(m, r, g, b) => '#' + r + r + g + g + b + b)
            .substring(1).match(/.{2}/g)
            .map(x => parseInt(x, 16));

    let r = hexToRgb(hex)[0];
    let g = hexToRgb(hex)[1];
    let b = hexToRgb(hex)[2];
    $("#inspectionColorPicked").html('rgb ('+r+', '+g+', '+b+')')
    inspectionStyle(r, g, b);
});