var tipo = $("#TipoIncidente").val();
var nivel = $("#NivelIncidente").val();
var gerencia = $("#Gerencia").val();
var supervisor = $("#Supervisor").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Reportes X Fecha
var data_inv_fechas = [];
var graph_inv_fechas;
//Variables gráfico Producto Incidente
var data_prod_rec_inc = [];
var data_prod_no_rec_inc = [];
var graph_prod_inc;
//Variables gráfico Nivel Incidente
var data_nivel_inc = [];
var graph_nivel_inc;
//Variables gráfico Tipo Incidente
var data_tipo_inc = [];
var graph_tipo_inc;
//Variables gráfico Gerencia
var data_gerencia = [];
var graph_gerencia;
//Variables gráfico Causa Inmediata Incidente
var data_causa_inme_inc = [];
var graph_causa_inme_inc;
//Variables gráfico Causa Basica Incidente
var data_causa_basica_inc = [];
var graph_causa_basica_inc;
//Variables gráfico Impacto Suelo
var data_tipo_suelo = [];
var graph_tipo_suelo;
//Variables gráfico Impacto Flora
var data_tipo_flora = [];
var graph_tipo_flora;
//Variables gráfico Impacto Fauna
var data_tipo_fauna = [];
var graph_tipo_fauna;
//Variables gráfico Impacto Agua
var data_tipo_agua = [];
var graph_tipo_agua;
//Variables gráfico Impacto Persona
var data_tipo_persona = [];
var graph_tipo_persona;


//Variables gráfico Impacto Suelo
var data_residuo_suelo = [];
var graph_residuo_suelo;
//Variables gráfico Impacto Flora
var data_residuo_flora = [];
var graph_residuo_flora;
//Variables gráfico Impacto Fauna
var data_residuo_fauna = [];
var graph_residuo_fauna;
//Variables gráfico Impacto Agua
var data_residuo_agua = [];
var graph_residuo_agua;

$(document).ready(function () {
    Filtrar();
});

function Filtrar() {
    ActualizarAvistamientosFaunaFecha();
    ActualizaNombreComun();
    ActualizaTipoIncidente();
    ActualizaGerenciaIncidente();
    ActualizarProductoInvestigacion();
    ActualizaCausaInmediataIncidente();
    ActualizaCausaBasicaIncidente();
    ActualizaImpactoSuelo();
    ActualizaImpactoFlora();
    ActualizaImpactoFauna();
    ActualizaImpactoAgua();
    ActualizaImpactoPersona();

    ActualizaResiduoSuelo();
    //ActualizaResiduoFlora();
    //ActualizaResiduoFauna();
    //ActualizaResiduoAgua();
}

function ActualizarAvistamientosFaunaFecha() {
    tipo = $("#TipoIncidente").val();
    nivel = $("#NivelIncidente").val();
    gerencia = $("#Gerencia").val();
    supervisor = $("#Supervisor").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();

    if (fechaInicio <= fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesIncidentesInvestigacion/GetInvestigacionesFechas",
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
                    CrearGraficaAvistamientosFaunaFecha(labels, data, Math.max(data) / 10);
                }
                //ActualizarContadorOxFecha();
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
function CrearGraficaAvistamientosFaunaFecha(labels, data, stepSize) {
    graph_inv_fechas = new Chart(
        document.getElementById("chart_inv_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Investigaciones",
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

//Para gráfico Producto Incidente
function ActualizarProductoInvestigacion() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetCantidadProductoIncidente",
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
            console.log(response);
            
            if (data_prod_rec_inc.length > 0 || data_prod_no_rec_inc.length > 0) {
                //Actualizar contador
                data_prod_rec_inc = response.registros[0];
                data_prod_no_rec_inc = response.registros[1];

                var labels = [];
                var dataRecuperado = [];
                var dataNoRecuperado = [];
                for (var i = 0; i < data_prod_rec_inc.length; i++) {
                    labels.push(data_prod_rec_inc[i].nombre);
                    dataRecuperado.push(data_prod_rec_inc[i].valor);
                }
                for (var i = 0; i < data_prod_no_rec_inc.length; i++) {
                    dataNoRecuperado.push(data_prod_no_rec_inc[i].valor);
                }
                //Actualizar gráfica
                graph_prod_inc.data.labels = labels;
                graph_prod_inc.data.datasets[0].data = dataRecuperado;
                graph_prod_inc.data.datasets[1].data = dataNoRecuperado;
               // graph_prod_inc.options.scales.yAxes[0].ticks.stepSize = Math.max(dataRecuperado) / 10;
                graph_prod_inc.update();
            }
            else {
                //Actualizar contador
                data_prod_rec_inc = response.registros[0];
                data_prod_no_rec_inc = response.registros[1];

                var labels = [];
                var dataRecuperado = [];
                var dataNoRecuperado = [];
                for (var i = 0; i < data_prod_rec_inc.length; i++) {
                    labels.push(data_prod_rec_inc[i].nombre);
                    dataRecuperado.push(data_prod_rec_inc[i].valor);
                }
                for (var i = 0; i < data_prod_no_rec_inc.length; i++) {
                    dataNoRecuperado.push(data_prod_no_rec_inc[i].valor);
                }
                //Crear gráfica
                CrearGraficaProductoInvestigacion(labels, dataRecuperado, dataNoRecuperado);
            }
        }
    });
}
function CrearGraficaProductoInvestigacion(labels, dataRecuperado, dataNoRecuperado) {
    graph_prod_inc = new Chart(
        document.getElementById("chart_prod_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "Cantidad de Producto Derramado Recuperado",
                        "data": dataRecuperado,
                        "fill": false,
                        "stack": "stack0",
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "Cantidad de Producto Derramado No Recuperado",
                        "data": dataNoRecuperado,
                        "fill": false,
                        "stack": "stack0",
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
                            //'stepSize': stepSize,
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

//Para gráfico Gerencias
function ActualizaGerenciaIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetNInvestigacionesGerenciaIncidente",
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
            if (data_gerencia.length > 0) {
                //Actualizar contador
                data_gerencia = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_gerencia.length; i++) {
                    labels.push(data_gerencia[i].nombre);
                    data.push(data_gerencia[i].valor);
                }
                //Actualizar gráfica
                graph_gerencia.data.labels = labels;
                graph_gerencia.data.datasets[0].data = data;
                graph_gerencia.update();
            } else {
                //Actualizar contador
                data_gerencia = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_gerencia.length; i++) {
                    labels.push(data_gerencia[i].nombre);
                    data.push(data_gerencia[i].valor);
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
    graph_gerencia = new Chart(
        document.getElementById("chart_gerencias"),
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

//Para gráfico Tipo incidente
function ActualizaTipoIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetNInvestigacionesTipoIncidente",
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
            if (data_tipo_inc.length > 0) {
                //Actualizar contador
                data_tipo_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_inc.length; i++) {
                    labels.push(data_tipo_inc[i].nombre);
                    data.push(data_tipo_inc[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_inc.data.labels = labels;
                graph_tipo_inc.data.datasets[0].data = data;
                graph_tipo_inc.update();
            } else {
                //Actualizar contador
                data_tipo_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_inc.length; i++) {
                    labels.push(data_tipo_inc[i].nombre);
                    data.push(data_tipo_inc[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoIncidente(labels, data);
            }
        }
    });
}
function CrearGraficoTipoIncidente(labels, data) {
    graph_tipo_inc = new Chart(
        document.getElementById("chart_tipo_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros",
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
function ActualizaNombreComun() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetNInvestigacionesNivelIncidente",
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
            if (data_nivel_inc.length > 0) {
                //Actualizar contador
                data_nivel_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_nivel_inc.length; i++) {
                    labels.push(data_nivel_inc[i].nombre);
                    data.push(data_nivel_inc[i].valor);
                }
                //Actualizar gráfica
                graph_nivel_inc.data.labels = labels;
                graph_nivel_inc.data.datasets[0].data = data;
                graph_nivel_inc.update();
            } else {
                //Actualizar contador
                data_nivel_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_nivel_inc.length; i++) {
                    labels.push(data_nivel_inc[i].nombre);
                    data.push(data_nivel_inc[i].valor);
                }
                //Crear gráfica
                CrearGraficoNombreComun(labels, data);
            }
        }
    });
}
function CrearGraficoNombreComun(labels, data) {
    graph_nivel_inc = new Chart(
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

//Para gráfico Causa Inemdiata incidente
function ActualizaCausaInmediataIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetNInvestigacionesCausasInmediatasIncidente",
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
            if (data_causa_inme_inc.length > 0) {
                //Actualizar contador
                data_causa_inme_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_causa_inme_inc.length; i++) {
                    labels.push(data_causa_inme_inc[i].nombre);
                    data.push(data_causa_inme_inc[i].valor);
                }
                //Actualizar gráfica
                graph_causa_inme_inc.data.labels = labels;
                graph_causa_inme_inc.data.datasets[0].data = data;
                graph_causa_inme_inc.update();
            } else {
                //Actualizar contador
                data_causa_inme_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_causa_inme_inc.length; i++) {
                    labels.push(data_causa_inme_inc[i].nombre);
                    data.push(data_causa_inme_inc[i].valor);
                }
                //Crear gráfica
                CrearGraficoCausaInmediata(labels, data);
            }
        }
    });
}
function CrearGraficoCausaInmediata(labels, data) {
    graph_causa_inme_inc = new Chart(
        document.getElementById("chart_causa_inmediata_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Causas Inmediatas",
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

//Para gráfico Causa Basica incidente
function ActualizaCausaBasicaIncidente() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetNInvestigacionesCausasBasicasIncidente",
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
            if (data_causa_basica_inc.length > 0) {
                //Actualizar contador
                data_causa_basica_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_causa_basica_inc.length; i++) {
                    labels.push(data_causa_basica_inc[i].nombre);
                    data.push(data_causa_basica_inc[i].valor);
                }
                //Actualizar gráfica
                graph_causa_basica_inc.data.labels = labels;
                graph_causa_basica_inc.data.datasets[0].data = data;
                graph_causa_basica_inc.update();
            } else {
                //Actualizar contador
                data_causa_basica_inc = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_causa_basica_inc.length; i++) {
                    labels.push(data_causa_basica_inc[i].nombre);
                    data.push(data_causa_basica_inc[i].valor);
                }
                //Crear gráfica
                CrearGraficoCausaBasica(labels, data);
            }
        }
    });
}
function CrearGraficoCausaBasica(labels, data) {
    graph_causa_basica_inc = new Chart(
        document.getElementById("chart_causa_basicas_incidente"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Causas Básicas ",
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

//Para gráfico Impacto Suelo
function ActualizaImpactoSuelo() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetImpactoSuelo",
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
            if (data_tipo_suelo.length > 0) {
                //Actualizar contador
                data_tipo_suelo = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_suelo.length; i++) {
                    labels.push(data_tipo_suelo[i].nombre);
                    data.push(data_tipo_suelo[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_suelo.data.labels = labels;
                graph_tipo_suelo.data.datasets[0].data = data;
                graph_tipo_suelo.update();
            } else {
                //Actualizar contador
                data_tipo_suelo = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_suelo.length; i++) {
                    labels.push(data_tipo_suelo[i].nombre);
                    data.push(data_tipo_suelo[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoSuelo(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoSuelo(labels, data) {
    graph_tipo_suelo = new Chart(
        document.getElementById("chart_impacto_suelo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Área Afectada (m2)",
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

//Para gráfico Impacto Flora
function ActualizaImpactoFlora() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetImpactoFlora",
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
            if (data_tipo_flora.length > 0) {
                //Actualizar contador
                data_tipo_flora = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_flora.length; i++) {
                    labels.push(data_tipo_flora[i].nombre);
                    data.push(data_tipo_flora[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_flora.data.labels = labels;
                graph_tipo_flora.data.datasets[0].data = data;
                graph_tipo_flora.update();
            } else {
                //Actualizar contador
                data_tipo_flora = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_flora.length; i++) {
                    labels.push(data_tipo_flora[i].nombre);
                    data.push(data_tipo_flora[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoFlora(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoFlora(labels, data) {
    graph_tipo_flora = new Chart(
        document.getElementById("chart_impacto_flora"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Especies Afectadas (u)",
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

//Para gráfico Impacto Fauna
function ActualizaImpactoFauna() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetImpactoFauna",
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
            if (data_tipo_fauna.length > 0) {
                //Actualizar contador
                data_tipo_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_fauna.length; i++) {
                    labels.push(data_tipo_fauna[i].nombre);
                    data.push(data_tipo_fauna[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_fauna.data.labels = labels;
                graph_tipo_fauna.data.datasets[0].data = data;
                graph_tipo_fauna.update();
            } else {
                //Actualizar contador
                data_tipo_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_fauna.length; i++) {
                    labels.push(data_tipo_fauna[i].nombre);
                    data.push(data_tipo_fauna[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoFauna(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoFauna(labels, data) {
    graph_tipo_fauna = new Chart(
        document.getElementById("chart_impacto_fauna"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Especies Afectadas (u)",
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

//Para gráfico Impacto Agua
function ActualizaImpactoAgua() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetImpactoAgua",
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
            if (data_tipo_agua.length > 0) {
                //Actualizar contador
                data_tipo_agua = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_agua.length; i++) {
                    labels.push(data_tipo_agua[i].nombre);
                    data.push(data_tipo_agua[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_agua.data.labels = labels;
                graph_tipo_agua.data.datasets[0].data = data;
                graph_tipo_agua.update();
            } else {
                //Actualizar contador
                data_tipo_agua = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_agua.length; i++) {
                    labels.push(data_tipo_agua[i].nombre);
                    data.push(data_tipo_agua[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoAgua(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoAgua(labels, data) {
    graph_tipo_agua = new Chart(
        document.getElementById("chart_impacto_agua"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Volumen de Agua Afectado (Gal)",
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

//Para gráfico Impacto Persona
function ActualizaImpactoPersona() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetImpactoPersona",
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
            if (data_tipo_persona.length > 0) {
                //Actualizar contador
                data_tipo_persona = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_persona.length; i++) {
                    labels.push(data_tipo_persona[i].nombre);
                    data.push(data_tipo_persona[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_persona.data.labels = labels;
                graph_tipo_persona.data.datasets[0].data = data;
                graph_tipo_persona.update();
            } else {
                //Actualizar contador
                data_tipo_persona = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_persona.length; i++) {
                    labels.push(data_tipo_persona[i].nombre);
                    data.push(data_tipo_persona[i].valor);
                }
                //Crear gráfica
                CrearGraficoImpactoPersona(labels, data);
            }
        }
    });
}
function CrearGraficoImpactoPersona(labels, data) {
    graph_tipo_persona = new Chart(
        document.getElementById("chart_impacto_persona"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Personas Afectadas",
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

//Para gráfico Residuo Suelo
function ActualizaResiduoSuelo() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetResiduoImpactoSuelo",
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
            if (data_residuo_suelo.length > 0) {
                //Actualizar contador
                data_residuo_suelo = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_suelo.length; i++) {
                    labels.push(data_residuo_suelo[i].nombre);
                    data.push(data_residuo_suelo[i].valor);
                }
                //Actualizar gráfica
                graph_residuo_suelo.data.labels = labels;
                graph_residuo_suelo.data.datasets[0].data = data;
                graph_residuo_suelo.update();
            } else {
                //Actualizar contador
                data_residuo_suelo = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_suelo.length; i++) {
                    labels.push(data_residuo_suelo[i].nombre);
                    data.push(data_residuo_suelo[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoSuelo(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoSuelo(labels, data) {
    graph_residuo_suelo = new Chart(
        document.getElementById("chart_residuo_suelo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Cantidad de Residuo Generado (Tn)",
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
//Para gráfico Residuo Flora
function ActualizaResiduoFlora() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetResiduoImpactoFlora",
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
            if (data_residuo_flora.length > 0) {
                //Actualizar contador
                data_residuo_flora = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_flora.length; i++) {
                    labels.push(data_residuo_flora[i].nombre);
                    data.push(data_residuo_flora[i].valor);
                }
                //Actualizar gráfica
                graph_residuo_flora.data.labels = labels;
                graph_residuo_flora.data.datasets[0].data = data;
                graph_residuo_flora.update();
            } else {
                //Actualizar contador
                data_residuo_flora = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_flora.length; i++) {
                    labels.push(data_residuo_flora[i].nombre);
                    data.push(data_residuo_flora[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoFlora(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoFlora(labels, data) {
    graph_residuo_flora = new Chart(
        document.getElementById("chart_residuo_flora"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Cantidad de Residuo Generado (Tn)",
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
//Para gráfico Residuo Fauna
function ActualizaResiduoFauna() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetResiduoImpactoFauna",
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
            if (data_residuo_fauna.length > 0) {
                //Actualizar contador
                data_residuo_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_fauna.length; i++) {
                    labels.push(data_residuo_fauna[i].nombre);
                    data.push(data_residuo_fauna[i].valor);
                }
                //Actualizar gráfica
                graph_residuo_fauna.data.labels = labels;
                graph_residuo_fauna.data.datasets[0].data = data;
                graph_residuo_fauna.update();
            } else {
                //Actualizar contador
                data_residuo_fauna = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_fauna.length; i++) {
                    labels.push(data_residuo_fauna[i].nombre);
                    data.push(data_residuo_fauna[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoFauna(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoFauna(labels, data) {
    graph_residuo_fauna = new Chart(
        document.getElementById("chart_residuo_fauna"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Cantidad de Residuo Generado (Tn)",
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
//Para gráfico Residuo Agua
function ActualizaResiduoAgua() {
    $.ajax({
        type: "POST",
        url: "/ReportesIncidentesInvestigacion/GetResiduoImpactoAgua",
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
            if (data_residuo_agua.length > 0) {
                //Actualizar contador
                data_residuo_agua = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_agua.length; i++) {
                    labels.push(data_residuo_agua[i].nombre);
                    data.push(data_residuo_agua[i].valor);
                }
                //Actualizar gráfica
                graph_residuo_agua.data.labels = labels;
                graph_residuo_agua.data.datasets[0].data = data;
                graph_residuo_agua.update();
            } else {
                //Actualizar contador
                data_residuo_agua = response.registros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_residuo_agua.length; i++) {
                    labels.push(data_residuo_agua[i].nombre);
                    data.push(data_residuo_agua[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoAgua(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoAgua(labels, data) {
    graph_residuo_agua = new Chart(
        document.getElementById("chart_residuo_agua"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "Cantidad de Residuo Generado (Tn)",
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