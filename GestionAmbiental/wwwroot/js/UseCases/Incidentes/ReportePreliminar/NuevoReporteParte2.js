$("#btn_nuevo_involucrado").on("click", function () {
    $("#modal_involucrado").modal('show');
});
$("#btn_nueva_accion").on("click", function () {
    $("#modal_accion").modal('show');
});
function ResetInputsInvolucrado() {
    $("#select_usuarios").val("0");
    $("#input_nombres").val("");
    $("#input_apellidos").val("");
    $("#input_correo").val("");
    $("#input_puesto").val("");
    $("#input_celular").val("");
    $("#select_turno").val("");
}
function HideInputsInvolucrado() {
    $("#label_nobmres_apellidos").hide();
    $("#select_usuarios").hide();
    $("#input_nombres").hide();
    $("#input_apellidos").hide();
    $("#label_correo").hide();
    $("#input_correo").hide();
    $("#label_puesto").hide();
    $("#input_puesto").hide();
    $("#label_celular").hide();
    $("#input_celular").hide();
    $("#label_turno").hide();
    $("#select_turno").hide();
}
$("#select_tipo_involucrado").on("change", function () {
    HideInputsInvolucrado();
    ResetInputsInvolucrado();
    if ($("#select_tipo_involucrado").val() == "Titular Minero") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_celular").show();
        $("#input_celular").show();
    }
    if ($("#select_tipo_involucrado").val() == "Contratista") {
        $("#label_nobmres_apellidos").show();
        $("#input_nombres").show();
        $("#input_apellidos").show();
        $("#label_correo").show();
        $("#input_correo").show();
        $("#label_celular").show();
        $("#input_celular").show();
    }
    if ($("#select_tipo_involucrado").val() == "Involucrado Interno") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
    if ($("#select_tipo_involucrado").val() == "Involucrado Externo") {
        $("#label_nobmres_apellidos").show();
        $("#input_nombres").show();
        $("#input_apellidos").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
});
$("#btn_guardar_involucrado").on("click", function () {
    $.ajax({
        type: "POST",
        url: "/ReportePreliminar/NuevoInvolucrado",
        data: {
            IdReporte: $("#Id_Reporte_Preliminar").val(),
            tipo: $("#select_tipo_involucrado").val(),
            usuario: $('#select_usuarios').val(),
            nombres: $('#input_nombres').val(),
            apellidos: $('#input_apellidos').val(),
            correo: $('#input_correo').val(),
            puesto: $("#input_puesto").val(),
            celular: $("#input_celular").val(),
            turno: $("#select_turno").val()
        },
        success: (response) => {
            HideInputsInvolucrado();
            ResetInputsInvolucrado();
            $("#modal_involucrado").modal('hide');
            $('#tabla_involucrados tbody').empty()
            $('#tabla_involucrados tbody').html(response.filas_involucrados)
            $('#NInvolucrados').val(response.nInvolucrados);
        }
    });
});

function ResetInputsAccion() {
    $('#input_descripcion_accion').val("");
    $('#select_usuarios_accion').val(0);
    $('#input_contratista_accion').val("");
    $('#input_fecha_accion').val("");
    $('#select_estatus').val("");
}
$("#btn_guardar_accion").on("click", function () {
    $.ajax({
        type: "POST",
        url: "/ReportePreliminar/NuevaAccion",
        data: {
            IdReporte: $("#Id_Reporte_Preliminar").val(),
            descripcion: $("#input_descripcion_accion").val(),
            usuario: $('#select_usuarios_accion').val(),
            contratista: $('#input_contratista_accion').val(),
            fecha: $('#input_fecha_accion').val(),
            estado: $('#select_estatus').val()
        },
        success: (response) => {
            ResetInputsInvolucrado();
            $("#modal_accion").modal('hide');
            $('#tabla_acciones tbody').empty();
            $('#tabla_acciones tbody').html(response.filas_acciones);
            $('#NAcciones').val(response.nAcciones);
        }
    });
});

$("#boton_guardar_estado").on("click", function () {
    $("#Opcion").val("GuardarEstado");
    $("#form_nuevoReporteParte2").submit();
});

$("#btn_nueva_causa").on("click", function () {
    $("#modal_causa").modal('show');
});
function ResetInputsCausa() {
    $('#select_actos').val(0);
    $('#select_condiciones').val(0);
}
function HideInputsCausa() {
    $("#actos_subestandar").hide();
    $("#condiciones_subestandar").hide();
}
$("#select_acto_condicion").on("change", function () {
    ResetInputsCausa();
    HideInputsCausa();
    if ($("#select_acto_condicion").val() == "Acto") {       
        $("#actos_subestandar").show();
    }
    if ($("#select_acto_condicion").val() == "Condicion") {
        $("#condiciones_subestandar").show();
    }
});
$("#btn_guardar_causa").on("click", function () {
    var IdActoCondicion = 0;
    var ActoCondicion = "";
    if ($("#select_acto_condicion").val() == "Acto") {
        IdActoCondicion = $("#select_actos").val();
        ActoCondicion = "Acto Subestándar";
    }else if ($("#select_acto_condicion").val() == "Condicion") {
        IdActoCondicion = $("#select_condiciones").val();
        ActoCondicion = "Condición Subestándar";
    }
    console.log(IdActoCondicion)
    $.ajax({
        type: "POST",
        url: "/ReportePreliminar/NuevaCausa",
        data: {
            IdReporte: $("#Id_Reporte_Preliminar").val(),
            IdActoCondicion: IdActoCondicion,
            Acto_Condicion: ActoCondicion
        },
        success: (response) => {
            HideInputsInvolucrado();
            ResetInputsInvolucrado();
            $("#modal_causa").modal('hide');
            $('#tabla_causas tbody').empty()
            $('#tabla_causas tbody').html(response.filas_acciones)
            $('#NCausas').val(response.nAcciones);
        }
    });
});

$("#boton_completar").on("click", function () {

    $("#alert_causas").html("");
    $("#alert_involucrados").html("");
    $("#alert_acciones").html("");

    $("#Opcion").val("Completar");
    var nCausas = $("#NCausas").val();
    var nAcciones = $("#NAcciones").val();
    var nInvolucrado = $("#NInvolucrados").val();
    if (nCausas > 0 && nAcciones > 0 && nInvolucrado > 0)
        $("#form_nuevoReporteParte2").submit();
    else {
        if (nCausas == 0)
            $("#alert_causas").html("Debe agregar causas del incidente");
        if (nInvolucrado == 0)
            $("#alert_involucrados").html("Debe agregar involucrados en el incidente");
        if (nAcciones == 0)
            $("#alert_acciones").html("Debe agregar acciones correctivas inmediatas");
    }
});
$("#Evidencias").change(function () {
    console.log($("#Evidencias").val())

    $("#danger_evidencia").html("");
    //Validacion del tamaño del archivo
    var maxSize = $("#Evidencias").data('max-size');
    var archivos = $("#Evidencias").get(0).files;

    if (archivos.length > 0) {
        var fileSize = 0
        for (var i = 0; i < archivos.length; i++) {
            fileSize += archivos[i].size;
        }
        if (fileSize > maxSize) {
            this.value = '';
            var filesname = this.files.length + " Archivos";
            $("#danger_evidencia").html("El  archivo supera el límite permitido de 8MB<br/>");
            return;
        }
        var filesName = "";
        for (var i = 0; i < archivos.length; i++) {
            switch (this.value.match(/\.([^\.]+)$/)[1]) {
                case 'jpg':
                case 'JPG':
                case 'jpeg':
                case 'JPEG':
                case 'png':
                case 'PNG':
                case 'xls':
                case 'xlsx':
                case 'doc':
                case 'docx':
                case 'pdf':
                    filesName += archivos[i].name + "<br/>";
                    break;
                default:
                    this.value = '';
                    var filesname = this.files.length + " Archivos";
                    $("#files_name").text(filesname);
                    $("#danger_evidencia").html("No se admiten este tipo de archivos<br/>");
                    return;
            }
        }
        filesName.length > 0 ? $("#files_name").html(filesName) : null;
    }
});