/* Funciones Stores */
function RecargarTablas(sender) {
    ruta = getIdStore(sender);
    recargarCombos([App[ruta + '_' + 'cmbValue'], App[ruta + '_' + 'cmbToolTip']]);
}

function RecargarControladores(sender) {
    ruta = getIdStore(sender);
    recargarCombos([App[ruta + '_' + 'cmbToolTip']]);
}

function RecargarCombos(sender, registro, index) {
    showLoadMaskCategorias(function (load) {
        recargarCombos([sender], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });
    });
}

/* Opciones Atributos */

function windowDataSetting(sender, registro, index) {
    ruta = 'AtributosConfiguracion';
    App[ruta + '_' + 'cmbTipoLista'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbTipoLista'].clearValue();
    //App[ruta + '_' + 'storeColumnasVinculadas'].reload();
    EsconderElementosVentanas(ruta);
    CargarStoresSerie([App[ruta + '_' + 'storeColumnasVinculadas']], function Fin(fin) {
        if (fin) {
            showLoadMaskCategorias(function (load) {
                TreeCore[ruta].MostrarSetting(
                    {
                        success: function (result) {
                            if (!result.Success) {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                load.hide();
                            }
                            else {
                                try {
                                    $('textarea[id="' + ruta + '_txtaSetting-inputEl' + '"]').amsifySuggestags({}, undefined, jsEmptyTextValor);
                                    var json = JSON.parse(result.Result);
                                    jQuery.each(json, function (key, val) {
                                        App[ruta + '_' + 'storeColumnasVinculadas'].add(val);
                                    });
                                } catch (e) {
                                    $('textarea[id="' + ruta + '_txtaSetting-inputEl' + '"]').amsifySuggestags({}, undefined, jsEmptyTextValor, result.Result);
                                }
                                CambiarDataSettingValidacion(ruta);
                                App[ruta + '_' + 'windowSetting'].show();
                                App[ruta + '_' + 'windowSetting'].center();
                                load.hide();
                            }
                        }
                    });
            });
        }
    });
}

function CloseWindowSetting(sender) {
    ruta = 'AtributosConfiguracion';
    App[ruta + '_' + 'storeColumnasVinculadas'].reload();
}

function CambiarDataSettingValidacion(ruta) {
    App[ruta + '_' + 'GridAddRestriction'].hide();
    App[ruta + '_' + 'GridFormatRule'].hide();
    App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(false);
    App[ruta + '_' + 'txtaSetting'].allowBlank = (true);
    App[ruta + '_' + 'cmbTable'].allowBlank = (true);
    App[ruta + '_' + 'cmbValue'].allowBlank = (true);
    App[ruta + '_' + 'cmbToolTip'].allowBlank = (true);
    if (App[ruta + '_' + 'cmbTipoLista'].selection != null) {
        switch (App[ruta + '_' + 'cmbTipoLista'].selection.data.field1) {
            case "1":
                App[ruta + '_' + 'txtaSetting'].allowBlank = (true);
                break;
            case "2":
                App[ruta + '_' + 'cmbTable'].allowBlank = (false);
                //App[ruta + '_' + 'cmbValue'].allowBlank = (false);
                //App[ruta + '_' + 'cmbToolTip'].allowBlank = (false);
                break;
            default:
                break;
        }
    }
    App[ruta + '_' + 'windowSetting'].center();
}

function GridFormatRule(pos) {
    //pos = sender;
    ruta = 'AtributosConfiguracion';
    CargarStoresSerie([App[ruta + '_' + 'storeFomatos']], function Fin(fin) {
        if (fin) {

            if (pos.y + App[ruta + '_GridFormatRule'].height > App.winGestion.height) {
                App[ruta + '_' + 'GridFormatRule'].show();
				App[ruta + '_' + 'GridAddRestriction'].hide();
                App[ruta + '_' + 'GridFormatRule'].setY(pos.offsetTop - App[ruta + '_GridFormatRule'].height);
                App[ruta + '_' + 'GridFormatRule'].setX(pos.offsetLeft - 225);
                App[ruta + '_' + 'GridFormatRule'].focus();
            }
            else {
                App[ruta + '_' + 'GridFormatRule'].show();
				App[ruta + '_' + 'GridAddRestriction'].hide();
                App[ruta + '_' + 'GridFormatRule'].setY(pos.offsetTop - 10);
                App[ruta + '_' + 'GridFormatRule'].setX(pos.offsetLeft - 225);
                App[ruta + '_' + 'GridFormatRule'].focus();
            }
        }
    })

}
function GridAddCondition(sender, registro, index) {

}

function GridAddRestriction(pos) {
    //pos = sender;
    ruta = 'AtributosConfiguracion';
    CargarStoresSerie([App[ruta + '_' + 'storeRolesRestringidos']], function Fin(fin) {
        if (fin) {            

            if (pos.y + App[ruta + '_GridAddRestriction'].height > App.winGestion.height) {
                App[ruta + '_' + 'GridAddRestriction'].show();
				App[ruta + '_' + 'GridFormatRule'].hide();
                App[ruta + '_' + 'GridAddRestriction'].setY(pos.offsetTop - App[ruta + '_GridAddRestriction'].height);
                App[ruta + '_' + 'GridAddRestriction'].setX(pos.offsetLeft - 225);
                App[ruta + '_' + 'GridAddRestriction'].focus();
            }
            else {
                //ventana = document.getElementById(ruta + '_' + 'GridAddRestriction');
                App[ruta + '_' + 'GridAddRestriction'].show();
				App[ruta + '_' + 'GridFormatRule'].hide();
                App[ruta + '_' + 'GridAddRestriction'].setY(pos.offsetTop - 10);
                App[ruta + '_' + 'GridAddRestriction'].setX(pos.offsetLeft - 225);
                App[ruta + '_' + 'GridAddRestriction'].focus();
            }
        }
    })
}

/* Ventana Data Setting */

function FormularioSettingValido(sender, valid) {
    if (sender.owner) {
        ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    } else {
        ruta = sender.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    }
    if (valid) {
        App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(false);
    }
    else {
        App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(true);
    }
}

function GuardarWindowSetting(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMask(sender.up().up(), function (load) {
        var json = {};
        if (App[ruta + '_' + 'cmbTipoLista'].selection.data.field1 == "2") {
            App[ruta + '_' + 'storeColumnasVinculadas'].data.items.forEach(col => {
                json[col.data.Name + col.data.ColumnaModeloDatoID] = {
                    "ColumnaID": col.data.ColumnaModeloDatoID,
                    "JsonRuta": col.data.JsonRuta,
                };
            });
        }
        TreeCore[ruta].GuardarSetting(JSON.stringify(json),
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        if (TreeCore[result.Result] != undefined) {
                            TreeCore[result.Result].PintarControl(true,
                                {
                                    success: function (result2) {
                                        if (!result2.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'windowSetting'].hide();
                                            DragDropCategorias();
                                            DragDropAtributosCategorias();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                        else {
                            TreeCore.PintarAtributos(true, true,
                                {
                                    success: function (result2) {
                                        if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'windowSetting'].hide();
                                            DragDropCategorias();
                                            DragDropAtributosCategorias();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                    }
                }
            });
    });
}

function EsconderElementosVentanas(ruta) {
    App[ruta + '_' + 'containerTable'].hide();
    App[ruta + '_' + 'containerValue'].hide();
    App[ruta + '_' + 'containerAux'].hide();
    App[ruta + '_' + 'containerTxtaSetting'].hide();
}

function SeleccionarTipoLista(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMask(this.up('panel').up('panel'), function (load) {
        App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(true);
        App[ruta + '_' + 'txtaSetting'].allowBlank = (true);
        App[ruta + '_' + 'cmbTable'].allowBlank = (true);
        App[ruta + '_' + 'cmbValue'].allowBlank = (true);
        App[ruta + '_' + 'cmbToolTip'].allowBlank = (true);
        EsconderElementosVentanas(ruta);
        sender.getTrigger(0).show();
        switch (sender.selection.data.field1) {
            case "1":
                App[ruta + '_' + 'containerTxtaSetting'].show();
                App[ruta + '_' + 'txtaSetting'].setValue('');
                App[ruta + '_' + 'txtaSetting'].allowBlank = (true);
                App[ruta + '_' + 'lbTxtaSetting'].setText(jsLista);
                App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(false);
                load.hide();
                break;
            case "2":
                recargarCombos([App[ruta + '_' + 'cmbTable']], function Fin(fin) {
                    if (fin) {
                        App[ruta + '_' + 'cmbTable'].clearValue();
                        App[ruta + '_' + 'btnAddColumn'].disable();
                        App[ruta + '_' + 'containerTable'].show();
                        App[ruta + '_' + 'cmbValue'].clearValue();
                        App[ruta + '_' + 'containerValue'].show();
                        App[ruta + '_' + 'containerAux'].show();
                        App[ruta + '_' + 'cmbToolTip'].clearValue();
                        App[ruta + '_' + 'cmbTable'].allowBlank = (false);
                        App[ruta + '_' + 'cmbValue'].allowBlank = (false);
                        App[ruta + '_' + 'windowSetting'].center();
                        load.hide();
                    }
                });
                break;
            default:
                break;
        }
        App[ruta + '_' + 'windowSetting'].center();
    });
}

function RecargarComboTipoLista(sender, registro, index) {
    ruta = getIdComponente(sender);
    sender.getTrigger(0).hide();
    sender.clearValue();
    EsconderElementosVentanas(ruta);
    App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(true);
    App[ruta + '_' + 'windowSetting'].center();
}

function SeleccionarTable(sender, registro, index) {
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'hdTablaActual'].setValue(sender.value);
    App[ruta + '_' + 'lbRutaEmplazamientoTipo'].setText(sender.rawValue);
    App[ruta + '_' + 'storeColumnasVinculadas'].reload();
    App[ruta + '_' + 'btnAddColumn'].enable();
}

function RecargarTable(sender, registro, index) {
    RecargarCombo(sender);
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'btnAddColumn'].disable();
    App[ruta + '_' + 'storeColumnasVinculadas'].reload();
}

function SeleccionarIndex(sender, registro, index) {
    sender.getTrigger(0).show();
}

function SeleccionarValue(sender, registro, index) {
    sender.getTrigger(0).show();
}

/* Ventana Format */

function hideGridFormatRule(sender, registro, index) {
    ruta = getIdComponente(sender);
    try {
        if (registro.toComponent.command != "EliminarFormato") {
            App[ruta + '_' + 'GridFormatRule'].hide();
        }
    } catch (e) {
        App[ruta + '_' + 'GridFormatRule'].hide();
    }
}

function CerrarVentanaFormat(sender, registro, index) {
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'storeFomatos'].reload();
    //TreeCore[ruta].LimpiarContenedorFormat();
}

function GuardarWinFormat(sender, registro, index) {

    var control = sender;
    ruta = getIdComponente(sender);
    if (App[ruta + '_' + 'cmbTiposPropiedades'].selection.data.Codigo == 'Regex') {
        try {
            re = new RegExp(App[ruta + '_' + 'txtCampoValueFormat'].value);
            showLoadMask(this.up('panel'), function (load) {
                TreeCore[ruta].GuardarFormat(
                    {
                        success: function (result) {
                            if (!result.Success) {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                load.hide();
                            }
                            else {
                                if (TreeCore[result.Result] != undefined) {
                                    TreeCore[result.Result].PintarControl(true,
                                        {
                                            success: function (result2) {
                                                if (!result2.Success) {
                                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                                } else {
                                                    App[ruta + '_' + 'winFormat'].hide();
                                                    DragDropCategorias();
                                                    DragDropAtributosCategorias();
                                                }
                                                load.hide();
                                            }
                                        });
                                }
                                else {
                                    TreeCore.PintarAtributos(true, true,
                                        {
                                            success: function (result2) {
                                                if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                                } else {
                                                    App[ruta + '_' + 'winFormat'].hide();
                                                    DragDropCategorias();
                                                    DragDropAtributosCategorias();
                                                }
                                                load.hide();
                                            }
                                        });
                                }
                            }
                        }
                    });
            });
        } catch (e) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsRegexInvalido, buttons: Ext.Msg.OK });
        }
    } else {
        showLoadMask(this.up('panel'), function (load) {
            TreeCore[ruta].GuardarFormat(
                {
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            load.hide();
                        }
                        else {
                            if (TreeCore[result.Result] != undefined) {
                                TreeCore[result.Result].PintarControl(true,
                                    {
                                        success: function (result2) {
                                            if (!result2.Success) {
                                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                            } else {
                                                App[ruta + '_' + 'winFormat'].hide();
                                                DragDropCategorias();
                                                DragDropAtributosCategorias();
                                            }
                                            load.hide();
                                        }
                                    });
                            }
                            else {
                                TreeCore.PintarAtributos(true, true,
                                    {
                                        success: function (result2) {
                                            if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                            } else {
                                                App[ruta + '_' + 'winFormat'].hide();
                                                DragDropCategorias();
                                                DragDropAtributosCategorias();
                                            }
                                            load.hide();
                                        }
                                    });
                            }
                        }
                    }
                });
        });
    }
}

function ValidarCampoFormat(sender) {
    var control = sender;
    ruta = getIdComponente(sender);
    if (App[ruta + '_' + 'cmbTiposPropiedades'].value == 'Regex') {
        try {
            re = new RegExp(control.value);
        } catch (e) {
            console.log(e);
        }
    }
}

function BotonAddFormatRule(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMaskCategorias(function (load) {
        recargarCombos([App[ruta + '_' + 'cmbTiposPropiedades']], function Fin(fin) {
            if (fin) {
                TreeCore[ruta].CambiarValorFormat(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App[ruta + '_' + 'winFormat'].show();
                            load.hide();
                        }
                    });
            }
        });
    });
}

function EliminarFormato(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMaskCategorias(function (load) {
        TreeCore[ruta].EliminarFormato(index.data.AtributoTipoDatoPropiedadID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        if (TreeCore[result.Result] != undefined) {
                            TreeCore[result.Result].PintarControl(true,
                                {
                                    success: function (result2) {
                                        if (!result2.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'storeFomatos'].reload();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                        else {
                            TreeCore.PintarAtributos(true, true,
                                {
                                    success: function (result2) {
                                        if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'storeFomatos'].reload();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                    }
                }
            });
    });
}

function SeleccionarTiposPropiedades(sender, registro, index) {
    ruta = getIdComponente(sender);
    sender.getTrigger(0).show();
    showLoadMask(this.up('panel').up('panel'), function (load) {
        TreeCore[ruta].CambiarValorFormat(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    load.hide();
                }
            });
    });
}
function RecargarTiposPropiedades(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMask(this.up('panel').up('panel'), function (load) {
        recargarCombos([App[ruta + '_' + 'cmbTiposPropiedades']], function Fin(fin) {
            if (fin) {
                TreeCore[ruta].CambiarValorFormat(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            load.hide();
                        }
                    });
            }
        });
    });
}

function FormularioFormatValido(sender, valid) {
    ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    if (valid) {
        App[ruta + '_' + 'btnGuardarFormat'].setDisabled(false);
    }
    else {
        App[ruta + '_' + 'btnGuardarFormat'].setDisabled(true);
    }

}

/* Ventana New Condition */

function GuardarWinNewCondition() {

}

function SeleccionarOperador(sender, registro, index) {
    sender.getTrigger(0).show();

}

function SeleccionarTablaNewCondition(sender, registro, index) {
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
}

/* Ventana Add Restriction */

function GuardarWinAddRestriction(sender, registro, index) {
    ruta = getIdComponente(sender);
    var lRestr;
    App[ruta + '_' + 'containerRestrictions'].items.items.forEach(item => { if (item.checked) lRestr = item.inputValue })
    showLoadMask(this.up('panel'), function (load) {
        TreeCore[ruta].GuardarRestriction(lRestr,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        if (TreeCore[result.Result] != undefined) {
                            TreeCore[result.Result].PintarControl(true,
                                {
                                    success: function (result2) {
                                        if (!result2.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'winAddRestriction'].hide();
                                            DragDropCategorias();
                                            DragDropAtributosCategorias();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                        else {
                            TreeCore.PintarAtributos(true, true,
                                {
                                    success: function (result2) {
                                        if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'winAddRestriction'].hide();
                                            DragDropCategorias();
                                            DragDropAtributosCategorias();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                    }
                }
            });
    });
}

function SeleccionarRoles(sender, registro, index) {
    sender.getTrigger(0).show();

}


function winAddRestriction(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMaskCategorias(function (load) {
        recargarCombos([App[ruta + '_' + 'cmbRoles']], function Fin(fin) {
            if (fin) {
                TreeCore[ruta].CambiarValorFormat(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App[ruta + '_' + 'radActive'].setValue(true);
                            App[ruta + '_' + 'winAddRestriction'].show();
                            //App[ruta + '_' + 'GridAddRestriction'].hide();
                            load.hide();
                        }
                    });
            }
        });
    });
}

function CerrarVentanaRestriction(sender) {
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'storeRolesRestringidos'].reload();
}

function hideGridAddRestriction(sender, registro, index) {
    ruta = getIdComponente(sender);
    try {
        if (registro.toComponent.command != "EliminarRestriction") {
            App[ruta + '_' + 'GridAddRestriction'].hide();
        }
    } catch (e) {
        App[ruta + '_' + 'GridAddRestriction'].hide();
    }
}

function rendeAddRestriction(value) {
    if (value == 3) {
        return '<span class="ico-hidden" >&nbsp;</span>';
    }
    else if (value == 2) {
        return '<span class="ico-disabled-render" >&nbsp;</span>';
    }
    else if (value == 1) {
        return '<span class="ico-checked-16-greyInv">&nbsp;</span>';
    }
}

function EliminarRestriccion(sender, registro, index) {
    ruta = getIdComponente(sender);
    showLoadMaskCategorias(function (load) {
        TreeCore[ruta].EliminarRestriction(index.data.AtributoRolRestringidoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        if (TreeCore[result.Result] != undefined) {
                            TreeCore[result.Result].PintarControl(true,
                                {
                                    success: function (result2) {
                                        if (!result2.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'storeRolesRestringidos'].reload();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                        else {
                            TreeCore.PintarAtributos(true, true,
                                {
                                    success: function (result2) {
                                        if (result2.Result != undefined && result2.Result != "" && result2.Result != null) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                        } else {
                                            App[ruta + '_' + 'storeRolesRestringidos'].reload();
                                        }
                                        load.hide();
                                    }
                                });
                        }
                    }
                }
            });
    });
}

function FormularioValidoRestriction(sender, valid) {
    ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    if (valid) {
        App[ruta + '_' + 'btnWinAddRestriction'].setDisabled(false);
    }
    else {
        App[ruta + '_' + 'btnWinAddRestriction'].setDisabled(true);
    }

}

function hideGridAddCondition() { App.GridAddCondition.hide() }

//#region Navegacion Columnas

function RenderCarpetas(value) {
    if (value == true || value == 1) {
        return '<span class="gen_folder">&nbsp;</span>'
    }
    else {
        return '<span class="gen_column">&nbsp;</span>'
    }
}

function RenderBtnAddColumn(value) {
    if (value == true || value == 1) {
        return '<span class="gen_folder">&nbsp;</span>'
    }
    else {
        return '<span class="gen_column">&nbsp;</span>'
    }
}

function ValidarLinkedList(sender) {
    ruta = getIdStore(sender);
    if (sender.data.length > 0) {
        App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(false);
    } else {
        App[ruta + '_' + 'btnGuardarWindowSetting'].setDisabled(true);
    }
}

function AccionSelectCol(sender, registro) {
    ruta = getIdComponente(sender.up());
    if (registro.data.esCarpeta) {
        var reg = {
            text: registro.data.Name,
            TablaID: registro.data.ObjectID,
            ColumnaID: registro.data.ColumnaID,
            IndiceColumna: registro.data.IndiceColumna
        };
        App[ruta + '_' + 'hdTablaActual'].setValue(registro.data.ObjectID);
        App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
        listaRuta.push(reg);
        SeleccionarRuta(sender, reg);
    } else {
        if (App[ruta + '_' + 'storeColumnasVinculadas'].data.getByKey(registro.data.ObjectID) != undefined) {
            Toast.create(jsInfo, jsColumnaYaAnadida, TOAST_STATUS.INFO, 2000);
        } else {
            App[ruta + '_' + 'storeColumnasVinculadas'].add({ "ID": registro.data.ObjectID, "Name": registro.data.Name, "AtributoID": App[ruta + '_' + 'hdAtributoID'].value, "ColumnaModeloDatoID": registro.data.ObjectID, "JsonRuta": JSON.stringify(listaRuta) })
            Toast.create(jsInfo, jsColumnaAnadida, TOAST_STATUS.SUCCESS, 2000);
        }
    }
}

function EliminarColumnaVin(sender, registro, index) {
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'storeColumnasVinculadas'].data.removeAtKey(index.data.ID)
}

function btnAddColumn(sender) {
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'hdTablaActual'].setValue(App[ruta + '_' + 'cmbTable'].value);
    App[ruta + '_' + 'lbRutaEmplazamientoTipo'].setText(App[ruta + '_' + 'cmbTable'].rawValue);
    IrRutaRaiz(sender)
    //App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
    App[ruta + '_' + 'winAddColumns'].show();
}

var listaRuta = [];

/*
    {
        text = "Test",
        TablaID = 8
    }
 */

function VolverAtras(sender) {
    ruta = getIdComponente(sender);
    if (listaRuta.length >= 2) {
        listaRuta.pop();
        var TblAnterior = listaRuta[listaRuta.length - 1];
        App[ruta + '_' + 'hdTablaActual'].setValue(TblAnterior.TablaID);
        App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
        App[ruta + '_' + 'lbRutaCategoria'].show();
        App[ruta + '_' + 'lbRutaCategoria'].setText(TblAnterior.text);
        App[ruta + '_' + 'menuRuta'].items.clear();
        GenerarRuta(sender);
    } else {
        IrRutaRaiz(sender);
    }
}

function GenerarID() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function IrRutaRaiz(sender) {
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'hdTablaActual'].setValue(App[ruta + '_' + 'cmbTable'].value);
    //App[ruta + '_' + 'lbRutaEmplazamientoTipo'].setText(App[ruta + '_' + 'cmbTable'].rawValue);
    LimpiarRuta(sender);
    //App[ruta + '_' + 'btnPadreCarpetaActucal'].hide();
    App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
}

function SeleccionarRuta(sender, select) {
    ruta = getIdComponente(sender.up());
    App[ruta + '_' + 'hdTablaActual'].setValue(select.TablaID);
    App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
    App[ruta + '_' + 'lbRutaCategoria'].show();
    App[ruta + '_' + 'btnCarpetaActual'].show();
    App[ruta + '_' + 'lbRutaCategoria'].setText(select.text);
    for (var i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].TablaID == select.TablaID) {
            listaRuta = listaRuta.splice(0, ++i);
            App[ruta + '_' + 'hdCampoVinculadoRuta'].setValue(JSON.stringify(listaRuta));
        }
    }
    App[ruta + '_' + 'menuRuta'].items.clear();
    GenerarRuta(sender);
}

function SeleccionarPadre(sender, select) {
    ruta = getIdComponente(sender);
    LimpiarRuta(sender);
    for (var i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].TablaID == select.TablaID) {
            listaRuta = listaRuta.splice(0, ++i);
        }
    }
    App[ruta + '_' + 'hdTablaActual'].setValue(select.TablaID);
    App[ruta + '_' + 'storeSelectCamposVinculados'].reload();
    App[ruta + '_' + 'lbRutaCategoria'].show();
    App[ruta + '_' + 'btnCarpetaActual'].show();
    App[ruta + '_' + 'lbRutaCategoria'].setText(select.text);
}

function LimpiarRuta(sender) {
    ruta = getIdComponente(sender);
    forzarCargaBuscadorPredictivo = true;
    App[ruta + '_' + 'btnMenuRuta'].hide();
    App[ruta + '_' + 'btnRaizCarpeta'].hide();
    App[ruta + '_' + 'lbRutaCategoria'].hide();
    App[ruta + '_' + 'btnCarpetaActual'].hide();
    App[ruta + '_' + 'menuRuta'].items.clear();
    listaRuta = [];
}

function GenerarRuta(sender) {
    ruta = getIdComponente(sender.up());
    App[ruta + '_' + 'btnMenuRuta'].show();
    App[ruta + '_' + 'btnRaizCarpeta'].show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    jQuery.each(listaRuta, function (i, item) {
        App[ruta + '_' + 'menuRuta'].add(new Ext.menu.TextItem({ text: item.text, TablaID: item.TablaID }))
    });
    if (App[ruta + '_' + 'menuRuta'].items.items.length > 1) {
        App[ruta + '_' + 'menuRuta'].items.last().hide();
    } else {
        App[ruta + '_' + 'btnMenuRuta'].hide();
        App[ruta + '_' + 'btnRaizCarpeta'].hide();
    }
}

function gridCamposVinculados_RowSelect(sender) {

}

function LinkedGridDoubleClick(sender) {

}

function trackDragDrop(sender) {

}

//#endregion