setTimeout(function () {
    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    //App.pnWrapMainLayout.update();
    //App.CenterPanelMain.update();
    ActiveResizer();
}, 400);

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
}

window.addEventListener('resize', function () {
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

});

// #region newResponsiveControl

var colOverride = "1cols";  // LOS MODOS SON 1cols 2cols 3cols
var colMode = "3colmode";
var isOnColmbl = 1;
var isOnColNormal = 1;

function ColOverrideControl() {
    if (document.getElementById('CenterPanelMain') != null) {

        var res = document.getElementById('CenterPanelMain').offsetWidth;

        if (colOverride == "3cols") {

        }
        else if (colOverride == "2cols") {
            if (App.ctMain3 != null) {
                App.ctMain3.hide();
            }

            if (res < 576) {
                App.btnNextSldr.show();
                App.btnPrevSldr.show();
            }
            else {
                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();
            }
        }

        else if (colOverride == "1cols") {
            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

            App.ctMain2.setHidden(true);
            App.ctMain3.setHidden(true);
        }
    }
}

function moveCtSldr(btn) {

    var res = document.getElementById('CenterPanelMain').offsetWidth;
    var btnPrssd = btn.id;

    if (res < 576 && colMode == "1colmode") { // MODO 1 COLUMNA

        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && isOnColmbl == 1 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();
            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();
            isOnColmbl = 2;

            if (colOverride == "2cols") {
                App.btnNextSldr.disable();
                App.btnPrevSldr.enable();
            }
        }

        else if (btnPrssd == 'btnNextSldr' && isOnColmbl == 2 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();
            App.ctMain3.show();
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();
            isOnColmbl = 3;
        }
        if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 3 && colMode == "1colmode") {
            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();
            isOnColmbl = 2;
        }

        else if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 2 && colMode == "1colmode") {
            App.ctMain1.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
            isOnColmbl = 1;
        }
    }

    else if (res <= 991 && res > 576 && colMode == "2colmode") { // MODO 2 COLS

        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && colMode == "2colmode") {
            App.ctMain3.show();
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();
        }
        if (btnPrssd == 'btnPrevSldr' && colMode == "2colmode") {
            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
        }
    }
}

window.addEventListener('resize',

    function PassiveResizer() {

        var el = document.getElementById('CenterPanelMain');

        if (el != null) {
            var res = document.getElementById('CenterPanelMain').offsetWidth;
            if (res <= 576 && colMode != "1colmode") {
                App.btnNextSldr.show();
                App.btnPrevSldr.show();
                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(true);
                App.ctMain3.setHidden(true);
                App.btnPrevSldr.disable();
                App.btnNextSldr.enable();
                colMode = "1colmode";
            }

            else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {
                App.btnNextSldr.show();
                App.btnPrevSldr.show();
                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(true);
                colMode = "2colmode";
                isOnColmbl = 1;
                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();
            }

            else if (res > 991 && colOverride == "3cols") {
                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();
                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(false);
                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();
                colMode = "3colmode";
                isOnColmbl = 1;
                isOnColNormal = 1;
            }
        }
        ColOverrideControl();
    }
);

function ActiveResizer() {

    var el = document.getElementById('CenterPanelMain');
    if (el != null) {
        var res = document.getElementById('CenterPanelMain').offsetWidth;
        if (res <= 576 && colMode != "1colmode") {
            App.btnNextSldr.show();
            App.btnPrevSldr.show();
            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(true);
            App.ctMain3.setHidden(true);
            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();
            colMode = "1colmode";
        }

        else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {
            App.btnNextSldr.show();
            App.btnPrevSldr.show();
            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(true);
            colMode = "2colmode";
            isOnColmbl = 1;
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
        }
        else if (res > 991 && colOverride == "3cols") {
            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();
            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(false);
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
            colMode = "3colmode";
            isOnColmbl = 1;
            isOnColNormal = 1;
        }
        ColOverrideControl();
    }
}

var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLite == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 0;
    }
}

var spPnLiteD = 0;
function hidePnLiteDirect() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLiteD == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLiteD = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLiteD = 0;
    }
}

function RowSelectAsideShow() {
    TreeCore.DirectShowHidePnAsideR();
}

function displayMenu(btn) {
    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridInfo.hide();
    App.pnInfoVersiones.hide();

    //GET componente a mostrar desde el boton por ID
    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
}

// #endregion

// #region RENDERS

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);
    if (valor == false) {
        imag.src = '';
    }
    else {
        imag.src = '../../ima/ico-accept.svg';
    }
}

function renderMultiflow(valor, id) {
    let imag = document.getElementById('imMultiflow' + id);
    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-subprocess.svg';
    }
}

function renderCommercial(valor, id) {
    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-vendor.svg';

    }
}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }
}

function renderProgBar(valor, id) {
    let bar = document.getElementById('progBar' + id);
    let ancho = valor;

    bar.style.width = ancho * 100 + "%";
}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }
}

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }
}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }
}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }
}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }
}

// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {
    obj.center();
    obj.update();
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
    obj.update();
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
    obj.update();
    //AQUI SE SETEA EL CENTER ABAJO
    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);
    obj.update();
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
    obj.update();
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
});

// #endregion

// #region DIRECT METHOD TEMPLATES
var seleccionado;

var handlePageSizeSelectTemplate = function (item, records) {
    var curPageSize = App.storeDocumentosCargaPlantillas.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDocumentosCargaPlantillas.pageSize = wantedPageSize;
        App.storeDocumentosCargaPlantillas.load();
    }
}

function Grid_RowSelectTemplate(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnDescargarTemplate.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDescargarTemplate.setTooltip(jsDescargarPlantilla);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.hdPlantillaSeleccionada.setValue(seleccionado.DocumentoCargaPlantillaID);
        App.hdLabelTab.setValue(seleccionado.DocumentoCargaPlantilla);
        App.CenterPanelMain.show();
        App.storeDocumentosCargaPlantillasTabs.reload();
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelectTemplate.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnDescargarTemplate.disable();
    App.hdPlantillaSeleccionada.setValue("");
    App.hdLabelTab.setValue("");
}

function VaciarFormulario() {
    App.formTemplate.getForm().reset();
}

function AgregarEditar() {
    VaciarFormulario();
    App.winSaveNewTabCols.setTitle(parent.jsAgregar);
    Agregar = true;
    App.winSaveNewTabCols.show();
    App.txtRuta.hide();
    App.txtRuta.setValue("");
    App.btnEditarRuta.hide();
    App.UploadF.show();
}

function FormularioValidoTemplate(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function winGestionBotonGuardarTemplate() {
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
                    App.winSaveNewTabCols.hide();
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

function MostrarEditar() {
    if (seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winSaveNewTabCols.setTitle(parent.jsEditar);

    if (seleccionado.RutaDocumento != "") {
        App.UploadF.hide();
        App.txtRuta.setValue(seleccionado.RutaDocumento);
        App.txtRuta.show();
        App.btnEditarRuta.show();
    }

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            }
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

function Refrescar() {
    DeseleccionarGrilla();
    App.storeDocumentosCargaPlantillas.reload();
    App.CenterPanelMain.hide();
}

function RecargarProyectoTipo() {
    recargarCombos([App.cmbProyectosTipos]);
}

function SeleccionarProyectoTipo() {
    App.cmbProyectosTipos.getTrigger(0).show();
}

function DescargarPlantilla() {
    window.open("adminTemplates.aspx?DescargarPlantilla=" + seleccionado.RutaDocumento);
}

function limpiar(value) {
    value.setValue("");
}

function buscador() {
    App.storeDocumentosCargaPlantillas.reload();
}

function editarRutaDocumento() {
    App.txtRuta.hide();
    App.txtRuta.setValue("");
    App.UploadF.show();
    App.btnEditarRuta.hide();
}

// #endregion

// #region DIRECT METHOD TABS
var seleccionadoTab;

var handlePageSizeSelectTab = function (item, records) {
    var curPageSize = App.storeDocumentosCargaPlantillasTabs.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDocumentosCargaPlantillasTabs.pageSize = wantedPageSize;
        App.storeDocumentosCargaPlantillasTabs.load();
    }
}

function Grid_RowSelectTab(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoTab = datos;
        App.btnEditarGrid.enable();
        App.btnEliminarGrid.enable();
        App.btnActivarGrid.enable();

        App.btnEditarGrid.setTooltip(jsEditar);
        App.btnAnadirGrid.setTooltip(jsAgregar);
        App.btnEliminarGrid.setTooltip(jsEliminar);

        if (seleccionadoTab.Activo) {
            App.btnActivarGrid.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarGrid.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrillaTabs() {
    App.GridRowSelectTab.clearSelections();
    App.btnEditarGrid.disable();
    App.btnEliminarGrid.disable();
    App.btnActivarGrid.disable();
}

function VaciarFormularioTab() {
    App.formTab.getForm().reset();
}

function AgregarEditarTab() {
    VaciarFormularioTab();
    App.winTabRules.setTitle(parent.jsAgregar);
    Agregar = true;
    App.winTabRules.show();
}

function FormularioValidoTab(valid) {
    if (valid) {
        App.btnGuardarTab.setDisabled(false);
    }
    else {
        App.btnGuardarTab.setDisabled(true);
    }
}

function winGestionBotonGuardarTab() {
    if (App.formTab.getForm().isValid()) {
        ajaxAgregarEditarTab();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarTab() {
    TreeCore.AgregarEditarTab(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winTabRules.hide();
                    RefrescarTab();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarTab() {
    if (seleccionadoTab != null) {
        ajaxEditarTab();
    }
}

function ajaxEditarTab() {
    Agregar = false;
    App.winTabRules.setTitle(parent.jsEditar);
    TreeCore.MostrarEditarTab(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            }
        });
}

function ActivarTab() {
    if (seleccionadoTab != null) {
        ajaxActivarTab();
    }
}

function ajaxActivarTab() {
    TreeCore.ActivarTab(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                RefrescarTab();
            }
        });
}

function EliminarTab() {
    if (seleccionadoTab != null) {
        Ext.Msg.alert(
            {
                title: parent.jsEliminar,
                msg: parent.jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarTab,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarTab(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarTab({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    RefrescarTab();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function RefrescarTab() {
    DeseleccionarGrillaTabs();
    App.storeDocumentosCargaPlantillasTabs.reload();
}

// #endregion