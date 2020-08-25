var tipo = $("#NombreComun").val();
var nivel = $("#TipoAvistamiento").val();
var gerencia = $("#EstadoFauna").val();
var supervisor = $("#LugarReferencia").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Avistamientos X Fecha
var data_avist_fechas = [];
var graph_avist_fechas;

//Variables gráfico Nombre Comun
var data_nombre_comun = [];
var graph_nombre_comun;

//Variables gráfico Estado Fauna
var data_estado_fauna = [];
var graph_estado_fauna;

//Variables gráfico Tipo Avistamiento
var data_tipo_avistamiento = [];
var graph_tipo_avistamiento;

$(document).ready(function () {
    Filtrar();
});

function Filtrar() {
    ActualizarAvistamientosFaunaFecha();
    ActualizaNombreComun();
    ActualizaEstadoFauna();
    ActualizaTipoAvistamiento();
}
function ActualizarAvistamientosFaunaFecha() {
    tipo = $("#NombreComun").val();
    nivel = $("#TipoAvistamiento").val();
    gerencia = $("#EstadoFauna").val();
    supervisor = $("#LugarReferencia").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();

    if (fechaInicio <= fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesAvistamientoFauna/GetAvistamientosFaunaFechas",
            data: {
                "nombre_comun": $("#NombreComun").val(),
                "tipo_avistamiento": $("#TipoAvistamiento").val(),
                "estado_fauna": $("#EstadoFauna").val(),
                "lugar_referencia": $("#LugarReferencia").val(),
                "FechaInicio": $("#FInicio").val(),
                "FechaFin": $("#FFin").val(),
                "frecuencia": $("#frecuencia").val()
            },
            success: (response) => {
                if (data_avist_fechas.length > 0) {
                    //Actualizar contador
                    data_avist_fechas = response.registros;

                    var labels = [];
                    var data = [];
                    for (var i = 0; i < data_avist_fechas.length; i++) {
                        labels.push(data_avist_fechas[i].nombre);
                        data.push(data_avist_fechas[i].valor);
                    }
                    //Actualizar gráfica
                    graph_avist_fechas.data.labels = labels;
                    graph_avist_fechas.data.datasets[0].data = data;
                    graph_avist_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(data) / 10;
                    graph_avist_fechas.update();
                } else {
                    //Actualizar contador
                    data_avist_fechas = response.registros;

                    var labels = [];
                    var data = [];
                    for (var i = 0; i < data_avist_fechas.length; i++) {
                        labels.push(data_avist_fechas[i].nombre);
                        data.push(data_avist_fechas[i].valor);
                    }
                    //Crear gráfica
                    CrearGraficaAvistamientosFaunaFecha(labels, data, Math.max(data) / 10);
                }
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
function CrearGraficaAvistamientosFaunaFecha(labels, data, stepSize) {
    graph_avist_fechas = new Chart(
        document.getElementById("chart_avistamientos_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Avistamientos",
                    "data": data,
                    "fill": false,
                    "backgroundColor": "rgb(46, 103, 147)",
                    "borderColor": "rgb(46, 103, 147)",
                    "lineTension": 0.1
                }]
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

//Para gráfico Nombre Común
function ActualizaNombreComun() {
    $.ajax({
        type: "POST",
        url: "/ReportesAvistamientoFauna/GetAvistamientosNombreComun",
        data: {
            "nombre_comun": $("#NombreComun").val(),
            "tipo_avistamiento": $("#TipoAvistamiento").val(),
            "estado_fauna": $("#EstadoFauna").val(),
            "lugar_referencia": $("#LugarReferencia").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_nombre_comun.length > 0) {
                //Actualizar contador
                data_nombre_comun = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_nombre_comun.length; i++) {
                    labels.push(data_nombre_comun[i].nombre);
                    data.push(data_nombre_comun[i].valor);
                }
                //Actualizar gráfica
                graph_nombre_comun.data.labels = labels;
                graph_nombre_comun.data.datasets[0].data = data;
                graph_nombre_comun.update();
            } else {
                //Actualizar contador
                data_nombre_comun = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_nombre_comun.length; i++) {
                    labels.push(data_nombre_comun[i].nombre);
                    data.push(data_nombre_comun[i].valor);
                }
                //Crear gráfica
                CrearGraficoNombreComun(labels, data);
            }
        }
    });
}
function CrearGraficoNombreComun(labels, data) {
    graph_nombre_comun = new Chart(
        document.getElementById("chart_avistamientos_nombre_comun"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Avistamientos",
                    "data": data,
                    "backgroundColor": '#2E6793',
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
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}

//Para gráfico Estado Fauna
function ActualizaEstadoFauna() {
    $.ajax({
        type: "POST",
        url: "/ReportesAvistamientoFauna/GetAvistamientosEstadoFauna",
        data: {
            "nombre_comun": $("#NombreComun").val(),
            "tipo_avistamiento": $("#TipoAvistamiento").val(),
            "estado_fauna": $("#EstadoFauna").val(),
            "lugar_referencia": $("#LugarReferencia").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_estado_fauna.length > 0) {
                //Actualizar contador
                data_estado_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_estado_fauna.length; i++) {
                    labels.push(data_estado_fauna[i].nombre);
                    data.push(data_estado_fauna[i].valor);
                }
                //Actualizar gráfica
                graph_estado_fauna.data.labels = labels;
                graph_estado_fauna.data.datasets[0].data = data;
                graph_estado_fauna.update();
            } else {
                //Actualizar contador
                data_estado_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_estado_fauna.length; i++) {
                    labels.push(data_estado_fauna[i].nombre);
                    data.push(data_estado_fauna[i].valor);
                }
                //Crear gráfica
                CrearGraficoEstadoFauna(labels, data);
            }
        }
    });
}
function CrearGraficoEstadoFauna(labels, data) {
    graph_estado_fauna = new Chart(
        document.getElementById("chart_avistamientos_estado_fauna"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Avistamientos",
                    "data": data,
                    "backgroundColor": [
                        '#2EFE2E',
                        '#2E6793',
                        '#CCCF00',
                        '#FF8000',
                        '#FE2E2E'
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
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}

//Para gráfico Tipo Avistamiento
function ActualizaTipoAvistamiento() {
    $.ajax({
        type: "POST",
        url: "/ReportesAvistamientoFauna/GetAvistamientosTipoAvistamiento",
        data: {
            "nombre_comun": $("#NombreComun").val(),
            "tipo_avistamiento": $("#TipoAvistamiento").val(),
            "estado_fauna": $("#EstadoFauna").val(),
            "lugar_referencia": $("#LugarReferencia").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_tipo_avistamiento.length > 0) {
                //Actualizar contador
                data_tipo_avistamiento = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_avistamiento.length; i++) {
                    labels.push(data_tipo_avistamiento[i].nombre);
                    data.push(data_tipo_avistamiento[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_avistamiento.data.labels = labels;
                graph_tipo_avistamiento.data.datasets[0].data = data;
                graph_tipo_avistamiento.update();
            } else {
                //Actualizar contador
                data_tipo_avistamiento = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_avistamiento.length; i++) {
                    labels.push(data_tipo_avistamiento[i].nombre);
                    data.push(data_tipo_avistamiento[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoAvsitamiento(labels, data);
            }
        }
    });
}
function CrearGraficoTipoAvsitamiento(labels, data) {
    //Colores Aleatorios
    var coloR = [];
    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };
    for (var i in data) {
        coloR.push(dynamicColors());
    }
    graph_tipo_avistamiento = new Chart(
        document.getElementById("chart_avistamientos_tipo_avistamiento"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Avistamientos",
                    "data": data,
                    "backgroundColor": coloR,
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
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}