
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    GridResizer();
    App.pnComboGrdVisor.update();
    //App.imgPlano.update();.

    //LINEA MAKE POPUP DRAGGABLE
    dragElement(document.getElementById("dvInfoSiteMap"));


}, 100);

function GridResizer() {
    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);


    // var offsetHeight = document.getElementById('PanelMainGrid').offsetHeight;

    var calcdH = vh;

    //PANELES A CONTROLAR
    App.pnComboGrdVisor.height = vh;
    App.pnVisor.height = vh;
    App.imgPlano.height = vh;



    // USAR CUANDO EL SLIDER APARECE ARRIBA HAY QUE RECALCULAR TENIENDOLO EN CUENTA

    App.pnComboGrdVisor.height = vh - 67;
   

}

function grdInsideH() {
    let pH = App.pnComboGrdVisor.height;
    App.grdInsidePn.height = pH;
}

var stBtn = 0;
function displayGrd() {
    if (stBtn == 0) {
        App.TreePanel2.show();
        App.btnSbCategory.setIconCls("ico-hide-menu");
        stBtn = 1;
    }

    else if (stBtn == 1) {
        App.TreePanel2.hide();
        App.btnSbCategory.setIconCls("ico-menu-gr");
        stBtn = 0;
	}
}

function visorResizer() {

    setTimeout(function () {
        let pH = App.pnComboGrdVisor.height - 46;
        App.pnVisor.setHeight(pH);
        
    }, 100);
   
    
}



function displayMenu(btn) {


    //ocultar todos los paneles

    App.pnGridsAsideR.hide();
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.getCmp(btn);
    PanelAMostrar.show();



}


function displayMenu(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridsAsideR.hide();
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide(); 
    App.pnMapFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}



function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}
function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}






// #region Codigo para hacer div DRAGGABLE


function dragElement(elmnt) {
    var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
    if (document.getElementById(elmnt.id + "header")) {
        // if present, the header is where you move the DIV from:
        document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
    } else {
        // otherwise, move the DIV from anywhere inside the DIV:
        elmnt.onmousedown = dragMouseDown;
    }

    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
    }

    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
        elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
    }

    function closeDragElement() {
        // stop moving when mouse button is released:
        document.onmouseup = null;
        document.onmousemove = null;
    }
}


// #endregion


function ShowPopup() {

    var v = document.getElementById("dvInfoSiteMap");
    v.style.display = 'block';

}