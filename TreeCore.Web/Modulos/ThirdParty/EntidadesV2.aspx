<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntidadesV2.aspx.cs" Inherits="TreeCore.ModGlobal.EntidadesV2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="/JS/common.js"></script>
</head>
<body>
    <link href="/CSS/mainStyle.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdTraducciones" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDCompanyBuscador" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Menu runat="server"
                ID="mnOpciones">
                <Items>
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, strDetalles %>"
                        ID="mnoDetalles"
                        Hidden="true"
                        Handler="MostrarDetalleCompany(this.up().dataRecord)" />
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsActivar %>"
                        ID="mnoActivar"
                        Handler="ActivarCompany(this.up().dataRecord.Code)" />
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsEliminar %>"
                        ID="mnoEliminar"
                        Cls="spanRedV2"
                        Handler="DeleteCompany(this.up().dataRecord.Code)" />
                </Items>
            </ext:Menu>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server" Region="Center" Layout="FitLayout">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tlbBase"
                                        Dock="Top"
                                        Cls="tlbGrid">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnAnadir"
                                                Cls="btnAnadir"
                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                Handler="AddCompanies();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnRefrescar"
                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                Cls="btnRefrescar"
                                                Handler="Refrescar();" />
                                            <ext:Button runat="server"
                                                ID="btnDescargar"
                                                Cls="btnDescargar"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                Handler="DescargarCompanies();" />
                                            <ext:ToolbarFill />
                                            <ext:Button runat="server"
                                                Text="<%$ Resources:Comun, strVerDesactivados %>"
                                                EnableToggle="true"
                                                ID="btnAllCompanies"
                                                Cls="btnActivarDesactivarV2"
                                                PressedCls="btnActivarDesactivarV2Pressed"
                                                MinWidth="185"
                                                Pressed="false"
                                                TextAlign="Left"
                                                Focusable="false"
                                                OverCls="none">
                                                <Listeners>
                                                    <Click Fn="Refrescar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:TextField
                                                ID="txtFiltroCompany"
                                                Cls="txtSearchD"
                                                runat="server"
                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                Width="300"
                                                EnableKeyEvents="true">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search" />
                                                    <ext:FieldTrigger Handler="ClearfilterCompany();" Hidden="true" Icon="clear" />
                                                </Triggers>
                                                <Listeners>
                                                    <Change Handler="App.storeCompany.reload();" Buffer="250" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbFilter"
                                        Dock="Top">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnSupplier"
                                                ToolTip="<%$ Resources:Comun, strProveedores %>"
                                                EnableToggle="true"
                                                Cls="btnRolFilter"
                                                PressedCls="iconRed"
                                                Pressed="false"
                                                Focusable="false"
                                                OverCls="none">
                                                <Listeners>
                                                    <AfterRender Fn="SetTextRolFilter" />
                                                    <Click Fn="Refrescar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnOwner"
                                                ToolTip="<%$ Resources:Comun, strPropietario %>"
                                                EnableToggle="true"
                                                Cls="btnRolFilter"
                                                PressedCls="iconBlue"
                                                Pressed="false"
                                                Focusable="false"
                                                OverCls="none">
                                                <Listeners>
                                                    <AfterRender Fn="SetTextRolFilter" />
                                                    <Click Fn="Refrescar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnCustomer"
                                                ToolTip="<%$ Resources:Comun, strEsCliente %>"
                                                EnableToggle="true"
                                                Cls="btnRolFilter"
                                                PressedCls="iconGreen"
                                                Pressed="false"
                                                Focusable="false"
                                                OverCls="none">
                                                <Listeners>
                                                    <AfterRender Fn="SetTextRolFilter" />
                                                    <Click Fn="Refrescar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnPayee"
                                                ToolTip="<%$ Resources:Comun, strBeneficiario %>"
                                                EnableToggle="true"
                                                Cls="btnRolFilter"
                                                PressedCls="iconOrange"
                                                Pressed="false"
                                                Focusable="false"
                                                OverCls="none">
                                                <Listeners>
                                                    <AfterRender Fn="SetTextRolFilter" />
                                                    <Click Fn="Refrescar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ComboBox
                                                ID="cmbFiltroEntidadesTipos"
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strEntidadesTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Name"
                                                QueryMode="Local"
                                                Mode="Local"
                                                AllowBlank="true"
                                                Hidden="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="storeTiposEntidad"
                                                        RemotePaging="false"
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
                                                </Store>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                                <Listeners>
                                                    <Select Fn="SelectCmbFiltro" />
                                                    <TriggerClick Fn="ReloadCmbFiltro" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ToolbarFill />
                                            <ext:Button runat="server"
                                                ID="btnClearFilters"
                                                ToolTip="<%$ Resources:Comun, strLimpiarFiltro %>"
                                                Cls="btnNoFiltros"
                                                Hidden="true"
                                                Handler="ClearFilters();" />
                                            <ext:Button runat="server"
                                                ID="btnGuardarVista"
                                                Cls="btn-ppal"
                                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                Focusable="false"
                                                MarginSpec="0 15 0 0"
                                                PressedCls="none"
                                                Hidden="true">
                                                <Listeners>
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbViews"
                                        Dock="Top">
                                        <Items>
                                            <ext:ToolbarFill />
                                            <ext:Button runat="server"
                                                ID="btnViewList"
                                                ToolTip="<%$ Resources:Comun, strVistaLista %>"
                                                EnableToggle="true"
                                                Cls="btnListView"
                                                Pressed="true"
                                                PressedCls="btnListView-Active"
                                                Focusable="false"
                                                Handler="OpenViewsList(this);" />
                                            <ext:Button runat="server"
                                                ID="btnViewGrid"
                                                ToolTip="<%$ Resources:Comun, strVistaCuadricula %>"
                                                EnableToggle="true"
                                                Cls="btnGridView"
                                                Pressed="false"
                                                PressedCls="btnGridView-Active"
                                                Focusable="false"
                                                Handler="OpenViewsCard(this);" />
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <BottomBar>
                                    <ext:PagingToolbar runat="server"
                                        ID="PagingToolBar"
                                        meta:resourceKey="PagingToolBar"
                                        StoreID="storeCompany"
                                        OverflowHandler="Scroller"
                                        Cls="bottomBarSites bottomline"
                                        DisplayInfo="true"
                                        HideRefresh="true">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                Cls="comboGrid"
                                                ID="cmbNumRegistros"
                                                Width="80">
                                                <Items>
                                                    <ext:ListItem Text="10" />
                                                    <ext:ListItem Text="20" />
                                                    <ext:ListItem Text="30" />
                                                    <ext:ListItem Text="40" />
                                                    <ext:ListItem Text="50" />
                                                </Items>
                                                <SelectedItems>
                                                    <ext:ListItem Value="20" />
                                                </SelectedItems>
                                                <Listeners>
                                                    <Select Fn="handlePageSizeSelect" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:PagingToolbar>
                                </BottomBar>
                                <Items>
                                    <ext:GridPanel
                                        runat="server"
                                        ID="gridCompany"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel gridGeneral"
                                        Header="false"
                                        EnableColumnHide="false"
                                        Region="Center"
                                        Hidden="false"
                                        AriaRole="main">
                                        <Listeners>
                                            <RowContextMenu Fn="ShowRightClickMenu" />
                                            <AfterRender Handler="GridColHandlerDinamicoV2(this);"></AfterRender>
                                            <Resize Handler="GridColHandlerDinamicoV2(this);"></Resize>
                                        </Listeners>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCompany"
                                                RemotePaging="true"
                                                AutoLoad="true"
                                                OnReadData="storeCompany_Refresh"
                                                RemoteSort="true"
                                                PageSize="20"
                                                RemoteFilter="true">
                                                <Listeners>
                                                    <BeforeLoad Fn="DeseleccionarGrilla" />
                                                    <DataChanged Fn="ShowCardsViews" />
                                                </Listeners>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Alias" />
                                                            <ext:ModelField Name="Phone" />
                                                            <ext:ModelField Name="Email" />
                                                            <ext:ModelField Name="Active" Type="Boolean" />
                                                            <ext:ModelField Name="Owner" />
                                                            <ext:ModelField Name="Supplier" />
                                                            <ext:ModelField Name="Customer" />
                                                            <ext:ModelField Name="Payee" />
                                                            <ext:ModelField Name="TaxIdentificationNumber" />
                                                            <ext:ModelField Name="CompanyTypeCode" />
                                                            <ext:ModelField Name="TaxpayerTypeCode" />
                                                            <ext:ModelField Name="TaxIdentificationNumberCategoryCode" />
                                                            <ext:ModelField Name="PaymentTermCode" />
                                                            <ext:ModelField Name="CreationDate" />
                                                            <ext:ModelField Name="LastModificationDate" />
                                                            <ext:ModelField Name="CreationUserEmail" />
                                                            <ext:ModelField Name="LastModificationUserEmail" />
                                                            <ext:ModelField Name="CurrencyCode" />
                                                            <ext:ModelField Name="LinkedBankAccount">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="BankCode" />
                                                                            <ext:ModelField Name="Code" />
                                                                            <ext:ModelField Name="IBAN" />
                                                                            <ext:ModelField Name="Description" />
                                                                            <ext:ModelField Name="SWIFT" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:ModelField>
                                                            <ext:ModelField Name="LinkedPaymentMethodCode">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="PaymentMethodCode" />
                                                                            <ext:ModelField Name="Default" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:ModelField>
                                                            <ext:ModelField Name="LinkedAddresses" Type="Object" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Name" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="GridFilters">
                                            </ext:GridFilters>
                                        </Plugins>

                                    </ext:GridPanel>
                                    <ext:Panel ID="pnViews" runat="server" Hidden="true" BodyCls="viewCardsCont">
                                        <Content>
                                            <div id="dtvTarjetas" class="containerStyle"></div>
                                        </Content>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                ID="pnDetails"
                                Region="East"
                                Collapsible="true"
                                Width="300"
                                CollapseDirection="Left"
                                CollapseMode="Header"
                                Layout="FitLayout"
                                Header="false"
                                Collapsed="true">
                                <Items>
                                    <ext:Panel runat="server" Layout="FitLayout" Cls="panelContract">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Label runat="server"
                                                        Text="<%$ Resources:Comun, strEntidad %>"
                                                        Cls="titleContract" />
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnControlPanel"
                                                        Handler="App.pnDetails.collapse();">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Top" Height="50">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lblTitleData"
                                                        Cls="dataModel" />
                                                    <ext:Image runat="server"
                                                        Alt="Logo"
                                                        Src="/ima/logo-treePlatform-only.svg"
                                                        MarginSpec="0 0 0 50" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Top" Height="50">
                                                <Items>
                                                    <ext:Label runat="server" Text="<%$ Resources:Comun, jsModeloDatos %>" Cls="dataModel" IconCls="btnDatamodelgreen" ID="lbPnDetail"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server"
                                                ID="pnDetailsCompany"
                                                Layout="AnchorLayout">
                                                <Items>
                                                    <ext:Panel runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="12%"
                                                        Cls="d-inline-block">
                                                        <Items>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnGeneralInfo"
                                                                Cls="btnGeneralInfo-asR"
                                                                PressedCls="none"
                                                                FocusCls="none"
                                                                ToolTip="<%$ Resources:Comun, strInfoGeneral %>"
                                                                Handler="MostrarDetalleCompany(App.storeCompany.getById(App.pnDetails.selectItem).data);">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnPagos"
                                                                Cls="btnPagos-asR"
                                                                PressedCls="none"
                                                                FocusCls="none"
                                                                ToolTip="<%$ Resources:Comun, strMetodosPagos %>"
                                                                Handler="MostrarDetalleCompanyMetodosPagos(App.storeCompany.getById(App.pnDetails.selectItem).data.LinkedPaymentMethodCode);">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnBancos"
                                                                Cls="btnBancos-asR"
                                                                PressedCls="none"
                                                                FocusCls="none"
                                                                ToolTip="<%$ Resources:Comun, strBankAccounts %>"
                                                                Handler="MostrarDetalleCompanyCuentasBancarias(App.storeCompany.getById(App.pnDetails.selectItem).data.LinkedBankAccount);">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnDireccionesCompany"
                                                                Cls="btnDirecciones-asR"
                                                                PressedCls="none"
                                                                FocusCls="none"
                                                                ToolTip="<%$ Resources:Comun, strDirecciones %>"
                                                                Handler="MostrarDetalleCompanyDirecciones(App.storeCompany.getById(App.pnDetails.selectItem).data.LinkedAddresses);">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Panel>
                                                    <ext:Panel
                                                        runat="server"
                                                        AnchorVertical="100%" AnchorHorizontal="88%"
                                                        Border="false"
                                                        OverflowY="Auto"
                                                        Header="false"
                                                        Cls="d-inline-block pnDetail">
                                                        <Content>
                                                            <div id="vsDtCompany">
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
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

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
