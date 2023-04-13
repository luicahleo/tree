var Agregar = false;
var seleccionado;

// #region DISEÑO

var bShowPrincipal = true;
var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 600;
    var tmn = App.ctSlider.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltrosYSliders.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        loadPanels();
    }
}

function loadPanels() {

    if (bShowOnlySecundary) {
        App.ctMain1.hide();
    }
    else {

        App.ctMain2.show();

        if (bShowPrincipal) {
            App.ctMain1.show();
        }
        else {
            App.ctMain1.hide();
        }

    }

}

function loadPanelByBtns(pressedBtn) {

    // CHECK FOR A PRESSED BTN
    if (pressedBtn != "") {
        if (pressedBtn == "Next") {
            iSelectedPanel++;
        }
        else {
            iSelectedPanel--;
        }
    }

    // CHECK FOR DISABLED BUTTONS
    if (iSelectedPanel == 1) {
        App.btnPrev.enable();
        App.btnNext.disable();
    }
    else {
        App.btnPrev.disable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    if (iSelectedPanel == 1) {
        App.ctMain1.hide();
        App.ctMain2.show();
    }

}

function showOnlySecundary() {

    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

function SetMaxHeightSuperior(sender, bool = false) {
    var tamPadre = sender.up().getHeight();

    if (App.tbFiltrosYSliders.hidden == false) {
        if (!bool) {
            sender.setMinHeight(tamPadre - 80);
            sender.setMaxHeight(tamPadre - 80);
            sender.updateLayout();
        }
        else {
            sender.setMinHeight(tamPadre);
            sender.setMaxHeight(tamPadre);
            sender.updateLayout();
        }
    }
    else {
        if (!bool) {
            sender.setMinHeight(tamPadre - 30);
            sender.setMaxHeight(tamPadre - 30);
            sender.updateLayout();
        }
        else {
            sender.setMinHeight(tamPadre);
            sender.setMaxHeight(tamPadre);
            sender.updateLayout();
        }
    }
}


// #endregion

function DragDropInventario() {
    DragDropCategorias();
    DragDropAtributosCategorias();
}

function resizeTwoPanels(sender) {
    SetMaxHeightSuperior(App.ctMain1);
    SetMaxHeightSuperior(App.ctMain2);
    SetMaxHeightSuperior(App.gridCustomFields, true);
    SetMaxHeightSuperior(App.pnConfigurador, true);
}

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnEditar.setTooltip(jsEditar);
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
        App.hdAtrID.setValue(datos.CustomFieldID);
        TreeCore.PintarAtributos(true, false);
        App.AtributosConfiguracion_GridFormatRule.hide();
        App.AtributosConfiguracion_GridAddRestriction.hide();
        App.AtributosConfiguracion_GridAddCondition.hide();
    }
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.gridCustomFields.updateSelection()
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.hdAtrID.setValue(0);
    if (document.getElementById('pnConfigurador_Content') != undefined) {
        document.getElementById('pnConfigurador_Content').style.display = 'none';
    }
    App.AtributosConfiguracion_GridFormatRule.hide();
    App.AtributosConfiguracion_GridAddRestriction.hide();
    App.AtributosConfiguracion_GridAddCondition.hide();
    //TreeCore.PintarAtributos(true, false);
}

//FIN GESTION GRID 

// INICIO DIRECT METHOD 

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
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    showLoadMask(App.winGestion, function (load) {
        recargarCombos([App.cmbTiposDatos], function Fin(fin) {
            if (fin) {
                Agregar = true;
                load.hide();
                App.cmbTiposDatos.enable();
                App.winGestion.show();
            }
        });
    });
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsTieneRegistros, buttons: Ext.Msg.OK });
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
                    if (!Agregar) {
                        App['ATR' + App.GridRowSelect.selected.items[0].data.CoreAtributoConfiguracionID + '_lbNombreAtr'].setValue(App.txtNombreCampo.value);
                    }
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
    if (registroSeleccionado(App.gridCustomFields) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    showLoadMask(App.winGestion, function (load) {
        recargarCombos([App.cmbTiposDatos], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            load.hide();
                            App.cmbTiposDatos.disable();
                            App.winGestion.show();
                        }
                    });
            }
        });
    });
}

function Activar() {
    if (registroSeleccionado(App.gridCustomFields) && seleccionado != null) {
        ajaxActivar();
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
    if (registroSeleccionado(App.gridCustomFields) && seleccionado != null) {
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
                    if (document.getElementById('pnConfigurador_Content') != undefined) {
                        document.getElementById('pnConfigurador_Content').style.display = 'none';
                    }
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Refrescar() {
    DeseleccionarGrilla();
    App.storePrincipal.reload();
}

function VerActivos() {
    if (App.colActivo.hidden) {
        App.colActivo.show();
    } else {
        App.colActivo.hide();
    }
    Refrescar();
}

// FIN DIRECT METHOD 

// INICIO CLIENTES

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

// FIN CLIENTES

function showLoadMaskCategorias(callback) {
    var myMask = new Ext.LoadMask({
        msg: App.jsCargando,
        target: App.ctSlider
    });

    myMask.show();
    callback(myMask, null);
}


// #region FILTROS

var dataGridCustomFields = [];

function CargarBuscadorPredictivoCustomFields() {
    dataGridCustomFields = [];
    App.storePrincipal.data.items.forEach(wFlow => {
        dataGridCustomFields.push({
            key: wFlow.data.Codigo.toLowerCase(),
            key2: wFlow.data.TipoDato.toLowerCase(),
            nombre: wFlow.data.Nombre,
            codigo: wFlow.data.TipoDato,
            id: wFlow.id
        });
    });

    dataGridCustomFields = dataGridCustomFields.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtSearchCustomFields";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataGridCustomFields, function (value) {
                    let value1 = value.key;
                    let value2 = value.key2;

                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {
                    var idEmplazamientoBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                    ContractID = idEmplazamientoBuscador;
                    filterCustomFields();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}

var ContractID = '';

function filterCustomFields() {
    var logic = App.storePrincipal,
        text = App.txtSearchCustomFields.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.txtSearchCustomFields.getTrigger(1).hide();
        App.txtSearchCustomFields.getTrigger(0).show();
        return;
    }
    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (ContractID != '') {
        var WorkFlowIDAux = ContractID;
        ContractID = '';
        logic.filterBy(function (node) {
            var valido = false;
            if (node.data.Seleccionado) {
                valido = true;
            }
            if (WorkFlowIDAux == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        App.txtSearchCustomFields.getTrigger(0).hide();
        App.txtSearchCustomFields.getTrigger(1).show();
        logic.filterBy(function (node) {
            var valido = false;
            if (re.test(node.data.Codigo)) {
                valido = true;
            }
            if (re.test(node.data.Nombre)) {
                valido = true;
            }
            return valido;
        });

    }
}

function ClearfilterCustomFields() {
    App.txtSearchCustomFields.reset();
    App.txtSearchCustomFields.getTrigger(1).hide();
    App.txtSearchCustomFields.getTrigger(0).show();
}

// #endregion