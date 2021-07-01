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
    $("#input_doc_identidad").val("");
    $("#input_edad").val("");
    $("#input_puesto").val("");
    $("#input_empresa").val("");
}
function HideInputsInvolucrado() {
    $("#label_nobmres_apellidos").hide();
    $("#select_usuarios").hide();
    $("#input_nombres").hide();
    $("#input_apellidos").hide();
    $("#label_doc_identidad").hide();
    $("#input_doc_identidad").hide();
    $("#label_edad").hide();
    $("#input_edad").hide();
    $("#label_puesto").hide();
    $("#input_puesto").hide();
    $("#label_empresa").hide();
    $("#input_empresa").hide();
}
function ValidateInputsInvolucrado(tipo_involucrado) {
    var result = true;
    switch (tipo_involucrado) {
        case "Supervisor Inmediato (Titular Minero)": if ($("#select_usuarios").val() == 0 ) result = false; break;
        case "Supervisor Inmediato (Contratista)": if ($("#input_nombres").val() == "" || $("#input_apellidos").val() == "" || $("#input_doc_identidad").val() == "" || $("#input_edad").val() == "" || $("#input_puesto").val() == "" || $("#input_empresa").val()=="") result = false; break;
        case "Supervisor Marcobre": if ($("#select_usuarios").val() == 0 || $("#input_doc_identidad").val() == "" || $("#input_edad").val() == "" || $("#input_puesto").val() == "" || $("#input_empresa").val() == "" ) result = false; break;
        case "Otro Involucrado": if ($("#input_nombres").val() == "" || $("#input_apellidos").val() == "" || $("#input_doc_identidad").val() == "" || $("#input_edad").val() == "" || $("#input_puesto").val() == "" || $("#input_empresa").val() == "") result = false; break;
        case "": result = false; break;
    }
    return result;
}
function EditarInvolucrado(idInvolucrado) {
    $("#Id_Involucrado").val(0);
    ResetInputsInvolucrado();
    HideInputsInvolucrado();
    $.ajax({
        type: "GET",
        url: "/InvestigacionFinal/EditarInvolucrado",
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
                $("#label_doc_identidad").show();
                $("#input_doc_identidad").show();
                $("#input_doc_identidad").val(response.docIdentidad);
                $("#label_edad").show();
                $("#input_edad").show();
                $("#input_edad").val(response.edad);
                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);
            }            
            if (response.tipo == "Supervisor Inmediato (Contratista)") {
                $("#select_tipo_involucrado").val("Supervisor Inmediato (Contratista)");
                $("#label_nobmres_apellidos").show();
                $("#input_nombres").show();
                $("#input_apellidos").show();
                $("#input_nombres").val(response.nombres);
                $("#input_apellidos").val(response.apellidos);
                $("#label_doc_identidad").show();
                $("#input_doc_identidad").show();
                $("#input_doc_identidad").val(response.docIdentidad);
                $("#label_edad").show();
                $("#input_edad").show();
                $("#input_edad").val(response.edad);
                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);
                $("#label_empresa").show();
                $("#input_empresa").show();
                $("#input_empresa").val(response.empresa);
            }
            if (response.tipo == "Supervisor Marcobre") {
                $("#select_tipo_involucrado").val("Supervisor Marcobre");
                $("#label_nobmres_apellidos").show();
                $("#select_usuarios").show();
                $("#select_usuarios").val(response.idUsuario);
                $("#label_doc_identidad").show();
                $("#input_doc_identidad").show();
                $("#input_doc_identidad").val(response.docIdentidad);
                $("#label_edad").show();
                $("#input_edad").show();
                $("#input_edad").val(response.edad);
                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);
                $("#label_empresa").show();
                $("#input_empresa").show();
                $("#input_empresa").val(response.empresa);
            }
            if (response.tipo == "Otro Involucrado") {
                $("#select_tipo_involucrado").val("Otro Involucrado");
                $("#label_nobmres_apellidos").show();
                $("#input_nombres").show();
                $("#input_apellidos").show();
                $("#input_nombres").val(response.nombres);
                $("#input_apellidos").val(response.apellidos);
                $("#label_doc_identidad").show();
                $("#input_doc_identidad").show();
                $("#input_doc_identidad").val(response.docIdentidad);
                $("#label_edad").show();
                $("#input_edad").show();
                $("#input_edad").val(response.edad);
                $("#label_puesto").show();
                $("#input_puesto").show();
                $("#input_puesto").val(response.puesto);
                $("#label_empresa").show();
                $("#input_empresa").show();
                $("#input_empresa").val(response.empresa);
            }
        }
    });
}
function EliminarInvolucrado(idInvolucrado) {
    if (confirm('¿Está seguro que desea eliminar este involucrado?')) {
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EliminarInvolucrado",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
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
    $("#select_acto_condicion").val("");
    HideInputsInvolucrado();
    ResetInputsInvolucrado();
    if ($("#select_tipo_involucrado").val() == "Supervisor Inmediato (Titular Minero)") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_doc_identidad").show();
        $("#input_doc_identidad").show();
        $("#label_edad").show();
        $("#input_edad").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
    }
    if ($("#select_tipo_involucrado").val() == "Supervisor Inmediato (Contratista)" ||
        $("#select_tipo_involucrado").val() == "Otro Involucrado") {
        $("#label_nobmres_apellidos").show();
        $("#input_nombres").show();
        $("#input_apellidos").show();
        $("#label_doc_identidad").show();
        $("#input_doc_identidad").show();
        $("#label_edad").show();
        $("#input_edad").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_empresa").show();
        $("#input_empresa").show();
    }
    if ($("#select_tipo_involucrado").val() == "Supervisor Marcobre") {
        $("#label_nobmres_apellidos").show();
        $("#select_usuarios").show();
        $("#label_doc_identidad").show();
        $("#input_doc_identidad").show();
        $("#label_edad").show();
        $("#input_edad").show();
        $("#label_puesto").show();
        $("#input_puesto").show();
        $("#label_empresa").show();
        $("#input_empresa").show();
        $("#label_empresa").show();
        $("#input_empresa").show();
    }
});
$("#btn_guardar_involucrado").on("click", function () {
    $("#danger_involucrado").html("");
    if (ValidateInputsInvolucrado($("#select_tipo_involucrado").val()))
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/NuevoInvolucrado",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                tipo: $("#select_tipo_involucrado").val(),
                usuario: $('#select_usuarios').val(),
                nombres: $('#input_nombres').val(),
                apellidos: $('#input_apellidos').val(),
                docIdentidad: $('#input_doc_identidad').val(),
                edad: $("#input_edad").val(),
                puestoTrabajo: $("#input_puesto").val(),
                empresa: $("#input_empresa").val()
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
    $("#danger_causa").html("");
    if (ValidateInputsInvolucrado($("#select_tipo_involucrado").val()) && ($("#Id_Involucrado").val() != "" || $("#Id_Involucrado").val() != 0))
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EditarInvolucrado",
            data: {
                IdInvolucrado: $("#Id_Involucrado").val(),
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                tipo: $("#select_tipo_involucrado").val(),
                usuario: $('#select_usuarios').val(),
                nombres: $('#input_nombres').val(),
                apellidos: $('#input_apellidos').val(),
                docIdentidad: $('#input_doc_identidad').val(),
                edad: $("#input_edad").val(),
                puestoTrabajo: $("#input_puesto").val(),
                empresa: $("#input_empresa").val()
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
        $("#danger_causa").html("Debe completar todos los campos");
});