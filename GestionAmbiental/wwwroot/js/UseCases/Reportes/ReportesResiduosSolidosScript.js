var acr_cmrs = $("#ACR_CMRS").val();
var ingreso_salida = $("#IngresoSalida").val();
var tipo_residuo = $("#TipoResiduo").val();
var motivo = $("#Motivo").val();
var fechaInicio = "";
var fechaFin = "";
var gerencia = $("#Gerencia").val();
var empresa = $("#Empresa").val();
var frecuencia = $("#frecuencia").val();

//Variables gráfico Cantidad X Fecha
var data_cant_fechas_conforme = [];
var data_cant_fechas_anulado = [];
var graph_cant_fechas;
//Variable gráfico Cantidad X ACR_CMRS
var data_acr_vs_cmrs = [];
var graph_acr_vs_cmrs;
//Variable gráfico Cantidad X Ingreso Salida
var data_ing_vs_sal = [];
var graph_ing_vs_sal;
//Variable gráfico tipo resíduo
var data_tipo_residuo = [];
var graph_tipo_residuo;
//Variable gráfico motivo
var data_motivo = [];
var graph_motivo;
//Variable gráfico motivo
var data_rs_no_peligroso = [];
var graph_rs_no_peligroso;
//Variable gráfico motivo
var data_rs_peligroso = [];
var graph_rs_peligroso;

$(document).ready(function () {
    Filtrar();
});
function Filtrar() {

    acr_cmrs = $("#ACR_CMRS").val();
    ingreso_salida = $("#IngresoSalida").val();
    tipo_residuo = $("#TipoResiduo").val();
    motivo = $("#Motivo").val();
    fechaInicio = $("#FInicio").val();
    fechaFin = $("#FFin").val();
    gerencia = $("#Gerencia").val();
    empresa = $("#Empresa").val();
    frecuencia = $("#frecuencia").val();
    if (fechaInicio < fechaFin || (fechaInicio == "" && fechaFin == "")) {
        ActualizarContadorxFecha();
        ActualizarDatosGraficoACR_CMRS();
        ActualizarDatosGraficoIngresoSalida();
        ActualizarDatosGraficoTipoResiduo();
        ActualizarDatosGraficoMotivo();
        ActualizarDatosGraficoResiduoNoPeligroso();
        ActualizarDatosGraficoResiduoPeligroso();
    } else {
        alert("La fecha de fin debe ser mayor a la fecha de inicio");
    }
}
//Para gráfico Contador x Fecha
function ActualizarContadorxFecha() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetRegistrosEntreFechas",
        data: {
            acr_cmrs: $("#ACR_CMRS").val(),
            ingreso_salida: $("#IngresoSalida").val(),
            tipo_residuo: $("#TipoResiduo").val(),
            motivo: $("#Motivo").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            responsable: $("#Gerencia").val(),
            empresa: $("#Empresa").val(),
            frecuencia: $("#frecuencia").val()
        },
        traditional: true,
        success: (response) => {
            console.log(response);
            if (data_cant_fechas_conforme.length > 0 || data_cant_fechas_anulado.length > 0) {
                //Actualizar contador
                data_cant_fechas_conforme = response.nRegistros[0];
                data_cant_fechas_anulado = response.nRegistros[1];

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                for (var i = 0; i < data_cant_fechas_conforme.length; i++) {
                    labels.push(data_cant_fechas_conforme[i].nombre);
                    dataIngreso.push(data_cant_fechas_conforme[i].valor);
                }
                for (var i = 0; i < data_cant_fechas_anulado.length; i++) {
                    dataSalida.push(data_cant_fechas_anulado[i].valor);
                }
                //Actualizar gráfica
                graph_cant_fechas.data.labels = labels;
                graph_cant_fechas.data.datasets[0].data = dataIngreso;
                graph_cant_fechas.data.datasets[1].data = dataSalida;
                graph_cant_fechas.options.scales.yAxes[0].ticks.stepSize = Math.max(dataIngreso) / 10;
                graph_cant_fechas.update();
            } else {
                //Actualizar contador
                data_cant_fechas_conforme = response.nRegistros[0];
                data_cant_fechas_anulado = response.nRegistros[1];

                var labels = [];
                var dataIngreso = [];
                var dataSalida = [];
                for (var i = 0; i < data_cant_fechas_conforme.length; i++) {
                    labels.push(data_cant_fechas_conforme[i].nombre);
                    dataIngreso.push(data_cant_fechas_conforme[i].valor);
                }
                for (var i = 0; i < data_cant_fechas_anulado.length; i++) {
                    dataSalida.push(data_cant_fechas_anulado[i].valor);
                }
                //Crear gráfica
                CrearGraficaContardorxFecha(labels, dataIngreso, dataSalida, Math.max(dataIngreso) / 10);
            }
        }
    });
}
function CrearGraficaContardorxFecha(labels, dataIngreso, dataSalida, stepSize) {
    graph_cant_fechas = new Chart(
        document.getElementById("chart_cant_fechas"),
        {
            "type": "bar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Cantidad de Residuos Sólido Ingresado (kg)",
                        "data": dataIngreso,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
                        "lineTension": 0.1
                    },
                    {
                        "label": "# Cantidad de Residuos Sólido Retirado (kg)",
                        "data": dataSalida,
                        "fill": false,
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
//Para gráfico ACR vs CMRS
function ActualizarDatosGraficoACR_CMRS() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetRegistrosACR_CRMS",
        data: {
            acr_cmrs: $("#ACR_CMRS").val(),
            ingreso_salida: $("#IngresoSalida").val(),
            tipo_residuo: $("#TipoResiduo").val(),
            motivo: $("#Motivo").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            responsable: $("#Gerencia").val(),
            empresa: $("#Empresa").val()
        },
        traditional: true,
        success: (response) => {
            if (data_acr_vs_cmrs.length > 0) {
                //Actualizar contador
                data_acr_vs_cmrs = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acr_vs_cmrs.length; i++) {
                    labels.push(data_acr_vs_cmrs[i].nombre);
                    data.push(data_acr_vs_cmrs[i].valor);
                }
                //Actualizar gráfica
                graph_acr_vs_cmrs.data.labels = labels;
                graph_acr_vs_cmrs.data.datasets[0].data = data;
                graph_acr_vs_cmrs.update();
            } else {
                //Actualizar contador
                data_acr_vs_cmrs = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_acr_vs_cmrs.length; i++) {
                    labels.push(data_acr_vs_cmrs[i].nombre);
                    data.push(data_acr_vs_cmrs[i].valor);
                }
                //Crear gráfica
                CrearGraficoACR_CMRS(labels, data);
            }
        }
    });
}
function CrearGraficoACR_CMRS(labels, data) {
    graph_acr_vs_cmrs = new Chart(
        document.getElementById("chart_acr_cmrs"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad ACR/CMRS (kg)",
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
//Para gráfico Ingreso vs Salida
function ActualizarDatosGraficoIngresoSalida() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetRegistrosIngresoSalida",
        data: {
            acr_cmrs: $("#ACR_CMRS").val(),
            ingreso_salida: $("#IngresoSalida").val(),
            tipo_residuo: $("#TipoResiduo").val(),
            motivo: $("#Motivo").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            responsable: $("#Gerencia").val(),
            empresa: $("#Empresa").val()
        },
        traditional: true,
        success: (response) => {
            
            if (data_ing_vs_sal.length > 0) {
                //Actualizar contador
                data_ing_vs_sal = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ing_vs_sal.length; i++) {
                    labels.push(data_ing_vs_sal[i].nombre);
                    data.push(data_ing_vs_sal[i].valor);
                }
                //Actualizar gráfica
                graph_ing_vs_sal.data.labels = labels;
                graph_ing_vs_sal.data.datasets[0].data = data;
                graph_ing_vs_sal.update();
            } else {
                //Actualizar contador
                data_ing_vs_sal = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_ing_vs_sal.length; i++) {
                    labels.push(data_ing_vs_sal[i].nombre);
                    data.push(data_ing_vs_sal[i].valor);
                }
                //Crear gráfica
                CrearGraficoIngresoSalida(labels, data);
            }
        }
    });
}
function CrearGraficoIngresoSalida(labels, data) {
    graph_ing_vs_sal = new Chart(
        document.getElementById("chart_ing_sal"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad Ingreso/Salida (kg)",
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
//Para gráfico Peligroso vs No Peligroso
function ActualizarDatosGraficoTipoResiduo() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetRegistrosTipoResiduo",
        data: {
            acr_cmrs: $("#ACR_CMRS").val(),
            ingreso_salida: $("#IngresoSalida").val(),
            tipo_residuo: $("#TipoResiduo").val(),
            motivo: $("#Motivo").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            responsable: $("#Gerencia").val(),
            empresa: $("#Empresa").val()
        },
        traditional: true,
        success: (response) => {
            
            if (data_tipo_residuo.length > 0) {
                //Actualizar contador
                data_tipo_residuo = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_residuo.length; i++) {
                    labels.push(data_tipo_residuo[i].nombre);
                    data.push(data_tipo_residuo[i].valor);
                }
                //Actualizar gráfica
                graph_tipo_residuo.data.labels = labels;
                graph_tipo_residuo.data.datasets[0].data = data;
                graph_tipo_residuo.update();
            } else {
                //Actualizar contador
                data_tipo_residuo = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_tipo_residuo.length; i++) {
                    labels.push(data_tipo_residuo[i].nombre);
                    data.push(data_tipo_residuo[i].valor);
                }
                //Crear gráfica
                CrearGraficoTipoResiduo(labels, data);
            }
        }
    });
}
function CrearGraficoTipoResiduo(labels, data) {
    graph_tipo_residuo = new Chart(
        document.getElementById("chart_tipo_residuo"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad Ingreso/Salida (kg)",
                    "data": data,
                    "backgroundColor": [
                        '#FF6363',
                        '#1cc88a'
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
//Para gráfico Motivo
function ActualizarDatosGraficoMotivo() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetRegistrosMotivo",
        data: {
            acr_cmrs: $("#ACR_CMRS").val(),
            ingreso_salida: $("#IngresoSalida").val(),
            tipo_residuo: $("#TipoResiduo").val(),
            motivo: $("#Motivo").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val(),
            responsable: $("#Gerencia").val(),
            empresa: $("#Empresa").val()
        },
        traditional: true,
        success: (response) => {
            
            if (data_motivo.length > 0) {
                //Actualizar contador
                data_motivo = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_motivo.length; i++) {
                    labels.push(data_motivo[i].nombre);
                    data.push(data_motivo[i].valor);
                }
                //Actualizar gráfica
                graph_motivo.data.labels = labels;
                graph_motivo.data.datasets[0].data = data;
                graph_motivo.update();
            } else {
                //Actualizar contador
                data_motivo = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_motivo.length; i++) {
                    labels.push(data_motivo[i].nombre);
                    data.push(data_motivo[i].valor);
                }
                //Crear gráfica
                CrearGraficoMotivo(labels, data);
            }
        }
    });
}
function CrearGraficoMotivo(labels, data) {
    graph_motivo = new Chart(
        document.getElementById("chart_motivo"),
        {
            "type": "doughnut",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad Ingreso/Salida (kg)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#1cc88a',
                        '#FF6363',
                        '#EEC408',
                        '#797D7F',
                        '#5202A2',
                        '#A20248',
                        '#FF6A32'
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
//Para gráfico Residuos Solido No Peligroso
function ActualizarDatosGraficoResiduoNoPeligroso() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetCantidadResiduoNoPeligroso",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rs_no_peligroso.length > 0) {
                //Actualizar contador
                data_rs_no_peligroso = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rs_no_peligroso.length; i++) {
                    labels.push(data_rs_no_peligroso[i].nombre);
                    data.push(data_rs_no_peligroso[i].valor);
                }
                //Actualizar gráfica
                graph_rs_no_peligroso.data.labels = labels;
                graph_rs_no_peligroso.data.datasets[0].data = data;
                graph_rs_no_peligroso.update();
            } else {
                //Actualizar contador
                data_rs_no_peligroso = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rs_no_peligroso.length; i++) {
                    labels.push(data_rs_no_peligroso[i].nombre);
                    data.push(data_rs_no_peligroso[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoNoPeligroso(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoNoPeligroso(labels, data) {
    graph_rs_no_peligroso = new Chart(
        document.getElementById("chart_rs_no_peligrosos"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (kg)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793'
                    ],
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
//Para gráfico Residuos Solido Peligroso
function ActualizarDatosGraficoResiduoPeligroso() {
    $.ajax({
        type: "POST",
        url: "/ReportesIngresoSalida/GetCantidadResiduoPeligroso",
        data: {
            observacion: $("#Observacion").val(),
            residuo_solido: $("#Residuo_Solido").val(),
            eors_salida: $("#EO_RS_Salida").val(),
            eors_llegada: $("#EO_RS_Llegada").val(),
            fechaInicio: $("#FInicio").val(),
            fechaFin: $("#FFin").val()
        },
        traditional: true,
        success: (response) => {
            if (data_rs_peligroso.length > 0) {
                //Actualizar contador
                data_rs_peligroso = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rs_peligroso.length; i++) {
                    labels.push(data_rs_peligroso[i].nombre);
                    data.push(data_rs_peligroso[i].valor);
                }
                //Actualizar gráfica
                graph_rs_peligroso.data.labels = labels;
                graph_rs_peligroso.data.datasets[0].data = data;
                graph_rs_peligroso.update();
            } else {
                //Actualizar contador
                data_rs_peligroso = response.nRegistros;

                var labels = [];
                var data = [];
                for (var i = 0; i < data_rs_peligroso.length; i++) {
                    labels.push(data_rs_peligroso[i].nombre);
                    data.push(data_rs_peligroso[i].valor);
                }
                //Crear gráfica
                CrearGraficoResiduoPeligroso(labels, data);
            }
        }
    });
}
function CrearGraficoResiduoPeligroso(labels, data) {
    graph_rs_peligroso = new Chart(
        document.getElementById("chart_rs_peligrosos"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [{
                    "label": "# Cantidad (kg)",
                    "data": data,
                    "backgroundColor": [
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793',
                        '#2e6793'
                    ],
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