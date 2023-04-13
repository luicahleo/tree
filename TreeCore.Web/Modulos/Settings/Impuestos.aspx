<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Impuestos.aspx.cs" Inherits="TreeCore.ModGlobal.Impuestos" %>

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
            ID="storePrincipal"
            AutoLoad="true"
            RemotePaging="false"
            OnReadData="storePrincipal_Refresh"
            RemoteSort="false">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    runat="server"
                    IDProperty="Code">
                    <Fields>
                        <ext:ModelField Name="Active" Type="Boolean" />
                        <ext:ModelField Name="Default" Type="Boolean" />
                        <ext:ModelField Name="Code" />
                        <ext:ModelField Name="Name" />
                        <ext:ModelField Name="CountryCode" />
                        <ext:ModelField Name="Valor" />
                        <ext:ModelField Name="Description" />
                        <%--<ext:ModelField Name="FechaActualizacion" Type="Date" />--%>
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Name" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <%--<ext:Store
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
                    IDProperty="ImpuestoDetalleID">
                    <Fields>
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="ImpuestoDetalleID" Type="Int" />
                        <ext:ModelField Name="ImpuestoID" Type="Int" />
                        <ext:ModelField Name="Valor" Type="Float" />
                        <ext:ModelField Name="FechaCambio" Type="Date" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="FechaCambio"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>--%>

        <ext:Store
            ID="storePaises"
            runat="server"
            AutoLoad="false"
            OnReadData="storePaises_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    IDProperty="PaisID"
                    runat="server">
                    <Fields>
                        <ext:ModelField Name="PaisID" Type="Int" />
                        <ext:ModelField Name="Pais" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window
            runat="server"
            ID="winGestion"
            meta:resourcekey="winGestion"
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
                        <ext:TextField
                            ID="txtImpuesto"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtImpuesto"
                            FieldLabel="<%$ Resources:Comun, strImpuesto %>" />
                        <ext:TextField
                            runat="server"
                            ID="txtCodigo"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true" />
                        <ext:NumberField
                            ID="nmbValor"
                            runat="server"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            meta:resourceKey="txtValor"
                            FieldLabel="<%$ Resources:Comun, strValor %>" />
                        <ext:ComboBox
                            meta:resourceKey="cmbPais"
                            ID="cmbPais"
                            runat="server"
                            StoreID="storePaises"
                            Mode="Local"
                            DisplayField="Pais"
                            ValueField="PaisID"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strPaises %>"
                            AllowBlank="true">
                            <Triggers>
                                <ext:FieldTrigger
                                    meta:resourceKey="RecargarLista"
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                            <Listeners>
                                <TriggerClick Handler="RecargarPaises();" />
                                <Select Fn="SeleccionaPais" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:TextField
                            runat="server"
                            ID="txtDescripcion"
                            FieldLabel="<%$ Resources:Comun, strDescripcion %>"
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

        <%--<ext:Window
            runat="server"
            ID="winGestionDetalle"
            meta:resourcekey="winGestionDetalle"
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
                        <ext:NumberField
                            ID="txtValorDetalle"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strValor %>"
                            MaxLength="100"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            meta:resourceKey="txtValorDetalle" />
                        <ext:DateField
                            ID="txtFechaRevion"
                            runat="server"
                            FieldLabel="Fecha Revision"
                            AllowBlank="false"
                            Format="<%$ Resources:Comun, FormatFecha %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFechaRevion" />
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
        </ext:Window>--%>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport
            runat="server"
            ID="vwContenedor"
            Cls="vwContenedor"
            Layout="Fitlayout">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
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
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
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
                                    ID="btnDefecto"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    meta:resourceKey="btnDefecto"
                                    Cls="btnDefecto"
                                    Handler="Defecto();" />


                                <ext:Button
                                    runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button
                                    runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Impuestos', #{gridMaestro}, '-1');" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                     <Listeners>
                            <AfterRender handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                            <Resize handler="GridColHandlerDinamicoV2(this)"></Resize>
                        </Listeners>
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="Activo"
                                DataIndex="Active"
                                Align="Center"
                                Cls="col-activo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                MinWidth="50"
                                MaxWidth="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="Defecto"
                                DataIndex="Default"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                MinWidth="50"
                                MaxWidth="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                DataIndex="Name"
                                Header="<%$ Resources:Comun, strImpuesto %>"
                                MinWidth="50"
                                Flex="1"
                                meta:resourceKey="columImpuesto"
                                ID="colImpuesto"
                                runat="server" />
                            <ext:Column
                                DataIndex="Valor"
                                Header="<%$ Resources:Comun, strValor %>"
                                MinWidth="50"
                                Flex="1"
                                meta:resourceKey="columValor"
                                ID="colValor"
                                runat="server" />
                            <ext:Column
                                DataIndex="CountryCode"
                                Header="<%$ Resources:Comun, strPais %>"
                                MinWidth="50"
                                Flex="1"
                                meta:resourceKey="columPais"
                                ID="colPais"
                                runat="server" />
                            <ext:Column
                                runat="server"
                                ID="Description"
                                meta:resourceKey="colDescricpcion"
                                DataIndex="Description"
                                Text="<%$ Resources:Comun, strDescripcion %>"
                                Width="50"
                                Flex="1" />
                            <%--<ext:DateColumn
                                DataIndex="FechaActualizacion"
                                Width="100"
                                meta:resourceKey="columFechaActualizacion"
                                ID="colFechaActualizacion"
                                runat="server"
                                Flex="1" />--%>
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
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
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
                <%--<ext:GridPanel
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
                                    ToolTip="Añadir"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="Editar"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="Eliminar"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnDefectoDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    meta:resourceKey="btnDefecto"
                                    Cls="btnDefecto"
                                    Handler="DefectoDetalle();" />
                                <ext:Button
                                    runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="Activar"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="Refrescar"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="Descargar"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Impuestos', #{GridDetalle}, #{hdModuloID}.value);" />
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
                                Cls="col-activo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colDefectoDetalle"
                                DataIndex="Defecto"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:DateColumn
                                DataIndex="FechaCambio"
                                Width="150"
                                meta:resourceKey="columnFechaCambio"
                                ID="colFechaCambio"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Valor"
                                Header="<%$ Resources:Comun, strValor %>"
                                Width="200"
                                Format="0.000,0000/i"
                                meta:resourceKey="columValor"
                                ID="colValorDetalle"
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
                            ID="gridFilters2"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                        <ext:CellEditing
                            runat="server"
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
                </ext:GridPanel>--%>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
