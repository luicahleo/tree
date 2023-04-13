// #region CONTROL NAVEGACION WIN product catalog

var TabN = 1;
var classActivo = "navActivo";
var classBtnActivo = "btn-ppal-winForm";
var classBtnDesactivo = "btn-secondary-winForm";



function btnPrev_Click(sender, registro, index) {
    var panelActual = getPanelActual(sender, registro, index);
    navegacionBotones(sender, --panelActual);
}

function btnNext_Click(sender, registro, index) {
    var panelActual = getPanelActual(sender, registro, index);
    navegacionBotones(sender, ++panelActual);
}

function navegacionBotones(sender, index) {
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
        App.ctUsuario.hide();
        App.ctIdioma.hide();
        App.ctEntidad.hide();
        App.ctMonedas.hide();

        switch (index) {
            case 0:
                App.ctUsuario.show();
                App.btnPrev.addCls(classBtnActivo);
                App.btnPrev.removeCls(classBtnDesactivo);
                App.btnNext.addCls(classBtnActivo);
                App.btnNext.removeCls(classBtnDesactivo);
                App.btnNext.show();
                App.btnAgregar.hide();
                break;
            case 1:

                TreeCore.EditarUsuario(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                            }
                            else {
                                App.ctIdioma.show();
                                App.btnPrev.addCls(classBtnActivo);
                                App.btnPrev.removeCls(classBtnDesactivo);
                                App.btnNext.addCls(classBtnActivo);
                                App.btnNext.removeCls(classBtnDesactivo);
                                App.btnNext.show();
                                App.btnAgregar.hide();

                            }
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    }
                );

                break;
            case 2:

                TreeCore.EditarLenguajeIdioma(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                            }
                            else {
                                App.ctEntidad.show();
                                App.btnPrev.addCls(classBtnActivo);
                                App.btnPrev.removeCls(classBtnDesactivo);
                                App.btnNext.addCls(classBtnActivo);
                                App.btnNext.removeCls(classBtnDesactivo);
                                App.btnNext.show();
                                App.btnAgregar.hide();

                            }
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    }
                );

                break;
            case 3:
                TreeCore.EditarEntidad(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                            }
                            else {
                                App.ctMonedas.show();
                                App.btnNext.hide();
                                App.btnNext.hide();
                                App.btnAgregar.show();
                            }
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    }
                );
                
                break;
            default:
                break;
        }
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

function winGestionGuardar() {
    TreeCore.EditarMonedas(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                }
                else {
                    parent.parent.App.winConfigInicial.hide();

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


function AnadirMoneda() {
    if (App.chkControlMoneda.checked) {
        App.cmbMonedas.disable();
        App.ctNuevaMoneda.show();
    } else {
        App.ctNuevaMoneda.hide();
        App.cmbMonedas.enable();
    }
}

function FormularioValidoUsuarios(valid) {

    if (valid == true && App.txtClave.value == App.txtClaveRepite.value) {
        if (ClaveIgual()) {
            App.btnNext.enable();
        }
        
    }
    else {
        App.btnNext.disable();
    }

}

function ClaveIgual() {
    if (App.txtClave.value == App.txtClaveRepite.value && App.txtClaveRepite.value != '') {
        App.btnNext.enable();
        return true;
    }
    else {
        App.btnNext.disable();
        return false;
    }
}
