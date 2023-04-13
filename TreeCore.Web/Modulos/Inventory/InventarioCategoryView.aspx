<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoryView.aspx.cs" Inherits="TreeCore.ModInventario.pages.InventarioCategoryView" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="js/Inventario_CategoryView.js"></script>
</head>
<body>
    <link href="css/styleInventario_CategoryView.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"></script>
    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdEmplazamientoID" runat="server" />
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdVistaPlantilla" runat="server" />
            <ext:Hidden ID="hdIdsResultados" runat="server" />
            <ext:Hidden ID="hdNameIndiceID" runat="server" />

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                    <DocumentReady Fn="CargarStores" />
                </Listeners>
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <ext:Store runat="server"
                ID="storeClientes"
                AutoLoad="false"
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
                    <Load Fn="CargarStores" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTipoEmplazamientos"
                AutoLoad="false"
                OnReadData="storeTipoEmplazamientos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="Tipo">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCategorias"
                AutoLoad="false"
                OnReadData="storeCategorias_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" />
                            <ext:ModelField Name="InventarioCategoria" />
                            <ext:ModelField Name="Icono" />
                            <ext:ModelField Name="NumElementos" Type="Int" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NumElementos" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Viewport runat="server" ID="MainVwP" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                </Content>
                <Items>
                    <%-----------------------Panel con menu visor elementos gráficos---------------------%>
                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="true" Layout="HBoxLayout" Flex="1">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Toolbar runat="server"
                                        ID="tbSliders"
                                        Dock="Top"
                                        Hidden="false"
                                        MinHeight="36"
                                        Cls="tbGrey tbNoborder">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnPrev"
                                                IconCls="ico-prev-w"
                                                Cls="btnMainSldr SliderBtn"
                                                Handler="loadPanelByBtns('Prev');"
                                                Disabled="true">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnNext"
                                                IconCls="ico-next-w"
                                                Cls="SliderBtn"
                                                Handler="loadPanelByBtns('Next');"
                                                Disabled="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <Items>
                                    <ext:Panel
                                        runat="server"
                                        ID="wrapComponenteCentralKPIResult"
                                        Hidden="false"
                                        Layout="BorderLayout"
                                        BodyCls="tbGrey">
                                        <Listeners>
                                            <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                                            <Resize Handler="showPanelsByWindowSize()"></Resize>
                                        </Listeners>
                                        <Items>
                                            <ext:GridPanel
                                                Hidden="false"
                                                Cls="gridPanelOnlyFocus"
                                                ID="TreePanel2"
                                                runat="server"
                                                ForceFit="true"
                                                StoreID="storeCategorias"
                                                Flex="12"
                                                Region="West"
                                                MaxWidth="350"
                                                MinWidth="250"
                                                OverflowX="Auto"
                                                HideHeaders="false"
                                                EnableColumnHide="false"
                                                EnableColumnMove="false"
                                                EnableColumnResize="false"
                                                RootVisible="false"
                                                OverflowY="Auto"
                                                Title="<%$ Resources:Comun, strTituloInventario %>">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tbClientes" Hidden="true">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClientes"
                                                                meta:resourceKey="cmbClientes"
                                                                StoreID="storeClientes"
                                                                DisplayField="Cliente"
                                                                ValueField="ClienteID"
                                                                LabelAlign="Top"
                                                                Cls="comboGrid"
                                                                QueryMode="Local"
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
                                                    <ext:Toolbar runat="server" ID="tbEmplazamientosTipo" Hidden="true">
                                                        <Items>
                                                            <ext:ComboBox meta:resourceKey="cmbTipoEmplazamientos"
                                                                ID="cmbTipoEmplazamientos" runat="server"
                                                                StoreID="storeTipoEmplazamientos"
                                                                DisplayField="Tipo"
                                                                ValueField="EmplazamientoTipoID"
                                                                Editable="true"
                                                                LabelAlign="Top"
                                                                Cls="comboGrid"
                                                                FieldLabel="<%$ Resources:Comun, strTipoEmplazamiento %>"
                                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                                AllowBlank="true"
                                                                QueryMode="Local">
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarTipoEmplazamientos" />
                                                                    <TriggerClick Fn="RecargarTipoEmplazamientos" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        Weight="-1"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                                </Triggers>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>


                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            ID="colIcono"
                                                            DataIndex="Icono"
                                                            Align="Center"
                                                            Sortable="false"
                                                            MinWidth="30"
                                                            MaxWidth="30">
                                                            <Renderer Fn="RenderIcono" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="TreeColumn12"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            Flex="1"
                                                            DataIndex="InventarioCategoria" />

                                                    </Columns>

                                                </ColumnModel>



                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectArbol"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="SelectItemMenu" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                            </ext:GridPanel>
                                            <ext:Panel ID="visorInsidePn"
                                                runat="server"
                                                Header="false"
                                                Border="false"
                                                Region="Center"
                                                Cls="grdNoHeader"
                                                Layout="FitLayout"
                                                Flex="1">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnCloseShowVisorTreeP"
                                                        IconCls="ico-hide-menu"
                                                        MaxWidth="48"
                                                        MaxHeight="44"
                                                        Handler="showOnlySecundary()"
                                                        Cls="positionBtn">
                                                    </ext:Button>
                                                    <ext:TabPanel ID="InventoryTabPanel" runat="server" Cls="gridPanel btnTabPanel" Layout="FitLayout">
                                                        <Listeners>
                                                            <Render Fn="CargarTabPrincipal" />
                                                            <TabChange Fn="CambiarTabFiltros" />
                                                            <TabClose Fn="CambiarTabFiltros" />
                                                        </Listeners>
                                                    </ext:TabPanel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
