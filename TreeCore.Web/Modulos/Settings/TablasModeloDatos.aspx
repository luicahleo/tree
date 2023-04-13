<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TablasModeloDatos.aspx.cs" Inherits="TreeCore.ModGlobal.TablasModeloDatos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>


    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden ID="hdSeleccionado" runat="server" />
        <ext:Hidden ID="hdDatabase" runat="server" />
        <ext:Hidden ID="hdDatabaseID" runat="server" />
        <ext:Hidden ID="hdValue" runat="server" />
        <ext:Hidden ID="hdColumnaID" runat="server" />
        <ext:Hidden ID="hdController" runat="server" />

        <ext:Hidden ID="hdForeignKey" runat="server" />

        <ext:Hidden ID="hdMaestroID" runat="server" />
        <ext:Hidden runat="server" ID="hdCliID" />
        <ext:Hidden runat="server" ID="ModuloID" />

        <ext:Hidden ID="hdDatabaseFK" runat="server" />
        <ext:Hidden ID="hdDataSourceID" runat="server" />



        <%-- FIN HIDDEN --%>

        <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

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
                <Load Fn="RecargarPrincipal" />
            </Listeners>
        </ext:Store>


        <ext:Store runat="server" ID="storePrincipal" AutoLoad="false" RemotePaging="false" OnReadData="storePrincipal_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="TablaModeloDatosID">
                    <Fields>

                        <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="NombreTabla" />
                        <ext:ModelField Name="Controlador" />
                        <ext:ModelField Name="Indice" />
                        <ext:ModelField Name="ClaveRecurso" />
                        <ext:ModelField Name="ClaveRecursoTabla" />
                        <ext:ModelField Name="ModuloID" Type="Int" />
                        <ext:ModelField Name="Modulo" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NombreTabla" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server" ID="storeDetalle" AutoLoad="false" RemotePaging="false" OnReadData="storeDetalle_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="ColumnaModeloDatosID">
                    <Fields>

                        <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="NombreColumna" />
                        <ext:ModelField Name="ClaveRecurso" />
                        <ext:ModelField Name="ClaveRecursoTabla" />
                        <ext:ModelField Name="ClaveRecursoColumna" />
                        <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="DataSourceTablaID" Type="Int" />
                        <ext:ModelField Name="NombreDataSource" />
                        <ext:ModelField Name="ForeignKeyID" Type="Int" />
                        <ext:ModelField Name="ForeignKeyNombre" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NombreColumna" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeTablas"
            runat="server"
            AutoLoad="false"
            OnReadData="storeTablas_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="TablaID">
                    <Fields>
                        <ext:ModelField Name="TablaID" />
                        <ext:ModelField Name="TablaNombre" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store ID="storeControladores"
            runat="server"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeControladores_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="ControladorID">
                    <Fields>
                        <ext:ModelField Name="ControladorID" Type="Int" />
                        <ext:ModelField Name="ControladorNombre" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="ControladorNombre" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeModulos"
            runat="server"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeModulos_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="ModuloID">
                    <Fields>
                        <ext:ModelField Name="ModuloID" Type="Int" />
                        <ext:ModelField Name="Modulo" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Modulo" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeTipoDato"
            runat="server"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeTipoDato_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="TipoDatoID">
                    <Fields>
                        <ext:ModelField Name="TipoDatoID" Type="Int" />
                        <ext:ModelField Name="TipoDato" />
                        <ext:ModelField Name="Codigo" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="TipoDato" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeTablasModelosDatos"
            runat="server"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeTablasModelosDatos_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="TablaModeloDatosID">
                    <Fields>

                        <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="NombreTabla" />
                        <ext:ModelField Name="ClaveRecurso" />
                        <ext:ModelField Name="ClaveRecursoTabla" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NombreTabla" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storeColumnas"
            runat="server"
            AutoLoad="false"
            OnReadData="storeColumnas_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="ColumnaTablaID">
                    <Fields>
                        <ext:ModelField Name="ColumnaTablaID" Type="Int" />
                        <ext:ModelField Name="ColumnaNombre" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store ID="storeColumnasFK"
            runat="server"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storeColumnasFK_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server"
                    IDProperty="ColumnaModeloDatosID">
                    <Fields>
                        <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                        <ext:ModelField Name="NombreColumna" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="NombreColumna" Direction="ASC" />
            </Sorters>
        </ext:Store>


        <%-- FIN STORES --%>




        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            Cls="winGestion-panel"
            meta:resourceKey="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="380"
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
                        <ext:ComboBox
                            ID="cmbTable"
                            runat="server"
                            StoreID="storeTablas"
                            DisplayField="TablaNombre"
                            ValueField="TablaNombre"
                            Mode="Local"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strTabla %>"
                            LabelAlign="Top"
                            AllowBlank="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarTable" />
                                <TriggerClick Fn="RecargarTablas" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:TextField ID="txtClaveRecurso"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strClaveRecurso %>"
                            LabelAlign="Top"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            Text=""
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="false"
                            meta:resourceKey="txtClaveRecurso" />

                        <ext:ComboBox
                            ID="cmbControlador"
                            runat="server"
                            StoreID="storeControladores"
                            DisplayField="ControladorNombre"
                            ValueField="ControladorNombre"
                            Mode="Local"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strControlador %>"
                            LabelAlign="Top"
                            AllowBlank="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarControlador" />
                                <TriggerClick Fn="RecargarControladores" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:ComboBox
                            ID="cmbIndice"
                            runat="server"
                            StoreID="storeColumnas"
                            DisplayField="ColumnaNombre"
                            ValueField="ColumnaNombre"
                            Mode="Local"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strIndice %>"
                            LabelAlign="Top"
                            AllowBlank="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarColumnaPadre" />
                                <TriggerClick Fn="RecargarColumnaPadre" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:ComboBox
                            ID="cmbModulo"
                            runat="server"
                            StoreID="storeModulos"
                            DisplayField="Modulo"
                            ValueField="ModuloID"
                            Mode="Local"
                            QueryMode="Local"
                            FieldLabel="<%$ Resources:Comun, strModulo %>"
                            LabelAlign="Top"
                            AllowBlank="true"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarModulo" />
                                <TriggerClick Fn="RecargarModulos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
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
                    Cls="btn-accept"
                    Disabled="true">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>

            </Buttons>
        </ext:Window>

        <ext:Window runat="server"
            ID="winGestionDetalle"
            meta:resourceKey="winGestionDetalle"
            Title="<%$ Resources:Comun, strColumnaTabla %>"
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

                        <ext:ComboBox
                            ID="cmbValue"
                            runat="server"
                            StoreID="storeColumnas"
                            DisplayField="ColumnaNombre"
                            ValueField="ColumnaNombre"
                            CausesValidation="true"
                            FieldLabel="<%$ Resources:Comun, strNombreColumna %>"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarValue" />
                                <TriggerClick Fn="RecargarColumnas" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:TextField ID="txtClaveRecursoDetalle"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strClaveRecurso %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            Text=""
                            AllowBlank="true"
                            CausesValidation="false"
                            meta:resourceKey="txtClaveRecursoDetalle" />

                        <ext:ComboBox
                            ID="cmbTipoDato"
                            runat="server"
                            StoreID="storeTipoDato"
                            DisplayField="TipoDato"
                            ValueField="TipoDatoID"
                            Disabled="true"
                            FieldLabel="<%$ Resources:Comun, strTipoDato %>"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="false"
                            CausesValidation="true"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarTipoDato" />
                                <TriggerClick Fn="RecargarTipoDato" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:ComboBox
                            ID="cmbDataSource"
                            runat="server"
                            StoreID="storeTablasModelosDatos"
                            DisplayField="ClaveRecursoTabla"
                            ValueField="TablaModeloDatosID"
                            FieldLabel="<%$ Resources:Comun, strDataSource %>"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="true"
                            Hidden="true"
                            CausesValidation="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarDataSource" />
                                <TriggerClick Fn="RecargarDataSource" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>


                        <ext:ComboBox
                            ID="cmbForeignKey"
                            runat="server"
                            StoreID="storeColumnasFK"
                            DisplayField="NombreColumna"
                            ValueField="ColumnaModeloDatosID"
                            FieldLabel="<%$ Resources:Comun, strForeignKey %>"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="true"
                            Hidden="true"
                            CausesValidation="false"
                            Editable="true"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarForeignKey" />
                                <TriggerClick Fn="RecargarForeignKey" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarDetalle"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarDetalle"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarDetalle();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winGestionDetalle}.center();" />
            </Listeners>
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server" ID="vwContenedor" Cls="vwContenedor" Layout="Anchor">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
                    AnchorHorizontal="100%"
                    AnchorVertical="58%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbBase"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>

                                <ext:Button runat="server"
                                    ID="btnAnadir"
                                    Enabled="true"
                                    meta:resourceKey="btnAnadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="Activar()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />

                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('TablasModeloDatos', hdCliID.value, #{gridMaestro}, '', '', -1);" />
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
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoMaestro"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="NombreTabla"
                                Text="<%$ Resources:Comun, strNombreTabla %>"
                                Width="150"
                                meta:resourceKey="colNombreTabla"
                                ID="colNombreTabla"
                                runat="server"
                                Flex="1" />
                            <ext:Column DataIndex="ClaveRecurso"
                                Text="<%$ Resources:Comun, strClaveRecurso %>"
                                Width="150"
                                meta:resourceKey="colClaveRecurso"
                                ID="colClaveRecurso"
                                runat="server"
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colColumnaTabla"
                                Text="<%$ Resources:Comun, strColumnaTabla %>"
                                Flex="1"
                                DataIndex="ClaveRecursoTabla"
                                meta:resourceKey="ClaveRecursoTabla"
                                Align="Center">
                            </ext:Column>
                            <ext:Column DataIndex="Controlador"
                                Text="<%$ Resources:Comun, strControlador %>"
                                Width="150"
                                meta:resourceKey="colControlador"
                                ID="colControlador"
                                runat="server"
                                Flex="1" />
                            <ext:Column DataIndex="Indice"
                                Text="<%$ Resources:Comun, strIndice %>"
                                Width="150"
                                meta:resourceKey="colIndice"
                                ID="colIndice"
                                runat="server"
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colModulo"
                                Text="<%$ Resources:Comun, strModulo %>"
                                Flex="1"
                                DataIndex="Modulo"
                                meta:resourceKey="Modulo"
                                Align="Center">
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
                <ext:GridPanel
                    runat="server"
                    ID="GridDetalle"
                    Cls="gridPanel gridDetalle"
                    SelectionMemory="false"
                    EnableColumnHide="false"
                    StoreID="storeDetalle"
                    AnchorHorizontal="100%"
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
                                    meta:resourceKey="btnAnadirDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('TablasModeloDatos', hdCliID.value, #{GridDetalle}, hdDatabaseID.value, '', 1);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="NombreColumna"
                                Width="200"
                                meta:resourceKey="colNombreColumna"
                                ID="colNombreColumna"
                                runat="server"
                                Text="<%$ Resources:Comun, strNombreColumna %>"
                                Flex="1" />
                            <ext:Column DataIndex="ClaveRecurso"
                                Width="200"
                                meta:resourceKey="colClaveRecurso"
                                ID="colClaveRecursoDetalle"
                                runat="server"
                                Text="<%$ Resources:Comun, strClaveRecurso %>"
                                Flex="1" />
                            <ext:Column runat="server"
                                ID="colClaveRecursoColumna"
                                Text="<%$ Resources:Comun, strColumnaTabla %>"
                                Flex="1"
                                DataIndex="ClaveRecursoColumna"
                                meta:resourceKey="ClaveRecursoColumna"
                                Align="Center">
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colTipoDato"
                                Text="<%$ Resources:Comun, strTipoDato %>"
                                Flex="1"
                                DataIndex="TipoDato"
                                meta:resourceKey="TipoDato"
                                Align="Center">
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colForeignKey"
                                Text="<%$ Resources:Comun, strForeignKey %>"
                                Flex="1"
                                DataIndex="ForeignKeyNombre"
                                meta:resourceKey="ForeignKeyNombre"
                                Align="Center">
                            </ext:Column>
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
                            meta:resourceKey="gridFiltersDetalle" />
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
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
