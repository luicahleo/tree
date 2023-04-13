<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkFlowsContenedor.aspx.cs" Inherits="TreeCore.ModWorkFlow.WorkFlowsContenedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link rel="stylesheet" type="text/css" href="/Scripts/slick.css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/slick-theme.css" />
    <script type="text/javascript" src="/Scripts/slick.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO  HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdEstadoPadreID" runat="server" />
            <ext:Hidden ID="hdWorkflowPadreID" runat="server" />

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
                                                ID="lnkBusinessProcess"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                meta:resourceKey="lnkBusinessProcess"
                                                Text="BUSINESS PROCESS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkFlow"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkWorkFlow"
                                                Text="WORKFLOW">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkStatus"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkStatus"
                                                Text="STATUS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkCustomField"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkCustomField"
                                                Text="CUSTOM FIELDS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkDiagram"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkDiagram"
                                                Text="DIAGRAM">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel ID="ctMain1" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="false">
                                        <Items>
                                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain"
                                                    runat="server"
                                                    Url="../WorkOrder/BusinessProcess.aspx"
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
                                                    Url="../WorkOrder/WorkFlows.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain3" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt3" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain3"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkFlowsEstados.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain4" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt4" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain4"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkFlowsCustomFields.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain5" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt5" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain5"
                                                    runat="server"
                                                    Url="/"
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
                                Collapsed="true"
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
                                        Text="More Info">
                                    </ext:Label>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblEstados"
                                        runat="server"
                                        Hidden="true"
                                        IconCls="ico-estado-info"
                                        Cls="lblHeadAsideDock"
                                        Text="<%$ Resources:Comun, strEstadosDetalles %>">
                                    </ext:Label>
                                </DockedItems>
                                <Items>

                                    <%-- PANEL Filtros--%>

                                    <%-- PANEL ESTADOS--%>

                                    <ext:Panel runat="server"
                                        ID="WrapEstados"
                                        Hidden="true"
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
                                                        ID="btnEstados"
                                                        ToolTip="<%$ Resources:Comun, strEstados %>"
                                                        Cls="btnEstado-asR btnNoBorder"
                                                        Handler="displayMenu('pnMoreInfo')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnEstadosGlobales"
                                                        ToolTip="<%$ Resources:Comun, strEstadosGlobales %>"
                                                        Cls="btnEstadoGlobal-asR btnNoBorder"
                                                        Handler="displayMenu('pnEstadosGlobales')">
                                                    </ext:Button>
                                                    <%--<ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnSubprocesos"
                                                        ToolTip="<%$ Resources:Comun, strSubprocesos %>"
                                                        Cls="btnSubprocess-asR btnNoBorder"
                                                        Handler="displayMenu('pnSubprocesos')">
                                                    </ext:Button>--%>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="<%$ Resources:Comun, strEstadoSiguiente %>"
                                                        ID="btnEstadosSiguientes"
                                                        Cls="btnEstadoSiguiente-asR btnNoBorder"
                                                        Handler="displayMenu('pnEstadosSiguientes', )">
                                                    </ext:Button>
                                                    <%--<ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnLinks"
                                                        ToolTip="Links"
                                                        Cls="btnCompartir-asR btnNoBorder"
                                                        Handler="displayMenu('pnLinks')">
                                                    </ext:Button>--%>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnDocumentos"
                                                        ToolTip="<%$ Resources:Comun, strTarea %>"
                                                        Cls="btnDocs-asR btnNoBorder"
                                                        Handler="displayMenu('pnTareas')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnRoles"
                                                        ToolTip="<%$ Resources:Comun, strRolesEscritura %>"
                                                        Cls="btnRoles-asR btnNoBorder"
                                                        Handler="displayMenu('pnRoles')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnRolesSeguimiento"
                                                        ToolTip="<%$ Resources:Comun, strRolesLectura %>"
                                                        Cls="btnRolesLectura-asR btnNoBorder"
                                                        Handler="displayMenu('pnRolesSeguimiento')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ToolTip="<%$ Resources:Comun, strNotificaciones %>"
                                                        ID="btnNotificaciones"
                                                        Cls="btnNotificaciones-asR btnNoBorder"
                                                        Handler="displayMenu('pnNotificaciones')">
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
                                                                IconCls="ico-estado-info"
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
                                                ID="pnEstadosGlobales"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbEstadosGlobales"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblEstadosGlobales"
                                                                runat="server"
                                                                IconCls="ico-estadosGlobales-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strEstadosGlobales %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalEstadosGlobales"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoEstadosGlobales" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--SUBPROCESOS--%>
                                            <ext:Panel runat="server"
                                                ID="pnSubprocesos"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbSubprocesos"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblSubprocesos"
                                                                runat="server"
                                                                IconCls="ico-subprocess-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strSubprocesos %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalSubprocesos"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoSubprocesos" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--ESTADOS SIGUIENTES--%>
                                            <ext:Panel runat="server"
                                                ID="pnEstadosSiguientes"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbEstadosSiguientes"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblEstadosSiguientes"
                                                                runat="server"
                                                                IconCls="ico-estadoSiguiente-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strEstadoSiguiente %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalEstadosSiguientes"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoEstadosSiguientes" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--LINKS--%>
                                            <ext:Panel runat="server"
                                                ID="pnLinks"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbLinks"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblLinks"
                                                                runat="server"
                                                                IconCls="ico-link-info"
                                                                Cls="lblHeadAside "
                                                                Text="Links"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalLinks"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoLinks" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--DOCUMENTOS--%>
                                            <ext:Panel runat="server"
                                                ID="pnTareas"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbTareas"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblTareas"
                                                                runat="server"
                                                                IconCls="ico-Docs-info"
                                                                Cls="lblHeadAside "
                                                                Text="Tasks"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalTareas"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoTareas" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--ROLES--%>
                                            <ext:Panel runat="server"
                                                ID="pnRoles"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbRoles"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblRoles"
                                                                runat="server"
                                                                IconCls="ico-RolEscritura-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strRolesEscritura %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalRoles"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoRoles" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>
                                            <%--ROLES--%>

                                            <ext:Panel runat="server"
                                                ID="pnRolesSeguimiento"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbRolesSeguimiento"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblRolesSeguimiento"
                                                                runat="server"
                                                                IconCls="ico-RolLectura-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strRolesLectura %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalRolesSeguimiento"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoRolesSeguimiento" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>

                                            <%--ESTADOS GLOBALES--%>
                                            <ext:Panel runat="server"
                                                ID="pnNotificaciones"
                                                Hidden="true"
                                                Cls="tbGrey grdIntoAside"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbNotificaciones"
                                                        Cls="tbGrey"
                                                        Dock="Top"
                                                        Padding="0">
                                                        <Items>
                                                            <ext:Label
                                                                ID="lblNotificaciones"
                                                                runat="server"
                                                                IconCls="ico-Notificaciones-info"
                                                                Cls="lblHeadAside "
                                                                Text="<%$ Resources:Comun, strNotificaciones %>"
                                                                MarginSpec="18 7 15 7">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Label
                                                                ID="lblTotalNotificaciones"
                                                                runat="server"
                                                                Cls="lblHeadAside "
                                                                Text=""
                                                                MarginSpec="18 8 15 8">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <div id="bodyTablaInfoNotificaciones" class="single-item">
                                                    </div>
                                                </Content>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL MORE INFO--%>
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
