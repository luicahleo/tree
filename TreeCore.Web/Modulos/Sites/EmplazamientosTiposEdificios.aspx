<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosTiposEdificios.aspx.cs" Inherits="TreeCore.ModGlobal.EmplazamientosTiposEdificios" %>

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

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

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
                    <ext:Model runat="server" IDProperty="EmplazamientoTipoEdificioID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEdificioID" Type="Int" />
                            <ext:ModelField Name="TipoEdificio" />
                            <ext:ModelField Name="CostoPorDesmantelamiento" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="TipoEdificio" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePaises"
                AutoLoad="true"
                OnReadData="storePaises_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="PaisID" runat="server">
                        <Fields>
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="Pais" />
                        </Fields>
                    </ext:Model>
                </Model>
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

            <ext:Store runat="server"
                ID="storeTiposEdificiosPaises"
                AutoLoad="false"
                OnReadData="storeTiposEdificiosPaises_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EmplazamientoTipoEdificioPaisID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEdificioPaisID" Type="Int" />
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="CostoPorDesmantelamiento" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Anyo" />
                            <ext:ModelField Name="TasaDescuento" />
                            <ext:ModelField Name="TasaInflacion" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeAnualidad"
                AutoLoad="true"
                OnReadData="storeAnualidad_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="AnualidadID" runat="server">
                        <Fields>
                            <ext:ModelField Name="AnualidadID" Type="Int" />
                            <ext:ModelField Name="Anualidad" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposEstructuras"
                AutoLoad="false"
                OnReadData="storeTiposEstructuras_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EmplazamientoTipoEstructuraID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEstructuraID" Type="Int" />
                            <ext:ModelField Name="TipoEstructura" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store runat="server"
                ID="storeTiposEstructurasLibres"
                AutoLoad="false"
                OnReadData="storeTiposEstructurasLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EmplazamientoTipoEstructuraID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEstructuraID" Type="Int" />
                            <ext:ModelField Name="TipoEstructura" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtTipoEdificio"
                                FieldLabel="<%$ Resources:Comun, strTipoEdificio %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField runat="server"
                                ID="txtCostoDesmantelamiento"
                                meta:resourceKey="txtCosto"
                                FieldLabel="Costo de Desmantelamiento"
                                LabelAlign="Top"
                                WidthSpec="100%"
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
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window
                ID="winTiposEstructuras"
                runat="server"
                meta:resourceKey="winTiposEstructuras"
                Title="Tipos Estructuras"
                Width="500"
                Height="500"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposEstructuras"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposEstructuras"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar1"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir"
                                        Handler="BotonAgregarTipoEstructura();" />
                                    <ext:Button runat="server"
                                        ID="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar"
                                        Disabled="true"
                                        Handler="BotonEliminarTipoEstructura();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescarTipo"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="BotonRefrescarTipoEstructura();" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="columnModelProyectoTipo" runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="TipoEstructura"
                                    Text="<%$ Resources:Comun, strTipoEstructura %>"
                                    Width="350"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="GridRowSelectTiposEstructuras"
                                Mode="Multi"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectTiposEstructuras" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersTiposEstructuras"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <BottomBar>
                    <ext:PagingToolbar runat="server"
                        ID="PagingToolBar2"
                        PageSize="25"
                        HideRefresh="true"
                        StoreID="storeTiposEstructuras"
                        DisplayInfo="true">
                        <Plugins>
                            <ext:SlidingPager ID="SlidingPager1" runat="server" />
                        </Plugins>
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winTiposEstructuras}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winTiposEstructurasLibres"
                runat="server"
                meta:resourceKey="winTiposEstructuras"
                Title="Tipos Estructuras"
                Width="400"
                Height="400"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposEstructurasLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposEstructurasLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <ColumnModel ID="columnModelTiposEstructurasLibres" runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="TipoEstructura"
                                    Text="<%$ Resources:Comun, strTipoEstructura %>"
                                    Width="350"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="GridRowSelectTiposEstructurasLibre"
                                Mode="Multi"
                                SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridTiposEstructurasLibresSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersTiposEstructurasLibre"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarEdifLibres"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winTiposEstructurasLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardarTiposEstructurasLibre"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="BotonGuardarTiposEstructurasLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winTiposEstructurasLibres}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winTipoEdificiosPaises"
                runat="server"
                meta:resourceKey="winTipoEdificiosPaises"
                Title="Asignar Coste Desmantelamiento"
                Width="800"
                Height="500"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="GridTipoEdificiosPaises"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposEdificiosPaises"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar2"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregarCoste"
                                        Cls="btnAnadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="BotonAgregarCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnEditarCoste"
                                        Cls="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        Handler="BotonEditarCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnActivarDesactivarCoste"
                                        Cls="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Handler="BotonActivarDesactivarCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnQuitarCoste"
                                        Cls="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Handler="BotonQuitarCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnPorDefecto"
                                        Cls="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        Handler="BotonPorDefectoCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescarCoste"
                                        Cls="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Handler="BotonRefrescarCoste();" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="columnModel1" runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="Activo"
                                    Width="50"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Align="Center">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    DataIndex="Defecto"
                                    Width="50"
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Align="Center">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    DataIndex="Pais"
                                    Text="<%$ Resources:Comun, strPais %>"
                                    Hidden="false" />
                                <ext:Column runat="server"
                                    DataIndex="Moneda"
                                    meta:resourceKey="colMoneda"
                                    Text="<%$ Resources:Comun, strMoneda %>"
                                    Hidden="false" />
                                <ext:Column runat="server"
                                    DataIndex="CostoPorDesmantelamiento"
                                    meta:resourceKey="colCoste"
                                    Text="Coste Desmantelamiento"
                                    Hidden="false"
                                    Flex="1" />
                                <ext:Column runat="server"
                                    DataIndex="Anyo"
                                    meta:resourceKey="colAnyo"
                                    Text="<%$ Resources:Comun, strAnyo %>"
                                    Hidden="false" />
                                <ext:Column runat="server"
                                    DataIndex="TasaDescuento"
                                    meta:resourceKey="colTasaDescuento"
                                    Text="Tasa Descuento"
                                    Hidden="false" />
                                <ext:Column runat="server"
                                    DataIndex="TasaInflacion"
                                    meta:resourceKey="colTasaInflacion"
                                    Text="Tasa Inflacion"
                                    Hidden="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="TipoEdificiosPaisesRowSelection"
                                SingleSelect="true"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridTipoEdificiosPaises_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersEdificiosPaises"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <BottomBar>
                    <ext:PagingToolbar runat="server"
                        ID="PagingToolBar1"
                        PageSize="25"
                        HideRefresh="true"
                        StoreID="storeTiposEdificiosPaises"
                        DisplayInfo="true">
                        <Plugins>
                            <ext:SlidingPager ID="SlidingPager2" runat="server" />
                        </Plugins>
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winTiposEstructuras}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window runat="server"
                ID="winCostesDesmantelamiento"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="500"
                BodyPaddingSummary="10 32"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formCostesDesmantelamiento"
                        Cls="formGris ctForm-resp ctForm-resp-col2"
                        Border="false">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbPais"
                                FieldLabel="<%$ Resources:Comun, strPais %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Mode="Local"
                                DisplayField="Pais"
                                ValueField="PaisID"
                                StoreID="storePaises"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true"
                                QueryMode="Local"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarPais" />
                                    <TriggerClick Fn="RecargarPais" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox runat="server"
                                ID="cmbMoneda"
                                FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Mode="Local"
                                DisplayField="Moneda"
                                ValueField="MonedaID"
                                QueryMode="Local"
                                StoreID="storeMonedas"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarMoneda" />
                                    <TriggerClick Fn="RecargarMoneda" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField runat="server"
                                ID="txtCoste"
                                meta:resourceKey="txtCoste"
                                FieldLabel="Coste"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:ComboBox runat="server"
                                ID="cmbAnualidad"
                                Mode="Local"
                                FieldLabel="<%$ Resources:Comun, strAnyo %>"
                                DisplayField="Anualidad"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                ValueField="AnualidadID"
                                QueryMode="Local"
                                StoreID="storeAnualidad"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="false"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarAnualidad" />
                                    <TriggerClick Fn="RecargarAnualidad" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField runat="server"
                                ID="txtTasaInflacion"
                                meta:resourceKey="txtTasaInflacion"
                                FieldLabel="Inflacion(%)"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtTasaDescuento"
                                meta:resourceKey="txtTasaDescuento"
                                FieldLabel="Tasa Descuento(%)"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoCosteDesmantelamiento(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarCostes"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winCostesDesmantelamiento}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarCostes"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winCostesBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

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
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAnadir"
                                        Cls="btnAnadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
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
                                        ID="btnCosteDesmantelamiento"
                                        ToolTip="Asignar Coste Desmantelamiento"
                                        meta:resourceKey="btnCosteDesmantelamiento"
                                        Cls="btnCoste"
                                        Handler="BotonAsignarCoste();" />
                                    <ext:Button runat="server"
                                        ID="btnTiposEstructuras"
                                        meta:resourceKey="btnTiposEstructuras"
                                        Cls="btnEstructuras"
                                        Hidden="true"
                                        Handler="BotonTiposEstructuras();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('EmplazamientosTiposEdificios', hdCliID.value, #{grid}, '');" />
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
                        <ColumnModel runat="server">
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
                                <ext:Column runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Cls="col-default"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colTipoEdificio"
                                    DataIndex="TipoEdificio"
                                    Text="<%$ Resources:Comun, strTipoEdificio %>"
                                    Width="150"
                                    Flex="1" />
                                <ext:NumberColumn runat="server"
                                    ID="colCostoPorDesmantelamiento"
                                    meta:resourceKey="columCostoPorDesmantelamiento"
                                    Hidden="true"
                                    DataIndex="CostoPorDesmantelamiento"
                                    Width="150"
                                    Align="Center"
                                    Format="0.00">
                                </ext:NumberColumn>
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
                                ID="PagingToolBar"
                                meta:resourceKey="PagingToolBar"
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
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
