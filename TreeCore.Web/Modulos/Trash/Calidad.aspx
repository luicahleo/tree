<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calidad.aspx.cs" Inherits="TreeCore.ModGlobal.Calidad" %>

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
            <ext:Hidden ID="hdOperadores" runat="server" />

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
                ID="storeColumnas" 
                runat="server" 
                AutoLoad="false" 
                OnReadData="storeColumnas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="ColumnaTablaID" 
                        runat="server">
                        <Fields>
                            <ext:ModelField 
                                Name="ColumnaTablaID" 
                                Type="Int" />
                            <ext:ModelField
                                Name="ColumnaNombre" 
                                Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store 
                ID="storeTipoDato"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTipoDato_Refresh"
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
                        IDProperty="CalidadID">
                        <Fields>
                            <ext:ModelField Name="CalidadID" Type="Int" />
                            <ext:ModelField Name="NombreCampo" />
                            <ext:ModelField Name="Operador" />
                            <ext:ModelField Name="Valor" />
                            <ext:ModelField Name="FechaCondicion" Type="Date" />
                            <ext:ModelField Name="Descripcion" />
                            <ext:ModelField Name="Activa" Type="Boolean" />
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="TipoDato" Type="String" />
                            <ext:ModelField Name="ModuloID" Type="Int" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="NombreCampo"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store 
                ID="storeOperador"
                runat="server" 
                AutoLoad="false" 
                RemoteSort="false" 
                OnReadData="storeOperadores_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>

                    <ext:Model 
                        IDProperty="valor"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="valor" Type="Int" />
                            <ext:ModelField Name="nombre" Type="string" />
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
                        Border="false">
                        <Items>
                            <ext:TextField 
                                ID="txtDescripcion"
                                runat="server" 
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                Text=""
                                MaxLength="200" 
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Width="320" 
                                meta:resourceKey="txtDescripcion" />
                            <ext:ComboBox 
                                meta:resourceKey="cmbColumnas" 
                                runat="server" 
                                ID="cmbColumnas" 
                                StoreID="storeColumnas"
                                Mode="local"
                                ValueField="ColumnaTablaID"
                                DisplayField="ColumnaNombre" 
                                LabelAlign="Left"
                                FieldLabel="<%$ Resources:Comun, strColumna %>" 
                                EmptyText="<%$ Resources:Comun, strSeleccioneColumna %>" 
                                AllowBlank="false"
                                Width="320" 
                                Editable="true"
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Handler="RecargarColumnas();" />
                                    <Select Fn="SeleccionaColumnas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"/>
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox 
                                meta:resourceKey="cmbTipoDato"
                                runat="server" 
                                ID="cmbTipoDato" 
                                StoreID="storeTipoDato"
                                Mode="local"
                                ValueField="TipoDatoID" 
                                DisplayField="TipoDato" 
                                LabelAlign="Left"
                                FieldLabel="<%$ Resources:Comun, strTipoDato %>" 
                                EmptyText="<%$ Resources:Comun, strSeleccioneTipoDato %>"
                                AllowBlank="false" 
                                Width="320"
                                Editable="true" 
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Handler="RecargarTiposDatos();" />
                                    <Select Fn="SeleccionaTiposDatos" />
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
                                runat="server" 
                                ID="cmbOperador"
                                StoreID="storeOperador"
                                FieldLabel="<%$ Resources:Comun, strOperador %>" 
                                ValueField="valor" 
                                DisplayField="nombre" 
                                Mode="Local" 
                                AllowBlank="false"
                                EmptyText="<%$ Resources:Comun, strSeleccioneOperador %>" 
                                Editable="false" 
                                Width="320"
                                meta:resourceKey="cmbOperador">
                                <Listeners>
                                    <TriggerClick Handler="RecargarOperador();" />
                                    <Select Fn="SeleccionaOperador" />
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
                                ID="txtValor" 
                                runat="server" 
                                FieldLabel="<%$ Resources:Comun, strValorAlfanumerico %>" 
                                Text="" 
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" 
                                Width="320" 
                                meta:resourceKey="txtValor" />
                            <ext:RadioGroup
                                ID="radValor" 
                                runat="server" 
                                FieldLabel="<%$ Resources:Comun, strValorBoleano %>" 
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                AllowBlank="true" 
                                meta:resourceKey="radValor">
                                <Items>
                                    <ext:Radio 
                                        ID="radSi" 
                                        runat="server"
                                        BoxLabel="<%$ Resources:Comun, strYes %>"
                                        meta:resourceKey="radSi" />
                                    <ext:Radio
                                        ID="radNo" 
                                        runat="server" 
                                        BoxLabel="<%$ Resources:Comun, strNo %>" 
                                        Checked="true" 
                                        meta:resourceKey="radNo" />
                                </Items>
                            </ext:RadioGroup>
                            <ext:NumberField 
                                ID="numValor"
                                runat="server" 
                                FieldLabel="<%$ Resources:Comun, strValorNumerico %>"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                AllowBlank="true" 
                                meta:resourceKey="numValor" 
                                Width="320" />
                            <ext:DateField
                                ID="dateFecha" 
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strFecha %>"
                                AllowBlank="true"
                                Width="320" 
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Editable="true"
                                meta:resourceKey="dateFecha">
                            </ext:DateField>
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
                                        Cls="btn-Activar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button 
                                        runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btn-Eliminar"
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
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('Calidad', hdCliID.value, #{grid}, '');" />
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
                        <ColumnModel 
                            runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
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
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column 
                                    DataIndex="Descripcion" 
                                    Width="200"
                                    meta:resourceKey="columDescripcion"
                                    ID="colDescripcion"
                                    Text="<%$ Resources:Comun, strDescripcion %>"
                                    runat="server" />
                                <ext:Column 
                                    DataIndex="NombreCampo" 
                                    Width="200" 
                                    meta:resourceKey="columNombreCampo" 
                                    ID="colNombreCampo" 
                                    runat="server"
                                    Text="<%$ Resources:Comun, strDato %>"/>
                                <ext:Column 
                                    DataIndex="Operador" 
                                    Width="200" 
                                    meta:resourceKey="columOperador"
                                    ID="colOperador" 
                                    runat="server"
                                    Text="<%$ Resources:Comun, strOperador %>" />
                                <ext:Column 
                                    DataIndex="Valor" 
                                    Width="200" 
                                    meta:resourceKey="columValor"
                                    ID="colValor"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strValor %>"/>
                                <ext:Column 
                                    DataIndex="TipoDato" 
                                    Width="200" 
                                    meta:resourceKey="columTipoDato"
                                    Text="<%$ Resources:Comun, strTipoDato %>"
                                    ID="colTipoDato"
                                    runat="server" />
                                <ext:DateColumn
                                    DataIndex="FechaCondicion"
                                    Width="150"
                                    Align="Center" 
                                    meta:resourceKey="columFechaCondicion" 
                                    Format="<%$ Resources:Comun, FormatFecha %>" 
                                    ID="colFechaCondicion" 
                                    runat="server"
                                    Text="<%$ Resources:Comun, strFechaCondicion %>"
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
