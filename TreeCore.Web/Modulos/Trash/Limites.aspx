<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Limites.aspx.cs" Inherits="TreeCore.ModGlobal.Limites" %>

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
                <ext:Model runat="server" IDProperty="GlobalLimiteID">
                    <Fields>
                        <ext:ModelField Name="GlobalLimiteID" Type="Int" />
                        <ext:ModelField Name="NombreLimite" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="FechaCreacion" Type="Date" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Vista" />
                        <ext:ModelField Name="Apellidos" />
                        <ext:ModelField Name="EMail" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                        <ext:ModelField Name="ProyectoTipo" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NombreLimite" Direction="ASC" />
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
                <ext:Model runat="server" IDProperty="GlobalLimiteCondicionID">
                    <Fields>

                        <ext:ModelField Name="GlobalLimiteCondicionID" Type="Int" />
                        <ext:ModelField Name="GlobalLimiteID" Type="Int" />
                        <ext:ModelField Name="NombreCondicion" />
                        <ext:ModelField Name="Campo" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="Valor" />
                        <ext:ModelField Name="Operador" />
                        <ext:ModelField Name="IncrementoPorcentaje" Type="Float" />
                        <ext:ModelField Name="FechaModificacion" Type="Date" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="Apellidos" />
                        <ext:ModelField Name="EMail" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="Vista" />
                        <ext:ModelField Name="Modificado" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="FechaModificacion" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeTiposDatos" runat="server" AutoLoad="false" OnReadData="storeTiposDatos_Refresh"
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
        <ext:Store ID="storeTipoCampoAsociadoCondiciones" runat="server" AutoLoad="false" OnReadData="storeTipoCampoAsociadoCondiciones_Refresh"
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
        <ext:Store ID="storeOperaciones" runat="server" AutoLoad="false" OnReadData="storeOperaciones_Refresh"
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
        <ext:Store ID="storeProyectosTipos" runat="server" AutoLoad="false" OnReadData="storeProyectosTipos_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="ProyectoTipoID" runat="server">
                    <Fields>
                        <ext:ModelField Name="ProyectoTipoID" />
                        <ext:ModelField Name="ProyectoTipo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            meta:resourcekey="winGestion"
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

                        <ext:TextField ID="txtLimite"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strLimite %>"
                            MaxLength="100"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtLimite"
                            QueryMode="Local"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" />
                        <ext:ComboBox ID="cmbTipoCampoAsociado"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strTabla %>"
                            Mode="Local"
                            DisplayField="TipoCampoAsociado"
                            ValueField="TipoCampoAsociadoID"
                            StoreID="storeTipoCampoAsociado"
                            QueryMode="Local"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
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
                        <ext:ComboBox 
                            ID="cmbProyectosTipos"
                            runat="server"
                            meta:resourceKey="cmbProyectosTipos"
                            StoreID="storeProyectosTipos"
                            DisplayField="ProyectoTipo"
                            ValueField="ProyectoTipoID"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
                            FieldLabel="<%$ Resources:Comun, strProyectoTipo %>"
                            QueryMode="Local"
                            AllowBlank="false">
                            <Listeners>
                                <Select Fn="SeleccionarProyectosTipos" />
                                <TriggerClick Fn="RecargarProyectosTipos" />
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
            meta:resourcekey="winGestionDetalle"
            Width="420"
            Resizable="false"
            Title="<%$ Resources:Comun, winGestion.Title %>"
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
                        <ext:TextField ID="txtCriterio"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strCriterio %>"
                            MaxLength="100"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            QueryMode="Local"
                            CausesValidation="true"
                            meta:resourceKey="txtCriterio" />
                        <ext:ComboBox ID="cmbTipoCampoAsociadoCondiciones"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strTabla %>"
                            Mode="Local"
                            DisplayField="TipoCampoAsociado"
                            ValueField="TipoCampoAsociadoID"
                            QueryMode="Local"
                            Hidden="true"
                            StoreID="storeTipoCampoAsociadoCondiciones"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
                            AllowBlank="true"
                            meta:resourceKey="cmbTipoCampoAsociado">
                            <Listeners>
                                <Select Fn="SeleccionarTipoCampoAsociadoCondiciones" />
                                <TriggerClick Fn="RecargarTipoCampoAsociadoCondiciones" />
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
                            runat="server"
                            Mode="Local"
                            DisplayField="CampoAsociado"
                            ValueField="CampoAsociadoID"
                            StoreID="storeCamposAsociados"
                            FieldLabel="<%$ Resources:Comun, strCampo %>"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
                            QueryMode="Local"
                            meta:resourceKey="cmbCamposAsociados"
                            Hidden="false">
                            <Listeners>
                                <Select Fn="SeleccionarCamposAsociados" />
                                <TriggerClick Fn="RecargarCamposAsociados" />
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
                            runat="server"
                            Mode="Local"
                            DisplayField="TipoDato"
                            ValueField="TipoDatoID"
                            FieldLabel="<%$ Resources:Comun, strTipoDato %>"
                            StoreID="storeTiposDatos"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
                            QueryMode="Local"
                            meta:resourceKey="cmbTiposDatos"
                            Hidden="false">
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
                        <ext:ComboBox ID="cmbOperaciones"
                            runat="server"
                            Mode="Local"
                            DisplayField="Operacion"
                            ValueField="OperacionID"
                            StoreID="storeOperaciones"
                            EmptyText="<%$ Resources:Comun, strNinguno %>"
                            Editable="true"
                            QueryMode="Local"
                            meta:resourceKey="cmbOperaciones"
                            FieldLabel="<%$ Resources:Comun, strOperador %>"
                            Hidden="false">
                            <Listeners>
                                <Select Fn="SeleccionarOperaciones" />
                                <TriggerClick Fn="RecargarOperaciones" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:TextField ID="txtValor"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strValor %>"
                            MaxLength="50"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtValor" />
                        <ext:NumberField ID="txtPorcentajeAdicional"
                            runat="server"
                            MaxLength="50"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtPorcentajeAdicional" />
                        <ext:Checkbox ID="ckModificado"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strModificado %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="ckModificado" />
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
                                    Cls="btnActivar"
                                    Handler="Activar();" />
                                <ext:Button runat="server"
                                    ID="btnDefecto"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    Cls="btnDefecto"
                                    Handler="Defecto();" />
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
                                    Handler="ExportarDatos('Limites', hdCliID.value, #{gridMaestro}, '-1');" />
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
                            <ext:Column runat="server"
                                ID="colDefecto"
                                DataIndex="Defecto"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Width="100">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:DateColumn DataIndex="FechaCreacion"
                                Width="100"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="colFechaCreacion"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                ID="colFechaCreacion"
                                Text="<%$ Resources:Comun, strFechaCreacion %>"
                                runat="server" />
                            <ext:Column DataIndex="NombreLimite"
                                Width="200"
                                meta:resourceKey="colNombreLimite"
                                ID="colNombreLimite"
                                Text="<%$ Resources:Comun, strNombre %>"
                                runat="server"
                                Flex="1" />
                            <ext:Column DataIndex="ProyectoTipo"
                                Width="200"
                                meta:resourceKey="colProyectoTipo"
                                Text="<%$ Resources:Comun, strProyectoTipo %>"
                                ID="colProyectoTipo"
                                runat="server" />
                            <ext:Column DataIndex="Vista"
                                Width="200"
                                meta:resourceKey="colVista"
                                Text="<%$ Resources:Comun, strVista %>"
                                ID="colVista"
                                runat="server" />
                            <ext:Column DataIndex="NombreCompleto"
                                Width="200"
                                meta:resourceKey="colNombreCompleto"
                                Text="<%$ Resources:Comun, strCreador %>"
                                ID="colNombreCompleto"
                                runat="server" />
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
                                    ID="btnDefectoDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    Cls="btnDefecto"
                                    Handler="DefectoDetalle();" />
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
                                    Handler="ExportarDatos('Limites', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                meta:resourceKey="colActivo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colDefectoDetalle"
                                DataIndex="Defecto"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Width="100">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>

                            <ext:DateColumn DataIndex="FechaModificacion"
                                Width="100"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="colFechaModificacion"
                                ID="colFechaModificacionDetalle"
                                Text="<%$ Resources:Comun, strFechaModificacion %>"
                                runat="server" />
                            <ext:Column DataIndex="NombreCondicion"
                                Width="200"
                                meta:resourceKey="colNombreCondicion"
                                ID="colNombreCondicionDetalle"
                                runat="server"
                                Text="<%$ Resources:Comun, strNombre %>"
                                Flex="1" />
                            <ext:Column DataIndex="Vista"
                                Width="250"
                                meta:resourceKey="colVista"
                                ID="colVistaDetalle"
                                Text="<%$ Resources:Comun, strVista %>"
                                runat="server" />
                            <ext:Column DataIndex="Campo"
                                Width="200"
                                meta:resourceKey="colCampo"
                                ID="colCampoDetalle"
                                Text="<%$ Resources:Comun, strCampo %>"
                                runat="server" />
                            <ext:Column DataIndex="TipoDato"
                                Width="200"
                                meta:resourceKey="colTipoDato"
                                ID="colTipoDatoDetalle"
                                Text="<%$ Resources:Comun, strTipoDato %>"
                                runat="server" />
                            <ext:Column DataIndex="Operador"
                                Width="100"
                                meta:resourceKey="colOperacion"
                                Text="<%$ Resources:Comun, strOperador %>"
                                ID="colOperacionDetalle"
                                runat="server" />
                            <ext:Column DataIndex="Valor"
                                Width="100"
                                meta:resourceKey="colValor"
                                Text="<%$ Resources:Comun, strValor %>"
                                ID="colValorDetalle"
                                runat="server" />
                            <ext:NumberColumn DataIndex="IncrementoPorcentaje"
                                Width="150"
                                meta:resourceKey="colIncrementoPorcentaje"
                                Text="<%$ Resources:Comun, strIncrementoPorcentaje %>"
                                ID="colIncrementoPorcentajeDetalle"
                                runat="server" />
                            <ext:Column DataIndex="Modificado"
                                Text="<%$ Resources:Comun, strModificado %>"
                                Width="100"
                                Align="Center"
                                meta:resourceKey="colModificado"
                                ID="colModificadoDetalle"
                                runat="server">
                                <Renderer Fn="DefectoRender" />
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
