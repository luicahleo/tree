<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalActaTecnicasFabricantesModelos.aspx.cs" Inherits="TreeCore.ModGlobal.GlobalActaTecnicasFabricantesModelos" %>

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
        <ext:Hidden runat="server" ID="hdModuloID" />

        <%-- FIN HIDDEN --%>

        <ext:ResourceManager
            runat="server"
            ID="ResourceManagerTreeCore"
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

        <ext:Store
            runat="server"
            ID="storeClientes"
            AutoLoad="true"
            OnReadData="storeClientes_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    runat="server"
                    IDProperty="ClienteID">
                    <Fields>
                        <ext:ModelField Name="ClienteID" Type="Int" />
                        <ext:ModelField Name="Cliente" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="Cliente"
                    Direction="ASC" />
            </Sorters>
            <Listeners>
                <Load Handler="CargarStores();" />
            </Listeners>
        </ext:Store>

        <ext:Store
            runat="server"
            ID="storePrincipal"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storePrincipal_Refresh"
            RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    runat="server"
                    IDProperty="GlobalAntenaFabricanteID">
                    <Fields>
                        <ext:ModelField Name="GlobalAntenaFabricanteID" Type="Int" />
                        <ext:ModelField Name="Fabricante" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="Fabricante"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store
            runat="server"
            ID="storeDetalle"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeDetalle_Refresh"
            RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    runat="server"
                    IDProperty="GlobalAntenaModeloID">
                    <Fields>
                        <ext:ModelField Name="GlobalAntenaModeloID" Type="Int" />
                        <ext:ModelField Name="GlobalAntenaFabricanteID" Type="Int" />
                        <ext:ModelField Name="Modelo" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Dimensiones" />
                        <ext:ModelField Name="AEV" Type="Float" />
                        <ext:ModelField Name="AEVCA" Type="Float" />
                        <ext:ModelField Name="CA" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Activo" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window
            runat="server"
            ID="winGestion"
            meta:resourceKey="winGestion"
            Title="Agregar"
            Width="400"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel
                    runat="server"
                    ID="formGestion"
                    Cls="form-gestion"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField
                            ID="txtFabricante"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strFabricante %>"
                            Text=""
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFabricante" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValido(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button
                    runat="server"
                    ID="btnCancelar"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button
                    runat="server"
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

        <ext:Window
            runat="server"
            ID="winGestionDetalle"
            meta:resourceKey="winGestionDetalle"
            Width="420"
            Resizable="false"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel
                    runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField
                            ID="txtModelo"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strModelo %>"
                            Text=""
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtModelo" />
                        <ext:TextField
                            ID="txtDimensiones"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strDimensiones %>"
                            Text=""
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtDimensiones" />
                        <ext:NumberField
                            ID="txtAEV"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strAEV %>"
                            Text=""
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtAEV" />
                        <ext:NumberField
                            ID="txtAEVCA"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strAEVCA %>"
                            Text=""
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtAEVCA" />
                        <ext:NumberField
                            ID="txtCA"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strCA %>"
                            Text=""
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtCA" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button
                    runat="server"
                    ID="btnCancelarDetalle"
                    meta:resourceKey="btnCancelarDetalle"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button
                    runat="server"
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

        <ext:Viewport
            runat="server"
            ID="vwContenedor"
            Cls="vwContenedor"
            Layout="Anchor">
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
                        <ext:Toolbar
                            runat="server"
                            ID="tlbBase"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="btnAnadir"
                                    meta:resourceKey="btnAnadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    meta:resourceKey="btnActivar"
                                    Cls="btn-Activar"
                                    Handler="Activar();">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button
                                    runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />

                                <ext:Button
                                    ID="btnDescargar"
                                    runat="server"
                                    Cls="btnDescargar-subM"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Hidden="false"
                                    meta:resourceKey="btnDescargar">
                                    <Menu>
                                        <ext:Menu
                                            ID="MnuExcels"
                                            runat="server"
                                            BoxMinWidth="110">
                                            <Items>
                                                <ext:MenuItem
                                                    ID="btnExcel"
                                                    meta:resourceKey="btnExcel"
                                                    Text="Excel"
                                                    Icon="PageExcel"
                                                    Handler="ExportarDatos('GlobalActaTecnicasFabricantesModelos', hdCliID.value, #{gridMaestro}, '-1');" />
                                                <ext:MenuItem
                                                    runat="server"
                                                    ID="btnCargarExcelFabricanteYmodelos"
                                                    Text="Excel  Fabricante & Modelos"
                                                    Icon="PageExcel"
                                                    meta:resourceKey="btnCargarExcelFabricanteYmodelos"
                                                    Handler="btnCargarExcelFabricanteYmodelos();" />
                                            </Items>
                                        </ext:Menu>
                                    </Menu>
                                </ext:Button>
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
                                        <ext:FieldTrigger
                                            meta:resourceKey="RecargarLista"
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
                            <ext:Column
                                ID="colActivoFabricante"
                                runat="server"
                                DataIndex="Activo"
                                Hideable="false"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Width="200"
                                Cls="col-activo"
                                meta:resourceKey="ActivoColumn">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                ID="colFabricante"
                                Header="<%$ Resources:Comun, strFabricante %>"
                                Text="<%$ Resources:Comun, strFabricante %>"
                                DataIndex="Fabricante"
                                Hideable="false"
                                meta:resourceKey="FabricanteColumn"
                                Width="100" runat="server" Flex="1">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel
                            runat="server"
                            ID="GridRowSelect"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters
                            runat="server"
                            ID="gridFilters"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing
                            runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar
                            runat="server"
                            ID="PagingToolBar1"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storePrincipal"
                            DisplayInfo="true"
                            HideRefresh="true">
                            <Items>
                                <ext:ComboBox
                                    runat="server"
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
                        <ext:Toolbar
                            runat="server"
                            ID="tlbDetalle"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="btnAnadirDetalle"
                                    meta:resourceKey="btnAnadirDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button
                                    runat="server"
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
                                    Handler="ExportarDatos('GlobalActaTecnicasFabricantesModelos',hdCliID.value, #{GridDetalle}, #{hdModuloID}.value);" />
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
                                Width="50"
                                meta:resourceKey="colActivo">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                DataIndex="Modelo"
                                Text="<%$ Resources:Comun, strModelo %>"
                                Width="200"
                                meta:resourceKey="columnModelo"
                                ID="colModelo"
                                runat="server"
                                Flex="1" />

                            <ext:Column
                                DataIndex="Dimensiones"
                                Text="<%$ Resources:Comun, strDimensiones %>"
                                Width="200"
                                meta:resourceKey="columnDimensiones"
                                ID="colDimensiones"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="AEV"
                                Text="<%$ Resources:Comun, strAEV %>"
                                Width="200"
                                meta:resourceKey="columnAEV"
                                ID="colAEV"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="AEVCA"
                                Text="<%$ Resources:Comun, strAEVCA %>"
                                Width="200"
                                meta:resourceKey="columnAEVCA"
                                ID="colAEVCA"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="CA"
                                Text="<%$ Resources:Comun, strCA %>"
                                Width="200"
                                meta:resourceKey="columnCA"
                                ID="colCA"
                                runat="server"
                                Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel
                            runat="server"
                            ID="GridRowSelectDetalle"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect_Detalle" />
                                <Deselect Fn="DeseleccionarGrillaDetalle" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters
                            runat="server"
                            ID="gridFiltersDetalle"
                            MenuFilterText="Filtros"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar
                            runat="server"
                            ID="PagingToolBar2"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storeDetalle"
                            DisplayInfo="true"
                            HideRefresh="true">
                            <Items>
                                <ext:ComboBox
                                    runat="server"
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
