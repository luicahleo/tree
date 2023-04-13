var TabN = 1;
var rutaEditar = "conProgramador";


function MostrarEditar(sender, registro, index) {

    //rutaEditar = getIdComponente(sender)

    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsServiciosFrecuencias);

    App[rutaEditar + '_' + 'cmbFrecuencia'].hide();
    App[rutaEditar + '_' + 'cmbFrecuencia'].allowBlank = true;
    document.getElementById(rutaEditar + '_' + 'cmbFrecuencia').parentNode.hidden = false;

    App[rutaEditar + '_' + 'txtFechaInicio'].show();
    App[rutaEditar + '_' + 'txtFechaInicio'].allowBlank = false;
    document.getElementById(rutaEditar + '_' + 'txtFechaInicio').parentNode.hidden = false;

    App[rutaEditar + '_' + 'txtFechaFin'].show();
    App[rutaEditar + '_' + 'txtFechaFin'].allowBlank = false;
    document.getElementById(rutaEditar + '_' + 'txtFechaFin').parentNode.hidden = false;

    App[rutaEditar + '_' + 'txtCronFormat'].show();
    App[rutaEditar + '_' + 'txtCronFormat'].allowBlank = false;
    App[rutaEditar + '_' + 'txtCronFormat'].disable();
    document.getElementById(rutaEditar + '_' + 'txtCronFormat').parentNode.hidden = false;

    App[rutaEditar + '_' + 'storeServiciosFrecuenciasTipos'].reload();

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App[rutaEditar + '_' + 'txtNombre'].focus(false, 200);
                App.storePrincipal.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}


function AgregarEditar() {
    VaciarFormulario();

    App[rutaEditar + '_' + 'cmbFrecuencia'].show();
    App[rutaEditar + '_' + 'cmbFrecuencia'].allowBlank = false;
    document.getElementById(rutaEditar + '_' + 'cmbFrecuencia').parentNode.hidden = false;

    App[rutaEditar + '_' + 'txtCronFormat'].enable();

    App.winGestion.setTitle(jsAgregar + ' ' + jsServiciosFrecuencias);
    Agregar = true;
    App.winGestion.show();
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

    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
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

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function VaciarFormulario() {

    

    Ext.each(App.winGestion.body.query('*'), function (value) {
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

                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }

            if (item.id.includes("conProgramador_") && item.id.includes("_Container")
                && !item.id.includes("conProgramador_txtNombre") && !item.id.includes("conProgramador_cmbTipo_")
                && !item.id.includes("conProgramador_cmbFrecuencia") && !item.id.includes("conProgramador_hd")) {

                document.getElementById(item.id).hidden = true;
            }
            App.formGestion.updateLayout();
        });
    });
}

function Refrescar() {
    //forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);

    }
}

function DeseleccionarGrilla() {
    if (App.hdCliID.value == 0 || App.hdCliID.value == undefined) {
        App.btnAnadir.disable();
    } else {
        App.btnAnadir.enable();
    }
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

//FIN GESTION GRID 


function RecargarComboTipo() {
    App.cmbTipo.clearValue();
}


function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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

function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsServiciosFrecuencias,
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



function SeleccionarTipo() {

}



