$('.next-btn').click( function (){

    $('.message').html('');
    let actual_id_pill = $(this).closest("div .tab-pane")[0].id;
    let actual_number_pill = parseInt(actual_id_pill.match(/(\d+)/g)[0]);
    let name_pill = actual_id_pill.replace (/[0-9]+/g, '');

    let empty_inputs = 0;

    $('#'+actual_id_pill + ' input').each( function (){
        if($(this).val() === ''){
            empty_inputs++;
        }
    });

    $('#'+actual_id_pill + ' select').each( function (){
        if ($(this).is(':disabled') === false) {
            if ($(this).val() === '0') {
                empty_inputs++;
            }
        }
    });

    $('#' + actual_id_pill + ' textarea').each(function () {
        if ($(this).val() === '') {
            empty_inputs++;
        }
    });

    if(empty_inputs > 0){
        $('.message').append('<span class="text-danger">*Todos los campos son obligatorios</span>')
    }
    else if (empty_inputs === 0) {
        $('#' + name_pill+(actual_number_pill) + '-tab').removeClass('active');
        $('#' + name_pill+(actual_number_pill)).removeClass('active show');
        $('#' + name_pill+(actual_number_pill) + '-tab').removeAttr('disabled');

        $('#' + name_pill+(actual_number_pill+1) + '-tab').addClass('active');
        $('#' + name_pill+(actual_number_pill+1)).addClass('active show');
        $('#' + name_pill+(actual_number_pill+1) + '-tab').removeAttr('disabled');
    }

});

$('.back-btn').click( function (){
    let actual_id_pill = $(this).closest("div .tab-pane")[0].id;
    let actual_number_pill = parseInt(actual_id_pill.match(/(\d+)/g)[0]);
    let name_pill = actual_id_pill.replace (/[0-9]+/g, '');

    $('#' + name_pill+(actual_number_pill) + '-tab').removeClass('active');
    $('#' + name_pill+(actual_number_pill)).removeClass('active show');

    $('#' + name_pill+(actual_number_pill-1) + '-tab').addClass('active');
    $('#' + name_pill+(actual_number_pill-1)).addClass('active show');
});