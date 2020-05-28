var tipo = $("#TipoIncidente").val();
var nivel = $("#NivelIncidente").val();
var gerencia = $("#Gerencia").val();
var supervisor = $("#Supervisor").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Reportes X Fecha
var data_inv_fechas = [];
var graph_inv_fechas;

//Variable gráfico Tipo Incidente
var data_rep_tipo_incidente = [];
var graph_rep_tipo_incidente;

//Variable gráfico Nivel Incidente
var data_rep_nivel_incidente = [];
var graph_rep_nivel_incidente;

//Variable gráfico Gerencia
var data_rep_gerencia_incidente = [];
var graph_rep_gerencia_incidente;

//Variable gráfico Causa Incidente
var data_rep_causa_incidente = [];
var graph_rep_causa_incidente;

$(document).ready(function () {
    ActualizarInvestigacionesFecha();
    ActualizaImpactoAgua();
    ActualizarDatosGraficoNivelIncidente();
    ActualizarDatosGraficoGerenciaIncidente();
    ActualizarDatosGraficoCausaIncidente();
});

function Filtrar() {
    ActualizarInvestigacionesFecha();
    ActualizaImpactoAgua();
    ActualizarDatosGraficoNivelIncidente();
    ActualizarDatosGraficoGerenciaIncidente();
    ActualizarDatosGraficoCausaIncidente();
}

function ActualizarInvestigacionesFecha() {
    tipo = $("#TipoIncidente").val();
    nivel = $("#NivelIncidente").val();
    gerencia = $("#Gerencia").val();
    supervisor = $("#Supervisor").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();

    if (fechaInicio <= fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesIncidentesReportePreliminar/GetRerpotesPreliminaresFechas",
            data: {
                "tipo_incidente": $("#TipoIncidente").val(),
                "nivel_incidente": $("#NivelIncidente").val(),
                "gerencia": $("#Gerencia").val(),
                "supervisor": $("#Supervisor").val(),
                "FechaInicio": $("#FInicio").val(),
                "FechaFin": $("#FFin").val(),
                "frecuencia": $("#frecuencia").val()
            },
            success: (response) => {
                if (data_inv_fechas.length > 0) {
                    //Actualizar contador
                    data_inv_fechas = response.registros;

                    var labels = [];
                    var data = [];
                    for (var i = 0; i < data_inv_fechas.length; i++) {
                        labels.push(data_inv_fechas[i].nombre);
                        data.push(data_inv_fechas[i].valor);
                    }
                    //Actualizar gráfica
                    graph_inv_fechas.data.labels = labels;
                    graph_inv_fechas.data.datasets[0].data = data;
                    graph_inv_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(data) / 10;
                    graph_inv_fechas.update();
                } else {
                    //Actualizar contador
                    data_inv_fechas = response.registros;

                    var labels = [];
                    var data = [];
                    for (var i = 0; i < data_inv_fechas.length; i++) {
                        labels.push(data_inv_fechas[i].nombre);
                        data.push(data_inv_fechas[i].valor);
                    }
                    //Crear gráfica
                    CrearGraficaInvestigacionesFecha(labels, data, Math.max(data) / 10);
                }
                //ActualizarContadorOxFecha();
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
function CrearGraficaInvestigacionesFecha(labels, data, stepSize) {
    graph_inv_fechas = new Chart(
        document.getElementById("chart_rep_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes Preliminares",
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

//Para gráfico Tipo Incidente
function ActualizaImpactoAgua() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesReportePreliminar/GetRerpotesPreliminaresTipoIncidente",
        data: {
            "tipo_incidente": $("#TipoIncidente").val(),
            "nivel_incidente": $("#NivelIncidente").val(),
            "gerencia": $("#Gerencia").val(),
            "supervisor": $("#Supervisor").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rep_tipo_incidente.length > 0) {
                //Actualizar contador
                data_rep_tipo_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_tipo_incidente.length; i++) {
                    labels.push(data_rep_tipo_incidente[i].nombre);
                    data.push(data_rep_tipo_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_rep_tipo_incidente.data.labels = labels;
                graph_rep_tipo_incidente.data.datasets[0].data = data;
                graph_rep_tipo_incidente.update();
            } else {
                //Actualizar contador
                data_rep_tipo_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_tipo_incidente.length; i++) {
                    labels.push(data_rep_tipo_incidente[i].nombre);
                    data.push(data_rep_tipo_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoAgua(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoAgua(labels, data) {
    graph_rep_tipo_incidente = new Chart(
        document.getElementById("chart_rp_tipo_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes Preliminares",
                    "data": data,
                    "backgroundColor": '#2e6793',
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

//Para gráfico Nivel Incidente
function ActualizarDatosGraficoNivelIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesReportePreliminar/GetRerpotesPreliminaresNivelIncidente",
        data: {
            "tipo_incidente": $("#TipoIncidente").val(),
            "nivel_incidente": $("#NivelIncidente").val(),
            "gerencia": $("#Gerencia").val(),
            "supervisor": $("#Supervisor").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rep_nivel_incidente.length > 0) {
                //Actualizar contador
                data_rep_nivel_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_nivel_incidente.length; i++) {
                    labels.push(data_rep_nivel_incidente[i].nombre);
                    data.push(data_rep_nivel_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_rep_nivel_incidente.data.labels = labels;
                graph_rep_nivel_incidente.data.datasets[0].data = data;
                graph_rep_nivel_incidente.update();
            } else {
                //Actualizar contador
                data_rep_nivel_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_nivel_incidente.length; i++) {
                    labels.push(data_rep_nivel_incidente[i].nombre);
                    data.push(data_rep_nivel_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoNivelIncidente(labels, data);
            }
        }
    });
}
function CrearGraficoNivelIncidente(labels, data) {
    graph_rep_nivel_incidente = new Chart(
        document.getElementById("chart_nivel_incidente"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes Preliminares",
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
                        formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },
                        defaultFontSize: 15
                    }
                }
            }
        }
    );
}

//Para gráfico Gerencia Incidente
function ActualizarDatosGraficoGerenciaIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesReportePreliminar/GetRerpotesPreliminaresGerencia",
        data: {
            "tipo_incidente": $("#TipoIncidente").val(),
            "nivel_incidente": $("#NivelIncidente").val(),
            "gerencia": $("#Gerencia").val(),
            "supervisor": $("#Supervisor").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rep_gerencia_incidente.length > 0) {
                //Actualizar contador
                data_rep_gerencia_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_gerencia_incidente.length; i++) {
                    labels.push(data_rep_gerencia_incidente[i].nombre);
                    data.push(data_rep_gerencia_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_rep_gerencia_incidente.data.labels = labels;
                graph_rep_gerencia_incidente.data.datasets[0].data = data;
                graph_rep_gerencia_incidente.update();
            } else {
                //Actualizar contador
                data_rep_gerencia_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_gerencia_incidente.length; i++) {
                    labels.push(data_rep_gerencia_incidente[i].nombre);
                    data.push(data_rep_gerencia_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoGerenciaIncidente(labels, data);
            }
        }
    });
}
function CrearGraficoGerenciaIncidente(labels, data) {
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
    graph_rep_gerencia_incidente = new Chart(
        document.getElementById("chart_gerencia_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros",
                    "data": data,
                    "backgroundColor": coloR,
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

//Para gráfico Tipo Incidente
function ActualizarDatosGraficoCausaIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesReportePreliminar/GetRerpotesPreliminaresCausaIncidente",
        data: {
            "tipo_incidente": $("#TipoIncidente").val(),
            "nivel_incidente": $("#NivelIncidente").val(),
            "gerencia": $("#Gerencia").val(),
            "supervisor": $("#Supervisor").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rep_causa_incidente.length > 0) {
                //Actualizar contador
                data_rep_causa_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_causa_incidente.length; i++) {
                    labels.push(data_rep_causa_incidente[i].nombre);
                    data.push(data_rep_causa_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_rep_causa_incidente.data.labels = labels;
                graph_rep_causa_incidente.data.datasets[0].data = data;
                graph_rep_causa_incidente.update();
            } else {
                //Actualizar contador
                data_rep_causa_incidente = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rep_causa_incidente.length; i++) {
                    labels.push(data_rep_causa_incidente[i].nombre);
                    data.push(data_rep_causa_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoCausaIncidente(labels, data);
            }
        }
    });
}
function CrearGraficoCausaIncidente(labels, data) {
    graph_rep_causa_incidente = new Chart(
        document.getElementById("chart_rp_causa_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes Preliminares",
                    "data": data,
                    "backgroundColor": '#2e6793',
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