<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalog.aspx.cs" Inherits="TreeCore.PaginasComunes.ProductCatalog" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%@ Register Src="~/Componentes/ReajustePrecios.ascx" TagName="updateSetting" TagPrefix="local" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdMinDate" Value="<%# DateTime.Now %>" runat="server" />
            <ext:Hidden ID="hdProductCatalogID" runat="server" />
            <ext:Hidden ID="hdCodigoCatalogoAutogenerado" runat="server" />
            <ext:Hidden ID="hdCondicionCatalogoReglaID" runat="server" />
            <ext:Hidden ID="hdCodigoCatalogoDuplicado" runat="server" />
            <ext:Hidden ID="hdServicioAsignadoID" runat="server" />
            <ext:Hidden ID="hdServicioID" runat="server" />
            <ext:Hidden ID="hdPrecio" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDCatalogoBuscador" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageCatalogos"></Change>
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscador2" runat="server" />
            <ext:Hidden ID="hdIDCatalogoBuscador2" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid2" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageServicios"></Change>
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscador3" runat="server" />
            <ext:Hidden ID="hdIDCatalogoBuscador3" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid3" runat="server">
            </ext:Hidden>

            <ext:Hidden ID="hdPackAsignadoID" runat="server" />
            <ext:Hidden ID="hdPrecioPack" runat="server" />
            <ext:Hidden ID="hdStringBuscadorPack" runat="server" />
            <ext:Hidden ID="hdIDPackBuscador" runat="server" />
            <ext:Hidden ID="hdTotalCountGridPack" runat="server" />

            <ext:Hidden ID="hdStringBuscadorServicioPack" runat="server" />
            <ext:Hidden ID="hdIDServicioPackBuscador" runat="server" />
            <ext:Hidden ID="hdTotalCountGridServicioPack" runat="server" />
            <ext:Hidden ID="hdEsPack" runat="server" />

            <ext:Hidden ID="hdStore" runat="server">
                <Listeners>
                    <%--<Change Fn="CargarStore" />--%>
                </Listeners>
            </ext:Hidden>

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store
                runat="server"
                ID="storePrincipal"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                PageSize="20"
                RemoteSort="true"
                RemotePaging="true"
                shearchBox="txtSearch"
                listNotPredictive="CoreProductCatalogID, ClienteID, EntidadID,CoreProducCatalogTipoID">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="CatalogTypeCode" />
                            <ext:ModelField Name="CurrencyCode" />
                            <ext:ModelField Name="LifecycleStatusCode" />
                            <ext:ModelField Name="StartDate" Type="Date" />
                            <ext:ModelField Name="EndDate" Type="Date" />
                            <ext:ModelField Name="Type" />
                            <ext:ModelField Name="StartDate" Type="Date" />
                            <ext:ModelField Name="NextDate" Type="Date" />
                            <ext:ModelField Name="EndDate" Type="Date" />
                            <ext:ModelField Name="CodeInflation" />
                            <ext:ModelField Name="FixedAmount" />
                            <ext:ModelField Name="FixedPercentege" />
                            <ext:ModelField Name="Frequency" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeProductCatalogTipos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeProductCatalogTipos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Code" runat="server">
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
                AutoLoad="false"
                OnReadData="storeMonedas_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Code" runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Symbol" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Code" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeEstadosGlobales"
                runat="server"
                AutoLoad="false"
                OnReadData="storeEstadosGlobales_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Code" runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Code" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeProductCatalogServicios"
                runat="server"
                AutoLoad="false"
                OnReadData="storeProductCatalogServicios_Refresh"
                RemoteSort="true"
                GroupField="ProductTypeCode">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Code" runat="server">
                        <Fields>
                            <ext:ModelField Name="CodeProduct" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Price" />
                            <ext:ModelField Name="ProductTypeCode" />
                            <ext:ModelField Name="CodeCatalog" />
                            <ext:ModelField Name="Currency" />
                            <ext:ModelField Name="EsAsignado" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProductTypeCode" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeServiciosAsignados"
                runat="server"
                AutoLoad="false"
                OnReadData="storeServiciosAsignados_Refresh"
                RemoteSort="true"
                shearchBox="txtSearch2"
                listNotPredictive="">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CodeProduct" runat="server">
                        <Fields>
                            <ext:ModelField Name="CodeProduct" />
                            <ext:ModelField Name="Currency" />
                            <ext:ModelField Name="Price" Type="Float" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="CodeProduct" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window ID="winGestion"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="750"
                HeightSpec="80vh"
                MaxHeight="550"
                Modal="true"
                Resizable="false"
                Centered="true"
                Layout="FitLayout"
                Cls="winForm-resp noPadWin"
                Hidden="true">
                <Listeners>
                    <Close Handler="cerrarWinGestion()" />
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>

                <Items>
                    <ext:FormPanel ID="pnInicial"
                        Cls="formGris formResp"
                        runat="server"
                        BodyPaddingSummary="0 32"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:FormPanel ID="pnFormProductCatalog"
                                Cls="formGris formResp"
                                MarginSpec="10px 0 0 0"
                                runat="server">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctFormProduct"
                                        Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                        <Items>
                                            <ext:TextField runat="server"
                                                ID="txtCodigo"
                                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server"
                                                ID="txtNombre"
                                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server"
                                                ID="txtDescripcion"
                                                FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:ComboBox runat="server"
                                                meta:resourcekey="cmbMoneda"
                                                ID="cmbMonedas"
                                                Mode="Local"
                                                FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                DisplayField="Code"
                                                ValueField="Code"
                                                StoreID="storeMonedas"
                                                QueryMode="Local"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                Editable="true"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <TriggerClick Fn="RecargarMonedas" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        Icon="Clear"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:ComboBox runat="server"
                                                ID="cmbProductCatalogTipo"
                                                FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                StoreID="storeProductCatalogTipos"
                                                DisplayField="Code"
                                                ValueField="Code"
                                                AllowBlank="false"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                Model="Local"
                                                QueryMode="Local">
                                                <Listeners>
                                                    <TriggerClick Fn="RecargarProductCatalogTipo" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        IconCls="ico-reload"
                                                        QTip="Recargar Lista" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:ComboBox runat="server"
                                                ID="cmbEstadosGlobales"
                                                FieldLabel="<%$ Resources:Comun, strCatalogLifecycleStatus %>"
                                                StoreID="storeEstadosGlobales"
                                                DisplayField="Code"
                                                ValueField="Code"
                                                AllowBlank="false"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                Model="Local"
                                                QueryMode="Local">
                                                <Listeners>
                                                    <TriggerClick Fn="RecargarEstadosGlobales" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        IconCls="ico-reload"
                                                        QTip="Recargar Lista" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:DateField runat="server"
                                                meta:resourcekey="txtFechaInicio"
                                                ID="txtFechaInicio"
                                                FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                MinDate="<%# DateTime.Now %>"
                                                AutoDataBind="true"
                                                Vtype="daterange"
                                                Format="<%$ Resources:Comun, FormatFecha %>">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>
                                                <Listeners>
                                                    <Change Fn="FormularioValidoCatalogo" />
                                                </Listeners>
                                            </ext:DateField>
                                            <ext:DateField runat="server"
                                                meta:resourcekey="txtFechaFin"
                                                ID="txtFechaFin"
                                                FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                                LabelAlign="Top"
                                                AllowBlank="true"
                                                MinDate="<%# DateTime.Now %>"
                                                AutoDataBind="true"
                                                Vtype="daterange"
                                                Format="<%$ Resources:Comun, FormatFecha %>">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="startDateField" Value="txtFechaInicio" Mode="Value" />
                                                </CustomConfig>
                                                <Listeners>
                                                    <Change Fn="FormularioValidoCatalogo" />
                                                </Listeners>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:FormPanel ID="pnReajustes"
                                Cls="formGris formResp"
                                runat="server"
                                MarginSpec="20px 0 0 0"
                                HeightSpec="99%"
                                Hidden="true"
                                OverflowY="Auto">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctUpdateSetting"
                                        Cls="winGestion-panel">
                                        <Content>
                                            <local:updateSetting
                                                ID="cmpReajustes"
                                                runat="server" />
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:FormPanel ID="pnServicios"
                                Cls="formGris formResp"
                                runat="server"
                                OverflowY="Auto"
                                Hidden="true">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctServicios"
                                        Cls="winGestion-panel noPadding">
                                        <Items>
                                            <ext:GridPanel runat="server"
                                                ID="gridWinServicios"
                                                Header="false"
                                                HideHeaders="true"
                                                WidthSpec="100%"
                                                Height="300"
                                                SelectionMemory="false"
                                                StoreID="storeProductCatalogServicios"
                                                Cls="gridPanel grdPnColIcons grdWinProdCat"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar3"
                                                        Cls="noBorder padding-0-ct"
                                                        Dock="Top"
                                                        Height="60"
                                                        Layout="ColumnLayout">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearch3"
                                                                Cls="txtSearch"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                Width="250"
                                                                MaxWidth="280"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                    <KeyPress Fn="filtrarPorBuscadorFormulario" Buffer="500" />
                                                                    <TriggerClick Fn="LimpiarFiltroBusquedaFormulario" />
                                                                    <FocusEnter Fn="ajaxGetDatosBuscadorFormulario"  Buffer="500"/>
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            Flex="2"
                                                            DataIndex="CodeProduct" />
                                                        <ext:Column runat="server"
                                                            Flex="3"
                                                            MinWidth="40"
                                                            TdCls="grdPackRow"
                                                            DataIndex="Name">
                                                        </ext:Column>
                                                        <ext:ComponentColumn runat="server"
                                                            MinWidth="92"
                                                            MaxWidth="92"
                                                            Flex="1"
                                                            ID="cmpColumn"
                                                            TdCls="grdRowTxt"
                                                            DataIndex="Price"
                                                            Editor="true">
                                                            <Component>
                                                                <ext:NumberField runat="server"
                                                                    ID="txtPrecio"
                                                                    MinValue="0"
                                                                    MaxWidth="89"
                                                                    DecimalPrecision="3"
                                                                    AllowDecimals="true">
                                                                </ext:NumberField>
                                                            </Component>
                                                            <Listeners>
                                                                <Edit Handler="guardarServicio(e)" Buffer="800" />
                                                            </Listeners>
                                                        </ext:ComponentColumn>
                                                        <ext:Column runat="server"
                                                            Flex="1"
                                                            DataIndex="Currency">
                                                        </ext:Column>
                                                        <ext:ComponentColumn runat="server"
                                                            Flex="1"
                                                            MaxWidth="80" 
                                                            MinWidth="80"
                                                            ID="cmpCheck"
                                                            TdCls="grdServChk"
                                                            DataIndex="CodeProduct"
                                                            Editor="true">
                                                            <Component>
                                                                <ext:Button runat="server"
                                                                    ID="btnToggle"
                                                                    Width="41"
                                                                    EnableToggle="true"
                                                                    Cls="btn-toggleGrid"
                                                                    Pressed="true"
                                                                    AriaLabel=""
                                                                    ToolTip="">
                                                                </ext:Button>
                                                            </Component>
                                                            <Listeners>
                                                                <Bind Fn="marcarCHK" />
                                                            </Listeners>
                                                        </ext:ComponentColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Features>
                                                    <ext:Grouping
                                                        runat="server"
                                                        HideGroupedHeader="true"
                                                        EnableGroupingMenu="true"
                                                        GroupHeaderTplString="{name}: ({rows.length} Item{[values.rows.length > 1 ? 's' : '']})"
                                                        StartCollapsed="false" />
                                                </Features>
                                                <View>
                                                    <ext:GridView runat="server">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:GridView>
                                                </View>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="TbNavegacionTabs"
                                Dock="Top"
                                PaddingSpec="20 32 10"
                                Cls="tbGrey nav-vistas nav-vistasObligatorio"
                                Hidden="false">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkProduct"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strCatalogos %>">
                                        <Listeners>
                                            <Click Handler="NavegacionWinGestion(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkUpdate"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strReajustes %>">
                                        <Listeners>
                                            <Click Handler="NavegacionWinGestion(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkServicios"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strServicio %>">
                                        <Listeners>
                                            <Click Handler="NavegacionWinGestion(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="Toolbar10"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCerrar"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnAgregarProductCatalog"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <Click Handler="winGestionGuardar()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <%-- FIN  WINDOWS --%>

            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <DockedItems>
                                    <ext:Container runat="server"
                                        ID="WrapAlturaCabecera"
                                        Dock="Top"
                                        Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server"
                                                ID="tbNavNAside"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder"
                                                Hidden="true"
                                                PaddingSpec="10 10 10 10"
                                                OverflowHandler="Scroller"
                                                Flex="1">
                                                <Items>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkCatalogo"
                                                        meta:resourceKey="lnkCatalogo"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="<%$ Resources:Comun, strCatalogos %>">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                    <ext:Toolbar runat="server"
                                        ID="tbFiltrosYSliders"
                                        Dock="Top"
                                        Cls="tbGrey tbNoborder "
                                        Hidden="true"
                                        Layout="HBoxLayout"
                                        Flex="1">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Toolbar runat="server"
                                                ID="tbSliders"
                                                Dock="Top"
                                                Hidden="false"
                                                MinHeight="36"
                                                Cls="tbGrey tbNoborder">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnPrev"
                                                        IconCls="ico-prev-w"
                                                        Cls="btnMainSldr SliderBtn"
                                                        Handler="loadPanelByBtns('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="loadPanelByBtns('Next');"
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="wrapComponenteCentral"
                                        Layout="HBoxLayout"
                                        BodyCls="tbGrey"
                                        MaxWidth="1700">
                                        <Listeners>
                                            <AfterRender Handler="showPanelsByWindowSize();"></AfterRender>
                                            <Resize Handler="showPanelsByWindowSize();"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:GridPanel ID="gridCatalogos"
                                                runat="server"
                                                Header="true"
                                                Flex="4"
                                                EnableColumnHide="false"
                                                MarginSpec="0 10 5 0"
                                                SelectionMemory="false"
                                                StoreID="storePrincipal"
                                                Region="Center"
                                                Hidden="false"
                                                Title="<%$ Resources:Comun, strCatalogos %>"
                                                Cls="gridPanel grdPnColIcons "
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                    <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBase"
                                                        Dock="top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                AriaLabel="Añadir"
                                                                ToolTip="<%$ Resources:Comun, strAnadir %>">
                                                                <Listeners>
                                                                    <Click Fn="AgregarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                AriaLabel="Editar"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="MostrarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                Cls="btnEliminar"
                                                                AriaLabel="Eliminar"
                                                                ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>"
                                                                Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="Eliminar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                AriaLabel="Refrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="refrescar()" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Descargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('ProductCatalog', '', #{gridCatalogos},'', undefined , 2, App.hdStringBuscador.value, App.hdIDCatalogoBuscador.value);">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server"
                                                        ID="tbfiltros"
                                                        Cls=""
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:Toolbar
                                                                runat="server"
                                                                ID="Toolbar1"
                                                                Cls="tlbGrid"
                                                                Layout="ColumnLayout">
                                                                <Items>
                                                                    <ext:TextField
                                                                        ID="txtSearch"
                                                                        Cls="txtSearch"
                                                                        runat="server"
                                                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelWidth="50"
                                                                        Width="250"
                                                                        EnableKeyEvents="true">
                                                                        <Triggers>
                                                                            <ext:FieldTrigger Icon="Search" />
                                                                            <ext:FieldTrigger Icon="clear" />
                                                                        </Triggers>
                                                                        <Listeners>
                                                                            <Render Fn="FieldSearch" Buffer="250" />
                                                                            <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                            <KeyPress Fn="filtrarCatalogosPorBuscador" Buffer="500" />
                                                                            <TriggerClick Fn="LimpiarFiltroBusqueda" />
                                                                            <FocusEnter Fn="ajaxGetDatosBuscadorCatalogos"  Buffer="500"/>
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Items>
                                                    </ext:Container>
                                                </DockedItems>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            Flex="8"
                                                            MinWidth="120"
                                                            DataIndex="Code" />
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            Flex="8"
                                                            MinWidth="120"
                                                            DataIndex="Name" />
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strDescripcion %>"
                                                            Flex="8"
                                                            MinWidth="120"
                                                            DataIndex="Description" />
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strTipo %>"
                                                            Flex="8"
                                                            MinWidth="120"
                                                            DataIndex="CatalogTypeCode" />
                                                        <ext:WidgetColumn ID="ColMoreCatalog"
                                                            runat="server"
                                                            Cls="NoOcultar col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="More"
                                                            Hidden="false"
                                                            MinWidth="90"
                                                            MaxWidth="90">
                                                            <Widget>
                                                                <ext:Button runat="server"
                                                                    Width="90"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMoreX">
                                                                    <Listeners>
                                                                        <Click Handler="parent.hideAsideR('WrapCatalog', App.storePrincipal, this)" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
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
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:Toolbar ID="pagin"
                                                        runat="server"
                                                        OverflowHandler="Scroller"
                                                        Cls="bottomBarSites">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnPagingInit"
                                                                Disabled="true"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-first">
                                                                <Listeners>
                                                                    <Click Fn="pagingInitCatalogos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPagingPre"
                                                                Disabled="true"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-prev">
                                                                <Listeners>
                                                                    <Click Fn="pagingPreCatalogos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator />
                                                            <ext:Label
                                                                runat="server"
                                                                Text="<%$ Resources:Comun, jsPagina %>" />
                                                            <ext:NumberField
                                                                runat="server"
                                                                ID="nfPaginNumber"
                                                                MinValue="0"
                                                                Width="50"
                                                                HideTrigger="true">
                                                                <Listeners>
                                                                    <BeforeRender Fn="nfPaginNumberBeforeRenderCatalogos" />
                                                                    <Change Fn="paginGoToCatalogos" />
                                                                </Listeners>
                                                            </ext:NumberField>
                                                            <ext:Label
                                                                runat="server"
                                                                Text="of" />
                                                            <ext:Label
                                                                runat="server"
                                                                ID="lbNumberPages"
                                                                Text="0" />
                                                            <ext:ToolbarSeparator />
                                                            <ext:Button runat="server"
                                                                ID="btnPaginNext"
                                                                Enabled="false"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-next">
                                                                <Listeners>
                                                                    <Click Fn="paginNextCatalogos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPaginLast"
                                                                Enabled="false"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-last">
                                                                <Listeners>
                                                                    <Click Fn="paginLastCatalogos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator />
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                MinWidth="80"
                                                                MaxWidth="80"
                                                                ID="cmbNumRegistros"
                                                                Flex="2">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20" />
                                                                </SelectedItems>
                                                                <Listeners>
                                                                    <Select Fn="pageSelect" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ToolbarFill />
                                                            <ext:Label runat="server"
                                                                ID="lbDisplaying"
                                                                Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </BottomBar>
                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                ID="pnServiciosCatalogo"
                                                Flex="3"
                                                Layout="VBoxLayout"
                                                BodyCls="tbGrey">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:GridPanel ID="gridServiciosCatalogos"
                                                        Title="<%$ Resources:Comun, strServicio %>"
                                                        runat="server"
                                                        Header="true"
                                                        Flex="1"
                                                        AutoLoad="false"
                                                        MarginSpec="0 10 5 10"
                                                        Region="Center"
                                                        SelectionMemory="false"
                                                        StoreID="storeServiciosAsignados"
                                                        EnableColumnHide="false"
                                                        Hidden="false"
                                                        Cls="gridPanel grdPnColIcons "
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <Listeners>
                                                            <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                        </Listeners>
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="Toolbar5"
                                                                Dock="Top"
                                                                Cls="tlbGrid"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarServicio"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        meta:resourceKey="btnRefrescar"
                                                                        Disabled="true"
                                                                        Cls="btnRefrescar"
                                                                        Handler="RefrescarServiciosAsignados();" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnDescargarServicios"
                                                                        meta:resourceKey="btnDescargarDetalle"
                                                                        Disabled="true"
                                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                        Cls="btnDescargar"
                                                                        Handler="ExportarDatos('ProductCatalog', hdCliID.value, #{gridServiciosCatalogos},hdProductCatalogID.value , '', 1, App.hdStringBuscador2.value, App.hdIDCatalogoBuscador2.value, App.hdEsPack.value);" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Container runat="server"
                                                                ID="tbfiltros2"
                                                                Cls=""
                                                                Dock="Top">
                                                                <Items>
                                                                    <ext:Toolbar
                                                                        runat="server"
                                                                        ID="Toolbar2"
                                                                        Cls="tlbGrid"
                                                                        Layout="ColumnLayout">
                                                                        <Items>
                                                                            <ext:TextField
                                                                                ID="txtSearch2"
                                                                                Cls="txtSearch"
                                                                                runat="server"
                                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                                LabelWidth="50"
                                                                                Width="250"
                                                                                EnableKeyEvents="true">
                                                                                <Triggers>
                                                                                    <ext:FieldTrigger Icon="Search" />
                                                                                    <ext:FieldTrigger Icon="clear" />
                                                                                </Triggers>
                                                                                <Listeners>
                                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                                    <KeyPress Fn="filtrarPorBuscadorServicios" Buffer="500" />
                                                                                    <TriggerClick Fn="BorrarFiltrosServicios" />
                                                                                    <FocusEnter Fn="ajaxGetDatosBuscadorServicios"  Buffer="500"/>
                                                                                </Listeners>
                                                                            </ext:TextField>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </Items>
                                                            </ext:Container>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                                    MinWidth="110"
                                                                    DataIndex="CodeProduct"
                                                                    Flex="2"
                                                                    ID="colCodigo"
                                                                    Cls="columGridCatalog">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    Text="<%$ Resources:Comun, strPrecio %>"
                                                                    MinWidth="110"
                                                                    DataIndex="Price"
                                                                    Flex="2"
                                                                    ID="colPrecio"
                                                                    Cls="columGridCatalog">
                                                                    <Renderer Fn="PrecioRender" />
                                                                </ext:Column>
                                                                <ext:WidgetColumn ID="ColMoreServices"
                                                                    runat="server"
                                                                    Cls="NoOcultar col-More"
                                                                    DataIndex=""
                                                                    Align="Center"
                                                                    Text="More"
                                                                    Flex="1"
                                                                    Hidden="false"
                                                                    MinWidth="90"
                                                                    MaxWidth="90">
                                                                    <Widget>
                                                                        <ext:Button runat="server"
                                                                            Width="90"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="btnColMoreX">
                                                                            <Listeners>
                                                                                <Click Handler="parent.hideAsideR('WrapServiciosCatalog', App.storeServiciosAsignados, this)" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <Plugins>
                                                            <ext:GridFilters runat="server"
                                                                ID="gridFiltersServicios"
                                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                                meta:resourceKey="GridFilters">
                                                            </ext:GridFilters>
                                                            <ext:CellEditing runat="server"
                                                                ClicksToEdit="2" />
                                                        </Plugins>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectServicios"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="grid_rowSelectServicios" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:Toolbar ID="pagin2"
                                                                runat="server"
                                                                OverflowHandler="Scroller"
                                                                Cls="bottomBarSites">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPagingInit2"
                                                                        Disabled="true"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-first">
                                                                        <Listeners>
                                                                            <Click Fn="pagingInitServicios" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPagingPre2"
                                                                        Disabled="true"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-prev">
                                                                        <Listeners>
                                                                            <Click Fn="pagingPreServicios" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ToolbarSeparator />
                                                                    <ext:Label
                                                                        runat="server"
                                                                        Text="<%$ Resources:Comun, jsPagina %>" />
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="nfPaginNumber2"
                                                                        MinValue="0"
                                                                        Width="50"
                                                                        HideTrigger="true">
                                                                        <Listeners>
                                                                            <BeforeRender Fn="nfPaginNumberBeforeRenderServicios" />
                                                                            <Change Fn="paginGoToServicios" />
                                                                        </Listeners>
                                                                    </ext:NumberField>
                                                                    <ext:Label
                                                                        runat="server"
                                                                        Text="of" />
                                                                    <ext:Label
                                                                        runat="server"
                                                                        ID="lbNumberPages2"
                                                                        Text="0" />
                                                                    <ext:ToolbarSeparator />
                                                                    <ext:Button runat="server"
                                                                        ID="btnPaginNext2"
                                                                        Enabled="false"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-next">
                                                                        <Listeners>
                                                                            <Click Fn="paginNextServicios" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPaginLast2"
                                                                        Enabled="false"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-last">
                                                                        <Listeners>
                                                                            <Click Fn="paginLastServicios" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ToolbarSeparator />
                                                                    <ext:ComboBox runat="server"
                                                                        Cls="comboGrid"
                                                                        MinWidth="80"
                                                                        MaxWidth="80"
                                                                        ID="cmbNumRegistros2"
                                                                        Flex="2">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                        <Listeners>
                                                                            <Select Fn="pageSelect2" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:ToolbarFill />
                                                                    <ext:Label runat="server"
                                                                        ID="lbDisplaying2"
                                                                        Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </BottomBar>
                                                    </ext:GridPanel>
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
