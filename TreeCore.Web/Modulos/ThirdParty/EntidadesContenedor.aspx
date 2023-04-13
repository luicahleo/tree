<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntidadesContenedor.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EntidadesContenedor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/EntidadesContenedor.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans btnCollapseAsRClosedv2" Hidden="false"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <Listeners>
                            <Click Fn="hidePnFilters" />
                            <AfterRender Handler="document.getElementById('btnCollapseAsR').style.opacity = 0;" />
                        </Listeners>
                    </ext:Button>
                </Content>
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls=""
                        Layout="BorderLayout">
                        <Items>

                            <%-- PANEL CENTRAL CON SLIDERS--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                PaddingSpec="20 0 20 20"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn bckGris">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tbNavNAside"
                                        Dock="Top"
                                        Cls="tbGrey tbNoborder"
                                        Hidden="false"
                                        Padding="10"
                                        OverflowHandler="Scroller"
                                        Flex="1">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkEntidades"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                meta:resourceKey="lnkEntidades"
                                                Text="ENTIDADES">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkEntidadesContactos"
                                                Cls="lnk-navView lnk-noLine"
                                                Hidden="false"
                                                meta:resourceKey="lnkEntidadesContactos"
                                                Text="CONTACTOS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Panel runat="server" ID="tagsContainer" Layout="ColumnLayout">
                                        <Items>
                                        </Items>
                                    </ext:Panel>
                                </DockedItems>
                                <Items>
                                    <ext:Panel ID="ctMain1" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="false">
                                        <Items>
                                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain"
                                                    runat="server"
                                                    Url="../ThirdParty/Entidades.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain2" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt2" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain2"
                                                    runat="server"
                                                    Url="../ThirdParty/EntidadesV2.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="false"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Header="false"
                                Border="false"
                                Width="380"
                                Hidden="false">
                                <Listeners>
                                    <%--<AfterRender Handler="ResizerAside(this)"></AfterRender>--%>
                                </Listeners>
                                <DockedItems>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameInfo"
                                        runat="server"
                                        IconCls="ico-head-info"
                                        Cls="lblHeadAsideDock"
                                        Hidden="false"
                                        Text="More Info">
                                    </ext:Label>
                                </DockedItems>
                                <Items>
                                    <%-- PANEL MORE INFO--%>

                                    <ext:Panel runat="server"
                                        ID="WrapCompany"
                                        Hidden="false"
                                        Cls="tbGrey"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%"
                                        Layout="AnchorLayout">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="MenuNavPnEstados"
                                                Cls="TbAsideNavPanel"
                                                Padding="0"
                                                Dock="Left"
                                                Layout="VBoxLayout">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnInfo"
                                                        ToolTip="More Info"
                                                        Cls="btnInfo-asR btnNoBorder"
                                                        Handler="displayMenu('pnMoreInfo')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnOwner"
                                                        ToolTip="Owner"
                                                        Cls="btnOwner-asR btnNoBorder"
                                                        Handler="displayMenu('pnOwner')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="Business Partner"
                                                        ID="btnBusinessPartner"
                                                        Cls="btnContratas-asR btnNoBorder"
                                                        Handler="displayMenu('pnBusinessPartner')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnSupplier"
                                                        ToolTip="Supplier"
                                                        Cls="btnCompany-asR btnNoBorder"
                                                        Handler="displayMenu('pnSupplier')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnOperator"
                                                        ToolTip="Operator"
                                                        Cls="btnRoles-asR btnNoBorder"
                                                        Handler="displayMenu('pnOperator')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <%-- PANEL MORE INFO--%>
                                            <ext:Panel runat="server"
                                                ID="pnMoreInfo"
                                                Hidden="false"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbMore"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblMore"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strEstados %>"
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

                                            <%--ESTADOS GLOBALES--%>
                                            <ext:Panel runat="server"
                                                ID="pnOwner"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbOwner"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblOwner"
                                                                runat="server"
                                                                IconCls="ico-head-owner"
                                                                Cls="lblHeadAside "
                                                                Text="Owner"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalOwner"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoOwner" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--SUBPROCESOS--%>
                                            <ext:Panel runat="server"
                                                ID="pnBusinessPartner"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBusinessPartner"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblBusinessPartner"
                                                                runat="server"
                                                                IconCls="ico-head-supplier"
                                                                Cls="lblHeadAside "
                                                                Text="Business Partner"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalBusinessPartner"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoBusinessPartner" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--ESTADOS SIGUIENTES--%>
                                            <ext:Panel runat="server"
                                                ID="pnSupplier"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbSupplier"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblSupplier"
                                                                runat="server"
                                                                IconCls="ico-head-company"
                                                                Cls="lblHeadAside "
                                                                Text="Supplier"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalSupplier"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoSupplier" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--LINKS--%>
                                            <ext:Panel runat="server"
                                                ID="pnOperator"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbOperator"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblOperator"
                                                                runat="server"
                                                                IconCls="ico-head-operator"
                                                                Cls="lblHeadAside "
                                                                Text="Operator"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalOperator"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoOperator" class="single-item">
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
