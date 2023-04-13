<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paises.aspx.cs" Inherits="TreeCore.ModGlobal.Paises" %>

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

            <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
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
                    <ext:Model runat="server" IDProperty="ClienteID">
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
                    <ext:Model runat="server" IDProperty="PaisID">
                        <Fields>
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="Pais_En" />
                            <ext:ModelField Name="Pais_Fr" />
                            <ext:ModelField Name="PaisCod" />
                            <ext:ModelField Name="RegionID" Type="Int" />
                            <ext:ModelField Name="Region" />
                            <ext:ModelField Name="Zoom" Type="Int" />
                            <ext:ModelField Name="Latitud" Type="Float" />
                            <ext:ModelField Name="Longitud" Type="Float" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="ActivoPais" Type="Boolean" />
                            <ext:ModelField Name="Prefijo" />
                            <ext:ModelField Name="Icono" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="Pais"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeRegiones"
                runat="server"
                AutoLoad="false"
                OnReadData="storeRegiones_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="RegionID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="RegionID" Type="Int" />
                            <ext:ModelField Name="Region" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeIconos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeIconos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ImagenID">
                        <Fields>
                            <ext:ModelField Name="IconCls" />
                            <ext:ModelField Name="ImagenID" />
                            <ext:ModelField Name="Imagen" />
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
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Cls="ctForm-resp ctForm-resp-col2"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtNombre"
                                FieldLabel="<%$ Resources:Comun, strPais %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Text=""
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Width="350" />
                            <ext:TextField runat="server"
                                ID="txtCodigo"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Text=""
                                MaxLength="3"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Width="350" />

                            <ext:TextField ID="txtPrefijo"
                                runat="server"
                                MaxLength="10"
                                FieldLabel="<%$ Resources:Comun, strPrefijo %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Text=""
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtPrefijo"
                                Regex="^(\+[0-9]{1,3})(\-[0-9]{1,3}){0,1}$"
                                RegexText="<%$ Resources:Comun, strRegexPrefijo %>" />

                            <ext:ComboBox
                                ID="cmbIconos"
                                runat="server"
                                meta:resourceKey="cmbIconos"
                                StoreID="storeIconos"
                                QueryMode="Local"
                                DisplayField="Imagen"
                                ValueField="Imagen"
                                Editable="true"
                                ForceSelection="false"
                                FieldLabel="<%$ Resources:Comun, strIcono %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="false">
                                <ListConfig>
                                    <ItemTpl runat="server">
                                        <Html>
                                            <div class="x-combo-list-item icon-combo-item {IconCls}" style="padding-left: 20px;">
                                                <span>{Imagen}</span>                                                                
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
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
                            <ext:ComboBox
                                ID="cmbRegion"
                                runat="server"
                                meta:resourceKey="cmbRegion"
                                StoreID="storeRegiones"
                                QueryMode="Local"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                DisplayField="Region"
                                ValueField="RegionID"
                                Editable="true"
                                ForceSelection="false"
                                FieldLabel="<%$ Resources:Comun, strRegion %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarRegion" />
                                    <TriggerClick Fn="RecargarRegion" />
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

                            <ext:NumberField ID="numbLatitud"
                                runat="server"
                                MaxLength="100"
                                FieldLabel="<%$ Resources:Comun, strLatitud %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                DecimalSeparator=","
                                DecimalPrecision="6"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="numbLatitud" />

                            <ext:NumberField ID="numbLongitud"
                                runat="server"
                                MaxLength="100"
                                FieldLabel="<%$ Resources:Comun, strLongitud %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                DecimalSeparator=","
                                DecimalPrecision="6"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="numbLongitud" />

                            <ext:NumberField ID="numbZoom"
                                runat="server"
                                MaxLength="100"
                                FieldLabel="<%$ Resources:Comun, strZoom %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                Regex="^\d*$"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="numbZoom" />

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
                        Disabled="true"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport
                runat="server"
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
                            <ext:Toolbar
                                runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAnadir"
                                        meta:resourceKey="btnAnadir"
                                        Cls="btnAnadir"
                                        AriaLabel="Añadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="AgregarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        meta:resourceKey="btnEditar"
                                        Cls="btnEditar"
                                        Handler="MostrarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        meta:resourceKey="btnActivar"
                                        Cls="btn-Activar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btn-Eliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        meta:resourceKey="btnDefecto"
                                        Cls="btnDefecto"
                                        Handler="Defecto();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        meta:resourceKey="btnRefrescar"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('Paises', hdCliID.value, #{grid}, '');" />
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
                                    <ext:TextField
                                        ID="txtSearch"
                                        Cls="txtSearchC"
                                        runat="server"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        LabelWidth="50"
                                        Width="250"
                                        EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyUp Fn="FiltrarColumnas" Buffer="250" />
                                            <TriggerClick Fn="LimpiarFiltroBusqueda" />
                                        </Listeners>

                                    </ext:TextField>

                                    <ext:Button runat="server"
                                        Width="30"
                                        ID="btnClearFilters"
                                        Cls="btn-trans btnRemoveFilters"
                                        ToolTip="Remove Filters">
                                        <Listeners>
                                            <Click Handler="BorrarFiltros(#{grid});"></Click>
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="ActivoPais"
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
                                    DataIndex="Pais"
                                    ID="Pais" runat="server"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strPais %>"
                                    meta:resourceKey="columPais"
                                    Flex="1" />
                                <ext:Column
                                    DataIndex="PaisCod"
                                    ID="PaisCod" runat="server"
                                    Text="<%$ Resources:Comun, strPaisCod %>"
                                    Width="150"
                                    meta:resourceKey="columCodPais" />
                                <ext:Column
                                    DataIndex="Region"
                                    ID="Region" runat="server"
                                    Text="<%$ Resources:Comun, strRegion %>"
                                    Width="150"
                                    meta:resourceKey="columRegion" />
                                <ext:Column
                                    DataIndex="Prefijo"
                                    ID="colPrefijo" runat="server"
                                    Text="<%$ Resources:Comun, strPrefijo %>"
                                    Width="150"
                                    meta:resourceKey="columPrefijo" />
                                <ext:Column
                                    DataIndex="Icono"
                                    ID="colIcono" runat="server"
                                    Text="<%$ Resources:Comun, strIcono %>"
                                    Width="150"
                                    meta:resourceKey="colIcono">
                                    <Renderer Fn="renderIconoBandera" />
                                </ext:Column>
                                <%-- <ext:Column
                                    DataIndex="Latitud"
                                    ID="Column1" runat="server"
                                    Text="<%$ Resources:Comun, strLatitud %>"
                                    Width="150"
                                    meta:resourceKey="columLatitud" />
                                <ext:Column
                                    DataIndex="Longitud"
                                    ID="Column2" runat="server"
                                    Text="<%$ Resources:Comun, strLongitud %>"
                                    Width="150"
                                    meta:resourceKey="columLongitud" />
                                <ext:Column
                                    DataIndex="Zoom"
                                    ID="Column3" runat="server"
                                    Text="<%$ Resources:Comun, strZoom %>"
                                    Width="150"
                                    meta:resourceKey="columZoom" />--%>
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
