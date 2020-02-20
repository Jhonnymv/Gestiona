var gerencia = $("#IdGerencia").val();
var empresa = $("#IdEmpresa").val();
var asesor = $("#IdAsesor").val();
var respLev = $("#IdRespLev").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Inspecciones X Fecha
var data_insp_fechas = [];
var graph_insp_fechas;
//Variable gráfico Observaciones X Fecha
var data_obs_fechas = [];
var graph_obs_fechas;
//Variable gráfico MAP vs SSO
var data_map_vs_sso = [];
var graph_map_vs_sso;
//Variable gráfico MAP vs SSO x Estado
var data_map_vs_sso_estado = [];
var graph_map_vs_sso_estado;


$(document).ready(function () {
    Filtrar();
});

function Filtrar() {
    gerencia = $("#IdGerencia").val();
    empresa = $("#IdEmpresa").val();
    asesor = $("#IdAsesor").val();
    respLev = $("#IdRespLev").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();
    if (fechaInicio < fechaFin || (fechaInicio == "" && fechaFin == "")) {
        $.ajax({
            type: "POST",
            url: "/ReportesInspecciones/ActualizarContadorInspecciones",
            data: {
                "gerencia": $("#IdGerencia").val(),
                "empresa": $("#IdEmpresa").val(),
                "asesor": $("#IdAsesor").val(),
                "respLevObs": $("#IdRespLev").val(),
                "FechaInicio": $("#FInicio").val(),
                "FechaFin": $("#FFin").val()
            },
            success: (response) => {
                var nObs = response.nInspecciones;
                ActualizarContadorInsXEst(nObs[0].valor, nObs[1].valor, nObs[2].valor, nObs[3].valor);

                ActualizarContadorRxFecha();
                ActualizarContadorOxFecha();
                ActualizarDatosGraficoMAP_SSO();
                GetDataContadorOxE();
                ActualizarDatosGraficoMAP_SSO_Estado();
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
//Para contador de inspecciones x estado
function ActualizarContadorInsXEst(pend, reg, elim, tot) {
    $("#InsPendiente").html(pend);
    $("#InsRegistrado").html(reg);
    $("#InsEliminado").html(elim);
    $("#InsTotal").html(tot);
    $("#InsEncont").html(pend + reg + elim);
}
//Para contador de inspecciones x fecha
$("#sel_frec_insp_fecha").on('change', function () {
    ActualizarContadorRxFecha();
    ActualizarDatosGraficoMAP_SSO();
    ActualizarDatosGraficoMAP_SSO_Estado();
});
function ActualizarContadorRxFecha() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarInspeccionesXFecha",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "frecuencia": $("#sel_frec_insp_fecha").val()
        },
        traditional: true,
        success: (response) => {
            if (data_insp_fechas.length > 0) {
                //Actualizar contador
                data_insp_fechas = response.inspeccionesXFecha;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_insp_fechas.length; i++) {
                    labels.push(data_insp_fechas[i].nombre);
                    data.push(data_insp_fechas[i].valor);
                }
                //Actualizar gráfica
                graph_insp_fechas.data.labels = labels;
                graph_insp_fechas.data.datasets[0].data = data;
                graph_insp_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(data) / 10;
                graph_insp_fechas.update();
            } else {
                //Actualizar contador
                data_insp_fechas = response.inspeccionesXFecha;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_insp_fechas.length; i++) {
                    labels.push(data_insp_fechas[i].nombre);
                    data.push(data_insp_fechas[i].valor);
                }
                //Crear gráfica
                CrearGraficaContardorIxFecha(labels, data, Math.max(data) / 10);
            }
            ActualizarContadorOxFecha();
        }
    });
}
function CrearGraficaContardorIxFecha(labels, data, stepSize) {
    graph_insp_fechas = new Chart(
        document.getElementById("chart_insp_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Inspecciones Registradas",
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
//Para contador de observaciones x fecha
$("#sel_frec_obs_estado").on('change', function () {
    ActualizarContadorOxFecha();
    GetDataContadorOxE();
    ActualizarDatosGraficoMAP_SSO();
    ActualizarDatosGraficoMAP_SSO_Estado();
});
function ActualizarContadorOxFecha() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObservacionesXFecha",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "frecuencia": $("#sel_frec_insp_fecha").val(),
            "estado": $("#sel_frec_obs_estado").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_fechas.length > 0) {
                //Actualizar contador
                data_obs_fechas = response.observacionesXFecha;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_fechas.length; i++) {
                    labels.push(data_obs_fechas[i].nombre);
                    data.push(data_obs_fechas[i].valor);
                }
                //Actualizar gráfica
                graph_obs_fechas.data.labels = labels;
                graph_obs_fechas.data.datasets[0].data = data;
                graph_obs_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(data) / 10;
                graph_obs_fechas.update();
            }
            else {
                //Actualizar contador
                data_obs_fechas = response.observacionesXFecha;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_fechas.length; i++) {
                    labels.push(data_obs_fechas[i].nombre);
                    data.push(data_obs_fechas[i].valor);
                }
                //Crear gráfica
                CrearGraficaContardorOxFecha(labels, data, Math.max(data) / 10);
            }
            ActualizarDatosGraficoMAP_SSO()
        }
    });
}
function CrearGraficaContardorOxFecha(labels, data, stepSize) {
    graph_obs_fechas = new Chart(
        document.getElementById("chart_obs_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Observaciones Registradas",
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
//Para Contador de Observaciones x Estado
function GetDataContadorOxE() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarContadorObservaciones",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#sel_frec_obs_estado").val()
        },
        success: (response) => {
            var nObs = response.nObservaciones;
            ActualizarContadorOxE(nObs[0].valor, nObs[1].valor, nObs[2].valor, nObs[3].valor, nObs[4].valor, nObs[5].valor, nObs[6].valor, nObs[7].valor);
        }
    });
}
function ActualizarContadorOxE(reg, imp, apr, rec, ven, amp, elim, total) {
    //Reiniciar valores
    $("#ObsRegistrado").html(0);
    $("#ObsImplementado").html(0);
    $("#ObsAprobado").html(0);
    $("#ObsRechazado").html(0);
    $("#ObsVencido").html(0);
    $("#ObsAmpliado").html(0);
    $("#ObsTotal").html(0);
    $("#ObsEncont").html(0);
    //Nuevo valores
    $("#ObsRegistrado").html(reg);
    $("#ObsImplementado").html(imp);
    $("#ObsAprobado").html(apr);
    $("#ObsRechazado").html(rec);
    $("#ObsVencido").html(ven);
    $("#ObsAmpliado").html(amp);
    $("#ObsEliminado").html(elim);
    $("#ObsTotal").html(total);
    $("#ObsEncont").html(reg + imp + apr + rec + ven + amp + elim);
}
//Para gráfico MAP vs SSO
function ActualizarDatosGraficoMAP_SSO() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsMAPvsSSO",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#sel_frec_obs_estado").val()
        },
        traditional: true,
        success: (response) => {
            if (data_map_vs_sso.length > 0) {
                //Actualizar contador
                data_map_vs_sso = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_map_vs_sso.length; i++) {
                    labels.push(data_map_vs_sso[i].nombre);
                    data.push(data_map_vs_sso[i].valor);
                }
                //Actualizar gráfica
                graph_map_vs_sso.data.labels = labels;
                graph_map_vs_sso.data.datasets[0].data = data;
                graph_map_vs_sso.update();
            } else {
                //Actualizar contador
                data_map_vs_sso = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_map_vs_sso.length; i++) {
                    labels.push(data_map_vs_sso[i].nombre);
                    data.push(data_map_vs_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoMAP_SSO(labels, data);
            }
        }
    });
}
function CrearGraficoMAP_SSO(labels, data) {
    graph_map_vs_sso = new Chart(
        document.getElementById("chart_MAP_vs_SSO"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes registrados",
                    "data": data,
                    "backgroundColor": [
                        '#3DFE8E',
                        '#FEC63D',
                        '#FE3D3D'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
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
//Para gráfico MAP vs SSO x Estado
function ActualizarDatosGraficoMAP_SSO_Estado() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsMAPvsSSO_x_Estado",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#sel_frec_obs_estado").val()
        },
        traditional: true,
        success: (response) => {
            if (data_map_vs_sso_estado.length > 0) {
                //Actualizar contador
                data_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_map_vs_sso_estado.length; i++) {
                    labels.push(data_map_vs_sso_estado[i].nombre);
                    data.push(data_map_vs_sso_estado[i].valor);
                }
                //Actualizar gráfica
                data_map_vs_sso_estado.data.labels = labels;
                data_map_vs_sso_estado.data.datasets[0].data = data;
                data_map_vs_sso_estado.update();
            } else {
                //Actualizar contador
                data_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_map_vs_sso_estado.length; i++) {
                    labels.push(data_map_vs_sso_estado[i].nombre);
                    data.push(data_map_vs_sso_estado[i].valor);
                }
                //Crear gráfica
                CrearGraficoMAP_SSO_Estado(labels, data);
            }
        }
    });
}
function CrearGraficoMAP_SSO_Estado(labels, data) {
    data_map_vs_sso_estado = new Chart(
        document.getElementById("chart_MA_vs_SSO_Estado"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Reportes registrados",
                    "data": data,
                    "backgroundColor": [
                        '#3DFE8E',
                        '#FEC63D',
                        '#FE3D3D'
                    ],
                    "lineTension": 0.1
                }]
            },
            "options": {
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
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