var Agregar = false;
var seleccionado;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);

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
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();
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

// INICIO GESTION 

function VaciarFormulario() {
    App.formGestion.getForm().reset();
}

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function AgregarEditar() {
    VaciarFormulario();

    var storesACargar = [App.storeColumnas, App.storeTipoDato];

    showLoadMask(App.grid, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                App.txtDescripcion.focus(false, 200);
                App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                Agregar = true;
                App.txtValor.hide();
                App.radValor.hide();
                App.numValor.hide();
                App.dateFecha.hide();
                App.cmbOperador.hide();
                App.winGestion.show();
            }
            load.hide();
        });
    });
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

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);
    var storesACargar = [App.storeColumnas, App.storeTipoDato];

    showLoadMask(App.grid, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.txtDescripcion.focus(false, 200);
                            App.storePrincipal.reload();
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    });
            }
            load.hide();
        });
    });
}

function RecargarColumnas() {
    recargarCombos([App.cmbColumnas]);
}

function SeleccionaColumnas() {
    App.cmbColumnas.getTrigger(0).show();
}

function RecargarTiposDatos() {
    recargarCombos([App.cmbTipoDato]);
}

function SeleccionaTiposDatos(combo, record, index) {
    App.cmbTipoDato.getTrigger(0).show();
    ActualizarOperador(combo, record, index);
}

function RecargarOperador() {
    recargarCombos([App.cmbOperador]);
}

function SeleccionaOperador() {
    App.cmbOperador.getTrigger(0).show();
}


function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        if (seleccionado.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        } else {
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
                title: jsEliminar + ' ' + jsTituloModulo,
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


function Defecto() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        if (!seleccionado.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
                });
        } else {
            ajaxDefecto('yes');
        }
    }
}

function ajaxDefecto(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefecto({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

function ActualizarOperador(combo, record, index) {
    if (record && record.length > 0) {

        var dato = record[0].data.TipoDato;
        if (dato == "Texto") {
            App.txtValor.show();
            App.radValor.hide();
            App.numValor.hide();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Numerico") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.show();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Entero") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.show();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "GeoPosicion") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.show();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Moneda") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.show();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Decimal") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.show();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Booleano") {
            App.txtValor.hide();
            App.radValor.show();
            App.numValor.hide();
            App.dateFecha.hide();
            App.cmbOperador.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);
            App.cmbOperador.clearValue();
        }
        else if (dato == "Fecha") {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.hide();
            App.dateFecha.show();
            App.storeOperador.removeAll();
            RellenarCombo(dato);

            App.cmbOperador.clearValue();

            App.cmbOperador.show();
        }
        else {
            App.txtValor.hide();
            App.radValor.hide();
            App.numValor.hide();
            App.dateFecha.hide();
            App.storeOperador.removeAll();
            App.cmbOperador.hide()
        }
    }
}

function Operadores(nombre, valor) {
    this.nombre = nombre;
    this.valor = valor;
}


function RellenarCombo(dato) {
    if (dato == "Texto") {

        App.hdOperadores.setValue("Igual;0-Distinto;3-Contiene;4-En la lista;5-No en la Lista;6");

    } else if (dato == "Numerico") {

        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2-Distinto;3-Mayor o igual;7-Menos o igual;8");

    } else if (dato == "Entero") {

        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2-Distinto;3-Mayor o igual;7-Menos o igual;8");

    } else if (dato == "GeoPosicion") {

        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2-Distinto;3-Mayor o igual;7-Menos o igual;8");

    } else if (dato == "Moneda") {

        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2-Distinto;3-Mayor o igual;7-Menos o igual;8");

    } else if (dato == "Decimal") {

        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2-Distinto;3-Mayor o igual;7-Menos o igual;8");

    } else if (dato == "Booleano") {

        App.hdOperadores.setValue("Igual;0");

    } else if (dato == "Fecha") {
        App.hdOperadores.setValue("Igual;0-Mayor;1-Menor;2");

    }
    App.storeOperador.reload()
}

// FIN GESTION 

// INICIO CLIENTES


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

// FIN CLIENTES