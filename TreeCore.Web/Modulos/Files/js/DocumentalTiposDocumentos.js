var editarTipoDoc = false;
var registroSeleccionadoEditar;

function ShowProfileOnlyWin() {
    App.lnkExtensions.hide();
    ChangeTabProfile();
    App.winGestionDocumento.show();
}
function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    App.GridOnlyExtensiones.getSelectionModel().deselectAll();
    App.GridOnlyRoles.getSelectionModel().deselectAll();
    App.btnEliminarRoles.disable();
    App.btnEliminarExtension.disable();
    registroSeleccionadoEditar = registro;

    if (registro.raw != undefined) {
        if (registro.raw.EsCarpeta) {
            App.btnAnadirExtension.disable();
            App.btnAnadirRoles.disable();
            App.hdEsCarpeta.setValue(true);
            App.hdCarpetaID.setValue(datos.id);

            App.btnActivar.disable();

            showEditDoc();
            showRemoveDoc();

            hideAddExtensions();
            hideAddProfiles();
            hideRemoveExtensions();
            hideRemoveProfiles();

            App.GridP2Bot.getStore().removeAll();
            App.GridP2Bot.getStore().sync();
            App.hdTipoDocSeleccionado.setValue(0);
            App.GridP3Top.getStore().removeAll();
            App.GridP3Top.getStore().sync();

        }
        else {
            if (datos != null) {
                seleccionado = datos;
                App.hdTipoDocSeleccionado.setValue(datos.id);
                App.btnAnadirExtension.enable();
                App.btnAnadirRoles.enable();
                App.hdEsCarpeta.setValue(false);
                showEditDoc();
                showRemoveDoc();
                showAddExtensions();
                showAddProfiles();

                App.btnActivar.enable();
                if (seleccionado.Activo) {
                    App.btnActivar.setTooltip(jsDesactivar);
                }
                else {
                    App.btnActivar.setTooltip(jsActivar);
                }

                App.btnAnadirRoles.setTooltip(jsAgregar);
                App.btnEliminarRoles.setTooltip(jsEliminar);
                App.btnAnadirExtension.setTooltip(jsAgregar);
                App.btnEditarDocumento.setTooltip(jsEditar);
                App.btnEliminarDocumento.setTooltip(jsEliminar);

                showLoadMask(App.MainVwP, function (load) {

                    CargarStoresSerie([App.StoreExtensiones, App.StoreRoles], function Fin(fin) {
                        if (fin) {
                            load.hide();
                        }
                    });

                });
            }
            App.hdCarpetaID.setValue(0);

        }
    }
}


function Grid_RowSelectRoles(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;
        App.hdDocumentoPerfilID.setValue(datos.PerfilID);
        App.btnEliminarRoles.disabled = false;
        App.btnEliminarRoles.removeCls("x-btn-disabled");
        App.btnEliminarRoles.removeCls("x-item-disabled");
        App.btnEliminarRoles.setTooltip(jsEliminar);
    }

}

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
// Renders Grid
var barGrid = function (value) {

    let colorBar;

    if (value > 0 && value < 0.20) {
        colorBar = 'barRed-grid';
    }
    else if (value >= 0.20 && value < 0.45) {
        colorBar = 'barYellow-grid';
    }

    else if (value >= 0.45 && value < 0.80) {
        colorBar = 'barBlue-grid';
    }

    else if (value >= 0.80 && value <= 1) {
        colorBar = 'barGreen-grid';
    }
    return `<div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value * 100}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value * 100}%;"><div class="x-progress-text" style="width:40px;"><div>${value * 100} %</div></div></div></div>`

}

var rojoRender = function (value) {
    let valorRojo = value;

    if (value != null || value != "") {
        return '<span class="dataRed">' + valorRojo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var amarilloRender = function (value) {
    let valorAmarillo = value;

    if (value != null || value != "") {
        return '<span class="dataYellow">' + valorAmarillo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var DefectoRender = function (value) {
    if (value) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var DocsRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-docsGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var FunctRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-functionalityGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var LinkRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-linkGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var NotifRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-notificationGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var SubprocessRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-subprocessGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}
// FIN Renders Grid

// #endregion

// #region NAVEGACION TABS SUPERIORES
function NavegacionTabs(who) {
    var LNo = who.textEl;

    App.cntWinExtensions.hide();
    App.ctnWinPermissions.hide();

    switch (who.id) {
        case 'lnkExtensions':
            ChangeTab(LNo);
            App.btnNext.show();
            App.btnAdd.hide();
            App.btnPrev.addCls(classBtnActivo);
            App.btnPrev.removeCls(classBtnDesactivo);
            App.btnNext.addCls(classBtnActivo);
            App.btnNext.removeCls(classBtnDesactivo);
            App.cntWinExtensions.show();
            break;

        case 'lnkPermissions':
            ChangeTab(LNo);
            App.btnNext.hide();
            App.btnAdd.show();
            App.btnPrev.addCls(classBtnActivo);
            App.btnPrev.removeCls(classBtnDesactivo);
            App.btnNext.addCls(classBtnActivo);
            App.btnNext.removeCls(classBtnDesactivo);
            App.ctnWinPermissions.show();

            if (App.chkPermisoDescarga.checked || App.chkPermisoEscritura.checked || App.chkPermisoLectura.checked) {
                App.btnAdd.enable();
            }
            else {
                App.btnAdd.disable();
            }

            break;
        default:
            break;
    }
}

// CONTROLES PARA LA WINDOW DE AÑADIR ROLES
function NavegacionTabsOnlyProf(who) {
    var LNo = who.textEl;
    App.ctnOPProfiles.hide();
    App.ctnOPPermissions.hide();
    switch (who.id) {
        case 'lnkProfilesOP':
            ChangeTabOP(LNo);
            App.ctnOPProfiles.show();
            App.ctnOPPermissions.hide();
            App.btnNextProfiles.show();
            App.btnAddProfiles.hide();
            App.btnPrevProfiles.addCls(classBtnDesactivo);
            App.btnPrevProfiles.removeCls(classBtnActivo);
            App.btnNextProfiles.addCls(classBtnActivo);
            App.btnNextProfiles.removeCls(classBtnDesactivo);
            break;
        case 'lnkPermissionsOP':
            ChangeTabOP(LNo);
            App.ctnOPProfiles.hide();
            App.ctnOPPermissions.show();
            App.btnPrevProfiles.addCls(classBtnActivo);
            App.btnPrevProfiles.removeCls(classBtnDesactivo);
            App.btnNextProfiles.hide();
            App.btnAddProfiles.show();
            break;
        default:
            break;
    }
}

function ChangeTab(vago) {
    let ct = document.getElementById('TbNavegacionTabs-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}

function ChangeTabOP(vago) {
    let ct = document.getElementById('TbNavegacionTabsOP-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}

// #endregion



function showNewFolder() {
    App.hdAnadirCarpeta.setValue("true");
    TreeCore.EditarVentanaCarpeta({
        success: function () {
            App.winNewFolder.show();
        }
    });
}

function addNewFolder() {
    showLoadMask(App.MainVwP, function (load) {
        if (App.hdAnadirCarpeta.value == "true") {
            TreeCore.AñadirNuevaCarpeta({
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        load.hide();
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.hdAnadirCarpeta.setValue("false");
                        App.winNewFolder.hide();
                        load.hide();
                        TreeCore.CargarMenu();
                    }
                }
            });
        } else {
            TreeCore.EditarCarpeta({
                success: function () {
                    App.winNewFolder.hide();
                    TreeCore.CargarMenu({
                        success: function () {
                            load.hide();
                        }
                    });

                }
            });
        }
        App.hdCarpetaID.setValue("");
    });
}


var TabN = 1;
var classActivo = "navActivo";
var classBtnActivo = "btn-ppal-winForm";
var classBtnDesactivo = "btn-secondary-winForm";

// #region CONTROL NAVEGACION WIN USUARIOS

function showFormTab(sender, registro, index) {

    var panelActual = getPanelActual(sender, registro, index);
    var index = 0;
    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }

    if (panelActual == 0) {
        ajaxAgregarEditar(sender, index);
    } else {
        changeTab(sender, index);
    }
}

function btnPrev_Click(sender, registro, index) {
    var panelActual = getPanelActual(sender, registro, index);
    changeTabNewDocument(sender, --panelActual);
}

function btnNext_Click(sender, registro, index) {

    if (App.chkPermisoDescarga.checked || App.chkPermisoEscritura.checked || App.chkPermisoLectura.checked) {
        App.btnAdd.enable();
    }
    else {
        App.btnAdd.disable();
    }

    var panelActual = getPanelActual(sender, registro, index);
    changeTabNewDocument(sender, ++panelActual);
}

function changeTabNewDocument(sender, index) {
    var arrayBotones = App.TbNavegacionTabs.ariaEl.getFirstChild().getFirstChild().dom.children;

    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }
        switch (index) {
            case 0:
                App.cntWinExtensions.show();
                App.ctnWinPermissions.hide();
                App.btnPrev.addCls(classBtnActivo);
                App.btnPrev.removeCls(classBtnDesactivo);
                App.btnNext.addCls(classBtnActivo);
                App.btnNext.removeCls(classBtnDesactivo);
                App.btnNext.show();
                App.btnAdd.hide();
                showLoadMask(App.winGestionDocumento, function (load) {
                    if (editarTipoDoc) {
                        TreeCore.cargarExtensionesSeleccionados({
                            success: function () {
                                load.hide();
                            }
                        });
                    } else {
                        load.hide();
                    }

                });
                break;
            case 1:
                App.btnNext.hide();
                App.btnAdd.show();
                App.cntWinExtensions.hide();
                App.ctnWinPermissions.show();
                break;
            default:
                break;
        }
    } else if (index == arrayBotones.length) {
        showLoadMask(App.MainVwP, function (load) {
            App.winGestionDocumento.hide();
            if (editarTipoDoc) {
                TreeCore.EditarTipoDoc({
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            load.hide();
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        } else {
                            load.hide();
                            refreshTreePanel();
                        }
                    }
                });
            } else {
                TreeCore.AñadirNuevoTipoDoc({
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            load.hide();
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        load.hide();
                        refreshTreePanel();
                    }
                });
            }
        });
    }
}

function getPanelActual(sender, registro, index) {
    var panelActual;
    var panels = App.TbNavegacionTabs.ariaEl.getFirstChild().getFirstChild().dom.children;
    for (let i = 0; i < panels.length; i++) {
        let cmp = Ext.getCmp(panels[i].id);
        if (document.getElementById(cmp.id).lastChild.classList.contains(classActivo)) {
            panelActual = i;
        }
    }
    return panelActual;
}

// #endregion

// #region ACCIONES TREEPANEL

var filterTree = function (tf, e) {
    var tree = App.TreePanelV1,
        store = tree.store,
        text = tf.getRawValue();
    tree.clearFilter();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    try {
        var re = new RegExp(".*" + text + ".*", "i");

    } catch (err) {
        return;
    }

    tree.filterBy(function (node) {
        return re.test(node.data.DocumentTipo);
    });

    tree.collapseAll();
    tree.expandAll();


};

function LimpiarFiltroTreePanel() {
    App.TextFilter.setValue("");
    App.TreePanelV1.clearFilter();

}

function refreshTreePanel() {
    hideEditDoc();
    hideRemoveDoc();
    hideAddExtensions();
    hideAddProfiles();
    hideRemoveExtensions();
    App.btnActivar.disable();
    hideRemoveProfiles();
    App.hdCarpetaID.setValue("");
    TreeCore.CargarMenu({
        success: function (result) {
            App.StoreExtensiones.reload();
            App.StoreRoles.reload();
        }
    });
}

function eliminarTipoDoc() {
    Ext.Msg.alert(
        {
            title: jsAtencion,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: confirmarEliminarTipoDoc,
            icon: Ext.MessageBox.QUESTION
        });
}

function confirmarEliminarTipoDoc(button) {
    if (button == "yes" || button == "si") {
        showLoadMask(App.MainVwP, function (load) {
            TreeCore.EliminarDocumento({
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    } else {
                        refreshTreePanel();
                        load.hide();
                    }

                }
            });
        });
    }
}
// #endregion

// #region MOSTRAR Y OCULTAR BOTONES
function showEditDoc() {
    App.btnEditarDocumento.disabled = false;
    App.btnEditarDocumento.removeCls("x-btn-disabled");
    App.btnEditarDocumento.removeCls("x-item-disabled");
}

function hideEditDoc() {
    App.btnEditarDocumento.disabled = true;
    App.btnEditarDocumento.addCls("x-btn-disabled");
    App.btnEditarDocumento.addCls("x-item-disabled");
}

function showRemoveDoc() {
    App.btnEliminarDocumento.disabled = false;
    App.btnEliminarDocumento.removeCls("x-btn-disabled");
    App.btnEliminarDocumento.removeCls("x-item-disabled");
}

function hideRemoveDoc() {
    App.btnEliminarDocumento.disabled = true;
    App.btnEliminarDocumento.addCls("x-btn-disabled");
    App.btnEliminarDocumento.addCls("x-item-disabled");
}


function showRemoveExtensions() {
    App.btnEliminarExtension.disabled = false;
    App.btnEliminarExtension.removeCls("x-btn-disabled");
    App.btnEliminarExtension.removeCls("x-item-disabled");
    App.btnEliminarExtension.setTooltip(jsEliminar);
}

function hideRemoveExtensions() {
    App.btnEliminarExtension.disabled = true;
    App.btnEliminarExtension.addCls("x-btn-disabled");
    App.btnEliminarExtension.addCls("x-item-disabled");    
}

function hideRemoveProfiles() {
    App.btnEliminarRoles.disabled = true;
    App.btnEliminarRoles.addCls("x-btn-disabled");
    App.btnEliminarRoles.addCls("x-item-disabled");
}

function showAddExtensions() {
    App.btnAnadirExtension.disabled = false;
    App.btnAnadirExtension.removeCls("x-btn-disabled");
    App.btnAnadirExtension.removeCls("x-item-disabled");
}

function hideAddExtensions() {
    App.btnAnadirExtension.disabled = true;
    App.btnAnadirExtension.addCls("x-btn-disabled");
    App.btnAnadirExtension.addCls("x-item-disabled");
}

function showAddProfiles() {
    App.btnAnadirRoles.disabled = false;
    App.btnAnadirRoles.removeCls("x-btn-disabled");
    App.btnAnadirRoles.removeCls("x-item-disabled");
}

function hideAddProfiles() {
    App.btnAnadirRoles.disabled = true;
    App.btnAnadirRoles.addCls("x-btn-disabled");
    App.btnAnadirRoles.addCls("x-item-disabled");
}
// #endregion

// #region ELIMINAR PROYECTOS, EXTENSIONES Y ROLES


function deleteExtensions() {
    Ext.Msg.alert(
        {
            title: jsAtencion,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: confirmDeleteExtensions,
            icon: Ext.MessageBox.QUESTION
        });
}

function confirmDeleteExtensions(button) {
    if (button == "yes" || button == "si") {
        TreeCore.eliminarExtensionDeTipoDoc({
            success: function () {
                hideRemoveExtensions();
                App.StoreExtensiones.reload();
            }
        });
    }
}

function deleteProfiles() {
    Ext.Msg.alert(
        {
            title: jsAtencion,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: confirmDeleteProfiles,
            icon: Ext.MessageBox.QUESTION
        });
}

function confirmDeleteProfiles(button) {
    if (button == "yes" || button == "si") {
        TreeCore.eliminarPerfilDeTipoDoc({
            success: function (result) {

                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                hideRemoveProfiles();
                App.StoreRoles.reload();
            }
        });
    }
}

// #endregion

// #region SHOW ONLY WINDOWS

function showOnlyExtensiones() {
    App.GridOnlyExtensiones.getSelectionModel().deselectAll();
    VaciarFormularioExtensiones()
    App.winOnlyExtensions.show();
    showLoadMask(App.winOnlyExtensions, function (load) {
        CargarStoresSerie([App.StoreOnlyExtensiones], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });
    });

}

function showOnlyRoles() {
    App.GridOnlyRoles.getSelectionModel().deselectAll();
    VaciarFormularioRoles();
    App.lnkProfilesOP.click();
    App.winAddRoles.show();
    showLoadMask(App.winAddRoles, function (load) {
        CargarStoresSerie([App.StoreRolesLibres], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });
    });

}
// #endregion

// REGION ACTIVAR

function Activar() {
    if (seleccionado != null) {
        ajaxActivar();
    }
}

function ajaxActivar() {

    TreeCore.Activar({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }

    });
    App.btnActivar.disable();
}

// FIN REGION ACTIVAR

// #region AÑADIR  EXTENSIONES Y ROLES

function addOnlyExtensiones() {
    if (App.GridOnlyExtensiones.getSelectionModel().selected.items.length == 0) {
        Ext.Msg.show({
            title: jsAtencion,
            msg: jsNoItemsSeleccionados,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
    }
    else {
        TreeCore.añadirExtensionDeTipoDoc({
            success: function () {
                App.StoreExtensiones.reload();
                App.StoreOnlyExtensiones.reload();
                App.winOnlyExtensions.hide();
            }
        });
    }
}

function addOnlyRoles() {
    if (App.GridOnlyRoles.getSelectionModel().selected.items.length == 0) {
        Ext.Msg.show({
            title: jsAtencion,
            msg: jsNoItemsSeleccionados,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
    } else {
        TreeCore.añadirPerfilDeTipoDoc({
            success: function () {
                forzarCargaBuscadorPredictivo = true;
                App.StoreRoles.reload();
                App.StoreRolesLibres.reload();
                App.winAddRoles.hide();
            }
        });
    }
}
// #endregion

// #region CONTROL NAVEGACION WIN PROFILES

function showFormTabProfiles(sender, registro, index) {

    var panelActual = getPanelActualProfiles(sender, registro, index);
    var index = 0;
    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }

    if (panelActual == 0) {
        ajaxAgregarEditar(sender, index);
    } else {
        changeTab(sender, index);
    }
}

function btnPrevProfiles_Click(sender, registro, index) {
    var panelActual = getPanelActualProfiles(sender, registro, index);
    changeTabProfiles(sender, --panelActual);
}

function btnNextProfiles_Click(sender, registro, index) {
    var panelActual = getPanelActualProfiles(sender, registro, index);
    changeTabProfiles(sender, ++panelActual);
}

function changeTabProfiles(sender, index) {
    var arrayBotones = App.TbNavegacionTabsOP.ariaEl.getFirstChild().getFirstChild().dom.children;

    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }
        switch (index) {
            case 0:
                App.ctnOPProfiles.show();
                App.ctnOPPermissions.hide();
                App.btnNextProfiles.show();
                App.btnAddProfiles.hide();
                App.btnPrevProfiles.addCls(classBtnDesactivo);
                App.btnPrevProfiles.removeCls(classBtnActivo);
                App.btnNextProfiles.addCls(classBtnActivo);
                App.btnNextProfiles.removeCls(classBtnDesactivo);
                break;
            case 1:
                App.ctnOPProfiles.hide();
                App.ctnOPPermissions.show();
                App.btnPrevProfiles.addCls(classBtnActivo);
                App.btnPrevProfiles.removeCls(classBtnDesactivo);
                App.btnNextProfiles.hide();
                App.btnAddProfiles.show();
                break;
            default:
                break;
        }
    } else if (index == arrayBotones.length) {
        addOnlyRoles();
    }
}

function getPanelActualProfiles(sender, registro, index) {
    var panelActual;
    var panels = App.TbNavegacionTabsOP.ariaEl.getFirstChild().getFirstChild().dom.children;
    for (let i = 0; i < panels.length; i++) {
        let cmp = Ext.getCmp(panels[i].id);
        if (document.getElementById(cmp.id).lastChild.classList.contains(classActivo)) {
            panelActual = i;
        }
    }
    return panelActual;
}

// #endregion

// #region AGREGAR/EDITAR DOCUMENTOS

function agregarEditarDocumento() {
    showLoadMask(App.MainVwP, function (load) {
        App.chkPermisoLectura.setValue("");
        App.chkPermisoEscritura.setValue("");
        App.chkPermisoDescarga.setValue("");
        App.lnkExtensions.click();
        VaciarFormularioDocumentos();
        CargarStoresSerie([App.StoreGridExtensiones], function Fin(fin) {
            if (fin) {
                App.winGestionDocumento.setTitle(jsAgregar);
                App.winGestionDocumento.show();
                load.hide();
            }
        });

    });
}
function MostrarEditarDocumento() {

    if (registroSeleccionadoEditar.raw.EsCarpeta) {
        App.hdAnadirCarpeta.setValue("false");
        if (App.hdEsCarpeta.value == "true") {
            TreeCore.EditarVentanaCarpeta({
                success: function () {
                    App.winNewFolder.show();
                }
            });
        }
    } else {
        if (registroSeleccionado(App.TreePanelV1) && seleccionado != null) {
            ajaxEditar();
        }
    }


}

function ajaxEditar() {
    App.hdAnadirCarpeta.setValue("false");
    if (App.hdEsCarpeta.value == "true") {
        TreeCore.EditarVentanaCarpeta({
            success: function () {
                App.winNewFolder.show();
            }
        });
    }
    else {
        App.winGestionDocumentosExtensiones.getSelectionModel().deselectAll();
        App.chkPermisoLectura.setValue("");
        App.chkPermisoEscritura.setValue("");
        App.chkPermisoDescarga.setValue("");
        App.lnkExtensions.click();
        VaciarFormularioDocumentos();
        showLoadMask(App.winGestionDocumento, function (load) {
            CargarStoresSerie([App.StoreGridExtensiones], function Fin(fin) {
                if (fin) {
                    TreeCore.MostrarEditarDocumento({
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                App.winGestionDocumento.setTitle(jsEditar);
                                App.winGestionDocumento.show();
                            }
                            load.hide();
                        }
                    });
                }

            });
        });
    }
}

function winGestionAgregarEditarDocumento() {
    if (App.formGestionDocumentos.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (App.winGestionDocumento.title.startsWith(jsAgregar)) {
        Agregar = true;
    }

    TreeCore.AgregarEditarDocumento(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionDocumento.hide();
                    refreshTreePanel();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });


}

function FormularioValido(valid) {
    if (valid) {
        App.btnAnadirCarpeta.setDisabled(false);
    }
    else {
        App.btnAnadirCarpeta.setDisabled(true);
    }
}

function VaciarFormularioDocumentos() {
    App.formGestionDocumentos.getForm().reset();
}
function VaciarFormularioRoles() {
    App.formGestionRoles.getForm().reset();
}
function VaciarFormularioExtensiones() {
    App.formGestionExtensiones.getForm().reset();
}

function FormularioValidoDocumentos(sender, valid) {
    if (valid) {
        App.btnAdd.setDisabled(false);
    }
    else {
        App.btnAdd.setDisabled(true);
    }
}

function changeChkPermisosRoles() {

    if (App.chkPermisoLecturaRoles.value ||
        App.chkPermisoDescargaRoles.value ||
        App.chkPermisoEscrituraRoles.value) {
        App.btnAddProfiles.enable();
    }
    else {
        App.btnAddProfiles.disable();
    }
}

function changeChkPermisos() {

    if (App.chkPermisoLectura.value ||
        App.chkPermisoDescarga.value ||
        App.chkPermisoEscritura.value) {
        App.btnAdd.enable();
    } else {
        App.btnAdd.disable();
    }
}

// #region PLANTILLA
var PuntoCorteL = 900;
var PuntoCorteS = 512;

var selectedCol = 0;
var isOnMobC = 0;

function ControlSlider(sender) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var pnmain = App.TreePanelV1;
    var col2 = Ext.getCmp('pnCol1');
    var tbsliders = Ext.getCmp('tbSliders');
    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        col2.show();
        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();



        if (selectedCol == 3) {
            col2.hide();
        }
        else {
            col2.show();
        }
        isOnMobC = 0;




    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true || col2.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}


function SliderMove(NextOrPrev) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    var pnmain = Ext.getCmp('TreePanelV1');
    var col1 = Ext.getCmp('pnCol1');

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            col1.show();
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
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }


    }


}

// #endregion

// #region DragAndDrop
function BeforeDropNodo(node, data, overModel, dropPosition, dropHandlers) {

    var targetNodeID = data.records[0].getId();
    var destinationNodeID = overModel.getId();

    dropHandlers.wait = true;

    let records = [];
    data.records.forEach(function (record) {
        records.push(record.data);
    });


    let targetsJson = JSON.stringify({ records: records });
    let destinationTypeElement = JSON.stringify(overModel.data);

    showLoadMask(App.TreePanelV1, function (load) {
        TreeCore.BeforeDropNodo(targetsJson, destinationNodeID, destinationTypeElement,
            {
                success: function (result) {
                    load.hide();
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
                        dropHandlers.cancelDrop();
                    }
                    else {
                        refreshTreePanel();
                    }
                },
            });
    });

}
// #endRegion