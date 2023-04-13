
// #region VARIABLES

var camposListadosReglas = [];
var camposListadosFiltro = [];

var jsEditar = array.find(x => x.key == "jsEditar").valor;
var jsAgregar = array.find(x => x.key == "jsAgregar").valor;
var jsEliminar = array.find(x => x.key == "jsEliminar").valor;
var jsDescargar = array.find(x => x.key == "jsDescargar").valor;
var jsDesactivar = array.find(x => x.key == "jsDesactivar").valor;
var jsActivar = array.find(x => x.key == "jsActivar").valor;
var jsAtencion = array.find(x => x.key == "jsAtencion").valor;
var jsFormularioNoValido = array.find(x => x.key == "jsFormularioNoValido").valor;
var jsMensajeProcesando = array.find(x => x.key == "jsMensajeProcesando").valor;
var jsMensajeEliminar = array.find(x => x.key == "jsMensajeEliminar").valor;
var jsEjecucion = array.find(x => x.key == "jsEjecucion").valor;
var jsEjecucionRealizada = array.find(x => x.key == "jsEjecucionRealizada").valor;
var jsEjecutar = array.find(x => x.key == "jsEjecutar").valor;
var jsCargando = array.find(x => x.key == "jsCargando").valor;
var jsFiltros = array.find(x => x.key == "jsFiltros").valor;
var jsDeseaEjecutarKPI = array.find(x => x.key == "jsDeseaEjecutarKPI").valor;
var jsError = array.find(x => x.key == "jsError").valor;
var strDetalleKPI = array.find(x => x.key == "strDetalleKPI").valor;
var jsEjecucionFallida = array.find(x => x.key == "jsEjecucionFallida").valor;
var strVersion = array.find(x => x.key == "strVersion").valor;
var strColumna = array.find(x => x.key == "strColumna").valor;
var strTabla = array.find(x => x.key == "strTabla").valor;
var jsGuardar = array.find(x => x.key == "jsGuardar").valor;
var jsNoSeCumpleRequisitos = array.find(x => x.key == "jsNoSeCumpleRequisitos").valor;

let DQKpi = "";
let DQKpiID = "";
let Clave = "";
let NombrePagina = "";

var PuntoCorteL = 900;
var PuntoCorteS = 512;

// #endregion

// #region DIRECT METHOD

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnTime.enable();
        App.btnDetalleKPI.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnTime.setTooltip(jsEjecutar);
        App.btnDescargar.setTooltip(jsDescargar);
        App.btnDetalleKPI.setTooltip(strDetalleKPI);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        if (seleccionado.IsAnd) {
            App.btnToggleCondition.setPressed(true);
        }
        else {
            App.btnToggleCondition.setPressed(false);
        }

        App.hdKPISeleccionado.setValue(seleccionado.DQKpiID);
        App.hdTablaModeloDatos.setValue(seleccionado.ClaveRecurso);
        App.storeDQKpisFiltros.reload();
        App.storeDQKpiGroups.reload();

        let registroSeleccionado = registro;
        let GridSeleccionado = App.grdmain;
        DQKpi = seleccionado.DQKpi;
        DQKpiID = seleccionado.DQKpiID;
        Clave = seleccionado.ClaveRecurso;
        NombrePagina = seleccionado.AliasModulo;
        enlaceRenderPnMore(seleccionado);
        cargarDatosPanelMoreInfoCalidad(registroSeleccionado, GridSeleccionado);

        cargarDatosPanelLateral(seleccionado.DQKpiID);


        //addEventoLinkPaginaKPI();

        var containerSize = Ext.get('CenterPanelMain').getWidth();

        if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
            if (selectedCol == 3) {
                App.pnFilters.hide();
                App.pnCondition.show();
            }
            else {
                App.pnFilters.show();
                App.pnCondition.hide();
            }
        }
        else if (containerSize > PuntoCorteL) {
            App.pnFilters.show();
            App.pnCondition.show();
        }

        App.gridRule.hide();
        App.cmpVersiones_txtSearch.setValue("");

        App.storeColumnasModelosDatos.reload();


        listenerLaunchAgregado = false;
        $(".dataGrd .ico-gotopageGrGrid").click(function (e) {
            SetEventClickLaunch(e);
        });
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.btnTime.disable();
    App.btnDetalleKPI.disable();

    App.pnFilters.hide();
    App.pnCondition.hide();
}

function VaciarFormulario() {

    App.formGestn.getForm().reset();

    App.ProgramadorKPI_txtFechaInicio.hide();
    App.ProgramadorKPI_txtPrevisualizar.hide();
    App.ProgramadorKPI_txtFechaFin.hide();
    App.ProgramadorKPI_txtCronFormat.hide();
    App.ProgramadorKPI_cmbDias.hide();
    App.ProgramadorKPI_txtDiaCadaMes.hide();
    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
    App.ProgramadorKPI_cmbMeses.hide();

    Ext.each(App.formGestn.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();



            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
                c.triggerWrap.addCls("itemForm-valid");
            }



            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);



                c.addListener("change", cambiarLiteral, false);
                c.addListener("validityChange", ValidarFormulario, false);



                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }



            if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
        }
    });
}

function cambiarLiteral() {
    App.btnGuardar.setText(jsGuardar);
    App.btnGuardar.setIconCls("");
    App.btnGuardar.removeCls("btnDisableClick");
    App.btnGuardar.addCls("btnEnableClick");
    App.btnGuardar.removeCls("animation-text");
}

function AgregarEditar() {
    VaciarFormulario();
    Agregar = true;
    App.winGestion.setTitle(jsAgregar);

    cambiarATapKPI(App.formKPI, 0);

    showLoadMask(App.grdmain, function (load) {
        if (load) {
            CargarStoresSerie([App.storeCategorias, App.storeSemaforos, App.storeDQTablasPaginas], function Fin(fin) {
                if (fin) {
                    load.hide();

                    Ext.each(App.winGestion.body.query('*'), function (value) {
                        Ext.each(value, function (item) {
                            var c = Ext.getCmp(item.id);
                            if (c != undefined && c.isFormField) {
                                c.reset();

                                if (c.triggerWrap != undefined) {
                                    c.triggerWrap.removeCls("itemForm-novalid");
                                }

                                if (!c.allowBlank && c.xtype != "checkboxfield" && c.xtype != "hidden") {
                                    c.addListener("change", anadirClsNoValido, false);
                                    c.addListener("focusleave", anadirClsNoValido, false);

                                    c.removeCls("ico-exclamacion-10px-red");
                                    c.addCls("ico-exclamacion-10px-grey");
                                }

                                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                                }
                            }
                        });
                    });
                    ValidarFormulario();
                    App.winGestion.show();
                }
            });
        }
    });

    App.cmbTablasPaginas.getTrigger(0).hide();
    App.cmbCategory.getTrigger(0).hide();
    App.cmbTraffic.getTrigger(0).hide();
}

function winGestionBotonGuardar() {
    if (App.formGestn.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {
    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
                    forzarCargaBuscadorPredictivo = true;
                    listenerLaunchAgregado = false;
                    App.storePrincipal.reload();

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditar() {
    if (registroSeleccionado(App.grdmain) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar);

    cambiarATapKPI(App.formKPI, 0);

    App.cmbTablasPaginas.getTrigger(0).show();
    App.cmbCategory.getTrigger(0).show();
    App.cmbTraffic.getTrigger(0).show();

    App.winGestion.show();

    App.winGestion.hide();

    showLoadMask(App.grdmain, function (load) {
        if (load) {
            TreeCore.MostrarEditar(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }

                        CargarStoresSerie([App.storeCategorias, App.storeSemaforos, App.storeDQTablasPaginas], function Fin(fin) {
                            if (fin) {
                                load.hide();

                                //REGION FRECUENCIAS MOSTRAR Y OCULTAR
                                if (App.ProgramadorKPI_cmbFrecuencia.value == 'NoSeRepite') {
                                    App.ProgramadorKPI_txtFechaFin.hide();
                                    App.ProgramadorKPI_txtFechaInicio.show();

                                    App.ProgramadorKPI_txtPrevisualizar.hide();
                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();

                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'Diario') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();

                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'Semanal') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();

                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'DiaLaborable') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();

                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'Semanal') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();

                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'Mensual') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    App.ProgramadorKPI_cmbDias.hide();
                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();


                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'SemanalCustom') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_cmbDias.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbMeses.hide();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.hide();
                                    App.ProgramadorKPI_txtDiaCadaMes.hide();
                                }
                                else if (App.ProgramadorKPI_cmbFrecuencia.value == 'MensualCustom') {
                                    App.ProgramadorKPI_txtFechaFin.show();
                                    App.ProgramadorKPI_txtFechaInicio.show();
                                    App.ProgramadorKPI_txtDiaCadaMes.show();
                                    App.ProgramadorKPI_cmbMeses.show();
                                    App.ProgramadorKPI_cmbTipoFrecuencia.show();
                                    App.ProgramadorKPI_txtPrevisualizar.show();
                                    document.getElementById('ProgramadorKPI_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 3;';

                                    //App.ProgramadorKPI_cmbMesInicio.hide();
                                    App.ProgramadorKPI_cmbDias.hide();

                                    if (App.ProgramadorKPI_cmbTipoFrecuencia.value != null) {
                                        //App.ProgramadorKPI_cmbMesInicio.show();
                                        App.ProgramadorKPI_cmbMeses.hide();
                                        App.ProgramadorKPI_cmbDias.hide();

                                    }

                                }
                    //

                                App.winGestion.show();
                            }
                        });
                    }
                });

        }
    });
}

function Activar() {
    if (registroSeleccionado(App.grdmain) && seleccionado != null) {
        ajaxActivar();
    }
}

function ajaxActivar() {

    TreeCore.Activar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    listenerLaunchAgregado = false;
                    App.storePrincipal.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Eliminar() {
    if (registroSeleccionado(App.grdmain) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    listenerLaunchAgregado = false;
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Refrescar() {
    CargarStoresSerie([App.storePrincipal], function Fin(fin) {
        if (fin) {
            listenerLaunchAgregado = false;
        }
    });
    //App.storePrincipal.reload();

    App.pnCondition.hide();
}

function ejecutarKPI(sender, registro, index) {
    TreeCore.ejecutarKPI(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result.Success) {
                        Ext.Msg.alert({
                            title: jsEjecucion,
                            icon: Ext.MessageBox.INFO,
                            msg: jsEjecucionRealizada
                        });
                    }
                    else {
                        Ext.Msg.alert({
                            title: jsEjecucion,
                            icon: Ext.MessageBox.WARNING,
                            msg: jsEjecucionFallida
                        });
                    }

                    Refrescar();
                    App.storeDQKpisMonitoring.reload();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function FormularioValidoTabCols(valid) {
    if (valid) {
        App.btnGuardarTabCols.setDisabled(false);
    }
    else {
        App.btnGuardarTabCols.setDisabled(true);
    }
}

//function FormularioValidoGestion(valid) {
//    if (valid) {
//        App.btnGuardar.setDisabled(false);

//        Ext.each(App.formGestn.body.query('*'), function (item) {
//            var c = Ext.getCmp(item.id);
//            if (c != undefined && c.isFormField && !c.hidden && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo")
//                && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
//                App.btnGuardar.setDisabled(true);
//            }
//        });
//    }
//    else {
//        App.btnGuardar.setDisabled(true);
//    }
//}

function cambiarConsultaKPI() {
    TreeCore.cambiarConsultaKPI(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDQKpiGroups.reload();
                    App.gridRule.hide();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

let listenerLaunchAgregado = false;
function addEventoLinkPaginaKPI() {
    if (!listenerLaunchAgregado && $(".linkColumn span.ico-gotopageGrGrid").length > 0) {
        listenerLaunchAgregado = true;

        $(".linkColumn span.ico-gotopageGrGrid").click(function (e) {
            SetEventClickLaunch(e);
        });
    }
}

function SetEventClickLaunch(e) {
    var nameKPI = e.currentTarget.getAttribute("data-nameKPI");
    var KPI = e.currentTarget.getAttribute("data-KPI");
    var tabla = e.currentTarget.getAttribute("data-tabla");

    Ext.Msg.alert(
        {
            title: jsEjecutar,
            msg: jsDeseaEjecutarKPI,
            buttons: Ext.Msg.YESNO,
            fn: launchKPIFromGrid,
            icon: Ext.MessageBox.QUESTION,
            nameKPI: nameKPI,
            KPI: KPI,
            tabla: tabla
        });
}

function launchKPIFromGrid(sender, registro, value) {
    if (sender == "yes" || sender == "si") {

        var nameKPI = value.nameKPI;
        var KPI = value.KPI;
        var tabla = value.tabla;

        TreeCore.GetRutaPagina(KPI,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        let url = result.Result;
                        let titulo = tabla + " - " + nameKPI;
                        let idTab = KPI + nameKPI;

                        addTab(parent.parent.App.tabPpal, idTab, titulo, "/" + url);
                    }
                    else {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsEjecucionFallida, buttons: Ext.Msg.OK });
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }

}

var enlaceRender = function (sender, registro, value) {

    if (value != undefined) {
        if (value.data.ClaveRecurso != "" && value.data.ClaveRecurso != undefined) {

            DQKpi = value.data.DQKpi;
            DQKpiID = value.data.DQKpiID;
            Clave = value.data.ClaveRecurso;

            return '<span class="ico-gotopageGrGrid" data-nameKPI="' + value.data.DQKpi + '" data-KPI="' + value.data.DQKpiID + '" data-tabla="' + value.data.AliasModulo + '">&nbsp;</span>';
        }
        else {
            return '<span>&nbsp;</span> ';
        }
    }
    else {
        if (Clave != "" && Clave != undefined) {
            return '<span class="ico-gotopageGrGrid" data-nameKPI="' + DQKpi + '" data-KPI="' + DQKpiID + '" data-tabla="' + NombrePagina + '">&nbsp;</span>';
        }
        else {
            return '<span>&nbsp;</span> ';
        }
    }
}

// #endregion

// #region DIRECT METHOD CONDITION

function Grid_RowSelectCondition(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadaCondition = datos;

        App.btnEditarCondition.enable();
        App.btnEliminarCondition.enable();
        App.btnActivarCondition.enable();

        App.btnEditarCondition.setTooltip(jsEditar);
        App.btnAnadirCondition.setTooltip(jsAgregar);
        App.btnEliminarCondition.setTooltip(jsEliminar);

        if (seleccionadaCondition.Activo) {
            App.btnActivarCondition.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarCondition.setTooltip(jsActivar);
        }

        if (seleccionadaCondition.IsAnd) {
            App.btnToggleRule.setPressed(true);
        }
        else {
            App.btnToggleRule.setPressed(false);
        }

        App.hdGroupSeleccionado.setValue(seleccionadaCondition.DQKpiGroupID);
        App.storeDQKpisGroupsRules.reload();
        App.gridRule.show();
    }
}

function DeseleccionarGrillaCondition() {

    App.GridRowSelectCondition.clearSelections();
    App.btnEditarCondition.disable();
    App.btnActivarCondition.disable();
    App.btnEliminarCondition.disable();
}

function FormularioValidoGestionCondition(valid) {
    if (valid) {
        App.btnGuardarCondition.setDisabled(false);

        Ext.each(App.formGestionCondition.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField && !c.hidden && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardarCondition.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardarCondition.setDisabled(true);
    }
}

function VaciarFormularioCondition() {
    Ext.each(App.formGestionCondition.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }
        }
    });
}

function AgregarEditarCondition() {
    VaciarFormularioCondition();
    AgregarCondition = true;
    App.winGestionCondicion.setTitle(jsAgregar);
    App.cmbGrupos.getTrigger(0).hide();

    showLoadMask(App.gridCondition, function (load) {
        if (load) {
            CargarStoresSerie([App.storeDQGroups], function Fin(fin) {
                if (fin) {
                    load.hide();
                    App.winGestionCondicion.show();
                }
            });
        }
    });
}

function winGestionBotonGuardarCondition() {
    if (App.formGestionCondition.getForm().isValid()) {
        ajaxAgregarEditarCondition();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarCondition() {
    TreeCore.AgregarEditarCondition(AgregarCondition,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionCondicion.hide();
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpiGroups.reload();
                    App.gridRule.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarCondition() {
    if (registroSeleccionado(App.gridCondition) && seleccionadaCondition != null) {
        ajaxEditarCondition();
    }
}

function ajaxEditarCondition() {
    VaciarFormularioCondition();
    AgregarCondition = false;
    App.winGestionCondicion.setTitle(jsEditar);

    App.cmbGrupos.getTrigger(0).show();

    showLoadMask(App.gridCondition, function (load) {
        if (load) {
            TreeCore.MostrarEditarCondition(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }

                        CargarStoresSerie([App.storeDQGroups], function Fin(fin) {
                            if (fin) {
                                App.winGestionCondicion.show();
                                load.hide();
                            }
                        });
                    }
                });

        }
    });
}

function ActivarCondition() {
    if (registroSeleccionado(App.gridCondition) && seleccionadaCondition != null) {
        ajaxActivarCondition();
    }
}

function ajaxActivarCondition() {

    TreeCore.ActivarCondition(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpiGroups.reload();
                    App.gridRule.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function EliminarCondition() {
    if (registroSeleccionado(App.gridCondition) && seleccionadaCondition != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarCondition,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarCondition(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarCondition({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpiGroups.reload();
                    App.gridRule.hide();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function RefrescarCondition() {
    App.storeDQKpiGroups.reload();
    App.gridRule.hide();
}

var handlePageSizeSelectCondition = function (item, records) {
    var curPageSizeCondition = App.storeDQKpiGroups.pageSize,
        wantedPageSizeCondition = parseInt(item.getValue(), 10);

    if (wantedPageSizeCondition != curPageSizeCondition) {
        App.storeDQKpiGroups.pageSize = wantedPageSizeCondition;
        App.storeDQKpiGroups.load();
    }
}

function cambiarConsultaCondition() {
    TreeCore.cambiarConsultaCondicion(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDQKpisGroupsRules.reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

// #endregion

// #region DIRECT METHOD RULE

function Grid_RowSelectRule(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadaRule = datos;

        App.btnEditarRule.enable();
        App.btnEliminarRule.enable();

        App.btnEditarRule.setTooltip(jsEditar);
        App.btnAnadirRule.setTooltip(jsAgregar);
        App.btnEliminarRule.setTooltip(jsEliminar);
    }
}

function DeseleccionarGrillaRule() {

    App.GridRowSelectRule.clearSelections();
    App.btnEditarRule.disable();
    App.btnEliminarRule.disable();
}

function FormularioValidoGestionReglas(valid) {
    if (valid) {
        App.btnGuardarReglas.setDisabled(false);

        Ext.each(App.formGestionReglas.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField && !c.hidden && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo")
                && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardarReglas.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardarReglas.setDisabled(true);
    }
}

function RefrescarRule() {
    App.storeDQKpisGroupsRules.reload();
}

function VaciarFormularioRule() {
    App.dateValorRule.hide();
    App.chkValorRule.hide();
    App.cmbTiposDinamicosReglas.hide();
    App.cmbMultiTiposDinamicosReglas.hide();
    App.cmbTablasPaginasAsociacionRule.hide();
    App.cmbTiposDinamicosReglas.setSelectedItems();
    App.cmbMultiTiposDinamicosReglas.setSelectedItems();
    App.cmbTablasPaginasAsociacionRule.setSelectedItems();
    App.cmbColumnasFiltro.setSelectedItems();
    App.numberValorRule.hide();
    App.btnToggleTabRule.hide();
    App.txtValorRule.hide();

    Ext.each(App.formGestionReglas.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }
        }
    });
}

function AgregarEditarRule() {
    VaciarFormularioRule();
    AgregarRule = true;
    App.winGestionRule.setTitle(jsAgregar);

    showLoadMask(App.gridRule, function (load) {
        if (load) {
            CargarStoresSerie([App.storeDQTablasPaginas, App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosReglas, App.storeTablasModelosDatos], function Fin(fin) {
                // CargarStoresSerie([App.storeDQTablasPaginas, App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosReglas], function Fin(fin) {
                if (fin) {
                    load.hide();
                    FormularioValidoGestionReglas(false);
                    App.winGestionRule.show();
                }
            });
        }
    });

    App.cmbColumnasReglas.setDisabled(true);
    App.cmbOperadorReglas.setDisabled(true);
    App.cmbTiposDinamicosReglas.hide();
    App.cmbMultiTiposDinamicosReglas.hide();
    App.cmbTablasPaginasAsociacionRule.hide();

    App.cmbTablasPaginasReglas.getTrigger(0).hide();
    App.cmbColumnasReglas.getTrigger(0).hide();
    App.cmbOperadorReglas.getTrigger(0).hide();
    App.txtValorRule.hide();
    App.cmbTiposDinamicosReglas.getTrigger(0).hide();
    App.cmbMultiTiposDinamicosReglas.getTrigger(0).hide();
    App.cmbTablasPaginasAsociacionRule.getTrigger(0).hide();
}

function winGestionBotonGuardarRule() {
    if (App.formGestionReglas.getForm().isValid()) {
        ajaxAgregarEditarRule();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarRule() {
    TreeCore.AgregarEditarRule(AgregarRule, App.GridRowSelectCondition.selected.items[0].data.DQKpiGroupID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionRule.hide();
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpisGroupsRules.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarRule() {
    if (registroSeleccionado(App.gridRule) && seleccionadaRule != null) {
        ajaxEditarRule();
    }
}

function ajaxEditarRule() {
    VaciarFormularioRule();
    AgregarRule = false;
    App.winGestionRule.setTitle(jsEditar);
    App.cmbColumnasReglas.setDisabled(false);
    App.cmbOperadorReglas.setDisabled(false);
    App.txtValorRule.hide();

    App.cmbTablasPaginasReglas.getTrigger(0).show();
    App.cmbColumnasReglas.getTrigger(0).show();
    App.cmbOperadorReglas.getTrigger(0).show();
    App.cmbTiposDinamicosReglas.getTrigger(0).show();
    App.cmbMultiTiposDinamicosReglas.getTrigger(0).show();
    App.cmbTablasPaginasAsociacionRule.getTrigger(0).show();

    showLoadMask(App.gridRule, function (load) {
        if (load) {
            TreeCore.MostrarEditarRule(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }

                        CargarStoresSerie([App.storeDQTablasPaginas, App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosReglas, App.storeTablasModelosDatos], function Fin(fin) {
                            //CargarStoresSerie([App.storeDQTablasPaginas, App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosReglas], function Fin(fin) {
                            if (fin) {
                                App.cmbColumnasReglas.setValue(App.hdColumnaReglaID.value);
                                App.cmbOperadorReglas.setValue(App.hdOperadorReglaID.value);
                                App.winGestionRule.show();
                                load.hide();
                            }
                        });

                        FormularioValidoGestionReglas(true);
                    }
                });

        }
    });
}

function EliminarRule() {
    if (registroSeleccionado(App.gridRule) && seleccionadaRule != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarRule,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarRule(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarRule({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpisGroupsRules.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

var handlePageSizeSelectRule = function (item, records) {
    var curPageSizeRule = App.storeDQKpisGroupsRules.pageSize,
        wantedPageSizeRule = parseInt(item.getValue(), 10);

    if (wantedPageSizeRule != curPageSizeRule) {
        App.storeDQKpisGroupsRules.pageSize = wantedPageSizeRule;
        App.storeDQKpisGroupsRules.load();
    }
}

function asignarTraduccionRule(sender, registro, index) {

    if (index != undefined && index.data.ClaveRecursoColumna != "" && index.data.ClaveRecursoColumna.includes("str")) {
        var valor = array.find(x => x.key == index.data.ClaveRecursoColumna).valor;

        if (index.data.NombreTabla != "" && index.data.NombreTabla.includes("str")) {
            var valorTabla = array.find(x => x.key == index.data.NombreTabla).valor;

            if (index.data.Valor == "1" && index.data.Codigo == "BOOLEAN") {
                return (valorTabla + ' WHERE ' + valor + ' ' + index.data.Operador + ' <span class="ico-defaultGrid">&nbsp;</span>');
            }
            else if (index.data.Valor == "0" && index.data.Codigo == "BOOLEAN") {
                return (valorTabla + ' WHERE ' + valor + ' ' + index.data.Operador + ' <span class="gen_Inactivo">&nbsp;</span>');
            }
            else if (index.data.Valor == "") {
                return (valorTabla + ' WHERE ' + valor + ' ' + index.data.Operador);
            }
            else {
                return (valorTabla + ' WHERE ' + valor + ' ' + index.data.Operador + ' ' + index.data.Valor);
            }
        }
    }
    else if (sender != "") {
        var valor = array.find(x => x.key == sender).valor;
        return valor;
    }
}

function cambiarComboRule(sender, registro, index) {
    ocultarCampos();

    if (App.btnToggleTabRule.pressed) {

        App.btnToggleTabRule.show();
        App.btnToggleTabRule.AriaLabel = strTabla;
        App.btnToggleTabRule.setTooltip(strTabla);

        App.cmbTablasPaginasAsociacionRule.show();
        App.cmbTablasPaginasAsociacionRule.setSelectedItems();
        App.cmbMultiTiposDinamicosReglas.hide();
        App.storeTablasModelosDatos.reload();
        App.cmbOperadorReglas.setDisabled(false);

        App.dateValorRule.allowBlank = true;
        App.cmbTiposDinamicosReglas.allowBlank = true;
        App.cmbTablasPaginasAsociacionRule.allowBlank = false;
        App.cmbMultiTiposDinamicosReglas.allowBlank = true;
        App.txtValorRule.allowBlank = true;
        App.numberValorRule.allowBlank = true;
    }
    else {

        App.btnToggleTabRule.show();
        App.btnToggleTabRule.AriaLabel = strColumna;
        App.btnToggleTabRule.setTooltip(strColumna);

        App.cmbMultiTiposDinamicosReglas.show();
        App.cmbTablasPaginasAsociacionRule.hide();
        App.cmbMultiTiposDinamicosReglas.setSelectedItems();
        App.storeTiposDinamicosReglas.reload();
        App.cmbOperadorReglas.setDisabled(false);

        App.dateValorRule.allowBlank = true;
        App.cmbTiposDinamicosReglas.allowBlank = true;
        App.cmbTablasPaginasAsociacionRule.allowBlank = true;
        App.cmbMultiTiposDinamicosReglas.allowBlank = false;
        App.txtValorRule.allowBlank = true;
        App.numberValorRule.allowBlank = true;
    }
}


// #endregion

// #region DIRECT METHOD FILTROS

function Grid_RowSelectFiltro(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoFiltro = datos;

        App.btnEditarFiltros.enable();
        App.btnEliminarFiltros.enable();
        App.btnActivarFiltros.enable();

        App.btnEditarFiltros.setTooltip(jsEditar);
        App.btnAnadirFiltros.setTooltip(jsAgregar);
        App.btnEliminarFiltros.setTooltip(jsEliminar);

        if (seleccionadoFiltro.Activo) {
            App.btnActivarFiltros.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarFiltros.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrillaFiltro() {

    App.GridRowSelectFiltro.clearSelections();
    App.btnEditarFiltros.disable();
    App.btnActivarFiltros.disable();
    App.btnEliminarFiltros.disable();
}

function FormularioValidoGestionFiltro(valid) {
    if (valid) {
        App.btnGuardarFiltro.setDisabled(false);

        Ext.each(App.formGestionFiltro.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField && !c.hidden && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo")
                && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardarFiltro.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardarFiltro.setDisabled(true);
    }
}

function RefrescarFiltro() {
    App.storeDQKpisFiltros.reload();
}

function VaciarFormularioFiltro() {
    App.dateValorFiltro.hide();
    App.chkValorFiltro.hide();
    App.cmbTiposDinamicosFiltro.hide();
    App.cmbMultiTiposDinamicosFiltro.hide();
    App.numberValorFiltro.hide();
    App.txtValorFiltro.hide();
    App.btnToggleFiltro.hide();
    App.cmbTablasPaginasAsociacionFiltro.hide();

    Ext.each(App.formGestionFiltro.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }
        }
    });
}

function AgregarEditarFiltro() {
    VaciarFormularioFiltro();
    AgregarFiltro = true;
    App.winGestionFiltro.setTitle(jsAgregar);

    showLoadMask(App.gridFiltros, function (load) {
        if (load) {
            CargarStoresSerie([App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosFiltros, App.storeTablasModelosDatos], function Fin(fin) {
                //CargarStoresSerie([App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosFiltros], function Fin(fin) {
                if (fin) {
                    load.hide();
                    FormularioValidoGestionFiltro(false);
                    App.winGestionFiltro.show();
                }
            });
        }
    });

    App.cmbOperadorFiltro.setDisabled(true);
    App.cmbTiposDinamicosFiltro.hide();
    App.cmbMultiTiposDinamicosFiltro.hide();
    App.cmbTablasPaginasAsociacionFiltro.hide();

    App.cmbColumnasFiltro.getTrigger(0).hide();
    App.cmbOperadorFiltro.getTrigger(0).hide();
    App.cmbTiposDinamicosFiltro.getTrigger(0).hide();
    App.cmbMultiTiposDinamicosFiltro.getTrigger(0).hide();
    App.cmbTablasPaginasAsociacionFiltro.getTrigger(0).hide();
}

function winGestionBotonGuardarFiltro() {
    if (App.formGestionFiltro.getForm().isValid()) {
        ajaxAgregarEditarFiltro();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarFiltro() {
    TreeCore.AgregarEditarFiltro(AgregarFiltro,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionFiltro.hide();
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpisFiltros.reload();
                }
            }
        });
}

function MostrarEditarFiltro() {
    if (registroSeleccionado(App.gridFiltros) && seleccionadoFiltro != null) {
        ajaxEditarFiltro();
    }
}

function ajaxEditarFiltro() {
    VaciarFormularioFiltro();
    AgregarFiltro = false;
    App.winGestionFiltro.setTitle(jsEditar);
    App.cmbOperadorFiltro.setDisabled(false);
    App.txtValorFiltro.hide();

    App.cmbColumnasFiltro.getTrigger(0).show();
    App.cmbOperadorFiltro.getTrigger(0).show();
    App.cmbTiposDinamicosFiltro.getTrigger(0).show();
    App.cmbTablasPaginasAsociacionFiltro.getTrigger(0).show();
    App.cmbMultiTiposDinamicosFiltro.getTrigger(0).show();

    showLoadMask(App.gridFiltros, function (load) {
        if (load) {
            TreeCore.MostrarEditarFiltro(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            CargarStoresSerie([App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosFiltros, App.storeTablasModelosDatos], function Fin(fin) {
                                //CargarStoresSerie([App.storeColumnasModelosDatos, App.storeTiposDatosOperadores, App.storeTiposDinamicosFiltros], function Fin(fin) {
                                if (fin) {
                                    App.cmbColumnasFiltro.setValue(App.hdColumnaFiltroID.value);
                                    App.cmbOperadorFiltro.setValue(App.hdOperadorFiltroID.value);
                                    FormularioValidoGestionFiltro(true);
                                    App.winGestionFiltro.show();
                                    load.hide();
                                }
                            });
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                });
        }
    });
}

function ActivarFiltro() {
    if (registroSeleccionado(App.gridFiltros) && seleccionadoFiltro != null) {
        ajaxActivarFiltro();
    }
}

function ajaxActivarFiltro() {

    TreeCore.ActivarFiltro(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpisFiltros.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function EliminarFiltro() {
    if (registroSeleccionado(App.gridFiltros) && seleccionadoFiltro != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarFiltro,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarFiltro(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarFiltro({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storeDQKpisFiltros.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

var handlePageSizeSelectFiltro = function (item, records) {
    var curPageSizeRule = App.storeDQKpiGroups.pageSize,
        wantedPageSizeRule = parseInt(item.getValue(), 10);

    if (wantedPageSizeRule != curPageSizeRule) {
        App.storeDQKpiGroups.pageSize = wantedPageSizeRule;
        App.storeDQKpiGroups.load();
    }
}

function asignarTraduccionFiltro(sender, registro, index) {

    if (index != undefined && index.data.ClaveRecursoColumna != "" && index.data.ClaveRecursoColumna.includes("str")) {
        var valor = array.find(x => x.key == index.data.ClaveRecursoColumna).valor;

        if (index.data.Valor == "1" && index.data.Codigo == "BOOLEAN") {
            return (valor + ' ' + index.data.Operador + ' <span class="ico-defaultGrid">&nbsp;</span>');
        }
        else if (index.data.Valor == "0" && index.data.Codigo == "BOOLEAN") {
            return (valor + ' ' + index.data.Operador + ' <span class="gen_Inactivo">&nbsp;</span>');
        }
        else if ((index.data.Valor == "" || index.data.Valor == "0") && index.data.Codigo != "NUMERICO") {
            return (valor + ' ' + index.data.Operador);
        }
        else if (index.data.IsDataTable && index.data.Valor.includes("str")) {
            var valorTabla = array.find(x => x.key == index.data.Valor).valor;
            return (valor + ' ' + index.data.Operador + ' ' + valorTabla);
        }
        else {
            return (valor + ' ' + index.data.Operador + ' ' + index.data.Valor);
        }

    }
    else if (sender != "") {
        var valor = array.find(x => x.key == sender).valor;
        return valor;
    }
}

function cambiarComboFiltro(sender, registro, index) {
    ocultarCamposFiltro();

    if (App.btnToggleFiltro.pressed) {

        App.btnToggleFiltro.show();
        App.btnToggleFiltro.AriaLabel = strTabla;
        App.btnToggleFiltro.setTooltip(strTabla);

        App.cmbTablasPaginasAsociacionFiltro.show();
        App.cmbMultiTiposDinamicosFiltro.hide();
        App.cmbTablasPaginasAsociacionFiltro.setSelectedItems();
        App.storeTablasModelosDatos.reload();
        App.cmbOperadorFiltro.setDisabled(false);

        App.dateValorFiltro.allowBlank = true;
        App.cmbTiposDinamicosFiltro.allowBlank = true;
        App.numberValorFiltro.allowBlank = true;
        App.txtValorFiltro.allowBlank = true;
        App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
        App.cmbTablasPaginasAsociacionFiltro.allowBlank = false;
    }
    else {

        App.btnToggleFiltro.show();
        App.btnToggleFiltro.AriaLabel = strColumna;
        App.btnToggleFiltro.setTooltip(strColumna);

        App.cmbMultiTiposDinamicosFiltro.show();
        App.cmbTablasPaginasAsociacionFiltro.hide();
        App.cmbMultiTiposDinamicosFiltro.setSelectedItems();
        App.storeTiposDinamicosFiltros.reload();
        App.cmbOperadorFiltro.setDisabled(false);

        App.dateValorFiltro.allowBlank = true;
        App.cmbTiposDinamicosFiltro.allowBlank = true;
        App.numberValorFiltro.allowBlank = true;
        App.txtValorFiltro.allowBlank = true;
        App.cmbMultiTiposDinamicosFiltro.allowBlank = false;
        App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
    }
}


// #endregion

// #region COMBOS

function SeleccionarTablasPaginas() {
    App.cmbTablasPaginas.getTrigger(0).show();
}

function RecargarTablasPaginas() {
    recargarCombos([App.cmbTablasPaginas]);
}

function SeleccionarTablasPaginasAsociacionFiltros() {
    App.cmbTablasPaginasAsociacionFiltro.getTrigger(0).show();
    FormularioValidoGestionFiltro(true);
}

function RecargarTablasPaginasAsociacionFiltros() {
    recargarCombos([App.cmbTablasPaginasAsociacionFiltro]);
}

function SeleccionarColumnasFiltro(sender, registro, index) {
    App.cmbColumnasFiltro.getTrigger(0).show();
    ocultarCamposFiltro();

    let tipoDatoFiltro = sender.selection.data.Codigo;

    if (tipoDatoFiltro != null && tipoDatoFiltro != "") {
        if (tipoDatoFiltro == "NUMERICO") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.numberValorFiltro.show();
        }
        else if (tipoDatoFiltro == "TEXTO") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.txtValorFiltro.show();
        }
        else if (tipoDatoFiltro == "FECHA") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.dateValorFiltro.show();
        }
        else if (tipoDatoFiltro == "BOOLEAN") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.chkValorFiltro.show();
        }
        else if (tipoDatoFiltro == "LISTA") {
            App.cmbTiposDinamicosFiltro.show();
            App.storeTiposDinamicosFiltros.reload();
            App.cmbOperadorFiltro.setDisabled(false);
        }
        else if (tipoDatoFiltro == "LISTAMULTIPLE") {
            App.cmbMultiTiposDinamicosFiltro.show();
            App.btnToggleFiltro.show();
            App.storeTiposDinamicosFiltros.reload();
            App.cmbOperadorFiltro.setDisabled(false);
        }
        else {
            App.cmbOperadorFiltro.setDisabled(false);
            App.txtValorFiltro.show();
        }
    }

    //App.cmbOperadorFiltro.setDisabled(false);
    RecargarOperadorFiltro();
}

function RecargarColumnasFiltro() {
    recargarCombos([App.cmbColumnasFiltro, App.cmbOperadorFiltro]);
    ocultarCamposFiltro();
    App.cmbOperadorFiltro.setDisabled(true);
    App.cmbTiposDinamicosFiltro.hide();
    App.cmbMultiTiposDinamicosFiltro.hide();
    App.cmbTablasPaginasAsociacionFiltro.hide();
}

function SeleccionarTiposDinamicosFiltros() {
    App.cmbTiposDinamicosFiltro.getTrigger(0).show();
    FormularioValidoGestionFiltro(true);
}

function RecargarTiposDinamicosFiltros() {
    recargarCombos([App.cmbTiposDinamicosFiltro]);
}

function SeleccionarMultiTiposDinamicosFiltros() {
    App.cmbMultiTiposDinamicosFiltro.getTrigger(0).show();
    FormularioValidoGestionFiltro(true);
}

function RecargarMultiTiposDinamicosFiltros() {
    recargarCombos([App.cmbMultiTiposDinamicosFiltro]);
}

function ocultarCamposFiltro() {
    App.cmbOperadorFiltro.setDisabled(true);
    App.dateValorFiltro.hide();
    App.chkValorFiltro.hide();
    App.cmbTiposDinamicosFiltro.hide();
    App.cmbMultiTiposDinamicosFiltro.hide();
    App.cmbTablasPaginasAsociacionFiltro.hide();
    App.numberValorFiltro.hide();
    App.txtValorFiltro.hide();
    App.btnToggleFiltro.hide();

    App.dateValorFiltro.setValue();
    App.numberValorFiltro.setValue();
    App.txtValorFiltro.setValue();
    App.cmbTiposDinamicosFiltro.setSelectedItems();
    App.cmbMultiTiposDinamicosFiltro.setSelectedItems();
    App.cmbTablasPaginasAsociacionFiltro.setSelectedItems();
}

function SeleccionarOperadorFiltro(sender, registro, index) {
    App.cmbOperadorFiltro.getTrigger(0).show();

    let requiereValorFiltro = sender.selection.data.RequiereValor;
    let requiereTablaFiltro = sender.selection.data.IsDataTable;
    let tipoDatoFiltro = sender.selection.data.Codigo;

    if (!requiereValorFiltro) {
        App.dateValorFiltro.hide();
        App.chkValorFiltro.hide();
        App.cmbTiposDinamicosFiltro.hide();
        App.cmbMultiTiposDinamicosFiltro.hide();
        App.cmbTablasPaginasAsociacionFiltro.hide();
        App.numberValorFiltro.hide();
        App.txtValorFiltro.hide();
        App.btnToggleFiltro.hide();

        App.dateValorFiltro.allowBlank = true;
        App.cmbTiposDinamicosFiltro.allowBlank = true;
        App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
        App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
        App.txtValorFiltro.allowBlank = true;
        App.numberValorFiltro.allowBlank = true;
    }
    else {
        if (tipoDatoFiltro == "NUMERICO") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.numberValorFiltro.show();

            App.dateValorFiltro.allowBlank = true;
            App.cmbTiposDinamicosFiltro.allowBlank = true;
            App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
            App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            App.txtValorFiltro.allowBlank = true;
            App.numberValorFiltro.allowBlank = false;

            anadirClsNoValidoDinamico(App.numberValorFiltro);
        }
        else if (tipoDatoFiltro == "TEXTO") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.txtValorFiltro.show();

            App.dateValorFiltro.allowBlank = true;
            App.cmbTiposDinamicosFiltro.allowBlank = true;
            App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
            App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            App.numberValorFiltro.allowBlank = true;
            App.txtValorFiltro.allowBlank = false;

            anadirClsNoValidoDinamico(App.txtValorFiltro);
        }
        else if (tipoDatoFiltro == "FECHA") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.dateValorFiltro.show();

            App.numberValorFiltro.allowBlank = true;
            App.cmbTiposDinamicosFiltro.allowBlank = true;
            App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
            App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            App.txtValorFiltro.allowBlank = true;
            App.dateValorFiltro.allowBlank = false;

            anadirClsNoValidoDinamico(App.dateValorFiltro);
        }
        else if (tipoDatoFiltro == "BOOLEAN") {
            App.cmbOperadorFiltro.setDisabled(false);
            App.chkValorFiltro.show();

            App.numberValorFiltro.allowBlank = true;
            App.cmbTiposDinamicosFiltro.allowBlank = true;
            App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
            App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            App.txtValorFiltro.allowBlank = true;
            App.dateValorFiltro.allowBlank = true;
        }
        else if (tipoDatoFiltro == "LISTA") {
            App.cmbTiposDinamicosFiltro.show();
            App.cmbTiposDinamicosFiltro.setSelectedItems();
            App.storeTiposDinamicosFiltros.reload();
            App.cmbOperadorFiltro.setDisabled(false);

            App.dateValorFiltro.allowBlank = true;
            App.numberValorFiltro.allowBlank = true;
            App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
            App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            App.txtValorFiltro.allowBlank = true;
            App.cmbTiposDinamicosFiltro.allowBlank = false;

            anadirClsNoValidoDinamico(App.cmbTiposDinamicosFiltro);
        }
        else if (tipoDatoFiltro == "LISTAMULTIPLE") {
            App.btnToggleFiltro.show();

            if (requiereTablaFiltro || App.btnToggleFiltro.pressed) {
                App.cmbTablasPaginasAsociacionFiltro.show();
                App.cmbMultiTiposDinamicosFiltro.hide();
                App.cmbTablasPaginasAsociacionFiltro.setSelectedItems();
                App.storeTablasModelosDatos.reload();
                App.cmbOperadorFiltro.setDisabled(false);

                App.dateValorFiltro.allowBlank = true;
                App.cmbTiposDinamicosFiltro.allowBlank = true;
                App.numberValorFiltro.allowBlank = true;
                App.txtValorFiltro.allowBlank = true;
                App.cmbMultiTiposDinamicosFiltro.allowBlank = true;
                App.cmbTablasPaginasAsociacionFiltro.allowBlank = false;
            }
            else {
                App.cmbMultiTiposDinamicosFiltro.show();
                App.cmbTablasPaginasAsociacionFiltro.hide();
                App.cmbMultiTiposDinamicosFiltro.setSelectedItems();
                App.storeTiposDinamicosFiltros.reload();
                App.cmbOperadorFiltro.setDisabled(false);

                App.dateValorFiltro.allowBlank = true;
                App.cmbTiposDinamicosFiltro.allowBlank = true;
                App.numberValorFiltro.allowBlank = true;
                App.txtValorFiltro.allowBlank = true;
                App.cmbMultiTiposDinamicosFiltro.allowBlank = false;
                App.cmbTablasPaginasAsociacionFiltro.allowBlank = true;
            }
        }
    }

    FormularioValidoGestionFiltro(true);
}

function RecargarOperadorFiltro() {
    recargarCombos([App.cmbOperadorFiltro, App.cmbTiposDinamicosFiltro, App.cmbMultiTiposDinamicosFiltro, App.cmbTablasPaginasAsociacionFiltro]);
    //recargarCombos([App.cmbOperadorFiltro, App.cmbTiposDinamicosFiltro, App.cmbMultiTiposDinamicosFiltro]);
    App.dateValorFiltro.setValue();
    App.numberValorFiltro.setValue();
    App.cmbTiposDinamicosFiltro.setSelectedItems();
    App.cmbMultiTiposDinamicosFiltro.setSelectedItems();
    App.cmbTablasPaginasAsociacionFiltro.setSelectedItems();
    FormularioValidoGestionFiltro(false);
}

function SeleccionarTablasPaginasRule() {
    App.cmbTablasPaginasReglas.getTrigger(0).show();
    App.storeColumnasModelosDatos.reload();
    App.cmbColumnasReglas.setDisabled(false);
}

function RecargarTablasPaginasRule() {
    recargarCombos([App.cmbTablasPaginasReglas, App.cmbColumnasReglas, App.cmbOperadorReglas]);
    App.cmbColumnasReglas.setDisabled(true);
    App.cmbOperadorReglas.setDisabled(true);
    App.cmbTiposDinamicosReglas.hide();
    App.cmbMultiTiposDinamicosReglas.hide();
    App.cmbTablasPaginasAsociacionRule.hide();
    App.btnToggleTabRule.hide();
    App.dateValorRule.hide();
    App.numberValorRule.hide();
    App.chkValorRule.hide();
}

function SeleccionarTablasPaginasAsociacionRule() {
    App.cmbTablasPaginasAsociacionRule.getTrigger(0).show();
    FormularioValidoGestionReglas(true);
}

function RecargarTablasPaginasAsociacionRule() {
    recargarCombos([App.cmbTablasPaginasAsociacionRule]);
}

function SeleccionarColumnasReglas(sender, registro, index) {
    App.cmbColumnasReglas.getTrigger(0).show();
    ocultarCampos();

    let tipoDato = sender.selection.data.Codigo;

    if (tipoDato != null && tipoDato != "") {
        if (tipoDato == "NUMERICO") {
            App.cmbOperadorReglas.setDisabled(false);
            App.numberValorRule.show();
        }
        else if (tipoDato == "TEXTO") {
            App.cmbOperadorReglas.setDisabled(false);
            App.txtValorRule.show();
        }
        else if (tipoDato == "FECHA") {
            App.cmbOperadorReglas.setDisabled(false);
            App.dateValorRule.setHidden(false);
        }
        else if (tipoDato == "BOOLEAN") {
            App.cmbOperadorReglas.setDisabled(false);
            App.chkValorRule.show();
        }
        else if (tipoDato == "LISTA") {
            App.cmbTiposDinamicosReglas.show();
            App.storeTiposDinamicosReglas.reload();
            App.cmbOperadorReglas.setDisabled(false);
        }
        else if (tipoDato == "LISTAMULTIPLE") {
            App.cmbMultiTiposDinamicosReglas.show();
            App.btnToggleTabRule.show();
            App.storeTiposDinamicosReglas.reload();
            App.cmbOperadorReglas.setDisabled(false);
        }
        else {
            App.cmbOperadorReglas.setDisabled(false);
            App.txtValorRule.show();
        }
    }
    //App.cmbOperadorReglas.setDisabled(false);
    RecargarOperadorReglas();
}

function RecargarColumnasReglas() {
    recargarCombos([App.cmbColumnasReglas, App.cmbOperadorReglas]);
    ocultarCampos();
    App.cmbOperadorReglas.setDisabled(true);
    App.cmbTiposDinamicosReglas.hide();
    App.cmbMultiTiposDinamicosReglas.hide();
    App.cmbTablasPaginasAsociacionRule.hide();
}

function ocultarCampos() {
    App.cmbOperadorReglas.setDisabled(true);
    App.dateValorRule.hide();
    App.cmbTiposDinamicosReglas.hide();
    App.cmbMultiTiposDinamicosReglas.hide();
    App.cmbTablasPaginasAsociacionRule.hide();
    App.numberValorRule.hide();
    App.chkValorRule.hide();
    App.txtValorRule.hide();
    App.btnToggleTabRule.hide();

    App.dateValorRule.setValue();
    App.numberValorRule.setValue();
    App.txtValorRule.setValue();
    App.cmbTiposDinamicosReglas.setSelectedItems();
    App.cmbMultiTiposDinamicosReglas.setSelectedItems();
    App.cmbTablasPaginasAsociacionRule.setSelectedItems();
}

function SeleccionarGrupo() {
    App.cmbGrupos.getTrigger(0).show();
}

function RecargarGrupo() {
    recargarCombos([App.cmbGrupos]);
}

function SeleccionarTiposDinamicosReglas() {
    App.cmbTiposDinamicosReglas.getTrigger(0).show();
    FormularioValidoGestionReglas(true);
}

function RecargarTiposDinamicosReglas() {
    recargarCombos([App.cmbTiposDinamicosReglas]);
}

function SeleccionarMultiTiposDinamicosReglas() {
    App.cmbMultiTiposDinamicosReglas.getTrigger(0).show();
    FormularioValidoGestionReglas(true);
}

function RecargarMultiTiposDinamicosReglas() {
    recargarCombos([App.cmbMultiTiposDinamicosReglas]);
}

function SeleccionarCategoria() {
    App.cmbCategory.getTrigger(0).show();
}

function RecargarCategoria() {
    recargarCombos([App.cmbCategory]);
}

function SeleccionarSemaforo() {
    App.cmbTraffic.getTrigger(0).show();
}

function RecargarSemaforo() {
    recargarCombos([App.cmbTraffic]);
}

function SeleccionarOperadorReglas(sender, registro, index) {
    App.cmbOperadorReglas.getTrigger(0).show();

    let requiereValor = sender.selection.data.RequiereValor;
    let requiereTablaRegla = sender.selection.data.IsDataTable;
    let tipoDatoRegla = sender.selection.data.Codigo;

    if (!requiereValor) {
        App.cmbTiposDinamicosReglas.hide();
        App.cmbMultiTiposDinamicosReglas.hide();
        App.cmbTablasPaginasAsociacionRule.hide();
        App.dateValorRule.hide();
        App.numberValorRule.hide();
        App.chkValorRule.hide();
        App.txtValorRule.hide();
        App.btnToggleTabRule.hide();

        App.dateValorRule.allowBlank = true;
        App.cmbTiposDinamicosReglas.allowBlank = true;
        App.cmbMultiTiposDinamicosReglas.allowBlank = true;
        App.cmbTablasPaginasAsociacionRule.allowBlank = true;
        App.txtValorRule.allowBlank = true;
        App.numberValorRule.allowBlank = true;
    }
    else {
        if (tipoDatoRegla == "NUMERICO") {
            App.cmbOperadorReglas.setDisabled(false);
            App.numberValorRule.show();

            App.dateValorRule.allowBlank = true;
            App.cmbTiposDinamicosReglas.allowBlank = true;
            App.cmbMultiTiposDinamicosReglas.allowBlank = true;
            App.cmbTablasPaginasAsociacionRule.allowBlank = true;
            App.txtValorRule.allowBlank = true;
            App.numberValorRule.allowBlank = false;

            anadirClsNoValidoDinamico(App.numberValorRule);
        }
        else if (tipoDatoRegla == "TEXTO") {
            App.cmbOperadorReglas.setDisabled(false);
            App.txtValorRule.show();

            App.dateValorRule.allowBlank = true;
            App.cmbTiposDinamicosReglas.allowBlank = true;
            App.cmbMultiTiposDinamicosReglas.allowBlank = true;
            App.cmbTablasPaginasAsociacionRule.allowBlank = true;
            App.txtValorRule.allowBlank = false;
            App.numberValorRule.allowBlank = true;

            anadirClsNoValidoDinamico(App.txtValorRule);
        }
        else if (tipoDatoRegla == "FECHA") {
            App.cmbOperadorReglas.setDisabled(false);
            App.dateValorRule.setHidden(false);

            App.dateValorRule.allowBlank = false;
            App.cmbTiposDinamicosReglas.allowBlank = true;
            App.cmbMultiTiposDinamicosReglas.allowBlank = true;
            App.cmbTablasPaginasAsociacionRule.allowBlank = true;
            App.txtValorRule.allowBlank = true;
            App.numberValorRule.allowBlank = true;

            anadirClsNoValidoDinamico(App.dateValorRule);
        }
        else if (tipoDatoRegla == "BOOLEAN") {
            App.chkValorRule.setDisabled(false);
            App.chkValorRule.show();

            App.dateValorRule.allowBlank = true;
            App.cmbTiposDinamicosReglas.allowBlank = true;
            App.cmbMultiTiposDinamicosReglas.allowBlank = true;
            App.cmbTablasPaginasAsociacionRule.allowBlank = true;
            App.txtValorRule.allowBlank = true;
            App.numberValorRule.allowBlank = true;
        }
        else if (tipoDatoRegla == "LISTA") {
            App.cmbTiposDinamicosReglas.show();
            App.cmbTiposDinamicosReglas.setSelectedItems();
            App.storeTiposDinamicosReglas.reload();
            App.cmbOperadorReglas.setDisabled(false);

            App.dateValorRule.allowBlank = true;
            App.cmbTiposDinamicosReglas.allowBlank = false;
            App.cmbMultiTiposDinamicosReglas.allowBlank = true;
            App.cmbTablasPaginasAsociacionRule.allowBlank = true;
            App.txtValorRule.allowBlank = true;
            App.numberValorRule.allowBlank = true;

            anadirClsNoValidoDinamico(App.cmbTiposDinamicosReglas);
        }
        else if (tipoDatoRegla == "LISTAMULTIPLE") {
            App.btnToggleTabRule.show();

            if (requiereTablaRegla || App.btnToggleTabRule.pressed) {
                App.cmbTablasPaginasAsociacionRule.show();
                App.cmbTablasPaginasAsociacionRule.setSelectedItems();
                App.cmbMultiTiposDinamicosReglas.hide();
                App.storeTablasModelosDatos.reload();
                App.cmbOperadorReglas.setDisabled(false);

                App.dateValorRule.allowBlank = true;
                App.cmbTiposDinamicosReglas.allowBlank = true;
                App.cmbTablasPaginasAsociacionRule.allowBlank = false;
                App.cmbMultiTiposDinamicosReglas.allowBlank = true;
                App.txtValorRule.allowBlank = true;
                App.numberValorRule.allowBlank = true;
            }
            else {
                App.cmbMultiTiposDinamicosReglas.show();
                App.cmbTablasPaginasAsociacionRule.hide();
                App.cmbMultiTiposDinamicosReglas.setSelectedItems();
                App.storeTiposDinamicosReglas.reload();
                App.cmbOperadorReglas.setDisabled(false);

                App.dateValorRule.allowBlank = true;
                App.cmbTiposDinamicosReglas.allowBlank = true;
                App.cmbTablasPaginasAsociacionRule.allowBlank = true;
                App.cmbMultiTiposDinamicosReglas.allowBlank = false;
                App.txtValorRule.allowBlank = true;
                App.numberValorRule.allowBlank = true;
            }
        }
    }

    FormularioValidoGestionReglas(true);
}

function anadirClsNoValidoDinamico(c) {
    if (c != undefined && c.isFormField) {
        c.reset();

        if (c.triggerWrap != undefined) {
            c.triggerWrap.removeCls("itemForm-novalid");
        }

        if (!c.allowBlank && c.xtype != "checkboxfield") {
            c.addListener("change", anadirClsNoValido, false);
            c.addListener("focusleave", anadirClsNoValido, false);

            c.removeCls("ico-exclamacion-10px-red");
            c.addCls("ico-exclamacion-10px-grey");
        }
    }
}

function RecargarOperadorReglas() {
    recargarCombos([App.cmbOperadorReglas, App.cmbTiposDinamicosReglas, App.cmbMultiTiposDinamicosReglas, App.cmbTablasPaginasAsociacionRule]);
    //recargarCombos([App.cmbOperadorReglas, App.cmbTiposDinamicosReglas, App.cmbMultiTiposDinamicosReglas]);
    App.dateValorRule.setValue();
    App.numberValorRule.setValue();
    App.cmbMultiTiposDinamicosReglas.setSelectedItems();
    App.cmbTiposDinamicosReglas.setSelectedItems();
    App.cmbTablasPaginasAsociacionRule.setSelectedItems();
    FormularioValidoGestionReglas(false);
}

function beforeLoadCmbColumna(sender, registro, index) {
    camposListadosReglas.forEach(camp => App.cmbColumnasReglas.removeByValue(camp.Id));
}

function beforeLoadCmbColumnaFiltro(sender, registro, index) {
    camposListadosFiltro.forEach(camp => App.cmbColumnasFiltro.removeByValue(camp.Id));
}

// #endregion

// #region PANEL LATERAL

var ColorRender = function (sender, registro, value) {
    let valor = "";
    if (value != undefined) {
        valor = value.data.Colour;
    }
    else {
        valor = sender;
    }

    if (valor == "green") {
        return '<span class="green">&nbsp;</span>'
    }
    else if (valor == "yellow") {
        return '<span class="yellow">&nbsp;</span>'
    }
    else if (valor == "red") {
        return '<span class="red">&nbsp;</span>'
    }
    else if (valor == "green-empty") {
        return '<span class="green-empty">&nbsp;</span>'
    }
    else if (valor == "yellow-empty") {
        return '<span class="yellow-empty">&nbsp;</span>'
    }
    else if (valor == "red-empty") {
        return '<span class="red-empty">&nbsp;</span>'
    }
    else if (valor == "error") {
        return '<span class="gen_Inactivo">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var CheckRender = function (sender, registro, value) {

    var datos = value.data;
    var html = '';

    html += "<div class='d-flx' >";
    html += "<ul class='ulboxes'>";
    html += "<li class='liNot'>";
    html += "<div class=cntNot>";
    html += "<h5>" + strVersion.split(':')[0] + ' ' + datos.Version + "</h5>";
    html += "<p style='font-weight: 500; margin-bottom: 0 !important;'>" + datos.FechaEjecucion.toLocaleDateString() + ' | ' + datos.FechaEjecucion.toLocaleTimeString() + "</p > ";
    html += "<p style='font-weight: 500; color: #9999A1; margin-bottom: 0 !important;'>" + datos.NombreCompleto + "</p ></div > ";
    html += "<div style='float: right; text-align-last: center;'>";
    html += "<h6>" + datos.Current + "</h6>";

    if (datos.Activa) {
        html += "<h6 class='ico-defaultGrid'></h6>";
    }

    html += "</div>";
    html += "</li>";
    html += "</ul>";
    html += "</div>";

    return html;
}

function showKPIByID(idKpi) {
    App.tabKPI.click();
    let data = App.grdmain.getRowsValues();

    data.forEach(function (row, index) {
        if (row.DQKpiID == idKpi) {
            App.grdmain.setSelection(index);
        }
    });

}

function RpanelBTNAlert() {
    App.pnNotificationsFull.show();
    App.pnNotesFull.hide();
}

function RpanelBTNNotes() {
    App.pnNotesFull.show();
    App.pnNotificationsFull.hide();
}

function cambiarActivo(sender, registro, index) {

    TreeCore.desactivarVersion(sender.record.get('DQKpiMonitoringID'),
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDQKpisMonitoring.reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

var SetToggleValue = function (column, cmp, record) {

    var bActiva = record.data.Activa;

    if (record.data.Ultima) {
        cmp.hide();
    }
    else {
        if (bActiva) {
            cmp.setPressed(true);
            cmp.setTooltip(jsDesactivar);
        }
        else {
            cmp.setPressed(false);
            cmp.setTooltip(jsActivar);
        }

        cmp.updateLayout();
    }
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

function Grid_RowSelectVersiones(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadaVersion = datos;
    }
}

function RefrescarVersiones() {
    App.storeDQKpisMonitoring.reload();
}

function hideAsideRCalidad(panel) {
    listenerLaunchAgregado = false;
    App.btnCollapseAsRClosed.show();
    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (panel != null) {
        App.WrapFilterControls.hide();
        App.WrapGestionColumnas.hide();

        switch (panel) {
            case "panelFiltros":
                App.WrapFilterControls.show();
                App.pnNotesFull.show();
                App.pnNotificationsFull.hide();
                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();
                break;

            case "panelColumnas":
                App.WrapGestionColumnas.show();
                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();
                break;

            case "panelMore":
                App.pnMoreInfo.show();
                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();
                //addEventoLinkPaginaKPI();
                break;

            case "panelFiltrosToggle":
                if (asideR.collapsed == false) {
                    btn.style.transform = 'rotate(-180deg)';
                    App.pnAsideR.collapse();
                    break;
                }
                else {
                    btn.style.transform = 'rotate(0deg)';
                    App.WrapFilterControls.show();
                    App.pnAsideR.expand();
                    break;
                }
                break;
        }
    }
    else {
        if (asideR.collapsed == false) {
            btn.style.transform = 'rotate(-180deg)';
            App.pnAsideR.collapse();
        }
        else {
            btn.style.transform = 'rotate(0deg)';
            App.pnAsideR.expand();
        }
    }

    GridColHandler();

    window.dispatchEvent(new Event('resizePlantilla'));
}



function hideAsidePnMore(sender, registro, index) {
    listenerLaunchAgregado = false;
    App.btnCollapseAsRClosed.show();
    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed != "rigth") {
        App.WrapFilterControls.hide();
        App.WrapGestionColumnas.hide();


        App.pnMoreInfo.show();
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();

    }
    else {
        if (asideR.collapsed == false) {
            btn.style.transform = 'rotate(-180deg)';
            App.pnAsideR.collapse();
        }
        else {
            btn.style.transform = 'rotate(0deg)';
            App.pnAsideR.expand();
        }
    }
    GridColHandler();
    NombrePagina = sender.$widgetRecord.data.AliasModulo;
    enlaceRenderPnMore(sender.$widgetRecord.data);
    cargarDatosPanelMoreInfoGrid(sender.$widgetRecord, sender.$widgetColumn);

    listenerLaunchAgregado = false;
    $(".ico-gotopageGrGrid").click(function (e) {
        SetEventClickLaunch(e);
    });



    window.dispatchEvent(new Event('resizePlantilla'));
}
var enlaceRenderPnMore = function (data) {

    if (data != undefined) {
        if (data.ClaveRecurso != "" && data.ClaveRecurso != undefined) {

            DQKpi = data.DQKpi;
            DQKpiID = data.DQKpiID;
            Clave = data.ClaveRecurso;

            return '<span class="ico-gotopageGrGrid" data-nameKPI="' + data.DQKpi + '" data-KPI="' + data.DQKpiID + '" data-tabla="' + data.AliasModulo + '">&nbsp;</span>';
        }
        else {
            return '<span>&nbsp;</span> ';
        }
    }
    else {
        if (Clave != "" && Clave != undefined) {
            return '<span class="ico-gotopageGrGrid" data-nameKPI="' + DQKpi + '" data-KPI="' + DQKpiID + '" data-tabla="' + NombrePagina + '">&nbsp;</span>';
        }
        else {
            return '<span>&nbsp;</span> ';
        }
    }


}


function cargarDatosPanelMoreInfoCalidad(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    grid = Grid.columnManager.getColumns();

    for (var columna of grid) {
        if (columna.cls != 'col-More' && columna.cls != "excluirPnInfo") {
            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                if (registro.get(columna.dataIndex) != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
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
function cargarDatosPanelMoreInfoGrid(registro, columnaID) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    grid = columnaID.gridRef.columnManager.getColumns();

    for (var columna of grid) {
        if (columna.cls != 'col-More' && columna.cls != "excluirPnInfo") {
            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                if (registro.get(columna.dataIndex) != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
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

// #endregion

// #region Control Slider y BOTONES SLIDER

var selectedCol = 0;
var isOnMobC = 0;

function SliderMove(NextOrPrev) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();

    var btnPrevSldr = Ext.getCmp('btnPrev');
    var btnNextSldr = Ext.getCmp('btnNext');

    var pnmain = Ext.getCmp('grdmain');
    var col1 = Ext.getCmp('pnFilters');
    var col2 = Ext.getCmp('pnCondition');

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
    }

    //SELECCION EN 1  PANEL
    else {

        if (NextOrPrev == 'Next' && selectedCol == 1) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            col2.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Next' && selectedCol == 2) {
            pnmain.hide();
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }

        else if (NextOrPrev == 'Prev' && selectedCol == 3) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }


    }


}

function ControlSlider(sender) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();
    listenerLaunchAgregado = false;

    var pnmain = Ext.getCmp('grdmain');
    var col2 = Ext.getCmp('pnFilters');
    var col3 = Ext.getCmp('pnCondition');
    var tbsliders = Ext.getCmp('tbSliders');
    var btnPrevSldr = Ext.getCmp('btnPrev');
    var btnNextSldr = Ext.getCmp('btnNext');


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        col2.show();
        col3.show();
        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();



        if (selectedCol == 3) {
            col2.hide();
            col3.show();
        }
        else {
            col2.show();
            col3.hide();
        }
        isOnMobC = 0;




    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();
        col3.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true || col2.hidden == true || col3.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false && col3.hidden == true) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}

function selectRowDetalle() {
    var containerSize = Ext.get('CenterPanelMain').getWidth();

    var pnmain = Ext.getCmp('grdmain');
    var col1 = Ext.getCmp('pnFilters');
    var col2 = Ext.getCmp('pnCondition');


    var btnPrevSldr = Ext.getCmp('btnPrev');
    var btnNextSldr = Ext.getCmp('btnNext');


    if (containerSize < PuntoCorteS) {
        pnmain.hide();
        col1.show();
        col2.hide();
        selectedCol = 2;

        btnPrevSldr.enable();
        btnNextSldr.enable();
    }

}

// #endregion

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
    var asideR = Ext.getCmp('pnAsideR');


    if (winsize < 520 && asideR.collapsed == false) {
        App.CenterPanelMain.hide();
        App.pnAsideR.setWidth(winsize);
    }
    else {
        App.CenterPanelMain.show();
        App.pnAsideR.setWidth(380);

    }


});


// #endregion

// #region DISEÑO

function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnNotificationsFull.hide();
    listenerLaunchAgregado = false;
    if (btn == 'pnNotificationsFull') {
        App.pnNotesFull.hide();
    }

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}

var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');
    listenerLaunchAgregado = false;
    if (spPnLite == 0) {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 0;

    }
}

function showwinAddTab() {
    App.winAddTabFilter.show();

}

function showwinSaveQF() {
    App.winSaveQF.show();

}

function StyleOnResize(sender) {
    var tbFiltros = document.getElementsByClassName("tlbGridRes");
    var ruta = getIdComponente(sender);

    //CONTROL PARA CUANDO NO HAYA BOTONES, SOLO COMBO FILTROS
    var GrupoBtnFilters = document.getElementsByClassName("GrupoBtnFilters");
    var BotonesVisibles = GrupoBtnFilters[0].style.display;

    var cmbMisfiltros = document.getElementsByClassName("cmbMisfiltros");
    var cmbProjects = document.getElementsByClassName("cmbProjects");

    if (BotonesVisibles == "none") {
        cmbMisfiltros[0].classList.add("cmbMisfiltrosNoBtns");
    }
    if (BotonesVisibles == "none") {
        cmbProjects[0].classList.add("cmbMisfiltrosNoBtns");
    }

    // ------------------------------------------

    for (i = 0; i < tbFiltros.length; i++) {

        if (tbFiltros[i] != null) {

            if (tbFiltros[i].clientWidth < 612) {

                tbFiltros[i].classList.add("tlbGridResMid");

            }
            else {

                tbFiltros[i].classList.remove("tlbGridResMid");

            }

            if (tbFiltros[i].clientWidth < 460) {

                tbFiltros[i].classList.add("tlbGridResMini");

            }
            else {
                tbFiltros[i].classList.remove("tlbGridResMini");

            }
            App[ruta + 'tbFiltros'].updateLayout();

        }

    }

}

// #endregion

// #region TABS

function habilitaLnk(vago) {
    let ct = document.getElementById('tbNavNAside-targetEl');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}

function showForms(who) {
    var LNo = who.textEl;
    $(".calidadTabPanel").hide();
    $($(".calidadTabPanel")[who.tabID]).show();
    App.lbltituloPrincipal.setText(who.text);
    let iframe = "";

    habilitaLnk(LNo);

    switch (who.id) {
        case 'tabKPI':
            App.storePrincipal.reload();
            break;

        case 'tabResults':
            iframe = $("iframe[name='ctMain2_IFrame']")[0];
            iframe.src = iframe.src;
            break;

        case 'tabReport':
            iframe = $("iframe[name='ctMain3_IFrame']")[0];
            iframe.src = iframe.src;
            break;
    }
}

function prepareTabs() {
    $(".calidadTabPanel").hide();
    $($(".calidadTabPanel")[0]).show();
}
// #endregion

// #region Frecuencias

function showFormsFormKPI(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    cambiarATapKPI(sender, index);
}

function cambiarATapKPI(sender, index) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";

    var arrayBotones = Ext.getCmp("cntNavVistasFormKPI").ariaEl.getFirstChild().getFirstChild().dom.children;


    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = document.getElementsByClassName("winGestion-paneles");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();
        }
        Ext.getCmp(panels[index].id).show();
        Ext.getCmp(panels[index].id).up('panel').update();

        if (index == 1) {
            App.btnGuardar.addCls("btnDisableClick");
        }
    }

    App.btnGuardar.addCls(classBtnActivo);
    App.btnGuardar.removeCls(classBtnDesactivo);
}

function ValidarFormulario(sender, valido, aux) {

    try {

        var formPanel = App.formGestn;
        App.btnGuardar.setDisabled(false);


        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");

        Ext.each(formPanel.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && !c.hidden && c.isFormField &&
                (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                    (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
            }
        });


        var classMandatory = "ico-formview-mandatory";
        var i = 0;

        // #region Validación Tabs
        var i = 0;
        Ext.each(App.formKPI.query('*'), function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App.lnkFormKPI.removeCls(classMandatory);
        }
        else {
            App.lnkFormKPI.addCls(classMandatory);
        }

        i = 0;
        let Programador = [
            App.ProgramadorKPI_cmbFrecuencia,
            App.ProgramadorKPI_txtFechaInicio,
            App.ProgramadorKPI_txtPrevisualizar,
            App.ProgramadorKPI_txtFechaFin,
            App.ProgramadorKPI_txtCronFormat,
            App.ProgramadorKPI_cmbDias,
            App.ProgramadorKPI_txtDiaCadaMes,
            App.ProgramadorKPI_cmbTipoFrecuencia,
            App.ProgramadorKPI_cmbMeses
        ];
        Ext.each(Programador, function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App.lnkFormFrecuencia.removeCls(classMandatory);
        }
        else {
            App.lnkFormFrecuencia.addCls(classMandatory);
        }

        // #endregion

    } catch (e) {

    }
}
 //#endregion