var seleccionado;
var Agregar = false;

// PLEGAR PANEL LATERAL
var stPn = 0;
function hidePn() {
    let pn = document.getElementById('pnAsideR');
    let btn = document.getElementById('btnCollapseAsR');

    if (stPn == 0) {
        pn.style.marginRight = '-360px';
        btn.style.transform = 'rotate(-180deg)';
        stPn = 1;
        App.pnAsideR.show();
    }
    else {
        pn.style.marginRight = '0';
        btn.style.transform = 'rotate(0deg)';
        stPn = 0;
    }

}

// FUNCIONES GESTION PAGINA

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
    }
}

function winFormCenterSimple(obj) {
    obj.center();
    obj.update();
}

function DeseleccionarGrilla() {
    if (App.btnFltDspg.pressed) {
        App.btnFltEnergy.disable();
        App.btnFltLegal.disable();
        App.btnFltIndoor.disable();
        App.btnFltSaving.disable();
        App.btnFltSharing.disable();
        ModulosSeleccionar("DESPLIEGUE");
        App.cmbModChart.setValue("DESPLIEGUE");
    }
    else if (App.btnFltEnergy.pressed) {
        App.btnFltDspg.disable();
        App.btnFltIndoor.disable();
        App.btnFltLegal.disable();
        App.btnFltSaving.disable();
        App.btnFltSharing.disable();
        ModulosSeleccionar("ENERGY");
        App.cmbModChart.setValue("ENERGY");
    }
    else if (App.btnFltIndoor.pressed) {
        App.btnFltDspg.disable();
        App.btnFltEnergy.disable();
        App.btnFltLegal.disable();
        App.btnFltSaving.disable();
        App.btnFltSharing.disable();
        ModulosSeleccionar("INDOOR");
        App.cmbModChart.setValue("INDOOR");
    }
    else if (App.btnFltSaving.pressed) {
        App.btnFltDspg.disable();
        App.btnFltEnergy.disable();
        App.btnFltIndoor.disable();
        App.btnFltLegal.disable();
        App.btnFltSharing.disable();
        ModulosSeleccionar("SAVING");
        App.cmbModChart.setValue("SAVING");
    }
    else if (App.btnFltSharing.pressed) {
        App.btnFltDspg.disable();
        App.btnFltEnergy.disable();
        App.btnFltLegal.disable();
        App.btnFltIndoor.disable();
        App.btnFltSaving.disable();
        ModulosSeleccionar("SHARING");
        App.cmbModChart.setValue("SHARING");
    }
    else if (App.btnFltLegal.pressed) {
        App.btnFltDspg.enable();
        App.btnFltEnergy.enable();
        App.btnFltIndoor.enable();
        App.btnFltSaving.enable();
        App.btnFltSharing.enable();
        ModulosSeleccionar("LEGAL");
        App.cmbModChart.setValue("LEGAL");
    }
    else {
        App.btnFltDspg.enable();
        App.btnFltLegal.enable();
        App.btnFltEnergy.enable();
        App.btnFltIndoor.enable();
        App.btnFltSaving.enable();
        App.btnFltSharing.enable();
        App.hdProyectoID.setValue("");
        App.cmbModChart.clearValue();
        App.hdMediaPorcentaje.setValue("");
        calculoMedia();
    }
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeTareas.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeTareas.pageSize = wantedPageSize;
        App.storeTareas.load();
    }
}


//RESIZER JS PARA PANELES Y GRID
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    CtSizer();
    if (App.grdTask != undefined) {
        App.grdTask.update();
        App.pnGridsAsideR.update();
    }


}, 100);


function CtSizer() {


    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

    //const offsetHeight = document.getElementById('vwResp').offsetHeight;
    var ifrm = document.querySelector('#tabPpal iframe');



    if (App.grdTask != undefined) {
        //PANELES A CONTROLAR
        App.grdTask.height = vh;

        // RECALC POR LAS TOOLBARS
        App.grdTask.height = vh - 300;
        App.pnGridsAsideR.height = vh - 190;
    }


}



// FIN FUNCIONES GESTION PAGINA

// FUNCIONES METHOD DIRECT

function Refrescar() {
    App.storeTareas.reload();
    App.GridRowSelect.clearSelections();
}

function Tareas() {
    App.storeTareas.reload();
}

// FIN FUNCIONES METHOD DIRECT

// TRIGGERS COMBOS

function DepartamentosSeleccionar() {
    if (App.cmbDepartamentos.getValue() != '') {
        App.hdDepartamentoID.setValue(App.cmbDepartamentos.getValue());
        App.storeTareas.reload();
        App.cmbDepartamentos.getTrigger(0).show();
    }
    else {
        App.hdDepartamentoID.setValue("");
    }
}

function RecargarDepartamentos() {
    App.cmbDepartamentos.clearValue();
    recargarCombos([App.cmbDepartamentos]);
    App.storeTareas.reload();
    App.hdProyectoID.setValue("");
}

function ProyectosSeleccionar() {
    if (App.cmbProyectos.getValue() != '') {
        App.hdProyectoID.setValue(App.cmbProyectos.getValue());
        App.storeTareas.reload();
        App.cmbProyectos.getTrigger(0).show();
    }
    else {
        App.hdProyectoID.setValue("");
    }
}

function RecargarProyectos() {
    App.cmbProyectos.clearValue();
    recargarCombos([App.cmbProyectos]);
    App.storeTareas.reload();
    App.hdProyectoID.setValue("");
}

function ModulosSeleccionar(modulo) {
    if (modulo == "DESPLIEGUE") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else if (modulo == "ENERGY") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else if (modulo == "INDOOR") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else if (modulo == "LEGAL") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else if (modulo == "SAVING") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else if (modulo == "SHARING") {
        TreeCore.CalcularMedia(modulo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    calculoMedia();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsProcesando
                }

            });
    }
    else {
        App.hdMediaPorcentaje.setValue("");
    }
}


// FIN TRIGGERS COMBOS

function calculoMedia() {
    var can = document.getElementById('canvasChart'),
        spanPercent = document.getElementById('percent'),
        c = can.getContext('2d'),
        media = App.hdMediaPorcentaje.value; //porcentaje de tareas completadas

    var posX = can.width / 2,
        posY = can.height / 2,
        fps = 1000 / 200,
        percent = 0,
        onePercent = 360 / 100,
        result = onePercent * media;

    c.lineCap = 'round';
    arcMove();

    function arcMove() {
        var deegres = 0;
        var acrInterval = setInterval(function () {
            deegres += 1;
            c.clearRect(0, 0, can.width, can.height);
            percent = deegres / onePercent;

            spanPercent.innerHTML = percent.toFixed();

            c.beginPath();
            c.arc(posX, posY, 40, (Math.PI / 180) * 270, (Math.PI / 180) * (270 + 360));
            c.strokeStyle = '#cccccc';
            c.lineWidth = '8';
            c.stroke();

            c.beginPath();
            c.strokeStyle = '#3B89DF';
            c.lineWidth = '8';
            c.arc(posX, posY, 40, (Math.PI / 180) * 270, (Math.PI / 180) * (270 + deegres));
            c.stroke();
            if (deegres >= result) clearInterval(acrInterval);
        }, fps);

    }

}

// END CHART PERCENT

//FAVORITES

function deleteFav(star) {
    let starBtn = star.parentElement;
    let el = starBtn.parentElement.parentElement; //liFav
    let ul = el.parentElement;//ulSlide
    let liSlide = ul.parentElement; // li con clase liCtSlide
    starBtn.remove(); //borramos liFav

    let allli = el.querySelectorAll('li');
    if (allli.length == 0) {
        liSlide.remove();
    }//si no quedan li borro el ul

    //let liSlide = ul.parentElement;
    let blk = liSlide.parentElement.parentElement;
    let allUl = blk.querySelectorAll('ul .ulSlide'); //miro si quedan ul dentro de los liSlide
    if (allUl.length == 0) {
        blk.remove();
    }//si no hay ul borro el bloque

    let ctUl = document.getElementById('ulFav');
    let allBlk = document.querySelectorAll('.blkFavorite');
    if (allBlk.length == 0) {
        ctUl.style.border = '1px dashed #ccc';
    }

}

function moveSlider(clicked) {
    let btnClkd = clicked.id;

    switch (btnClkd) {
        case 'btnNext':
            ulFav.style.marginLeft = '-235px';
            break;

        case 'btnPrev':
            ulFav.style.marginLeft = '0px';
            break;
    }
}

function slider(oBtn) {
    let btnName = oBtn.id;
    let sldr = oBtn.parentNode.parentNode;
    let slide = sldr.querySelector('li .liCtSlide');
    let ulSlide = slide.querySelector('ul');
    let lnks = ulSlide.children.length;
    let num = btnName.charAt(btnName.length - 1);
    let allBtn = sldr.querySelectorAll('a');

    allBtn.forEach(function (el) {
        el.classList.remove("btnDot-activo");
    });

    if (lnks >= 3) {
        switch (num) {

            case '1':
                slide.style.marginLeft = '0px';
                allBtn[0].classList.add("btnDot-activo");

                break;

            case '2':
                slide.style.marginLeft = '-235px';
                allBtn[1].classList.add("btnDot-activo");
                break;

            default:
                slide.style.marginLeft = '0px';
        }
    }


}

function cargarFavoritos() {
    TreeCore.cargarFavoritos({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                document.getElementById('pnFav').hide();
            }
            else {
                calculoMedia();
                var fav = eval(result);
                crearArbol(fav);
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

var modulosTotales;
var listModulosTotales = [];

function crearArbol(sLista) {
    let tab = document.getElementById('ulFav');
    let countByModule = 1;
    let contModulos = 0;

    for (var i = 0; i < sLista.length; i++) {
        if (i == 0) {
            contModulos++;
            listModulosTotales.push(sLista[i].MenuModulo);
        }
        if (i > 0 && sLista[i].MenuModulo != sLista[i - 1].MenuModulo) {
            contModulos++;
            listModulosTotales.push(sLista[i].MenuModulo);

        }
        let myLiModulo = document.createElement('li');
        let myLiMod = document.createElement('li');
        let myLiPagina = document.createElement('li');
        let myUlModulo = document.createElement('ul');
        let myUlSlide = document.createElement('ul');
        let myNav = document.createElement('nav');
        let myLabel = document.createElement('Label');
        let myHyper = document.createElement('HyperLinkButton');
        let myButtonDocModulo = document.createElement('a');
        let myButtonDocMod = document.createElement('a');
        let myImage = document.createElement('img');


        myLiModulo.className = "blkFavorite";
        myLiModulo.id = "li" + sLista[i].MenuModulo;

        myLabel.id = "lbl" + sLista[i].MenuModulo;
        myLabel.className = "labelBlkFav";
        myLabel.textContent = sLista[i].MenuModulo;

        myUlModulo.id = "ul" + sLista[i].MenuModulo;
        myUlModulo.className = "favSlider";

        myLiMod.id = "li" + sLista[i].MenuModulo;
        myLiMod.className = "liCtSlide";

        myUlSlide.className = "ulSlide";
        myUlSlide.id = "ulSlide" + sLista[i].MenuModulo;

        myLiPagina.id = "li" + sLista[i].Nombre;
        myLiPagina.className = "liFav";

        myHyper.id = "lnk" + sLista[i].Nombre;
        myHyper.className = "lnkFav";
        myHyper.textContent = sLista[i].Nombre;
        myHyper.setAttribute("nombre", sLista[i].Nombre);
        myHyper.setAttribute("rutaPagina", sLista[i].RutaPagina);

        let mod = sLista[i].RutaPagina.split('/');
        let nomMod = sLista[i].MenuModulo;
        let MenuID = sLista[i].MenuID;
        let slugModulo = `${sLista[i].MenuModulo.replaceAll(" ", "").replaceAll("/", "").toLowerCase()}`;


        myHyper.addEventListener("click", function (a) {
            parent.window.location.replace(`/${slugModulo}?openFavTab=${MenuID}`);
        });


        myImage.src = sLista[i].Icono;

        if (i != 0 && sLista[i - 1].MenuModulo == sLista[i].MenuModulo) {
            const id = "ulSlide" + sLista[i].MenuModulo;
            const modulo = document.getElementById(id);

            modulo.appendChild(myLiPagina);
            myLiPagina.appendChild(myImage);
            myLiPagina.appendChild(myHyper);

            tab.appendChild(myLiMod);

            ++countByModule;

            if (countByModule > 2) {
                var navigation = document.getElementById("nav" + sLista[i].MenuModulo);
                navigation.className = "navDot";
            }


        }
        else {

            myNav.id = "nav" + sLista[i].MenuModulo;
            myNav.className = "d-none";

            myButtonDocModulo.id = "btnDot" + sLista[i].MenuModulo + "1";
            myButtonDocModulo.className = "btnDot btnDot-activo";
            myButtonDocModulo.addEventListener("click", function () {
                slider(this);
            });

            myButtonDocMod.id = "btnDot" + sLista[i].MenuModulo + "2";
            myButtonDocMod.className = "btnDot";
            myButtonDocMod.addEventListener("click", function () {
                slider(this);
            });

            countByModule = 1;
            myLiModulo.appendChild(myLabel);
            myLiModulo.appendChild(myUlModulo);
            myUlModulo.appendChild(myLiMod);

            myLiMod.appendChild(myUlSlide);
            myUlSlide.appendChild(myLiPagina);

            myLiPagina.appendChild(myImage);
            myLiPagina.appendChild(myHyper);
            myLiModulo.appendChild(myNav);
            myNav.appendChild(myButtonDocModulo);
            myNav.appendChild(myButtonDocMod);

            tab.appendChild(myLiModulo);
            modulosTotales = contModulos;
        }
    }
}

//END FAVORITES


//ASIDE RIGHT NOTIFICATIONS

function showNotes(btn) {

    if (btn == 'note') {

        App.lblHeadNotif.hide();
        App.lblHeadNotes.show();

        App.btnNotas.hide();
        App.btnNotif.show();

        App.pnNotifications.hide();
        App.pnNotes.show();
    }

    else if (btn == 'notif') {
        App.lblHeadNotif.show();
        App.lblHeadNotes.hide();

        App.btnNotas.show();
        App.btnNotif.hide();

        App.pnNotifications.show();
        App.pnNotes.hide();
    }
}

function gestNotif() {
    App.frmEditNot.show();
    App.pnNotifications.disable();
    App.btnNotas.disable();
    App.btnNotif.disable();
}

function closeF() {
    App.frmEditNot.hide();
    App.pnNotifications.setDisabled(false);
    App.btnNotas.setDisabled(false);
    App.btnNotif.setDisabled(false);
}

//END ASIDE RIGHT NOTIFICATIONS

//RIGHT PANEL TAB BUTTONS

function RpanelBTNAlert() {

    App.pnNotificationsFull.show();
    App.pnNotesFull.hide();

}

function RpanelBTNNotes() {

    App.pnNotesFull.show();
    App.pnNotificationsFull.hide();

}

//END RIGHT PANEL TAB BUTTONS

//RIGHT PANEL TAB FORM

function VaciarFormularioNotificaciones() {
    App.frmEditNot.getForm().reset();
}

function RpanelBTNForm() {

    App.frmEditNot.hide();
    App.tlbNotifications.show();

}

function RpanelBTNAdd() {

    App.frmEditNot.show();
    App.tlbNotifications.hide();

    Agregar = true;
    ajaxAgregarEditarNotificaciones();
}

function ajaxAgregarEditarNotificaciones() {
    TreeCore.AgregarEditarNotif(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeNotificaciones.reload();
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
    if (registroSeleccionado(App.gridsAsideR) && seleccionado != null) {
        ajaxEditarNotificacion();
    }
}

function ajaxEditar() {
    VaciarFormularioNotificaciones();
    Agregar = false;

    TreeCore.MostrarEditarNotif(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storeNotificaciones.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function RpanelBTNFormAlert() {

    App.frmEditAlert.hide();
    App.tlbNotes.show();

}

function RpanelBTNAddAlert() {

    App.frmEditAlert.show();
    App.tlbNotes.hide();

}

//END RIGHT PANEL TAB FORM

var linkRendererMaps = function (value, metadata, record) {
    return Ext.String.format("<a class='testclass' href=\"javascript:addUserTabMaps('{0}','{1}');\">{1} <a/>", record.data.id, record.data.name);
}

function testmenu() {
    var rectest = grdTask.getSelectionModel().getSelection()[0];

}

function addUserTabMaps(id, title) {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_' + id);
    var url1 = '/PaginasComunes/Mapas.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: title,
            iconCls: '#TextSignature',
            closable: true,
            Cls: '#Testclass',
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Mapa..  "
                    //msg: jsCargandoMapa
                }
            }
        });

    }

    tabPanel.setActiveTab(tab);
}

var linkRendererSites = function (value, meta, record, index) {
    return Ext.String.format("<a href='#' onclick='addUserTabSites({1});'>{0}</a>", value, index);
}

var addUserTabSites = function (id, title) {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_' + id);
    var url1 = '/ModGlobal/pages/Emplazamientos.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: title,
            iconCls: '#TextSignature',
            closable: true,
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Emplazamientos..  "
                    //msg: jsCargandoMapa
                }
            }
        });
    }

    tabPanel.setActiveTab(tab);
}

function addUserTabSitesContext() {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_');
    var url1 = '/ModGlobal/pages/Emplazamientos.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: "Emplazamientos",
            iconCls: 'ico-sites',
            closable: true,
            Cls: '#Testclass',
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Sites..  "
                    //msg: jsCargandoMapa
                }
            }
        });

    }

    tabPanel.setActiveTab(tab);
}

function addUserTabMapsContext() {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_');
    var url1 = '/PaginasComunes/Mapas.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: "Mapa Emplazamiento",
            iconCls: 'ico-map-sites',
            closable: true,
            Cls: '#Testclass',
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Mapa..  "
                    //msg: jsCargandoMapa
                }
            }
        });

    }

    tabPanel.setActiveTab(tab);
}

function addUserTabTrackingContext() {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_');
    var url1 = '/PaginasComunes/Seguimiento.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: "Seguimiento",
            iconCls: 'ico-seguimiento',
            closable: true,
            Cls: '#Testclass',
            height: '100%',
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Seguimiento..  "
                    //msg: jsCargandoMapa
                }
            }
        });

    }

    tabPanel.setActiveTab(tab);
}


//RENDERS GRID PRUEBA

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

//FIN RENDERS GRID PRUEBA

// #region VENTANA PÁGINAS FAVORITAS

function cargarPaginasFavoritas() {
    showLoadMask(App.winGestFav, function (load) {
        CargarStoresSerie([App.storeGestFav], function Fin(fin) {
            if (fin) {
                TreeCore.CargarPaginasFavoritas({
                    success: function () {
                        load.hide();
                    }
                });
            }
        });
    });
}

function añadirPaginasFavoritas() {
    var elementosSeleccionados = [];
    for (var i = 0; i < App.gridGestFav.getSelectionModel().selected.items.length; i++) {
        elementosSeleccionados.push(App.gridGestFav.getSelectionModel().selected.items[i].data.Pagina);
    }
    if (elementosSeleccionados.length < 5 && (modulosTotales < 3 || modulosTotales == undefined || listModulosTotales.includes(App.cbGestFav.value))) {
        showLoadMask(App.winGestFav, function (load) {
            TreeCore.AñadirPaginasFavoritas(elementosSeleccionados, {
                success: function () {
                    load.hide();
                    App.winGestFav.hide();
                    location.reload();
                }
            });
        });
    }
    else if (modulosTotales == 3 && !listModulosTotales.includes(App.cbGestFav.value)) {
        Ext.Msg.show({
            title: jsAtencion,
            msg: jsMaxCargaModulos,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
    }
    else {
        Ext.Msg.show({
            title: jsAtencion,
            msg: jsMaxCargaFavoritos,
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO
        });
    }
}

function showWinGestFav() {
    App.cbGestFav.clearValue();
    App.storeModulos.reload();
    App.storeGestFav.reload();
    App.winGestFav.show();
}

// #endregion