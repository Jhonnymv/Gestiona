var resolucion = $("#Resolucion").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Volumen X Fecha
var data_vol_fechas = [];
var graph_vol_fechas;

//Variables gráfico VolumenRestrante
var data_vol_utilizado = [];
var data_vol_restante = [];
var graph_vol_restante;

$(document).ready(function () {
    Filtrar();
});

function Filtrar() {
    ActualizarVolumenFecha();
    ActualizarVolumenRestante();
}

function ActualizarVolumenFecha() {
    resolucion = $("#Resolucion").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();

    if (fechaInicio <= fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesConsumoAguaMar/GetVolumenFecha",
            data: {
                "Resolucion": $("#Resolucion").val(),
                "FechaInicio": $("#FInicio").val(),
                "FechaFin": $("#FFin").val(),
                "frecuencia": $("#frecuencia").val()
            },
            success: (response) => {
                if (data_vol_fechas.length > 0) {
                    //Actualizar contador
                    data_vol_fechas = response.registros;

                    var labels = [];
                    var data = [];
                    for (var i = 0; i < data_vol_fechas.length; i++) {
                        labels.push(data_vol_fechas[i].nombre);
                        data.push(data_vol_fechas[i].valor);
                    }
                    //Actualizar gráfica
                    graph_vol_fechas.data.labels = labels;
                    graph_vol_fechas.data.datasets[0].data = data;
                    graph_vol_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(data) / 10;
                    graph_vol_fechas.update();
                } else {
                    //Actualizar contador
                    data_vol_fechas = response.registros;

                    CargarDataGraficorVolumenFecha();
                }
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
function CargarDataGraficorVolumenFecha() {
    var labels = [];
    var data = [];
    for (var i = 0; i < data_vol_fechas.length; i++) {
        labels.push(data_vol_fechas[i].nombre);
        data.push(data_vol_fechas[i].valor);
    }
    //Crear gráfica
    CrearGraficaVolumenFecha(labels, data, Math.max(data) / 10);
}
function CrearGraficaVolumenFecha(labels, data, stepSize) {
    graph_vol_fechas = new Chart(
        document.getElementById("chart_volumen_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "m3",
                    "data": data,
                    "fill": false,
                    "backgroundColor": "rgb(46, 103, 147)",
                    "borderColor": "rgb(46, 103, 147)",
                    "lineTension": 0.1
                }]
            },
            "options": {
                "responsive": true,
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

//Para gráfico Contador x Fecha
function ActualizarVolumenRestante() {
    $.ajax({
        type: "POST",
        url: "/ReportesConsumoAguaMar/GetVolumenRestante",
        data: {
            "Resolucion": $("#Resolucion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_vol_utilizado.length > 0 || data_vol_restante.length > 0) {
                //Actualizar contador
                data_vol_utilizado = response.registros[0];
                data_vol_restante = response.registros[1];

                var labels = [];
                var dataUtilizado = [];
                var dataRestante = [];
                for (var i = 0; i < data_vol_utilizado.length; i++) {
                    labels.push(data_vol_utilizado[i].nombre);
                    dataUtilizado.push(data_vol_utilizado[i].valor);
                }
                for (var i = 0; i < data_vol_restante.length; i++) {
                    dataRestante.push(data_vol_restante[i].valor);
                }
                //Actualizar gráfica
                graph_vol_restante.data.labels = labels;
                graph_vol_restante.data.datasets[0].data = dataUtilizado;
                graph_vol_restante.data.datasets[1].data = dataRestante;
                graph_vol_restante.options.scales.yAxes[0].ticks.stepSize = Math.max(dataUtilizado) / 10;
                graph_vol_restante.update();
            } else {
                //Actualizar contador
                data_vol_utilizado = response.registros[0];
                data_vol_restante = response.registros[1];

                CargarDataGraficorVolumenRestante()
            }
        }
    });
}

function CargarDataGraficorVolumenRestante() {
    var labels = [];
    var dataUtilizado = [];
    var dataRestante = [];
    for (var i = 0; i < data_vol_utilizado.length; i++) {
        labels.push(data_vol_utilizado[i].nombre);
        dataUtilizado.push(data_vol_utilizado[i].valor);
    }
    for (var i = 0; i < data_vol_restante.length; i++) {
        dataRestante.push(data_vol_restante[i].valor);
    }
    //Crear gráfica
    CrearGraficaVolumenRestante(labels, dataUtilizado, dataRestante, Math.max(dataUtilizado) / 10);
}
function CrearGraficaVolumenRestante(labels, dataUtilizado, dataRestante, stepSize) {
    graph_vol_restante = new Chart(
        document.getElementById("chart_volumen_restante"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "Volumen Utilizado (m3)",
                        "data": dataUtilizado,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "Volumen Restante (m3)",
                        "data": dataRestante,
                        "fill": false,
                        "backgroundColor": "rgb(255, 99, 99)",
                        "borderColor": "rgb(255, 99, 99)",
                        "lineTension": 0.1
                    }
                ]
            },
            "options": {
                "responsive":true,
                "scales": {
                    'yAxes': [{
                        'stacked': true,
                        'ticks': {
                            'stepSize': stepSize,
                            'beginAtZero': true
                        }
                    }],
                    'xAxes': [{
                        'stacked': true,
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