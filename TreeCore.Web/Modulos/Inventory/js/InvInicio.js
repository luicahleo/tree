let graficoOperadores, graficoCategorias, graficoEstadosOperacionales;
var graficosKPI = [];
var kpiItems = [];
let idKPIIncremental = 1;
let ValorQueMostrar = "CREACION";

const tension = 0.4;
const fillKPI = true;
const COLORS = [
    '#4671AA',
    '#FB82B0',
    '#6CDFFB',
    '#F77E72',
    '#acc236',
    '#166a8f',
    '#00a950',
    '#58595b',
    '#8549ba'
];
const colorGraficoKPI = COLORS[0];
const colorGraficoTransKPI = "#90a9cc";


var bindParams = function () {

    loadAllCharts();
    
}

function loadAllCharts() {
    $("#grid-kpi").empty();
    graficosKPI.forEach(g => {
        g.destroy();
    });
    kpiItems = [];


    showLoadMask(App.pnGraficos2, function (load) {
        asyncStoreInventarioElementosOperadores(load);
    });
    showLoadMask(App.pnGraficos3, function (load) {
        asyncStoreInventarioElementosCategorias(load);
    });
    showLoadMask(App.pnGraficos4, function (load) {
        asyncStoreInventarioElementosEstadosOperacionales(load);
    });
    showLoadMask(App.pnGraficos1, function (load) {
        asyncStoreInventarioElementosTotales(load);
    });
    asyncInventarioCategoriasKPI();
}

// #region async functions
async function asyncStoreInventarioElementosOperadores(load) {
    App.storeInventarioElementosOperadores.load({
        callback: function (r, options, success) {
            if (success === true) {
                load.hide();
            }
        }
    });
}
async function asyncStoreInventarioElementosCategorias(load) {
    App.storeInventarioElementosCategorias.load({
        callback: function (r, options, success) {
            if (success === true) {
                load.hide();
            }
        }
    });
}
async function asyncStoreInventarioElementosEstadosOperacionales(load) {
    App.storeInventarioElementosEstadosOperacionales.load({
        callback: function (r, options, success) {
            if (success === true) {
                load.hide();
            }
        }
    });
}
async function asyncStoreInventarioElementosTotales(load) {
    App.storeInventarioElementosTotales.load({
        callback: function (r, options, success) {
            if (success === true) {
                load.hide();
            }
        }
    });
}
async function asyncInventarioCategoriasKPI() {
    showLoadMask(App.pnGraficos1, function (load) {
        TreeCore.GetTopCategories(8,
            {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    load.hide();
                }
                else {
                    let idsCategories = result.Result;

                    idsCategories.forEach(categoryID => {
                        TreeCore.InventarioCategoriasKPI(categoryID,
                            {
                                success: function (result) {
                                    if (!result.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                        load.hide();
                                    }
                                    else {
                                        if (result.Result[0] != undefined) {

                                            let inventarioElementos = result.Result;
                                            let yTemp = 0;
                                            let number = 0;
                                            let nameCategory;
                                            
                                            let dataset = {
                                                borderColor: colorGraficoKPI,
                                                backgroundColor: colorGraficoTransKPI,
                                                fill: fillKPI,
                                                cubicInterpolationMode: 'monotone',
                                                tension: tension,
                                                data: []
                                            };

                                            for (let e in inventarioElementos) {
                                                let invItem = inventarioElementos[e];

                                                // #region logica for
                                                let dateTmp = new Date(invItem.fecha);
                                                dateTmp.setHours(0);
                                                dateTmp.setMinutes(0);
                                                dateTmp.setSeconds(0);
                                                dateTmp.setMilliseconds(0);

                                                let informacion = JSON.parse(invItem.informacion);
                                                let valor = 0;

                                                if (ValorQueMostrar == "CREACION") {
                                                    valor = informacion.Creados;
                                                }
                                                else if (ValorQueMostrar == "TOTALES_CATEGORIA") {
                                                    valor = informacion.TotalCategory;
                                                }
                                                else if (ValorQueMostrar == "MODIFICACION") {
                                                    valor = informacion.Modificados;
                                                }


                                                if (dataset.data.find(x => x.x == dateTmp.toString()) == undefined) {
                                                    nameCategory = invItem.InventarioCategoria;
                                                    yTemp = valor;
                                                    number = informacion.TotalCategory;
                                                    dataset.data.push({
                                                        x: dateTmp,
                                                        y: yTemp
                                                    });
                                                }
                                                else {
                                                    let indexDt = dataset.data.findIndex(x => x.x == dateTmp.toString());
                                                    nameCategory = invItem.InventarioCategoria;
                                                    yTemp = dataset.data[indexDt].y + valor;
                                                    number = informacion.Total;
                                                    dataset.data[indexDt].y = yTemp;
                                                }

                                                // #endregion
                                            }

                                            let idKPI = idKPIIncremental++;
                                            kpiItems.push({
                                                id: idKPI,
                                                title: nameCategory,
                                                value: number,
                                                data: dataset
                                            });

                                            printKpiItems(idKPI);
                                        }
                                    }
                                }
                            });
                    });
                    load.hide();
                }
            }
        });

        
    });
}
// #endregion

// #region loadChartCategoria
function loadChartCategoria(sender, registro, index) {
    
    let inventarioElementos = App.storeInventarioElementosCategorias.data.items;
    let datasetsCategories = [];

    for (let e in inventarioElementos) {
        let invItem = inventarioElementos[e].data;
        // #region graficoLineal
        let dateTmp = new Date(invItem.fecha);
        dateTmp.setHours(0);
        dateTmp.setMinutes(0);
        dateTmp.setSeconds(0);
        dateTmp.setMilliseconds(0);

        let informacion = JSON.parse(invItem.informacion);

        let valor = 0;

        if (ValorQueMostrar == "CREACION") {
            valor = informacion.Creados;
        }
        else if (ValorQueMostrar == "TOTALES_CATEGORIA") {
            valor = informacion.TotalCategory;
        }
        else if (ValorQueMostrar == "MODIFICACION") {
            valor = informacion.Modificados;
        }


        if (datasetsCategories.find(x => x.label == invItem.InventarioCategoria) == undefined) {
            datasetsCategories.push({
                label: invItem.InventarioCategoria,
                borderColor: COLORS[datasetsCategories.length],
                backgroundColor: COLORS[datasetsCategories.length],
                fill: false,
                cubicInterpolationMode: 'monotone',
                tension: tension,
                data: [{
                    x: dateTmp,
                    y: valor
                }]
            });
        }
        else {
            let indexC = datasetsCategories.findIndex(c => c.label == invItem.InventarioCategoria);
            
            if (datasetsCategories[indexC].data.find(x => x.x == dateTmp.toString()) == undefined) {
                datasetsCategories[indexC].data.push({
                    x: dateTmp,
                    y: valor
                });
            }
            else {
                let indexDt = datasetsCategories[indexC].data.findIndex(x => x.x == dateTmp.toString());
                datasetsCategories[indexC].data[indexDt].y = datasetsCategories[indexC].data[indexDt].y + valor;
            }
        }

        // #endregion
    }
    if (graficoOperadores != undefined) {
        graficoOperadores.destroy();
    }

    graficoOperadores = graficoLineal("lineal-sites", datasetsCategories);
}
// #endregion

// #region loadChartOperadores
function loadChartOperadores(sender, registro, index) {
    let inventarioElementos = App.storeInventarioElementosOperadores.data.items;
    let operadores = [];
    let dataByOperadores = [];

    for (let o in inventarioElementos) {
        operadores.push(inventarioElementos[o].data.Operador);
        dataByOperadores.push(inventarioElementos[o].data.ocurrencies);
    }
    if (graficoCategorias != undefined) {
        graficoCategorias.destroy();
    }

    graficoCategorias = graficoTarta("tarta-operadores", operadores, dataByOperadores);
}
// #endregion

// #region loadChartEstadosOperacionales
function loadChartEstadosOperacionales(sender, registro, index) {
    let inventarioElementos = App.storeInventarioElementosEstadosOperacionales.data.items;
    let estadosOperacionales = [];
    let dataByEstado = [];

    for (let o in inventarioElementos) {
        estadosOperacionales.push(inventarioElementos[o].data.NombreAtributoEstado);
        dataByEstado.push(inventarioElementos[o].data.ocurrencies);
    }
    if (graficoEstadosOperacionales != undefined) {
        graficoEstadosOperacionales.destroy();
    }

    graficoEstadosOperacionales = graficoDonut("donut-estados-operacionales", estadosOperacionales, dataByEstado);
}
// #endregion

// #region loadChartTotales
function loadChartTotales() {
    let inventarioElementos = App.storeInventarioElementosTotales.data.items;
    let yTemp = 0;
    let number = 0;

    let dataset = {
        borderColor: colorGraficoKPI,
        backgroundColor: colorGraficoTransKPI,
        fill: fillKPI,
        cubicInterpolationMode: 'monotone',
        tension: tension,
        data: []
    };

    for (let e in inventarioElementos) {
        let invItem = inventarioElementos[e].data;

        // #region logica for
        let dateTmp = new Date(invItem.fecha);
        dateTmp.setHours(0);
        dateTmp.setMinutes(0);
        dateTmp.setSeconds(0);
        dateTmp.setMilliseconds(0);

        let informacion = JSON.parse(invItem.informacion);

        if (dataset.data.find(x => x.x == dateTmp.toString()) == undefined) {
            yTemp = informacion.Creados;
            number = informacion.Total;
            dataset.data.push({
                x: dateTmp,
                y: yTemp
            });
        }
        else {
            let indexDt = dataset.data.findIndex(x => x.x == dateTmp.toString());
            yTemp = dataset.data[indexDt].y + informacion.Creados;
            number = informacion.Total;
            dataset.data[indexDt].y = yTemp;
        }

        // #endregion
    }
    let idKPI = 0;

    kpiItems.push({
        id: idKPI,
        title: "Number of inventory",
        value: number,
        data: dataset
    });

    printKpiItems(idKPI);
}
// #endregion

// #region Graficos

// #region graficoTarta
function graficoTarta(idMyChart, labels, dataArray) {
    
    const data = {
        labels: labels,
        datasets: [
            {
                data: dataArray,
                backgroundColor: COLORS,
            }
        ]
    };

    const config = {
        type: 'pie',
        data: data,
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                }
            }
        },
    };

    return new Chart(
        document.getElementById(idMyChart),
        config
    );
}
// #endregion

// #region graficoLineal
function graficoLineal(idMyChart, datasets) {
    const data = {
        datasets: datasets
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'month'
                    }
                }
            },
            plugins: {
                legend: {
                    position: 'right',
                }
            }
        }
    };

    return new Chart(
        document.getElementById(idMyChart),
        config
    );
}
// #endregion

// #region graficoDonut
function graficoDonut(idMyChart, labels, dataArray) {

    const data = {
        labels: labels,
        datasets: [
            {
                data: dataArray,
                backgroundColor: COLORS,
            }
        ]
    };

    const config = {
        type: 'doughnut',
        data: data,
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                }
            }
        },
    };

    return new Chart(
        document.getElementById(idMyChart),
        config
    );
}
// #endregion

// #region graficoLinealKPI
async function graficoLinealKPI(idMyChart, datasets) {
    const data = {
        datasets: datasets
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'month'
                    },
                    ticks: {
                        display: false
                    },
                    grid: {
                        display: false
                    }
                },
                y: {
                    ticks: {
                        display: false
                    },
                    grid: {
                        display: false
                    }
                }
            },
            elements: {
                point: {
                    radius: 0
                }
            }
        }
    };

    graficosKPI.push(new Chart(
        document.getElementById(idMyChart),
        config
    ));
}
// #endregion

// #region printKpiItems
function printKpiItems(id) {
    
    let kpiItem = kpiItems.find(i => i.id == id);

    if (kpiItem != null && kpiItem != undefined) {
        let chartKpiID = `chart-kpi-${kpiItem.id}`;

        if (kpiItem.data.data == undefined) {
            for (let u in kpiItem.data) {
                kpiItem.data[u].x = new Date(kpiItem.data[u].x);
            }

            kpiItem.data = {
                borderColor: colorGraficoKPI,
                backgroundColor: colorGraficoTransKPI,
                fill: fillKPI,
                cubicInterpolationMode: 'monotone',
                tension: tension,
                data: kpiItem.data
            }
        }

        //Calculo de tendencia
        let x = [];
        let y = [];
        let ico;
        for (let r in kpiItem.data.data) {
            let registro = kpiItem.data.data[r];
            x.push(new Date(registro.x).getTime());
            y.push(registro.y);
        }
        let tendencia = linearRegression(y, x);

        if (tendencia.slope > 0) {
            ico = "ico-chart-up";
        } else if (tendencia.slope < 0) {
            ico = "ico-chart-down";
        } else {
            ico = "ico-chart-equals";
        }

        let htmlItem = `<div class="kpi-item">
                                <div class="text">${kpiItem.title}</div>
                                <div class="value">
                                    <div class="number">${kpiItem.value}</div>
                                    <div class="${ico} chartIco">&nbsp;</div>
                                </div>
                                <div class="kpi-chart">
                                    <canvas id="${chartKpiID}" />
                                </div>
                            </div>`;

        if (id == 0) {
            $("#grid-kpi").prepend(htmlItem);
        }
        else {
            $(`#grid-kpi`).append(htmlItem);
        }

        graficoLinealKPI(chartKpiID, [kpiItem.data]);
    }
}
// #endregion

// #endregion

function changeCmbRangoTiempo() {
    $("#grid-kpi").empty();
    ValorQueMostrar = App.cmbValoresQueMostrar.value;
    graficosKPI.forEach(g => {
        g.destroy();
    });
    kpiItems = [];
    showLoadMask(App.pnGraficos1, function (load) {
        asyncStoreInventarioElementosTotales(load);
    });
    asyncInventarioCategoriasKPI();
}

// #region WinConfigCharts
function openConfigCharts() {
    let combos = [App.cmbOperadores, App.cmbCategorias, App.cmbEstadosOperacionales];
    recargarCombos(combos, function () {
        App.WinConfigCharts.show();
    });
}

function WinConfigChartsBotonGuardar() {
    TreeCore.GuardarConfiguracionHome({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            } else {
                //changeCmbRangoTiempo();
                loadAllCharts();
                App.WinConfigCharts.hide();
            }
        }
    });
}

function validateCombosForm(sender) {
    
    if ((sender.getSelectedRecords().length > parseInt(App.hdTOP_CATEGORIAS.value)) ||
            (sender.getSelectedRecords().length > parseInt(App.hdTOP_ESTADOS_OPERACIONALES.value)) ||
            (sender.getSelectedRecords().length > parseInt(App.hdTOP_OPERADORES.value))) {
        App.btnGuardar.disable();
    }
    else
    {
        App.btnGuardar.enable();
    }
}
// #endregion

// #region Calculo Tendencia
function linearRegression(y, x) {
    let lr = {};
    let n = y.length;
    let sum_x = 0;
    let sum_y = 0;
    let sum_xy = 0;
    let sum_xx = 0;
    let sum_yy = 0;
    let cantidad = y.length;
    let varianzaX = 0.0;
    let varianzaY = 0.0;
    let varianza = 0.0
    let promedioX = 0.0;
    let promedioY = 0.0;

    for (let i = 0; i < n; i++) {
        sum_x += parseFloat(x[i]);
        sum_y += parseFloat(y[i]);
        sum_xy += (parseFloat(x[i]) * parseFloat(y[i]));
        sum_xx += (parseFloat(x[i]) * parseFloat(x[i]));
        sum_yy += (parseFloat(y[i]) * parseFloat(y[i]));
    }

    lr['slope'] = (n * sum_xy - sum_x * sum_y) / (n * sum_xx - sum_x * sum_x);
    lr['intercept'] = (sum_y - lr.slope * sum_x) / n;
    lr['r2'] = Math.pow((n * sum_xy - sum_x * sum_y) / Math.sqrt((n * sum_xx - sum_x * sum_x) * (n * sum_yy - sum_y * sum_y)), 2).toFixed(2);
    lr['p'] = 0;

    //
    promedioX = sum_x / cantidad;
    promedioY = sum_y / cantidad;

    // Calculo de varianzas
    for (i = 0; i < cantidad; i++) {
        varianzaX = varianzaX + Math.pow(x[i] - promedioX, 2);
        varianzaY = varianzaY + Math.pow(y[i] - promedioY, 2);
        varianza += ((x[i] - promedioX) * (y[i] - promedioY));
    }

    // P-value
    let SLP = varianza / varianzaX;
    let SSEF = (varianzaY - SLP * SLP * varianzaX);
    let SSRF = SLP * SLP * varianzaX;
    let MSEF = SSEF / (cantidad - 2);
    let FVAL1 = SSRF / MSEF;
    let Fp = FVAL1 + "";
    let fDesde = 1;
    let fHasta = cantidad - 2;
    let p = Fmt(FishF(FVAL1, fDesde, fHasta));
    if ((FVAL1 + "").indexOf("Infinity") == -1) {
        if ((p + "").indexOf("e") != -1) {
            lr['p'] = "Almost Zero";
        }
        else {
            lr['p'] = p;
        }
    }

    return lr;
}

function FishF(f, n1, n2) {
    let x = n2 / (n1 * f + n2)
    if ((n1 % 2) == 0) {
        return StatCom(1 - x, n2, n1 + n2 - 4, n2 - 2) * Math.pow(x, n2 / 2);
    }
    if ((n2 % 2) == 0) {
        return 1 - StatCom(x, n1, n1 + n2 - 4, n1 - 2) * Math.pow(1 - x, n1 / 2);
    }
    let th = Math.atan(Math.sqrt(n1 * f / n2));
    let a = th / (Math.PI / 2);
    let sth = Math.sin(th);
    let cth = Math.cos(th);
    if (n2 > 1) {
        a = a + sth * cth * StatCom(cth * cth, 2, n2 - 3, -1) / (Math.PI / 2);
    }
    if (n1 == 1) {
        return 1 - a;
    }
    let c = 4 * StatCom(sth * sth, n2 + 1, n1 + n2 - 4, n2 - 2) * sth * Math.pow(cth, n2) / Pi;
    if (n2 == 1) {
        return 1 - a + c / 2;
    }
    let k = 2;
    while (k <= (n2 - 1) / 2) {
        c = c * k / (k - .5);
        k = k + 1;
    }
    return 1 - a + c;
}

function StatCom(q, i, j, b) {
    let zz = 1;
    let z = zz;
    let k = i;
    while (k <= j) {
        zz = zz * q * k / (k - b);
        z = z + zz;
        k = k + 2;
    }
    return z
}

/* FMT: Redondea a 4 decimales */
function Fmt(x) {
    let v;
    if (x >= 0) {
        v = '' + (x + 0.00005);
    } else {
        v = '' + (x - 0.00005);
    }
    return v.substring(0, v.indexOf('.') + 5);
}

// #endregion

