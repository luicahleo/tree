<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriasAtributosContenedor.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriasAtributosContenedor" %>

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
                    <ext:Panel ID="ctMain1" runat="server" Hidden="false" Flex="1" Layout="FitLayout">
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
                                        meta:resourceKey="lnkVinculaciones"
                                        Text="<%$ Resources:Comun, strTituloPlantillas %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                <Loader ID="LoaderMain"
                                    runat="server"
                                    Url="InventarioCategoriasAtributos.aspx"
                                    Mode="Frame">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="ctMain2" runat="server" Hidden="true" Layout="FitLayout" Flex="1">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="HyperlinkButton1"
                                        Cls="lnk-navView lnk-noLine"
                                        meta:resourceKey="lnkCategorias"
                                        Text="<%$ Resources:Comun, strEntidadesDatos %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="HyperlinkButton2"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        meta:resourceKey="lnkVinculaciones"
                                        Text="<%$ Resources:Comun, strTituloPlantillas %>">
                                        <Listeners>
                                            <Click Fn="NavegacionTabs"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:Container ID="InventarioGestion_Content" runat="server" Layout="FitLayout">
                                <Loader ID="LoaderGestion_Content"
                                    runat="server"
                                    Url="InventarioCategoriasAtributosPlantillas.aspx"
                                    Mode="Frame">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
