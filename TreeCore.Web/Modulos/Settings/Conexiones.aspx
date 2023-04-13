<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Conexiones.aspx.cs" Inherits="TreeCore.ModGlobal.Conexiones" %>


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
                    <ext:Model runat="server" IDProperty="ToolConexionID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="ToolConexionID" Type="Int" />
                            <ext:ModelField Name="Servidor" />
                            <ext:ModelField Name="Usuario" />
                            <ext:ModelField Name="Clave" />
                            <ext:ModelField Name="IntegracionID" />
                            <ext:ModelField Name="Integracion" />
                            <ext:ModelField Name="ActivaConexion" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="IntegracionID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeIntegracion"
                AutoLoad="false"
                OnReadData="storeIntegracion_Refresh"
                RemoteSort="false">
                <Listeners>
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="IntegracionID" runat="server">
                        <Fields>
                            <ext:ModelField Name="IntegracionID" Type="Int" />
                            <ext:ModelField Name="Integracion" Type="string" />
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
                            <ext:ComboBox
                                ID="cmbIntegracion"
                                runat="server"
                                FieldLabel="Integración"
                                Mode="Local"
                                DisplayField="Integracion"
                                ValueField="IntegracionID"
                                StoreID="storeIntegracion"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="false"
                                meta:resourceKey="cmbIntegracion"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarIntegracion" />
                                    <TriggerClick Fn="RecargarIntegracion" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField
                                ID="txtServidor"
                                runat="server"
                                FieldLabel="Servidor"
                                MaxLength="500"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtServidor" />
                            <ext:TextField
                                ID="txtUsuario"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strUsuario %>"
                                MaxLength="500"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:TextField>
                            <ext:TextField
                                ID="txtClave"
                                runat="server"
                                FieldLabel="Clave"
                                InputType="Password"
                                MaxLength="500"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                EnableKeyEvents="true"
                                meta:resourceKey="txtClave">
                            </ext:TextField>
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
                                        Handler="ExportarDatos('Conexiones', hdCliID.value, #{grid}, '');" />
                                    <ext:Button runat="server" 
                                        ID="btnEstructura" 
                                        Cls="btnDescargarTodo" 
                                        Hidden="true"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                                        <Listeners>
                                            <Click Handler="BotonEstructura();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox runat="server"
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
                                    DataIndex="ActivaConexion"
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
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colIntegracion"
                                    DataIndex="Integracion"
                                    Width="100"
                                    Header="Integración"
                                    meta:resourceKey="columIntegracion" />
                                <ext:Column runat="server"
                                    ID="colServidor"
                                    DataIndex="Servidor"
                                    Header="Servidor"
                                    Width="150"
                                    Flex="1"
                                    meta:resourceKey="columServidor" />
                                <ext:Column runat="server"
                                    ID="colUsuario"
                                    DataIndex="Usuario"
                                    Header="<%$ Resources:Comun, strRegion %>"
                                    Width="200" />
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
