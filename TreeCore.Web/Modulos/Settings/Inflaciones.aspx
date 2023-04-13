<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inflaciones.aspx.cs" Inherits="TreeCore.ModGlobal.Inflaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden runat="server" ID="hdCliID" />
        <ext:Hidden runat="server" ID="ModuloID" />
        <ext:Hidden runat="server" ID="hdEditando" />

        <%-- FIN HIDDEN --%>

        <ext:ResourceManager runat="server"
            ID="ResourceManagerTreeCore"
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

        <ext:Store runat="server"
            ID="storePaises"
            AutoLoad="false"
            OnReadData="storePaises_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="Code" runat="server">
                    <Fields>
                        <ext:ModelField Name="Code" />
                        <ext:ModelField Name="Name" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="true" OnReadData="storePrincipal_Refresh" RemoteSort="false" PageSize="20">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="Code">
                    <Fields>
                        <ext:ModelField Name="Code" />
                        <ext:ModelField Name="Name" />
                        <ext:ModelField Name="Description" />
                        <ext:ModelField Name="Estandar" />
                        <ext:ModelField Name="Active" Type="Boolean" />
                        <ext:ModelField Name="CountryName" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Code" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <%--<ext:Store runat="server" 
            ID="storeDetalle" 
            AutoLoad="false" 
            RemotePaging="false" 
            OnReadData="storeDetalle_Refresh" 
            RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="InflacionDetalleID">
                    <Fields>
                        <ext:ModelField Name="InflacionDetalleID" Type="Int" />
                        <ext:ModelField Name="InflacionID" Type="Int" />
                        <ext:ModelField Name="Mes" Type="Int" />
                        <ext:ModelField Name="Anualidad" Type="Int" />
                        <ext:ModelField Name="Valor" Type="Float" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="FechaDesde" Type="Date" />
                        <ext:ModelField Name="FechaHasta" Type="Date" />
                        <ext:ModelField Name="Anual" Type="Float" />
                        <ext:ModelField Name="Interanual" Type="Float" />
                        <ext:ModelField Name="Trimestral" Type="Float" />
                        <ext:ModelField Name="Semestral" Type="Float" />
                        <ext:ModelField Name="Cuatrimestral" Type="Float" />
                        <ext:ModelField Name="Acumulado" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Activo" Direction="ASC" />
            </Sorters>
        </ext:Store>--%>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestion"
                    Cls="form-gestion"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField
                            runat="server"
                            ID="txtCodigo"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            WidthSpec="100%"
                            LabelAlign="Top" >
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="txtInflacion"
                            FieldLabel="Inflacion"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            WidthSpec="100%"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtInflacion"
                            LabelAlign="Top" >
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                            </Listeners>
                        </ext:TextField>
                        <ext:ComboBox runat="server"
                            ID="cmbPais"
                            StoreID="storePaises"
                            Mode="Local"
                            DisplayField="Name"
                            ValueField="Code"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strPaises %>"
                            AllowBlank="false"
                            WidthSpec="100%"
                            LabelAlign="Top">
                            <Listeners>
                                <Change Fn="anadirClsNoValido" />
                                <FocusLeave Fn="anadirClsNoValido" />
                                <Select Fn="SeleccionarPais" />
                                <TriggerClick Fn="RecargarPais" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>"
                                    Hidden="true"
                                    Weight="-1" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:Container ID="ctrEstandar" runat="server" Layout="HBoxLayout" Cls="checks" Border="true" MarginSpec="25 0 10">
                            <Items>
                                <ext:Checkbox ID="chkEstandar"
                                    runat="server"
                                    AllowBlank="false"
                                    ValidationGroup="FORM"
                                    CausesValidation="true">
                                </ext:Checkbox>
                                <ext:Label runat="server"
                                    ID="lblEstándar"
                                    Text="Estándar"
                                    meta:resourceKey="ckEstandar">
                                </ext:Label>
                            </Items>
                        </ext:Container>

                        <ext:TextArea runat="server"
                            ID="txtDescripcion"
                            FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                            MaxLength="240"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            WidthSpec="100%"
                            LabelAlign="Top" />
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
                    Cls="btn-secondary-winForm"
                    Focusable="false">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Disabled="true"
                    Focusable="false"
                    Cls="btn-ppal-winForm">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <%--<ext:Window runat="server"
            ID="winGestionDetalle"
            Width="420"
            Resizable="false"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            ID="txtMes"
                            FieldLabel="<%$ Resources:Comun, strMes %>"
                            MaxLength="500"
                            MinValue="1"
                            MaxValue="12"
                            Step="1"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true" />
                        <ext:NumberField runat="server"
                            ID="txtAnualidad"
                            FieldLabel="Anualidad"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtAnualidad" />
                        <ext:DateField ID="dateDesde"
                            meta:resourceKey="dateDesde"
                            runat="server"
                            FieldLabel="Fecha Desde"
                            Hidden="false"
                            Format="<%$ Resources:Comun, FormatFecha %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            StartDateField="dateDesde">
                        </ext:DateField>
                        <ext:DateField ID="dateHasta"
                            meta:resourceKey="dateHasta"
                            runat="server"
                            FieldLabel="Fecha Hasta"
                            Hidden="false"
                            Format="<%$ Resources:Comun, FormatFecha %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            StartDateField="dateHasta">
                        </ext:DateField>
                        <ext:NumberField runat="server"
                            ID="txtValor"
                            FieldLabel="Valor Mensual"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValor" />
                        <ext:NumberField runat="server"
                            ID="txtValorAnual"
                            FieldLabel="Valor Anual"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorAnual" />
                        <ext:NumberField runat="server"
                            ID="txtValorInteranual"
                            FieldLabel="Valor Interanual"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorInteranual" />
                        <ext:NumberField runat="server"
                            ID="txtValorTrimestral"
                            FieldLabel="Valor Trimestral"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorTrimestral" />
                        <ext:NumberField runat="server"
                            ID="txtValorCuatrimestral"
                            FieldLabel="Valor Cuatrimestral"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorCuatrimestral" />
                        <ext:NumberField runat="server"
                            ID="txtValorSemestral"
                            FieldLabel="Valor Semestral"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorSemestral" />
                        <ext:NumberField runat="server"
                            ID="txtValorAcumulado"
                            FieldLabel="Valor Acumulado"
                            MaxLength="500"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            DecimalPrecision="4"
                            DecimalSeparator=","
                            meta:resourceKey="txtValorAcumulado" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarDetalle"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarDetalle"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Disabled="true"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarDetalle();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winGestionDetalle}.center();" />
            </Listeners>
        </ext:Window>--%>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server"
            ID="vwContenedor"
            Cls="vwContenedor"
            Layout="Fitlayout">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
                    AnchorHorizontal="-100"
                    AnchorVertical="100%"
                    AriaRole="main">
                    <Listeners>
                        <AfterRender Handler="GridColHandlerDinamicoV2(this);"></AfterRender>
                        <Resize Handler="GridColHandlerDinamicoV2(this);"></Resize>
                    </Listeners>
                    <DockedItems>
                        <ext:Toolbar runat="server"
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
                                    ID="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btn-Activar"
                                    Handler="Activar();">
                                </ext:Button>
                                
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    Cls="btnDescargar-subM"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Hidden="false">
                                    <Menu>
                                        <ext:Menu ID="MnuExcels"
                                            runat="server"
                                            BoxMinWidth="110">
                                            <Items>
                                                <ext:MenuItem
                                                    ID="btnExcel"
                                                    meta:resourceKey="btnExcel"
                                                    Text="Excel"
                                                    Icon="PageExcel"
                                                    Handler="ExportarDatos('Inflaciones', hdCliID.value, #{gridMaestro}, '-1');" />
                                                <%--<ext:MenuItem runat="server"
                                                    ID="btnExcelInflacionYdetalles"
                                                    Text="Excel Inflacion & Detalles"
                                                    Icon="PageExcel"
                                                    meta:resourceKey="btnExcelInflacionYdetalles"
                                                    Handler="btnCargarExcelInflacionYdetalles();" />--%>
                                            </Items>
                                        </ext:Menu>
                                    </Menu>
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnInterfazIndices"
                                    meta:resourceKey="toolInterfazIndices"
                                    ToolTip="SAP Indices"
                                    Cls="btnSAP">
                                    <Listeners>
                                        <Click Fn="abrirInterfaz" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="colActivo"
                                DataIndex="Active"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                MinWidth="50"
                                MaxWidth="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                runat="server"
                                ID="Codigo"
                                meta:resourceKey="colCodigo"
                                DataIndex="Code"
                                Text="<%$ Resources:Comun, strCodigo %>"
                                MinWidth="100"
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colInflacion"
                                Text="Inflacion"
                                DataIndex="Name"
                                Hideable="false"
                                meta:resourceKey="InflacionColumn"
                                MinWidth="100"
                                Flex="1">
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colPais"
                                Text="<%$ Resources:Comun, strPais %>"
                                DataIndex="CountryName"
                                Hideable="false"
                                MinWidth="100"
                                Flex="1">
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colEstandar"
                                DataIndex="Estandar"
                                Text="Estándar"
                                MinWidth="100"
                                MaxWidth="100"
                                Align="Center"
                                meta:resourceKey="colEstandar">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colDescripcion"
                                Text="<%$ Resources:Comun, strDescripcion %>"
                                DataIndex="Description"
                                Hideable="false"
                                MinWidth="100"
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
                            ID="PagingToolBar1"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storePrincipal"
                            DisplayInfo="true"
                            HideRefresh="true"
                            OverflowHandler="Scroller">
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
                <%--<ext:GridPanel
                    runat="server"
                    ID="GridDetalle"
                    Cls="gridPanel gridDetalle"
                    SelectionMemory="false"
                    EnableColumnHide="false"
                    StoreID="storeDetalle"
                    AnchorHorizontal="-100"
                    AnchorVertical="42%"
                    AriaRole="main">
                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbDetalle"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAnadirDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('Inflaciones', hdCliID.value, #{GridDetalle}, #{ModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivo"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                DataIndex="Activo"
                                Align="Center"
                                Text="Active"
                                Flex="1">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:DateColumn runat="server"
                                ID="colFechaDesde"
                                DataIndex="FechaDesde"
                                Text="F.Desde"
                                Flex="2"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columFechaDesde" />
                            <ext:DateColumn
                                DataIndex="FechaHasta"
                                Text="F.Hasta"
                                Flex="2"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columFechaHasta"
                                ID="colFechaHasta"
                                runat="server" />
                            <ext:Column
                                DataIndex="Mes"
                                Text="<%$ Resources:Comun, strMes %>"
                                Flex="2"
                                ID="colMes"
                                runat="server" />
                            <ext:Column
                                DataIndex="Anualidad"
                                Text="Anualidad"
                                Flex="2"
                                meta:resourceKey="columnAnualidad"
                                ID="colAnualidad"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Valor"
                                Text="<%$ Resources:Comun, strValor %>"
                                Flex="2"
                                Format="0.00"
                                ID="colValor"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Anual"
                                Text="Anual"
                                Flex="2"
                                meta:resourceKey="columnAnual"
                                Format="0.00"
                                ID="colAnual"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Interanual"
                                Text="Interanual"
                                Flex="2"
                                meta:resourceKey="columnInteranual"
                                Format="0.00"
                                ID="colInteranual"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Trimestral"
                                Text="Trimestral"
                                Flex="2"
                                meta:resourceKey="columnTrimestral"
                                Format="0.00"
                                ID="colTrimestral"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Cuatrimestral"
                                Text="Cuatrimestral"
                                Flex="2"
                                meta:resourceKey="columnCuatrimestral"
                                Format="0.00"
                                ID="colCuatrimestral"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Semestral"
                                Text="Semestral"
                                Flex="2"
                                meta:resourceKey="columnSemestral"
                                Format="0.00"
                                ID="colSemestral"
                                runat="server" />
                            <ext:NumberColumn
                                DataIndex="Acumulado"
                                Text="Acumulado"
                                Flex="2"
                                meta:resourceKey="columnAcumulado"
                                Format="0.00"
                                ID="colAcumulado"
                                runat="server" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server"
                            ID="GridRowSelectDetalle"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect_Detalle" />
                                <Deselect Fn="DeseleccionarGrillaDetalle" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFiltersDetalle"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"
                            ID="PagingToolBar2"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storeDetalle"
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
                                        <Select Fn="handlePageSizeSelectDetalle" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>--%>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
