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
function ValidateInputsCausa(tipoCausa) {
    var result = true;
    switch (tipoCausa) {
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
        url: "/InvestigacionFinal/EditarCausa",
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
            url: "/InvestigacionFinal/EliminarCausa",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
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
            url: "/InvestigacionFinal/NuevaCausa",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
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
            url: "/InvestigacionFinal/EditarCausa",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
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