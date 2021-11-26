//Accion
$("#btn_nueva_accion").on("click", function () {
    ResetInputsAccion();
    $("#btn_editar_accion").hide();
    $("#btn_guardar_accion").show();
    $("#modal_accion").modal('show');
});
function ResetInputsAccion() {
    $('#input_descripcion_accion').val("");
    $('#select_tipo_accion').val("");
    $('#select_usuarios_accion').val(0);
    $('#select_usuarios_accion2').val(0);
    $('#input_contratista_accion').val("");
    $('#input_fecha_accion').val("");
    $('#select_estatus').val("");
}
function ValidateInputsAccion() {
    var result = true;
    if ($('#input_descripcion_accion').val() == "" ||
        $('#select_tipo_accion').val() == "" ||
        $('#input_fecha_accion').val() == "" ||
        $('#select_estatus').val() == "")
        result = false;
    if (($('#select_usuarios_accion').val() == 0 ||$('#select_usuarios_accion2').val() == 0) &&
        $('#input_contratista_accion').val() == "")
        result = false;
    return result;
}
function EditarAccion(idAccion) {
    $("#Id_Accion").val(0);
    ResetInputsAccion();
    $.ajax({
        type: "GET",
        url: "/InvestigacionFinal/EditarAccion",
        data: {
            IdAccion: idAccion
        },
        success: (response) => {
            $("#modal_accion").modal('show');
            $("#btn_editar_accion").show();
            $("#btn_guardar_accion").hide();

            $("#Id_Accion").val(response.idAccion);
            $("#select_tipo_accion").val(response.tipo),
            $("#input_descripcion_accion").val(response.accion);
            $("#select_usuarios_accion").val(response.idUsuario);
            $("#select_usuarios_accion2").val(response.idUsuario2);
            $("#input_contratista_accion").val(response.responsableCon);
            $("#input_fecha_accion").val(response.fecha);
            $("#select_estatus").val(response.estatus);
        }
    });
}
function EliminarAccion(idAccion) {
    if (confirm('¿Está seguro que desea eliminar esta acción?')) {
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EliminarAccion",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                IdAccion: idAccion,
            },
            success: (response) => {
                $('#tabla_acciones_inmediatas tbody').empty();
                $('#tabla_acciones_inmediatas tbody').html(response.filas_acciones_inmediatas);
                $('#tabla_acciones_correctivas tbody').empty();
                $('#tabla_acciones_correctivas tbody').html(response.filas_acciones_correctivas);
                $('#NAcciones').val(response.nAcciones);
                dataModulo();
            }
        });
    }
}
$("#btn_guardar_accion").on("click", function () {
    $("#danger_accion").html("");
    if (ValidateInputsAccion())
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/NuevaAccion",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                tipoAccion: $("#select_tipo_accion").val(),
                accion: $("#input_descripcion_accion").val(),
                usuario: $('#select_usuarios_accion').val(),
                usuario2: $('#select_usuarios_accion2').val(),
                contratista: $('#input_contratista_accion').val(),
                fecha: $('#input_fecha_accion').val(),
                estado: $('#select_estatus').val()
            },
            success: (response) => {
                ResetInputsAccion();
                $("#modal_accion").modal('hide');
                $('#tabla_acciones_inmediatas tbody').empty();
                $('#tabla_acciones_inmediatas tbody').html(response.filas_acciones_inmediatas);
                $('#tabla_acciones_correctivas tbody').empty();
                $('#tabla_acciones_correctivas tbody').html(response.filas_acciones_correctivas);
                $('#NAcciones').val(response.nAcciones);
                dataModulo();
            }
        });
    else
        $("#danger_accion").html("Debe completar todos los campos");
});
$("#btn_editar_accion").on("click", function () {
    $("#danger_accion").html("");
    if (ValidateInputsAccion() && ($("#IdAccion").val() != "" || $("#IdAccion").val() != 0))
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EditarAccion",
            data: {
                IdAccion: $("#Id_Accion").val(),
                tipoAccion: $("#select_tipo_accion").val(),
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                accion: $("#input_descripcion_accion").val(),
                usuario: $('#select_usuarios_accion').val(),
                usuario2: $('#select_usuarios_accion2').val(),
                contratista: $('#input_contratista_accion').val(),
                fecha: $('#input_fecha_accion').val(),
                estado: $('#select_estatus').val()
            },
            success: (response) => {
                ResetInputsAccion();
                $("#modal_accion").modal('hide');
                $('#tabla_acciones_inmediatas tbody').empty();
                $('#tabla_acciones_inmediatas tbody').html(response.filas_acciones_inmediatas);
                $('#tabla_acciones_correctivas tbody').empty();
                $('#tabla_acciones_correctivas tbody').html(response.filas_acciones_correctivas);
                $('#NAcciones').val(response.nAcciones);
                dataModulo();
            }
        });
    else
        $("#danger_accion").html("Debe completar todos los campos");
});