<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriasContenedor.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriasContenedor" %>

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

            <ext:Hidden runat="server" ID="hdCliID" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>



            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>



            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server" ID="pnContenedor" Hidden="false" Flex="1" Layout="FitLayout">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="TbNavegacionTabs" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkCategorias"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        meta:resourceKey="lnkCategorias"
                                        Text="<%$ Resources:Comun, strEntidadesDatos %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkVinculaciones"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strVinculacion %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkDiagramas"
                                        Cls="lnk-navView lnk-noLine "
                                        Hidden="false"
                                        meta:resourceKey="lnkDiagrama"
                                        Text="<%$ Resources:Comun, strInventarioDiagrama %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSubcategorias"
                                        Cls="lnk-navView lnk-noLine"
                                        meta:resourceKey="lnkCategorias"
                                        Text="<%$ Resources:Comun, strComponentes %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:Panel ID="ctMain1" runat="server" Hidden="false" Flex="1" Layout="FitLayout">
                                <Items>
                                    <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                        <Loader ID="LoaderMain"
                                            runat="server"
                                            Url="../Inventory/InventarioCategorias.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="ctMain2" runat="server" Hidden="true" Layout="FitLayout" Flex="1">
                                <Items>
                                    <ext:Container ID="InventarioGestion_Content" runat="server" Layout="FitLayout">
                                        <Loader ID="LoaderGestion_Content"
                                            runat="server"
                                            Url="../Inventory/InventarioCategoriasVinculaciones.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="ctMain3" runat="server" Hidden="true" Layout="FitLayout" Flex="1">
                                <Items>
                                    <ext:Container ID="Container1" runat="server" Layout="FitLayout">
                                        <Loader ID="Loader1"
                                            runat="server"
                                            Url="../Inventory/InventarioCategoriaDiagrama.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="ctMain4" runat="server" Hidden="false" Flex="1" Layout="FitLayout">
                                <Items>
                                    <ext:Container ID="Container2" runat="server" Layout="FitLayout">
                                        <Loader ID="Loader2"
                                            runat="server"
                                            Url="../Inventory/InventarioCategoriasAtributos.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>
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
