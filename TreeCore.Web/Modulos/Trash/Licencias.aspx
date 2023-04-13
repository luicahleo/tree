﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Licencias.aspx.cs" Inherits="TreeCore.ModGlobal.Licencias" %>

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
                    <ext:Model runat="server" IDProperty="LicenciaID">
                        <Fields>
                            <ext:ModelField Name="LicenciaID" Type="Int" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="CodigoLicencia" />
                            <ext:ModelField Name="LicenciaTipo" />
                            <ext:ModelField Name="LicenciaTipoID" Type="Int" />
                            <ext:ModelField Name="Disponibles" Type="Int" />
                            <ext:ModelField Name="NumeroLicencias" Type="Int" />
                            <ext:ModelField Name="FechaCaducidad" Type="Date" />
                            <ext:ModelField Name="FechaActivacion" Type="Date" />
                            <ext:ModelField Name="CodigoLicenciaTipo" />
                            <ext:ModelField Name="Web" Type="Boolean" />
                            <ext:ModelField Name="Movil" Type="Boolean" />
                            <ext:ModelField Name="Actualizaciones" Type="Boolean" />
                            <ext:ModelField Name="Soporte" Type="Boolean" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activa" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="LicenciaTipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeLicenciasTipo"
                AutoLoad="false"
                OnReadData="storeLicenciasTipo_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="LicenciaTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="LicenciaTipoID" Type="Int" />
                            <ext:ModelField Name="LicenciaTipo" Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGenerar"
                meta:resourceKey="winGenerar"
                Title="Generar"
                BodyStyle="padding:10px;"
                Width="400"
                AutoHeight="true"
                Resizable="false"
                Modal="true"
                ShowOnLoad="false"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="FormPanelGenerar"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbTiposLicencias"
                                FieldLabel="<%$ Resources:Comun, strTipoLicencia %>"
                                Mode="Local"
                                DisplayField="LicenciaTipo"
                                ValueField="LicenciaTipoID"
                                StoreID="storeLicenciasTipo"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="false"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarTipoLicencia" />
                                    <TriggerClick Fn="RecargarTipoLicencia" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:NumberField runat="server"
                                ID="txtNumeroLicencias"
                                FieldLabel="<%$ Resources:Comun, strUnidades %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:DateField runat="server"
                                ID="txtFechaActivacion"
                                FieldLabel="<%$ Resources:Comun, strFechaActivacion %>"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:DateField runat="server"
                                ID="txtFechaCaducidad"
                                meta:resourceKey="txtFechaCaducidad"
                                FieldLabel="Fecha Caducidad"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false" 
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGenerar(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGenerarCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGenerar}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarGenerar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGenerarBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winGenerar}.center();" />
                </Listeners>
            </ext:Window>

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
                                ID="txtCodigoLicencia"
                                meta:resourceKey="txtCodigoLicencia"
                                FieldLabel="Codigo Licencia"
                                MaxLength="100"
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
                        AnchorHorizontal="100%"
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
                                        Handler="Agregar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('Licencias', hdCliID.value, #{grid}, '');" />
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGenerarCodigo"
                                        ToolTip="Generar Codigo"
                                        meta:resourceKey="btnGenerarCodigo"
                                        Cls="btnGenerarCodigo"
                                        Handler="BotonGenerarCodigo();" />
                                    <ext:Button runat="server"
                                        ID="btnTiposLicencia"
                                        ToolTip="<%$ Resources:Comun, strTipoLicencia %>"
                                        Cls="btnTiposLicencia"
                                        Handler="BotonTiposLicencias();" />
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
                                    DataIndex="Activa"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Align="Center"
                                    Cls="col-activo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colLicenciaTipo"
                                    DataIndex="LicenciaTipo"
                                    Text="<%$ Resources:Comun, strTipoLicencia %>"
                                    Width="200" />
                                <ext:Column runat="server"
                                    ID="colNumeroLicencias"
                                    DataIndex="NumeroLicencias"
                                    Text="<%$ Resources:Comun, strUnidades %>"
                                    Width="100"
                                    Align="Center" />
                                <ext:Column runat="server"
                                    ID="colDisponibles"
                                    meta:resourceKey="columDisponibles"
                                    DataIndex="Disponibles"
                                    Text="Disponibles"
                                    Width="100"
                                    Align="Center" />
                                <ext:DateColumn runat="server"
                                    ID="colFechaActivacion"
                                    meta:resourceKey="colFechaActivacion"
                                    DataIndex="FechaActivacion"
                                    Text="<%$ Resources:Comun, strFechaActivacion %>"
                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                    Align="Center"
                                    Width="200" />
                                <ext:DateColumn runat="server"
                                    ID="colFechaCaducidad"
                                    meta:resourceKey="columFechaCancelacion"
                                    DataIndex="FechaCaducidad"
                                    Text="Fecha Caducidad"
                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                    Align="Center"
                                    Width="200" />
                                <ext:Column runat="server"
                                    ID="colActualizaciones"
                                    meta:resourceKey="columActualizaciones"
                                    DataIndex="Actualizaciones"
                                    Text="Actualizaciones"
                                    Align="Center"
                                    Width="150">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colMovil"
                                    DataIndex="Movil"
                                    Text="<%$ Resources:Comun, strMovil %>"
                                    Width="100"
                                    Align="Center">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colSoporte"
                                    meta:resourceKey="columSoporte"
                                    DataIndex="Soporte"
                                    Text="Soporte"
                                    Width="100"
                                    Align="Center">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colWeb"
                                    meta:resourceKey="columWeb"
                                    DataIndex="Web"
                                    Text="Web"
                                    Width="100"
                                    Align="Center">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colCodigoLicencia"
                                    DataIndex="CodigoLicencia"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    Width="300"
                                    Flex="1">
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
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
