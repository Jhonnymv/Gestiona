$(document).ready(function () {
    //IMG MODAL LOGIN
    $('div.modal .login-image-background').css('background-image', 'url("/img/landing-page-img.png")');

    //MENU PAGE
    $('.menu_img_bg').css('background-image', 'url("/img/uploads/Home/Modulo_Modulos20220222231041.png")');

    //MENU MODALS
    $('.modal-menu-inspection .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Inspection20210817204730.jpg")');
    $('.modal-menu-rac .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_RAC20210817205422.jpeg")');
    $('.modal-menu-residuos .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Residuos20210817204730.jpg")');
    $('.modal-menu-incidentes .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Incidentes20210817205858.JPG")');
    $('.modal-menu-fauna .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Fauna20210817204745.jpg")');
    $('.modal-menu-agua .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Agua20210817205022.jpg")');
    $('.modal-menu-sga .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_SGA20210817205037.jpg")');
    $('.modal-menu-tablas .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Tablas20210701221654.jpg")');
    $('.modal-menu-dashboard .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
    $('.modal-menu-compromisos .modal-content .modal-image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Compromisos20211227175905.jpg")');

    $('.modal-menu-marcobre .modal-content .modal-image-header-container').css('background-image', 'url("/img/landing-page-img.png")');

    //MAIN MODALS
    $('.modal-sga .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');

    //MARCOBRE MESSAGE MODAL
    $('div.modal .mc-modal-image-background').css('background-image', 'url("/img/landing-page-img.png")');

    //INSPECCIONES
    let r1 = 17;
    let g1 = 85;
    let b1 = 212;
    inspectionStyle(r1, g1, b1);
    $("#inspectionColorPicked").html('rgb ('+r1+', '+g1+', '+b1+')');
    let inspectionColor = rgbToHex(r1, g1, b1);
    $("#inspection-colorPicker").val(inspectionColor);

    //RAC
    let r2 = 217;
    let g2 = 69;
    let b2 = 79;
    racStyle(r2, g2, b2);
    $("#racColorPicked").html('rgb (' + r2 + ', ' + g2 + ', ' + b2 + ')');
    let racColor = rgbToHex(r2, g2, b2);
    $("#rac-colorPicker").val(racColor);

    //RESIDUOS
    let r3 = 213;
    let g3 = 178;
    let b3 = 1;
    residuosStyle(r3, g3, b3);
    $("#residuosColorPicked").html('rgb (' + r3 + ', ' + g3 + ', ' + b3 + ')');
    let residuosColor = rgbToHex(r3, g3, b3);
    $("#residuos-colorPicker").val(residuosColor);

    //INCIDENTES
    let r4 = 254;
    let g4 = 144;
    let b4 = 42;
    incidentesStyle(r4, g4, b4);
    $("#incidentesColorPicked").html('rgb (' + r4 + ', ' + g4 + ', ' + b4 + ')');
    let incidentesColor = rgbToHex(r4, g4, b4);
    $("#incidentes-colorPicker").val(incidentesColor);

    //FAUNA
    let r5 = 60;
    let g5 = 195;
    let b5 = 87;
    faunaStyle(r5, g5, b5);
    $("#faunaColorPicked").html('rgb (' + r5 + ', ' + g5 + ', ' + b5 + ')');
    let faunaColor = rgbToHex(r5, g5, b5);
    $("#fauna-colorPicker").val(faunaColor);

    //AGUA
    let r6 = 3;
    let g6 = 95;
    let b6 = 165;
    aguaStyle(r6, g6, b6);
    $("#aguaColorPicked").html('rgb (' + r6 + ', ' + g6 + ', ' + b6 + ')');
    let aguaColor = rgbToHex(r6, g6, b6);
    $("#agua-colorPicker").val(aguaColor);

    //SGA
    let r7 = 55;
    let g7 = 1;
    let b7 = 91;
    sgaStyle(r7, g7, b7);
    $("#sgaColorPicked").html('rgb (' + r7 + ', ' + g7 + ', ' + b7 + ')');
    let sgaColor = rgbToHex(r7, g7, b7);
    $("#sga-colorPicker").val(sgaColor);

    //TABLAS
    let r8 = 117;
    let g8 = 0;
    let b8 = 12;
    tablasStyle(r8, g8, b8);
    $("#tablasColorPicked").html('rgb (' + r8 + ', ' + g8 + ', ' + b8 + ')');
    let tablasColor = rgbToHex(r8, g8, b8);
    $("#tablas-colorPicker").val(tablasColor);

    //DASHBOARD
    let r9 = 0;
    let g9 = 0;
    let b9 = 0;
    dashboardStyle(r9, g9, b9);
    $("#dashboard-ColorPicked").html('rgb (' + r9 + ', ' + g9 + ', ' + b9 + ')');
    let dashboardColor = rgbToHex(r9, g9, b9);
    $("#dashboard-colorPicker").val(dashboardColor);

    //COMPROMISOS
    let r10 = 0;
    let g10 = 188;
    let b10 = 228;
    compromisosStyle(r10, g10, b10);
    $("#compromisos-ColorPicked").html('rgb (' + r10 + ', ' + g10 + ', ' + b10 + ')');
    let compromisosColor = rgbToHex(r10, g10, b10);
    $("#compromisos-colorPicker").val(compromisosColor);
});

const rgbToHex = (r, g, b) => '#' + [r, g, b].map(x => {
    const hex = x.toString(16)
    return hex.length === 1 ? '0' + hex : hex
}).join('')

function inspectionStyle (r, g, b){
    $('.inspection-opacity-color').css('background-color', 'rgba('+r+', '+g+', '+b+', 0.7)');
    $('.table-responsive.inspection-color table tbody tr td').css({"color":'rgb('+r+', '+g+', '+b+')', "background-color":'rgba('+r+', '+g+', '+b+', 0.2)'});
    $('.action-container.inspection-color').css("border-color", 'rgb('+r+', '+g+', '+b+')');
    $('.inspection .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Inspection20210817204730.jpg")');

    //DIV SEPARATORS
    $('.inspection .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.inspection .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.inspection .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.inspection .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.inspection .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.inspection-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.inspection-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.inspection-color .pagination .page-item .page-link').attr('style', 'color : rgb('+r+', '+g+', '+b+') !important');
    $('.pagination-container.inspection-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb('+r+', '+g+', '+b+') !important');
    $('.pagination-container.inspection-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb('+r+', '+g+', '+b+') !important');


    //MAIN BUTTON
    $('.btn-special-action.inspection-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.inspection-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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

function racStyle(r, g, b) {
    $('.rac-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.rac-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.rac-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.rac .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_RAC20210817205422.jpeg")');

    //DIV SEPARATORS
    $('.rac .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.rac .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.rac .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.rac .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.rac .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.rac-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.rac-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.rac-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.rac-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.rac-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.rac-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.rac-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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
    $('.residuos .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Residuos20210817204730.jpg")');

    //DIV SEPARATORS
    $('.residuos .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.residuos .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.residuos .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.residuos .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.residuos .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.residuos-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.residuos-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.residuos-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.residuos-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.residuos-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.residuos-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.residuos-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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
    $('.incidentes .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Incidentes20210817205858.JPG")');

    //DIV SEPARATORS
    $('.incidentes .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.incidentes .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.incidentes .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.incidentes .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.incidentes .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.incidentes-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.incidentes-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.incidentes-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.incidentes-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.incidentes-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.incidentes-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.incidentes-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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
    $('.fauna .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Fauna20210817204745.jpg")');

    //DIV SEPARATORS
    $('.fauna .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.fauna .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.fauna .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.fauna .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.fauna .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.fauna-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.fauna-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.fauna-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.fauna-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.fauna-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.fauna-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.fauna-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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
    $('.agua-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.agua-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.agua-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.agua .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Agua20210817205022.jpg")');

    //DIV SEPARATORS
    $('.agua .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.agua .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.agua .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.agua .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.agua .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.agua-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.agua-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.agua-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.agua-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.agua-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.agua-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.agua-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

    $('.btn-special-action-outline.agua-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.agua-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-agua .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function sgaStyle(r, g, b) {
    $('.sga-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.sga-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.sga-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.sga .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_SGA20210817205037.jpg")');

    //DIV SEPARATORS
    $('.sga .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.sga .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.sga .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.sga .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.sga .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.sga-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.sga-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.sga-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.sga-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.sga-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.sga-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.sga-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

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
    $('.tablas-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.tablas-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.tablas-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.tablas .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Tablas20210701221654.jpg")');

    //DIV SEPARATORS
    $('.tablas .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.tablas .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.tablas .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.tablas .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.tablas .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.tablas-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.tablas-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.tablas-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.tablas-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.tablas-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.tablas-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.tablas-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

    $('.btn-special-action-outline.tablas-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.tablas-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-tablas .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function dashboardStyle(r, g, b) {
    //MENU MODAL
    $('.modal-menu-dashboard .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}

function compromisosStyle(r, g, b) {
    $('.compromisos-opacity-color').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
    $('.table-responsive.compromisos-color table tbody tr td').css({ "color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgba(' + r + ', ' + g + ', ' + b + ', 0.2)' });
    $('.action-container.compromisos-color').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.compromisos .image-header-container').css('background-image', 'url("/img/uploads/Home/Modulo_Compromisos20211227175905.jpg")');

    //DIV SEPARATORS
    $('.compromisos .mc-separator-top-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.compromisos .mc-separator-top-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.compromisos .mc-separator-bottom-right').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.compromisos .mc-separator-bottom-left').css("border-color", 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PROGRESS BAR
    $('.compromisos .progress-mc .progress-bar').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.compromisos-color table tbody .progress').css('background-color', 'rgb(' + r + ', ' + g + ', ' + b + ')');
    $('.table-responsive.compromisos-color table tbody .progress .percent').css('color', 'rgb(' + r + ', ' + g + ', ' + b + ')');

    //PAGINATION STYLE
    $('.pagination-container.compromisos-color .pagination .page-item .page-link').attr('style', 'color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.compromisos-color .pagination .page-item.active .page-link').attr('style', 'border-color : rgb(' + r + ', ' + g + ', ' + b + ') !important');
    $('.pagination-container.compromisos-color .pagination .page-item.active .page-link').attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important');


    //MAIN BUTTON
    $('.btn-special-action.compromisos-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "background-color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action.compromisos-color').mouseenter(function () {
        $(this).attr('style', 'box-shadow: 0px 4px 20px rgba(' + r + ', ' + g + ', ' + b + ', 0.75) !important; border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        $(this).mouseleave(function () {
            $(this).attr('style', 'box-shadow: 0px 4px 4px rgba(0, 0, 0, 0.25); border-color: rgb(' + r + ', ' + g + ', ' + b + '); background-color:rgb(' + r + ', ' + g + ', ' + b + '); color: #ffffff');
        });
    });

    $('.btn-special-action-outline.compromisos-color').css({ "border-color": 'rgb(' + r + ', ' + g + ', ' + b + ')', "color": 'rgb(' + r + ', ' + g + ', ' + b + ')' });
    $('.btn-special-action-outline.compromisos-color').mouseenter(function () {
        $(this).attr('style', 'background-color: rgb(' + r + ', ' + g + ', ' + b + ') !important; color: #ffffff !important');
        $(this).mouseleave(function () {
            $(this).attr('style', 'background-color: transparent; color: rgb(' + r + ', ' + g + ', ' + b + '); border-color: rgb(' + r + ', ' + g + ', ' + b + ')');
        });
    });

    //MENU MODAL
    $('.modal-menu-compromisos .modal-content .modal-image-header-container .modal-header').css('background-color', 'rgba(' + r + ', ' + g + ', ' + b + ', 0.7)');
}