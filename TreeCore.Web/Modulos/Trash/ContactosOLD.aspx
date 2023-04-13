<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactosOLD.aspx.cs" Inherits="TreeCore.ModGlobal.ContactosOLD" %>

<%@ Register Src="../../Componentes/GridEmplazamientosContactos.ascx" TagName="GridContactosGlobales" TagPrefix="local" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Componentes/js/GridEmplazamientosContactos.js"></script>
    <!--<script src="../../JS/common.js"></script>-->
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <WindowResize Handler="GridResizer()" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeClientes"
                AutoLoad="true"
                OnReadData="storeClientes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ClienteID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Cliente" Direction="ASC" />
                </Sorters>
                <%--<Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>--%>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="FitLayout">
                <Items>
                    <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                        <Content>
                            <local:GridContactosGlobales ID="gridContactos"
                                vista="ContactoGlobal"
                                runat="server" />
                        </Content>
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
