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
    SetMaxHeightSuperior(App.GridPanelCategorias, true);
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

        if (document.getElementById('pnConfigurador_Content') != undefined) {
            document.getElementById('pnConfigurador_Content').style.display = 'none';
        }
        if (seleccionado.EsSubcategoria) {
            showLoadMaskCategorias(function (load) {
                AnadirScriptjs('../../Componentes/js/CategoriasAtributos.js');
                AnadirScriptjs('../../Componentes/js/Atributos.js');
                App.hdCatSelect.setValue(seleccionado.InventarioAtributoCategoriaID);
                App.hdListaCategorias.setValue('');
                TreeCore.SeleccionarCategoriaPlantilla(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            DragDropInventario();
                            load.hide();
                        }
                    });
            });
        }
    }
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
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
    App.txtInventarioCategoria.focus(false, 200);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    Agregar = true;
    App.btnSeccion.enable();
    App.btnSubcategoria.enable();
    App.btnSubcategoriaPlantilla.enable();
    SeleccionarTipoSeccion(App.btnSeccion);
    App.winGestion.show();
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
    if (registroSeleccionado(App.GridPanelCategorias) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.btnSeccion.disable();
                App.btnSubcategoria.disable();
                App.btnSubcategoriaPlantilla.disable();
                App.txtInventarioCategoria.focus(false, 200);
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Activar() {
    if (registroSeleccionado(App.GridPanelCategorias) && seleccionado != null) {
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
    if (registroSeleccionado(App.GridPanelCategorias) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarSubcategorias,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarSubcategorias(button) {
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
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
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

function CambiarRestriccionDefecto(sender) {
    if (sender != App.btnRestriccionActive) {
        App.btnRestriccionActive.enable();
    }
    if (sender != App.btnRestriccionDisabled) {
        App.btnRestriccionDisabled.enable();
    }
    if (sender != App.btnRestriccionHidden) {
        App.btnRestriccionHidden.enable();
    }
    sender.disable();
    TreeCore.CambiarRestriccionDefecto(sender.modo);
}

function TipoSubcategoriaRender(value, test1, test2, test3) {
    if (!test2.data.EsSubcategoria) {
        return '<div class="tooltipSubcategorias">' +
            '<span class="ico-seccion">&nbsp;</span>' +
            '<span class="tooltiptextSubcategorias">' + jsSubcategoria + '</span >' +
            '</div >';
    } else if (!test2.data.EsPlantilla) {
        return '<div class="tooltipSubcategorias">' +
            '<span class="ico-subcategoria">&nbsp;</span>' +
            '<span class="tooltiptextSubcategorias">' + jsSubcategoriaAtributos + '</span >' +
            '</div >';
    } else {
        return '<div class="tooltipSubcategorias">' +
            '<span class="ico-subcategoriaplantilla">&nbsp;</span>' +
            '<span class="tooltiptextSubcategorias">' + jsSubcategoriaPlantillas + '</span >' +
            '</div >';
    }
}

function SeleccionarTipoSeccion(sender) {
    if (sender != App.btnSeccion) {
        App.btnSeccion.setPressed(false);
    } else {
        App.btnSeccion.setPressed(true);
    }
    if (sender != App.btnSubcategoria) {
        App.btnSubcategoria.setPressed(false);
    }
    if (sender != App.btnSubcategoriaPlantilla) {
        App.btnSubcategoriaPlantilla.setPressed(false);
    }
}

function TooltipGrid(toolTip, grid) {
    var view = grid.getView(),
        record = view.getRecord(toolTip.triggerElement),
        data;
    if (!record.data.EsSubcategoria) {
        data = jsSubcategoria;
    } else if (!record.data.EsPlantilla) {
        data = jsSubcategoriaAtributos;
    } else {
        data = jsSubcategoriaPlantillas;
    }

    toolTip.update(data);
}