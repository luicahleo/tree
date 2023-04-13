var VisorTreePClosed = false;
var esVersion = false;
var iconMenu = "ico-moverow-gr";
var iconClose = "ico-hide-menu";
var ModoVisor = "ModoVisor";
var ModoGrid = "ModoGrid";
var tooltipID = "toolTipdoDocumento";
var TextFilterBox = "TextFilter";
var allDocsSelected = [];
var inputTextShare;
var idToOpenVisorBySlug = null;
var documentosCortados = [];

// #region variables nueva version doc
var NuevaVDocumentoPadreID = 0;
var NuevaVversionDocAnterior = 0;
var NuevaVdocumentoEstado = 0;
// #endregion

function AsRclosed() {
    //Oculta panel lateral al inicio
    MostrarPanelLateral(App.btnCollapseAsRClosed);
}

function ColOverrideControl() {

    if (document.getElementById('CenterPanelMain') != null) {

        var res = document.getElementById('CenterPanelMain').offsetWidth;
        if (colOverride == "3cols") {

        }

        else if (colOverride == "2cols") {

            if (App.ctMain3 != null) {
                App.ctMain3.hide();
            }
            if (res < 576) {

                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }

            }
            else {

                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();

            }
        }

        else if (colOverride == "1cols") {
            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

            App.ctMain2.setHidden(true);
            App.ctMain3.setHidden(true);
        }
    }
    //App.MainVwP.center();

}

// #region newResponsiveControl

var colOverride = "1cols";
var colMode = "3colmode";
var isOnColmbl = 1;
var isOnColNormal = 1;

function moveCtSldr(btn) {

    var res = document.getElementById('CenterPanelMain').offsetWidth;
    var btnPrssd = btn.id;

    if (res < 576 && colMode == "1colmode") { // MODO 1 COLUMNA

        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && isOnColmbl == 1 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();

            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();
            isOnColmbl = 2;

            if (colOverride == "2cols") {
                App.btnNextSldr.disable();
                App.btnPrevSldr.enable();

            }
        }

        else if (btnPrssd == 'btnNextSldr' && isOnColmbl == 2 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();

            App.ctMain3.show();
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();
            isOnColmbl = 3;
        }

        if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 3 && colMode == "1colmode") {
            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();

            isOnColmbl = 2;
        }

        else if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 2 && colMode == "1colmode") {

            if (ctmain1ForcedHide != true) {
                App.ctMain1.show();
            }
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
            isOnColmbl = 1;

        }

    }

    else if (res <= 991 && res > 576 && colMode == "2colmode") { // MODO 2 COLS

        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && colMode == "2colmode") {

            App.ctMain3.show();
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();

        }

        if (btnPrssd == 'btnPrevSldr' && colMode == "2colmode") {
            App.ctMain2.show();
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
        }
    }

}

window.addEventListener('resize',

    function PassiveResizer() {

        var el = document.getElementById('CenterPanelMain');

        if (el != null) {
            var res = document.getElementById('CenterPanelMain').offsetWidth;
            if (res <= 576 && colMode != "1colmode") {

                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }
                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);
                    App.ctMain2.setHidden(true);
                }
                App.ctMain3.setHidden(true);

                App.btnPrevSldr.disable();
                App.btnNextSldr.enable();

                colMode = "1colmode";

            }

            else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {
                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }
                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);
                }

                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(true);

                colMode = "2colmode";

                isOnColmbl = 1;

                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

            }
            else if (res > 991 && colOverride == "3cols") {
                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();
                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);
                }

                if (ctmain1ForcedHide == true) {

                    App.btnNextSldr.hide();
                    App.btnPrevSldr.hide();
                }
                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(false);
                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

                colMode = "3colmode";

                isOnColmbl = 1;
                isOnColNormal = 1;

            }

        }

        if (ctmain1ForcedHide == true) {

            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

        }

        resizeGridInfo();
    }

);

function ActiveResizer() {

    var el = document.getElementById('CenterPanelMain');
    if (el != null) {
        var res = document.getElementById('CenterPanelMain').offsetWidth;
        if (res <= 576 && colMode != "1colmode") {
            if (ctmain1ForcedHide != true) {
                App.btnNextSldr.show();
                App.btnPrevSldr.show();
            }
            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(true);
            }

            App.ctMain3.setHidden(true);

            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();

            colMode = "1colmode";

        }

        else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();

            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);

            }

            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(true);

            colMode = "2colmode";

            isOnColmbl = 1;

            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

        }
        else if (res > 991 && colOverride == "3cols") {

            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();
            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);

            }

            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(false);
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

            colMode = "3colmode";

            isOnColmbl = 1;
            isOnColNormal = 1;

        }

        ColOverrideControl();
        App.pnComboGrdVisor.updateLayout();

        resizeGridInfo();

    }
}

function displayMenuDoc(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridInfo.hide();
    App.pnInfoVersiones.hide();
    App.pnMetaData.hide();
    App.pnQuickFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}

function resizeGridInfo() {
    App.pnGridInfo.maxHeight = document.getElementById('mnAsideR-body').clientHeight;
    App.pnGridInfo.minHeight = document.getElementById('mnAsideR-body').clientHeight;
    App.pnGridInfo.updateLayout();
    App.grAsR1.maxHeight = document.getElementById('pnGridInfo-body').clientHeight - document.getElementById('Label1').clientHeight - 110;
    App.grAsR1.minHeight = document.getElementById('pnGridInfo-body').clientHeight - document.getElementById('Label1').clientHeight - 110;
    App.grAsR1.updateLayout();
}

// #endregion

// #region CONTROL VISORMODE
function btnCloseShowVisorTreeP(sender, index) {
    if (!VisorTreePClosed) {
        //ColapsePnAsideR();
        App.ctMain1.hide();
        App.btnCloseShowVisorTreeP.setIconCls(iconMenu);
        FHideCtMain1();

        VisorTreePClosed = true;
    }
    else if (VisorTreePClosed) {
        //ColapsePnAsideR();
        App.ctMain1.show();
        App.btnCloseShowVisorTreeP.setIconCls(iconClose);

        FHideCtMain1();
        VisorTreePClosed = false;
    }
}

function VisorIconReset() {
    App.btnCloseShowVisorTreeP.setIconCls(iconClose);
    VisorTreePClosed = false;
}

var ctmain1ForcedHide = false;

function FHideCtMain1() {

    if (ctmain1ForcedHide == false) {
        ctmain1ForcedHide = true;
    }
    else {
        ctmain1ForcedHide = false;
    }
}

function VisorMode(sender, registro, index) {
    if (sender == ModoVisor || sender.id == "GridbtnvistaVisor") {
        //
        if (App.hdVisor.getValue() == "0") {
            if (App.GridRowSelect.selected.items.length > 0) {
                let seleccionado = App.GridRowSelect.selected.items[0].data;
                let permisoLectura = seleccionado.Lectura;

                if (seleccionado.EsDocumento) {
                    CargarDocumentoVisor(seleccionado.id, permisoLectura);
                }
            }
        }

        OcultaColumnas(function () {
            //recargarRutaActual();
        });

        colOverride = "2cols";
        App.ctMain2.show();
        ctmain1ForcedHide = false;

        App.hdVisor.setValue("1");
        App.GridbtnvistaVisor.hide();
    }
    else if (sender == ModoGrid || sender.id == "GridbtnvistaGrid") {

        colOverride = "1cols";
        App.ctMain2.hide();
        App.ctMain1.show();
        ctmain1ForcedHide = false;
        VisorIconReset();

        App.hdVisor.setValue("0");
        App.GridbtnvistaVisor.show();

        MuestraColumnas(function () {
            //recargarRutaActual();
        });
    }
}
function OcultaColumnas(callback) {
    App.grid.columns.forEach(function (col, index) {
        if (index > 1) {
            col.setHidden(true);
        }
    });
    callback();
}

function MuestraColumnas(callback) {
    for (let i = App.grid.columns.length - 1; i > 0; i--) {
        App.grid.columns[i].setHidden(false);
    }

    callback();
}

function grdInsideH() {
    let pH = App.pnComboGrdVisor.height;
    App.grdInsidePn.height = pH;
}

function ShowTreeGrd() {
    App.PanelVisorMain.show();
    App.CenterPanelMain.hide();

}

function HideTreeGrd() {
    App.PanelVisorMain.hide();
    App.CenterPanelMain.show();

}

// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)
function winFormCenterSimple(obj) {
    obj.center();
    obj.updateLayout();

}

function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();

}

function winFormResizeDockBot(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();
    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);
    obj.updateLayout();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();
    obj.updateLayout();

}

window.addEventListener('resize', function () {
    var dv = document.querySelectorAll('div.winForm-respSimple');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormCenterSimple(obj);
    }
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

    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }

    resizeGridInfo();

});

function winFormResizeAddNewDocument(obj) {

    var res = obj.width;
    let ele = document.getElementById('winAddDocument-innerCt');
    if (ele != null) {
        if (res <= 670) {

            ele.classList.add('grid1colAdv5Rows');

            App.winAddDocument.updateLayout();

        }
        else {
            ele.classList.remove('grid1colAdv5Rows');
            App.winAddDocument.updateLayout();
        }

    }
}

// #endregion

var DocID = 0;
var DocVerID = 0;
var Agregar = false;
var myDropzone = null;
let archivoPermitido = true;
var permisoLectura = false;
var spPnLite = 0;

function CheckMultiSelect_Grid_RowSelectArbol(sender, registro, index) {

    // TODO: tratar cuando no es hoja
    // OBJETIVO: Deshabilitar funcionalidad previsualizacion y panel derecho en multiselect, si previsualizacion abierta -> cambiar imagen; cerrar panel derecho en multiselect;

    var allDocsSelected = sender.selected.items;

    Grid_RowSelectArbol(sender, registro, index);

    if (allDocsSelected.length > 1) {

        // ARROW RIGHT PANEL BUTTON
        App.btnCollapseAsRClosed.disable();
        if (!App.pnAsideR.collapsed) {
            App.pnAsideR.collapse();
            document.getElementById('btnCollapseAsRClosed').style.transform = 'rotate(-180deg)';
            spPnLite = 1;
        }

        // PRE-VISOR DOC BUTTON
        App.GridbtnvistaGrid.disable();
        App.GridbtnvistaVisor.disable();

        // TOOLBAR VISOR BUTTONS
        App.btnInfoVisor.disable();
        App.btnHistoricoVisor.disable();
        App.btnDescargarVisor.disable();
        App.btnDesactivarVisor.disable();
    }
    else {

        // ARROW RIGHT PANEL BUTTON
        //document.getElementById('btnCollapseAsRClosed').style.transform = 'rotate(-180deg)';
        App.btnCollapseAsRClosed.enable();

        // PRE-VISOR DOC BUTTON
        App.GridbtnvistaGrid.enable();
        App.GridbtnvistaVisor.enable();

        // TOOLBAR VISOR BUTTONS
        App.btnInfoVisor.enable();
        App.btnHistoricoVisor.enable();
        App.btnDescargarVisor.enable();
        App.btnDesactivarVisor.enable();
    }
}

function CargarDocumentoVisor(DocumentoID, permisoLectura) {
    if (permisoLectura == undefined) {
        permisoLectura = false;
    }
    App.btnDescargarVisor.disable();

    TreeCore.CargarDocumentoVisor(DocumentoID, permisoLectura,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                }
            },
            eventMask: {
                showMask: true,
            }
        });

}

function QuitarDocumentoVisorMultiSelect() {

    TreeCore.QuitarDocumentoVisorMultiSelect(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: {
                showMask: true,
            }
        });
}


function DeseleccionarGrilla() {
    //App.btnAgregarDocumento.disable();
    App.btnEditarDocumento.disable();
    App.btnDesactivar.disable();
    App.btnInfoVisor.disable();
    App.btnHistoricoVisor.disable();
    App.btnDescargarVisor.disable();
    App.btnDesactivarVisor.disable();
    App.btnTbHeaderDescargar.disable();
    App.btnDescargar.disable();
}

function Grid_RowSelectVersiones(sender, registro, index) {
    var datos = registro.data;
    esVersion = true;

    DocVerID = registro.data.DocumentoID;
    if (DocVerID != 0 && DocVerID == DocID) {
        DocVerID = 0;
    }

    if (datos != null) {
        seleccionadoVersion = datos;
    }
}

function editarDocumento() {
    App.txtDescripcionDocumento.setValue("");
    Agregar = false;
    App.winAddDocumentUpload.setTitle(jsEditarDocumento);
    App.paneEmpty.hide();
    App.panelCenterShow.show();
    App.cmbTiposDocumentos.hide();
    //App.panelCenterShow.hide();
    //App.paneEmpty.show();
    App.winUploadFieldDocument.setTitle(jsEditar + ' ' + jsDocumento);
    App.winUploadFieldDocument.show();
    App.grid.disable();

    if (myDropzone == null) {
        inicializarDragAndDrop();
    }

}

function AgregarDocumento() {

    App.txtDescripcionDocumento.setValue("");
    recargarCombos([App.cmbTiposDocumentos]);
    App.hdTipoDocumentoID.setValue(0);
    App.storeTiposDocumentos.reload();
    Agregar = true;
    App.winAddDocumentUpload.setTitle(jsAgregarDocumento);
    App.grid.disable();
    App.cmbTiposDocumentos.show();
    //App.panelCenterShow.show();
    App.panelCenterShow.hide();
    App.paneEmpty.show();
    App.winUploadFieldDocument.setTitle(jsAgregar + ' ' + jsDocumentos);
    //App.hdCarpetaPadreID

    App.winUploadFieldDocument.show();

    if (myDropzone == null) {

        inicializarDragAndDrop();

    }


}

$(document).ready(function () {
    inicializarDragAndDrop();
});

function inicializarDragAndDrop() {

    if ($('#panelCenterShow').length) {
        myDropzone = new Dropzone("#panelCenterShow",
            {
                addRemoveLinks: false,
                autoProcessQueue: true,
                url: '/PaginasComunes/DocumentosVista.ashx',
                parallelUploads: 2,
                clickable: true,
                uploadMultiple: false,
                maxFilesize: 512,
                chunkSize: 2048,
                timeout: 100000,
                params: function () {

                    var vObjetoID;
                    if (App.hdObjetoID.value != '') {
                        vObjetoID = App.hdObjetoID.value;
                    }
                    else {
                        vObjetoID = App.hdEditadoObjetoID.value;
                    }

                    objeto = {
                        "UsuarioID": App.hdUsuarioID.value,
                        "ModuloID": App.hdModuloID.value,
                        "UsuarioID": App.hdUsuarioID.value,
                        "ModuloID": App.hdModuloID.value,
                        "DocumentoTipoID": App.hdTipoDocumentoID.value,
                        "ExtensionNoPermitida": jsExtensionNoPermitida,
                        "NumeroMaximoCaracteres": jsNumeroMaximoCaracteres,
                        "CodeLanguage": App.hdCulture.value,
                        "Agregar": Agregar,
                        "ObjetoID": vObjetoID,
                        "ObjetoTipo": App.hdObjetoTipo.value,
                        "DocumentoPadreID": NuevaVDocumentoPadreID,
                        "versionDocAnterior": NuevaVversionDocAnterior,
                        "documentoEstado": NuevaVdocumentoEstado,
                        "descripcionDoc": App.txtDescripcionDocumento.value
                    }

                    return objeto;
                },
                init: function () {
                    myDropzone = this;
                    archivoPermitido = true;
                    this.on("success", function (file, responseText) {
                        let response = JSON.parse(responseText);
                        App.txtDescripcionDocumento.setValue("");

                        updateDocumentToWinUploadField(file, response);
                        if (response.files == undefined || response.allError || response.files.filter(x => x.message != "").length > 0) {
                            App.ExitoLabel.setText(jsSubidaIncorrecta);
                            App.btnAceptarSubida.setText(jsCerrar);
                            App.btnAceptarSubida.enable();
                        }
                        else {
                            App.ExitoLabel.setText(jsSubidaExito);
                            App.btnAceptarSubida.setText(jsCerrar);
                            App.btnAceptarSubida.enable();
                        }

                        

                    });
                    this.on("sending", function (file, response, formData) {
                        App.ExitoLabel.setText(jsSubiendo);
                        App.btnAceptarSubida.setText(jsEspera);
                        App.btnAceptarSubida.disable();
                        App.winUploadFieldDocument.hide();

                        App.winAddDocumentUpload.show();
                        addDocumentToWinUploadField(file, response, formData);
                    });
                    this.on("uploadprogress", function (file, progress, bytesSent) {
                        updateProgressDocumentToWinUploadField(file, progress);
                    });
                },
                error: function (file, message) {
                    updateDocumentToWinUploadField(file, message);
                    //App.ExitoLabel.setText("Fracaso");
                    App.btnAceptarSubida.setText(jsCerrar);
                    App.btnAceptarSubida.enable();
                },
                dragover: function (event) {
                    let drop = $('#panelCenterShow');
                    drop.css('background-color', '#A8CFEB');
                    //$('.dropzone *').css('background', '#A8CFEB');
                },
                dragleave: function (event) {
                    let drop = $('#panelCenterShow');
                    drop.css('background-color', 'white');
                    //$('.dropzone *').css('background', 'white');
                },
                drop: function (event) {
                    let drop = $('#panelCenterShow');
                    drop.css('background-color', 'white');
                    //$('.dropzone *').css('background', 'white');
                },
                addedfile: function (file) {
                    myDropzone = this;
                    if (file.size <= 0) {
                        myDropzone.removeFile(file);
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDocumentoTamano0, buttons: Ext.Msg.OK });
                    }
                    //myDropzone.processQueue();
                },
                maxfilesreached: function (file) {
                    console.log("MAXFILESREACHED");
                },
            });
    }

    $('#panelCenterShow').on('drop', function (e, ui) {
        e.preventDefault();
        e.stopPropagation();
    });
}

function addDocumentToWinUploadField(file, response, formData) {
    let panelNuevoDocumento = new Ext.Panel({
        cls: "pnDocumentUpload " + file.upload.uuid,
        Hidden: "false",
        items: [
            {
                xtype: "netlabel",
                text: "",
                cls: "icon " + file.upload.uuid,
            },
            {
                xtype: "netlabel",
                text: "",
                cls: "progress " + file.upload.uuid,
            },
            {
                xtype: "netlabel",
                text: file.upload.filename,
                cls: "filename " + file.upload.uuid,
            },
            {
                xtype: "netlabel",
                text: "",
                cls: "message " + file.upload.uuid,
            }
        ]
    });

    App.pnProgresFiles.add(panelNuevoDocumento);
    updateProgressDocumentToWinUploadField(file, file.upload.progress);
}

function updateDocumentToWinUploadField(file, response) {
    let iconYes = `<span class="ico-checked-16-gr d-inBlk">&nbsp;</span>`;
    let iconNo = `<span class="ico-x-16-red d-inBlk">&nbsp;</span>`;
    let icon;

    //$(".progress." + file.upload.uuid + " span").text(file.upload.progress);
    $(".progress." + file.upload.uuid + " span").hide();


    let fichero = null;
    if (response.files != undefined) {
        fichero = response.files.filter(x => x.fileName == file.upload.filename && x.message != "");
    }

    let maxSizeFile = parseInt(App.hdMaxRequestLength.getValue()) * 1024;

    if (file.size > maxSizeFile) {
        $(".message." + file.upload.uuid + " span").html(`<b>${jsTamanoDocumentoExcedido}</b>`);
        icon = iconNo;
    }
    else if (fichero == null || response.allError) {
        $(".message." + file.upload.uuid + " span").html(`<b>${jsMensajeGenerico}</b>`);
        icon = iconNo;
    }
    else if (fichero.length > 0) {
        //Error
        icon = iconNo;

        let message = "";
        if (fichero.length > 0 && fichero[0].message != undefined && fichero[0].message != "") {
            message = fichero[0].message;
        }

        $(".message." + file.upload.uuid + " span").html(`<b>${message}</b>`);

    } else {
        //No error
        icon = iconYes;
    }
    $(".icon." + file.upload.uuid + " span").html(icon);
}

function btnAceptarSubida() {
    App.winAddDocumentUpload.hide();
    App.pnProgresFiles.removeAll();

    App.grid.enable();

    recargarRutaActual();
}

function HabilitarGrid() {

    App.grid.enable();

}

function updateProgressDocumentToWinUploadField(file, progress) {
    //$(".progress." + file.upload.uuid + " span").text(progress);
    console.log(file.upload.uuid, Math.trunc(progress));
}

function mostrarDocuVersion(docID) {
    if (document.getElementById('hdVisor').value != 1) {
        VisorMode(ModoVisor);
    }

    if (permisoLectura == undefined) {
        permisoLectura = false;
    }
    App.btnDescargarVisor.disable();

    TreeCore.CargarDocumentoVisor(docID, permisoLectura,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                }
                permisoLecturaVersion = false;
            },
            eventMask: {
                showMask: true,
            }
        });
}

var docVersionSeleccionado = 0;
function restaurarDocumento(sender, registro, index) {
    if (sender != null) {
        docVersionSeleccionado = sender.record.data.DocumentoID;
    }

    Ext.Msg.alert({
        title: jsAtencion,
        msg: jsMensajeRestaurar,
        buttons: Ext.Msg.YESNO,
        fn: confirmarRestaurarDocumento,
        icon: Ext.MessageBox.QUESTION
    });
}

function confirmarRestaurarDocumento(button) {
    if (button == "yes" || button == "si") {
        showLoadMask(App.MainVwP, function (load) {
            TreeCore.RestaurarDocumento(docVersionSeleccionado,
                {
                    success: function (result) {
                        load.hide();
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {

                            recargarRutaActual();

                            App.btnEditarDocumento.disable();
                            App.btnDesactivar.disable();
                            App.storeVersiones.reload();
                        }
                        docVersionSeleccionado = 0;
                    },
                    error: function () {
                        docVersionSeleccionado = 0;
                    }
                });
        });
    }
    else {
        docVersionSeleccionado = 0;
    }
}

function desactivarVersionDocumento(sender) {
    esVersion = true;
    desactivarDocumento(sender);
}

function desactivarDocumento(sender) {
    let activo = false;
    if (sender != undefined) {
        docVersionSeleccionado = sender.record.data.DocumentoID;
        activo = sender.record.data.Activo;
    }
    else {
        activo = App.GridRowSelect.selected.items[0].data.Activo;
    }

    if (activo) {
        Ext.Msg.alert({
            title: jsAtencion,
            msg: jsMensajeDesactivar,
            buttons: Ext.Msg.YESNO,
            fn: confirmarDesactivarDocumento,
            icon: Ext.MessageBox.QUESTION
        });
    }
    else {
        confirmarDesactivarDocumento("yes");
    }

}

function confirmarDesactivarDocumento(button) {
    if (button == "yes" || button == "si") {
        showLoadMask(App.MainVwP, function (load) {
            TreeCore.DesactivarDocumento(docVersionSeleccionado, esVersion,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {

                            recargarRutaActual();

                            App.btnEditarDocumento.disable();
                            App.btnDesactivar.disable();
                            VisorMode(ModoGrid);
                            //App.storeVersiones.reload();
                            //App.storeDocumentoLateral.reload();
                        }
                        docVersionSeleccionado = 0;
                        esVersion = false;
                        load.hide();
                    },
                    error: function () {
                        docVersionSeleccionado = 0;
                        esVersion = false;
                    }
                });
        });
    }
    else {
        docVersionSeleccionado = 0;
    }
}

function refreshStorePrincipal() {
    App.storePrincipal.clearFilter();
    App.TextFilter.setValue("");
    App.hdStringBuscador.setValue("");
    App.hdIdDocumentBuscador.setValue("");
    App.GridRowSelect.clearSelections();
    
    App.PagingToolBar.getStore().loadPage(1);
}

function BuscadorStorePrincipal() {
    App.storePrincipal.clearFilter();

    App.PagingToolBar.getStore().loadPage(1);
}

function HideBtnMore(sender, registro, index) {
    if (!index.data.EsDocumento) {
        registro.setHidden(true);
    }
}

function MostrarPanelLateralVersiones() {
    if (App.pnAsideR.collapsed) {
        //App.pnAsideR.animCollapse = true;
        ColapsePnAsideR();
    }
    displayMenuDoc('pnInfoVersiones');

    html = '';


    if (DocID != '') {
        CargarStoresSerie([App.storeDocumentoLateral], function (fin) {
            if (fin) {
                TreeCore.RecargaPanelLateral(DocID,
                    {
                        success: function (result) {
                            if (result.Success != null && !result.Success) {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                tablaBody = document.getElementById('bodyTablaInfoDocumentos');
                                tablaBody.innerHTML = '';
                                lista = result.Result;
                                for (var dato in lista) {
                                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato + '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                                }
                                tablaBody.innerHTML = html;


                            }
                        },
                        eventMask: {
                            showMask: true,
                        }
                    });
            }
        });
    }
}

function ColapsePnAsideR() {
    App.pnAsideR.toggleCollapse();

    let btn = document.getElementById('btnCollapseAsRClosed');
    if (spPnLite == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 0;
    }
};

function MostrarPanelLateral(sender, registro, index) {

    let docSeleccionadoID = '';

    if (sender != null) {
        if (sender.$widgetRecord != undefined && sender.$widgetRecord.data.id != 'root') {

            docSeleccionadoID = sender.$widgetRecord.data.id;

        } else if (App.GridRowSelect.selected.length > 0) {

            docSeleccionadoID = App.GridRowSelect.selected.items[0].data.id;
        }
    }
    else {
        docSeleccionadoID = DocID;
    }

    if (App.pnAsideR.collapsed || sender.id == "btnCollapseAsRClosed") {
        //App.pnAsideR.animCollapse = true;
        ColapsePnAsideR();
    }


    displayMenuDoc('pnGridInfo');

    if (docSeleccionadoID != '') {
        html = '';
        tablaBody = document.getElementById('bodyTablaInfoDocumentos');
        tablaBody.innerHTML = '';

        TreeCore.RecargaPanelLateral(docSeleccionadoID,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        tablaBody.innerHTML = '';
                        lista = result.Result;
                        for (var dato in lista) {
                            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato +
                                '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                        }
                        tablaBody.innerHTML = html;
                    }
                },
                eventMask: {
                    showMask: true,
                }
            });
        resizeGridInfo();
    }
}

function MostrarPanelLateralInfo(sender, registro, index) {

    let docSeleccionadoID = '';

    if (sender != null) {
        if (sender.$widgetRecord != undefined && sender.$widgetRecord.data.id != 'root') {

            docSeleccionadoID = sender.$widgetRecord.data.id;

        } else if (App.GridRowSelect.selected.length > 0) {

            docSeleccionadoID = App.GridRowSelect.selected.items[0].data.id;
        }
    }
    else {
        docSeleccionadoID = DocID;
    }

    if (App.pnAsideR.collapsed) {
        //App.pnAsideR.animCollapse = true;
        ColapsePnAsideR();
    }

    displayMenuDoc('pnGridInfo');

    if (docSeleccionadoID != '') {
        html = '';
        CargarStoresSerie([App.storeDocumentoLateral], function (fin) {
            if (fin) {
                TreeCore.RecargaPanelLateral(docSeleccionadoID,
                    {
                        success: function (result) {
                            if (result.Success != null && !result.Success) {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                tablaBody = document.getElementById('bodyTablaInfoDocumentos');
                                tablaBody.innerHTML = '';
                                lista = result.Result;
                                for (var dato in lista) {
                                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato + '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                                }
                                tablaBody.innerHTML = html;


                            }
                        },
                        eventMask: {
                            showMask: true,
                        }
                    });
            }
        });

        resizeGridInfo();
    }
}

function descargarDocumento() {
    
    var id = DocID;
    if (DocVerID != 0) {
        id = DocVerID;
    }

    TreeCore.DescargarDocumentoSeleccionado(id,
        {
            isUpload: true,
            error: function (a) {
                console.log(a)
            },
            success: function (result) {
                if (result.Success != null && result.Success == false) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                }
            }
        });
}

function descargarDocumentos() {
    if (allDocsSelected && allDocsSelected != null && allDocsSelected.length > 0) {
        let listaSeleccionados = [];
        let tamanoMaximo = hdTamanoMaximoDescarga.value;
        let tamanoTemp = 0;
        let DocumentoSinPermiso = false;

        allDocsSelected.forEach(elemDoc => {
            listaSeleccionados.push({
                id: elemDoc.data.id,
                DocumentoTipoID: elemDoc.data.DocumentoTipoID,
                EsCarpeta: elemDoc.data.EsCarpeta,
                EsDocumento: elemDoc.data.EsDocumento,
                Tamano: elemDoc.Tamano
            });
            if (elemDoc.Tamano) {
                tamanoTemp += elemDoc.Tamano
            }
            if (!elemDoc.data.Descarga) {
                DocumentoSinPermiso = true;
            }
        });


        if (tamanoTemp > tamanoMaximo) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsTamanoDocumentoExcedido, buttons: Ext.Msg.OK });
        }
        else
        {
            if (DocumentoSinPermiso) {

                Ext.Msg.alert({
                    title: jsAtencion,
                    msg: jsDocumentosSinPermisoDescarga,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDescargarDocumentosSeleccionados,
                    icon: Ext.MessageBox.QUESTION,
                    listaSeleccionados: listaSeleccionados
                });

            }
            else {
                ajaxDescargarDocumentosSeleccionados(listaSeleccionados);
            } 
        }
    }
}

function ajaxDescargarDocumentosSeleccionados(listaSeleccionados, sender, index) {
    let continuar = false;
    if (Array.isArray(listaSeleccionados)) {
        continuar = true;

    } else if (listaSeleccionados == "yes" || listaSeleccionados == "si") {
        continuar = true;
        listaSeleccionados = index.listaSeleccionados;
    }

    if (continuar) {
        let outPut = {
            elementos: listaSeleccionados
        }

        TreeCore.DescargarDocumentosSeleccionados(JSON.stringify(outPut),
            {
                isUpload: true,
                error: function (a) {
                    console.log(a)
                },
                success: function (result) {
                    if (result.Success != null && result.Success == false) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {

                    }
                }
            });   
    }
}

function tooltipPermisosTipoDocumentoLeave(sender, registro, index) {
    var tooltipPermisos = $(`#${tooltipID}`);
    tooltipPermisos.css("display", "none");

}

function tooltipPermisosTipoDocumento(sender, registro, index) {
    if (false && registro != null && registro.data != null &&
        registro.data.Lectura != undefined && registro.data.Escritura != undefined && registro.data.Descarga != undefined) {

        var tooltipPermisos = $(`#${tooltipID}`);

        var iconVisible = "ico-visible-16";
        var iconEdit = "ico-edit-16";
        var iconDownload = "ico-download-16";
        var cheked = "ico-checked-16-gr FloatR";
        var X_RED = "ico-x-16-red";

        var iconYes = ` <span class='${cheked}'>&nbsp;</span> `;
        var iconNo = ` <span class='${X_RED}'>&nbsp;</span> `;

        var tooltipContent = $(
            `<ul class='document-permits'>
                <li><span class='${iconVisible}'>&nbsp;</span> ${jsLectura} ${((registro.data.Lectura) ? iconYes : iconNo)}</li>
                <li><span class='${iconEdit}'>&nbsp;</span> ${jsEscritura} ${((registro.data.Escritura) ? iconYes : iconNo)}</li>
                <li><span class='${iconDownload}'>&nbsp;</span> ${jsDescarga} ${((registro.data.Descarga) ? iconYes : iconNo)} </li>
            </ul>`);


        tooltipPermisos.empty();
        tooltipPermisos.append(tooltipContent);
        tooltipPermisos.css("display", "block");

        window.onmousemove = function (e) {
            var x = e.clientX,
                y = e.clientY;

            tooltipPermisos.css("top", Math.trunc(y - 20 - tooltipPermisos.height()) + 'px');
            tooltipPermisos.css("left", (x + 20) + 'px');
        };
    }
}

// #endregion


function btnTgDocumentsActivos() {
    App.btnEditarDocumento.disable();
    App.btnDesactivar.disable();

    App.colDesactivado.setHidden(!App.btnTgDocumentsActivos.pressed);

    recargarRutaActual();

}

var renderInactiveDocuments = function (value) {

    if (value == true || value == 1) {
        return '<span>&nbsp;</span> ';
    }
    else {
        return '<span class="gen_Inactivo">&nbsp;</span>';
    }
};


// #region ContextMenuTreeL

function beforeShowContextMenu(sender, registro, index) {
    let mostrarMenu = false;
    App.Preview.setHidden(true);
    App.Download.setHidden(true);
    App.Versions.setHidden(true);
    App.CutFile.setHidden(true);
    App.PasteFile.setHidden(true);
    App.ShowContracts.setHidden(true);
    App.changeFileName.setHidden(true);
    App.btnCompartir.setHidden(true);
    App.mnCambiarEstadoMulti.setHidden(true);
    App.btnHistorico.setHidden(true);

    let items = App.GridRowSelect.selected.items;

    if (documentosCortados.length > 0) {
        //App.PasteFile.setHidden(false);
        App.menuPegarComo.loader.load();
    }

    if (items.length == 1 && items[0].data.EsDocumento) {
        App.changeFileName.setHidden(false);
        App.btnCompartir.setHidden(false);
        App.Download.setHidden(false);
        App.Versions.setHidden(false);
        App.Preview.setHidden(false);
        App.btnHistorico.setHidden(false);

        if (items[0].data.Escritura) {
            App.CutFile.setHidden(false);
        }

        mostrarMenu = true;
    }
    else if (items.length > 0) {
        let todosDocumentos = true;
        let todosCarpetas = true;
        let todosTiposDocumentos = true;

        items.forEach(function (item) {
            if (!item.data.EsDocumento) {
                todosDocumentos = false;
            }
            if (!item.data.EsCarpeta) {
                todosCarpetas = false;
            }
            if (item.data.DocumentoTipoID != undefined && item.data.EsCarpeta) {
                todosTiposDocumentos = false;
            }
        });

        if (todosDocumentos) {
            //mostrarMenu = true;
            App.CutFile.setHidden(false);
            App.Download.setHidden(false);
            App.mnCambiarEstadoMulti.setHidden(false);
        }
    }

    if (!mostrarMenu) {
        $(".x-menu-default").css("border-style", "none");
    }
    else {
        $(".x-menu-default").css("border-style", "solid");
    }
}

function changeMetadataDoc(sender, registro, index) {
    let seleccionado = App.GridRowSelect.selected.items[0].data;

    if (seleccionado.EsDocumento) {
        

        App.storeDocumentosEstados.reload();
            TreeCore.CargarEditarMetadatos(seleccionado.id, {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.winChangeNameFile.show();
                    }
                },
                eventMask: {
                    showMask: true,
                }
            });
        
    }
}

function winChangeNameFileBotonGuardar() {
    App.winChangeNameFile.hide();
    TreeCore.WinChangeNameFileBotonGuardar(App.GridRowSelect.selected.items[0].data.id, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            } else {
                TreeCore.RecargaPanelLateral(App.GridRowSelect.selected.items[0].data.id, {
                    success: function (result2) {
                        if (result2.Result != null && !result2.Result) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                        } else {
                            tablaBody = document.getElementById('bodyTablaInfoDocumentos');
                            tablaBody.innerHTML = '';
                            html = "";
                            lista = result2.Result;
                            for (var dato in lista) {
                                html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato + '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                            }
                            tablaBody.innerHTML = html;
                        }
                    }
                });
            }
            recargarRutaActual();
        },
        eventMask: {
            showMask: true,
        }
    });
}

function FormChangeNameFileValido() {
    if (App.txtNameFile.value != null && App.txtNameFile.value != "") {
        App.btnGuardarNameFile.setDisabled(false)
    }
    else {
        App.btnGuardarNameFile.setDisabled(true);
    }
}

function shareDocument() {
    let urlBase = window.location.protocol + "//" + window.location.host;
    let seleccionado = App.GridRowSelect.selected.items[0].data;
    if (seleccionado.EsDocumento) {
        let slug = seleccionado.slug;
        let tipoDocumento = seleccionado.DocumentoTipo;

        let textLabel = seleccionado.text;

        let urlDescarga = `${urlBase}/Doc/download/${slug}`;
        let urlDoc = `${urlBase}/Doc/show/${slug}`;

        App.lbElementName1.setText(textLabel);
        App.lbElementName2.setText(textLabel);
        App.txtUrl.setValue(urlDoc);
        App.txtUrlDescarga.setValue(urlDescarga);
        cambiarATapShareDoc(null, 0);
        App.winCompartirDocumento.show();
    }
}

function historyDocument() {
    let seleccionado = App.GridRowSelect.selected.items[0].data;

    if (seleccionado.EsDocumento) {
        var nameTab = seleccionado.Nombre;
        var nameObj = jsHistorico;
        var documentoID = seleccionado.DocumentoPadreID;
        if (documentoID == null) {
            documentoID = seleccionado.id;
        }

        var pagina = "/ModDocumental/pages/DocumentalHistoricos.aspx?DocumentoID=" + documentoID;
        parent.addTab(parent.App.tabPpal, nameObj + documentoID, nameObj + " " + nameTab, pagina);
    }
}

function menuPrevisualizar() {
    VisorMode(ModoVisor);
}

function menuDescargar() {
    descargarDocumentos();
}

function menuVersiones() {
    MostrarPanelLateralVersiones();
}

function menuCortar() {
    documentosCortados = [];
    let items = App.GridRowSelect.selected.items;

    items.forEach(item => {
        if (item.data.EsDocumento) {
            documentosCortados.push(item.data.id);
        }
    });

}

function pegarDocumentoComo(sender, registro, index) {
    showLoadMask(App.grid, function (load) {
        TreeCore.MoverDocumentos(documentosCortados, registro.id,
            {
                success: function (result) {
                    load.hide();
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        documentosCortados = [];
                        recargarRutaActual();
                    }
                },
            });
    });
}

function cambiarEstadoDocumento(sender, registro, index) {

    showLoadMask(App.grid, function (load) {
        let seleccionados = [];
        App.GridRowSelect.selected.items.forEach(item => {
            seleccionados.push(item.data.id);
        });

        TreeCore.CambiarEstadoMulti(seleccionados, registro.id,
            {
                success: function (result) {
                    load.hide();
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                    recargarRutaActual();
                },
            });
    });
}

// #endregion

function hideIfNotUltimaVersion(sender, registro, index) {
    if (index.data.UltimaVersion) {
        registro.setHidden(true);
    } else {
        registro.setHidden(false);
    }
}

function hideIfNotUltimaVersionDeactivate(sender, registro, index) {
    hideIfNotUltimaVersion(sender, registro, index);

    if (index.data.Activo) {
        registro.setTooltip(jsDesactivar);
    }
    else {
        registro.setTooltip(jsActivar);
    }
}

function nameDocumentHistorical() {
    $(function () {

        $(".nameDocumentHistorical").tooltip({
            show: null,
            position: {
                my: "left top",
                at: "left bottom"
            },
            open: function (event, ui) {
                ui.tooltip.animate({ top: ui.tooltip.position().top + 10 }, "fast");
            }
        });

    });
}

function renderTamanoHistorico(sender, registro, index) {
    let hist = index.data;

    return "<div class=\"customCol1\" onClick=\"mostrarDocuVersion(" + hist.DocumentoID+");\">"+
        "<p class=\"TopTitleCustomCol1 nameDocumentHistorical\" title=\"" + hist.Documento + "\">" + hist.Documento +"</p>"+
        "<div class=\"customColDiv1\">"+
            hist.Fecha +
        "</div>"+
        "<div class=\"customColDiv1\">"+
            hist.Creador +
        "</div>"+
        "<div class=\"customColDiv1\">v"+
        hist.Version + " | " + formatBytes(hist.Tamano) +
        "</div>"+
    "</div>";


}

function BeforeDropNodo(node, data, overModel, dropPosition, dropHandlers) {

    var targetNodeID = data.records[0].getId();
    var destinationNodeID = overModel.getId();

    dropHandlers.wait = true;

    let records = [];
    data.records.forEach(function (record) {
        records.push(record.data);
    });


    let targetsJson = JSON.stringify({ records: records });
    let destinationTypeElement = JSON.stringify(overModel.data);

    showLoadMask(App.grid, function (load) {
        TreeCore.BeforeDropNodo(targetsJson, destinationNodeID, destinationTypeElement,
            {
                success: function (result) {
                    load.hide();
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
                        //dropHandlers.processDrop();
                    }
                    else {
                        //dropHandlers.cancelDrop();
                    }
                },
            });
    });
    
}

var rendererTamano = function (value, col, data) {

    if (data.data && data.data.Tamano) {
        return formatBytes(data.data.Tamano);
    }
    else {
        return '';
    }
};

// INIT Compartir 
function docNotFound() {
    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDocumentoNoEncontrado, buttons: Ext.Msg.OK });
}

function showTabsShare(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    cambiarATapShareDoc(sender, index);
    
}

function cambiarATapShareDoc(sender, index) {
    var classActivo = "navActivo";

    var arrayBotones = Ext.getCmp("cntNavVistasShare").ariaEl.getFirstChild().getFirstChild().dom.children;

    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = document.getElementsByClassName("winCompartirDocumento-paneles");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();

            if (index == i) {
                let inputText = $($(panels[i]).find(".urlcopy")[0]).find("input[type='text']");
                inputText.select();
                inputTextShare = inputText.val();
            }
        }
        Ext.getCmp(panels[index].id).show();
        Ext.getCmp(panels[index].id).up('panel').update();
    }
}

function winCompartirCopiarUrl() {
    
    var el = document.createElement('textarea');
    el.value = inputTextShare;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);

    App.winCompartirDocumento.hide();
}


// #region 
function docNotFound() {
    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDocumentoNoEncontrado, buttons: Ext.Msg.OK });
}

function docNotPermitDownload() {
    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDocumentoTipoSinPermisoDescarga, buttons: Ext.Msg.OK });
}
// #endregion

function abrirVisorSlug(idDocument) {
    idToOpenVisorBySlug = idDocument;
}

function afterArbolJerarquico() {
    if (idToOpenVisorBySlug) {
        if (idToOpenVisorBySlug != -1) {
            var record = App.grid.getStore().getById(idToOpenVisorBySlug);
            App.grid.getSelectionModel().select(record);
        }

        btnCloseShowVisorTreeP();
        VisorMode(ModoVisor);

        if (idToOpenVisorBySlug != -1) {
            CheckMultiSelect_Grid_RowSelectArbol(record);
        }
        else {
            docNotFound();
        }
    }
}
// END Compartir

// INIT Filtros Rápidos
var aplicandoFiltro = false;
function sotorePrincipalDataChanged(sender, registro, index) {
    if (!aplicandoFiltro) {

        BuscadorPredictivoDocumentos(sender, registro, index);


        var valoresCmbfiltCreador = App.cmbfiltCreador.value;
        var valoresCmbfiltExtension = App.cmbfiltExtension.value;
        var valoresCmbfiltEstado = App.cmbfiltEstado.value;
        var valoresCmbfiltDocumentoTipo = App.cmbfiltDocumentoTipo.value;

        recargarCombos([App.cmbfiltCreador, App.cmbfiltExtension, App.cmbfiltEstado, App.cmbfiltDocumentoTipo], function () {
               
            let idsTipos = hdFiltDocumentTipoIDs.value.split(",");
            let Extensiones = hdFiltExtensiones.value.split(",");
            let idsEstados = hdFiltEstadosIDs.value.split(",");
            let idsCreadores = hdFiltCreadoresIDs.value.split(",");

            let idsCreadoresRemove = [];
            let ExtensionesRemove = [];
            let idsEstadosRemove = [];
            let idsTiposRemove = [];

            App.cmbfiltCreador.store.data.items.forEach(i => {
                if (!idsCreadores.includes(i.data.UsuarioID.toString())) {
                    idsCreadoresRemove.push(i.data.UsuarioID);
                }
            });
            App.cmbfiltExtension.store.data.items.forEach(i => {
                if (!Extensiones.includes(i.data.Extension)) {
                    ExtensionesRemove.push(i.data.Extension);
                }
            });
            App.cmbfiltEstado.store.data.items.forEach(i => {
                if (!idsEstados.includes(i.data.DocumentoEstadoID.toString())) {
                    idsEstadosRemove.push(i.data.DocumentoEstadoID);
                }
            });
            App.cmbfiltDocumentoTipo.store.data.items.forEach(i => {
                if (!idsTipos.includes(i.data.DocumentTipoID.toString())) {
                    idsTiposRemove.push(i.data.DocumentTipoID);
                }
            });

            idsCreadoresRemove.forEach(i => {
                App.cmbfiltCreador.removeByValue(i);
            });
            ExtensionesRemove.forEach(i => {
                App.cmbfiltExtension.removeByValue(i);
            });
            idsEstadosRemove.forEach(i => {
                App.cmbfiltEstado.removeByValue(i);
            });
            idsTiposRemove.forEach(i => {
                App.cmbfiltDocumentoTipo.removeByValue(i);
            });

            //Setear valores anteriores
            App.cmbfiltCreador.setValue(valoresCmbfiltCreador);
            App.cmbfiltExtension.setValue(valoresCmbfiltExtension);
            App.cmbfiltEstado.setValue(valoresCmbfiltEstado);
            App.cmbfiltDocumentoTipo.setValue(valoresCmbfiltDocumentoTipo);

        });
    }
    aplicandoFiltro = false;
    
}

function RecargarCreador() {
    //App.cmbfiltCreador.getTrigger(0).hide();
    App.cmbfiltCreador.clearValue();
}

function SeleccionaCreador() {
    App.cmbfiltCreador.getTrigger(0).show();
}

function RecargarExtension() {
    //App.cmbfiltExtension.getTrigger(0).hide();
    App.cmbfiltExtension.clearValue();
}

function SeleccionaExtension() {
    App.cmbfiltExtension.getTrigger(0).show();
}

function RecargarEstado() {
    //App.cmbfiltEstado.getTrigger(0).hide();
    App.cmbfiltEstado.clearValue();
}

function SeleccionaEstado() {
    App.cmbfiltEstado.getTrigger(0).show();
}

function RecargarFiltroDocumentoTipo() {
    //App.cmbfiltDocumentoTipo.getTrigger(0).hide();
    App.cmbfiltDocumentoTipo.clearValue();
}

function SeleccionaFiltroDocumentoTipo() {
   App.cmbfiltDocumentoTipo.getTrigger(0).show();
}


function limpiarFiltrosRapidos() {
    RecargarCreador();
    RecargarExtension();
    RecargarEstado();
    RecargarFiltroDocumentoTipo();
    LimpiarDateStart();
    LimpiarDateEnd();
    aplicarFiltrosRapidos();
}
function aplicarFiltrosRapidos() {
    recargarRutaActual();
    aplicandoFiltro = true;

    /*App.storePrincipal.filterBy(function (node) {
        let correcto = true;
        let item = node.data;
        var filtrosVacios = false;


        if (App.cmbfiltCreador.value.length == 0 &&
            App.cmbfiltExtension.value.length == 0 &&
            App.cmbfiltEstado.value.length == 0 &&
            App.cmbfiltDocumentoTipo.value.length == 0 &&
            !App.datfiltDateStart.value && !App.datfiltDateEnd.value) {
            filtrosVacios = true;
        }

        if (!filtrosVacios) {
            if (App.cmbfiltCreador.value.length > 0 && !App.cmbfiltCreador.value.includes(item.CreadorID)) {
                correcto = false;
            }
            if (App.cmbfiltExtension.value.length > 0 && !App.cmbfiltExtension.value.includes(item.Extension)) {
                correcto = false;
            }
            if (App.cmbfiltEstado.value.length > 0 && !App.cmbfiltEstado.value.includes(item.EstadoID)) {
                correcto = false;
            }
            if (App.cmbfiltDocumentoTipo.value.length > 0 && !App.cmbfiltDocumentoTipo.value.includes(item.DocumentoTipoID)) {
                correcto = false;
            }
            if (App.datfiltDateStart.value && App.datfiltDateEnd.value) {
                if (App.datfiltDateStart.value >= node.data.Fecha && App.datfiltDateEnd.value <= node.data.Fecha) {
                    correcto = false;
                }
            }
            else {
                if (App.datfiltDateStart.value && App.datfiltDateStart.value >= node.data.Fecha) {
                    correcto = false;
                }
                if (App.datfiltDateEnd.value && App.datfiltDateEnd.value <= node.data.Fecha) {
                    correcto = false;
                }
            }
        }
        else {
            correcto = true;
        }

        return correcto;
    });*/

}

function LimpiarFiltrosRapidos() {
    RecargarCreador();
    RecargarExtension();
    RecargarEstado();
    RecargarFiltroDocumentoTipo();
    LimpiarDateStart();
    LimpiarDateEnd();
    recargarRutaActual();
}

function vaciarIdsFiltrados() {
    RecargarCreador();
    RecargarExtension();
    RecargarEstado();
    RecargarFiltroDocumentoTipo();
    LimpiarDateStart();
    LimpiarDateEnd();
    App.hdFiltDocumentTipoIDs.setValue("");
    App.hdFiltExtensiones.setValue("");
    App.hdFiltEstadosIDs.setValue("");
    App.hdFiltCreadoresIDs.setValue("");
}

function LimpiarDateStart() {
    App.datfiltDateStart.setValue("");
    App.datfiltDateStart.getTrigger(0).hide();
}

function SeleccionaDateStart() {
    App.datfiltDateStart.getTrigger(0).show();
}

function LimpiarDateEnd() {
    App.datfiltDateEnd.setValue("");
    App.datfiltDateEnd.getTrigger(0).hide();
}

function SeleccionaDateEnd() {
    App.datfiltDateEnd.getTrigger(0).show();
}

function hidePnQuickFilters() {
    MostrarPanelLateral(App.btnCollapseAsRClosed);
    displayMenuDoc('pnQuickFilters');
}

// END Filtros Rápidos



function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    allDocsSelected = sender.selected.items;

    //App.btnAgregarDocumento.disable();
    App.btnEditarDocumento.disable();
    App.btnDesactivar.setDisabled(true);
    App.btnInfoVisor.disable();
    App.btnHistoricoVisor.disable();
    App.btnDescargarVisor.disable();
    App.btnDesactivarVisor.disable();
    App.btnDescargar.disable();
    App.btnTbHeaderDescargar.disable();

    NuevaVDocumentoPadreID = 0;
    NuevaVversionDocAnterior = 0;
    NuevaVdocumentoEstado = 0;
    var AllDocumentsActives = true;
    var AllDocumentsDesactives = true;
    var carpetaSeleccionada = false;

    allDocsSelected.forEach(dato => {
        let temp = dato.data;

        var lObjetoID;
        switch (App.hdObjetoTipo.value) {
            case "Emplazamiento":
                lObjetoID = temp.EmplazamientoID;
                break;
            case "InventarioElemento":
                lObjetoID = temp.InventarioElementoID;
                break;
            default:
                lObjetoID = -1;
                break;
        }
        App.hdEditadoObjetoID.setValue(lObjetoID);

        if (temp != null) {

            seleccionado = temp;

            if (seleccionado.id != null) {

                if (seleccionado.EsCarpeta) {
                    App.btnDescargar.enable();
                    App.btnDesactivar.disable();
                    carpetaSeleccionada = true;

                    AllDocumentsActives = false;
                    AllDocumentsDesactives = false;
                }
                else if (!seleccionado.EsCarpeta && !seleccionado.EsDocumento) {
                    //Es tipo
                    if (seleccionado.Escritura) {
                        //App.hdTipoDocumentoID.setValue(seleccionado.DocumentoTipoID);
                        App.hdEditadoObjetoID.setValue(lObjetoID);

                    }
                    if (seleccionado.Descarga) {
                        App.btnDescargar.enable();
                    }

                    AllDocumentsActives = false;
                    AllDocumentsDesactives = false;

                } else if (seleccionado.EsDocumento) {

                    App.hdTipoDocumentoID.setValue(seleccionado.DocumentoTipoID);
                    //Es Documento
                    App.btnInfoVisor.enable();
                    App.btnHistoricoVisor.enable();
                    //App.btnDesactivarVisor.enable();

                    
                    if (seleccionado.Escritura) {
                        App.btnEditarDocumento.enable();
                        //App.btnDesactivar.enable();

                        if (seleccionado.Activo) {
                            AllDocumentsDesactives = false;
                        } else {
                            AllDocumentsActives = false;
                        }

                    } else {
                        AllDocumentsActives = false;
                        AllDocumentsDesactives = false;
                    }

                    
                    //Parametros de versión
                    NuevaVDocumentoPadreID = ((seleccionado.DocumentoPadreID != null) ? seleccionado.DocumentoPadreID : seleccionado.id);
                    NuevaVversionDocAnterior = seleccionado.Version;
                    NuevaVdocumentoEstado = seleccionado.EstadoID;
                    //App.hdTipoDocumentoID.value = seleccionado.DocumentoTipoID;
                    //END Parametros de versión

                    permisoLectura = seleccionado.Lectura;
                    if (seleccionado.Descarga) {
                        App.btnDescargarVisor.enable();
                        App.btnTbHeaderDescargar.enable();
                        App.btnDescargar.enable();
                    }
                    else {
                        /*App.btnDescargarVisor.disable();
                        App.btnTbHeaderDescargar.disable();
                        App.btnDescargar.disable();*/
                    }
                    

                    DocID = seleccionado.id;
                    esVersion = false;

                    if (App.hdVisor.getValue() == "1") {

                        if (allDocsSelected.length > 1) {

                            QuitarDocumentoVisorMultiSelect();
                        } else {

                            CargarDocumentoVisor(seleccionado.id, permisoLectura);
                        }
                    }

                    if (!App.pnAsideR.collapsed) {
                        CargarStoresSerie([App.storeDocumentoLateral], function (fin) {
                            if (fin) {
                                html = '';
                                TreeCore.RecargaPanelLateral(seleccionado.id,
                                    {
                                        success: function (result) {
                                            if (result.Success != null && !result.Success) {
                                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                            } else {
                                                tablaBody = document.getElementById('bodyTablaInfoDocumentos');
                                                tablaBody.innerHTML = '';
                                                lista = result.Result;
                                                for (var dato in lista) {
                                                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato + '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                                                }
                                                tablaBody.innerHTML = html;


                                            }
                                        },
                                        eventMask: {
                                            showMask: true,
                                        }
                                    });
                            }
                        });

                    }
                }

                if (!seleccionado.Activo) {
                    App.btnDesactivar.setTooltip(jsActivar);
                }
                else {
                    App.btnDesactivar.setTooltip(jsDesactivar);
                }
            }

        }


    });

    if (carpetaSeleccionada) {
        App.btnDesactivar.disable();
    }
    else if (AllDocumentsActives) {
        //App.btnDesactivar.enable();
        App.btnDesactivar.enable();
        App.btnDesactivar.setTooltip(jsDesactivar);
    }
    else if (AllDocumentsDesactives) {
        //App.btnDesactivar.enable();
        App.btnDesactivar.enable();
        App.btnDesactivar.setTooltip(jsActivar);
    }
    else if (!AllDocumentsActives && !AllDocumentsDesactives) {
        App.btnDesactivar.disable();
    }
    else {
        App.btnDesactivar.disable();
        //App.btnDescargar.disable();
    }

    if (allDocsSelected.length > 1) {
        App.btnEditarDocumento.disable();

    }


    //ToolTip
    App.btnEditarDocumento.setTooltip(jsEditar);
    //App.btnDesactivar.setTooltip(jsDesactivar);
    App.btnDescargar.setTooltip(jsDescargar);
    App.btnInfoVisor.setTooltip(jsInfoDoc);
    App.btnDesactivarVisor.setTooltip(jsDesactivar);
    App.btnDescargarVisor.setTooltip(jsDescargar);
    App.btnHistoricoVisor.setTooltip(jsHistorico);
}


//#Region GRID
var listaRuta = [];

function VolverAtras() {
    if (listaRuta.length >= 2) {
        var ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.hdCarpetaPadreID.setValue(ElePadre.DocumentoTipoID);
        App.storePrincipal.reload();
        App.lbRutaCategoria.show();
        App.lbRutaCategoria.setText(ElePadre.Nombre);
        App.menuRuta.items.clear();
        //GenerarPadres();
        GenerarRuta();
    } else {
        IrRutaRaiz();
    }
}

function GenerarID() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function IrRutaRaiz() {
    LimpiarRuta();
    App.btnPadreCarpetaActucal.hide();
    App.storePrincipal.reload();
}

function SeleccionarRuta(sender, select) {
    vaciarIdsFiltrados();
    forzarCargaBuscadorPredictivo = true;
    App.hdCarpetaPadreID.setValue(select.DocumentoTipoID);
    App.storePrincipal.reload();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.lbRutaCategoria.setText(select.text);
    for (var i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].idUnico == select.IDUnico) {
            listaRuta = listaRuta.splice(0, ++i);
        }
    }
    App.menuRuta.items.clear();
    GenerarRuta();
}

function SeleccionarPadre() {
    LimpiarRuta();
    App.hdCarpetaPadreID.setValue(select.DocumentoTipoID);
    App.storePrincipal.reload();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.lbRutaCategoria.setText(select.text);
}

function EntrarEnCarpeta() {
    if (App.GridRowSelect.selected.items[0] != undefined && App.GridRowSelect.selected.items[0].data.EsCarpeta) {
        vaciarIdsFiltrados();
        forzarCargaBuscadorPredictivo = true;
        App.hdCarpetaPadreID.setValue(App.GridRowSelect.selected.items[0].data.DocumentoTipoID);
        App.storePrincipal.reload();
        App.btnAgregarDocumento.enable();
        App.lbRutaCategoria.show();
        App.btnCarpetaActual.show();
        App.lbRutaCategoria.setText(App.GridRowSelect.selected.items[0].data.Nombre);
        if (!listaRuta.some(ruta => ruta.DocumentoTipoID == App.GridRowSelect.selected.items[0].data.DocumentoTipoID)) {
            listaRuta.push({ Nombre: App.GridRowSelect.selected.items[0].data.Nombre, DocumentoTipoID: App.GridRowSelect.selected.items[0].data.DocumentoTipoID, idUnico: GenerarID() });
            App.storePrincipal.reload();
        }
        App.menuRuta.items.clear();
        GenerarRuta();
        App.storePrincipal.reload();
    }
}

function LimpiarRuta() {
    forzarCargaBuscadorPredictivo = true;
    vaciarIdsFiltrados();
    App.btnMenuRuta.hide();
    App.btnRaizCarpeta.hide();
    App.lbRutaCategoria.hide();
    App.btnCarpetaActual.hide();
    App.menuRuta.items.clear();
    listaRuta = [];
    App.hdCarpetaPadreID.setValue(0);
    //App.btnAgregarDocumento.disable();
}

function GenerarRuta() {
    App.btnMenuRuta.show();
    App.btnRaizCarpeta.show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    for (var item in listaRuta) {
        App.menuRuta.add(new Ext.menu.TextItem({ text: listaRuta[item].Nombre, DocumentoTipoID: listaRuta[item].DocumentoTipoID, IDUnico: listaRuta[item].idUnico }))
    }
    if (App.menuRuta.items.items.length > 1) {
        App.menuRuta.items.last().hide();
    } else {
        App.btnMenuRuta.hide();
        App.btnRaizCarpeta.hide();
    }
}

function recargarRutaActual() {
    forzarCargaBuscadorPredictivo = true;
    App.hdCarpetaPadreID.setValue(hdCarpetaActual.value);
    App.storePrincipal.reload();
    App.btnAgregarDocumento.enable();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();

    if (App.GridRowSelect.selected.length > 0) {
        if (App.GridRowSelect.selected.items[0].data.EsCarpeta) {
            App.lbRutaCategoria.setText(App.GridRowSelect.selected.items[0].data.Nombre);
            if (!listaRuta.some(ruta => ruta.DocumentoTipoID == App.GridRowSelect.selected.items[0].data.DocumentoTipoID)) {
                listaRuta.push({ Nombre: App.GridRowSelect.selected.items[0].data.Nombre, DocumentoTipoID: App.GridRowSelect.selected.items[0].data.DocumentoTipoID, idUnico: GenerarID() });
            }
            App.menuRuta.items.clear();
            GenerarRuta();
        }
    }
    
}

//#Endregion

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

//#region CLIENTES

function CargarStores() {
    App.hdCarpetaPadreID.setValue(0);
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

//#endregion                                            

function RecargarDocumentoTipo() {
    App.hdTipoDocumentoID.setValue(0);
    App.panelCenterShow.hide();
    App.paneEmpty.show();
    recargarCombos([App.cmbTiposDocumentos]);
}

function SeleccionarDocumentoTipo() {

    App.hdTipoDocumentoID.setValue(App.cmbTiposDocumentos.value);
    App.cmbTiposDocumentos.getTrigger(0).show();
    App.paneEmpty.hide();
    App.panelCenterShow.show();
}
var dataArray = [];
function BuscadorPredictivoDocumentos(sender, registro, index) {

    if (dataArray.length == 0 || forzarCargaBuscadorPredictivo) {
        dataArray = [];

        TreeCore.cargarPredictivoBuscador({
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    var documentosObj = JSON.parse(result.Result);

                    documentosObj.forEach(doc => {

                        Object.entries(doc).forEach(p => {
                            if (p[0] != "id" && p[0] != "Icono" && p[0] != "Extension") {
                                if (p[1] != "" && p[1] != null && !dataArray.some(a => a.id === doc.id)) {
                                    dataArray.push({
                                        key: doc.Nombre.toLowerCase(),
                                        key2: doc.Descripcion.toLowerCase(),
                                        value: doc.Nombre,
                                        icon: doc.Icono,
                                        extension: doc.Extension,
                                        id: doc.id,
                                        name: doc.Nombre,
                                        description: doc.Descripcion
                                    });
                                }
                            }
                        });
                    });

                    dataArray = dataArray.sort(function (a, b) {
                        return a.name.toString().toLowerCase().localeCompare(b.name.toString().toLowerCase());
                    });


                    $(function () {
                        let textBuscado = "";
                        $(`#TextFilter-inputEl`).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(dataArray, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    
                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idDocumentBuscador = $(e.currentTarget).attr("data-id-documento");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIdDocumentBuscador.setValue(idDocumentBuscador);


                                    BuscadorStorePrincipal();
                                    
                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            return $("<li>")
                                .append(
                                    `<div class="document-item" data-id-documento="${item.id}">` +
                                        `<div class="item-Buscador">` +
                                            `<img src="${item.icon}" alt="${item.extension}" >` +
                                            `<div class="title">${boldQuery(item.name, textBuscado)}</div>` +
                                        `</div>` +
                                        `<div class="description">${boldQuery(caracteresDescripcion(item.description), textBuscado)}</div>` +
                                    `</div>`)
                                .appendTo(ul);
                        };
                    });

                }
            },
            eventMask: {
                showMask: true,
            }
        });

    }
    
}

function caracteresDescripcion(descripcion) {
    if (descripcion.length > 195) {
        descripcion = descripcion.substr(0, 194);
        descripcion = descripcion.substr(0, descripcion.lastIndexOf(" ")) + "...";
    }
    return descripcion;
}

function FiltrarColumnas(sender, registro) {
    //filtroBuscador(App.storePrincipal, App.grid, sender.getRawValue());



}

function LimpiarFiltroBusqueda() {
    App.storePrincipal.clearFilter();
    App.TextFilter.setValue("");
    App.hdStringBuscador.setValue("");
    App.hdIdDocumentBuscador.setValue("");

    refreshStorePrincipal();
}

function buscarTextoTextFilter(sender, registro, index) {
    
    if (registro.keyCode && registro.keyCode == 13) {

        App.hdStringBuscador.setValue(App.TextFilter.value);
        App.hdIdDocumentBuscador.setValue("");

        BuscadorStorePrincipal();
    }
}


