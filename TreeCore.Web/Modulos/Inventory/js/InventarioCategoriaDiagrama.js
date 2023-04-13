//TREE Management
var seleccionado;
var jsTituloModulo = "Inventario";
var seleccionadoPlanPreventivo;
var seleccionadoDT;

//#region GESTION GRID



function RecargarTipoEmplazamientos() {
    //cmbTipoEmplazamientos.
  
    //InicioGrafico();
    ajaxCreateXML();
    InitGraphAfter();
   
 
}

function SeleccionarTipoEmplazamientos(sender) {
    App.cmbTipoEmplazamientos.getTrigger(0).show();
  
    //InicioGrafico();
    
    ajaxCreateXML();
    InitGraphAfter();
   
}

function ClearTipoEmplazamientos(sender) {
    App.cmbTipoEmplazamientos.setValue (0);

    //InicioGrafico();

    ajaxCreateXML();
    InitGraphAfter();

}
function createPopupMenu(graph, menu, cell, evt) {
    if (cell != null) {
        menu.addItem('Cell Item', 'editors/images/image.gif', function () {
            mxUtils.alert('MenuItem1');
        });
    }
    else {
        menu.addItem('No-Cell Item', 'editors/images/image.gif', function () {
            mxUtils.alert('MenuItem2');
        });
    }

};


function SeleccionarTipoVinculacion(sender) {
    btnToggle.pressed = false;
    App.btnToggle1.show();
    App.btnToggle2.show();

    //InicioGrafico();

    ajaxCreateXML();
    InitGraphAfter();

}

function ajaxCreateXML() {
    Sites.CreatesXML(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                   
                }
                InitGraphAfter();

            },
            eventMask:
            {
                showMask: true,
                msg: parent.jsMensajeProcesando
            }
        });
}

function SeleccionarEmplazamientoTipo() {
    //CargarArbol();
    storeCategoriasPrincipal.reload();
}

function InicioGrafico()
{
    
    var sFichero = hdFichero.Value;
   
        //if (sFichero != null && sFichero != "" && !sFichero.Equals("undefined")) {
            var graph = editorUI.editor.graph;
            var req = mxUtils.load(sFichero);
            var root = req.getDocumentElement();
            var dec = new mxCodec(root);
            dec.decode(root, graph.getModel());
            graph.getModel().endUpdate();
       // }
        
        
   
    
   
}

function InitGraphAfter() {
    // Extends EditorUi to update I/O action states based on availability of backend
    ajaxCreateXML();
 
    (function () {
        var editorUiInit = EditorUi.prototype.init;

        EditorUi.prototype.init = function () {
            editorUiInit.apply(this, arguments);
            this.actions.get('export').setEnabled(false);

            //previous repeated code collapsed for brevity 

            this.editor.setFilename('doc1.xml');

            //save editorUi object
            var editorUI = this;

            // Shows the given graph if exits
            // Workaround because window['mxGraphModel'] is not defined
            window['mxEditor'] = mxEditor;
            window['mxGeometry'] = mxGeometry;
            window['mxDefaultKeyHandler'] = mxDefaultKeyHandler;
            window['mxDefaultPopupMenu'] = mxDefaultPopupMenu;
            window['mxGraph'] = mxGraph;
            window['mxCell'] = mxCell;
            window['mxCellPath'] = mxCellPath;
            window['mxGraph'] = mxGraph;
            window['mxStylesheet'] = mxStylesheet;
            window['mxDefaultToolbar'] = mxDefaultToolbar;
            window['mxGraphModel'] = mxGraphModel;
            //InitGraph();
            var graph = editorUI.editor.graph;
            graph.getModel().beginUpdate();
            try {
                var sFichero = hdFichero.value;
                if (sFichero != null && sFichero != "" && sFichero != 'undefined') {

                    var req = mxUtils.load(sFichero);
                    var root = req.getDocumentElement();
                    var dec = new mxCodec(root.OwnerDocument);
                    dec.decode(root, graph.getModel());
                    //let layout = new mxFastOrganicLayout(graph);mxCompactTreeLayout
                    let layout = new mxCompactTreeLayout(graph);
                    layout.orientation = mxConstants.DIRECTION_SOUTH;
                    layout.execute(graph.getDefaultParent());
                    graph.fit();

                }
            } finally {
                graph.getModel().endUpdate();
            }

            // Right click menu
            graph.popupMenuHandler.factoryMethod = function (menu, cell, evt) {
                if (cell.edge) {
                    menu.addItem('First edge option', null, function () {
                        alert('This is the first option of edge ');
                    })
                    menu.addItem('Second edge option', null, function () {
                        alert('This is the second option of edge ');
                    })
                }
                if (cell.vertex) {
                    menu.addItem('First vertex option', null, function () {
                        alert('This is the first option of vertex ');
                    })
                    menu.addItem('Second vertex option', null, function () {
                        alert('This is the second option of vertex ');
                    })
                }
            }

            //this part shal be inserted
            //override EditorUi.saveFile function for customization
            this.save = saveXml;
            function saveXml() {

                if (editorUI.editor.graph.isEditing()) {
                    editorUI.editor.graph.stopEditing();
                }

                var xml = mxUtils.getXml(editorUI.editor.getGraphXml());
                //xml = encodeURIComponent(xml);

                if (xml.length < MAX_REQUEST_SIZE) {
                    //$.ajax({
                    //    type: "POST",
                    //    url: "home/save",
                    //    processData: false,
                    //    contentType: "application/json; charset=utf-8",
                    //    data: JSON.stringify({ 'xml': xml }),
                    //    success: function (response) {
                    //        //alert(response.message);
                    //    },
                    //    error: function (ex) {
                    //        alert(ex.message);
                    //    }
                    //});
                    SaveDiagram(xml);
                }
                else {
                    mxUtils.alert(mxResources.get('drawingTooLarge'));
                    mxUtils.popup(xml);

                    return;
                }

            };

            // Updates action states which require a backend
            if (!Editor.useLocalStorage) {
                mxUtils.post(OPEN_URL, '', mxUtils.bind(this, function (req) {
                    var enabled = req.getStatus() != 404;
                    this.actions.get('open').setEnabled(enabled || Graph.fileSupport);
                    this.actions.get('import').setEnabled(enabled || Graph.fileSupport);
                    this.actions.get('save').setEnabled(true);
                    this.actions.get('saveAs').setEnabled(enabled);
                    this.actions.get('export').setEnabled(enabled);
                }));
            }
        };

        // Adds required resources (disables loading of fallback properties, this can only
        // be used if we know that all keys are defined in the language specific file)
        mxResources.loadDefaultBundle = false;
        var bundle = mxResources.getDefaultBundle(RESOURCE_BASE, mxLanguage) ||
            mxResources.getSpecialBundle(RESOURCE_BASE, mxLanguage);

        // Fixes possible asynchronous requests
        mxUtils.getAll([bundle, STYLE_PATH + '/default.xml'], function (xhr) {
            // Adds bundle text to resources
            mxResources.parse(xhr[0].getText());

            // Configures the default graph theme
            var themes = new Object();
            themes[Graph.prototype.defaultThemeName] = xhr[1].getDocumentElement();

            // Main
            new EditorUi(new Editor(urlParams['chrome'] == '0', themes));
        }, function () {
            document.body.innerHTML = '<center style="margin-top:10%;">Error loading resource files. Please check browser console.</center>';
        });
    })();
}


function BotonExportarInventarioEmplazamiento() {
    Sites.ExportInventarioEmplazamiento({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}


//GUARDAR SOLICITUD

function BotonGuardarSolicitud() {

    ajaxGuardarSolicitud();
}

function ajaxGuardarSolicitud() {
    Sites.GuardarSolicitud({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: jsSolicitudGuardadaCorrectamente, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

// FIN GUARDAR SOLICITUD


//ENVIAR SOLICITUD



//ELIMINAR INVENTARIO HIJO

function BotonEliminarElementoHijo() {
    if (registroSeleccionado(gridElemHijosInventario) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsEliminar + jsTituloModulo,
                msg: jsEliminarMsg + ' ' + jsTituloModulo + ' ?',
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarHijo,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarHijo(button) {
    if (button == 'yes' || button == 'si') {
        Sites.EliminarElemHijo({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

//FIN ELIMINAR INVENTARIO

function filasColorBySolicitud(record, index, rowParams, ds) {
    var datos = record.data;

    if (datos.TowerCustomerEmplazamientoID == hdTowerCustomerEmplazamientoID.value) {
        rowParams.tstyle += "weight: bold; color:#2A3EBF;"
    }
    else {
        rowParams.tstyle += "weight: bold; color:#000000;"
    }
}


//CANCELAR SOLICITUD

function BotonCancelarSolicitud() {

    Ext.Msg.show(
        {
            title: jsEliminar + ' Request',
            msg: jsEliminarMsg + ' ' + 'request?',
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarSolicitud,
            icon: Ext.MessageBox.QUESTION
        });

}

function ajaxEliminarSolicitud(button) {
    if (button == 'yes' || button == 'si') {

        Ext.net.Mask.show();

        Sites.EliminarSolicitud({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: 'The request was canceled', buttons: Ext.Msg.OK });
                    parent.winSolicitarPresupuestoInventario.hide();
                    parent.parent.winSeguimiento.hide();
                    Ext.net.Mask.hide();

                }
            }
        });
    }
}


// #region Mantenimiento

function BotonAbrirPlanMantenimiento() {

    Ext.net.Mask.show();
    DeseleccionarPlanPreventivo();

    storeMantenimientosPreventivosInventario.load({
        callback: function (r, options, success) {
            if (success === true) {
                storeActividadesPreventivas.load({
                    callback: function (r, options, success) {
                        if (success === true) {

                            storeInventarioPreventivo.load({
                                callback: function (r, options, success) {
                                    if (success === true) {
                                        winMantenimientosPreventivos.show();
                                        Ext.net.Mask.hide();
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    });

}

function GridRowSelectPlanPreventivo_RowSelect(sender, index, registro) {

    var datos = registro.data;
    if (datos != null) {

        seleccionadoPlanPreventivo = datos;

        hdMantenimientoEmplazamientoPadreID.setValue(datos.MantenimientoEmplazamientoID);
        toolAgregarPlanMantenimientoInventario.disable();
        toolEditarPlanMantenimientoInventario.enable();

        storeActividadesPreventivas.load({
            callback: function (r, options, success) {
                if (success === true) {

                    storeInventarioPreventivo.load({
                        callback: function (r, options, success) {
                            if (success === true) {
                            }
                        }
                    });
                }
            }
        });
    }
}

function DeseleccionarPlanPreventivo() {
    GridRowSelectPlanPreventivo.clearSelections();
    hdMantenimientoEmplazamientoPadreID.setValue('');

    toolAgregarPlanMantenimientoInventario.enable();
    toolEditarPlanMantenimientoInventario.disable();

}

function RecargarStoresInventario() {
    hdMantenimientoEmplazamientoPadreID.setValue('');
    storeActividadesPreventivas.reload();
    storeInventarioPreventivo.reload();
}


function BotonAgregarPlanMantenimientoInventario() {

    Ext.net.Mask.show();
    formPanelPlanPreventivo.getForm().reset();

    var stores = [storeProyectosMantenimientoPreventivo, storeMantenimientoAgencias, storeTipologiasMantenimiento, storeCadencias, storePreventivosInventarioCategoria];
    var loadedStores = 0;

    stores.forEach(function (storeCur, index, storearray) {

        storeCur.load({
            callback: function (r, options, success) {
                if (success === true) {
                    loadedStores = loadedStores + 1;
                    if (loadedStores == stores.length) {
                        Ext.net.Mask.hide();
                        winGestionPlanPreventivo.show();
                    }
                }
            }
        });
    });
}

function BotonEditarPlanMantenimientoInventario() {

    Ext.net.Mask.show();
    formPanelPlanPreventivo.getForm().reset();

    var stores = [storeProyectosMantenimientoPreventivo, storeMantenimientoAgencias, storeTipologiasMantenimiento, storeCadencias, storePreventivosInventarioCategoria];
    var loadedStores = 0;

    stores.forEach(function (storeCur, index, storearray) {

        storeCur.load({
            callback: function (r, options, success) {
                if (success === true) {
                    loadedStores = loadedStores + 1;
                    if (loadedStores == stores.length) {
                        Ext.net.Mask.hide();
                        winGestionPlanPreventivo.show();
                    }
                }
            }
        });
    });

}


var TriggerProyectos = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbProyectos.clearValue();
            cmbAgencias.clearValue();
            break;
        case 1:
            cmbProyectos.clearValue();
            storeProyectosMantenimientoPreventivo.reload();
            cmbAgencias.clearValue();
            storeMantenimientoAgencias.reload();
            break;
    }
}

function SeleccionarProyecto() {
    storeMantenimientoAgencias.reload();
}

var TriggerAgencia = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbAgencias.clearValue();
            break;
        case 1:
            cmbAgencias.clearValue();
            storeMantenimientoAgencias.reload();
            break;
    }
}

var TriggerCadencias = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbCadencias.clearValue();
            break;
        case 1:
            cmbCadencias.clearValue();
            storeCadencias.reload();
            break;
    }
}

var TriggerPreventivosInventarioCategoria = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbPreventivosInventarioCategoria.clearValue();
            break;
        case 1:
            cmbPreventivosInventarioCategoria.clearValue();
            storePreventivosInventarioCategoria.reload();
            break;
    }
}

var TriggerTipologia = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbTipologiasMantenimiento.clearValue();
            break;
        case 1:
            cmbTipologiasMantenimiento.clearValue();
            storeTipologiasMantenimiento.reload();
            break;
    }
}

function FormularioValidoPlanPreventivo(valid) {
    if (valid) {
        btnGuardarPlanPreventivo.setDisabled(false);
    }
    else {
        btnGuardarPlanPreventivo.setDisabled(true);
    }
}


function ChangedTipoRepeticiones() {
    repeticionPorFecha = RadioFecha.checked;

    hdRepPorFecha.setValue(repeticionPorFecha);

    if (repeticionPorFecha) {
        txtFechaInicioRepeticiones.show();
        txtFechaInicioRepeticiones.allowBlank = false;
        txtFechaFinRepeticiones.show();
        txtFechaFinRepeticiones.allowBlank = false;
        txtNumRepeticiones.hide();
        txtNumRepeticiones.allowBlank = true;
    }
    else {
        txtFechaInicioRepeticiones.show();
        txtFechaInicioRepeticiones.allowBlank = false;
        txtFechaFinRepeticiones.hide();
        txtFechaFinRepeticiones.allowBlank = true;
        txtNumRepeticiones.show();
        txtNumRepeticiones.allowBlank = false;
    }
}
function ChangedTipoExportacion() {
    exportacionporcategoria = RadioExportCat.checked;

    hdCat.setValue(exportacionporcategoria);

    if (exportacionporcategoria) {
        cmbCategoriasInv.show();
    }
    else {
        cmbCategoriasInv.hide();
    }
}

function winGestionBotonGuardarPlanPreventivo() {

    Ext.net.Mask.show();

    Sites.AgregarPlanMantenimientoPreventivo({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                winGestionPlanPreventivo.hide();
                storeMantenimientosPreventivosInventario.reload();
                Ext.net.Mask.hide();
            }
        }
    });

}

function FormularioValidoIncluirElementoInventarioPlanPreventivo(valid) {
    if (valid) {
        btnGuardarElementoInventarioPlanPreventivo.setDisabled(false);
    }
    else {
        btnGuardarElementoInventarioPlanPreventivo.setDisabled(true);
    }
}


function winGestionBotonGuardarElementoInventarioPlanPreventivo() {

    Ext.net.Mask.show();

    Sites.AgregarInventarioPlanMantenimientoPreventivo({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                winIncluirElementoInventarioPlanPreventivo.hide();
                storeInventarioPreventivo.reload();
                Ext.net.Mask.hide();
            }
        }
    });

}

var TriggerInventarioElementosNoAsignadosPlanPreventivo = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbInventarioElementosNoAsignadosPlanPreventivo.clearValue();
            break;
        case 1:
            cmbInventarioElementosNoAsignadosPlanPreventivo.clearValue();
            storeInventarioNoAsignadoPreventivo.reload();
            break;
    }
}

function BotonAgregarInventarioMantenimiento() {

    cmbInventarioElementosNoAsignadosPlanPreventivo.clearValue();
    storeInventarioNoAsignadoPreventivo.reload();
    winIncluirElementoInventarioPlanPreventivo.show();

}

// #endregion

function Exportacion() {
    FormPanel5.reset();
    winExportaciones.show();


}



var TriggerCategoriasInv = function (el, trigger, index) {
    switch (index) {
        case 0:
            cmbCategoriasInv.clearValue();
            break;
        case 1:
            storeCategorias.reload();
            break;
    }
}

function SeleccionarCategoriaInv() {



}
function Aceptar() {

    if (RadioExportCat.checked) {
        Sites.ExportarPorCategoria({
            success: function (result) {
                if (result.Result != null && result.Result != '') {

                } else {
                    // error
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

    } else {
        Sites.ExportarCamposFijos({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    //transmitExcelFile(result.Result)
                } else {
                    // error
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

    }
    var URL = "Inventario_v2.aspx?Documento=" + "";
    window.open(URL);
    winExportaciones.hide();
}
function transmitExcelFile(file) {

    Sites.TransmitCategorias(file);
}
function FormularioExportValido(valid) {
    if (valid) {
        btnAceptar.setDisabled(false);
    }
    else {
        btnAceptar.setDisabled(true);
    }
}

// Documentos de las categorias
function AbrirDocumentos() {
    var identificador = hd_ElementoID_Seleccionada.value;

    var valIdentificador = identificador.replace("CARPETA", "");

    parent.addTab(parent.tabPrincipal, jsDocumentacion + identificador, "/Inventario/pages/InventarioCategoriasDocumentos.aspx?CategoriaID=" + identificador, jsDocumentacion + ' ' + valIdentificador, "icon-pageattach", true)
}

function BotonDocumentosElemento() {
    var identificador = hdElementoPadreID.value;

    var template = hdPlantilla.value;

    parent.addTab(parent.tabPrincipal, jsDocumentacion + identificador, "/Inventario/pages/InventarioCategoriasDocumentos.aspx?ElementoID=" + identificador + "&Template=" + template, jsDocumentacion + ' ' + identificador, "icon-pageattach", true)
}

function AbrirDigitalTwin() {

    Sites.ShowDigitalTwin(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    winDigitalTwin.show();
                }
            }
        });
}

// Update DT
function UpdateDigitalTwin() {
    Ext.Msg.show(
        {
            title: jsUpdate,
            msg: jsUpsateDTMsg,
            buttons: Ext.Msg.YESNO,
            fn: ajaxUpdateDigitalTwin,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxUpdateDigitalTwin(button) {
    if (button == 'yes' || button == 'si') {

        Ext.net.Mask.show();

        Sites.UpdateDigitalTwin({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: jsDigitalTwinUpdated, buttons: Ext.Msg.OK });

                }
                storeDigitalTwin.reload();
                Ext.net.Mask.hide();
            }
        });
    }
}


// Update DT Pictures
function UpdateDigitalTwinPictures() {
    Ext.Msg.show(
        {
            title: jsUpdatePictures,
            msg: jsUpsateDTPicturesMsg,
            buttons: Ext.Msg.YESNO,
            fn: ajaxUpdateDTPictures,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxUpdateDTPictures(button) {
    if (button == 'yes' || button == 'si') {

        Ext.net.Mask.show();

        Sites.UpdateDigitalTwinPictures({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: jsDTPicturesUpdated, buttons: Ext.Msg.OK });
                    storeDigitalTwin.reload();
                    Ext.net.Mask.hide();

                }
            }
        });
    }
}

// Update DT 3D
function UpdateDigitalTwin3DModel() {
    Ext.Msg.show(
        {
            title: jsUpdate3D,
            msg: jsUpsateDT3DMsg,
            buttons: Ext.Msg.YESNO,
            fn: ajaxUpdateDT3D,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxUpdateDT3D(button) {
    if (button == 'yes' || button == 'si') {

        Ext.net.Mask.show();

        Sites.UpdateDigitalTwin3DModel({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: jsDT3DUpdated, buttons: Ext.Msg.OK });
                    storeDigitalTwin.reload();
                    Ext.net.Mask.hide();

                }
            }
        });
    }
}

function ShowDetails() {
    var identificador = hdDigitalTwinID.value;
    winDigitalTwin.hide();

    parent.addTab(parent.tabPrincipal, jsDigitalTwin + identificador, "/Inventario/pages/InventarioDigitalTwin.aspx?DigitalTwinID=" + identificador, jsDigitalTwin + ' ' + seleccionadoDT.Nombre, "icon-cameramagnify", true)
}


function GridRowSelectDigitalTwin_RowSelect(sender, index, registro) {

    var datos = registro.data;
    if (datos != null) {

        seleccionadoDT = datos;

        hdDigitalTwinID.setValue(datos.InventarioDigitalTwinID);
        toolDTDetails.enable();

    }
}

function DeseleccionarDigitalTwin() {

    hdDigitalTwinID.setValue('');
    toolDTDetails.disable();

}
function Grid_RowSelectCategorias(sender, index, registro) {

    var datos = registro.data;
    if (datos != null) {

        if (hdTowerCustomerEmplazamientoID.value != "" && node.id.startsWith("BLOCKED")) {

            btnAgregarElem.disable();
        }
        else {

            btnAgregarElem.enable();
        }

        toolDocumentos.enable();
        hd_ElementoID_Seleccionada.setValue(datos.InventarioCategoriaID);
        RecargarGrids();

    }
}

function DeseleccionarCategorias() {

    btnAgregarElem.disable();
    toolDocumentos.disable();
    hd_ElementoID_Seleccionada.setValue('');
    RecargarGrids();

}


function ExtensionImagenValida(Componente, newValue, oldValue) {

}

// Representación gráfica del inventario
function AbrirInventarioGrafico() {
    var identificador = hdEmplazamientoID.value;

    parent.addTab(parent.tabPrincipal, jsInventarioGrafico + identificador, "/Inventario/pages/InventarioGraficos.aspx?EmplazamientoID=" + identificador, jsInventarioGrafico + ' ' + identificador, "icon-chartorganisation", true)
}

function BotonVincularElementoHijo() {
    storeCategoriasVinculadas.reload();
    formVinculados.getForm().reset();
    winElementosVinculados.show();
}

//Combo Categorias vinculadas

function RecargarCategoriasVinculaciones() {
    DeleteCategoriasVinculaciones();
    storeCategoriasVinculadas.reload();
}

function DeleteCategoriasVinculaciones() {
    cmbCategoriasVinculadas.clearValue();
}

var TriggerCategoriasVinculaciones = function (el, trigger, index) {
    switch (index) {
        case 0:
            DeleteCategoriasVinculaciones();
            DeleteElementosVinculaciones();
            RecargarElementosVinculaciones();
            break;
        case 1:
            RecargarCategoriasVinculaciones();
            RecargarElementosVinculaciones();
            break;
    }
}

function SeleccionarCategoriasVinculaciones() {
    RecargarElementosVinculaciones();
}


