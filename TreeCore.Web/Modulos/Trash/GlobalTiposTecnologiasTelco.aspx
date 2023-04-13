<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalTiposTecnologiasTelco.aspx.cs" Inherits="TreeCore.ModGlobal.GlobalTiposTecnologiasTelco" %>

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

            <ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientes_Refresh" RemoteSort="false">
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

            <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="false" OnReadData="storePrincipal_Refresh" RemoteSort="true" PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalTipoTecnologiaTelcoID">
                        <Fields>

                            <ext:ModelField Name="GlobalTipoTecnologiaTelcoID" Type="Int" />
                            <ext:ModelField Name="NombreTipoTecnologia" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />

                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreTipoTecnologia" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeFrecuenciasLibres" runat="server" AutoLoad="false" OnReadData="storeFrecuenciasLibres_Refresh"
                RemoteSort="true" RemotePaging="false" PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaFrecuenciasLibres" />
                </Listeners>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalFrecuenciaTelcoID">
                        <Fields>
                            <ext:ModelField Name="GlobalFrecuenciaTelcoID" Type="Int" />
                            <ext:ModelField Name="NombreFrecuenciaTelco" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreFrecuenciaTelco" Direction="ASC" />
                </Sorters>
                <Parameters>
                    <ext:StoreParameter Name="start" Value="0" Mode="Raw" />
                    <ext:StoreParameter Name="limit" Mode="Raw" Value="25" />
                </Parameters>
            </ext:Store>

            <ext:Store ID="storeFrecuenciasAsignadas" runat="server" AutoLoad="false" OnReadData="storeFrecuenciasAsignadas_Refresh"
                RemoteSort="true" RemotePaging="false" PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaFrecuenciasAsignadas" />
                </Listeners>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalFrecuenciaTelcoID">
                        <Fields>
                            <ext:ModelField Name="GlobalFrecuenciaTelcoID" Type="Int" />
                            <ext:ModelField Name="NombreFrecuenciaTelco" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreFrecuenciaTelco" Direction="ASC" />
                </Sorters>
                <Parameters>
                    <ext:StoreParameter Name="start" Value="0" Mode="Raw" />
                    <ext:StoreParameter Name="limit" Mode="Raw" Value="25" />
                </Parameters>
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
                            <ext:TextField meta:resourceKey="txtNombre"
                                ID="txtNombre"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                Text=""
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

            <ext:Window ID="winFrecuenciasLibres"
                runat="server"
                Title="<%$ Resources:Comun, strFrecuenciasLibres %>"
                Width="690"
                Height="500"
                Modal="true"
                Resizable="false"
                ShowOnLoad="false"
                Hidden="true"
                meta:resourceKey="winFrecuenciasLibres">
                <Items>
                    <ext:GridPanel runat="server"
                        ID="gridFrecuenciasLibres"
                        Border="false"
                        Frame="false"
                        Header="false"
                        Height="410"
                        StoreID="storeFrecuenciasLibres"
                        SelectionMemory="false"
                        EnableColumnHide="false">
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="NombreFrecuenciaTelco"
                                    ID="colNombreFrecuenciaTelco"
                                    Width="600"
                                    Header="<%$ Resources:Comun, strFrecuencia %>"
                                    meta:resourceKey="colNombreFrecuenciaTelco"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="GridRowSelectFrecuenciasLibresRowSelection" runat="server" Mode="Multi" EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridRowSelectFrecuenciasLibresRowSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersLibres"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server"
                                ID="PagingToolBarLibres"
                                PageSize="25"
                                StoreID="storeFrecuenciasLibres"
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
                                            <Select Fn="handlePageSizeSelectLibres" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancelarFrecuencias"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel"
                        meta:resourceKey="btnCancelar"
                        Handler="#{winFrecuenciasLibres}.hide();" />
                    <ext:Button ID="btnAgregarFrecuenciaLibres"
                        runat="server"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        meta:resourceKey="btnAgregar"
                        Handler="BotonAgregarFrecuenciaLibres();" />
                </Buttons>
                <Listeners>
                    <Show Handler="#{winFrecuenciasLibres}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winFrecuenciasAsignadas"
                runat="server"
                Title="<%$ Resources:Comun, strFrecuencias %>"
                Width="600"
                Height="400"
                Modal="true"
                Resizable="false"
                ShowOnLoad="false"
                Hidden="true"
                meta:resourceKey="winFrecuenciasAsignadas">
                <Items>
                    <ext:GridPanel runat="server"
                        ID="GridPanelFrecuenciasAsignadas"
                        Border="false"
                        Frame="false"
                        Header="false"
                        Height="310"
                        StoreID="storeFrecuenciasAsignadas"
                        SelectionMemory="false"
                        EnableColumnHide="false">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:Button ID="btnAgregarFrecuenciaAsignadas"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        runat="server"
                                        Cls="btnAnadir"
                                        meta:resourceKey="btnAgregarFrecuenciaAsignadas"
                                        Handler="BotonAgregarFrecuenciaAsignada();" />
                                    <ext:Button ID="btnQuitarFrecuenciasAsignada"
                                        ToolTip="<%$ Resources:Comun, strQuitar %>"
                                        runat="server"
                                        Cls="btnEliminar"
                                        meta:resourceKey="btnQuitarFrecuenciasAsignada"
                                        Handler="BotonEliminarFrecuenciaAsignada();" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="NombreFrecuenciaTelco"
                                    ID="colNombreFrecuenciaTelcoAsignada"
                                    Width="500"
                                    Header="<%$ Resources:Comun, strFrecuencia %>"
                                    meta:resourceKey="colNombreFrecuenciaTelco" Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="GridRowSelectFrecuenciasAsignadasRowSelection" runat="server" Mode="Multi" EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridRowSelectFrecuenciasAsignadaRowSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersAsignados"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server"
                                ID="PagingToolBarAsignados"
                                PageSize="25"
                                meta:resourceKey="PagingToolBar"
                                StoreID="storeFrecuenciasAsignadas"
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
                                            <Select Fn="handlePageSizeSelectAsignadas" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
                <Listeners>
                    <Show Handler="#{winFrecuenciasAsignadas}.center(); " />
                </Listeners>
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
                                    <ext:Button
                                        runat="server"
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
                                        Handler="ExportarDatos('GlobalTiposTecnologiasTelco', hdCliID.value, #{grid}, '');" />
                                    <ext:Button meta:resourceKey="btnAsignarFrecuencias"
                                        ID="btnAsignarFrecuencias"
                                        runat="server"
                                        ToolTip="Asignar Frecuencias"
                                        Icon="ChartCurve"
                                        Handler="BotonAsignarFrecuencias();" />
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
                                <ext:Column
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="Activo"
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
                                <ext:Column DataIndex="NombreTipoTecnologia" 
                                    Text="<%$ Resources:Comun, strNombre %>" 
                                    Width="250" 
                                    ID="colNombre"
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
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />                            
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
