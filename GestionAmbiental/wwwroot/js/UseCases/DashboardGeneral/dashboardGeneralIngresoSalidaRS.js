var acr_cmrs = $("#ACR_CMRS_ISRS").val();
var ingreso_salida = $("#IngresoSalida_ISRS").val();
var tipo_residuo = $("#TipoResiduo_ISRS").val();
var motivo = $("#Motivo_ISRS").val();
var fechaInicio = $("#FInicio_ISRS").val();
var fechaFin = $("#FFin_ISRS").val();
var gerencia = $("#Gerencia_ISRS").val();
var empresa = $("#Empresa_ISRS").val();

//Variables ingreso y salida según tipo de residuos
var data_ingreso_tipo_residuo = [];
var data_salida_tipo_residuo = [];
var graph_ingreso_salida_tipo_residuo;
//Variables salida según tipo de residuos
var graph_salida_tipo_residuo;
//Variables destino x salida x TipoResiduo
var datasetDestinoxSalidaxTipoResiduo =[];
/*
$(document).ready(function () {
    ActualizarIngresoSalidaTipoResiduo();
});
*/
function FiltrarIngresoSalidaRS() {
    ActualizarIngresoSalidaTipoResiduo();
    ActualizarDestinoxTipoResiduoxSalida();
}
//Para ingreso y salida según tipo de residuos
function ActualizarIngresoSalidaTipoResiduo() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetIngresoSalidaTipoResiduo",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "empresa": empresa,
        },
        traditional: true,
        success: (response) => {
            if (data_ingreso_tipo_residuo.length > 0 || data_salida_tipo_residuo.length > 0) {
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                var soloSalida = data_salida_tipo_residuo.filter(data => data.valor > 0).sort((a, b) => (a.valor < b.valor) ? 1 : -1);
                var labelSoloSalida = soloSalida.map(data=>data.nombre);
                var dataSoloSalida = soloSalida.map(data=>data.valor);
                for (var i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (var i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Actualizar gráfica ingreso y salida
                graph_ingreso_salida_tipo_residuo.data.labels = labels;
                graph_ingreso_salida_tipo_residuo.data.datasets[0].data = dataIngreso;
                graph_ingreso_salida_tipo_residuo.data.datasets[1].data = dataSalida;
                graph_ingreso_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataIngreso) / 10;
                graph_ingreso_salida_tipo_residuo.update();
                //Actualizar gráfica solo salida
                graph_salida_tipo_residuo.data.labels = labelSoloSalida;
                graph_salida_tipo_residuo.data.datasets[0].data = dataSoloSalida;
                graph_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataSoloSalida) / 10;
                graph_salida_tipo_residuo.update();
            } else {
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                var soloSalida = data_salida_tipo_residuo.filter(data => data.valor > 0).sort((a, b) => (a.valor < b.valor) ? 1 : -1);
                var labelSoloSalida = soloSalida.map(data=>data.nombre);
                var dataSoloSalida = soloSalida.map(data=>data.valor);
                for (var i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (var i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Crear gráfica
                CrearGraficaIngresoSalidaTipoResiduo(labels, dataIngreso, dataSalida, Math.max(dataIngreso) / 10);
                CrearGraficaSalidaTipoResiduo(labelSoloSalida, dataSoloSalida, Math.max(dataSoloSalida) / 10);
            }
        }
    });
}
function CrearGraficaIngresoSalidaTipoResiduo(labels, dataIngreso, dataSalida, stepSize) 
{
    graph_ingreso_salida_tipo_residuo = new Chart(
        document.getElementById("chart_ingreso_salida_tipo_residuo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Cantidad de Residuos Sólido Ingresado (kg)",
                        "data": dataIngreso,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Cantidad de Residuos Sólido Retirado (kg)",
                        "data": dataSalida,
                        "fill": false,
                        "backgroundColor": "rgb(255, 99, 99)",
                        "borderColor": "rgb(255, 99, 99)",
                        "lineTension": 0.1
                    }
                ]
            },
            "options": {
                "scales": {
                    'yAxes': [{
                        'ticks': {
                            'stepSize': stepSize,
                            'beginAtZero': true
                        }
                    }]
                },
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
//Para salida según tipo de residuos
function CrearGraficaSalidaTipoResiduo(labels, dataSalida, stepSize) 
{
    graph_salida_tipo_residuo = new Chart(
        document.getElementById("chart_salida_tipo_residuo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Cantidad de Residuos Sólido Retirado (kg)",
                        "data": dataSalida,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    }
                ]
            },
            "options": {
                "scales": {
                    'yAxes': [{
                        'ticks': {
                            'stepSize': stepSize,
                            'beginAtZero': true
                        }
                    }]
                },
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
//Para destino x salida x TipoResiduo
function ActualizarDestinoxTipoResiduoxSalida() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetDestinoxTipoResiduoxIngresoSalida",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "empresa": empresa,
        },
        traditional: true,
        success: (response) => {
            console.log(response);
            if (data_ingreso_tipo_residuo.length > 0 || data_salida_tipo_residuo.length > 0) {
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                for (var i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (var i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Actualizar gráfica ingreso y salida
                graph_ingreso_salida_tipo_residuo.data.labels = labels;
                graph_ingreso_salida_tipo_residuo.data.datasets[0].data = dataIngreso;
                graph_ingreso_salida_tipo_residuo.data.datasets[1].data = dataSalida;
                graph_ingreso_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataIngreso) / 10;
                graph_ingreso_salida_tipo_residuo.update();
                //Actualizar gráfica solo salida
                graph_salida_tipo_residuo.data.labels = labels;
                graph_salida_tipo_residuo.data.datasets[0].data = dataIngreso;
                graph_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataSalida) / 10;
                graph_salida_tipo_residuo.update();
            } else {
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                for (var i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (var i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Crear gráfica
                CrearGraficaIngresoSalidaTipoResiduo(labels, dataIngreso, dataSalida, Math.max(dataIngreso) / 10);
                CrearGraficaSalidaTipoResiduo(labels, dataSalida, Math.max(dataSalida) / 10);
            }
        }
    });
}