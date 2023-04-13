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

var stPn = 0;

function hidePn() {
    let pn = document.getElementById('pnAsideR');
    let btn = document.getElementById('btnCollapseAsR');
    if (stPn == 0) {
        pn.style.marginRight = '-360px';
        btn.style.transform = 'rotate(-180deg)';
        stPn = 1;
    }
    else {
        pn.style.marginRight = '0';
        btn.style.transform = 'rotate(0deg)';
        stPn = 0;
    }

}


var stSldr = 0;
var stSldrMbl = 0;
//function moveCtSldr(btn) {
//    let btnPrssd = btn.id;
//    let ct1 = document.getElementById('ctMain1');
//    let ct2 = document.getElementById('ctMain2');
//    let ct3 = document.getElementById('ctMain3');
//    var res = window.innerWidth;


//    if (res > 480) {


//        if (stSldr == 0 && btnPrssd == 'btnPrevSldr') {
//            App.ctMain2.hide();
//            App.btnPrevSldr.disable();
//            App.btnNextSldr.enable();

//            stSldr = 1;

//        }
//        else if (stSldr != 0 && btnPrssd == 'btnNextSldr') {
//            App.ctMain2.show();
//            App.btnPrevSldr.enable();
//            App.btnNextSldr.disable();
//            stSldr = 0;

//        }

//    }

//    else if (res <= 480) {

//        if (stSldrMbl == 0 && btnPrssd == 'btnPrevSldr') {
//            App.ctMain1.hide();
//            App.btnNextSldr.enable();
//            stSldrMbl = 1;

//        }

//        else if (stSldrMbl == 1 && btnPrssd == 'btnPrevSldr') {
//            App.ctMain2.hide();
//            App.btnPrevSldr.disable();
//            App.btnNextSldr.enable();
//            stSldrMbl = 2;

//        }

//        else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr') {
//            App.ctMain1.show();
//            App.btnPrevSldr.enable();
//            App.btnNextSldr.disable();
//            stSldrMbl = 0;

//        }

//        else if (stSldrMbl == 2 && btnPrssd == 'btnNextSldr') {
//            App.ctMain2.show();
//            App.btnPrevSldr.disable();
//            App.btnNextSldr.enable();
//            stSldrMbl = 1;

//        }
//    }


//}

window.addEventListener('resize', function () {
    var resol = window.innerWidth;
    if (resol > 1024) {
        //App.ctMain2.show();
        App.ctMain1.show();
        //App.btnPrevSldr.enable();
        //App.btnNextSldr.disable();
        stSldr = 0;
    }

});

function resizeTwoPanels(sender) {
    SetMaxHeightSuperior(App.ctMain1);
    SetMaxHeightSuperior(App.ctMain2);
    SetMaxHeightSuperior(App.GridPanelCategorias, true);
    SetMaxHeightSuperior(App.pnConfigurador, true);
}


//BOTONES NAV

function habilitaLnk(vago) {
    let ct = document.getElementById('cntNavVistasForm-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}


function showForms(who) {
    var LNo = who.textEl;


    switch (who.id) {
        case 'lnkSiteF':
            habilitaLnk(LNo);
            App.formSite.show();
            App.formLocation.hide();
            App.formAdditional.hide();
            break;

        case 'lnkLocationF':

            habilitaLnk(LNo);
            App.formLocation.show();
            App.formSite.hide();
            App.formAdditional.hide();
            break;

        case 'lnkAdditionalF':
            habilitaLnk(LNo);
            App.formAdditional.show();
            App.formLocation.hide();
            App.formSite.hide();
            break;

    }
}



function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}


setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    winFormResize();
    //App.TreePanelCategorias.update();
    //App.pnConfigurador.update();

}, 1000);



function winFormResize() {

    var res = window.innerWidth;
    var bodyHeight = window.innerHeight;
    try {
        App.TreePanelCategorias.setHeight(bodyHeight);
        App.pnConfigurador.setHeight(bodyHeight);
        //100% del tamaño de la página = bodyHeight

        //var porcentaje = (90 * bodyHeight) / 100;
        //porcentaje = Math.round(porcentaje)

        // 90% de la página = porcentaje

        //App.winGestion.height = porcentaje + "px";


        if (res > 1024) {
            App.winGestion.setWidth(862)
        }

        if (res <= 1024 && res > 670) {
            App.winGestion.setWidth(445)
            App.winGestion.setHeight(450)
        }

        if (res <= 670) {
            App.winGestion.setWidth(425)
            App.winGestion.setHeight(430)
        }
    } catch (e) {

    }

}



window.addEventListener('resize', function () {
    winFormResize();
});

function windowDataSetting() { App.windowSetting.show() }
function winFormat() { App.winFormat.show() }

function ShowWindowGestion() {
    App.winGestion.show();
    App.winGestion.focus() // se coge el foco para que se pueda ocultar cuando se clique fuera en la function hideGridFormatRule()
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.winGestion.setY(y - 0);
    App.winGestion.setX(x - 0);
}

function GridFormatRule() {
    App.GridFormatRule.show();
    App.GridFormatRule.focus() // se coge el foco para que se pueda ocultar cuando se clique fuera en la function hideGridFormatRule()
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.GridFormatRule.setY(y - 50);
    App.GridFormatRule.setX(x - 160);
}

function GridAddCondition() {
    App.GridAddCondition.show();
    App.GridAddCondition.focus() // se coge el foco para que se pueda ocultar cuando se clique fuera en la function hideGridFormatRule()
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.GridAddCondition.setY(y - 100);
    App.GridAddCondition.setX(x - 160);
}
function winNewCondition() { App.winNewCondition.show() }

function GridAddRestriction() {
    App.GridAddRestriction.show();
    App.GridAddRestriction.focus() // se coge el foco para que se pueda ocultar cuando se clique fuera en la function hideGridFormatRule()
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.GridAddRestriction.setY(y - 100);
    App.GridAddRestriction.setX(x - 160);
}


/* Funciones Js */

var seleccionadoArbol;

function Grid_RowSelectArbol(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoArbol = datos;
        if (seleccionadoArbol.id != 'root' && !seleccionadoArbol.EsRoot) {
            App.btnEditar.enable();
            App.btnEliminar.enable();
            App.btnActivar.enable();
            App.btnNuevaCategorias.enable();
        } else {
            App.btnEditar.disable();
            App.btnEliminar.disable();
            App.btnActivar.disable();
        }

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionadoArbol.Activo) {
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
    App.GridRowSelectArbol.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnNuevaCategorias.disable();
    App.cmbNuevaCategorias.disable();
}

function SelectItemMenu(sender, registro, index) {
    var seleccionadoArbol = registro.data;
    if (document.getElementById('pnConfigurador_Content') != undefined) {
        document.getElementById('pnConfigurador_Content').style.display = 'none';
    }
    showLoadMaskCategorias(function (load) {
        //LimpiarAtributos();
        App.cmbNuevaCategorias.enable();
        App.hdCatSelect.setValue(seleccionadoArbol.InventarioCategoriaID);
        App.hdListaCategorias.setValue('');
        TreeCore.RecargarItemsNuevasCategorias(
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
//#region Drag&Drop

function DragDropInventario() {
    DragDropCategorias();
    DragDropAtributosCategorias();
}

//#endregion

function VaciarFormulario() {
    App.formGestion.getForm().reset();

    Ext.each(App.formGestion.body.query('*'), function (item) {
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

            if ((c.allowBlank && c.cls == 'txtContainerCategorias') || c.xtype == "checkboxfield") {
                try {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                } catch (e) {

                }
            }
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

function AgregarEditar() {
    VaciarFormulario();
    showLoadMaskCategorias(function (load) {
        recargarCombos([App.cmbIconos], function Fin(fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                Agregar = true;
                App.winGestion.show();
                load.hide();
            }
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
                    Refrescar();
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
    if (seleccionadoArbol != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    showLoadMaskCategorias(function (load) {
        recargarCombos([App.cmbIconos], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            load.hide();
                        }
                    });
            }
        });
    });
}

function Activar() {
    if (seleccionadoArbol != null) {
        ajaxActivar();
    }
}

function ajaxActivar() {
    showLoadMaskCategorias(function (load) {
        TreeCore.Activar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    load.hide();
                    Refrescar();
                }
            });
    });
}

function Eliminar() {
    if (seleccionadoArbol != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarCat,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarCat(button) {
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

function Refrescar() {
    showLoadMaskCategorias(function (load) {
        RecargarCategorias();
        App.hdCatSelect.setValue('');
        App.pnConfigurador.clearContent();
        DeseleccionarGrilla();
        load.hide();
    });
}

function VerActivos() {
    if (App.colActivo.hidden) {
        App.colActivo.show();
    } else {
        App.colActivo.hide();
    }
    Refrescar();
}

/* Trigger Combos */

function RecargarCategorias() {
    App.storeCategorias.reload();
}

function RecargarTipoEmplazamientos() {
    recargarCombos([App.cmbTipoEmplazamientos]);
    App.pnConfigurador.clearContent();
    RecargarCategorias();
}

function SeleccionarTipoEmplazamientos(sender) {
    App.cmbTipoEmplazamientos.getTrigger(0).show();
    App.pnConfigurador.clearContent();
    RecargarCategorias();
}

function RecargarIconos() {
    App.cmbIconos.clearValue();
    App.cmbIconos.getTrigger(0).hide();
}

function SeleccionarIconos() {
    App.cmbIconos.getTrigger(0).show();
}

/* Fin Trigger Combos */

/* Fin Funciones Js*/

/* Gestion Categorias */

/*function LimpiarAtributos() {
    App.pnConfigurador.items.clear();
    App.pnConfigurador.update()
}*/

function SelectItemNuevaCategorias(sender, registro, index) {
    var Categoria = new Object();
    Categoria.id = registro[0].data.InventarioAtributoCategoriaID;
    Categoria.nombre = registro[0].data.InventarioAtributoCategoria;
    showLoadMaskCategorias(function (load) {
        TreeCore.SeleccionarNuevaCategoria(Categoria.id,
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

var cmbFieldCargado = false;
function focusField() {
    if (!cmbFieldCargado) {
        cmbFieldCargado = true;
        App.storeTipoEmplazamientos.reload();
    }
}

//function AñadirNuevaCategoria(sender, registro, index) {
//    var Categoria = new Object();
//    Categoria.id = sender.selection.data.InventarioAtributoCategoriaID;
//    Categoria.nombre = sender.selection.data.InventarioAtributoCategoria;
//    showLoadMaskCategorias(function (load) {
//        TreeCore.SeleccionarNuevaCategoria(Categoria.id,
//            {
//                success: function (result) {
//                    if (result.Result != null && result.Result != '') {
//                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                    }
//                    sender.getTrigger(0).hide();
//                    load.hide();
//                }
//            });
//    });
//}

/* Fin Gestion Categorias */

function showLoadMaskCategorias(callback) {
    var myMask = new Ext.LoadMask({
        msg: App.jsCargando,
        target: App.vwResp
    });

    myMask.show();
    callback(myMask, null);
}

// Exportar Modelo

function ExportarModelo() {

    TreeCore.ExportarModeloDatos(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

// Fin Exportar Modelo

var listaRuta = [];

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
