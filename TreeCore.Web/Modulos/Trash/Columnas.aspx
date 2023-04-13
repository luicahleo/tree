<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Columnas.aspx.cs" Inherits="TreeCore.ModGlobal.Columnas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden runat="server" ID="hdCliID" />
        <ext:Hidden runat="server" ID="ModuloID" />

        <%-- FIN HIDDEN --%>

        <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

        <ext:Store runat="server" ID="storePrincipal" AutoLoad="false" RemotePaging="false" OnReadData="storePrincipal_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="CategoriaTablaID">
                    <Fields>

                        <ext:ModelField Name="CategoriaTablaID" Type="Int" />
                        <ext:ModelField Name="CategoriaNombre" />
                        <ext:ModelField Name="TablaID" Type="Int" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="EsComercial" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="CategoriaNombre" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server" ID="storeDetalle" AutoLoad="false" RemotePaging="false" OnReadData="storeDetalle_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="ColumnaTablaID">
                    <Fields>

                        <ext:ModelField Name="ColumnaTablaID" Type="Int" />
                        <ext:ModelField Name="ColumnaNombre" />
                        <ext:ModelField Name="NombreTable" />
                        <ext:ModelField Name="Componente" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="Literal" />
                        <ext:ModelField Name="Valores" />
                        <ext:ModelField Name="CategoriaID" Type="Int" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Visible" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="ColumnaNombre" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientes_Refresh" RemoteSort="false">
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

        <ext:Store ID="storeTablas" runat="server" AutoLoad="false" OnReadData="storeTablas_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="TablaID" runat="server">
                    <Fields>
                        <ext:ModelField Name="TablaID" Type="Int" />
                        <ext:ModelField Name="TablaNombre" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeColumnas" runat="server" AutoLoad="false" OnReadData="storeColumnas_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="ColumnaTablaID" runat="server">
                    <Fields>
                        <ext:ModelField Name="ColumnaTablaID" Type="Int" />
                        <ext:ModelField Name="ColumnaNombre" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeComponentes" runat="server" AutoLoad="true" OnReadData="storeComponentes_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="ComponenteID" runat="server">
                    <Fields>
                        <ext:ModelField Name="ComponenteID" Type="Int" />
                        <ext:ModelField Name="Componente" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeTiposDatos" runat="server" AutoLoad="true" OnReadData="storeTiposDatos_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="TipoDatoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="TipoDato" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeValores" runat="server" AutoLoad="false" OnReadData="storeValores_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="ValorID" runat="server">
                    <Fields>
                        <ext:ModelField Name="ValorID" Type="Int" />
                        <ext:ModelField Name="Valor" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>

        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>


        <%-- VALORES WIN --%>
        <ext:Window
            ID="winValores"
            runat="server"
            Title="Gestión de Valores"
            Width="500"
            Height="500"
            Resizable="false"
            Modal="true" ShowOnLoad="true" Hidden="true"
            meta:resourceKey="winValores"
            Scrollable="Vertical">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridValores"
                    meta:resourceKey="grid"
                    SelectionMemory="false"
                    Cls="gridPanel"
                    StoreID="storeValores"
                    Title="etiqgridTitle"
                    Header="false"
                    EnableColumnHide="false"
                    AnchorHorizontal="-100"
                    AnchorVertical="100%"
                    AriaRole="main"
                    Scrollable="disabled">
                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="Toolbar1"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAgregarValores"
                                    meta:resourceKey="btnAgregar"
                                    Cls="btnAnadir"
                                    Handler="BotonAgregarValores();" />

                                <ext:Button runat="server"
                                    ID="btnEliminarValores"
                                    meta:resourceKey="btnQuitar"
                                    Cls="btnEliminar"
                                    Disabled="true"
                                    Handler="BotonEliminarValores();" />

                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel ID="columnModelValores" runat="server">
                        <Columns>
                            <ext:Column DataIndex="Valor" Header="Valor" Width="350" meta:resourceKey="Valor" runat="server" Flex="1"/>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="GridRowSelectValores" runat="server" Mode="Single" EnableViewState="true">
                            <Listeners>
                                <Select Fn="Grid_RowSelectValores" />
                                <Deselect Fn="DeseleccionarGrillaValores" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>

                </ext:GridPanel>

            </Items>
            <BottomBar>
                <ext:PagingToolbar ID="PagingToolBar3" runat="server" PageSize="25" StoreID="storeValores"
                    DisplayInfo="true">
                    <Plugins>
                        <ext:SlidingPager ID="SlidingPager1" runat="server" />
                    </Plugins>
                </ext:PagingToolbar>
            </BottomBar>
            <Buttons>
                <ext:Button ID="btnCerrarValores" runat="server" Text="Cerrar" IconCls="ico-cancel"
                        Cls="btn-cancel"
                    meta:resourceKey="btnCerrarValores">
                    <Listeners>
                        <Click Handler="#{winValores}.hide(); storeDetalle.reload();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winValores}.center();" />
            </Listeners>
        </ext:Window>

        <ext:Window ID="winValoresPendientes" runat="server" Width="400" Resizable="false"
            Title="Agregar Valores" AutoHeight="true" Modal="true" ShowOnLoad="false" Hidden="true"
            meta:resourceKey="winValoresPendientes">
            <Items>
                <ext:FormPanel ID="FormPanelPendientes" runat="server" LabelWidth="150" LabelAlign="Top"
                    BodyStyle="padding:10px;" MonitorPoll="500" MonitorValid="true" Border="false">
                    <Items>
                        <ext:TextField ID="txtValor" runat="server" Text="" MaxLength="100" ValidationGroup="FORM" Width="350"
                            AllowBlank="false" CausesValidation="true" meta:resourceKey="txtValor" />

                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoValoresPendientes(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnGuardarPendientes" runat="server" IconCls="ico-accept"
                        Cls="btn-accept" Text="Aceptar"
                    meta:resourceKey="btnGuardarPendientes">
                    <Listeners>
                        <Click Handler="BotonGuardarValores();" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="btnCancelarPendientes" runat="server" IconCls="ico-cancel"
                        Cls="btn-cancel" Text="Cancelar"
                    meta:resourceKey="btnCancelarPendientes">
                    <Listeners>
                        <Click Handler="#{winValoresPendientes}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winValoresPendientes}.center();" />
            </Listeners>
        </ext:Window>


        <%-- FIN VALORES WIN --%>


        <ext:Window runat="server"
            ID="winGestion"
            meta:resourceKey="winGestion"
            Title="Agregar"
            Width="400"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestion"
                    Cls="form-gestion"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>

                        <ext:TextField ID="txtCategoria" runat="server" FieldLabel="Categoria" Text="" MaxLength="50"
                            MaxLengthText="Ha superado el máximo número de caracteres" AllowBlank="false"
                            ValidationGroup="FORM" CausesValidation="true" meta:resourceKey="txtCategoria" />
                        <ext:Checkbox ID="chkEsComercial" runat="server" FieldLabel="Es Comercial"
                            Checked="false" meta:resourceKey="chkEsComercial">
                        </ext:Checkbox>
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValido(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelar"
                    meta:resourceKey="btnCancelar"
                    Text="Cancelar"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardar"
                    meta:resourceKey="btnGuardar"
                    Text="Guardar"
                    Disabled="true"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>

            </Buttons>
        </ext:Window>

        <ext:Window runat="server"
            ID="winGestionDetalle"
            meta:resourceKey="winGestionDetalle"
            Width="420"
            Resizable="false"
            Modal="true"
            Hidden="true">

            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>

                        <ext:ComboBox meta:resourceKey="cmbTablas" runat="server" ID="cmbTablas" StoreID="storeTablas"
                            Mode="local" ValueField="TablaNombre" DisplayField="TablaNombre" LabelAlign="Left"
                            FieldLabel="Tablas" EmptyText="Seleccione tabla" Editable="false" AllowBlank="false">
                            <Triggers>
                                
                                <ext:FieldTrigger meta:resourceKey="RecargarLista" IconCls="ico-reload" QTip="Recargar Lista" />
                            </Triggers>
                            <Listeners>
                                <Select Handler="#{cmbColumnas}.clearValue(); #{storeColumnas}.reload();" />
                                <TriggerClick Fn="TriggerTablas" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox meta:resourceKey="cmbColumnas" runat="server" ID="cmbColumnas" StoreID="storeColumnas"
                            Mode="local" ValueField="ColumnaNombre" DisplayField="ColumnaNombre" LabelAlign="Left"
                            FieldLabel="Columna" EmptyText="Seleccione columna" AllowBlank="false" Editable="false">
                            <Triggers>
                                
                                <ext:FieldTrigger meta:resourceKey="RecargarLista" IconCls="ico-reload" QTip="Recargar Lista" />
                            </Triggers>
                            <Listeners>
                                <TriggerClick Fn="TriggerColumnas" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField ID="txtLiteral" runat="server" FieldLabel="Literal" Text="" MaxLength="50"
                            MaxLengthText="Ha superado el máximo número de caracteres" AllowBlank="false"
                            ValidationGroup="FORM" CausesValidation="true" meta:resourceKey="txtLiteral" />
                        <ext:ComboBox meta:resourceKey="cmbComponentes" runat="server" ID="cmbComponentes"
                            StoreID="storeComponentes" Mode="local" ValueField="ComponenteID" DisplayField="Componente"
                            LabelAlign="Left" AllowBlank="false" FieldLabel="Componente" EmptyText="Seleccione componente" Editable="false">
                        </ext:ComboBox>
                        <ext:ComboBox meta:resourceKey="cmbTiposDatos" runat="server" ID="cmbTiposDatos"
                            StoreID="storeTiposDatos" Mode="local" ValueField="TipoDatoID" DisplayField="TipoDato"
                            LabelAlign="Left" FieldLabel="Tipo Dato" AllowBlank="false" EmptyText="Seleccione tipo dato" Editable="false">
                        </ext:ComboBox>
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarDetalle"
                    meta:resourceKey="btnCancelarDetalle"
                    Text="Cancelar"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarDetalle"
                    meta:resourceKey="btnGuardarDetalle"
                    Disabled="true"
                    Text="Guardar"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarDetalle();" />
                    </Listeners>
                </ext:Button>

            </Buttons>
            <Listeners>
                <Show Handler="#{winGestionDetalle}.center();" />
            </Listeners>
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server" ID="vwContenedor" Cls="vwContenedor" Layout="Anchor">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
                    AnchorHorizontal="-100"
                    AnchorVertical="58%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbBase"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAnadir"
                                    meta:resourceKey="btnAnadir"
                                    ToolTip="Añadir"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="Editar"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="Eliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="Activar"
                                    Cls="btnActivar"
                                    Handler="Activar()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="Refrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="Descargar"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Columnas', #{gridMaestro}, '-1');" />
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
                                    QueryMode="Local"
                                    Hidden="true"
                                    EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                    FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                    <Listeners>
                                        <Select Fn="SeleccionarCliente" />
                                        <TriggerClick Fn="RecargarClientes" />
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
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>

                            <ext:Column runat="server"
                                ID="Activa"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="EsComercial" Header="Es Comercial" Width="100" Align="Center" meta:resourceKey="columnEsComercial" ID="EsComercial" runat="server">

                                <Renderer Fn="ActivoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="CategoriaNombre" Width="300" meta:resourceKey="columnCategoriaNombre" ID="CategoriaNombre" runat="server" Flex="1" />
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
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFilters" 
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"
                            ID="PagingToolBar1"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storePrincipal"
                            DisplayInfo="true"
                            HideRefresh="true">
                            <Items>
                                <ext:ComboBox runat="server"
                                    Cls="comboGrid"
                                    Width="80">
                                    <Items>
                                        <ext:ListItem Text="1" />
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
                </ext:GridPanel>
                <ext:GridPanel
                    runat="server"
                    ID="GridDetalle"
                    Cls="gridPanel gridDetalle"
                    SelectionMemory="false"
                    EnableColumnHide="false"
                    StoreID="storeDetalle"
                    AnchorHorizontal="-100"
                    AnchorVertical="42%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbDetalle"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAnadirDetalle"
                                    meta:resourceKey="btnAnadirDetalle"
                                    ToolTip="Añadir"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="Editar"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="Eliminar"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="Activar"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()"
                                    Hidden="true" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="Refrescar"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnValores"
                                    meta:resourceKey="btnValores"
                                    ToolTip="Valores"
                                    Icon="DatabaseStart"
                                    Cls="btnValores"
                                    Handler="BotonValores()" />
                                <ext:Button runat="server"
                                    ID="btnVisible"
                                    meta:resourceKey="btnVisible"
                                    ToolTip="Visible"
                                    Icon="Zoom"
                                    Cls="btnVisible"
                                    Handler="BotonVisible()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="Descargar"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Columnas', #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="Activo"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                Width="50"
                                Hidden="true">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>

                            <ext:Column DataIndex="ColumnaNombre" Width="200" meta:resourceKey="ColumnColumnaNombre" ID="ColumnaNombre" runat="server" />
                            <ext:Column DataIndex="Componente" Width="150" meta:resourceKey="columnComponente" ID="Componente" runat="server" />
                            <ext:Column DataIndex="TipoDato" Width="150" meta:resourceKey="ColumnTipoDato" ID="TipoDato" runat="server" />
                            <ext:Column DataIndex="Literal" Width="150" meta:resourceKey="ColumnLiteral" ID="Literal" runat="server" />
                            <ext:Column DataIndex="Valores" Width="200" meta:resourceKey="ColumnValores" ID="Valores" runat="server" />
                            <ext:Column meta:resourceKey="columnVisible" DataIndex="Visible" Header="Visible" Flex="1"
                                Width="100" Align="Center" ID="Visible" runat="server">
                                <Renderer Fn="ActivoRender" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server"
                            ID="GridRowSelectDetalle"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect_Detalle" />
                                <Deselect Fn="DeseleccionarGrillaDetalle" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFiltersDetalle" 
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"
                            ID="PagingToolBar2"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storeDetalle"
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
                                        <Select Fn="handlePageSizeSelectDetalle" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
