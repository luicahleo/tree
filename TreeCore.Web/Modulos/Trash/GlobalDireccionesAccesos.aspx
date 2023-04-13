<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalDireccionesAccesos.aspx.cs" Inherits="TreeCore.ModGlobal.GlobalDireccionesAccesos" %>

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
                <ext:Model runat="server" IDProperty="GlobalDireccionAccesoID">
                    <Fields>
                        <ext:ModelField Name="GlobalDireccionAccesoID" Type="Int" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Nombre" Direction="ASC" />
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
                    IDProperty="GlobalDireccionAccesoJornadaID">
                    <Fields>
                        <ext:ModelField Name="GlobalDireccionAccesoJornadaID" Type="Int" />
                        <ext:ModelField Name="Lunes" Type="Boolean" />
                        <ext:ModelField Name="Martes" Type="Boolean" />
                        <ext:ModelField Name="Miercoles" Type="Boolean" />
                        <ext:ModelField Name="Jueves" Type="Boolean" />
                        <ext:ModelField Name="Viernes" Type="Boolean" />
                        <ext:ModelField Name="Sabado" Type="Boolean" />
                        <ext:ModelField Name="Domingo" Type="Boolean" />
                        <ext:ModelField Name="Festivos" Type="Boolean" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="HoraInicio" />
                        <ext:ModelField Name="HoraFin" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="HoraInicio"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window
            runat="server"
            ID="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
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
                            runat="server"
                            ID="txtNombre"
                            FieldLabel="<%$ Resources:Comun, strNombre %>"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true" />
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
                        <ext:Checkbox runat="server"
                            ID="chkLunes"
                            FieldLabel="<%$ Resources:Comun, strLunes %>"
                            Checked="true" />
                        <ext:Checkbox runat="server"
                            ID="chkMartes"
                            FieldLabel="<%$ Resources:Comun, strMartes %>" />
                        <ext:Checkbox runat="server"
                            ID="chkMiercoles"
                            FieldLabel="<%$ Resources:Comun, strMiercoles %>" />
                        <ext:Checkbox runat="server"
                            ID="chkJueves"
                            FieldLabel="<%$ Resources:Comun, strJueves %>" />
                        <ext:Checkbox runat="server"
                            ID="chkViernes"
                            FieldLabel="<%$ Resources:Comun, strViernes %>" />
                        <ext:Checkbox runat="server"
                            ID="chkSabado"
                            FieldLabel="<%$ Resources:Comun, strSabado %>" />
                        <ext:Checkbox runat="server"
                            ID="chkDomingo"
                            FieldLabel="<%$ Resources:Comun, strDomingo %>" />
                        <ext:Checkbox runat="server"
                            ID="chkFestivo"
                            FieldLabel="<%$ Resources:Comun, strFestivo %>" />
                        <ext:TimeField runat="server"
                            ID="tfHoraInicio"
                            meta:resourceKey="tfHoraInicio"
                            FieldLabel="Hora Inicio Jornada"
                            MinTime="0:00"
                            MaxTime="23:59"
                            Increment="30"
                            SelectedTime="09:00"
                            Format="H:mm"
                            AllowBlank="false" />
                        <ext:TimeField runat="server"
                            ID="tfHoraFin"
                            meta:resourceKey="tfHoraFin"
                            FieldLabel="Hora Fin Jornada"
                            MinTime="0:00"
                            MaxTime="23:59"
                            Increment="30"
                            SelectedTime="09:30"
                            Format="H:mm"
                            AllowBlank="false" />
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
                                    ID="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnDefecto"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    Cls="btnDefecto"
                                    Handler="Defecto();" />
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="Activar()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('GlobalDireccionesAccesos', hdCliID.value, #{gridMaestro}, '-1');" />
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
                            <ext:Column
                                runat="server"
                                ID="colActivoMaestro"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colDefecto"
                                DataIndex="Defecto"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Cls="col-default"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                meta:resourceKey="ID"
                                DataIndex="GlobalDireccionAccesoID"
                                Text="GlobalDireccionAccesoID"
                                Hidden="true" />
                            <ext:Column
                                runat="server"
                                ID="colNombre"
                                DataIndex="Nombre"
                                Text="<%$ Resources:Comun, strNombre %>"
                                Width="250"
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
                                    ID="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('GlobalDireccionesAccesos', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colLunes"
                                DataIndex="Lunes"
                                Text="<%$ Resources:Comun, strLunes %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colMartes"
                                DataIndex="Martes"
                                Text="<%$ Resources:Comun, strMartes %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colMiercoles"
                                DataIndex="Miercoles"
                                Text="<%$ Resources:Comun, strMiercoles %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colJueves"
                                DataIndex="Jueves"
                                Text="<%$ Resources:Comun, strJueves %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colViernes"
                                DataIndex="Viernes"
                                Text="<%$ Resources:Comun, strViernes %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colSabado"
                                DataIndex="Sabado"
                                Text="<%$ Resources:Comun, strSabado %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colDomingo"
                                DataIndex="Domingo"
                                Text="<%$ Resources:Comun, strDomingo %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colFestivos"
                                DataIndex="Festivos"
                                Text="<%$ Resources:Comun, strFestivo %>"
                                Width="100"
                                Flex="1"
                                Align="Center">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colHoraInicio"
                                meta:resourceKey="columnHoraInicio"
                                DataIndex="HoraInicio"
                                Text="HoraInicio"
                                Width="100"
                                Flex="1"
                                Align="Center" />
                            <ext:Column
                                runat="server"
                                ID="colHoraFin"
                                meta:resourceKey="columnHoraFin"
                                DataIndex="HoraFin"
                                Text="HoraFin"
                                Width="100"
                                Flex="1"
                                Align="Center" />
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
                            ID="gridFilters2"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="gridFilters2" />
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
