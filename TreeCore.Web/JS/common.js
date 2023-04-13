//#region Control Sesion

var Ausente = false;
var AusenteTimer;
var LogoutTimer;

var NombreCookie = "VentanasTree";
var NombreCookieExit = "VentanasTreeExit";

$(document).ready(function ConfigurarToast() {
    window.onfocus = function () {
        //parent.EntraFocus();
    };

    window.onblur = function () {
        //parent.SaleFocus();
    };

})

//#region Cookie Sesiones

function EntraFocus() {
    parent.EntraFocus();
};

function SaleFocus() {
    parent.SaleFocus();
};

//#endregion

//#endregion

function addTab(tabPanel, id, opcion, url, menuItem) {
    var tab = Ext.getCmp(id);
    if (!tab) {
        tab = tabPanel.add({
            id: id,
            title: opcion,
            closable: true,
            menuItem: menuItem,
            loader: {
                renderer: "frame",
                url: url,
                loadMask: {
                    showMask: true,
                    msg: jsCargando
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
}

function addTabUnClosable(tabPanel, id, opcion, url, menuItem) {
    var tab = Ext.getCmp(id);
    if (!tab) {
        tab = tabPanel.add({
            id: id,
            title: opcion,
            closable: false,
            menuItem: menuItem,
            loader: {
                renderer: "frame",
                url: url,
                loadMask: {
                    showMask: true,
                    msg: jsCargando
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
}

function addTabUnclosable(tabPanel, id, opcion, url, menuItem) {
    var tab = Ext.getCmp(id);
    if (!tab) {
        tab = tabPanel.add({
            id: id,
            title: opcion,
            cls: 'unclosable-fullw',
            closable: false,
            menuItem: menuItem,
            loader: {
                renderer: "frame",
                url: url,
                loadMask: {
                    showMask: true,
                    msg: jsCargando
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
}

function addTabFromGlobal(tabPanel, id, opcion, url, menuItem) {
    var tab = Ext.getCmp(id);
    var tbPn = Ext.getCmp('tabPpal');
    if (!tab) {
        tab = tbPn.add({
            id: id,
            title: opcion,
            closable: true,
            menuItem: menuItem,
            loader: {
                renderer: "frame",
                url: url,
                loadMask: {
                    showMask: true,
                    msg: jsCargando
                }
            }
        });
    }
    tbPn.setActiveTab(tab);
}

function OpenWindowWithPost(url, windowoption, name, params) {

    var form = document.createElement("form");
    form.setAttribute("method", "post");

    if (params != undefined && params != "") {
        form.setAttribute("action", url + '?openFavTab=' + params);
    }
    else {
        form.setAttribute("action", url);
    }

    form.setAttribute("target", name);

    document.body.appendChild(form);
    window.open("post.htm", name, windowoption);
    form.submit();
    document.body.removeChild(form);

}

function registroSeleccionado(grid) {
    var hay = grid.getSelectionModel().hasSelection();
    if (!hay) {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: App.jsMensajeSeleccionado, buttons: Ext.Msg.OK });
    }
    return hay;
}

function formatDate(date) {
    let dat = new Date(date);
    let day = dat.getDate();
    let month = dat.getMonth() + 1;
    let year = dat.getFullYear();

    if (month < 10) {
        return (`${day}/0${month}/${year}`);
    } else {
        return (`${day}/${month}/${year}`);
    }
}

//#region Renders

function RenderColDinamica(valor, sender, data) {
    let value = data.data;
    sender.column.indice.split('_').forEach(x => {
        value = value[x];
    });
    let render;
    switch (sender.column.tipo) {
        case 'Text':
            render = RenderColText(value);
            break;
        case 'Date':
            render = RenderColDate(value);
            break;
        case 'Bool':
            render = RenderColBool(value);
            break;
        default:
    }
    return render
}

function RenderColText(valor) {
    return valor;
}

function RenderColDate(valor) {
    if (isNaN(Date.parse(valor)) || valor.length < 19) {
        return valor;
    } else {
        return formatDate(Date.parse(valor));
    }
}

function RenderColBool(valor) {
    if (valor == true || valor == 'true') {
        return '<span class="renderActivo" >&nbsp;</span>';
    }
    else {
        return '<span class="renderNoActivo" >&nbsp;</span>';
    }
}

//#endregion

var LaunchRender = function (value) {

    if (value == true || value == 1) {
        return '<span class="ico-launchGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var GoRender = function (value) {

    if (value == true || value == 1) {
        return '<span class="ico-gotopageGrGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-accept.svg';
    }



}

function renderMultiflow(valor, id) {

    let imag = document.getElementById('imMultiflow' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '/ima/ico-subprocess.svg';
    }


}

function renderCommercial(valor, id) {

    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-vendor.svg';

    }


}

function renderReport(valor, id) {
    let imag = document.getElementById('imReport' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-invisible.svg';

    }


}

function renderContacts(valor, id) {
    let imag = document.getElementById('imContacts' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-contacts-gr.svg';

    }


}

function renderActive(valor, id) {
    let imag = document.getElementById('imActive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-cancel.svg';

    }


}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-cancel.svg';

    }


}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '/ima/ico-region-gr.svg';

    }
}

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '/ima/ico-cancel.svg';
    }

    else {
        imag.src = '/ima/ico-accept.svg';

    }


}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '/ima/ico-cancel.svg';
    }

    else {
        imag.src = '/ima/ico-accept.svg';

    }


}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '/ima/ico-cancel.svg';
    }

    else {
        imag.src = '/ima/ico-accept.svg';

    }

}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '/ima/ico-cancel.svg';
    }

    else {
        imag.src = '/ima/ico-accept.svg';

    }

}

var TrueFalseWHeightRender = function (value) {

    if (value == true || value == 1) {

        return '<span class="gen_activowHeight" >&nbsp;</span>';
    }
    else {
        return '<span class="gen_noactivowHeight" >&nbsp;</span>';
    }
}

var ActivoRender = function (value) {

    if (value == true || value == 1) {

        return '<span class="gen_activo" >&nbsp;</span>';
    }
    else {
        return '<span class="gen_noactivo" >&nbsp;</span>';
    }
}

var InactivoRender = function (value) {

    if (value == true || value == 1) {

        return '<span class="" >&nbsp;</span>';
    }
    else {
        return '<span class="gen_Inactivo" >&nbsp;</span>';
    }
}





// Renders Grid
var barGrid = function (value) {

    let colorBar;

    if (value > 0 && value < 0.20) {
        colorBar = 'barRed-grid';
    }
    else if (value >= 0.20 && value < 0.45) {
        colorBar = 'barYellow-grid';
    }

    else if (value >= 0.45 && value < 0.80) {
        colorBar = 'barBlue-grid';
    }

    else if (value >= 0.80 && value <= 1) {
        colorBar = 'barGreen-grid';
    }
    return `<div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value * 100}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value * 100}%;"><div class="x-progress-text" style="width:40px;"><div>${value * 100} %</div></div></div></div>`

}

var barGridEstados = function (value) {

    let colorBar;

    if (value > 0 && value < 20) {
        colorBar = 'barRed-grid';
    }
    else if (value >= 20 && value < 45) {
        colorBar = 'barYellow-grid';
    }

    else if (value >= 45 && value < 80) {
        colorBar = 'barBlue-grid';
    }

    else if (value >= 80 && value <= 100) {
        colorBar = 'barGreen-grid';
    }
    return `<div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value}%;"><div class="x-progress-text" style="width:40px;"><div>${value} %</div></div></div></div>`

}

var rojoRender = function (value) {
    let valorRojo = value;

    if (value != null || value != "" && value != undefined) {
        return '<span class="dataRed">' + valorRojo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var amarilloRender = function (value) {
    let valorAmarillo = value;

    if (value != null || value != "" && value != undefined) {
        return '<span class="dataYellow">' + valorAmarillo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}


var DocsRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-docsGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}


var MultiDocsRender = function (value) {
    if (value == "icoexcel") {
        return '<span class="DocExcel" style="background-position: center center !important;">&nbsp;</span>'
    }

    else if (value == "icoword") {

        return '<span class="DocWord" style="background-position: center center !important;">&nbsp;</span>'

    }

    else if (value == "icopdf") {

        return '<span class="DocPDF" style="background-position: center center !important;">&nbsp;</span>'

    }
    else if (value == "icopp") {

        return '<span class="DocPowerPoint" style="background-position: center center !important;">&nbsp;</span>'

    }
    else if (value == "icoother") {

        return '<span class="DocOther" style="background-position: center center !important;">&nbsp;</span>'

    }
    else {
        return '<span>&nbsp;</span> '
    }
}



var FunctRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-functionalityGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var LinkRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-linkGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var NotifRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-notificationGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var SubprocessRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-subprocessGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

// FIN Renders Grid





String.prototype.startsWith = function (s) { if (this.indexOf(s) == 0) return true; return false; }

// INICIO EXPORTACION

function ExportarDatosSinCliente(pagina, grid, aux, aux2, aux3, aux4) {
    if (aux2 == undefined) {
        aux2 = "EXPORTAR";
    }
    if (aux2 == "") aux2 = "EXPORTAR";

    var orden = Orden(grid);
    var dir = Direccion(grid);
    var filtro = Filtros(grid);

    if (filtro != "undefined" && filtro != "") {
        var datos = JSON.parse(filtro);

        if (datos[0].type == 'date') {
            var valor = datos[0].value.split('/');
            var valorf = valor.slice();
            valorf[0] = valor[1];
            valorf[1] = valor[0];
            datos[0].value = valorf.join('/');
            filtro = JSON.stringify(datos);
        }
    }

    window.open(pagina + ".aspx?opcion=" + aux2 + "&orden=" + orden + "&dir=" + dir + "&filtro=" + filtro + "&aux=" + aux + "&aux3=" + aux3 + "&aux4=" + aux4);
}

function ExportarDatos(pagina, cliente, grid, aux, opcion, aux3, aux4, aux5, aux6) {
    if (opcion == undefined) {
        opcion = "EXPORTAR";
    }
    if (opcion == "") opcion = "EXPORTAR";

    var orden = Orden(grid);
    var dir = Direccion(grid);
    var filtro = Filtros(grid);

    if (filtro != "") {
        var datos = JSON.parse(filtro);

        if (datos[0].type == 'date') {
            var valor = datos[0].value.split('/');
            var valorf = valor.slice();
            valorf[0] = valor[1];
            valorf[1] = valor[0];
            datos[0].value = valorf.join('/');
            filtro = JSON.stringify(datos);
        }
    }

    window.open(pagina + ".aspx?opcion=" + opcion + "&orden=" + orden + "&cliente=" + cliente + "&dir=" + dir + "&filtro=" + filtro + "&aux=" + aux + "&aux3=" + aux3 + "&aux4=" + aux4 + "&aux5=" + aux5 + "&aux6=" + aux6);
}

function ExportarDatosSinFiltros(pagina, cliente, grid, aux, aux2, aux3, aux4) {
    if (aux2 == undefined) {
        aux2 = "EXPORTAR";
    }
    if (aux2 == "") aux2 = "EXPORTAR";

    var orden = Orden(grid);
    var dir = Direccion(grid);
    var filtro = "";

    if (filtro != "") {
        var datos = JSON.parse(filtro);

        if (datos[0].type == 'date') {
            var valor = datos[0].value.split('/');
            var valorf = valor.slice();
            valorf[0] = valor[1];
            valorf[1] = valor[0];
            datos[0].value = valorf.join('/');
            filtro = JSON.stringify(datos);
        }
    }

    window.open(pagina + ".aspx?opcion=" + aux2 + "&orden=" + orden + "&cliente=" + cliente + "&dir=" + dir + "&filtro=" + filtro + "&aux=" + aux + "&aux3=" + aux3 + "&aux4=" + aux4);
}

function Filtros(grid) {

    var filters = grid.store.getFilters().items;
    var out = [];
    var i;
    if (filters.length > 0) {
        for (i = 0; i < filters.length; i++) {
            out[i] = filters[i].serialize();
        }

        return Ext.util.JSON.encode(out);
    }
    else {
        return "";
    }
}

function Orden(grid) {
    if (grid.store.sorters.items.length > 0) {
        return grid.store.sorters.items[0].config.property;
    }
    else {
        return "";
    }

}

function Direccion(grid) {
    if (grid.store.sorters.items.length > 0) {
        return grid.store.sorters.items[0]._direction;
    }
    else {
        return "";
    }
}

// FIN EXPORTACION


var template = '<span style="color:{0};">{1}</span>';

var change = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value);
};

var pctChange = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value + "%");
};

function ExtensionPermitida(extensionArchivo, ExtensionesPermitidas) {
    var Extensiones = ExtensionesPermitidas.split(",");
    var extension = extensionArchivo.split(".");
    var encontrada = false;

    for (i = 0; i < Extensiones.length; i++) {
        if (Extensiones[i].toLowerCase() == extension[extension.length - 1].toLowerCase()) {
            encontrada = true;
            return encontrada;
        }
    }
    return encontrada;
}

//Carga los stores en el orden es cual se los pasas
function CargarStoresSerie(stores, callback) {
    var loadedStores = 0;
    stores.forEach(function (storeCur, index, storearray) {

        storeCur.load({
            callback: function (r, options, success) {
                if (success === true) {
                    loadedStores++;
                    if (loadedStores == stores.length) {
                        callback(true, null);
                    }
                }
            }
        });
    });
}

//Muestra el mensaje de cargando en el panel expecificado. Devuelve en el callback el objeto LoadMask para ocultar el mensaje.
function showLoadMask(panel, callback) {
    var myMask = new Ext.LoadMask({
        msg: jsCargando,
        target: panel
    });

    myMask.show();
    callback(myMask, null);
}

//Recarga los combos y sus stores en orden
function recargarCombos(combos, callback) {
    var loadCombos = 0;
    combos.forEach(combo => {
        combo.reset();
        combo.getTrigger(0).hide();
        combo.store.load({
            callback: function (r, options, success) {
                if (success === true) {
                    loadCombos++;
                    if (loadCombos == combos.length) {
                        try {
                            callback(true, null);
                        } catch (e) {

                        }
                    }
                }
            }
        });
    });
}

function recargarCombosSinLimpiarValor(combos, callback) {
    var loadCombos = 0;
    combos.forEach(combo => {
        combo.getTrigger(0).hide();
        combo.store.load({
            callback: function (r, options, success) {
                if (success === true) {
                    loadCombos++;
                    if (loadCombos == combos.length) {
                        try {
                            callback(true, null);
                        } catch (e) {

                        }
                    }
                }
            }
        });
    });
}


function RecargarCombosComponente(componente, callback) {
    recargarCombos(componente.items.items[0].items.items, function Fin(Fin) {
        if (Fin) {
            callback(true, null);
        }
    });
}
function RecargarCombosComponenteV2(componente, callback) {
    recargarCombos(componente.items.item, function Fin(Fin) {
        if (Fin) {
            callback(true, null);
        }
    });
}



//Render Boolean Grid
var DefectoRender = function (value) {
    if (value == true || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var PublicoPrivadoRender = function (value) {
    if (value == "true" || value == 1) {
        return '<span class="ico-publico">&nbsp;</span>';
    }
    else {
        return '<span class="ico-privado">&nbsp;</span>';
    }
}

function setClienteIDComponentes() {
    document.getElementsByName('hdClienteIDComponente').forEach(element =>
        element.value = App.hdCliID.value);
}

/**
 * 
 * @param {string} url La ruta del archivo que se requiere cargar
 * @param {any} callback si necesitas un 
 */
function AñadirScriptjs(url, callback) {
    var head = document.getElementsByTagName('head')[0];
    var existe = false;
    head.childNodes.forEach(js => {
        if (js.tagName == 'SCRIPT' && js.src.split('/').pop() == url.split('/').pop()) {
            existe = true;
        }
    });
    if (!existe) {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        try {
            script.onreadystatechange = callback;
            script.onload = callback;
        } catch (e) {

        }
        head.appendChild(script);
    } else {
        if (callback !== undefined) {
            callback();
        }
    }
}
function AnadirScriptjs(url, callback) {
    var head = document.getElementsByTagName('head')[0];
    var existe = false;
    head.childNodes.forEach(js => {
        if (js.tagName == 'SCRIPT' && js.src.split('/').pop() == url.split('/').pop()) {
            existe = true;
        }
    });
    if (!existe) {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        try {
            script.onreadystatechange = callback;
            script.onload = callback;
        } catch (e) {

        }
        head.appendChild(script);
    } else {
        if (callback !== undefined) {
            callback();
        }
    }
}

/**
 * 
 * @param {Ext.net.Componente} object Se necesita que se le envie un objeto que pertenezca el componentente el cual quieras saber el id.
 */
function getIdComponente(object) {
    var ruta;
    if (object != undefined) {
        ruta = object.config.id.split('_');
        ruta.pop();
        ruta = ruta.join('_');
    }
    else {
        ruta = '_';
    }
    return ruta;
}

/**
 * 
 * @param {Ext.net.Componente} object Se necesita que se le envie un objeto que pertenezca el componentente el cual quieras saber el id.
 */
function getIdComponentePadre(object) {
    var ruta = object.config.id.split('_');
    ruta.pop();
    ruta.pop();
    return ruta.join('_');
}

/**
 *
 * @param {Ext.net.Componente} object Se necesita que se le envie un store que pertenezca el componentente el cual quieras saber el id.
 */
function getIdStore(object) {
    var ruta = object.config.storeId.split('_');
    ruta.pop();
    return ruta.join('_');
}

function RecargarCombo(sender, registro, index) {
    recargarCombos([sender]);
}

function SeleccionarCombo(sender, registro, index) {
    sender.getTrigger(0).show();
}

function LimpiarFormularioVentana(ventana) {
    Ext.each(ventana.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            c.reset();
        }
    });
}

// #region Layout Responsive

function DisplayBtnsSliders() {
    let res = window.innerWidth;

    setTimeout(function () {
        let cols = document.querySelectorAll('.col');
        let btnsCt = document.getElementById('ctBtnSldr');

        if (cols.length >= 3 && res <= 991) {
            btnsCt.style.display = 'block';
        }

        else if (cols.length >= 2 && res <= 576) {
            btnsCt.style.display = 'block';
        }

    }, 200);

}

var stPn = (stPn == undefined) ? 0 : stPn;
function hidePn() {
    let pn = document.getElementById('pnAsideR');
    let btn = document.getElementById('btnCollapseAsR');

    if (stPn == 0) {
        pn.style.marginRight = '0';
        btn.style.transform = 'rotate(-180deg)';
        stPn = 1;
    }
    else {
        pn.style.marginRight = '-360px';
        btn.style.transform = 'rotate(0deg)';
        stPn = 0;
    }

}




var stSldr = 0;
var stSldrMbl = 0;
function moveCtSldr(btn) {
    var cols = document.querySelectorAll('.col');
    var columns1 = document.querySelector('.colCt1');
    var idCol1 = columns1.id;
    var Comp1 = Ext.getCmp(idCol1);

    if (cols.length >= 2) {
        var columns2 = document.querySelector('.colCt2');
        var idCol2 = columns2.id;
        var Comp2 = Ext.getCmp(idCol2);
    }



    var btnPrssd = btn.id;
    var res = window.innerWidth;


    if (res > 576) {
        if (btnPrssd == 'btnNextSldr' && cols.length >= 2) {
            Comp2.setHidden(true);
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();

        }

        else if (btnPrssd == 'btnPrevSldr' && cols.length >= 2) {
            Comp2.setHidden(false);
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

        }


    }

    else if (res <= 576) {

        if (stSldrMbl == 0 && btnPrssd == 'btnNextSldr') {
            Comp1.setHidden(true);
            if (cols.length >= 3) {
                App.btnNextSldr.enable();
                App.btnPrevSldr.enable();
            }
            else {
                App.btnNextSldr.disable();
                App.btnPrevSldr.enable();
            }

            stSldrMbl = 1;

        }

        else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr' && cols.length >= 2) {
            Comp2.setHidden(true);
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();
            stSldrMbl = 2;

        }


        else if (stSldrMbl == 1 && btnPrssd == 'btnPrevSldr') {
            Comp1.setHidden(false);
            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();
            stSldrMbl = 0;

        }

        else if (stSldrMbl == 2 && btnPrssd == 'btnPrevSldr' && cols.length >= 2) {
            Comp2.setHidden(false);
            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();
            stSldrMbl = 1;

        }


    }


}
// #endregion


// #region Descarga Documentos

var DescargaDocumentoRender = function (value, otro, record) {

    var docID = record.get("DocumentoID");

    if ((docID != null) && (docID != '')) {
        var Plantilla = '<a ext:qtip="{0}" href="PaginasComunes.aspx?DocumentoID={1}" class="gen_Documentos_img">&nbsp;</a>';
        return String.format(Plantilla, docID);
    }
    else {
        return ""
    }
}

// #endregion

function setSearchEvent(sender, registro, index) {
    this.items.items[0].on('click', RefreshSearchEvent());
    this.items.items[1].on('click', RefreshSearchEvent());
    this.items.items[7].on('click', RefreshSearchEvent());
    this.items.items[8].on('click', RefreshSearchEvent());
}


function SetStatusBarText(value) {
    var num = value.text.split(' ', 1);

    if (num[0] != "Nothing") { App.StatusBar.setText(num[0] + " " + jsStatusBar); }
    else { App.StatusBar.setText(jsStatusBarNothingFound); }
}

function RefreshSearchEvent() {

    var p = App.grid.liveSearchPlugin;
    if (p.searchValue) {
        p.getSearchValue(p.searchValue);
    }
}

var ComboBoxKeyUp = function (combo, e) {
    var v = combo.getRawValue();
    combo.store.filter(combo.displayField, new RegExp(v));
    combo.onLoad();
};

function getActualXY(item, event) {
    var x = event.pageX;
    var y = event.pageY;
    var w = item.up();
    if (w) {
        x -= w.getX();
        y -= w.getY();
    }
    return [x, y];
}

// #region *** CONVERTIR GRADOS ***
/**
* Convierte una coordenada en formato decimal a su correspondiente
* versión en formato sexagesimal (grados-minutos-segundos).
*
* @param	Float	Valor real de la coordenada.
* @param	Int	Tipo de la coordenada {Coordenada.LATITUD, Coordenada.LONGITUD}.
* @return	Array	['grados', 'minutos', 'segundos', 'direccion', 'valor'].
*/

function dec2gms(valor, tipo) {
    var resultado = "";
    grados = Math.abs(parseInt(valor));
    minutos = (Math.abs(valor) - grados) * 60;
    segundos = minutos;
    minutos = Math.abs(parseInt(minutos));
    segundos = Math.round((segundos - minutos) * 60 * 1000000) / 1000000;
    signo = (valor < 0) ? -1 : 1; direccion = (tipo == "LATITUD") ?
        ((signo > 0) ? 'N' : 'S') :
        ((signo > 0) ? 'E' : 'W');

    //if (isNaN(direccion)) {
    //    grados = grados * signo;
    //}

    resultado = grados + "$" + minutos + "$" + segundos + "$" + direccion;

    return resultado;
}

/**
* Convierte una coordenada en formato sexagesimal (grados-minutos-segundos) a su
* correspondiente versión en formato decimal.
*
* @param	Float	Grados de la coordenada.
* @param	Float	Minutos de la coordenada.
* @param	Float	Segundos de la coordenada.
* @param	String	Sentido de la coordenada {Coordenada.NORTE,
Coordenada.SUR, Coordenada.ORIENTE,
Coordenada.OCCIDENTE}
* @return	Array	['decimal', 'valor'].
*/

function gms2dec(grados, minutos, segundos, direccion) {
    let dec = "";

    if (direccion) {
        signo = (direccion.toLowerCase() == 'w' || direccion.toLowerCase() == 's') ? -1 : 1;
        direccion = (direccion.toLowerCase() == 'w' ||
            direccion.toLowerCase() == 's' ||
            direccion.toLowerCase() == 'n' ||
            direccion.toLowerCase() == 'e') ?
            direccion.toLowerCase() : '';
    }
    else {
        signo = (grados < 0) ? -1 : 1;
        direccion = '';
    }

    dec = Math.round((Math.abs(grados) + ((minutos * 60) + segundos) / 3600) * 1000000) / 1000000;

    if (isNaN(direccion) || direccion == '')
        dec = dec * signo;

    return dec;
}
// #endregion *** FIN CONVERTIR GRADOS ***

function OcultarContenedorPadre(sender) {
    document.getElementById(sender.id).parentNode.style.display = 'none';
}

function MostrarContenedorPadre(sender) {
    document.getElementById(sender.id).parentNode.style.display = 'flex';
}

function MostrarContenedorPadreForm(sender) {
    document.getElementById(sender.id).parentNode.style.display = 'block';
}

function OcultarContenedorPadreForm(sender) {
    if (App[sender.id].hidden) {
        App[sender.id].container.dom.style.display = 'none';
    }
}

// FUNCION PARA VISUALIZAR RECUADRO ROJO EN FORMULARIO
function anadirClsNoValido(sender, registro, index) {

    if (registro != null && registro != undefined) {
        if (registro.fromComponent != undefined) {

            if (App[registro.fromComponent.id] != undefined) {
                if ((App[registro.fromComponent.id].value == 0 || App[registro.fromComponent.id].value != "") && App[registro.fromComponent.id].value != null && App[registro.fromComponent.id].isValid() &&
                    ((sender.xtype != "combobox" && sender.xtype != "multicombo" && sender.xtype != "combo") || (sender.allowBlank) || (sender.selection != null && sender.rawValue == sender.selection.data[sender.displayField]))) {
                    App[registro.fromComponent.id].removeCls("ico-exclamacion-10px-red");
                    App[registro.fromComponent.id].addCls("ico-exclamacion-10px-grey");

                    if (App[registro.fromComponent.id].triggerWrap != undefined) {
                        App[registro.fromComponent.id].triggerWrap.removeCls("itemForm-novalid");
                        App[registro.fromComponent.id].triggerWrap.addCls("itemForm-valid");
                    }

                    if (App[registro.fromComponent.id].cls == 'txtContainerCategorias') {
                        App[getIdComponente(registro.fromComponent) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-red");
                        App[getIdComponente(registro.fromComponent) + "_lbNombreAtr"].addCls("ico-exclamacion-10px-grey");
                    }
                }
                else {
                    App[registro.fromComponent.id].removeCls("ico-exclamacion-10px-grey");
                    App[registro.fromComponent.id].addCls("ico-exclamacion-10px-red");

                    if (App[registro.fromComponent.id].triggerWrap != undefined) {
                        App[registro.fromComponent.id].triggerWrap.addCls("itemForm-novalid");
                        App[registro.fromComponent.id].triggerWrap.removeCls("itemForm-valid");
                    }

                    if (App[registro.fromComponent.id].cls == 'txtContainerCategorias') {
                        App[getIdComponente(registro.fromComponent) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                        App[getIdComponente(registro.fromComponent) + "_lbNombreAtr"].addCls("ico-exclamacion-10px-red");
                    }
                }
            }
        }
        else {
            if (App[sender.id] != undefined) {
                if ((App[sender.id].value == 0 || App[sender.id].value != "") && App[sender.id].value != null && App[sender.id].isValid() &&
                    ((sender.xtype != "combobox" && sender.xtype != "multicombo" && sender.xtype != "combo") || (sender.allowBlank) || (sender.selection != null && sender.rawValue == sender.selection.data[sender.displayField]))) {
                    App[sender.id].removeCls("ico-exclamacion-10px-red");
                    App[sender.id].addCls("ico-exclamacion-10px-grey");

                    if (App[sender.id].triggerWrap != undefined) {
                        App[sender.id].triggerWrap.removeCls("itemForm-novalid");
                    }
                } else {
                    App[sender.id].removeCls("ico-exclamacion-10px-grey");
                    App[sender.id].addCls("ico-exclamacion-10px-red");
                }
            }
        }

        if (App[sender.id] != undefined) {
            if (!sender.id.includes('hidden') && !App[sender.id].isValid() && (App[sender.id].value == undefined || App[sender.id].value == "")) {
                App[sender.id].clearInvalid();
            }
        }
    }
}

// FUNCION BORRAR FILTROS COMUN

function BorrarFiltros(grid) {
    var gridID = grid.id.split('_')[0];
    var tree = App[gridID + "_" + grid.value];

    if (tree == undefined) {
        if (grid.value != undefined) {
            var tree = App[grid.value];
        }
        else {
            var tree = App[grid.id];
        }
    }

    var store = tree.store;
    tree.filters.clearFilters();
    store.clearFilter();

    if (App[gridID + "_" + store.shearchBox] != undefined) {
        App[gridID + "_" + store.shearchBox].setValue("");
    }

    else if (App[store.shearchBox] != undefined) {
        App[store.shearchBox].setValue("");
    }

    if (tree.filters.activeFilterMenuItem != undefined) {
        tree.filters.activeFilterMenuItem.setChecked(null);
    }
}


// #region Buscador predictivo
var dataArray = [];
var storeIDtemp;
var idComponente;
var forzarCargaBuscadorPredictivo = false;
function BuscadorPredictivo(sender, registro) {
    /*
     * Esta función para que funcione debe ser llamada por un store.
     * El store que lanza la función debe tener las siguientes propiedades:
     * - "shearchBox" con el ID de la caja de busqueda
     * - "listNotPredictive" listado separado por coma con las propiedades que se quieren excluir del buscador
     * - dentro del textField de "txtSearch", hay que llamar a la función "FiltrarColumnas" de la página del store con la opción "Change"
     * - dentro de la función "FiltrarColumnas" de la página del store, hay que llamar a filtroBuscador
     * Cuando agregamos, editamos o eliminamos elementos del grid, antes de recargar este debemos poner la variable "forzarCargaBuscadorPredictivo = true;".
     * */

    if (sender.storeId != null && (dataArray.length == 0 || storeIDtemp != sender.storeId) || forzarCargaBuscadorPredictivo) {
        storeIDtemp = sender.storeId;
        dataArray = [];

        if (sender.storeId.includes('_')) {
            idComponente = sender.storeId.split('_');
            idComponente.pop();
        }
        else {
            idComponente = "";
        }


        let nameSearchBox;
        try {
            nameSearchBox = registro.fn.arguments[0].shearchBox;
        }
        catch (error) {
            nameSearchBox = "txtSearch";
        }

        let listNotPredictive;
        try {
            let temp = registro.fn.arguments[0].listNotPredictive.split(',');
            listNotPredictive = temp;
        }
        catch (err) {
            listNotPredictive = [
                "EmplazamientoID",
                "ClienteID"
            ];
        }

        let items;
        if (sender.allData != undefined) {
            items = sender.allData.items;
        } else if (sender.data.items) {
            items = sender.data.items;
        }
        var control;
        items.forEach(i => {
            Object.entries(i.data).forEach(p => {
                if (!listNotPredictive.includes(p[0])) {
                    if ((p[1] === true || p[1] === false) && p[1] != "" && p[1] != null) {
                        try {
                            p[1] = eval('js' + p[0]);
                            control = true;
                            for (var i = 0; i < dataArray.length; i++) {

                                if (dataArray[i].key.includes(p[1])) {
                                    control = false;
                                }
                            }
                            if (control) {
                                dataArray.push({
                                    key: p[1],
                                    value: p[1]
                                });
                            }
                        } catch (error) {
                            console.error(error);
                        }


                    } else if (p[1] != "" && p[1] != null && !dataArray.some(a => a.key === p[1].toString().toLowerCase())) {
                        dataArray.push({
                            key: p[1].toString().toLowerCase(),
                            value: p[1].toString()
                        });
                    }
                }

            });
        });

        dataArray = dataArray.sort(function (a, b) {
            return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
        });

        if (idComponente != "") {
            var selectorSearchBox = `#${idComponente}_${nameSearchBox}-inputEl`;
        }
        else {
            var selectorSearchBox = `#${nameSearchBox}-inputEl`;
        }

        $(function () {
            let textBuscado = "";
            $(selectorSearchBox).autocomplete({
                source: function (request, response) {
                    textBuscado = request.term;
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                    let results = $.grep(dataArray, function (value) {
                        value = value.key;

                        return matcher.test(value) || matcher.test(normalize(value));
                    });

                    response(results.slice(0, 10));

                    $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                        if (idComponente != "") {
                            App[`${idComponente}_${nameSearchBox}`].setValue(e.toElement.textContent);
                        }
                        else {
                            if (e.toElement != undefined) {
                                App[`${nameSearchBox}`].setValue(e.toElement.textContent);
                            }
                        }

                    });
                }
            }).autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div>" + boldQuery(item.label, textBuscado) + "</div>")
                    .appendTo(ul);
            };
        });
    }

    forzarCargaBuscadorPredictivo = false;
}

function boldString(str, substr) {
    var strRegExp = new RegExp(substr, 'i');
    return str.replace(strRegExp, '<b>' + substr + '</b>');
}

function boldQuery(str, query) {
    const n = str.toUpperCase();
    const q = query.toUpperCase();
    const x = n.indexOf(q);
    if (!q || x === -1) {
        return str; // bail early
    }
    const l = q.length;
    return str.substr(0, x) + '<b>' + str.substr(x, l) + '</b>' + str.substr(x + l);
}

var accentMap = {
    //"": "a", "": "e", "": "i", "": "o", "": "u",
    //Minusculas
    "á": "a", "é": "e", "í": "i", "ó": "o", "ú": "u",
    "à": "a", "è": "e", "i": "i", "ò": "o", "ù": "u",
    "ä": "a", "ë": "e", "ï": "i", "ö": "o", "ü": "u",
    "â": "a", "ê": "e", "î": "i", "ô": "o", "û": "u",
    //Mayusculas
    "Á": "a", "É": "e", "Í": "i", "Ó": "o", "Ú": "u",
    "À": "a", "È": "e", "Ì": "i", "Ò": "o", "Ù": "u",
    "Ä": "a", "Ë": "e", "Ï": "i", "Ö": "o", "Ü": "u",
    "Â": "a", "Ê": "e", "Î": "i", "Ô": "o", "Û": "u",
    //Incompleots
    "Å": "a",
    "Ã": "a", "Õ": "o",
    "å": "a",
    "ã": "a", "õ": "o"
};

var normalize = function (term) {
    var ret = "";
    if (term != undefined) {
        for (var i = 0; i < term.toString().length; i++) {
            ret += accentMap[term.toString().charAt(i)] || term.toString().charAt(i);
        }
    }
    return ret;
};
//var identifierSearch;
function filtroBuscador(store, grid, text) {
    var re;
    try {
        re = new RegExp($.ui.autocomplete.escapeRegex(normalize(text)), "i");
    } catch (err) {
        store.clearFilter();
        return;
    }

    store.filterBy(function (node) {
        var correcto = false;

        if (grid.columnManager.columns != null) {
            grid.columnManager.columns.forEach(valores => {
                if (!correcto) {
                    if (node.data[valores.dataIndex] == true) {
                        try {
                            if (re.test(eval('js' + valores.dataIndex)) || re.test(normalize(eval('js' + valores.dataIndex)))) {
                                correcto = true;
                            }
                        } catch (err) {

                        }

                    } else {
                        if (re.test(node.data[valores.dataIndex]) || re.test(normalize(node.data[valores.dataIndex]))) {
                            correcto = true;
                        }
                    }

                }
            });
        }

        return correcto;
    });

}
/**
 * 
 * Añadir atributo a la caja del buscador con el ID del TreePanel Ej: IdTreePanel="TreePanelV1"
 * Añadir atributo al treePanel con la lista de columnas a omitir Ej; listNotPredictive="EmplazamientoID,ClienteID"
 */
function filterTree(sender, e) {
    var tree = App[sender.idTreePanel],
        text = sender.getRawValue();

    BuscadorPredictivoTreePanel(tree, sender.id);

    tree.clearFilter();
    tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    if (e.getKey() === e.ESC) {
        tree.clearFilter();
    } else {
        try {
            var re = new RegExp($.ui.autocomplete.escapeRegex(normalize(text)), "i");
        } catch (err) {
            return;
        }

        tree.filterBy(function (node) {
            let correcto = false;
            tree.columnManager.columns.forEach(valores => {
                if (!correcto) {
                    correcto = re.test(node.data[valores.dataIndex]) || re.test(normalize(node.data[valores.dataIndex]));
                }
            });
            return correcto;
        });
    }
}

function BuscadorPredictivoTreePanel(sender, nameSearchBox) {
    forzarCargaBuscadorPredictivo = true;
    if (forzarCargaBuscadorPredictivo) {
        dataArray = [];

        let listNotPredictive;
        try {
            let temp = sender.listNotPredictive.split(',');
            listNotPredictive = temp;
        }
        catch (err) {
            listNotPredictive = [
                "EmplazamientoID",
                "ClienteID"
            ];
        }

        let items = [];
        if (sender.getRootNode() != undefined) {
            sender.getRootNode().eachChild(x => {
                if (x.data != undefined) {
                    items.push(x.data);
                }
            });
            //items = sender.allData.items;
        }


        items.forEach(i => {
            Object.entries(i).forEach(p => {

                if (!listNotPredictive.includes(p[0]) && p[1] != "" && p[1] != null && !dataArray.some(a => a.key === p[1].toString().toLowerCase())) {
                    dataArray.push({
                        key: p[1].toString().toLowerCase(),
                        value: p[1].toString()
                    });
                }
            });
        });

        dataArray = dataArray.sort(function (a, b) {
            return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
        });

        if (idComponente != undefined && idComponente != "") {
            var selectorSearchBox = `#${idComponente}_${nameSearchBox}-inputEl`;
        }
        else {
            var selectorSearchBox = `#${nameSearchBox}-inputEl`;
        }

        $(function () {
            let textBuscado = "";
            $(selectorSearchBox).autocomplete({
                source: function (request, response) {
                    textBuscado = request.term;
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                    let results = $.grep(dataArray, function (value) {
                        value = value.key;

                        return matcher.test(value) || matcher.test(normalize(value));
                    });

                    response(results.slice(0, 10));

                    /*$(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                        if (idComponente != undefined && idComponente != "") {
                            App[`${idComponente}_${nameSearchBox}`].setValue(e.toElement.textContent);
                        }
                        else {
                            App[`${nameSearchBox}`].setValue(e.toElement.textContent);
                        }

                    });*/
                }
            }).autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div>" + boldQuery(item.label, textBuscado) + "</div>")
                    .appendTo(ul);
            };
        });


    }

    forzarCargaBuscadorPredictivo = false;
}
// #endregion Fin Filtro buscador predictivo

/**
 * Generador de Codigo
 * @param {any} campoCodigo campo al cual se le va a asignar el codigo
 * @param {any} formulario formulario donde se encuentran los campos que van a afectar al codigo
 * @param {any} pautas pautas de la generacion de codigos
 * @param {any} hdCodigo variable donde se guarda el codigo para autoincrementarlo
 */

function GenerarCodigo(campoCodigo, formulario, pautas, hdCodigo) {
    codigo = '';
    for (var cod in pautas) {
        var pauta = pautas[cod];
        switch (pauta.TipoCondicion) {
            case 'Auto_Numerico':
            case 'Auto_Caracter':
            case 'Constante':
            case 'Separador':
            case 'Tabla':
                codigo += pauta.Valor;
                break;
            case 'Formulario':
                codigoAux = pauta.Valor;
                Ext.each(formulario.body.query('*'), function (item) {
                    var c = Ext.getCmp(item.id);
                    if (c && c.isFormField && c.isValid() && c.config.tabla != undefined) {
                        if (c.config.tabla == pauta.Tabla) {
                            if (c.selection != undefined && c.selection.data[pauta.Campo] != undefined) {
                                if (pauta.Longitud == 0) {
                                    codigoAux = String(c.selection.data[pauta.Campo]);
                                }
                                else {
                                    codigoAux = String(c.selection.data[pauta.Campo].slice(0, pauta.Longitud));
                                }
                            }
                            else if (c.rawValue != undefined && c.rawValue != '') {
                                if (pauta.Longitud == 0) {
                                    codigoAux = String(c.rawValue);
                                }
                                else {
                                    codigoAux = String(c.rawValue.slice(0, pauta.Longitud));
                                }
                            }
                        }
                    }
                });
                codigo += codigoAux;
                break;
            default:
                break;
        }
    }
    if ((hdCodigo.value == '' || hdCodigo.value == campoCodigo.value || campoCodigo.value == '') && codigo != '') {
        hdCodigo.setValue(codigo);
        campoCodigo.setValue(codigo);
        campoCodigo.setEmptyText(codigo);
    }
}

/**
 * Generador de Codigo si existe Duplicidad
 * @param {any} campoCodigo campo al cual se le va a asignar el codigo
 * @param {any} formulario formulario donde se encuentran los campos que van a afectar al codigo
 * @param {any} pautas pautas de la generacion de codigos
 * @param {any} hdCodigo variable donde se guarda el codigo para autoincrementarlo
 */

function GenerarCodigoDuplicado(campoCodigo, formulario, pautas, hdCodigo) {
    codigo = '';
    for (var cod in pautas) {
        var pauta = pautas[cod];
        switch (pauta.TipoCondicion) {
            case 'Auto_Numerico':
            case 'Auto_Caracter':
            case 'Constante':
            case 'Separador':
            case 'Tabla':
                codigo += pauta.Valor;
                break;
            case 'Formulario':
                codigoAux = pauta.Valor;
                Ext.each(formulario.body.query('*'), function (item) {
                    var c = Ext.getCmp(item.id);
                    if (c && c.isFormField && c.isValid() && c.config.tabla != undefined) {
                        if (c.config.tabla == pauta.Tabla) {
                            if (c.selection != undefined && c.selection.data[pauta.Campo] != undefined) {
                                if (pauta.Longitud == 0) {
                                    codigoAux = String(c.selection.data[pauta.Campo]);
                                }
                                else {
                                    codigoAux = String(c.selection.data[pauta.Campo].slice(0, pauta.Longitud));
                                }
                            }
                            else if (c.rawValue != undefined && c.rawValue != '') {
                                if (pauta.Longitud == 0) {
                                    codigoAux = String(c.rawValue);
                                }
                                else {
                                    codigoAux = String(c.rawValue.slice(0, pauta.Longitud));
                                }
                            }
                        }
                    }
                });
                codigo += codigoAux;
                break;
            default:
                break;
        }
    }
    hdCodigo.setValue(codigo);
    campoCodigo.setValue(codigo);
    campoCodigo.setEmptyText(codigo);
}

//#region Drag&Drop
function DragDropCategorias() {
    var attrs = {};
    if ($("#pnConfigurador_Content")[0] != undefined) {
        $.each($("#pnConfigurador_Content")[0].attributes, function (idx, attr) {
            attrs[attr.nodeName] = attr.nodeValue;
        });
        $("#pnConfigurador_Content").replaceWith(function () {
            return $("<ul />", attrs).append($(this).contents());
        });
        $("#pnConfigurador_Content > div").replaceWith(function () {
            var attrs = {};
            $.each($("#pnConfigurador_Content > div"), function (idx, attr) {
                attrs[attr.nodeName] = attr.nodeValue;
            });
            return $("<li />", attrs).append($(this).contents());
        });


        $(function () {
            $("#pnConfigurador_Content").sortable({
                handle: ".ico-drag-vertical",
                pullPlaceholder: false,
                // animation on drop
                isValidTarget: function ($item, container) {
                    if ($item.is(".highlight"))
                        return true;
                    else
                        return $item.parent("ul")[0].id == container.el[0].id;
                },
                onDrop: function ($item, container, _super) {
                    var $clonedItem = $('<li/>').css({ height: 0 });
                    $item.before($clonedItem);
                    $clonedItem.animate({ 'height': $item.height() });

                    $item.animate($clonedItem.position(), function () {
                        $clonedItem.detach();
                        _super($item, container);
                    });
                    CatArriba = X.getCmp($item[0].firstElementChild.id);
                    var order = 0;
                    var ruta;
                    for (var item in document.getElementById('pnConfigurador_Content').children) {
                        if (document.getElementById('pnConfigurador_Content').children[item].tagName == "LI" && document.getElementById('pnConfigurador_Content').children[item].children.length > 0) {
                            ruta = document.getElementById('pnConfigurador_Content').children[item].children[0].id.split('_'); ruta.pop();
                            ruta = ruta.join('_');
                            TreeCore[ruta].MoverElementoOrden(order);
                            order++;
                        }
                    }

                },
                // set $item relative to cursor position
                onDragStart: function ($item, container, _super) {
                    var offset = $item.offset(),
                        pointer = container.rootGroup.pointer;

                    adjustment = {
                        left: pointer.left - offset.left,
                        top: pointer.top - offset.top
                    };

                    _super($item, container);
                },
                onDrag: function ($item, position) {
                    $item.css({
                        left: position.left - adjustment.left,
                        top: (position.top - adjustment.top) + document.getElementById('pnConfigurador-body').scrollTop
                    });
                }
            });
        });
    }
}

function DragDropAtributosCategorias() {
    var attrs = {};
    if ($(".flexContainer [id *= _Content]") != undefined) {
        $(".flexContainer [id *= _Content]").replaceWith(function (idx) {
            var attrs = {};
            var attribute = $('.flexContainer [id *= _Content]')[idx];
            if (attribute != undefined) {
                $.each(attribute.attributes, function (idx, attr) {
                    attrs[attr.nodeName] = attr.nodeValue;
                });
                return $("<ul />", attrs).append($(this).contents());
            }
        });

        atributos = $('.flexContainer [id *= _Content] > div');

        $(".flexContainer [id *= _Content] > div").replaceWith(function (idx) {
            var attrs = {};
            var attribute = atributos[idx];
            if (attribute != undefined) {
                $.each(attribute.attributes, function (idx, attr) {
                    attrs[attr.nodeName] = attr.nodeValue;
                });
                return $("<li />", attrs).append($(this).contents());
            }
        });


        $(function () {
            $(".flexContainer [id *= _Content]").sortable({
                handle: ".ico-drag-horizontal",
                pullPlaceholder: false,
                // animation on drop
                isValidTarget: function ($item, container) {
                    if ($item.is(".highlight"))
                        return true;
                    else
                        return $item.parent("ul")[0].id == container.el[0].id;
                },
                onDrop: function ($item, container, _super) {
                    var $clonedItem = $('<li/>').css({ height: 0 });
                    $item.before($clonedItem);
                    $clonedItem.animate({ 'height': $item.height() });

                    $item.animate($clonedItem.position(), function () {
                        $clonedItem.detach();
                        _super($item, container);
                    });
                    CatArriba = X.getCmp($item[0].firstElementChild.id);
                    var order = 0;
                    var ruta;
                    for (var item in document.getElementById(container.el[0].id).children) {
                        if (document.getElementById(container.el[0].id).children[item].tagName == "LI" && document.getElementById(container.el[0].id).children[item].children.length > 0) {
                            ruta = document.getElementById(container.el[0].id).children[item].children[0].id.split('_'); ruta.pop();
                            ruta = ruta.join('_');
                            TreeCore[ruta].MoverElementoOrden(order);
                            order++;
                        }
                    }
                },
                // set $item relative to cursor position
                onDragStart: function ($item, container, _super) {
                    var offset = $item.offset(),
                        pointer = container.rootGroup.pointer;

                    adjustment = {
                        left: pointer.left - offset.left,
                        top: pointer.top - offset.top
                    };

                    _super($item, container);
                },
                onDrag: function ($item, position) {
                    $item.css({
                        left: position.left - adjustment.left,
                        top: position.top - adjustment.top
                    });
                }
            });
        });
    }

}

//#endregion


//#region PLANTILLA


function GridColHandler(grid) {
    // Con esta variable se controla si la columna more esta visible siempre o no
    var ForceShowColmore = false;

    //Variables de entorno(no editar)
    if (grid != null) {
        var gridW = grid.getWidth();

        const colArray = grid.columns;
        const colArrayNoColMore = grid.columns.slice(0);

        var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
        var AllcolsMinWTotal = 0;
        var visiblecolsMinWTotal = 0;


        // Se crea un array sin la columna More    Se ha cambiado a seleccion por el CLS de la columna que tiene que ser col-More

        if (LastCol.cls == "col-More" || LastCol.cls == "NoOcultar col-More") {
            colArrayNoColMore.pop();
        }

        // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

        colArrayNoColMore.forEach(function (colArrayNoColMore) {
            if (colArrayNoColMore.hidden != true) {
                visiblecolsMinWTotal = visiblecolsMinWTotal + colArrayNoColMore.minWidth;
            }

            AllcolsMinWTotal = AllcolsMinWTotal + colArrayNoColMore.minWidth;


        });


        //Controles de anchura y hide (aqui esta el tema)

        for (let i = 0; i < 18 && visiblecolsMinWTotal <= gridW + 90; i++) {


            var HiddenCols = colArrayNoColMore.filter(x => {
                return x.hidden == true;
            })


            if (HiddenCols.length > 0) {
                var FirstHiddenColIndex = HiddenCols[0].fullColumnIndex;
                grid.columns[FirstHiddenColIndex].show();

                //// Se Suma la anchuraminima del computo de las columnas visibles que Hay

                var minWLastShownCol = HiddenCols[0].minWidth;
                visiblecolsMinWTotal = visiblecolsMinWTotal + minWLastShownCol;
            }


        }


        while (visiblecolsMinWTotal >= gridW - 70) {



            var VisibleCols = colArrayNoColMore.filter(x => {
                return x.hidden != true;
            })



            if (VisibleCols.length > 0) {



                //Se oculta Ultima Columna de las VISIBLES
                var indexUltimaColumna = VisibleCols.length - 1;
                grid.columns[indexUltimaColumna].hide();

                // Se resta la anchuraminima del computo de las columnas visibles que quedan
                var minWLastCol = VisibleCols[VisibleCols.length - 1].minWidth;
                visiblecolsMinWTotal = visiblecolsMinWTotal - minWLastCol;

            } else {
                break
            }


        }



        // #region AQUI SE ESCONDE LA COLUMNA MORE (debe ser la ultima) POR DEFECTO!
        //Index colmore
        var colMoreIndex = grid.columns.length - 1;

        //if (!grid.columns[colMoreIndex].cls == "NoOcultar col-More") {

        if (AllcolsMinWTotal < gridW - 70) {
            if (grid.columns[colMoreIndex].cls != "NoOcultar col-More") {
                grid.columns[colMoreIndex].hide();
            }

        } else if (visiblecolsMinWTotal <= gridW + 90) {
            grid.columns[colMoreIndex].show();

        }

        if (ForceShowColmore == true) {

            grid.columns[colMoreIndex].show();
        }
        //}

        //#endregion

    }


}



function GridColHandlerDinamico(grid) {
    // Con esta variable se controla si la columna more esta visible siempre o no
    var ForceShowColmore = false;

    //Variables de entorno(no editar)
    if (grid.columnManager.getColumns() != null && grid.columnManager.getColumns().length > 0) {
        var gridW = grid.getWidth();

        const colArray = grid.columnManager.getColumns();
        const colArrayNoColMore = grid.columnManager.getColumns().slice(0);

        var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
        var AllcolsMinWTotal = 0;
        var visiblecolsMinWTotal = 0;


        // Se crea un array sin la columna More    Se ha cambiado a seleccion por el CLS de la columna que tiene que ser col-More

        if (LastCol.cls == "col-More" || LastCol.cls == "NoOcultar col-More") {
            colArrayNoColMore.pop();
        }

        // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

        colArrayNoColMore.forEach(function (colArrayNoColMore) {
            if (colArrayNoColMore.hidden != true) {
                visiblecolsMinWTotal = visiblecolsMinWTotal + colArrayNoColMore.minWidth;
            }

            AllcolsMinWTotal = AllcolsMinWTotal + colArrayNoColMore.minWidth;


        });


        //Controles de anchura y hide (aqui esta el tema)

        for (let i = 0; i < 18 && visiblecolsMinWTotal <= gridW + 90; i++) {


            var HiddenCols = colArrayNoColMore.filter(x => {
                return x.hidden == true;
            })


            if (HiddenCols.length > 0) {
                var FirstHiddenColIndex = HiddenCols[0].fullColumnIndex;
                grid.columnManager.getColumns()[FirstHiddenColIndex].show();

                //// Se Suma la anchuraminima del computo de las columnas visibles que Hay

                var minWLastShownCol = HiddenCols[0].minWidth;
                visiblecolsMinWTotal = visiblecolsMinWTotal + minWLastShownCol;
            }


        }

        if (grid.columnManager.getColumns() != null) {

            while (visiblecolsMinWTotal >= gridW - 70) {



                var VisibleCols = colArrayNoColMore.filter(x => {
                    return x.hidden != true;
                })



                if (VisibleCols.length > 0) {



                    //Se oculta Ultima Columna de las VISIBLES
                    var LastVisibleColIndex = VisibleCols.length - 1;
                    grid.columnManager.getColumns()[LastVisibleColIndex].hidden = true;
                    grid.columnManager.getColumns()[LastVisibleColIndex].hide();


                    // Se resta la anchuraminima del computo de las columnas visibles que quedan
                    var minWLastCol = VisibleCols[VisibleCols.length - 1].minWidth
                    visiblecolsMinWTotal = visiblecolsMinWTotal - minWLastCol;

                } else {
                    break
                }


            }


        }



        // #region AQUI SE ESCONDE LA COLUMNA MORE (debe ser la ultima) POR DEFECTO!
        //Index colmore
        var colMoreIndex = grid.columnManager.getColumns().length - 1;



        if (AllcolsMinWTotal < gridW - 70) {
            if (grid.columnManager.getColumns()[colMoreIndex].cls != "NoOcultar col-More") {
                grid.columnManager.getColumns()[colMoreIndex].hide();
            }

        } else if (visiblecolsMinWTotal <= gridW + 90) {
            grid.columnManager.getColumns()[colMoreIndex].show();

        }

        if (ForceShowColmore == true || grid.columnManager.getColumns()[colMoreIndex].cls == "NoOcultar col-More") {

            grid.columnManager.getColumns()[colMoreIndex].show();
        }


        //#endregion

    }


}

function GridColHandlerDinamicoV2(grid) {
    // Con esta variable se controla si la columna more esta visible siempre o no
    var ForceShowColmore = false;
    var AnchoTotal = 0;

    //Variables de entorno(no editar)
    if (grid.columnManager.getColumns() != null && grid.columnManager.getColumns().length > 0) {

        var gridW = grid.getWidth();

        const colArrayNoColMore = grid.columnManager.getColumns().slice(0);

        var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
        var AllcolsMinWTotal = 0;
        var visiblecolsMinWTotal = 0;


        // Se crea un array sin la columna More    Se ha cambiado a seleccion por el CLS de la columna que tiene que ser col-More

        if (LastCol.cls == "col-More" || LastCol.cls == "NoOcultar col-More") {
            colArrayNoColMore.pop();
        }

        // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

        colArrayNoColMore.forEach(function (colArray) {
            AnchoTotal += colArray.minWidth;
            if (AnchoTotal <= gridW - 75) {
                colArray.show();
            }
            else {
                colArray.hide();
            }
        });
    }


}

function winResiz(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(500);
    }

    obj.center();
}
//La columna que no quieres que se muestre debe tener el Cls debe de ser 'excluirPnInfo'

function cargarDatosPanelMoreInfo(registro, columnaID) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";
    grid = columnaID.gridRef.columnManager.getColumns();

    for (var columna of grid) {
        if (columna.cls != 'col-More' && columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                if (columna.xtype == 'datecolumn') {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + Ext.Date.format(registro.get(columna.dataIndex), 'd/m/Y') + '</span></td></tr>';
                }
                else if (registro.get(columna.dataIndex) != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
                }
            }
            else {
                if (columna.tooltip != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
                }
                else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
            }
        }
    }


    tabla.innerHTML = html;
}

function cargarDatosPanelMoreInfoGrid(registro, Grid) {
    html = '';
    let tabla = document.getElementById('bodyTablaInfoElementos');
    let grid;
    tabla.innerHTML = "";

    grid = Grid.columnManager.getColumns();

    for (var columna of grid) {
        if (columna.cls != 'col-More' && columna.cls != 'NoOcultar col-More' && columna.xtype != 'widgetcolumn' && columna.cls != "excluirPnInfo") {
            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn' || columna.xtype == 'hyperlinkcolumn' || columna.xtype == 'componentcolumn')) {
                if (columna.xtype == 'datecolumn') {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + Ext.Date.format(registro.get(columna.dataIndex), 'd/m/Y') + '</span></td></tr>';
                }
                else if (registro.get(columna.dataIndex) != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
                }
            }
            else {
                if (columna.tooltip != undefined) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
                }
                else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
                else if (columna.renderer.name.includes("bound")) {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name.split(' ')[1]](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
                else {
                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
                }
            }
        }
    }


    tabla.innerHTML = html;
}


function OcultarPanelLateral() {
    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');
    if (asideR.collapsed == false) {

        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();
    }
    GridColHandler();
    window.dispatchEvent(new Event('resizePlantilla'));
}

function hidePanelMoreInfo(panel, registro) {
    let registroSeleccionado = panel.$widgetRecord;
    let ColumnaSeleccionado = panel.$widgetColumn;
    App.btnCollapseAsRClosed.show();

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed == false) {

        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();

    }

    if (App.WrapGestionColumnas != undefined) {
        App.WrapGestionColumnas.hide();
    }

    if (App.WrapFilterControls != undefined) {
        App.WrapFilterControls.hide();
    }


    App.pnMoreInfo.show();
    btn.style.transform = 'rotate(0deg)';
    App.pnAsideR.expand();
    cargarDatosPanelMoreInfo(registroSeleccionado, ColumnaSeleccionado);
    GridColHandler();

    window.dispatchEvent(new Event('resizePlantilla'));
}

function hideAsideR(panel) {

    App.btnCollapseAsRClosed.show();

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');



    if (panel != null) {

        App.WrapFilterControls.hide();
        App.WrapGestionColumnas.hide();


        switch (panel) {
            case "panelFiltros":
                App.WrapFilterControls.show();
                App.pnNotesFull.show();


                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;

            case "panelColumnas":

                App.WrapGestionColumnas.show();

                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;

            case "panelMore":
                App.pnMoreInfo.show();

                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;


            case "panelFiltrosToggle":

                if (asideR.collapsed == false) {

                    btn.style.transform = 'rotate(-180deg)';
                    App.pnAsideR.collapse();
                    break;

                }
                else {
                    btn.style.transform = 'rotate(0deg)';
                    App.WrapFilterControls.show();

                    App.pnAsideR.expand();
                    break;


                }



                break;



        }


    }
    else {

        if (asideR.collapsed == false) {

            btn.style.transform = 'rotate(-180deg)';
            App.pnAsideR.collapse();
        }
        else {
            btn.style.transform = 'rotate(0deg)';
            App.pnAsideR.expand();

        }

    }
    GridColHandler();

    window.dispatchEvent(new Event('resizePlantilla'));

}

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


window.addEventListener('resizePlantilla', function () {


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




    //ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL
    //var pnCentral = document.getElementsByClassName("pnCentralWrap");
    var winsize = window.innerWidth;
    var asideR = Ext.getCmp('pnAsideR');


    if (winsize < 520 && asideR.collapsed == false) {
        App.CenterPanelMain.hide();
        App.pnAsideR.setWidth(winsize);
    }
    else {
        App.CenterPanelMain.show();
        App.pnAsideR.setWidth(380);

    }


});

function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}


function showwinAddTab() {
    App.winAddTabFilter.show();

}


function showwinSaveQF() {
    App.winSaveQF.show();

}
//#endregion

// #region HISTORICOS

function crearColumnaEstadoActual(oDatosJSON) {

    var sHtml = "<div class='historico_columna columna_historico_estado_actual'>";
    sHtml += "<table>";

    // Cabecera de la columna estado actual
    sHtml += "<th class='historico_columna_cabecera'>" + jsEstadoActual + "</th>";

    // INCIO CARGA ELEMENTOS LISTA
    sHtml += "<tbody class='historico_columna_cuerpo'>";
    for (var key in oDatosJSON) {

        var valor = oDatosJSON[key];
        sHtml += "<tr class='historico_columna_valor'><td>" + valor + "</td></tr>";
    }
    sHtml += "</tbody>";
    // FIN CARGA ELEMENTOS LISTA

    sHtml += "</table></div>";

    document.getElementById("historico_contenedor").innerHTML += sHtml;
}

function agregarColumnaDinamica(oDatosJSON, oEstadoActualJSON, codigo, cabecera) {

    if (document.getElementById("columna_historico_" + codigo) == null) {

        if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
            let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

            if (check.hasClass("x-mcombo-item-checked")) {
                check.removeClass("x-mcombo-item-checked");
                check.addClass("x-mcombo-item-unchecked");
            }
        } else if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
            let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

            if (check.hasClass("x-mcombo-item-checked")) {
                check.removeClass("x-mcombo-item-unchecked");
                check.addClass("x-mcombo-item-checked");
            }
        }

        var sHtml = "<div id=columna_historico_" + codigo + " class='historico_columna historico_columna_dinamico'>";
        sHtml += "<table>";
        // CABECERA
        sHtml += "<th class='historico_columna_cabecera'>" + cabecera;

        // BOTON "CLOSE" HISTORIAL
        sHtml += `<button onclick=\"cerrarHistorico('${codigo}')\" class='boton_cerrar_historico' /></th>`;

        // INCIO CARGA ELEMENTOS LISTA
        sHtml += "<tbody class='historico_columna_cuerpo'>";
        for (var key in oEstadoActualJSON) {

            var valor = oDatosJSON[key];
            var valorEstadoActual = oEstadoActualJSON[key];

            var claseCSS = "historico_columna_valor";
            if (valor != valorEstadoActual) {

                claseCSS += " historico_columna_valor_diferente";
            }

            sHtml += "<tr class='" + claseCSS + "'><td>" + ((!valor) ? "" : valor) + "</ td></tr>";
        }
        sHtml += "</tbody>";
        // FIN CARGA ELEMENTOS LISTA

        sHtml += "</table></div>";

        document.getElementById("historico_contenedor").innerHTML += sHtml;
    }
    else {
        cerrarHistorico(codigo);
    }
}

function agregarColumnaDinamicaDesdeStore(sender, registro, index, oEstadoActualJSON) {

    var elementoDatos = registro.store.config.readParameters("Datos").apply["Datos"];
    var oDatosJSON = JSON.parse(registro.data[elementoDatos]);


    var elementoCabecera = registro.store.config.readParameters("Cabecera").apply["Cabecera"];
    var cabecera = registro.data[elementoCabecera].toLocaleString();

    var lastIndex = registro.data[elementoCabecera].toLocaleString().split(" ").length - 1;

    var elementoCodigo = registro.store.config.readParameters("Codigo").apply["Codigo"];
    var codigo = registro.data[elementoCodigo];

    agregarColumnaDinamica(oDatosJSON, oEstadoActualJSON, codigo, cabecera);
}

function agregarColumnaDinamicaDesdeCalendario(data, event, oEstadoActualJSON) {

    let temp = JSON.parse(event.json);
    let oDatosJSON = JSON.parse(temp);

    let cabecera = event.name;
    let codigo = event.id;

    if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
        let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

        if (check.hasClass("x-mcombo-item-checked")) {
            check.removeClass("x-mcombo-item-checked");
            check.addClass("x-mcombo-item-unchecked");
        }
    } else if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
        let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

        if (check.hasClass("x-mcombo-item-checked")) {
            check.removeClass("x-mcombo-item-unchecked");
            check.addClass("x-mcombo-item-checked");
        }
    }

    agregarColumnaDinamica(oDatosJSON, oEstadoActualJSON, codigo, cabecera);
}

function cerrarHistorico(codigo) {

    var id = "columna_historico_" + codigo;
    var top = document.getElementById("historico_contenedor");
    var historico = document.getElementById(id);

    if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
        let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

        if (check.hasClass("x-mcombo-item-checked")) {
            check.removeClass("x-mcombo-item-checked");
            check.addClass("x-mcombo-item-unchecked");
        }
    } else if ($(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked").length > 0) {
        let check = $(`.event-container[data-event-index='${codigo}']`).find("x-mcombo-item-checked");

        if (check.hasClass("x-mcombo-item-checked")) {
            check.removeClass("x-mcombo-item-unchecked");
            check.addClass("x-mcombo-item-checked");
        }
    }

    top.removeChild(historico);
}

// #endregion

/**
 * Para hacer uso de este método debemos añadir la siguiente linea a nuestro aspx
 * <script src="/Scripts/html2canvas.js"></script>
 * /
 * @param idElement ID del elemento del dom del cual queremos obtener la imagen.
 */
function DescargarPNG(idElement, filename) {
    if (!filename) {
        filename = "myImage";
    }

    var divExport = document.getElementById(idElement);

    html2canvas(divExport).then(function (canvas) {
        var image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
        var a = document.createElement("a");
        a.setAttribute('download', filename + '.png');
        a.setAttribute('href', image);
        a.click();
    });
}

function SetMaxHeight(sender) {
    sender.minHeight = window.innerHeight;
    sender.maxHeight = window.innerHeight;
    sender.updateLayout();
}

function SetMaxHeightSuperior(sender, bool = false) {
    if (sender.getMinHeight() != sender.up().getHeight()) {
        if (!bool) {
            sender.setMinHeight(sender.up().getHeight() - 30);
            sender.setMaxHeight(sender.up().getHeight() - 30);
            sender.updateLayout();
        }
        else {
            sender.setMinHeight(sender.up().getHeight());
            sender.setMaxHeight(sender.up().getHeight());
            sender.updateLayout();
        }
    }
}

function RenderIcono(ruta) {
    return '<img src="' + ruta + '" width="20" height="20">';
}

/**
 * 
 * @param {any} data
 */
function encodeQueryData(data) {
    const ret = [];
    for (let d in data)
        ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
    return ret.join('&');
}

/**
 * Devuelve una cadena del tipo 5GB con el tamaño recibido en Bytes
 * @param {any} bytes
 * @param {any} decimals
 */
function formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

function winErrorTimeout() {

    var ventana = '<div id="winErrorTimeout" class="winErrorTimeout">' +
        '<div class="bodyErrorTimeout">' +
        '<p>' + jsErrorTimeout + '</p>' +
        '<button id="reload" class="btnReloadTimeout" onclick="reloadPage()">' + jsRecargar + '</button>' +
        '</div>' +
        '</div>';

    var body = document.getElementsByTagName('body')[0];
    body.innerHTML = body.innerHTML + ventana;

    document.getElementById('winErrorTimeout').classList.add('show');

    document.getElementById('reload').addEventListener('click', () => {
        window.location.reload();
    });
}
function reloadPage() {
    window.location.reload();
}

function renderFrecuenciaCronFormat(valor) {
    var cronstrue = window.cronstrue;
    let textoCron = "";
    try {
        if (valor != null && valor != "" && valor != "_") {
            textoCron = cronstrue.toString(valor, { locale: App.hdLocale.value });
        } else if (valor != null && valor == "_") {
            textoCron = jsSoloUnaVez;
        }
    }
    catch (ex) {
        textoCron = "";
    }

    return `<span title="${textoCron}">${textoCron}</span>`;
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + '/' + month + '/' + year;
}

//#region ICONOS BUSCADOR

function FieldSearch(sender, registro) {
    var iconClear = sender.getTrigger("_trigger2");
    iconClear.hide();
}

function FiltrarColumnas(sender, registro) {
    var iconSearch = sender.getTrigger("_trigger1");
    var iconClear = sender.getTrigger("_trigger2");
    var text = sender.getRawValue();

    if (Ext.isEmpty(text, false)) {
        iconClear.hide();
        iconSearch.show();
    }

    if (!Ext.isEmpty(text, false)) {
        iconSearch.hide();
        iconClear.show();
    }
}

//#endregion
function getFormattedTime(date) {
    let hour = date.getHours()
    let minutes = date.getMinutes();


    hour = hour.toString();
    hour = hour.length > 1 ? hour : '0' + hour;

    minutes = minutes.toString();
    minutes = minutes.length > 1 ? minutes : '0' + minutes;

    return `${hour}:${minutes}`;
}

// OCULTAR HEADER

var headState = 0;
function noHeader() {
    var hder = document.getElementById('hdDefault');
    var cont = document.getElementById('ctDefault');
    var ifrm = document.querySelector('#tabPpal iframe');
    var mod = document.getElementById('lblModNoHeader');
    var btn = document.getElementById('btnNoHeader');
    var pnl = document.getElementById('tabPpal');

    switch (headState) {
        case 0:
            hder.style.display = 'none';
            cont.style.top = '0';
            btn.style.transform = 'rotate(-180deg)';
            //mod.style.display = 'block';

            if (ifrm != null) {
                ifrm.style.height = '95vh';
            }
            if (pnl != null) {
                pnl.classList.add('tabPnl-Hide');
            }
            headState = 1;
            TopContracted = false;
            // CtSizer();
            break;

        case 1:
            hder.style.display = 'flex';
            cont.style.top = '56px';
            btn.style.transform = 'rotate(360deg)';
            //mod.style.display = 'none';

            if (ifrm != null) {
                ifrm.style.height = '90vh';
            }
            if (pnl != null) {
                pnl.classList.remove('tabPnl-Hide');
            }
            headState = 0;
            TopContracted = true;
            // CtSizer();

            break;

    }

}

//FIN OCULTAR HEADER

//CARDS WORKFLOWS

function CreateCard(oData, btns) {
    let html = '';

    html += '<div class="contWFCard contWFCard--data" onclick="" public=' + oData.Public + '>' +
        '<div class="contHeader">';
    if (oData.Active == false) {
        html += '<img class="contHeader__icon" src="/ima/ico-launcher.svg" alt="off" />';
    }
    else {
        html += '<img class="contHeader__icon" src="/ima/ico-launcher-gr.svg" alt="Launch" />';
    }
    html += '<div class="contHeader__name">' + oData.Name + '</div>' +
        '<img class="contHeader__icon" src="/ima/ico-Workflow.svg" alt="WF" />' +
        '</div>' +
        '<div class="contdata">' +
        '<div class="dataCard dataCard--code">' + oData.Code + '</div>';
    if (oData.Description != null)
        html += '<div class="dataCard dataCard--duration">' + oData.Description + '</div>';
    html += '<div>';
    if (oData.LinkedRoles) {
        for (var i = 0; i < 4 && i < oData.LinkedRoles.length; i++) {
            html += '<div class="dataCard dataCard--roles"><img class="dataCard__rol" src="/ima/ico-lectura.svg" alt="WF" />' + oData.LinkedRoles[i] + '</div>';
        }
    }
    /*html += '<div class="dataCard"><img class="dataCard__rol" src="/ima/ico-escritura.svg" alt="WF" />' + x.data.Public + '</div>' +*/
    html += '</div>' +
        '<div class="dvDescription hidden">' + oData.Description + '</div>' +
        '<div class="dvBotones">';
    if (btns != null && btns.length > 0)
        btns.forEach(x => {
            html += '<div class="btnAccion btnAccion--' + x.btn + ' hidden" onclick="' + x.func + '"></div>';
        });
    html += '</div>' +
        '</div>' +
        '</div>';

    return html;
}

//FIN CARDS WORKFLOWS