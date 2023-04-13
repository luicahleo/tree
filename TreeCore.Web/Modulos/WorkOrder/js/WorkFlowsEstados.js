//#region DIRECT METHOD ESTADOS
let slickCreado = false;
let arrayRolesAsignados = [];
let arrayUsuariosAsignados = [];
let arrayRolesEstadosSeguimientoAsignados = [];
let arrayTareasEstadosAsignados = [];
let agregarNotificacion = false;

function Grid_RowSelectEstados(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnDuplicar.enable();
        App.btnActivar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnDuplicar.setTooltip(jsDuplicar);
        App.btnRefrescar.setTooltip(jsRefrescar);
        App.btnDescargar.setTooltip(jsDescargar);
        App.btnExport.setTooltip(jsExportar);
        parent.App.hdWorkflowPadreID.setValue(App.cmbWorkflows.getValue());

        let registroSeleccionado = registro;
        let GridSeleccionado = App.gridMain1;
        App.hdEstadoID.setValue(seleccionado.CoreEstadoID);
        parent.App.hdEstadoPadreID.setValue(seleccionado.CoreEstadoID);
        App.hdEstado.setValue(registro);

        App.storeTareasEstadosAsignados.reload();
        App.storeRolesEstadosSeguimiento.reload();

        parent.displayMenu('pnMoreInfo', false);
        parent.cargarDatosPanelMoreInfoGridWF(registroSeleccionado, GridSeleccionado);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {

    if (App.hdCliID.value == 0 || App.hdCliID.value == undefined) {
        App.btnAnadir.disable();
    } else {
        App.btnAnadir.enable();
    }

    App.GridRowSelectEstados.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();
    App.btnDuplicar.disable();
    App.btnActivar.disable();
}

function ProgressBarForm() {
    App.progressBar.setValue(App.txtProgress.value / 100);
}

function VaciarFormulario() {
    App.containerFormEstados.getForm().reset();
    App.progressBar.setValue();

    Ext.each(App.containerFormEstados.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);
                    c.addListener("change", FormularioValido, false);

                    c.addListener("change", cambiarLiteral, false);

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

function FormularioValido(sender, valid) {

    if (valid != null) {
        App.btnNextEstado.setDisabled(false);

        Ext.each(App.containerFormEstados.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo")
                && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnNextEstado.setDisabled(true);
            }
        });

    }
    else {
        App.btnNextEstado.setDisabled(true);
        App.lnkFormGlobalSTS.setDisabled(true);
        App.lnkFormNextSTS.setDisabled(true);
        App.lnkFormDocs.setDisabled(true);
        App.lnkFormProfile.setDisabled(true);
        App.lnkFormNotification.setDisabled(true);
    }
}

function AgregarEditar(sender, registro, index) {
    VaciarFormulario();
    App.winGestionEstados.setTitle(jsAgregar + ' ' + jsEstado);
    Agregar = true;
    Duplicar = false;
    cambiarATapEstado(undefined, 0);
    App.winGestionEstados.show();
    App.hdEstadoID.setValue("");
    App.storeRoles.reload();
    App.cmbDepartamento.getTrigger(0).hide();
    App.cmbGrupos.getTrigger(0).hide();
    App.cmbEstadosGlobales.getTrigger(0).hide();
    //App.cmbSubprocesos.getTrigger(0).hide();
    //App.cmbWorkflow.getTrigger(0).hide();
    App.cmbName.getTrigger(0).hide();
    //App.cmbEstado.getTrigger(0).hide();
    App.cmbInformaciones.getTrigger(0).hide();
    App.cmbTareasAcciones.getTrigger(0).hide();
    //App.cmbRoles.getTrigger(0).hide();
    App.cmbObject.getTrigger(0).hide();
    App.cmbEstadosGlobales.getTrigger(0).hide();
    App.formState.show();

    App.lnkFormGlobalSTS.setDisabled(true);
    App.lnkFormNextSTS.setDisabled(true);
    App.lnkFormDocs.setDisabled(true);
    App.lnkFormProfile.setDisabled(true);
    App.lnkFormNotification.setDisabled(true);

    App.storeDepartamentos.reload();
    App.storeEstadosAgrupaciones.reload();
    App.storeWor
}

function ajaxAgregarEditar(sender, panelActual) {

    showLoadMask(App.winGestionEstados, function (load) {
        TreeCore.AgregarEditar(Agregar, Duplicar,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        App.btnNextEstado.setText(jsGuardar);
                        App.btnNextEstado.setIconCls("");
                        load.hide();
                    }
                    else {
                        Refrescar();

                        App.storeCoreObjetosNegocioTipos.reload();
                        App.gridFiltersObjetos.clearFilters();

                        App.storeCoreEstadosSiguientes.reload();
                        App.gridFiltersEstadosSiguientes.clearFilters();

                        App.storeCoreEstadosRoles.reload();

                        App.storeCoreEstadosGlobales.reload();
                        App.gridFiltersEstadosGlobales.clearFilters();

                        App.storeEstadosGlobales.reload();
                        App.storeEstadosAgrupaciones.reload();
                        App.storeInventarioElementosAtributosEstados.reload();
                        App.storeCoreEstadosTareas.reload();
                        App.storeDocumentosEstados.reload();
                        App.storeRoles.reload();
                        App.storeUsuarios.reload();
                        App.storeCoreEstadosNotificaciones.reload();

                        cargarComboUsuarios();
                        cargarComboRoles();
                        //cargarComboRolesEstadosSeguimiento();
                        //cargarComboRolesTareasEstados();

                        App.lnkFormGlobalSTS.setDisabled(false);
                        //App.lnkFormSubprocess.setDisabled(false);
                        App.lnkFormNextSTS.setDisabled(false);
                        //App.lnkFormLinks.setDisabled(false);
                        App.lnkFormDocs.setDisabled(false);
                        App.lnkFormProfile.setDisabled(false);
                        App.lnkFormNotification.setDisabled(false);
                        App.lnkFormState.setDisabled(false);

                        App.btnNextEstado.setDisabled(false);

                        setTimeout(function () {
                            App.btnNextEstado.addCls("animation-text");
                            App.btnNextEstado.addCls("btnDisableClick");
                            App.btnNextEstado.removeCls("btnEnableClick");
                            App.btnNextEstado.setText(jsGuardado);
                            App.btnNextEstado.setIconCls("ico-tic-wh");
                            load.hide();
                        }, 250);
                    }
                }
            });
    });

}

function MostrarEditar() {
    if (registroSeleccionado(App.gridMain1) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    Duplicar = false;

    App.cmbDepartamento.getTrigger(0).show();
    App.cmbGrupos.getTrigger(0).show();
    //App.cmbSubprocesos.getTrigger(0).show();
    App.cmbWorkflows.getTrigger(0).show();
    App.cmbName.getTrigger(0).show();
    //App.cmbEstado.getTrigger(0).show();
    App.cmbInformaciones.getTrigger(0).show();
    App.cmbTareasAcciones.getTrigger(0).show();
    //App.cmbRoles.getTrigger(0).show();

    App.btnNextEstado.setDisabled(false);

    showLoadMask(App.gridMain1, function (load) {
        recargarCombos([App.cmbInformaciones, App.cmbTareasAcciones, App.cmbName], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                App.btnNextEstado.setText(jsGuardar);
                                App.btnNextEstado.setIconCls("");
                            }
                            else {
                                App.formState.show();
                                App.formGlobalSTS.hide();
                                //App.formSubprocess.hide();
                                App.formNextSTS.hide();
                                //App.formLinks.hide();
                                App.formDocs.hide();
                                App.formProfile.hide();
                                App.formNotification.hide();

                                App.storeDepartamentos.reload();
                                App.storeCoreObjetosNegocioTipos.reload();
                                App.gridFiltersObjetos.clearFilters();

                                App.storeCoreEstadosSiguientes.reload();
                                App.gridFiltersEstadosSiguientes.clearFilters();

                                App.storeCoreEstadosRoles.reload();

                                App.storeCoreEstadosGlobales.reload();
                                App.gridFiltersEstadosGlobales.clearFilters();

                                App.storeEstadosGlobales.reload();
                                App.storeEstadosAgrupaciones.reload();
                                App.storeInventarioElementosAtributosEstados.reload();
                                App.storeDocumentosEstados.reload();
                                App.storeCoreEstadosTareas.reload();
                                App.storeRoles.reload();
                                App.storeUsuarios.reload();
                                App.storeCoreEstadosNotificaciones.reload();

                                cargarComboUsuarios();
                                cargarComboRoles();
                                cargarComboRolesEstadosSeguimiento();
                                cargarComboRolesTareasEstados();

                                cambiarATapEstado(undefined, 0);
                                App.winGestionEstados.setTitle(jsEditar + " " + jsEstado);
                                App.winGestionEstados.show();

                                App.btnNextEstado.setText(jsGuardado);
                                App.btnNextEstado.removeCls("btnEnableClick");
                                App.btnNextEstado.setIconCls("ico-tic-wh");

                                setTimeout(function () {
                                    App.lnkFormGlobalSTS.setDisabled(false);
                                    //App.lnkFormSubprocess.setDisabled(false);
                                    App.lnkFormNextSTS.setDisabled(false);
                                    //App.lnkFormLinks.setDisabled(false);
                                    App.lnkFormDocs.setDisabled(false);
                                    App.lnkFormProfile.setDisabled(false);
                                    App.lnkFormNotification.setDisabled(false);
                                }, 250);

                                var contRolesEscritura = document.getElementById('pnRolestagsEscritura');

                                if (App.btnWorkFlowPublicRolEscritura.pressed) {
                                    App.labelPrivado.enable();
                                    App.labelPublico.disable();
                                    contRolesEscritura.classList.remove('tagsPublicos');
                                    contRolesEscritura.classList.add('tagsPrivados');
                                    valor = "Privado";
                                }
                                else {
                                    App.labelPrivado.disable();
                                    App.labelPublico.enable();
                                    contRolesEscritura.classList.add('tagsPublicos');
                                    contRolesEscritura.classList.remove('tagsPrivados');
                                    valor = "Publico";
                                }

                                var contRoles = document.getElementById('pnRolestags');

                                if (App.btnWorkFlowPublicRol.pressed) {
                                    App.lbPrivate.enable();
                                    App.lbPublic.disable();
                                    contRoles.classList.remove('tagsPublicos');
                                    contRoles.classList.add('tagsPrivados');
                                    valor = "Privado";
                                }
                                else {
                                    App.lbPrivate.disable();
                                    App.lbPublic.enable();
                                    contRoles.classList.add('tagsPublicos');
                                    contRoles.classList.remove('tagsPrivados');
                                    valor = "Publico";
                                }
                            }

                            load.hide();
                        }
                    });
            }
        });

    });
}

function Eliminar() {
    if (registroSeleccionado(App.gridMain1) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsEstado,
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
                    Refrescar();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Defecto() {
    TreeCore.AsignarPorDefecto(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
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

function Activar() {
    if (registroSeleccionado(App.gridMain1) && seleccionado != null) {
        if (seleccionado.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        }
        else {
            ajaxActivar();
        }
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
                    App.storeCoreEstados.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Refrescar() {
    App.storeCoreEstados.reload();
    App.GridRowSelectEstados.clearSelections();
}

function cerrarWindow() {
    App.storeCoreEstados.reload();
    App.GridRowSelectEstados.clearSelections();
    App.winGestionEstados.hide();
}

function guardarCambios(sender, registro, index) {
    App.btnNextEstado.setText(jsGuardado);
    App.btnNextEstado.setIconCls("ico-tic-wh");

    showLoadMask(App.winGestionEstados, function (load) {
        TreeCore.ComprobarEstadoExiste(
            {
                success: function (result) {
                    if (result.Result == 'Codigo') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: 'Registration already exists', buttons: Ext.Msg.OK });
                        App.btnNextEstado.setText(jsGuardar);
                        App.btnNextEstado.setIconCls("");
                    }
                    else {
                        if (document.getElementsByClassName("winGestion-panel navActivo")[0].id == "formState") {
                            ajaxAgregarEditar();
                        }
                    }

                    load.hide();
                }
            });
    });
}

function ajaxDuplicar() {
    showLoadMask(App.gridMain1, function (load) {
        VaciarFormulario();
        App.winGestionEstados.setTitle(jsDuplicar + ' ' + jsEstado);
        Agregar = true;
        Duplicar = true;
        App.winGestionEstados.show();
        cambiarATapEstado(undefined, 0);
        load.hide();
        Refrescar();

        App.storeDepartamentos.reload();
        App.storeEstadosAgrupaciones.reload();

        App.lnkFormGlobalSTS.setDisabled(true);
        //App.lnkFormSubprocess.setDisabled(true);
        App.lnkFormNextSTS.setDisabled(true);
        //App.lnkFormLinks.setDisabled(true);
        App.lnkFormDocs.setDisabled(true);
        App.lnkFormProfile.setDisabled(true);
        App.lnkFormNotification.setDisabled(true);
    });
}

function btnDuplicar() {
    if (registroSeleccionado(App.gridMain1) && seleccionado != null) {
        ajaxDuplicar();
    }
}

function DuplicarWorkFlow() {
    TreeCore.DuplicarWorkflow({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsMsgAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.winDuplicarWorkflow.hide();
                Refrescar();
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function btnDuplicarWorkflow() {

    App.winDuplicarWorkflow.setTitle(jsDuplicar + ' ' + jsWorkFlow);
    App.formDuplicarWorkflow.getForm().reset();
    App.winDuplicarWorkflow.show();
}

function FormularioValidoDuplicarWorkflow(valid) {

    if (valid) {
        App.btnGuardarDuplicarWorkflow.setDisabled(false);
    }
    else {
        App.btnGuardarDuplicarWorkflow.setDisabled(true);
    }
}

function BotonExportarFlujo() {
    TreeCore.ExportarFlujo({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsEstado, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                Refrescar();
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function BotonImportarFlujo() {
    TreeCore.ImportarFlujo({
        success: function (result) {
            if (result.Result == null && result.Result == '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                Refrescar();

                var msg = '';
                listaIDs = [];
                lista = JSON.parse(result.Result);

                lista.map(x => x.Estado).forEach(i => {
                    if (listaIDs == "") {
                        listaIDs.push(i);
                    }
                    else if (!listaIDs.includes(i)) {
                        listaIDs.push(i);
                    }
                });

                listaIDs.forEach(y => {

                    if (y == 'Resumen') {
                        msg += '<p/><p><span style="font-weight:bold; font-size:large;">' + y + '</span>';
                    }
                    else {
                        msg += '<p/><p><span style="font-weight:bold; font-size:large;">' + jsEstado + ' - ' + y + '</span>';
                    }

                    lista.forEach(p => {
                        if (p.Estado == y) {
                            if (msg != '') {
                                msg += '<br/><br>';
                            }

                            if (p.Correcto) {
                                icono = '<span class="ico-defaultGrid">&nbsp;</span>';
                            }
                            else {
                                icono = '<span class="gen_Inactivo">&nbsp;</span>';
                            }

                            msg += icono + '  ' + p.Mensaje + ' (' + p.Valor + ')';
                        }
                    });

                    msg += '<br/><br>';
                });

                msg = Ext.Msg.show({ title: jsEstado, icon: Ext.MessageBox.INFO, msg: msg, minWidth: 800 });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

//#endregion

// #region DIRECT METHOD ESTADOS GLOBALES

function Grid_RowSelectEstadosGlobales(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoEstadoGlobales = datos;
    }
}

function FormularioValidoEstadosGlobales(valid) {
    if (valid) {
        App.btnAnadirEstadoGlobal.setDisabled(false);
    }
    else {
        App.btnAnadirEstadoGlobal.setDisabled(true);
    }
}

function btnAgregarEstadoGlobal(sender) {

    TreeCore.AgregarEstadoGlobal(
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeCoreEstadosGlobales.reload();
                    recargarComboObject();
                    Refrescar();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            }
        });
}

function EliminarEstadoGlobal(sender, registro, index) {
    TreeCore.EliminarEstadoGlobal(index.data.CoreEstadoGlobalID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeCoreEstadosGlobales.reload();
                    recargarComboObject();
                    Refrescar();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
}

function EstadosGlobales(sender, registro, index) {

    if (sender.record.data.EstadosGlobales != "" && sender.record.data.EstadosGlobales.includes('(')) {

        App.WinEstadosGlobalesDetails.showAt(getActualXY(App.WinEstadosGlobalesDetails, registro.event));
        App.WinEstadosGlobalesDetails.focus();

        showLoadMask(App.GridEstadosGlobalesAsig, function (load) {

            TreeCore.MostrarEstadosGlobalesAsignados(sender.record.data.CoreEstadoID,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                        }
                    }
                });
        });
    }
}

function HideWinEstadosGlobales() {
    App.WinEstadosGlobalesDetails.hide();
}

function columnEstadosGlobales(sender, registro, index) {

    var datos = index.data;

    if (datos.EstadosGlobales.includes('(')) {
        registro.tdCls = "tbNavPath";
    }
}

// #endregion

// #region DIRECT METHOD ESTADOS SIGUIENTES

function Grid_RowSelectEstadosSiguientes(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoEstadoSiguiente = datos;
    }
}

function FormularioValidoEstadoSiguiente(valid) {
    if (valid) {
        App.btnAnadirEstadosSiguientes.setDisabled(false);
    }
    else {
        App.btnAnadirEstadosSiguientes.setDisabled(true);
    }
}

function btnAgregarEstadoSiguiente(sender) {

    TreeCore.AgregarEstadoSiguiente(
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeCoreEstadosSiguientes.reload();
                    App.storeCoreEstadosSiguientesLibres.reload();
                    App.cmbName.setValue();
                    Refrescar();
                    FormularioValidoEstadoSiguiente(false);

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            }
        });
}

function CambiarEstadoSiguiente(sender, registro, index) {

    if (registro == "Eliminar") {
        TreeCore.EliminarEstadoSiguiente(index.data.CoreEstadoSiguienteID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        Refrescar();
                        App.storeCoreEstadosSiguientes.reload();
                        App.storeCoreEstadosSiguientesLibres.reload();

                        App.btnNextEstado.setText(jsGuardado);
                        App.btnNextEstado.removeCls("animation-text");
                        App.btnNextEstado.setIconCls("ico-tic-wh");

                        setTimeout(function () {
                            App.btnNextEstado.addCls("animation-text");
                        }, 250);
                    }
                },
                eventMask: { showMask: true, msg: jsMensajeProcesando }
            });
    }
    else if (registro == "Defecto") {
        TreeCore.AsignarDefectoEstadoSiguiente(index.data.CoreEstadoPosibleID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeCoreEstadosSiguientes.reload();
                        Refrescar();

                        App.btnNextEstado.setText(jsGuardado);
                        App.btnNextEstado.removeCls("animation-text");
                        App.btnNextEstado.setIconCls("ico-tic-wh");

                        setTimeout(function () {
                            App.btnNextEstado.addCls("animation-text");
                        }, 250);
                    }
                },
                eventMask: { showMask: true, msg: jsMensajeProcesando }
            });
    }
}

function EstadosSiguientes(sender, registro, index) {

    if (sender.record.data.EstadosSiguientes != "" && sender.record.data.EstadosSiguientes.includes('(')) {

        App.WinEstadosSiguientesDetails.showAt(getActualXY(App.WinEstadosSiguientesDetails, registro.event));
        App.WinEstadosSiguientesDetails.focus();

        showLoadMask(App.GridEstadosSiguientesAsig, function (load) {

            TreeCore.MostrarEstadosSiguientesAsignados(sender.record.data.CoreEstadoID,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                        }
                    }
                });
        });
    }
}

function HideWinEstadosSiguientes() {
    App.WinEstadosSiguientesDetails.hide();
}

function columnEstadosSiguientes(sender, registro, index) {

    var datos = index.data;

    if (datos.EstadosSiguientes.includes('(')) {
        registro.tdCls = "tbNavPath";
    }
}

// #endregion

// #region DIRECT METHOD TAREAS

function Grid_RowSelectObjetos(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoObjetos = datos;
    }
}

function FormularioValidoTareas() {
    let valInfo = App.cmbInformaciones.selection;
    let valAcc = App.cmbTareasAcciones.selection;
    let valDesc = App.txtDescripcionTarea.getValue();

    if (valInfo !== null && valAcc !== null && valDesc !== null && valDesc !== "") {
        App.btnAnadirObjeto.setDisabled(false);
    }
    else {
        App.btnAnadirObjeto.setDisabled(true);
    }
}

function btnAgregarObjeto(sender) {

    TreeCore.AgregarEstadoTarea(
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeCoreEstadosTareas.reload();

                    Refrescar();
                    App.chkMandat.setValue();
                    App.cmbInformaciones.setValue();
                    App.cmbTareasAcciones.setValue();
                    App.txtDescripcionTarea.setValue();
                    FormularioValidoTareas(false);

                    RecargarInfo();
                    App.storeCoreWorkflowsInformaciones.reload();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            }
        });
}

function EliminarObjeto(sender, registro, index) {
    TreeCore.EliminarEstadoTarea(index.data.CoreEstadoTareaID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Refrescar();
                    RecargarInfo();
                    App.storeCoreWorkflowsInformaciones.reload();
                    App.storeCoreEstadosTareas.reload();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                    //App.storeCoreDocumentosTipos.reload();
                    //App.storeDocumentTiposLibres.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
}
// #endregion

// #region DIRECT METHOD ROLES

var amsifySuggestagsRolesTareasEstados = null;
var amsifySuggestagsRolesEstadosSeguimiento = null;

function Grid_RowSelectRoles(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoRol = datos;
    }
}

function cargarComboRolesTareasEstados() {

    var arrayUsuarios = App.storeRoles.data.items;
    var rolestareasestados = [];

    for (var i = 0; i < arrayUsuarios.length; i++) {
        rolestareasestados[i] = { 'tag': arrayUsuarios[i].data.Codigo, 'value': arrayUsuarios[i].data.RolID };
    }
    if (amsifySuggestagsRolesTareasEstados != null) {
        amsifySuggestagsRolesTareasEstados.destroy();
    }

    amsifySuggestagsRolesTareasEstados = new AmsifySuggestags($('input[name="rolestareasestados"]'));

    amsifySuggestagsRolesTareasEstados._settings({
        suggestions: rolestareasestados,
        whiteList: true,
        afterAdd: function (value) {
            //arrayTareasEstadosAsignados.push(value);
            ajaxAnadirTareasEstadosAsignados(value);
        },
        afterRemove: function (value) {
            //arrayTareasEstadosAsignados.pop(value);
            ajaxEliminarTareasEstadosAsignados(value);
        }
    });

    document.getElementsByName('rolestareasestados')[0].value = '';
    document.getElementsByName('rolestareasestados')[0].placeholder = jsAgregar + '  Rol';
    amsifySuggestagsRolesTareasEstados._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[0].style = 'max-height: 60px; overflow-y: scroll;';

    if (Agregar == false) {
        var tareasEstadosAsignados = [];
        arrayTareasEstadosAsignados = App.storeTareasEstadosAsignados.data.items;
        for (var i = 0; i < arrayTareasEstadosAsignados.length; i++) {
            tareasEstadosAsignados = arrayTareasEstadosAsignados[i].data.RolID;
            amsifySuggestagsRolesTareasEstados.addTag(tareasEstadosAsignados);
        }
    }
}

function ajaxAnadirTareasEstadosAsignados(rolID) {
    TreeCore.AgregarTareasEstadosAsignados(rolID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function ajaxEliminarTareasEstadosAsignados(rolID) {
    TreeCore.EliminarTareasEstadosAsignados(rolID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function cargarComboRolesEstadosSeguimiento() {

    var arrayRoles = App.storeRoles.data.items;
    var rolesestadosseguimientos = [];

    for (var i = 0; i < arrayRoles.length; i++) {
        rolesestadosseguimientos[i] = { 'tag': arrayRoles[i].data.Codigo, 'value': arrayRoles[i].data.RolID };
    }
    if (amsifySuggestagsRolesEstadosSeguimiento != null) {
        amsifySuggestagsRolesEstadosSeguimiento.destroy();
    }

    amsifySuggestagsRolesEstadosSeguimiento = new AmsifySuggestags($('input[name="rolesestadosseguimientos"]'));

    amsifySuggestagsRolesEstadosSeguimiento._settings({
        suggestions: rolesestadosseguimientos,
        whiteList: true,
        afterAdd: function (value) {
            //arrayRolesEstadosSeguimientoAsignados.push(value);
            ajaxAnadirRolesEstadosSeguimiento(value);

        },
        afterRemove: function (value) {
            //arrayRolesEstadosSeguimientoAsignados.pop(value);
            ajaxEliminarRolesEstadosSeguimiento(value);

        }
    });

    document.getElementsByName('rolesestadosseguimientos')[0].value = '';
    document.getElementsByName('rolesestadosseguimientos')[0].placeholder = jsAgregar + '  Rol';
    amsifySuggestagsRolesEstadosSeguimiento._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[1].style = 'max-height: 60px; overflow-y: scroll;';

    if (Agregar == false) {
        var rolesEstadosSeguimiento = [];
        arrayTareasEstadosAsignados = App.storeRolesEstadosSeguimiento.data.items;
        for (var i = 0; i < arrayTareasEstadosAsignados.length; i++) {
            rolesEstadosSeguimiento = arrayTareasEstadosAsignados[i].data.RolID;
            amsifySuggestagsRolesEstadosSeguimiento.addTag(rolesEstadosSeguimiento);
        }
    }
}

function ajaxAnadirRolesEstadosSeguimiento(rolID) {
    TreeCore.AgregarTareasEstadosSeguimiento(rolID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function ajaxEliminarRolesEstadosSeguimiento(rolID) {
    TreeCore.EliminarTareasEstadosSeguimiento(rolID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

// #endregion

// #region DIRECT METHOD NOTIFICATION

function FormularioValidoNotificacion(sender, registro, index) {
    if (sender.value != "") {
        if (sender.value.length < App[sender.id].maxLength) {
            sender.removeCls("ico-exclamacion-10px-red");
            sender.addCls("ico-exclamacion-10px-grey");
            sender.triggerWrap.removeCls("itemForm-novalid");

            App.btnAnadirNotificacion.setDisabled(false);
        }
        else {
            sender.addCls("ico-exclamacion-10px-red");
            sender.removeCls("ico-exclamacion-10px-grey");
            sender.triggerWrap.addCls("itemForm-novalid");

            App.btnAnadirNotificacion.setDisabled(true);
        }
    }
    else {
        sender.addCls("ico-exclamacion-10px-red");
        sender.removeCls("ico-exclamacion-10px-grey");
        sender.triggerWrap.addCls("itemForm-novalid");

        App.btnAnadirNotificacion.setDisabled(true);
    }
}

function btnAgregarNotificacion() {

    TreeCore.AgregarNotificacion(arrayRolesAsignados, arrayUsuariosAsignados,
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.txtMensaje.clear();
                    arrayRolesAsignados = [];
                    arrayUsuariosAsignados = [];
                    cargarComboRoles();
                    cargarComboUsuarios();
                    App.storeCoreEstadosNotificaciones.reload();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            }
        });
}

function EliminarNotificacion(lNotificacionID) {

    TreeCore.EliminarNotificacion(lNotificacionID,
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.txtMensaje.clear();
                    arrayRolesAsignados = [];
                    arrayUsuariosAsignados = [];
                    cargarComboRoles();
                    cargarComboUsuarios();
                    App.storeCoreEstadosNotificaciones.reload();

                    App.btnNextEstado.setText(jsGuardado);
                    App.btnNextEstado.removeCls("animation-text");
                    App.btnNextEstado.setIconCls("ico-tic-wh");

                    setTimeout(function () {
                        App.btnNextEstado.addCls("animation-text");
                    }, 250);
                }
            }
        });
}

var amsifySuggestagsUsuarios = null;
var amsifySuggestagsRoles = null;

function cargarComboUsuarios() {

    var arrayUsuarios = App.storeUsuarios.data.items;
    var usuarios = [];

    for (var i = 0; i < arrayUsuarios.length; i++) {
        usuarios[i] = { 'tag': arrayUsuarios[i].data.EMail, 'value': arrayUsuarios[i].data.UsuarioID };
    }
    if (amsifySuggestagsUsuarios != null) {
        amsifySuggestagsUsuarios.destroy();
    }

    amsifySuggestagsUsuarios = new AmsifySuggestags($('input[name="usuarios"]'));

    amsifySuggestagsUsuarios._settings({
        suggestions: usuarios,
        whiteList: true,
        afterAdd: function (value) {
            arrayUsuariosAsignados.push(value);
        },
        afterRemove: function (value) {
            arrayUsuariosAsignados.pop(value);
        }
    });

    document.getElementsByName('usuarios')[0].value = '';
    document.getElementsByName('usuarios')[0].placeholder = jsAgregar + ' ' + jsUsuario;
    amsifySuggestagsUsuarios._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[0].style = 'max-height: 60px; overflow-y: scroll;';
}

function cargarComboRoles() {

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
        },
        afterRemove: function (value) {
            arrayRolesAsignados.pop(value);
        }
    });

    document.getElementsByName('roles')[0].value = '';
    document.getElementsByName('roles')[0].placeholder = jsAgregar + '  Rol';
    amsifySuggestagsRoles._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[1].style = 'max-height: 60px; overflow-y: scroll;';
}

// #endregion

//#region DISEÑO

// #region CONTROLES BASICOS PAGINA (ASIDE ETC)

function hideAsideR(panel) {

    App.btnCollapseAsRClosed.show();

    //var asideR = Ext.getCmp('pnAsideR');
    //let btn = document.getElementById('btnCollapseAsRClosed');

    //if (asideR.collapsed == false) {

    //    btn.style.transform = 'rotate(-180deg)';
    //    App.pnAsideR.collapse();
    //}
    //else {
    //    btn.style.transform = 'rotate(0deg)';
    //    App.pnAsideR.expand();

    //}

    if (panel != null) {

        App.WrapEstados.hide();

        switch (panel) {

            case "WrapEstados":

                App.WrapEstados.show();
                App.pnMoreInfo.show()

                btn.style.transform = 'rotate(0deg)';
                //App.pnAsideR.expand();

                break;

        }

    }
    GridColHandler();

    window.dispatchEvent(new Event('resize'));

}

function GridColHandler(grid) {
    // Con esta variable se controla si la columna more esta visible siempre o no
    var ForceShowColmore = false;

    //Variables de entorno(no editar)
    if (grid != null) {
        var gridW = grid.getWidth();

        const colArray = grid.columns;
        const colArrayNoColMore = grid.columns.slice(0);

        var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
        var AllcolsMinWTotal = 0;
        var visiblecolsMinWTotal = 0;

        // Se crea un array sin la columna More    Se ha cambiado a seleccion por el CLS de la columna que tiene que ser col-More

        if (LastCol.cls == "col-More") {
            colArrayNoColMore.pop();
        }

        // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

        colArrayNoColMore.forEach(function (colArrayNoColMore) {
            if (colArrayNoColMore.hidden != true) {
                visiblecolsMinWTotal = visiblecolsMinWTotal + colArrayNoColMore.minWidth;
            }

            AllcolsMinWTotal = AllcolsMinWTotal + colArrayNoColMore.minWidth;

        });

        //Controles de anchura y hide (aqui esta el tema)

        for (let i = 0; i < 18 && visiblecolsMinWTotal <= gridW + 90; i++) {
            var HiddenCols = colArrayNoColMore.filter(x => {
                return x.hidden == true;
            })

            if (HiddenCols.length > 0) {
                var FirstHiddenColIndex = HiddenCols[0].fullColumnIndex;
                grid.columns[FirstHiddenColIndex].show();

                //// Se Suma la anchuraminima del computo de las columnas visibles que Hay

                var minWLastShownCol = HiddenCols[0].minWidth;
                visiblecolsMinWTotal = visiblecolsMinWTotal + minWLastShownCol;
            }

        }

        while (visiblecolsMinWTotal >= gridW - 70) {

            var VisibleCols = colArrayNoColMore.filter(x => {
                return x.hidden != true;
            })

            if (VisibleCols.length > 0) {

                //Se oculta Ultima Columna de las VISIBLES
                var LastVisibleColIndex = VisibleCols.length - 1;
                grid.columns[LastVisibleColIndex].hide();

                // Se resta la anchuraminima del computo de las columnas visibles que quedan
                var minWLastCol = VisibleCols[VisibleCols.length - 1].minWidth
                visiblecolsMinWTotal = visiblecolsMinWTotal - minWLastCol;

            } else {
                break
            }

        }

        // #region AQUI SE ESCONDE LA COLUMNA MORE (debe ser la ultima) POR DEFECTO!
        //Index colmore
        var colMoreIndex = grid.columns.length - 1;

        if (AllcolsMinWTotal < gridW - 70) {
            grid.columns[colMoreIndex].hide();
        } else if (visiblecolsMinWTotal <= gridW + 90) {
            grid.columns[colMoreIndex].show();
        }

        if (ForceShowColmore == true) {
            grid.columns[colMoreIndex].show();
        }

        //#endregion

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
        obj.setWidth(750);
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
        obj.setWidth(750);
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
        obj.setWidth(750);
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
    //var asideR = Ext.getCmp('pnAsideR');

    //if (winsize < 520 && asideR.collapsed == false) {
    //    App.CenterPanelMain.hide();
    //    App.pnAsideR.setWidth(winsize);
    //}
    //else {
    //    App.CenterPanelMain.show();
    //    App.pnAsideR.setWidth(380);
    //}

});

// #endregion

function cambiarLiteral() {
    App.btnNextEstado.setText(jsGuardar);
    App.btnNextEstado.setIconCls("");
    App.btnNextEstado.removeCls("btnDisableClick");
    App.btnNextEstado.addCls("btnEnableClick");
    App.btnNextEstado.removeCls("animation-text");
}

function cargarPanelMoreInfo(panel, label) {
    parent.CargarPanelMoreInfo('WrapEstados', 'lblEstados', App);
}

function showwinAddTab() {
    App.winAddTabFilter.show();
}

function showwinSaveQF() {
    App.winSaveQF.show();
}

function showFormsEstados(sender, idComponente, index) {
    var panelActual = getPanelActualEstado(sender, idComponente, index);
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }

    //if (panelActual == 0) {
    //    ajaxAgregarEditar(sender, index);
    //}
    //else {
    cambiarATapEstado(sender, index);
    //}
}

function btnPrevEstado(sender, registro, index) {
    var panelActual = getPanelActualEstado(sender, registro, index);
    cambiarATapEstado(sender, --panelActual);
}

function btnNextEstado(sender, registro, index) {
    var panelActual = getPanelActualEstado(sender, registro, index);

    if (panelActual == 0) {
        ajaxAgregarEditar(sender, ++panelActual);
    }
    else if (panelActual == 7) {
        App.winGestionEstados.hide();
    }
    else {
        cambiarATapEstado(sender, ++panelActual);
    }
}

function getPanelActualEstado(sender, registro, index) {
    var panelActual;
    var panels = App.containerFormEstados.ariaEl.dom.getElementsByClassName("winGestion-panel");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
        }
    }
    return panelActual;
}

function cambiarATapEstado(sender, index) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";

    var idForm = sender;

    var arrayBotones = Ext.getCmp("cntNavVistasFormEstados").ariaEl.getFirstChild().getFirstChild().dom.children;


    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = App.containerFormEstados.ariaEl.dom.getElementsByClassName("winGestion-panel");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();
        }

        if (index != 0) {
            App.storeCoreObjetosNegocioTipos.reload();
            App.gridFiltersObjetos.clearFilters();

            App.storeCoreEstadosSiguientes.reload();
            App.gridFiltersEstadosSiguientes.clearFilters();

            App.storeCoreEstadosRoles.reload();

            App.storeCoreEstadosGlobales.reload();
            App.gridFiltersEstadosGlobales.clearFilters();

            App.storeEstadosGlobales.reload();
            App.storeEstadosAgrupaciones.reload();
            App.storeInventarioElementosAtributosEstados.reload();
            App.storeCoreEstadosTareas.reload();
            App.storeDocumentosEstados.reload();
            App.storeRoles.reload();
            App.storeUsuarios.reload();
            App.storeCoreEstadosNotificaciones.reload();

            cargarComboUsuarios();
            cargarComboRoles();

            App.storeTareasEstadosAsignados.reload();
            cargarComboRolesTareasEstados();
            cargarComboRolesEstadosSeguimiento();
            App.btnNextEstado.setText(jsGuardado);
            App.btnNextEstado.addCls("btnDisableClick");
            App.btnNextEstado.removeCls("btnEnableClick");
            App.btnNextEstado.setIconCls("ico-tic-wh");
        }

        Ext.getCmp(panels[index].id).show();
    }

    if (index == 0) {

        if (App.txtNombre.getValue() != "") {
            App.btnNextEstado.setText(jsGuardado);
            App.btnNextEstado.addCls("btnDisableClick");
            App.btnNextEstado.removeCls("btnEnableClick");
            App.btnNextEstado.setIconCls("ico-tic-wh");
        }
        else {
            App.btnNextEstado.setText(jsGuardar);
            App.btnNextEstado.setIconCls("");
            App.btnNextEstado.removeCls("btnDisableClick");
            App.btnNextEstado.addCls("btnEnableClick");
            App.btnNextEstado.removeCls("animation-text");
        }
        //FormularioValido('', true);
    }

    App.btnNextEstado.addCls(classBtnActivo);
    App.btnNextEstado.removeCls(classBtnDesactivo);

    ////botones prev y next
    //if (index <= 0) {
    //    App.btnPrevEstado.addCls(classBtnDesactivo);
    //    App.btnPrevEstado.removeCls(classBtnActivo);
    //    App.btnNextEstado.setText(jsGuardar);
    //    App.btnNextEstado.removeCls(classBtnDesactivo);
    //    App.btnNextEstado.addCls(classBtnActivo);
    //}
    //else if (index >= arrayBotones.length - 1) {
    //    App.btnNextEstado.setText(jsGuardar);
    //    App.btnNextEstado.setDisabled(false);
    //    App.btnPrevEstado.removeCls(classBtnDesactivo);
    //    App.btnPrevEstado.addCls(classBtnActivo);
    //    App.btnPrevEstado.setDisabled(false);
    //}
    //else {
    //    App.btnPrevEstado.addCls(classBtnActivo);
    //    App.btnNextEstado.addCls(classBtnActivo);
    //    App.btnNextEstado.setText(jsSiguiente);
    //    App.btnNextEstado.setDisabled(false);
    //    App.btnNextEstado.removeCls(classBtnDesactivo);
    //    App.btnPrevEstado.removeCls(classBtnDesactivo);
    //    App.btnPrevEstado.setDisabled(false);
    //}

}

//#endregion

//#region COMBOS

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

var EscrituraRender = function (sender, registro, index) {
    var datos = index.data;

    if (datos.PublicoEscritura) {
        return '<span class="ico-publico">&nbsp;</span>';
    }
    else {
        return '<span class="ico-privado">&nbsp;</span>';
    }
}

var LecturaRender = function (sender, registro, index) {
    var datos = index.data;

    if (datos.PublicoLectura) {
        return '<span class="ico-publico">&nbsp;</span>';
    }
    else {
        return '<span class="ico-privado">&nbsp;</span>';
    }
}

var CompletadoRender = function (sender, registro, index) {
    var datos = index.data;

    if (datos.Completado) {
        return '<span class="ico-defaultGrid">&nbsp;</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var DocumentosRender = function (value) {
    if (value == "1" || value == "true") {
        return '<span class="ico-docsGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
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

function abrirPanel(sender, registro, index) {
    var seleccionado = sender.$widgetRecord;
    parent.cargarDatosPanelMoreInfoGridWF(seleccionado, App.gridMain1);
    hideAsideR('panelMore');
}

var pageSelect = function (item, records) {
    var curPageSize = App.storeCoreEstados.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeCoreEstados.pageSize = wantedPageSize;
        App.storeCoreEstados.load();
    }
}

function cargarCombo(sender, registro, index) {
    App[sender.config.storeId].data.items.forEach(p => p.data.Completo = p.data.NombreEstado + ', ' + p.data.CodigoEstado);
}

function RecargarWorkflows() {
    showLoadMask(App.gridMain1, function (load) {
        if (load) {
            CargarStoresSerie([App.storeCoreEstados], function Fin(fin) {
                if (fin) {
                    load.hide();
                }
            });
        }
    });
}

function RecargarWorkflow() {
    recargarCombos([App.cmbWorkflows]);
    App.storeCoreEstados.reload();
}

function SeleccionarWorkflow() {
    App.hdWorkflows.setValue(App.cmbWorkflows.getValue());
    App.cmbWorkflows.getTrigger(0).show();
    App.storeCoreEstados.reload();
}

function RecargarWorkflo() {
    recargarCombos([App.cmbWorkflows]);
}

function SeleccionarWorkflo() {
    App.cmbWorkflows.getTrigger(0).show();
}

function SeleccionarEstadoSiguiente() {
    App.cmbName.getTrigger(0).show();
    FormularioValidoEstadoSiguiente(true);
}

function RecargarEstadoSiguiente() {
    recargarCombos([App.cmbName]);
    FormularioValidoEstadoSiguiente(false);
}

function seleccionarComboObject() {
    App.cmbEstadosGlobales.clearValue();
    App.cmbObject.getTrigger(0).show();
    App.storeEstadosGlobales.reload();
    App.cmbEstadosGlobales.getTrigger(0).hide();
    App.cmbEstadosGlobales.setDisabled(false);
}

function recargarComboObject() {
    App.cmbObject.clearValue();
    App.cmbEstadosGlobales.clearValue();
    App.storeEstadosGlobales.reload();
    App.cmbObject.getTrigger(0).hide();
    App.cmbEstadosGlobales.getTrigger(0).hide();
    App.cmbEstadosGlobales.setDisabled(true);
    FormularioValidoEstadosGlobales(false);
}

function SeleccionarDepartamento() {
    App.cmbDepartamento.getTrigger(0).show();
}

function RecargarDepartamento() {
    recargarCombos([App.cmbDepartamento]);
}

function SeleccionarAgrupacion() {
    App.cmbGrupos.getTrigger(0).show();
}

function RecargarAgrupacion() {
    recargarCombos([App.cmbGrupos]);
}

function SeleccionarEstadoGlobal() {
    App.cmbEstadosGlobales.getTrigger(0).show();
}

function RecargarEstadoGlobal() {
    recargarCombos([App.cmbEstadosGlobales]);
    FormularioValidoEstadosGlobales(false);
}

//function SeleccionarSubproceso() {
//    App.cmbSubprocesos.getTrigger(0).show();
//}

//function RecargarSubproceso() {
//    recargarCombos([App.cmbSubprocesos]);
//}

function SeleccionarInfo() {
    App.cmbInformaciones.getTrigger(0).show();
    FormularioValidoTareas();

    App.storeCoreTiposInformacionesAcciones.reload();
    App.cmbTareasAcciones.clearValue();
    App.cmbTareasAcciones.getTrigger(0).hide();
    App.cmbTareasAcciones.setDisabled(false);
}

function RecargarInfo() {
    App.cmbInformaciones.clearValue();
    App.cmbTareasAcciones.clearValue();
    App.storeCoreTiposInformacionesAcciones.reload();
    App.cmbInformaciones.getTrigger(0).hide();
    App.cmbTareasAcciones.getTrigger(0).hide();
    App.cmbTareasAcciones.setDisabled(true);
    FormularioValidoTareas();
}

function seleccionarAccion(sender, registro, index) {
    App.cmbTareasAcciones.getTrigger(0).show();
    FormularioValidoTareas();
}

function RecargarAcciones(sender, registro, index) {
    App.cmbTareasAcciones.clearValue();
    App.storeCoreTiposInformacionesAcciones.reload();
    FormularioValidoTareas();
}

//function SeleccionarRol() {
//    App.cmbRoles.getTrigger(0).show();
//}

//function RecargarRol() {
//    recargarCombos([App.cmbRoles]);
//}

function tglRestWorkFlowEscritura(sender) {
    var valor = "";
    var contRolesEscritura = document.getElementById('pnRolestagsEscritura');

    if (sender.pressed) {
        App.labelPrivado.enable();
        App.labelPublico.disable();
        contRolesEscritura.classList.remove('tagsPublicos');
        contRolesEscritura.classList.add('tagsPrivados');
        valor = "Privado";
    }
    else {
        App.labelPrivado.disable();
        App.labelPublico.enable();
        contRolesEscritura.classList.add('tagsPublicos');
        contRolesEscritura.classList.remove('tagsPrivados');
        valor = "Publico";
    }

    //TreeCore.CambiarRolPublicoPrivadoEscritura(valor, {
    //    success: function (result) {
    //        if (result.Result != null && result.Result != '') {
    //            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //        }
    //        else {
    //            forzarCargaBuscadorPredictivo = true;
    //            Refrescar();
    //        }
    //    },
    //    eventMask: { showMask: true, msg: jsMensajeProcesando }
    //});
}

function tglRestWorkFlow(sender) {
    var valor = "";
    var contRoles = document.getElementById('pnRolestags');
    if (sender.pressed) {
        App.lbPrivate.enable();
        App.lbPublic.disable();
        contRoles.classList.remove('tagsPublicos');
        contRoles.classList.add('tagsPrivados');
        valor = "Privado";
    }
    else {
        App.lbPrivate.disable();
        App.lbPublic.enable();
        contRoles.classList.add('tagsPublicos');
        contRoles.classList.remove('tagsPrivados');
        valor = "Publico";
    }

    //TreeCore.CambiarRolPublicoPrivadoLectura(valor, {
    //    success: function (result) {
    //        if (result.Result != null && result.Result != '') {
    //            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //        }
    //        else {
    //            forzarCargaBuscadorPredictivo = true;
    //            Refrescar();
    //        }
    //    },
    //    eventMask: { showMask: true, msg: jsMensajeProcesando }
    //});
}

//#endregion
