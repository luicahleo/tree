<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogPacks.aspx.cs" Inherits="TreeCore.PaginasComunes.ProductCatalogPacks" %>

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
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdProductCatalogID" runat="server" />
            <ext:Hidden ID="hdCodigoCatalogoAutogenerado" runat="server" />
            <ext:Hidden ID="hdCondicionCatalogoReglaID" runat="server" />
            <ext:Hidden ID="hdCodigoCatalogoDuplicado" runat="server" />
            <ext:Hidden ID="hdServicioAsignadoID" runat="server" />
            <ext:Hidden ID="hdServicioID" runat="server" />
            <ext:Hidden ID="hdPrecio" runat="server" />
            <ext:Hidden runat="server" ID="hdEditando" />

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

            <%--<ext:Hidden ID="hdRadio" runat="server" />
            <ext:Hidden ID="hdProximaFecha" runat="server" />
            <ext:Hidden ID="hdUltimaFecha" runat="server" />
            <ext:Hidden ID="hdInflacion" runat="server" />
            <ext:Hidden ID="hdCadencia" runat="server" />
            <ext:Hidden ID="hdValor" runat="server" />--%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <ext:Store
                runat="server"
                ID="storePrincipal"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                PageSize="20"
                RemoteSort="true"
                RemoteFilter="true"
                RemotePaging="true"
                shearchBox="txtSearch"
                listNotPredictive="CoreProductCatalogPackID">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="CompanyCode" />
                            <ext:ModelField Name="Active" Type="Boolean" />
                            <ext:ModelField Name="IsPack" Type="Boolean" />
                            <ext:ModelField Name="Default" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <%--<DataChanged Fn="BuscadorPredictivo" />--%>
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

            <ext:Store runat="server"
                ID="storeEntidades"
                AutoLoad="false"
                OnReadData="storeEntidades_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Active" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Code" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <%--<ext:Store ID="storeMonedas" runat="server" AutoLoad="false" OnReadData="storeMonedas_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="MonedaID" runat="server">
                        <Fields>
                            <ext:ModelField Name="MonedaID" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="Simbolo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Moneda" Direction="ASC" />
                </Sorters>
            </ext:Store>--%>

            <ext:Store ID="storeProductCatalogServicios"
                runat="server"
                AutoLoad="false"
                OnReadData="storeProductCatalogServicios_Refresh"
                RemoteSort="true"
                GroupField="NombreCatalogServicioTipo">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="CompanyCode" />
                            <ext:ModelField Name="Active" Type="Boolean" />
                            <ext:ModelField Name="IsPack" Type="Boolean" />
                            <ext:ModelField Name="Default" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeServiciosAsignados" runat="server" AutoLoad="false" OnReadData="storeServiciosAsignados_Refresh"
                RemoteSort="true" shearchBox="txtSearch2"
                listNotPredictive="">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="CompanyCode" />
                            <ext:ModelField Name="Active" Type="Boolean" />
                            <ext:ModelField Name="IsPack" Type="Boolean" />
                            <ext:ModelField Name="Default" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Window ID="winGestion"
                runat="server"
                Title="<%$ Resources:Comun, jsAgregar %>"
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
                    <ext:FormPanel ID="pnInicial" Cls="formGris formResp"
                        runat="server"
                        BodyPaddingSummary="0 32"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:FormPanel ID="pnFormProductCatalogPacks"
                                Cls="formGris formResp"
                                HeightSpec="100%"
                                MarginSpec="10px 0 0 0"
                                runat="server"
                                OverflowY="Auto">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctFormProduct"
                                        HeightSpec="100%"
                                        Cls="winGestion-panel ctForm-resp ctForm-resp-col4">
                                        <Items>

                                            <ext:TextField runat="server"
                                                ID="txtNombre"
                                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                Cls="dosColumnasInicio"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <Change Fn="anadirClsNoValido" />
                                                    <FocusLeave Fn="anadirClsNoValido" />
                                                </Listeners>
                                            </ext:TextField>

                                            <ext:TextField runat="server"
                                                ID="txtCodigo"
                                                Cls="dosColumnasFinal"
                                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <Change Fn="anadirClsNoValido" />
                                                    <FocusLeave Fn="anadirClsNoValido" />
                                                </Listeners>
                                            </ext:TextField>

                                            <ext:TextField runat="server"
                                                ID="txtDescripcion"
                                                Cls="dosColumnasInicio"
                                                FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <Change Fn="anadirClsNoValido" />
                                                    <FocusLeave Fn="anadirClsNoValido" />
                                                </Listeners>
                                            </ext:TextField>

                                            <ext:ComboBox runat="server"
                                                meta:resourcekey="cmbEntidad"
                                                ID="cmbEntidad"
                                                Cls="dosColumnasFinal"
                                                Mode="Local"
                                                FieldLabel="<%$ Resources:Comun, strEntidades %>"
                                                DisplayField="Code"
                                                ValueField="Code"
                                                StoreID="storeEntidades"
                                                QueryMode="Local"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                Editable="true"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <TriggerClick Fn="RecargarEmpresaProveedoraAsociada" />
                                                    <Change Fn="anadirClsNoValido" />
                                                    <FocusLeave Fn="anadirClsNoValido" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        Icon="Clear"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>



                                            <ext:TextField runat="server"
                                                ID="txtunidad"
                                                Cls="dosColumnasInicio"
                                                FieldLabel="<%$ Resources:Comun, strUnidad %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true">
                                                <Listeners>
                                                    <Change Fn="anadirClsNoValido" />
                                                    <FocusLeave Fn="anadirClsNoValido" />
                                                </Listeners>
                                            </ext:TextField>

                                            <ext:ComboBox runat="server"
                                                ID="cmbProductCatalogTipo"
                                                FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                StoreID="storeProductCatalogTipos"
                                                DisplayField="Code"
                                                ValueField="Code"
                                                AllowBlank="false"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                Mode="Local"
                                                Cls="dosColumnasFinal"
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

                                            <%--<ext:ComboBox runat="server"
                                                ID="cmbProductCatalogTipo"
                                                Cls="dosColumnasFinal"
                                                FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                DisplayField="Nombre"
                                                ValueField="CoreProductCatalogTipoID"
                                                AllowBlank="false"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                Model="Local"
                                                QueryMode="Local">
                                                <Listeners>--%>
                                            <%--<TriggerClick Fn="RecargarProductCatalogTipo" />--%>
                                            <%--</Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        IconCls="ico-reload"
                                                        QTip="Recargar Lista" />
                                                </Triggers>
                                            </ext:ComboBox>--%>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:FormPanel ID="pnServicios"
                                Cls="formGris formResp"
                                runat="server"
                                OverflowY="Auto"
                                Hidden="true">
                                <Items>
                                    <ext:Container runat="server" ID="ctServicios" Cls="winGestion-panel" PaddingSpec="12 0 0 0">
                                        <Items>
                                            <%--<ext:Label runat="server" Cls="TituloCabecera" Text="<%$ Resources:Comun, strServicio %>" Height="25" MarginSpec="10 0 10 0" />--%>

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
                                                                    <FocusEnter Fn="ajaxGetDatosBuscadorFormulario" Buffer="500" />
                                                                </Listeners>
                                                            </ext:TextField>

                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>

                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            Flex="2"
                                                            DataIndex="NombreCatalogServicioTipo" />
                                                        <ext:Column runat="server"
                                                            Flex="3"
                                                            MinWidth="40"
                                                            TdCls="grdPackRow"
                                                            DataIndex="NombreCatalogServicio">
                                                        </ext:Column>

                                                        <ext:ComponentColumn runat="server" Flex="1" ID="cmpCheck" TdCls="grdPackChk" MaxWidth="120" DataIndex="CoreProductCatalogServicioID" Editor="true">
                                                            <Component>
                                                                <ext:Button runat="server"
                                                                    ID="btnToggle"
                                                                    Width="41"
                                                                    EnableToggle="true"
                                                                    Cls="btn-toggleGrid"
                                                                    Pressed="true"
                                                                    AriaLabel=""
                                                                    ToolTip="">
                                                                    <Listeners>
                                                                        <Click Handler="guardarServicio(this)" />
                                                                        <Render Handler="marcarCHK(this)" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Component>
                                                        </ext:ComponentColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Features>
                                                    <ext:Grouping
                                                        runat="server"
                                                        HideGroupedHeader="true"
                                                        EnableGroupingMenu="false"
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
                                <Listeners>
                                    <%--<ValidityChange Fn="FormularioValido" />--%>
                                </Listeners>

                            </ext:FormPanel>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="TbNavegacionTabs" Dock="Top" PaddingSpec="20 32 10"
                                Cls="tbGrey nav-vistas nav-vistasObligatorio" Hidden="false">
                                <Items>

                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkProduct"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strPaquetes %>">
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
                            <ext:Toolbar runat="server" ID="Toolbar10" Cls="greytb" Dock="Bottom" Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <%--<ext:Button runat="server" ID="btnPrev" Cls="btn-secondary " MinWidth="110" Text="<%$ Resources:Comun, strAnterior %>" Focusable="false" PressedCls="none" Hidden="false">
                                        <Listeners>
                                            <Click Fn="btnPrev_Click" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnNext" Cls="btn-ppal" Text="<%$ Resources:Comun, strSiguiente %>" Focusable="false" PressedCls="none" Hidden="false">
                                        <Listeners>
                                            <Click Fn="btnNext_Click" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnAgregarProductCatalog" Cls="btn-ppal" Text="<%$ Resources:Comun, strCerrar %>" Focusable="false" PressedCls="none" Hidden="true">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>--%>
                                    <ext:Button runat="server" ID="btnCerrar" Width="100px"
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
                                    <ext:Button runat="server" ID="btnAgregarProductCatalogPacks" Cls="btn-ppal-winForm" Text="<%$ Resources:Comun, strGuardar %>" PressedCls="none">
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

            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" Flex="1" OverflowY="auto">
                <%--<Content>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans btnCollapseAsRClosedv2" Hidden="true"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <Listeners>
                            <Click Handler="hideAsideR();" />--%>
                <%--<AfterRender Handler="document.getElementById('btnCollapseAsR').style.transform = 'rotate(-180deg)';" />--%>
                <%--</Listeners>
                    </ext:Button>
                </Content>--%>
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
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <%--<ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strCatalogos %>" Height="25" MarginSpec="10 0 10 0" />
                                                </Items>
                                            </ext:Toolbar>--%>
                                        </Items>
                                    </ext:Container>
                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="true" Layout="HBoxLayout" Flex="1">
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
                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1700">
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
                                                Flex="12"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                StoreID="storePrincipal"
                                                Region="West"
                                                ForceFit="true"
                                                Hidden="false"
                                                MaxWidth="300"
                                                Title="<%$ Resources:Comun, strPaquetes %>"
                                                Cls="gridPanel grdPnColIcons grdLabelPacks"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                    <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnAnadir" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="<%$ Resources:Comun, strAnadir %>">
                                                                <Listeners>
                                                                    <Click Fn="AgregarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEditar" Cls="btnEditar" AriaLabel="Editar" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="MostrarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEliminar" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="Eliminar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescar" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="refrescar()" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('ProductCatalogPacks', hdCliID.value, #{gridCatalogos},'', undefined , 2, App.hdStringBuscador.value, App.hdIDCatalogoBuscador.value);">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
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
                                                                            <FocusEnter Fn="ajaxGetDatosBuscadorCatalogos" Buffer="500" />
                                                                        </Listeners>
                                                                    </ext:TextField>

                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Items>
                                                    </ext:Container>
                                                </DockedItems>

                                                <ColumnModel runat="server">
                                                    <Columns>

                                                        <ext:TemplateColumn runat="server" Text="<%$ Resources:Comun, strPaquetes %>" DataIndex="Codigo" MenuDisabled="true" Flex="3">

                                                            <Template runat="server">
                                                                <Html>

                                                                    <tpl for=".">
                                                                        <div class="d-flx">
                                                                            <ul class="ulGrid noMargin">
                                                                                <li class="detailsLi detailLiPack">
                                                                                    <span>{Name}</span>
                                                                                </li>
                                                                                <li class="detailsLi detailLiDesc">
                                                                                    <span>{Code}</span>
                                                                                </li>
                                                                            </ul>
                                                                        </div>

                                                                    </tpl>

                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>

                                                        <%--<ext:Column runat="server" Text="<%$ Resources:Comun, strCodigo %>" Flex="8" MinWidth="120" DataIndex="Codigo" />--%>
                                                        <ext:WidgetColumn ID="ColMoreCatalog" runat="server" Cls="NoOcultar col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="90" MaxWidth="90">
                                                            <Widget>
                                                                <ext:Button runat="server" Width="90" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMoreX">
                                                                    <Listeners>
                                                                        <Click Handler="parent.hideAsideR('WrapPack', App.storePrincipal, this)" />
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
                                                ID="pnCol1"
                                                Cls="grdNoHeader"
                                                Region="Center"
                                                Flex="3"
                                                Layout="VBoxLayout"
                                                MinWidth="200"
                                                OverflowX="Auto"
                                                OverflowY="Auto">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar4"
                                                        Dock="Top"
                                                        Height="42"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                Height="42"
                                                                Handler="showOnlySecundary();">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                Cls="HeaderLblVisor"
                                                                Text="<%$ Resources:Comun, strServicio %>" />
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server" ID="Toolbar5" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
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
                                                                Handler="ExportarDatos('ProductCatalogPacks', hdCliID.value, #{gridServiciosCatalogos},hdProductCatalogID.value , '', 1, App.hdStringBuscador2.value, App.hdIDCatalogoBuscador2.value);" />



                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server" ID="tbfiltros2" Cls="" Dock="Top">
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
                                                                            <FocusEnter Fn="ajaxGetDatosBuscadorServicios" Buffer="500" />
                                                                        </Listeners>
                                                                    </ext:TextField>

                                                                </Items>
                                                            </ext:Toolbar>
                                                        </Items>
                                                    </ext:Container>
                                                </DockedItems>
                                                <Items>
                                                    <ext:GridPanel ID="gridServiciosCatalogos"
                                                        runat="server"
                                                        Header="false"
                                                        Flex="1"
                                                        Cls="gridPanel"
                                                        AutoLoad="false"
                                                        Region="Center"
                                                        SelectionMemory="false"
                                                        StoreID="storeServiciosAsignados"
                                                        EnableColumnHide="false"
                                                        Hidden="false"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <Listeners>
                                                            <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                        </Listeners>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strCodigo %>" MinWidth="110" DataIndex="Code" Flex="2" ID="colCodigo" Cls="columGridCatalog linkNumber" TdCls="linkNumber">
                                                                </ext:Column>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" MinWidth="110" DataIndex="Name" Flex="2" ID="colNombre" Cls="columGridCatalog">
                                                                </ext:Column>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strDescripcion %>" MinWidth="110" DataIndex="Description" Flex="2" ID="colIdentificador" Cls="columGridCatalog">
                                                                </ext:Column>
                                                                <ext:DateColumn runat="server" Text="<%$ Resources:Comun, strFechaModificacion %>" MinWidth="110" DataIndex="FechaModificacion" Flex="2" ID="colFechaModificacion" Format="<%$ Resources:Comun, FormatFecha %>">
                                                                </ext:DateColumn>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strTipo %>" MinWidth="110" DataIndex="CompanyCode" Flex="2" ID="colNombreServicioTipo" Cls="columGridCatalog">
                                                                </ext:Column>
                                                                <ext:WidgetColumn ID="ColMoreServices" runat="server" Cls="NoOcultar col-More" DataIndex="" Align="Center" Text="More" Flex="1" Hidden="false" MinWidth="90" MaxWidth="90">
                                                                    <Widget>
                                                                        <ext:Button runat="server" Width="90" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMoreX">
                                                                            <Listeners>
                                                                                <Click Handler="parent.hideAsideR('WrapServicePacks', App.gridServiciosCatalogos, this)" />
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

                                            <ext:Panel runat="server" ID="pnCol2" Flex="3" Layout="VBoxLayout" BodyCls="tbGrey" Hidden="true">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>

                                                    <ext:GridPanel ID="gridEmpresaProveedora"
                                                        Title="<%$ Resources:Comun, strClausulas %>"
                                                        runat="server"
                                                        Flex="2"
                                                        MarginSpec="0 10 5 10"
                                                        HideHeaders="true"
                                                        Region="Center"
                                                        Hidden="true"
                                                        SelectionMemory="false"
                                                        Cls="gridPanel"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server" ID="tlbBaseZone" Dock="Top" Cls="tlbGrid">
                                                                <Items>
                                                                    <ext:Button runat="server" ID="btnAnadirClausulas" Cls="btnAnadir" AriaLabel="Añadir" Disabled="true" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" Handler="AgregarEditarEmpresaProveedora()"></ext:Button>
                                                                    <ext:Button runat="server" ID="btnEliminaClausulas" Cls="btnEliminar" AriaLabel="Eliminar" Disabled="true" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>" Handler="EliminarEmpresaProveedora()"></ext:Button>
                                                                    <ext:Button runat="server" ID="btnRefrescarClausulas" Cls="btnRefrescar" AriaLabel="Refrescar" Disabled="true" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Handler="RefrescarEmpresasProveedoras()"></ext:Button>

                                                                </Items>
                                                            </ext:Toolbar>

                                                        </DockedItems>

                                                        <ColumnModel>
                                                            <Columns>

                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" Selectable="false" DataIndex="Nombre" Flex="8" ID="colNombreEmpresaProveedora">
                                                                </ext:Column>
                                                                <ext:Column
                                                                    runat="server"
                                                                    ID="colActivoEmpresaProveedora"
                                                                    DataIndex="Activo"
                                                                    Cls="col-activo"
                                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                                    meta:resourceKey="colActivo"
                                                                    MinWidth="50">
                                                                    <%--<Renderer Fn="DefectoRender" />--%>
                                                                </ext:Column>

                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server" ID="RowSelectionModelEmpresaProveedora" PruneRemoved="false" Mode="Single">
                                                                <Listeners>
                                                                    <%--<Select Fn="Grid_RowSelectEmpresaProveedora" />--%>
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>

                                                    </ext:GridPanel>

                                                </Items>
                                            </ext:Panel>

                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <%--<ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                AnimCollapse="false"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Listeners>
                                    <AfterLayout Handler=""></AfterLayout>
                                    <Resize Handler=""></Resize>
                                </Listeners>
                                <Items>
                                    <ext:Panel runat="server" ID="pnMoreInfoCatalog" Hidden="false" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar7" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO CATALOGS" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="pnMoreInfoService" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar8" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label2" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO SERVICE" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementosService">
                                                    <tbody id="bodyTablaInfoElementosService">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>--%>
                        </Items>
                    </ext:Panel>

                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
