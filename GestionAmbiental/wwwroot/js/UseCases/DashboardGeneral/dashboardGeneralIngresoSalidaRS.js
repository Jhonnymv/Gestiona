let acr_cmrs = $("#ACR_CMRS_ISRS").val();
let ingreso_salida = $("#IngresoSalida_ISRS").val();
let tipo_residuo = $("#TipoResiduo_ISRS").val();
let motivo = $("#Motivo_ISRS").val();
let fechaInicio = $("#FInicio_ISRS").val();
let fechaFin = $("#FFin_ISRS").val();
let gerencia = $("#Gerencia_ISRS").val();
let empresa = $("#Empresa_ISRS").val();
let rgb_list = [
    {
        'r':45,
        'g':100,
        'b':155
    },
    {
        'r':205,
        'g':55,
        'b':40
    },
    {
        'r':193,
        'g':216,
        'b':47
    },
    {
        'r':0,
        'g':188,
        'b':288
    },
    {
        'r':110,
        'g':41,
        'b':141
    },
]
//variables ingreso y salida según tipo de residuos
let data_ingreso_tipo_residuo = [];
let data_salida_tipo_residuo = [];
let graph_ingreso_salida_tipo_residuo;
//variables salida según tipo de residuos
let graph_salida_tipo_residuo;
//variables destino x salida x TipoResiduo
let data_destino_tipos_residuo_salida =[];
let graph_destino_tipos_residuo_salida;
//Top 10 Gerencias x ingreso
let data_top_gerencia_ingreso =[];
let graph_top_gerencia_ingreso;
//Top 10 Nombre Residuo x ingreso
let data_top_nombre_residuo_ingreso =[];
let graph_top_nombre_residuo_ingreso;
//Top 10 Nombre Residuo x salida
let data_top_nombre_residuo_salida =[];
let graph_top_nombre_residuo_salida;
//variables gerencia x salida x TipoResiduo
let data_gerencia_tipos_residuo_salida =[];
let graph_gerencia_tipos_residuo_salida;

function AjustarLabel(label){
    let palabras = label.split(' ');
    if(palabras.length > 3){
        palabras.splice(3,0, '\n');
        return palabras.join(' ');
    }
    else{
        return label;
    }
}
/*
$(document).ready(function () {
    //ActualizarIngresoSalidaTipoResiduo();
});*/


function FiltrarIngresoSalidaRS() {
    acr_cmrs = $("#ACR_CMRS_ISRS").val();
    ingreso_salida = $("#IngresoSalida_ISRS").val();
    tipo_residuo = $("#TipoResiduo_ISRS").val();
    motivo = $("#Motivo_ISRS").val();
    fechaInicio = $("#FInicio_ISRS").val();
    fechaFin = $("#FFin_ISRS").val();
    gerencia = $("#Gerencia_ISRS").val();
    empresa = $("#Empresa_ISRS").val();
    ActualizarIngresoSalidaTipoResiduo();
    ActualizarDestinoxTipoResiduoxSalida();
    ActualizarTopGerenciasIngresoResiduo();
    ActualizarTopGerenciaxTipoResiduoxSalida();
}
$("#ISRS_NombreResiduo_DTRS, #ISRS_Destino_DTRS").on('change',function(){
    ActualizarDestinoxTipoResiduoxSalida();
});
$("#ISRS_NombreResiduo_GTRI, #ISRS_Gerencia_GTRI").on('change',function(){
    ActualizarTopGerenciaxTipoResiduoxSalida();
});
//Para ingreso y salida según tipo de residuos
function ActualizarIngresoSalidaTipoResiduo() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetIngresoSalidaTipoResiduo",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "empresa": empresa,
        },
        traditional: true,
        success: (response) => {
            if (data_ingreso_tipo_residuo.length > 0 || data_salida_tipo_residuo.length > 0) {
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                let labels = [];
                let dataIngreso = [];
                let dataSalida = [];

                let soloSalida = data_salida_tipo_residuo.filter(data => data.valor > 0).sort((a, b) => (a.valor < b.valor) ? 1 : -1);
                let labelSoloSalida = soloSalida.map(data=>data.nombre);
                let dataSoloSalida = soloSalida.map(data=>data.valor);                

                for (let i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (let i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Actualizar gráfica ingreso y salida
                graph_ingreso_salida_tipo_residuo.data.labels = labels;
                graph_ingreso_salida_tipo_residuo.data.datasets[0].data = dataIngreso;
                graph_ingreso_salida_tipo_residuo.data.datasets[1].data = dataSalida;
                graph_ingreso_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataIngreso) / 10;
                graph_ingreso_salida_tipo_residuo.update();
                //Actualizar gráfica solo salida
                graph_salida_tipo_residuo.data.labels = labelSoloSalida;
                graph_salida_tipo_residuo.data.datasets[0].data = dataSoloSalida;
                graph_salida_tipo_residuo.options.scales.yAxes[0].ticks.stepSize = Math.max(dataSoloSalida) / 10;
                graph_salida_tipo_residuo.update();
                //Actualizar gráfica top ingreso 
                let labelsTopIngreso = [];
                let dataTopIngreso = [];
                data_top_nombre_residuo_ingreso = response.registros;
                data_top_nombre_residuo_ingreso.sort((a,b)=> {return b.ingreso-a.ingreso});
                for (let i = 0; i < data_top_nombre_residuo_ingreso.length; i++) {
                    labelsTopIngreso.push(data_top_nombre_residuo_ingreso[i].nombreResiduo);
                    dataTopIngreso.push(data_top_nombre_residuo_ingreso[i].ingreso);
                }
                graph_top_nombre_residuo_ingreso.data.labels = labelsTopIngreso;
                graph_top_nombre_residuo_ingreso.data.datasets[0].data = dataTopIngreso;
                graph_top_nombre_residuo_ingreso.options.scales.yAxes[0].ticks.stepSize = Math.max(dataTopIngreso) / 10;
                graph_top_nombre_residuo_ingreso.update();
                //Actualizar gráfica top salida
                let labelsTopSalida = [];
                let dataTopSalida = [];
                data_top_nombre_residuo_salida = response.registros;
                data_top_nombre_residuo_salida.sort((a,b)=> {return b.salida-a.salida});
                for (let i = 0; i < data_top_nombre_residuo_salida.length; i++) {
                    labelsTopSalida.push(data_top_nombre_residuo_salida[i].nombreResiduo);
                    dataTopSalida.push(data_top_nombre_residuo_salida[i].salida);
                }
                graph_top_nombre_residuo_salida.data.labels = labelsTopSalida;
                graph_top_nombre_residuo_salida.data.datasets[0].data = dataTopSalida;
                graph_top_nombre_residuo_salida.options.scales.yAxes[0].ticks.stepSize = Math.max(dataTopSalida) / 10;
                graph_top_nombre_residuo_salida.update();
            } else {              
                data_top_nombre_residuo_ingreso = response.registros;
                data_top_nombre_residuo_salida = response.registros;
                data_top_nombre_residuo_ingreso.sort((a,b)=> {return b.ingreso-a.ingreso});
                data_top_nombre_residuo_salida.sort((a,b)=> {return b.salida-a.salida});                  
                //Actualizar contador
                data_ingreso_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                data_salida_tipo_residuo = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.nombreResiduo;             
                    obj.valor = residuo.salida;     
                    return obj;       
                });

                let labels = [];
                let dataIngreso = [];
                let dataSalida = [];
                let soloSalida = data_salida_tipo_residuo.filter(data => data.valor > 0).sort((a, b) => (a.valor < b.valor) ? 1 : -1);
                let labelSoloSalida = soloSalida.map(data=>data.nombre);
                let dataSoloSalida = soloSalida.map(data=>data.valor);
                for (let i = 0; i < data_ingreso_tipo_residuo.length; i++) {
                    labels.push(data_ingreso_tipo_residuo[i].nombre);
                    dataIngreso.push(data_ingreso_tipo_residuo[i].valor);
                }
                for (let i = 0; i < data_salida_tipo_residuo.length; i++) {
                    dataSalida.push(data_salida_tipo_residuo[i].valor);
                }
                //Actualizar gráfica top ingreso 
                let labelsTopIngreso = [];
                let dataTopIngreso = [];
                data_top_nombre_residuo_ingreso = response.registros;
                data_top_nombre_residuo_ingreso.sort((a,b)=> {return b.ingreso-a.ingreso});
                for (let i = 0; i < data_top_nombre_residuo_ingreso.length; i++) {
                    labelsTopIngreso.push(AjustarLabel(data_top_nombre_residuo_ingreso[i].nombreResiduo));
                    dataTopIngreso.push(data_top_nombre_residuo_ingreso[i].ingreso);
                }
                //Actualizar gráfica top salida
                let labelsTopSalida = [];
                let dataTopSalida = [];
                data_top_nombre_residuo_salida = response.registros;
                data_top_nombre_residuo_salida.sort((a,b)=> {return b.salida-a.salida});
                for (let i = 0; i < data_top_nombre_residuo_salida.length; i++) {
                    labelsTopSalida.push(data_top_nombre_residuo_salida[i].nombreResiduo);
                    dataTopSalida.push(data_top_nombre_residuo_salida[i].salida);
                }
                //Crear gráfica
                CrearGraficaIngresoSalidaTipoResiduo(labels, dataIngreso, dataSalida, Math.max(dataIngreso) / 10);
                CrearGraficaSalidaTipoResiduo(labelSoloSalida, dataSoloSalida, Math.max(dataSoloSalida) / 10);
                CrearGraficaTopTipoResiduoIngreso(labelsTopIngreso, dataTopIngreso, Math.max(dataTopIngreso) / 10);
                CrearGraficaTopTipoResiduoSalida(labelsTopSalida, dataTopSalida, Math.max(dataTopSalida) / 10);
            }
        }
    });
}
function CrearGraficaIngresoSalidaTipoResiduo(labels, dataIngreso, dataSalida, stepSize) 
{
    graph_ingreso_salida_tipo_residuo = new Chart(
        document.getElementById("chart_ingreso_salida_tipo_residuo"),
        {
            "type": "horizontalBar",
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
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
//Para salida según tipo de residuos
function CrearGraficaSalidaTipoResiduo(labels, dataSalida, stepSize) 
{
    graph_salida_tipo_residuo = new Chart(
        document.getElementById("chart_salida_tipo_residuo"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Cantidad de Residuos Sólido Retirado (kg)",
                        "data": dataSalida,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
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
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
//Para tipo de residuos x ingreso
function CrearGraficaTopTipoResiduoIngreso(labels, dataIngreso, stepSize) 
{
    graph_top_nombre_residuo_ingreso = new Chart(
        document.getElementById("chart_top_tipo_residuo_ingreso"),
        {
            "type": "horizontalBar",
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
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
                /*
                plugins: [{
                    beforeInit: function(chart) {
                       chart.data.labels.forEach(function(e, i, a) {
                          if (/\n/.test(e)) {
                             a[i] = e.split(/\n/);
                          }
                       });
                    }
                 }]*/
            }
        }
    );
}
function CrearGraficaTopTipoResiduoSalida(labels, dataIngreso, stepSize) 
{
    graph_top_nombre_residuo_salida = new Chart(
        document.getElementById("chart_top_tipo_residuo_salida"),
        {
            "type": "horizontalBar",
            "data": {
                "labels": labels,
                "datasets": [
                    {
                        "label": "# Cantidad de Residuos Sólido Salida (kg)",
                        "data": dataIngreso,
                        "fill": false,
                        "backgroundColor": "rgb(46, 103, 147)",
                        "borderColor": "rgb(46, 103, 147)",
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
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
//Para destino x salida x TipoResiduo
function ActualizarDestinoxTipoResiduoxSalida() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetTipoResiduoxDestinoxSalida",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "destinos": $("#ISRS_Destino_DTRS").val(),
            "residuos": $("#ISRS_NombreResiduo_DTRS").val(),
        },
        traditional: true,
        success: (response) => {
            let dataset = response.registros;
            let empresas = [];
            let salidaEmpresas = [];
            dataset.forEach(element => {
                element.destinos.forEach(destino => {
                    if (empresas.indexOf(destino.destino) === -1) {
                        empresas.push(destino.destino);
                      }
                });
            });
            let rgb_list = [
                {
                    'r':45,
                    'g':100,
                    'b':155
                },
                {
                    'r':205,
                    'g':55,
                    'b':40
                },
                {
                    'r':193,
                    'g':216,
                    'b':47
                },
                {
                    'r':0,
                    'g':188,
                    'b':288
                },
                {
                    'r':110,
                    'g':41,
                    'b':141
                },
            ]
            let rgbCounter = 0;
            empresas.forEach(element => {
                //rgba(198,224,129,0.1)
                let color = "rgba(" + rgb_list[rgbCounter].r +"," + rgb_list[rgbCounter].g+"," + rgb_list[rgbCounter].b+",0.9)";
                if(rgb_list.length-1 == rgbCounter){
                    rgbCounter=0;
                }else{
                    rgbCounter++;
                }
                let obj = {
                    label : element,
                    data: [],
                    backgroundColor: color,
                    borderColor: color,
                    yAxisID: ("y-axis-"+element)
                }
                salidaEmpresas.push(obj)
            });

            //Recorrer cada empresa
            for(let i=0;i<salidaEmpresas.length;i++){
                //Recorrer por cada empresa todo el dataset
                for(let j=0;j<dataset.length;j++){                    
                //Por cada Tipo Residuo asignar 0 si no contiene el destino o el valor de salida si lo contiene
                    let salida = 0;
                    for(let k=0;k<dataset[j].destinos.length;k++){
                        if(dataset[j].destinos[k].destino === salidaEmpresas[i].label){ 
                            salida = dataset[j].destinos[k].totalSalida;
                            break;
                        }
                    }    
                    salidaEmpresas[i].data.push(salida);                
                }
                
            }            
            let labelsData = dataset.map(x=>x.nombreResiduo);
            let dataProvider = {
                labels:labelsData,
                datasets: salidaEmpresas
            };
            let yaxis = [];
            salidaEmpresas.forEach(element => {
                let obj = {
                    'id':element.yAxisID
                    //'ticks': {
                    //    'stepSize': 100,
                    //    'beginAtZero': true
                    //}
                }
                yaxis.push(obj)
            });
            console.log("DATAPROVIDER",dataProvider)
            console.log("yaxis",yaxis)
            let chartOptions = {
                "scales": {
                  "yAxes": yaxis
                },
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'center'
                    }
                }
              };            
            
            if(graph_destino_tipos_residuo_salida != null){
                graph_destino_tipos_residuo_salida.data = dataProvider;
                graph_destino_tipos_residuo_salida.options = chartOptions;
                graph_destino_tipos_residuo_salida.update();

            }else{
                CrearGraficaDestinoxTipoResiduoxSalida(dataProvider, chartOptions);
            }
        }
    });
}
function CrearGraficaDestinoxTipoResiduoxSalida(dataProvider, chartOptions) 
{
    graph_destino_tipos_residuo_salida = new Chart(
        document.getElementById("chart_salida_tipo_residuo_destino"),
        {
            type: 'bar',
            data: dataProvider,
            options: chartOptions
        });
        // {
        //     "type": "horizontalBar",
        //     "data": {
        //         "labels": labels,
        //         "datasets": [
        //             {
        //                 "label": "# Cantidad de Residuos Sólido Ingresado (kg)",
        //                 "data": dataIngreso,
        //                 "fill": false,
        //                 "backgroundColor": "rgb(46, 103, 147)",
        //                 "borderColor": "rgb(46, 103, 147)",
        //                 "lineTension": 0.1
        //             }
        //         ]
        //     },
        //     "options": {
        //         "scales": {
        //             'yAxes': [{
        //                 'ticks': {
        //                     'stepSize': stepSize,
        //                     'beginAtZero': true
        //                 }
        //             }]
        //         },
        //         "legend": {
        //             "labels": {
        //                 // This more specific font property overrides the global property
        //                 fontColor: 'black'
        //             }
        //         },
        //         "plugins": {
        //             datalabels: {
        //                 color: 'black',
        //                 //defaultFontSize: 15,
        //                 anchor: 'end',
        //                 clamp: true,
        //                 align: 'end'
        //             }
        //         }
        //     }
        // }
    // );
}

function ActualizarTopGerenciasIngresoResiduo() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetTopGerenciasIngreso",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "empresa": empresa,
        },
        traditional: true,
        success: (response) => {
            if (data_top_gerencia_ingreso.length > 0) {
                //Actualizar contador
                data_top_gerencia_ingreso = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.gerencia;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });
                let labels = [];
                let data = [];
                for (let i = 0; i < data_top_gerencia_ingreso.length; i++) {
                    labels.push(data_top_gerencia_ingreso[i].nombre);
                    data.push(data_top_gerencia_ingreso[i].valor);
                }
                //Actualizar gráfica
                graph_top_gerencia_ingreso.data.labels = labels;
                graph_top_gerencia_ingreso.data.datasets[0].data = data;
                graph_top_gerencia_ingreso.update();
            } else {
                //Actualizar contador
                data_top_gerencia_ingreso = response.registros.map((residuo)=>{
                    let obj = {};
                    obj.nombre = residuo.gerencia;             
                    obj.valor = residuo.ingreso;     
                    return obj;       
                });

                let labels = [];
                let data = [];
                for (let i = 0; i < data_top_gerencia_ingreso.length; i++) {
                    labels.push(data_top_gerencia_ingreso[i].nombre);
                    data.push(data_top_gerencia_ingreso[i].valor);
                }
                //Crear gráfica
                CrearGraficaTopGerenciasIngresoResiduo(labels, data, Math.max(data) / 10);
            }
        }
    });
}

function CrearGraficaTopGerenciasIngresoResiduo(labels, dataIngreso, stepSize) 
{
    graph_top_gerencia_ingreso = new Chart(
        document.getElementById("chart_top_areas_generadoras"),
        {
            "type": "horizontalBar",
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
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'end'
                    }
                }
            }
        }
    );
}
function ActualizarTopGerenciaxTipoResiduoxSalida() {
    $.ajax({
        type: "GET",
        url: "/DashboardResiduos/GetTopGerenciasIngresoTipoResiduo",
        data: {
            "acr_cmrs": acr_cmrs,
            "ingreso_salida": ingreso_salida,
            "tipo_residuo": tipo_residuo,
            "motivo": motivo,
            "fechaInicio": fechaInicio,
            "fechaFin": fechaFin,
            "gerencia": gerencia,
            "gerencias": $("#ISRS_Gerencia_GTRI").val(),
            "residuos": $("#ISRS_NombreResiduo_GTRI").val(),
        },
        traditional: true,
        success: (response) => {
            console.log("RESPONSE TEST: ",response)
            
            let dataset = response.registros;
            let gerencias = [];
            let ingresoGerencias = [];
            dataset.forEach(element => {
                element.gerencias.forEach(gerencia => {
                    if (gerencias.indexOf(gerencia.gerencia) === -1) {
                        gerencias.push(gerencia.gerencia);
                      }
                });
            });
            let rgbCounter = 0;
            gerencias.forEach(element => {
                //rgba(198,224,129,0.1)
                let color = "rgba(" + rgb_list[rgbCounter].r +"," + rgb_list[rgbCounter].g+"," + rgb_list[rgbCounter].b+",0.9)";
                if(rgb_list.length-1 == rgbCounter){
                    rgbCounter=0;
                }else{
                    rgbCounter++;
                }
                let obj = {
                    label : element,
                    data: [],
                    backgroundColor: color,
                    borderColor: color,
                    yAxisID: ("y-axis-"+element)
                }
                ingresoGerencias.push(obj)
            });

            console.log("salidaGerencias", ingresoGerencias)
            console.log("dataset", dataset)
            
            //Recorrer cada empresa
            for(let i=0;i<ingresoGerencias.length;i++){
                //Recorrer por cada empresa todo el dataset
                for(let j=0;j<dataset.length;j++){                    
                //Por cada Tipo Residuo asignar 0 si no contiene la gerencia o el valor de ingreso si lo contiene
                    let ingreso = 0;
                    for(let k=0;k<dataset[j].gerencias.length;k++){                        
                        if(dataset[j].gerencias[k].gerencia === ingresoGerencias[i].label){ 
                            ingreso = dataset[j].gerencias[k].ingreso;
                            break;
                        }
                    }    
                    ingresoGerencias[i].data.push(ingreso);                
                }
                
            }            
            let labelsData = dataset.map(x=>x.nombreResiduo);
            let dataProvider = {
                labels:labelsData,
                datasets: ingresoGerencias
            };
            let yaxis = [];
            ingresoGerencias.forEach(element => {
                let obj = {
                    'id':element.yAxisID
                    //'ticks': {
                    //    'stepSize': 100,
                    //    'beginAtZero': true
                    //}
                }
                yaxis.push(obj)
            });
            console.log("DATAPROVIDER 2",dataProvider)
            console.log("yaxis 2",yaxis)
            let chartOptions = {
                "scales": {
                  "yAxes": yaxis
                },
                "legend": {
                    "labels": {
                        // This more specific font property overrides the global property
                        fontColor: 'black'
                    }
                },
                "plugins": {
                    datalabels: {
                        color: 'black',
                        //defaultFontSize: 15,
                        anchor: 'end',
                        clamp: true,
                        align: 'center'
                    }
                }
              };            
            
            if(graph_gerencia_tipos_residuo_salida != null){
                graph_gerencia_tipos_residuo_salida.data = dataProvider;
                graph_gerencia_tipos_residuo_salida.options = chartOptions;
                graph_gerencia_tipos_residuo_salida.update();

            }else{
                CrearGraficaTopGerenciaxTipoResiduoxSalida(dataProvider, chartOptions);
            }
            
        }
    });
}
function CrearGraficaTopGerenciaxTipoResiduoxSalida(dataProvider, chartOptions) 
{
    graph_gerencia_tipos_residuo_salida = new Chart(
        document.getElementById("chart_top_ingreso_tipo_residuo_gerencia"),
        {
            type: 'bar',
            data: dataProvider,
            options: chartOptions
        });
}