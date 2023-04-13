// #region DIRECT METHOD
let arrayRolesAsignados = [];
var amsifySuggestagsRoles = null;
let parametro = '';

function cargarComboRoles() {
    arrayRolesAsignados = [];
    var arrayRoles = App.storeRoles.data.items;
    var roles = [];

    for (var i = 0; i < arrayRoles.length; i++) {
        roles[i] = { 'tag': arrayRoles[i].data.Codigo, 'value': arrayRoles[i].data.RolID };
    }
    if (amsifySuggestagsRoles != null) {
        amsifySuggestagsRoles.destroy();
    }

    amsifySuggestagsRoles = new AmsifySuggestags($('input[name="roles"]'));

    amsifySuggestagsRoles._settings({
        suggestions: roles,
        whiteList: true,
        afterAdd: function (value) {
            arrayRolesAsignados.push(value);

            if (arrayRolesAsignados.length > 0 && App.txtGrupoAcceso.value != "" && App.txtURL.value != "") {
                FormularioValido(true);
            }
            else {
                FormularioValido(false);
            }
        },
        afterRemove: function (value) {
            arrayRolesAsignados.pop(value);

            if (arrayRolesAsignados.length > 0 && App.txtGrupoAcceso.value != "" && App.txtURL.value != "") {
                FormularioValido(true);
            }
            else {
                FormularioValido(false);
            }
        }
    });

    document.getElementsByName('roles')[0].value = '';
    document.getElementsByName('roles')[0].placeholder = jsAgregar + '  Rol';
    amsifySuggestagsRoles._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[0].style = 'max-height: 60px; overflow-y: auto;';

    var rol = [];
    var arrayAsignados = App.storeRolesAsignados.data.items;
    for (var i = 0; i < arrayAsignados.length; i++) {
        rol = arrayAsignados[i].data.RolID;
        amsifySuggestagsRoles.addTag(rol);
    }
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnAnadir.enable();
        App.btnEliminar.setTooltip(jsEliminar);
        App.hdGrupoAccesoWeb.setValue(seleccionado.GrupoAccesoWebRolID);
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function VaciarFormulario() {
    App.formGestion.getForm().reset();

    document.getElementsByName('roles')[0].value = '';
    document.getElementsByName('roles')[0].placeholder = jsAgregar + '  Rol';
    amsifySuggestagsRoles._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[0].style = 'max-height: 60px; overflow-y: auto;';
}

function FormularioValido(valid) {
    if (App.txtGrupoAcceso.value != "" && App.txtURL.value != "") {
        if (arrayRolesAsignados.length > 0 && valid) {
            App.btnGuardar.setDisabled(false);
        }
        else {
            App.btnGuardar.setDisabled(true);
        }
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function AgregarEditar() {
    CargarStoresSerie([App.storeRoles], function Fin(fin) {
        if (fin) {
            cargarComboRoles();
            VaciarFormulario();
            App.winGestion.setTitle(jsAgregar);
            Agregar = true;
            App.winGestion.show();
        }
    });
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {
    var RolesAsignados = [];
    var cantidad = App.storeRolesAsignados.data.items.length;

    for (var i = 0; i < cantidad; i++) {
        RolesAsignados.push(App.storeRolesAsignados.data.items[i].id);
    }

    TreeCore.AgregarEditar(Agregar, arrayRolesAsignados, RolesAsignados,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    arrayRolesAsignados = [];
                    App.winGestion.hide();
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
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar);

    CargarStoresSerie([App.storeRoles, App.storeRolesAsignados], function Fin(fin) {
        if (fin) {
            cargarComboRoles();
            TreeCore.MostrarEditar(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
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

function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

function cambiarFactor() {
    parametro = 'FACTOR';
    TreeCore.CambiarParametro(parametro,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function cambiarUsuario(sender, registro, index) {
    parametro = 'USUARIO';
    TreeCore.CambiarParametro(parametro, sender.pressed,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function cambiarMultihoming() {
    parametro = 'MULTIHOMING';
    TreeCore.CambiarParametro(parametro, '',
        {
            success: function (result) {
                if (result.success != null && result.success != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else if (result.Result != null && result.Result != '') {
                    if (result.Result == 'SI' || result.Result == 'YES') {
                        DeseleccionarGrilla();
                        App.storePrincipal.reload();
                        App.btnAnadir.enable();
                        App.btnRefrescar.enable();
                        App.btnDescargar.enable();
                    }
                    else if (result.Result == 'NO') {
                        App.btnAnadir.disable();
                        App.btnEditar.disable();
                        App.btnEliminar.disable();
                        App.btnRefrescar.disable();
                        App.btnDescargar.disable();
                        App.storePrincipal.removeAll();
                    }
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

// #endregion

// #region LDAP

function Grid_RowSelectLDAP(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoLDAP = datos;
        App.btnEditarLDAP.enable();
        App.btnEliminarLDAP.enable();

        App.btnEditarLDAP.setTooltip(jsEditar);
        App.btnAnadirLDAP.setTooltip(jsAgregar);
        App.btnAnadirLDAP.enable();
        App.btnEliminarLDAP.setTooltip(jsEliminar);
    }
}

function DeseleccionarGrillaLDAP() {

    if (App.btnToggleLDAP.pressed) {
        App.GridRowSelectLDAP.clearSelections();
        App.btnEditarLDAP.disable();
        App.btnEliminarLDAP.disable();
        App.btnRefrescarLDAP.enable();
        App.btnDescargarLDAP.enable();
        App.btnTgLoginCreated.enable();

        App.btnAnadirLDAP.setTooltip(jsAgregar);
        App.btnRefrescarLDAP.setTooltip(jsRefrescar);
        App.btnDescargarLDAP.setTooltip(jsDescargar);

        FormularioValidoLDAP('', false, '');
    }
}

var handlePageSizeSelectLDAP = function (item, records) {
    var curPageSize = App.storeToolConexiones.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeToolConexiones.pageSize = wantedPageSize;
        App.storeToolConexiones.load();
    }
}

function VaciarFormularioLDAP() {
    App.formLDAP.getForm().reset();
}

function AgregarEditarLDAP() {
    VaciarFormularioLDAP();
    App.winGestionLDAP.setTitle(jsAgregar);
    AgregarLDAP = true;
    App.winGestionLDAP.show();
}

function winGestionBotonGuardarLDAP() {
    if (App.formLDAP.getForm().isValid()) {
        ajaxAgregarEditarLDAP();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarLDAP() {
    TreeCore.AgregarEditarLDAP(AgregarLDAP,
        {
            success: function (result) {
                if (result.success != null && result.success != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result.Result != "error") {
                        App.winGestionLDAP.hide();
                        App.storeToolConexiones.reload();
                    }
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarLDAP() {
    if (registroSeleccionado(App.gridLDAP) && seleccionadoLDAP != null) {
        ajaxEditarLDAP();
    }
}

function ajaxEditarLDAP() {
    VaciarFormularioLDAP();
    AgregarLDAP = false;
    App.winGestionLDAP.setTitle(jsEditar);

    TreeCore.MostrarEditarLDAP(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function EliminarLDAP() {
    if (registroSeleccionado(App.gridLDAP) && seleccionadoLDAP != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarLDAP,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarLDAP(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarLDAP({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeToolConexiones.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function RefrescarLDAP() {
    App.storeToolConexiones.reload();
    App.GridRowSelectLDAP.clearSelections();
    App.btnEditarLDAP.setDisabled(true);
    App.btnEliminarLDAP.setDisabled(true);
}

function addlistenerValidacion(sender, registro, index) {

    Ext.each(App.formLDAP.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('validitychange', FormularioValidoLDAP);
        }
    });

}

function FormularioValidoLDAP(sender, valid, registro) {
    App.btnGuardarLDAP.setDisabled(false);

    Ext.each(App.formLDAP.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo")
            && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
            App.btnGuardarLDAP.setDisabled(true);
        }
    });
}

function cambiarLDAP(sender, registro, value) {
    parametro = 'LDAP';
    TreeCore.CambiarParametro(parametro, '',
        {
            success: function (result) {
                if (result.Result == 'SI' || result.Result == 'YES') {
                    DeseleccionarGrillaLDAP();
                    App.storeToolConexiones.reload();
                    App.btnTgLoginCreated.enable();
                    App.btnAnadirLDAP.enable();
                    App.btnRefrescarLDAP.enable();
                    App.btnDescargarLDAP.enable();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

    if (sender.pressed) {
        App.btnGuardarLDAP.setDisabled(false);
    }
    else {
        App.formLDAP.getForm().reset();
        App.btnGuardarLDAP.setDisabled(true);
        App.txtServer.setValue("");
        App.txtUsuario.setValue("");
        App.txtPasswordField.setValue("");
        App.txtPasswordConfirm.setValue("");
    }
}

// #endregion