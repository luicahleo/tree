
setTimeout(resizeWinForm, 200);
var Editando = ""

function CargarVentanaContactos(sender, idComponente, vaciar) {
    var ruta;

    if (sender.$widgetColumn != undefined) {
        ruta = getIdComponente(sender.$widgetColumn) + '_' + idComponente;
    } else {
        ruta = getIdComponente(sender) + '_' + idComponente;
    }

    if (ruta.startsWith('_')) {
        ruta = idComponente;
    }

    var formPanel = App[ruta + '_' + 'containerFormContacto'];

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('change', FormContactosValido);
        }
    });

    var classMandatory = "ico-formview-mandatory";

    // #region FORMCONTACTO
    var i = 0;
    Ext.each(App[ruta + '_formContacto'].query('*'), function (value) {
        var c = Ext.getCmp(value.id);

        if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
            if (!c.isValid()) {
                i++;
            }
        }
    });

    if (i == 0) {
        App[ruta + '_lnkContactos'].removeCls(classMandatory);
    }
    else {
        App[ruta + '_lnkContactos'].addCls(classMandatory);
    }

    // #endregion

    // #region FORMLOCATION
    var j = 0;
    if (!App[ruta + '_geoPunto_txtDireccion'].isValid()) {
        j++;
    }
    else if (!App[ruta + '_geoPunto_cmbMunicipioProvincia'].isValid()) {
        j++;
    }

    if (j == 0) {
        App[ruta + '_lnkDirContactos'].removeCls(classMandatory);
    }
    else {
        App[ruta + '_lnkDirContactos'].addCls(classMandatory);
    }

    // #endregion

    showLoadMask(App[ruta + "_containerFormContacto"], function (load) {
        CargarStoresSerie([App[ruta + "_storeTiposContactos"]], function Fin(fin) {
            if (fin) {
                App[ruta + "_geoPunto_toolPosicionar"].enable();
                App[ruta + "_containerFormContacto"].update();
                load.hide();
            }
        });
    });
}

function CargarVentanaByEmplazamiento(sender, idComponente, vaciar = true) {
    var ruta = getIdComponente(sender.$widgetColumn) + '_' + idComponente;
    showLoadMask(App[ruta + "_containerFormContacto"], function (load) {
        CargarStoresSerie([App[ruta + "_storeTiposContactos"]], function Fin(fin) {
            if (fin) {
                //RecargarCombosLocalizaciones(ruta + '_locGeografica', false);
                App[ruta + "_containerFormContacto"].update();
                load.hide();
            }
        })
    });
}


// VALIDACION
function addlistenerValidacion(sender, registro, index) {

    let ruta = sender.id.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    var formPanel = App[ruta + '_' + 'containerFormContacto'];

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('change', FormContactosValido);
        }
    });

}

function FormContactosValido(sender, valido, ruta) {

    try {
        if (ruta == undefined || !(ruta instanceof String)) {

            if (sender.owner != undefined) {
                ruta = sender.owner.id.split('_');
            }
            else {
                ruta = sender.id.split('_');
            }

            if (ruta[1].includes('txt') || ruta[1].includes('geo')) {
                ruta = ruta[0];
            }
            else {
                ruta = ruta[0] + '_' + ruta[1];
            }
        }

        //App[ruta + '_' + 'btnGuardarAgregarEditarContacto'].setDisabled(false);
        Editando = App[ruta + "_hdEditando"].getValue();

        var formPanel = App[ruta + '_' + 'containerFormContacto'];
        App[ruta + '_btnNextContacto'].setDisabled(false);

        if (Editando == "Editar") {
            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    //App[ruta + '_' + 'btnGuardarAgregarEditarContacto'].setDisabled(true);
                    App[ruta + '_btnNextContacto'].setDisabled(true);
                    App[ruta + '_btnNextContacto'].setText(jsGuardar);
                    App[ruta + '_btnNextContacto'].setIconCls("");
                    App[ruta + '_btnNextContacto'].removeCls("btnDisableClick");
                    App[ruta + '_btnNextContacto'].addCls("btnEnableClick");
                    App[ruta + '_btnNextContacto'].removeCls("animation-text");
                }
            });
        }
        else {
            App[ruta + '_btnNextContacto'].setText(jsGuardar);
            App[ruta + '_btnNextContacto'].setIconCls("");
            App[ruta + '_btnNextContacto'].removeCls("btnDisableClick");
            App[ruta + '_btnNextContacto'].addCls("btnEnableClick");
            App[ruta + '_btnNextContacto'].removeCls("animation-text");

            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField &&
                    (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                        (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    //App[ruta + '_' + 'btnGuardarAgregarEditarContacto'].setDisabled(true);
                    App[ruta + '_btnNextContacto'].setDisabled(true);
                }
            });
        }

        var classMandatory = "ico-formview-mandatory";

        // #region FORMCONTACTO
        var i = 0;
        Ext.each(App[ruta + '_formContacto'].query('*'), function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App[ruta + '_lnkContactos'].removeCls(classMandatory);
        }
        else {
            App[ruta + '_lnkContactos'].addCls(classMandatory);
        }

        // #endregion

        // #region FORMLOCATION
        var j = 0;
        if (!App[ruta + '_geoPunto_txtDireccion'].isValid()) {
            j++;
        }
        else if (!App[ruta + '_geoPunto_cmbMunicipioProvincia'].isValid()) {
            j++;
        }

        if (j == 0) {
            App[ruta + '_lnkDirContactos'].removeCls(classMandatory);
        }
        else {
            App[ruta + '_lnkDirContactos'].addCls(classMandatory);
        }

        // #endregion

    } catch (e) {

    }
}

// DIRECT METHOD

function VaciarFormularioContacto(ruta) {

    if (ruta == "UCGridEmplazamientosContactos") {
        ruta = "UCGridEmplazamientosContactos_formAgregarEditarContacto";
    }

    var formPanel = App[ruta + '_containerFormContacto'];

    Ext.each(formPanel.body.query('*'), function (item) {
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
                c.addListener('validitychange', FormContactosValido);

                c.addListener("change", cambiarLiteral, false);

                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }
            else if (c.allowBlank) {
                c.addListener("focus", cambiarBoton, true);
            }
        }
    });

    App[ruta + '_cmbTipoContacto'].getTrigger(0).hide();
    App[ruta + '_hdEmplazamientoID'].setValue('');
    App[ruta + '_hdContactoGlobalID'].setValue('');
    App[ruta + '_hdEntidadID'].setValue('');
    App[ruta + '_hdEditando'].setValue('');
}

function winGestionBotonGuardarContacto(sender, registro, index) {
    ruta = getIdComponente(sender);
    if (App[ruta + '_containerFormContacto'].isValid()) {
        var id = ruta.split('_')[0];

        if (id == "gridContactos") {
            ajaxAgregarEditarContactoGlobal(sender, registro, index, ruta);
        } else if (id == "formAgregarEditarContacto") {
            ajaxAgregarEditarContactoEntidad(sender, registro, index, ruta);
        }
        else {
            ajaxAgregarEditarContacto(sender, registro, index, ruta);
        }

    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarContacto(sender, registro, index, ruta) {

    TreeCore[ruta].AgregarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                    setTimeout(function () {
                        App[ruta + '_btnNextContacto'].setText(jsGuardado);
                        App[ruta + '_btnNextContacto'].setIconCls("ico-tic-wh");
                        App[ruta + '_btnNextContacto'].addCls("animation-text");
                        App[ruta + '_btnNextContacto'].addCls("btnDisableClick");
                        App[ruta + '_btnNextContacto'].removeCls("btnEnableClick");
                    }, 250);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ajaxAgregarEditarContactoEntidad(sender, registro, index, ruta) {

    TreeCore[ruta].AgregarEditarContactoEntidad(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                    App.storeContactosGlobalesEntidadesVinculadas.reload();

                    setTimeout(function () {
                        App[ruta + '_btnNextContacto'].setText(jsGuardado);
                        App[ruta + '_btnNextContacto'].setIconCls("ico-tic-wh");
                        App[ruta + '_btnNextContacto'].addCls("animation-text");
                        App[ruta + '_btnNextContacto'].addCls("btnDisableClick");
                        App[ruta + '_btnNextContacto'].removeCls("btnEnableClick");
                    }, 250);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ajaxAgregarEditarContactoGlobal(sender, registro, index, ruta) {

    TreeCore[ruta].AgregarEditarContacto(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                    setTimeout(function () {
                        App[ruta + '_btnNextContacto'].setText(jsGuardado);
                        App[ruta + '_btnNextContacto'].setIconCls("ico-tic-wh");
                        App[ruta + '_btnNextContacto'].addCls("animation-text");
                        App[ruta + '_btnNextContacto'].addCls("btnDisableClick");
                        App[ruta + '_btnNextContacto'].removeCls("btnEnableClick");
                    }, 250);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function guardarCambios(sender, registro, index) {
    ruta = getIdComponente(sender);
    value = getIdComponentePadre(sender);

    TreeCore[ruta].ComprobarContactoExiste(
        {
            success: function (result) {

                var id = ruta.split('_')[0];

                if (id == "gridContactos") {
                    ajaxAgregarEditarContactoGlobal(sender, registro, index, ruta);
                }
                else if (id == "formAgregarEditarContacto") {
                    ajaxAgregarEditarContactoEntidad(sender, registro, index, ruta);
                }
                else {
                    ajaxAgregarEditarContacto(sender, registro, index, ruta);
                }
            }
        });
}

function cerrarWindow(sender, registro, index) {
    var ventana = "winGestionContacto";
    ruta = getIdComponente(sender);
    
    value = getIdComponente(sender);
    forzarCargaBuscadorPredictivo = true;

    if (value == "gridContactos_formAgregarEditarContacto") {
        if (App[value + '_btnNextContacto'].getText() == jsGuardar && !App[value + '_btnNextContacto'].disabled) {
            Ext.Msg.alert(
                {
                    title: jsAtencion,
                    msg: jsMensajeCerrar,
                    buttons: Ext.Msg.YESNO,
                    fn: function (btn) {
                        cabecera = value.split('_')[0];

                        if (btn == 'yes' || btn == 'si') {
                            ajaxAgregarEditarContactoGlobal(sender, registro, index, value);
                        }

                        App[cabecera + "_storeContactosGlobalesEmplazamientos"].reload();
                        App[cabecera + '_winGestionContacto'].hide();
                        VaciarFormularioContacto(ruta, ventana);
                    }
                });
        }
        else {
            cabecera = value.split('_')[0];
            App[cabecera + "_storeContactosGlobalesEmplazamientos"].reload();
            App[cabecera + '_winGestionContacto'].hide();
            VaciarFormularioContacto(ruta, ventana);
        }
    }
    else if (value == "formAgregarEditarContacto") {
        if (App[value + '_btnNextContacto'].getText() == jsGuardar && !App[value + '_btnNextContacto'].disabled) {
            Ext.Msg.alert(
                {
                    msg: jsMensajeCerrar,
                    buttons: Ext.Msg.YESNO,
                    fn: function (btn) {
                        if (btn == 'yes' || btn == 'si') {
                            ajaxAgregarEditarContactoEntidad(sender, registro, index, value);
                        }

                        App.storeContactosGlobalesEntidadesVinculadas.reload();
                        App.winAgregarContacto.hide();
                        VaciarFormularioContacto(ruta, ventana);
                    }
                });
        }
        else {
            App.storeContactosGlobalesEntidadesVinculadas.reload();
            App.winAgregarContacto.hide();
            VaciarFormularioContacto(ruta, ventana);
        }
    }
    else {
        if (App[value + '_btnNextContacto'].getText() == jsGuardar && !App[value + '_btnNextContacto'].disabled) {
            Ext.Msg.alert(
                {
                    msg: jsMensajeCerrar,
                    buttons: Ext.Msg.YESNO,
                    fn: function (btn) {
                        if (btn == 'yes' || btn == 'si') {
                            ajaxAgregarEditarContacto(sender, registro, index, value);
                        }

                        App.storeContactosGlobalesEmplazamientosVinculados.reload();
                        App.winGestionContactoEmplazamiento.hide();
                        VaciarFormularioContacto(ruta, ventana);
                    }
                });
        }
        else {
            App.storeContactosGlobalesEmplazamientosVinculados.reload();
            App.winGestionContactoEmplazamiento.hide();
            VaciarFormularioContacto(ruta, ventana);
        }
    }
}

//CERRAR VENTANA
function closeWindowContacto(sender, registro, index) {
    var ruta = getIdComponente(sender);
    var id = ruta.split('_')[0];

    if (id == "UCGridEmplazamientos") {
        var ventana = "winGestionContactoEmplazamiento";
        VaciarFormularioContacto(ruta, ventana);
    }
    else if (id == "UCGridEmplazamientosContactos" || id == "gridContactos") {
        var ventana = "winGestionContacto";
        VaciarFormularioContacto(ruta, ventana);
        forzarCargaBuscadorPredictivo = true;
        App[id + "_storeContactosGlobalesEmplazamientos"].reload();
    }
}

function CerrarVentana(ruta, ventana) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";
    var panels = App[ruta + "_containerFormContacto"].ariaEl.dom.getElementsByClassName("winGestionContacto-panel");
    for (let i = 0; i < panels.length; i++) {
        Ext.getCmp(panels[i].id).hide();
    }
    Ext.getCmp(panels[0].id).show();
    Ext.getCmp(panels[0].id).up('panel').update();
    ruta = panels[0].id.split('_'); ruta.pop(); ruta = ruta.join('_');
    var arrayBotones = Ext.getCmp(ruta + "_cntNavVistasFormContacto").ariaEl.getFirstChild().getFirstChild().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
    }
    document.getElementById(arrayBotones[0].id).lastChild.classList.add(classActivo);
    Ext.getCmp(ruta + "_btnPrevContacto").addCls(classBtnDesactivo);
    Ext.getCmp(ruta + "_btnPrevContacto").removeCls(classBtnActivo);
    Ext.getCmp(ruta + "_btnNextContacto").removeCls(classBtnDesactivo);
    Ext.getCmp(ruta + "_btnNextContacto").addCls(classBtnActivo);
    //#endregion
    ruta = panels[0].id.split('_'); ruta.pop(); ruta = ruta.join('_');
    //TreeCore[ruta + '_' + 'locGeografica'].LimpiarCombos(true);
    App[ruta + '_containerFormContacto'].getForm().reset();
    //App[ruta + '_cmbTiposContactos'].getTrigger(0).hide();
    App[ruta + '_hdEmplazamientoID'].setValue('');
    App[ruta + '_hdEntidadID'].setValue('');
    App[ruta + '_hdContactoGlobalID'].setValue('');
    App[ruta + '_hdEditando'].setValue('');
    ventana.hide();
}

//GESTION TABS
function showFormsFormContactos(sender, idComponente, index) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    var ruta = getIdComponente(sender);
    cambiarATapContacto(sender, index);
}

function btnPrevContacto(sender, registro, index) {
    var panelActual = getPanelActualContacto(sender, registro, index);
    cambiarATapContacto(sender, --panelActual);
}

function btnNextContacto(sender, registro, index) {
    var panelActual = getPanelActualContacto(sender, registro, index);
    cambiarATapContacto(sender, ++panelActual);

}

function getPanelActualContacto(sender, registro, index) {
    var idForm = sender.id.split("_");
    idForm.pop();
    idForm = idForm.join("_");

    var panelActual;
    var panels = App[idForm + "_containerFormContacto"].ariaEl.dom.getElementsByClassName("winGestionContacto-panel");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
        }
    }
    return panelActual;
}

function cambiarATapContacto(sender, index) {
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

    var arrayBotones = Ext.getCmp(idForm + "_cntNavVistasFormContacto").ariaEl.getFirstChild().getFirstChild().dom.children;


    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = App[idForm + "_containerFormContacto"].ariaEl.dom.getElementsByClassName("winGestionContacto-panel");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();
        }
        Ext.getCmp(panels[index].id).show();
        Ext.getCmp(panels[index].id).up('panel').update();

        if (index == 1) {
            App[idForm + '_btnNextContacto'].addCls("btnDisableClick");
        }
    }

    if (index == 0) {

        if (App[idForm + '_txtNombre'].getValue() != "")
        {
            App[idForm + '_btnNextContacto'].setText(jsGuardar);
            App[idForm + '_btnNextContacto'].setIconCls("");
            App[idForm + '_btnNextContacto'].removeCls("btnDisableClick");
            App[idForm + '_btnNextContacto'].addCls("btnEnableClick");
            App[idForm + '_btnNextContacto'].removeCls("animation-text");
        }
    }

    App[idForm + '_btnNextContacto'].addCls(classBtnActivo);
    App[idForm + '_btnNextContacto'].removeCls(classBtnDesactivo);

}

//function FormContactosValido(sender, registro, index) {
//    var ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_')
//    if (registro) {
//        App[ruta + '_btnGuardarAgregarEditarContacto'].enable();
//    }
//    else {
//        App[ruta + '_btnGuardarAgregarEditarContacto'].disable();
//    }
//}

function SeleccionarCombo(sender, registro, index) {
    App[sender.id].getTrigger(0).show();
}

function RecargarComboTipos(sender, registro, index) {
    App[sender.id].getTrigger(0).hide();
    App[sender.id].clearValue();
    App[sender.config.store.storeId].reload();
}

function DefinirDatosContacto(ruta, dato) {
    App[ruta + "_hdTipoContacto"] = dato.ContactoTipoID;
    App[ruta + "_hdMunicipio"] = dato.MunicipioID;
}

function cambiarLiteral(sender, registro, index) {

    ruta = getIdComponente(sender);

    if (App[ruta + '_btnNextContacto'] != undefined && App[ruta + '_txtNombre'].getValue() != "") {
        if (sender.id.includes('geoPunto')) {
            value = getIdComponentePadre(sender);

            App[value + '_btnNextContacto'].setText(jsGuardar);
            App[value + '_btnNextContacto'].setIconCls("");
            App[value + '_btnNextContacto'].removeCls("btnDisableClick");
            App[value + '_btnNextContacto'].addCls("btnEnableClick");
            App[value + '_btnNextContacto'].removeCls("animation-text");
        }
        else {
            App[ruta + '_btnNextContacto'].setText(jsGuardar);
            App[ruta + '_btnNextContacto'].setIconCls("");
            App[ruta + '_btnNextContacto'].removeCls("btnDisableClick");
            App[ruta + '_btnNextContacto'].addCls("btnEnableClick");
            App[ruta + '_btnNextContacto'].removeCls("animation-text");
        }
    }
    else {
        if (sender.id.includes('geoPunto')) {
            value = getIdComponentePadre(sender);

            App[value + '_btnNextContacto'].setText(jsGuardado);
            App[value + '_btnNextContacto'].addCls("btnDisableClick");
            App[value + '_btnNextContacto'].removeCls("btnEnableClick");
            App[value + '_btnNextContacto'].setIconCls("ico-tic-wh");
        }
        else {
            App[ruta + '_btnNextContacto'].setText(jsGuardado);
            App[ruta + '_btnNextContacto'].addCls("btnDisableClick");
            App[ruta + '_btnNextContacto'].removeCls("btnEnableClick");
            App[ruta + '_btnNextContacto'].setIconCls("ico-tic-wh");
        }
    }
}

function cambiarBoton(sender, registro, index) {

    if (sender.id.includes('geoPunto')) {
        ruta = getIdComponentePadre(sender);
    }
    else {
        ruta = getIdComponente(sender);
    }

    App[ruta + '_btnNextContacto'].setText(jsGuardar);
    App[ruta + '_btnNextContacto'].setIconCls("");
    App[ruta + '_btnNextContacto'].removeCls("btnDisableClick");
    App[ruta + '_btnNextContacto'].addCls("btnEnableClick");
    App[ruta + '_btnNextContacto'].removeCls("animation-text");
}

function RowSelectAsideShowInfo() {

    App.pnAsideR.expand();

    App.btnCollapseAsRClosed.show();


    App.WrapGestionColumnas.hide();
    App.WrapFilterControls.hide();
    App.pnMoreInfo.show();
}

// #region FUNCION COLUMNAS DINAMICAS PARA GRID, INSERTAR COMO LISTENER ON RESIZE Y AFTER RENDER EN EL PROPIO GRID


// LAS COLUMNAS DE DICHOS GRIDS TIENEN QUE TENER EL ATRIBUTO MINWIDTH DEFININO EN TODAS LAS COLS

//LA COLMORE DEBE SER APROX MINW 90 Y MAXW90

//function GridColHandler(grid) {
//    // Con esta variable se controla si la columna more esta visible siempre o no
//    var ForceShowColmore = false;

//    if (grid != null) {
//        //Variables de entorno(no editar)
//        var gridW = grid.getWidth();
//        const colArray = grid.columns;
//        const colArrayNoColMore = grid.columns.slice(0);
//        var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
//        var AllcolsMinWTotal = 0;
//        var visiblecolsMinWTotal = 0;


//        // Se crea un array sin la columna More
//        if (LastCol.initialCls == "col-More") {
//            colArrayNoColMore.pop();
//        }

//        // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

//        colArrayNoColMore.forEach(function (colArrayNoColMore) {
//            if (colArrayNoColMore.hidden != true) {
//                visiblecolsMinWTotal = visiblecolsMinWTotal + colArrayNoColMore.minWidth;
//            }

//            AllcolsMinWTotal = AllcolsMinWTotal + colArrayNoColMore.minWidth;


//        });


//        //Controles de anchura y hide (aqui esta el tema)

//        for (let i = 0; i < 18 && visiblecolsMinWTotal <= gridW + 90; i++) {


//            var HiddenCols = colArrayNoColMore.filter(x => {
//                return x.hidden == true;
//            })


//            if (HiddenCols.length > 0) {
//                var FirstHiddenColIndex = HiddenCols[0].fullColumnIndex;
//                grid.columns[FirstHiddenColIndex].show();

//                //// Se Suma la anchuraminima del computo de las columnas visibles que Hay

//                var minWLastShownCol = HiddenCols[0].minWidth;
//                visiblecolsMinWTotal = visiblecolsMinWTotal + minWLastShownCol;
//            }


//        }


//        while (visiblecolsMinWTotal >= gridW - 100) {



//            var VisibleCols = colArrayNoColMore.filter(x => {
//                return x.hidden != true;
//            })



//            if (VisibleCols.length > 0) {



//                //Se oculta Ultima Columna de las VISIBLES
//                var LastVisibleColIndex = VisibleCols.length - 1;
//                grid.columns[LastVisibleColIndex].hide();

//                // Se resta la anchuraminima del computo de las columnas visibles que quedan
//                var minWLastCol = VisibleCols[VisibleCols.length - 1].minWidth
//                visiblecolsMinWTotal = visiblecolsMinWTotal - minWLastCol;

//            } else {
//                break
//            }


//        }



//        // #region AQUI SE ESCONDE LA COLUMNA MORE (debe ser la ultima) POR DEFECTO!
//        //Index colmore
//        var colMoreIndex = grid.columns.length - 1;



//        if (AllcolsMinWTotal < gridW - 70) {
//            grid.columns[colMoreIndex].hide();

//        } else if (visiblecolsMinWTotal <= gridW + 90) {
//            grid.columns[colMoreIndex].show();

//        }

//        if (ForceShowColmore == true) {

//            grid.columns[colMoreIndex].show();
//        }


//    //#endregion
//    }
//}

// #endregion
// #region RESIZER

function winFormResize(obj) {
    var res = window.innerWidth;
    var resH = window.innerHeight;

    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(600);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    // control altura
    if (resH > 824) {
        obj.setHeight(550);
    }

    if (resH < 600) {
        obj.setHeight(400);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;
    var resH = window.innerHeight;


    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(570);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }

    //Control altura

    if (resH > 824) {
        obj.setHeight(380);
    }

    if (resH < 600) {
        obj.setHeight(230);
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

}

window.addEventListener('resize', resizeWinForm);

// #endregion