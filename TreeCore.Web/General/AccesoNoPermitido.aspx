<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoNoPermitido.aspx.cs" Inherits="TreeCore.AccesoNoPermitido" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="CSS/tCore.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script>
        function windowReload() {
            window.location.href = window.location.origin + "/General/Login.aspx";
        }
    </script>
</head>
<body>
    <form id="fomrAccesoNoPermitido" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <ext:Viewport runat="server" ID="MainVP" Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server" ID="AccesoNoPermitido1">
                        <Content>
                            <div id="acceso-box-image" class="acceso-box-image">
                                <img src="ima/ico-lock-195-350.svg" alt="Tree-platform" />
                            </div>
                            <div id="acceso-box-text" class="acceso-box-text">
                                <h1>
                                    <ext:Label ID="accesoTitulo" Text="<%$ Resources:Comun, strAccesoNoPermitido %>" runat="server"></ext:Label>
                                </h1>
                                <ext:Label ID="accesoMensaje" Cls="accesoMensaje" Text="<%$ Resources:Comun, strAccesoDenegadoMensaje %>" runat="server"></ext:Label>
                                <ext:Button runat="server" ID="btnCerrarSesion" Cls="btn-ppal btn-acceso" Text="<%$ Resources:Comun, strCerrarSesion %>">
                                    <DirectEvents>
                                        <Click OnEvent="Logout"
                                            Success="windowReload()">
                                            <EventMask ShowMask="true"
                                                Msg="#{strSalida}" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>

                            </div>
                        </Content>

                    </ext:Panel>
                </Items>
            </ext:Viewport>


        </div>
    </form>

</body>
</html>
