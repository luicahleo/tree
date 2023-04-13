<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapaEmplazamientosCercanos.aspx.cs" Inherits="TreeCore.PaginasComunes.MapaEmplazamientosCercanos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="/Componentes/Mapas.ascx" TagName="Mapa" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript" src="js/MapaEmplazamientosCercanos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/Mapas.js"></script>
    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>

    <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js"></script>
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="/css/styleEmplazamientos.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>

            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" Name="hdCliID" runat="server" />
            <ext:Hidden ID="hdLatitudCercanos" runat="server" />
            <ext:Hidden ID="hdLongitudCercanos" runat="server" />
            <ext:Hidden ID="CurrentControl" runat="server" />
            <ext:Hidden ID="hdTabName" runat="server" />
            <ext:Hidden ID="hdPageName" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Viewport runat="server" Layout="FitLayout" Cls="vwContenedor">
               
                        <Content>
                            <local:Mapa runat="server" ID="UCMapas" />
                        </Content>
                  
            </ext:Viewport>

        </div>
    </form>
</body>
</html>
