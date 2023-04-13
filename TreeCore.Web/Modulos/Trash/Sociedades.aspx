<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sociedades.aspx.cs" Inherits="TreeCore.ModGlobal.Sociedades" %>

<%@ Register Src="/Componentes/Localizaciones.ascx" TagName="Localizaciones" TagPrefix="local" %>

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

            <ext:ResourceManager
                runat="server"
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

            <ext:Store 
                runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
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
                    <ext:Model runat="server" IDProperty="SociedadID">
                        <Fields>
                            <ext:ModelField Name="SociedadID" Type="Int" />
                            <ext:ModelField Name="Sociedad" />
                            <ext:ModelField Name="NIF" />
                            <ext:ModelField Name="Direccion" />
                            <ext:ModelField Name="CodigoPostal" />
                            <ext:ModelField Name="Municipio" />
                            <ext:ModelField Name="Provincia" />
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="Region" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                            <ext:ModelField Name="CodigoSociedad" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="Simbolo" />
                            <ext:ModelField Name="Activa" Type="Boolean" />
                            <ext:ModelField Name="SociedadCECO" Type="Boolean" />
                            <ext:ModelField Name="DefectoSociedad" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Sociedad" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeMonedas"
                AutoLoad="false"
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

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window 
                runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
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
                            <ext:FieldContainer runat="server" >
                                <Content>
                                    <ext:TextField runat="server"
                                        ID="txtSociedad"
                                        FieldLabel="<%$ Resources:Comun, strSociedad %>"
                                        MaxLength="150"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCodigoSociedad"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtNIF"
                                        meta:resourceKey="txtNIF"
                                        FieldLabel="NIF"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="false" />
                                    <ext:TextField runat="server"
                                        ID="txtDireccion"
                                        FieldLabel="<%$ Resources:Comun, strDireccion %>"
                                        MaxLength="250"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCodigoPostal"
                                        FieldLabel="<%$ Resources:Comun, strCodigoPostal %>"
                                        MaxLength="250"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <local:Localizaciones runat="server" 
                                        ID="locSociedades" 
                                        Requerido="false" 
                                        PaisDefecto="true" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbMoneda"
                                        StoreID="storeMonedas"
                                        Mode="local"
                                        ValueField="MonedaID"
                                        DisplayField="Moneda"
                                        LabelAlign="Left"
                                        AllowBlank="true"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                        Editable="true"
                                        ForceSelection="true"
                                        QueryMode="Local">
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
                                    <ext:Checkbox runat="server"
                                        meta:resourceKey="chkSocidedadCECO"
                                        ID="chkSocidedadCECO"
                                        FieldLabel="Sociedad CECO"
                                        Checked="true">
                                    </ext:Checkbox>
                                </Content>
                            </ext:FieldContainer>
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
                                        Handler="ExportarDatos('Sociedades', hdCliID.value, #{grid}, '');" />
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
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column 
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="Activa"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Cls="col-activo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column 
                                    runat="server"
                                    ID="colDefecto"
                                    DataIndex="DefectoSociedad"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Cls="col-default"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column 
                                    runat="server"
                                    ID="colCodigoSociedad"
                                    DataIndex="CodigoSociedad"
                                    Width="50"
                                    Text="<%$ Resources:Comun, strCodigo %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colSociedad"
                                    DataIndex="Sociedad"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strSociedad %>"
                                    Flex="1" />
                                <ext:Column 
                                    runat="server"
                                    meta:resourceKey="columNIF"
                                    ID="colNIF"
                                    Text="NIF"
                                    DataIndex="NIF"
                                    Width="100" />
                                <ext:Column 
                                    runat="server"
                                    ID="colDireccion"
                                    DataIndex="Direccion"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strDireccion %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colCodigoPostal"
                                    DataIndex="CodigoPostal"
                                    Width="100"
                                    Text="<%$ Resources:Comun, strCodigoPostal %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colPais"
                                    DataIndex="Pais"
                                    Width="100"
                                    Text="<%$ Resources:Comun, strPais %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colRegion"
                                    DataIndex="Region"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strRegion %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colProvincia"
                                    DataIndex="Provincia"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strProvincia %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colMunicipio"
                                    DataIndex="Municipio"
                                    Width="150"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, strMunicipio %>" />
                                <ext:Column
                                    runat="server"
                                    ID="colMoneda"
                                    DataIndex="Moneda"
                                    Width="100"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, strMoneda %>" />
                                <ext:Column 
                                    runat="server"
                                    ID="colSociedadCECO"
                                    DataIndex="SociedadCECO"
                                    Width="150"
                                    Align="Center"
                                    meta:resourceKey="columSociedadCECO"
                                    Text="Sociedad CECO">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
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
                            <ext:CellEditing runat="server"
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
