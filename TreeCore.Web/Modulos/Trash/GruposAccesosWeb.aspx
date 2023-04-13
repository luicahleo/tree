<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GruposAccesosWeb.aspx.cs" Inherits="TreeCore.ModGlobal.GruposAccesosWeb" %>

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
                    <ext:Model runat="server" IDProperty="GrupoAccesoWebID">
                        <Fields>
                            <ext:ModelField Name="GrupoAccesoWebID" Type="Int" />
                            <ext:ModelField Name="GrupoAcceso" />
                            <ext:ModelField Name="URL" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="GrupoAcceso" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeUsuarios" runat="server" AutoLoad="false" OnReadData="storeUsuarios_Refresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="UsuarioID" runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="GrupoAccesoWebID" Type="Int" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="GrupoAcceso" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreCompleto" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store ID="storeUsuariosLibres" runat="server" AutoLoad="false" OnReadData="storeUsuariosLibres_Refresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="UsuarioID" runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="NombreCompleto" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreCompleto" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
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

                            <ext:TextField ID="txtGrupoAcceso"
                                runat="server"
                                FieldLabel="Grupo Acceso"
                                Text=""
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtGrupoAcceso" />
                            <ext:TextField ID="txtURL"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strURL %>"
                                Text=""
                                MaxLength="500"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtURL" />
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
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>

                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>


            <%--VENTANAS USUARIOS --%>

            <ext:Window
                ID="winUsuarios"
                runat="server"
                Title="<%$ Resources:Comun, strUsuariosGrupo %>"
                Width="500"
                Height="500"
                Resizable="false"
                Modal="true" ShowOnLoad="true" Hidden="true"
                meta:resourceKey="winUsuarios"
                Scrollable="Vertical">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="GridUsuarios"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeUsuarios"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main"
                        Scrollable="Disabled">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar1"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregarUsuario"
                                        meta:resourceKey="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir"
                                        Handler="BotonAgregarUsuario();" />

                                    <ext:Button runat="server"
                                        ID="btnEliminarUsuario"
                                        meta:resourceKey="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, strQuitar %>"
                                        Cls="btnEliminar"
                                        Disabled="true"
                                        Handler="BotonEliminarUsuario();" />

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="coluModUsuario" runat="server">
                            <Columns>
                                <ext:Column DataIndex="NombreCompleto"
                                    ID="colUsuario"
                                    Text="<%$ Resources:Comun, strUsuario %>"
                                    Width="350"
                                    meta:resourceKey="colUsuario"
                                    runat="server"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModelUsuario" runat="server" Mode="Multi" EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectUsuario" />
                                    <Deselect Fn="DeseleccionarGrillaUsuario" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersUsuarios"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters"
                                Visible="true" />
                        </Plugins>
                    </ext:GridPanel>

                </Items>
                <BottomBar>
                    <ext:PagingToolbar
                        runat="server"
                        ID="PagingToolBar1"
                        meta:resourceKey="PagingToolBar"
                        StoreID="storeUsuarios"
                        DisplayInfo="true"
                        HideRefresh="false">
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winUsuarios}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winUsuariosLibres"
                runat="server"
                Title="<%$ Resources:Comun, strUsuarios %>"
                Width="400"
                Height="400"
                Resizable="false"
                Modal="true" ShowOnLoad="true" Hidden="true"
                meta:resourceKey="winUsuariosLibres"
                Scrollable="Vertical">

                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="GridPanelUsuariosLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeUsuariosLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main"
                        Scrollable="Disabled">

                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column DataIndex="NombreCompleto"
                                    ID="colUsuarioLibre"
                                    Text="<%$ Resources:Comun, strUsuario %>"
                                    Width="150"
                                    runat="server"
                                    Flex="1"
                                    meta:resourceKey="colUsuarioLibre" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModelUsuariosLibres"
                                runat="server"
                                Mode="Multi"
                                SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectUsuariosLibres" />
                                    <Deselect Fn="DeseleccionarGrillaUsuariosLibres" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersUsuariosLibres"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters"
                                Visible="true" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancelarUsuariosLibres"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel"
                        meta:resourceKey="btnCancelar">
                        <Listeners>
                            <Click Handler="#{winUsuariosLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnAgregarUsuariosLibres"
                        meta:resourceKey="btnAceptar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="BotonAgregarUsuariosGrupo();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winUsuariosLibres}.center();" />
                </Listeners>
            </ext:Window>


            <%--FIN VENTANA USUARIO --%>


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
                                        Handler="ExportarDatos('GruposAccesosWeb', hdCliID.value, #{grid}, '');" />
                                    <ext:Button runat="server"
                                        ID="btnUsuarios"
                                        ToolTip="<%$ Resources:Comun, strUsuariosGrupo %>"
                                        meta:resourceKey="btnUsuarios"
                                        Cls="btnUsuarios"
                                        Handler="BotonGestionUsuarios();" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
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
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
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
                                    meta:resourceKey="colActivo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    Align="Center"
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    meta:resourceKey="colDefecto"
                                    Width="50"
                                    Hidden="false">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>

                                <ext:Column DataIndex="GrupoAcceso"
                                    Text="Grupo Acceso"
                                    Width="200"
                                    meta:resourceKey="colGrupoAcceso"
                                    ID="colGrupoAcceso"
                                    runat="server" />
                                <ext:Column DataIndex="URL"
                                    Text="<%$ Resources:Comun, strURL %>"
                                    Width="450"
                                    meta:resourceKey="colURL"
                                    ID="colURL"
                                    runat="server"
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
