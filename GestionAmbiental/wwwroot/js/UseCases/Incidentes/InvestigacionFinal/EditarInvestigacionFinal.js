function ValidarInputs() {
    var result = true;

    $("[data-valmsg-for='Num_Trabajadores']").html("");
    $("[data-valmsg-for='Id_Tipo_Incidente']").html("");
    $("[data-valmsg-for='Id_Nivel_Incidente']").html("");
    $("[data-valmsg-for='Lugar_Incidente']").html("");
    $("[data-valmsg-for='Coordenada_Este']").html("");
    $("[data-valmsg-for='Coordenada_Norte']").html("");
    $("[data-valmsg-for='Localidad']").html("");
    $("[data-valmsg-for='Distrito']").html("");
    $("[data-valmsg-for='Provincia']").html("");
    $("[data-valmsg-for='Departamento']").html("");
    $("[data-valmsg-for='Fecha_Incidente']").html("");
    $("[data-valmsg-for='Hora_Incidente']").html("");
    $("[data-valmsg-for='Fecha_Inicio_Investigacion']").html("");
    $("[data-valmsg-for='Turno']").html("");
    $("[data-valmsg-for='Descripcion_Incidente']").html("");
    $("[data-valmsg-for='Id_Gerencia_Responsable']").html("");
    $("[data-valmsg-for='Id_Supervisor']").html("");

    if ($("#Num_Trabajadores").val() == "") {
        $("#Num_Trabajadores").focus();
        $("[data-valmsg-for='Num_Trabajadores']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Id_Tipo_Incidente").val() == 0) {
        $("#Id_Tipo_Incidente").focus();
        $("[data-valmsg-for='Id_Tipo_Incidente']").html("Debe seleccionar una opción");
        result = false;
    }

    if ($("#Id_Nivel_Incidente").val() == 0) {
        $("#Id_Nivel_Incidente").focus();
        $("[data-valmsg-for='Id_Nivel_Incidente']").html("Debe seleccionar una opción");
        result = false;
    }

    if ($("#Lugar_Incidente").val() == "") {
        $("#Lugar_Incidente").focus();
        $("[data-valmsg-for='Lugar_Incidente']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Coordenada_Este").val() == "") {
        $("#Coordenada_Este").focus();
        $("[data-valmsg-for='Coordenada_Este']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Coordenada_Norte").val() == "") {
        $("#Coordenada_Norte").focus();
        $("[data-valmsg-for='Coordenada_Norte']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Localidad").val() == "") {
        $("#Localidad").focus();
        $("[data-valmsg-for='Localidad']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Distrito").val() == "") {
        $("#Distrito").focus();
        $("[data-valmsg-for='Distrito']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Provincia").val() == "") {
        $("#Provincia").focus();
        $("[data-valmsg-for='Provincia']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Departamento").val() == "") {
        $("#Departamento").focus();
        $("[data-valmsg-for='Departamento']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Fecha_Incidente").val() == "") {
        $("#Fecha_Incidente").focus();
        $("[data-valmsg-for='Fecha_Incidente']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Hora_Incidente").val() == "") {
        $("#Hora_Incidente").focus();
        $("[data-valmsg-for='Hora_Incidente']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Fecha_Inicio_Investigacion").val() == "") {
        $("#Fecha_Inicio_Investigacion").focus();
        $("[data-valmsg-for='Fecha_Inicio_Investigacion']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Turno").val() == 0) {
        $("#Turno").focus();
        $("[data-valmsg-for='Turno']").html("Debe seleccionar una opción");
        result = false;
    }

    if ($("#Descripcion_Incidente").val() == "") {
        $("#Descripcion_Incidente").focus();
        $("[data-valmsg-for='Descripcion_Incidente']").html("Debe completar este campo");
        result = false;
    }

    if ($("#Id_Gerencia_Responsable").val() == 0) {
        $("#Id_Gerencia_Responsable").focus();
        $("[data-valmsg-for='Id_Gerencia_Responsable']").html("Debe seleccionar una opción");
        result = false;
    }

    if ($("#Id_Supervisor").val() == 0) {
        $("#Id_Supervisor").focus();
        $("[data-valmsg-for='Id_Supervisor']").html("Debe seleccionar una opción");
        result = false;
    }

    return result;
}