var ActualAddress;
var ActualBankAccount;
var ActualNode;

var contAddress = 0;
var contBankAccount = 0;

$(document).ready(function ConfigurarToast() {
    Toast.enableTimers(true);
    Toast.setTheme(TOAST_THEME.LIGHT);
    Toast.setPlacement(TOAST_PLACEMENT.TOP_RIGHT);
    Toast.setMaxCount(2);
});

function CancelForm() {
    parent.App.tabPpal.activeTab.close()
}

function SaveCompany() {
    try {
        App[ActualNode.data.Panel].events.hide.listeners[0].fn();
    } catch (e) {

    }
    TreeCore.UploadCompany(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ControlDuplicidadAddresses(sender) {
    if (GetNodeAddresses().childNodes.filter(x => x.data.text == sender.value && x.id != ActualNode.id).length > 0) {
        sender.setValue(ActualNode.data.OriginalCode);
        Toast.create(jsInfo, 'Address Code Already exist', TOAST_STATUS.INFO, 3000);
    }
}

function ControlDuplicidadBanksAccounts(sender) {
    if (GetNodebanks().childNodes.filter(x => x.data.text == sender.value && x.id != ActualNode.id).length > 0) {
        sender.setValue(ActualNode.data.OriginalCode);
        Toast.create(jsInfo, 'Bank Account Code Already exist', TOAST_STATUS.INFO, 3000);
    }
}

function CompanyValid(sender) {
    let btnSave = App.btnGuardarCompany;
    btnSave.disable();
    let valid = true;
    if (sender) {
        if (sender.id == App.txtCode.id) {
            ControlDuplicidadAddresses(sender);
        } else if (sender.id == App.txtNameBank.id) {
            ControlDuplicidadBanksAccounts(sender);
        }
    }

    //#region ActualForm

    Ext.each(App[ActualNode.data.Panel].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.disabled) {
                document.getElementById(c.id).setAttribute('invalid', false);

            } else {
                if (c.cls && c.cls.includes('required') && !c.value) {
                    document.getElementById(c.id).setAttribute('invalid', true);
                    valid = false;
                } else {
                    document.getElementById(c.id).setAttribute('invalid', false);
                }
            }
        }
    });

    if (!valid) {
        ActualNode.addCls('errorNode');
        return;
    } else {
        ActualNode.removeCls('errorNode');
    }

    try {
        App[ActualNode.data.Panel].events.hide.listeners[0].fn();
    } catch (e) {

    }
    //#endregion

    let oCompany = JSON.parse(App.hdObjeto.value);

    //#region DataModel

    if (oCompany.Code == undefined || oCompany.Code == '') {
        valid = false;
        return;
    }
    if (oCompany.Name == undefined || oCompany.Name == '') {
        valid = false;
        return;
    }
    if (oCompany.CompanyTypeCode == undefined || oCompany.CompanyTypeCode == '') {
        valid = false;
        return;
    }
    if (oCompany.CurrencyCode == undefined || oCompany.CurrencyCode == '') {
        valid = false;
        return;
    }
    //#endregion

    //#region Payments Methods

    if (oCompany.LinkedPaymentMethodCode != undefined && oCompany.LinkedPaymentMethodCode.length > 0) {
        oCompany.LinkedPaymentMethodCode.forEach(paymentMethods => {
            if (paymentMethods.PaymentMethodCode == undefined || paymentMethods.PaymentMethodCode == '') {
                valid = false;
                return;
            }
        });
    }

    //#endregion

    //#region Banks Accounts

    if (oCompany.LinkedBankAccount != undefined && oCompany.LinkedBankAccount.length > 0) {
        oCompany.LinkedBankAccount.forEach(bankAccount => {
            if (bankAccount.Code == undefined || bankAccount.Code == '') {
                valid = false;
                return;
            }
            if (bankAccount.BankCode == undefined || bankAccount.BankCode == '') {
                valid = false;
                return;
            }
            if (bankAccount.IBAN == undefined || bankAccount.IBAN == '') {
                valid = false;
                return;
            }

            if (bankAccount.SWIFT == undefined || bankAccount.SWIFT == '') {
                valid = false;
                return;
            }

        });
    }

    //#endregion

    //#region Addresses

    if (oCompany.LinkedAddresses != undefined && oCompany.LinkedAddresses.length > 0) {
        oCompany.LinkedAddresses.forEach(address => {
            if (address.Code == undefined || address.Code == '') {
                valid = false;
                return;
            }
            if (address.Name == undefined || address.Name == '') {
                valid = false;
                return;
            }
            if (address.Address == undefined || address.Address == '') {
                valid = false;
                return;
            }
            if (address.Address.Address1 == undefined || address.Address.Address1 == '') {
                valid = false;
                return;
            }
            if (address.Address.PostalCode == undefined || address.Address.PostalCode == '') {
                valid = false;
                return;
            }
            if (address.Address.Locality == undefined || address.Address.Locality == '') {
                valid = false;
                return;
            }
            if (address.Address.Country == undefined || address.Address.Country == '') {
                valid = false;
                return;
            }

        });
    }

    //#endregion

    if (valid) {
        btnSave.enable();
    }
}

function SelectSeccion(sender, panel) {
    if (panel.id == 'root') {
        panel = App.gridSecciones.getRootNode().childNodes[0];
    }
    if (panel.childNodes.length > 0 && !panel.isExpanded()) {
        App.btnContinuar.show();
    }
    App.formDataModel.hide();
    App.formFinantial.hide();
    App.formAdditionalInfo.hide();
    App.formPaymentMethods.hide();
    App.formAllBank.hide();
    App.formBank.hide();
    App.formAllAddresses.hide();
    App.formAddresses.hide();
    ActualAddress = panel.data.AddressCode;
    ActualBankAccount = panel.data.BankCode;
    ActualNode = panel;
    ClearSeccion(panel.data.Panel);
    App[panel.data.Panel].show();
}

function ClearSeccion(seccion) {
    Ext.each(App[seccion].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.value != null || c.value != undefined) {
                c.reset();
            }
        }
    });
}

function SiguienteSeccion() {
    if (ActualNode.childNodes.length > 0) {
        ActualNode.expand();
    }
    let oNodo = SiguienteNodo(ActualNode);
    if (!oNodo) {
        App.gridSecciones.getSelectionModel().select(App.gridSecciones.getRootNode().childNodes[0]);
        App.btnContinuar.hide();
    } else {
        App.gridSecciones.getSelectionModel().select(oNodo);
    }
}

//#region Control Nodos

function SiguienteNodo(ActualNode) {
    let esSig = false;
    let node;
    App.gridSecciones.getRootNode().childNodes.forEach(a => {
        if (esSig) {
            node = a;
            esSig = false
        }
        if (a.id == ActualNode.id) {
            esSig = true;
        }
        if (!node) {
            node = RecorrerNodo(a, ActualNode, esSig);
        }
        if (node) {
            return node;
        }
    });
    return node;
}

function RecorrerNodo(Node, ActualNode, esSig) {
    let node;
    Node.childNodes.forEach(a => {
        if (esSig) {
            node = a;
            esSig = false
        }
        if (a.id == ActualNode.id) {
            esSig = true;
        }
        if (!node) {
            node = RecorrerNodo(a, ActualNode, esSig);
        }
        if (node) {
            return node;
        }
    });
    return node;
}

function GetNodeAddressActual() {
    let Node;
    Node = GetNodeAddresses().childNodes.filter(x => x.data.AddressCode == ActualAddress)[0];
    return Node;
}

function GetNodeAddresses() {
    let Node;
    Node = App.gridSecciones.getRootNode().childNodes.filter(x => x.data.Panel == 'formAllAddresses')[0];
    return Node;
}

function GetNodeBankActual() {
    let Node;
    Node = GetNodebanks().childNodes.filter(x => x.data.BankCode == ActualBankAccount)[0];
    return Node;
}

function GetNodebanks() {
    let Node;
    Node = App.gridSecciones.getRootNode().childNodes.filter(x => x.data.Panel == 'formAllBank')[0];
    return Node;
}

//#endregion

//#region TapDataModel

function ShowDataModel() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    App.txtCodigo.setValue(oCompany.Code);
    App.txtNombre.setValue(oCompany.Name);
    App.txtLocalName.setValue(oCompany.Alias);
    App.txtTelefono1.setValue(oCompany.Phone);
    App.txtEmail.setValue(oCompany.Email);
    App.cmbTipo.setValue(oCompany.CompanyTypeCode);
    App.cmbMonedas.setValue(oCompany.CurrencyCode);

    for (let item of document.getElementsByClassName('RolCompany')) {
        if (item.getAttribute("code") == "Owner") {
            item.setAttribute("activo", oCompany.Owner);
        }
        if (item.getAttribute("code") == "Supplier") {
            item.setAttribute("activo", oCompany.Supplier);
        }
        if (item.getAttribute("code") == "Customer") {
            item.setAttribute("activo", oCompany.Customer);
        }
        if (item.getAttribute("code") == "Payee") {
            item.setAttribute("activo", oCompany.Payee);
        }
    }
}

function SaveDataModel() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    oCompany.Code = App.txtCodigo.value;
    oCompany.Name = App.txtNombre.value;
    oCompany.Alias = App.txtLocalName.value;
    oCompany.Phone = App.txtTelefono1.value;
    oCompany.Email = App.txtEmail.value;
    oCompany.CompanyTypeCode = App.cmbTipo.value;
    oCompany.CurrencyCode = App.cmbMonedas.value;
    for (let item of document.getElementsByClassName('RolCompany')) {
        if (item.getAttribute("code") == "Owner") {
            oCompany.Owner = item.getAttribute("activo");
        }
        if (item.getAttribute("code") == "Supplier") {
            oCompany.Supplier = item.getAttribute("activo");
        }
        if (item.getAttribute("code") == "Customer") {
            oCompany.Customer = item.getAttribute("activo");
        }
        if (item.getAttribute("code") == "Payee") {
            oCompany.Payee = item.getAttribute("activo");
        }
    }
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

function RoleCompanies() {
    let divContainer = '';
    let oCompany = JSON.parse(App.hdObjeto.value);

    App.storeRoleCompanies.data.items.forEach(x => {
        divContainer += '<div class="contCardCheck RolCompany" code="' + x.data.FieldDTO + '" onclick="selectRolCompany(this, event)" activo="' + (oCompany[x.data.FieldDTO]) + '">' +
            '<div class="checkCard"><div></div></div>' +
            '<span class="nameCard" code="' + x.data.FieldDTO + '">' + x.data.Name + '</span>' +
            '</div>';
    });
    document.getElementById('cardRoleCompanies').innerHTML = divContainer;
}

function selectRolCompany(sender, event) {
    event.stopPropagation();

    if (sender.getAttribute("activo") == "true") {
        sender.setAttribute("activo", "false");
    }
    else {
        sender.setAttribute("activo", "true");
    }
}

//#endregion

//#region Informacion Financiera

function ShowFinantial() {
    if (App.storeTiposContrubuyentes.data.items.length == 0) {
        App.storeTiposContrubuyentes.reload();
    }
    if (App.storeTiposNIF.data.items.length == 0) {
        App.storeTiposNIF.reload();
    }
    if (App.storeCondicionesPagos.data.items.length == 0) {
        App.storeCondicionesPagos.reload();
    }
    let oCompany = JSON.parse(App.hdObjeto.value);
    App.cmbTipoContrubuyente.setValue(oCompany.TaxpayerTypeCode);
    App.cmbTiposNIF.setValue(oCompany.TaxIdentificationNumberCategoryCode);
    App.txtNIF.setValue(oCompany.TaxIdentificationNumber);
    App.cmbCondicionesPagos.setValue(oCompany.PaymentTermCode);
    //App.cmbPaises.setValue(oCompany.);
}

function SaveFinantial() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    oCompany.TaxpayerTypeCode = App.cmbTipoContrubuyente.value;
    oCompany.TaxIdentificationNumberCategoryCode = App.cmbTiposNIF.value;
    oCompany.TaxIdentificationNumber = App.txtNIF.value;
    oCompany.PaymentTermCode = App.cmbCondicionesPagos.value;
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

//#endregion

//#region Adicional

function ShowAdditionalInfo() {

}

function SaveAdditionalInfo() {

}

//#endregion

//#region Metodos de Pago

function ShowPaymentMethod() {
    if (App.storePaymentMethods.data.items.length == 0) {
        App.storePaymentMethods.reload();
    }
    ClearfilterPaymentMethods();
    PaymentMethods();
    let oCompany = JSON.parse(App.hdObjeto.value);
    if (!oCompany.LinkedPaymentMethodCode) {
        oCompany.LinkedPaymentMethodCode = [];
    }
    for (let item of document.getElementsByClassName('PaymentMethods')) {
        let PaymentMethod = oCompany.LinkedPaymentMethodCode.filter(x => x.PaymentMethodCode == item.getAttribute("code"))[0];
        if (PaymentMethod) {
            item.setAttribute("activo", "true");
        } else {
            item.setAttribute("activo", "false");
        }
        if (PaymentMethod && PaymentMethod.Default == true) {
            item.getElementsByClassName("defaultCard")[0].setAttribute("default", "true");
        }
    }
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

function SavePaymentMethod() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    oCompany.LinkedPaymentMethodCode = [];
    let PaymentMethods;

    for (let item of document.getElementsByClassName('PaymentMethods')) {
        if (item.getAttribute("activo") == "true") {
            PaymentMethods = {
                PaymentMethodCode: item.getAttribute("code"),
                Default: item.getElementsByClassName("defaultCard")[0].getAttribute("default")
            };
            oCompany.LinkedPaymentMethodCode.push(PaymentMethods);
        }
    }

    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

function PaymentMethods() {
    if (App.storePaymentMethods.data.items.length == 0) {
        App.storePaymentMethods.reload();
    }
    let divContainer = '';
    let company = JSON.parse(App.hdObjeto.value);

    if (!company.LinkedPaymentMethodCode) {
        company.LinkedPaymentMethodCode = [];
    }

    App.storePaymentMethods.data.items.forEach(x => {
        divContainer += '<div class="contCard PaymentMethods" code="' + x.data.Code + '" onclick="selectMethods(this, event)" activo="' + (company.LinkedPaymentMethodCode.filter(c => c.PaymentMethodCode == x.data.Code).length > 0) + '">' +
            '<div class="checkCard"><div></div></div>' +
            '<span class="nameCard">' + x.data.Name + '</span>' +
            '<div class="defaultCard" onclick="DefaultMethods(this, event)" default="' + (company.LinkedPaymentMethodCode.filter(c => c.PaymentMethodCode == x.data.Code && c.Default == "true").length > 0) + '"></div>' +
            '</div>';
    });
    document.getElementById('cardPaymentMethods').innerHTML = divContainer;
}

function selectMethods(sender, event) {
    event.stopPropagation();

    if (sender.getAttribute("activo") == "true") {
        if (sender.childNodes[2].getAttribute("default") == "false") {
            sender.setAttribute("activo", "false");
        }
    }
    else {
        sender.setAttribute("activo", "true");
    }
}

function DefaultMethods(sender, event) {
    event.stopPropagation();
    let def = sender.getAttribute("default");

    for (let item of document.getElementsByClassName('defaultCard')) {
        item.setAttribute("default", "false");
    }

    if (def == "true") {
        sender.setAttribute("default", "false");
    }
    else {
        sender.setAttribute("default", "true");
        sender.parentElement.setAttribute("activo", "true");
    }
}

function ClearfilterPaymentMethods() {
    App.txtFiltroPaymentMethods.reset();
    App.txtFiltroPaymentMethods.getTrigger(1).hide();
    App.txtFiltroPaymentMethods.getTrigger(0).show();
}

function filterPaymentMethods(sender) {
    if (sender.value == '') {
        sender.getTrigger(1).hide();
        sender.getTrigger(0).show();
        return;
    } else {
        sender.getTrigger(0).hide();
        sender.getTrigger(1).show();
    }
    var $paymentMethods = $('.PaymentMethods');
    var filterVal = sender.value;

    if (filterVal == '') {
        $paymentMethods.show();
    } else {
        $paymentMethods.hide().filter(function (x, e) { return e.getElementsByClassName('nameCard')[0].innerHTML.toLowerCase().includes(filterVal.toLowerCase()) }).show()
    }
}

//#endregion

//#region Agregar Cuenta Bancaria

function AddNewBankAccount() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    let code = contBankAccount++;
    while (GetNodebanks().childNodes.filter(x => x.data.text == jsCuentaBancaria + code).length > 0) {
        code = contBankAccount++;
    }
    GetNodebanks().appendChild({
        text: jsCuentaBancaria + code,
        leaf: true,
        Panel: 'formBank',
        AddressCode: '#None',
        BankCode: code,
        OriginalCode: jsCuentaBancaria + code,
        expanded: false,
        expandable: false,
    });
    let bankAccount = {};
    bankAccount.Code = jsCuentaBancaria + code;
    if (oCompany.LinkedBankAccount == null)
        oCompany.LinkedBankAccount = [];
    oCompany.LinkedBankAccount.push(bankAccount);
    App.hdObjeto.setValue(JSON.stringify(oCompany));
    ShowBanksAccounts();
    CompanyValid();
}

function SelectBankAccount(sender) {
    let BankCode = sender.getAttribute('code');
    GetNodebanks().expand();
    let NodeBankAccount = GetNodebanks().childNodes.filter(x => x.data.text == BankCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeBankAccount);
}

function editBankAccount(sender) {
    let BankCode = sender.parentElement.parentElement.getAttribute('code');
    GetNodebanks().expand();
    let NodeBankAccount = GetNodebanks().childNodes.filter(x => x.data.text == BankCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeBankAccount);
}

var DelBankAcc;

function deleteBankAccount(sender) {
    DelBankAcc = sender.parentElement.parentElement.getAttribute('code');
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxdeleteBankAccount,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxdeleteBankAccount(button) {
    if (button == 'yes' || button == 'si') {
        let oCompany = JSON.parse(App.hdObjeto.value);
        let NodeBankAccount = GetNodebanks().childNodes.filter(x => x.data.text == DelBankAcc)[0];
        oCompany.LinkedBankAccount = oCompany.LinkedBankAccount.filter(function (value, index, arr) {
            return value.Code != DelBankAcc;
        });
        GetNodebanks().removeChild(NodeBankAccount);
        App.hdObjeto.setValue(JSON.stringify(oCompany));
        ShowBanksAccounts();
        CompanyValid();
    }
}

//#endregion

//#region Cuenta Bancaria

function ShowCuentaBancaria() {
    if (App.storeBancos.data.items.length == 0) {
        App.storeBancos.reload();
    }
    let oCompany = JSON.parse(App.hdObjeto.value);
    let oNode = GetNodeBankActual();
    let cuenta = {};
    if (oCompany.LinkedBankAccount.length > 0) {
        cuenta = oCompany.LinkedBankAccount.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!cuenta) {
        cuenta = {}
    }
    App.txtNameBank.setValue(cuenta.Code);
    App.txtBankAccount.setValue(cuenta.IBAN);
    App.cmbBanco.setValue(cuenta.BankCode);
    App.txtDescripcion.setValue(cuenta.Description);
    App.txtSwift.setValue(cuenta.SWIFT);
}

function SaveCuentaBancaria() {
    let oNode = GetNodeBankActual();
    let oCompany = JSON.parse(App.hdObjeto.value);
    let cuenta = {};

    if (oCompany.LinkedBankAccount.length > 0) {
        cuenta = oCompany.LinkedBankAccount.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!cuenta) {
        cuenta = {};
    }

    cuenta.Code = App.txtNameBank.value;
    if (cuenta.Code == '') {
        cuenta.Code = oNode.data.OriginalCode;
    }
    oNode.data.text = cuenta.Code;

    cuenta.IBAN = App.txtBankAccount.value;
    cuenta.BankCode = App.cmbBanco.value;
    cuenta.SWIFT = App.txtSwift.value;
    cuenta.Description = App.txtDescripcion.value;
    if (oCompany.LinkedBankAccount.length == 0 || !oCompany.LinkedBankAccount.filter(x => x.Code == oNode.data.text).length) {
        oCompany.LinkedBankAccount.push(cuenta);
    } else {
        let index = oCompany.LinkedBankAccount.findIndex(x => x.Code == oNode.data.text)[0];
        oCompany.LinkedBankAccount[index] = cuenta;
    }
    oNode.save()
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

function ShowBanksAccounts() {
    if (App.storeBancos.data.items.length == 0) {
        App.storeBancos.reload();
    }
    let html = '';
    let Contenedor = document.getElementById('contBanksAccounts');
    let oCompany = JSON.parse(App.hdObjeto.value);
    html += CreateCardNew(jsAddNew, 'AddNewBankAccount');
    if (oCompany.LinkedBankAccount != null) {
        oCompany.LinkedBankAccount.forEach(x => {
            html += CreateCard(x.Code, 'ico-banks', [
                { title: 'IBAN', text: x.IBAN },
                { title: 'Type', text: x.BankCode },
                { title: 'description', text: x.Description }
            ], 'SelectBankAccount', [
                { btn: 'btnEdit', func: 'editBankAccount' },
                { btn: 'btnDelete', func: 'deleteBankAccount' }
            ]);
        });
    }
    Contenedor.innerHTML = html;
}

//#endregion

//#region Agregar Direccion

function AddNewAddress() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    let code = contAddress++;
    while (GetNodeAddresses().childNodes.filter(x => x.data.text == jsDireccion + code).length > 0) {
        code = contAddress++;
    }
    GetNodeAddresses().appendChild({
        text: jsDireccion + code,
        leaf: true,
        Panel: 'formAddresses',
        AddressCode: code,
        BankCode: '#None',
        OriginalCode: jsDireccion + code,
        expanded: false,
        expandable: false,
    });
    let address = {};
    address.Code = jsDireccion + code;
    address.Default = 'false';
    if (oCompany.LinkedAddresses == null)
        oCompany.LinkedAddresses = [];
    oCompany.LinkedAddresses.push(address);
    App.hdObjeto.setValue(JSON.stringify(oCompany));
    ShowAddresses();
    CompanyValid();
}

function SelectAddress(sender) {
    let AddressCode = sender.getAttribute('code');
    GetNodeAddresses().expand();
    let NodeAddress = GetNodeAddresses().childNodes.filter(x => x.data.text == AddressCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeAddress);
}

function editAddress(sender) {
    let AddressCode = sender.parentElement.parentElement.getAttribute('code');
    GetNodeAddresses().expand();
    let NodeAddress = GetNodeAddresses().childNodes.filter(x => x.data.text == AddressCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeAddress);
}

var DelAddress;
var AddressDefault;

function deleteAddress(sender) {
    DelAddress = sender.parentElement.parentElement.getAttribute('code');
    AddressDefault = sender.parentElement.parentElement.getElementsByClassName('defaultCard')[0].getAttribute('default');
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxdeleteAddress,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxdeleteAddress(button) {
    if (button == 'yes' || button == 'si') {
        let oCompany = JSON.parse(App.hdObjeto.value);
        if (AddressDefault != "true") {
            let NodeAddress = GetNodeAddresses().childNodes.filter(x => x.data.text == DelAddress)[0];
            oCompany.LinkedAddresses = oCompany.LinkedAddresses.filter(function (value, index, arr) {
                return value.Code != DelAddress;
            });
            GetNodeAddresses().removeChild(NodeAddress);
            App.hdObjeto.setValue(JSON.stringify(oCompany));
            ShowAddresses();
            CompanyValid();
        }
        else {
            Toast.create(jsInfo, jsEliminarDefecto, TOAST_STATUS.INFO, 3000);
        }
    }
}

//#endregion

//#region Direcciones

var geocoder;
var map;
var marker;
var direccion = {};
var markers = [];

function ShowDireccion() {
    let oCompany = JSON.parse(App.hdObjeto.value);
    let oNode = GetNodeAddressActual();
    if (oCompany.LinkedAddresses.length > 0) {
        direccion = oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!direccion) {
        direccion = {
            Address: { Address2: '', Sublocality: '' }
        };
    }
    if (!direccion.Address) {
        direccion.Address = { Address2: '', Sublocality: '' };
    }
    App.txtCode.setValue(direccion.Code);
    App.txtNombreAddress.setValue(direccion.Name);
    App.txtDireccion.setValue(direccion.Address.Address1);
    App.txtAddress2.setValue(direccion.Address.Address2);
    App.txtCountry.setValue(direccion.Address.Country);
    App.txtLocality.setValue(direccion.Address.Locality);
    App.txtSubLocality.setValue(direccion.Address.Sublocality);
    App.txtPostalCode.setValue(direccion.Address.PostalCode);

    let paisDefecto = App.storePaises.data.items.filter(x => x.data.Default)[0].data.Name;

    if (oCompany.LinkedAddresses.length > 0) {
        if (oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0].Address.Address1) {
            let address = oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0].Address.Address1;
            MapFocusCountryDefault(address, 8);
        }
        else {
            MapFocusCountryDefault(paisDefecto, 5);
        }
    }
    else {
        MapFocusCountryDefault(paisDefecto, 5);
    }
}

function SaveDireccion() {
    let oNode = GetNodeAddressActual();
    let oCompany = JSON.parse(App.hdObjeto.value);

    if (oCompany.LinkedAddresses.length > 0) {
        direccion = oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!direccion) {
        direccion = {
            Address: { Address2: '', Sublocality: '' }
        };
    }
    if (!direccion.Address) {
        direccion.Address = { Address2: '', Sublocality: '' };
    }

    direccion.Code = App.txtCode.value;
    if (direccion.Code == '') {
        direccion.Code = oNode.data.OriginalCode;
    }
    oNode.data.text = direccion.Code;

    direccion.Name = App.txtNombreAddress.value;
    direccion.Address.Address2 = App.txtAddress2.value;

    if (oCompany.LinkedAddresses.length == 0 || !oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text).length) {
        oCompany.LinkedAddresses.push(direccion);
    } else {
        let index = oCompany.LinkedAddresses.findIndex(x => x.Code == oNode.data.text)[0];
        oCompany.LinkedAddresses[index] = direccion;
    }
    oNode.save()
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

var searchBox;

function initAutocomplete() {
    geocoder = new google.maps.Geocoder();
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 0, lng: 0 },
        zoom: 8,
        streetViewControl: false,
        fullscreenControl: false,
        mapTypeId: "roadmap"
    });
    // Create the search box and link it to the UI element.
    const input = document.getElementById("Input-Search");
    searchBox = new google.maps.places.SearchBox(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // Clear out the old markers.
        markers.forEach((marker) => {
            marker.setMap(null);
        });
        markers = [];

        // For each place, get the icon, name and location.
        const bounds = new google.maps.LatLngBounds();
        let address = input.value;
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == 'OK') {
                let oCompany = JSON.parse(App.hdObjeto.value);
                let oNode = GetNodeAddressActual();
                if (oCompany.LinkedAddresses.length > 0) {
                    direccion = oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0];
                }
                if (!direccion) {
                    direccion = {
                        Address: { Sublocality: '', PostalCode: '' }
                    };
                }
                direccion.Address = {};
                oNode.data.text = direccion.Code;

                let ditectionResult = results[0].address_components;

                direccion.Address.Address1 = results[0].formatted_address;
                if (ditectionResult.filter(a => a.types.includes('postal_code'))[0] != undefined) {
                    direccion.Address.PostalCode = ditectionResult.filter(a => a.types.includes('postal_code'))[0].long_name;
                }
                if (ditectionResult.filter(a => a.types.includes('locality'))[0] != undefined) {
                    direccion.Address.Locality = ditectionResult.filter(a => a.types.includes('locality'))[0].long_name;
                }
                if (ditectionResult.filter(a => a.types.includes('postal_town'))[0] != undefined) {
                    direccion.Address.Sublocality = ditectionResult.filter(a => a.types.includes('postal_town'))[0].long_name;
                    if (!direccion.Address.Locality || direccion.Address.Locality == '')
                        direccion.Address.Locality = ditectionResult.filter(a => a.types.includes('postal_town'))[0].long_name;
                }
                if (!direccion.Address.Sublocality || direccion.Address.Sublocality == '')
                    direccion.Address.Sublocality = direccion.Address.Locality;
                if (ditectionResult.filter(a => a.types.includes('country'))[0] != undefined) {
                    direccion.Address.Country = ditectionResult.filter(a => a.types.includes('country'))[0].long_name;
                }
                App.txtDireccion.setValue(direccion.Address.Address1);
                App.txtCountry.setValue(direccion.Address.Country);
                App.txtLocality.setValue(direccion.Address.Locality);
                App.txtSubLocality.setValue(direccion.Address.Sublocality);
                App.txtPostalCode.setValue(direccion.Address.PostalCode);
                if (oCompany.LinkedAddresses.length == 0 || !oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text).length) {
                    oCompany.LinkedAddresses.push(direccion);
                } else {
                    let index = oCompany.LinkedAddresses.findIndex(x => x.Code == oNode.data.text)[0];
                    oCompany.LinkedAddresses[index] = direccion;
                }
                oNode.save()
                App.hdObjeto.setValue(JSON.stringify(oCompany));
            }
            else {
                Toast.create(jsAtencion, jsInactividad, TOAST_STATUS.WARNING, 30000);
            }

        });

        places.forEach((place) => {
            if (!place.geometry || !place.geometry.location) {
                console.log("Returned place contains no geometry");
                return;
            }

            // Create a marker for each place.
            markers.push(
                new google.maps.Marker({
                    map,
                    title: place.name,
                    position: place.geometry.location,
                })
            );
            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng, map);
    });
}

function placeMarker(location, map) {
    markers.forEach((marker) => {
        marker.setMap(null);
    });
    markers = [];

    markers.push(
        new google.maps.Marker({
            position: location,
            map: map
        })
    );

    let latLong = new google.maps.LatLng({ lat: markers[0].position.lat(), lng: markers[0].position.lng() });
    geocoder.geocode({
        latLng: latLong
    }, function (results, status) {
        if (status == 'OK') {
            let oCompany = JSON.parse(App.hdObjeto.value);
            let oNode = GetNodeAddressActual();
            if (oCompany.LinkedAddresses.length > 0) {
                direccion = oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text)[0];
            }
            if (!direccion) {
                direccion = {
                    Address: { Sublocality: '', PostalCode: '' }
                };
            }
            direccion.Address = {};
            oNode.data.text = direccion.Code;

            let ditectionResult = results[0].address_components;

            direccion.Address.Address1 = results[0].formatted_address;
            if (ditectionResult.filter(a => a.types.includes('postal_code'))[0] != undefined) {
                direccion.Address.PostalCode = ditectionResult.filter(a => a.types.includes('postal_code'))[0].long_name;
            }
            if (ditectionResult.filter(a => a.types.includes('locality'))[0] != undefined) {
                direccion.Address.Locality = ditectionResult.filter(a => a.types.includes('locality'))[0].long_name;
            }
            if (ditectionResult.filter(a => a.types.includes('postal_town'))[0] != undefined) {
                direccion.Address.Sublocality = ditectionResult.filter(a => a.types.includes('postal_town'))[0].long_name;
                if (!direccion.Address.Locality || direccion.Address.Locality == '')
                    direccion.Address.Locality = ditectionResult.filter(a => a.types.includes('postal_town'))[0].long_name;
            }
            if (!direccion.Address.Sublocality || direccion.Address.Sublocality == '')
                direccion.Address.Sublocality = direccion.Address.Locality;
            if (ditectionResult.filter(a => a.types.includes('country'))[0] != undefined) {
                direccion.Address.Country = ditectionResult.filter(a => a.types.includes('country'))[0].long_name;
            }
            App.txtDireccion.setValue(direccion.Address.Address1);
            App.txtCountry.setValue(direccion.Address.Country);
            App.txtLocality.setValue(direccion.Address.Locality);
            App.txtSubLocality.setValue(direccion.Address.Sublocality);
            App.txtPostalCode.setValue(direccion.Address.PostalCode);
            if (oCompany.LinkedAddresses.length == 0 || !oCompany.LinkedAddresses.filter(x => x.Code == oNode.data.text).length) {
                oCompany.LinkedAddresses.push(direccion);
            } else {
                let index = oCompany.LinkedAddresses.findIndex(x => x.Code == oNode.data.text)[0];
                oCompany.LinkedAddresses[index] = direccion;
            }
            oNode.save()
            App.hdObjeto.setValue(JSON.stringify(oCompany));
        }
        else {
            Toast.create(jsAtencion, jsInactividad, TOAST_STATUS.WARNING, 30000);
        }
    });
}

function MapFocusCountryDefault(pais, zoom) {
    markers.forEach((marker) => {
        marker.setMap(null);
    });
    markers = [];

    geocoder.geocode({ 'address': pais }, function (results, status) {
        if (status == 'OK') {
            map.setCenter(results[0].geometry.location);
            markers.push(
                new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                })
            );
            map.setZoom(zoom);
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}

function ShowAddresses() {
    let html = '';
    let Contenedor = document.getElementById('contAddresses');
    let oCompany = JSON.parse(App.hdObjeto.value);
    html += CreateCardNew(jsAddNew, 'AddNewAddress');
    if (oCompany.LinkedAddresses != null) {
        oCompany.LinkedAddresses.forEach(x => {
            html += CreateCard(x.Code, 'ico-geolocation', [
                { title: 'Address', text: (!x.Address) ? '' : x.Address.Address1 },
                { title: 'Description', text: x.Name }
            ], 'SelectAddress', [
                { btn: 'btnEdit', func: 'editAddress' },
                { btn: 'btnDelete', func: 'deleteAddress' }
            ],
                x.Default);
        });
    }
    Contenedor.innerHTML = html;
}

function DefaultMethodsAddresses(sender, event) {
    event.stopPropagation();
    let def = sender.getAttribute("default");

    for (let item of document.getElementById('contAddresses').getElementsByClassName('defaultCard')) {
        item.setAttribute("default", "false");
    }

    if (def == "true") {
        sender.setAttribute("default", "false");
    }
    else {
        sender.setAttribute("default", "true");
    }
    let address = sender.parentElement.getAttribute("code");

    let oCompany = JSON.parse(App.hdObjeto.value);
    oCompany.LinkedAddresses.forEach(x => {
        x.Default = x.Code == address;
    });
    App.hdObjeto.setValue(JSON.stringify(oCompany));
}

//#endregion

function RecargarCombo(sender, registro, index) {
    recargarCombos([sender], function Fin(fin) {
        if (fin) {
            sender.reset();
            sender.cancelFocus()
        }
    });
}

function SeleccionarCombo(sender, registro, index) {
    sender.getTrigger(0).show();
}

function CreateCard(title, image, Items, f, btns, def) {
    let html = '';

    html += '<div class="contCard" code="' + title + '" ondblclick="' + f + '(this)">';
    if (def != undefined) {
        html += '<div class="defaultCard Hidden" onclick="DefaultMethodsAddresses(this, event)" default="' + def + '"></div>';
    }
    html += '<div class="contHeader">' +
        '<object data="/ima/' + image + '.svg" class="imgTitle"> </object>' +
        '<h4 class="itemCardTitle">' + title + '</h4>' +
        '</div>' +
        '<div class="contDetails">';

    Items.forEach(x => {
        html += '<div class="contLine">' +

            '<div class="iconText">' +
            '<span class="iconSpan">' + ((!x.text) ? '' : x.text) + '</span>' +
            '</div>' +
            '</div>';
    });

    html += '</div>' +
        '<div class="btnAcciones">';
    btns.forEach(x => {
        html += '<div class="' + x.btn + ' Hidden" onclick="' + x.func + '(this)">' +
            '</div>';
    });
    html += '</div ></div>';

    return html;
}

function CreateCardNew(title, f) {
    let html = '';

    html += '<div class="contCard" onclick="' + f + '(this)">' +
        '<div class="contAddNew">' +
        '<div class="icon-add"></div >' +
        '<div class="textAdd">' + title + '</div>' +
        '</div>';


    html += '</div>';

    return html;
}