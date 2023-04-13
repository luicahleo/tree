<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntidadesV2Form.aspx.cs" Inherits="TreeCore.Modulos.ThirdParty.EntidadesV2Form" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&callback=initAutocomplete&libraries=places&v=weekly" async defer></script>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdObjeto" />
            <ext:Hidden runat="server" ID="hdCompanyCode" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Handler="App.gridSecciones.getSelectionModel().select(App.gridSecciones.getRootNode().childNodes[0])" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeTiposEntidad"
                AutoLoad="true"
                OnReadData="storeTiposEntidad_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposContrubuyentes"
                AutoLoad="false"
                OnReadData="storeTiposContrubuyentes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposNIF"
                AutoLoad="false"
                OnReadData="storeTiposNIF_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCondicionesPagos"
                AutoLoad="false"
                OnReadData="storeCondicionesPagos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeMonedas"
                runat="server"
                AutoLoad="true"
                OnReadData="storeMonedas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Symbol" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeBancos"
                AutoLoad="false"
                OnReadData="storeBancos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePaises"
                AutoLoad="true"
                OnReadData="storePaises_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Default" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePaymentMethods"
                AutoLoad="true"
                OnReadData="storePaymentMethods_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeRoleCompanies"
                AutoLoad="true"
                OnReadData="storeRoleCompanies_Refresh"
                RemoteSort="false">
                <Listeners>
                    <DataChanged Fn="RoleCompanies" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Name">
                        <Fields>
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="FieldDTO" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnSecciones"
                                Region="West"
                                Collapsible="true"
                                Width="300"
                                Header="false"
                                CollapseDirection="Right"
                                CollapseMode="Header"
                                Collapsed="false"
                                Hidden="false"
                                Layout="FitLayout">
                                <DockedItems>
                                </DockedItems>
                                <Items>
                                    <ext:TreePanel
                                        runat="server"
                                        ID="gridSecciones"
                                        SelectionMemory="false"
                                        Cls="treeGridGeneral"
                                        Header="true"
                                        Title="Sections"
                                        EnableColumnHide="false"
                                        AnchorVertical="100%"
                                        Region="Center"
                                        Hidden="false"
                                        RootVisible="false"
                                        AriaRole="main">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:TreeColumn
                                                    ID="colSeccCode"
                                                    runat="server"
                                                    Filterable="false"
                                                    Flex="1"
                                                    Sortable="true"
                                                    Hidden="false"
                                                    DataIndex="text">
                                                    <Filter>
                                                        <ext:StringFilter />
                                                    </Filter>
                                                </ext:TreeColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:TreeSelectionModel runat="server"
                                                ID="selSeccion"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="SelectSeccion" />
                                                </Listeners>
                                            </ext:TreeSelectionModel>
                                        </SelectionModel>
                                    </ext:TreePanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" Region="Center" Cls="pnForm" Layout="FitLayout">
                                <DockedItems>
                                    <ext:Toolbar Height="0" runat="server" Dock="Top" Cls="tbFlotante">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnGuardarCompany"
                                                Cls="btnSave me-2 mt-3"
                                                Width="100"
                                                Text="<%$ Resources:Comun, strGuardar %>"
                                                Focusable="false"
                                                Disabled="true"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="SaveCompany" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar Height="0" runat="server" Dock="Bottom" Cls="tbFlotante">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnCancelar"
                                                Cls="btnSave me-2"
                                                Width="120"
                                                Text="<%$ Resources:Comun, strCancelar %>"
                                                Focusable="false"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="CancelForm"></Click>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnContinuar"
                                                Cls="btnSave me-2"
                                                Width="100"
                                                Text="<%$ Resources:Comun, strSiguiente %>"
                                                Focusable="false"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="SiguienteSeccion"></Click>
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="formDataModel" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Handler="ShowDataModel(); RoleCompanies();" />
                                            <Hide Fn="SaveDataModel" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strInfoGeneral %>">
                                                <Items>
                                                    <ext:TextField ID="txtCodigo"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtNombre"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtLocalName"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strAlias %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" />
                                                    <ext:TextField ID="txtTelefono1"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strTelefono %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" />
                                                    <%--<ext:DropDownField runat="server"
                                                        meta:resourcekey="txtTelefono1"
                                                        ID="txtTelefono1"
                                                        FieldLabel="<%$ Resources:Comun, strTelefono %>"
                                                        LabelAlign="Left"
                                                        TriggerCls="icoprefix"
                                                        FocusCls="testfocus"
                                                        Cls="telefono"
                                                        Regex="/^[[+]([0-9])*]? ([0-9])*$/">
                                                        <Component>
                                                            <ext:MenuPanel runat="server" ID="MenuPanel1" MaxWidth="120">
                                                                <Menu runat="server"
                                                                    ID="MenuPrefijos2">
                                                                    <Items>
                                                                        <ext:MenuItem
                                                                            runat="server"
                                                                            IconCls="x-loading-indicator"
                                                                            Text="<%$ Resources:Comun, jsMensajeProcesando %>"
                                                                            Focusable="false"
                                                                            HideOnClick="false" />
                                                                    </Items>
                                                                    <Loader runat="server"
                                                                        Mode="Component"
                                                                        DirectMethod="#{DirectMethods}.LoadPrefijos"
                                                                        RemoveAll="true">
                                                                    </Loader>
                                                                    <Listeners>
                                                                        <Click Handler="#{txtTelefono1}.setValue(menuItem.text);" />
                                                                    </Listeners>
                                                                </Menu>
                                                            </ext:MenuPanel>
                                                        </Component>
                                                    </ext:DropDownField>--%>
                                                    <ext:TextField ID="txtEmail"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strEmail %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" />
                                                    <ext:ComboBox
                                                        ID="cmbTipo"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeTiposEntidad"
                                                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbMonedas"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeMonedas"
                                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Code}</h5>
                                                                        <p class="subTitle">{Symbol}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Container runat="server" Cls="ctForm">
                                                <Content>
                                                    <ext:Label runat="server"
                                                        Cls="lblForm"
                                                        Hidden="false"
                                                        Text="<%$ Resources:Comun, strTextRolCompany %>" />
                                                    <ext:Label runat="server"
                                                        Cls="descripcion"
                                                        Hidden="true"
                                                        Text="descripcion" />

                                                    <div id="cardRoleCompanies" class="cardFormCheck"></div>

                                                </Content>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="formFinantial" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowFinantial" />
                                            <Hide Fn="SaveFinantial" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strInfoFinanciera %>">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="cmbTipoContrubuyente"
                                                        runat="server"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeTiposContrubuyentes"
                                                        FieldLabel="<%$ Resources:Comun, strContribuyente %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbTiposNIF"
                                                        runat="server"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeTiposNIF"
                                                        FieldLabel="<%$ Resources:Comun, strTipoNIFCategory %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:TextField ID="txtNIF"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNumIdentificacion %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" />
                                                    <ext:ComboBox
                                                        ID="cmbCondicionesPagos"
                                                        runat="server"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeCondicionesPagos"
                                                        FieldLabel="<%$ Resources:Comun, strCondicionPago %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="formAdditionalInfo" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowAdditionalInfo" />
                                            <Hide Fn="SaveAdditionalInfo" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Hidden="true" Header="true" Title="<%$ Resources:Comun, strInfoAdicional %>">
                                                <Items>
                                                    <ext:Panel
                                                        runat="server"
                                                        Cls="dz-preview dz-file-preview"
                                                        ID="paneEmpty"
                                                        Layout="AutoLayout"
                                                        Margin="15"
                                                        Hidden="true"
                                                        BodyCls="tbGrey"
                                                        Height="300">
                                                    </ext:Panel>
                                                    <ext:Action
                                                        runat="server"
                                                        ID="UploadFileDropzone"
                                                        Text="Subir Archivo"
                                                        Disabled="false"
                                                        Handler="SubirArchivo();" />
                                                    <ext:Panel
                                                        runat="server"
                                                        Cls="dropzone dz-preview dz-file-preview"
                                                        ID="panelCenterShow"
                                                        Layout="AutoLayout"
                                                        Margin="15"
                                                        BodyCls="tbGrey"
                                                        Height="300">
                                                        <LayoutConfig>
                                                        </LayoutConfig>
                                                        <Items>
                                                            <ext:Label
                                                                runat="server"
                                                                Text="<%$ Resources:Comun, strSoltarDocumentos %>"
                                                                DefaultAlign="center"
                                                                Cls="DropAreaText"
                                                                meta:resourceKet="lblDropzone" />
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="ctForm">
                                                <Items>
                                                    <ext:TextArea ID="txtAdditionalInfo"
                                                        LabelAlign="Top"
                                                        LabelCls="car"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                        MaxLength="50"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        ValidationGroup="FORM"
                                                        CausesValidation="false" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="formPaymentMethods" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowPaymentMethod" />
                                            <Hide Fn="SavePaymentMethod" />
                                        </Listeners>
                                        <Items>
                                            <ext:Container runat="server" Cls="ctForm">
                                                <Items>
                                                    <ext:Toolbar runat="server" Dock="Top" Cls="menuCabecera">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:TextField
                                                                ID="txtFiltroPaymentMethods"
                                                                Cls="txtSearchD"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                WidthSpec="400px"
                                                                StyleSpec="margin-right: 50px;"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Handler="ClearfilterPaymentMethods();" Hidden="true" Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Change Fn="filterPaymentMethods" Buffer="250" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                                <Content>
                                                    <div id="cardPaymentMethods" class="cardMultiSelect"></div>
                                                </Content>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server"
                                        ID="formAllBank"
                                        Cls="content100"
                                        Scrollable="Vertical"
                                        Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowBanksAccounts" />
                                        </Listeners>
                                        <DockedItems>
                                        </DockedItems>
                                        <Content>
                                            <div id="contBanksAccounts" class="containerStyle">
                                            </div>
                                        </Content>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="formBank" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowCuentaBancaria" />
                                            <Hide Fn="SaveCuentaBancaria" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strBankAccounts %>">
                                                <Items>
                                                    <ext:TextField ID="txtNameBank"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:ComboBox
                                                        ID="cmbBanco"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeBancos"
                                                        FieldLabel="<%$ Resources:Comun, strBanco %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:TextField ID="txtBankAccount"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strIBAN %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtSwift"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strSWIFT %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextArea ID="txtDescripcion"
                                                        LabelAlign="Left"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                        MaxLength="250"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server"
                                        ID="formAllAddresses"
                                        Cls="content100"
                                        Scrollable="Vertical"
                                        Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowAddresses" />
                                        </Listeners>
                                        <DockedItems>
                                        </DockedItems>
                                        <Content>
                                            <div id="contAddresses" class="containerStyle">
                                            </div>
                                        </Content>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="formAddresses" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowDireccion" />
                                            <Hide Fn="SaveDireccion" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strDirecciones %>">
                                                <Items>
                                                    <ext:Panel
                                                        runat="server"
                                                        Width="500"
                                                        Height="300"
                                                        Border="false"
                                                        StyleSpec="margin: 1rem auto;">
                                                        <Content>
                                                            <input
                                                                id="Input-Search"
                                                                class="FloatingSearchBox"
                                                                type="text"
                                                                style="position: relative; z-index: 10;" />
                                                            <div style="width: 500px; height: 300px; top: -40px;" id="map"></div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:TextField ID="txtCode"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtNombreAddress"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtDireccion"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDireccion %>"
                                                        MaxLength="500"
                                                        Disabled="true"
                                                        DisabledCls="disabledInput"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtAddress2"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDireccion2 %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="CompanyValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtCountry"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strPais %>"
                                                        Disabled="true"
                                                        DisabledCls="disabledInput"
                                                        LabelAlign="Left" />
                                                    <ext:TextField ID="txtLocality"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strLocalidad %>"
                                                        Disabled="true"
                                                        DisabledCls="disabledInput"
                                                        LabelAlign="Left" />
                                                    <ext:TextField ID="txtSubLocality"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strSublocality %>"
                                                        Disabled="true"
                                                        DisabledCls="disabledInput"
                                                        LabelAlign="Left" />
                                                    <ext:TextField ID="txtPostalCode"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigoPostal %>"
                                                        Disabled="true"
                                                        DisabledCls="disabledInput"
                                                        LabelAlign="Left" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
