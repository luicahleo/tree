<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TreeCore.Login" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="/CSS/tCore.min.css" rel="stylesheet" type="text/css" />
    <link href="css/styleLogin.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <script type="text/javascript" src="General/js/Login.js"></script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div>
            <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore" />
            <%-- INICIO STORES --%>

            <ext:Store ID="storeIdiomas" runat="server" AutoLoad="true" OnReadData="storeIdiomas_Refresh" RemoteSort="true">
                <%--<Listeners>
                        <BeforeLoad Fn="ObtenerMenu" />
                    </Listeners>--%>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CodigoIdioma">
                        <Fields>
                            <ext:ModelField Name="CodigoIdioma" />
                            <ext:ModelField Name="Idioma" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Idioma" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%-- FIN STORES --%>

            <%-- INICIO WINDOWS --%>

            <ext:Window ID="winCambiarClave" runat="server" Title="Agregar" Width="400" AutoHeight="true" Collapsible="false"
                Modal="true" Resizable="false" ShowOnLoad="false" Hidden="true" meta:resourceKey="winCambiarClave">
                <Items>
                    <ext:FormPanel ID="formClave" runat="server" LabelWidth="100" LabelAlign="Top" BodyStyle="padding:10px;"
                        Border="false" MonitorPoll="500" MonitorValid="true">
                        <Items>
                            <ext:TextField ID="txtEmail" runat="server" FieldLabel="E-mail" MaxLength="200" ValidationGroup="FORM"
                                AllowBlank="false" Vtype="email" CausesValidation="true" meta:resourceKey="txtEmail" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnGuardar" runat="server" Text="Cambiar" Cls="btn-ppal" Icon="Accept" meta:resourceKey="btnCambiar">
                        <Listeners>
                            <Click Handler="winCambiarClaveBotonCambiar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Hide Handler="#{winCambiarClave}.hide();" />
                </Listeners>
            </ext:Window>

            <%-- FIN WINDOWS --%>

            <ext:Viewport runat="server">
                <Content>
                    <div id="login">
                        <div id="logoLogin">
                            <img src="/ima/logo-login.svg" alt="Tree-platform" />

                        </div>

                        <div id="dvFormLogin">

                            <div id="logoClienteLogin">
                                <img src="/ima/ima-logo-login.svg" alt="Entorno Demo" />

                            </div>




                            <div id="formLogin" class="dvFormLogin">

                                <ext:FormPanel
                                    runat="server"
                                    Title=""
                                    Frame="true">
                                    <Items>
                                        <ext:TextField
                                            ID="txtUserName"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Usuario"
                                            EmptyText="Usuario"
                                            IconCls="ico-lock-16px"
                                            CtCls="right-icon"
                                            Cls="formLogin"
                                            Text=""
                                            HideLabel="true"
                                            meta:resourceKey="txtUserName"
                                            AriaLabel="userName"
                                            EnableKeyEvents="true">
                                            <Listeners>
                                                <KeyPress Fn="pulsointro" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:TextField
                                            ID="txtPassword"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel=""
                                            EmptyText="Contraseña"
                                            InputType="Password"
                                            IconCls="ico-user-16px"
                                            CtCls="right-icon"
                                            Cls="formLogin"
                                            meta:resourceKey="txtPassword"
                                            Text=""
                                            AriaLabel="password"
                                            EnableKeyEvents="true">
                                            <Listeners>
                                                <KeyPress Fn="pulsointro" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:TextField ID="txtCode"
                                            runat="server"
                                            AllowBlank="true"
                                            Hidden="true"
                                            InputType="Text"
                                            HideLabel="true"
                                            EnableKeyEvents="true"
                                            meta:resourceKey="txtCode"
                                            MaxLength="6"
                                            AriaLabel="Code"
                                            IconCls="ico-user-16px"
                                            CtCls="right-icon"
                                            Cls="formLogin">
                                            <Listeners>
                                                <KeyPress Fn="pulsointro" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Button ID="btnLogin"
                                            Cls="btn-ppal"
                                            runat="server"
                                            Text="Iniciar Sesión"
                                            meta:resourceKey="btnLogin"
                                            Focusable="false"
                                            AriaLabel="Iniciar Sesión"
                                            AriaRole="button">
                                            <Listeners>
                                                <Click Handler="LoginClick();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:FormPanel>

                            </div>
                            <!-- formLogin -->
                            <div id="olvidado">
                                <ext:HyperlinkButton runat="server" ID="lnkOlvidado" Cls="lnk lnk-noline" Text="¿Has olvidado tu contraseña?" meta:resourceKey="lnkOlvidado">
                                    <Listeners>
                                        <Click Handler="EnviarCorreoCambioClave();" />
                                    </Listeners>
                                </ext:HyperlinkButton>
                                <ext:Label ID="lblVersion" runat="server" Cls="version" Text="V" />
                                <ext:Label ID="lblEntorno" runat="server" Cls="version" Text="Demo" />
                            </div>
                        </div>
                        <!--cajaFormLogin -->
                        <div id="pieLogin" class="d-flx">
                            <ext:CycleButton ID="cycleIdiomas" Cls="btn-splt-trans" runat="server" ShowText="true" Text="<%$ Resources:Comun, strIdioma %>">
                                <Menu>
                                    <ext:Menu ID="mnuIdiomas" runat="server">
                                        <Listeners>
                                            <Click Fn="CambioIdioma" />
                                        </Listeners>
                                    </ext:Menu>
                                </Menu>
                            </ext:CycleButton>
                            <ext:Button runat="server" ID="btnDesarrollador" Hidden="true" Text="Login" Cls="btn-splt-trans">
                                <Menu>
                                    <ext:Menu runat="server" ID="mnuDesarrollador">
                                        <Items>
                                            <ext:MenuItem runat="server" Text="Admin" IconCls="ico-desarrolladorAdmin">
                                                <Listeners>
                                                    <Click Handler="pasaLogin('ADMIN');" />
                                                </Listeners>
                                            </ext:MenuItem>
                                            <ext:MenuItem runat="server" Text="Súper" IconCls="ico-desarrolladorSuper">
                                                <Listeners>
                                                    <Click Handler="pasaLogin('SUPER');" />
                                                </Listeners>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            <div id="dvAyuda" class="d-none">
                                <a id="ayuda" class="lnk lnk-noLine" href="#">Ayuda</a>
                            </div>
                        </div>
                    </div>
                </Content>


            </ext:Viewport>

        </div>





        <!--login -->
    </form>

</body>
</html>
