<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificacionesGestion.aspx.cs" Inherits="TreeCore.ModGlobal.NotificacionesGestion" %>

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
        <ext:Hidden runat="server" ID="hdModuloID" />

        <%-- FIN HIDDEN --%>

        <ext:ResourceManager
            runat="server"
            ID="ResourceManagerTreeCore"
            DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

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
                <ext:Model
                    runat="server"
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

        <ext:Store
            runat="server"
            ID="storePrincipal"
            AutoLoad="false"
            RemotePaging="false"
            OnReadData="storePrincipal_Refresh"
            RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="NotificacionID">
                    <Fields>
                        <ext:ModelField Name="NotificacionID" Type="Int" />
                        <ext:ModelField Name="Notificacion" />
                        <ext:ModelField Name="Asunto" />
                        <ext:ModelField Name="Contenido" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="FechaCreacion" Type="Date" />
                        <ext:ModelField Name="Contenido" />
                        <ext:ModelField Name="Nombre" />
                        <ext:ModelField Name="Apellidos" />
                        <ext:ModelField Name="EMail" />
                        <ext:ModelField Name="NombreCompleto" />
                        <ext:ModelField Name="NotificacionGrupoID" Type="Int" />
                        <ext:ModelField Name="NotificacionCadenciaID" Type="Int" />
                        <ext:ModelField Name="NotificacionGrupoCriterio" />
                        <ext:ModelField Name="NotificacionCadencia" />
                        <ext:ModelField Name="FechaActivacion" Type="Date" />
                        <ext:ModelField Name="FechaDesactivacion" Type="Date" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Tabla" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="Notificacion"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store
            runat="server"
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
                <ext:Model
                    runat="server"
                    IDProperty="NotificacionCorreoID">
                    <Fields>
                        <ext:ModelField Name="NotificacionCorreoID" Type="Int" />
                        <ext:ModelField Name="NotificacionID" Type="Int" />
                        <ext:ModelField Name="Correo" />
                        <ext:ModelField Name="Activo" />
                        <ext:ModelField Name="PerfilID" Type="Int" />
                        <ext:ModelField Name="Perfil_esES" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="Correo"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store
            ID="storePerfiles"
            runat="server"
            AutoLoad="false"
            OnReadData="storePerfiles_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    IDProperty="PerfilID"
                    runat="server">
                    <Fields>
                        <ext:ModelField Name="PerfilID" Type="Int" />
                        <ext:ModelField Name="Perfil_esES" Type="string" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="Perfil_esES"
                    Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store
            ID="storeProyectosTipos"
            runat="server"
            AutoLoad="false"
            OnReadData="storeProyectosTipos_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    IDProperty="ProyectoTipoID"
                    runat="server">
                    <Fields>
                        <ext:ModelField Name="ProyectoTipoID" />
                        <ext:ModelField Name="ProyectoTipo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store
            ID="storeNotificacionesGruposCriterios"
            runat="server"
            AutoLoad="false"
            OnReadData="storeNotificacionesGruposCriterios_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    IDProperty="NotificacionGrupoCriterioID"
                    runat="server">
                    <Fields>
                        <ext:ModelField Name="NotificacionGrupoCriterioID" />
                        <ext:ModelField Name="NotificacionGrupoCriterio" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store
            ID="storeNotificacionesCadencias"
            runat="server"
            AutoLoad="false"
            OnReadData="storeNotificacionesCadencias_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    IDProperty="NotificacionCadenciaID"
                    runat="server">
                    <Fields>
                        <ext:ModelField Name="NotificacionCadenciaID" />
                        <ext:ModelField Name="NotificacionCadencia" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>

        <%-- INICIO WINDOWS --%>

        <ext:Window
            runat="server"
            ID="winGestion"
            meta:resourceKey="winGestion"
            Title="Agregar"
            Width="400"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel
                    runat="server"
                    ID="formGestion"
                    Cls="form-gestion"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:DateField
                            ID="txtFechaActivacion"
                            runat="server"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            FieldLabel="<%$ Resources:Comun, strFechaActivacion %>"
                            meta:resourceKey="txtFechaActivacion" />
                        <ext:DateField
                            ID="txtFechaDesactivacion"
                            runat="server"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            FieldLabel="<%$ Resources:Comun, strFechaDesinstalacion %>"
                            meta:resourceKey="txtFechaDesactivacion" />
                        <ext:TextField
                            ID="txtNotificacion"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            FieldLabel="<%$ Resources:Comun, strNotificacion %>"
                            meta:resourceKey="txtNotificacion" />
                        <ext:ComboBox
                            ID="cmbGrupo"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strGrupo %>"
                            Mode="Local"
                            DisplayField="NotificacionGrupoCriterio"
                            ValueField="NotificacionGrupoCriterioID"
                            StoreID="storeNotificacionesGruposCriterios"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="false"
                            AllowBlank="false"
                            meta:resourceKey="cmbGrupo">
                            <Listeners>
                                <Select Fn="SeleccionarGrupo" />
                                <TriggerClick Fn="RecargarGrupos" />
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
                        <ext:ComboBox
                            ID="cmbCadencias"
                            runat="server"
                            FieldLabel="<%$ Resources:Comun, strCadencia %>"
                            Mode="Local"
                            DisplayField="NotificacionCadencia"
                            ValueField="NotificacionCadenciaID"
                            StoreID="storeNotificacionesCadencias"
                            Editable="false"
                            AllowBlank="false"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            meta:resourceKey="cmbCadencias">
                            <Listeners>
                                <Select Fn="SeleccionarCadencia" />
                                <TriggerClick Fn="RecargarCadencias" />
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
                            ID="txtAsunto"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            FieldLabel="<%$ Resources:Comun, strAsunto %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtAsunto" />
                        <ext:TextArea
                            ID="txaCuerpo"
                            runat="server"
                            Text=""
                            MaxLength="1000"
                            FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            Height="100"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txaCuerpo" />
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
                    Disabled="true"
                    IconCls="ico-accept"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>

            </Buttons>
        </ext:Window>

        <ext:Window
            runat="server"
            ID="winGestionDetalle"
            meta:resourceKey="winGestionDetalle"
            Width="420"
            Resizable="false"
            Modal="true"
            Hidden="true"
            Title="Agregar">
            <Items>
                <ext:FormPanel
                    runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField
                            ID="txtCorreo"
                            runat="server"
                            Text=""
                            MaxLength="100"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            FieldLabel="<%$ Resources:Comun, strEmail %>"
                            ValidationGroup="FORM"
                            AllowBlank="false"
                            CausesValidation="true"
                            Regex="<%$ Resources:Comun, regexEmail %>"
                            meta:resourceKey="txtCorreo">
                        </ext:TextField>
                        <ext:ComboBox
                            ID="cmbProyectosTipos"
                            runat="server"
                            Mode="Local"
                            DisplayField="ProyectoTipo"
                            ValueField="ProyectoTipoID"
                            StoreID="storeProyectosTipos"
                            FieldLabel="<%$ Resources:Comun, strProyectoTipo %>"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="true"
                            meta:resourceKey="cmbProyectosTipos"
                            Hidden="false"
                            AllowBlank="true">
                            <Listeners>
                                <Select Fn="SeleccionarProyectoTipo" />
                                <TriggerClick Fn="RecargarProyectosTipos" />
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
                        <ext:ComboBox
                            ID="cmbPerfiles"
                            runat="server"
                            Mode="Local"
                            DisplayField="Perfil_esES"
                            ValueField="PerfilID"
                            StoreID="storePerfiles"
                            FieldLabel="<%$ Resources:Comun, strPerfiles %>"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            Editable="false"
                            meta:resourceKey="cmbPerfiles"
                            Hidden="false"
                            AllowBlank="true">
                            <Listeners>
                                <Select Fn="SeleccionarPerfil" />
                                <TriggerClick Fn="RecargarPerfiles" />
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
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button
                    runat="server"
                    ID="btnCancelarDetalle"
                    meta:resourceKey="btnCancelarDetalle"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button
                    runat="server"
                    ID="btnGuardarDetalle"
                    meta:resourceKey="btnGuardarDetalle"
                    Disabled="true"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
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
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport
            runat="server"
            ID="vwContenedor"
            Cls="vwContenedor"
            Layout="Anchor">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
                    AnchorHorizontal="-100"
                    AnchorVertical="58%"
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
                                    ToolTip="Añadir"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="Editar"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="Eliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button
                                    runat="server"
                                    ID="btnActivar"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="Activar"
                                    Cls="btnActivar"
                                    Handler="Activar()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="Refrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button
                                    runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="Descargar"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('NotificacionesGestion', hdCliID.value, #{gridMaestro}, '-1');" />
                                <ext:Button
                                    runat="server"
                                    ID="btnEnviar"
                                    meta:resourceKey="btnEnviar"
                                    ToolTip="Enviar"
                                    Cls="btnEnviar"
                                    Handler="BotonNotificar()" />
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
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="colActivoMaestro"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                DataIndex="Notificacion"
                                Width="200"
                                meta:resourceKey="columNotificacion"
                                ID="colNotificacion"
                                Header="<%$ Resources:Comun, strNotificacion %>"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="Asunto"
                                Width="200"
                                meta:resourceKey="columAsunto"
                                ID="colAsunto"
                                Header="<%$ Resources:Comun, strAsunto %>"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="Contenido"
                                Width="200"
                                meta:resourceKey="columContenido"
                                ID="colContenido"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="NotificacionGrupoCriterio"
                                Width="100"
                                meta:resourceKey="columNotificacionGrupoCriterio"
                                ID="colNotificacionGrupoCriterio"
                                Header="<%$ Resources:Comun, strGrupo %>"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="NotificacionCadencia"
                                Width="100"
                                meta:resourceKey="columNotificacionCadencia"
                                ID="colNotificacionCadencia"
                                Header="<%$ Resources:Comun, strCadencia %>"
                                runat="server"
                                Flex="1" />
                            <ext:DateColumn
                                DataIndex="FechaDesactivacion"
                                Width="100"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columFechaDesactivacion"
                                Header="<%$ Resources:Comun, strFechaDesinstalacion %>"
                                ID="colFechaDesactivacion"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="NombreCompleto"
                                Width="100"
                                meta:resourceKey="columNombreCompleto"
                                ID="colNombreCompleto"
                                runat="server"
                                Flex="1" />
                            <ext:DateColumn
                                DataIndex="FechaCreacion"
                                Width="100"
                                Format="<%$ Resources:Comun, FormatFecha %>"
                                meta:resourceKey="columFechaCreacion"
                                ID="colFechaCreacion"
                                runat="server"
                                Flex="1" />
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
                            ID="PagingToolBar1"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storePrincipal"
                            DisplayInfo="true"
                            HideRefresh="true">
                            <Items>
                                <ext:ComboBox
                                    runat="server"
                                    Cls="comboGrid"
                                    Width="80">
                                    <Items>
                                        <ext:ListItem Text="1" />
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
                    AnchorHorizontal="-100"
                    AnchorVertical="42%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar
                            runat="server"
                            ID="tlbDetalle"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="btnAnadirDetalle"
                                    meta:resourceKey="btnAnadirDetalle"
                                    ToolTip="Añadir"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="Editar"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="Eliminar"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="Activar"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="Refrescar"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button
                                    runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="Descargar"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('NotificacionesGestion', #{GridDetalle}, #{hdModuloID}.value);" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column
                                runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column
                                DataIndex="NotificacionCorreoID"
                                ID="colNotificacionCorreoID"
                                Hidden="true"
                                Width="10"
                                meta:resourceKey="columNotificacionCorreoID"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="Correo"
                                Width="180"
                                meta:resourceKey="columCorreo"
                                ID="colCorreo"
                                runat="server"
                                Flex="1" />
                            <ext:Column
                                DataIndex="Perfil_esES"
                                Width="200"
                                meta:resourceKey="columPerfil"
                                ID="colPerfil_esES"
                                runat="server"
                                Flex="1" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel
                            runat="server"
                            ID="GridRowSelectDetalle"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect_Detalle" />
                                <Deselect Fn="DeseleccionarGrillaDetalle" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters
                            runat="server"
                            ID="gridFilters2"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="gridFilters2" />
                        <ext:CellEditing
                            runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar
                            runat="server"
                            ID="PagingToolBar2"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storeDetalle"
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
