<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfraestructurasSubServicios.aspx.cs" Inherits="TreeCore.ModGlobal.InfraestructurasSubServicios" %>

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

            <ext:Hidden id="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:Resourcemanager runat="server" 
                ID="ResourceManagerTreeCore" 
                DirectMethodNamespace="TreeCore">
            </ext:Resourcemanager>

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
                    <ext:Model runat="server" IDProperty="GlobalSubElementoMarcoID">
                        <Fields>
                            <ext:ModelField Name="GlobalSubElementoMarcoID" Type="Int" />
                            <ext:ModelField Name="GlobalSubElemento" />
                            <ext:ModelField Name="FechaCreacion" Type="Date" />
                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="TipoDato" />
                            <ext:ModelField Name="EsCantidad" Type="Boolean" />
                            <ext:ModelField Name="TieneRedondeo" Type="Boolean" />
                            <ext:ModelField Name="Cantidad" Type="float" />
                            <ext:ModelField Name="Unidad" />
                            <ext:ModelField Name="Valor" Type="float" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="FactorComuna" Type="float" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="GlobalSubElemento" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTipoDato"  
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
                            <ext:ModelField Name="TipoDato" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeMoneda"  
                AutoLoad="false" 
                OnReadData="storeMoneda_Refresh"
                Remotesort="false">
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
                                ID="txtSubElementos"  
                                meta:resourceKey="txtSubElementos"
                                FieldLabel="SubElemento" 
                                MaxLength="250"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                                AllowBlank="false" 
                                Width="350"
                                ValidationGroup="FORM" 
                                CausesValidation="true" />
                            <ext:NumberField runat="server"
                                ID="numCantidad"  
                                FieldLabel="Cantidad"  
                                AllowBlank="false"
                                ValidationGroup="FORM" 
                                CausesValidation="true" 
                                meta:resourcekey="numCantidad" 
                                MinValue="0"
                                Width="350" />
                            <ext:TextField runat="server"
                                ID="txtUnidad"  
                                FieldLabel="Unidad"  
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                                AllowBlank="false" 
                                Width="350"
                                ValidationGroup="FORM" 
                                CausesValidation="true" 
                                meta:resourceKey="txtUnidad" />
                            <ext:NumberField runat="server"
                                ID="NumValor"  
                                FieldLabel="<%$ Resources:Comun, strValor %>" 
                                AllowBlank="false"
                                ValidationGroup="FORM" 
                                CausesValidation="true" 
                                MinValue="0"
                                Width="350" />
                            <ext:NumberField runat="server"
                                ID="NumFactorComuna"  
                                FieldLabel="Factor Comuna"  
                                AllowBlank="false"
                                ValidationGroup="FORM" 
                                CausesValidation="true" 
                                meta:resourcekey="NumFactorComuna" 
                                MinValue="0"
                                Width="350" />
                            <ext:ComboBox runat="server"
                                ID="cmbMoneda" 
                                StoreID="storeMoneda"
                                Mode="Local" 
                                DisplayField="Moneda" 
                                ValueField="MonedaID" 
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>" 
                                Width="350"
                                Editable="true" 
                                FieldLabel="<%$ Resources:Comun, strMoneda %>" 
                                AllowBlank="false" 
                                QueryMode="Local">
                                <Listeners>
                                    <Select Fn="SeleccionarMoneda" />
                                    <TriggerClick Fn="RecargarMoneda" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger IconCls="ico-reload" 
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox runat="server"
                                ID="cmbTipoDato"  
                                StoreID="storeTipoDato"
                                Mode="Local" 
                                DisplayField="TipoDato" 
                                ValueField="TipoDatoID" 
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true" 
                                FieldLabel="<%$ Resources:Comun, strTipoDato %>" 
                                AllowBlank="false" 
                                QueryMode="Local" 
                                Width="350">
                                <Listeners>
                                    <Select Fn="SeleccionarTipoDato" />
                                    <TriggerClick Fn="RecargarTipoDato" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger IconCls="ico-reload" 
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Hidden="true" 
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:Checkbox runat="server"
                                ID="chkEsCantidad"
                                meta:resourceKey="ckEsCantidad"
                                FieldLabel="Es Cantidad" 
                                AllowBlank="false"
                                ValidationGroup="FORM" 
                                CausesValidation="true" />
                            <ext:Checkbox runat="server"
                                ID="chkTieneRedondeo"
                                meta:resourceKey="ckTieneRedondeo"
                                FieldLabel="Tiene Redondeo" 
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

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:viewport runat="server"
                id="vwContenedor"
                cls="vwContenedor"
                layout="Anchor">
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
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('InfraestructurasSubServicios', hdCliID.value, #{grid}, '');" />
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
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Align="Center"
                                    Cls="col-default"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colGlobalSubElemento"
                                    meta:resourceKey="colGlobalSubElemento"
                                    DataIndex="GlobalSubElemento" 
                                    Text="SubElemento" 
                                    Width="250"
                                    Flex="1"/>
                                <ext:Column runat="server"
                                    ID="colCantidad"
                                    meta:resourceKey="colCantidad"
                                    DataIndex="Cantidad" 
                                    Text="Cantidad" 
                                    Width="100"
                                    Flex="1"
                                    Align="Center" />
                                <ext:Column runat="server"
                                    ID="colUnidad"
                                    meta:resourceKey="colUnidad"
                                    DataIndex="Unidad" 
                                    Text="Unidad" 
                                    Width="100"
                                    Flex="1"
                                    Align="Center" />
                                <ext:Column runat="server"
                                    ID="colValor"
                                    DataIndex="Valor" 
                                    Text="<%$ Resources:Comun, strValor %>" 
                                    Width="100"
                                    Flex="1"
                                    Align="Center" />
                                <ext:Column runat="server"
                                    ID="colFactorComuna"
                                    meta:resourceKey="colFactorComuna"
                                    DataIndex="FactorComuna" 
                                    Text="Factor Comuna" 
                                    Width="100"
                                    Flex="1"
                                    Align="Center" />
                                <ext:Column runat="server"
                                    ID="colMoneda"
                                    DataIndex="Moneda" 
                                    Text="<%$ Resources:Comun, strMoneda %>" 
                                    Width="100" 
                                    Flex="1"
                                    Align="Center"    />
                                <ext:DateColumn runat="server"
                                    ID="colFechaCreacion"
                                    DataIndex="FechaCreacion" 
                                    Text="<%$ Resources:Comun, strFechaCreacion %>" 
                                    Width="140"
                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                    Flex="1"
                                    Align="Center" >
                                </ext:DateColumn>
                                <ext:DateColumn runat="server"
                                    ID="colFechaModificacion"
                                    DataIndex="FechaModificacion" 
                                    Text="<%$ Resources:Comun, strFechaModificacion %>" 
                                    Width="140"
                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                    Flex="1"
                                    Align="Center" >
                                </ext:DateColumn>
                                <ext:Column runat="server"
                                    ID="colNombreCompleto"
                                    DataIndex="NombreCompleto" 
                                    Text="<%$ Resources:Comun, strNombreCompleto %>" 
                                    Width="250"
                                    Flex="1"/>
                                <ext:Column runat="server"
                                    ID="colTipoDato"
                                    DataIndex="TipoDato"
                                    Flex="1"
                                    Text="<%$ Resources:Comun, strTipoDato %>" 
                                    Width="100" />
                                <ext:Column runat="server"
                                    ID="colEsCantidad"
                                    meta:resourceKey="colEsCantidad"
                                    DataIndex="EsCantidad" 
                                    Text="Es Cantidad"
                                    Flex="1"
                                    Width="100" 
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colTieneRedondeo"
                                    meta:resourceKey="colTieneRedondeo"
                                    DataIndex="TieneRedondeo" 
                                    Text="Tiene Redondeo"
                                    Flex="1"
                                    Width="100" 
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
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
            </ext:viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
