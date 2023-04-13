// #region DISEÑO
let slickCreado = false;

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)




function winFormCenterSimple(obj) {


    obj.center();
    obj.updateLayout();

}


function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();


}



function winFormResizeDockBot(obj) {


    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();




    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    obj.updateLayout();


}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();
    obj.updateLayout();




}


 

window.addEventListener('resize', function () {


    var dv = document.querySelectorAll('div.winForm-respSimple');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormCenterSimple(obj);
    }


    var dv = document.querySelectorAll('div.winForm-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResize(obj);
    }

    var frm = document.querySelectorAll('div.ctForm-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formResize(obj);
    }



    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }



    //ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL
    //var pnCentral = document.getElementsByClassName("pnCentralWrap");
    var winsize = window.innerWidth;
    var asideR = Ext.getCmp('gridMain1');


    if (winsize < 540 && asideR.collapsed == false) {
        App.GridPanelSideL.hide();
        App.gridMain1.setWidth(winsize);
    }
    else {
        App.GridPanelSideL.show();
        App.gridMain1.setWidth(380);

    }


});


// #endregion

function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}

function showwinAddTab() {
    App.winAddTabFilter.show();

}

function showwinSaveQF() {
    App.winSaveQF.show();

}

//  #region JS VISOR 2 PANELES


Ext.onReady(function () {
    var asideL = Ext.getCmp('GridPanelSideL');
    var AspxMaxW = asideL.maxWidth;

});






//ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL

var GridPnl = document.getElementsByClassName("GridPnl");


var forcedVisor = false;
var PuntoCorteS = 512;

function VisorSwitch(sender) {

    var asideL = Ext.getCmp('GridPanelSideL');
    var btnclose = Ext.getCmp('btnCloseShowVisorTreeP');

    if (asideL.hidden == true) {
        forcedVisor = false;
        btnclose.setIconCls('ico-hide-menu');
        App.GridPanelSideL.show();

    }
    else {
        forcedVisor = true;
        btnclose.setIconCls('ico-moverow-gr');
        App.GridPanelSideL.hide();

    }

}

function ControlPaneles(sender) {

    var winsize = window.innerWidth;
    var containerSize = Ext.get('CenterPanelMain').getWidth();

    if (forcedVisor != true) {

        //HIDE PANEL2
        if (containerSize < 480) {
            GridPnl[0].classList.remove("TreePL");

            App.GridPanelSideL.maxWidth = 9999;
            App.gridMain1.hide();

        }
        else {
            GridPnl[0].classList.add("TreePL");

            App.GridPanelSideL.maxWidth = 400;
            App.gridMain1.show();

        }

        //HIDE PANEL1
        if (containerSize < 120) {
            App.GridPanelSideL.hide();

        }
        else {
            App.GridPanelSideL.show();

        }
    }
    else {

        if (containerSize < 160) {
            App.gridMain1.hide();
            GridPnl[0].classList.remove("TreePL");
        }
        else {
            App.gridMain1.show();
            GridPnl[0].classList.add("TreePL");
        }
    }

    if (containerSize < PuntoCorteS) {

        App.tbSliders.show();

        if (App.GridPanelSideL.hidden == false && App.gridMain1.hidden == true) {
            App.btnPrev.disable();

        }
    }
    else {
        App.tbSliders.hide();
        App.btnPrev.disable();
        App.btnNext.enable();

    }
}

function SliderMove(NextOrPrev) {

    if (NextOrPrev == 'Next') {
        forcedVisor = true;
        App.GridPanelSideL.hide();
        App.gridMain1.show();

        App.btnPrev.enable();
        App.btnNext.disable();

    }
    else {
        forcedVisor = false;
        App.GridPanelSideL.show();
        App.gridMain1.hide();

        App.btnPrev.disable();
        App.btnNext.enable();

    }
}

// #endregion

// #endregion

// #region DIRECT METHOD

function CalcularElementosCorrectos(sender, registro, index) {

    if (index.data.NumeroElementos == -1) {
        return `<span class="gen_Inactivo">&nbsp;</span>`;
    }
    else {
        return (index.data.Total - index.data.NumeroElementos);
    }
}

function cmbDQCategorias() {
    deseleccionarGrilla();
    App.cmbDQCategorias.getTrigger(0).show();
    App.storePrincipal.reload();
}

function recargarDQCategorias() {
    App.storePrincipal.reload();
    recargarCombos([App.cmbDQCategorias]);
}

function btnKPI(sender, registro, index) {
    if (App.GridPanelSideL.selection) {
        parent.showKPIByID(App.GridPanelSideL.selection.data.DQKpiID);
    }
}

function rendererColError(sender, registro, index) {
    let ruta = index.data.RutaPagina;
    let errores = index.data.NumeroElementos;
    let DQKpi = index.data.DQKpi;
    let DQKpiMonitoringID = index.data.DQKpiMonitoringID;

    let ids = index.data.idsResults;

    if (errores == -1) {
        return `<span class="gen_Inactivo">&nbsp;</span>`;
    }
    else {
        return `<a class="linkColumn" data-url="${ruta}" data-resultKPI="${DQKpiMonitoringID}" data-nameKPI="${DQKpi}" data-ids="${ids}" href="#">${errores}</a>`;
    }
}

function rendererColActivo(sender, registro, index) {
    let Total = index.data.Total;

    if (Total == -1) {
        return `<span class="gen_Inactivo">&nbsp;</span>`;
    }
    else {
        return Total;
    }
}

function addEventoLinkPagina() {
    $("a.linkColumn").click(function (e) {
        let url = e.currentTarget.getAttribute("data-url");
        let ids = e.currentTarget.getAttribute("data-ids")
        let resultKPI = e.currentTarget.getAttribute("data-resultKPI");
        let nameKPI = e.currentTarget.getAttribute("data-nameKPI");
        let path = "/" + url;

        if (ids) {
            ids = ids.split(",");

            if (ids.length > 0) {
                let params = "";

                ids.forEach(function (id) {
                    params = ((params.length > 0)? "&":"") + "idsResultados=" + id;
                });

                path += "?" + params;
            } 
        }

        addTab(parent.parent.App.tabPpal, resultKPI, nameKPI, path);
    });
}

function btnSemaforo(sender, registro, index) {
    alert();
}

function btnDescargar(sender, registro, index) {
    let nameFile = "KPI";
    if (App.GridPanelSideL.selection && App.GridPanelSideL.selection.data && App.GridPanelSideL.selection.data.DQKpi) {
        nameFile = App.GridPanelSideL.selection.data.DQKpi;
    }
    DescargarPNG('gridMain1-innerCt', nameFile);
}

var handlePageSizeSelect = function (item, records) {
    let curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

var loadding = null;
function hideShowCharts() {
    if ($(".charts").is(":visible")) {
        $(".charts").hide();

        showLoadMask(App.cont1, function (load) {
            loadding = load;
        });
    }
    else {
        loadding.hide();
        $(".charts").show();
    }
}

function SelectTreepn(sender, registro, index) {
    $(".charts").show();
    hideShowCharts();
    $(".report-piechart").empty();
    $(".content-piechart> .text").text("");
    $("#graficoLineal").empty();

    let DQKpiID = registro.data.DQKpiID;

    App.hdDQKpiID.setValue(DQKpiID);
    App.lblTituloReport.setText(registro.data.DQKpi);

    App.storeGrupos.reload();
    App.storeUltimosKpisMonitoring.reload();
    App.storeKpisMonitoring.reload();

    App.btnDescargar.setDisabled(false);
    App.btnSemaforo.setDisabled(false);
    App.btnKPI.setDisabled(false);
    App.btnDetalleKPI.setDisabled(false);

    App.btnDescargar.setTooltip(jsDescargar);
    App.btnSemaforo.setTooltip(jsSemaforo);
    App.btnKPI.setTooltip(jsKPI);
    App.btnDetalleKPI.setTooltip(jsFiltros);

    parent.App.hdKPISeleccionado.setValue(DQKpiID);
    parent.cargarDatosPanelLateral(DQKpiID);

}

function deseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    $(".charts").hide();
    $(".report-piechart").empty();
    $("#graficoLineal").empty();

    App.btnDescargar.setDisabled(true);
    App.btnSemaforo.setDisabled(true);
    App.btnKPI.setDisabled(true);
    App.btnDetalleKPI.setDisabled(true);
}

function pintarGraficosGrupos(sender, registro, index) {

    let divGroups = $("#piecharts-groups");

    if (slickCreado) {
        divGroups.slick('unslick');
    }
    divGroups.empty();


    let grupoCls = "grupo";
    let grupos = {};
    let groups = [];

    App.storeGrupos.data.items.forEach(function (grupo) {
        let NombreTipoCondicion = grupo.data.NombreTipoCondicion;
        let DQGroupID = grupo.data.DQGroupID;
        let DQGroupMonitoringID = grupo.data.DQGroupMonitoringID;

        if (grupos[DQGroupID] == undefined) {
            grupos[DQGroupID] = {};
            grupos[DQGroupID].DQGroupID = DQGroupID;
            grupos[DQGroupID].elements = [];
            groups.push(DQGroupID);
            divGroups.append(`<div id="${grupoCls + DQGroupID}" class="${grupoCls}"></div>`);

        }

        let element = `<div class="content-piechart piechart-version" data-version="${grupo.data.Version}"><div id="pieGroup-${DQGroupMonitoringID}" class="report-piechart"></div><div class="text">${NombreTipoCondicion}</div></div>`;
        grupos[DQGroupID].elements.push({
            element: element,
            data: grupo.data,
            chart: [
                {
                    "value": grupo.data.Total - grupo.data.NumeroElementos,
                    "category": jsCorrectos
                },
                {
                    "value": grupo.data.NumeroElementos,
                    "category": jsErrores
                }
            ]
        });
    });

    groups.forEach(function (g) {
        let versionRestantes = 3;
        let versionKPI = App.GridPanelSideL.selection.data.Version;

        /*if (grupos[g].elements.length < 3) {
        }*/

        grupos[g].elements.forEach(function (e) {
            if (e.data.Version < versionKPI) {
                let element = `<div class="content-piechart piechart-version" data-version="${versionKPI}" data-uno="asdf"/>`;
                $("#" + grupoCls + g).append(element);
                versionRestantes--;
                versionKPI--;
            }
            versionKPI--;
            


            $("#" + grupoCls + g).append(e.element);

            pintarDonut(e.chart, "pieGroup-" + e.data.DQGroupMonitoringID);
            $("#pieGroup-" + e.data.DQGroupMonitoringID + " .text").show();

            versionRestantes--;
        });

        if (versionRestantes > 0) {
            for (let i = 0; i < versionRestantes; i++) {
                let element = `<div class="content-piechart piechart-version" data-version="0"/>`;
                $("#" + grupoCls + g).append(element);
            }
        }
    });


    $("#piecharts-groups").slick({
        prevArrow: "<div class='btnArrowleft-ToolB slick-arrow slick-prev'></div>",
        nextArrow: "<div class='btnArrowleft-ToolB slick-arrow slick-next'></div>"
    });
    slickCreado = true;
    hideShowCharts();
}

function pintarDonut(chartData, chartdiv) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Show text
        $("." + chartdiv).show();

        // Create chart instance
        var chart = am4core.create(chartdiv, am4charts.PieChart);

        // Add data
        chart.data = chartData;

        // Add label
        chart.innerRadius = 20;


        // Add and configure Series
        var pieSeries = chart.series.push(new am4charts.PieSeries());
        pieSeries.dataFields.value = "value";
        pieSeries.dataFields.category = "category";

        // hide labels
        pieSeries.labels.template.disabled = true;

        pieSeries.colors.list = [
            am4core.color("#58E5A9"),//Verde
            am4core.color("#EF5970"),//Rojo
            am4core.color("#F7BB74"),//Amarillo
        ];

    }); // end am4core.ready()
}

function storeUltimosKpisMonitoring() {
    let chartdiv = "Donut";
    let countChart = 1;

    App.storeUltimosKpisMonitoring.data.items.forEach(function (kpi) {
        let dataTemp = [
            {
                "value": kpi.data.Total - kpi.data.NumeroElementos,
                "category": jsCorrectos
            },
            {
                "value": kpi.data.NumeroElementos,
                "category": jsErrores
            }
        ];

        let date = new Date(kpi.data.FechaEjecucion);
        let fechaVers = date.getDate() + "/" + (date.getMonth()+1) + "/" + date.getFullYear();
        let horaVer = date.getHours() + ":" + addZero(date.getUTCMinutes());
        fechaVers = fechaVers + " | " + horaVer; 
        let nameDonutVersion = "v" + kpi.data.Version;
        $(".text." + chartdiv + countChart).html(nameDonutVersion + "<br>" + fechaVers);

        if (kpi.data.NumeroElementos != -1) {
            pintarDonut(dataTemp, chartdiv + countChart++);
        } else {
            let divTmp = chartdiv + countChart++;
            $("#" + divTmp).text("");
        }
    });
}

function addZero(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function storeKpisMonitoring() {
    let graficoLineal = "graficoLineal";
    let kpisMonitoring = [];

    App.storeKpisMonitoring.data.items.forEach(function (kpi) {
        kpisMonitoring.push(kpi.data);

    });



    pintarGraficoLineal(kpisMonitoring, graficoLineal);
}

function pintarGraficoLineal(chartData, chartdiv) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create(chartdiv, am4charts.XYChart);
        chart.paddingRight = 20;

        chart.data = chartData;

        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        dateAxis.renderer.grid.template.location = 0;
        dateAxis.minZoomCount = 5;


        // this makes the data to be grouped
        //dateAxis.groupData = true;
        //dateAxis.groupCount = 500;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        var series = chart.series.push(new am4charts.LineSeries());
        series.dataFields.dateX = "FechaEjecucion";
        series.dataFields.valueY = "NumeroElementos";
        series.tooltipText = "{valueY}";
        series.tooltip.pointerOrientation = "vertical";
        series.tooltip.background.fillOpacity = 0.5;
        series.strokeWidth = 2;

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.xAxis = dateAxis;

        var scrollbarX = new am4core.Scrollbar();
        scrollbarX.marginBottom = 20;
        chart.scrollbarX = scrollbarX;

    }); // end am4core.ready()
}

// #endregion

function ResizeCont1() {
    
    if (slickCreado) {
        $("#piecharts-groups").slick("slickGoTo", $("#piecharts-groups").slick("slickCurrentSlide"));
    }
}