$(document).ready(function () {
    ActualizarDatosGraficoActoCondicionBP();
    ActualizarDatosGraficoCondicion();
    ActualizarDatosGraficoTopActos();
    ActualizarDatosGraficoTopCondicion();
    ActualizarDatosGraficoRankingGerenciaIngresoInsp();
    ActualizarDatosGraficoRankingGerenciaObsVencidas();
    ActualizarDatosGraficoRankingUsuarioIngresoInsp();
    ActualizarDatosGraficoRankingUsuarioObsVencidas();
});
//Actos Subestándar - Condiciones Subestándar - Buenas Prácticas
//Variables gráficos chart_acto_condicion_bp
var data_acto_condicion_bp_map = [];
var graph_acto_condicion_bp_map;

var data_acto_condicion_bp_sso = [];
var graph_acto_condicion_bp_sso;

var data_acto_condicion_bp_todo = [];
var graph_acto_condicion_bp_todo;
//Para gráficos chart_acto_condicion_bp
function ActualizarDatosGraficoActoCondicionBP() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/InspeccionesActoCondicionBP",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_acto_condicion_bp_map.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map.length; i++) {
                    labels.push(data_acto_condicion_bp_map[i].nombre);
                    data.push(data_acto_condicion_bp_map[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_map.data.labels = labels;
                graph_acto_condicion_bp_map.data.datasets[0].data = data;
                graph_acto_condicion_bp_map.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_map.length; i++) {
                    labels.push(data_acto_condicion_bp_map[i].nombre);
                    data.push(data_acto_condicion_bp_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoActoCondicionBPMAP(labels, data);
            }
            //SSO
            if (data_acto_condicion_bp_sso.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso.length; i++) {
                    labels.push(data_acto_condicion_bp_sso[i].nombre);
                    data.push(data_acto_condicion_bp_sso[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_sso.data.labels = labels;
                graph_acto_condicion_bp_sso.data.datasets[0].data = data;
                graph_acto_condicion_bp_sso.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_sso.length; i++) {
                    labels.push(data_acto_condicion_bp_sso[i].nombre);
                    data.push(data_acto_condicion_bp_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoActoCondicionBPSSO(labels, data);
            }
            //Todo
            if (data_acto_condicion_bp_todo.length > 0) {
                //Actualizar contador
                data_acto_condicion_bp_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo.length; i++) {
                    labels.push(data_acto_condicion_bp_todo[i].nombre);
                    data.push(data_acto_condicion_bp_todo[i].valor);
                }
                //Actualizar gráfica
                graph_acto_condicion_bp_todo.data.labels = labels;
                graph_acto_condicion_bp_todo.data.datasets[0].data = data;
                graph_acto_condicion_bp_todo.update();
            } else {
                //Actualizar contador
                data_acto_condicion_bp_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acto_condicion_bp_todo.length; i++) {
                    labels.push(data_acto_condicion_bp_todo[i].nombre);
                    data.push(data_acto_condicion_bp_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoActoCondicionBPTodo(labels, data);
            }
        }
    });
}
function CrearGraficoActoCondicionBPMAP(labels, data) {
    graph_acto_condicion_bp_map = new Chart(
        document.getElementById("chart_acto_condicion_bp_map"),
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
function CrearGraficoActoCondicionBPSSO(labels, data) {
    graph_acto_condicion_bp_sso = new Chart(
        document.getElementById("chart_acto_condicion_bp_sso"),
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
function CrearGraficoActoCondicionBPTodo(labels, data) {
    graph_acto_condicion_bp_todo = new Chart(
        document.getElementById("chart_acto_condicion_bp_todo"),
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
//Variables gráficos chart_condicion_bp
var data_condicion_map = [];
var graph_condicion_map;

var data_condicion_sso = [];
var graph_condicion_sso;

var data_condicion_todo = [];
var graph_condicion_todo;
//Para gráficos chart_condicion_bp

function ActualizarDatosGraficoCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/InspeccionesEstadosCondicion",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_condicion_map.length > 0) {
                //Actualizar contador
                data_condicion_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map.length; i++) {
                    labels.push(data_condicion_map[i].nombre);
                    data.push(data_condicion_map[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_map.data.labels = labels;
                graph_condicion_map.data.datasets[0].data = data;
                graph_condicion_map.update();
            } else {
                //Actualizar contador
                data_condicion_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_map.length; i++) {
                    labels.push(data_condicion_map[i].nombre);
                    data.push(data_condicion_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoCondicionMAP(labels, data);
            }
            //SSO
            if (data_condicion_sso.length > 0) {
                //Actualizar contador
                data_condicion_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso.length; i++) {
                    labels.push(data_condicion_sso[i].nombre);
                    data.push(data_condicion_sso[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_sso.data.labels = labels;
                graph_condicion_sso.data.datasets[0].data = data;
                graph_condicion_sso.update();
            } else {
                //Actualizar contador
                data_condicion_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_sso.length; i++) {
                    labels.push(data_condicion_sso[i].nombre);
                    data.push(data_condicion_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoCondicionSSO(labels, data);
            }
            //Todo
            if (data_condicion_todo.length > 0) {
                //Actualizar contador
                data_condicion_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo.length; i++) {
                    labels.push(data_condicion_todo[i].nombre);
                    data.push(data_condicion_todo[i].valor);
                }
                //Actualizar gráfica
                graph_condicion_todo.data.labels = labels;
                graph_condicion_todo.data.datasets[0].data = data;
                graph_condicion_todo.update();
            } else {
                //Actualizar contador
                data_condicion_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_condicion_todo.length; i++) {
                    labels.push(data_condicion_todo[i].nombre);
                    data.push(data_condicion_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoCondicionMAP(labels, data) {
    graph_condicion_map = new Chart(
        document.getElementById("chart_condicion_map"),
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
function CrearGraficoCondicionSSO(labels, data) {
    graph_condicion_sso = new Chart(
        document.getElementById("chart_condicion_sso"),
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
function CrearGraficoCondicionTodo(labels, data) {
    graph_condicion_todo = new Chart(
        document.getElementById("chart_condicion_todo"),
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
//Variables gráficos chart_top_actos
var data_top_actos_map = [];
var graph_top_actos_map;

var data_top_actos_sso = [];
var graph_top_actos_sso;

var data_top_actos_todo = [];
var graph_top_actos_todo;
//Para gráficos chart_top_actos
function ActualizarDatosGraficoTopActos() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/TopActosSubestandar",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_actos_map.length > 0) {
                //Actualizar contador
                data_top_actos_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map.length; i++) {
                    labels.push(data_top_actos_map[i].nombre);
                    data.push(data_top_actos_map[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_map.data.labels = labels;
                graph_top_actos_map.data.datasets[0].data = data;
                graph_top_actos_map.update();
            } else {
                //Actualizar contador
                data_top_actos_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_map.length; i++) {
                    labels.push(data_top_actos_map[i].nombre);
                    data.push(data_top_actos_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopActosMAP(labels, data);
            }
            //SSO
            if (data_top_actos_sso.length > 0) {
                //Actualizar contador
                data_top_actos_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso.length; i++) {
                    labels.push(data_top_actos_sso[i].nombre);
                    data.push(data_top_actos_sso[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_sso.data.labels = labels;
                graph_top_actos_sso.data.datasets[0].data = data;
                graph_top_actos_sso.update();
            } else {
                //Actualizar contador
                data_top_actos_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_sso.length; i++) {
                    labels.push(data_top_actos_sso[i].nombre);
                    data.push(data_top_actos_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopActosSSO(labels, data);
            }
            //Todo
            if (data_top_actos_todo.length > 0) {
                //Actualizar contador
                data_top_actos_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo.length; i++) {
                    labels.push(data_top_actos_todo[i].nombre);
                    data.push(data_top_actos_todo[i].valor);
                }
                //Actualizar gráfica
                graph_top_actos_todo.data.labels = labels;
                graph_top_actos_todo.data.datasets[0].data = data;
                graph_top_actos_todo.update();
            } else {
                //Actualizar contador
                data_top_actos_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_actos_todo.length; i++) {
                    labels.push(data_top_actos_todo[i].nombre);
                    data.push(data_top_actos_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopActosTodo(labels, data);
            }
        }
    });
}
function CrearGraficoTopActosMAP(labels, data) {
    graph_top_actos_map = new Chart(
        document.getElementById("chart_top_actos_map"),
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
function CrearGraficoTopActosSSO(labels, data) {
    graph_top_actos_sso = new Chart(
        document.getElementById("chart_top_actos_sso"),
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
function CrearGraficoTopActosTodo(labels, data) {
    graph_top_actos_todo = new Chart(
        document.getElementById("chart_top_actos_todo"),
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
//Variables gráficos chart_top_condicion
var data_top_condicion_map = [];
var graph_top_condicion_map;

var data_top_condicion_sso = [];
var graph_top_condicion_sso;

var data_top_condicion_todo = [];
var graph_top_condicion_todo;
//Para gráficos chart_top_condicion
function ActualizarDatosGraficoTopCondicion() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/TopCondicionesSubestandar",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_top_condicion_map.length > 0) {
                //Actualizar contador
                data_top_condicion_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map.length; i++) {
                    labels.push(data_top_condicion_map[i].nombre);
                    data.push(data_top_condicion_map[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_map.data.labels = labels;
                graph_top_condicion_map.data.datasets[0].data = data;
                graph_top_condicion_map.update();
            } else {
                //Actualizar contador
                data_top_condicion_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_map.length; i++) {
                    labels.push(data_top_condicion_map[i].nombre);
                    data.push(data_top_condicion_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopCondicionMAP(labels, data);
            }
            //SSO
            if (data_top_condicion_sso.length > 0) {
                //Actualizar contador
                data_top_condicion_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso.length; i++) {
                    labels.push(data_top_condicion_sso[i].nombre);
                    data.push(data_top_condicion_sso[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_sso.data.labels = labels;
                graph_top_condicion_sso.data.datasets[0].data = data;
                graph_top_condicion_sso.update();
            } else {
                //Actualizar contador
                data_top_condicion_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_sso.length; i++) {
                    labels.push(data_top_condicion_sso[i].nombre);
                    data.push(data_top_condicion_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopCondicionSSO(labels, data);
            }
            //Todo
            if (data_top_condicion_todo.length > 0) {
                //Actualizar contador
                data_top_condicion_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo.length; i++) {
                    labels.push(data_top_condicion_todo[i].nombre);
                    data.push(data_top_condicion_todo[i].valor);
                }
                //Actualizar gráfica
                graph_top_condicion_todo.data.labels = labels;
                graph_top_condicion_todo.data.datasets[0].data = data;
                graph_top_condicion_todo.update();
            } else {
                //Actualizar contador
                data_top_condicion_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_top_condicion_todo.length; i++) {
                    labels.push(data_top_condicion_todo[i].nombre);
                    data.push(data_top_condicion_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoTopCondicionTodo(labels, data);
            }
        }
    });
}
function CrearGraficoTopCondicionMAP(labels, data) {
    graph_top_condicion_map = new Chart(
        document.getElementById("chart_top_condicion_map"),
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
function CrearGraficoTopCondicionSSO(labels, data) {
    graph_top_condicion_sso = new Chart(
        document.getElementById("chart_top_condicion_sso"),
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
function CrearGraficoTopCondicionTodo(labels, data) {
    graph_top_condicion_todo = new Chart(
        document.getElementById("chart_top_condicion_todo"),
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
//Variables gráficos chart_ranking_ingreso_inspecciones
var data_ranking_ingreso_inspecciones_map = [];
var graph_ranking_ingreso_inspecciones_map;

var data_ranking_ingreso_inspecciones_sso = [];
var graph_ranking_ingreso_inspecciones_sso;

var data_ranking_ingreso_inspecciones_todo = [];
var graph_ranking_ingreso_inspecciones_todo;
//Para gráficos chart_ranking_ingreso_inspecciones
function ActualizarDatosGraficoRankingGerenciaIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/RankingGerenciaReportante",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_ingreso_inspecciones_map.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_map.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_map[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_map[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_inspecciones_map.data.labels = labels;
                graph_ranking_ingreso_inspecciones_map.data.datasets[0].data = data;
                graph_ranking_ingreso_inspecciones_map.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_map.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_map[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_ingreso_inspecciones_sso.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_sso.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_sso[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_sso[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_inspecciones_sso.data.labels = labels;
                graph_ranking_ingreso_inspecciones_sso.data.datasets[0].data = data;
                graph_ranking_ingreso_inspecciones_sso.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_sso.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_sso[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_ingreso_inspecciones_todo.length > 0) {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_todo.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_todo[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_todo[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_ingreso_inspecciones_todo.data.labels = labels;
                graph_ranking_ingreso_inspecciones_todo.data.datasets[0].data = data;
                graph_ranking_ingreso_inspecciones_todo.update();
            } else {
                //Actualizar contador
                data_ranking_ingreso_inspecciones_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_ingreso_inspecciones_todo.length; i++) {
                    labels.push(data_ranking_ingreso_inspecciones_todo[i].nombre);
                    data.push(data_ranking_ingreso_inspecciones_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRankingGerenciaIngresoInspMAP(labels, data) {
    graph_ranking_ingreso_inspecciones_map = new Chart(
        document.getElementById("chart_ranking_ingreso_inspecciones_map"),
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
function CrearGraficoRankingGerenciaIngresoInspSSO(labels, data) {
    graph_ranking_ingreso_inspecciones_sso = new Chart(
        document.getElementById("chart_ranking_ingreso_inspecciones_sso"),
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
function CrearGraficoRankingGerenciaIngresoInspTodo(labels, data) {
    graph_ranking_ingreso_inspecciones_todo = new Chart(
        document.getElementById("chart_ranking_ingreso_inspecciones_todo"),
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
//Variables gráficos chart_ranking_obs_vencidas
var data_ranking_obs_vencidas_map = [];
var graph_ranking_obs_vencidas_map;

var data_ranking_obs_vencidas_sso = [];
var graph_ranking_obs_vencidas_sso;

var data_ranking_obs_vencidas_todo = [];
var graph_ranking_obs_vencidas_todo;
//Para gráficos chart_ranking_ingreso_inspecciones
function ActualizarDatosGraficoRankingGerenciaObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/RankingGerenciaObsVencidas",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_obs_vencidas_map.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map[i].nombre);
                    data.push(data_ranking_obs_vencidas_map[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_map.data.labels = labels;
                graph_ranking_obs_vencidas_map.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_map.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_map.length; i++) {
                    labels.push(data_ranking_obs_vencidas_map[i].nombre);
                    data.push(data_ranking_obs_vencidas_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_obs_vencidas_sso.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_sso.data.labels = labels;
                graph_ranking_obs_vencidas_sso.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_sso.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_sso.length; i++) {
                    labels.push(data_ranking_obs_vencidas_sso[i].nombre);
                    data.push(data_ranking_obs_vencidas_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_obs_vencidas_todo.length > 0) {
                //Actualizar contador
                data_ranking_obs_vencidas_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_obs_vencidas_todo.data.labels = labels;
                graph_ranking_obs_vencidas_todo.data.datasets[0].data = data;
                graph_ranking_obs_vencidas_todo.update();
            } else {
                //Actualizar contador
                data_ranking_obs_vencidas_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_obs_vencidas_todo.length; i++) {
                    labels.push(data_ranking_obs_vencidas_todo[i].nombre);
                    data.push(data_ranking_obs_vencidas_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingGerenciaObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRankingGerenciaObsVencidasMAP(labels, data) {
    graph_ranking_obs_vencidas_map = new Chart(
        document.getElementById("chart_ranking_obs_vencidas_map"),
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
function CrearGraficoRankingGerenciaObsVencidasSSO(labels, data) {
    graph_ranking_obs_vencidas_sso = new Chart(
        document.getElementById("chart_ranking_obs_vencidas_sso"),
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
function CrearGraficoRankingGerenciaObsVencidasTodo(labels, data) {
    graph_ranking_obs_vencidas_todo = new Chart(
        document.getElementById("chart_ranking_obs_vencidas_todo"),
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
//Variables gráficos chart_ranking_usuarios_ingreso
var data_ranking_usuarios_ingreso_inspecciones_map = [];
var graph_ranking_usuarios_ingreso_inspecciones_map;

var data_ranking_usuarios_ingreso_inspecciones_sso = [];
var graph_ranking_usuarios_ingreso_inspecciones_sso;

var data_ranking_usuarios_ingreso_inspecciones_todo = [];
var graph_ranking_usuarios_ingreso_inspecciones_todo;
//Para gráficos chart_ranking_usuarios_usuarios_ingreso
function ActualizarDatosGraficoRankingUsuarioIngresoInsp() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/RankingUsuariosIngresoInspecciones",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_ingreso_inspecciones_map.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_map.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_map[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_map[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_inspecciones_map.data.labels = labels;
                graph_ranking_usuarios_ingreso_inspecciones_map.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_inspecciones_map.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_map.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_map[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioIngresoInspMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_ingreso_inspecciones_sso.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_sso.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_sso[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_sso[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_inspecciones_sso.data.labels = labels;
                graph_ranking_usuarios_ingreso_inspecciones_sso.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_inspecciones_sso.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_sso.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_sso[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioIngresoInspSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_ingreso_inspecciones_todo.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_todo.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_todo[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_todo[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_ingreso_inspecciones_todo.data.labels = labels;
                graph_ranking_usuarios_ingreso_inspecciones_todo.data.datasets[0].data = data;
                graph_ranking_usuarios_ingreso_inspecciones_todo.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_ingreso_inspecciones_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_ingreso_inspecciones_todo.length; i++) {
                    labels.push(data_ranking_usuarios_ingreso_inspecciones_todo[i].nombre);
                    data.push(data_ranking_usuarios_ingreso_inspecciones_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioIngresoInspTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRankingUsuarioIngresoInspMAP(labels, data) {
    graph_ranking_usuarios_ingreso_inspecciones_map = new Chart(
        document.getElementById("chart_ranking_usuarios_ingreso_map"),
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
function CrearGraficoRankingUsuarioIngresoInspSSO(labels, data) {
    graph_ranking_usuarios_ingreso_inspecciones_sso = new Chart(
        document.getElementById("chart_ranking_usuarios_ingreso_sso"),
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
function CrearGraficoRankingUsuarioIngresoInspTodo(labels, data) {
    graph_ranking_usuarios_ingreso_inspecciones_todo = new Chart(
        document.getElementById("chart_ranking_usuarios_ingreso_todo"),
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
//Variables gráficos chart_ranking_usuarios_obs_vencidas
var data_ranking_usuarios_obs_vencidas_map = [];
var graph_ranking_usuarios_obs_vencidas_map;

var data_ranking_usuarios_obs_vencidas_sso = [];
var graph_ranking_usuarios_obs_vencidas_sso;

var data_ranking_usuarios_obs_vencidas_todo = [];
var graph_ranking_usuarios_obs_vencidas_todo;
//Para gráficos chart_ranking_usuarios_ingreso
function ActualizarDatosGraficoRankingUsuarioObsVencidas() {
    $.ajax({
        type: "POST",
        url: "/DashboardGeneral/RankingUsuariosObsVencidas",
        //data: {
        //    "tipo_incidente": $("#TipoIncidente").val(),
        //},
        traditional: true,
        success: (response) => {
            //MAP
            if (data_ranking_usuarios_obs_vencidas_map.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_map.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_map.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_map.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_map = response.data.resultadosMAP;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_map.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_map[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_map[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioObsVencidasMAP(labels, data);
            }
            //SSO
            if (data_ranking_usuarios_obs_vencidas_sso.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_sso.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_sso.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_sso.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_sso = response.data.resultadosSSO;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_sso.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_sso[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_sso[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioObsVencidasSSO(labels, data);
            }
            //Todo
            if (data_ranking_usuarios_obs_vencidas_todo.length > 0) {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo[i].valor);
                }
                //Actualizar gráfica
                graph_ranking_usuarios_obs_vencidas_todo.data.labels = labels;
                graph_ranking_usuarios_obs_vencidas_todo.data.datasets[0].data = data;
                graph_ranking_usuarios_obs_vencidas_todo.update();
            } else {
                //Actualizar contador
                data_ranking_usuarios_obs_vencidas_todo = response.data.resultadosTodos;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ranking_usuarios_obs_vencidas_todo.length; i++) {
                    labels.push(data_ranking_usuarios_obs_vencidas_todo[i].nombre);
                    data.push(data_ranking_usuarios_obs_vencidas_todo[i].valor);
                }
                //Crear gráfica
                CrearGraficoRankingUsuarioObsVencidasTodo(labels, data);
            }
        }
    });
}
function CrearGraficoRankingUsuarioObsVencidasMAP(labels, data) {
    graph_ranking_usuarios_obs_vencidas_map = new Chart(
        document.getElementById("chart_ranking_usuarios_obs_vencido_map"),
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
function CrearGraficoRankingUsuarioObsVencidasSSO(labels, data) {
    graph_ranking_usuarios_obs_vencidas_sso = new Chart(
        document.getElementById("chart_ranking_usuarios_obs_vencido_sso"),
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
function CrearGraficoRankingUsuarioObsVencidasTodo(labels, data) {
    graph_ranking_usuarios_obs_vencidas_todo = new Chart(
        document.getElementById("chart_ranking_usuarios_obs_vencido_todo"),
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
