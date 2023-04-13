<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TreeCore.General.Default" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="/Componentes/Menu.ascx" TagName="Menu" TagPrefix="local" %>
<%@ Register Src="/Componentes/Header.ascx" TagName="Header" TagPrefix="local" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Tree Platform</title>


    <%--<%: Scripts.Render("~/Content/js/Scripts", "~/Content/js/General", "~/Content/js/Global") %>--%>
    <%--<%: Styles.Render("~/Content/CSS") %>--%>
</head>
<body class="unscrollBody">
    <link href="General/css/styleDefault.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/Default.js"></script>

    <%--<script type="text/javascript" src="JS/common.js"></script>--%>

    <%-- Componente Header --%>
    <div>
        <ext:Container runat="server">
            <Content>
                <local:Header ID="ComponenteHeader"
                    runat="server"
                    Modulo="<%$ Resources:Comun, strGlobal %>" />
            </Content>
        </ext:Container>
    </div>

    <form id="form1" runat="server">

        <%--INICIO HIDDEN --%>

        <ext:Hidden ID="complejidad" runat="server" Text="50" />
        <ext:Hidden ID="Anno" runat="server" />
        <ext:Hidden ID="hdProyecto" runat="server" />
        <ext:Hidden ID="hdSeccion" runat="server" />
        <ext:Hidden ID="hdCliente" runat="server" />
        <ext:Hidden ID="hdCliID" runat="server" />
        <ext:Hidden ID="hdProAgID" runat="server" />
        <ext:Hidden ID="hdProTipoID" runat="server" />
        <ext:Hidden ID="hdUsuarioID" runat="server" />

        <%--FIN HIDDEN --%>
        <%--INICIO  RESOURCEMANAGER --%>

        <ext:ResourceManager
            ID="ResourceManagerTreeCore"
            runat="server"
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%--FIN  RESOURCEMANAGER --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window ID="winCambiarClave"
            runat="server"
            Title="Change Password"
            Width="400"
            AutoHeight="true"
            Modal="true"
            Resizable="false"
            ShowOnLoad="false"
            Closable="false"
            Hidden="true"
            meta:resourceKey="winCambiarClave">
            <Items>
                <ext:FormPanel ID="formClave"
                    BodyStyle="padding:10px;"
                    Border="false"
                    runat="server">
                    <Items>
                        <ext:Label runat="server"
                            ID="lblTitulo"
                            Cls="bold"
                            MinHeight="30"
                            Text="First access to the environment, you must change the password." />
                        <ext:TextField ID="txtCambiarClave"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strContraseña %>"
                            Text=""
                            InputType="Password"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM2"
                            EnableKeyEvents="true"
                            meta:resourceKey="txtCambiarClave"
                            Anchor="90%"
                            Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                            RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                        </ext:TextField>
                        <ext:TextField ID="txtCambiarClave2"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strConfirmarContraseña %>"
                            Text=""
                            InputType="Password"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM2"
                            EnableKeyEvents="true"
                            meta:resourceKey="txtCambiarClave2"
                            Anchor="90%"
                            Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                            RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                        </ext:TextField>
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoCambiarClave(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnCambiarGuardar"
                    runat="server"
                    Cls="btn-accept"
                    Disabled="true"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>">
                    <Listeners>
                        <Click Handler="winCambiarClaveBotonCambiar();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <ext:Window ID="winConfigInicial"
            runat="server"
            Title="<%$ Resources:Comun, strConfiguracion %>"
            Height="450"
            Width="680"
            Closable="false"
            Resizable="false"
            Modal="true"
            Cls="winForm-resp winGestion"
            Scrollable="Disabled"
            Hidden="true">
            <DockedItems>
            </DockedItems>
            <Items>

                <ext:Container ID="ctMain1" runat="server" MinHeight="400">
                    <Loader ID="LoaderMain"
                        runat="server"
                        Url="./ModGlobal/pages/ConfigInicialForm.aspx"
                        Mode="Frame">
                        <LoadMask ShowMask="false" />
                    </Loader>

                </ext:Container>
            </Items>
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%--INICIO  VIEWPORT --%>

        <div id="ctDefault">
            <%-- Componente Menu --%>
            <local:Menu ID="ComponenteMenu"
                runat="server"
                NombreModulo="" />
            <div id="centerDefault" class="ancla">
                <ext:Button runat="server"
                    ID="btnNoHeader"
                    Cls="btn-trans"
                    PressedCls="none"
                    FocusCls="none"
                    Handler="noHeader();">
                </ext:Button>
                <ext:Label ID="lblModNoHeader" runat="server" Text="nombreModulo"></ext:Label>
                <ext:TabPanel runat="server" Layout="FitLayout"
                    ID="tabPpal"
                    Region="Center"
                    Cls="tabPnl">
                    <Items>
                        <ext:Panel runat="server"
                            ID="pnInicio"
                            meta:resourceKey="pnInicio"
                            Title="Inicio"
                            Cls="gran-contenedor"
                            Hidden="true">
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </div>
        </div>
        <%--FIN  VIEWPORT --%>
    </form>
    <!--<script type="text/javascript" src="JS/Default.js"></script>-->
</body>
</html>
