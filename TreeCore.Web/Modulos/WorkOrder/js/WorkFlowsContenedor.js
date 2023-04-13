var stPn = 1;
var pnLateralAbierto;
let slickCreado = false;
let arrayRolesAsignados = [];
let arrayUsuariosAsignados = [];
let arrayRolesEstadosSeguimientoAsignados = [];
let arrayTareasEstadosAsignados = [];
let agregarNotificacion = false;

function displayMenuWF(btn, label) {

    //ocultar todos los paneles
    hidePnFilters(true);
    App.WrapEstados.hide();

    App.lblAsideNameInfo.hide();
    App.lblEstados.hide();

    //GET componente a mostrar desde el boton por ID
    if (btn != null) {
        App[btn].show();
    }
    if (label != null) {
        App[label].show();
    }

    displayMenu('pnMoreInfo');
}

function displayMenu(btn) {
    var name = '#' + btn;
    let tabla;
    let label;
    let grid;

    //ocultar todos los paneles
    App.pnEstadosGlobales.hide();
    App.pnSubprocesos.hide();
    App.pnEstadosSiguientes.hide();
    App.pnLinks.hide();
    App.pnTareas.hide();
    App.pnRoles.hide();
    App.pnRolesSeguimiento.hide();
    App.pnNotificaciones.hide();
    App.pnMoreInfo.hide();

    if (btn == 'pnEstadosGlobales') {
        tabla = document.getElementById('bodyTablaInfoEstadosGlobales');
        grid = pageActual.gridEstadosGlobales;
        label = App.lblTotalEstadosGlobales;
        App.pnEstadosGlobales.show();
    }
    else if (btn == 'pnSubprocesos') {
        tabla = document.getElementById('bodyTablaInfoSubprocesos');
        label = App.lblTotalSubprocesos;
        App.pnSubprocesos.show();
    }
    else if (btn == 'pnEstadosSiguientes') {
        tabla = document.getElementById('bodyTablaInfoEstadosSiguientes');
        grid = pageActual.gridEstadosSiguientes;
        label = App.lblTotalEstadosSiguientes;
        App.pnEstadosSiguientes.show();
    }
    else if (btn == 'pnLinks') {
        tabla = document.getElementById('bodyTablaInfoLinks');
        label = App.lblTotalLinks;
        App.pnLinks.show();
    }
    else if (btn == 'pnTareas') {
        tabla = document.getElementById('bodyTablaInfoTareas');
        grid = pageActual.gridObjetos;
        label = App.lblTotalTareas;
        App.pnTareas.show();
    }
    else if (btn == 'pnRoles') {
        tabla = document.getElementById('bodyTablaInfoRoles');
        label = App.lblTotalRoles;
        App.pnRoles.show();
    }
    else if (btn == 'pnRolesSeguimiento') {
        tabla = document.getElementById('bodyTablaInfoRolesSeguimiento');
        label = App.lblTotalRolesSeguimiento;
        App.pnRolesSeguimiento.show();
    }
    else if (btn == 'pnNotificaciones') {
        tabla = document.getElementById('bodyTablaInfoNotificaciones');
        label = App.lblTotalNotificaciones;
        App.pnNotificaciones.show();
    }
    else if (btn == 'pnMoreInfo') {
        App.pnMoreInfo.show();
        grid = App.gridMain1;
    }

    if (btn != undefined && btn != null) {
        TreeCore.RecargarPanelLateral(btn,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Success, buttons: Ext.Msg.OK });
                    }
                    else {

                        lista = JSON.parse(result.Result);

                        //if (btn == 'pnMoreInfo') {
                        //    //cargarDatosPanelMoreInfoGridEstados(lista, grid);
                        //    CargarPanelMoreInfo('WrapEstados', 'lblEstados');
                        //}
                        //else
                        if (btn == 'pnNotificaciones') {
                            cargarDatosPanelNotificacionesEstados(tabla, lista, label);
                        }
                        else if (btn == 'pnRoles') {
                            cargarDatosPanelRolesEstados(tabla, lista, label);
                        }
                        else if (btn == 'pnRolesSeguimiento') {
                            cargarDatosPanelRolesEstadosSeguimiento(tabla, lista, label);
                        }
                        else if (btn != 'pnMoreInfo' || btn == 'pnTareas') {
                            cargarDatosPanelMoreInfoEstados(tabla, grid, lista, label);
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                }
            });
    }

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();


}

function cargarDatosPanelMoreInfoGridWF(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    if (Grid != undefined) {
        grid = Grid.columnManager.getColumns();

        for (var columna of grid) {
            if (columna.cls != 'col-More' && columna.cls != 'NoOcultar col-More' && columna.xtype != 'widgetcolumn' && columna.cls != "excluirPnInfo") {
                if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn' || columna.xtype == 'hyperlinkcolumn' || columna.xtype == 'componentcolumn')) {
                    if (registro.get(columna.dataIndex) != undefined) {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
                    }
                    else {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
                    }
                }
                else {
                    if ((columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) && columna.tooltip != undefined) {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.data[columna.dataIndex]) + '</span></td></tr>';
                    }
                    else if (columna.tooltip != undefined) {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
                    }
                    else if (columna.renderer.name.includes("bound")) {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name.split(' ')[1]](registro.data[columna.dataIndex]) + '</span></td></tr>';
                    }
                    else {
                        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.data[columna.dataIndex]) + '</span></td></tr>';
                    }
                }
            }
        }
    }
    
    tabla.innerHTML = html;
}

function cargarDatosPanelMoreInfoGridEstados(lista, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    if (Grid != undefined) {
        grid = Grid.columnManager.getColumns();

        if (lista.length != 0) {
            for (var prop of Object.keys(lista[0])) {
                for (var columna of grid) {
                    if (columna.cls != 'col-More' && columna.cls != 'NoOcultar col-More' && columna.xtype != 'widgetcolumn' && columna.cls != "excluirPnInfo") {
                        if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn' || columna.xtype == 'hyperlinkcolumn' || columna.xtype == 'componentcolumn')) {
                            if (prop == columna.dataIndex) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + lista[0][prop] + '</span></td></tr>';
                            }
                        }
                        else {
                            if (columna.tooltip != undefined && prop == columna.dataIndex) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](lista[0][prop]) + '</span></td></tr>';
                            }
                            else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render") && prop == columna.dataIndex) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](lista[0][prop]) + '</span></td></tr>';
                            }
                            else if (columna.renderer.name.includes("bound") && prop == columna.dataIndex) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd moreInfo-Share">' + this[columna.renderer.name.split(' ')[1]](lista[0][prop]) + '</span></td></tr>';
                            }
                        }
                    }
                }
            }
        }
    }

    tabla.innerHTML = html;
}

function cargarDatosPanelNotificacionesEstados(tabla, lista, label) {
    html = '';
    tabla.innerHTML = "";

    if (slickCreado) {
        $('.single-item').slick('unslick');
    }

    $('.single-item').empty();

    for (var i = 0; i < lista.length; i++) {

        html += '<div class="tmpCol-td" colspan = "3">';

        for (var prop of Object.keys(lista[i])) {
            html += '<div><span class="lblGrd">' + prop + ' : </span><span class="dataGrd">' + lista[i][prop] + '</span></div>';
        }

        html += '</div>';
    }

    tabla.innerHTML = html;

    if (label != undefined && label != "" && label != null && lista.length != 0) {
        $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
            label.setText(1 + '/' + lista.length);
        });

        $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
            label.setText((nextSlide + 1) + '/' + lista.length);
        });
    }

    $('.single-item').slick({
        dots: false,
        infinite: true,
        arrows: true,
        slidesToShow: 1,
        slidesToScroll: 1,
    });

    slickCreado = true;
}

function cargarDatosPanelRolesEstados(tabla, lista, label) {
    html = '';
    tabla.innerHTML = "";

    if (slickCreado) {
        $('.single-item').slick('unslick');
    }

    $('.single-item').empty();

    for (var i = 0; i < lista.length; i++) {

        html += '<div class="tmpCol-td" colspan = "3">';

        for (var prop of Object.keys(lista[i])) {
            html += '<div><span class="lblGrd">' + prop + ' : </span><span class="dataGrd">' + lista[i][prop] + '</span></div>';
        }

        html += '</div>';
    }

    tabla.innerHTML = html;

    if (label != undefined && label != "" && label != null && lista.length != 0) {
        $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
            label.setText(1 + '/' + lista.length);
        });

        $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
            label.setText((nextSlide + 1) + '/' + lista.length);
        });
    }

    $('.single-item').slick({
        dots: false,
        infinite: true,
        arrows: true,
        slidesToShow: 1,
        slidesToScroll: 1,
    });

    slickCreado = true;
}

function cargarDatosPanelRolesEstadosSeguimiento(tabla, lista, label) {
    html = '';
    tabla.innerHTML = "";

    if (slickCreado) {
        $('.single-item').slick('unslick');
    }

    $('.single-item').empty();

    for (var i = 0; i < lista.length; i++) {

        html += '<div class="tmpCol-td" colspan = "3">';

        for (var prop of Object.keys(lista[i])) {
            html += '<div><span class="lblGrd">' + prop + ' : </span><span class="dataGrd">' + lista[i][prop] + '</span></div>';
        }

        html += '</div>';
    }

    tabla.innerHTML = html;

    if (label != undefined && label != "" && label != null && lista.length != 0) {
        $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
            label.setText(1 + '/' + lista.length);
        });

        $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
            label.setText((nextSlide + 1) + '/' + lista.length);
        });
    }

    $('.single-item').slick({
        dots: false,
        infinite: true,
        arrows: true,
        slidesToShow: 1,
        slidesToScroll: 1,
    });

    slickCreado = true;
}

function cargarDatosPanelMoreInfoEstados(tabla, grid, lista, label) {
    html = '';
    tabla.innerHTML = "";
    let nombreGrid;

    if (slickCreado) {
        $('.single-item').slick('unslick');
    }

    $('.single-item').empty();

    if (grid != undefined) {
        nombreGrid = grid.columnManager.getColumns();

        for (var i = 0; i < lista.length; i++) {

            html += '<div class="tmpCol-td" colspan = "3">';

            for (var prop of Object.keys(lista[i])) {
                for (var columna of nombreGrid) {
                    if (columna.cls != 'col-More' && columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
                        if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                            if (prop == columna.dataIndex) {
                                html += '<div><span class="lblGrd">' + columna.config.text + ' : </span><span class="dataGrd">' + lista[i][prop] + '</span></div>';
                            }
                        }
                    }
                }
            }

            html += '</div>';
        }

        tabla.innerHTML = html;

        if (label != undefined && label != "" && label != null && lista.length != 0) {
            $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
                label.setText(1 + '/' + lista.length);
            });

            $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
                label.setText((nextSlide + 1) + '/' + lista.length);
            });
        }

        $('.single-item').slick({
            dots: false,
            infinite: true,
            arrows: true,
            slidesToShow: 1,
            slidesToScroll: 1,
        });

        slickCreado = true;
    }
}

function hidePnFilters(onlyShow) {
    let pn = App.pnAsideR;
    let btn = document.getElementById('btnCollapseAsR');
    if (stPn == 0 || onlyShow == true) {
        pn.expand();
        btn.style.opacity = 1;
        stPn = 1;
    }
    else {
        pn.collapse();
        btn.style.opacity = 0;
        stPn = 0;
    }

}

var pageActual;
function CargarPanelMoreInfo(panel, label, app) {
    pageActual = app;
    displayMenuWF(panel, label);
}

function NavegacionTabs(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('lnkBusinessProcess').classList.remove("navActivo");
    document.getElementById('lnkBusinessProcess').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkWorkFlow').classList.remove("navActivo");
    document.getElementById('lnkWorkFlow').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkStatus').classList.remove("navActivo");
    document.getElementById('lnkStatus').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkCustomField').classList.remove("navActivo");
    document.getElementById('lnkCustomField').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkDiagram').classList.remove("navActivo");
    document.getElementById('lnkDiagram').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkBusinessProcess') {
        document.getElementById('lnkBusinessProcess').classList.add("navActivo");
        document.getElementById('lnkBusinessProcess').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkWorkFlow') {
        document.getElementById('lnkWorkFlow').classList.add("navActivo");
        document.getElementById('lnkWorkFlow').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkStatus') {
        document.getElementById('lnkStatus').classList.add("navActivo");
        document.getElementById('lnkStatus').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.show();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkCustomField') {
        document.getElementById('lnkCustomField').classList.add("navActivo");
        document.getElementById('lnkCustomField').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.show();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkDiagram') {
        document.getElementById('lnkDiagram').classList.add("navActivo");
        document.getElementById('lnkDiagram').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.show();
    }
    else {

    }

    //let tab = getTabSelected();
    //var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    //App.hdFiltrosAplicados.setValue(fa);

}

var barGridEstados = function (value) {

    let colorBar;
    let colorPorcentaje;

    if (value > 0 && value < 20) {
        colorBar = 'barRed-grid';
        colorPorcentaje = 'barRed-porcentaje';
    }
    else if (value >= 20 && value < 45) {
        colorBar = 'barYellow-grid';
        colorPorcentaje = 'barYellow-porcentaje';
    }

    else if (value >= 45 && value < 80) {
        colorBar = 'barBlue-grid';
        colorPorcentaje = 'barBlue-porcentaje';
    }

    else if (value >= 80 && value <= 100) {
        colorBar = 'barGreen-grid';
        colorPorcentaje = 'barGreen-porcentaje';
    }
    return `<div id="porcentaje-ProgressBar" class="porcentaje-ProgressBar ${colorPorcentaje}">${value}%</div><div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value}%;"><div class="x-progress-text" style="width:50px;"><div>${value} %</div></div></div></div>`

}

var RequiereRender = function (sender, registro, index) {
    var datos = index.data;

    if (datos.Obligatorio) {
        return '<span class="ico-defaultGrid">&nbsp;</span>';
    }
    else {
        return '<span class="gen_Inactivo">&nbsp;</span>';
    }
}

var NotificacionRender = function (value) {
    if (value == "1" || value == "true") {
        return '<span class="ico-notificationGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var RolRender = function (value) {
    if (value == "1" || value == "true") {
        return '<span class="ico-functionalityGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var DefaultRender = function (value) {
    if (value == "true" || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var EscrituraRender = function (sender, registro, index) {
    if (sender == "true" || sender == 1) {
        return '<span class="ico-publico">&nbsp;</span>';
    }
    else {
        return '<span class="ico-privado">&nbsp;</span>';
    }
}

var LecturaRender = function (sender, registro, index) {
    if (sender == "true" || sender == 1) {
        return '<span class="ico-publico">&nbsp;</span>';
    }
    else {
        return '<span class="ico-privado">&nbsp;</span>';
    }
}

var CompletadoRender = function (sender, registro, index) {
    if (sender == "true" || sender == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}
