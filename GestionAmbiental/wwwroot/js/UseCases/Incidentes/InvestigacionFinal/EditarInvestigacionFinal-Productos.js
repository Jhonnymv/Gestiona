//Productos
$("#btn_nueva_producto").on("click", function () {
    ResetInputsProducto();
    $("#btn_editar_producto").hide();
    $("#btn_guardar_producto").show();
    $("#modal_producto").modal('show');
});
function ResetInputsProducto() {
    $("#select_producto").val("");
    $("#select_tipo_producto").val("");
    $("#select_estado_producto").val("");
    $("#input_cantidad_derramada").val("");
    $("#select_cantidad_derramada").val("");
    $("#input_equipo").val("");
    $("#select_estado_equipo").val("");
    $("#input_cantidad_recuperado").val("");
    $("#select_cantidad_recuperado").val("");
    $("#input_cantidad_no_recuperado").val("");
    $("#select_cantidad_no_recuperado").val("");
}
function ValidateInputsProducto() {
    var result = true;
    if (
        $("#select_producto").val() == "" ||
        $("#select_tipo_producto").val() == "" ||
        $("#select_estado_producto").val() == "" ||
        $("#input_cantidad_derramada").val() == "" ||
        $("#select_cantidad_derramada").val() == "" ||
        $("#input_equipo").val() == "" ||
        $("#select_estado_equipo").val() == "" ||
        $("#input_cantidad_recuperado").val() == "" ||
        $("#select_cantidad_recuperado").val() == "" ||
        $("#input_cantidad_no_recuperado").val() == "" ||
        $("#select_cantidad_no_recuperado").val() == "")
        result = false;
    return result;
}
function EditarProducto(idProducto) {
    $("#Id_Producto").val(0);
    ResetInputsProducto();
    $.ajax({
        type: "GET",
        url: "/InvestigacionFinal/EditarProducto",
        data: {
            IdInvolucrado: idProducto
        },
        success: (response) => {
            $("#modal_producto").modal('show');
            $("#btn_editar_producto").show();
            $("#btn_guardar_producto").hide();
            $("#Id_Producto").val(response.idProducto);

            console.log(response)
             
            $("#select_producto").val(response.producto);
            $("#select_tipo_producto").val(response.tipo);
            $("#select_estado_producto").val(response.estado_producto);
            $("#input_cantidad_derramada").val(response.cantidad_derramada);
            $("#select_cantidad_derramada").val(response.unidad);
            $("#input_equipo").val(response.equipo);
            $("#select_estado_equipo").val(response.estado_equipo);
            $("#input_cantidad_recuperado").val(response.cantidad_recuperado);
            $("#select_cantidad_recuperado").val(response.unidad);
            $("#input_cantidad_no_recuperado").val(response.cantidad_no_recuperado);
            $("#select_cantidad_no_recuperado").val(response.unidad);
        }
    });
}
function EliminarProducto(idProducto) {
    if (confirm('¿Está seguro que desea eliminar este producto?')) {
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EliminarProducto",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                IdProducto: idProducto,
            },
            success: (response) => {
                $('#tabla_productos tbody').empty()
                $('#tabla_productos tbody').html(response.filas_productos)
                $('#NProductos').val(response.nProductos);
            }
        });
    }
}

$("#btn_guardar_producto").on("click", function () {
    $("#danger_productos").html("");
    if (ValidateInputsProducto())
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/NuevoProducto",
            data: {
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                producto: $("#select_producto").val(),
                tipo: $("#select_tipo_producto").val(),
                estado_producto: $("#select_estado_producto").val(),
                cantidad_derramada: $("#input_cantidad_derramada").val(),
                unidad: $("#select_cantidad_derramada").val(),
                equipo: $("#input_equipo").val(),
                estado_equipo: $("#select_estado_equipo").val(),
                cantidad_recuperado: $("#input_cantidad_recuperado").val(),
                cantidad_no_recuperado: $("#input_cantidad_no_recuperado").val()
            },
            success: (response) => {
                ResetInputsProducto();
                $("#modal_producto").modal('hide');
                $('#tabla_productos tbody').empty()
                $('#tabla_productos tbody').html(response.filas_productos)
                $('#NProductos').val(response.nProductos);
            }
        });
    else
        $("#danger_productos").html("Debe completar todos los campos");
});
$("#btn_editar_producto").on("click", function () {
    $("#danger_productos").html("");
    if (ValidateInputsProducto())
        $.ajax({
            type: "POST",
            url: "/InvestigacionFinal/EditarProducto",
            data: {
                IdProducto: $("#Id_Producto").val(),
                IdInvestigacion: $("#Id_Investigacion_Final").val(),
                producto: $("#select_producto").val(),
                tipo: $("#select_tipo_producto").val(),
                estado_producto: $("#select_estado_producto").val(),
                cantidad_derramada: $("#input_cantidad_derramada").val(),
                unidad: $("#select_cantidad_derramada").val(),
                equipo: $("#input_equipo").val(),
                estado_equipo: $("#select_estado_equipo").val(),
                cantidad_recuperado: $("#input_cantidad_recuperado").val(),
                cantidad_no_recuperado: $("#input_cantidad_no_recuperado").val()
            },
            success: (response) => {
                ResetInputsProducto();
                $("#modal_producto").modal('hide');
                $('#tabla_productos tbody').empty()
                $('#tabla_productos tbody').html(response.filas_productos)
                $('#NProductos').val(response.nProductos);
            }
        });
    else
        $("#danger_productos").html("Debe completar todos los campos");
});

$("#select_cantidad_derramada").on('change', function () {
    var unidad = $("#select_cantidad_derramada").val();
    $("#select_cantidad_recuperado").val(unidad);
    $("#select_cantidad_no_recuperado").val(unidad);
});

$('#input_cantidad_recuperado').bind("keyup change", function () {
    var cantidad_derramada = $("#input_cantidad_derramada").val();
    var recuperado = $("#input_cantidad_recuperado").val();
    if (cantidad_derramada != "" && cantidad_derramada > 0)
        if (parseFloat(cantidad_derramada) - parseFloat(recuperado) >= 0)
            $("#input_cantidad_no_recuperado").val(cantidad_derramada - recuperado)
        else {
            $("#input_cantidad_recuperado").val(0);
            $("#input_cantidad_no_recuperado").val(0);
        }
});

$('#input_cantidad_derramada').bind("keyup change", function () {
    var cantidad_derramada = $("#input_cantidad_derramada").val();
    var recuperado = $("#input_cantidad_recuperado").val();
    if (recuperado != "" && recuperado != 0) {
        if (parseFloat(cantidad_derramada) <= parseFloat(recuperado)) {
            $("#input_cantidad_recuperado").val(cantidad_derramada);
            $("#input_cantidad_no_recuperado").val(0);
        }
        else {
            $("#input_cantidad_no_recuperado").val(cantidad_derramada - recuperado)
        }
    }
});