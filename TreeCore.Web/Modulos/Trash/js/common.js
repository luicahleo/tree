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
                    msg: "Cargando..."
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
}

var ActivoRender = function (value) {

    if (value == true || value == 1) {

        return '<span class="gen_activo" >&nbsp;</span>';
    }
    else {
        return '<span class="gen_noactivo" >&nbsp;</span>';
    }
}



String.prototype.startsWith = function (s) { if (this.indexOf(s) == 0) return true; return false; }

// INICIO EXPORTACION

function ExportarDatos(pagina, grid, aux, aux2, aux3) {
    if (aux2 == undefined) {
        aux2 = "EXPORTAR";
    }
    if (aux2 == "") aux2 = "EXPORTAR";

    var orden = Orden(grid);
    var dir = Direccion(grid);
    var filtro = Filtros(grid);
    window.open(pagina + ".aspx?opcion=" + aux2 + "&orden=" + orden + "&dir=" + dir + "&filtro=" + filtro + "&aux=" + aux + "&aux3=" + aux3);
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
        return grid.store.sorters.items[0].config.direction;
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

