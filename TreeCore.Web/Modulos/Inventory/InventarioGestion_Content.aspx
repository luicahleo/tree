<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioGestion_Content.aspx.cs" Inherits="TreeCore.ModInventario.InventarioGestion_Content" %>

<%@ Register Src="/Componentes/FormGestionElementos.ascx" TagName="FormGestion" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>InventarioGestion_Content</title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <%-- <link href="css/styleTracking.css" rel="stylesheet" type="text/css" />--%>
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <script type="text/javascript" src="js/InventarioGestion_Content.js"></script>
</head>
<body>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <link href="css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <link href="css/styleInventarioGestion_Content.css" rel="stylesheet" type="text/css" />


    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdEmplazamientoID" runat="server" />
            <ext:Hidden ID="hdEmplazamientoNombre" runat="server" />
            <ext:Hidden ID="hdNivelMaxPermitido" runat="server" />
            <ext:Hidden ID="hdVistaPlantilla" runat="server" />
            <ext:Hidden ID="hdHideMenuClick" runat="server" />
            <ext:Hidden ID="hdOperador" runat="server" />
            <ext:Hidden ID="hdEstado" runat="server" />
            <ext:Hidden ID="hdFechaMinCrea" runat="server" />
            <ext:Hidden ID="hdFechaMaxCrea" runat="server" />
            <ext:Hidden ID="hdFechaMinMod" runat="server" />
            <ext:Hidden ID="hdFechaMaxMod" runat="server" />
            <ext:Hidden ID="hdUsuario" runat="server" />
            <ext:Hidden runat="server" ID="hdElementoPadre" />

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

            <%--INICIO STORES--%>
            <ext:Store
                ID="storePrincipal"
                runat="server"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                BufferedRenderer="false"
                RemoteSort="true"
                RemotePaging="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="InventarioElementoVinculacionID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="InventarioElementoVinculacionID"
                                Type="int" />
                            <ext:ModelField
                                Name="InventarioElementoID"
                                Type="int" />
                            <ext:ModelField
                                Name="NumeroInventario"
                                Type="string" />
                            <ext:ModelField
                                Name="Nombre"
                                Type="string" />
                            <ext:ModelField
                                Name="Icono"
                                Type="string" />
                            <ext:ModelField
                                Name="InventarioCategoria"
                                Type="string" />
                            <ext:ModelField
                                Name="EstadoInventarioElemento"
                                Type="string" />
                            <ext:ModelField
                                Name="Operador"
                                Type="string" />
                            <ext:ModelField
                                Name="FechaCreacion"
                                Type="date" />
                            <ext:ModelField
                                Name="Padres" />
                            <ext:ModelField
                                Name="Vinculaciones" />
                            <ext:ModelField Name="OperadorID" Type="Int" />
                            <ext:ModelField Name="EstadoID" Type="Int" />
                            <ext:ModelField Name="CreadorID" Type="Int" />
                            <ext:ModelField Name="FechaAlta" />
                            <ext:ModelField Name="FechaMod" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="NumeroInventario" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeElementos"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storeElementos_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="txtSearch"
                listNotPredictive="InventarioElementoID,Nombre">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioElementoID">
                        <Fields>
                            <ext:ModelField Name="InventarioElementoID" Type="Int" />
                            <ext:ModelField Name="NumeroInventario" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="CargarBuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeElementosForm"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeElementosForm_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="txtSearch">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioElementoID">
                        <Fields>
                            <ext:ModelField Name="InventarioElementoID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposVinculaciones"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storeTiposVinculaciones_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="txtSearch">
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
            </ext:Store>

            <%--FIN STORES--%>

            <%--MENU BOTON DERECHO--%>

            <ext:Menu runat="server"
                ID="ContextMenu">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-Documents-ctxMnu"
                        Text="<%$ Resources:Comun, strDocumentos %>"
                        ID="ShowDocuments" />
                    <ext:MenuItem runat="server"
                        IconCls="ico-historial-ctxMnu"
                        Text="<%$ Resources:Comun, strHistorico %>"
                        ID="ShowHistorial" />
                </Items>
                <Listeners>
                    <Click Fn="OpcionSeleccionada" />
                </Listeners>
            </ext:Menu>

            <%--FIN MENU BOTON DERECHO--%>

            <%-- INICIO WINDOWS  --%>

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
                                ID="cmbElementos"
                                FieldLabel="<%$ Resources:Comun, strElementos %>"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                DisplayField="Nombre"
                                StoreID="storeElementosForm"
                                AllowBlank="false"
                                WidthSpec="100%"
                                ValueField="InventarioElementoID"
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

            <%-- FIN WINDOWS  --%>


            <ext:Viewport runat="server" ID="MainVwP" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Items>
                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
                        <Items>
                            <ext:GridPanel
                                runat="server"
                                Region="Center"
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
                                Flex="1"
                                MinHeight="600"
                                AriaRole="main">
                                <Listeners>
                                    <FilterChange Fn="ClearFilter" />
                                </Listeners>
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
                                                Hidden="true"
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
                                            <ext:Button runat="server"
                                                ID="btnFiltros"
                                                Cls="btnFiltros"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                Handler="parent.AbrirFiltros();" />
                                            <ext:ToolbarFill />
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
                                                    <KeyPress Fn="BuscarElemento" />
                                                    <TriggerClick Fn="LimpiarBusqueda" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:ToolbarFill />
                                            <ext:MultiCombo
                                                ID="cmbTiposVinculaciones"
                                                runat="server"
                                                LabelAlign="Left"
                                                Cls="txtSearchC"
                                                MaxWidth="300"
                                                FieldLabel="<%$ Resources:Comun, strInventarioTiposVinculaciones %>"
                                                DisplayField="Nombre"
                                                StoreID="storeTiposVinculaciones"
                                                ValueField="InventarioTipoVinculacionID">
                                                <Listeners>
                                                    <Select Fn="SeleccionarCombo" />
                                                    <TriggerClick Fn="RecargarCombo" />
                                                    <FocusLeave Handler="App.storePrincipal.reload()" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger
                                                        IconCls="ico-reload"
                                                        Hidden="true"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                        Weight="-1" />
                                                </Triggers>
                                            </ext:MultiCombo>
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
                                                ID="lbRutaEmplazamiento">
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
                                                ID="lbRutaElemento"
                                                Cls="rutaCategoria btnBack"
                                                Height="32"
                                                Hidden="true">
                                            </ext:Label>
                                            <ext:Button runat="server" Hidden="true" ID="btnPadreEleActucal" Cls="noBorder btnMenuRuta btnBack" IconCls="ico-link-vertical">
                                                <Menu>
                                                    <ext:Menu runat="server" ID="menuPadreEleActual" Cls="menuRuta">
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
                                            ID="colIcono"
                                            DataIndex="Icono"
                                            Align="Center"
                                            Sortable="false"
                                            Width="30"
                                            MinWidth="30">
                                            <Renderer Fn="RenderIcono" />
                                        </ext:Column>
                                        <ext:Column runat="server"
                                            ID="colNombre"
                                            meta:resourceKey="colNombre"
                                            DataIndex="Nombre"
                                            Text="<%$ Resources:Comun, strNombre %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:Column runat="server"
                                            ID="colNumeroInventario"
                                            meta:resourceKey="colNumeroInventario"
                                            DataIndex="NumeroInventario"
                                            Text="<%$ Resources:Comun, strCodigo %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:Column runat="server"
                                            ID="colInventarioCategoria"
                                            meta:resourceKey="colInventarioCategoria"
                                            DataIndex="InventarioCategoria"
                                            Text="<%$ Resources:Comun, strCategoria %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:Column runat="server"
                                            ID="colEstadoInventarioElemento"
                                            meta:resourceKey="colEstadoInventarioElemento"
                                            DataIndex="EstadoInventarioElemento"
                                            Text="<%$ Resources:Comun, strEstado %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:Column runat="server"
                                            ID="colOperador"
                                            meta:resourceKey="colOperador"
                                            DataIndex="Operador"
                                            Text="<%$ Resources:Comun, strOperador %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:DateColumn runat="server"
                                            ID="colFechaCreacion"
                                            meta:resourceKey="colFechaCreacion"
                                            DataIndex="FechaCreacion"
                                            Format="d/m/Y"
                                            Text="<%$ Resources:Comun, strFechaCreacion %>"
                                            MinWidth="100"
                                            Flex="1" />
                                        <ext:ComponentColumn runat="server"
                                            ID="colTiposVinculaciones"
                                            meta:resourceKey="colTiposVinculaciones"
                                            Cls="NoOcultar"
                                            Align="Center"
                                            Hidden="false"
                                            Text="<%$ Resources:Comun, strVinculacion %>"
                                            MinWidth="100">
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
                                            Cls="NoOcultar col-More"
                                            Align="Center"
                                            Hidden="false"
                                            Text="<%$ Resources:Comun, strElementosPadres %>"
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
                                                <Bind Fn="AbrirElePadres" />
                                            </Listeners>
                                        </ext:ComponentColumn>
                                        <ext:WidgetColumn runat="server"
                                            ID="colVerMas"
                                            meta:resourceKey="colVerMas"
                                            Text="<%$ Resources:Comun, strMas %>"
                                            Cls="NoOcultar col-More"
                                            Draggable="false"
                                            MinWidth="50"
                                            MaxWidth="50"
                                            HideMode="Visibility"
                                            Hideable="false">
                                            <Widget>
                                                <ext:Button runat="server"
                                                    OverCls="Over-btnMore"
                                                    PressedCls="Pressed-none"
                                                    FocusCls="Focus-none"
                                                    Cls="btnColMore">
                                                    <Listeners>
                                                        <Click Fn="VerMas" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Widget>
                                        </ext:WidgetColumn>
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
                                <View>
                                    <ext:GridView>
                                        <Listeners>
                                            <RowContextMenu Fn="ShowRightClickMenu" />
                                        </Listeners>
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                </Items>
                <Listeners>
                    <AfterRender Fn="AddGrid" />
                </Listeners>
            </ext:Viewport>

        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
