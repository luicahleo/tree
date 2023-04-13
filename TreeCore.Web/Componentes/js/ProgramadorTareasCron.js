//function Seleccion(sender, registro, index) {
//    var datos = registro.TipoReajuste;
//    ruta = getIdComponente(sender)
//    switch (datos) {
//        case "1":

//import { data } from "jquery";


//            App[ruta + '_' + 'txtFechaInicioRevision'].hide();
//            App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;

//            App[ruta + '_' + 'txtFechaProxima'].hide();
//            App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

//            App[ruta + '_' + 'chkControlFechaFin'].hide();

//            App[ruta + '_' + 'txtFechaFinRevision'].hide();
//            App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = false;

//            App[ruta + '_' + 'cmbInflaciones'].hide();
//            App[ruta + '_' + 'cmbInflaciones'].allowBlank = false;

//            App[ruta + '_' + 'txtCadencia'].hide();
//            App[ruta + '_' + 'txtCadencia'].allowBlank = false;

//            App[ruta + '_' + 'txtPorcentaje'].hide();
//            App[ruta + '_' + 'txtPorcentaje'].allowBlank = false;

//            App[ruta + '_' + 'txtCantidad'].hide();
//            App[ruta + '_' + 'txtCantidad'].allowBlank = false;

//            break;
//        case "2":
//            App[ruta + '_' + 'txtFechaInicioRevision'].show();
//            if (App.winGestion.title.startsWith(jsEditar)) {

//                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
//                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = true;

//                App[ruta + '_' + 'txtFechaProxima'].show();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

//            }

//            if (App.winGestion.title.startsWith(jsAgregar)) {
//                App[ruta + '_' + 'txtFechaProxima'].hide();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

//                App['txtFechaInicio'].enable();
//            }

//            App[ruta + '_' + 'chkControlFechaFin'].show();

//            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
//                App[ruta + '_' + 'txtFechaFinRevision'].show();
//                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = true;
//            }


//            App[ruta + '_' + 'cmbInflaciones'].show();
//            App[ruta + '_' + 'cmbInflaciones'].allowBlank = true;

//            App[ruta + '_' + 'txtCadencia'].hide();
//            App[ruta + '_' + 'txtCadencia'].allowBlank = false;

//            App[ruta + '_' + 'txtPorcentaje'].hide();
//            App[ruta + '_' + 'txtPorcentaje'].allowBlank = false;

//            App[ruta + '_' + 'txtCantidad'].hide();
//            App[ruta + '_' + 'txtCantidad'].allowBlank = false;
//            break;
//        case "3":
//            App[ruta + '_' + 'txtFechaInicioRevision'].show();
//            if (App.winGestion.title.startsWith(jsEditar)) {

//                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
//                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = true;

//                App[ruta + '_' + 'txtFechaProxima'].show();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

//                App[ruta + '_' + 'txtFechaInicioRevision'].show();

//            }

//            if (App.winGestion.title.startsWith(jsAgregar)) {
//                App[ruta + '_' + 'txtFechaProxima'].hide();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;
//                App['txtFechaInicio'].enable();
//            }

//            App[ruta + '_' + 'chkControlFechaFin'].show();

//            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
//                App[ruta + '_' + 'txtFechaFinRevision'].show();
//                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = true;
//            }

//            App[ruta + '_' + 'cmbInflaciones'].hide();
//            App[ruta + '_' + 'cmbInflaciones'].allowBlank = false;

//            App[ruta + '_' + 'txtCadencia'].show();
//            App[ruta + '_' + 'txtCadencia'].allowBlank = true;

//            App[ruta + '_' + 'txtPorcentaje'].hide();
//            App[ruta + '_' + 'txtPorcentaje'].allowBlank = false;

//            App[ruta + '_' + 'txtCantidad'].show();
//            App[ruta + '_' + 'txtCantidad'].allowBlank = true;
//            break;
//        case "4":
//            App[ruta + '_' + 'txtFechaInicioRevision'].show();
//            if (App.winGestion.title.startsWith(jsEditar)) {

//                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
//                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = true;

//                App[ruta + '_' + 'txtFechaProxima'].show();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

//            }
//            if (App.winGestion.title.startsWith(jsAgregar)) {
//                App[ruta + '_' + 'txtFechaProxima'].hide();
//                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;
//                App['txtFechaInicio'].enable();

//            }


//            App[ruta + '_' + 'chkControlFechaFin'].show();

//            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
//                App[ruta + '_' + 'txtFechaFinRevision'].show();
//                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = true;
//            }

//            App[ruta + '_' + 'cmbInflaciones'].hide();
//            App[ruta + '_' + 'cmbInflaciones'].allowBlank = false;

//            App[ruta + '_' + 'txtCadencia'].show();
//            App[ruta + '_' + 'txtCadencia'].allowBlank = true;

//            App[ruta + '_' + 'txtPorcentaje'].show();
//            App[ruta + '_' + 'txtPorcentaje'].allowBlank = true;

//            App[ruta + '_' + 'txtCantidad'].hide();
//            App[ruta + '_' + 'txtCantidad'].allowBlank = false;
//            break;
//    }
//    App.pnFormProductCatalog.update();

//}


function SeleccionarFrecuenciaMeses(sender, registro, index) {

    App[sender.id].getTrigger(0).show();

    App[ruta + '_' + 'cmbMeses'].hide();
    App[ruta + '_' + 'cmbMeses'].allowBlank = true;


    //App[ruta + '_' + 'cmbMesInicio'].show();
    //App[ruta + '_' + 'cmbMesInicio'].allowBlank = false;

}

function SeleccionarMesInicio(sender, registro, index) {
    App[sender.id].getTrigger(0).show();
}

function SeleccionarTipo(sender, registro, index) {
    App[sender.id].getTrigger(0).show();
}

var RutaFormulario;

function SeleccionarFrecuencia(sender, registro, index) {
    App[sender.id].getTrigger(0).show();

    ruta = getIdComponente(sender)

    RutaFormulario = ruta;

    var datos = App[ruta + '_' + 'cmbFrecuencia'].value;

    if (datos == 'NoSeRepite') {

        App[ruta + '_' + 'txtCronFormat'].hide();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].hide();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        //document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].hide();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].hide();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();


        Validar(sender, registro, index);
        //MetodoBtnGuardar()

        //App.formGestion.updateLayout();

    }
    else if (datos == 'Diario') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();


        Validar(sender, registro, index);
        //MetodoBtnGuardar()

        //App.formGestion.updateLayout();

    }
    else if (datos == 'DiaLaborable') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();


        Validar(sender, registro, index);
        //MetodoBtnGuardar()

        //App.formGestion.updateLayout();

    }
    else if (datos == 'Semanal') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();


        Validar(sender, registro, index);
        //MetodoBtnGuardar()

        //App.formGestion.updateLayout();

    }
    else if (datos == 'Mensual') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();

        //App.formGestion.updateLayout();

        Validar(sender, registro, index);
        //MetodoBtnGuardar()
    }
    else if (datos == 'SemanalCustom') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 2;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].show();
        App[ruta + '_' + 'cmbDias'].allowBlank = false;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();


        Validar(sender, registro, index);
        //MetodoBtnGuardar()

        //App.formGestion.updateLayout();

    }
    else if (datos == 'MensualCustom') {

        //App[ruta + '_' + 'txtCronFormat'].show();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtCronFormat').parentNode.hidden = false;

        App[ruta + '_' + 'txtPrevisualizar'].show();
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.hidden = false;
        document.getElementById(ruta + '_' + 'txtPrevisualizar').parentNode.style = 'grid-column-end: -1; grid-row: span 3;';

        App[ruta + '_' + 'txtFechaInicio'].show();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtFechaInicio').parentNode.hidden = false;

        App[ruta + '_' + 'txtFechaFin'].show();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = true;
        document.getElementById(ruta + '_' + 'txtFechaFin').parentNode.hidden = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].show();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbTipoFrecuencia').parentNode.hidden = false;

        App[ruta + '_' + 'txtDiaCadaMes'].show();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = false;
        document.getElementById(ruta + '_' + 'txtDiaCadaMes').parentNode.hidden = false;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;
        //document.getElementById(ruta + '_' + 'cmbMesInicio').parentNode.hidden = false;

        App[ruta + '_' + 'cmbMeses'].show();
        App[ruta + '_' + 'cmbMeses'].allowBlank = false;
        document.getElementById(ruta + '_' + 'cmbMeses').parentNode.hidden = false;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;
        document.getElementById(ruta + '_' + 'cmbDias').parentNode.hidden = false;

        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.style = 'grid-column-start: 2; justify-self: end;';
        //App[ruta + '_' + 'btnGenerar'].show();
        //document.getElementById(ruta + '_' + 'btnGenerar').parentNode.hidden = false;

        App[sender.id].getTrigger(0).show();

        //App.formGestion.updateLayout();

        Validar(sender, registro, index);
        //MetodoBtnGuardar()
    }
    else {
        //App[ruta + '_' + 'txtCronFormat'].hide();
        App[ruta + '_' + 'txtCronFormat'].allowBlank = false;

        App[ruta + '_' + 'txtPrevisualizar'].hide();

        App[ruta + '_' + 'txtFechaInicio'].hide();
        App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;

        App[ruta + '_' + 'txtFechaFin'].hide();
        App[ruta + '_' + 'txtFechaFin'].allowBlank = false;

        App[ruta + '_' + 'cmbTipoFrecuencia'].hide();
        App[ruta + '_' + 'cmbTipoFrecuencia'].allowBlank = true;

        App[ruta + '_' + 'txtDiaCadaMes'].hide();
        App[ruta + '_' + 'txtDiaCadaMes'].allowBlank = true;

        //App[ruta + '_' + 'cmbMesInicio'].hide();
        //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;

        App[ruta + '_' + 'cmbMeses'].hide();
        App[ruta + '_' + 'cmbMeses'].allowBlank = true;

        App[ruta + '_' + 'cmbDias'].hide();
        App[ruta + '_' + 'cmbDias'].allowBlank = true;

        //App[ruta + '_' + 'btnGenerar'].hide();

        //App.formGestion.updateLayout();
        Validar(sender, registro, index);
        //MetodoBtnGuardar()
    }

    if (App[ruta + "_txtFechaInicio"].value == null) {
        App[ruta + "_txtFechaInicio"].setValue(new Date());
    }

}

function RecargarComboFrecuencia(sender, registro, index) {
    App[sender.id].getTrigger(0).hide();
    App[sender.id].clearValue();


    App[ruta + '_' + 'txtCronFormat'].hide();
    App[ruta + '_' + 'txtCronFormat'].allowBlank = false;

    App[ruta + '_' + 'txtPrevisualizar'].hide();

    App[ruta + '_' + 'txtFechaInicio'].hide();
    App[ruta + '_' + 'txtFechaInicio'].allowBlank = false;

    App[ruta + '_' + 'txtFechaFin'].hide();
    App[ruta + '_' + 'txtFechaFin'].allowBlank = false;

    App[RutaFormulario + '_' + 'cmbTipoFrecuencia'].hide();
    App[RutaFormulario + '_' + 'cmbTipoFrecuencia'].allowBlank = true;

    App[RutaFormulario + '_' + 'txtDiaCadaMes'].hide();
    App[RutaFormulario + '_' + 'txtDiaCadaMes'].allowBlank = true;

    App[RutaFormulario + '_' + 'cmbMeses'].hide();
    App[RutaFormulario + '_' + 'cmbMeses'].allowBlank = true;

    //App[ruta + '_' + 'cmbMesInicio'].hide();
    //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;

    App[RutaFormulario + '_' + 'cmbDias'].hide();
    App[RutaFormulario + '_' + 'cmbDias'].allowBlank = true;

    //App[ruta + '_' + 'btnGenerar'].hide();

}

function Seleccion(sender, registro, index) {
    var datos = registro.TipoReajuste;
    ruta = getIdComponente(sender)
    switch (datos) {
        case "1":

            break;
        case "2":

            break;
    }
}


function RecargarComboTipoFrecuencia(sender, registro, index) {
    App[sender.id].getTrigger(0).hide();
    App[sender.id].reset();

    App[ruta + '_' + 'cmbMeses'].show();
    App[ruta + '_' + 'cmbMeses'].reset();
    App[ruta + '_' + 'cmbMeses'].allowBlank = false;


    //App[ruta + '_' + 'cmbMesInicio'].hide();
    //App[ruta + '_' + 'cmbMesInicio'].setValue(null);
    //App[ruta + '_' + 'cmbMesInicio'].allowBlank = true;

}

function RecargarComboTipo(sender, registro, index) {

    App[sender.id].getTrigger(0).hide();
    App[sender.id].clearValue();

}

function RecargarMesInicio(sender, registro, index) {
    App[sender.id].getTrigger(0).hide();
    App[sender.id].clearValue();
    Validar(sender, registro, index);
}

function validarFechaInicioRevision(sender, registro, index) {
    ruta = getIdComponente(sender);

    App[ruta + '_' + 'txtFechaFin'].setMinValue(App[ruta + '_' + 'txtFechaInicio'].value);
    Validar(sender, registro, index);

}

function validarFechaFinRevision(sender, registro, index) {
    ruta = getIdComponente(sender);
    Validar(sender, registro, index);
    App[ruta + '_' + 'txtFechaInicio'].setMaxValue(App[ruta + '_' + 'txtFechaFin'].value);

}

function VaciarTxtCron(sender, registro, index) {
    Validar(sender, registro, index);
    ruta = getIdComponente(sender);
    App[ruta + '_' + 'txtCronFormat'].value = null;

}

function Validar(sender, registro, index) {
    ruta = getIdComponente(sender);
    combo = App[ruta + '_' + 'cmbFrecuencia'];
    if (combo.value == 'NoSeRepite') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '') {
            //App[ruta + '_' + 'btnGenerar'].enable();
            //BotonGenerar(sender, registro, index);
            App[ruta + '_' + 'txtCronFormat'].setValue('_');
            App[ruta + '_' + 'txtFechaFin'].setValue(null);

        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();
            //App[ruta + '_' + 'txtPrevisualizar'].value = "No cumple los requisitos";
        }
    }
    else if (combo.value == 'Diario') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '') {
            if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                BotonGenerar(sender, registro, index);
                
            } else {
                const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                var separadaIni = inicio.split("/", 3);
                var separadaFin = fin.split("/", 3);

                var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                if (FinFormateada >= IniFormateada) {
                    BotonGenerar(sender, registro, index);
                }
                else {
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }
        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
        }
    }
    else if (combo.value == 'DiaLaborable') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '' && App[ruta + '_' + 'txtFechaInicio'].rawValue != null) {
            //App[ruta + '_' + 'btnGenerar'].enable();
            if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                BotonGenerar(sender, registro, index);

            } else {
                const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                var separadaIni = inicio.split("/", 3);
                var separadaFin = fin.split("/", 3);

                var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                if (FinFormateada >= IniFormateada) {
                    BotonGenerar(sender, registro, index);
                }
                else {
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }
        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
        }
    }
    else if (combo.value == 'Semanal') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '' && App[ruta + '_' + 'txtFechaInicio'].rawValue != null) {
            if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                BotonGenerar(sender, registro, index);

            } else {
                const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                var separadaIni = inicio.split("/", 3);
                var separadaFin = fin.split("/", 3);

                var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                if (FinFormateada >= IniFormateada) {
                    BotonGenerar(sender, registro, index);
                }
                else {
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }

        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
        }
    }
    else if (combo.value == 'SemanalCustom') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '' && App[ruta + '_' + 'cmbDias'].value != 0) {
            if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                BotonGenerar(sender, registro, index);

            } else {
                const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                var separadaIni = inicio.split("/", 3);
                var separadaFin = fin.split("/", 3);

                var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                if (FinFormateada >= IniFormateada) {
                    BotonGenerar(sender, registro, index);
                }
                else {
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }

        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();

            App[ruta + '_' + 'txtCronFormat'].setValue(null);
            //App[ruta + '_' + 'cmbDias'].reload;
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
        }
    }
    else if (combo.value == 'Mensual') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '') {
            if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                BotonGenerar(sender, registro, index);

            } else {
                const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                var separadaIni = inicio.split("/", 3);
                var separadaFin = fin.split("/", 3);

                var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                if (FinFormateada >= IniFormateada) {
                    BotonGenerar(sender, registro, index);
                }
                else {
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }

        } else {
            //App[ruta + '_' + 'btnGenerar'].disable();
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
        }
    }
    else if (combo.value == 'MensualCustom') {
        if (App[ruta + '_' + 'txtFechaInicio'].rawValue != '' && App[ruta + '_' + 'txtDiaCadaMes'].value != 0 && App[ruta + '_' + 'txtDiaCadaMes'].value != '' && App[ruta + '_' + 'txtDiaCadaMes'].value != null && App[ruta + '_' + 'txtDiaCadaMes'].value < 31) {

            if (App[ruta + '_' + 'cmbTipoFrecuencia'].value != null) {

                if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                    BotonGenerar(sender, registro, index);

                } else {
                    const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                    const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                    var separadaIni = inicio.split("/", 3);
                    var separadaFin = fin.split("/", 3);

                    var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                    var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                    if (FinFormateada >= IniFormateada) {
                        BotonGenerar(sender, registro, index);
                    }
                    else {
                        App.btnGuardar.disable();
                        App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                    }
                }

            } else {

                if (App[ruta + '_' + 'cmbMeses'].value != 0) {
                    if (App[ruta + '_' + 'txtFechaFin'].rawValue == '') {
                        BotonGenerar(sender, registro, index);

                    } else {
                        const inicio = App[ruta + '_' + 'txtFechaInicio'].rawValue;
                        const fin = App[ruta + '_' + 'txtFechaFin'].rawValue;

                        var separadaIni = inicio.split("/", 3);
                        var separadaFin = fin.split("/", 3);

                        var IniFormateada = separadaIni[2] + '/' + separadaIni[1] + '/' + separadaIni[0];
                        var FinFormateada = separadaFin[2] + '/' + separadaFin[1] + '/' + separadaFin[0];

                        if (FinFormateada >= IniFormateada) {
                            BotonGenerar(sender, registro, index);
                        }
                        else {
                            App.btnGuardar.disable();
                            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                        }
                    }

                } else {
                    //App[ruta + '_' + 'btnGenerar'].disable();
                    App[ruta + '_' + 'txtCronFormat'].setValue(null);
                    App[ruta + '_' + 'cmbMeses'].clearValue();
                    App.btnGuardar.disable();
                    App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
                }
            }

        } else {
            App[ruta + '_' + 'txtPrevisualizar'].setValue(jsNoSeCumpleRequisitos);
            App[ruta + '_' + 'txtCronFormat'].setValue(null);
        }
    }

    //MetodoBtnGuardar()

}


function BotonGenerar(sender, registro, index) {

    ruta = getIdComponente(sender);
    TreeCore[ruta].GenerarCron(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnGuardar.enable();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

//function MetodoBtnGuardar() {
//    if (App[ruta + '_' + 'txtFechaInicio'].value != '' && App[ruta + '_' + 'txtCronFormat'].value != null && App[ruta + '_' + 'txtCronFormat'].value != '' && App[ruta + '_' + 'cmbFrecuencia'].value != '') {
//        App.btnGuardar.enable();
//    } else {
//        App.btnGuardar.disable();
//    }
//}