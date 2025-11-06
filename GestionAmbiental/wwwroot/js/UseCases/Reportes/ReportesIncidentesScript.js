var tipo = $("#TipoIncidente").val();
var nivel = $("#NivelIncidente").val();
var gerencia = $("#Gerencia").val();
var supervisor = $("#Supervisor").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Reportes X Fecha
var data_inc_flashreports_fechas = [];
var data_inc_incidentesmenores_fechas = [];
var data_inc_incidentesmayores_fechas = [];
var graph_inc_fechas;
var incidentesTotales = 0;

//Variable gráfico Tipo Incidente
//var data_rep_tipo_incidente = [];
//var data_inc_flashreports_tipo_incidente = [];
var data_inc_incidentesmenores_tipo_incidente = [];
var data_inc_incidentesmayores_tipo_incidente = [];
var graph_inc_tipo_incidente;

//Variable gráfico Nivel Incidente
//var data_rep_nivel_incidente = [];
var data_inc_flashreports_nivel_incidente = [];
var data_inc_incidentesmenores_nivel_incidente = [];
var data_inc_incidentesmayores_nivel_incidente = [];
var graph_inc_nivel_incidente;

//Variable gráfico Gerencia
//var data_rep_gerencia_incidente = [];
var data_inc_flashreports_gerencia_incidente = [];
var data_inc_incidentesmenores_gerencia_incidente = [];
var data_inc_incidentesmayores_gerencia_incidente = [];
var graph_rep_gerencia_incidente;

//Variable gráfico Causa inmediatas Incidente
//var data_rep_causa_incidente = [];
//var data_inc_flashreports_causa_incidente = [];
var data_inc_incidentesmenores_causa_incidente = [];
var data_inc_incidentesmayores_causa_incidente = [];
var graph_inc_causa_incidente;

//Variable gráfico Causas Basicas Incidente
var data_inc_incidentesmenores_causa_basica_incidente = [];
var data_inc_incidentesmayores_causa_basicas_incidente = [];
var graph_inc_causa_basicas_incidente;

$(document).ready(function () {
    ActualizarIncidentesFechas();
    ActualizaTipoIncidente();
    ActualizarDatosGraficoNivelIncidente();
    ActualizarDatosGraficoGerenciaIncidente();
    ActualizarDatosGraficoCausaInmediataIncidente();
    ActualizarDatosGraficoCausaBasicaIncidente();
});

function Filtrar() {
    ActualizarIncidentesFechas();
    ActualizaTipoIncidente();
    ActualizarDatosGraficoNivelIncidente();
    ActualizarDatosGraficoGerenciaIncidente();
    ActualizarDatosGraficoCausaInmediataIncidente();
    ActualizarDatosGraficoCausaBasicaIncidente();
}

function ActualizarIncidentesFechas() {
    tipo = $("#TipoIncidente").val();
    nivel = $("#NivelIncidente").val();
    gerencia = $("#Gerencia").val();
    supervisor = $("#Supervisor").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();
    incidentesTotales = 0;

    if (fechaInicio <= fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesIncidentes/GetRerpotesFechas",
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
                console.log(response.registros)
                if (data_inc_flashreports_fechas.length > 0 || data_inc_incidentesmenores_fechas.length > 0 || data_inc_incidentesmayores_fechas.length > 0) {
                    //Actualizar contador
                    data_inc_flashreports_fechas = response.registros.flashReports;
                    data_inc_incidentesmenores_fechas = response.registros.incidentesMenores;
                    data_inc_incidentesmayores_fechas = response.registros.incidentesMayores;

                    var labels = [];
                    var dataFlashReports = [];
                    var dataIncidenteMenor = [];
                    var dataIncidenteMayor = [];
                    for (var i = 0; i < data_inc_flashreports_fechas.length; i++) {
                        labels.push(data_inc_flashreports_fechas[i].nombre);
                        dataFlashReports.push(data_inc_flashreports_fechas[i].valor);
                    }
                    for (var i = 0; i < data_inc_incidentesmenores_fechas.length; i++) {
                        if (!labels.includes(data_inc_incidentesmenores_fechas[i].nombre)) {
                            labels.push(data_inc_incidentesmenores_fechas[i].nombre);
                        }
                        dataIncidenteMenor.push(data_inc_incidentesmenores_fechas[i].valor);
                        incidentesTotales = incidentesTotales + data_inc_incidentesmenores_fechas[i].valor;
                    }
                    for (var i = 0; i < data_inc_incidentesmayores_fechas.length; i++) {
                        if (!labels.includes(data_inc_incidentesmayores_fechas[i].nombre)) {
                            labels.push(data_inc_incidentesmayores_fechas[i].nombre);
                        }
                        dataIncidenteMayor.push(data_inc_incidentesmayores_fechas[i].valor);
                        incidentesTotales = incidentesTotales + data_inc_incidentesmayores_fechas[i].valor;
                    }
                    //Actualizar gráfica
                    graph_inc_fechas.data.labels = labels;
                    //graph_inc_fechas.data.datasets[0].data = dataFlashReports;
                    graph_inc_fechas.data.datasets[0].data = dataIncidenteMenor;
                    graph_inc_fechas.data.datasets[1].data = dataIncidenteMayor;
                    graph_inc_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(dataFlashReports) / 10;
                    graph_inc_fechas.update();
                } else {
                    //Actualizar contador
                    data_inc_flashreports_fechas = response.registros.flashReports;
                    data_inc_incidentesmenores_fechas = response.registros.incidentesMenores;
                    data_inc_incidentesmayores_fechas = response.registros.incidentesMayores;

                    var labels = [];
                    var dataFlashReports = [];
                    var dataIncidenteMenor = [];
                    var dataIncidenteMayor = [];
                    for (var i = 0; i < data_inc_flashreports_fechas.length; i++) {
                        labels.push(data_inc_flashreports_fechas[i].nombre);
                        dataFlashReports.push(data_inc_flashreports_fechas[i].valor);
                    }
                    for (var i = 0; i < data_inc_incidentesmenores_fechas.length; i++) {
                        if (!labels.includes(data_inc_incidentesmenores_fechas[i].nombre)) {
                            labels.push(data_inc_incidentesmenores_fechas[i].nombre);
                        }
                        dataIncidenteMenor.push(data_inc_incidentesmenores_fechas[i].valor);
                        incidentesTotales = incidentesTotales + data_inc_incidentesmenores_fechas[i].valor;
                    }
                    for (var i = 0; i < data_inc_incidentesmayores_fechas.length; i++) {
                        if (!labels.includes(data_inc_incidentesmayores_fechas[i].nombre)) {
                            labels.push(data_inc_incidentesmayores_fechas[i].nombre);
                        }
                        dataIncidenteMayor.push(data_inc_incidentesmayores_fechas[i].valor);
                        incidentesTotales = incidentesTotales + data_inc_incidentesmayores_fechas[i].valor;
                    }
                    console.log(labels)
                    console.log(dataFlashReports)
                    //Crear gráfica
                    CrearGraficaIncidentesFechas(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor, Math.max(...dataFlashReports) / 10);                    
                }
                $("#IncTotal").text(incidentesTotales);
                //ActualizarContadorOxFecha();
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
function CrearGraficaIncidentesFechas(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor, stepSize) {
    graph_inc_fechas = new Chart(
        document.getElementById("chart_inc_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [
                    //{
                    //"label": "# Flash Reports",
                    //"data": dataFlashReports,
                    //"fill": false,
                    //"backgroundColor": "rgb(46, 103, 147)",
                    //"borderColor": "rgb(46, 103, 147)",
                    //"lineTension": 0.1
                    //},
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
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

//Para gráfico Tipo Incidente
function ActualizaTipoIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentes/GetNReportesTipoIncidente",
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
            if (data_inc_incidentesmenores_tipo_incidente.length > 0 || data_inc_incidentesmayores_tipo_incidente.length > 0) {
                //Actualizar contador
                data_inc_incidentesmenores_tipo_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_tipo_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_tipo_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_tipo_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_tipo_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_tipo_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_tipo_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_tipo_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_tipo_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_inc_tipo_incidente.data.labels = labels;
                graph_inc_tipo_incidente.data.datasets[0].data = dataIncidenteMenor;
                graph_inc_tipo_incidente.data.datasets[1].data = dataIncidenteMayor;
                graph_inc_tipo_incidente.update();
            } else {
                //Actualizar contador
                data_inc_incidentesmenores_tipo_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_tipo_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_tipo_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_tipo_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_tipo_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_tipo_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_tipo_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_tipo_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_tipo_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoIncidente(labels, dataIncidenteMenor, dataIncidenteMayor);
            }
        }
    });
}
function CrearGraficoTipoIncidente(labels, dataIncidenteMenor, dataIncidenteMayor) {
    graph_inc_tipo_incidente = new Chart(
        document.getElementById("chart_tipo_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
                        "lineTension": 0.1
                    }
                ]
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
        url: "/ReportesIncidentes/GetNReportesNivelIncidente",
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
            if (data_inc_flashreports_nivel_incidente.length > 0 || data_inc_incidentesmenores_nivel_incidente.length > 0 || data_inc_incidentesmayores_nivel_incidente.length > 0) {
                //Actualizar contador
                data_inc_flashreports_nivel_incidente = response.registros.flashReports;
                data_inc_incidentesmenores_nivel_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_nivel_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataFlashReports = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_flashreports_nivel_incidente.length; i++) {
                    labels.push(data_inc_flashreports_nivel_incidente[i].nombre);
                    dataFlashReports.push(data_inc_flashreports_nivel_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmenores_nivel_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmenores_nivel_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmenores_nivel_incidente[i].nombre);
                    }
                    dataIncidenteMenor.push(data_inc_incidentesmenores_nivel_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_nivel_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_nivel_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_nivel_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_nivel_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_inc_nivel_incidente.data.labels = labels;
                //graph_inc_nivel_incidente.data.datasets[0].data = dataFlashReports;
                graph_inc_nivel_incidente.data.datasets[0].data = dataIncidenteMenor;
                graph_inc_nivel_incidente.data.datasets[1].data = dataIncidenteMayor;
                graph_inc_nivel_incidente.update();
            } else {
                //Actualizar contador
                data_inc_flashreports_nivel_incidente = response.registros.flashReports;
                data_inc_incidentesmenores_nivel_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_nivel_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataFlashReports = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_flashreports_nivel_incidente.length; i++) {
                    labels.push(data_inc_flashreports_nivel_incidente[i].nombre);
                    dataFlashReports.push(data_inc_flashreports_nivel_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmenores_nivel_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmenores_nivel_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmenores_nivel_incidente[i].nombre);
                    }
                    dataIncidenteMenor.push(data_inc_incidentesmenores_nivel_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_nivel_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_nivel_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_nivel_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_nivel_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoNivelIncidente(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor);
            }
        }
    });
}
function CrearGraficoNivelIncidente(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor) {
    graph_inc_nivel_incidente = new Chart(
        document.getElementById("chart_nivel_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    //{
                    //    "label": "# Flash Reports",
                    //    "data": dataFlashReports,
                    //    "fill": false,
                    //    "backgroundColor": "rgb(46, 103, 147)",
                    //    "borderColor": "rgb(46, 103, 147)",
                    //    "lineTension": 0.1
                    //},
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
                        "lineTension": 0.1
                    }
                ]
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
                        //formatter: (value, ctx) => {
                        //    let sum = 0;
                        //    let dataArr = ctx.chart.data.datasets[0].data;
                        //    dataArr.map(data => {
                        //        sum += data;
                        //    });
                        //    let percentage = (value * 100 / sum).toFixed(2) + "%";
                        //    return percentage;
                        //},
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
        url: "/ReportesIncidentes/GetNReportesGerenciaIncidente",
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
            if (data_inc_flashreports_gerencia_incidente.length > 0 || data_inc_incidentesmenores_gerencia_incidente.length > 0 || data_inc_incidentesmayores_gerencia_incidente.length > 0) {
                //Actualizar contador
                data_inc_flashreports_gerencia_incidente = response.registros.flashReports;
                data_inc_incidentesmenores_gerencia_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_gerencia_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataFlashReports = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_flashreports_gerencia_incidente.length; i++) {
                    labels.push(data_inc_flashreports_gerencia_incidente[i].nombre);
                    dataFlashReports.push(data_inc_flashreports_gerencia_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmenores_gerencia_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmenores_gerencia_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmenores_gerencia_incidente[i].nombre);
                    }
                    dataIncidenteMenor.push(data_inc_incidentesmenores_gerencia_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_gerencia_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_gerencia_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_gerencia_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_gerencia_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_inc_gerencia_incidente.data.labels = labels;
                //graph_inc_gerencia_incidente.data.datasets[0].data = dataFlashReports;
                graph_inc_gerencia_incidente.data.datasets[0].data = dataIncidenteMenor;
                graph_inc_gerencia_incidente.data.datasets[1].data = dataIncidenteMayor;
                graph_inc_gerencia_incidente.update();
            } else {
                //Actualizar contador
                data_inc_flashreports_gerencia_incidente = response.registros.flashReports;
                data_inc_incidentesmenores_gerencia_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_gerencia_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataFlashReports = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_flashreports_gerencia_incidente.length; i++) {
                    labels.push(data_inc_flashreports_gerencia_incidente[i].nombre);
                    dataFlashReports.push(data_inc_flashreports_gerencia_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmenores_gerencia_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmenores_gerencia_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmenores_gerencia_incidente[i].nombre);
                    }                    
                    dataIncidenteMenor.push(data_inc_incidentesmenores_gerencia_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_gerencia_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_gerencia_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_gerencia_incidente[i].nombre);
                    }  
                    dataIncidenteMayor.push(data_inc_incidentesmayores_gerencia_incidente[i].valor);
                }
                console.log("labels", labels)
                //Crear gráfica
                CrearGraficoGerenciaIncidente(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor);
            }
        }
    });
}
function CrearGraficoGerenciaIncidente(labels, dataFlashReports, dataIncidenteMenor, dataIncidenteMayor) {
    //Colores Aleatorios
    var coloR = [];
    var dynamicColors = function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    };
    for (var i in dataFlashReports) {
        coloR.push(dynamicColors());
    }
    graph_inc_gerencia_incidente = new Chart(
        document.getElementById("chart_gerencia_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    //{
                    //    "label": "# Flash Reports",
                    //    "data": dataFlashReports,
                    //    "fill": false,
                    //    "backgroundColor": "rgb(46, 103, 147)",
                    //    "borderColor": "rgb(46, 103, 147)",
                    //    "lineTension": 0.1
                    //},
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
                        "lineTension": 0.1
                    }
                ]
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

//Para gráfico Causa Inmediata Incidente
function ActualizarDatosGraficoCausaInmediataIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentes/GetNReportesCausasInmediatasIncidente",
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
            if (data_inc_incidentesmenores_causa_incidente.length > 0 || data_inc_incidentesmayores_causa_incidente > 0) {
                //Actualizar contador
                data_inc_incidentesmenores_causa_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_causa_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_causa_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_causa_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_causa_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_causa_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_causa_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_causa_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_causa_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_inc_causa_incidente.data.labels = labels;
                graph_inc_causa_incidente.data.datasets[0].data = dataIncidenteMenor;
                graph_inc_causa_incidente.data.datasets[1].data = dataIncidenteMayor;
                graph_inc_causa_incidente.update();
            } else {
                //Actualizar contador
                data_inc_incidentesmenores_causa_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_causa_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_causa_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_causa_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_causa_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_causa_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_causa_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_causa_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_causa_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoCausaInmediataIncidente(labels, dataIncidenteMenor, dataIncidenteMayor);
            }
        }
    });
}
function CrearGraficoCausaInmediataIncidente(labels, dataIncidenteMenor, dataIncidenteMayor) {
    graph_inc_causa_incidente = new Chart(
        document.getElementById("chart_causa_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
                        "lineTension": 0.1
                    }
                ]
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

//Para gráfico Causa Basica Incidente
function ActualizarDatosGraficoCausaBasicaIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentes/GetNReportesCausasBasicasIncidente",
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
            if (data_inc_incidentesmenores_causa_basica_incidente.length > 0 || data_inc_incidentesmayores_causa_basicas_incidente > 0) {
                //Actualizar contador
                data_inc_incidentesmenores_causa_basica_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_causa_basicas_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_causa_basica_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_causa_basica_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_causa_basica_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_causa_basicas_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_causa_basicas_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_causa_basicas_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_causa_basicas_incidente[i].valor);
                }
                //Actualizar gráfica
                graph_inc_causa_basicas_incidente.data.labels = labels;
                graph_inc_causa_basicas_incidente.data.datasets[0].data = dataIncidenteMenor;
                graph_inc_causa_basicas_incidente.data.datasets[1].data = dataIncidenteMayor;
                graph_inc_causa_basicas_incidente.update();
            } else {
                //Actualizar contador
                data_inc_incidentesmenores_causa_basica_incidente = response.registros.incidentesMenores;
                data_inc_incidentesmayores_causa_basicas_incidente = response.registros.incidentesMayores;

                var labels = [];
                var dataIncidenteMenor = [];
                var dataIncidenteMayor = [];
                for (var i = 0; i < data_inc_incidentesmenores_causa_basica_incidente.length; i++) {
                    labels.push(data_inc_incidentesmenores_causa_basica_incidente[i].nombre);
                    dataIncidenteMenor.push(data_inc_incidentesmenores_causa_basica_incidente[i].valor);
                }
                for (var i = 0; i < data_inc_incidentesmayores_causa_basicas_incidente.length; i++) {
                    if (!labels.includes(data_inc_incidentesmayores_causa_basicas_incidente[i].nombre)) {
                        labels.push(data_inc_incidentesmayores_causa_basicas_incidente[i].nombre);
                    }
                    dataIncidenteMayor.push(data_inc_incidentesmayores_causa_basicas_incidente[i].valor);
                }
                //Crear gráfica
                CrearGraficoCausaBasicaIncidente(labels, dataIncidenteMenor, dataIncidenteMayor);
            }
        }
    });
}
function CrearGraficoCausaBasicaIncidente(labels, dataIncidenteMenor, dataIncidenteMayor) {
    graph_inc_causa_basicas_incidente = new Chart(
        document.getElementById("chart_causa_basicas_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Incidentes Menores",
                        "data": dataIncidenteMenor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 194, 0)",
                        "borderColor": "rgb(255, 194, 0)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Incidentes Mayores",
                        "data": dataIncidenteMayor,
                        "fill": false,
                        "backgroundColor": "rgb(255, 51, 0)",
                        "borderColor": "rgb(255, 51, 0)",
                        "lineTension": 0.1
                    }
                ]
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