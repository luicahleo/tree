<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaginaNoEncontrada.aspx.cs" Inherits="TreeCore.PaginaNoEncontrada" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
    <script src="/JS/PaginaNoEncontrada.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="fomrPaginaNoEncontrada" runat="server">
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
                                <img src="/ima/ico-404error.svg" alt="Tree-platform" />
                            </div>
                            <div id="acceso-box-text" class="acceso-box-text">
                                <h1>
                                    <ext:Label ID="accesoTitulo" Text="Something’s wrong here…" runat="server"></ext:Label>
                                </h1>
                                <ext:Label ID="accesoMensaje" Cls="accesoMensaje" Text="We can’t find the page you are looking for. Please check the URL, and refresh in a few minutes. Put in contact with your administrator if the problem persist." runat="server"></ext:Label>
                                <ext:Button 
                                    runat="server" 
                                    ID="btnCerrarSesion" 
                                    Cls="btn-ppal btn-acceso" 
                                    Text="Go Homepage" >
                                    <Listeners>
                                        <Click Handler="openGlobal()" />
                                    </Listeners>
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
