
//ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL

var TreepnL = document.getElementsByClassName("TreePnl");


var forcedVisor = false;


function VisorSwitch(sender) {

    ruta = getIdComponente(sender);
    var asideL = Ext.getCmp([ruta + '_' + 'TreePanelV1']);
    var btnclose = Ext.getCmp([ruta + '_' + 'btnCloseShowVisorTreeP']);




    if (asideL.hidden == true) {
        forcedVisor = false;
        btnclose.setIconCls('ico-hide-menu');
        App[ruta + '_' + 'TreePanelV1'].show();

    }
    else {
        forcedVisor = true;
        btnclose.setIconCls('ico-moverow-gr');
        App[ruta + '_' + 'TreePanelV1'].hide();




    }


}


function ControlPaneles(sender) {

    var winsize = window.innerWidth;

    var containerSize = Ext.get('CenterPanelMain').getWidth();

    ruta = getIdComponente(sender);

    if (forcedVisor != true) {


        //HIDE PANEL2
        if (containerSize < 480) {
            TreepnL[0].classList.remove("TreePL");

            App[ruta + '_' + 'TreePanelV1'].maxWidth = 9999;
            App[ruta + '_' + 'gridMain1'].hide();


        }
        else {
            TreepnL[0].classList.add("TreePL");

            App[ruta + '_' + 'TreePanelV1'].maxWidth = 300;
            App[ruta + '_' + 'gridMain1'].show();


        }

        //HIDE PANEL1
        if (containerSize < 120) {
            App[ruta + '_' + 'TreePanelV1'].hide();

        }
        else {
            App[ruta + '_' + 'TreePanelV1'].show();

        }


    }
    else {

        if (containerSize < 160) {
            App[ruta + '_' + 'gridMain1'].hide();
            TreepnL[0].classList.remove("TreePL");


        }
        else {
            App[ruta + '_' + 'gridMain1'].show();

            TreepnL[0].classList.add("TreePL");


        }
    }




}


function SelectTreepn(sender) {

    if (App[ruta + '_' + 'gridMain1'].hidden == true) {
        forcedVisor = true;

        var btnclose = Ext.getCmp([ruta + '_' + 'btnCloseShowVisorTreeP']);
        btnclose.setIconCls('ico-moverow-gr');

        App[ruta + '_' + 'TreePanelV1'].hide();

        App[ruta + '_' + 'gridMain1'].show();
    }
}

