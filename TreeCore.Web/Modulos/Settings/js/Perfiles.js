//setTimeout(function () {

//    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
//    //GridResizer();
//    App.pnComboGrdVisor.update();
//    //App.CenterPanelMain.update();
//    ActiveResizer();



//}, 100);

function grdInsideH() {
    let pH = App.pnComboGrdVisor.height;
    App.grdInsidePn.height = pH;
}

function ShowTreeGrd() {
    App.TreePanel2.show();
    App.PanelVisorMain.show();


    App.CenterPanelMain.hide();



}

function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}

function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}


// #region newResponsiveControl

var colOverride = "2cols";  // LOS MODOS SON 1cols 2cols 3cols

var colMode = "3colmode";
var isOnColmbl = 1;
var isOnColNormal = 1;

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

                App.btnNextSldr.show();
                App.btnPrevSldr.show();


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
    //App.MainVwP.update();

}

function moveCtSldr(btn) {

    var res = document.getElementById('CenterPanelMain').offsetWidth;
    var btnPrssd = btn.id;



    if (res < 576 && colMode == "1colmode") { // MODO 1 COLUMNA

        App.ctMain1.hide();
        App.ctMain2.hide();

        if (btnPrssd == 'btnNextSldr' && isOnColmbl == 1 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();

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


            App.ctMain1.show();

            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();


            isOnColmbl = 1;

        }





    }

    else if (res <= 991 && res > 576 && colMode == "2colmode") { // MODO 2 COLS

        App.ctMain2.hide();

        if (btnPrssd == 'btnNextSldr' && colMode == "2colmode") {

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


                App.btnNextSldr.show();
                App.btnPrevSldr.show();


                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(true);



                App.btnPrevSldr.disable();
                App.btnNextSldr.enable();

                colMode = "1colmode";

            }

            else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

                App.btnNextSldr.show();
                App.btnPrevSldr.show();


                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);

                colMode = "2colmode";

                isOnColmbl = 1;

                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

            }


            else if (res > 991 && colOverride == "3cols") {


                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();

                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);


                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

                colMode = "3colmode";

                isOnColmbl = 1;
                isOnColNormal = 1;

            }

            if (res >= 991 && colOverride == "2cols") {

                App.btnNextSldr.show();
                App.btnPrevSldr.show();


                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);

                colMode = "2colmode";

                isOnColmbl = 1;

                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();


            }



        }

        ColOverrideControl();

        //App.pnPermisosRoles.maxHeight = document.getElementById('pnDetalle-body').clientHeight - document.getElementById('cnPerfilesRol').clientHeight - 35;
        //App.pnPermisosRoles.minHeight = document.getElementById('pnDetalle-body').clientHeight - document.getElementById('cnPerfilesRol').clientHeight - 35;
        //App.pnPermisosRoles.updateLayout();
        //App.gridPermisos.maxHeight = document.getElementById('pnPermisosRoles-body').clientHeight - document.getElementById('ctCombosPermisos').clientHeight;
        //App.gridPermisos.minHeight = document.getElementById('pnPermisosRoles-body').clientHeight - document.getElementById('ctCombosPermisos').clientHeight;
        //App.gridPermisos.updateLayout();

    }

);

function ActiveResizer() {

    var el = document.getElementById('CenterPanelMain');


    if (el != null) {
        var res = document.getElementById('CenterPanelMain').offsetWidth;


        if (res <= 576 && colMode != "1colmode") {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();



            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(true);



            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();

            colMode = "1colmode";

        }

        else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();


            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);

            colMode = "2colmode";

            isOnColmbl = 1;

            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

        }


        else if (res > 991 && colOverride == "3cols") {

            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();


            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);


            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

            colMode = "3colmode";

            isOnColmbl = 1;
            isOnColNormal = 1;

        }


        if (res >= 991 && colOverride == "2cols") {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();


            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);

            colMode = "2colmode";

            isOnColmbl = 1;

            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();


        }


        ColOverrideControl();


    }

}


var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLite == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 0;
    }
}

var spPnLiteD = 0;
function hidePnLiteDirect() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLiteD == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLiteD = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLiteD = 0;
    }
}

function RowSelectAsideShow() {

    TreeCore.DirectShowHidePnAsideR();

}

function displayMenu(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridInfo.hide();
    App.pnInfoVersiones.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}

// #endregion

// #region Renders

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';
    }



}

function renderMultiflow(valor, id) {

    let imag = document.getElementById('imMultiflow' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '../../ima/ico-subprocess.svg';
    }


}

function renderCommercial(valor, id) {

    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-vendor.svg';

    }


}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }


}

function renderProgBar(valor, id) {
    let bar = document.getElementById('progBar' + id);
    let ancho = valor;

    bar.style.width = ancho * 100 + "%";


}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }


}

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

// #endregion


//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {


    obj.center();
    obj.update();

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
    obj.update();

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
    obj.update();


    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);


    obj.update();


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
    obj.update();

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

    //App.pnPermisosRoles.maxHeight = document.getElementById('pnDetalle-body').clientHeight - document.getElementById('cnPerfilesRol').clientHeight - 35;
    //App.pnPermisosRoles.minHeight = document.getElementById('pnDetalle-body').clientHeight - document.getElementById('cnPerfilesRol').clientHeight - 35;
    //App.pnPermisosRoles.updateLayout();
    //App.gridPermisos.maxHeight = document.getElementById('pnPermisosRoles-body').clientHeight - document.getElementById('ctCombosPermisos').clientHeight;
    //App.gridPermisos.minHeight = document.getElementById('pnPermisosRoles-body').clientHeight - document.getElementById('ctCombosPermisos').clientHeight;
    //App.gridPermisos.updateLayout();

});

// #endregion


// #region NAVEGACION TABS SUPERIORES

function NavegacionTabs(who) {
    var LNo = who.textEl;

    App.CenterPanelMain.hide();
    App.pnFuncionalidades.hide();
    App.wrapComponenteCentral.hide();

    switch (who.id) {
        case 'lnkPerfiles':
            ChangeTab(LNo);
            App.CenterPanelMain.show();
            break;

        case 'lnkFuncionalidades':
            ChangeTab(LNo);
            showLoadMask(App.MainVwP, function mascara(load) {
                TreeCore.CargarArbolModulos(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                App.pnFuncionalidades.show();
                                load.hide();
                            }
                        }
                    });
            });
            break;
        case 'lnkRoles':
            ChangeTab(LNo);
            App.wrapComponenteCentral.show();
            break;

    }
}

function ChangeTab(vago) {
    let ct = document.getElementById('TbNavegacionTabs-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}


// #endregion

//#region STORES

function cargarStoresPerfiles(sender, registro) {
    recargarCombos([App.cmbTipoProyecto]);
    GridPerfiles_DeseleccionarGrilla();
}

function CargarTreePerfiles(sender, registro) {

    GridPerfiles_DeseleccionarGrilla();
}

function CargarPaginasAsociadas(sender, registro) {

}

//#endregion

//#region PAGINA PERFILES

var AgregarPerfiles;

function GridPerfiles_Row_Select(sender, registro) {
    App.lbNombrePerfil.setText(registro.data.Nombre);
    //App.lbDescripcionPerfil.setText(registro.data.Descripcion);
    if (registro.data.text == 'Perfil') {
        showLoadMask(App.MainVwP, function mascara(load) {
            App.btnAnadirPerfiles.disable();
            App.btnEditarPerfiles.enable();
            App.btnEliminarPerfiles.enable();
            App.btnActivarPerfiles.enable();
            App.hdProyectoTipoSeleccionado.setValue(0);
            App.hdPerfilSeleccionado.setValue(registro.data.ID);
            if (registro.data.Activo) {
                App.btnActivarPerfiles.setTooltip(jsDesactivar);
            }
            else {
                App.btnActivarPerfiles.setTooltip(jsActivar);
            }
            CargarStoresSerie([App.storeUserInterfaces], function Fin(fin) {
                if (fin) {
                    App.BtnConfirmarCambiosPerfiles.enable();
                    load.hide();
                }
            });
            //TreeCore.PintarPermisosAsociados(true,
            //    {
            //        success: function (result) {
            //            if (result.Result != null && result.Result != '') {
            //                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            //            }
            //            load.hide();
            //        }
            //    });
        });
    } else {
        App.BtnConfirmarCambiosPerfiles.disable();
        App.btnAnadirPerfiles.enable();
        App.hdPerfilSeleccionado.setValue(0);
        App.hdProyectoTipoSeleccionado.setValue(registro.data.ID);
        App.btnEditarPerfiles.disable();
        App.btnActivarPerfiles.disable();
        App.btnEliminarPerfiles.disable();
    }
    App.btnEditarPerfiles.setTooltip(jsEditar);
    App.btnAnadirPerfiles.setTooltip(jsAgregar);
    App.btnEliminarPerfiles.setTooltip(jsEliminar);
    App.hdPaginaSeleccionada.setValue(0);
}

function GridPerfiles_DeseleccionarGrilla(sender, registro) {
    //App.hdPerfilSeleccionado.setValue("");
    App.btnAnadirPerfiles.disable();
    App.btnAnadirPerfiles.setTooltip(jsAgregar);
    App.BtnConfirmarCambiosPerfiles.disable();
    App.btnEditarPerfiles.disable();
    App.btnEliminarPerfiles.disable();
}

//#region Toolbar

function BtnAgregarPerfil(sender, registro) {
    AgregarPerfiles = true;
    LimpiarFormularioPerfil();
    recargarCombos([App.cmbPerfilesFuncionalidadesTipos], function Fin(fin) {
        if (fin) {
            App.winAddProfile.setTitle(jsAgregar + ' ' + jsTituloModuloPerfiles);
            App.winAddProfile.setHeight(390);
            //App.cmbPerfilesFuncionalidadesTipos.show();
            App.winAddProfile.show();
        }
    });
}

function BtnEditarPerfil(sender, registro) {
    AgregarPerfiles = false;
    App.winAddProfile.setTitle(jsEditar + ' ' + jsTituloModuloPerfiles);
    showLoadMask(App.MainVwP, function mascara(load) {
        recargarCombos([App.cmbTipoProyecto], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditarPerfil(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                App.winAddProfile.setHeight(320);
                                //App.cmbPerfilesFuncionalidadesTipos.hide();
                                load.hide();
                                App.winAddProfile.show();
                            }

                        }
                    });
            }
        })
    });
}

function BtnEliminarPerfil(sender, registro) {
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsTituloModuloPerfiles,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarPerfil,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxEliminarPerfil(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.EliminarPerfil(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            BtnRefrescarPerfil();
                            LimpiarContenedorPermisos();
                        }
                        load.hide();
                    }
                });
        });
    }
}

function BtnRefrescarPerfil(sender, registro) {
    showLoadMask(App.MainVwP, function mascara(load) {
        recargarCombos([App.cmbTipoProyecto], function Fin(fin) {
            if (fin) {
                TreeCore.CargarArbolPerfiles(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                load.hide();
                                LimpiarContenedorPermisos();
                            }
                        }
                    });
            }
        })
    });
}

function BtnActivarPerfil(sender, registro) {
    showLoadMask(App.MainVwP, function mascara(load) {
        TreeCore.ActivarPerfiles(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                    }
                }
            });
    });
}

function BtnDescargarPerfil(sender, registro) {

}

function FiltrarPerfiles(sender, registro) {
    var tree = App.TreePanelPerfiles,
        logic = tree,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    if (registro.getKey() === registro.ESC) {
        clearFilter();
    } else {
        // this will allow invalid regexp while composing, for example "(examples|grid|color)"
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        } catch (err) {
            return;
        }

        logic.filterBy(function (node) {
            return (re.test(node.data.Nombre))
        });
    }
}

function LimpiarFiltroPerfiles(sender, registro) {
    var field = App.txtFiltroPerfiles,
        tree = App.TreePanelPerfiles,
        logic = tree;

    field.setValue("");
    logic.clearFilter(true);
}

function RecargarClientes(sender, registro) {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    cargarStoresPerfiles();
}

function SeleccionarCliente(sender, registro) {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    cargarStoresPerfiles();
    BtnRefrescarPerfil();
}

function RecargarProyectosTipos(sender, registro) {

    App.btnAnadirPerfiles.disable();
    App.hdProyectoTipoSeleccionado.setValue(0);

    showLoadMask(App.MainVwP, function mascara(load) {
        recargarCombos([App.cmbTipoProyecto], function Fin(fin) {
            if (fin) {
                TreeCore.CargarArbolPerfiles(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                load.hide();
                            }
                        }
                    });
            }
        })
    });
}

function SeleccionarProyectosTipos(sender, registro) {
    sender.getTrigger(0).show();
    App.btnAnadirPerfiles.enable();
    App.hdProyectoTipoSeleccionado.setValue(registro[0].id);
    showLoadMask(App.MainVwP, function mascara(load) {
        TreeCore.CargarArbolPerfiles(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                    }
                }
            });
    });
}

//#endregion

//#region Administacion Permisos

function LimpiarContenedorPermisos() {
    App.ctMain2.clearContent();
    App.lbNombrePerfil.setText('');
    //App.lbDescripcionPerfil.setText('');
}

function LimpiarcmbPaginaLibres(sender, registro) {
    sender.getTrigger(0).hide();
    sender.reset();
}

function SeleccionarNuevaPagina(sender, registro) {
    sender.getTrigger(0).show();
}
function AnadirNuevaPagina(sender, registro) {
    App.hdPaginaSeleccionada.setValue(sender.value);
    showLoadMask(App.MainVwP, function mascara(load) {
        TreeCore.PintarPermisosNuevaPagina(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        sender.getTrigger(0).hide();
                        load.hide();
                    }
                }
            });
    });
}

function ModificarPermisos(sender, registro) {
    if (sender.config.doChange == true) {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.CambiarAsignacionPermisos(sender.config.iDFuncionalidad, sender.value,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                        }
                    }
                });
        });
    }
}

var botonEliminar;

function EliminarPaginaPerfil(sender) {
    botonEliminar = sender;
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsPagina,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarPaginaPerfil,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxEliminarPaginaPerfil(button) {
    if (button == 'yes' || button == 'si') {
        sender = botonEliminar;
        sender.up().up().contentEl.dom.childNodes.forEach(control => {
            var chk = Ext.getCmp(control.lastElementChild.id);
            if (chk.value == true)
                chk.setValue(false);
        });
        contenedor = sender.up().up();
        contenedor.hide();
        var arr = App.hdPaginaSeleccionada.value.split(',');
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == String(contenedor.config.moduloID)) {
                arr.splice(i, 1);
            }
        }
        App.hdPaginaSeleccionada.setValue(arr.join(','));
    }
}

function RenderChkFuncionalidad(sender, registro) {
    sender.setValue(sender.config.valorInicial);
    sender.config.doChange = true;
}

//#endregion

function BtnGuardarPerfil(sender, registro) {
    showLoadMask(App.winAddProfile, function mascara(load) {
        TreeCore.AgregarEditarPerfil(AgregarPerfiles, [],
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        CargarStoresSerie([App.storeUserInterfaces], function Fin(fin) {
                            if (fin) {
                                App.BtnConfirmarCambiosPerfiles.enable();
                                App.winAddProfile.hide();
                                BtnRefrescarPerfil();
                                load.hide();
                            }
                        });
                        //TreeCore.PintarPermisosAsociados(true,
                        //    {
                        //        success: function (result) {
                        //            if (result.Result != null && result.Result != '') {
                        //                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        //            }
                        //            else {
                        //                App.winAddProfile.hide();
                        //                //BtnRefrescarPerfil();
                        //                //LimpiarContenedorPermisos();
                        //                load.hide();
                        //            }
                        //        }
                        //    });
                    }
                }
            });
    });
}

function ValidarFormularioPerfil(sender, valido) {
    if (valido) {
        App.btnGuardarPerfil.enable();
        Ext.each(App.winAddProfile.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid()) {
                App.btnGuardarPerfil.disable();
            }
        });
    } else {
        App.btnGuardarPerfil.disable();
    }
}

function LimpiarFormularioPerfil() {
    Ext.each(App.winAddProfile.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            c.reset();
        }
    });
}

function BtnGuardarFuncionalidades() {
    showLoadMask(App.MainVwP, function mascara(load) {
        let listaFun = [];
        document.querySelectorAll('.chkDone:checked').forEach(x => {
            listaFun.push(x.getAttribute('code'));
        });
        TreeCore.AgregarEditarPerfil(false, listaFun,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                    load.hide();
                }
            });
    });
}

//#endregion

//#region PAGINA FUNCIONALIDADES

var AgregarFuncionalidades;
var TipoSeleccionado;

function GridFuncionalidades_Row_Select(sender, registro) {
    App.btnAnadirFuncionalidades.enable();
    TipoSeleccionado = registro.data.text;
    if (registro.data.text == 'Funcionalidad' || registro.data.text == 'Modulo') {
        App.hdFuncionalidadModuloSeleccionado.setValue(registro.data.ID);
        if (registro.data.text == 'Modulo') {
            App.btnAnadirFuncionalidades.enable();
        } else {
            App.btnAnadirFuncionalidades.disable();
        }
        if (registro.data.Activo) {
            App.btnActivarFuncionalidades.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarFuncionalidades.setTooltip(jsActivar);
        }
        App.btnActivarFuncionalidades.enable();
        App.btnEditarFuncionalidades.enable();
        App.btnEliminarFuncionalidades.enable();
    } else {
        App.hdFuncionalidadModuloSeleccionado.setValue(0);
        App.btnAnadirFuncionalidades.enable();
        App.btnActivarFuncionalidades.disable();
        App.btnEditarFuncionalidades.disable();
        App.btnEliminarFuncionalidades.disable();
    }
    App.btnEditarFuncionalidades.setTooltip(jsEditar);
    App.btnAnadirFuncionalidades.setTooltip(jsAgregar);
    App.btnEliminarFuncionalidades.setTooltip(jsEliminar);
}

function GridFuncionalidades_DeseleccionarGrilla(sender, registro) {
    App.hdFuncionalidadModuloSeleccionado.setValue(0);
    App.btnAnadirFuncionalidades.disable();
    App.btnEditarFuncionalidades.disable();
    App.btnEliminarFuncionalidades.disable();
}

//#region Toolbar

function BtnAgregarFuncionalidad(sender, registro) {
    AgregarFuncionalidades = true;
    if (TipoSeleccionado == 'Modulo') {
        LimpiarFormularioFuncionalidad();
        recargarCombos([App.cmbTipoFuncionalidadFuncionalidad], function Fin(fin) {
            if (fin) {
                App.winAddFuncionalidad.setTitle(jsAgregar + ' ' + jsTituloModuloFuncionalidades);
                App.nbCodigoFuncionalidad.enable();
                App.winAddFuncionalidad.show();
            }
        })
    }
    else if (TipoSeleccionado == 'Proyecto') {
        LimpiarFormularioModulo();
        App.winAddModulo.setTitle(jsAgregar + ' ' + jsTituloModuloModulos);
        App.winAddModulo.show();
    }
}

function BtnEditarFuncionalidad(sender, registro) {
    AgregarFuncionalidades = false;
    if (TipoSeleccionado == 'Funcionalidad') {
        App.winAddFuncionalidad.setTitle(jsEditar + ' ' + jsTituloModuloFuncionalidades);
        showLoadMask(App.MainVwP, function mascara(load) {
            recargarCombos([App.cmbTipoFuncionalidadFuncionalidad], function Fin(fin) {
                if (fin) {
                    TreeCore.MostrarEditarFuncioanlidad(
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                }
                                else {
                                    load.hide();
                                    App.nbCodigoFuncionalidad.disable();
                                    App.winAddFuncionalidad.show();
                                }
                            }
                        });
                }
            })
        });
    }
    else if (TipoSeleccionado == 'Modulo') {
        LimpiarFormularioModulo();
        App.winAddModulo.setTitle(jsEditar + ' ' + jsTituloModuloModulos);
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.MostrarEditarModulo(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                            App.winAddModulo.show();
                        }
                    }
                });
        });
    }
}

function BtnEliminarFuncionalidad(sender, registro) {
    if (TipoSeleccionado == 'Funcionalidad') {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModuloFuncionalidades,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarFuncionalidad,
                icon: Ext.MessageBox.QUESTION
            });
    }
    else if (TipoSeleccionado == 'Modulo') {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModuloModulos,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarModulos,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarFuncionalidad(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.EliminarFuncioanlidad(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                            BtnRefrescarFuncionalidad();
                        }
                    }
                });
        });
    }
}

function ajaxEliminarModulos(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.EliminarModulo(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                            BtnRefrescarFuncionalidad();
                        }
                    }
                });
        });
    }
}

function BtnRefrescarFuncionalidad(sender, registro) {
    showLoadMask(App.MainVwP, function mascara(load) {
        TreeCore.CargarArbolModulos(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                    }
                }
            });
    });
}

function BtnActivarFuncionalidad(sender, registro) {
    if (TipoSeleccionado == 'Funcionalidad') {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.ActivarFunciuonlidad(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                        }
                    }
                });
        });
    }
    else if (TipoSeleccionado == 'Modulo') {
        showLoadMask(App.MainVwP, function mascara(load) {
            TreeCore.ActivarModulo(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            load.hide();
                        }
                    }
                });
        });
    }
}

function BtnDescargarFuncionalidad(sender, registro) {

}

function FiltrarFuncionaliades(sender, registro) {
    var tree = App.TreePanelFuncionalidades,
        logic = tree,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    if (registro.getKey() === registro.ESC) {
        clearFilter();
    } else {
        // this will allow invalid regexp while composing, for example "(examples|grid|color)"
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        } catch (err) {
            return;
        }

        logic.filterBy(function (node) {
            valido = false;
            if (re.test(node.data.Nombre)) {
                valido = true;
            }
            if (re.test(node.data.Info)) {
                valido = true;
            }
            if (re.test(node.data.TipoFuncionalidad)) {
                valido = true;
            }
            if (re.test(node.data.Descripcion)) {
                valido = true;
            }
            return valido;
        });
    }

}

function LimpiarFiltroFuncionalidades(sender, registro) {
    var field = App.txtFiltroFuncionalidades,
        tree = App.TreePanelFuncionalidades,
        logic = tree;

    field.setValue("");
    logic.clearFilter(true);
}

//#endregion

//#region VENTANA FUNCIONALIDAD

function SeleccionarTipoFuncionalidadFuncionalidad(sender, registro) {
    sender.getTrigger(0).show();
    ValidarFormularioFuncionalidad(sender, true);
}

function BtnGuardarFuncionalidad(sender, registro) {
    showLoadMask(App.winAddFuncionalidad, function mascara(load) {
        TreeCore.AgregarEditarFuncioanlidad(AgregarFuncionalidades,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                        App.winAddFuncionalidad.hide();
                        BtnRefrescarFuncionalidad(sender, registro);
                    }
                }
            });
    });
}

function ValidarFormularioFuncionalidad(sender, valido) {
    if (valido) {
        App.btnGuardarFuncionalidad.enable();
        Ext.each(App.winAddFuncionalidad.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid()) {
                App.btnGuardarFuncionalidad.disable();
            }
        });
    } else {
        App.btnGuardarFuncionalidad.disable();
    }
}

function LimpiarFormularioFuncionalidad() {
    Ext.each(App.winAddFuncionalidad.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            c.reset();
        }
    });
}

//#endregion

//#region VENTANA MODULO

function BtnGuardarModulo(sender, registro) {
    showLoadMask(App.winAddModulo, function mascara(load) {
        TreeCore.AgregarEditarModulo(AgregarFuncionalidades,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                        App.winAddModulo.hide();
                        BtnRefrescarFuncionalidad(sender, registro);
                    }
                }
            });
    });
}

function ValidarFormularioModulo(sender, valido) {
    if (valido) {
        App.btnGuardarModulo.enable();
        Ext.each(App.winAddModulo.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid()) {
                App.btnGuardarModulo.disable();
            }
        });
    } else {
        App.btnGuardarModulo.disable();
    }
}

function LimpiarFormularioModulo() {
    Ext.each(App.winAddModulo.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            c.reset();
        }
    });
}

//#endregion

//#endregion

// #region ROLES

// #region PLANTILLA ROLES

var iSelectedPanel = 0;
var iSelectedPanelProfile = 0;
var PuntoCorteS = 512;
var PuntoCorteP = 650;

function VisorSwitch(sender) {

    var asideL = Ext.getCmp(['pnRoles']);

    if (asideL.hidden == true) {
        forcedVisor = false;
        App.pnRoles.show();

    }
    else {
        forcedVisor = true;
        App.pnRoles.hide();

    }

}

function ControlPaneles(sender) {

    var containerSize = App.wrapComponenteCentral.getWidth();

    if (containerSize < PuntoCorteS) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        SliderMove("");
    }
    else {
        App.tbFiltrosYSliders.hide();
        App.btnPrev.hide();
        App.btnNext.hide();
        App.pnRoles.show();
        App.pnDetalle.show();
    }
}

function SliderMove(NextOrPrev) {

    if (NextOrPrev != "") {
        if (NextOrPrev == 'Next') {
            iSelectedPanel++;
            App.pnRoles.hide();
            App.pnDetalle.show();
            App.pnPermisosRoles.setHidden(false);

            if (App.hdRolSeleccionado != undefined && App.hdRolSeleccionado.value != "") {
                CargarStoresSerie([App.storePerfilesAsignados, App.storePermisosRoles], function Fin(fin) {
                    if (fin) {
                        cargarComboPerfiles();
                        CrearGrid();
                    }
                });
            }
            else {
                App.pnPermisosRoles.hide();
            }

            App.btnPrev.enable();
            App.btnNext.disable();

        }
        else {
            iSelectedPanel--;
            App.pnRoles.show();
            App.pnDetalle.hide();

            App.btnPrev.disable();
            App.btnNext.enable();

        }
    }
    else {
        if (iSelectedPanel == 0) {
            App.btnPrev.disable();
            App.btnNext.enable();
        }
        else {
            App.btnPrev.enable();
            App.btnNext.disable();
        }

        // LOAD PANEL
        if (iSelectedPanel == 0) {
            App.pnRoles.show();
            App.pnDetalle.hide();
        }
        if (iSelectedPanel == 1) {
            App.pnRoles.hide();
            App.pnDetalle.show();
        }
    }
}

function ControlPanelesProfile(sender) {

    var containerSize = App.CenterPanelMain.getWidth();

    if (containerSize < PuntoCorteP) {
        App.tbFiltrosYSlidersProfile.show();
        App.btnPrevP.show();
        App.btnNextP.show();
        SliderMoveProfile("");
    }
    else {
        App.tbFiltrosYSlidersProfile.hide();
        App.btnPrevP.hide();
        App.btnNextP.hide();
        App.ctMain1.show();
        App.ctMain2.show();
    }
}

function SliderMoveProfile(NextOrPrev) {

    if (NextOrPrev != "") {
        if (NextOrPrev == 'Next') {
            iSelectedPanelProfile++;
            App.ctMain1.hide();
            App.ctMain2.show();

            App.btnPrevP.enable();
            App.btnNextP.disable();

        }
        else {
            iSelectedPanelProfile--;
            App.ctMain1.show();
            App.ctMain2.hide();

            App.btnPrevP.disable();
            App.btnNextP.enable();

        }
    }
    else {
        if (iSelectedPanelProfile == 0) {
            App.btnPrevP.disable();
            App.btnNextP.enable();
        }
        else {
            App.btnPrevP.enable();
            App.btnNextP.disable();
        }

        // LOAD PANEL
        if (iSelectedPanelProfile == 0) {
            App.ctMain1.show();
            App.ctMain2.hide();
        }
        if (iSelectedPanelProfile == 1) {
            App.ctMain1.hide();
            App.ctMain2.show();
        }
    }
}

// #endregion

function btnRefrescarRoles() {
    App.storeRoles.reload();
    DeseleccionarGrillaRoles();
}


// #region AGREGAR/EDITAR ROLES
var controlROLES = false;

function Grid_RowSelectRoles(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.hdRolSeleccionado.setValue(seleccionado.RolID);
        App.btnEditarRol.setDisabled(false);
        App.btnEliminarRol.setDisabled(false);
        App.btnActivarRol.setDisabled(false);

        App.lbCodigoRol.setText(seleccionado.Codigo);
        App.lbTituloRol.setText(seleccionado.Nombre);
        App.lbDescripcionRol.setText(seleccionado.Descripcion);
        App.cnPerfilesRol.setHidden(false);
        App.pnPermisosRoles.setHidden(false);

        CargarStoresSerie([App.storePerfilesAsignados, App.storePermisosRoles], function Fin(fin) {
            if (fin) {
                cargarComboPerfiles();
                CrearGrid();
            }
        });

    }
}
function DeseleccionarGrillaRoles() {
    App.GridRowSelectRoles.clearSelections();
    App.btnEditarRol.setDisabled(true);
    App.btnEliminarRol.setDisabled(true);
    App.btnActivarRol.setDisabled(true);

}

function vaciarFormularioRoles() {
    App.formRoles.getForm().reset();
    App.txtCodigo.reset();
    App.txtNombre.reset();
    App.txtaDescripcion.reset();

    Ext.each(App.formRoles.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield" && c.xtype != "hidden") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);

                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });
}

function btnAnadirRoles() {
    controlROLES = true;
    vaciarFormularioRoles();
    App.winGestionRoles.setTitle(jsAgregar + ' ' + jsRoles);
    App.winGestionRoles.show();
}
function ajaxAgregarEditar() {

    TreeCore.AgregarEditarRoles(controlROLES,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeRoles.reload();
                    App.winGestionRoles.hide();
                }
            }
        });
}

function btnEditarRoles() {
    controlROLES = false;
    vaciarFormularioRoles();
    TreeCore.MostrarEditarRoles(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                    App.winGestionRoles.setTitle(jsEditar + ' ' + jsRoles);
                    App.winGestionRoles.show();
                    btnRefrescarRoles();
                }
            }
        });
}

function btnEliminarRoles() {
    if (registroSeleccionado(App.gridRoles) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsRoles,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarRoles,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarRoles(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarRoles({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnEditarRol.setDisabled(false);
                    App.btnEliminarRol.setDisabled(false);
                    App.storeRoles.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });

        App.lbCodigoRol.setText("");
        App.lbTituloRol.setText("");
        App.lbDescripcionRol.setText("");
        App.cnPerfilesRol.setHidden(true);
        App.pnPermisosRoles.setHidden(true);
    }
}

function ajaxActivarRol() {
    TreeCore.ActivarRoles({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.btnEditarRol.setDisabled(false);
                App.btnEliminarRol.setDisabled(false);
                App.storeRoles.reload();
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}



function validarFormulario(valid) {
    if (valid) {
        App.btnGuardarRol.setDisabled(false);
    } else {
        App.btnGuardarRol.setDisabled(true);
    }
}

var tagsRolesWF = null;
function cargarComboPerfiles() {
    var arrayPerfiles = App.storePerfiles.data.items;
    var perfiles = [];
    for (var i = 0; i < arrayPerfiles.length; i++) {
        perfiles[i] = { 'tag': arrayPerfiles[i].data.Perfil_esES, 'value': arrayPerfiles[i].data.PerfilID };
    }
    if (tagsRolesWF != null) {
        tagsRolesWF.destroy();
    }

    tagsRolesWF = new AmsifySuggestags($('input[name="perfiles"]'));

    tagsRolesWF._settings({
        suggestions: perfiles,
        whiteList: true,
        showAllSuggestions: true,
        afterAdd: function (value) {
            ajaxAnadirPerfilRol(value);
        },
        afterRemove: function (value) {
            ajaxEliminarPerfilRol(value);
        }
    })
    document.getElementsByName('perfiles')[0].value = '';
    tagsRolesWF._init();


    var perfil = [];
    var arrayPerfilesAsignados = App.storePerfilesAsignados.data.items;
    for (var i = 0; i < arrayPerfilesAsignados.length; i++) {
        perfil = arrayPerfilesAsignados[i].data.PerfilID;
        tagsRolesWF.addTag(perfil);
    }


}

function ajaxAnadirPerfilRol(perfilID) {
    TreeCore.AgregarPerfilRol(perfilID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}
function ajaxEliminarPerfilRol(perfilID) {
    TreeCore.EliminarPerfilRol(perfilID, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {


            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}


//#endregion

function CrearGrid() {
    grid = document.getElementById("items-ct");
    var store = App.storePermisosRoles.data.items;
    var html = "<tpl for='.'>";

    for (var i = 0; i < store.length; i++) {
        if (store[i].data.ListaItem.length > 0) {
            html += "<div class='group-header'>";
            html += "<h4><div><img src='" + cambiarIcono(store[i].data.Tipo) + "' class='iconoPermisos' />" + store[i].data.Tipo + "</div></h4>";
            html += "<div class='group-body'>";
            html += "<tpl for='ListaItem'>";

            if (jsEmplazamientoAtributos == store[i].data.Tipo) {
                var item = store[i].data.ListaItem;
                for (var x = 0; x < item.length; x++) {

                    html += "<div id='" + item[x].Nombre + "' class='item-wrap rowPermisos'>";
                    html += "<h6 class='buscador'>" + item[x].Nombre + "</h6>";

                    if (item[x].Restriccion == 'ACTIVE') {
                        html += "<input type='checkbox' disabled checked /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsOculto + "</span>";
                    } else if (item[x].Restriccion == 'DISABLED') {
                        html += "<input type='checkbox' disabled /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled checked /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsOculto + "</span>";
                    } else if (item[x].Restriccion == 'HIDDEN') {
                        html += "<input type='checkbox' disabled /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled checked /><span>" + jsOculto + "</span>";
                    }
                    html += "</div>";

                }
            } else if (store[i].data.Tipo == jsDocumentoTipo) {
                var item = store[i].data.ListaItem;

                for (var j = 0; j < item.length; j++) {
                    html += "<div id='" + item[j].Nombre + "' class='item-wrap rowPermisos'>";
                    html += "<h6 class='buscador'>" + item[j].Nombre + "</h6>";
                    if (item[j].PermisoEscritura) {
                        html += "<input type='checkbox' disabled checked /><span>" + jsEscritura + "</span>";
                    } else {
                        html += "<input type='checkbox' disabled /><span>" + jsEscritura + "</span>";
                    }

                    if (item[j].PermisoLectura) {
                        html += " <input type='checkbox' disabled checked /><span>" + jsLectura + "</span>";
                    } else {
                        html += " <input type='checkbox' disabled /><span>" + jsLectura + "</span>";
                    }

                    if (item[j].PermisoDescarga) {
                        html += " <input type='checkbox' disabled checked /><span>" + jsDescarga + "</span>";
                    } else {
                        html += " <input type='checkbox' disabled /><span>" + jsDescarga + "</span>";
                    }



                    html += "</div>";
                }

            } else if (jsInventarioElemento == store[i].data.Tipo) {
                var item = store[i].data.ListaItem;
                for (var x = 0; x < item.length; x++) {

                    html += "<div id='" + item[x].Nombre + "' class='item-wrap rowPermisos'>";
                    html += "<h6 class='buscador' >" + item[x].Nombre + "</h6>";
                    if (item[x].Restriccion == 'ACTIVE') {
                        html += "<input type='checkbox' disabled checked /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsOculto + "</span>";
                    } else if (item[x].Restriccion == 'DISABLED') {
                        html += "<input type='checkbox' disabled /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled checked /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsOculto + "</span>";
                    } else if (item[x].Restriccion == 'HIDDEN') {
                        html += "<input type='checkbox' disabled /><span>" + jsActivo + "</span>";
                        html += "<input type='checkbox' disabled /><span>" + jsDesactivo + "</span>";
                        html += "<input type='checkbox' disabled checked /><span>" + jsOculto + "</span>";
                    }
                    html += "</div>";

                }
            } else if (jsGruposAccesosWeb == store[i].data.Tipo) {
                var item = store[i].data.ListaItem;
                for (var x = 0; x < item.length; x++) {

                    html += "<div id='" + item[x].Nombre + "' class='item-wrap rowPermisos'>";
                    html += "<h6 class='buscador' >" + item[x].Nombre + "</h6>";

                    html += "</div>";

                }
            }



            html += " </tpl> </div> </div>";
        }
    }


    html += "</tpl>";

    if (grid != null) {
        grid.innerHTML = html;
    }

}

function refrescarPermisos() {
    CargarStoresSerie([App.storePermisosRoles], function Fin(fin) {
        if (fin) {
            CrearGrid();
        }
    });
}

function limpiarComboPermisos(sender, registro) {
    var field = App.cmbFiltroPermisos;
    field.setValue("");
    refrescarPermisos()
}
function limpiarComboPermisos(sender, registro) {
    var field = App.cmbFiltroPermisos;
    field.setValue("");
    refrescarPermisos();
}
function LimpiarFiltroPermisos(sender, registro) {
    var field = App.txtBusquedaPermisos;
    field.setValue("");
    refrescarPermisos();
}

function FiltrarPermisos(sender, registro) {
    var tree = document.getElementsByClassName('buscador');
    var text = sender.getRawValue();

    for (i = 0; i < tree.length; i++) {
        a = tree[i];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(text.toUpperCase()) > -1) {
            a.parentNode.style.display = "";
        } else {
            a.parentNode.style.display = "none";
        }
    }

}

function cambiarIcono(dato) {
    var icono;
    switch (dato) {
        case jsEmplazamientoAtributos:
            icono = '/ima/ico-geolocation-green.svg';
            break;
        case jsDocumentoTipo:
            icono = '/ima/ico-docs-gr.svg';
            break;
        case jsInventarioElemento:
            icono = '/ima/ico-InventoryElement-gr.svg';
            break;
        case jsGruposAccesosWeb:
            icono = '/ima/ico-acceso-gr-20px.svg';
            break;
        default:
            icono = '';
            break;
    }

    return icono;
}

// #endregion