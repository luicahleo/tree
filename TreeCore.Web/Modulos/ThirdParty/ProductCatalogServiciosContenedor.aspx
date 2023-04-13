<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogServiciosContenedor.aspx.cs" Inherits="TreeCore.ModGlobal.ProductCatalogServiciosContenedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO  HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdServicioPadreID" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarServiciosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdIDServicioBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarServiciosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscadorPrecios" runat="server">
                <Listeners>
                    <Change Fn="filtrarPreciosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdIDPrecioBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarPreciosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscadorTraduccion" runat="server">
                <Listeners>
                    <Change Fn="filtrarTraduccionPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdIDTraduccionBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarTraduccionPorBuscador" />
                </Listeners>
            </ext:Hidden>

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv2"
                        OnDirectClick="ShowHidePnAsideR"
                        Handler="hidePnLite();"
                        Disabled="false"
                        Hidden="true">
                    </ext:Button>
                </Content>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <DockedItems>
                                    <ext:Container runat="server"
                                        ID="WrapAlturaCabecera"
                                        MinHeight="60"
                                        Layout="VBoxLayout"
                                        Dock="Top">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server"
                                                ID="tbTitulo"
                                                Dock="Top"
                                                Cls="tbGrey tbTitleAlignBot tbNoborder"
                                                Hidden="false"
                                                Flex="1">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lbltituloPrincipal"
                                                        Cls="TituloCabecera"
                                                        Text="<%$ Resources:Comun, strProductCatalog %>"
                                                        Height="25" />
                                                </Items>
                                            </ext:Toolbar>

                                            <%-- TABS NAVEGACION--%>

                                            <ext:Toolbar runat="server"
                                                ID="tbNavNAside"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder"
                                                Hidden="false"
                                                PaddingSpec="10 10 10 10"
                                                OverflowHandler="Scroller"
                                                Flex="1">
                                                <Items>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabCatalogos"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="<%$ Resources:Comun, strCatalogos %>"
                                                        tabID="0">
                                                        <Listeners>
                                                            <Click Handler="showForms(this);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabServicios"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine"
                                                        Text="<%$ Resources:Comun, strProductCatalogServicios %>"
                                                        tabID="1">
                                                        <Listeners>
                                                            <Click Handler="showForms(this);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabClausulas"
                                                        Hidden="true"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strClausulas %>"
                                                        tabID="2">
                                                        <%--<Listeners>
                                                            <Click Handler="showForms(this, '/Modulos/DataQuality/CalidadKPIresults.aspx', 'CalidadKPIresults', '/Modulos/DataQuality/js/CalidadKPIresults.js')"></Click>
                                                        </Listeners>--%>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabPacks"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strPaquetes %>"
                                                        tabID="3">
                                                         <Listeners>
                                                            <Click Handler="showForms(this);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabPrecios"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strPrecios %>"
                                                        tabID="4">
                                                        <Listeners>
                                                            <Click Handler="showForms(this);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabFormulas"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strFormulas %>"
                                                        tabID="5">
                                                        <Listeners>
                                                            <Click Handler="showForms(this);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                </Items>
                                                <%--<Listeners>
                                                    <AfterRender Fn="prepareTabs" />
                                                </Listeners>--%>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Content>
                                    <ext:Container
                                        ID="ctMain7"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldCatalogos"
                                            runat="server"
                                            Url="/Modulos/ThirdParty/ProductCatalog.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                    <ext:Container
                                        ID="ctMain2"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldServicios"
                                            runat="server"
                                            Url="/Modulos/ThirdParty/ProductCatalogServicios.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                    <ext:Container
                                        ID="ctMain3"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldClausulas"
                                            runat="server"
                                            Url="/"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                    <ext:Container
                                        ID="ctMain4"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldPacks"
                                            runat="server"
                                            Url="/Modulos/ThirdParty/ProductCatalogPacks.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                    <ext:Container
                                        ID="ctMain5"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldPrecios"
                                            runat="server"
                                            Url="/Modulos/ThirdParty/ProductCatalogPrecios.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                    <ext:Container
                                        ID="ctMain6"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="ldFormulas"
                                            runat="server"
                                            Url="/Modulos/ThirdParty/Formulas.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>

                                </Content>
                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>

                            <ext:Panel runat="server"
                                ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                AnimCollapse="false"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false"
                                Border="false"
                                Hidden="false">
                                <Listeners>
                                    <AfterLayout Handler=""></AfterLayout>
                                    <Resize Fn="resizeGridInfo" Buffer="50" />
                                    <AfterRender Fn="resizeGridInfo" />
                                </Listeners>
                                <Items>

                                    <%-- PANEL Filtros--%>

                                    <ext:Panel runat="server"
                                        ID="WrapService"
                                        Hidden="true"
                                        Cls="tbGrey"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%"
                                        Layout="AnchorLayout">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="tlbServices"
                                                Cls="tbGrey"
                                                Dock="Top"
                                                Padding="0">
                                                <Items>
                                                    <ext:Label Dock="Top"
                                                        MinHeight="60"
                                                        MinWidth="300"
                                                        PaddingSpec="20 0 0 20"
                                                        meta:resourcekey="lblAsideNameR"
                                                        ID="lblServices"
                                                        runat="server"
                                                        IconCls="ico-head-info"
                                                        Cls="lblHeadAsideDock"
                                                        Text="<%$ Resources:Comun, strDetallesProductCatalog %>">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server"
                                                ID="MenuNavPnServicios"
                                                Cls="TbAsideNavPanel"
                                                Padding="0"
                                                Dock="Left"
                                                Layout="VBoxLayout">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnEstados"
                                                        Cls="btnServie-asR btnNoBorder"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="<%$ Resources:Comun, strServicio %>"
                                                        Handler="displayMenu('pnServicios')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnPrecios"
                                                        Cls="btnPrice-asR btnNoBorder"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="<%$ Resources:Comun, strPrecios %>"
                                                        Handler="displayMenu('pnPrecios')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnClausulas"
                                                        Cls="btnCatalog-asR btnNoBorder"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="Catalogs"
                                                        Handler="displayMenu('pnClausulas')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnLink"
                                                        Cls="btnClip-asR btnNoBorder"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="Link"
                                                        Handler="displayMenu('pnLink')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <%-- PANEL MORE INFO--%>
                                            <ext:Panel runat="server"
                                                ID="pnServicios"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside pnServicios"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbServicios"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblServicios"
                                                                runat="server"
                                                                IconCls="ico-head-service"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strServicio %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
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
                                            
                                            <%-- PRECIOS --%>
                                            <ext:Panel runat="server"
                                                ID="pnPrecios"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbPrecios"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblPrecios"
                                                                runat="server"
                                                                IconCls="ico-head-price"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strPrecios %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Panel runat="server"
                                                        Cls="liNot pntraffic"
                                                        Width="302">
                                                        <Items>
                                                            <ext:GridPanel
                                                                runat="server"
                                                                Header="false"
                                                                ID="gridPrices"
                                                                Cls="gridPanel gridPanelNoBorder">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeCoreProductCatalogServiciosAsignados"
                                                                        RemotePaging="false"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeCoreProductCatalogServiciosAsignados_Refresh"
                                                                        RemoteSort="true"
                                                                        PageSize="20">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server"
                                                                                IDProperty="CoreProductCatalogServicioAsignadoID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="CoreProductCatalogServicioAsignadoID" Type="Int" />
                                                                                    <ext:ModelField Name="NombreProductCatalog" Type="String" />
                                                                                    <ext:ModelField Name="CantidadCatalogServicio" Type="Int" />
                                                                                    <ext:ModelField Name="Precio" Type="Int" />
                                                                                    <ext:ModelField Name="Simbolo" Type="String" />
                                                                                    <ext:ModelField Name="Identificador" Type="String" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="NombreProductCatalog" Direction="DESC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel>
                                                                    <Columns>
                                                                        <ext:Column runat="server"
                                                                            ID="colNombre"
                                                                            Flex="1"
                                                                            Text="<%$ Resources:Comun, strNombreProductCatalog %>"
                                                                            DataIndex="NombreProductCatalog"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            ID="colPrecio"
                                                                            Text="<%$ Resources:Comun, strPrecio %>"
                                                                            Flex="1"
                                                                            DataIndex="Precio"
                                                                            Align="Center">
                                                                            <Renderer Fn="PrecioRender" />
                                                                        </ext:Column>
                                                                    </Columns>
                                                                </ColumnModel>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>

                                            <%-- CLAUSULAS --%>
                                            <ext:Panel runat="server"
                                                ID="pnClausulas"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="Toolbar2"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="Label1"
                                                                runat="server"
                                                                IconCls="ico-head-catalog"
                                                                Cls="lblHeadAside "
                                                                Text="Catalogs"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <table class="tmpCol-table" id="tablaInfoCatalog">
                                                        <tbody id="bodyTablaInfoClausulas"></tbody>
                                                        </table>
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%-- CATALOG --%>
                                            <ext:Panel runat="server"
                                                ID="pnMoreInfoCatalog"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbCatalogs"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblClausulas"
                                                                runat="server"
                                                                IconCls="ico-head-catalog"
                                                                Cls="lblHeadAside "
                                                                Text="Catalogs"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div>
                                                        <table class="tmpCol-table" id="tablaInfoElementosCatalog">
                                                            <tbody id="bodyTablaInfoElementosCatalog">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%-- PACK --%>
                                            <ext:Panel runat="server"
                                                ID="pnMoreInfoPack"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbPacks"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblPacks"
                                                                runat="server"
                                                                IconCls="ico-head-catalog"
                                                                Cls="lblHeadAside "
                                                                Text="Packs"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div>
                                                        <table class="tmpCol-table" id="tablaInfoElementosPack">
                                                            <tbody id="bodyTablaInfoElementosPack">
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%-- SERVICIOS --%>
                                            <ext:Panel runat="server"
                                                ID="pnMoreInfoService"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="Label3"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside "
                                                                Text="MORE INFO SERVICE"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
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

                                            <%-- LINK --%>
                                            <ext:Panel runat="server"
                                                ID="pnLink"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbLink"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblLink"
                                                                runat="server"
                                                                IconCls="ico-link-info"
                                                                Cls="lblHeadAside "
                                                                Text="Link"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoLink" class="single-item">
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
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
