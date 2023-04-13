
var TabN = 1;
var classActivo = "navActivo";
var classBtnActivo = "btn-ppal-winForm";
var classBtnDesactivo = "btn-secondary-winForm";
var psswrdStrength = 0;

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

    changeTab(sender, index);
}

function btnPrev_Click(sender, registro, index) {
    var panelActual = getPanelActual(sender, registro, index);
    Agregar = false;
    changeTab(sender, --panelActual);
}

function btnNext_Click(sender, registro, index) {
    var panelActual = getPanelActual(sender, registro, index);

    if (panelActual == 0) {
        ajaxAgregarEditar(sender, ++panelActual);
    } else {
        changeTab(sender, ++panelActual);
    }
}

function changeTab(sender, index) {

    var arrayBotones = App.conNavVistasNewContactos.ariaEl.getFirstChild().getFirstChild().dom.children;


    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = document.getElementsByClassName("formGris formResp");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();
        }
        if (index == 1) {
            App.storeRoles.reload();
            App.storeRolesLibres.reload();
            App.btnNext.addCls("btnDisableClick");
        }
        else if (index == 2) {
            App.storeProyectosAgrupaciones.reload();
            App.storeProyectos.reload();
            App.storePermisosAgregados.reload();
        }

        Ext.getCmp(panels[index].id).show();
    }

    if (index == 0) {

        if (App.txtNombre.getValue() != "") {
            App.btnNext.setText(jsGuardado);
            App.btnNext.addCls("btnDisableClick");
            App.btnNext.removeCls("btnEnableClick");
            App.btnNext.setIconCls("ico-tic-wh");
        }
        else {
            App.btnNext.setText(jsGuardar);
            App.btnNext.setIconCls("");
            App.btnNext.removeCls("btnDisableClick");
            App.btnNext.addCls("btnEnableClick");
            App.btnNext.removeCls("animation-text");
        }
        FormularioValido(true);
    }

    App.btnNext.addCls(classBtnActivo);
    App.btnNext.removeCls(classBtnDesactivo);
}

function getPanelActual(sender, registro, index) {

    var panelActual;
    var panels = document.getElementsByClassName("formGris formResp");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
        }
    }
    return panelActual;
}

// #endregion

// #region PERMISOS

function EliminarPermiso(sender, registro, index) {

    showLoadMask(App.winGestionUsuarios, function (load) {

        TreeCore.QuitarPermisos(index.data.UsuariosProyectosID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        recargarCombos([App.cmbProyectosAgrupaciones, App.cmbProyectos], function Fin(fin) {
                            if (fin) {
                                App.storePermisosAgregados.reload();
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

    });

}

function btnGuardarPermiso() {
    ajaxGuardarPermiso();
}

function ajaxGuardarPermiso() {

    showLoadMask(App.winGestionUsuarios, function (load) {

        TreeCore.GuardarPermiso(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        recargarCombos([App.cmbProyectosAgrupaciones, App.cmbProyectos], function Fin(fin) {
                            if (fin) {
                                App.storePermisosAgregados.reload();
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

    });

}

// #endregion

// #region PERFILES

function BotonGuardarPerfiles() {

    TreeCore.AgregarRoles(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.cmbRolesLibres.clearValue();
                    App.storeRoles.reload();
                    App.storeRolesLibres.reload();
                    App.btnNext.setText(jsGuardado);
                    App.btnNext.removeCls("animation-text");
                    App.btnNext.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNext.addCls("animation-text");
                    }, 250);
                }
            }
        });

    App.winGestionUsuarios.update();
}

function EliminarPerfil(sender, registro, index) {

    TreeCore.QuitarRoles(index.data.UsuarioRolID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storeRoles.reload();
                App.storeRolesLibres.reload();
                App.btnNext.setText(jsGuardado);
                App.btnNext.removeCls("animation-text");
                App.btnNext.setIconCls("ico-tic-wh");

                setTimeout(function () {
                    App.btnNext.addCls("animation-text");
                }, 250);

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

    App.winGestionUsuarios.update();

}

//#endregion


function verFechas(sender, registro, index) {

    for (var i = 0; i < registro.length; i++) {
        if (registro[i].data.FechaUltimoAcceso != null) {
            App.colFechaUltimoAcceso.setText(Ext.Date.format(registro[i].data.FechaUltimoAcceso, 'd/m/Y'));
        } 

        if (registro[i].data.FechaUltimoCambio != null) {
            App.colFechaUltimoCambio.setText(Ext.Date.format(registro[i].data.FechaUltimoCambio, 'd/m/Y'));
        } 

        if (registro[i].data.FechaCaducidadUsuario != null) {
            App.colFechaCaducidad.setText(Ext.Date.format(registro[i].data.FechaCaducidadUsuario, 'd/m/Y'));
        }

        if (registro[i].data.FechaCaducidadClave != null) {
            App.colFechaCaducidadClave.setText(Ext.Date.format(registro[i].data.FechaCaducidadClave, 'd/m/Y'));
        }
    }
}

function VaciarFormulario() {

    Ext.each(App.winGestionUsuarios.body.query('*'), function (value) {
        Ext.each(value, function (item) {
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

                    c.addCls("ico-exclamacion-10px-red");
                    c.removeCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });
}

function cambiarLiteral() {
    App.btnNext.setText(jsGuardar);
    App.btnNext.setIconCls("");
    App.btnNext.removeCls("btnDisableClick");
    App.btnNext.addCls("btnEnableClick");
    App.btnNext.removeCls("animation-text");
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function FormularioValido(valid) {

    if (valid == true) {
        App.btnNext.setDisabled(false);
        App.btnPrevTab.setDisabled(false);
        Ext.each(App['formGestion'].body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnNext.setDisabled(true);
                App.lnkPerfiles.setDisabled(true);
            }
        });
    }
    else {
        App.btnNext.setDisabled(true);
        App.lnkPerfiles.setDisabled(true);
    }
}

function ajaxAgregarEditar() {
    App.GridRowSelect.clearSelections();
    App.lnkPerfiles.setDisabled(true);

    showLoadMask(App.winGestionUsuarios, function (load) {

        TreeCore.AgregarEditar(Agregar, Duplicar, {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    load.hide();
                }
                else {
                    recargarCombos([App.cmbRolesLibres], function Fin(fin) {
                        if (fin) {
                            App.lnkPerfiles.setDisabled(false);
                            setTimeout(function () {
                                App.btnNext.addCls("animation-text");
                                App.btnNext.addCls("btnDisableClick");
                                App.btnNext.removeCls("btnEnableClick");
                                load.hide();
                            }, 250);
                        }
                    });
                }
            }
        });
    });

}

function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function DeseaEliminar() {
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsTituloModulo,
            msg: jsTieneRegistrosDeseaEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeseaEliminar,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxDeseaEliminar(button) {
    if (button == 'yes' || button == 'si') {
        ajaxActivar();
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    if (result.Result == jsTieneRegistrosDeseaEliminar) {
                        DeseaEliminar();
                    }
                    else {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function AgregarEditar() {

    App.GridRowSelect.clearSelections();
    VaciarFormulario();
    App.hdUsuarioID.setValue("");
    Agregar = true;
    Duplicar = false;

    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbEntidad], function Fin(fin) {
            if (fin) {

                App.formGestion.show();
                App.txtClave.enable();
                App.txtClave.allowBlank = false;
                App.txtClave.show();
                App.winGestionUsuarios.setTitle(jsAgregar + ' ' + jsTituloModulo);
                App.winGestionUsuarios.show();
                App.lnkPerfiles.setDisabled(true);
                App.txtNombre.focus();

                load.hide();
            }
        });

    });
}

function AbrirDocumentos() {

    showLoadMask(App.grid, function (load) {

        recargarCombos([App.cmbDocumentosTipos], function Fin(fin) {
            if (fin) {
                App.storeDocumentos.reload();
                App.winAnadirDocumentos.show();
                load.hide();

            }
        });
    }

    );
}

function AgregarDocumento() {

    TreeCore.AgregarDocumento({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.cmbDocumentosTipos.clearValue();
                App.uploadFieldDocumento.reset();
                App.storeDocumentos.reload();
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });

}

function ajaxEditar() {

    VaciarFormulario();
    Agregar = false;
    Duplicar = false;

    App.txtClave.setDisabled();
    App.txtClave.allowBlank = true;
    App.txtClave.hide();

    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbEntidad], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {

                                App.formGestion.show();
                                App.FormAnadirPerfiles.hide();
                                App.lnkPerfiles.setDisabled(false);
                                App.btnNext.setText(jsGuardado);
                                App.btnNext.setIconCls("ico-tic-wh");

                                App.winGestionUsuarios.setTitle(jsEditar + " " + jsTituloModulo);
                                App.winGestionUsuarios.show();
                                App.txtNombre.focus();
                            }

                            load.hide();
                        }
                    });
            }
        });

    });
}

function Refrescar() {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

function cerrarWindow() {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
    App.winGestionUsuarios.hide();
}

function guardarCambios(sender, registro, index) {
    App.btnNext.setText(jsGuardado);
    App.btnNext.setIconCls("ico-tic-wh");

    showLoadMask(App.winGestionUsuarios, function (load) {
        TreeCore.ComprobarUsuarioExiste(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Agregar = false;
                        Duplicar = false;

                        ajaxAgregarEditar();
                    }
                    else {
                        if (document.getElementsByClassName("formGris formResp navActivo")[0].id == "formGestion") {
                            ajaxAgregarEditar();
                        }
                    }

                    load.hide();
                }
            });
    });
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {


        let registroSeleccionado = registro;
        let GridSeleccionado = App.grid;
        cargarDatosPanelMoreInfoGrid(registroSeleccionado, GridSeleccionado);

        seleccionado = datos;
        App.btnEditar.enable();
        App.btnAnadir.enable();
        App.btnActivar.enable();
        App.btnDuplicarUsuario.enable();
        App.btnDuplicarConfig.enable();
        App.btnPassword.enable();
        if (seleccionado.MacDispositivo != null) {
            App.btnDesvincularDispositivoMovil.enable();
        } else {
            App.btnDesvincularDispositivoMovil.disable();
        }

        App.btnEditar.setTooltip(jsEditar);
        App.btnDuplicarUsuario.setTooltip(jsDuplicarUsuario);
        App.btnDuplicarConfig.setTooltip(jsDuplicarConfiguracion);
        App.btnPassword.setTooltip(jsContraseña);


        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {

    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnDuplicarUsuario.disable();
    App.btnDuplicarConfig.disable();
    App.btnPassword.disable();
    App.btnDesvincularDispositivoMovil.disable();
}

// #region Activar/Desactivar

function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxActivar();
    }
}

function ajaxActivar() {

    TreeCore.Activar({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                forzarCargaBuscadorPredictivo = true;
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


// #endregion

// #region CAMBIAR CLAVE
function BotonCambiarClave() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        App.txtCambiarClave.setValue("");
        App.txtCambiarClave2.setValue("");
        App.txtCambiarClave.clearInvalid();
        App.txtCambiarClave2.clearInvalid();
        App.winCambiarClave.setTitle(jsClaveCambiar + ' ' + jsTituloModulo);
        App.winCambiarClave.show();
    }
}

function winCambiarClaveBotonCambiar() {
    if (App.formClave.getForm().isValid()) {
        ajaxCambiarClave();
    }
    else {
        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxCambiarClave() {
    var clave = false;

    TreeCore.CambiarClave(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '' && result.Result != "Cambio" && result.Result != 'ClaveRepetida') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result.Result == "Cambio") {
                        Ext.Msg.show({ title: jsInfo, icon: Ext.MessageBox.INFO, msg: jsCambioEfectuado, buttons: Ext.Msg.OK });
                        App.winCambiarClave.hide();
                    }
                    else if (result.Result == "ClaveRepetida") {
                        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsClaveRepetida, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.txtCambiarClave.clear();
                        App.txtCambiarClave2.clear();
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

function BotonCambiarClaveDoble() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        App.txtClaveDoble.setValue("");
        App.txtClaveDobleConfirmar.setValue("");
        App.txtClaveDoble.clearInvalid();
        App.txtClaveDobleConfirmar.clearInvalid();
        App.winClaveDobleValidacion.setTitle(jsClaveCambiar + ' ' + jsTituloModulo);
        App.winClaveDobleValidacion.show();
    }
}

function winCambiarClaveBotonDoble() {
    if (App.formClaveDoble.getForm().isValid()) {
        ajaxCambiarClaveDoble();
    }
    else {
        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxCambiarClaveDoble() {
    var clave = false;


    var clave = txtClaveDoble.getValue();
    var claveConfirmar = txtClaveDobleConfirmar.getValue();
    var esNumeroClave = isNaN(clave);
    var esNumeroClaveConfirm = isNaN(claveConfirmar);

    if (!esNumeroClave && !esNumeroClaveConfirm) {

        if (clave.length == 8 && claveConfirmar.length == 8) {
            if (clave == claveConfirmar) {
                TreeCore.CambiarClaveDoble(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                App.winClaveDobleValidacion.hide();
                            }
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    });
            }
            else {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsClavesNoCoinciden, buttons: Ext.Msg.OK });
            }
        }
        else {
            Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsMenor8, buttons: Ext.Msg.OK });
        }



    }
    else {
        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsClavesNoNumericas, buttons: Ext.Msg.OK });
    }


}

// #endregion

// #region CLIENTES


function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    CargarStores();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

// #endregion

// #region Duplicar Configuración

function winDuplicarConfigBotonGuardar() {
    TreeCore.DuplicarConfig({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsMsgAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function btnDuplicarConfigClick() {

    App.winDuplicarConfig.setTitle(jsDuplicar + ' ' + jsConfiguracion);
    App.formDuplicarConfig.getForm().reset();
    App.winDuplicarConfig.show();
}

// #endregion

// #region Duplicar Usuario

function ajaxDuplicar() {

    showLoadMask(App.grid, function (load) {
        VaciarFormulario();
        App.cmpFiltro_cmbClientes.focus(false, 200);
        App.winGestionUsuarios.setTitle(jsDuplicar + ' ' + jsTituloModulo);
        Agregar = true;
        Duplicar = true;

        App.txtClave.show();
        App.txtClave.enable();
        App.txtClave.allowBlank = false;

        recargarCombos([App.cmbEntidad, App.cmbRolesLibres], function Fin(fin) {
            if (fin) {
                App.FormAnadirPerfiles.hide();
                changeTab('', 0);
                App.winGestionUsuarios.show();
                load.hide();
            }
        });

    });
}

function btnDuplicarUsuarioClick() {

    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxDuplicar();
    }
}

// #endregion

// #region btnDesvincularDispositivoMovil()

function btnDesvincularDispositivoMovil() {

    Ext.Msg.alert(
        {
            title: jsDesvincularDispositivoMovil,
            msg: jsDeseaDesvincularDispositivo,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDesvincularDispositivoMovil,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxDesvincularDispositivoMovil(button) {

    if (button == 'yes' || button == 'si') {
        TreeCore.DesvincularDispositivoMovil({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsMsgAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storePrincipal.reload();
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }

}

// #endregion

//#region TRIGGERS
var TriggerDuplicarConfig = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbUsuarios.clearValue();
            break;
        case 1:
            App.storeUsuarios.reload();
            break;
    }
}

function RecargarTiposDocumentos() {
    recargarCombos([App.cmbDocumentosTipos]);
}

function RecargarEmpresaProveedoraAsociada() {
    recargarCombos([App.cmbEntidad]);
}

function SeleccionarEmpresaProveedora() {
    App.cmbEntidad.getTrigger(0).show();
    FormularioValido(true);
}

function RecargarProyecto() {
    recargarCombos([App.cmbProyectos]);
}

function SeleccionarProyecto() {
    App.cmbProyectos.getTrigger(0).show();
}

function RecargarProyectosAgrupaciones() {
    recargarCombos([App.cmbProyectosAgrupaciones]);
    RecargarProyecto();
}

function SeleccionarProyectosAgrupaciones() {
    App.cmbProyectosAgrupaciones.getTrigger(0).show();
    App.cmbProyectos.clearValue();
    App.storeProyectos.reload();
}

function RecargarPerfilesLibres() {
    recargarCombos([App.cmbRolesLibres]);
}

function SeleccionarPerfilesLibres() {
    App.cmbRolesLibres.getTrigger(0).show();
}

function RecargarEntidad() {
    recargarCombos([App.cmbEntidades]);
    App.storePrincipal.reload();
}

function SeleccionarEntidad() {
    App.cmbEntidades.getTrigger(0).show();
    App.storePrincipal.reload();
}
// #endregion

function SeleccionarComando(command, value) {
    if (command == "DescargarDocumento") {
        if (value.data != null) {
            window.open("../../PaginasComunes/DocumentosDescarga.aspx?DocumentoID=" + value.data.DocumentoID);
        }
    }
}

var SetToggleValue = function (column, cmp, record) {

    var Activo = record.get('DocumentoID');

    if (Activo) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();

}

function cambiarAsignacion(sender, registro, index) {

    TreeCore.ActivarDesactivarDocumento(sender.record.get('DocumentoID'),
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDocumentos.reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function FormularioValidoDuplicarConfig(valid) {

    if (valid) {
        App.btnGuardarDuplicarConfig.setDisabled(false);
    }
    else {
        App.btnGuardarDuplicarConfig.setDisabled(true);
    }
}

function FormularioValidoCambiarClave(valid) {

    if (valid) {
        App.btnCambiarGuardar.setDisabled(false);
    }
    else {
        App.btnCambiarGuardar.setDisabled(true);
    }
}

function CheckPasswordStrength(sender, e) {
    let p = e.target.value;
    psswrdStrength = CalculatePasswordStrength(p);
}

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {

    obj.center();
    obj.update();

}

function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(750);
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
        obj.setWidth(750);
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
        obj.setWidth(750);
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

// #region Context Menu
function irADocumentos(sender, registro, index) {
    //let usuarioID = ;

    if (App.GridRowSelect.selected.items && App.GridRowSelect.selected.items.length > 0) {
        let usuario = App.GridRowSelect.selected.items[0].data;

        let nameObj = jsDocumentos;
        let nameTab = usuario.Nombre + " " + usuario.Apellidos;
        let usuarioID = usuario.UsuarioID;

        var pagina = "/PaginasComunes/DocumentosVista.aspx?ObjetoID=" + usuarioID + "&ObjetoTipo=Usuario&ProyectoTipo=GLOBAL";
        parent.addTab(parent.App.tabPpal, nameObj + usuarioID, nameObj + " " + nameTab, pagina);
    }
}

// #endregion