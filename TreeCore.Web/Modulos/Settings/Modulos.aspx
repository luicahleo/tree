<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modulos.aspx.cs" Inherits="TreeCore.ModGlobal.Modulos" %>

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

        <ext:Store runat="server"
            ID="storePrincipal"
            AutoLoad="true"
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
                <ext:Model runat="server" IDProperty="ModuloID">
                    <Fields>
                        <ext:ModelField Name="ModuloID" Type="Int" />
                        <ext:ModelField Name="Modulo" />
                        <ext:ModelField Name="Pagina" />
                        <ext:ModelField Name="Descripcion" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Modulo" Direction="ASC" />
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
                <ext:Model runat="server" IDProperty="FuncionalidadID">
                    <Fields>
                        <ext:ModelField Name="FuncionalidadID" Type="Int" />
                        <ext:ModelField Name="Codigo" Type="Int" />
                        <ext:ModelField Name="Funcionalidad" />
                        <ext:ModelField Name="Descripcion" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Funcionalidad" Direction="ASC" />
            </Sorters>
        </ext:Store>



        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            meta:resourceKey="winGestion"
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

                        <ext:TextField ID="txtModulo"
                            FieldLabel="<%$ Resources:Comun, strModulo %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtModulo" />
                        <ext:TextField ID="txtPagina"
                            FieldLabel="<%$ Resources:Comun, strPagina %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            meta:resourceKey="txtPagina" />
                        <ext:TextField ID="txtDescripcion"
                            FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            runat="server"
                            Text=""
                            MaxLength="200"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            meta:resourceKey="txtDescripcion" />
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
            Title="<%$ Resources:Comun, winGestion.Title %>"
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

                        <ext:TextField ID="txtFuncionalidadDetalle"
                            FieldLabel="<%$ Resources:Comun, strFuncionalidad %>"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFuncionalidadDetalle" />
                        <ext:TextField ID="txtDescripcionDetalle"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                            Text=""
                            MaxLength="100"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            meta:resourceKey="txtDescripcionDetalle" />
                        <ext:NumberField ID="txtCodigoDetalle"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            runat="server"
                            MaxLength="50"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCodigoDetalle" />
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
                                    Handler="ExportarDatos('Modulos', #{gridMaestro}, '-1');" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>

                            <ext:Column DataIndex="Modulo"
                                ID="colModulo"
                                runat="server"
                                Width="200"
                                Text="<%$ Resources:Comun, strModulo %>"
                                meta:resourceKey="colModulo" />
                            <ext:Column DataIndex="Pagina"
                                ID="colPagina"
                                runat="server"
                                Width="200"
                                Text="<%$ Resources:Comun, strPagina %>"
                                meta:resourceKey="colPagina" />
                            <ext:Column DataIndex="Descripcion"
                                ID="colDescripcion"
                                runat="server"
                                Width="400"
                                Text="<%$ Resources:Comun, strDescripcion %>"
                                meta:resourceKey="colDescripcionModulo" Flex="1" />
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
                                    Handler="ActivarDetalle()"
                                    Hidden="true" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Modulos', #{GridDetalle}, #{ModuloID}.value);" />
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
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                meta:resourceKey="colActivo"
                                Width="50"
                                Hidden="true">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>

                            <ext:Column DataIndex="Codigo"
                                ID="colCodigo"
                                runat="server"
                                Width="150"
                                Header="<%$ Resources:Comun, strCodigo %>"
                                meta:resourceKey="colCodigo" />
                            <ext:Column DataIndex="Funcionalidad"
                                ID="colFuncionalidad"
                                runat="server"
                                Width="200"
                                Header="<%$ Resources:Comun, strFuncionalidad %>"
                                meta:resourceKey="colFuncionalidad" />
                            <ext:Column DataIndex="Descripcion"
                                ID="colDescripcionDetalle"
                                runat="server"
                                Width="500"
                                Header="<%$ Resources:Comun, strDescripcion %>"
                                meta:resourceKey="colDescripcion"
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
