<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogServicios.aspx.cs" Inherits="TreeCore.ModGlobal.ProductCatalogServicios" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

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
            <ext:Hidden ID="hdServicioID" runat="server" />
            <ext:Hidden runat="server" ID="hdEditando" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                MaxWidth="750"
                Height="600"
                Hidden="true"
                OverflowY="Auto"
                Layout="FitLayout"
                Cls="winForm-resp">
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="tbVistasForm"
                        Cls="pnNavVistas pnVistasForm"
                        Dock="Top"
                        Padding="20"
                        OverflowHandler="Scroller">
                        <Items>
                            <ext:Container runat="server"
                                ID="cntNavVistasForm"
                                Cls="nav-vistas ctForm-tab-resp-col3"
                                ActiveIndex="2"
                                ActiveItem="2">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormServicio"
                                        Value="3"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strServicio %>">
                                        <Listeners>
                                            <Click Fn="showFormsServicios"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormLink"
                                        Value="4"
                                        Hidden="true"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="LINK">
                                        <Listeners>
                                            <Click Fn="showFormsServicios"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormPrecios"
                                        Cls="lnk-navView lnk-noLine"
                                        Value="5"
                                        Hidden="true"
                                        Text="PRICE FORMULA">
                                        <Listeners>
                                            <Click Fn="showFormsServicios"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormTask"
                                        Value="6"
                                        Hidden="true"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="TASK">
                                        <Listeners>
                                            <Click Fn="showFormsServicios"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="containerForm"
                        Cls="formGris formResp"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="formServicio"
                                Hidden="false"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2 navActivo">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtServicio"
                                        FieldLabel="<%$ Resources:Comun, strServicio %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Cls="item-form"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtCodigo"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Cls="item-form"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtDescripcion"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Cls="item-form"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEntidades"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strEntidad %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbDepartament"
                                        FieldBodyCls="cmbDepartament"
                                        DisplayField="Code"
                                        ValueField="Code"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeEntidades"
                                                AutoLoad="false"
                                                OnReadData="storeEntidades_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Active" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Code" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarEntidad" />
                                            <TriggerClick Fn="RecargarEntidad" />
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTipos"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbGroup"
                                        AllowBlank="false"
                                        FieldBodyCls="cmbGroup"
                                        DisplayField="Code"
                                        ValueField="Code"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCoreProductCatalogServiciosTipos"
                                                AutoLoad="false"
                                                OnReadData="storeCoreProductCatalogServiciosTipos_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Description" />
                                                            <ext:ModelField Name="Active" Type="Boolean" />
                                                            <ext:ModelField Name="Default" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Code" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarTipo" />
                                            <TriggerClick Fn="RecargarTipo" />
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <%--<ext:RadioGroup runat="server"
                                        ID="rdgFrecuenciasUnidades"
                                        GroupName="FrecuenciasUnidades"
                                        FieldLabel="Frecuency/Units"
                                        FieldBodyCls="cmbGroup"
                                        AutoFitErrors="false"
                                        Cls="x-check-group-alt">
                                        <Items>
                                            <ext:Radio runat="server"
                                                ID="boxFrecuencia"
                                                BoxLabel="<%$ Resources:Comun, strFrecuencia %>"
                                                InputValue="1" />
                                            <ext:Radio runat="server"
                                                ID="boxUnidad"
                                                BoxLabel="<%$ Resources:Comun, strUnidades %>"
                                                InputValue="2" />
                                        </Items>
                                        <Listeners>
                                            <Change Fn="pulsoRadio" />
                                        </Listeners>
                                    </ext:RadioGroup>
                                    <ext:ComboBox runat="server"
                                        ID="cmbFrecuencias"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strFrecuencia %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbGroup"
                                        Disabled="true"
                                        FieldBodyCls="cmbGroup"
                                        DisplayField="Codigo"
                                        AllowBlank="false"
                                        ValueField="CoreFrecuenciaID"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCoreFrecuencias"
                                                AutoLoad="false"
                                                OnReadData="storeCoreFrecuencias_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="CoreFrecuenciaID">
                                                        <Fields>
                                                            <ext:ModelField Name="CoreFrecuenciaID" Type="Int" />
                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarFrecuencia" />
                                            <TriggerClick Fn="RecargarFrecuencia" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbUnidades"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strUnidades %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbGroup"
                                        Disabled="true"
                                        FieldBodyCls="cmbGroup"
                                        DisplayField="Codigo"
                                        ValueField="CoreUnidadID"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCoreUnidades"
                                                AutoLoad="false"
                                                OnReadData="storeCoreUnidades_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="CoreUnidadID">
                                                        <Fields>
                                                            <ext:ModelField Name="CoreUnidadID" Type="Int" />
                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarUnidad" />
                                            <TriggerClick Fn="RecargarUnidad" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>--%>
                                    <ext:NumberField runat="server"
                                        ID="numCantidad"
                                        FieldLabel="<%$ Resources:Comun, strCantidad %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Number="1"
                                        Cls="item-form"
                                        AllowBlank="false"
                                        MinValue="1"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Change Fn="anadirClsNoValido" />
                                            <FocusLeave Fn="anadirClsNoValido" />
                                        </Listeners>
                                    </ext:NumberField>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formLink"
                                Hidden="true"
                                OverflowY="Hidden"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <%--<ext:ComboBox runat="server"
                                        ID="cmbObject"
                                        Cls=""
                                        FieldLabel="Object"
                                        LabelAlign="Top"
                                        EmptyCls="cmbObject"
                                        FieldBodyCls="cmbObject"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Items>
                                            <ext:ListItem Text="SITES" />
                                            <ext:ListItem Text="INVENTORY" />
                                            <ext:ListItem Text="DOCUMENTS" />
                                        </Items>
                                        <Listeners>
                                            <Select Fn="seleccionarComboObject" />
                                            <TriggerClick Fn="recargarComboObject" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEstadosGlobales"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strEstadosGlobales %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbGlobalState"
                                        DisplayField="Nombre"
                                        ValueField="ID"
                                        Disabled="true"
                                        StoreID="storeEstadosGlobales"
                                        FieldBodyCls="cmbGlobalState"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarEstadoGlobal" />
                                            <TriggerClick Fn="RecargarEstadoGlobal" />
                                            <Change Fn="FormularioValidoEstadosGlobales" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="btnAnadirEstadoGlobal"
                                        Width="100"
                                        Height="32"
                                        Text="<%$ Resources:Comun, strAnadir %>"
                                        IconCls="ico-addBtn"
                                        Cls="btn-mini-ppal ultimaColumna"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Fn="btnAgregarEstadoGlobal" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridEstadosGlobales"
                                        MinHeight="200"
                                        SelectionMemory="false"
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        StoreID="storeCoreEstadosGlobales"
                                        Cls="grdFormElAdded dosColumnasInicio"
                                        Scroll="Vertical">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colObjetoNegocio"
                                                    Text="Object"
                                                    DataIndex="NombreObjecto"
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colEstadoGlobal"
                                                    Text="<%$ Resources:Comun, strEstadoGlobal %>"
                                                    DataIndex="NombreEstado"
                                                    Flex="4">
                                                </ext:Column>
                                                <ext:CommandColumn runat="server"
                                                    Width="50"
                                                    Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Eliminar"
                                                            IconCls="ico-close">
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarEstadoGlobal" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectEstadoGlobal"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectEstadosSiguientes" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFiltersEstadosGlobales"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                        </Plugins>
                                    </ext:GridPanel>--%>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formPrecios"
                                Hidden="true"
                                OverflowY="Hidden"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <%--<ext:ComboBox runat="server"
                                        ID="cmbRoles"
                                        Cls="dosColumnasInicio"
                                        FieldLabel="Rols"
                                        LabelAlign="Top"
                                        DisplayField="Nombre"
                                        ValueField="RolID"
                                        StoreID="storeRolesLibres"
                                        EmptyCls="cmbModulo"
                                        FieldBodyCls="cmbModulo"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarRol" />
                                            <TriggerClick Fn="RecargarRol" />
                                            <Change Fn="FormularioValidoRoles" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button
                                        ID="btnAgregarRol"
                                        runat="server"
                                        Width="130"
                                        Height="32"
                                        TextAlign="Center"
                                        Text="<%$ Resources:Comun, winGestion.Title %>"
                                        Disabled="true"
                                        Cls="btn-mini-ppal btnAgregarPerfil"
                                        IconCls="ico-addBtn"
                                        ToolTip="<%$ Resources:Comun, strAñadirPerfiles %>">
                                        <Listeners>
                                            <Click Fn="btnAgregarRol" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridRoles"
                                        MinHeight="200"
                                        SelectionMemory="false"
                                        EnableColumnHide="false"
                                        Scrollable="Vertical"
                                        StoreID="storeCoreEstadosRoles"
                                        Cls="grdFormElAdded tresColumnas"
                                        Scroll="Vertical">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colRol"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    DataIndex="NombreRol"
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:CommandColumn runat="server"
                                                    Width="50"
                                                    Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Eliminar" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarRol" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectRol"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectRoles" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFiltersRoles"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                        </Plugins>
                                    </ext:GridPanel>--%>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formTask"
                                Hidden="true"
                                OverflowY="Hidden"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="tlbBotones"
                        Cls="greytb"
                        Dock="Bottom"
                        Padding="20">
                        <Items>
                            <ext:ToolbarFill />
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
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
            </ext:Window>

            <%-- FIN  WINDOWS --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Content>
                    <ext:Button
                        runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv3"
                        Handler="hideAsideR();"
                        Disabled="false"
                        Hidden="true">
                    </ext:Button>
                </Content>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbMain"
                                Dock="Top"
                                Cls="tbGrey tbNoborder "
                                Hidden="true"
                                Layout="HBoxLayout"
                                Flex="1">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Toolbar runat="server"
                                        ID="tlbSlider"
                                        Dock="Top"
                                        Hidden="false"
                                        MinHeight="36"
                                        Cls="tbGrey tbNoborder">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnPrevContenedor"
                                                IconCls="ico-prev-w"
                                                Cls="btnMainSldr SliderBtn"
                                                Handler="SliderMove('Prev');"
                                                Disabled="true">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnNextContenedor"
                                                IconCls="ico-next-w"
                                                Cls="SliderBtn"
                                                Handler="SliderMove('Next');"
                                                Disabled="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <Items>
                                    <ext:Panel
                                        runat="server"
                                        ID="wrapComponenteCentralServicios"
                                        Hidden="false"
                                        Layout="BorderLayout"
                                        BodyCls="tbGrey">
                                        <Items>
                                            <ext:GridPanel
                                                Region="Center"
                                                Hidden="false"
                                                runat="server"
                                                ForceFit="true"
                                                Header="false"
                                                ID="gridServicios"
                                                Scrollable="Vertical"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                Cls="gridPanel grdNoHeader "
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                                    <Resize Handler="GridColHandler(this)"></Resize>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBase"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                Handler="AgregarEditar();">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="MostrarEditar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Cls="btnEliminar"
                                                                Disabled="true"
                                                                Handler="Eliminar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Cls="btnRefrescar"
                                                                Handler="Refrescar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Cls="btnDescargar"
                                                                Handler="ExportarDatosSinCliente('ProductCatalogServicios', #{gridServicios}, '');" />
                                                            <ext:Button runat="server"
                                                                ID="btnExport"
                                                                ToolTip="<%$ Resources:Comun, strExportar %>"
                                                                Cls="btnExport"
                                                                Handler="BotonExportarFlujo();">
                                                            </ext:Button>
                                                            <ext:FileUploadField runat="server"
                                                                ID="FileUploadImportar"
                                                                Cls="btn-trans btnUploadFldGrid"
                                                                ButtonOnly="true">
                                                                <Listeners>
                                                                    <Change Fn="BotonImportarFlujo" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <ext:ToolTip runat="server"
                                                                        ID="ToolFileUpload"
                                                                        Html="<%$ Resources:Comun, strImportar %>"
                                                                        Target="#{FileUploadImportar}" />
                                                                </ToolTips>
                                                            </ext:FileUploadField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server"
                                                        ID="tbfiltros"
                                                        Cls=""
                                                        Dock="Top">
                                                        <Content>
                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storeCoreEstados"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                QuitarFiltros="true"
                                                                Grid="gridMain1"
                                                                MostrarBusqueda="false" />
                                                        </Content>
                                                    </ext:Container>
                                                </DockedItems>
                                                <Store>
                                                    <ext:Store
                                                        ID="storeCoreProductCatalogServicios"
                                                        runat="server"
                                                        AutoLoad="true"
                                                        OnReadData="storeCoreProductCatalogServicios_Refresh"
                                                        RemoteSort="false"
                                                        RemotePaging="false"
                                                        PageSize="20"
                                                        shearchBox="cmpFiltro_txtSearch"
                                                        listNotPredictive="CoreProductCatalogServicioID,FechaModificacion">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Code">
                                                                <Fields>
                                                                    <ext:ModelField Name="Code" />
                                                                    <ext:ModelField Name="Name" />
                                                                    <ext:ModelField Name="CompanyCode" />
                                                                    <ext:ModelField Name="Description" />
                                                                    <ext:ModelField Name="ProductTypeCode" />
                                                                    <ext:ModelField Name="Amount" Type="Float" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="Code" Direction="ASC" />
                                                        </Sorters>
                                                        <Listeners>
                                                            <BeforeLoad Fn="DeseleccionarGrilla" />
                                                            <DataChanged Fn="BuscadorPredictivo" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column
                                                            ID="colCodigo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            MinWidth="120"
                                                            DataIndex="Code"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strServicio %>"
                                                            MinWidth="120"
                                                            DataIndex="Name"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colDescripcion"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strDescripcion %>"
                                                            MinWidth="120"
                                                            DataIndex="Description"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colFrecuencia"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strFrecuenciaUnidad %>"
                                                            MinWidth="120"
                                                            DataIndex="Amount"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colTipo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strTipo %>"
                                                            MinWidth="120"
                                                            DataIndex="ProductTypeCode"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colEntidad"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strEntidad %>"
                                                            MinWidth="120"
                                                            DataIndex="CompanyCode"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:WidgetColumn ID="colMore"
                                                            runat="server"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="true"
                                                            MinWidth="90">
                                                            <Widget>
                                                                <ext:Button ID="btnMore"
                                                                    runat="server"
                                                                    Width="90"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Handler="parent.hideAsideR('WrapService');" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelect"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect"></Select>
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        StoreID="storeCoreProductCatalogServicios"
                                                        Cls="PgToolBMainGrid"
                                                        ID="PagingToolbar5"
                                                        MaintainFlex="true"
                                                        Flex="8"
                                                        HideRefresh="true"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                ID="ComboBox4"
                                                                MaxWidth="65">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
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
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
