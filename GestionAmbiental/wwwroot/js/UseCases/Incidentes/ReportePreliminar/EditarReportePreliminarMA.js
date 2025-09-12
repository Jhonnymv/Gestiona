//Involucrados
$("#btn_nuevo_involucrado").on("click", function () {
    $("#select_tipo_involucrado").val("");
    ResetInputsInvolucrado();
    HideInputsInvolucrado();
    $("#btn_editar_involucrado").hide();
    $("#btn_guardar_involucrado").show();
    $("#modal_involucrado").modal('show');
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
function ValidateInputsInvolucrado(tipo_involucrado) {
    var result = true;
    switch (tipo_involucrado) {
        case "Supervisor Inmediato (Titular Minero)": if ($("#select_usuarios").val() == 0 || $("#input_celular").val() == "" || $("#input_puesto").val() == "" || $("#select_turno").val() == 0) result = false; break;
        case "Supervisor Inmediato (Contratista)": if ($("#input_nombres").val() == "" || $("#input_apellidos").val() == "" || $("#input_correo").val() == "" || $("#input_celular").val() == "" || $("#input_puesto").val() == "" || $("#select_turno").val() == "") result = false; break;
        case "Supervisor Marcobre": if ($("#select_usuarios").val() == 0 || $("#input_celular").val() == "" || $("#input_puesto").val() == "" || $("#select_turno").val() == 0) result = false; break;
        case "Otro Involucrado": if ($("#input_nombres").val() == "" || $("#input_apellidos").val() == "" || $("#input_correo").val() == "" || $("#input_celular").val() == "" || $("#input_puesto").val() == "" || $("#select_turno").val() == "") result = false; break;
        case "": result = false; break;
    }
    return result;
}
function EditarInvolucrado(idInvolucrado) {
    $("#Id_Involucrado").val(0);
    ResetInputsInvolucrado();
    HideInputsInvolucrado()
    $.ajax({
        type: "GET",
        url: "/ReportePreliminarMA/EditarInvolucrado",
        data: {
            IdInvolucrado: idInvolucrado
        },
        success: (response) => {
            $("#modal_involucrado").modal('show');
            $("#btn_editar_involucrado").show();
            $("#btn_guardar_involucrado").hide();
            $("#Id_Involucrado").val(response.idInvolucrado);

            if (response.tipo == "Supervisor Inmediato (Titular Minero)") {
                $("#select_tipo_involucrado").val("Supervisor Inmediato (Titular Minero)");
                $("#label_nobmres_apellidos").show();
                $("#select_usuarios").show();
                $("#select_usuarios").val(response.idUsuario);

                $("#label_correo").show();
                $("#input_correo").show();
                $("#input_correo").val(response.email);

                $("#label_celular").show();
                $("#input_celular").show();
                $("#input_celular").val(response.celular);

                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);

                $("#label_turno").show();
                $("#select_turno").show();
                $("#select_turno").val(response.turno);
            }
            if (response.tipo == "Supervisor Inmediato (Contratista)") {
                $("#select_tipo_involucrado").val("Supervisor Inmediato (Contratista)");
                $("#label_nobmres_apellidos").show();
                $("#input_nombres").show();
                $("#input_apellidos").show();
                $("#input_nombres").val(response.nombres);
                $("#input_apellidos").val(response.apellidos);

                $("#label_correo").show();
                $("#input_correo").show();
                $("#input_correo").val(response.email);

                $("#label_celular").show();
                $("#input_celular").show();
                $("#input_celular").val(response.celular);

                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);

                $("#label_turno").show();
                $("#select_turno").show();
                $("#select_turno").val(response.turno);
            }
            if (response.tipo == "Supervisor Marcobre") {
                $("#select_tipo_involucrado").val("Supervisor Marcobre");
                $("#label_nobmres_apellidos").show();
                $("#select_usuarios").show();
                $("#select_usuarios").val(response.idUsuario);

                $("#label_correo").show();
                $("#input_correo").show();
                $("#input_correo").val(response.email);

                $("#label_celular").show();
                $("#input_celular").show();
                $("#input_celular").val(response.celular);

                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);

                $("#label_turno").show();
                $("#select_turno").show();
                $("#select_turno").val(response.turno);
            }
            if (response.tipo == "Otro Involucrado") {
                $("#select_tipo_involucrado").val("Otro Involucrado");
                $("#label_nobmres_apellidos").show();
                $("#input_nombres").show();
                $("#input_apellidos").show();
                $("#input_nombres").val(response.nombres);
                $("#input_apellidos").val(response.apellidos);

                $("#label_correo").show();
                $("#input_correo").show();
                $("#input_correo").val(response.email);

                $("#label_celular").show();
                $("#input_celular").show();
                $("#input_celular").val(response.celular);

                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);

                $("#label_turno").show();
                $("#select_turno").show();
                $("#select_turno").val(response.turno);
            }

        }
    });
}
function EliminarInvolucrado(idInvolucrado) {
    if (confirm('¿Está seguro que desea eliminar este involucrado?')) {
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/EliminarInvolucrado",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
                IdInvolucrado: idInvolucrado,
            },
            success: (response) => {
                $('#tabla_involucrados tbody').empty()
                $('#tabla_involucrados tbody').html(response.filas_involucrados)
                $('#NInvolucrados').val(response.nInvolucrados);
                dataModulo();
            }
        });
    }
}
$("#select_tipo_involucrado").on("change", function () {
    ResetInputsInvolucrado();
    HideInputsInvolucrado();
    if ($("#select_tipo_involucrado").val() == "Supervisor Inmediato (Titular Minero)") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_celular").show();
        $("#input_celular").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
    if ($("#select_tipo_involucrado").val() == "Supervisor Inmediato (Contratista)") {
        $("#label_nobmres_apellidos").show();
        $("#input_nombres").show();
        $("#input_apellidos").show();
        $("#label_correo").show();
        $("#input_correo").show();
        $("#label_celular").show();
        $("#input_celular").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
    if ($("#select_tipo_involucrado").val() == "Supervisor Marcobre") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_celular").show();
        $("#input_celular").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
    if ($("#select_tipo_involucrado").val() == "Otro Involucrado") {
        $("#label_nobmres_apellidos").show();
        $("#input_nombres").show();
        $("#input_apellidos").show();
        $("#label_correo").show();
        $("#input_correo").show();
        $("#label_celular").show();
        $("#input_celular").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_turno").show();
        $("#select_turno").show();
    }
});
$("#btn_guardar_involucrado").on("click", function () {
    $("#danger_involucrado").html("");
    if (ValidateInputsInvolucrado($("#select_tipo_involucrado").val()))
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/NuevoInvolucrado",
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
                $("#tabla_involucrados").show();
                HideInputsInvolucrado();
                ResetInputsInvolucrado();
                $("#modal_involucrado").modal('hide');
                $('#tabla_involucrados tbody').empty()
                $('#tabla_involucrados tbody').html(response.filas_involucrados)
                $('#NInvolucrados').val(response.nInvolucrados);
                dataModulo();
            }
        });
    else
        $("#danger_involucrado").html("Debe completar todos los campos");
});
$("#btn_editar_involucrado").on("click", function () {
    var IdActoCondicion = 0;
    var ActoCondicion = "";
    if ($("#select_acto_condicion").val() == "Acto") {
        IdActoCondicion = $("#select_actos").val();
        ActoCondicion = "Acto Subestándar";
    } else if ($("#select_acto_condicion").val() == "Condicion") {
        IdActoCondicion = $("#select_condiciones").val();
        ActoCondicion = "Condición Subestándar";
    }
    $("#danger_involucrado").html("");
    if (ValidateInputsInvolucrado($("#select_tipo_involucrado").val()) && ($("#Id_Involucrado").val() != "" || $("#Id_Involucrado").val() != 0))
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/EditarInvolucrado",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
                IdInvolucrado: $("#Id_Involucrado").val(),
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
                dataModulo();
            }
        });
    else
        $("#danger_involucrado").html("Debe completar todos los campos");
});
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
        $('#input_fecha_accion').val() == "" ||
        $('#select_estatus').val() == "")
        result = false;
    if ($('#select_usuarios_accion').val() == 0 &&
        $('#input_contratista_accion').val() == "")
        result = false;
    return result;
}
function EditarAccion(idAccion) {
    $("#Id_Accion").val(0);
    ResetInputsAccion();
    $.ajax({
        type: "GET",
        url: "/ReportePreliminarMA/EditarAccion",
        data: {
            IdAccion: idAccion
        },
        success: (response) => {
            $("#modal_accion").modal('show');
            $("#btn_editar_accion").show();
            $("#btn_guardar_accion").hide();

            $("#Id_Accion").val(response.idAccion);
            $("#select_tipo_accion").val(response.tipo);
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
            url: "/ReportePreliminarMA/EliminarAccion",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
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
            url: "/ReportePreliminarMA/NuevaAccion",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
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
            url: "/ReportePreliminarMA/EditarAccion",
            data: {
                IdAccion: $("#Id_Accion").val(),
                IdReporte: $("#Id_Reporte_Preliminar").val(),
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
//Causas
function ResetInputsCausa() {
    $('#select_actos').val(0);
    $('#select_condiciones').val(0);
    $('#select_factores_personales').val(0);
    $('#select_factores_laboral').val(0);
    $('#input_descripcion_causa').val("");
}
function HideInputsCausa() {
    $("#actos_subestandar").hide();
    $("#condiciones_subestandar").hide();
    $("#factores_personales").hide();
    $("#factores_laborales").hide();
    $("#descripcion_causa").hide();
}
function ValidateInputsCausa(actoCondicion) {
    var result = true;
    switch (actoCondicion) {
        case "Acto": if ($('#select_actos').val() == 0 && $('#input_descripcion_causa').val() == "") result = false; break;
        case "Condicion": if ($('#select_condiciones').val() == 0 && $('#input_descripcion_causa').val() == "") result = false; break;
        case "FactorP": if ($('#select_factores_personales').val() == 0 && $('#input_descripcion_causa').val() == "") result = false; break;
        case "FactorL": if ($('#select_factores_laboral').val() == 0 && $('#input_descripcion_causa').val() == "") result = false; break;
        case "": result = false; break;
    }
    return result;
}
function EditarCausa(idCausa) {
    $("#Id_Causa").val(0);
    ResetInputsCausa();
    HideInputsCausa();
    $.ajax({
        type: "GET",
        url: "/ReportePreliminarMA/EditarCausa",
        data: {
            IdCausa: idCausa
        },
        success: (response) => {
            $("#modal_causa").modal('show');
            $("#btn_editar_causa").show();
            $("#btn_guardar_causa").hide();

            $("#Id_Causa").val(response.idCausa);
            $('#input_descripcion_causa').val(response.descripcion)
            $("#descripcion_causa").show();
            if (response.tipoActoCondicion == "Acto Subestándar") {
                $("#actos_subestandar").show();
                $("#select_acto_condicion").val("Acto");
                $("#select_actos").val(response.idActoCondicion);
            }
            else if (response.tipoActoCondicion == "Condición Subestándar") {
                $("#condiciones_subestandar").show();
                $("#select_acto_condicion").val("Condicion");
                $("#select_condiciones").val(response.idActoCondicion);
            }
            else if (response.tipoActoCondicion == "Factor Personal") {
                $("#factores_personales").show();
                $("#select_acto_condicion").val("FactorP");
                $("#select_factores_personales").val(response.idFactor);
            }
            else if (response.tipoActoCondicion == "Factor Laboral") {
                $("#factores_laborales").show();
                $("#select_acto_condicion").val("FactorL");
                $("#select_factores_laboral").val(response.idFactor);
            }
        }
    });
}
function EliminarCausa(idCausa) {
    if (confirm('¿Está seguro que desea eliminar esta causa?')) {
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/EliminarCausa",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
                IdCausa: idCausa,
            },
            success: (response) => {
                $('#tabla_causas_inmediatas tbody').empty();
                $('#tabla_causas_inmediatas tbody').html(response.filas_causas_inmediatas);
                $('#tabla_causas_basicas tbody').empty();
                $('#tabla_causas_basicas tbody').html(response.filas_causas_basicas);
                $('#NCausas').val(response.nCausas);
                dataModulo();
            }
        });
    }
}
$("#btn_nueva_causa").on("click", function () {
    $("#select_acto_condicion").val("");
    ResetInputsCausa();
    HideInputsCausa();
    $("#btn_editar_causa").hide();
    $("#btn_guardar_causa").show();
    $("#modal_causa").modal('show');
});
$("#select_acto_condicion").on("change", function () {
    ResetInputsCausa();
    HideInputsCausa();
    if ($("#select_acto_condicion").val() == "Acto") {
        $("#actos_subestandar").show();
        $("#descripcion_causa").show();
    }
    if ($("#select_acto_condicion").val() == "Condicion") {
        $("#condiciones_subestandar").show();
        $("#descripcion_causa").show();
    }
    if ($("#select_acto_condicion").val() == "FactorP") {
        $("#factores_personales").show();
        $("#descripcion_causa").show();
    }
    if ($("#select_acto_condicion").val() == "FactorL") {
        $("#factores_laborales").show();
        $("#descripcion_causa").show();
    }
});
$("#btn_guardar_causa").on("click", function () {
    var IdActoCondicion = 0;
    var ActoCondicion = "";
    if ($("#select_acto_condicion").val() == "Acto") {
        IdActoCondicion = $("#select_actos").val();
        ActoCondicion = "Acto Subestándar";
    } else if ($("#select_acto_condicion").val() == "Condicion") {
        IdActoCondicion = $("#select_condiciones").val();
        ActoCondicion = "Condición Subestándar";
    } else if ($("#select_acto_condicion").val() == "FactorP") {
        IdActoCondicion = $("#select_factores_personales").val();
        ActoCondicion = "Factor Personal";
    } else if ($("#select_acto_condicion").val() == "FactorL") {
        IdActoCondicion = $("#select_factores_laboral").val();
        ActoCondicion = "Factor Laboral";
    }
    $("#danger_causa").html("");
    if (ValidateInputsCausa($("#select_acto_condicion").val()))
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/NuevaCausa",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
                IdActoCondicion: IdActoCondicion,
                Acto_Condicion: ActoCondicion,
                Descripcion: $("#input_descripcion_causa").val()
            },
            success: (response) => {
                $("#modal_causa").modal('hide');
                HideInputsCausa();
                ResetInputsCausa();
                $('#tabla_causas_inmediatas tbody').empty();
                $('#tabla_causas_inmediatas tbody').html(response.filas_causas_inmediatas);
                $('#tabla_causas_basicas tbody').empty();
                $('#tabla_causas_basicas tbody').html(response.filas_causas_basicas);
                $('#NCausas').val(response.nCausas);
                dataModulo();
            }
        });
    else
        $("#danger_causa").html("Debe completar todos los campos");
});
$("#btn_editar_causa").on("click", function () {
    var IdActoCondicion = 0;
    var ActoCondicion = "";
    if ($("#select_acto_condicion").val() == "Acto") {
        IdActoCondicion = $("#select_actos").val();
        ActoCondicion = "Acto Subestándar";
    } else if ($("#select_acto_condicion").val() == "Condicion") {
        IdActoCondicion = $("#select_condiciones").val();
        ActoCondicion = "Condición Subestándar";
    } else if ($("#select_acto_condicion").val() == "FactorP") {
        IdActoCondicion = $("#select_factores_personales").val();
        ActoCondicion = "Factor Personal";
    } else if ($("#select_acto_condicion").val() == "FactorL") {
        IdActoCondicion = $("#select_factores_laboral").val();
        ActoCondicion = "Factor Laboral";
    }
    $("#danger_causa").html("");
    if (ValidateInputsCausa($("#select_acto_condicion").val()) && ($("#Id_Causa").val() != "" || $("#Id_Causa").val() != 0))
        $.ajax({
            type: "POST",
            url: "/ReportePreliminarMA/EditarCausa",
            data: {
                IdReporte: $("#Id_Reporte_Preliminar").val(),
                IdCausa: $("#Id_Causa").val(),
                IdActoCondicion: IdActoCondicion,
                Acto_Condicion: ActoCondicion,
                Descripcion: $("#input_descripcion_causa").val()
            },
            success: (response) => {
                HideInputsCausa();
                ResetInputsCausa();
                $("#modal_causa").modal('hide');
                $('#tabla_causas_inmediatas tbody').empty();
                $('#tabla_causas_inmediatas tbody').html(response.filas_causas_inmediatas);
                $('#tabla_causas_basicas tbody').empty();
                $('#tabla_causas_basicas tbody').html(response.filas_causas_basicas);
                $('#NCausas').val(response.nCausas);
                dataModulo();
            }
        });
    else
        $("#danger_causa").html("Debe completar todos los campos");
});

$("#Evidencias").change(function () {

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
            var filesname = archivos.length + " Archivos";
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
                    var filesname = this.archivos.length + " Archivos";
                    $("#files_name").text(filesname);
                    $("#danger_evidencia").html("No se admiten este tipo de archivos<br/>");
                    return;
            }
        }
        filesName.length > 0 ? $("#files_name").html(filesName) : null;
    }
});
//Restrict to integer
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}
setInputFilter(document.getElementById("input_celular"), function (value) {
    return /^-?\d*$/.test(value);
});