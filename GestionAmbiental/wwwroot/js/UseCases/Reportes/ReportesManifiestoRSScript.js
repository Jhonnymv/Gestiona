var observacion = $("#Observacion").val();
var residuo_solido = $("#Residuo_Solido").val();
var eors_salida = $("#EO_RS_Salida").val();
var eore_llegada = $("#EO_RS_Llegada").val();
var fechaInicio = "";
var fechaFin = "";
var frecuencia = $("#frecuencia").val();

//Variables gráfico Cantidad X Fecha
var data_cant_fechas_conforme = [];
var data_cant_fechas_anulado = [];
var graph_cant_fechas;
//Variable gráfico Observacion
var data_cant_observacion = [];
var graph_cant_observacion;
//Variable gráfico Camion
var data_cant_camion = [];
var graph_cant_camion;
//Variable gráfico Tipo Salida
var data_cant_tipo_salida = [];
var graph_cant_tipo_salida;
//Variable gráfico Tipo Llegada
var data_cant_tipo_llegada = [];
var graph_cant_tipo_llegada;

$(document).ready(function () {
    Filtrar();
});

function Filtrar() {
    observacion = $("#Observacion").val();
    residuo_solido = $("#Residuo_Solido").val();
    eors_salida = $("#EO_RS_Salida").val();
    eore_llegada = $("#EO_RS_Llegada").val();
    fechaInicio = "";
    fechaFin = "";
    frecuencia = $("#frecuencia").val();

    if (fechaInicio < fechaFin || (fechaInicio == "" && fechaFin == "")) {
        ActualizarContadorxFecha();
        ActualizarDatosGraficoObservacion();
        ActualizarDatosGraficoCamion();
        ActualizarDatosGraficoTipoSalida();
        ActualizarDatosGraficoTipoLlegada();
    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
//Para gráfico Contador x Fecha
function ActualizarContadorxFecha() {
    $.ajax({
        type: "POST",
        url: "/ReportesManifiestoRS/GetRegistrosEntreFechas",
        data: {
            observacion : $("#Observacion").val(),
            residuo_solido : $("#Residuo_Solido").val(),
            eors_salida : $("#EO_RS_Salida").val(),
            eors_llegada : $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            frecuencia: $("#frecuencia").val()
        },
        traditional: true,
        success: (response) => {
            console.log(response);
            if (data_cant_fechas_conforme.length > 0 || data_cant_fechas_anulado.length > 0) {
                //Actualizar contador
                data_cant_fechas_conforme = response.nRegistros[0];
                data_cant_fechas_anulado = response.nRegistros[1];

                var labels = [];
                var dataConforme = [];
                var dataAnulado = [];
                for (var i = 0; i < data_cant_fechas_conforme.length; i++) {
                    labels.push(data_cant_fechas_conforme[i].nombre);
                    dataConforme.push(data_cant_fechas_conforme[i].valor);
                }
                for (var i = 0; i < data_cant_fechas_anulado.length; i++) {
                    dataAnulado.push(data_cant_fechas_anulado[i].valor);
                }
                //Actualizar gráfica
                graph_cant_fechas.data.labels = labels;
                graph_cant_fechas.data.datasets[0].data = dataConforme;
                graph_cant_fechas.data.datasets[1].data = dataAnulado;
                graph_cant_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(dataConforme) / 10;
                graph_cant_fechas.update();
            } else {
                //Actualizar contador
                data_cant_fechas_conforme = response.nRegistros[0];
                data_cant_fechas_anulado = response.nRegistros[1];

                var labels = [];
                var dataConforme = [];
                var dataAnulado = [];
                for (var i = 0; i < data_cant_fechas_conforme.length; i++) {
                    labels.push(data_cant_fechas_conforme[i].nombre);
                    dataConforme.push(data_cant_fechas_conforme[i].valor);
                }
                for (var i = 0; i < data_cant_fechas_anulado.length; i++) {
                    dataAnulado.push(data_cant_fechas_anulado[i].valor);
                }
                //Crear gráfica
                CrearGraficaContardorxFecha(labels, dataConforme, dataAnulado, Math.max(dataConforme) / 10);
            }
        }
    });
}
function CrearGraficaContardorxFecha(labels, dataConforme, dataAnulado, stepSize) {
    graph_cant_fechas = new Chart(
        document.getElementById("chart_man_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Manifiesto Conforme",
                        "data": dataConforme,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Manifiesto Anulado",
                        "data": dataAnulado,
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
                        color: 'white',
                        defaultFontSize: 15
                    }
                }
            }
        });
}
//Para gráfico Observacion
function ActualizarDatosGraficoObservacion() {
    $.ajax({
        type: "POST",
        url: "/ReportesManifiestoRS/GetRegistrosObservacion",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_cant_observacion.length > 0) {
                //Actualizar contador
                data_cant_observacion = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_observacion.length; i++) {
                    labels.push(data_cant_observacion[i].nombre);
                    data.push(data_cant_observacion[i].valor);
                }
                //Actualizar gráfica
                graph_cant_observacion.data.labels = labels;
                graph_cant_observacion.data.datasets[0].data = data;
                graph_cant_observacion.update();
            } else {
                //Actualizar contador
                data_cant_observacion = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_observacion.length; i++) {
                    labels.push(data_cant_observacion[i].nombre);
                    data.push(data_cant_observacion[i].valor);
                }
                //Crear gráfica
                CrearGraficoObservacion(labels, data);
            }
        }
    });
}
function CrearGraficoObservacion(labels, data) {
    graph_cant_observacion = new Chart(
        document.getElementById("chart_observacion"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (TM)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#ff6363'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "legend": {
                    "labels": {
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'white',
                        /*formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },*/
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}
//Para gráfico Camion
function ActualizarDatosGraficoCamion() {
    $.ajax({
        type: "POST",
        url: "/ReportesManifiestoRS/GetRegistrosCamion",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_cant_camion.length > 0) {
                //Actualizar contador
                data_cant_camion = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_camion.length; i++) {
                    labels.push(data_cant_camion[i].nombre);
                    data.push(data_cant_camion[i].valor);
                }
                //Actualizar gráfica
                graph_cant_camion.data.labels = labels;
                graph_cant_camion.data.datasets[0].data = data;
                graph_cant_camion.update();
            } else {
                //Actualizar contador
                data_cant_camion = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_camion.length; i++) {
                    labels.push(data_cant_camion[i].nombre);
                    data.push(data_cant_camion[i].valor);
                }
                //Crear gráfica
                CrearGraficoCamion(labels, data);
            }
        }
    });
}
function CrearGraficoCamion(labels, data) {
    graph_cant_camion = new Chart(
        document.getElementById("chart_camion"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (TM)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            beginAtZero: true
                        }
                    }]
                },
                "legend": {
                    "labels": {
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'white',
                        /*formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },*/
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}
//Para gráfico tipo salida
function ActualizarDatosGraficoTipoSalida() {
    $.ajax({
        type: "POST",
        url: "/ReportesManifiestoRS/GetRegistrosTipoSalida",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_cant_tipo_salida.length > 0) {
                //Actualizar contador
                data_cant_tipo_salida = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_tipo_salida.length; i++) {
                    labels.push(data_cant_tipo_salida[i].nombre);
                    data.push(data_cant_tipo_salida[i].valor);
                }
                //Actualizar gráfica
                graph_cant_tipo_salida.data.labels = labels;
                graph_cant_tipo_salida.data.datasets[0].data = data;
                graph_cant_tipo_salida.update();
            } else {
                //Actualizar contador
                data_cant_tipo_salida = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_tipo_salida.length; i++) {
                    labels.push(data_cant_tipo_salida[i].nombre);
                    data.push(data_cant_tipo_salida[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoSalida(labels, data);
            }
        }
    });
}
function CrearGraficoTipoSalida(labels, data) {
    graph_cant_tipo_salida = new Chart(
        document.getElementById("chart_tipo_salida"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (TM)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            beginAtZero: true
                        }
                    }]
                },
                "legend": {
                    "labels": {
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'white',
                        /*formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },*/
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}
//Para gráfico tipo llegada
function ActualizarDatosGraficoTipoLlegada() {
    $.ajax({
        type: "POST",
        url: "/ReportesManifiestoRS/GetRegistrosTipoLlegada",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_cant_tipo_llegada.length > 0) {
                //Actualizar contador
                data_cant_tipo_llegada = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_tipo_llegada.length; i++) {
                    labels.push(data_cant_tipo_llegada[i].nombre);
                    data.push(data_cant_tipo_llegada[i].valor);
                }
                //Actualizar gráfica
                graph_cant_tipo_llegada.data.labels = labels;
                graph_cant_tipo_llegada.data.datasets[0].data = data;
                graph_cant_tipo_llegada.update();
            } else {
                //Actualizar contador
                data_cant_tipo_llegada = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_cant_tipo_llegada.length; i++) {
                    labels.push(data_cant_tipo_llegada[i].nombre);
                    data.push(data_cant_tipo_llegada[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoLlegada(labels, data);
            }
        }
    });
}
function CrearGraficoTipoLlegada(labels, data) {
    graph_cant_tipo_llegada = new Chart(
        document.getElementById("chart_tipo_llegada"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (TM)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            beginAtZero: true
                        }
                    }]
                },
                "legend": {
                    "labels": {
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'white',
                        /*formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },*/
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}