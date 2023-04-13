// #region GESTION GRID
var oOperador = null;
var oProveedor = null;
var oEmpresaProveedora = null;
function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        let registroSeleccionado = registro;
        let GridSeleccionado = App.grid;
        cargarDatosPanelMoreInfoEntidad(registroSeleccionado, GridSeleccionado);

        App.btnAnadir.enable();
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnGestionContactos.enable();
        App.btnEntidadCliente.enable();

        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEditar.setTooltip(jsEditar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnRefrescar.setTooltip(jsRefrescar);
        App.btnDescargar.setTooltip(jsDescargar);
        App.btnEntidadCliente.setTooltip(jsCliente);
        App.btnGestionContactos.setTooltip(jsContactos);
        App.btnAsignarImagen.setTooltip(jsAsignarImagen);

        App.hdEntidadID.setValue(seleccionado.EntidadID);

        if (seleccionado.EsOperador) {
            App.btnAsignarImagen.enable();
        } else {
            App.btnAsignarImagen.disable();
        }
    }
}

function DeseleccionarGrilla() {
    App.hdEntidadID.setValue("");
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnGestionContactos.disable();
    App.btnAsignarImagen.disable();
    App.btnEntidadCliente.disable();
    App.GridRowSelect.clearSelections();
}

function Refrescar() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }

    BorrarFiltros(App.grid);
}

function RecargarGrilla(sender, registro, index) {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
}

var numMaximo = function (value) {
    if (value != 0) {
        return value;
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var ownerRender = function (value) {
    if (value) {
        return '<span class="ico-owner-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var supplierRender = function (value) {
    if (value) {
        return '<span class="ico-supplier-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var companyRender = function (value) {
    if (value) {
        return '<span class="ico-company-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var operatorRender = function (value) {
    if (value) {
        return '<span class="ico-operator-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

function ClickShowContactsD(sender, registro, index) {
    let entidadID = this.getWidgetRecord().id;
    App.hdEntidadID.setValue(entidadID);
    var el = sender.id;
    var pos = document.getElementById(el).getBoundingClientRect();
    App.storeContactosGlobalesEntidad.reload();
    App.WinContactsDetails.show();
    App.WinContactsDetails.setX(pos.right - 480);
    App.WinContactsDetails.setY(pos.top);
}

function ClickShowModulos(sender, registro, index) {
    let entidadID = this.getWidgetRecord().id;
    App.hdEntidadID.setValue(entidadID);
    var el = sender.id;
    var pos = document.getElementById(el).getBoundingClientRect();
    App.storeModulosEmpresaProveedora.reload();
    App.WinModulosEmpresaProveedoras.show();
    App.WinModulosEmpresaProveedoras.setX(pos.right - 480);
    App.WinModulosEmpresaProveedoras.setY(pos.top);

}

// #endregion

// #region AGREGAR/EDITAR/ELIMINAR

function VaciarFormulario(ruta, registro, index) {
    LimpiarFormularioVentana(App.WinGestion);
    App.formGestionContactos.getForm().reset();
    App.formCompaniaProveedora.getForm().reset();
    App.FormGestionOperator.getForm().reset();
    App.btnOperador.setPressed(false);
    App.btnPropietario.setPressed(false);
    App.btnCompania.setPressed(false);
    App.btnProveedor.setPressed(false);

    Ext.each(App.formInicial.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);
                //c.addListener("select", FormularioValidoEntidades, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }

            if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
        }
    });

    Ext.each(App.formGeo.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('validitychange', FormularioValidoEntidades, false);
        }
    });
}

function VaciarFormularioOperador() {
    App.FormGestionOperator.getForm().reset();

}

function VaciarFormularioEmpresaProveedora() {
    App.formCompaniaProveedora.getForm().reset();

}

function VaciarFormularioProveedor() {
    App.FormProveedor.getForm().reset();

}

function FormularioValidoEntidadesV1(sender, valid) {
    if (valid == true) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function FormularioValidoIntermedio(sender) {
    FormularioValidoEntidades(sender, sender.isValid());
}

function FormularioValidoEntidades(sender, valid) {
    if (valid == true) {
        App.btnGuardar.setDisabled(false);

        Ext.each(App.formInicial.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function FormularioValidoOperador(valid) {
    App.btnGuardarOperador.setDisabled(false);
    Ext.each(App['FormGestionOperator'].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
            App['btnGuardarOperador'].setDisabled(true);
        }
    });
}

function FormularioValidoProveedor(sender, valid) {
    App.btnGuardarProveedor.setDisabled(false);
}

function FormularioValidoCompaniaProveedora(valid) {
    App.btnGuardarCompaniaProveedora.setDisabled(false);
    Ext.each(App['formCompaniaProveedora'].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
            App['btnGuardarCompaniaProveedora'].setDisabled(true);
        }
    });
}

function AgregarEditar() {

    VaciarFormulario();
    oOperador = null;
    oProveedor = null;
    oEmpresaProveedora = null;
    var combos = [App.cmbTipoEntidad];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.btnEditarCompania.setDisabled(true)
                App.btnEditarProveedor.setDisabled(true)
                App.btnEditarOperador.setDisabled(true)
                App.WinGestion.setTitle(jsAgregar + ' ' + jsTituloEntidades);
                App.btnGuardar.setDisabled(true);
                App.hdControlFormulario.setValue('agregar')
                App.WinGestion.show();
                load.hide()
            }

        });
    })

}

function winGestionBotonGuardar() {
    if (App.formGestionEntidad.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (App.WinGestion.title.startsWith(jsAgregar)) {
        Agregar = true;
    }

    TreeCore.AgregarEditar(Agregar, oOperador, oProveedor, oEmpresaProveedora,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.WinGestion.hide();
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

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {

    VaciarFormulario();
    var combos = [App.cmbTipoEntidad];

    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.WinGestion.setTitle(jsEditar + ' ' + jsTituloEntidades);
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.hdControlFormulario.setValue('editar');
                            showLoadMask(App.WinGestion, function (load2) {
                                load2.hide();
                            });

                            App.WinGestion.show();
                            load.hide()
                        }
                    }
                );
            }

        });
    })

}

function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloEntidades,
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
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #region Contactos

function GestionarContacto() {
    //App.txtBuscarMail.focus(false, 200);
    App.storeContactosGlobalesEntidadesVinculadas.reload();
    App.WinGestionContactos.show();
}

function buscador() {
    App.storeContactosGlobalesEntidadesVinculadas.reload();
}

function limpiar(value) {
    value.setValue("");
    App.storeContactosGlobalesEntidadesVinculadas.reload();
}

// #endregion

// #region Operador

function toogleOperador() {

    FormularioValidoOperador();
    if (App.btnOperador.pressed == true) {
        App.btnEditarOperador.enable();
        App.winGestionOperador.show();
    } else {
        App.btnEditarOperador.disable();
        App.hdControlOperador.setValue("false");
        VaciarFormularioOperador();
        if (App.hdControlFormulario.value == 'editar') {
            TreeCore.EliminarOperador(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            App.btnEditarOperador.enable();
                            App.btnOperador.setPressed(true);
                        }
                        else {
                            App.btnEditarOperador.disable();
                            App.hdControlOperador.setValue("false");
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
}

function GestionarComoOperador() {
    TreeCore.MostrarEditarOperador(oOperador,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.winGestionOperador.show();
            }
        }
    );
}

function cerrarWinOperador() {
    if (App.hdControlOperador.value == "false" && App.hdControlFormulario.value != "editar") {
        App.btnOperador.setPressed(false)
        App.btnEditarOperador.disable();
        VaciarFormularioOperador();
    } else if (App.hdControlOperador.value == "salvar") {
        TreeCore.MostrarEditarOperador(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                    }
                }
            }
        );
        App.hdControlOperador.setValue("false");
    }
}

function winGestionOperadorGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var Friendly = App.chkFriendly.checked;
        var Torrero = App.chkTorre.checked;
        var EsCliente = App.chkCliente.checked;
        oOperador = [Friendly, Torrero, EsCliente];
    }
    App.hdControlOperador.setValue("true");
    TreeCore.AgregarEditarOperador(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.winGestionOperador.hide();
            }
        }
    );
}

// #endregion

// #region Empresa Proveedora

function tooglebEditarCompania() {
    FormularioValidoCompaniaProveedora();
    if (App.btnCompania.pressed == true) {
        App.btnEditarCompania.enable();
        GestionarComoCompania();
    }
    else {
        App.btnEditarCompania.disable()
        VaciarFormularioEmpresaProveedora();
        if (App.hdControlFormulario.value == 'editar') {
            TreeCore.EliminarEmpresaProveedora(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            App.btnEditarCompania.enable();
                            App.btnCompania.setPressed(true);
                        }
                        else {
                            App.btnEditarCompania.disable();
                            App.hdControlEmpresaProveedora.setValue('false');
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
}

function GestionarComoCompania() {

    var combos = [App.cmbModulos];

    showLoadMask(App.WinGestion, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {

                App.btnEditarCompania.enable();
                App.winGestionCompaniaProveedora.show();
                load.hide();
            }
        });
    })

}

function GestionarComoEmpresaProveedoraMostrar() {
    var combos = [App.cmbModulos];

    if (App.hdControlEmpresaProveedora.value == "false") {
        showLoadMask(App.WinGestion, function (load) {
            recargarCombos(combos, function (fin) {
                if (fin) {
                    TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                }

                                App.btnEditarCompania.enable();
                                App.winGestionCompaniaProveedora.show();
                                load.hide()
                            }
                        }
                    );


                }
            });
        })
    } else {
        App.winGestionCompaniaProveedora.show();
    }


}

function winGestionCompaniaProveedoraGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var Modulos = App.cmbModulos.value;
        oEmpresaProveedora = Modulos;
    }
    App.hdControlEmpresaProveedora.setValue("true")
    TreeCore.AgregarEditarEmpresaProveedora(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.hdControlEmpresaProveedora.setValue("false");
                App.winGestionCompaniaProveedora.hide();
            }
        }
    );

}

function cerrarWinEmpresaProveedora() {
    if (hdControlFormulario.value == "agregar") {
        if (oEmpresaProveedora == null) {
            App.btnCompania.setPressed(false);
            App.btnEditarCompania.disable();
            VaciarFormularioEmpresaProveedora();
        } else {
            TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );
            App.hdControlEmpresaProveedora.setValue("false");
        }
    } else {
        if (App.hdControlEmpresaProveedora.value == "false") {
            TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );

        }

    }

}

// #endregion

// #region Proveedor

function GestionarComoProveedorMostrar() {
    var combos = [App.cmbMetodoPago,
    App.cmbTipoContribuyente,
    App.cmbIdentificacion,
    App.cmbTratamiento,
    App.cmbGrupoCuenta,
    App.cmbCuenta,
    App.cmbClaveClasificacion,
    App.cmbTesoreria,
    App.cmbCondicionPago];

    if (App.hdControlProveedor.value == "false") {
        showLoadMask(App.WinGestion, function (load) {
            recargarCombos(combos, function (fin) {
                if (fin) {
                    TreeCore.MostrarEditarProveedor(oProveedor,
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                }
                                if (App.cmbMetodoPago.value != undefined) {
                                    App.cmbMetodoPago.getTrigger(0).show();
                                }
                                if (App.cmbTipoContribuyente.value != undefined) {
                                    App.cmbTipoContribuyente.getTrigger(0).show();
                                }
                                if (App.cmbIdentificacion.value != undefined) {
                                    App.cmbIdentificacion.getTrigger(0).show();
                                }
                                if (App.cmbTratamiento.value != undefined) {
                                    App.cmbTratamiento.getTrigger(0).show();
                                }
                                if (App.cmbGrupoCuenta.value != undefined) {
                                    App.cmbGrupoCuenta.getTrigger(0).show();
                                }
                                if (App.cmbCuenta.value != undefined) {
                                    App.cmbCuenta.getTrigger(0).show();
                                }
                                if (App.cmbClaveClasificacion.value != undefined) {
                                    App.cmbClaveClasificacion.getTrigger(0).show();
                                }
                                if (App.cmbTesoreria.value != undefined) {
                                    App.cmbTesoreria.getTrigger(0).show();
                                }
                                if (App.cmbCondicionPago.value != undefined) {
                                    App.cmbCondicionPago.getTrigger(0).show();
                                }
                                App.btnEditarProveedor.enable();
                                App.winGestionProveedor.show();
                                load.hide()
                            }
                        }
                    );


                }
            });
        })
    } else {
        App.winGestionProveedor.show();
    }


}

function toogleProveedor() {
    FormularioValidoProveedor();
    if (App.btnProveedor.pressed == true) {
        GestionarComoProveedor();
    }
    else {
        VaciarFormularioProveedor();
        App.btnEditarProveedor.setDisabled(true);
        if (App.hdControlFormulario.value == 'editar') {
            TreeCore.EliminarProveedor(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            App.btnEditarProveedor.enable();
                            App.btnProveedor.setPressed(true);
                        }
                        else {
                            App.btnEditarProveedor.disable();
                            App.hdControlProveedor.setValue('false');
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                }
            );
        }
    }
}

function GestionarComoProveedor() {

    var combos = [App.cmbMetodoPago,
    App.cmbTipoContribuyente,
    App.cmbIdentificacion,
    App.cmbTratamiento,
    App.cmbGrupoCuenta,
    App.cmbCuenta,
    App.cmbClaveClasificacion,
    App.cmbTesoreria,
    App.cmbCondicionPago];

    showLoadMask(App.WinGestion, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.btnEditarProveedor.enable();
                App.winGestionProveedor.show();
                load.hide()
            }
        });
    })
}

function winGestionProveedorGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var MetodoPago = App.cmbMetodoPago.value;
        var TipoContribuyente = App.cmbTipoContribuyente.value;
        var Identificacion = App.cmbIdentificacion.value;
        var Tratamiento = App.cmbTratamiento.value;
        var GrupoCuenta = App.cmbGrupoCuenta.value;
        var Cuenta = App.cmbCuenta.value;
        var ClaveClasificacion = App.cmbClaveClasificacion.value;
        var Tesoreria = App.cmbTesoreria.value;
        var CondicionPago = App.cmbCondicionPago.value;
        oProveedor = [MetodoPago, TipoContribuyente, Identificacion, Tratamiento, GrupoCuenta, Cuenta, ClaveClasificacion, Tesoreria, CondicionPago];
    }
    App.hdControlProveedor.setValue("true");
    TreeCore.AgregarEditarProveedor(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.hdControlProveedor.setValue("false");
                App.winGestionProveedor.hide();
            }
        }
    );

}

function cerrarWinProveedor() {
    if (hdControlFormulario.value == "agregar") {
        if (oProveedor == null) {
            App.btnProveedor.setPressed(false);
            App.btnEditarProveedor.disable();
        } else {
            TreeCore.MostrarEditarProveedor(oProveedor,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );
            App.hdControlProveedor.setValue("false");
        }
    } else {
        if (App.hdControlProveedor.value == "false") {
            TreeCore.MostrarEditarProveedor(oProveedor,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );

        }

    }

}


// #endregion

// #endregion


// #region Propietario
function togglePropietario() {
    if (App.btnPropietario.pressed == false) {
        if (App.hdControlFormulario.value == 'editar') {
            TreeCore.EliminarPropietario(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            btnPropietario.setPressed(true);
                        }
                        else {
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                });
        }
    } else {
        if (App.hdControlFormulario.value != 'agregar') {
            TreeCore.AñadirPropietario(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
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
}
// #endregion

function GenerarPlantillaEntidades() {
    TreeCore.GenerarPlantillaEntidades(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function NavegacionTabs(who, PaginaACargar, NombrePagina, script) {
    var LNo = who.textEl;
    var test = PaginaACargar;

    switch (who.id) {
        case 'lnkEntities':
            App.grid.show();
            App.hugeCt.hide();
            document.getElementById("lnkContacts").lastChild.classList.remove("navActivo")
            document.getElementById("lnkEntities").lastChild.classList.add("navActivo")
            break;

        case 'lnkContacts':

            App.grid.hide();
            App.hugeCt.show();
            document.getElementById("lnkEntities").lastChild.classList.remove("navActivo")
            document.getElementById("lnkContacts").lastChild.classList.add("navActivo");
            App.gridContactos_grdContactos.store.reload();
            break;

    }
}

// #region GESTIÓN CONTACTOS

function cambiarAsignacion(sender, registro, index) {

    var idComponente = sender.record.store.config.storeId.split('_')[0];

    TreeCore.AsignarEntidad(sender.record.get('ContactoGlobalID'),
        sender.record.get('EntidadID'),
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeContactosGlobalesEntidadesVinculadas.reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

var SetToggleValue = function (column, cmp, record) {

    var lEntSelecID = App.GridRowSelect.selected.items[0].data.EntidadID;

    var lEntID = record.get('EntidadID');

    if (lEntSelecID == lEntID) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();

}

function Grid_RowSelectContactos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    let registroSeleccionado = registro;
    let GridSeleccionado = App[ruta + "_grdContactos"];
    cargarDatosPanelMoreInfoGrid(registroSeleccionado, GridSeleccionado);
}

function ResizerAside(pn) {
    var elmnt = document.getElementById("vwResp-innerCt");

    if (elmnt != null) {
        var HeightVisorPadre = elmnt.offsetHeight;
        if (App != null) {
            App.pnAsideR.setHeight(HeightVisorPadre + 60);
        }
    }

}

//function agregarContactoEntidad(sender, registro, index) {
//    var idComponente = sender.id.split('_')[0];
//    var ControlURL = '/Componentes/FormContactos.ascx';
//    var NombreControl = 'FormContactos';

//    var achjs = ControlURL.split('/');
//    achjs.pop(); achjs = achjs.join('/') + '/js/' + NombreControl + '.js';
//    AñadirScriptjs(achjs);
//    App.hdControlURL.value = ControlURL;
//    App.hdControlName.value = NombreControl;

//    CargarVentanaContactos(sender, 'formAgregarEditarContacto');
//    App.winAgregarContacto.setTitle(jsAgregar);
//    App.winAgregarContacto.show();

//}
function agregarContactoEntidad(sender, registro, index) {
    var entID = App["GridRowSelect"].selected.items[0].data.EntidadID;
    App['hdEntidadID'].setValue(entID);
    App["winAgregarContacto"].setTitle(jsAgregar + " " + jsContacto);
    App["winAgregarContacto"].show();

    VaciarFormularioContacto('formAgregarEditarContacto');
    cambiarATapContacto('formAgregarEditarContacto', 0);
    CargarVentanaContactos(sender, 'formAgregarEditarContacto', true, function Fin(fin) { });

}

function editarContactoEntidad(sender, registro, index) {
    var idComponente = this.$widgetColumn.id.split('_')[0];

    App["winAgregarContacto"].setTitle(jsEditar + " " + jsContacto);
    App["winAgregarContacto"].show();

    showLoadMask(App["winAgregarContacto"], function (load) {

        VaciarFormularioContacto('formAgregarEditarContacto');
        DefinirDatosContacto('formAgregarEditarContacto', sender.$widgetRecord.data);
        cambiarATapContacto('formAgregarEditarContacto', 0);

        TreeCore['formAgregarEditarContacto'].MostrarEditarContacto(sender.$widgetRecord.data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        CargarVentanaContactos(sender, 'formAgregarEditarContacto', true, function Fin(fin) {
                            load.hide();
                        });
                    }
                }
            });

        load.hide();

    });
}

//function editarContactoEntidad(sender, registro, index) {
//    //DefinirDatosContacto(idComponente, App.GridRowSelectContacto.selected.items[0].data);
//    let contactoID = this.getWidgetRecord().id;
//    TreeCore.MostrarEditarContacto(contactoID,
//        {
//            success: function (result) {
//                if (result.Result != null && result.Result != '') {
//                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                } else {
//                    CargarVentanaContactos(undefined, 'formAgregarEditarContacto');
//                    App.winAgregarContacto.setTitle(jsEditar);
//                    App.winAgregarContacto.show();
//                }
//            },
//            eventMask:
//            {
//                showMask: true
//            }
//        });
//}




// #endregion

// #region RESIZER

function winFormResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(600);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(570);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormContactosResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(500);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formContactosResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(500);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormProveedorResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(800);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formProveedorResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(800);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormResizeDockBot(obj) {

    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    //obj.update();
}

function winCenter(obj) {
    obj.center();
}

function resizeWinForm() {
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

    var dv = document.querySelectorAll('div.WinFormContactos-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormContactosResize(obj);
    }

    var frm = document.querySelectorAll('div.ctFormContactos-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formContactosResize(obj);
    }

    var dv = document.querySelectorAll('div.WinFormProveedor-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormProveedorResize(obj);
    }

    var frm = document.querySelectorAll('div.ctFormProveedor-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formProveedorResize(obj);
    }

    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }

    var frm = document.querySelectorAll('div.winFormCenter');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        winCenter(obj);
    }

    resizeGridInfo();
}

window.addEventListener('resize', resizeWinForm);

function resizeGridInfo() {
    if (App.pnMoreInfo != null && App.pnMoreInfo != undefined) {
        App.pnMoreInfo.setMinHeight(window.innerHeight - 20);
        App.pnMoreInfo.setMaxHeight(window.innerHeight - 20);
        App.pnMoreInfo.updateLayout();
    }
}

// #endregion

//Toolbar filtros nuevo


function FiltrarColumnas(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App["gridEntities"];
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    //tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        App.btnDescargar.enable();
        return;
    }

    if (registro.getKey() === registro.ESC) {
        clearFilter();
    } else {
        // this will allow invalid regexp while composing, for example "(examples|grid|color)"
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        } catch (err) {
            return;
        }
        var columnas = hdColumnasEntidades.value.split("-");

        logic.filterBy(function (node) {
            var correcto = false;
            App.btnDescargar.disable();
            columnas.forEach(valores => {
                if (!correcto)
                    correcto = re.test(node.data[valores])

            });
            return correcto;
        });
    }
}


function LimpiarFiltroBusqueda(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App["gridEntities"];
    var store = tree.store,
        logic = store,
        field = App["txtSearch"];

    field.setValue("");
    App.btnDescargar.enable();
    logic.clearFilter();

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
function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

//AsignarImagenOperador
function AsignarImagenOperador() {
    if (registroSeleccionado(App.grid) && seleccionado != null && seleccionado.EsOperador) {
        App.btnGuardarImagenOperador.setDisabled(true);
        App.fuImagenOperador.setRawValue(null);

        //imgImagenOperador
        TreeCore.MostrarAsignarImagenOpeador(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                    }
                    else {
                        App.WinAsignarImagenOperador.show();

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

function validarExtensionImagenOperador(sender, registro, index) {

    let sp = sender.value.split(".");
    let extension = sp[sp.length - 1];

    if (extension.toLowerCase() == "svg") {
        App.btnGuardarImagenOperador.setDisabled(false);
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormatoImagenNoPermitido, buttons: Ext.Msg.OK });
        App.btnGuardarImagenOperador.setDisabled(true);
    }
}

function winAsignarImagenOperadorGuardar() {
    ajaxAsignarImagenOperador();
}

function ajaxAsignarImagenOperador() {
    TreeCore.AsignarImagenOpeador(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                }
                else {
                    App.WinAsignarImagenOperador.hide();
                    App.fuImagenOperador.setRawValue(null);
                    App.btnGuardarImagenOperador.setDisabled(true);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}
//End AsignarImagenOperador

function hideAsideREntidad(panel) {
    App.btnCollapseAsRClosed.show();
    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed == false) {
        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();
    }

    App.pnMoreInfo.show();
    btn.style.transform = 'rotate(0deg)';
    App.pnAsideR.expand();

    window.dispatchEvent(new Event('resizePlantilla'));
    GridColHandler();
}


function AsignarEntidadCliente() {
    TreeCore.AsignarEntidadCliente(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                }
                else {
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
function SeleccionarComboEntidades(sender, registro, index) {
    sender.getTrigger(0).show();
    FormularioValidoEntidades(sender, true);
}

function SeleccionarComboEntidadesProveedores(sender, registro, index) {
    sender.getTrigger(0).show();
    FormularioValidoProveedor(true);
}

function hidePanelMoreInfoEntidad(panel, registro) {
    let registroSeleccionado = panel.$widgetRecord;
    let ColumnaSeleccionado = panel.$widgetColumn;
    App.btnCollapseAsRClosed.show();

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed == false) {

        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();
    }

    if (App.WrapGestionColumnas != undefined) {
        App.WrapGestionColumnas.hide();
    }

    if (App.WrapFilterControls != undefined) {
        App.WrapFilterControls.hide();
    }

    App.pnMoreInfo.show();
    btn.style.transform = 'rotate(0deg)';
    GridColHandler();

    window.dispatchEvent(new Event('resizePlantilla'));
}

function hideAsideR(panel) {

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


function cargarDatosPanelMoreInfoEntidad(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    grid = Grid.store.data.items

    for (var columna of grid) {
        for (var prop of Object.keys(columna.data)) {
            if (columna.data.EntidadID == registro.id) {
                if (!prop.includes('ID')) {
                    if (columna.data[prop] != true && columna.data[prop] != false) {
                        for (var indice of Grid.columnManager.columns) {
                            if (indice.dataIndex == prop) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + indice.text + ' : </span><span class="dataGrd">' + columna.data[prop] + '</span></td></tr>';
                            }
                        }
                    }
                    else if (columna.data[prop] == false) {
                        for (var indice of Grid.columnManager.columns) {
                            if (indice.dataIndex == prop) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + indice.text + ' : </span><span class="dataGrd">&nbsp;</span></td></tr>';
                            }
                        }
                    }
                    else if (columna.data[prop] == "") {
                        for (var indice of Grid.columnManager.columns) {
                            if (indice.dataIndex == prop) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + indice.text + ' : </span><span class="dataGrd">' + columna.data[prop] + '</span></td></tr>';
                            }
                        }
                    }
                    else {
                        for (var indice of Grid.columnManager.columns) {
                            if (indice.dataIndex == prop) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + indice.text + ' : </span><span class="dataGrd positionLeftPanelMore">' + indice.renderer(columna.data[prop]) + '</span></td></tr>';
                            }
                        }
                    }
                }
            }
        }
    }

    tabla.innerHTML = html;
}