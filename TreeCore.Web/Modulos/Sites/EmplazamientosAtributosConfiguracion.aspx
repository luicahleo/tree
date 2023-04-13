<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosAtributosConfiguracion.aspx.cs" Inherits="TreeCore.ModGlobal.EmplazamientosAtributosConfiguracion" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="/Componentes/AtributosConfiguracion.ascx" TagName="AtributosConfiguracion" TagPrefix="local" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="System.Collections.Generic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sites - Global</title>
</head>
<body>
    
    <link href="../../Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Componentes/js/CategoriasAtributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/Atributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/AtributosConfiguracion.js"></script>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdListaCategorias" runat="server" />
            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
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
                <Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCategoriasLibres"
                AutoLoad="true"
                OnReadData="storeCategoriasLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoAtributoCategoriaID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoAtributoCategoriaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport ID="vwSites" runat="server" Layout="FitLayout">
                <Items>
                    <ext:Container runat="server" HeightSpec="100%" Layout="FitLayout">
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnConfigurador"
                                Cls="gridPanelEmplazamiento"
                                meta:resourcekey="pnContenido"
                                Scrollable="Vertical"
                                MarginSpec="10 20 20 20"
                                Title="Configuracion Atributos Inventario">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tlbCLientes"
                                        Cls="toolbarHacked"
                                        Dock="Top">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:ComboBox runat="server"
                                                ID="cmbClientes"
                                                meta:resourceKey="cmbClientes"
                                                StoreID="storeClientes"
                                                DisplayField="Cliente"
                                                ValueField="ClienteID"
                                                Cls="comboGrid pos-boxGrid"
                                                QueryMode="Local"
                                                Hidden="true"
                                                EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                <Listeners>
                                                    <Select Fn="SeleccionarCliente" />
                                                    <TriggerClick Fn="RecargarClientes" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger
                                                        IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbSelCategoria"
                                        Cls="toolbarHacked"
                                        Dock="Top">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnRestriccionActive"
                                                IconCls="btn-filtGrid ico-checked-16-grey">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="Modo" Value="Active" />
                                                </CustomConfig>
                                                <Listeners>
                                                    <Click Fn="CambiarRestriccionDefecto" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnRestriccionDisabled"
                                                IconCls="btn-filtGrid ico-disabled-16-grey">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="Modo" Value="Disabled" />
                                                </CustomConfig>
                                                <Listeners>
                                                    <Click Fn="CambiarRestriccionDefecto" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnRestriccionHidden"
                                                IconCls="btn-filtGrid ico-hidden-16-grey">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="Modo" Value="Hidden" />
                                                </CustomConfig>
                                                <Listeners>
                                                    <Click Fn="CambiarRestriccionDefecto" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:ComboBox runat="server"
                                                ID="cmbCategoriaLibres"
                                                Text="<%$ Resources:Comun, strCategorias %>"
                                                Height="32"
                                                Width="140"
                                                MarginSpec="0 31 0 0"
                                                IconCls="ico-addBtn"
                                                Cls="cmbFakeButton"
                                                Editable="false"
                                                StoreID="storeCategoriasLibres"
                                                DisplayField="Nombre"
                                                ValueField="EmplazamientoAtributoCategoriaID"
                                                Scrollable="Vertical"
                                                Disabled="false"
                                                AllowBlank="true">
                                                <Listeners>
                                                    <Select Fn="SeleccionarCategoriaLibres" />
                                                    <FocusLeave Fn="LimpiarcmbCategoriaLibres" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                            </ext:Panel>
                        </Items>
                        <Content>
                            <local:AtributosConfiguracion
                                ID="AtributosConfiguracion"
                                runat="server" />
                        </Content>
                        <Listeners>
                            <AfterRender Fn="SetMaxHeight" />
                        </Listeners>
                    </ext:Container>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
