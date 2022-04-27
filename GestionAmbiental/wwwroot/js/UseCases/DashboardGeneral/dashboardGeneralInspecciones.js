$(document).ready(function () {
    ActualizarInspeccionDatosGraficoActoCondicionBP();
    ActualizarInspeccionDatosGraficoCondicion();
    ActualizarInspeccionDatosGraficoTopActos();
    ActualizarInspeccionDatosGraficoTopCondicion();
    ActualizarInspeccionDatosGraficoRankingGerenciaIngresoInsp();
    ActualizarInspeccionDatosGraficoRankingGerenciaObsVencidas();
    ActualizarInspeccionDatosGraficoRankingUsuarioIngresoInsp();
    ActualizarInspeccionDatosGraficoRankingUsuarioObsVencidas();
});
function FiltrarInspecciones() {
    ActualizarInspeccionDatosGraficoActoCondicionBP();
    ActualizarInspeccionDatosGraficoCondicion();
    ActualizarInspeccionDatosGraficoTopActos();
    ActualizarInspeccionDatosGraficoTopCondicion();
    ActualizarInspeccionDatosGraficoRankingGerenciaIngresoInsp();
    ActualizarInspeccionDatosGraficoRankingGerenciaObsVencidas();
    ActualizarInspeccionDatosGraficoRankingUsuarioIngresoInsp();
    ActualizarInspeccionDatosGraficoRankingUsuarioObsVencidas();
}
//Actos Subestándar - Condiciones Subestándar - Buenas Prácticas
//Variables gráficos chart_inspeccion_acto_condicion_bp
var data_acto_condicion_bp_map_inspeccion = [];
var graph_acto_condicion_bp_map_inspeccion;

var data_acto_condicion_bp_sso_inspeccion = [];
var graph_acto_condicion_bp_sso_inspeccion;

var data_acto_condicion_bp_todo_inspeccion = [];
var graph_acto_condicion_bp_todo_inspeccion;
//Para gráficos chart_inspeccion_acto_condicion_bp
function ActualizarInspeccionDatosGraficoActoCondicionBP() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/InspeccionesActoCondicionBP",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_acto_condicion_bp_map_inspeccion.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_map_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_map_inspeccion.data.labels = labels;
                graph_acto_condicion_bp_map_inspeccion.data.datasets[0].data = data;
                graph_acto_condicion_bp_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_map_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesActoCondicionBPMAP(labels, data);
            }
            //SSO
            if (data_acto_condicion_bp_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_sso_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_sso_inspeccion.data.labels = labels;
                graph_acto_condicion_bp_sso_inspeccion.data.datasets[0].data = data;
                graph_acto_condicion_bp_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_sso_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesActoCondicionBPSSO(labels, data);
            }
            //Todo
            if (data_acto_condicion_bp_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_todo_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_todo_inspeccion.data.labels = labels;
                graph_acto_condicion_bp_todo_inspeccion.data.datasets[0].data = data;
                graph_acto_condicion_bp_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo_inspeccion.length; i++) {
                    labels.push(data_acto_condicion_bp_todo_inspeccion[i].nombre);
                    data.push(data_acto_condicion_bp_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesActoCondicionBPTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionesActoCondicionBPMAP(labels, data) {
    graph_acto_condicion_bp_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_acto_condicion_bp_map"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0335",
                        "#0368ff",
                        "#ffe603"
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
function CrearGraficoInspeccionesActoCondicionBPSSO(labels, data) {
    graph_acto_condicion_bp_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_acto_condicion_bp_sso"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0335",
                        "#0368ff",
                        "#ffe603"
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
function CrearGraficoInspeccionesActoCondicionBPTodo(labels, data) {
    graph_acto_condicion_bp_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_acto_condicion_bp_todo"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0335",
                        "#0368ff",
                        "#ffe603"
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

//Condiciones Subestándar (Abierto - Cerrado -En Plazo)
//Variables gráficos chart_inspeccion_condicion_bp
var data_condicion_map_inspeccion = [];
var graph_condicion_map_inspeccion;

var data_condicion_sso_inspeccion = [];
var graph_condicion_sso_inspeccion;

var data_condicion_todo_inspeccion = [];
var graph_condicion_todo_inspeccion;
//Para gráficos chart_inspeccion_condicion_bp

function ActualizarInspeccionDatosGraficoCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/InspeccionesEstadosCondicion",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_condicion_map_inspeccion.length > 0) {
                //Actualizar contador
                data_condicion_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map_inspeccion.length; i++) {
                    labels.push(data_condicion_map_inspeccion[i].nombre);
                    data.push(data_condicion_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_map_inspeccion.data.labels = labels;
                graph_condicion_map_inspeccion.data.datasets[0].data = data;
                graph_condicion_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_condicion_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map_inspeccion.length; i++) {
                    labels.push(data_condicion_map_inspeccion[i].nombre);
                    data.push(data_condicion_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesCondicionMAP(labels, data);
            }
            //SSO
            if (data_condicion_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_condicion_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso_inspeccion.length; i++) {
                    labels.push(data_condicion_sso_inspeccion[i].nombre);
                    data.push(data_condicion_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_sso_inspeccion.data.labels = labels;
                graph_condicion_sso_inspeccion.data.datasets[0].data = data;
                graph_condicion_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_condicion_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso_inspeccion.length; i++) {
                    labels.push(data_condicion_sso_inspeccion[i].nombre);
                    data.push(data_condicion_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesCondicionSSO(labels, data);
            }
            //Todo
            if (data_condicion_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_condicion_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo_inspeccion.length; i++) {
                    labels.push(data_condicion_todo_inspeccion[i].nombre);
                    data.push(data_condicion_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_todo_inspeccion.data.labels = labels;
                graph_condicion_todo_inspeccion.data.datasets[0].data = data;
                graph_condicion_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_condicion_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo_inspeccion.length; i++) {
                    labels.push(data_condicion_todo_inspeccion[i].nombre);
                    data.push(data_condicion_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionesCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionesCondicionMAP(labels, data) {
    graph_condicion_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_condicion_map"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0303",
                        "#0368ff",
                        "#81ff03"
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
function CrearGraficoInspeccionesCondicionSSO(labels, data) {
    graph_condicion_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_condicion_sso"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0303",
                        "#0368ff",
                        "#81ff03"
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
function CrearGraficoInspeccionesCondicionTodo(labels, data) {
    graph_condicion_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_condicion_todo"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "#Registro Encontrados",
                    "data": data,
                    "backgroundColor": [
                        "#ff0303",
                        "#0368ff",
                        "#81ff03"
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

//Top 10 Actos Subestándar Más Reportados
//Variables gráficos chart_inspeccion_top_actos
var data_top_actos_map_inspeccion = [];
var graph_top_actos_map_inspeccion;

var data_top_actos_sso_inspeccion = [];
var graph_top_actos_sso_inspeccion;

var data_top_actos_todo_inspeccion = [];
var graph_top_actos_todo_inspeccion;
//Para gráficos chart_inspeccion_top_actos
function ActualizarInspeccionDatosGraficoTopActos() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/TopActosSubestandar",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_actos_map_inspeccion.length > 0) {
                //Actualizar contador
                data_top_actos_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map_inspeccion.length; i++) {
                    labels.push(data_top_actos_map_inspeccion[i].nombre);
                    data.push(data_top_actos_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_map_inspeccion.data.labels = labels;
                graph_top_actos_map_inspeccion.data.datasets[0].data = data;
                graph_top_actos_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_actos_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map_inspeccion.length; i++) {
                    labels.push(data_top_actos_map_inspeccion[i].nombre);
                    data.push(data_top_actos_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopActosMAP(labels, data);
            }
            //SSO
            if (data_top_actos_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_top_actos_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso_inspeccion.length; i++) {
                    labels.push(data_top_actos_sso_inspeccion[i].nombre);
                    data.push(data_top_actos_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_sso_inspeccion.data.labels = labels;
                graph_top_actos_sso_inspeccion.data.datasets[0].data = data;
                graph_top_actos_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_actos_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso_inspeccion.length; i++) {
                    labels.push(data_top_actos_sso_inspeccion[i].nombre);
                    data.push(data_top_actos_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopActosSSO(labels, data);
            }
            //Todo
            if (data_top_actos_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_top_actos_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo_inspeccion.length; i++) {
                    labels.push(data_top_actos_todo_inspeccion[i].nombre);
                    data.push(data_top_actos_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_todo_inspeccion.data.labels = labels;
                graph_top_actos_todo_inspeccion.data.datasets[0].data = data;
                graph_top_actos_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_actos_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo_inspeccion.length; i++) {
                    labels.push(data_top_actos_todo_inspeccion[i].nombre);
                    data.push(data_top_actos_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopActosTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionTopActosMAP(labels, data) {
    graph_top_actos_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_actos_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionTopActosSSO(labels, data) {
    graph_top_actos_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_actos_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionTopActosTodo(labels, data) {
    graph_top_actos_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_actos_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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

//Top 10 Condición Subestándar Más Reportados
//Variables gráficos chart_inspeccion_top_condicion
var data_top_condicion_map_inspeccion = [];
var graph_top_condicion_map_inspeccion;

var data_top_condicion_sso_inspeccion = [];
var graph_top_condicion_sso_inspeccion;

var data_top_condicion_todo_inspeccion = [];
var graph_top_condicion_todo_inspeccion;
//Para gráficos chart_inspeccion_top_condicion
function ActualizarInspeccionDatosGraficoTopCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/TopCondicionesSubestandar",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_condicion_map_inspeccion.length > 0) {
                //Actualizar contador
                data_top_condicion_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map_inspeccion.length; i++) {
                    labels.push(data_top_condicion_map_inspeccion[i].nombre);
                    data.push(data_top_condicion_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_map_inspeccion.data.labels = labels;
                graph_top_condicion_map_inspeccion.data.datasets[0].data = data;
                graph_top_condicion_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_condicion_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map_inspeccion.length; i++) {
                    labels.push(data_top_condicion_map_inspeccion[i].nombre);
                    data.push(data_top_condicion_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopCondicionMAP(labels, data);
            }
            //SSO
            if (data_top_condicion_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_top_condicion_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso_inspeccion.length; i++) {
                    labels.push(data_top_condicion_sso_inspeccion[i].nombre);
                    data.push(data_top_condicion_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_sso_inspeccion.data.labels = labels;
                graph_top_condicion_sso_inspeccion.data.datasets[0].data = data;
                graph_top_condicion_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_condicion_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso_inspeccion.length; i++) {
                    labels.push(data_top_condicion_sso_inspeccion[i].nombre);
                    data.push(data_top_condicion_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopCondicionSSO(labels, data);
            }
            //Todo
            if (data_top_condicion_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_top_condicion_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo_inspeccion.length; i++) {
                    labels.push(data_top_condicion_todo_inspeccion[i].nombre);
                    data.push(data_top_condicion_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_todo_inspeccion.data.labels = labels;
                graph_top_condicion_todo_inspeccion.data.datasets[0].data = data;
                graph_top_condicion_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_top_condicion_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo_inspeccion.length; i++) {
                    labels.push(data_top_condicion_todo_inspeccion[i].nombre);
                    data.push(data_top_condicion_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionTopCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionTopCondicionMAP(labels, data) {
    graph_top_condicion_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_condicion_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionTopCondicionSSO(labels, data) {
    graph_top_condicion_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_condicion_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionTopCondicionTodo(labels, data) {
    graph_top_condicion_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_top_condicion_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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

//Ranking De Gerencias que Ingresaron Inspecciones
//Variables gráficos chart_inspeccion_ranking_ingreso_inspecciones
var data_ranking_ingreso_map_inspeccion = [];
var graph_ranking_ingreso_map_inspeccion;

var data_ranking_ingreso_sso_inspeccion = [];
var graph_ranking_ingreso_sso_inspeccion;

var data_ranking_ingreso_todo_inspeccion = [];
var graph_ranking_ingreso_todo_inspeccion;
//Para gráficos chart_inspeccion_ranking_ingreso_inspecciones
function ActualizarInspeccionDatosGraficoRankingGerenciaIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/RankingGerenciaReportante",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_ingreso_map_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_map_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_map_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_map_inspeccion.data.labels = labels;
                graph_ranking_ingreso_map_inspeccion.data.datasets[0].data = data;
                graph_ranking_ingreso_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_map_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_map_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_ingreso_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_sso_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_sso_inspeccion.data.labels = labels;
                graph_ranking_ingreso_sso_inspeccion.data.datasets[0].data = data;
                graph_ranking_ingreso_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_sso_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_ingreso_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_todo_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_todo_inspeccion.data.labels = labels;
                graph_ranking_ingreso_todo_inspeccion.data.datasets[0].data = data;
                graph_ranking_ingreso_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_ingreso_todo_inspeccion[i].nombre);
                    data.push(data_ranking_ingreso_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionRankingGerenciaIngresoInspMAP(labels, data) {
    graph_ranking_ingreso_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_ingreso_inspecciones_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionRankingGerenciaIngresoInspSSO(labels, data) {
    graph_ranking_ingreso_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_ingreso_inspecciones_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionRankingGerenciaIngresoInspTodo(labels, data) {
    graph_ranking_ingreso_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_ingreso_inspecciones_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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

//Ranking De Gerencias que Tiene Observaciones Vencidas
//Variables gráficos chart_inspeccion_ranking_obs_vencidas
var data_ranking_obs_vencidas_map_inspeccion = [];
var graph_ranking_obs_vencidas_map_inspeccion;

var data_ranking_obs_vencidas_sso_inspeccion = [];
var graph_ranking_obs_vencidas_sso_inspeccion;

var data_ranking_obs_vencidas_todo_inspeccion = [];
var graph_ranking_obs_vencidas_todo_inspeccion;
//Para gráficos chart_inspeccion_ranking_ingreso_inspecciones
function ActualizarInspeccionDatosGraficoRankingGerenciaObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/RankingGerenciaObsVencidas",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_obs_vencidas_map_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_map_inspeccion.data.labels = labels;
                graph_ranking_obs_vencidas_map_inspeccion.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_obs_vencidas_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_sso_inspeccion.data.labels = labels;
                graph_ranking_obs_vencidas_sso_inspeccion.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_obs_vencidas_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_todo_inspeccion.data.labels = labels;
                graph_ranking_obs_vencidas_todo_inspeccion.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo_inspeccion[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingGerenciaObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionRankingGerenciaObsVencidasMAP(labels, data) {
    graph_ranking_obs_vencidas_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_obs_vencidas_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionRankingGerenciaObsVencidasSSO(labels, data) {
    graph_ranking_obs_vencidas_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_obs_vencidas_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionRankingGerenciaObsVencidasTodo(labels, data) {
    graph_ranking_obs_vencidas_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_obs_vencidas_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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

//Ranking De Personas que Ingresaron Inspecciones
//Variables gráficos chart_inspeccion_ranking_usuarios_ingreso
var data_ranking_usuarios_ingreso_map_inspeccion = [];
var graph_ranking_usuarios_ingreso_map_inspeccion;

var data_ranking_usuarios_ingreso_sso_inspeccion = [];
var graph_ranking_usuarios_ingreso_sso_inspeccion;

var data_ranking_usuarios_ingreso_todo_inspeccion = [];
var graph_ranking_usuarios_ingreso_todo_inspeccion;
//Para gráficos chart_inspeccion_ranking_usuarios_usuarios_ingreso
function ActualizarInspeccionDatosGraficoRankingUsuarioIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/RankingUsuariosIngresoInspecciones",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_ingreso_map_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_map_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_map_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_map_inspeccion.data.labels = labels;
                graph_ranking_usuarios_ingreso_map_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_map_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_map_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_ingreso_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_sso_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_sso_inspeccion.data.labels = labels;
                graph_ranking_usuarios_ingreso_sso_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_sso_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_ingreso_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_todo_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_todo_inspeccion.data.labels = labels;
                graph_ranking_usuarios_ingreso_todo_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_todo_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionRankingUsuarioIngresoInspMAP(labels, data) {
    graph_ranking_usuarios_ingreso_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_ingreso_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionRankingUsuarioIngresoInspSSO(labels, data) {
    graph_ranking_usuarios_ingreso_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_ingreso_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionRankingUsuarioIngresoInspTodo(labels, data) {
    graph_ranking_usuarios_ingreso_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_ingreso_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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

//Ranking De Gerencias que Tiene Observaciones Vencidas
//Variables gráficos chart_inspeccion_ranking_usuarios_obs_vencidas
var data_ranking_usuarios_obs_vencidas_map_inspeccion = [];
var graph_ranking_usuarios_obs_vencidas_map_inspeccion;

var data_ranking_usuarios_obs_vencidas_sso_inspeccion = [];
var graph_ranking_usuarios_obs_vencidas_sso_inspeccion;

var data_ranking_usuarios_obs_vencidas_todo_inspeccion = [];
var graph_ranking_usuarios_obs_vencidas_todo_inspeccion;
//Para gráficos chart_inspeccion_ranking_usuarios_ingreso
function ActualizarInspeccionDatosGraficoRankingUsuarioObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardInspecciones/RankingUsuariosObsVencidas",
        data: {
            "IdGerenciaInsp": $("#IdGerenciaInsp").val(),
            "IdEmpresaInsp": $("#IdEmpresaInsp").val(),
            "FInicioInsp": $("#FInicioInsp").val(),
            "FFinInsp": $("#FFinInsp").val(),
            "IdGerenciaInspeccionadaInsp": $("#IdGerenciaInspeccionadaInsp").val(),
            "IdAsesorInsp": $("#IdAsesorInsp").val(),
            "IdRespLevInsp": $("#IdRespLevInsp").val(),
            "EstadoInsp": $("#EstadoInsp").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_obs_vencidas_map_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_map_inspeccion.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_map_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_map_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map_inspeccion = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_obs_vencidas_sso_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_sso_inspeccion.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_sso_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_sso_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso_inspeccion = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_obs_vencidas_todo_inspeccion.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo_inspeccion[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_todo_inspeccion.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_todo_inspeccion.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_todo_inspeccion.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo_inspeccion = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo_inspeccion.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo_inspeccion[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo_inspeccion[i].valor);
                }
                //Crear gráfica
                CrearGraficoInspeccionRankingUsuarioObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoInspeccionRankingUsuarioObsVencidasMAP(labels, data) {
    graph_ranking_usuarios_obs_vencidas_map_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_obs_vencido_map"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
                    "data": data,
                    "backgroundColor":'#2e6793',
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
function CrearGraficoInspeccionRankingUsuarioObsVencidasSSO(labels, data) {
    graph_ranking_usuarios_obs_vencidas_sso_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_obs_vencido_sso"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
function CrearGraficoInspeccionRankingUsuarioObsVencidasTodo(labels, data) {
    graph_ranking_usuarios_obs_vencidas_todo_inspeccion = new Chart(
        document.getElementById("chart_inspeccion_ranking_usuarios_obs_vencido_todo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Registros Encontrados",
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
