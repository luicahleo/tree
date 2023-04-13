// #region DIRECT METHOD
var Editando = "";


function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        if (sender.selected.items.length == 1) {
            parent.App.hdServicioPadreID.setValue(seleccionado.Code);
            parent.nombreGridServicios = App.gridServicios;
            parent.registroServicios = registro;

            App.hdServicioID.setValue(seleccionado.Code);
            App.btnEditar.enable();
            App.btnEliminar.enable();

            let registroSeleccionado = registro;
            let GridSeleccionado = App.gridServicios;
            //parent.cargarDatosPanelMoreInfoServicio(registroSeleccionado, GridSeleccionado);
            //parent.cargarDatosPanelLateral(seleccionado.CoreProductCatalogServicioID);
        }
        else {
            App.btnEditar.disable();
            App.btnEliminar.disable();
        }
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    //App.colUsuario.setHidden(true);
    App.colEntidad.setHidden(true);
}

function Refrescar() {
    App.storeCoreProductCatalogServicios.reload();
    DeseleccionarGrilla();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeCoreProductCatalogServicios.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function FormularioValido(sender, registro, index) {
    Editando = App.hdEditando.getValue();

    App.btnGuardar.setDisabled(false);

    if (Editando == "Editar") {
        Ext.each(App.containerForm.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && !c.hidden && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
                App.btnGuardar.setText(jsGuardar);
                App.btnGuardar.setIconCls("");
                App.btnGuardar.removeCls("btnDisableClick");
                App.btnGuardar.addCls("btnEnableClick");
                App.btnGuardar.removeCls("animation-text");
            }
        });
    }
    else {
        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");

        Ext.each(App.containerForm.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && !c.hidden && c.isFormField &&
                (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                    (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
            }
        });
    }
}

//function VaciarFormulario() {
//    App.containerForm.getForm().reset();

//    Ext.each(App.containerForm.body.query('*'), function (value) {
//        Ext.each(value, function (item) {
//            var c = Ext.getCmp(item.id);
//            if (c != undefined && c.isFormField) {
//                c.reset();

//                if (c.triggerWrap != undefined) {
//                    c.triggerWrap.removeCls("itemForm-novalid");
//                }

//                if (!c.allowBlank && c.xtype != "checkboxfield") {
//                    c.addListener("change", anadirClsNoValido, false);
//                    c.addListener("focusleave", anadirClsNoValido, false);

//                    c.removeCls("ico-exclamacion-10px-red");
//                    c.addCls("ico-exclamacion-10px-grey");
//                }

//                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
//                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
//                }
//            }
//        });
//    });
//}

function VaciarFormulario() {
    Ext.each(App.containerForm.body.query('*'), function (item) {
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
                c.addListener('validitychange', FormularioValido);
                c.addListener('change', FormularioValido);

                c.addListener("change", cambiarLiteral, false);

                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }
            else if (c.allowBlank) {
                c.addListener("focus", cambiarBoton, true);
            }
        }
    });

    App.hdEditando.setValue('');
}

function cambiarLiteral(sender, registro, index) {
    if (App.btnGuardar != undefined) {
        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");
    }
    else {
        App.btnGuardar.setText(jsGuardado);
        App.btnGuardar.addCls("btnDisableClick");
        App.btnGuardar.removeCls("btnEnableClick");
        App.btnGuardar.setIconCls("ico-tic-wh");
    }
}

function cambiarBoton(sender, registro, index) {
    App.btnGuardar.setText(jsGuardar);
    App.btnGuardar.setIconCls("");
    App.btnGuardar.removeCls("btnDisableClick");
    App.btnGuardar.addCls("btnEnableClick");
    App.btnGuardar.removeCls("animation-text");
}


function AgregarEditar(sender, registro, index) {
    VaciarFormulario();
    App.winGestion.setTitle(jsAgregar);
    Agregar = true;
    //cambiarATap(undefined, 0);
    App.winGestion.show();

    App.storeEntidades.reload();
    App.storeCoreProductCatalogServiciosTipos.reload();
    //App.storeCoreFrecuencias.reload();
    //App.storeCoreUnidades.reload();

    App.cmbEntidades.getTrigger(0).hide();
    App.cmbTipos.getTrigger(0).hide();
    App.formServicio.show();

    //App.lnkFormLink.setDisabled(true);
    //App.lnkFormPrecios.setDisabled(true);
    //App.lnkFormTask.setDisabled(true);
}

function ajaxAgregarEditar(sender, panelActual) {

    showLoadMask(App.winGestion, function (load) {
        TreeCore.AgregarEditar(Agregar,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        load.hide();
                        //cambiarATap(sender, panelActual++);
                        Refrescar();

                        //App.lnkFormLink.setDisabled(false);
                        //App.lnkFormPrecios.setDisabled(false);
                        //App.lnkFormTask.setDisabled(false);

                        //App.btnNext.setDisabled(false);
                        //App.btnPrev.setDisabled(false);
                    }
                }
            });
    });

}

function MostrarEditar() {
    if (registroSeleccionado(App.gridServicios) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;

    App.cmbEntidades.getTrigger(0).show();
    App.cmbTipos.getTrigger(0).show();

    showLoadMask(App.gridServicios, function (load) {
        {
            if (load) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {

                                App.storeEntidades.reload();
                                App.storeCoreProductCatalogServiciosTipos.reload();
                                //App.storeCoreFrecuencias.reload();
                                //App.storeCoreUnidades.reload();

                                App.formServicio.show();
                                //App.formLink.hide();
                                //App.formPrecios.hide();
                                //App.formTask.hide();

                                //App.btnNext.setDisabled(false);
                                //App.btnPrev.setDisabled(false);

                                //cambiarATap(undefined, 0);
                                App.winGestion.setTitle(jsEditar);
                                App.winGestion.show();
                            }

                            load.hide();
                        }
                    });
            }
        }

    });
}

function Eliminar() {
    if (registroSeleccionado(App.gridServicios) && seleccionado != null) {
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
                    Refrescar();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
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

                //var msg = '';
                //listaIDs = [];
                //lista = JSON.parse(result.Result);

                //lista.map(x => x.Estado).forEach(i => {
                //    if (listaIDs == "") {
                //        listaIDs.push(i);
                //    }
                //    else if (!listaIDs.includes(i)) {
                //        listaIDs.push(i);
                //    }
                //});

                //listaIDs.forEach(y => {

                //    if (y == 'Resumen') {
                //        msg += '<p/><p><span style="font-weight:bold; font-size:large;">' + y + '</span>';
                //    }
                //    else {
                //        msg += '<p/><p><span style="font-weight:bold; font-size:large;">' + jsEstado + ' - ' + y + '</span>';
                //    }

                //    lista.forEach(p => {
                //        if (p.Estado == y) {
                //            if (msg != '') {
                //                msg += '<br/><br>';
                //            }

                //            if (p.Correcto) {
                //                icono = '<span class="ico-defaultGrid">&nbsp;</span>';
                //            }
                //            else {
                //                icono = '<span class="gen_Inactivo">&nbsp;</span>';
                //            }

                //            msg += icono + '  ' + p.Mensaje + ' (' + p.Valor + ')';
                //        }
                //    });

                //    msg += '<br/><br>';
                //});

                //msg = Ext.Msg.show({ title: jsEstado, icon: Ext.MessageBox.INFO, msg: msg, minWidth: 800 });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

// #endregion

// #region WINDOWS

function showFormsServicios(sender, idComponente, index) {
    var panelActual = getPanelActual(sender, idComponente, index);
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
    //    cambiarATap(sender, index);
    //}
}

//function btnPrev(sender, registro, index) {
//    var panelActual = getPanelActual(sender, registro, index);
//    cambiarATap(sender, --panelActual);
//}

//function btnNext(sender, registro, index) {
//    var panelActual = getPanelActual(sender, registro, index);

//    if (panelActual == 0) {
//        ajaxAgregarEditar(sender, ++panelActual);
//    }
//    else if (panelActual == 3) {
//        App.winGestion.hide();
//    }
//    else {
//        cambiarATap(sender, ++panelActual);
//    }
//}

function getPanelActual(sender, registro, index) {
    var panelActual;
    var panels = App.containerForm.ariaEl.dom.getElementsByClassName("winGestion-panel");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
        }
    }
    return panelActual;
}

//function cambiarATap(sender, index) {
//    var classActivo = "navActivo";
//    var classBtnActivo = "btn-ppal-winForm";
//    var classBtnDesactivo = "btn-secondary-winForm";

//    var idForm = sender;

//    var arrayBotones = Ext.getCmp("cntNavVistasForm").ariaEl.getFirstChild().getFirstChild().dom.children;


//    if (index >= 0 && index < arrayBotones.length) {
//        for (let i = 0; i < arrayBotones.length; i++) {
//            let cmp = Ext.getCmp(arrayBotones[i].id);
//            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
//            cmp.removeCls(classActivo);
//            if (index == i) {
//                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
//            }
//        }

//        var panels = App.containerForm.ariaEl.dom.getElementsByClassName("winGestion-panel");
//        for (let i = 0; i < panels.length; i++) {
//            Ext.getCmp(panels[i].id).hide();
//        }

//        Ext.getCmp(panels[index].id).show();
//    }

//    //botones prev y next
//    if (index <= 0) {
//        App.btnPrev.addCls(classBtnDesactivo);
//        App.btnPrev.removeCls(classBtnActivo);
//        App.btnNext.setText(jsGuardar);
//        App.btnNext.removeCls(classBtnDesactivo);
//        App.btnNext.addCls(classBtnActivo);
//    }
//    else if (index >= arrayBotones.length - 1) {
//        App.btnNext.setText(jsGuardar);
//        App.btnNext.setDisabled(false);
//        App.btnPrev.removeCls(classBtnDesactivo);
//        App.btnPrev.addCls(classBtnActivo);
//        App.btnPrev.setDisabled(false);
//    }
//    else {
//        App.btnPrev.addCls(classBtnActivo);
//        App.btnNext.addCls(classBtnActivo);
//        App.btnNext.setText(jsSiguiente);
//        App.btnNext.setDisabled(false);
//        App.btnNext.removeCls(classBtnDesactivo);
//        App.btnPrev.removeCls(classBtnDesactivo);
//        App.btnPrev.setDisabled(false);
//    }

//}

//function pulsoRadio(sender, registro, index) {
//    if (registro.FrecuenciasUnidades == 1) {
//        App.cmbFrecuencias.enable();
//        App.cmbFrecuencias.allowBlank = false;

//        App.cmbUnidades.disable();
//        App.cmbUnidades.reset();
//        App.cmbUnidades.allowBlank = true;
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-grey");
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-red");
//        App.cmbUnidades.triggerWrap.removeCls("itemForm-novalid");
//        App.cmbUnidades.removeListener("change", FormularioValido, false);

//        App.cmbFrecuencias.addListener("change", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("focusleave", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("change", FormularioValido, false);

//        App.cmbFrecuencias.addCls("ico-exclamacion-10px-grey");
//    }
//    else if (registro.FrecuenciasUnidades == 2) {
//        App.cmbUnidades.enable();
//        App.cmbUnidades.allowBlank = false;

//        App.cmbFrecuencias.reset();
//        App.cmbFrecuencias.disable();
//        App.cmbFrecuencias.allowBlank = true;
//        App.cmbFrecuencias.removeCls("ico-exclamacion-10px-grey");
//        App.cmbFrecuencias.removeCls("ico-exclamacion-10px-red");
//        App.cmbFrecuencias.triggerWrap.removeCls("itemForm-novalid");
//        App.cmbFrecuencias.removeListener("change", FormularioValido, false);

//        App.cmbUnidades.addListener("change", anadirClsNoValido, false);
//        App.cmbUnidades.addListener("focusleave", anadirClsNoValido, false);
//        App.cmbUnidades.addListener("change", FormularioValido, false);

//        App.cmbUnidades.addCls("ico-exclamacion-10px-grey");
//    }
//}

// #endregion

// #region COMBOS

function RecargarEntidad() {
    recargarCombos([App.cmbEntidades]);
}

function SeleccionarEntidad() {
    App.cmbEntidades.getTrigger(0).show();
}

function RecargarTipo() {
    recargarCombos([App.cmbTipos]);
}

function SeleccionarTipo() {
    App.cmbTipos.getTrigger(0).show();
}

//function RecargarFrecuencia() {
//    recargarCombos([App.cmbFrecuencias]);
//    FormularioValido('', false);
//}

//function SeleccionarFrecuencia() {
//    App.cmbFrecuencias.getTrigger(0).show();
//    FormularioValido('', true);
//}

//function RecargarUnidad() {
//    recargarCombos([App.cmbUnidades]);
//    FormularioValido('', false);
//}

//function SeleccionarUnidad() {
//    App.cmbUnidades.getTrigger(0).show();
//    FormularioValido('', true);
//}

// #endregion

// #region DISEÑO



// #endregion