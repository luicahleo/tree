var pautasCodigo;
var pautasNombre;



function CargarFormulario(ruta, callback, recargar = true) {
    var finRecargar;
    TreeCore[ruta].PintarCategorias(recargar, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                recargarCombos([App[ruta + '_' + 'cmbEstado'], App[ruta + '_' + 'cmbCategoriaElemento'], App[ruta + '_' + 'cmbPlantilla'], App[ruta + '_' + 'cmbOperador']], function Fin(fin) {
                    if (fin) {
                        if (App[ruta + '_' + 'hdCatID'].value != "" && App[ruta + '_' + 'hdCatID'].value != 0) {
                            App[ruta + '_' + 'cmbCategoriaElemento'].setValue(App[ruta + '_' + 'hdCatID'].value);
                        }
                        if (recargar) {
                            Ext.each(App[ruta + '_' + 'pnConfigurador'].body.query('*'), function (item) {
                                var c = Ext.getCmp(item.id);
                                if (c && c.isFormField) {
                                    if (c.value != null || c.value != undefined) {
                                        if (c.id != ruta + '_' + 'cmbEstado' && c.id != ruta + '_' + 'cmbOperador' && c.id != ruta + '_' + 'cmbCategoriaElemento') {
                                            c.reset();
                                        }
                                        if (c.triggerWrap != undefined) {
                                            c.triggerWrap.removeCls("itemForm-novalid");
                                        }

                                        if (!c.allowBlank && c.xtype != "checkboxfield") {
                                            c.addListener("change", anadirClsNoValido, false);
                                            c.addListener("focusleave", anadirClsNoValido, false);

                                            c.removeCls("ico-exclamacion-10px-red");
                                            c.addCls("ico-exclamacion-10px-grey");
                                        }

                                        if ((c.allowBlank && c.cls == 'txtContainerCategorias') || c.xtype == "checkboxfield") {
                                            App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                                        }
                                    }
                                }
                                if (item.id == App[ruta + '_' + 'pnConfigurador'].body.last().id) {
                                    GenerarCodigoInventario(ruta, function GenerarCodigo(codigoGenerado) {
                                        if (codigoGenerado) {
                                            callback(true, null);
                                        }
                                    })
                                }
                            });
                        } else {
                            pautasCodigo = {};
                            pautasNombre = {};
                            callback(true, null);
                        }
                    }
                })
            }
        }
    });
}

function GenerarCodigoInventario(ruta, callback) {
    TreeCore[ruta].GenerarCodigoInventario({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                pautasCodigo = result.Result;
                TreeCore[ruta].GenerarNombreInventario({
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            pautasNombre = result.Result;
                            GenerarCodigo(App[ruta + '_' + 'txtCodigoElemento'], App[ruta + '_' + 'pnConfigurador'], pautasCodigo, App[ruta + '_' + 'hdCodigoAutogenerado']);
                            GenerarCodigo(App[ruta + '_' + 'txtNombreElemento'], App[ruta + '_' + 'pnConfigurador'], pautasNombre, App[ruta + '_' + 'hdNombreAutogenerado']);
                        }
                        callback(true, null);
                    }
                });
            }
        }
    });
}


//#region Combos

function SeleccionarCategoria(sender) {
    showLoadMask(this.up('panel').up().up(), function (load) {
        sender.getTrigger(0).show();
        ruta = getIdComponente(sender);
        App[ruta + '_' + 'hdCatID'].setValue(sender.value);
        recargarCombos([App[ruta + '_' + 'cmbPlantilla']]);
        try {
            TreeCore[ruta].PintarCategorias(true,
                {
                    success: function (result) {
                        try {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                GenerarCodigo(App[ruta + '_' + 'txtCodigoElemento'], App[ruta + '_' + 'pnConfigurador'], pautasCodigo, App[ruta + '_' + 'hdCodigoAutogenerado']);
                                GenerarCodigo(App[ruta + '_' + 'txtNombreElemento'], App[ruta + '_' + 'pnConfigurador'], pautasNombre, App[ruta + '_' + 'hdNombreAutogenerado']);
                                try {
                                    App[ruta + '_' + 'contenedorCategorias'].updateLayout();
                                } catch (e) {
                                    console.log('error1');
                                }
                            }
                            load.hide();
                        } catch (e) {
                            console.log('error2');
                        }
                    }
                });
        } catch (e) {
            console.log('error3');
        }
    });
}

function RecargarCategoria(sender) {
    showLoadMask(this.up('panel').up().up(), function (load) {
        recargarCombos([App[ruta + '_' + 'cmbCategoriaElemento']], function Fin(fin) {
            if (fin) {
                App[ruta + '_' + 'hdCatID'].setValue('');
                TreeCore[ruta].PintarCategorias(true,
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                GenerarCodigo(App[ruta + '_' + 'txtCodigoElemento'], App[ruta + '_' + 'pnConfigurador'], pautasCodigo, App[ruta + '_' + 'hdCodigoAutogenerado']);
                                GenerarCodigo(App[ruta + '_' + 'txtNombreElemento'], App[ruta + '_' + 'pnConfigurador'], pautasNombre, App[ruta + '_' + 'hdNombreAutogenerado']);
                            }
                            load.hide();
                        }
                    });
            }
        })
    });
}

function SeleccionarEstado(sender) {

}

function RecargarEstado(sender) {

}

function SeleccionarPlantilla(sender) {
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
    showLoadMask(this.up('panel'), function (load) {
        TreeCore[ruta].SeleccionarPlantilla(
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        load.hide();
                    }
                }
            });
    });
}

function RecargarPlantilla(sender) {
    recargarCombos([sender]);
}

//#endregion

//#region Botones Guardar/Cancelar

function FormularioValidoInventario(sender, valid, aux, ruta) {
    try {
        //if (ruta == undefined) {
        ruta = sender.id.split('_'); ruta.pop(); ruta = ruta.join('_');
        //}
        if (valid == true && App.hdVistaPlantilla != undefined && (App.hdVistaPlantilla.value != undefined && App.hdVistaPlantilla.value != "")) {
            App[ruta + '_' + 'btnGuardarAgregarEditar'].setDisabled(false);
        }
        else {
            App[ruta + '_' + 'btnGuardarAgregarEditar'].setDisabled(false);
            Ext.each(App[ruta + '_' + 'pnConfigurador'].body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App[ruta + '_' + 'btnGuardarAgregarEditar'].setDisabled(true);

                    if (c.triggerWrap != undefined) {
                        c.triggerWrap.removeCls("itemForm-novalid");
                    }

                    if (!c.allowBlank && c.xtype != "checkboxfield") {
                        c.addListener("change", anadirClsNoValido, false);
                        c.addListener("focusleave", anadirClsNoValido, false);

                        c.removeCls("ico-exclamacion-10px-red");
                        c.addCls("ico-exclamacion-10px-grey");
                    }

                    if ((c.allowBlank && c.cls == 'txtContainerCategorias') || c.xtype == "checkboxfield") {
                        App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                    }
                }
            });
        }
    } catch (e) {

    }
}
function FormularioValido(sender, valid, aux, ruta) {
    FormularioValidoInventario(sender, valid, aux, ruta);


}


let VarRutaInventario;
let VarRegistroInventario;
let VarIndexInventario;
let VarSenderInventario;


function btnGuardarFormElementos(sender, registro, index, ruta) {
    ruta = getIdComponente(sender);

    VarRegistroInventario = registro;
    VarIndexInventario = index;
    VarSenderInventario = sender;
    VarRutaInventario = ruta;

    TreeCore[ruta].GuardarValor(false,
        {
            success: function (result) {
                if (result.Result == 'Editado') {
                    Ext.Msg.alert(
                        {
                            title: jsControlEdicion,
                            msg: jsComprobarEdicionRegistro,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                no: jsRecargarFormulario,
                                yes: jsSobrescribir
                            },
                            fn: ajaxAgregarEditarInventarioEditado,
                            cls: 'winFormEditado',
                            width: '500px'
                        });
                }
                else if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    Refrescar();
                    App["winGestion"].close();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}


function ajaxAgregarEditarInventarioEditado(sender, registro, index, ruta) {

    if (sender == "yes") {
        if (VarRutaInventario != "") {
            TreeCore[VarRutaInventario].GuardarValor(true,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            Refrescar();
                            App["winGestion"].close();
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
    else {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        CargarFormulario(sender, 'formAgregarEditar', false, function Fin(fin) {
                            load.hide();
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

}

function DisabledControl(sender) {
    sender.disable();
}
//#endregion

function RecargarComboInventario(sender, registro, index) {
    recargarCombos([sender]);
    ruta = getIdComponente(sender);
    GenerarCodigo(App[ruta + '_' + 'txtCodigoElemento'], App[ruta + '_' + 'pnConfigurador'], pautasCodigo, App[ruta + '_' + 'hdCodigoAutogenerado']);
    GenerarCodigo(App[ruta + '_' + 'txtNombreElemento'], App[ruta + '_' + 'pnConfigurador'], pautasNombre, App[ruta + '_' + 'hdNombreAutogenerado']);
}

function SeleccionarComboInventario(sender, registro, index) {
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
    GenerarCodigo(App[ruta + '_' + 'txtCodigoElemento'], App[ruta + '_' + 'pnConfigurador'], pautasCodigo, App[ruta + '_' + 'hdCodigoAutogenerado']);
    GenerarCodigo(App[ruta + '_' + 'txtNombreElemento'], App[ruta + '_' + 'pnConfigurador'], pautasNombre, App[ruta + '_' + 'hdNombreAutogenerado']);
    FormularioValidoInventario(sender, true, undefined, ruta);
}

function LimpiarFormulario(ruta) {
    Ext.each(App[ruta + '_' + 'pnConfigurador'].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.value != null || c.value != undefined) {
                if (c.id != ruta + '_' + 'cmbEstado' && c.id != ruta + '_' + 'cmbOperador' && c.id != ruta + '_' + 'cmbCategoriaElemento') {
                    c.reset();
                }
            }
        }
    });
}