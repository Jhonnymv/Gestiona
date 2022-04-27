$("#pills-rac-tab").on('click', function () {
    FiltrarRAC();
});
function FiltrarRAC() {
    ActualizarRACDatosGraficoActoCondicionBP();
    ActualizarRACDatosGraficoCondicion();
    ActualizarRACDatosGraficoTopActos();
    ActualizarRACDatosGraficoTopCondicion();
    ActualizarRACDatosGraficoRankingGerenciaIngresoInsp();
    ActualizarRACDatosGraficoRankingGerenciaObsVencidas();
    ActualizarRACDatosGraficoRankingUsuarioIngresoInsp();
    ActualizarRACDatosGraficoRankingUsuarioObsVencidas();
}
//Actos Subestándar - Condiciones Subestándar - Buenas Prácticas
//Variables gráficos chart_rac_acto_condicion_bp
var data_acto_condicion_bp_map_rac = [];
var graph_acto_condicion_bp_map_rac;

var data_acto_condicion_bp_sso_rac = [];
var graph_acto_condicion_bp_sso_rac;

var data_acto_condicion_bp_todo_rac = [];
var graph_acto_condicion_bp_todo_rac;
//Para gráficos chart_rac_acto_condicion_bp
function ActualizarRACDatosGraficoActoCondicionBP() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RACActoCondicionBP",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_acto_condicion_bp_map_rac.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_map_rac[i].nombre);
                    data.push(data_acto_condicion_bp_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_map_rac.data.labels = labels;
                graph_acto_condicion_bp_map_rac.data.datasets[0].data = data;
                graph_acto_condicion_bp_map_rac.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_map_rac[i].nombre);
                    data.push(data_acto_condicion_bp_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACActoCondicionBPMAP(labels, data);
            }
            //SSO
            if (data_acto_condicion_bp_sso_rac.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_sso_rac[i].nombre);
                    data.push(data_acto_condicion_bp_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_sso_rac.data.labels = labels;
                graph_acto_condicion_bp_sso_rac.data.datasets[0].data = data;
                graph_acto_condicion_bp_sso_rac.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_sso_rac[i].nombre);
                    data.push(data_acto_condicion_bp_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACActoCondicionBPSSO(labels, data);
            }
            //Todo
            if (data_acto_condicion_bp_todo_rac.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_todo_rac[i].nombre);
                    data.push(data_acto_condicion_bp_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_todo_rac.data.labels = labels;
                graph_acto_condicion_bp_todo_rac.data.datasets[0].data = data;
                graph_acto_condicion_bp_todo_rac.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo_rac.length; i++) {
                    labels.push(data_acto_condicion_bp_todo_rac[i].nombre);
                    data.push(data_acto_condicion_bp_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACActoCondicionBPTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACActoCondicionBPMAP(labels, data) {
    graph_acto_condicion_bp_map_rac = new Chart(
        document.getElementById("chart_rac_acto_condicion_bp_map"),
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
function CrearGraficoRACActoCondicionBPSSO(labels, data) {
    graph_acto_condicion_bp_sso_rac = new Chart(
        document.getElementById("chart_rac_acto_condicion_bp_sso"),
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
function CrearGraficoRACActoCondicionBPTodo(labels, data) {
    graph_acto_condicion_bp_todo_rac = new Chart(
        document.getElementById("chart_rac_acto_condicion_bp_todo"),
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
//Variables gráficos chart_rac_condicion_bp
var data_condicion_map_rac = [];
var graph_condicion_map_rac;

var data_condicion_sso_rac = [];
var graph_condicion_sso_rac;

var data_condicion_todo_rac = [];
var graph_condicion_todo_rac;
//Para gráficos chart_rac_condicion_bp

function ActualizarRACDatosGraficoCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RACEstadosCondicion",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_condicion_map_rac.length > 0) {
                //Actualizar contador
                data_condicion_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map_rac.length; i++) {
                    labels.push(data_condicion_map_rac[i].nombre);
                    data.push(data_condicion_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_map_rac.data.labels = labels;
                graph_condicion_map_rac.data.datasets[0].data = data;
                graph_condicion_map_rac.update();
            } else {
                //Actualizar contador
                data_condicion_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map_rac.length; i++) {
                    labels.push(data_condicion_map_rac[i].nombre);
                    data.push(data_condicion_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACCondicionMAP(labels, data);
            }
            //SSO
            if (data_condicion_sso_rac.length > 0) {
                //Actualizar contador
                data_condicion_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso_rac.length; i++) {
                    labels.push(data_condicion_sso_rac[i].nombre);
                    data.push(data_condicion_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_sso_rac.data.labels = labels;
                graph_condicion_sso_rac.data.datasets[0].data = data;
                graph_condicion_sso_rac.update();
            } else {
                //Actualizar contador
                data_condicion_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso_rac.length; i++) {
                    labels.push(data_condicion_sso_rac[i].nombre);
                    data.push(data_condicion_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACCondicionSSO(labels, data);
            }
            //Todo
            if (data_condicion_todo_rac.length > 0) {
                //Actualizar contador
                data_condicion_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo_rac.length; i++) {
                    labels.push(data_condicion_todo_rac[i].nombre);
                    data.push(data_condicion_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_todo_rac.data.labels = labels;
                graph_condicion_todo_rac.data.datasets[0].data = data;
                graph_condicion_todo_rac.update();
            } else {
                //Actualizar contador
                data_condicion_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo_rac.length; i++) {
                    labels.push(data_condicion_todo_rac[i].nombre);
                    data.push(data_condicion_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACCondicionMAP(labels, data) {
    graph_condicion_map_rac = new Chart(
        document.getElementById("chart_rac_condicion_map"),
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
function CrearGraficoRACCondicionSSO(labels, data) {
    graph_condicion_sso_rac = new Chart(
        document.getElementById("chart_rac_condicion_sso"),
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
function CrearGraficoRACCondicionTodo(labels, data) {
    graph_condicion_todo_rac = new Chart(
        document.getElementById("chart_rac_condicion_todo"),
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
//Variables gráficos chart_rac_top_actos
var data_top_actos_map_rac = [];
var graph_top_actos_map_rac;

var data_top_actos_sso_rac = [];
var graph_top_actos_sso_rac;

var data_top_actos_todo_rac = [];
var graph_top_actos_todo_rac;
//Para gráficos chart_rac_top_actos
function ActualizarRACDatosGraficoTopActos() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/TopActosSubestandar",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_actos_map_rac.length > 0) {
                //Actualizar contador
                data_top_actos_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map_rac.length; i++) {
                    labels.push(data_top_actos_map_rac[i].nombre);
                    data.push(data_top_actos_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_map_rac.data.labels = labels;
                graph_top_actos_map_rac.data.datasets[0].data = data;
                graph_top_actos_map_rac.update();
            } else {
                //Actualizar contador
                data_top_actos_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map_rac.length; i++) {
                    labels.push(data_top_actos_map_rac[i].nombre);
                    data.push(data_top_actos_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopActosMAP(labels, data);
            }
            //SSO
            if (data_top_actos_sso_rac.length > 0) {
                //Actualizar contador
                data_top_actos_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso_rac.length; i++) {
                    labels.push(data_top_actos_sso_rac[i].nombre);
                    data.push(data_top_actos_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_sso_rac.data.labels = labels;
                graph_top_actos_sso_rac.data.datasets[0].data = data;
                graph_top_actos_sso_rac.update();
            } else {
                //Actualizar contador
                data_top_actos_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso_rac.length; i++) {
                    labels.push(data_top_actos_sso_rac[i].nombre);
                    data.push(data_top_actos_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopActosSSO(labels, data);
            }
            //Todo
            if (data_top_actos_todo_rac.length > 0) {
                //Actualizar contador
                data_top_actos_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo_rac.length; i++) {
                    labels.push(data_top_actos_todo_rac[i].nombre);
                    data.push(data_top_actos_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_todo_rac.data.labels = labels;
                graph_top_actos_todo_rac.data.datasets[0].data = data;
                graph_top_actos_todo_rac.update();
            } else {
                //Actualizar contador
                data_top_actos_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo_rac.length; i++) {
                    labels.push(data_top_actos_todo_rac[i].nombre);
                    data.push(data_top_actos_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopActosTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACTopActosMAP(labels, data) {
    graph_top_actos_map_rac = new Chart(
        document.getElementById("chart_rac_top_actos_map"),
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
function CrearGraficoRACTopActosSSO(labels, data) {
    graph_top_actos_sso_rac = new Chart(
        document.getElementById("chart_rac_top_actos_sso"),
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
function CrearGraficoRACTopActosTodo(labels, data) {
    graph_top_actos_todo_rac = new Chart(
        document.getElementById("chart_rac_top_actos_todo"),
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
//Variables gráficos chart_rac_top_condicion
var data_top_condicion_map_rac = [];
var graph_top_condicion_map_rac;

var data_top_condicion_sso_rac = [];
var graph_top_condicion_sso_rac;

var data_top_condicion_todo_rac = [];
var graph_top_condicion_todo_rac;
//Para gráficos chart_rac_top_condicion
function ActualizarRACDatosGraficoTopCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/TopCondicionesSubestandar",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_condicion_map_rac.length > 0) {
                //Actualizar contador
                data_top_condicion_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map_rac.length; i++) {
                    labels.push(data_top_condicion_map_rac[i].nombre);
                    data.push(data_top_condicion_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_map_rac.data.labels = labels;
                graph_top_condicion_map_rac.data.datasets[0].data = data;
                graph_top_condicion_map_rac.update();
            } else {
                //Actualizar contador
                data_top_condicion_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map_rac.length; i++) {
                    labels.push(data_top_condicion_map_rac[i].nombre);
                    data.push(data_top_condicion_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopCondicionMAP(labels, data);
            }
            //SSO
            if (data_top_condicion_sso_rac.length > 0) {
                //Actualizar contador
                data_top_condicion_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso_rac.length; i++) {
                    labels.push(data_top_condicion_sso_rac[i].nombre);
                    data.push(data_top_condicion_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_sso_rac.data.labels = labels;
                graph_top_condicion_sso_rac.data.datasets[0].data = data;
                graph_top_condicion_sso_rac.update();
            } else {
                //Actualizar contador
                data_top_condicion_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso_rac.length; i++) {
                    labels.push(data_top_condicion_sso_rac[i].nombre);
                    data.push(data_top_condicion_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopCondicionSSO(labels, data);
            }
            //Todo
            if (data_top_condicion_todo_rac.length > 0) {
                //Actualizar contador
                data_top_condicion_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo_rac.length; i++) {
                    labels.push(data_top_condicion_todo_rac[i].nombre);
                    data.push(data_top_condicion_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_todo_rac.data.labels = labels;
                graph_top_condicion_todo_rac.data.datasets[0].data = data;
                graph_top_condicion_todo_rac.update();
            } else {
                //Actualizar contador
                data_top_condicion_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo_rac.length; i++) {
                    labels.push(data_top_condicion_todo_rac[i].nombre);
                    data.push(data_top_condicion_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACTopCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACTopCondicionMAP(labels, data) {
    graph_top_condicion_map_rac = new Chart(
        document.getElementById("chart_rac_top_condicion_map"),
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
function CrearGraficoRACTopCondicionSSO(labels, data) {
    graph_top_condicion_sso_rac = new Chart(
        document.getElementById("chart_rac_top_condicion_sso"),
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
function CrearGraficoRACTopCondicionTodo(labels, data) {
    graph_top_condicion_todo_rac = new Chart(
        document.getElementById("chart_rac_top_condicion_todo"),
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

//Ranking De Gerencias que Ingresaron RAC
//Variables gráficos chart_rac_ranking_ingreso_races
var data_ranking_ingreso_map_rac = [];
var graph_ranking_ingreso_map_rac;

var data_ranking_ingreso_sso_rac = [];
var graph_ranking_ingreso_sso_rac;

var data_ranking_ingreso_todo_rac = [];
var graph_ranking_ingreso_todo_rac;
//Para gráficos chart_rac_ranking_ingreso_races
function ActualizarRACDatosGraficoRankingGerenciaIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RankingGerenciaReportante",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_ingreso_map_rac.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_map_rac.length; i++) {
                    labels.push(data_ranking_ingreso_map_rac[i].nombre);
                    data.push(data_ranking_ingreso_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_map_rac.data.labels = labels;
                graph_ranking_ingreso_map_rac.data.datasets[0].data = data;
                graph_ranking_ingreso_map_rac.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_map_rac.length; i++) {
                    labels.push(data_ranking_ingreso_map_rac[i].nombre);
                    data.push(data_ranking_ingreso_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_ingreso_sso_rac.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_sso_rac.length; i++) {
                    labels.push(data_ranking_ingreso_sso_rac[i].nombre);
                    data.push(data_ranking_ingreso_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_sso_rac.data.labels = labels;
                graph_ranking_ingreso_sso_rac.data.datasets[0].data = data;
                graph_ranking_ingreso_sso_rac.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_sso_rac.length; i++) {
                    labels.push(data_ranking_ingreso_sso_rac[i].nombre);
                    data.push(data_ranking_ingreso_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_ingreso_todo_rac.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_todo_rac.length; i++) {
                    labels.push(data_ranking_ingreso_todo_rac[i].nombre);
                    data.push(data_ranking_ingreso_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_todo_rac.data.labels = labels;
                graph_ranking_ingreso_todo_rac.data.datasets[0].data = data;
                graph_ranking_ingreso_todo_rac.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_todo_rac.length; i++) {
                    labels.push(data_ranking_ingreso_todo_rac[i].nombre);
                    data.push(data_ranking_ingreso_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACRankingGerenciaIngresoInspMAP(labels, data) {
    graph_ranking_ingreso_map_rac = new Chart(
        document.getElementById("chart_rac_ranking_ingreso_rac_map"),
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
function CrearGraficoRACRankingGerenciaIngresoInspSSO(labels, data) {
    graph_ranking_ingreso_sso_rac = new Chart(
        document.getElementById("chart_rac_ranking_ingreso_rac_sso"),
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
function CrearGraficoRACRankingGerenciaIngresoInspTodo(labels, data) {
    graph_ranking_ingreso_todo_rac = new Chart(
        document.getElementById("chart_rac_ranking_ingreso_rac_todo"),
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
//Variables gráficos chart_rac_ranking_obs_vencidas
var data_ranking_obs_vencidas_map_rac = [];
var graph_ranking_obs_vencidas_map_rac;

var data_ranking_obs_vencidas_sso_rac = [];
var graph_ranking_obs_vencidas_sso_rac;

var data_ranking_obs_vencidas_todo_rac = [];
var graph_ranking_obs_vencidas_todo_rac;
//Para gráficos chart_rac_ranking_ingreso_races
function ActualizarRACDatosGraficoRankingGerenciaObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RankingGerenciaObsVencidas",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_obs_vencidas_map_rac.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_map_rac.data.labels = labels;
                graph_ranking_obs_vencidas_map_rac.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_map_rac.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_obs_vencidas_sso_rac.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_sso_rac.data.labels = labels;
                graph_ranking_obs_vencidas_sso_rac.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_sso_rac.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_obs_vencidas_todo_rac.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_todo_rac.data.labels = labels;
                graph_ranking_obs_vencidas_todo_rac.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_todo_rac.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo_rac.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo_rac[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingGerenciaObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACRankingGerenciaObsVencidasMAP(labels, data) {
    graph_ranking_obs_vencidas_map_rac = new Chart(
        document.getElementById("chart_rac_ranking_obs_vencidas_map"),
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
function CrearGraficoRACRankingGerenciaObsVencidasSSO(labels, data) {
    graph_ranking_obs_vencidas_sso_rac = new Chart(
        document.getElementById("chart_rac_ranking_obs_vencidas_sso"),
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
function CrearGraficoRACRankingGerenciaObsVencidasTodo(labels, data) {
    graph_ranking_obs_vencidas_todo_rac = new Chart(
        document.getElementById("chart_rac_ranking_obs_vencidas_todo"),
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

//Ranking De Personas que Ingresaron RAC
//Variables gráficos chart_rac_ranking_usuarios_ingreso
var data_ranking_usuarios_ingreso_map_rac = [];
var graph_ranking_usuarios_ingreso_map_rac;

var data_ranking_usuarios_ingreso_sso_rac = [];
var graph_ranking_usuarios_ingreso_sso_rac;

var data_ranking_usuarios_ingreso_todo_rac = [];
var graph_ranking_usuarios_ingreso_todo_rac;
//Para gráficos chart_rac_ranking_usuarios_usuarios_ingreso
function ActualizarRACDatosGraficoRankingUsuarioIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RankingUsuariosIngresoRAC",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_ingreso_map_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_map_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_map_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_map_rac.data.labels = labels;
                graph_ranking_usuarios_ingreso_map_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_map_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_map_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_map_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_ingreso_sso_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_sso_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_sso_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_sso_rac.data.labels = labels;
                graph_ranking_usuarios_ingreso_sso_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_sso_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_sso_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_sso_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_ingreso_todo_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_todo_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_todo_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_todo_rac.data.labels = labels;
                graph_ranking_usuarios_ingreso_todo_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_todo_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_todo_rac.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_todo_rac[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACRankingUsuarioIngresoInspMAP(labels, data) {
    graph_ranking_usuarios_ingreso_map_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_ingreso_map"),
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
function CrearGraficoRACRankingUsuarioIngresoInspSSO(labels, data) {
    graph_ranking_usuarios_ingreso_sso_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_ingreso_sso"),
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
function CrearGraficoRACRankingUsuarioIngresoInspTodo(labels, data) {
    graph_ranking_usuarios_ingreso_todo_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_ingreso_todo"),
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
//Variables gráficos chart_rac_ranking_usuarios_obs_vencidas
var data_ranking_usuarios_obs_vencidas_map_rac = [];
var graph_ranking_usuarios_obs_vencidas_map_rac;

var data_ranking_usuarios_obs_vencidas_sso_rac = [];
var graph_ranking_usuarios_obs_vencidas_sso_rac;

var data_ranking_usuarios_obs_vencidas_todo_rac = [];
var graph_ranking_usuarios_obs_vencidas_todo_rac;
//Para gráficos chart_rac_ranking_usuarios_ingreso
function ActualizarRACDatosGraficoRankingUsuarioObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardRAC/RankingUsuariosObsVencidas",
        data: {
            "IdGerenciaRAC": $("#IdGerenciaRAC").val(),
            "IdEmpresaRAC": $("#IdEmpresaRAC").val(),
            "FInicioRAC": $("#FInicioRAC").val(),
            "FFinRAC": $("#FFinRAC").val(),
            "IdGerenciaInspeccionadaRAC": $("#IdGerenciaInspeccionadaRAC").val(),
            "IdAsesorRAC": $("#IdAsesorRAC").val(),
            "IdRespLevRAC": $("#IdRespLevRAC").val(),
            "EstadoRAC": $("#EstadoRAC").val(),
        },
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_obs_vencidas_map_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_map_rac.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_map_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_map_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map_rac = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_obs_vencidas_sso_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_sso_rac.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_sso_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_sso_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso_rac = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_obs_vencidas_todo_rac.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo_rac[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_todo_rac.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_todo_rac.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_todo_rac.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo_rac = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo_rac.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo_rac[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo_rac[i].valor);
                }
                //Crear gráfica
                CrearGraficoRACRankingUsuarioObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRACRankingUsuarioObsVencidasMAP(labels, data) {
    graph_ranking_usuarios_obs_vencidas_map_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_obs_vencido_map"),
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
function CrearGraficoRACRankingUsuarioObsVencidasSSO(labels, data) {
    graph_ranking_usuarios_obs_vencidas_sso_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_obs_vencido_sso"),
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
function CrearGraficoRACRankingUsuarioObsVencidasTodo(labels, data) {
    graph_ranking_usuarios_obs_vencidas_todo_rac = new Chart(
        document.getElementById("chart_rac_ranking_usuarios_obs_vencido_todo"),
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
