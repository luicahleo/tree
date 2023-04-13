<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoryViewVistaCategoria.aspx.cs" Inherits="TreeCore.ModInventario.pages.InventarioCategoryViewVistaCategoria" %>

<%@ Register Src="/Componentes/FormGestionElementos.ascx" TagName="FormGestion" TagPrefix="local" %>
<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>
    <link href="css/styleInventarioGraphicView.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/InventarioCategoryViewVistaCategoria.js"></script>
</head>
<body>
    <script type="text/javascript" src="../../Componentes/js/GestionCategoriasAtributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/GestionAtributos.js"></script>
    <link href="css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <ext:Hidden runat="server" ID="hdPageLoad" />
            <ext:Hidden runat="server" ID="hdErrorCarga" />
            <ext:Hidden runat="server" ID="hdEmplazamientoID" />
            <ext:Hidden runat="server" ID="hdCategoriaID" />
            <ext:Hidden runat="server" ID="hdCategoriaActiva" />
            <ext:Hidden runat="server" ID="hdNumeroMaximoCol" />
            <ext:Hidden runat="server" ID="hdVistaPlantilla" />
            <ext:Hidden runat="server" ID="hdVistaInventario" />
            <ext:Hidden runat="server" ID="hdColumas" />
            <ext:Hidden runat="server" ID="hdFiltros">
                <Listeners>
                    <AfterRender Fn="CargarFiltrosAplicados" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden ID="hdOperador" runat="server" />
            <ext:Hidden ID="hdEstado" runat="server" />
            <ext:Hidden ID="hdFechaMinCrea" runat="server" />
            <ext:Hidden ID="hdFechaMaxCrea" runat="server" />
            <ext:Hidden ID="hdFechaMinMod" runat="server" />
            <ext:Hidden ID="hdFechaMaxMod" runat="server" />
            <ext:Hidden ID="hdUsuario" runat="server" />
            <ext:Hidden ID="hdCodigoDuplicado" runat="server" />
            <ext:Hidden ID="hdEmplazamientosCargados" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" />
            <ext:Hidden ID="hdViewJson" runat="server">
                <Listeners>
                    <Change Fn="AplicarView" />
                </Listeners>
            </ext:Hidden>

            <ext:Store runat="server"
                ID="storeViews"
                AutoLoad="false"
                RemoteFilter="false"
                RemoteSort="true"
                OnReadData="storeViews_Refresh">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreGestionVistaID">
                        <Fields>
                            <ext:ModelField Name="CoreGestionVistaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="JsonColumnas" />
                            <ext:ModelField Name="JsonFiltros" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeEmplazamientos"
                AutoLoad="false"
                RemoteFilter="false"
                RemoteSort="false">
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoID" Type="Int" />
                            <ext:ModelField Name="NombreSitio" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Seleccionado" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Seleccionado" Direction="DESC" />
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                </Listeners>
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <%--MENU BOTON DERECHO--%>

            <ext:Menu runat="server"
                ID="ContextMenu">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-historial-ctxMnu"
                        Text="<%$ Resources:Comun, strHistorico %>"
                        ID="ShowHistorial" />
                    <ext:MenuItem runat="server"
                        IconCls="ico-Documents-ctxMnu"
                        Text="<%$ Resources:Comun, strDocumentos %>"
                        ID="ShowDocuments" />
                </Items>
                <Listeners>
                    <Click Fn="OpcionSeleccionada" />
                </Listeners>
            </ext:Menu>

            <%--FIN MENU BOTON DERECHO--%>

            <ext:Window runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="800"
                HeightSpec="80vh"
                Resizable="true"
                Modal="true"
                Cls="winForm-resp winNoPadding"
                Layout="FitLayout"
                Hidden="true">
                <Items>
                    <ext:Hidden runat="server" ID="hdElementoID"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdCodigoAutogenerado"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdCondicionCodigoReglaID"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdNombreAutogenerado"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdCondicionNombreReglaID"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdProyectoTipo"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hdHistoricoInventario"></ext:Hidden>

                    <ext:FormPanel
                        Hidden="false"
                        runat="server"
                        ForceFit="true"
                        ID="pnConfigurador"
                        Layout=""
                        BodyPaddingSummary="12 32"
                        HeightSpec="100%"
                        OverflowX="Auto"
                        OverflowY="Auto">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <DockedItems>
                            <ext:Toolbar runat="server" Dock="Bottom" Cls="tbGrey">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCancelarAgregarEditar"
                                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                        Cls=" btn-secondary">
                                        <Listeners>
                                            <Click Fn="btnCancelarFormElementos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGuardarAgregarEditar"
                                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                        Cls="btn-ppal"
                                        CausesValidation="true"
                                        Focusable="false"
                                        Disabled="true"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Click Fn="btnGuardarFormElementos" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>

                        </DockedItems>

                        <Items>
                            <ext:Container runat="server"
                                Cls="ctForm-resp ctForm-resp-col3" ID="flexContainer">

                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombreElemento"
                                        meta:resourceKey="txtNombreElemento"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        Text=""
                                        MaxLength="40"
                                        Cls="item-form"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoInventario" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtCodigoElemento"
                                        meta:resourceKey="txtCodigoElemento"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        LabelAlign="Top"
                                        Text=""
                                        MaxLength="40"
                                        Cls="item-form"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoInventario" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEstado"
                                        DisplayField="Nombre"
                                        Mode="Local"
                                        QueryMode="Local"
                                        LabelAlign="Top"
                                        ValueField="InventarioElementoAtributoEstadoID"
                                        Cls="item-form"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strEstado %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="InventarioElementosAtributosEstados" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeEstados"
                                                AutoLoad="false"
                                                OnReadData="storeEstados_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy Timeout="120000" />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="InventarioElementoAtributoEstadoID">
                                                        <Fields>
                                                            <ext:ModelField Name="InventarioElementoAtributoEstadoID" Type="Int" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboInventario" />
                                            <TriggerClick Fn="RecargarComboInventario" />
                                            <Change Fn="FormularioValidoInventario" />
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
                            </ext:Container>
                            <ext:Container runat="server"
                                Cls="gridFormBasic" ID="Container1">

                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCategoriaElemento"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                        AllowBlank="true"
                                        Hidden="true"
                                        Cls="item-form"
                                        Disabled="true">
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbCategoriaElemento"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        QueryMode="Local"
                                        Cls="item-form"
                                        Disabled="true"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="InventarioCategorias" />
                                        </CustomConfig>
                                        <Listeners>
                                            <Select Fn="SeleccionarCategoria" />
                                            <TriggerClick Fn="RecargarCategoria" />
                                            <Change Fn="FormularioValidoInventario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbOperador"
                                        DisplayField="Nombre"
                                        LabelAlign="Top"
                                        ValueField="EntidadID"
                                        AllowBlank="false"
                                        Cls="item-form"
                                        Mode="Local"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strOperador %>">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="Entidades" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeOperadores"
                                                AutoLoad="false"
                                                OnReadData="storeOperadores_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy Timeout="120000" />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EntidadID">
                                                        <Fields>
                                                            <ext:ModelField Name="EntidadID" Type="Int" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboInventario" />
                                            <TriggerClick Fn="RecargarComboInventario" />
                                            <Change Fn="FormularioValidoInventario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbPlantilla"
                                        DisplayField="Nombre"
                                        LabelAlign="Top"
                                        ValueField="InventarioElementoID"
                                        Hidden="true"
                                        Cls="item-form"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strPlantilla %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storePlantillas"
                                                AutoLoad="false"
                                                OnReadData="storePlantillas_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy Timeout="120000" />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="InventarioElementoID">
                                                        <Fields>
                                                            <ext:ModelField Name="InventarioElementoID" Type="Int" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarPlantilla" />
                                            <TriggerClick Fn="RecargarPlantilla" />
                                            <Change Fn="FormularioValidoInventario" />
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
                            </ext:Container>
                            <ext:Container runat="server" ID="contenedorCategorias" Layout="AutoLayout" Scrollable="Vertical">
                                <Items>
                                </Items>
                                <%--                                <Listeners>
                                    <AfterRender Fn="PintarCategorias" />
                                </Listeners>--%>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Fn="FormularioValidoInventario" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Listeners>
                    <Close Fn="DeseleccionarGrilla" />
                    <Show Fn="ComprobarFormulario" />
                    <BeforeClose Handler="LimpiarFormulario('formGestion')" />
                </Listeners>
            </ext:Window>

            <ext:Window runat="server"
                ID="winGestionElementos"
                meta:resourcekey="winGestionElementos"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                MaxWidth="400"
                Width="400"
                Height="500"
                Resizable="false"
                Modal="true"
                Cls="winForm-resp"
                PaddingSpec="10px 15px"
                Hidden="true">
                <DockedItems>
                    <ext:Toolbar runat="server" Dock="Bottom" Cls="tbGrey">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnGuardarGestionElementos"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Cls="btn-ppal"
                                CausesValidation="true"
                                Focusable="false"
                                Disabled="true"
                                ValidationGroup="FORM">
                                <Listeners>
                                    <Click Fn="btnGuardarGestionElementos" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:NumberField runat="server"
                        ID="nbCopias"
                        FieldLabel="Numero Copias"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MinValue="1"
                        WidthSpec="100%"
                        Hidden="true"
                        MaxHeight="32"
                        ValidationGroup="FORM"
                        CausesValidation="true" />
                    <ext:GridPanel
                        OverflowX="Hidden"
                        OverflowY="Auto"
                        runat="server"
                        ID="gridEmplazamientos"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        StoreID="storeEmplazamientos"
                        EnableColumnHide="false"
                        EnableColumnMove="false"
                        Height="400"
                        Cls="gridPanel"
                        Region="Center">
                        <DockedItems>
                            <ext:TextField runat="server"
                                ID="txtFilterEmplazamientos"
                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                EnableKeyEvents="true"
                                WidthSpec="100%">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" />
                                    <ext:FieldTrigger
                                        IconCls="ico-close"
                                        Hidden="true"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Weight="-1" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Fn="CerrarSeleccionarEmplazamientos" />
                                    <Change Fn="filterEmplazamientos" Buffer="500" />
                                </Listeners>
                            </ext:TextField>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server" DataIndex="Seleccionado" Width="50">
                                    <Renderer Fn="RenderCheckBox" />
                                </ext:Column>
                                <ext:Column runat="server" DataIndex="NombreSitio" Flex="1">
                                </ext:Column>
                                <ext:Column runat="server" DataIndex="Codigo" Flex="1">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridEmplazamientosFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters">
                            </ext:GridFilters>
                        </Plugins>
                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <ext:Viewport runat="server" ID="MainVwP" Layout="FitLayout" OverflowY="auto">
                <Items>
                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
                        <Items>
                            <ext:GridPanel
                                OverflowX="Hidden"
                                OverflowY="Auto"
                                runat="server"
                                ID="grid"
                                meta:resourceKey="grid"
                                SelectionMemory="false"
                                Cls="gridPanel"
                                Title="etiqgridTitle"
                                Header="false"
                                Region="Center"
                                EnableColumnHide="true">
                                <Listeners>
                                    <%--<ColumnHide Fn="Refrescar" />
                                    <ColumnShow Fn="Refrescar" />--%>
                                    <ViewReady Handler="this.store.reload();"></ViewReady>
                                    <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                    <%--<AfterRender Handler="GridColHandler(this)"></AfterRender>--%>
                                    <RowContextMenu Fn="ShowRightClickMenu" />
                                    <FilterChange Fn="ClearFilter" />
                                </Listeners>
                                <Store>
                                    <ext:Store runat="server"
                                        ID="storePrincipal"
                                        RemotePaging="true"
                                        AutoLoad="false"
                                        OnReadData="storePrincipal_Refresh"
                                        RemoteSort="true"
                                        PageSize="20"
                                        RemoteFilter="true">
                                        <Listeners>
                                            <BeforeLoad Fn="DeseleccionarGrilla" />
                                        </Listeners>
                                        <Proxy>
                                            <ext:PageProxy Timeout="120000" />
                                        </Proxy>
                                        <Model>
                                            <ext:Model runat="server" IDProperty="InventarioElementoID">
                                                <Fields>
                                                    <ext:ModelField Name="InventarioElementoID" Type="Int" />
                                                    <ext:ModelField Name="Nombre" />
                                                    <ext:ModelField Name="NumeroInventario" />
                                                    <ext:ModelField Name="Operador" />
                                                    <ext:ModelField Name="NombreAtributoEstado" />
                                                    <ext:ModelField Name="Codigo" />
                                                    <ext:ModelField Name="OperadorID" Type="Int" />
                                                    <ext:ModelField Name="EstadoID" Type="Int" />
                                                    <ext:ModelField Name="CreadorID" Type="Int" />
                                                    <ext:ModelField Name="FechaCreacion" />
                                                    <ext:ModelField Name="FechaMod" />
                                                    <ext:ModelField Name="InventarioCategoria" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Sorters>
                                            <ext:DataSorter Property="NumeroInventario" Direction="ASC" />
                                        </Sorters>
                                        <Listeners>
                                            <DataChanged Fn="CargarStoresFormulario" />
                                            <BeforeLoad Fn="CargarFiltros" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
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
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                Cls="btnEditar">
                                                <Listeners>
                                                    <Click Fn="MostrarEditar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnCopiar"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, strCopiar %>"
                                                Cls="btnCopiar">
                                                <Listeners>
                                                    <Click Fn="btnCopiarElementos" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnClonar"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, strClonar %>"
                                                Cls="btnClonar">
                                                <Listeners>
                                                    <Click Fn="btnClonarElementos" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnMover"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, strClonar %>"
                                                Cls="btnMover">
                                                <Listeners>
                                                    <Click Fn="btnMoverElementos" />
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
                                                Cls="btnDescargar-subM btnDescargarMenu"
                                                AriaLabel="Descargar">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:MenuItem
                                                                meta:resourceKey="mnuDwldExcel"
                                                                ID="mnuDwldExcel"
                                                                runat="server"
                                                                Text="<%$ Resources:Comun, strVistaActual %>"
                                                                IconCls="ico-CntxMenuExcel"
                                                                Handler="Exportar();" />
                                                            <ext:MenuItem
                                                                meta:resourceKey="mnuDwldLoadSites"
                                                                ID="btnDescargarNoColumnModel"
                                                                runat="server"
                                                                Text="<%$ Resources:Comun, strTodasColumnas %>"
                                                                IconCls="ico-CntxMenuExcel"
                                                                Handler="ExportarNoColumnModel();" />
                                                            <ext:MenuItem
                                                                ID="btnDescargarTodo"
                                                                Text="<%$ Resources:Comun, strTodasColumnas %>"
                                                                Hidden="true"
                                                                IconCls="ico-CntxMenuExcel"
                                                                Handler="parent.parent.ExportarCategorias();" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:Button>
                                            <%--<ext:Button runat="server"
                                                ID="btnDescargarTodo"
                                                ToolTip="<%$ Resources:Comun, strExportarInventario %>"
                                                Hidden="true"
                                                Cls="btnDescargarTodo">
                                                <Listeners>
                                                    <Click Handler="parent.parent.ExportarCategorias();" />
                                                </Listeners>
                                            </ext:Button>--%>
                                            <ext:Button runat="server"
                                                ID="btnFiltros"
                                                Cls="btnFiltros"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                Handler="parent.parent.MostrarPnFiltros(App.hdCategoriaID.value, App.hdColumas.value);">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbFiltros"
                                        Dock="Top"
                                        Cls="tlbGrid">
                                        <Items>
                                            <ext:ToolbarFill runat="server" />
                                            <ext:Button
                                                runat="server"
                                                Width="30"
                                                ID="btnClearFilters"
                                                Cls="btn-trans btnRemoveFilters"
                                                AriaLabel="Quitar Filtros"
                                                ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                <Listeners>
                                                    <Click Fn="BorrarFiltrosCategoria"></Click>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ComboBox runat="server"
                                                ID="cmbViews"
                                                LabelAlign="Top"
                                                DisplayField="Nombre"
                                                ValueField="CoreGestionVistaID"
                                                QueryMode="Local"
                                                Disabled="true"
                                                StoreID="storeViews">
                                                <Listeners>
                                                    <Select Fn="SelectView" />
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
                                    <%--                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                        <Content>

                                            <local:toolbarFiltros
                                                ID="cmpFiltro"
                                                runat="server"
                                                Stores="storePrincipal"
                                                MostrarComboFecha="false"
                                                FechaDefecto="Dia"
                                                Grid="grid"
                                                MostrarBotones="true"
                                                MostrarComboMisFiltros="true"
                                                MostrarBusqueda="false" />

                                        </Content>
                                    </ext:Container>--%>
                                </DockedItems>
                                <ColumnModel runat="server">
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server"
                                        ID="GridRowSelect"
                                        Mode="Multi">
                                        <Listeners>
                                            <Select Fn="Grid_RowSelect" />
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
                                        StoreID="storePrincipal"
                                        OverflowHandler="Scroller"
                                        DisplayInfo="true"
                                        HideRefresh="true">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                Cls="comboGrid"
                                                ID="cmbPagination"
                                                Width="80">
                                                <Items>
                                                    <ext:ListItem Text="10" Value="10" />
                                                    <ext:ListItem Text="20" Value="20" />
                                                    <ext:ListItem Text="30" Value="30" />
                                                    <ext:ListItem Text="40" Value="40" />
                                                    <ext:ListItem Text="50" Value="50" />
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
