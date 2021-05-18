$(document).ready(function () {
    //LANDING PAGE
    $('div.landing-page').css('background-image', 'url("/img/landing-page-img.png")');

    //MENU PAGE
    $('.menu_img_bg').css('background-image', 'url("/img/menu-bg.png")');

    //MODAL LOGIN
    $('div.modal .login-image-background').css('background-image', 'url("/img/landing-page-img.png")');

    //MENU MODAL
    $('.modal-menu-inspection .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-rac .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-residuos .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-incidentes .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-fauna .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-agua .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-sga .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-tablas .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-dashboard .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-settings .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //MAIN MODALS
    $('.modal-sga .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //MARCOBRE MESSAGE MODAL
    $('div.modal .mc-modal-image-background').css('background-image', 'url("/img/landing-page-img.png")');

    //INSPECCIONES
    let r1 = 20;
    let g1 = 27;
    let b1 = 89;
    inspectionStyle(r1, g1, b1);
    $("#inspectionColorPicked").html('rgb ('+r1+', '+g1+', '+b1+')');
    let inspectionColor = rgbToHex(r1, g1, b1);
    $("#inspectionColorPicker").val(inspectionColor);

    //RAC
    let r2 = 152;
    let g2 = 0;
    let b2 = 0;
    racStyle(r2, g2, b2);
    $("#racColorPicked").html('rgb (' + r2 + ', ' + g2 + ', ' + b2 + ')');
    let racColor = rgbToHex(r2, g2, b2);
    $("#racColorPicker").val(racColor);

    //RESIDUOS
    let r3 = 110;
    let g3 = 41;
    let b3 = 141;
    residuosStyle(r3, g3, b3);
    $("#residuosColorPicked").html('rgb (' + r3 + ', ' + g3 + ', ' + b3 + ')');
    let residuosColor = rgbToHex(r3, g3, b3);
    $("#residuosColorPicker").val(residuosColor);

    //INCIDENTES
    let r4 = 0;
    let g4 = 188;
    let b4 = 228;
    incidentesStyle(r4, g4, b4);
    $("#incidentesColorPicked").html('rgb (' + r4 + ', ' + g4 + ', ' + b4 + ')');
    let incidentesColor = rgbToHex(r4, g4, b4);
    $("#incidentesColorPicker").val(incidentesColor);

    //FAUNA
    let r5 = 8;
    let g5 = 99;
    let b5 = 61;
    faunaStyle(r5, g5, b5);
    $("#faunaColorPicked").html('rgb (' + r5 + ', ' + g5 + ', ' + b5 + ')');
    let faunaColor = rgbToHex(r5, g5, b5);
    $("#faunaColorPicker").val(faunaColor);

    //AGUA
    let r6 = 65;
    let g6 = 182;
    let b6 = 230;
    aguaStyle(r6, g6, b6);
    $("#aguaColorPicked").html('rgb (' + r6 + ', ' + g6 + ', ' + b6 + ')');
    let aguaColor = rgbToHex(r6, g6, b6);
    $("#aguaColorPicker").val(aguaColor);

    //SGA
    let r7 = 129;
    let g7 = 110;
    let b7 = 12;
    sgaStyle(r7, g7, b7);
    $("#sgaColorPicked").html('rgb (' + r7 + ', ' + g7 + ', ' + b7 + ')');
    let sgaColor = rgbToHex(r7, g7, b7);
    $("#sgaColorPicker").val(sgaColor);

    //TABLAS
    let r8 = 196;
    let g8 = 34;
    let b8 = 151;
    tablasStyle(r8, g8, b8);
    $("#tablasColorPicked").html('rgb (' + r8 + ', ' + g8 + ', ' + b8 + ')');
    let tablasColor = rgbToHex(r8, g8, b8);
    $("#tablasColorPicker").val(tablasColor);

    //DASHBOARD
    let r9 = 152;
    let g9 = 0;
    let b9 = 0;
    dashboardStyle(r9, g9, b9);
    $("#dashboardColorPicked").html('rgb (' + r9 + ', ' + g9 + ', ' + b9 + ')');
    let dashboardColor = rgbToHex(r9, g9, b9);
    $("#dashboardColorPicker").val(dashboardColor);
});

const rgbToHex = (r, g, b) => '#' + [r, g, b].map(x => {
    const hex = x.toString(16)
    return hex.length === 1 ? '0' + hex : hex
}).join('')

function inspectionStyle (r, g, b){
    $('.inspection-opacity-color').css('background-color', 'rgba('+r+', '+g+', '+b+', 0.7)');
    $('.table-responsive.inspection-color table tbody tr td').css({"color":'rgb('+r+', '+g+', '+b+')', "background-color":'rgba('+r+', '+g+', '+b+', 0.2)'});
    $('.action-container.inspection-color').css("border-color", 'rgb('+r+', '+g+', '+b+')');
    $('.inspection .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.inspection-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.inspection-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

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

    //MENU MODAL
    $('.modal-menu-inspection .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
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

function racStyle(r, g, b) {
    $('.rac-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.rac-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.rac-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.rac .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.rac-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.rac-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.rac-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.rac-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.rac-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.rac-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.rac-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-rac .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function residuosStyle(r, g, b) {
    $('.residuos-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.residuos-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.residuos-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.residuos .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.residuos-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.residuos-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.residuos-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.residuos-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.residuos-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.residuos-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.residuos-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-residuos .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function incidentesStyle(r, g, b) {
    $('.incidentes-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.incidentes-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.incidentes-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.incidentes .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.incidentes-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.incidentes-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.incidentes-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.incidentes-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.incidentes-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.incidentes-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.incidentes-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-incidentes .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function faunaStyle(r, g, b) {
    $('.fauna-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.fauna-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.fauna-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.fauna .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.fauna-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.fauna-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.fauna-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.fauna-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.fauna-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.fauna-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.fauna-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-fauna .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function aguaStyle(r, g, b) {
    //MENU MODAL
    $('.modal-menu-agua .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function sgaStyle(r, g, b) {
    $('.sga-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.sga-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.sga-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.sga .image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //PROGRESS BAR
    $('.table-responsive.sga-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.sga-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.sga-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.sga-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.sga-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action-outline.sga-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.sga-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-sga .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function tablasStyle(r, g, b) {
    //MENU MODAL
    $('.modal-menu-tablas .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function dashboardStyle(r, g, b) {
    //MENU MODAL
    $('.modal-menu-dashboard .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}