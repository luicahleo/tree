var Agregar = false;
var seleccionado;

function ShowRightClickMenu(item, record, node, index, e) {
    e.preventDefault();
    var menu = App.mnOpciones;
    showMenu(record.data, getActualXY(menu, e));
}

function showMenu(data, position) {
    var menu = App.mnOpciones;
    menu.dataRecord = data;
    menu.showAt(position);
}

//#region GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnAnadir.setTooltip(jsAgregar);

        if (seleccionado.Active) {
            App.mnoActivar.setText(jsDesactivar);
        }
        else {
            App.mnoActivar.setText(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.GridRowSelect.clearSelections();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeCompany.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeCompany.pageSize = wantedPageSize;
        App.storeCompany.load();
    }
}

//#endregion 

//#region GESTION

function DetalleCompany(sender) {
    MostrarDetalleCompany(sender.getWidgetRecord().data);
}

function MostrarDetalleCompany(data) {
    App.lbPnDetail.setText(jsModeloDatos);
    App.lbPnDetail.setIconCls('btnDatamodelgreen');
    try {
        $("#vsDtCompany").accordion("destroy");    // Removes the accordion bits
        $("#vsDtCompany").empty();                // Clears the contents
    } catch (e) {

    }
    App.pnDetails.selectItem = data.Code;
    App.lblTitleData.setText(data.Name.toString());
    let tablaBody = document.getElementById('vsDtCompany');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    let rols = '';
    if (data.Supplier)
        rols += '<div class="iconTag iconRed"><span>' + jsSupplier.substring(0, 1) + '</span></div>';
    if (data.Owner)
        rols += '<div class="iconTag iconBlue"><span>' + jsOwner.substring(0, 1) + '</span></div>';
    if (data.Customer)
        rols += '<div class="iconTag iconGreen"><span>' + jsCustomer.substring(0, 1) + '</span></div>';
    if (data.Payee)
        rols += '<div class="iconTag iconOrange"><span>' + jsBeneficiario.substring(0, 1) + '</span></div>';
    html += '<div class="tmpCol-td">' +
        '<span class="lblGrd">' + jsOperationalRole + '</span>' +
        '<span class="dataGrd">' + rols + '</span>' +
        '</div >';
    for (let val in data) {
        if (val == 'Supplier' || val == 'Customer' || val == 'Owner' || val == 'Payee')
            continue;
        if (trad[val] != undefined) {
            let valor = data[val];
            if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                valor = formatDate(Date.parse(valor));
            } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                valor = RenderColBool(valor);
            }
            html += '<div class="tmpCol-td">' +
                '<span class="lblGrd">' + trad[val] + '</span>' +
                '<span class="dataGrd">' + valor + '</span>' +
                '</div >';
        }
    }
    tablaBody.innerHTML = html;
    App.pnDetails.expand();
}

function MostrarDetalleCompanyMetodosPagos(pagos) {
    App.lbPnDetail.setText(jsMetodosPago);
    App.lbPnDetail.setIconCls('btnPagos-asR');
    try {
        $("#vsDtCompany").accordion("destroy");    // Removes the accordion bits
        $("#vsDtCompany").empty();                // Clears the contents
    } catch (e) {

    }
    let tablaBody = document.getElementById('vsDtCompany');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    for (let pID in pagos) {
        let pago = pagos[pID]
        html += '<h3 class="titleAcor">' + pago.PaymentMethodCode + '</h3><div>';
        for (let val in pago) {
            let valor = pago[val];
            if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                valor = formatDate(Date.parse(valor));
            } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                valor = RenderColBool(valor);
            }
            html += '<div class="tmpCol-td">' +
                '<span class="lblGrd">' + trad[val] + '</span>' +
                '<span class="dataGrd">' + valor + '</span>' +
                '</div>';
        }
        html += '</div>';
    }
    tablaBody.innerHTML = html;
    $("#vsDtCompany").accordion();
}

function MostrarDetalleCompanyCuentasBancarias(banks) {
    App.lbPnDetail.setText(jsCuentasBancarias);
    App.lbPnDetail.setIconCls('btnBancos-asR');
    try {
        $("#vsDtCompany").accordion("destroy");    // Removes the accordion bits
        $("#vsDtCompany").empty();                // Clears the contents
    } catch (e) {

    }
    let tablaBody = document.getElementById('vsDtCompany');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    for (let bID in banks) {
        let bank = banks[bID]
        html += '<h3 class="titleAcor">' + bank.Code + '</h3><div>';
        for (let val in bank) {
            let valor = bank[val];
            if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                valor = formatDate(Date.parse(valor));
            } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                valor = RenderColBool(valor);
            }
            html += '<div class="tmpCol-td">' +
                '<span class="lblGrd">' + trad[val] + '</span>' +
                '<span class="dataGrd">' + valor + '</span>' +
                '</div>';
        }
        html += '</div>';
    }
    tablaBody.innerHTML = html;
    $("#vsDtCompany").accordion();
}

function MostrarDetalleCompanyDirecciones(direcciones) {
    App.lbPnDetail.setText(jsDirecciones);
    App.lbPnDetail.setIconCls('btnDirecciones-asR');
    try {
        $("#vsDtCompany").accordion("destroy");    // Removes the accordion bits
        $("#vsDtCompany").empty();                // Clears the contents
    } catch (e) {

    }
    let tablaBody = document.getElementById('vsDtCompany');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    for (let dID in direcciones) {
        let direccion = direcciones[dID]
        html += '<h3 class="titleAcor">' + direccion.Code + '</h3><div>';
        for (let val in direccion) {
            if (val == 'Address') {
                for (let valID in direccion[val]) {
                    let valor = direccion[val][valID];
                    if (valor == undefined || valor == null || valor == null) {
                        continue;
                    }
                    if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                        valor = formatDate(Date.parse(valor));
                    } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                        valor = RenderColBool(valor);
                    }
                    html += '<div class="tmpCol-td">' +
                        '<span class="lblGrd">' + trad[valID] + '</span>' +
                        '<span class="dataGrd">' + valor + '</span>' +
                        '</div>';
                }
            }
            else if (trad[val] != undefined) {
                let valor = direccion[val];
                if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                    valor = formatDate(Date.parse(valor));
                }
                html += '<div class="tmpCol-td">' +
                    '<span class="lblGrd">' + trad[val] + '</span>' +
                    '<span class="dataGrd">' + valor + '</span>' +
                    '</div>';
            }
        }
        html += '</div>';
    }
    tablaBody.innerHTML = html;
    $("#vsDtCompany").accordion();
}

function AccionesCompany(sender) {
    showMenu(sender.getWidgetRecord().data, [sender.getX() + 20, sender.getY() + 20]);
}

function AddCompanies() {
    addTab(parent.App.tabPpal, jsAgregarEntidades, jsAgregarEntidades, "/Modulos/ThirdParty/EntidadesV2Form.aspx");
}

function EditarCompany(sender, registro, aux) {
    var code = sender.$widgetRecord.data.Code;
    var pagina = "/Modulos/ThirdParty/EntidadesV2Form.aspx?CompanyCode=" + code;
    parent.addTab(parent.parent.App.tabPpal, jsEditar + code, jsEditar + " " + code, pagina);
}

var codeCompany;

function DeleteCompany(code) {
    codeCompany = code;
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeleteCompany,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxDeleteCompany(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.DeleteCompany(codeCompany,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.storeCompany.reload();
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

function ActivarCompany(code) {
    TreeCore.ActivarCompany(code,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    App.storeCompany.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function DescargarCompany() {
}

function Refrescar() {
    App.storeCompany.reload();
    App.GridRowSelect.clearSelections();
}

function OpenViewsList(sender) {
    App.pnViews.hide();
    App.gridCompany.show();
    App.btnViewList.setPressed(true);
    App.btnViewGrid.setPressed(false);
}

function OpenViewsCard(sender) {
    App.gridCompany.hide();
    App.pnViews.show();
    App.btnViewGrid.setPressed(true);
    App.btnViewList.setPressed(false);
}

//#endregion 

//#region GRID

function SetTextRolFilter(sender) {
    switch (sender.id) {
        case 'btnSupplier':
            sender.setText(jsSupplier.substring(0, 1));
            break;
        case 'btnOwner':
            sender.setText(jsOwner.substring(0, 1));
            break;
        case 'btnCustomer':
            sender.setText(jsCustomer.substring(0, 1));
            break;
        case 'btnPayee':
            sender.setText(jsBeneficiario.substring(0, 1));
            break;
    };
}

function RendersRole(valor, sender, data) {
    let final = '';

    if (data.data.Supplier) {
        final += '<div class="iconTag iconRed"><p>' + jsSupplier.substring(0, 1) + '</p></div>';
    }
    if (data.data.Owner) {
        final += '<div class="iconTag iconBlue"><p>' + jsOwner.substring(0, 1) + '</p></div>';
    }
    if (data.data.Customer) {
        final += '<div class="iconTag iconGreen"><p>' + jsCustomer.substring(0, 1) + '</p></div>';
    }
    if (data.data.Payee) {
        final += '<div class="iconTag iconOrange"><p>' + jsBeneficiario.substring(0, 1) + '</p></div>';
    }
    return final;
}

// #region BUSCADOR

function filtrarCompanyPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeCompany.reload();
    }
}

function LimpiarFiltroBusqueda(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDCompanyBuscador.setValue("");
    App.txtFiltroCompany.setValue("");

    App.storeCompany.clearFilter();
    App.storeCompany.reload();
}
var dataGridCompany = [];

function ajaxGetDatosBuscadorCompany() {

    updatePaginCompany()

    if (dataGridCompany.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridCompany = [];

        TreeCore.GetDatosBuscador({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        dataGridCompany.push({
                            key: serv.Name.toLowerCase(),
                            key2: serv.Code.toLowerCase(),
                            nombre: serv.Name,
                            codigo: serv.Code,
                            id: serv.EntidadID
                        });
                    });

                    dataGridCompany = dataGridCompany.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var nameSearchBox = "txtFiltroCompany";
                    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

                    $(function () {
                        let textBuscado = "";
                        $(selectorSearchBox).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(dataGridCompany, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIDCompanyBuscador.setValue(idServicioBuscador);
                                    App.storeCompany.reload();

                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
                            return $("<li>")
                                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                                    `<div class="item-Buscador">` +
                                    `<div class="title">${title}</div>` +
                                    `</div>` +
                                    `<div class="description"></div>` +
                                    "</div>")
                                .appendTo(ul);
                        };
                    });
                }
            }
        });
    }
}

// #endregion

//#endregion

//#region CARDS

function ShowCardsViews() {
    let divContainer = '';

    App.storeCompany.data.items.forEach(x => {
        let subName = x.data.Name.split(" ");
        let txtLogo = subName[0].substring(0, 1).toString();
        if (subName.length > 1) {
            txtLogo += subName[1].substring(0, 1).toString();
        }
        divContainer += '<div class="contCard CardsViewsCompany" code="' + x.data.Code + '" onclick="ShowCardDetail(this, event)">' +
            '<div class="contHeader">' +
            '<div class="elemHeader">';
        //if (imagen) {
        //    divContainer += '<img class="compannyImage" src="" alt="' + subName[0].substring(0, 1) + '" />';
        //}
        //else {
        divContainer += '<div class="dvCompanyLogo"><h4 class="textCompanyLogo">' + txtLogo + '</h4></div>';
        //}
        divContainer += '<div class="headerSecond">' +
            '<div style="max-width: 200px">' +
            '<span class="itemCardTitle">' + x.data.Name + '</span>' +
            '<span class="itemCardSubTitle SL">S.L.</span></div>' +
            '<span class="itemCardSubTitle d-blk">' + x.data.Code + '</span>' +
            '</div></div>' +
            '<div class="smallIconsHeader">' +
            '<div class="iconTag iconRed" activo="' + x.data.Supplier + '"><span>' + jsSupplier.substring(0, 1) + '</span></div>' +
            '<div class="iconTag iconBlue" activo="' + x.data.Owner + '"><span>' + jsOwner.substring(0, 1) + '</span></div>' +
            '<div class="iconTag iconGreen" activo="' + x.data.Customer + '"><span>' + jsCustomer.substring(0, 1) + '</span></div>' +
            '<div class="iconTag iconOrange" activo="' + x.data.Payee + '"><span>' + jsBeneficiario.substring(0, 1) + '</span></div>' +
            '</div></div>' +
            '<div class="lineStyle"></div >' +
            '<div class="contDetails">' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-iconPhone"></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.Phone != null) ? x.data.Phone : '') + '</span>' +
            '</div></div>' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-LinkedPaymentMethodCode"></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.LinkedPaymentMethodCode && x.data.LinkedPaymentMethodCode.filter(x => x.Default == true).length > 0) ? x.data.LinkedPaymentMethodCode.filter(x => x.Default == true)[0].PaymentMethodCode : '') + '</span>' +
            '</div></div>' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-Email"></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.Email != null) ? x.data.Email : '') + '</span>' +
            '</div></div>' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-TaxIdentificationNumberCategoryCode"></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.TaxIdentificationNumberCategoryCode != null) ? x.data.TaxIdentificationNumberCategoryCode : '') + ' ' + ((x.data.TaxIdentificationNumber != null) ? x.data.TaxIdentificationNumber : '') + '</span>' +
            '</div></div>' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-location "></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.LinkedAddresses && x.data.LinkedAddresses.filter(x => x.Default == true).length > 0) ? x.data.LinkedAddresses.filter(x => x.Default == true)[0].Name : '') + '</span>' +
            '</div></div>' +
            '<div class="contLine">' +
            '<div class="iconDetail ico-PaymentTermCode"></div>' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((x.data.PaymentTermCode != null) ? x.data.PaymentTermCode : '') + '</span>' +
            '</div></div>' +
            '</div>' +
            '</div>';
    });
    document.getElementById('dtvTarjetas').innerHTML = divContainer;
}

function ShowCardDetail(sender, event) {
}

//#endregion

//#region FILTROS

//#region BUSCADOR PREDICTIVO

function ClearfilterCompany() {
    App.txtFiltroCompany.reset();
    App.txtFiltroCompany.getTrigger(1).hide();
    App.txtFiltroCompany.getTrigger(0).show();
}

function CargarBuscadorPredictivoCompany() {
    dataGridComp = [];
    App.storeCompany.data.items.forEach(oCmp => {
        dataGridComp.push({
            key: oCmp.data.Code.toLowerCase(),
            codigo: oCmp.data.Code,
            id: oCmp.id
        });
    });

    dataGridComp = dataGridComp.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtFiltroCompany";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataGridComp, function (value) {
                    let value1 = value.key;
                    return matcher.test(value1) || matcher.test(normalize(value1));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {
                    var idCol = $(e.currentTarget).attr("data-emplazamientoID");
                    ColumnaID = idCol;
                    filterCompany();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}


var CompanyID = '';

function filterCompany() {
    var logic = App.storeCompany,
        text = App.txtFiltroCompany.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.txtFiltroCompany.getTrigger(1).hide();
        App.txtFiltroCompany.getTrigger(0).show();
        return;
    }
    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (CompanyID != '') {
        var CompIDAux = CompanyID;
        CompanyID = '';
        logic.filterBy(function (node) {
            var valido = false;
            if (CompIDAux == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        App.txtFiltroCompany.getTrigger(0).hide();
        App.txtFiltroCompany.getTrigger(1).show();
        logic.filterBy(function (node) {
            var valido = false;
            if (re.test(node.data.Code)) {
                valido = true;
            }
            return valido;
        });

    }
}

//#endregion

function ReloadCmbFiltro(sender) {
    sender.reset();
    App.storeCompany.reload();
    recargarCombos([sender]);
}

function SelectCmbFiltro(sender) {
    sender.getTrigger(0).show();
    App.storeCompany.reload();
}

function ClearFilters() {
    App.cmbFiltroRol.reset();
    App.cmbFiltroRol.getTrigger(0).hide();
    App.cmbFiltroEntidadesTipos.reset();
    App.cmbFiltroEntidadesTipos.getTrigger(0).hide();
    App.storeCompany.reload();
}

//#endregion