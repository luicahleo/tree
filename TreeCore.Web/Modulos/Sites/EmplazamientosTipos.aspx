﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosTipos.aspx.cs" Inherits="TreeCore.ModGlobal.EmplazamientosTipos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true" PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoTipoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
            </ext:Store>


            <ext:Store
                ID="storeTiposEdificios"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposEdificios_Refresh"
                RemoteSort="false">

                <Listeners>
                    <BeforeLoad Handler="App.GridRowSelectTiposEdificios.clearSelections();App.btnQuitar.disable();" />
                </Listeners>

                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EmplazamientoTipoEdificioID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEdificioID" Type="Int" />
                            <ext:ModelField Name="TipoEdificio" Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeTiposEdificiosLibres"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposEdificiosLibres_Refresh"
                RemoteSort="false">

                <Listeners>
                    <BeforeLoad Handler="App.GridRowSelectTiposEdificiosLibres.clearSelections();" />
                </Listeners>

                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="EmplazamientoTipoEdificioID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoEdificioID" Type="Int" />
                            <ext:ModelField Name="TipoEdificio" Type="String" />
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
                Title="<%$ Resources:Comun, winGestion.Title %>"
                BodyPaddingSummary="10 32"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestion"
                        Border="false">
                        <Items>
                            <ext:TextField
                                ID="txtTipo"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTipo %>"
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
                    <ext:Button
                        runat="server"
                        ID="btnCancelar"
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-secondary">
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
                        Cls="btn-ppal"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>

                </Buttons>
            </ext:Window>

            <ext:Window
                ID="winTiposEdificios"
                runat="server"
                Title="<%$ Resources:Comun, strTiposEdificios %>"
                Width="500"
                Height="500"
                Resizable="false"
                Modal="true"
                Scrollable="Vertical"
                ShowOnLoad="true"
                Hidden="true">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposEdificios"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposEdificios"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        Scroll="None"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="Toolbar1"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button
                                        runat="server"
                                        ID="btnAgregar"
                                        meta:resourceKey="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir">
                                        <Listeners>
                                            <Click Handler="BotonAgregarTipoEdificio();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnQuitar"
                                        meta:resourceKey="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, strQuitar %>"
                                        Cls="btnEliminar"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Handler="BotonEliminarTipoEdificio();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="columnModelTiposEdificios" runat="server">
                            <Columns>
                                <ext:Column
                                    DataIndex="TipoEdificio"
                                    ID="colTipoEdificio"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strTipoEdificio %>"
                                    meta:resourceKey="colTipoEdificio"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectTiposEdificios"
                                runat="server"
                                Mode="Multi"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectTiposEdificios" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters
                                runat="server"
                                ID="gridFiltersTiposEdificios"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="gridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>

                    </ext:GridPanel>
                </Items>
                <BottomBar>
                    <ext:PagingToolbar
                        ID="PagingToolBar2"
                        runat="server"
                        PageSize="25"
                        StoreID="storeTiposEdificios"
                        DisplayInfo="true">
                        <Plugins>
                            <ext:SlidingPager ID="SlidingPager1" runat="server" />
                        </Plugins>
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winTiposEdificios}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winTiposEdificiosLibres"
                runat="server"
                Title="<%$ Resources:Comun, strTiposEdificiosLibres %>"
                Width="400"
                Height="400"
                Resizable="false"
                Scrollable="Vertical"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridTiposEdificiosLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeTiposEdificiosLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        Scroll="None"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">

                        <ColumnModel
                            ID="columnModelTiposEdificiosLbres"
                            runat="server">
                            <Columns>
                                <ext:Column
                                    DataIndex="TipoEdificio"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strTipoEdificio %>"
                                    Width="350"
                                    meta:resourceKey="columTipoEdificio"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectTiposEdificiosLibres"
                                runat="server"
                                Mode="Multi"
                                SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridTiposEdificiosLibresSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters
                                runat="server"
                                ID="gridFiltersTiposEdificiosLbres"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="gridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        
                    </ext:GridPanel>
                </Items>
                
                <Buttons>
                    <ext:Button
                        runat="server"
                        ID="btnCancelarEdifLibres"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-secondary"
                        meta:resourceKey="btnCancelarEdifLibres">
                        <Listeners>
                            <Click Handler="#{winTiposEdificiosLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnAceptar"
                        meta:resourceKey="btnAceptar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-ppal">
                        <Listeners>
                            <Click Handler="BotonGuardarTiposEdificiosLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                
                <Listeners>
                    <Show Handler="#{winTiposEdificios}.center();" />
                </Listeners>
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
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btn-Eliminar"
                                        Handler="Eliminar();" />
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
                                        Handler="ExportarDatos('EmplazamientosTipos', hdCliID.value, #{grid}, '');" />
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        meta:resourceKey="btnActivar"
                                        Cls="btn-Activar"
                                        Handler="Activar();">
                                    </ext:Button>

                                    <ext:Button runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        meta:resourceKey="btnDefecto"
                                        Cls="btnDefecto"
                                        Handler="Defecto();" />

                                    <ext:Button runat="server"
                                        ID="btnTiposEdificios"
                                        ToolTip="<%$ Resources:Comun, strTiposEdificios %>"
                                        meta:resourceKey="btnTiposEdificios"
                                        Cls="btnTiposEdificios"
                                        Hidden="true"
                                        Handler="BotonTiposEdificios();" />

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
                                    DataIndex="Tipo"
                                    ID="colTipo" runat="server"
                                    Text="<%$ Resources:Comun, strTipo %>"
                                    Width="150"
                                    Flex="1"
                                    meta:resourceKey="colTipo" />
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
