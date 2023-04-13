var seleccionado = null;
var versionseleccionada = null;
var TopContracted = false;

function Errores(msg) {
    Ext.Msg.show({ title: jsAtencion, msg: msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.WARNING });
}

var TriggerClientes = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbCliente.clearValue();
            break;
        case 1:
            App.storeClientes.reload();
            break;
    }
}

//VERTICALES
var TriggerVerticales = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbVertical.clearValue();
            break;
        case 1:
            App.storeVerticales.reload();
            break;
    }
}


function LoadInicio() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "pages/DataUpload.aspx");
}


// FIN BOTONES

function RecargarInicio() {
    window.location.reload();
}

function ajaxLogout(button) {
    if (button == 'ok') {
        TreeCore.LogoutInicio(
            {
                success: function (result) {
                    if (result.Result != null && result.result != '') {
                        Ext.msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        RecargarInicio();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
}

//ACTIVAR CAMPOS DE LOS FORMULARIOS
function FormularioCenter() {

    App.txtURL.show();
    App.txtNombreResponsable.show();
    App.txtEmailResponsable.show();

    App.txtAPI.hide();
    App.txtUserAPI.hide();
    App.txtClaveAPI.hide();
    App.cmbCliente.hide();
    App.cmbVertical.hide();
    App.txtVersion.hide();
    App.txtContactoAdministrativo.hide();
    App.txtContactoAdministrativoEmail.hide();
    App.txtContactoTecnico.hide();
    App.txtContactoTecnicoEmail.hide();

    App.numPrimerOcteto.allowBlank = true;
    App.numSegundoOcteto.allowBlank = true;
    App.numTercerOcteto.allowBlank = true;
    App.numCuartoOcteto.allowBlank = true;

    App.txtAPI.allowBlank = true;
    App.txtUserAPI.allowBlank = true;
    App.txtClaveAPI.allowBlank = true;
    App.cmbCliente.allowBlank = true;
    App.cmbVertical.allowBlank = true;

}

function FormularioServerHardware() {

    App.txtURL.show();
    App.txtNombreResponsable.show();
    App.txtEmailResponsable.show();

    App.txtAPI.hide();
    App.txtUserAPI.hide();
    App.txtClaveAPI.hide();
    App.cmbCliente.hide();
    App.cmbVertical.hide();
    App.txtVersion.hide();
    App.txtContactoAdministrativo.hide();
    App.txtContactoAdministrativoEmail.hide();
    App.txtContactoTecnico.hide();
    App.txtContactoTecnicoEmail.hide();

    App.numPrimerOcteto.allowBlank = true;
    App.numSegundoOcteto.allowBlank = true;
    App.numTercerOcteto.allowBlank = true;
    App.numCuartoOcteto.allowBlank = true;

    App.txtAPI.allowBlank = true;
    App.txtUserAPI.allowBlank = true;
    App.txtClaveAPI.allowBlank = true;
    App.cmbCliente.allowBlank = true;
    App.cmbVertical.allowBlank = true;
}

function FormularioServer() {

    App.txtURL.hide();
    App.txtNombreResponsable.hide();
    App.txtEmailResponsable.hide();

    App.txtAPI.show();
    App.txtUserAPI.show();
    App.txtClaveAPI.show();
    App.cmbCliente.show();
    App.cmbVertical.show();
    App.txtVersion.show();
    App.txtContactoAdministrativo.show();
    App.txtContactoAdministrativoEmail.show();
    App.txtContactoTecnico.show();
    App.txtContactoTecnicoEmail.show();

    App.numPrimerOcteto.allowBlank = false;
    App.numSegundoOcteto.allowBlank = false;
    App.numTercerOcteto.allowBlank = false;
    App.numCuartoOcteto.allowBlank = false;

    App.txtAPI.allowBlank = false;
    App.txtUserAPI.allowBlank = false;
    App.txtClaveAPI.allowBlank = false;
    App.cmbCliente.allowBlank = false;
    App.cmbVertical.allowBlank = false;

    App.storeClientes.reload();
    App.storeVerticales.reload();
}

// OCULTAR MENU PPAL

var menState = 0;


function setMini() {
    var menAdm = document.getElementById('mnuAdmistracion');
    setTimeout(function () {
        menAdm.classList.toggle('miniMenu');
    }, 100)
}

function miniMenu() {

    var client = document.getElementById('cliente');
    var infoEntorno = document.getElementById('infoEntorno');
    var asd = document.getElementById('asideLeft');
    var tit = document.querySelectorAll('div .x-title-text');
    var sp = document.querySelectorAll('#mainMenu span');
    var btn = document.getElementById('btnNoAside');
    var btnScrollUp = document.getElementById('mnAdministracion-before-scroller');
    var btnScrollDown = document.getElementById('mnAdministracion-after-scroller');
    var headAdm = document.getElementById('mnuAdmistracion_header');
    var menAdm = document.getElementById('mnuAdmistracion');


    switch (menState) {
        case 0:
            sp.forEach(function (el) {
                el.style.visibility = 'hidden';
            })

            tit.forEach(function (el) {
                el.style.visibility = 'hidden';
            })

            btnScrollUp.style.left = '10%';
            btnScrollDown.style.left = '10%';

            document.getElementById('mnuAdmistracion').classList.add('miniMenu');
            client.style.visibility = 'hidden';
            infoEntorno.style.visibility = 'hidden';
            asd.style.overflowX = 'hidden';
            asd.style.width = '50px';
            btn.style.transform = 'rotate(-180deg)';

            headAdm.addEventListener('click', setMini);

            setTimeout(function () {
                client.style.height = '0px';
            }, 300)


            menState = 1;

            break;

        case 1:
            client.style.height = '90px';
            sp.forEach(function (el) {
                el.style.visibility = 'visible';
            })

            tit.forEach(function (el) {
                el.style.visibility = 'visible';
            })


            asd.style.width = '240px';
            App.mainMenu.maxHeight = '600';



            menAdm.classList.remove('miniMenu');
            headAdm.removeEventListener('click', setMini);


            btnScrollUp.style.left = '50%';
            btnScrollDown.style.left = '50%';
            btn.style.transform = 'rotate(360deg)';

            setTimeout(function () {
                client.style.visibility = 'visible';
                infoEntorno.style.visibility = 'visible';
                asd.style.overflowX = 'visible';
            }, 300);


            menState = 0;
            break;
    }


}

//FIN OCULTAR MENU PPAL

//OCULTAR HEADER

var headState = 0;
function noHeader() {
    var hder = document.getElementById('hdDefault');
    var cont = document.getElementById('ctDefault');
    var ifrm = document.querySelector('#tabPpal iframe');
    var mod = document.getElementById('lblModNoHeader');
    var btn = document.getElementById('btnNoHeader');


    switch (headState) {
        case 0:
            hder.style.display = 'none';
            cont.style.top = '0';
            btn.style.transform = 'rotate(-180deg)';
            mod.style.display = 'block';

            if (ifrm != null) {
                ifrm.style.height = '95vh';
            }
            headState = 1;
            TopContracted = false;
            // CtSizer();
            break;

        case 1:
            hder.style.display = 'flex';
            cont.style.top = '56px';
            btn.style.transform = 'rotate(360deg)';
            mod.style.display = 'none';

            if (ifrm != null) {
                ifrm.style.height = '90vh';
            }
            headState = 0;
            TopContracted = true;
            // CtSizer();

            break;

    }

}

//CONTROL EXTENSION PERMANENTE COMBO SEARCH GLOBAL

function ExtendCombo() {
    var el = document.getElementById("btnSearchTB");
    el.style.width = "300px";
    el.style.opacity = 1;


    var el2 = document.getElementById("btnSearchTB-bodyEl");
    el2.style.opacity = 1;

    App.ComboTipoSearch.show();



    //CAMBIAR EL ICONO A CRUZ DURANTE LA EXTENSION DEL COMBO

    document.getElementById("icosearchtopbar").src = "/ima/ico-close-topbar.svg";



}


function HideCombo() {

    var el = document.getElementById("btnSearchTB");


    el.style.width = "0px";
    el.style.opacity = 0;


    var el2 = document.getElementById("btnSearchTB-bodyEl");
    el2.style.opacity = 0;

    App.ComboTipoSearch.hide();
    App.btnSearchTB.clearValue();
    App.ComboTipoSearch.clearValue();

    //RESET DEL ICO AL DEFECTO

    var eldef = document.getElementById("ComboTipoSearch-trigger-picker");

    var icoClass = "ico-columnas-yesno";

    var urlstring = "url('/ima/" + icoClass + ".svg";

    eldef.style.backgroundImage = urlstring;

    document.getElementById("icosearchtopbar").src = "/ima/ico-search-topbar.svg";


}


function UpdateIcon() {

    //AQUI CAMBIAMOS EL ICON SELECCIONADO DEL COMBO AL DEL ITEM QUE PICAMOS

    var el = document.getElementById("ComboTipoSearch-trigger-picker");

    var icoClass = App.ComboTipoSearch.getValue();

    var urlstring = "url('/ima/" + icoClass + ".svg";

    el.style.backgroundImage = urlstring;
}




function CtSizer() {


    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

    //const offsetHeight = document.getElementById('vwResp').offsetHeight;

    var calcdH = vh;


    var test = Ext.get('grdTask').set({ value: 'ss' });
    //PANELES A CONTROLAR
    App.grdTask.setHeight = calcdH;

    // RECALC POR LAS TOOLBARS
    App.grdTask.height = calcdH - 300;
    App.grAsR1.height = calcdH - 190;

}