<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monedas.aspx.cs" Inherits="TreeCore.ModGlobal.Monedas" %>

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
        <ext:Hidden runat="server" ID="hdEditando" />

        <%-- FIN HIDDEN --%>

        <ext:ResourceManager runat="server"
            ID="ResourceManagerTreeCore"
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

        <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="true" OnReadData="storePrincipal_Refresh" RemoteSort="false" PageSize="20">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="Code">
                    <Fields>
                        <ext:ModelField Name="Code" />
                        <ext:ModelField Name="Symbol" />
                        <ext:ModelField Name="DollarChange" Type="Float" />
                        <ext:ModelField Name="EuroChange" Type="Float" />
                        <ext:ModelField Name="Active" Type="Boolean" />
                        <ext:ModelField Name="Default" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Code" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <%--<ext:Store runat="server"
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
                <ext:Model runat="server" IDProperty="MonedaEvolucionID">
                    <Fields>
                        <ext:ModelField Name="MonedaEvolucionID" Type="Int" />
                        <ext:ModelField Name="MonedaID" Type="Int" />
                        <ext:ModelField Name="CambioDollarUS" Type="Float" />
                        <ext:ModelField Name="CambioEuro" Type="Float" />
                        <ext:ModelField Name="FechaCambio" Type="Date" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="FechaCambio" Direction="ASC" />
            </Sorters>
        </ext:Store>--%>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="500"
            BodyPaddingSummary="10 32"
            Modal="true"
            Resizable="false"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestion"
                    Cls="form-gestion ctForm-resp ctForm-resp-col2"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField runat="server"
                            ID="txtMoneda"
                            FieldLabel="<%$ Resources:Comun, strMoneda %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLength="50"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true">
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="txtSimbolo"
                            FieldLabel="<%$ Resources:Comun, strSimbolo %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLength="10"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true">
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:TextField>
                        <ext:NumberField runat="server"
                            ID="txtDolar"
                            MaxLength="20"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtDolar">
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:NumberField>
                        <ext:NumberField runat="server"
                            ID="txtEuro"
                            MaxLength="20"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtEuro">
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:NumberField>
                    </Items>
                    <%--<Listeners>
                        <ValidityChange Handler="FormularioValido(valid);" />
                    </Listeners>--%>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    Cls="btn-secondary-winForm"
                    Focusable="false">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Disabled="true"
                    Focusable="false"
                    Cls="btn-ppal-winForm">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <%--<ext:Window runat="server"
            ID="winGestionDetalle"
            Width="420"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Resizable="false"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle ctForm-resp ctForm-resp-col2"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            ID="txtDolarDetalle"
                            MaxLength="20"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtDolar" />
                        <ext:NumberField runat="server"
                            ID="txtEuroDetalle"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLength="20"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            meta:resourceKey="txtEuro" />
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
        </ext:Window>--%>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server"
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
                    AnchorHorizontal="100%"
                    AnchorVertical="100%"
                    AriaRole="main">
                    <Listeners>
                        <AfterRender Handler="GridColHandlerDinamicoV2(this);"></AfterRender>
                        <Resize Handler="GridColHandlerDinamicoV2(this);"></Resize>
                    </Listeners>
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
                                    ID="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="Activar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnDefecto"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    Cls="btnDefecto"
                                    Handler="Defecto();" />

                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Monedas', hdCliID.value, #{gridMaestro}, '-1');" />
                                <%--<ext:Button runat="server"
                                    ID="btnGlobales"
                                    ToolTip="Monedas Globales"
                                    Hidden="true"
                                    Cls="btnMoneda"
                                    meta:resourceKey="btnGlobales">
                                    <Listeners>
                                        <Click Fn="abrirMonedas" />
                                    </Listeners>
                                </ext:Button>--%>
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="colActivo"
                                DataIndex="Active"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                MinWidth="50"
                                MaxWidth="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="colDefecto"
                                DataIndex="Default"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Cls="col-default"
                                MinWidth="50"
                                MaxWidth="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column runat="server"
                                DataIndex="Code"
                                Text="<%$ Resources:Comun, strMoneda %>"
                                MinWidth="50"
                                ID="colMoneda"
                                Flex="1" />
                            <ext:Column runat="server"
                                DataIndex="Symbol"
                                MinWidth="50"
                                Text="<%$ Resources:Comun, strSimbolo %>"
                                ID="colSimbolo"
                                Flex="1" />
                            <%--<ext:DateColumn runat="server"
                                DataIndex="FechaActualizacion"
                                Width="100"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columFechaActualizacion"
                                ID="colFechaActualizacion"
                                Flex="1" />--%>
                            <ext:NumberColumn runat="server"
                                DataIndex="DollarChange"
                                Format="0.000,0000/i"
                                MinWidth="100"
                                meta:resourceKey="columCambioDollarUS"
                                ID="colCambioDollarUS"
                                Flex="1" />
                            <ext:NumberColumn runat="server"
                                DataIndex="EuroChange"
                                Format="0.000,0000/i"
                                MinWidth="100"
                                meta:resourceKey="columCambioEuro"
                                ID="colCambioEuro"
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
                            OverflowHandler="Scroller"
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
                <%--<ext:GridPanel
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
                                    Handler="ExportarDatos('Monedas', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:DateColumn runat="server"
                                DataIndex="FechaCambio"
                                Width="150"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columnFechaCambio"
                                ID="colFechaCambio"
                                Flex="1" />
                            <ext:NumberColumn runat="server"
                                DataIndex="CambioDollarUS"
                                Width="200"
                                Format="0.000,0000/i"
                                meta:resourceKey="columCambioDollarUS"
                                ID="colCambioDollarUSDetalle"
                                Flex="1" />
                            <ext:NumberColumn runat="server"
                                DataIndex="CambioEuro"
                                Width="200"
                                Format="0.000,0000/i"
                                meta:resourceKey="columCambioEuro"
                                ID="colCambioEuroDetalle"
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
                </ext:GridPanel>--%>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
