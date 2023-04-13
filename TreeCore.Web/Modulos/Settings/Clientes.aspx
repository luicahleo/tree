﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TreeCore.ModGlobal.Clientes" %>

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
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ClienteID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                            <ext:ModelField Name="CIF" />
                            <ext:ModelField Name="Operador" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="VexEntorno" />
                            <ext:ModelField Name="VexProyecto" />
                            <ext:ModelField Name="Imagen" />
                            <ext:ModelField Name="CodigoInstancia" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClienteID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeOperadores"
                runat="server"
                AutoLoad="false"
                OnReadData="storeOperadores_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="OperadorID" runat="server">
                        <Fields>
                            <ext:ModelField Name="OperadorID" Type="Int" />
                            <ext:ModelField Name="Operador" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>
            <ext:Store
                ID="storeMonedas"
                runat="server"
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
                <Sorters>
                    <ext:DataSorter Property="Moneda" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeProyectosTipos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeProyectosTipos_Refresh"
                PageSize="13"
                RemoteSort="true">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaProyectosTipos" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeProyectosTiposLibres"
                runat="server"
                AutoLoad="false"
                OnReadData="storeProyectosTiposLibres_Refresh"
                RemoteSort="true">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaProyectosTiposLibres" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
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

                            <ext:TextField
                                ID="txtCliente"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCliente %>"
                                Text=""
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtCliente" />
                            <ext:TextField
                                ID="txtCIF"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCIF %>"
                                Text=""
                                MaxLength="12"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtCIF" />
                            <ext:ComboBox
                                ID="cmbOperadores"
                                runat="server"
                                meta:resourceKey="cmbOperadores"
                                StoreID="storeOperadores"
                                DisplayField="Operador"
                                ValueField="OperadorID"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true"
                                FieldLabel="<%$ Resources:Comun, strOperador %>"
                                QueryMode="Local"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarOperadores" />
                                    <TriggerClick Fn="RecargarOperadores" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox
                                ID="cmbMoneda"
                                runat="server"
                                meta:resourceKey="cmbMoneda"
                                StoreID="storeMonedas"
                                Mode="Local"
                                DisplayField="Moneda"
                                ValueField="MonedaID"
                                QueryMode="Local"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true"
                                FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarMoneda" />
                                    <TriggerClick Fn="RecargarMoneda" />
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
                                ID="txtVexEntorno"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strVexEntorno %>"
                                Text=""
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtVexEntorno" />
                            <ext:TextField
                                ID="txtVexProyecto"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strVexProyecto %>"
                                Text=""
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtVexProyecto" />
                            <ext:TextField
                                ID="txtCodigoInstancia"
                                runat="server"
                                FieldLabel="Codigo Instancia"
                                Text=""
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtCodigoInstancia" />
                            <ext:FileUploadField
                                ID="FileImagen"
                                runat="server"
                                meta:resourceKey="FileImagen"
                                AllowBlank="true"
                                EmptyText="Seleccione una Imagen"
                                FieldLabel="<%$ Resources:Comun, strImagen %>"
                                ButtonText=""
                                IconCls="ico-image">
                                <Listeners>
                                    <Change Fn="CargarImagen" />
                                </Listeners>
                            </ext:FileUploadField>
                            <ext:Image
                                ID="logoCliente"
                                runat="server"
                                Alt="<%$ Resources:Comun, strImagen %>"
                                MinHeight="100"
                                Cls="form-logo-cliente">
                                <%--<ResizableConfig 
                                    runat="server" 
                                    PreserveRatio="true" 
                                    HandlesSummary="s e se"/>--%>
                            </ext:Image>
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

            <%--VENTANAS PROYECTOS TIPOS --%>

            <ext:Window
                ID="winProyectosTipos"
                runat="server"
                Title="<%$ Resources:Comun, strProyectosTipos %>"
                Width="600"
                Height="500"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true"
                meta:resourceKey="winProyectosTipos"
                Scrollable="Disabled">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridProyectosTipos"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeProyectosTipos"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        Height="450px"
                        Scrollable="Vertical"
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
                                        ID="btnAgregarProyectosTipos"
                                        meta:resourceKey="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir"
                                        Handler="BotonAgregarProyectosTipos();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnQuitarProyectosTipos"
                                        meta:resourceKey="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, strQuitar %>"
                                        Cls="btnEliminar"
                                        Disabled="true"
                                        Handler="BotonEliminarProyectosTipos();" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="columnModelProyectosTipos" runat="server">
                            <Columns>
                                <ext:Column
                                    DataIndex="ProyectoTipo"
                                    ID="colProyectoTipo"
                                    Header="<%$ Resources:Comun, strProyectoTipo %>"
                                    Width="350"
                                    meta:resourceKey="colProyectosTipos"
                                    runat="server"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectProyectosTipos"
                                runat="server"
                                Mode="Multi"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectProyectosTipos" />
                                    <Deselect Fn="DeseleccionarGrillaProyectosTipos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersProyectosTipos" 
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>

                </Items>
                <BottomBar>
                    <ext:PagingToolbar
                        runat="server"
                        ID="PagingToolBar1"
                        meta:resourceKey="PagingToolBar"
                        StoreID="storeProyectosTipos"
                        DisplayInfo="true"
                        HideRefresh="false">
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winProyectosTipos}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winProyectosTiposLibres"
                runat="server"
                Title="<%$ Resources:Comun, strProyectosTiposLibre %>"
                Width="500"
                Height="500"
                Resizable="false"
                Modal="true"
                ShowOnLoad="true"
                Hidden="true"
                meta:resourceKey="winProyectosTiposLibres"
                Scrollable="Disabled">

                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridProyectosTiposLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="StoreProyectosTiposLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        Height="450px"
                        AriaRole="main"
                        Scrollable="Vertical">

                        <ColumnModel ID="columnModelTiposEstructurasLibres" runat="server">
                            <Columns>
                                <ext:Column
                                    DataIndex="ProyectoTipo"
                                    ID="colProyectoTipoLibre" runat="server"
                                    Header="<%$ Resources:Comun, strProyectoTipo %>"
                                    Width="350"
                                    meta:resourceKey="colProyectoTipo"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                ID="GridRowSelectProyectosTiposLibres"
                                runat="server"
                                Mode="Multi"
                                SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridProyectosTiposLibresSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersProyectoTipoLibre" 
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>

                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        ID="btnCancelarEdifLibres"
                        runat="server"
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winProyectosTiposLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        ID="btnGuardarProyectosTiposLibre"
                        runat="server"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="BotonGuardarProyectosTiposLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winProyectosTiposLibres}.center();" />
                </Listeners>
            </ext:Window>

            <%--FIN PROYECTOS TIPOS --%>

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
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btnEliminar"
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
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnProjects"
                                        ToolTip="<%$ Resources:Comun, strProyectosTipos %>"
                                        meta:resourceKey="btnProjects"
                                        Cls="btnProjects"
                                        Handler="BotonProyectosTipos();" />
                                    <ext:Button runat="server"
                                        ID="btnAnonimizar"
                                        ToolTip="<%$ Resources:Comun, strAnonimizar %>"
                                        meta:resourceKey="btnAnonimizar"
                                        Cls="btnAnonimizar"
                                        Handler="BotonAnonimizar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('Clientes', hdCliID.value, #{grid}, '');" />
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

                                <ext:Column
                                    DataIndex="Cliente"
                                    ID="colCliente"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCliente %>"
                                    Width="250"
                                    Flex="1"
                                    meta:resourceKey="colCliente" />
                                <ext:Column
                                    DataIndex="CIF"
                                    ID="CIF"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCIF %>"
                                    Width="120"
                                    meta:resourceKey="colCIF" />
                                <ext:Column
                                    DataIndex="Operador"
                                    ID="colOperador"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strOperador %>"
                                    Width="150"
                                    meta:resourceKey="colOperador" />
                                <ext:Column
                                    DataIndex="Moneda"
                                    ID="colMoneda"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strMoneda %>"
                                    Width="120"
                                    meta:resourceKey="colMoneda" />
                                <ext:Column
                                    DataIndex="VexEntorno"
                                    ID="colVexEntorno"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strVexEntorno %>"
                                    Width="150"
                                    meta:resourceKey="colVexEntorno" />
                                <ext:Column
                                    DataIndex="VexProyecto"
                                    ID="colVexProyecto"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strVexProyecto %>"
                                    Width="150"
                                    meta:resourceKey="colVexProyecto" />
                                <ext:Column
                                    DataIndex="CodigoInstancia"
                                    ID="colCodigoInstancia"
                                    runat="server"
                                    Width="150"
                                    meta:resourceKey="colCodigoInstancia" />
                                <ext:Column
                                    DataIndex="Imagen"
                                    ID="colImagen"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strImagen %>"
                                    Width="80"
                                    Align="Center"
                                    meta:resourceKey="col">
                                    <Renderer Fn="ImageRender" />
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
