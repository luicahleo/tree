<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Metodos.aspx.cs" Inherits="TreeCore.ModGlobal.Metodos" %>

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

        <ext:Store runat="server" ID="storePrincipal" AutoLoad="false" RemotePaging="false" OnReadData="storePrincipal_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="MetodoID">
                    <Fields>

                        <ext:ModelField Name="MetodoID" Type="Int" />
                        <ext:ModelField Name="Metodo" />
                        <ext:ModelField Name="NumParametros" Type="Int" />
                        <ext:ModelField Name="NombreClase" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="ExtensionURL" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Metodo" Direction="ASC" />
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
                <ext:Model runat="server" IDProperty="MetodoParametroID">
                    <Fields>

                        <ext:ModelField Name="MetodoParametroID" Type="Int" />
                        <ext:ModelField Name="Campo" />
                        <ext:ModelField Name="Tabla" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="ActivoParametros" Type="Boolean" />
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="MetodoID" Type="Int" />
                        <ext:ModelField Name="Orden" Type="Int" />
                        <ext:ModelField Name="Controlador" />
                        <ext:ModelField Name="CampoIntegracion" />
                        <ext:ModelField Name="ActivoMetodoParametro" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Orden" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeTiposDatos" runat="server" AutoLoad="false" OnReadData="storeTiposDatos_Refresh"
            RemoteSort="true">
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
            <Sorters>
                <ext:DataSorter Property="TipoDato" Direction="ASC" />
            </Sorters>
        </ext:Store>
        <ext:Store ID="storeTipoCampoAsociado" runat="server" AutoLoad="false" OnReadData="storeTipoCampoAsociado_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="TipoCampoAsociadoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="TipoCampoAsociadoID" />
                        <ext:ModelField Name="TipoCampoAsociado" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeCamposAsociados" runat="server" AutoLoad="false" OnReadData="storeCamposAsociados_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="CampoAsociadoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="CampoAsociadoID" Type="Int" />
                        <ext:ModelField Name="CampoAsociado" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

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

                        <ext:TextField ID="txtMetodo"
                            FieldLabel="<%$ Resources:Comun, strMetodo %>"
                            runat="server"
                            MaxLength="100"
                            Text=""
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="Metodo" />
                        <ext:TextField ID="txtNombreClase"
                            FieldLabel="<%$ Resources:Comun, strNombre %>"
                            runat="server"
                            MaxLength="100"
                            Text=""
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtNombreClase" />
                        <ext:TextField ID="txtExtensionURL"
                            FieldLabel="<%$ Resources:Comun, strURL %>"
                            runat="server"
                            MaxLength="1000"
                            Text=""
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="false"
                            meta:resourceKey="txtExtensionURL" />
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
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardar"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
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
            Title="<%$ Resources:Comun, winGestion.Title %>"
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

                        <ext:ComboBox ID="cmbTipoCampoAsociado"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strTabla %>"
                            Mode="Local"
                            DisplayField="TipoCampoAsociado"
                            ValueField="TipoCampoAsociado"
                            StoreID="storeTipoCampoAsociado"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            QueryMode="Local"
                            AllowBlank="false"
                            meta:resourceKey="cmbTipoCampoAsociado">
                            <Listeners>
                                <Select Fn="SeleccionarTipoCampoAsociado" />
                                <TriggerClick Fn="RecargarTipoCampoAsociado" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:ComboBox ID="cmbCamposAsociados"
                            FieldLabel="<%$ Resources:Comun, strCampo %>"
                            runat="server"
                            Mode="Local"
                            DisplayField="CampoAsociado"
                            ValueField="CampoAsociado"
                            StoreID="storeCamposAsociados"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            QueryMode="Local"
                            meta:resourceKey="cmbCamposAsociados"
                            Hidden="false"
                            AllowBlank="false">
                            <Listeners>
                                <Select Fn="SeleccionarCamposAsociados" />
                                <TriggerClick Fn="RecargarCamposAsociados" />
                                <%--<BeforeSelect Handler="App.storeCamposAsociados.reload();" />--%>
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:ComboBox ID="cmbTiposDatos"
                            FieldLabel="<%$ Resources:Comun, strTipoDato %>"
                            runat="server"
                            Mode="Local"
                            DisplayField="TipoDato"
                            ValueField="TipoDatoID"
                            StoreID="storeTiposDatos"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            QueryMode="Local"
                            meta:resourceKey="cmbTiposDatos"
                            Hidden="false"
                            AllowBlank="false">
                            <Listeners>
                                <Select Fn="SeleccionarTiposDatos" />
                                <TriggerClick Fn="RecargarTiposDatos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:NumberField ID="txtOrden"
                            FieldLabel="<%$ Resources:Comun, strOrden %>"
                            runat="server"
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtOrden" />
                        <ext:TextField ID="txtControlador"
                            FieldLabel="<%$ Resources:Comun, strControlador %>"
                            runat="server"
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtControlador" />
                        <ext:TextField ID="txtCampoIntegracion"
                            FieldLabel="<%$ Resources:Comun, strCampoIntegracion %>"
                            runat="server"
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCampoIntegracion" />
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
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
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
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
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
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    meta:resourceKey="btnActivar"
                                    Cls="btnActivar"
                                    Handler="Activar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Metodos', hdCliID.value, #{gridMaestro}, '-1');" />
                            </Items>
                        </ext:Toolbar>
                        <ext:Toolbar runat="server"
                            ID="tlbClientes"
                            Dock="Top">
                            <Items>
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
                                ID="colActivo"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>

                            <ext:Column DataIndex="Metodo"
                                Text="<%$ Resources:Comun, strMetodo %>"
                                Width="150"
                                Align="Center"
                                meta:resourceKey="colMetodo"
                                ID="colMetodo"
                                runat="server" />
                            <ext:Column DataIndex="NumParametros"
                                Text="<%$ Resources:Comun, strNumParametros %>"
                                Width="150"
                                Align="Center"
                                meta:resourceKey="colNumParametros"
                                ID="colNumParametros"
                                runat="server" />
                            <ext:Column DataIndex="NombreClase"
                                Text="<%$ Resources:Comun, strNombre %>"
                                Width="150"
                                Align="Center"
                                meta:resourceKey="colNombreClase"
                                ID="colNombreClase"
                                runat="server" />
                            <ext:Column DataIndex="ExtensionURL"
                                Text="<%$ Resources:Comun, strURL %>"
                                Width="150"
                                Align="Center"
                                meta:resourceKey="colExtensionURL"
                                ID="colExtensionURL"
                                runat="server"
                                Flex="1" />
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
                            meta:resourceKey="GridFilters" />
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
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Metodos', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                DataIndex="ActivoParametros"
                                Align="Center"
                                Cls="col-activo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="Orden"
                                Text="<%$ Resources:Comun, strOrden %>"
                                Width="50"
                                Align="Center"
                                meta:resourceKey="colOrden"
                                ID="colOrden"
                                runat="server" />
                            <ext:Column DataIndex="Tabla"
                                Width="250"
                                Text="<%$ Resources:Comun, strTabla %>"
                                Align="Center"
                                meta:resourceKey="colTabla"
                                ID="colTabla"
                                runat="server" />
                            <ext:Column DataIndex="Campo"
                                Width="100"
                                Text="<%$ Resources:Comun, strCampo %>"
                                Align="Center"
                                meta:resourceKey="colCampo"
                                ID="colCampo"
                                runat="server"
                                Flex="1" />
                            <ext:Column DataIndex="TipoDato"
                                Width="200"
                                Text="<%$ Resources:Comun, strTipoDato %>"
                                Align="Center"
                                meta:resourceKey="colTipoDato"
                                ID="colTipoDato"
                                runat="server" />
                            <ext:Column DataIndex="Controlador"
                                Width="200"
                                Text="<%$ Resources:Comun, strControlador %>"
                                Align="Center"
                                meta:resourceKey="colControlador"
                                ID="colControlador"
                                runat="server" />
                            <ext:Column DataIndex="CampoIntegracion"
                                Width="200"
                                Text="<%$ Resources:Comun, strCampoIntegracion %>"
                                Align="Center"
                                meta:resourceKey="colCampoIntegracion"
                                ID="colCampoIntegracion"
                                runat="server" />
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
                            ID="gridFilters2"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="gridFiltersDetalle" />
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
