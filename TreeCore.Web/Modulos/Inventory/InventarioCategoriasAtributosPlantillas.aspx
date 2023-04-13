<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriasAtributosPlantillas.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriasAtributosPlantillas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden ID="hdCatSelect" runat="server" />
            <ext:Hidden ID="hdCatConfSelect" runat="server" />
            <ext:Hidden ID="hdListaCategorias" runat="server" />
            <ext:Hidden ID="hdVistaPlantilla" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
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
                    <ext:Model runat="server"
                        IDProperty="ClienteID">
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
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="CoreInventarioCategoriaAtributoCategoriaConfiguracionID">
                        <Fields>
                            <ext:ModelField Name="CoreInventarioCategoriaAtributoCategoriaConfiguracionID" Type="Int" />
                            <ext:ModelField Name="InventarioAtributoCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioAtributoCategoria" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioAtributoCategoria" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePlantillas"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storePlantillas_Refresh"
                RemoteSort="false"
                PageSize="20"
                RemoteFilter="false">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaPlantillas" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreInventarioPlantillaAtributoCategoriaID">
                        <Fields>
                            <ext:ModelField Name="CoreInventarioPlantillaAtributoCategoriaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="860"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtNombrePlantilla"
                                meta:resourceKey="txtNombrePlantilla"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                LabelAlign="Top"
                                Cls="ico-exclamacion-10px-grey"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                                </ext:TextField>
                            <ext:Container runat="server" ID="contenedorCategorias" Layout="AutoLayout" Scrollable="Vertical">
                                <Items>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Disabled="true"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="FitLayout">
                <Items>
                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server">
                                <Items>
                                    <ext:GridPanel
                                        Hidden="false"
                                        Cls="gridPanel TreePnl"
                                        ID="GridPanelCategorias"
                                        meta:resourceKey="GridPanelCategorias"
                                        runat="server"
                                        Scrollable="Vertical"
                                        MinHeight="600"
                                        ForceFit="true"
                                        StoreID="storePrincipal"
                                        Title="<%$ Resources:Comun, strPlantillas %>"
                                        RootVisible="false"
                                        EnableColumnHide="false"
                                        EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClientes"
                                                        meta:resourceKey="cmbClientes"
                                                        StoreID="storeClientes"
                                                        DisplayField="Cliente"
                                                        ValueField="ClienteID"
                                                        Cls="comboGrid pos-boxGrid"
                                                        Width="200"
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
                                                    <ext:ToolbarFill />
                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarButtons" Layout="ColumnLayout" Hidden="false" StyleSpec="border-style: hidden !important;">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnActivos"
                                                        ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                        Cls="btn-toggleGrid"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Width="42"
                                                        Handler="VerActivos();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colCategoria"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="InventarioAtributoCategoria"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="100"
                                                    Flex="1" />
                                                <ext:Column runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Cls="col-activo"
                                                    Hidden="true"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    Width="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" /> 
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server" StyleSpec="height: calc(100% - 30px) !important;">
                                <Items>
                                    <ext:GridPanel
                                        OverflowX="Hidden"
                                        OverflowY="Auto"
                                        runat="server"
                                        ID="grid"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanelInventario pnConfiguradorAtributos"
                                        Title="etiqgridTitle"
                                        Header="false"
                                        Region="Center"
                                        StoreID="storePlantillas"
                                        EnableColumnHide="true">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="tlbBase"
                                                Dock="Top"
                                                Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnAnadirPlantilla"
                                                        Disabled="true"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="AgregarEditarPlantilla" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditarPlantilla"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Cls="btnEditar">
                                                        <Listeners>
                                                            <Click Fn="MostrarEditarPlantilla" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminarPlantilla"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        Cls="btnEliminar">
                                                        <Listeners>
                                                            <Click Fn="EliminarPlantilla" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescarPlantilla"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar">
                                                        <Listeners>
                                                            <Click Fn="RefrescarPlantilla" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colNombre"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="Nombre"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="100"
                                                    Flex="1" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectPlantillas"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectPlantillas" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="GridFilters">
                                            </ext:GridFilters>
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />
                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePlantillas"
                                                DisplayInfo="true"
                                                HideRefresh="true">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        Cls="comboGrid"
                                                        Width="80">
                                                        <Items>
                                                            <ext:ListItem Text="10" />
                                                            <ext:ListItem Text="20" />
                                                            <ext:ListItem Text="30" />
                                                            <ext:ListItem Text="40" />
                                                            <ext:ListItem Text="50" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="20" />
                                                        </SelectedItems>
                                                        <Listeners>
                                                            <Select Fn="handlePageSizeSelect" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
