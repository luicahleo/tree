<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MigradorExpImp.aspx.cs" Inherits="TreeCore.ModGlobal.MigradorExpImp" %>

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

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

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
                            <ext:ModelField
                                Name="ClienteID"
                                Type="Int" />
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
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="AlquilerTipoContratacionID">
                        <Fields>

                            <ext:ModelField
                                Name="AlquilerTipoContratacionID"
                                Type="Int" />
                            <ext:ModelField
                                Name="TipoContratacion" />
                            <ext:ModelField
                                Name="ClienteID"
                                Type="Int" />
                            <ext:ModelField
                                Name="Defecto"
                                Type="Boolean" />
                            <ext:ModelField
                                Name="Activo"
                                Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="TipoContratacion"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeTiposContratos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposContratos_Refresh"
                RemoteSort="false">
                <Listeners>
                    <BeforeLoad Handler="App.GridRowSelectTiposContratos.clearSelections();App.btnEliminarTiposContratos.disable();" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="AquilerTipoContratacionTipoContratoID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="AquilerTipoContratacionTipoContratoID"
                                Type="Int" />
                            <ext:ModelField
                                Name="AlquilerTipoContratoID"
                                Type="Int" />
                            <ext:ModelField
                                Name="TipoContrato"
                                Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeTiposContratosLibres"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposContratosLibres_Refresh"
                RemoteSort="false">
                <Listeners>
                    <BeforeLoad Handler="App.GridRowSelectTiposContratosLibre.clearSelections();" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="AlquilerTipoContratoID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="AlquilerTipoContratoID"
                                Type="Int" />
                            <ext:ModelField
                                Name="TipoContrato"
                                Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
                Title="Agregar"
                Width="450"
                Resizable="false"
                Cls="winGestion"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestion"
                        Border="false">
                        <Items>
                            <ext:TextField
                                ID="txtTipoContratacion"
                                runat="server"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                FieldLabel="Tipo Contratación"
                                Text=""
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtTipoContratacion" />
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
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--VENTANAS TIPOS CONTRATACIONES --%>

            <ext:Window
                ID="winTiposContratos"
                runat="server"
                Title="Tipos Contratos"
                Width="500"
                Height="500"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true"
                meta:resourceKey="winTiposContratos">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposContratos"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposContratos"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="Toolbar1"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button
                                        runat="server"
                                        ID="btnAgregar"
                                        meta:resourceKey="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir"
                                        Handler="BotonAgregarTipoContrato();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnEliminarTiposContratos"
                                        meta:resourceKey="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar"
                                        Disabled="true"
                                        Handler="BotonEliminarTipoContrato();" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel
                            ID="columnModelTiposContratos"
                            runat="server">
                            <Columns>
                                <ext:Column
                                    DataIndex="TipoContrato"
                                    Header="<%$ Resources:Comun, strTipoContrato %>"
                                    Width="350"
                                    meta:resourceKey="columTipoContrato"
                                    runat="server"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectTiposContratos"
                                runat="server"
                                Mode="Multi"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectTiposContratos" />
                                    <Deselect Fn="DeseleccionarGrillaTiposContratos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters
                                runat="server"
                                ID="gridFiltersTiposContratos"
                                MenuFilterText="Filtros"
                                meta:resourceKey="GridFilters"
                                Visible="true" />
                            <ext:CellEditing
                                runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>

                </Items>
                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolBar2"
                        runat="server"
                        PageSize="25"
                        StoreID="storeTiposContratos"
                        DisplayInfo="true">
                        <Plugins>
                            <ext:SlidingPager
                                ID="SlidingPager1"
                                runat="server" />
                        </Plugins>
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winTiposContratos}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winTiposContratosLibres"
                runat="server"
                Title="Tipos Contratos"
                Width="400"
                Height="400"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true"
                meta:resourceKey="winTiposContratos">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposEstructurasLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposContratosLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <ColumnModel
                            ID="columnModelTiposEstructurasLibres"
                            runat="server">
                            <Columns>
                                <ext:Column
                                    ColumnID="colTipoContrato"
                                    DataIndex="TipoContrato"
                                    Header="Tipo Contrato"
                                    Width="150"
                                    runat="server"
                                    Flex="1"
                                    meta:resourceKey="columTipoContrato" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectTiposContratosLibre"
                                runat="server"
                                Mode="Multi"
                                SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridTiposContratosLibresSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters
                                runat="server"
                                ID="gridFiltersTiposEstructurasLibres"
                                MenuFilterText="Filtros"
                                meta:resourceKey="GridFilters"
                                Visible="true" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        ID="btnCancelarEdifLibres"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel"
                        meta:resourceKey="btnCancelar">
                        <Listeners>
                            <Click Handler="#{winTiposContratosLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnGuardarTiposContratosLibre"
                        meta:resourceKey="btnAceptar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="BotonGuardarTiposContratosLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winTiposContratosLibres}.center();" />
                </Listeners>
            </ext:Window>


            <%--FIN TIPOS CONTRATACIONES --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
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
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
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
                                        Cls="btnAnadir"
                                        AriaLabel="Añadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="AgregarEditar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        meta:resourceKey="btnEditar"
                                        Cls="btnEditar"
                                        Handler="MostrarEditar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        meta:resourceKey="btnActivar"
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        meta:resourceKey="btnDefecto"
                                        Cls="btnDefecto"
                                        Handler="Defecto();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        meta:resourceKey="btnRefrescar"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnContratos"
                                        ToolTip="<%$ Resources:Comun, strTiposContratos %>"
                                        meta:resourceKey="btnTiposContratos"
                                        Cls="btnContratos"
                                        Handler="BotonTiposContratos();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnExport"
                                        ToolTip="Exportación Contrataciones"
                                        meta:resourceKey="btnExcelContrataciones_Contratos"
                                        Cls="btnExport"
                                        Handler="btnExportarContrataciones_Contratos();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('AlquileresTiposContrataciones', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox
                                        runat="server"
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
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    meta:resourceKey="colActivo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    Align="Center"
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    meta:resourceKey="colDefecto"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="TipoContratacion"
                                    Header="Tipo de Contratación"
                                    Width="250"
                                    meta:resourceKey="columTipoContratacion"
                                    ID="TipoContratacion"
                                    runat="server"
                                    Flex="1" />
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
                                ID="PagingToolBar"
                                meta:resourceKey="PagingToolBar"
                                StoreID="storePrincipal"
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
                                            <Select Fn="handlePageSizeSelect" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
