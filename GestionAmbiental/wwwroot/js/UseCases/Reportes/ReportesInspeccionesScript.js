var gerencia = $("#IdGerencia").val();
var empresa = $("#IdEmpresa").val();
var asesor = $("#IdAsesor").val();
var respLev = $("#IdRespLev").val();
var usuarioInvolucrado = $("#UsuarioInvolucrado").val();
var tipoObservacion = $("#TipoObservacion").val();
var fechaInicio = "";
var fechaFin = "";

//Variables gráfico Inspecciones X Fecha
var data_insp_fechas = [];
var graph_insp_fechas;
//Variable gráfico Observaciones X Fecha
var data_obs_fechas = [];
var data_bp_fechas = [];
var graph_obs_fechas;
//Variable gráfico MAP vs SSO
var data_map_vs_sso = [];
var graph_map_vs_sso;
//Variable gráfico MAP vs SSO x Estado
var data_map_vs_sso_estado = [];
var graph_map_vs_sso_estado;
//Variable gráfico MAP vs SSO
var data_bp_map_vs_sso = [];
var graph_bp_map_vs_sso;
//Variable gráfico MAP vs SSO x Estado
var data_bp_map_vs_sso_estado = [];
var graph_bp_map_vs_sso_estado;
//Variables gráfico MA nivel de riesgo
var data_obs_ma_nr = [];
var graph_obs_ma_nr;
//Variables gráfico SSO nivel de riesgo
var data_obs_sso_nr = [];
var graph_obs_sso_nr;
//Variables gráfico MA tipo condición
var data_obs_ma_c = [];
var graph_obs_ma_c;
//Variables gráfico SSO tipo condición
var data_obs_sso_c = [];
var graph_obs_sso_c;
//Variables gráfico MA tipo condición por nivel riesgo
var data_ma_nr_tipo = [];
var graph_ma_tol_tipo;
var graph_ma_ale_tipo;
var graph_ma_noa_tipo;
var graph_ma_noaplica_tipo;
//Variables gráfico SSO tipo condición por nivel riesgo
var data_sso_nr_tipo = [];
var graph_sso_baj_tipo;
var graph_sso_med_tipo;
var graph_sso_alt_tipo;
var graph_sso_noaplica_tipo;

//Variables gráfico Gerencias Inspeccionadas por Estado de Observaciones
var data_obs_gerencias_estado = [];
var graph_obs_gerencias_estado;


$(document).ready(function () {
    var savedData = localStorage.getItem("ResumenReportesInspecciones");

    if (savedData) {
        const filtrosData = JSON.parse(savedData);
        $('#IdGerencia').val(filtrosData.gerencia);
        $('#IdEmpresa').val(filtrosData.empresa);
        $('#IdAsesor').val(filtrosData.asesor);
        $('#IdRespLev').val(filtrosData.respLev);
        $('#FInicio').val(filtrosData.fechaInicio);
        $('#FFin').val(filtrosData.fechaFin);
        $("#UsuarioInvolucrado").val(filtrosData.usuarioInvolucrado);
        $("#TipoObservacion").val(filtrosData.tipoObservacion);
    }
    ActualizarInspeccionesFecha();
});
function Filtrar() {
    var filtros = {
        gerencia : $("#IdGerencia").val(),
        empresa : $("#IdEmpresa").val(),
        asesor : $("#IdAsesor").val(),
        respLev : $("#IdRespLev").val(),
        fechaInicio : $("#FInicio").val(),
        fechaFin: $("#FFin").val(),
        usuarioInvolucrado: $("#UsuarioInvolucrado").val(),
        tipoObservacion: $("#TipoObservacion").val()
    };
    localStorage.setItem("ResumenReportesInspecciones", JSON.stringify(filtros));
    ActualizarInspeccionesFecha();
}

function ActualizarInspeccionesFecha() {
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
                "FechaFin": $("#FFin").val(),
                "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
                "tipoObservacion": $("#TipoObservacion").val()
            },
            success: (response) => {
                var nObs = response.nInspecciones;
                ActualizarContadorInsXEst(nObs[0].valor, nObs[1].valor, nObs[2].valor);

                ActualizarInspeccionesXFecha();
                ActualizarContadorOxFecha();
                ActualizaMAPvsSSO();
                ActualizaBPMAPvsSSO();
                GetDataContadorOxE();
                ActualizarDatosGraficoMAP_SSO_Estado();
                ActualizarDatosGraficoBPMAP_SSO_Estado();
                ActualizarDatosGraficoMA_NR();
                ActualizarDatosGraficoSSO_NR();
                ActualizarDatosGraficoMA_Clas();
                ActualizarDatosGraficoSSO_Clas();
                ActualizarDatosGraficoGerenciasInspeccionadaxEstadoObservaciones();
            }
        });

    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
//Para contador de inspecciones x estado
function ActualizarContadorInsXEst(pend, reg, tot) {
    //$("#InsPendiente").html(pend);
    //$("#InsRegistrado").html(reg);
    $("#InsTotal").html(tot);
    $("#InsEncont").html(reg);
}
//Para contador de inspecciones x fecha
$("#sel_frec_insp_fecha").on('change', function () {
    ActualizarInspeccionesXFecha();
});
function ActualizarInspeccionesXFecha() {
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
            "frecuencia": $("#sel_frec_insp_fecha").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
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
                CrearGraficaInspeccionesFecha(labels, data, Math.max(data) / 10);
            }
            ActualizarContadorOxFecha();
        }
    });
}
function CrearGraficaInspeccionesFecha(labels, data, stepSize) {
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
//$("#sel_frec_obs_estado").on('change', function () {
//    ActualizarContadorOxFecha();
//    GetDataContadorOxE();
//    ActualizaMAPvsSSO();
//    ActualizarDatosGraficoMAP_SSO_Estado();
//    ActualizaBPMAPvsSSO();
//    ActualizarDatosGraficoBPMAP_SSO_Estado();
//    ActualizarDatosGraficoMA_NR();
//    ActualizarDatosGraficoSSO_NR();
//    ActualizarDatosGraficoMA_Clas();
//    ActualizarDatosGraficoSSO_Clas();
//});
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
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_fechas.length > 0 || data_bp_fechas.length > 0) {
                //Actualizar contador
                data_obs_fechas = response.observacionesXFecha[0];
                data_bp_fechas = response.observacionesXFecha[1];

                var labels = [];
                var dataObs = [];
                var dataBP = [];
                for (var i = 0; i < data_obs_fechas.length; i++) {
                    labels.push(data_obs_fechas[i].nombre);
                    dataObs.push(data_obs_fechas[i].valor);
                }
                for (var i = 0; i < data_bp_fechas.length; i++) {
                    dataBP.push(data_bp_fechas[i].valor);
                }
                //Actualizar gráfica
                graph_obs_fechas.data.labels = labels;
                graph_obs_fechas.data.datasets[0].data = dataObs;
                graph_obs_fechas.data.datasets[1].data = dataBP;
                graph_obs_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(dataObs) / 10;
                graph_obs_fechas.update();
            }
            else {
                //Actualizar contador
                data_obs_fechas = response.observacionesXFecha[0];
                data_bp_fechas = response.observacionesXFecha[1];

                var labels = [];
                var dataObs = [];
                var dataBP = [];
                for (var i = 0; i < data_obs_fechas.length; i++) {
                    labels.push(data_obs_fechas[i].nombre);
                    dataObs.push(data_obs_fechas[i].valor);
                }
                for (var i = 0; i < data_bp_fechas.length; i++) {
                    dataBP.push(data_bp_fechas[i].valor);
                }
                //Crear gráfica
                CrearGraficaContardorOxFecha(labels, dataObs, dataBP, Math.max(dataObs) / 10);
            }
            //ActualizaMAPvsSSO();
            //ActualizaBPMAPvsSSO();
        }
    });
}
function CrearGraficaContardorOxFecha(labels, dataObs, dataBP, stepSize) {
    graph_obs_fechas = new Chart(
        document.getElementById("chart_obs_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Observaciones Registradas",
                        "data": dataObs,
                        "fill": false,
                        "backgroundColor": "#36b9cc",
                        "borderColor": "#36b9cc",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Buenas Prácticas Registradas",
                        "data": dataBP,
                        "fill": false,
                        "backgroundColor": "#36CC3F",
                        "borderColor": "#36CC3F",
                        "lineTension": 0.1
                    },
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
                        color: '#242424',
                        defaultFontSize: 15,
                        anchor: 'end',
                        align: 'end',
                        //offset: 2,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
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
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        success: (response) => {
            var nObs = response.nObservaciones;
            ActualizarContadorOxE(nObs[0].valor, nObs[1].valor, nObs[2].valor, nObs[3].valor, nObs[4].valor, nObs[5].valor, nObs[6].valor, nObs[7].valor, nObs[8].valor, nObs[9].valor);
        }
    });
}
function ActualizarContadorOxE(reg, imp, apr, rec, ven, amp, total,bimp,bapr,brec) {
    //Reiniciar valores
    $("#ObsRegistrado").html(0);
    $("#ObsImplementado").html(0);
    $("#ObsAprobado").html(0);
    $("#ObsRechazado").html(0);
    $("#ObsVencido").html(0);
    $("#ObsAmpliado").html(0);
    $("#ObsTotal").html(0);
    $("#ObsEncont").html(0);
    $("#BPEncont").html(0);
    $("#BImplementadas").html(0);
    $("#BPAprobado").html(0);
    $("#BPRechazado").html(0);
    //Nuevo valores
    $("#ObsRegistrado").html(reg);
    $("#ObsImplementado").html(imp);
    $("#ObsAprobado").html(apr);
    $("#ObsRechazado").html(rec);
    $("#ObsVencido").html(ven);
    $("#ObsAmpliado").html(amp);
    $("#ObsTotal").html(total);
    $("#ObsEncont").html(reg + imp + apr + rec + ven + amp);
    $("#BPEncont").html(bimp + bapr + brec);
    $("#BImplementadas").html(bimp);
    $("#BPAprobado").html(bapr);
    $("#BPRechazado").html(brec);
}
//Para gráfico MAP vs SSO
function ActualizaMAPvsSSO() {
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
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
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
                CrearGraficoMAPvsSSO(labels, data);
            }
        }
    });
}
function CrearGraficoMAPvsSSO(labels, data) {
    graph_map_vs_sso = new Chart(
        document.getElementById("chart_MAP_vs_SSO"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#1cc88a'
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
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_map_vs_sso_estado.length > 0) {
                //Actualizar contador
                data_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var dataset1 = [];
                var dataset2 = [];
                for (var i = 0; i < data_map_vs_sso_estado[0].length; i++) {
                    labels.push(data_map_vs_sso_estado[0][i].nombre);
                    dataset1.push(data_map_vs_sso_estado[0][i].valor);
                }
                for (var i = 0; i < data_map_vs_sso_estado[1].length; i++) {
                    dataset2.push(data_map_vs_sso_estado[1][i].valor);
                }
                //Actualizar gráfica
                graph_map_vs_sso_estado.data.labels = labels;
                graph_map_vs_sso_estado.data.datasets[0].data = dataset1;
                graph_map_vs_sso_estado.data.datasets[1].data = dataset2;
                graph_map_vs_sso_estado.update();
            } else {
                //Actualizar contador
                data_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var dataset1 = [];
                var dataset2 = [];
                for (var i = 0; i < data_map_vs_sso_estado[0].length; i++) {
                    labels.push(data_map_vs_sso_estado[0][i].nombre);
                    dataset1.push(data_map_vs_sso_estado[0][i].valor);
                }
                for (var i = 0; i < data_map_vs_sso_estado[1].length; i++) {
                    dataset2.push(data_map_vs_sso_estado[1][i].valor);
                }
                //Crear gráfica
                CrearGraficoMAP_SSO_Estado(labels, dataset1, dataset2);
            }
        }
    });
}
function CrearGraficoMAP_SSO_Estado(labels, dataset1, dataset2) {
    graph_map_vs_sso_estado = new Chart(
        document.getElementById("chart_MA_vs_SSO_Estado"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Observaciones MAP",
                        "data": dataset1,
                        "backgroundColor":'#2e6793',
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Observaciones SSO",
                        "data": dataset2,
                        "backgroundColor": '#1cc88a',
                        "lineTension": 0.1
                    }
                ]
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
                        color: '#242424',
                        defaultFontSize: 15,
                        anchor: 'end',
                        align: 'end',
                        offset: 5,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        }
    );
}
//Para gráfico BP MAP vs SSO
function ActualizaBPMAPvsSSO() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarBPMAPvsSSO",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_bp_map_vs_sso.length > 0) {
                //Actualizar contador
                data_bp_map_vs_sso = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_bp_map_vs_sso.length; i++) {
                    labels.push(data_bp_map_vs_sso[i].nombre);
                    data.push(data_bp_map_vs_sso[i].valor);
                }
                //Actualizar gráfica
                graph_bp_map_vs_sso.data.labels = labels;
                graph_bp_map_vs_sso.data.datasets[0].data = data;
                graph_bp_map_vs_sso.update();
            } else {
                //Actualizar contador
                data_bp_map_vs_sso = response.mapvssso;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_bp_map_vs_sso.length; i++) {
                    labels.push(data_bp_map_vs_sso[i].nombre);
                    data.push(data_bp_map_vs_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoBPMAPvsSSO(labels, data);
            }
        }
    });
}
function CrearGraficoBPMAPvsSSO(labels, data) {
    graph_bp_map_vs_sso = new Chart(
        document.getElementById("chart_BP_MAP_vs_SSO"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Buenas Prácticas",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#1cc88a'
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
//Para gráfico BP MAP vs SSO x Estado
function ActualizarDatosGraficoBPMAP_SSO_Estado() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarBPMAPvsSSO_x_Estado",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_bp_map_vs_sso_estado.length > 0) {
                //Actualizar contador
                data_bp_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var dataset1 = [];
                var dataset2 = [];
                for (var i = 0; i < data_bp_map_vs_sso_estado[0].length; i++) {
                    labels.push(data_bp_map_vs_sso_estado[0][i].nombre);
                    dataset1.push(data_bp_map_vs_sso_estado[0][i].valor);
                }
                for (var i = 0; i < data_bp_map_vs_sso_estado[1].length; i++) {
                    dataset2.push(data_bp_map_vs_sso_estado[1][i].valor);
                }
                //Actualizar gráfica
                graph_bp_map_vs_sso_estado.data.labels = labels;
                graph_bp_map_vs_sso_estado.data.datasets[0].data = dataset1;
                graph_bp_map_vs_sso_estado.data.datasets[1].data = dataset2;
                graph_bp_map_vs_sso_estado.update();
            } else {
                //Actualizar contador
                data_bp_map_vs_sso_estado = response.mapvssso;

                var labels = [];
                var dataset1 = [];
                var dataset2 = [];
                for (var i = 0; i < data_bp_map_vs_sso_estado[0].length; i++) {
                    labels.push(data_bp_map_vs_sso_estado[0][i].nombre);
                    dataset1.push(data_bp_map_vs_sso_estado[0][i].valor);
                }
                for (var i = 0; i < data_bp_map_vs_sso_estado[1].length; i++) {
                    dataset2.push(data_bp_map_vs_sso_estado[1][i].valor);
                }
                //Crear gráfica
                CrearGraficoBPMAP_SSO_Estado(labels, dataset1, dataset2);
            }
        }
    });
}
function CrearGraficoBPMAP_SSO_Estado(labels, dataset1, dataset2) {
    graph_bp_map_vs_sso_estado = new Chart(
        document.getElementById("chart_BP_MA_vs_SSO_Estado"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Buenas Prácticas MAP",
                        "data": dataset1,
                        "backgroundColor": [
                            '#2e6793',
                            '#2e6793',
                            '#2e6793',
                            '#2e6793'
                        ],
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Buenas Prácticas SSO",
                        "data": dataset2,
                        "backgroundColor": [
                            '#1cc88a',
                            '#1cc88a',
                            '#1cc88a',
                            '#1cc88a'
                        ],
                        "lineTension": 0.1
                    }
                ]
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
                        color: '#242424',
                        defaultFontSize: 15,
                        anchor: 'end',
                        align: 'end',
                        offset: 5,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        }
    );
}
//-------------------------------------------------------------------------------------
//Para gráfico MA Nivel de Riesgo
function ActualizarDatosGraficoMA_NR() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsNivelRiesgoMA",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_ma_nr.length > 0) {
                //Actualizar contador
                data_obs_ma_nr = response.nivelRiesgoMA;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_ma_nr.length; i++) {
                    labels.push(data_obs_ma_nr[i].nombre);
                    data.push(data_obs_ma_nr[i].valor);
                }
                //Actualizar gráfica
                graph_obs_ma_nr.data.labels = labels;
                graph_obs_ma_nr.data.datasets[0].data = data;
                graph_obs_ma_nr.update();
            } else {
                //Actualizar contador
                data_obs_ma_nr = response.nivelRiesgoMA;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_ma_nr.length; i++) {
                    labels.push(data_obs_ma_nr[i].nombre);
                    data.push(data_obs_ma_nr[i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_NR(labels, data);
            }
        }
    });
}
function CrearGraficoMA_NR(labels, data) {
    graph_obs_ma_nr = new Chart(
        document.getElementById("chart_MA_Nivel_Riesgo"),
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
                        defaultFontSize: 15,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        }
    );
}
//Para gráfico SSO Nivel de Riesgo
function ActualizarDatosGraficoSSO_NR() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsNivelRiesgoSSO",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_sso_nr.length > 0) {
                //Actualizar contador
                data_obs_sso_nr = response.nivelRiesgoSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_sso_nr.length; i++) {
                    labels.push(data_obs_sso_nr[i].nombre);
                    data.push(data_obs_sso_nr[i].valor);
                }
                //Actualizar gráfica
                graph_obs_sso_nr.data.labels = labels;
                graph_obs_sso_nr.data.datasets[0].data = data;
                graph_obs_sso_nr.update();
            } else {
                //Actualizar contador
                data_obs_sso_nr = response.nivelRiesgoSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_sso_nr.length; i++) {
                    labels.push(data_obs_sso_nr[i].nombre);
                    data.push(data_obs_sso_nr[i].valor);
                }
                //Crear gráfica
                CrearGraficoSSO_NR(labels, data);
            }
        }
    });
}
function CrearGraficoSSO_NR(labels, data) {
    graph_obs_sso_nr = new Chart(
        document.getElementById("chart_SSO_Nivel_Riesgo"),
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
                        defaultFontSize: 15,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        }
    );
}
//Para gráfico MA Clasificación
function ActualizarDatosGraficoMA_Clas() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsClasificacionMA",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_ma_c.length > 0) {
                //Actualizar contador
                data_obs_ma_c = response.clasificacionMA;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_ma_c.length; i++) {
                    labels.push(data_obs_ma_c[i].nombre);
                    data.push(data_obs_ma_c[i].valor);
                }
                //Actualizar gráfica
                graph_obs_ma_c.data.labels = labels;
                graph_obs_ma_c.data.datasets[0].data = data;
                graph_obs_ma_c.update();
            } else {
                //Actualizar contador
                data_obs_ma_c = response.clasificacionMA;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_ma_c.length; i++) {
                    labels.push(data_obs_ma_c[i].nombre);
                    data.push(data_obs_ma_c[i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_Clas(labels, data);
            }
        }
    });
}
function CrearGraficoMA_Clas(labels, data) {
    graph_obs_ma_c = new Chart(
        document.getElementById("chart_MA_Clasificacion"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Inspecciones Registradas",
                    "data": data,
                    "backgroundColor": [
                        '#ed4c4c',
                        '#36b9cc',
                        '#3DFE8E'
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
                        defaultFontSize: 15,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        }
    );
}
//Para gráfico SSO Clasificación
function ActualizarDatosGraficoSSO_Clas() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsClasificacionSSO",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            if (data_obs_sso_c.length > 0) {
                //Actualizar contador
                data_obs_sso_c = response.clasificacionSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_sso_c.length; i++) {
                    labels.push(data_obs_sso_c[i].nombre);
                    data.push(data_obs_sso_c[i].valor);
                }
                //Actualizar gráfica
                graph_obs_sso_c.data.labels = labels;
                graph_obs_sso_c.data.datasets[0].data = data;
                graph_obs_sso_c.update();
            } else {
                //Actualizar contador
                data_obs_sso_c = response.clasificacionSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_obs_sso_c.length; i++) {
                    labels.push(data_obs_sso_c[i].nombre);
                    data.push(data_obs_sso_c[i].valor);
                }
                //Crear gráfica
                CrearGraficoSSO_Clas(labels, data);
            }
        }
    });
}
function CrearGraficoSSO_Clas(labels, data) {
    graph_obs_sso_c = new Chart(
        document.getElementById("chart_SSO_Clasificacion"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Inspecciones Registradas",
                    "data": data,
                    "backgroundColor": [
                        '#ed4c4c',
                        '#36b9cc',
                        '#3DFE8E'
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

function ActualizarDatosGraficoMA_NR_Tipo() {
    ActualizarInspeccionesFecha();
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsTipoCondicionxNivelRiesgoMA",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            //0: Tolerable
            //1: Alerta
            //2: No aceptable
            //3: No aplica
            if (data_ma_nr_tipo.length > 0) {
                //Actualizar contador
                data_ma_nr_tipo = response.tipoNivelRiesgoMA;

                var labels = [];
                var data = [];
                //Tolerable
                for (var i = 0; i < data_ma_nr_tipo[0].length; i++) {
                    labels.push(data_ma_nr_tipo[0][i].nombre);
                    data.push(data_ma_nr_tipo[0][i].valor);
                }
                //Actualizar gráfica
                graph_ma_tol_tipo.data.labels = labels;
                graph_ma_tol_tipo.data.datasets[0].data = data;
                graph_ma_tol_tipo.update();
                labels = [];
                data = [];
                //Alerta
                for (var i = 0; i < data_ma_nr_tipo[1].length; i++) {
                    labels.push(data_ma_nr_tipo[1][i].nombre);
                    data.push(data_ma_nr_tipo[1][i].valor);
                }
                //Actualizar gráfica
                graph_ma_ale_tipo.data.labels = labels;
                graph_ma_ale_tipo.data.datasets[0].data = data;
                graph_ma_ale_tipo.update();
                labels = [];
                data = [];
                //No aceptable
                for (var i = 0; i < data_ma_nr_tipo[2].length; i++) {
                    labels.push(data_ma_nr_tipo[2][i].nombre);
                    data.push(data_ma_nr_tipo[2][i].valor);
                }
                //Actualizar gráfica
                graph_ma_noa_tipo.data.labels = labels;
                graph_ma_noa_tipo.data.datasets[0].data = data;
                graph_ma_noa_tipo.update();
                labels = [];
                data = [];
                //No Aplica
                for (var i = 0; i < data_ma_nr_tipo[3].length; i++) {
                    labels.push(data_ma_nr_tipo[3][i].nombre);
                    data.push(data_ma_nr_tipo[3][i].valor);
                }
                //Actualizar gráfica
                graph_ma_noaplica_tipo.data.labels = labels;
                graph_ma_noaplica_tipo.data.datasets[0].data = data;
                graph_ma_noaplica_tipo.update();
            }
            else {
                //Actualizar contador
                data_ma_nr_tipo = response.tipoNivelRiesgoMA;

                var labels = [];
                var data = [];
                //Tolerable
                for (var i = 0; i < data_ma_nr_tipo[0].length; i++) {
                    labels.push(data_ma_nr_tipo[0][i].nombre);
                    data.push(data_ma_nr_tipo[0][i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_Tolerable_Tipo(labels, data);
                labels = [];
                data = [];

                //Alerta
                for (var i = 0; i < data_ma_nr_tipo[1].length; i++) {
                    labels.push(data_ma_nr_tipo[1][i].nombre);
                    data.push(data_ma_nr_tipo[1][i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_Alerta_Tipo(labels, data);
                labels = [];
                data = [];
                //No aceptable
                for (var i = 0; i < data_ma_nr_tipo[2].length; i++) {
                    labels.push(data_ma_nr_tipo[2][i].nombre);
                    data.push(data_ma_nr_tipo[2][i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_No_Aceptable_Tipo(labels, data);
                labels = [];
                data = [];
                //No aplica
                for (var i = 0; i < data_ma_nr_tipo[3].length; i++) {
                    labels.push(data_ma_nr_tipo[3][i].nombre);
                    data.push(data_ma_nr_tipo[3][i].valor);
                }
                //Crear gráfica
                CrearGraficoMA_No_Aplica_Tipo(labels, data);
            }
            $("#Modal_Nivel_Riesgo_MAP").modal('show');
        }
    });
}

function CrearGraficoMA_Tolerable_Tipo(labels, data) {
    graph_ma_tol_tipo = new Chart(document.getElementById('chart_MAP_Nivel_Riesgo_Tolerable'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones Tolerables MA",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoMA_Alerta_Tipo(labels, data) {
    graph_ma_ale_tipo = new Chart(document.getElementById('chart_MAP_Nivel_Riesgo_Alerta'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones Alerta MA",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoMA_No_Aceptable_Tipo(labels, data) {
    graph_ma_noa_tipo = new Chart(document.getElementById('chart_MAP_Nivel_Riesgo_No_Aceptable'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones No Aceptables MA",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoMA_No_Aplica_Tipo(labels, data) {
    graph_ma_noaplica_tipo = new Chart(document.getElementById('chart_MAP_Nivel_Riesgo_No_Aplica'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones No Aplica MA",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function ActualizarDatosGraficoSSO_NR_Tipo() {
    ActualizarInspeccionesFecha();
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarObsTipoCondicionxNivelRiesgoSSO",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            //0: Bajo
            //1: Medio
            //2: Alto
            if (data_sso_nr_tipo.length > 0) {
                //Actualizar contador
                data_sso_nr_tipo = response.tipoNivelRiesgoSSO;

                var labels = [];
                var data = [];
                //Bajo
                for (var i = 0; i < data_sso_nr_tipo[0].length; i++) {
                    labels.push(data_sso_nr_tipo[0][i].nombre);
                    data.push(data_sso_nr_tipo[0][i].valor);
                }
                //Actualizar gráfica
                graph_sso_baj_tipo.data.labels = labels;
                graph_sso_baj_tipo.data.datasets[0].data = data;
                graph_sso_baj_tipo.update();
                labels = [];
                data = [];
                //Medio
                for (var i = 0; i < data_sso_nr_tipo[1].length; i++) {
                    labels.push(data_sso_nr_tipo[1][i].nombre);
                    data.push(data_sso_nr_tipo[1][i].valor);
                }
                //Actualizar gráfica
                graph_sso_med_tipo.data.labels = labels;
                graph_sso_med_tipo.data.datasets[0].data = data;
                graph_sso_med_tipo.update();
                labels = [];
                data = [];
                //Alto
                for (var i = 0; i < data_sso_nr_tipo[2].length; i++) {
                    labels.push(data_sso_nr_tipo[2][i].nombre);
                    data.push(data_sso_nr_tipo[2][i].valor);
                }
                //Actualizar gráfica
                graph_sso_alt_tipo.data.labels = labels;
                graph_sso_alt_tipo.data.datasets[0].data = data;
                graph_sso_alt_tipo.update();
                labels = [];
                data = [];
                //No Aplica
                for (var i = 0; i < data_sso_nr_tipo[3].length; i++) {
                    labels.push(data_sso_nr_tipo[3][i].nombre);
                    data.push(data_sso_nr_tipo[3][i].valor);
                }
                //Actualizar gráfica
                graph_sso_noaplica_tipo.data.labels = labels;
                graph_sso_noaplica_tipo.data.datasets[0].data = data;
                graph_sso_noaplica_tipo.update();
            }
            else {
                //Actualizar contador
                data_sso_nr_tipo = response.tipoNivelRiesgoSSO;

                var labels = [];
                var data = [];
                //Bajo
                for (var i = 0; i < data_sso_nr_tipo[0].length; i++) {
                    labels.push(data_sso_nr_tipo[0][i].nombre);
                    data.push(data_sso_nr_tipo[0][i].valor);
                }                
                //Crear gráfica
                CrearGraficoSSO_Bajo_Tipo(labels, data);
                labels = [];
                data = [];

                //Medio
                for (var i = 0; i < data_sso_nr_tipo[1].length; i++) {
                    labels.push(data_sso_nr_tipo[1][i].nombre);
                    data.push(data_sso_nr_tipo[1][i].valor);
                }                
                //Crear gráfica
                CrearGraficoSSO_Medio_Tipo(labels, data);
                labels = [];
                data = [];
                //Alto
                for (var i = 0; i < data_sso_nr_tipo[2].length; i++) {
                    labels.push(data_sso_nr_tipo[2][i].nombre);
                    data.push(data_sso_nr_tipo[2][i].valor);
                }                
                //Crear gráfica
                CrearGraficoSSO_Alto_Tipo(labels, data);
                labels = [];
                data = [];
                //No Aplica
                for (var i = 0; i < data_sso_nr_tipo[3].length; i++) {
                    labels.push(data_sso_nr_tipo[3][i].nombre);
                    data.push(data_sso_nr_tipo[3][i].valor);
                }
                //Crear gráfica
                CrearGraficoSSO_Noaplica_Tipo(labels, data);
                labels = [];
                data = [];
            }
            $("#Modal_Nivel_Riesgo_SSO").modal('show');
        }
    });
}

function CrearGraficoSSO_Bajo_Tipo(labels, data) {
    graph_sso_baj_tipo = new Chart(document.getElementById('chart_SSO_Nivel_Riesgo_Bajo'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones Bajo SSO",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoSSO_Medio_Tipo(labels, data) {
    graph_sso_med_tipo = new Chart(document.getElementById('chart_SSO_Nivel_Riesgo_Medio'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones Medio SSO",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoSSO_Alto_Tipo(labels, data) {
    graph_sso_alt_tipo = new Chart(document.getElementById('chart_SSO_Nivel_Riesgo_Alto'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones Alto SSO",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

function CrearGraficoSSO_Noaplica_Tipo(labels, data) {
    graph_sso_noaplica_tipo = new Chart(document.getElementById('chart_SSO_Nivel_Riesgo_No_Aplica'),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Observaciones No Aplica SSO",
                    "data": data,
                    "fill": false,
                    "backgroundColor": ["rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(54, 162, 235, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                    "borderColor": ["rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(54, 162, 235)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                    "borderWidth": 1
                }]
            },
            "options": {
                "scales": {
                    "xAxes": [{
                        "ticks": {
                            "beginAtZero": true,
                            "stepSize": 1
                        }
                    }]
                },
                "plugins": {
                    "datalabels": {
                        "anchor": "center",
                        "color": "black",
                        "defaultFontSize": 15
                    }
                }
            }
        });
}

//Para gráfico Gerencias Inspeccionada x estado de observaciones
function ActualizarDatosGraficoGerenciasInspeccionadaxEstadoObservaciones() {
    $.ajax({
        type: "POST",
        url: "/ReportesInspecciones/ActualizarGerenciaInspeccionadaxEstadoObs",
        data: {
            "gerencia": $("#IdGerencia").val(),
            "empresa": $("#IdEmpresa").val(),
            "asesor": $("#IdAsesor").val(),
            "respLevObs": $("#IdRespLev").val(),
            "FechaInicio": $("#FInicio").val(),
            "FechaFin": $("#FFin").val(),
            "estado": $("#Estado").val(),
            "idGerenciaInspeccionada": $("#IdGerenciaInspeccionada").val(),
            "usuarioInvolucrado": $("#UsuarioInvolucrado").val(),
            "tipoObservacion": $("#TipoObservacion").val()
        },
        traditional: true,
        success: (response) => {
            console.log(response)
            if (data_obs_gerencias_estado.length > 0) {
                //Actualizar contador
                data_obs_gerencias_estado = response.gerenciaInspeccionadaxEstadoObs;
                var labels = [];
                var dataTotal = [];
                var dataPendiente = [];
                var dataVerificacion = [];
                var dataVencido = [];
                var dataAprobado = [];

                for (var i = 0; i < data_obs_gerencias_estado.length; i++) {
                    labels.push(data_obs_gerencias_estado[i].gerenciaInspeccionada);
                    dataTotal.push(data_obs_gerencias_estado[i].total);
                    dataPendiente.push(data_obs_gerencias_estado[i].pendientes);
                    dataVerificacion.push(data_obs_gerencias_estado[i].verificacion);
                    dataVencido.push(data_obs_gerencias_estado[i].vencido);
                    //dataAprobado.push(data_obs_gerencias_estado[i].aprobado);
                }
                //Actualizar gráfica
                graph_obs_gerencias_estado.data.labels = labels;
                //graph_obs_gerencias_estado.data.datasets[0].data = dataTotal;
                graph_obs_gerencias_estado.data.datasets[0].data = dataPendiente;
                graph_obs_gerencias_estado.data.datasets[1].data = dataVerificacion;
                graph_obs_gerencias_estado.data.datasets[2].data = dataVencido;
                //graph_obs_gerencias_estado.data.datasets[3].data = dataAprobado;
                graph_obs_gerencias_estado.options.scales.yAxes[0].ticks.stepSize = Math.max(dataTotal) / 10;
                graph_obs_gerencias_estado.update();
            }
            else {
                //Actualizar contador
                data_obs_gerencias_estado = response.gerenciaInspeccionadaxEstadoObs;
                var labels = [];
                var dataTotal = [];
                var dataPendiente = [];
                var dataVerificacion = [];
                var dataVencido = [];
                var dataAprobado = [];


                for (var i = 0; i < data_obs_gerencias_estado.length; i++) {
                    labels.push(data_obs_gerencias_estado[i].gerenciaInspeccionada);
                    dataTotal.push(data_obs_gerencias_estado[i].total);
                    dataPendiente.push(data_obs_gerencias_estado[i].pendientes);
                    dataVerificacion.push(data_obs_gerencias_estado[i].verificacion);
                    dataVencido.push(data_obs_gerencias_estado[i].vencido);
                    dataAprobado.push(data_obs_gerencias_estado[i].aprobado);
                }
                //Crear gráfica
                CrearGraficoGerenciasInspeccionadaxEstadoObservaciones(labels, Math.max(dataTotal) / 10, dataTotal, dataPendiente, dataVerificacion, dataVencido, dataAprobado);
            }
        }
    });
}
function CrearGraficoGerenciasInspeccionadaxEstadoObservaciones(labels, stepSize, total, pendientes, verificacion, vencidos,aprobados) {
    graph_obs_gerencias_estado = new Chart(
        document.getElementById("char_estado_observaciones_x_gerencia"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    //{
                    //    "label": "Totales",
                    //    "data": total,
                    //    "fill": false,
                    //    "backgroundColor": "#36b9cc",
                    //    "borderColor": "#36b9cc",
                    //    "lineTension": 0.1
                    //},
                    {
                        "label": "Pendientes de Implementación",
                        "data": pendientes,
                        "fill": false,
                        "backgroundColor": "#00caca",
                        "borderColor": "#00caca",
                        "lineTension": 0.1
                    },
                    {
                        "label": "Pendientes de Verificación",
                        "data": verificacion,
                        "fill": false,
                        "backgroundColor": "#0050ca",
                        "borderColor": "#0050ca",
                        "lineTension": 0.1
                    },
                    {
                        "label": "Vencidos",
                        "data": vencidos,
                        "fill": false,
                        "backgroundColor": "#ff0000",
                        "borderColor": "#ff0000",
                        "lineTension": 0.1
                    },
                    //{
                    //    "label": "Aprobados",
                    //    "data": aprobados,
                    //    "fill": false,
                    //    "backgroundColor": "#00ca15",
                    //    "borderColor": "#00ca15",
                    //    "lineTension": 0.1
                    //},
                ]
            },
            "options": {
                responsive: true, // Habilita la respuesta dinámica
                maintainAspectRatio: false, // Permite cambiar el tamaño sin mantener la relación de aspecto
                "scales": {
                    'yAxes': [{
                        //stacked: true,
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
                        color: '#242424',
                        defaultFontSize: 15,
                        anchor: 'end',
                        align: 'end',
                        offset: 5,
                        display: function (context) {
                            // Retorna false si el valor es 0, así no se muestra la etiqueta
                            return context.dataset.data[context.dataIndex] !== 0;
                        }
                    }
                }
            }
        });
    //console.log(graph_obs_gerencias_estado)
}
const resizeObserver_Chart_estado_observaciones_x_gerencia = new ResizeObserver(() => {
    graph_obs_gerencias_estado.resize();
});
resizeObserver_Chart_estado_observaciones_x_gerencia.observe(document.getElementById('container_char_estado_observaciones_x_gerencia'));