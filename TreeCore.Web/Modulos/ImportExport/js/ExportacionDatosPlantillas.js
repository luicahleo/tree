
var VisorTreePClosed = false;
var ctmain1ForcedHide = false;
var Collapse = "Collapse";
var Expand = "Expand";
var spPnLiteExpanded = 0;
var tempModelo = null;
var itCelda = 0;
var itTipofila = 0;
var idsColCategory = [];
var celdaIDAdd = null;
var reglaTransFormacionIDedit = null;
var cronstrue = window.cronstrue;
var camposListados = [];




// #region Diseño

var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 600;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        App.btnCloseShowVisorTreeP.disable();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltrosYSliders.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        App.btnCloseShowVisorTreeP.enable();
        loadPanels();
    }
}

function loadPanels() {
    var btnclose = Ext.getCmp(['btnCloseShowVisorTreeP']);

    if (bShowOnlySecundary) {
        App.ctMain1.hide();
        btnclose.setIconCls('ico-moverow-gr');
    }
    else {
        App.ctMain2.show();
        App.ctMain1.show();
        btnclose.setIconCls('ico-hide-menu');
    }
}

function loadPanelByBtns(pressedBtn) {

    // CHECK FOR A PRESSED BTN
    if (pressedBtn != "") {
        if (pressedBtn == "Next") {
            iSelectedPanel++;
        }
        else {
            iSelectedPanel--;
        }
    }

    // CHECK FOR DISABLED BUTTONS
    if (iSelectedPanel == 1) {
        App.btnPrev.enable();
        App.btnNext.disable();
    }
    else {
        App.btnPrev.disable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    if (iSelectedPanel == 1) {
        App.ctMain1.hide();
        App.ctMain2.show();
    }

}

function showOnlySecundary() {
    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

//var stSldr = 0;
//var stSldrMbl = 0;

//window.addEventListener('resize', function () {
//    var resol = window.innerWidth;
//    if (resol > 1024) {
//        //App.ctMain2.show();
//        App.ctMain1.show();
//        //App.btnPrevSldr.enable();
//        //App.btnNextSldr.disable();
//        stSldr = 0;
//    }

//});

setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    winFormResize();
    //App.TreePanelCategorias.update();
    //App.pnConfigurador.update();

}, 1000);

function winFormResize() {

    var res = window.innerWidth;
    try {
        //100% del tamaño de la página = bodyHeight

        //var porcentaje = (90 * bodyHeight) / 100;
        //porcentaje = Math.round(porcentaje)

        // 90% de la página = porcentaje

        //App.winGestion.height = porcentaje + "px";

        if (res > 1024) {
            App.winGestion.setWidth(862)
        }

        if (res <= 1024 && res > 670) {
            App.winGestion.setWidth(445)
            App.winGestion.setHeight(450)
        }

        if (res <= 670) {
            App.winGestion.setWidth(425)
            App.winGestion.setHeight(430)
        }
    } catch (e) {

    }
}

window.addEventListener('resize', function () {
    winFormResize();
});

function resizeCenterPanel(sender) {
    SetMaxHeightSuperiorCenter(App.CenterPanelMain);
}

function SetMaxHeightSuperiorCenter(sender) {
    var tamPadre = sender.up().getHeight();

    sender.setMinHeight(tamPadre - 40);
    sender.setMaxHeight(tamPadre - 40);
    sender.updateLayout();
}

// #endregion

// #region Grid

function PlantillaTemplateColumn(valor, b) {

    let record = b.record.data;
    let textoCron = "";
    try {
        if (record.CronFormat != null && record.CronFormat != "_") {
            textoCron = cronstrue.toString(record.CronFormat, { locale: App.hdLocale.value });
        } else if (record.CronFormat != null && record.CronFormat == "_") {
            textoCron = jsSoloUnaVez;
        }
    }
    catch (ex) {
        textoCron = "";
    }

    return `<div  Class='grid-dinamico-template'>
                <div Class='line-grid-template'>
                    <div Class='Nombre-Template'>${record.Nombre}</div>
                    <div Class='fGrey texto-template ico-launchGrid' data-cronformat="${record.CronFormat}">${textoCron}</div>
                </div>
                <div Class='line-grid-template'>
                    <div Class='modulo-template'>${record.ClaveRecurso}</div>
                </div>
            </div>`;
}

var onShow = function (toolTip, grid) {
    var view = grid.getView(),
        record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
        data = ((record.CronFormat != null && record.CronFormat != "_") ? cronstrue.toString(record.data.CronFormat, { locale: App.hdLocale.value }) : "");

    if (data.length > 58) {
        toolTip.show();
        toolTip.update(data);
    }
    else {
        toolTip.close();
    }
};
// #endregion

// #region Gestion
function AgregarEditar() {
    VaciarFormulario();
    App.winAddTemplateDataExport.setTitle(parent.jsAgregar);
    Agregar = true;

    App.cmbTablasModelosDatos.enable();
    App.ProgramadorExportar_cmbFrecuencia.getTrigger(0).hide();
    recargarCombos([App.cmbTablasModelosDatos], function () { });
    App.cmbColumnasModeloDatosForm.enable();

    cambiarATapExportar(App.formExport, 0);

    Ext.each(App.winAddTemplateDataExport.body.query('*'), function (value) {
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
    App.winAddTemplateDataExport.show();
    App.btnGuardar.disable();
}



//function ValidarFormulario(sender, valido) {
//    if (valido && (!sender.allowBlank || !sender.hidden)) {
//        App.btnGuardar.enable();
//        Ext.each(App.winAddTemplateDataExport.body.query('*'), function (item) {
//            var c = Ext.getCmp(item.id);
//            if (c && c.isFormField && !c.isValid()) {
//                App.btnGuardar.disable();
//            }
//        });
//    } else {
//        App.btnGuardar.disable();
//    }
//}

function MostrarEditar() {
    if (seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winAddTemplateDataExport.setTitle(parent.jsEditar);

    cambiarATapExportar(App.formExport, 0);

    App.winAddTemplateDataExport.show();

    App.winAddTemplateDataExport.hide();

    var combos = [App.cmbTablasModelosDatos, App.cmbColumnasModeloDatosForm, App.ProgramadorExportar_cmbFrecuencia, App.ProgramadorExportar_cmbTipoFrecuencia];
    recargarCombos(combos, function () {

        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    forzarCargaBuscadorPredictivo = true;

                    App.ProgramadorExportar_cmbFrecuencia.getTrigger(0).show();

                    //REGION FRECUENCIAS MOSTRAR Y OCULTAR
                    if (App.ProgramadorExportar_cmbFrecuencia.value == 'NoSeRepite') {
                        App.ProgramadorExportar_txtFechaInicio.show();

                        App.ProgramadorExportar_txtPrevisualizar.hide();
                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();

                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'Diario') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();

                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'Semanal') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();

                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'DiaLaborable') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();

                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'Semanal') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();

                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'Mensual') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';
                        
                        App.ProgramadorExportar_cmbDias.hide();
                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.getTrigger(0).show();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();


                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'SemanalCustom') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_cmbDias.show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbMeses.hide();
                        App.ProgramadorExportar_cmbTipoFrecuencia.hide();
                        App.ProgramadorExportar_txtDiaCadaMes.hide();
                    }
                    else if (App.ProgramadorExportar_cmbFrecuencia.value == 'MensualCustom') {
                        App.ProgramadorExportar_txtFechaFin.show();
                        App.ProgramadorExportar_txtFechaInicio.show();
                        App.ProgramadorExportar_txtDiaCadaMes.show();
                        App.ProgramadorExportar_cmbMeses.show();
                        App.ProgramadorExportar_cmbTipoFrecuencia.show();
                        App.ProgramadorExportar_cmbTipoFrecuencia.getTrigger(0).show();
                        App.ProgramadorExportar_txtPrevisualizar.show();
                        document.getElementById('ProgramadorExportar_txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 3;';

                        //App.ProgramadorExportar_cmbMesInicio.hide();
                        App.ProgramadorExportar_cmbDias.hide();

                        if (App.ProgramadorExportar_cmbTipoFrecuencia.value != null) {
                            //App.ProgramadorExportar_cmbMesInicio.show();
                            App.ProgramadorExportar_cmbMeses.hide();
                            App.ProgramadorExportar_cmbDias.hide();

                        }

                    }
                    //
                }
            });
    });

}



function Activar() {
    if (seleccionado != null) {
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
                forzarCargaBuscadorPredictivo = true;
                Refrescar();
            }
        });
}

function Eliminar() {
    if (seleccionado != null) {
        Ext.Msg.alert(
            {
                title: parent.jsEliminar,
                msg: parent.jsMensajeEliminar,
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
                    let containerConfigTemplate = $("#containerConfigTemplate");
                    containerConfigTemplate.empty();
                    forzarCargaBuscadorPredictivo = true;
                    Refrescar();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

var isPreview = false;
function btnPreview() {

    if (isPreview) {
        loadPreview();
        App.pnConfiguration.hide();
        App.pnPreview.show();
    }
    else {
        App.pnPreview.hide();
        App.pnConfiguration.show();
    }
    isPreview = !isPreview;
}

function loadPreview() {
    App.storegrdPreview.removeAll();
    TreeCore.ShowPreview({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                result.Result.forEach(item => {
                    App.storegrdPreview.add(item);
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

function DeseleccionarGrilla() {
    App.GridRowSelectTemplate.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnPreview.disable();
    App.btnShowFilter.disable();
    App.btnHistorico.disable();

    isPreview = false;
    btnPreview();
}

function Refrescar() {
    DeseleccionarGrilla();
    App.storeExportacionDatosPlantillas.reload();
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    limpiarConfiguracionPlantilla(true);

    isPreview = false;
    btnPreview();

    // #region filtro
    App.tagsContainerTemplate.removeAll();
    App.storeFiltros.clearData()
    App.storeFiltros.reload();
    App.cmbField.reset();
    ocultarYResetearCampos();
    // #endregion

    if (datos != null) {
        App.hdTablaseleccionadaID.setValue(datos.TablaModeloDatosID);
        App.hdTablaModeloDatoForm.setValue(datos.TablaModeloDatosID);
        App.hdColumnaSeleccionadaID.setValue(datos.ColumnaModeloDatoID);

        let stores = [App.storeColumnaCategoria, App.storeColumnasModeloDatos];
        CargarStoresSerie(stores, function () {
            mostrarConfiguracionPlantilla(datos);
            App.storeCampos.reload();

            FiltrosAplicados = [];
            filtros = [];

            if (datos.Filtro) {
                let filtroObj = JSON.parse(datos.Filtro);
                MostrarEditarFiltroGuardado(filtroObj);
                AñadirFiltro(filtroObj);
            }
            App.storeCoreExportacionDatosPlantillasHistoricos.reload();

        });

        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnDescargar.enable();
        App.btnPreview.enable();
        App.btnShowFilter.enable();
        App.btnHistorico.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDescargar.setTooltip(jsDescargarPlantilla);
        App.btnHistorico.setTooltip(jsHistorico);

        App.btnShowFilter.setTooltip(jsFiltros);
        App.btnPreview.setTooltip(jsVistaPrevia);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.lblTabs.setText(seleccionado.Nombre);

    }
}

function winAddTemplateDataExportGuardar() {
    if (App.formTemplate.getForm().isValid()) {
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
                    forzarCargaBuscadorPredictivo = true;
                    App.winAddTemplateDataExport.hide();
                    Refrescar();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeExportacionDatosPlantillas.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeExportacionDatosPlantillas.pageSize = wantedPageSize;
        App.storeExportacionDatosPlantillas.load();
    }
}

function btnActivos() {
    if (App.btnActivos.pressed) {
        App.colActivo.hide();
    }
    else {
        App.colActivo.show();
    }
    Refrescar();
}

// #endregion

// #region Add / Edit

function SeleccionarColumnasModeloDatosForm() {
    App.cmbColumnasModeloDatosForm.getTrigger(0).show();
}

function RecargarColumnasModeloDatosForm() {
    recargarCombos([App.cmbColumnasModeloDatosForm]);
}

function SeleccionarTablaModeloDato(sender, index, registro) {
    App.hdTablaModeloDatoForm.setValue(index[0].data.TablaModeloDatosID);
    RecargarColumnasModeloDatosForm();
    App.cmbTablasModelosDatos.getTrigger(0).show();
}

function RecargarTablaModeloDato() {
    App.hdTablaModeloDatoForm.setValue(0);
    recargarCombos([App.cmbTablasModelosDatos, App.cmbColumnasModeloDatosForm], function () {
        
    });
}

function SeleccionarstoreFrecuencias() {
    App.cmbFrecuencias.getTrigger(0).show();
}

function RecargarstoreFrecuencias() {
    recargarCombos([App.cmbFrecuencias]);
}

function VaciarFormulario() {
    App.formTemplate.getForm().reset();

    App.ProgramadorExportar_txtFechaInicio.hide();
    App.ProgramadorExportar_txtPrevisualizar.hide();
    App.ProgramadorExportar_txtFechaFin.hide();
    App.ProgramadorExportar_txtCronFormat.hide();
    App.ProgramadorExportar_cmbDias.hide();
    App.ProgramadorExportar_txtDiaCadaMes.hide();
    App.ProgramadorExportar_cmbTipoFrecuencia.hide();
    //App.ProgramadorExportar_cmbMesInicio.hide();
    App.ProgramadorExportar_cmbMeses.hide();

    Ext.each(App.formTemplate.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();



            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
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

// #endregion

// #region Buscador
forzarCargaBuscadorPredictivo = true;
function buscador() {

}

function limpiar() {
    App.txtBuscador.setValue("");
    App.txtBuscador.getTrigger(0).hide();
}

function FiltrarColumnas(sender, registro) {

    App.txtBuscador.getTrigger(0).show();
    App.hdStringBuscador.setValue(sender.value);
    App.storeExportacionDatosPlantillas.reload();
    
}
// #endregion

// #region Configuración Plantilla
function limpiarConfiguracionPlantilla(loader) {
    idsColCategory = [];
    let containerConfigTemplate = $("#containerConfigTemplate");
    containerConfigTemplate.empty();

    if (loader) {
        containerConfigTemplate.addClass("x-mask-msg-text");
    }
    else {
        containerConfigTemplate.removeClass("x-mask-msg-text");
    }
}

function mostrarConfiguracionPlantilla(datos) {
    limpiarConfiguracionPlantilla(true);

    if (datos) {
        TreeCore.GetModeloPlantilla({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    limpiarConfiguracionPlantilla(false);
                    tempModelo = result.Result;

                    printTableModel(tempModelo);
                }
            }
        });
    }
}

function printTableModel(model) {
    limpiarConfiguracionPlantilla(false);
    let tabla = $(`<table id="tablaModel"></table>`);
    let tbody = $(`<tbody id="bodyTablaModel"></tbody>`);
    let trow = $(`<tr id="headerTablaModel"><th class="celda col-id">${jsID}</th></tr>`);
    if (model.mostrarColumnaCategoria) {
        let colCategory = $(`<th class="celda header"></th>`);
        let containerCategory = $(`<div class="nomCategory"></div>`);

        let textCategory = $(`<div>${jsCategoria}</div>`);
        containerCategory.append(textCategory);

        if (model.mostrarBotonAddFila && Boolean(App.hdPermiteEdicion.value)) {
            let buttonAddTipoFila = $(`<button class="addTipoFila ico-add-tabla" onclick="addTipoFilaToModel()" type="button" ></button>`);
            containerCategory.append(buttonAddTipoFila);
        }

        colCategory.append(containerCategory);
        trow.append(colCategory);
    }

    tbody.append(trow);
    tabla.append(tbody);
    $("#containerConfigTemplate").append(tabla);


    insertheaderTablaModel(model);
    addTipoFila(model);
}

function addTipoFila(model) {
    if (model.tiposFila) {
        for (let idTyp = 0; idTyp < model.tiposFila.length; idTyp++) {
            let filaObj = model.tiposFila[idTyp];
            let row = $("#bodyTablaModel").append("<tr></tr>");
            let valueID = idTyp + 1;


            let tdID = $(`<td class="col-id celda"></td>`);
            let spanID = $(`<span>${valueID}</span>`);
            tdID.append(spanID);
            row.append(tdID);

            // #region button Eliminar Fila
            let btnDeleteFila = $(`<button class="delete-fila ico-eliminar-tabla" type="button" data-filaid="${filaObj.tipoID}" ></button>`)
            btnDeleteFila.click(function (e) {
                let filaID = e.currentTarget.dataset.filaid;
                console.log(filaID);

                removeFila(filaID);
            });

            if (model.tiposFila.length > 1 && Boolean(App.hdPermiteEdicion.value)) {
                tdID.append(btnDeleteFila);
            }
            // #endregion

            // #region Columna de Categoría
            if (model.mostrarColumnaCategoria) {
                let colCatetory = $(`<td class="col-categoria celda"></td>`);
                let valueSelectCategory = $(`<div></div>`);
                let selectCategory = $(`<select class="selectCategory" data-fila-id="${filaObj.tipoID}"/>`);

                let arrayOptionsCategory = App.storeColumnaCategoria.data.items;
                selectCategory.append(`<option disabled selected value style="display:none">${jsSeleccioneOpcion}</option>`);
                arrayOptionsCategory.forEach(option => {
                    selectCategory.append(`<option value="${option.data.Id}">${option.data.Name}</option>`);
                });

                if (filaObj.TipoFiltroDinamicoID) {
                    idsColCategory.push(filaObj.TipoFiltroDinamicoID);
                    selectCategory.val(filaObj.TipoFiltroDinamicoID.toString());
                    valueSelectCategory.text(arrayOptionsCategory.find(x => x.data.Id == filaObj.TipoFiltroDinamicoID).data.Name);
                    
                }
                else if (filaObj.TipoFiltroID) {
                    idsColCategory.push(filaObj.TipoFiltroID);
                    selectCategory.val(filaObj.TipoFiltroID.toString());
                    valueSelectCategory.text(arrayOptionsCategory.find(x => x.data.Id == filaObj.TipoFiltroID).data.Name);
                }

                if (Boolean(App.hdPermiteEdicion.value)) {
                    colCatetory.append(selectCategory);
                }
                else {
                    colCatetory.append(valueSelectCategory);
                }
                
                row.append(colCatetory);

                selectCategory.on('focusin', function () {
                    $(this).data('oldVal', $(this).val());
                });

                selectCategory.change(function (ev) {
                    let value = ev.currentTarget.value;
                    let filaTipoID = $(this).attr("data-fila-id");
                    let oldValue = $(this).data('oldVal');

                    for (let i2 in tempModelo.tiposFila) {
                        if (tempModelo.tiposFila[i2].tipoID.toString() == filaTipoID) {
                            let fila = tempModelo.tiposFila[i2];

                            if (tempModelo.esCategoriaDinamica) {
                                fila.TipoFiltroDinamicoID = value;
                            }
                            else {
                                fila.TipoFiltroID = value;
                            }

                            if (oldValue) {
                                Ext.Msg.alert(
                                    {
                                        title: jsAtencion,
                                        msg: jsCambiarColumnaCategoria,
                                        buttons: Ext.Msg.YESNO,
                                        fn: CambiarCombosCategoriaConfirmacion,
                                        icon: Ext.MessageBox.QUESTION,
                                        fila: fila,
                                        filaTipoID: filaTipoID,
                                        value: value,
                                        oldValue: oldValue
                                    });
                            }
                            else {
                                let data = {
                                    fila: fila,
                                    filaTipoID: filaTipoID,
                                    value: value,
                                    oldValue: oldValue
                                };
                                CambiarCombosCategoriaConfirmacion("yes", null, data);
                            }


                            break;
                        }
                    }
                });
            }
            // #endregion

            // #region Columnas modelo
            for (let iColumn in model.columnas) {
                
                let colID = model.columnas[iColumn].id;
                let tipoID = model.tiposFila[idTyp].tipoID;
                let td = $(`<td class="col-attr celda"></td>`);

                //Ordenación de celdas
                model.tiposFila[idTyp].celdas = model.tiposFila[idTyp].celdas.sort((a, b) => (a.id > b.id) ? 1 : -1);

                model.tiposFila[idTyp].celdas.find(celd => {

                    if (celd.columnID == colID) {
                        let div;

                        if (!celd.mismaCelda) {
                            div = $(`<div class="atributte-area"></div>`);
                        }
                        else {
                            div = $(`<div class="atributte-area background-area"></div>`);
                        }

                        // #region Creacción del combo por columna

                        let categoriaid = (filaObj.TipoFiltroID) ? filaObj.TipoFiltroID : -1;

                        let divSelectHtml = $(`<div></div>`);
                        let selectDiv = $(`<div class="selectHTML">Select a Option<button class="btnSelectHTML"></button></div>`);
                        let btnOptionDiv = $(`<div class="optionsHTML"><button class="btn-addlinkedfield" type="button" value="-1" >${jsAgregarDeCampoVinculado}</button></div>`);
                        let optionsDiv = $(`<ul></ul>`);

                        divSelectHtml.append(selectDiv);
                        btnOptionDiv.append(optionsDiv);
                        divSelectHtml.append(btnOptionDiv);

                        let valueSelect = $(`<div class="valueSelect"></div>`);
                        let select = $(`<select data-col-id="${colID}" data-col-tipo="${tipoID}" data-categoriaid="${categoriaid}" data-celda-id="${celd.id}" />`);
                        let btnAddLinkedField = $(`<option data-celda-id="${celd.id}" data-addlinkedfield="true" data-categoriaid="${categoriaid}"><button class="btn-addlinkedfield" type="button" value="-1" >${jsAgregarDeCampoVinculado}</button></option>`);

                        select.append(btnAddLinkedField);
                        /*btnAddLinkedField.click(function (e) {
                            let celdaID = e.currentTarget.dataset.celdaId;
                            let categoriaID = e.currentTarget.dataset.categoriaid;

                            openWinSelectCampoVinculado(celdaID, "/", categoriaID);
                        });*/

                        // #region Agregar atributo celda
                        let btnAddItemToCel = null;
                        if (!celd.configurarSeparador && (celd.attributoID != null || celd.CampoVinculado || celd.TextoFijo != null)) {
                            btnAddItemToCel = $(`<button class="add-item-cel ico-compartir-tabla" type="button" data-col-id="${colID}" data-col-tipo="${tipoID}" data-celda-id=${celd.id} tooltip="${jsNuevoCampoConcatenado}"> </button>`);
                            btnAddItemToCel.click(function () {
                                let colID = $(this).attr("data-col-id");
                                let tipoID = $(this).attr("data-col-tipo");
                                let celdaID = $(this).attr("data-celda-id");
                                //this.parentElement.className += " background-area";

                                addItemToCel(colID, tipoID, celdaID);
                            });
                        }
                        // #endregion

                        // #region Eliminar Atributo
                        let btnDeleteItemToCel = null;
                        if (celd.mismaCelda && celd.celdaPadreID) {

                            if (celd.configurarSeparador) {
                                btnDeleteItemToCel = $(`<button class="delete-item-cel ico-eliminar-tabla btnWithSeparador" type="button" data-col-id="${colID}" data-col-tipo="${tipoID}" data-celda-id="${celd.id}" ></button>`)
                                btnDeleteItemToCel.click(function () {
                                    let colID = $(this).attr("data-col-id");
                                    let tipoID = $(this).attr("data-col-tipo");
                                    let celdaID = $(this).attr("data-celda-id");

                                    removeItemToCell(celdaID, tipoID, colID);
                                });
                            }
                            else {
                                btnDeleteItemToCel = $(`<button class="delete-item-cel ico-eliminar-tabla" type="button" data-col-id="${colID}" data-col-tipo="${tipoID}" data-celda-id="${celd.id}" ></button>`)
                                btnDeleteItemToCel.click(function () {
                                    let colID = $(this).attr("data-col-id");
                                    let tipoID = $(this).attr("data-col-tipo");
                                    let celdaID = $(this).attr("data-celda-id");

                                    removeItemToCell(celdaID, tipoID, colID);
                                });
                            }
                        } else if (!celd.mismaCelda) {
                            //Resetear celda simple a estado inicial
                            btnDeleteItemToCel = $(`<button class="delete-item-cel ico-eliminar-tabla btnWithSeparador" type="button" data-col-id="${colID}" data-col-tipo="${tipoID}" data-celda-id="${celd.id}" ></button>`)
                            btnDeleteItemToCel.click(function () {
                                let colID = $(this).attr("data-col-id");
                                let tipoID = $(this).attr("data-col-tipo");
                                let celdaID = $(this).attr("data-celda-id");

                                resetCeldToInitState(colID, tipoID, celdaID);
                            });
                        }
                        // #endregion

                        let arrayOptions = App.storeColumnasModeloDatos.data.items;

                        select.append(`<option disabled selected value style="display:none" >${jsSeleccioneOpcion}</option>`);
                        arrayOptions.forEach(option => {

                            let catID;
                            if (filaObj.TipoFiltroDinamicoID) {
                                catID = filaObj.TipoFiltroDinamicoID;
                            }
                            else if (filaObj.TipoFiltroID) {
                                catID = filaObj.TipoFiltroID;
                            }

                            if (option.data.TypeDynamicID == -1 || option.data.TypeDynamicID == catID) {
                                select.append(`<option value="${option.data.ID}" data-dynamic="${option.data.Dynamic}" data-type="${option.data.DataType}" data-addlinkedfield="false" >${option.data.Name}</option>`);
                            }
                            //else {
                            //    console.log(`${option.data.TypeDynamicID} != ${catID}" ${option.data.Name}`);
                            //}
                        });

                        if (celd.attributoID) {
                            select.val(celd.attributoID.toString());
                            let title = arrayOptions.find(op => op.data.ID == celd.attributoID);
                            if (title != undefined) {
                                select.attr("title", title.data.Name);
                                valueSelect.text(title.data.Name);
                            }
                        }
                        else if (celd.CampoVinculado)
                        {
                            select.append(`<option value="-1" disabled style="display:none" >${celd.CampoVinculado.DisplayField}</option>`);
                            select.val(-1);
                            select.attr("title", celd.CampoVinculado.DisplayField);
                            valueSelect.text(celd.CampoVinculado.DisplayField);
                        }
                        else
                        {
                            valueSelect.text("");
                            model.tiposFila[idTyp].celdas.push({
                                id: "temp-" + itCelda,
                                attributoID: 0,
                                columnID: colID,
                                columnaModeloDatoID: -1
                            });
                            itCelda++;
                        }
                        // #endregion

                        // #region boton add transformacion
                        let btnAdd = null;
                        if (/*(celd.reglasTransformacion == null || celd.reglasTransformacion.length == 0) && */(celd.attributoID != null || celd.CampoVinculado)) {

                            if (celd.CampoVinculado) {
                                celd.TipoDato = celd.CampoVinculado.TipoDato;
                            }

                            btnAdd = $(`<button class="ico-expandOption" type="button" tooltip="${jsAgregarReglaTransformacion}"></button>`);
                            btnAdd.click(function () {
                                reglaTransFormacionIDedit = null;
                                addTransformationToCellColumn(model, tipoID, colID, celd.id, celd.TipoDato, celd.FormatoFecha);
                            });
                        }
                        // #endregion

                        // #region Reglas Transformacion
                        let idContenedorReglas = `transformacion-${celd.id}`;
                        let reglasTransformacion = $(`<ul id="${idContenedorReglas}" class="transformationRules" />`);
                        if (celd.reglasTransformacion) {
                            let tipoFila = model.tiposFila[idTyp];

                            celd.reglasTransformacion.forEach(regla => {
                                let btnEditTransformation = $(`<button class="ico-editar-atributo btnEditRules" style="margin-top: 2px;" type="button" data-reglaCeldaID="${regla.reglaCeldaID}" data-idTyp="${tipoFila.tipoID}" data-column="${celd.columnID}" data-celdid="${celd.id}" data-tipoDato="${celd.TipoDato}" ></button>`);
                                btnEditTransformation.click(function (e) {
                                    let reglaCeldaID = e.currentTarget.dataset.reglaceldaid;
                                    let columnID = e.currentTarget.dataset.column;
                                    let typID = e.currentTarget.dataset.idtyp;
                                    let celdID = e.currentTarget.dataset.celdid;
                                    let TipoDato = e.currentTarget.dataset.tipodato;

                                    MostrarEditarTransformacion(reglaCeldaID, celdID, TipoDato, reglaCeldaID);
                                });
                                let btnDeleteTransformation = $(`<button class="ico-eliminar-atributo btnDeleteRules" style="margin-top: 2px;" type="button" data-reglaCeldaID="${regla.reglaCeldaID}" data-idTyp="${tipoFila.tipoID}" data-column="${celd.columnID}" ></button>`);
                                btnDeleteTransformation.click(function (e) {
                                    let reglaCeldaID = e.currentTarget.dataset.reglaceldaid;
                                    let columnID = e.currentTarget.dataset.column;
                                    let typID = e.currentTarget.dataset.idtyp;

                                    deleteTransformation(reglaCeldaID, columnID, typID);
                                });

                                let classDefault = "";
                                let tagElemento = "li";
                                let txtRegla = `${regla.regla} ${regla.valorDisplay}`;
                                if (regla.CheckValorDefecto) {
                                    txtRegla = `${jsValorDefecto}: ${regla.valorDisplay}`;
                                    classDefault = "default not-sortable";
                                    tagElemento = "div";
                                }

                                let icoMenuTransf = "<div/>";
                                if (Boolean(App.hdPermiteEdicion.value)) {
                                    icoMenuTransf = `<div class="ico-menu-atributo" style="display: inline-block; vertical-align: middle; margin-top: 2px;"></div>`;
                                }

                                let itemReg = $(`<${tagElemento} class="divAttrib ${classDefault}" tooltip="${txtRegla}" data-reglaCeldaID="${regla.reglaCeldaID}">
                                                    ${icoMenuTransf}
                                                    <span style="vertical-align: middle; font-size: 13px; overflow-x: hidden; text-overflow: ellipsis;">${txtRegla}</span>
                                                </${tagElemento}>`);

                                if (Boolean(App.hdPermiteEdicion.value)) {
                                    itemReg.append(btnEditTransformation);
                                    itemReg.append(btnDeleteTransformation);
                                }
                                reglasTransformacion.append(itemReg);
                            });

                            // #region Drag and Drop Transformaciones
                            $(function () {
                                
                                $(`#${idContenedorReglas}`).sortable({
                                    handle: ".ico-menu-atributo",
                                    revert: true,
                                    items: "li:not(.not-sortable)",
                                    onDrop: function ($item, container, _super) {
                                        _super($item, container);

                                        let childrens = $(`#${idContenedorReglas}`).children();

                                        for (let indexChil = 0; indexChil < childrens.length; indexChil++) {
                                            let idTransformacion = $(childrens[indexChil]).attr("data-reglaceldaid");
                                            if (idTransformacion != undefined || idTransformacion != null) {
                                                TreeCore.ActualizarOrdenTransformacion(idTransformacion, indexChil, {
                                                    success: function (result) {
                                                        if (!result.Success) {
                                                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                        }
                                                        else {

                                                        }
                                                    }
                                                });
                                            }
                                        }

                                        let fijos = $(".divAttrib.not-sortable");
                                        for (let indexFijo = 0; indexFijo < fijos.length; indexFijo++) {
                                            $(`#${idContenedorReglas}`).append($(fijos[indexFijo]));
                                        }

                                    }
                                    /*pullPlaceholder: false,
                                    // animation on drop
                                    isValidTarget: function ($item, container) {
                                        if ($item.is(".highlight"))
                                            return true;
                                        else
                                            return $item.parent("ul")[0].id == container.el[0].id;
                                    },
                                    onDrop: function ($item, container, _super) {
                                        var $clonedItem = $('<li/>').css({ height: 0 });
                                        $item.before($clonedItem);
                                        $clonedItem.animate({ 'height': $item.height() });

                                        $item.animate($clonedItem.position(), function () {
                                            $clonedItem.detach();
                                            _super($item, container);
                                        });

                                        var order = 0;
                                        var ruta;
                                        for (var item in document.getElementById(idContenedorReglas).children) {
                                            if (document.getElementById(idContenedorReglas).children[item].tagName == "LI" && document.getElementById(idContenedorReglas).children[item].children.length > 0) {
                                                ruta = document.getElementById(idContenedorReglas).children[item].children[0].id.split('_'); ruta.pop();
                                                ruta = ruta.join('_');
                                                //TreeCore[ruta].MoverElementoOrden(order);
                                                order++;
                                            }
                                        }
                                    },
                                    // set $item relative to cursor position
                                    onDragStart: function ($item, container, _super) {
                                        var offset = $item.offset(),
                                            pointer = container.rootGroup.pointer;

                                        adjustment = {
                                            left: pointer.left - offset.left,
                                            top: pointer.top - offset.top
                                        };

                                        _super($item, container);
                                    },
                                    onDrag: function ($item, position) {
                                        $item.css({
                                            left: position.left - adjustment.left,
                                            top: (position.top - adjustment.top) + document.getElementById(idContenedorReglas).scrollTop
                                        });
                                    }*/
                                });
                                $("ul, li").disableSelection();
                            });
                            // #endregion
                        }
                        // #endregion

                        // #region Formato fecha
                        let itemRegFecha;
                        if ((celd.FormatoFecha && celd.TipoDato == "FECHA") || (celd.CampoVinculado && celd.CampoVinculado.TipoDato == "FECHA")) {
                            //let btnDeleteTransformation = $(`<button class="ico-eliminar-atributo btnDeleteRules" style="margin-top: 2px;" type="button" data-celdID="${celd.id}" ></button>`);
                            //btnDeleteTransformation.click(function (e) {
                            //    let celdID = e.currentTarget.dataset.celdid;
                            //    deleteFormatoFecha(celdID);
                            //});

                            let btnEditTransformation = $(`<button class="ico-editar-atributo btnEditRules" style="margin-top: 2px; grid-column-end: -1;" type="button" data-celdID="${celd.id}" data-tipoDato="${celd.TipoDato}" ></button>`);
                            btnEditTransformation.click(function (e) {

                                let celdID = e.currentTarget.dataset.celdid;
                                let TipoDato = e.currentTarget.dataset.tipodato;

                                MostrarEditarTransformacion(null, celdID, TipoDato, null, true);
                            });

                            let icoMenuFecha = "<div/>";
                            if (Boolean(App.hdPermiteEdicion.value)) {
                                icoMenuFecha = `<div class="ico-menu-atributo" style="display: inline-block; vertical-align: middle; margin-top: 2px;"></div>`;
                            }

                            if (celd.FormatoFecha != null || celd.FormatoFecha.length <= 15) {
                                itemRegFecha = $(`<div class="divAttrib not-sortable">
                                                ${icoMenuFecha}
                                                <span class="colSpanHover" style="vertical-align: middle; font-size: 13px; overflow-x: hidden; text-overflow: ellipsis;">${celd.FormatoFecha}</span>
                                            </div>`);

                            } else {
                                itemRegFecha = $(`<div class="divAttrib not-sortable" tooltip="${celd.FormatoFecha}">
                                                ${icoMenuFecha}
                                                <span class="colSpanHover" style="vertical-align: middle; font-size: 13px; overflow-x: hidden; text-overflow: ellipsis;">${celd.FormatoFecha}</span>
                                            </div>`);
                            }

                            if (Boolean(App.hdPermiteEdicion.value)) {
                                itemRegFecha.append(btnEditTransformation);
                            }
                            
                        }
                        // #endregion

                        // #region Separador
                        let btnSeparador = $(`<div></div>`);

                        if (celd.configurarSeparador) {
                            let separadorTemp = (celd.separador) ? celd.separador : "";

                            let labelSeparador = $(`<div class="separador" data-separador="${separadorTemp}" data-celdaID="${celd.id}" data-fila-id="${tipoID}" >
                                                        <span class="textS" tooltip="${jsSeparador}">${separadorTemp}</span>
                                                    </div>`);
                            if (Boolean(App.hdPermiteEdicion.value)) {
                                labelSeparador.dblclick(function (e) {
                                    let celdaID = $(this).attr("data-celdaID");
                                    let filaID = $(this).attr("data-fila-id");
                                    let separador = $(this).attr("data-separador");
                                    $(this).empty();

                                    let inputSeparador = $(`<input type="text" maxlength="10" value="${separador}" />`);
                                    inputSeparador.keypress(function (e2) {
                                        if (e2.which == 13) {
                                            changeSeparator(filaID, celdaID, e2);
                                        }
                                    });
                                    inputSeparador.focusout(function (e2) {
                                        changeSeparator(filaID, celdaID, e2);
                                    });

                                    $(this).append(inputSeparador);
                                });
                            }

                            btnSeparador.append(labelSeparador);
                        }
                        // #endregion

                        // #region Cambiar atributo en columna
                        select.change(function (ev) {
                            let isAddlinkedfield = $(ev.currentTarget.options[ev.currentTarget.selectedIndex]).attr("data-addlinkedfield");
                            let colTipo = $(this).attr("data-col-tipo");
                            let celdaID = $(this).attr("data-celda-id");
                            let categoriaid = $(this).attr("data-categoriaid");

                            if (isAddlinkedfield == "true") {
                                App.hdCampoVinculadoCategoriaOriginal.setValue(categoriaid);
                                openWinSelectCampoVinculado(celdaID, "/", categoriaid);

                            }
                            else {
                                let esDinamico = $(ev.currentTarget.options[ev.currentTarget.selectedIndex]).attr("data-dynamic") === "true";
                                let dataType = $(ev.currentTarget.options[ev.currentTarget.selectedIndex]).attr("data-type");
                                let colID = $(this).attr("data-col-id");
                                let value = ev.currentTarget.value;
                                let celdaAGuardar = null;
                                let filaID = null;

                                for (let i2 in tempModelo.tiposFila) {
                                    if (tempModelo.tiposFila[i2].tipoID == colTipo) {
                                        for (let i3 in tempModelo.tiposFila[i2].celdas) {
                                            if (tempModelo.tiposFila[i2].celdas[i3].columnID == colID && tempModelo.tiposFila[i2].celdas[i3].id == celdaID) {
                                                tempModelo.tiposFila[i2].celdas[i3].attributoID = value;
                                                tempModelo.tiposFila[i2].celdas[i3].esDinamico = esDinamico;

                                                if (dataType != "FECHA") {
                                                    tempModelo.tiposFila[i2].celdas[i3].FormatoFecha = null;
                                                } else if (tempModelo.tiposFila[i2].celdas[i3].FormatoFecha == null || tempModelo.tiposFila[i2].celdas[i3].FormatoFecha == undefined) {
                                                    tempModelo.tiposFila[i2].celdas[i3].FormatoFecha = "dd/MM/yyyy";
                                                }

                                                filaID = tempModelo.tiposFila[i2].tipoID;
                                                celdaAGuardar = tempModelo.tiposFila[i2].celdas[i3];
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                if (celdaAGuardar != null) {
                                    saveCellToModel(celdaAGuardar, filaID);
                                }
                            }
                        });

                        // #endregion

                        // #region textoFijo
                        let textoFijo = $(`<button class="btnTextFixed" type="button" data-celda-id="${celd.id}">+${jsTexto}</button>`);
                        
                        textoFijo.click(function (e) {
                            let celdaID = $(this).attr("data-celda-id");
                            showWinTextoFijo(celdaID);
                        });
                        
                        let printBotonTextoFijo = (celd.CoreAtributosConfiguracionID == null && celd.attributoID == null && celd.CampoVinculado == undefined);
                        // #endregion

                        
                        if (printBotonTextoFijo && Boolean(App.hdPermiteEdicion.value)) {
                            div.append(textoFijo);
                            div.addClass("dvFixedText");
                        }
                        if (btnAdd != null && Boolean(App.hdPermiteEdicion.value)) {
                            div.append(btnAdd);
                        }

                        if (celd.TextoFijo == null && Boolean(App.hdPermiteEdicion.value)) {
                            div.append(select);
                        }
                        else {
                            div.append(valueSelect);
                        }
                        if (celd.TextoFijo != null) {
                            valueSelect.text(celd.TextoFijo);
                            div.append(valueSelect);
                        }
                        if (btnAddItemToCel != null && Boolean(App.hdPermiteEdicion.value)) {
                            div.append(btnAddItemToCel);
                            //div.append(btnAddItemToCelToolTip);
                        }
                        div.append(reglasTransformacion);
                        reglasTransformacion.append(itemRegFecha);
                        if (btnDeleteItemToCel != null && Boolean(App.hdPermiteEdicion.value)) {
                            div.append(btnDeleteItemToCel);
                        }
                        div.append(btnSeparador);
                        td.append(div);

                    }
                });

                row.append(td);
            }
            // #endregion
        }
        CambiarCombosCategoria();
    }
}

function resetCeldToInitState(colID, tipoID, celdaID) {
    Ext.Msg.alert(
        {
            title: parent.jsEliminar,
            msg: jsEliminarCelda,
            buttons: Ext.Msg.YESNO,
            fn: ajaxResetCeldToInitState,
            icon: Ext.MessageBox.QUESTION,
            colID: colID,
            tipoID: tipoID,
            celdaID: celdaID
        });
}

function ajaxResetCeldToInitState(button, a, data) {
    if (button == "yes" || button == "si") {
        let colID = data.colID;
        let tipoID = data.tipoID;
        let celdaID = data.celdaID;


        TreeCore.ResetCeldToInitState(colID, tipoID, celdaID, {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        });
    }
}

function changeSeparator(filaID, celdaID, e2) {
    let celda = tempModelo.tiposFila.find(x => x.tipoID == filaID).celdas.find(x => x.id == celdaID);
    if (celda) {
        celda.separador = $(e2.target).val();

        saveCellToModel(celda, filaID);
    }
}

function CambiarCombosCategoriaConfirmacion(button, a, data) {
    if (button == "yes" || button == "si") {
        CambiarCombosCategoria(data.value, data.oldValue);
        saveFilaModel(data.fila, data.filaTipoID);
    }
    else {
        TreeCore.GetModeloPlantilla({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    tempModelo = result.Result;

                    printTableModel(tempModelo);

                    // #region filtro
                    App.tagsContainerTemplate.removeAll();
                    App.storeFiltros.clearData()
                    App.storeFiltros.reload();
                    App.cmbField.reset();
                    ocultarYResetearCampos();
                    // #endregion
                }
            }
        });
    }
}

function CambiarCombosCategoria(categoryIDSelected, oldValue) {
    let options = $(".selectCategory option");


    if (categoryIDSelected) {
        idsColCategory.push(categoryIDSelected);
        if (oldValue) {
            idsColCategory = idsColCategory.filter(x => { x.toString() != oldValue.toString() });
        }
    }
    if (options) {
        for (let option of options) {
            let exist = idsColCategory.find(function (x) {
                return x.toString() == option.value;
            });
        }
    }
}

function addTransformationToCellColumn(model, tipoID, colID, celdaID, TipoDato, FormatoFecha) {
    celdaIDAdd = celdaID;
    App.hdCeldaAddTransformation.setValue(celdaID);

    CargarStoresSerie([App.storeCoreExportacionDatosPlantillasReglasCeldas, App.storeReglaTransformacion], function () {
        let transformacionesCelda = App.storeCoreExportacionDatosPlantillasReglasCeldas.data.items;
        let hayValorPorDefecto = false;

        for (let indexTrans in transformacionesCelda) {
            if (transformacionesCelda[indexTrans].data.CheckValorDefecto) {
                hayValorPorDefecto = true;
            }
        }
        App.btnValuePorDefecto.setPressed(false);
        if (hayValorPorDefecto) {
            App.labelBtnValuePorDefecto.hide();
            App.btnValuePorDefecto.hide();
        } else {
            App.labelBtnValuePorDefecto.show();
            App.btnValuePorDefecto.show();
            App.btnValuePorDefecto.setPressed(false);
        }


        LimpiarFormularioReglasTransformacion();

        if (TipoDato != "FECHA") {
            App.txtFormatoFecha.disable();
            App.txtFormatoFecha.reset();
            App.txtFormatoFecha.hide();
            App.txtFormatoFecha.allowBlank = true;

            App.txtCellValueIs.allowBlank = false;
            App.txtCellValueIs.reset();
            App.cmbRule.allowBlank = false;
            App.cmbRule.reset();
            App.txtValorRegla.allowBlank = false;
            App.txtValorRegla.reset();

            App.txtFormatoFecha.setValue("");
        }
        else {

            ////
            App.txtCellValueIs.enable();
            App.txtCellValueIs.reset();
            App.txtCellValueIs.show();
            App.txtCellValueIs.allowBlank = true;

            App.cmbRule.enable();
            App.cmbRule.reset();
            App.cmbRule.show();
            App.cmbRule.allowBlank = true;

            App.txtValorRegla.enable();
            App.txtValorRegla.reset();
            App.txtValorRegla.show();
            App.txtValorRegla.allowBlank = true;
            ////

            //App.txtFormatoFecha.enable();
            //App.txtFormatoFecha.reset();
            //App.txtFormatoFecha.show();
            //App.txtFormatoFecha.allowBlank = false;

            App.numberValorRegla.hide();
            App.numberValorRegla.disable();
            App.numberValorRegla.allowBlank = true;
            App.numberValorRegla.reset();

            App.txtValorRegla.hide();
            App.txtValorRegla.disable();
            App.txtValorRegla.allowBlank = true;
            App.txtValorRegla.reset();

            App.checkboxValorRegla.hide();
            App.checkboxValorRegla.disable();
            App.checkboxValorRegla.allowBlank = true;
            App.checkboxValorRegla.reset();

            App.dateValorRegla.hide();
            App.dateValorRegla.disable();
            App.dateValorRegla.allowBlank = true;
            App.dateValorRegla.setValue(new Date());

            App.cmbValorRegla.hide();
            App.cmbValorRegla.disable();
            App.cmbValorRegla.allowBlank = true;
            App.cmbValorRegla.reset();

            if (FormatoFecha) {
                App.txtFormatoFecha.setValue(FormatoFecha);
            }
        }

        if (TipoDato == "LISTAMULTIPLE" || TipoDato == "LISTA") {
            App.hdCeldaID.setValue(celdaID);
            App.storeValorRegla.reload();
        }

        App.winAddTransformationRule.setTitle(`${parent.jsAgregar} ${parent.jsReglaTransformacion}`);
        App.winAddTransformationRule.show();
    });
}

function addItemToCel(colID, tipoID, celdaID) {
    let newCelda = {
        id: "temp",
        attributoID: null,
        columnID: colID,
        columnaModeloDatoID: null,
        celdaPadreID: celdaID
    };
    saveCellToModel(newCelda, tipoID);

}

function removeItemToCell(celdaID, filaID, columnaID) {

    TreeCore.EliminarCelda(celdaID, filaID, columnaID,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        });
}

function removeFila(filaID) {
    Ext.Msg.alert(
        {
            title: parent.jsEliminar,
            msg: jsEliminarFila,
            buttons: Ext.Msg.YESNO,
            fn: ajaxRemoveFila,
            icon: Ext.MessageBox.QUESTION,
            filaID: filaID
        });
}

function ajaxRemoveFila(button, a, data) {
    if (button == "yes" || button == "si") {
        let filaID = data.filaID;

        TreeCore.EliminarFila(filaID, {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        });
    }
}

function changeNameColumn(_this, e2) {
    let colID = $(_this).attr("data-col-id");
    let value = $(e2.target).val();
    let valueEmpty = value.replace(/\s/g, '');

    if (valueEmpty != "") {
        for (let i2 in tempModelo.columnas) {
            if (tempModelo.columnas[i2].id == colID) {
                tempModelo.columnas[i2].nombre = value;

                saveColumnToModel(tempModelo.columnas[i2]);
                break;
            }
        }
    }
    else {
        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsCampoVacio, buttons: Ext.Msg.OK });
    }
}
function insertheaderTablaModel(model) {
    if (model.columnas) {
        let firstcolum = true;

        for (let i = 0; i < model.columnas.length; i++) {
            let col = model.columnas[i];


            let btnAdd = $(`<button class="addColum ico-add-tabla" type="button" onclick="addColumnToModel()" ></button>`);
            let btnRemove = $(`<button class="RemoveColum ico-eliminar-tabla" type="button" onclick="removeColumnToModel(${col.id})" ></button>`);
            let contName = $(`<div class="contenedorNombreCol"></div>`);
            let th = $(`<th class="celda header"></th>`);

            let colName = $(`<div data-modelid="${model.CoreExportacionDatoPlantillaID}" data-col-id="${col.id}" data-name="${col.nombre}">${col.nombre}</div>`);
            if (Boolean(App.hdPermiteEdicion.value)) {
                colName.dblclick(function (e) {
                    let modelID = $(this).attr("data-modelid");
                    let colID = $(this).attr("data-col-id");
                    let name = $(this).attr("data-name");
                    $(this).empty();

                    let input = $(`<input type="text" value="${name}" data-col-id="${colID}" />`);
                    input.keypress(function (e2) {
                        if (e2.which == 13) {
                            changeNameColumn(this, e2);
                        }
                    });
                    input.focusout(function (e2) {
                        changeNameColumn(this, e2);
                    });

                    $(this).append(input);
                });
            }

            contName.append(colName);

            if (i == model.columnas.length - 1 && Boolean(App.hdPermiteEdicion.value)) {
                contName.append(btnAdd);
            }

            if (firstcolum && model.columnas.length <= 1) {
                firstcolum = false;
            }
            else {
                if (Boolean(App.hdPermiteEdicion.value)) {
                    contName.append(btnRemove);
                }
            }

            th.append(contName);

            $("#headerTablaModel").append(th);
        }




        //colName.appendTo(".contenedorNombreCol");

    }
}

function addColumnToModel() {
    let idGenerado = null;
    let cnt = 0;
    while (idGenerado == null) {
        let tmp = tempModelo.columnas.find(item => item.id == "temp-" + cnt);
        if (tmp == null || tmp == undefined) {
            idGenerado = "temp-" + cnt;
        }
        cnt++;
    }

    let column = {
        id: idGenerado,
        nombre: jsNombreColumna
    };
    tempModelo.columnas.push(column);
    let filasIDs = [];
    tempModelo.tiposFila.forEach(tp => {
        filasIDs.push(tp.tipoID);
    });

    column.filasIDs = filasIDs;

    saveColumnToModel(column);
    //printTableModel(tempModelo);
}

function removeColumnToModel(colID) {
    Ext.Msg.alert(
        {
            title: parent.jsEliminar,
            msg: jsEliminarColumna,
            buttons: Ext.Msg.YESNO,
            fn: ajaxRemoveColumnToModel,
            icon: Ext.MessageBox.QUESTION,
            colID: colID
        });
}

function ajaxRemoveColumnToModel(button, a, data) {
    if (button == "yes" || button == "si") {
        let colRemove;
        let colID = data.colID;

        tempModelo.columnas.forEach(col => {
            if (col.id == colID) {
                col.remove = true;
                colRemove = col;
            }
        });

        saveColumnToModel(colRemove);
    }
}

function addTipoFilaToModel() {
    let fila = {
        tipoID: "temp-" + itTipofila,
        columnaID: "0",
        attributoID: "",
        celdas: []
    };
    tempModelo.tiposFila.push(fila);
    saveFilaModel(fila);
    itTipofila++;
}

function saveColumnToModel(column) {

    TreeCore.GuardarColumna(JSON.stringify(column),
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        });
}

function saveCellToModel(celdaAGuardar, filaID) {
    TreeCore.GuardarCelda(JSON.stringify(celdaAGuardar), filaID,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        });
}

function saveFilaModel(fila) {
    TreeCore.GuardarFila(JSON.stringify(fila),
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                TreeCore.GetModeloPlantilla({
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            tempModelo = result.Result;

                            printTableModel(tempModelo);

                            // #region filtro
                            App.tagsContainerTemplate.removeAll();
                            App.storeFiltros.clearData()
                            App.storeFiltros.reload();
                            App.cmbField.reset();
                            ocultarYResetearCampos();
                            // #endregion
                        }
                    }
                });
            }
        });
}

/*
 
function ModeloPLantilla(params) {
    this.Activo = params["Activo"];
    this.ClaveRecurso = params["ClaveRecurso"];
    this.CoreExportacionDatoPlantillaID = params["CoreExportacionDatoPlantillaID"];
    this.Nombre = params["Nombre"];
    this.UnaVez = params["UnaVez"];
    this.campoFila = params["campoFila"];
    this.columnas = [];
    this.tiposFila = [];
}
*/
// #endregion

// #region winAddTransformationRule

function winAddTransformationRuleGuardar() {
    TreeCore.AddTransformationRuleGuardar(celdaIDAdd, reglaTransFormacionIDedit, {
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.winAddTransformationRule.hide();
                tempModelo = result.Result;

                printTableModel(tempModelo);
            }
        }
    });
}

function deleteFormatoFecha
    (celdID) {
    TreeCore.DeleteFormatoFecha(celdID, {
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                tempModelo = result.Result;

                printTableModel(tempModelo);
            }
        }
    });
}

function deleteTransformation(reglaID, columnID, typID) {

    TreeCore.DeleteTransformation(reglaID, columnID, typID, {
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                tempModelo = result.Result;

                printTableModel(tempModelo);
            }
        }
    });
}

function MostrarEditarTransformacion(reglaID, celdID, TipoDato, rtfID, showDateFormat) {

    App.hdCeldaAddTransformation.setValue(celdID);
    celdaIDAdd = celdID;
    reglaTransFormacionIDedit = rtfID;
    CargarStoresSerie([App.storeReglaTransformacion], function () {
        App.winAddTransformationRule.setTitle(`${parent.jsEditar} ${parent.jsReglaTransformacion}`);
        LimpiarFormularioReglasTransformacion();

        if (TipoDato == "LISTAMULTIPLE" || TipoDato == "LISTA") {
            App.hdCeldaID.setValue(celdID);
            App.storeValorRegla.reload();
        }

        TreeCore.MostrarEditarTransformacion(reglaID, celdID, TipoDato, {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (showDateFormat) {
                        App.txtFormatoFecha.enable();
                        App.txtFormatoFecha.show();
                        App.txtFormatoFecha.allowBlank = false;
                    }
                    else {
                        App.txtFormatoFecha.hide();
                    }
                    //tempModelo = result.Result;
                    //printTableModel(tempModelo);
                }
            }
        });
    });
}

function cmbValorRegla() {

}

function SeleccionarReglaTransformacion() {
    App.cmbRule.getTrigger(0).show();
    let data = App.cmbRule.selection.data;

    App.ReglaRequiereValor.setValue(data.RequiereValor);

    App.txtFormatoFecha.disable();
    //App.txtFormatoFecha.reset();
    App.txtFormatoFecha.hide();
    App.txtFormatoFecha.allowBlank = true;

    App.numberValorRegla.hide();
    App.numberValorRegla.disable();
    App.numberValorRegla.reset();
    App.numberValorRegla.allowBlank = true;

    App.txtValorRegla.hide();
    App.txtValorRegla.disable();
    App.txtValorRegla.reset();
    App.txtValorRegla.allowBlank = true;

    App.dateValorRegla.hide();
    App.dateValorRegla.disable();
    App.dateValorRegla.setValue(new Date());
    App.dateValorRegla.allowBlank = true;

    App.checkboxValorRegla.hide();
    App.checkboxValorRegla.disable();
    App.checkboxValorRegla.allowBlank = true;
    App.checkboxValorRegla.reset();

    App.cmbValorRegla.hide();
    App.cmbValorRegla.disable();
    App.cmbValorRegla.allowBlank = true;
    App.cmbValorRegla.reset();

    if (data.RequiereValor) {

        if (data.TipoDato == "FECHA") {
            App.dateValorRegla.enable();
            App.dateValorRegla.setValue(new Date());
            App.dateValorRegla.show();
            App.dateValorRegla.allowBlank = false;
        }
        else if (data.TipoDato == "NUMERICO") {
            App.numberValorRegla.enable();
            App.numberValorRegla.reset();
            App.numberValorRegla.show();
            App.numberValorRegla.allowBlank = false;
        }
        else if (data.TipoDato == "BOOLEAN") {
            App.checkboxValorRegla.enable();
            App.checkboxValorRegla.reset();
            App.checkboxValorRegla.show();
            App.checkboxValorRegla.allowBlank = false;
        }
        else if (data.TipoDato == "LISTAMULTIPLE" || data.TipoDato == "LISTA") {
            App.cmbValorRegla.enable();
            App.cmbValorRegla.reset();
            App.cmbValorRegla.show();
            App.cmbValorRegla.allowBlank = false;
        }
        else
        {
            App.txtValorRegla.enable();
            App.txtValorRegla.reset();
            App.txtValorRegla.show();
            App.txtValorRegla.allowBlank = false;
        }
    }
    if (data.TipoDato == "FECHA") {

        //App.txtFormatoFecha.enable();
        //App.txtFormatoFecha.reset();
        //App.txtFormatoFecha.show();
        //App.txtFormatoFecha.allowBlank = false;
    }
}

function chkValuePorDefecto(sender) {
    if (App.btnValuePorDefecto.pressed) {
        App.txtFormatoFecha.disable();
        App.txtFormatoFecha.reset();
        //App.txtFormatoFecha.hide();
        App.txtFormatoFecha.allowBlank = true;

        //App.numberValorRegla.hide();
        App.numberValorRegla.disable();
        App.numberValorRegla.reset();
        App.numberValorRegla.allowBlank = true;

        //App.txtValorRegla.hide();
        App.txtValorRegla.disable();
        App.txtValorRegla.reset();
        App.txtValorRegla.allowBlank = true;

        //App.dateValorRegla.hide();
        App.dateValorRegla.disable();
        App.dateValorRegla.setValue(new Date());
        App.dateValorRegla.allowBlank = true;

        //App.checkboxValorRegla.hide();
        App.checkboxValorRegla.disable();
        App.checkboxValorRegla.allowBlank = true;
        App.checkboxValorRegla.reset();

        //App.cmbValorRegla.hide();
        App.cmbValorRegla.disable();
        App.cmbValorRegla.allowBlank = true;
        App.cmbValorRegla.reset();

        //App.cmbRule.hide();
        App.cmbRule.disable();
        App.cmbRule.allowBlank = true;

        //App.txtCellValueIs.hide();
        App.txtCellValueIs.disable();
        App.txtCellValueIs.allowBlank = true;
        App.txtCellValueIs.reset();

        //App.txtValorRegla.hide();
        App.txtValorRegla.disable();
        App.txtValorRegla.allowBlank = true;
        App.txtValorRegla.reset();

        App.txtValorRegla.allowBlank = false;
        App.txtElseCellValue.enable();
        App.txtValorRegla.reset();
        App.btnValuePorDefecto.allowBlank = false;
    }
    else {
        LimpiarFormularioReglasTransformacion();
    }
}

function RecargarReglaTransformacion() {
    recargarCombos([App.cmbRule]);
}

function ValidarFormularioReglasTransformacion(sender, valido) {
    if (valido && (!sender.allowBlank || !sender.hidden)) {
        App.btnAceptarRegla.enable();
        Ext.each(App.winAddTransformationRule.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid() && !c.hidden && !c.disabled) {
                App.btnAceptarRegla.disable();
            }
        });
    } else {
        App.btnAceptarRegla.disable();
    }
}

function LimpiarFormularioReglasTransformacion() {

    App.txtValorRegla.enable();
    App.txtValorRegla.reset();
    //App.txtValorRegla.show();
    //App.txtValorRegla.allowBlank = false;


    App.cmbRule.show();
    App.cmbRule.enable();
    App.cmbRule.allowBlank = false;

    App.txtCellValueIs.show();
    App.txtCellValueIs.enable();
    App.txtCellValueIs.allowBlank = false;

    App.txtValorRegla.show();
    App.txtValorRegla.enable();
    App.txtValorRegla.allowBlank = false;

    App.txtElseCellValue.show();
    App.txtElseCellValue.disable();

    App.dateValorRegla.hide();
    App.dateValorRegla.disable();
    App.dateValorRegla.setValue(new Date());
    App.dateValorRegla.allowBlank = true;

    App.numberValorRegla.hide();
    App.numberValorRegla.disable();
    App.numberValorRegla.reset();
    App.numberValorRegla.allowBlank = true;

    App.checkboxValorRegla.hide();
    App.checkboxValorRegla.disable();
    App.checkboxValorRegla.reset();
    App.checkboxValorRegla.allowBlank = true;

    App.cmbValorRegla.hide();
    App.cmbValorRegla.disable();
    App.cmbValorRegla.reset();
    App.cmbValorRegla.allowBlank = true;

    Ext.each(App.winAddTransformationRule.body.query('*'), function (value) {
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
}

// #endregion

// #region Filters
var FiltrosAplicados = [];
var filtros = [];

var stPn = 1;
var btnCollapsedHide = true;

function hidePnFilters() {

    if (!App.pnAsideR2.collapsed) {
        App.pnAsideR2.setCollapsed(true);
    }

    let pn = App.pnAsideR;
    if (stPn == 0) {
        pn.expand();
        App.tbCollapseAsR.show();
        App.btnCollapseAsR.show();
        btnCollapsedHide = false;
        stPn = 1;
    }
    else {
        pn.collapse();
        App.tbCollapseAsR.hide();
        App.btnCollapseAsR.hide();
        btnCollapsedHide = true;
        stPn = 0;
    }

}

function btnShowFilter() {
    App.cmbField.reset();
    ocultarYResetearCampos();

    if (!App.pnAsideR2.collapsed) {
        App.pnAsideR2.setCollapsed(true);
    }

    //App.pnAsideR.setCollapsed(!App.pnAsideR.collapsed);
    if (!App.pnAsideR.collapsed) {
        App.pnAsideR.setCollapsed(true);
        App.btnCollapseAsR.hide();
    } else {
        App.pnAsideR.setCollapsed(false);
        App.btnCollapseAsR.show();
    }

    App.pnCFilters.show();
    if (btnCollapsedHide) {
        App.storeCoreExportacionDatosPlantillasHistoricos.reload();
        App.tbCollapseAsR.show();
        //App.btnCollapseAsR.show();
        //App.pnCFilters.show();
        App.pnHistorico.hide();
        btnCollapsedHide = false;
        stPn = 1;
    }
    else {
        App.tbCollapseAsR.hide();
        //App.btnCollapseAsR.hide();
        //App.pnCFilters.hide();
        App.pnHistorico.hide();
        btnCollapsedHide = true;
        stPn = 0;
    }
}

//function NombreFiltroValido() {
//    if (App.pnNewFilterNombre.isValid() && App.storeFiltros.data.items.length > 0) {
//        App.btnSaveFilter.enable();
//    } else {
//        App.btnSaveFilter.disable();
//    }
//}

function newFilter(clear = true) {
    App.cmbField.reset();
    ocultarYResetearCampos();

    App.storeFiltros.clearData()
    App.storeFiltros.reload();
    FiltrosAplicados = [];
    filtros = [];
}

function reloadFields(field) {
    App.cmbField.store.reload();

    camposListados = camposListados.filter(function (obj) {
        return obj.Id !== field;
    });
}

function beforeLoadCmbField() {
    ocultarYResetearCampos();
    camposListados.forEach(camp => App.cmbField.removeByValue(camp.Id));
}

function ocultarYResetearCampos() {
    //Ocultar campos
    App.dateInputSearch.hide();
    App.textInputSearch.hide();
    App.numberInputSearch.hide();
    App.cmbOperatorField.hide();
    App.cmbTiposDinamicos.hide();
    App.chkTiposDinamicos.hide();

    //Resetear campos
    App.cmbOperatorField.reset();
    App.cmbTiposDinamicos.reset();
    App.dateInputSearch.reset();
    App.textInputSearch.reset();
    App.chkTiposDinamicos.reset();
    App.numberInputSearch.reset();
}

function selectField(sender) {
    ocultarYResetearCampos();
    let typeData = sender.selection.data.TypeData;

    if (typeData == "NUMERICO") {
        App.cmbOperatorField.show();
        App.numberInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '30px';
    }
    else if (typeData == "FECHA") {
        App.cmbOperatorField.show();
        App.dateInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '30px';
    }
    else if (typeData == "LISTA" || typeData == "LISTAMULTIPLE") {
        App.hdQuery.setValue(sender.selection.data.QueryValores);
        App.cmbTiposDinamicos.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
        App.storeTiposDinamicos.reload();
    }
    else if (typeData == "BOOLEAN") {
        App.chkTiposDinamicos.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
    }
    else {
        App.textInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
    }
}

function addElementFilter() {
    if (App.cmbField.selection) {
        var newFilter;
        let campo = App.cmbField.selection.data;
        let typeData = App.cmbField.selection.data.TypeData;
        if (typeData == "NUMERICO") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.numberInputSearch.value,
                "DisplayValue": App.cmbOperatorField.value + ' ' + App.numberInputSearch.value,
                "TypeData": campo.TypeData,
                "Operador": App.cmbOperatorField.rawValue,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "FECHA") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": getFormattedDate(App.dateInputSearch.value),
                "DisplayValue": App.cmbOperatorField.selection.data.field2 + ' ' + getFormattedDate(App.dateInputSearch.value),
                "TypeData": campo.TypeData,
                "Operador": App.cmbOperatorField.rawValue,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "LISTA" || typeData == "LISTAMULTIPLE") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.cmbTiposDinamicos.getSelectedValues().join(),
                "DisplayValue": App.cmbTiposDinamicos.getSelectedText().join(),
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "BOOLEAN") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.chkTiposDinamicos.value,
                "DisplayValue": App.chkTiposDinamicos.value,
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        else {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.textInputSearch.value,
                "DisplayValue": App.textInputSearch.value,
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        filtros.push(newFilter);
        App.storeFiltros.add(newFilter);
    }
    App.cmbField.reset();
    ocultarYResetearCampos();
}

function saveFilter() {
    var Filtro = {
        "CoreExportacionDatosPlantillaisID": App.GridRowSelectTemplate.selected.items[0].data.CoreExportacionDatoPlantillaID,
        "listaFiltros": filtros,
        "Saved": true,
    };

    if (filtros.length > 0) {
        TreeCore.GuardarFiltro(JSON.stringify(Filtro),
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        AñadirFiltro(Filtro);
                    }
                    loadPreview();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
    else {
        desaplicarFiltro();
        loadPreview();
    }
}

function AplicarFiltros(AppAux) {
    var tree = AppAux.grid,
        store = tree.store,
        logic = tree;

    tree.clearFilter();
    logic.clearFilter();

    try {
        var reOperador = new RegExp(AppAux.hdOperador.value.join('|'), "i");
    } catch (e) {

    }
    try {
        var reEstado = new RegExp(AppAux.hdEstado.value.join('|'), "i");
    } catch (e) {

    }
    try {
        var reUsuario = new RegExp(AppAux.hdUsuario.value.join('|'), "i");
    } catch (e) {

    }
    var valido;
    logic.filterBy(function (node) {
        valido = true;
        if (!reOperador.test(node.data.OperadorID)) {
            valido = false;
        }
        if (!reEstado.test(node.data.EstadoID)) {
            valido = false;
        }
        if (!reUsuario.test(node.data.CreadorID)) {
            valido = false;
        }
        if (!(((AppAux.hdFechaMinCrea.value != undefined) ? AppAux.hdFechaMinCrea.value <= new Date(node.data.FechaAlta) : true) && ((AppAux.hdFechaMaxCrea.value != undefined) ? AppAux.hdFechaMaxCrea.value >= new Date(node.data.FechaAlta) : true))) {
            valido = false;
        }
        if (!(((AppAux.hdFechaMinMod.value != undefined) ? AppAux.hdFechaMinMod.value <= new Date(node.data.FechaMod) : true) && ((AppAux.hdFechaMaxMod.value != undefined) ? AppAux.hdFechaMaxMod.value >= new Date(node.data.FechaMod) : true))) {
            valido = false;
        }
        return valido;
    });
}

function ResizerAside(pn) {
    var elmnt = document.getElementById("vwContenedor-innerCt");

    if (elmnt != null) {
        var HeightVisorPadre = elmnt.offsetHeight;
        if (App != null) {
            App.pnAsideR.setHeight(HeightVisorPadre + 60);
        }
    }
}

function mostrarFiltro(datos) {
    if (datos.Filtro) {
        filtrosAplicados = datos.Filtro;
    }

}

function AñadirFiltro(filtro) {
    FiltrosAplicados = [];
    FiltrosAplicados.push(filtro);
    mostrarFiltrosCabecera();
}

function mostrarFiltrosCabecera() {
    App.tagsContainerTemplate.removeAll();
    FiltrosAplicados.forEach(fl => {
        let panelFiltroCabecera;
        let showBtn = true;
        let clsRead = " TagBorRad";
        if (Boolean(App.hdPermiteEdicion.value)) {
            showBtn = false;
            clsRead = "";
        }

        panelFiltroCabecera = new Ext.Panel({
            Hidden: "false",
            idTree: fl.CoreExportacionDatosPlantillaisID,
            items: [
                {
                    xtype: "netlabel",
                    text: jsFiltroAplicado,
                    cls: "TagSaved"+clsRead,
                    iconCls: "ico-filters-16px",
                },
                {
                    xtype: "button",
                    cls: "CloseSaved",
                    focusable: false,
                    pressedCls: "none",
                    focusCls: "none",
                    hidden: showBtn,
                    handler: desaplicarFiltro,
                },
            ]
        });


        App.tagsContainerTemplate.add(panelFiltroCabecera);
    });
}

function EliminarFiltro(sender, registro, index) {
    App.storeFiltros.data.removeAtKey(index.id);
    filtros = filtros.filter(function (obj) {
        return obj.Campo !== index.id;
    });
}

function desaplicarFiltro(sender, registro, index) {
    TreeCore.EliminarFiltro(
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.tagsContainerTemplate.removeAll();
                    App.storeFiltros.clearData()
                    App.storeFiltros.reload();
                    App.cmbField.reset();
                    ocultarYResetearCampos();
                    FiltrosAplicados = [];
                    filtros = [];
                    App.tagsContainerTemplate.removeAll();
                    mostrarFiltrosCabecera();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}


function MostrarEditarFiltroGuardado(filtro) {
    //newFilter();

    filtro.listaFiltros.forEach(f => {
        filtros.push(f);
        App.storeFiltros.add(f);
    });

}
// #endregion

// #region Frecuencias
function showFormsFormExportar(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    cambiarATapExportar(sender, index);
}

function cambiarATapExportar(sender, index) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";

    var arrayBotones = Ext.getCmp("cntNavVistasFormExportar").ariaEl.getFirstChild().getFirstChild().dom.children;


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

    //if (index == arrayBotones.length - 1 && App.hdAdicionalCargado.value == 'false') {
    //    document.getElementById('formAdditional').style.opacity = '0';
    //    showLoadMask(App.winGestion, function (load) {
    //        TreeCore.PintarCategorias(true,
    //            {
    //                success: function (result) {
    //                    if (!result.Success) {
    //                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //                    } else {
    //                        document.getElementById('formAdditional').style.opacity = '1'
    //                        App.hdAdicionalCargado.setValue('true');
    //                        load.hide();
    //                    }
    //                }
    //            });
    //    });
    //}
}

function ValidarFormulario(sender, valido, aux) {

    try {

        var formPanel = App.formTemplate;
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
        let i = 0;

        // #region Validación Tabs
        Ext.each(App.formExport.query('*'), function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App.lnkFormExport.removeCls(classMandatory);
        }
        else {
            App.lnkFormExport.addCls(classMandatory);
        }

        let o = 0;
        let Programador = [
            App.ProgramadorExportar_cmbFrecuencia,
            App.ProgramadorExportar_txtFechaInicio,
            App.ProgramadorExportar_txtPrevisualizar,
            App.ProgramadorExportar_txtFechaFin,
            App.ProgramadorExportar_txtCronFormat,
            App.ProgramadorExportar_cmbDias,
            App.ProgramadorExportar_txtDiaCadaMes,
            App.ProgramadorExportar_cmbTipoFrecuencia,
            //App.ProgramadorExportar_cmbMesInicio,
            App.ProgramadorExportar_cmbMeses
        ];
        Ext.each(Programador, function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    o++;
                }
            }
        });

        if (o == 0) {
            App.lnkFormFrecuencia.removeCls(classMandatory);
        }
        else {
            App.lnkFormFrecuencia.addCls(classMandatory);
        }

        // #endregion

    } catch (e) {

    }
}
// #endregion

// #region WinSelectCampoVinculado

function RendererIconLinkedField(value) {
    if (value == "TablaModeloDato") {
        return '<span class="ico-folder-grid-gr">&nbsp;</span>';
    }
    else if (value == "InventarioCategoria") {
        return '<span class="ico-folder-grid">&nbsp;</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

function LinkedGridDoubleClick(sender, registro, index) {
    let data = registro.data;


    if (data.DataType == "TablaModeloDato" || data.DataType == "InventarioCategoria") {

        if (data.DataType == "InventarioCategoria") {
            App.hdCampoVinculadoCategoria.setValue(data.TypeDynamicID);
            App.hdCampoVinculadoTipo.setValue("DINAMICO");
        }
        else {
            App.hdCampoVinculadoTipo.setValue("ESTATICO");
        }

        let ruta = App.hdCampoVinculadoRuta.getValue();
        if (ruta != "") {
            ruta = JSON.parse(ruta);
        }
        else {
            ruta = {
                path: []
            };
        }
        let idTb = data.TypeDynamicID;
        //ruta += `${idTb}/`;
        let uID = GenerarID();
        ruta.path.push({
            id: idTb,
            tipo: App.hdCampoVinculadoTipo.value,
            uID: uID
        });


        console.log(ruta);
        App.hdCampoVinculadoRuta.setValue(JSON.stringify(ruta));

        
        App.lbRutaCategoria.show();
        App.btnCarpetaActual.show();
        App.lbRutaCategoria.setText(data.Name);
        if (!listaRuta.some(r => r.TypeDynamicID == data.TypeDynamicID)) {
            listaRuta.push({ Name: data.Name, TypeDynamicID: data.TypeDynamicID, idUnico: uID });
            App.storeSelectCamposVinculados.reload();
        }
        App.menuRuta.items.clear();
        GenerarRuta();
        App.storeSelectCamposVinculados.reload();
        
    }
    else {
        console.log("Guardar la columnaseleccionada", data.TypeDynamicID)

        TreeCore.GuardarCampoVinculado(data.TypeDynamicID, data.Dynamic, data.DataType,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    App.winSelectCampoVinculado.hide();
                    tempModelo = result.Result;
                    printTableModel(tempModelo);
                }
            }
        });
    }
}

function limpiarCampoVinculadoBuscador() {

}

function FiltrarColumnasCampoVinculadoBuscador() {

}

function gridCamposVinculados_RowSelect() {

}

function openWinSelectCampoVinculado(celdaID, ruta, categoria) {
    App.hdCampoVinculadoCeldaID.setValue(celdaID);
    App.hdCampoVinculadoRuta.setValue(ruta);
    App.hdCampoVinculadoCategoria.setValue(categoria);
    App.hdCampoVinculadoTipo.setValue(null);

    App.winSelectCampoVinculado.show();
    IrRutaRaiz();
}

function closeWinSelectCampoVinculado() {
    TreeCore.GetModeloPlantilla({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                limpiarConfiguracionPlantilla(false);
                tempModelo = result.Result;

                printTableModel(tempModelo);
            }
        }
    });
}


var listaRuta = [];

function VolverAtras() {
    if (listaRuta.length >= 2) {
        var ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        //App.hdCarpetaPadreID.setValue(ElePadre.TypeDynamicID);
        App.storeSelectCamposVinculados.reload();
        App.lbRutaCategoria.show();
        App.lbRutaCategoria.setText(ElePadre.Name);
        App.menuRuta.items.clear();
        GenerarRuta();
    } else {
        App.btnBack.hide();
        IrRutaRaiz();
    }
}

function GenerarID() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function IrRutaRaiz() {
    LimpiarRuta();
    App.hdCampoVinculadoTipo.setValue(null);
    App.hdCampoVinculadoCategoria.setValue(App.hdCampoVinculadoCategoriaOriginal.value);
    App.hdCampoVinculadoRuta.setValue([]);
    App.btnPadreCarpetaActucal.hide();
    App.btnBack.hide();
    App.storeSelectCamposVinculados.reload();
}

function SeleccionarRuta(sender, select) {
    //vaciarIdsFiltrados();
    forzarCargaBuscadorPredictivo = true;
    //App.hdCarpetaPadreID.setValue(select.TypeDynamicID);
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.btnBack.show();
    App.lbRutaCategoria.setText(select.text);
    
    for (var i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].idUnico == select.IDUnico) {

            let ruta = JSON.parse(App.hdCampoVinculadoRuta.value);
            for (let z = ruta.path.length-1; z >= 0; z--) {
                console.log(z);
                
                if (ruta.path[z].uID == listaRuta[i].idUnico) {
                    App.hdCampoVinculadoCategoria.setValue(ruta.path[z].id);
                    break;
                }
                else {
                    ruta.path.pop();
                }
            }
            listaRuta = listaRuta.splice(0, ++i);

            App.hdCampoVinculadoRuta.setValue(JSON.stringify(ruta));
        }
    }
    App.storeSelectCamposVinculados.reload();
    App.menuRuta.items.clear();
    GenerarRuta();
}

function SeleccionarPadre(sender, select) {
    LimpiarRuta();
    //App.hdCarpetaPadreID.setValue(select.TypeDynamicID);
    App.storeSelectCamposVinculados.reload();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.lbRutaCategoria.setText(select.text);
}

//function EntrarEnCarpeta() {
//    if (App.GridRowSelect.selected.items[0] != undefined && App.GridRowSelect.selected.items[0].data.EsCarpeta) {
//        vaciarIdsFiltrados();
//        forzarCargaBuscadorPredictivo = true;
//        App.hdCarpetaPadreID.setValue(App.GridRowSelect.selected.items[0].data.TypeDynamicID);
//        App.storeSelectCamposVinculados.reload();
//        App.btnAgregarDocumento.enable();
//        App.lbRutaCategoria.show();
//        App.btnCarpetaActual.show();
//        App.lbRutaCategoria.setText(App.GridRowSelect.selected.items[0].data.Nombre);
//        if (!listaRuta.some(ruta => ruta.TypeDynamicID == App.GridRowSelect.selected.items[0].data.TypeDynamicID)) {
//            listaRuta.push({ Nombre: App.GridRowSelect.selected.items[0].data.Nombre, TypeDynamicID: App.GridRowSelect.selected.items[0].data.TypeDynamicID, idUnico: GenerarID() });
//            App.storeSelectCamposVinculados.reload();
//        }
//        App.menuRuta.items.clear();
//        GenerarRuta();
//        App.storeSelectCamposVinculados.reload();
//    }
//}

function LimpiarRuta() {
    forzarCargaBuscadorPredictivo = true;
    //vaciarIdsFiltrados();
    App.btnMenuRuta.hide();
    App.btnRaizCarpeta.hide();
    App.lbRutaCategoria.hide();
    App.btnCarpetaActual.hide();
    App.menuRuta.items.clear();
    listaRuta = [];
    //App.hdCarpetaPadreID.setValue(0);
}

function GenerarRuta() {
    App.btnMenuRuta.show();
    App.btnRaizCarpeta.show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    for (var item in listaRuta) {
        App.menuRuta.add(new Ext.menu.TextItem({ text: listaRuta[item].Name, TypeDynamicID: listaRuta[item].TypeDynamicID, IDUnico: listaRuta[item].idUnico }))
    }
    if (App.menuRuta.items.items.length > 1) {
        App.menuRuta.items.last().hide();
    } else {
        App.btnMenuRuta.hide();
        App.btnRaizCarpeta.hide();
    }
}

//function recargarRutaActual() {
//    forzarCargaBuscadorPredictivo = true;
//    App.hdCarpetaPadreID.setValue(hdCarpetaActual.value);
//    App.storeSelectCamposVinculados.reload();
//    App.btnAgregarDocumento.enable();
//    App.lbRutaCategoria.show();
//    App.btnCarpetaActual.show();

//    if (App.GridRowSelect.selected.length > 0) {
//        if (App.GridRowSelect.selected.items[0].data.EsCarpeta) {
//            App.lbRutaCategoria.setText(App.GridRowSelect.selected.items[0].data.Nombre);
//            if (!listaRuta.some(ruta => ruta.TypeDynamicID == App.GridRowSelect.selected.items[0].data.TypeDynamicID)) {
//                listaRuta.push({ Nombre: App.GridRowSelect.selected.items[0].data.Nombre, TypeDynamicID: App.GridRowSelect.selected.items[0].data.TypeDynamicID, idUnico: GenerarID() });
//            }
//            App.menuRuta.items.clear();
//            GenerarRuta();
//        }
//    }
//}

// #endregion

// #region TextoFijo

function showWinTextoFijo(celdaID) {

    TreeCore.MostrarEditarTextoFijo(celdaID, {
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }

            App.WinTextoFijo.show();
        }
    });
}

function WinTextoFijoGuardar() {

    TreeCore.GuardarTextoFijo({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.WinTextoFijo.hide();
                tempModelo = result.Result;
                printTableModel(tempModelo);
            }
        }
    });
}

function ValidarFormularioTextoFijo(sender, valido) {
    if (valido && (!sender.allowBlank || !sender.hidden)) {
        App.btnGuardarTextoFijo.enable();
        Ext.each(App.WinTextoFijo.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid() && !c.hidden && !c.disabled) {
                App.btnGuardarTextoFijo.disable();
            }
        });
    } else {
        App.btnGuardarTextoFijo.disable();
    }
}
// #endregion

// #region Historico
function VerHistorico() {

    if (!App.pnAsideR.collapsed) {
        App.pnAsideR.setCollapsed(true);
    }

    if (App.pnAsideR2.collapsed) {
        App.pnAsideR2.setCollapsed(false);
    }
    
    
    App.tbCollapseAsR.show();
    App.btnCollapseAsR.show();
    App.pnHistorico.show();
    btnCollapsedHide = false;
    stPn = 1;
    
}

function HistoricoTemplate(valor, b) {
    let record = b.record.data;
    let strFecha = "";
    let strHora = "";
    let name = "";
    let version = "";

    try {

        name = record.Archivo;
        version = record.Version;

        let date = new Date(record.FechaEjecucion);
        strFecha = getFormattedDate(date);
        strHora = getFormattedTime(date);
    }
    catch (ex) {

    }

    return `<div data-version="v${version}">
                <div class="dateIE">
                    <span>${strFecha}</span> | <span>${strHora}</span>
                </div>
                <div>${name}</div>
            </div>`;
}

function renderHistorico() {
    var panel = document.getElementById('pnHistorico-innerCt');

    panel.style.height = (panel.clientHeight + 10) + "px";
}

function DownloadHistory(sender, registro, index) {
    let historicoID = sender.record.data.CoreExportacionDatosPlantillaHistoricoID;

    TreeCore.DescargarHistorico(historicoID, {
        isUpload: true,
        error: function (a) {
            console.log(a)
            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
        },
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {

            }
        }
    });
}

function DeactivateHistory(sender, registro, index) {
    let historicoID = sender.record.data.CoreExportacionDatosPlantillaHistoricoID;

    Ext.Msg.alert(
        {
            title: parent.jsEliminar,
            msg: parent.jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeactivateHistory,
            icon: Ext.MessageBox.QUESTION,
            historicoID: historicoID
        });
}

function ajaxDeactivateHistory(button, a, data) {
    if (button == 'yes' || button == 'si') {
        
        TreeCore.DesactivarHistorico(data.historicoID, {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeCoreExportacionDatosPlantillasHistoricos.reload();
                }
            }
        });
    }
}





// #endregion
