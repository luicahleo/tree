// #region DISEÑO

var nombreGridServicios;
var registroServicios;
var panelAbierto = false;

function hideAsideR(panel, grid, registro) {

    App.btnCollapseAsRClosed.show();
    App.pnAsideR.expand();

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');
    btn.style.transform = 'rotate(0deg)';

    if (panel != null) {

        App.WrapService.hide();

        switch (panel) {

            case "WrapService":

                App.WrapService.show();
                App.pnServicios.show()
                App.pnAsideR.expand();

                App.btnEstados.show();
                App.btnLink.show();
                App.btnPrecios.show();
                App.MenuNavPnServicios.updateLayout();

                App.btnClausulas.removeCls('btnDisableClick');
                App.btnClausulas.addCls('btnEnableClick');

                displayMenu('pnServicios', grid, registro);

                break;
            case "WrapPrices":

                App.WrapService.show();
                App.pnClausulas.show();

                App.pnAsideR.expand();

                App.btnEstados.hide();
                App.btnLink.hide();
                App.btnPrecios.hide();
                App.MenuNavPnServicios.updateLayout();

                App.btnClausulas.removeCls('btnEnableClick');
                App.btnClausulas.addCls('btnDisableClick');

                displayMenu('pnClausulas', grid, registro);

                break;
            case "WrapCatalog":

                App.WrapService.show();
                App.pnMoreInfoService.hide();
                App.pnMoreInfoCatalog.show();
                App.pnMoreInfoPack.hide();

                App.pnAsideR.expand();

                App.btnEstados.hide();
                App.btnLink.hide();
                App.btnPrecios.hide();
                App.MenuNavPnServicios.updateLayout();

                App.btnClausulas.addCls('btnDisableClick');

                displayMenu('pnMoreInfoCatalog', grid, registro);

                break;
            case "WrapPack":

                App.WrapService.show();
                App.pnMoreInfoService.hide();
                App.pnMoreInfoCatalog.hide();
                App.pnMoreInfoPack.show();

                App.pnAsideR.expand();

                App.btnEstados.hide();
                App.btnLink.hide();
                App.btnPrecios.hide();
                App.btnClausulas.hide();
                App.MenuNavPnServicios.updateLayout();

                App.btnClausulas.addCls('btnDisableClick');

                displayMenu('pnMoreInfoPack', grid, registro);

                break;
            case "WrapServiciosCatalog":

                if (registro.$widgetRecord.data.CoreProductCatalogServicioID != "") {
                    App.WrapService.show();
                    App.pnMoreInfoCatalog.hide();
                    App.pnMoreInfoService.show();
                    App.pnMoreInfoPack.hide();

                    App.pnAsideR.expand();

                    App.btnEstados.hide();
                    App.btnLink.hide();
                    App.btnPrecios.hide();
                    App.btnClausulas.hide();
                    App.MenuNavPnServicios.updateLayout();

                    App.btnClausulas.addCls('btnDisableClick');

                    displayMenu('pnMoreInfoService', grid, registro);
                }
                else {
                    App.WrapService.show();
                    App.pnMoreInfoService.hide();
                    App.pnMoreInfoCatalog.hide();
                    App.pnMoreInfoPack.show();

                    App.pnAsideR.expand();

                    App.btnEstados.hide();
                    App.btnLink.hide();
                    App.btnPrecios.hide();
                    App.MenuNavPnServicios.updateLayout();

                    App.btnClausulas.addCls('btnDisableClick');

                    displayMenu('pnMoreInfoPack', grid, registro);
                }

                break;
            case "WrapServicePacks":

                App.WrapService.show();
                App.pnServicios.show()
                App.pnAsideR.expand();

                App.btnEstados.hide();
                App.btnClausulas.hide();
                App.btnLink.hide();
                App.btnPrecios.hide();
                App.MenuNavPnServicios.updateLayout();

                App.btnEstados.addCls('btnDisableClick');

                displayMenu('pnServicios', grid, registro);
        }

    }
    GridColHandler();

    window.dispatchEvent(new Event('resize'));

    resizeGridInfo();
}

function resizeGridInfo() {
    App.pnServicios.maxHeight = window.innerHeight - 121;
    App.pnServicios.minHeight = window.innerHeight - 121;
    App.pnServicios.updateLayout();
}

var PrecioRender = function (sender, registro, value) {
    let valor = "";

    if (value != undefined) {
        valor = value.data.CantidadCatalogServicio * value.data.Precio + ' ' + value.data.Simbolo + ' / ' + value.data.Identificador;
    }

    if (valor != null) {
        return '<span>' + valor + '</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

function displayMenu(btn, grid, registro) {

    //ocultar todos los paneles
    var name = '#' + btn;
    let tabla;
    let registroSeleccionado;
    App.pnPrecios.hide();
    App.pnMoreInfoCatalog.hide();
    App.pnMoreInfoPack.hide();
    App.pnClausulas.hide();
    App.pnLink.hide();
    App.pnMoreInfoService.hide();

    if (btn == 'pnPrecios') {
        App.pnPrecios.show();
        App.pnServicios.hide();
        App.pnClausulas.hide();
        App.pnMoreInfoCatalog.hide();
        App.pnLink.hide();
        App.pnMoreInfoService.hide();
        App.pnMoreInfoPack.hide();

        App.storeCoreProductCatalogServiciosAsignados.reload();
    }
    else if (btn == 'pnServicios') {
        tabla = document.getElementById('bodyTablaInfoElementos');
        App.pnServicios.show();
        App.pnPrecios.hide();
        App.pnClausulas.hide();
        App.pnMoreInfoCatalog.hide();
        App.pnLink.hide();
        App.pnMoreInfoService.hide();
        App.pnMoreInfoPack.hide();
    }
    else if (btn == 'pnClausulas') {
        tabla = document.getElementById('bodyTablaInfoClausulas');
        App.pnClausulas.show();
        App.pnPrecios.hide();
        App.pnServicios.hide();
        App.pnLink.hide();

        if (registro != undefined) {
            if (registro != undefined && registro.$widgetRecord != undefined) {
                registroSeleccionado = registro.$widgetRecord;
            }
            else {
                registroSeleccionado = registro;
            }
        }
        else {
            btn = 'pnClausulasServicios';
        }
    }
    else if (btn == 'pnMoreInfoCatalog') {

        if (registro != undefined && registro.$widgetRecord != undefined) {
            registroSeleccionado = registro.$widgetRecord;
        }
        else {
            registroSeleccionado = registro;
        }

        tabla = document.getElementById('bodyTablaInfoElementosCatalog');
        cargarDatosPanelMoreInfoCatalogo(registroSeleccionado, grid, tabla);
        App.pnMoreInfoCatalog.show();
        App.pnPrecios.hide();
        App.pnClausulas.hide();
        App.pnServicios.hide();
        App.pnLink.hide();
        App.pnMoreInfoService.hide();
        App.pnMoreInfoPack.hide();
    }
    else if (btn == 'pnMoreInfoPack') {

        if (registro != undefined && registro.$widgetRecord != undefined) {
            registroSeleccionado = registro.$widgetRecord;
        }
        else {
            registroSeleccionado = registro;
        }

        tabla = document.getElementById('bodyTablaInfoElementosPack');
        cargarDatosPanelMoreInfoPack(registroSeleccionado, grid, tabla);
        App.pnMoreInfoPack.show();
        App.pnMoreInfoCatalog.hide();
        App.pnPrecios.hide();
        App.pnClausulas.hide();
        App.pnServicios.hide();
        App.pnLink.hide();
        App.pnMoreInfoService.hide();
    }
    else if (btn == 'pnMoreInfoService') {
        if (registro != undefined && registro.$widgetRecord != undefined) {
            registroSeleccionado = registro.$widgetRecord;
        }
        else {
            registroSeleccionado = registro;
        }
        cargarDatosPanelMoreInfoServicioCatalogo(registroSeleccionado, grid);
        App.pnMoreInfoService.show();
        App.pnMoreInfoCatalog.hide();
        App.pnMoreInfoPack.hide();
        App.pnPrecios.hide();
        App.pnClausulas.hide();
        App.pnServicios.hide();
        App.pnLink.hide();
    }
    else if (btn == 'pnLink') {
        tabla = document.getElementById('bodyTablaInfoLink');
        App.pnLink.show();
        App.pnPrecios.hide();
        App.pnClausulas.hide();
        App.pnMoreInfoCatalog.hide();
        App.pnMoreInfoPack.hide();
        App.pnServicios.hide();
        App.pnMoreInfoService.hide();
    }

    if (btn != undefined && btn != null) {
        TreeCore.PanelLateralServicios(btn,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Success, buttons: Ext.Msg.OK });
                    }
                    else {

                        lista = JSON.parse(result.Result);

                        if (btn == 'pnServicios') {
                            if (registro != undefined && registro.$widgetRecord != undefined) {
                                cargarDatosPanelMoreInfoServicio(registro.$widgetRecord, grid);
                            }
                            else {
                                cargarDatosPanelMoreInfoServicio(registroServicios, nombreGridServicios);
                            }

                        }
                        else if (btn == 'pnClausulas' || btn == 'pnClausulasServicios') {
                            if (grid != undefined) {
                                cargarDatosPanelMoreInfoCatalogo(registroSeleccionado, grid, tabla);
                            }
                            else {
                                html = '';
                                tabla.innerHTML = "";

                                for (var i in lista) {
                                    html += '<div class="tmpCol-td" colspan = "3">';
                                    html += '<div><span class="lblGrd"> Code : </span > <span class="dataGrd">' + lista[i].CodigoProductCatalog + '</span></div>';
                                    html += '<div><span class="lblGrd"> Company : </span > <span class="dataGrd">' + lista[i].NombreEntidad + '</span></div>';
                                    html += '</div>';
                                }

                                tabla.innerHTML = html;
                            }
                        }
                        else if (btn == 'pnLink') {
                            html = '';
                            tabla.innerHTML = "";

                            for (var i in lista) {
                                html += '<div class="tmpCol-td" colspan = "3">';
                                html += '<div><span class="lblGrd"> Object : </span > <span class="dataGrd">' + lista[i].Nombre + '</span></div>';
                                html += '</div>';
                            }

                            tabla.innerHTML = html;
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

function cargarDatosPanelLateral(seleccionadoID) {
    if (seleccionadoID != '') {
        TreeCore.RecargarPanelLateral(seleccionadoID,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                },
                eventMask: {
                    showMask: true,
                }
            });
    }
}

function cargarDatosPanelMoreInfoServicio(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    grid = Grid.columnManager.getColumns();

    for (var columna of grid) {
        if (columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                if (registro.get(columna.dataIndex) != undefined && columna.xtype != 'datecolumn') {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
                }
                else if (registro.get(columna.dataIndex) != undefined && columna.xtype == 'datecolumn') {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex).toLocaleDateString() + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
                }
            }
            else {
                if (columna.tooltip != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
                }
                else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
            }
        }
    }

    tabla.innerHTML = html;
}

function cargarDatosPanelMoreInfoGridServicio(tabla, lista, grid) {
    html = '';
    tabla.innerHTML = "";
    let nombreGrid;

    //if (slickCreado) {
    //    $('.single-item').slick('unslick');
    //}

    //$('.single-item').empty();

    if (grid != undefined) {
        nombreGrid = grid.columnManager.columns;

        for (var i = 0; i < lista.length; i++) {

            html += '<div class="tmpCol-td" colspan = "3">';

            for (var prop of Object.keys(lista[i])) {
                for (var columna of nombreGrid) {
                    if (columna.cls != 'col-More' && columna.cls != "excluirPnInfo") {
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

        //if (label != undefined && label != "" && label != null && lista.length != 0) {
        //    $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
        //        label.setText(1 + '/' + lista.length);
        //    });

        //    $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
        //        label.setText((nextSlide + 1) + '/' + lista.length);
        //    });
        //}

        //$('.single-item').slick({
        //    dots: false,
        //    infinite: true,
        //    arrows: true,
        //    slidesToShow: 1,
        //    slidesToScroll: 1,
        //});

        //slickCreado = true;
    }
}

function cargarDatosPanelMoreInfoPack(registro, Grid, tabla) {

    if (tabla != "" && tabla == undefined) {
        tabla = document.getElementById('bodyTablaInfoElementosPack');
    }

    html = '';
    let grid;
    tabla.innerHTML = "";

    grid = Grid.data.items;
    var dato;
    for (var columna of grid) {

        dato = columna.data;
        if (dato.CoreProductCatalogPackID == registro.data.CoreProductCatalogPackID) {
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsCodigo + ' : </span><span class="dataGrd">' + dato.Codigo + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsNombre + ' : </span><span class="dataGrd">' + dato.Nombre + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsEntidad + ' : </span><span class="dataGrd">' + dato.NombreEntidad + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsTipo + ' : </span><span class="dataGrd">' + dato.Identificador + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaModificacion + ' : </span><span class="dataGrd">' + dato.FechaModificacion + '</span></td></tr>';

            tabla.innerHTML = html;
            break;
        }


    }



}

function cargarDatosPanelMoreInfoCatalogo(registro, Grid, tabla) {

    if (tabla == undefined) {
        tabla = document.getElementById('bodyTablaInfoElementosCatalog');
    }

    html = '';
    let grid;
    tabla.innerHTML = "";

    grid = Grid.data.items;
    var dato;
    for (var columna of grid) {

        dato = columna.data;
        if (dato.CoreProductCatalogID == registro.data.CoreProductCatalogID) {
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsCodigo + ' : </span><span class="dataGrd">' + dato.Codigo + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsNombre + ' : </span><span class="dataGrd">' + dato.NombreProductCatalog + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsEntidad + ' : </span><span class="dataGrd">' + dato.NombreEntidad + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsTipo + ' : </span><span class="dataGrd">' + dato.NombreCoreProductCatalogTipo + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaInicio + ' : </span><span class="dataGrd">' + dato.FechaInicioVigencia.toLocaleDateString() + '</span></td></tr>';
            if (dato.FechaFinVigencia != null) {
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaFin + ' : </span><span class="dataGrd">' + dato.FechaFinVigencia.toLocaleDateString() + '</span></td></tr>';
            }
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsMoneda + ' : </span><span class="dataGrd">' + dato.Moneda + '</span></td></tr>';
            if (dato.Tipo != null && dato.Tipo != 1) {
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaInicioRevision + ' : </span><span class="dataGrd">' + dato.FechaInicioReajuste.toLocaleDateString() + '</span></td></tr>';
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaProximaRevision + ' : </span><span class="dataGrd">' + dato.FechaProximaReajuste.toLocaleDateString() + '</span></td></tr>';
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsFechaFinRevision + ' : </span><span class="dataGrd">' + dato.FechaFinReajuste.toLocaleDateString() + '</span></td></tr>';

            }
            switch (dato.Tipo) {
                case 2:
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsInflacion + ' : </span><span class="dataGrd">' + dato.Inflacion + '</span></td></tr>';

                    break;
                case 3:
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsPeriodicidad + ' : </span><span class="dataGrd">' + dato.Periodicidad + '</span></td></tr>';
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsValor + ' : </span><span class="dataGrd">' + dato.CantidadFija + '</span></td></tr>';

                    break;
                case 4:
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsPeriodicidad + ' : </span><span class="dataGrd">' + dato.Periodicidad + '</span></td></tr>';
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsPorcentaje + ' : </span><span class="dataGrd">' + dato.PorcentajeFijo + '</span></td></tr>';

                    break;
            }
            if (dato.FechaCreaccion != null) {
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsCreaccion + ' : </span><span class="lastDateMoreInfo"><span class="dataGrd">' + dato.UsuarioCreador + '</span><span class="dataGrd">' + dato.FechaCreaccion.toLocaleDateString() + '</span></span></td></tr>';

            }
            if (dato.FechaUltimaModificacion != null) {
                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsUltimaModificacion + ' : </span><span class="lastDateMoreInfo"><span class="dataGrd">' + dato.UsuarioModificador + '</span><span class="dataGrd">' + dato.FechaUltimaModificacion.toLocaleDateString() + '</span></span></td></tr>';

            }


            tabla.innerHTML = html;
            break;
        }


    }



}

function cargarDatosPanelMoreInfoServicioCatalogo(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementosService');
    let grid;
    tabla.innerHTML = "";

    grid = Grid.data.items;
    var dato;
    for (var columna of grid) {

        dato = columna.data;
        if (dato.CoreProductCatalogServicioAsignadoID == registro.data.CoreProductCatalogServicioAsignadoID) {
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsCodigo + ' : </span><span class="dataGrd">' + dato.CodigoProductCatalogServicio + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsNombre + ' : </span><span class="dataGrd">' + dato.NombreCatalogServicio + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsEntidad + ' : </span><span class="dataGrd">' + dato.NombreEntidad + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsTipo + ' : </span><span class="dataGrd">' + dato.NombreCatalogServicioTipo + '</span></td></tr>';
            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + jsPrecio + ' : </span><span class="dataGrd">' + dato.Precio + ' ' + dato.Simbolo + '</span></td></tr>';
            tabla.innerHTML = html;
            break;
        }


    }



    //grid = Grid.columnManager.getColumns();

    //for (var columna of grid) {
    //    if (columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
    //        if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
    //            if (registro.get(columna.dataIndex) != undefined && columna.xtype != 'datecolumn') {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
    //            }
    //            else if (registro.get(columna.dataIndex) != undefined && columna.xtype == 'datecolumn') {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex).toLocaleDateString() + '</span></td></tr>';
    //            }
    //            else {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
    //            }
    //        }
    //        else {
    //            if (columna.tooltip != undefined) {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
    //            }
    //            else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
    //            }
    //            else {
    //                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
    //            }
    //        }
    //    }
    //}

    //tabla.innerHTML = html;
}

var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');
    listenerLaunchAgregado = false;
    if (spPnLite == 0) {
        spPnLite = 1;
    }
    else {
        spPnLite = 0;
    }
    App.btnCollapseAsRClosed.hide();
}

function habilitaLnk(vago) {
    let ct = document.getElementById('tbNavNAside-targetEl');

    if (ct != undefined) {
        let aLinks = ct.querySelectorAll('a');

        aLinks.forEach(function (itm) {
            itm.classList.remove("navActivo");
        });

        if (vago.cls != undefined) {
            document.getElementById(vago.id).classList.add("navActivo");
            document.getElementById(vago.id).childNodes[1].classList.add("navActivo");
        }
    }
}

function showForms(who) {

    showLoadMask(App.CenterPanelMain, function (load) {
        $(".calidadTabPanel").hide();
        $($(".calidadTabPanel")[who.tabID]).show();
        let iframe = "";

        habilitaLnk(who);
        load.hide();
    });
}

var DefaultRender = function (value) {
    if (value == "true" || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

// #endregion

// #region DIRECT METHOD

var desdeTab = false;
var desdeTabPrecios = false;
var desdeTabTraduccion = false;

function filtrarServiciosPorBuscador(sender) {
    if (!desdeTab) {
        App.hdStringBuscador.setValue(sender.value);
    }
    desdeTab = false;
}

function filtrarPreciosPorBuscador(sender) {
    if (!desdeTabPrecios) {
        App.hdStringBuscadorPrecios.setValue(sender.value);
    }
    desdeTabPrecios = false;
}

function filtrarTraduccionPorBuscador(sender) {
    if (!desdeTabTraduccion) {
        App.hdStringBuscadorTraduccion.setValue(sender.value);
    }
    desdeTabTraduccion = false;
}

// #endregion

//#region ICONOS BUSCADOR

function FieldSearch(sender, registro) {
    var iconClear = sender.getTrigger("_trigger2");
    iconClear.hide();
}

function FiltrarColumnas(sender, registro) {

    var iconSearch = sender.getTrigger("_trigger1");
    var iconClear = sender.getTrigger("_trigger2");
    var text = sender.getRawValue();

    if (Ext.isEmpty(text, false)) {
        iconClear.hide();
        iconSearch.show();
    }

    if (!Ext.isEmpty(text, false)) {
        iconSearch.hide();
        iconClear.show();
    }
}

//#endregion