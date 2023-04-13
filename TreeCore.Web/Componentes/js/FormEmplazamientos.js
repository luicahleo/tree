var pautasCodigo;

var ContadorIntentos = 100;

setTimeout(resizeWinForm, 100);

function DefinirDatosEmplazamiento(ruta, dato) {
    App[ruta + "_hdEstadoGlobal"] = dato.EstadoGlobal;
    App[ruta + "_hdMoneda"] = dato.Moneda;
    App[ruta + "_hdCategoria"] = dato.CategoriaSitio;
    App[ruta + "_hdTipos"] = dato.Tipo;
    App[ruta + "_hdTipoEdificio"] = dato.TipoEdificio;
    App[ruta + "_hdTipoEstructura"] = dato.TipoEstructura;
    App[ruta + "_hdTamano"] = dato.Tamano;
}

function CargarVentanaEmplazamiento(sender, idComponente, vaciar, callback) {

    var ruta = getIdComponente(sender) + '_' + idComponente;
    TreeCore[ruta].PintarCategorias(false);
    CargarStoresSerie([App[ruta + '_' + 'storeOperadores'], App[ruta + '_' + 'storeEstadosGlobales'], App[ruta + '_' + 'storeCategorias'],
    App[ruta + '_' + 'storeTamanos'], App[ruta + '_' + 'storeTipos'], App[ruta + '_' + 'storeMonedas'], App[ruta + '_' + 'storeTiposEstructuras'],
    App[ruta + '_' + 'storeTiposEdificios']], function Fin(fin) {
        if (fin) {
            try {
                if (vaciar) {
                    TreeCore[ruta].GenerarCodigoEmplazamiento(
                        {
                            success: function (result) {
                                if (!result.Success) {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                } else {
                                    pautasCodigo = result.Result;
                                    GenerarCodigo(App[ruta + '_' + 'txtCodigo'], App[ruta + '_' + 'containerFormEmplazamiento'], pautasCodigo, App[ruta + '_' + 'hdCodigoEmplazamientoAutogenerado']);
                                }
                                callback(true, null);
                            }
                        });
                } else {
                    pautasCodigo = {};
                    callback(true, null);
                }
            } catch (e) {
                pautasCodigo = {};
                callback(true, null);
            }
        }
    });

}

// DIRECT METHOD

function VaciarFormularioEmplazamiento(ruta) {

    App[ruta + '_' + 'txtCodigo'].setValue();
    App[ruta + '_' + 'txtNombre'].setValue();
    App[ruta + '_' + 'cmbOperadores'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbEstadosGlobales'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbCategorias'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbTipos'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbTamanos'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbMonedas'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbTiposEstructuras'].getTrigger(0).hide();
    App[ruta + '_' + 'cmbTiposEdificios'].getTrigger(0).hide();
    App[ruta + '_' + 'hdEmplazamientoID'].setValue('');
    App[ruta + '_' + 'hdEstadoGlobal'].setValue('');
    App[ruta + '_' + 'hdCategoria'].setValue('');
    App[ruta + '_' + 'hdTipos'].setValue('');
    App[ruta + '_' + 'hdTamanos'].setValue('');
    App[ruta + '_' + 'hdTipoEstructura'].setValue('');
    App[ruta + '_' + 'hdTipoEdificio'].setValue('');
    App[ruta + '_' + 'hdOperador'].setValue('');
    App[ruta + '_' + 'hdMoneda'].setValue('');

    var formPanel = App[ruta + '_' + 'containerFormEmplazamiento'];

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }

            if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
            try {
                c.reset();
            } catch (e) {

            }
        }
    });

    App[ruta + '_' + 'geoEmplazamiento' + '_' + 'cmbMunicipioProvincia'].addListener("select", SeleccionarComboEmplazamiento, false);
}

function winGestionBotonGuardarEmplazamiento(sender, registro, index) {
    ruta = getIdComponente(sender);
    if (App[ruta + '_' + 'containerFormEmplazamiento'].isValid()) {
        ajaxAgregarEditarEmplazamiento(sender, registro, index, ruta);

    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

let VarRuta;
let VarRegistro;
let VarIndex;
let VarSender;

function ajaxAgregarEditarEmplazamiento(sender, registro, index, ruta) {

    ContadorIntentos = 100;

    VarRuta = ruta;
    VarRegistro = registro;
    VarIndex = index;
    VarSender = sender;

    TreeCore[ruta].AgregarEditar(false,
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
                            fn: ajaxAgregarEditarEmplazamientoEditado,
                            cls: 'winFormEditado',
                            width: '500px'
                        });
                } else if (result.Result == 'Codigo') {
                    Ext.Msg.alert(
                        {
                            title: jsControlCodigo,
                            msg: jsComprobarCodigoGenerado,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                no: jsCodigoManual,
                                yes: jsGenerarCodigo
                            },
                            fn: ajaxGenerarNuevoCodigo,
                            cls: 'winFormEditado',
                            width: '500px'
                        });
                }
                else if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    let idComponente = getIdComponente(sender);
                    let window = Ext.getCmp(idComponente + '_containerFormEmplazamiento').up('window');
                    window.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}


function ajaxComprobarHdEditado() {
    if (ContadorIntentos == 0) {
        Ext.Msg.alert(
            {
                title: jsControlCodigo,
                msg: jsLimiteIntentosCodigo,
                buttons: Ext.Msg.OK,
                cls: 'winFormEditado',
                width: '500px'
            });
    }
    //else if (App.hdCodigoDuplicado.Value == "Duplicado" && ContadorIntentos > 0) {
    //    ajaxGenerarNuevoCodigo()
    //}
}


function ajaxGenerarNuevoCodigo(sender) {

    //TreeCore.GenerarCodigoInventarioDuplicado(
    //    {
    //        success: function (result) {
    //            if (result.Result != null && result.Result != '') {
    //                App.txtCodigoElemento.setValue(App.hdCodigoEmplazamientoAutogenerado.value.toString());
    //            }
    //        },
    //        eventMask:
    //        {
    //            showMask: true,
    //            msg: jsMensajeProcesando
    //        }
    //    });

    //if (ContadorIntentos == 0) {
    //    Ext.Msg.alert(
    //        {
    //            title: jsControlCodigo,
    //            msg: jsComprobarCodigoGenerado,
    //            buttons: Ext.Msg.OK,
    //            cls: 'winFormEditado',
    //            width: '500px'
    //        });
    //}

    //do {
    if (sender == "yes") {

        TreeCore[ruta].GenerarCodigoEmplazamiento({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    pautasCodigo = result.Result;

                    GenerarCodigoDuplicado(App[ruta + '_' + 'txtCodigo'], App[ruta + '_' + 'containerFormEmplazamiento'], pautasCodigo, App[ruta + '_' + 'hdCodigoEmplazamientoAutogenerado']);

                    ContadorIntentos = ContadorIntentos - 1;

                    TreeCore[ruta].ComprobarCodigoEmplazamientoDuplicado(
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    App[ruta + '_' + 'txtCodigo'].setValue(App.hdCodigoEmplazamientoAutogenerado.value.toString());
                                }
                            },
                            eventMask:
                            {
                                showMask: true,
                                msg: jsMensajeProcesando
                            }
                        });
                    ajaxComprobarHdEditado();
                }
                load.hide();
            }
        });
    }
    else {

        App[ruta + '_' + 'txtCodigo'].setValue("");
        App[ruta + '_' + 'txtCodigo'].setEmptyText("");
    }

    //} while (ContadorIntentos > 0 && hdCodigoDuplicado.value.toString() == "Duplicado");

    //if (ContadorIntentos > 0 && hdCodigoDuplicado.value.toString() == "Duplicado") {

    //    GenerarCodigoDuplicado(App.txtCodigoElemento, App.pnConfigurador, pautasCodigo, App.hdCodigoEmplazamientoAutogenerado);

    //    ContadorIntentos = ContadorIntentos - 1;

    //    TreeCore.ComprobarCodigoInventarioDuplicado(
    //        {
    //            success: function (result) {
    //                if (result.Result != null && result.Result != '') {
    //                    App.txtCodigoElemento.setValue(App.hdCodigoEmplazamientoAutogenerado.value.toString());
    //                }
    //            },
    //            eventMask:
    //            {
    //                showMask: true,
    //                msg: jsMensajeProcesando
    //            }
    //        });
    //}
}

function ajaxAgregarEditarEmplazamientoEditado(sender, registro, index, ruta) {

    if (sender == "yes") {
        TreeCore[VarRuta].AgregarEditar(true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        let idComponente = getIdComponente(VarSender);
                        let window = Ext.getCmp(idComponente + '_containerFormEmplazamiento').up('window');
                        window.hide();
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
        var RutaFinal = VarRuta.split("_", 1)
        TreeCore[RutaFinal].MostrarEditar(App[RutaFinal + '_GridRowSelect'].selected.items[0].data.EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        CargarVentanaEmplazamiento(sender, 'formAgregarEditar', false, function Fin(fin) {
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

//CERRAR VENTANA
function closeWindowEmplazamiento(sender, registro, inde) {
    var ruta = getIdComponente(sender);
    VaciarFormularioEmplazamiento(ruta);
}

//GESTION TABS
function showFormsFormEmplazamientos(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        //cmp.removeCls(classActivo);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    cambiarATapEmplazamiento(sender, index);
    /*sender.ariaEl.addCls(classActivo);

    
    var panels = document.getElementsByClassName("winGestion-paneles");
    for (let i = 0; i < panels.length; i++) {
        panels[i].hidden = true;
    }
    panels[index].hidden = false;*/
}

function btnPrevEmplazamiento(sender, registro, index) {
    var panelActual = getPanelActualEmplazamiento(sender, registro, index);
    cambiarATapEmplazamiento(sender, --panelActual);
}

function btnNextEmplazamiento(sender, registro, index) {
    var panelActual = getPanelActualEmplazamiento(sender, registro, index);
    cambiarATapEmplazamiento(sender, ++panelActual);

}

function getPanelActualEmplazamiento(sender, registro, index) {
    var idForm = sender.id.split("_");
    idForm.pop();
    idForm = idForm.join("_");

    var panelActual;
    var panels = document.getElementsByClassName("winGestion-paneles");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
            //panelActual = Ext.getCmp(panels[i].id);
        }
    }
    return panelActual;
}

function cambiarATapEmplazamiento(sender, index) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";

    var idForm;

    if (sender.id != undefined) {
        idForm = sender.id.split("_");
        idForm.pop();
        idForm = idForm.join("_");
    } else {
        idForm = sender;
    }

    var arrayBotones = Ext.getCmp(idForm + "_cntNavVistasFormEmplazamiento").ariaEl.getFirstChild().getFirstChild().dom.children;


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
    }

    //botones prev y next
    if (index <= 0) {
        Ext.getCmp(idForm + "_btnPrevEmplazamiento").addCls(classBtnDesactivo);
        Ext.getCmp(idForm + "_btnPrevEmplazamiento").removeCls(classBtnActivo);

        Ext.getCmp(idForm + "_btnNextEmplazamiento").removeCls(classBtnDesactivo);
        Ext.getCmp(idForm + "_btnNextEmplazamiento").addCls(classBtnActivo);
    }
    else if (index >= arrayBotones.length - 1) {
        Ext.getCmp(idForm + "_btnNextEmplazamiento").addCls(classBtnDesactivo);
        Ext.getCmp(idForm + "_btnNextEmplazamiento").removeCls(classBtnActivo);

        Ext.getCmp(idForm + "_btnPrevEmplazamiento").removeCls(classBtnDesactivo);
        Ext.getCmp(idForm + "_btnPrevEmplazamiento").addCls(classBtnActivo);
    }
    else {
        Ext.getCmp(idForm + "_btnPrevEmplazamiento").addCls(classBtnActivo);
        Ext.getCmp(idForm + "_btnNextEmplazamiento").addCls(classBtnActivo);
        Ext.getCmp(idForm + "_btnNextEmplazamiento").removeCls(classBtnDesactivo);
        Ext.getCmp(idForm + "_btnPrevEmplazamiento").removeCls(classBtnDesactivo);
    }
    if (index == arrayBotones.length - 1) {
        TreeCore[idForm].PintarCategorias(false);
    }

}

function addlistenerValidacionFormEmplazamientos(sender, registro, index) {

    let ruta = sender.id.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    var formPanel = App[ruta + '_' + 'containerFormEmplazamiento'];

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('validitychange', FormEmplazamientosValido);
        }
    });

}

function FormEmplazamientosValido(sender, valido, aux, ruta) {

    try {
        if (ruta == undefined || !(ruta instanceof String)) {

            if (sender.owner != undefined) {
                ruta = sender.owner.id.split('_');
            }
            else {
                ruta = sender.id.split('_');
            }
            ruta = ruta[0] + '_' + ruta[1];
        }


        if (valido == true) {
            App[ruta + '_' + 'btnGuardarAgregarEditarEmplazamiento'].setDisabled(false);

            var formPanel = App[ruta + '_' + 'containerFormEmplazamiento'];

            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App[ruta + '_' + 'btnGuardarAgregarEditarEmplazamiento'].setDisabled(true);
                }
            });

        }
        else {
            App[ruta + '_' + 'btnGuardarAgregarEditarEmplazamiento'].setDisabled(true);
        }
    } catch (e) {

    }
}

// #region RESIZER

function winFormResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(600);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(720);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(570);
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

function winFormHeight(obj) {
    res = obj.height;
    var dv = document.querySelectorAll('div.formGris');
    for (i = 0; i < dv.length; i++) {
        var obj1 = Ext.getCmp(dv[i].id);
        obj1.height = res - 44;
        obj1.update();
    }
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

    var dv = document.querySelectorAll('div.winGestionEmp');
    for (i = 0; i < dv.height; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormHeight(obj);
    }

    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }
}

window.addEventListener('resize', resizeWinForm);

// #endregion

function RecargarComboEmplazamiento(sender, registro, index) {
    recargarCombos([sender]);
    ruta = getIdComponente(sender);
    GenerarCodigo(App[ruta + '_' + 'txtCodigo'], App[ruta + '_' + 'containerFormEmplazamiento'], pautasCodigo, App[ruta + '_' + 'hdCodigoEmplazamientoAutogenerado']);
}

function SeleccionarComboEmplazamiento(sender, registro, index) {
    sender.getTrigger(0).show();
    ruta = getIdComponente(sender);
    GenerarCodigo(App[ruta + '_' + 'txtCodigo'], App[ruta + '_' + 'containerFormEmplazamiento'], pautasCodigo, App[ruta + '_' + 'hdCodigoEmplazamientoAutogenerado']);
    FormEmplazamientosValido(sender, true, undefined, ruta);
}


