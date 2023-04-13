<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriasVinculaciones.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriasVinculaciones" %>

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
            <ext:Hidden runat="server" ID="hdCategoriaPadre" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore"
                DisableViewState="true"
                ShowWarningOnAjaxFailure="false">
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
                    <ext:PageProxy Timeout="120000" />
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
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioCategoriaVinculacionID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaVinculacionID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoria" />
                            <ext:ModelField Name="InventarioCategoriaPadre" />
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="Icono" />
                            <ext:ModelField Name="Padres" />
                            <ext:ModelField Name="Vinculaciones" />
                            <ext:ModelField Name="TipoVinculacion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioCategoria" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCategorias"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCategorias_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="txtSearch"
                listNotPredictive="InventarioCategoriaID">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoria" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioCategoria" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="CargarBuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCategoriasForm"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCategoriasForm_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="txtSearch"
                listNotPredictive="InventarioCategoriaID">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoria" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioCategoria" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="CargarBuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTipoEmplazamientos"
                AutoLoad="false"
                OnReadData="storeTipoEmplazamientos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="EmplazamientoTipoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="DeseleccionarGrilla" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTipoVinculaciones"
                AutoLoad="false"
                OnReadData="storeTipoVinculacioness_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioTipoVinculacionID">
                        <Fields>
                            <ext:ModelField Name="InventarioTipoVinculacionID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="DeseleccionarGrilla" />
                </Listeners>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbCategorias"
                                FieldLabel="<%$ Resources:Comun, strEntidadDatos %>"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                DisplayField="InventarioCategoria"
                                StoreID="storeCategoriasForm"
                                AllowBlank="false"                                
                                WidthSpec="100%"
                                ValueField="InventarioCategoriaID"
                                Cls="item-form comboForm"
                                QueryMode="Local"
                                ValidationGroup="FORM">
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:MultiCombo runat="server"
                                ID="cmbTipoVinculaciones"
                                FieldLabel="<%$ Resources:Comun, strInventarioTiposVinculaciones %>"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                DisplayField="Nombre"
                                StoreID="storeTipoVinculaciones"
                                AllowBlank="false"
                                WidthSpec="100%"
                                ValueField="InventarioTipoVinculacionID"
                                Cls="item-form comboForm"
                                QueryMode="Local"
                                ValidationGroup="FORM">
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:MultiCombo>
                            <ext:ComboBox runat="server"
                                ID="cmbTipoVin"
                                FieldLabel="<%$ Resources:Comun, strCardinalidad %>"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                DisplayField="Name"
                                AllowBlank="false"
                                WidthSpec="100%"
                                Cls="item-form comboForm"
                                QueryMode="Local"
                                ValidationGroup="FORM">
                                <Items>
                                    <ext:ListItem Text="1-1" Value="1" />
                                    <ext:ListItem Text="1-N" Value="2" />
                                    <ext:ListItem Text="N-1" Value="3" />
                                    <ext:ListItem Text="N-M" Value="4" />
                                </Items>
                            </ext:ComboBox>
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
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Disabled="true"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Fn="winGestionBotonGuardar" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Layout="Anchor">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="grid"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storePrincipal"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        EnableColumnMove="false"
                        EnableColumnResize="false"
                        ForceFit="true"
                        MinHeight="700"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAnadir"
                                        Cls="btnAnadir"
                                        AriaLabel="Añadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                        <Listeners>
                                            <Click Fn="AgregarEditar" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        Cls="btnEditar">
                                        <Listeners>
                                            <Click Fn="MostrarEditar" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar">
                                        <Listeners>
                                            <Click Fn="Eliminar" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Cls="btnActivar">
                                        <Listeners>
                                            <Click Fn="Activar" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnActivos"
                                        ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                        Cls="btn-toggleGrid"
                                        EnableToggle="true"
                                        Pressed="true"
                                        Width="42">
                                        <Listeners>
                                            <Click Fn="VerActivos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar">
                                        <Listeners>
                                            <Click Fn="Refrescar" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Hidden="true"
                                        Handler="ExportarDatos('InventarioCategoriasVinculaciones', hdCliID.value, #{grid}, '');" />
                                    <ext:ToolbarFill />
                                    <ext:ComboBox meta:resourceKey="cmbTipoEmplazamientos"
                                        ID="cmbTipoEmplazamientos" runat="server"
                                        StoreID="storeTipoEmplazamientos"
                                        DisplayField="Tipo"
                                        ValueField="EmplazamientoTipoID"
                                        Editable="true"
                                        ForceSelection="true"
                                        FieldLabel="<%$ Resources:Comun, strTiposEmplazamientos %>"
                                        LabelAlign="Left"
                                        Scrollable="Vertical"
                                        OverflowX="Hidden"
                                        Height="35"
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
                            <ext:Toolbar runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbClientes"
                                        StoreID="storeClientes"
                                        DisplayField="Cliente"
                                        ValueField="ClienteID"
                                        Cls="comboGrid pos-boxGrid"
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
                                                QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbBuscador"
                                Dock="Top">
                                <Items>
                                    <ext:TextField
                                        ID="txtSearch"
                                        Cls="txtSearchC"
                                        runat="server"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        LabelWidth="50"
                                        Width="250"
                                        EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyPress Fn="BuscarCategoria" />
                                            <TriggerClick Fn="LimpiarBusqueda" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ToolbarFill />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbRuta"
                                Cls="tbGrey"
                                Dock="Top">
                                <Items>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnBack"
                                        IconCls="ico-prev"
                                        ID="btnBack">
                                        <Listeners>
                                            <Click Fn="VolverAtras" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Text="<%$ Resources:Comun, strComun %>"
                                        Cls="tbNavPath btnBack"
                                        ID="lbRutaEmplazamientoTipo">
                                        <Listeners>
                                            <Click Fn="IrRutaRaiz" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnNextRuta"
                                        IconCls="ico-next"
                                        Hidden="true"
                                        ID="btnRaizCarpeta">
                                    </ext:Button>
                                    <ext:Button runat="server" Hidden="true" ID="btnMenuRuta" Cls="noBorder btnMenuRuta btnBack" IconCls="ico-nav-folder-gr-16">
                                        <Menu>
                                            <ext:Menu runat="server" ID="menuRuta" Cls="menuRuta">
                                                <Items>
                                                </Items>
                                                <Listeners>
                                                    <Click Fn="SeleccionarRuta" />
                                                </Listeners>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnNextRuta"
                                        IconCls="ico-next"
                                        Hidden="true"
                                        ID="btnCarpetaActual">
                                    </ext:Button>
                                    <ext:Label runat="server"
                                        ID="lbRutaCategoria"
                                        Cls="rutaCategoria btnBack"
                                        Height="32"
                                        Hidden="true">
                                    </ext:Label>
                                    <ext:Button runat="server" Hidden="true" ID="btnPadreCatActucal" Cls="noBorder btnMenuRuta btnBack" IconCls="ico-link-vertical">
                                        <Menu>
                                            <ext:Menu runat="server" ID="menuPadreCatActual" Cls="menuRuta">
                                                <Items>
                                                </Items>
                                                <Listeners>
                                                    <Click Fn="SeleccionarPadre" />
                                                </Listeners>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    ID="colActivo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Cls="col-activo"
                                    Hidden="true"
                                    Width="50"
                                    MinWidth="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colIcono"
                                    DataIndex="Icono"
                                    Align="Center"
                                    Sortable="false"
                                    Width="30"
                                    MinWidth="30">
                                    <Renderer Fn="RenderIcono" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colCategoria"
                                    meta:resourceKey="colNombre"
                                    DataIndex="InventarioCategoria"
                                    Text="<%$ Resources:Comun, strEntidadDatos %>"
                                    MinWidth="100"
                                    Flex="4" />
                                <ext:Column runat="server"
                                    ID="colTipo"
                                    meta:resourceKey="colTipo"
                                    DataIndex="Tipo"
                                    Text="<%$ Resources:Comun, strTipoEmplazamiento %>"
                                    MinWidth="100"
                                    Flex="1" />
                                <ext:Column runat="server"
                                    ID="colTipoVinculacion"
                                    meta:resourceKey="colTipo"
                                    DataIndex="TipoVinculacion"
                                    Text="<%$ Resources:Comun, strCardinalidad %>"
                                    Align="Center"
                                    MinWidth="100"
                                    Flex="1" />
                                <ext:ComponentColumn runat="server"
                                    ID="colTiposVinculaciones"
                                    meta:resourceKey="colTiposVinculaciones"
                                    Cls="NoOcultar"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, strInventarioTiposVinculaciones %>"
                                    Hidden="false"
                                    MinWidth="150">
                                    <Component>
                                        <ext:HyperlinkButton runat="server" Cls="linkColumn">
                                            <Menu>
                                                <ext:Menu runat="server" ID="menuCategoriaTiposVinculaciones">
                                                    <Items>
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:HyperlinkButton>
                                    </Component>
                                    <Listeners>
                                        <Bind Fn="AbrirTiposVinculaciones" />
                                    </Listeners>
                                </ext:ComponentColumn>
                                <ext:ComponentColumn runat="server"
                                    ID="colVerPadres"
                                    meta:resourceKey="colVerPadres"
                                    Cls="NoOcultar"
                                    Align="Center"
                                    Hidden="false"
                                    Text="<%$ Resources:Comun, strCategoriasPadres %>"
                                    MinWidth="120">
                                    <Component>
                                        <ext:HyperlinkButton runat="server" Cls="linkColumn">
                                            <Menu>
                                                <ext:Menu runat="server" ID="menuCategoriaPadreHijas">
                                                    <Items>
                                                    </Items>
                                                    <Listeners>
                                                        <Click Fn="SeleccionarPadre" />
                                                    </Listeners>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:HyperlinkButton>
                                    </Component>
                                    <Listeners>
                                        <Bind Fn="AbrirCatPadres" />
                                    </Listeners>
                                </ext:ComponentColumn>
                            </Columns>
                        </ColumnModel>
                        <Listeners>
                            <DoubleTap Fn="AccederVinculacion" />
                        </Listeners>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="GridRowSelect"
                                Mode="Single">
                                <Listeners>
                                    <Select Fn="Grid_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="gridFilters" />
                        </Plugins>
                    </ext:GridPanel>
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
