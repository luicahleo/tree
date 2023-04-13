<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonedasGlobales.aspx.cs" Inherits="TreeCore.ModGlobal.MonedasGlobales" %>

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
                <ext:Model runat="server" IDProperty="MonedaGlobalID">
                    <Fields>
                        <ext:ModelField Name="MonedaGlobalID" Type="Int" />
                        <ext:ModelField Name="MonedaID" Type="Int" />
                        <ext:ModelField Name="Moneda" />
                        <ext:ModelField Name="Simbolo" />
                        <ext:ModelField Name="CambioDolar" Type="Float" />
                        <ext:ModelField Name="CambioEuro" Type="Float" />
                        <ext:ModelField Name="FechaInicio" Type="Date" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Moneda" Direction="ASC" />
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
                <ext:Model runat="server" IDProperty="MonedaGlobalID">
                    <Fields>
                        <ext:ModelField Name="MonedaGlobalID" Type="Int" />
                        <ext:ModelField Name="MonedaID" Type="Int" />
                        <ext:ModelField Name="Moneda" />
                        <ext:ModelField Name="Simbolo" />
                        <ext:ModelField Name="CambioDolar" Type="Float" />
                        <ext:ModelField Name="CambioEuro" Type="Float" />
                        <ext:ModelField Name="FechaInicio" Type="Date" />
                        <ext:ModelField Name="FechaFin" Type="Date" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="FechaInicio" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server"
            ID="storeMonedas"  
            AutoLoad="true" 
            OnReadData="storeMonedas_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="MonedaID" runat="server">
                    <Fields>
                        <ext:ModelField Name="MonedaID" Type="Int" />
                        <ext:ModelField Name="Moneda" />
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
                        <ext:ComboBox runat="server"
                            ID="cmbMonedas"  
                            AllowBlank="false"
                            StoreID="storeMonedas" 
                            Mode="Local" 
                            DisplayField="Moneda" 
                            ValueField="MonedaID"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            FieldLabel="<%$ Resources:Comun, strMoneda %>"
                            Editable="true"
                            ForceSelection="true" 
                            QueryMode="Local" >
                            <Listeners>
                                <Select Fn="SeleccionarMoneda" />
                                <TriggerClick Fn="RecargarMoneda" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:DateField runat="server"
                            ID="txtFechaInicio"  
                            MaxLength="100" 
                            ValidationGroup="FORM"
                            Format="<%$ Resources:Comun, FormatFecha %>"
                            AllowBlank="true" 
                            CausesValidation="true"
                            meta:resourceKey="txtFechaInicio" />
                        <ext:NumberField runat="server"
                            ID="txtDolar"  
                            MaxLength="20" 
                            ValidationGroup="FORM"
                            AllowBlank="false" 
                            CausesValidation="true" 
                            DecimalPrecision="4" 
                            meta:resourceKey="txtDolar" />
                        <ext:NumberField runat="server"
                            ID="txtEuro"  
                            MaxLength="20" 
                            ValidationGroup="FORM"
                            AllowBlank="false" 
                            CausesValidation="true" 
                            DecimalPrecision="4" 
                            meta:resourceKey="txtEuro" />
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
                        <ext:NumberField runat="server"
                            ID="txtDolarDetalle"  
                            MaxLength="100" 
                            ValidationGroup="FORM"
                            AllowBlank="false" 
                            CausesValidation="true" 
                            meta:resourceKey="txtDolar" />
                        <ext:NumberField runat="server"
                            ID="txtEuroDetalle"  
                            MaxLength="50" 
                            AllowBlank="false"
                            ValidationGroup="FORM" 
                            CausesValidation="true" 
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
                                    ID="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('MonedasGlobales', hdCliID.value, #{gridMaestro}, '-1');" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colMoneda"
                                DataIndex="Moneda"
                                Text="<%$ Resources:Comun, strMoneda %>"
                                Width="200"
                                Flex="1"/>
                            <ext:Column runat="server"
                                ID="colSimbolo"
                                DataIndex="Simbolo"
                                Text="<%$ Resources:Comun, strSimbolo %>"
                                Width="100"
                                Flex="1"/>
                            <ext:DateColumn runat="server"
                                ID="colFechaInicio"
                                DataIndex="FechaInicio" 
                                Width="100" 
                                Format="<%$ Resources:Comun, FormatFecha %>" 
                                Flex="1"
                                meta:resourceKey="columFechaInicio" />
                            <ext:NumberColumn runat="server"
                                ID="colCambioDolar"
                                DataIndex="CambioDolar" 
                                Format="0.000,0000/i" 
                                Width="100" 
                                Flex="1"
                                meta:resourceKey="columCambioDollarUS" />
                            <ext:NumberColumn runat="server"
                                ID="colCambioEuro"
                                DataIndex="CambioEuro" 
                                Format="0.000,0000/i" 
                                Width="100" 
                                Flex="1"
                                meta:resourceKey="columCambioEuro" />
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
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"/>
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
                                    Handler="ExportarDatos('MonedasGlobales', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:DateColumn runat="server"
                                DataIndex="FechaInicio" 
                                ID="colFechaInicioDetalle"
                                Width="150" 
                                Format="<%$ Resources:Comun, FormatFecha %>" 
                                Flex="1"
                                meta:resourceKey="columFechaInicio" />
                            <ext:DateColumn runat="server"
                                DataIndex="FechaFin" 
                                ID="colFechaFin"
                                Width="150" 
                                Format="<%$ Resources:Comun, FormatFecha %>" 
                                Flex="1"
                                meta:resourceKey="columFechaFin" />
                            <ext:NumberColumn runat="server"
                                DataIndex="CambioDolar"
                                ID="colCambioDolarDetalle"
                                Width="200" 
                                Format="0.000,0000/i" 
                                Flex="1"
                                meta:resourceKey="columCambioDollarUS" />
                            <ext:NumberColumn runat="server"
                                DataIndex="CambioEuro" 
                                ID="colCambioEuroDetalle"
                                Width="200" 
                                Flex="1"
                                Format="0.000,0000/i" 
                                meta:resourceKey="columCambioEuro" />
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
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"/>
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
