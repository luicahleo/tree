//INICIO SELECCION DE LUGAR

//function SeleccionarRegionPaises(sender, registro, index) {
//    this.getTrigger(0).show();

//    var Panel = this.up("container");
//    var combosFiltrados = [];
//    for (var i = 1; i < Panel.items.items.length; i++) {
//        combosFiltrados[i - 1] = Panel.items.items[i];
//    }

//    recargarCombos(combosFiltrados);
//}


function SeleccionarRegionPaises(sender, registro, index) {

    this.getTrigger(0).show();

    let idComponente = getIdComponente(sender);

    let combos = [Ext.getCmp(idComponente + '_cmbPais'),
    Ext.getCmp(idComponente + '_cmbRegion'),
    Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];


    recargarCombos(combos);



}

//function RecargarRegionPaises() {
//    //var Panel = this.up("container");
//    //var combosFiltrados = Panel.items.items;
//    //recargarCombos(combosFiltrados);
//}

function RecargarRegionPaises(sender, registro, index) {
    let idComponente = getIdComponente(sender);

    let combos = [Ext.getCmp(idComponente + '_cmbRegionPaises'),
    Ext.getCmp(idComponente + '_cmbPais'),
    Ext.getCmp(idComponente + '_cmbRegion'),
    Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];

    recargarCombos(combos);
    //}
}


function SeleccionarPais(sender, registro, index) {
    this.getTrigger(0).show();

    let idComponente = getIdComponente(sender);

    let combos = [Ext.getCmp(idComponente + '_cmbRegion'),
    Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];

    recargarCombos(combos);
}

function RecargarPais(sender, registro, index) {
    let idComponente = getIdComponente(sender);

    let combos = [Ext.getCmp(idComponente + '_cmbPais'),
    Ext.getCmp(idComponente + '_cmbRegion'),
    Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];

    recargarCombos(combos);
}

function SeleccionarRegion(sender, registro, index) {
    this.getTrigger(0).show();

    let idComponente = getIdComponente(sender);
    let combos = [Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];
    recargarCombos(combos);
}

function RecargarRegion(sender, registro, index) {
    let idComponente = getIdComponente(sender);

    let combos = [Ext.getCmp(idComponente + '_cmbRegion'),
    Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];

    recargarCombos(combos);
}

function SeleccionarProvincia(sender, registro, index) {

    this.getTrigger(0).show();
    let idComponente = getIdComponente(sender);
    let combos = [Ext.getCmp(idComponente + '_cmbMunicipio')];
    recargarCombos(combos);

}

function RecargarProvincia(sender, registro, index) {
    let idComponente = getIdComponente(sender);
    let combos = [Ext.getCmp(idComponente + '_cmbProvincia'),
    Ext.getCmp(idComponente + '_cmbMunicipio')];
    recargarCombos(combos);
}

function SeleccionarMunicipio(sender, registro, index) {
    this.getTrigger(0).show();
}

function RecargarMunicipio(sender, registro, index) {
    let idComponente = getIdComponente(sender);
    let combos = [Ext.getCmp(idComponente + '_cmbMunicipio')];
    recargarCombos(combos);
}

function DefinirDatos(dato, nombreContainer) {
    Ext.getCmp(nombreContainer + '_hdPais').setValue(dato.PaisID);
    Ext.getCmp(nombreContainer + '_hdRegion').setValue(dato.RegionID);
    Ext.getCmp(nombreContainer + '_hdProvincia').setValue(dato.ProvinciaID);
    Ext.getCmp(nombreContainer + '_hdMunicipio').setValue(dato.MunicipioID);
    try {
        Ext.getCmp(nombreContainer + '_hdRegionPaises').setValue(dato.PaisID);
    } catch (e) {
        Ext.getCmp(nombreContainer + '_hdRegionPaises').setValue(0);
    }
}

function RecargarCombosLocalizaciones(idComponente, NOlimpiarvalores, callback) {

    let combos = [
        Ext.getCmp(idComponente + '_cmbPais'),
        Ext.getCmp(idComponente + '_cmbRegion'),
        Ext.getCmp(idComponente + '_cmbProvincia'),
        Ext.getCmp(idComponente + '_cmbMunicipio')];

    //try {
    //    combos[3] = Ext.getCmp(idComponente + '_cmbRegionPaises').setValue(dato.PaisID);
    //} catch (e) {
    //    combos[3] = Ext.getCmp(idComponente + '_cmbRegionPaises').setValue(0);
    //}

    recargarCombos(combos, function Fin(fin) {
        if (fin) {

            if (NOlimpiarvalores) {
                Ext.getCmp(idComponente + '_cmbPais').setValue(Ext.getCmp(idComponente + '_hdPais').value);
                Ext.getCmp(idComponente + '_cmbRegion').setValue(Ext.getCmp(idComponente + '_hdRegion').value);
                Ext.getCmp(idComponente + '_cmbProvincia').setValue(Ext.getCmp(idComponente + '_hdProvincia').value);
                Ext.getCmp(idComponente + '_cmbMunicipio').setValue(Ext.getCmp(idComponente + '_hdMunicipio').value);
            }
            callback(true, null);
        }
    });


}


//FIN SELECCION DE LUGAR
