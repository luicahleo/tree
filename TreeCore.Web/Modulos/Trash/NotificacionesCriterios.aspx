<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificacionesCriterios.aspx.cs" Inherits="TreeCore.ModGlobal.NotificacionesCriterios" %>

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

        <ext:ResourceManager runat="server" 
            ID="ResourceManagerTreeCore" 
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

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
            ID="storePrincipal" 
            AutoLoad="false" 
            RemotePaging="false" 
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
                <ext:Model runat="server" IDProperty="NotificacionGrupoCriterioID">
                    <Fields>
                        <ext:ModelField Name="NotificacionGrupoCriterioID" Type="Int" />
                        <ext:ModelField Name="NotificacionGrupoCriterio" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="FechaCreacion" Type="Date" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Tabla" />
                        <ext:ModelField Name="Apellidos" />
                        <ext:ModelField Name="EMail" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NotificacionGrupoCriterio" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server" 
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
                <ext:Model runat="server" IDProperty="NotificacionCriterioID">
                    <Fields>
                        <ext:ModelField Name="NotificacionCriterioID" Type="Int" />
                        <ext:ModelField Name="NotificacionGrupoCriterioID" Type="Int" />
                        <ext:ModelField Name="NotificacionCriterio" />
                        <ext:ModelField Name="Campo" />
                        <ext:ModelField Name="FechaCreacion" Type="Date" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Apellidos" />
                        <ext:ModelField Name="EMail" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="Valor" />
                        <ext:ModelField Name="Operador" />
                        <ext:ModelField Name="Tabla" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="FechaCreacion" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeTiposDatos"  
            AutoLoad="true" 
            OnReadData="storeTiposDatos_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="TipoDatoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="TipoDato" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeTipoCampoAsociado"  
            AutoLoad="false" 
            OnReadData="storeTipoCampoAsociado_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="TipoCampoAsociadoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="TipoCampoAsociadoID" Type="Int" />
                        <ext:ModelField Name="TipoCampoAsociado" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeCamposAsociados"  
            AutoLoad="true" 
            OnReadData="storeCamposAsociados_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="CampoAsociadoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="CampoAsociadoID" Type="Int" />
                        <ext:ModelField Name="CampoAsociado" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeCampos" 
            AutoLoad="false"
            RemotePaging="false" 
            OnReadData="storeCampos_Refresh"
            RemoteSort="false" 
            PageSize="20">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="Campo" runat="server">
                    <Fields>
                        <ext:ModelField Name="CampoID" Type="Int" />
                        <ext:ModelField Name="Campo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeCamposLibres"  
            AutoLoad="false" 
            OnReadData="storeCamposLibres_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="Campo" runat="server">
                    <Fields>
                        <ext:ModelField Name="CampoID" Type="Int" />
                        <ext:ModelField Name="Campo"/>
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeOperaciones"  
            AutoLoad="true" 
            OnReadData="storeOperaciones_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="OperacionID" runat="server">
                    <Fields>
                        <ext:ModelField Name="OperacionID" Type="Int" />
                        <ext:ModelField Name="Operacion" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
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
                        <ext:TextField runat="server"
                            ID="txtGrupo"  
                            FieldLabel="<%$ Resources:Comun, strGrupo %>"
                            MaxLength="100" 
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                            AllowBlank="false"
                            ValidationGroup="FORM" 
                            CausesValidation="true"  />
                        <ext:ComboBox runat="server"
                            ID="cmbTipoCampoAsociado"  
                            FieldLabel="<%$ Resources:Comun, strTipoCampo %>"
                            QueryMode="Local"
                            DisplayField="TipoCampoAsociado" 
                            ValueField="TipoCampoAsociadoID"
                            StoreID="storeTipoCampoAsociado" 
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>" 
                            Editable="false" 
                            AllowBlank="false">
                            <Listeners>
                                    <Select Fn="SeleccionarTipoCampoAsociado" />
                                    <TriggerClick Fn="RecargarTipoCampoAsociado" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger 
                                        IconCls="ico-reload"
                                        Qtip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
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
                        <ext:TextField runat="server"
                            ID="txtCriterio" 
                            FieldLabel="<%$ Resources:Comun, strCriterio %>" 
                            MaxLength="100" 
                            ValidationGroup="FORM"
                            AllowBlank="false" 
                            CausesValidation="true"  />
                        <ext:ComboBox runat="server"
                            ID="cmbCamposAsociados"  
                            Mode="Local" 
                            DisplayField="CampoAsociado"
                            ValueField="CampoAsociadoID" 
                            FieldLabel="<%$ Resources:Comun, strCampo %>"
                            StoreID="storeCamposAsociados" 
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"  
                            Hidden="false">
                            <Listeners>
                                    <Select Fn="SeleccionarCampoAsociado" />
                                    <TriggerClick Fn="RecargarCampoAsociado" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger IconCls="ico-reload"
                                        Qtip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
                                </Triggers>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="cmbTiposDatos"  
                            Mode="Local" 
                            DisplayField="TipoDato"
                            ValueField="TipoDatoID"
                            FieldLabel="<%$ Resources:Comun, strTipoDato %>"
                            StoreID="storeTiposDatos" 
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>" 
                            Editable="false"
                            Hidden="false">
                            <Listeners>
                                    <Select Fn="SeleccionarTipoDato" />
                                    <TriggerClick Fn="RecargarTipoDato" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger IconCls="ico-reload"
                                        Qtip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
                                </Triggers>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="cmbOperaciones"  
                            Mode="Local" 
                            DisplayField="Operacion"
                            ValueField="OperacionID" 
                            FieldLabel="Operacion"
                            StoreID="storeOperaciones" 
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>" 
                            Editable="false"
                            meta:resourceKey="cmbOperaciones" 
                            Hidden="false">
                            <Listeners>
                                    <Select Fn="SeleccionarOperacion" />
                                    <TriggerClick Fn="RecargarOperacion" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger IconCls="ico-reload"
                                        Qtip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
                                </Triggers>
                        </ext:ComboBox>
                        <ext:TextField runat="server"
                            ID="txtValor" 
                            FieldLabel="<%$ Resources:Comun, strValor %>"
                            MaxLength="50" 
                            AllowBlank="false" 
                            ValidationGroup="FORM"
                            CausesValidation="true"  />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarDetalle"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarDetalle"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Disabled="true"
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

        <ext:Window ID="winCampos"
            runat="server"
            Title="Campos"
            Width="600"
            Height="400"
            Resizable="false"
            Modal="true"
            ShowOnLoad="false"
            Hidden="true"
            meta:resourceKey="winCampos">
            <Items>
                <ext:GridPanel runat="server"
                    ID="gridCampos"
                    Border="false"
                    Header="false"
                    Height="310"
                    StoreID="storeCampos"
                    SelectionMemory="false"
                    EnableColumnHide="false">
                    <TopBar>
                        <ext:Toolbar ID="toolBarCampos"
                            runat="server"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAgregarCampo"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="BotonAgregarCampo();" />
                                <ext:Button runat="server"
                                    ID="btnQuitarCampo"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Disabled="true"
                                    Handler="BotonEliminarCampo();" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarCampo"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarCampo();" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <ColumnModel ID="columnModelCampos" runat="server">
                        <Columns>
                            <ext:Column runat="server" 
                                ID="colCampo" 
                                DataIndex="Campo" 
                                Header="<%$ Resources:Comun, strCampo %>" 
                                Width="350" 
                                Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="GridRowSelectCampos"
                            runat="server"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelectCampos" />
                                <Deselect Fn="DeseleccionarGrillaCampos" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFiltersCampos"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolBarCampo"
                            runat="server"
                            PageSize="25"
                            StoreID="storeCampos"
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
                                        <Select Fn="handlePageSizeSelectCampo" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnGuardarCampo"
                    runat="server"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="#{winCampos}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winCampos}.center();" />
            </Listeners>
        </ext:Window>

        <ext:Window ID="winCamposLibres"
            runat="server"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            Height="300"
            Modal="true"
            Resizable="false"
            ShowOnLoad="false"
            Hidden="true">
            <Items>
                <ext:GridPanel runat="server"
                    ID="gridCamposLibres"
                    meta:resourceKey="grid"
                    Cls="gridPanel"
                    Title="Campos"
                    Frame="false"
                    Header="false"
                    Height="210"
                    StoreID="storeCamposLibres"
                    SelectionMemory="false"
                    AutoExpandColumn="Campo"
                    EnableColumnHide="false"
                    Scrollable="Vertical">
                    <ColumnModel ID="ColumnModelCampoLibre" runat="server">
                        <Columns>
                            <ext:Column runat="server" 
                                CellId="Campo" 
                                DataIndex="Campo" 
                                Header="<%$ Resources:Comun, strCampo %>"
                                Flex="1"
                                Width="150"  />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="GridRowSelectCampoLibre" 
                            runat="server" 
                            SingleSelect="false"
                            EnableViewState="true">
                            <Listeners>
                                <Select Fn="GridCamposLibresSeleccionar_RowSelect" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFiltersCamposLibres"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                </ext:GridPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnCerrar"
                    runat="server"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winCamposLibres}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="btnGuardarCamposLibres"
                    runat="server"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="BotonGuardarCamposLibres();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winCamposLibres}.center();" />
            </Listeners>
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server" 
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
                    AnchorHorizontal="100%"
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
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="Activar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnGlobales"
                                    meta:resourceKey="btnGlobales"
                                    Cls="btnBuscar"
                                    ToolTip="Campos"
                                    Handler="BotonNotificar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('NotificacionesCriterios', hdCliID.value, #{gridMaestro}, '-1');" />
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
                                        <ext:FieldTrigger 
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
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:DateColumn runat="server"
                                ID="colFechaCreacion"
                                DataIndex="FechaCreacion" 
                                Width="100"
                                Text="<%$ Resources:Comun, strFechaCreacion %>"
                                Format="<%$ Resources:Comun, FormatFecha %>"  
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colNotificacionGrupoCriterio"
                                DataIndex="NotificacionGrupoCriterio" 
                                Width="200"
                                Text="<%$ Resources:Comun, strCriterio %>"
                                meta:resourceKey="colNotificacionGrupoCriterio"   
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colNombreCompleto"
                                DataIndex="NombreCompleto" 
                                Width="100" 
                                Text="<%$ Resources:Comun, strNombreCompleto %>" 
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
                    AnchorHorizontal="100%"
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
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('NotificacionesCriterios', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:DateColumn runat="server"
                                ID="colFechaCreacionDetalle" 
                                DataIndex="FechaCreacion" 
                                Width="100" 
                                Text="<%$ Resources:Comun, strFechaCreacion %>"
                                Format="<%$ Resources:Comun, FormatFecha %>"   
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colNotificacionCriterio"
                                DataIndex="NotificacionCriterio" 
                                Text="<%$ Resources:Comun, strCriterio %>"
                                Width="200" 
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colTabla"
                                DataIndex="Tabla" 
                                Text="<%$ Resources:Comun, strTabla %>"
                                Width="150" 
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colCampoDetalle"
                                DataIndex="Campo" 
                                Text="<%$ Resources:Comun, strCampo %>"
                                Width="100" 
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colTipoDato"
                                DataIndex="TipoDato"
                                Text="<%$ Resources:Comun, strTipoDato %>"
                                Width="100"   
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colOperador"
                                DataIndex="Operador"
                                Text="<%$ Resources:Comun, strOperador %>"
                                Width="100" 
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colValor"
                                DataIndex="Valor"
                                Text="<%$ Resources:Comun, strValor %>"
                                Width="100" 
                                Flex="1" />
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
                            meta:resourceKey="GridFilters" />
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
