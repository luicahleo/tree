<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proyectos.aspx.cs" Inherits="TreeCore.PaginasComunes.Proyectos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/styleProyectosContenedor.css" rel="stylesheet" type="text/css" />--%>
    <script src="js/Proyectos.js"></script>
    <!--<script src="../../JS/common.js"></script>-->
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdProyectoID" runat="server" />
            <%--RESOURCE MANAGER--%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--FIN RESOURCE MANAGER--%>

            <%--VIEWPORT--%>
            <ext:Viewport ID="vwResp" runat="server" OverflowY="auto" Layout="FitLayout">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                </Content>
                <Items>

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
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" MinHeight="60" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>



                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strProyectos %>" Height="25" />

                                                </Items>
                                            </ext:Toolbar>


                                            <%-- TABS NAVEGACION--%>


                                            <ext:Toolbar runat="server" ID="tbNavNAside" Dock="Top" Cls="tbGrey tbNoborder" Hidden="false" PaddingSpec="10 10 10 10" OverflowHandler="Scroller" Flex="1">
                                                <Items>



                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkProyectos"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="<%$ Resources:Comun, strProyectos %>">
                                                        <Listeners>
                                                            <Click Handler="CambiarVista(1);"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>


                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkProyectosSLA"
                                                        Hidden="false"
                                                        Disabled="true"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strSLA %>">
                                                        <Listeners>
                                                            <Click Handler="CambiarVista(2)"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>


                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkProyectosUsuarios"
                                                        Hidden="false"
                                                        Disabled="true"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strMiembros %>">
                                                        <Listeners>
                                                            <Click Handler="CambiarVista(3)"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>

                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>


                                </DockedItems>
                                <Items>
                                    <ext:Container ID="ctMain1" runat="server" Layout="FitLayout">
                                        <Loader ID="LoaderMain"
                                            runat="server"
                                            Url="./ProyectosGestion.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="false" />
                                        </Loader>

                                    </ext:Container>
                                    <ext:Container ID="ctMain2" runat="server" Layout="FitLayout">
                                        <Loader ID="Loader1"
                                            runat="server"
                                            Url="./ProyectosSLA.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>

                                    </ext:Container>
                                    <ext:Container ID="ctMain3" runat="server" Layout="FitLayout">
                                        <Loader ID="Loader3"
                                            runat="server"
                                            Url="./ProyectosUsuarios.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                            <Params>
                                                <ext:Parameter Name="ProyectoID" Value="#{hdProyectoID.Value}" Mode="Raw" />
                                            </Params>
                                        </Loader>

                                    </ext:Container>
                                </Items>


                            </ext:Panel>


                            <%-- PANEL LATERAL DESPLEGABLE--%>

                            <ext:Panel runat="server" ID="pnAsideR"
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



                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="false" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
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

                                </Items>
                            </ext:Panel>

                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
            <%--FIN VIEWPORT--%>
        </div>
    </form>
</body>
</html>
