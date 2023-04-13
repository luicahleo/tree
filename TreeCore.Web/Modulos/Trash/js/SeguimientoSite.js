// #region OVERRIDE CLASE ALERTS DEBE ESTAR AL PRINCIPIO (necesario para el order de los botones)

Ext.define('Ext.overrides.MessageBox', {
    override: 'Ext.window.MessageBox',

    OK: 1,
    CANCEL: 2,
    NO: 4,
    YES: 8,

    OKCANCEL: 3,
    YESNO: 12,
    YESNOCANCEL: 14,

    buttonIds: [
        'ok', 'cancel', 'no', 'yes'
    ]
});


// #endregion




function BotonDone() {

    //Ext.Msg.alert(
    //    {
    //        title: 'Mark As Undone',
    //        msg: 'Cambiar el estado a no hecho, borrara todos los campos y dejara la tarea lista para empezar de nuevo',
    //        buttons: Ext.Msg.YESNO,
    //        fn: BotonToggleP2,
    //        icon: Ext.MessageBox.QUESTION
    //    });

    App.winUndone.show();

}

function BotonClear() {

    //Ext.Msg.alert(
    //    {
    //        title: 'Clear Fields',
    //        Cls: testalert,
    //        msg: '¿Quiere restaurar todos los valores de la tarea?',
    //        buttons: Ext.Msg.YESNO,
    //        buttonText: {
    //            yes: 'Restaurar',
    //            no: 'Cancelar'
    //        },
    //        fn: BotonToggleP2,
    //        icon: Ext.MessageBox.QUESTION
    //    });
    App.winClearFields.show();

}






function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}

function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}


function BotonToggleFiltrosMain() {

    if (App.btnFiltrosMain.text == "Filtros") {
        App.btnFiltrosMain.setText('Aplicar');

    }
    else {
        App.btnFiltrosMain.setText('Filtros');

    }

}

function BotonToggleFiltrosP2() {

    if (App.ToggleFiltrosP2.text == "Filtros") {
        App.ToggleFiltrosP2.setText('Aplicar');

    }
    else {
        App.ToggleFiltrosP2.setText('Filtros');

    }

}

function ShowMapPn() {
    if (App.ContP1.hidden == true) {
        App.ContP1.show();
        App.ContP2.show();
        App.pnMaps1.hide();
    }
    else {
        App.pnMaps1.show();
        App.ContP1.hide();
        App.ContP2.hide();


    }

}


function ShowFiltersTb() {

    BotonToggleFiltrosMain();
    if (App.toolbarfiltros.hidden == true) {
        App.toolbarfiltros.show();
    }
    else {
        App.toolbarfiltros.hide();
    }

}

function ShowFiltersP2() {

    BotonToggleFiltrosP2();
    if (App.toolbarFiltrosP2.hidden == true) {
        App.toolbarFiltrosP2.show();
    }
    else {
        App.toolbarFiltrosP2.hide();
    }

}


function BotonToggle() {
    if (App.btnDoneUndone.text == "Done") {
        App.btnDoneUndone.setText('Undone');

    }
    else {
        App.btnDoneUndone.setText('Done');

    }

}

function BotonToggleP2() {
    if (App.btnDoneUndone.text == "Done") {
        App.btnDoneUndone.setText('Undone');

    }
    else {
        App.btnDoneUndone.setText('Done');

    }

}



function ShowWorkFlow() {

    if (App.pnShowWorkflow.hidden == true) {
        App.pnShowWorkflow.show();
    }
    else {
        App.pnShowWorkflow.hide();
    }
}




var runProgressbar = function (bar) {

    var Porcentaje = 0.8;
    bar.updateProgress(Porcentaje);

    if (Porcentaje >= 1) {
        App.pnAverage.hide();
    }
};


function EsconderBarPgr(id) {
    App.pnAverage.hide();
}


function ShowHoverChanges() {

    App.WinTrackChanges.show();
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.WinTrackChanges.setY(y);
    App.WinTrackChanges.setX(x);

}

function HideHoverChanges() {
    App.WinTrackChanges.hide();
}


function ShowHoverDocum() {

    App.WinTrackDocum.show();
    var x = event.clientX, y = event.clientY,
        elementMouseIsOver = document.elementFromPoint(x, y);

    App.WinTrackDocum.setY(y);
    App.WinTrackDocum.setX(x);

}

function HideHoverDocum() {
    App.WinTrackDocum.hide();
}









function showForms(who) {
    var LNo = who.textEl;
    let Fok;
    let FobjOk;
    let FlOk;
    let FlobjOk;

    switch (who.id) {

        case 'lnkTracking':
            habilitaLnk(LNo);

            hideallUpperTabs();
            App.grdTracking.show();


            break;

        case 'lnkContract':
            habilitaLnk(LNo);

            hideallUpperTabs();

            App.DloadTabPContract.show();
            App.cntTabPanelContract.show();

            break;



        case 'lnkSite':
            habilitaLnk(LNo);

            hideallUpperTabs();

            App.DloadTabPSite.show();
            App.cntTabPanelSite.show();
            break;


       


            break;


    }
}

function hideallUpperTabs() {
    App.grdTracking.hide();
    App.cntTabPanelContract.hide();
    App.cntTabPanelSite.hide();


    App.DloadTabPContract.hide();
    App.DloadTabPSite.hide();

}


function habilitaLnk(vago) {
    let ct = document.getElementById('conNavVistas-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}












