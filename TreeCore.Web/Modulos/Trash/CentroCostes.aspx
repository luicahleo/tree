<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CentroCostes.aspx.cs" Inherits="TreeCore.ModGlobal.CentroCostes" %>

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
                    <ext:Model runat="server" 
                        IDProperty="ClienteID">
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
                    <ext:Model runat="server" IDProperty="CentroCosteID">
                        <Fields>
                            <ext:ModelField Name="CentroCosteID" Type="Int" />
                            <ext:ModelField Name="CentroCoste" />
                            <ext:ModelField Name="Descripcion" />
                            <ext:ModelField Name="Responsable" />
                            <ext:ModelField Name="SociedadID" Type="Int" />
                            <ext:ModelField Name="Sociedad" />
                            <ext:ModelField Name="NIF" />
                            <ext:ModelField Name="CodigoSociedad" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="Simbolo" />
                            <ext:ModelField Name="Activa" Type="Boolean" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="CentroCoste" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" 
                ID="storeSociedades"  
                AutoLoad="true" 
                OnReadData="storeSociedades_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="SociedadID" runat="server">
                        <Fields>
                            <ext:ModelField Name="SociedadID" Type="Int" />
                            <ext:ModelField Name="Sociedad" />
                            <ext:ModelField Name="Activa" Type="Boolean" />
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
                                ID="txtCentroCoste" 
                                meta:resourceKey="txtCentroCoste" 
                                FieldLabel="Centro Coste" 
                                MaxLength="200" 
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                                AllowBlank="false"
                                ValidationGroup="FORM" 
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtDescripcion" 
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>" 
                                MaxLength="400" 
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                                AllowBlank="true"
                                ValidationGroup="FORM" 
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtResponsable" 
                                meta:resourceKey="txtResponsable" 
                                FieldLabel="Responsable" 
                                MaxLength="200" 
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>" 
                                AllowBlank="true"
                                ValidationGroup="FORM" 
                                CausesValidation="false" />
                            <ext:ComboBox runat="server"
                                ID="cmbSociedad"
                                meta:resourceKey="cmbSociedad"   
                                StoreID="storeSociedades" 
                                QueryMode="Local"
                                ValueField="SociedadID" 
                                DisplayField="Sociedad" 
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strSociedad %>" 
                                Editable="true" 
                                ForceSelection="false" >
                                <Listeners>
                                    <Select Fn="SeleccionarSociedad" />
                                    <TriggerClick Fn="RecargarSociedad" />
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
                                        AriaLabel="Añadir"
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
                                        Handler="ExportarDatos('CentroCostes', hdCliID.value, #{grid}, '');" />
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
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Cls="col-default"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colCentroCoste"
                                    meta:resourceKey="columCentroCoste"
                                    DataIndex="CentroCoste" 
                                    Width="200"  
                                    Text="Centro de Costes" />
                                <ext:Column runat="server"
                                    ID="colDescripcion"
                                    DataIndex="Descripcion" 
                                    Width="300"  
                                    Text="<%$ Resources:Comun, strDescripcion %>" />
                                <ext:Column runat="server"
                                    ID="colResponsable"
                                    meta:resourceKey="columResponsable"
                                    DataIndex="Responsable" 
                                    Width="200"  
                                    Text="Responsable" />
                                <ext:Column runat="server"
                                    ID="colSociedad"
                                    DataIndex="Sociedad"
                                    Width="150"  
                                    Text="<%$ Resources:Comun, strSociedad %>" />
                                <ext:Column runat="server"
                                    ID="colCodigoSociedad"
                                    DataIndex="CodigoSociedad" 
                                    Width="100"  
                                    Text="<%$ Resources:Comun, strCodigo %>" />
                                <ext:Column runat="server"
                                    ID="colNIF"
                                    meta:resourceKey="columNIF"
                                    DataIndex="NIF" 
                                    Width="100"  
                                    Text="NIF" />
                                <ext:Column runat="server"
                                    ID="colMoneda"
                                    DataIndex="Moneda" 
                                    Width="100" 
                                    Align="Center"  
                                    Text="<%$ Resources:Comun, strMoneda %>" />
                                <ext:Column runat="server"
                                    ID="colSimbolo"
                                    DataIndex="Simbolo" 
                                    Width="50" 
                                    Align="Center"  
                                    Text="<%$ Resources:Comun, strSimbolo %>"
                                    Flex="1"/>
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
