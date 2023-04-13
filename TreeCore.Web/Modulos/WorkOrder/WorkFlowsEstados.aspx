<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkFlowsEstados.aspx.cs" Inherits="TreeCore.ModWorkFlow.WorkFlowsEstados" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body class="bodyFMenu">

    <link rel="stylesheet" type="text/css" href="/Scripts/slick.css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/slick-theme.css" />
    <link href="../../CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.amsify.suggestags.js"></script>
    <script type="text/javascript" src="/Scripts/slick.min.js"></script>
    <link href="css/styleWorkFlows.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdProyectoParametro" runat="server" />
            <ext:Hidden ID="hdDescargaExcel" runat="server" />
            <ext:Hidden ID="hdWorkflows" runat="server" />
            <ext:Hidden ID="hdEstadoID" runat="server" />
            <ext:Hidden ID="hdEstado" runat="server" />
            <ext:Hidden ID="hdRolSeleccionado" runat="server" />

            <%--FIN HIDDEN --%>

            <%-- INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeCoreEstados"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCoreEstados_Refresh"
                RemoteSort="true"
                PageSize="20"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="CoreEstadoID,CoreWorkflowID,Defecto,Completado,EstadosSiguientes,EstadosGlobales,DepartamentoID,EstadoAgrupacionID,TieneDocumento,TieneRol,TieneNotificacion,PublicoLectura,PublicoEscritura">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoID" Type="Int" />
                            <ext:ModelField Name="NombreEstado" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="CoreWorkflowID" Type="Int" />
                            <ext:ModelField Name="Porcentaje" Type="Int" />
                            <ext:ModelField Name="Departamento" Type="String" />
                            <ext:ModelField Name="DepartamentoID" Type="Int" />
                            <ext:ModelField Name="EstadoAgrupacionID" Type="Int" />
                            <ext:ModelField Name="NombreAgrupacion" Type="String" />
                            <ext:ModelField Name="EstadosSiguientes" Type="String" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Completado" Type="Boolean" />
                            <ext:ModelField Name="TieneRol" Type="Boolean" />
                            <ext:ModelField Name="TieneNotificacion" Type="Boolean" />
                            <ext:ModelField Name="EstadosGlobales" Type="String" />
                            <ext:ModelField Name="Descripcion" Type="String" />
                            <ext:ModelField Name="PublicoLectura" Type="Boolean" />
                            <ext:ModelField Name="PublicoEscritura" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Porcentaje" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="cargarCombo" />
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosRoles"
                AutoLoad="false"
                OnReadData="storeCoreEstadosRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoRolID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoRolID" Type="Int" />
                            <ext:ModelField Name="CodigoEstado" Type="String" />
                            <ext:ModelField Name="NombreEstado" Type="String" />
                            <ext:ModelField Name="CodigoRol" Type="String" />
                            <ext:ModelField Name="NombreRol" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="CodigoRol" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeRolesLibres"
                AutoLoad="false"
                OnReadData="storeRolesLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
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

            <ext:Store runat="server"
                ID="storeRoles"
                AutoLoad="false"
                OnReadData="storeRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
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

            <ext:Store runat="server"
                ID="storeTareasEstadosAsignados"
                AutoLoad="false"
                OnReadData="storeTareasEstadosAsignados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoRolEscrituraID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoRolEscrituraID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoID" Type="Int" />
                            <ext:ModelField Name="RolID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <%--<Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>--%>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeRolesEstadosSeguimiento"
                AutoLoad="false"
                OnReadData="storeRolesEstadosSeguimiento_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoRolLecturaID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoRolLecturaID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoID" Type="Int" />
                            <ext:ModelField Name="RolID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <%--<Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>--%>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosGlobales"
                AutoLoad="false"
                OnReadData="storeCoreEstadosGlobales_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="ID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoGlobalID" Type="Int" />
                            <ext:ModelField Name="Tabla" Type="String" />
                            <ext:ModelField Name="Estado" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tabla" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeEstadosGlobales"
                AutoLoad="false"
                OnReadData="storeEstadosGlobales_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="ID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeInventarioElementosAtributosEstados"
                AutoLoad="false"
                OnReadData="storeInventarioElementosAtributosEstados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="ID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeDocumentosEstados"
                AutoLoad="false"
                OnReadData="storeDocumentosEstados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="ID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreWorkflows"
                OnReadData="storeCoreWorkflows_Refresh"
                AutoLoad="true"
                RemoteSort="true"
                RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreWorkFlowID">
                        <Fields>
                            <ext:ModelField Name="CoreWorkFlowID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Handler="RecargarWorkflows();" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosSiguientes"
                OnReadData="storeCoreEstadosSiguientes_Refresh"
                AutoLoad="false"
                RemoteSort="true"
                RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoSiguienteID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoSiguienteID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoPosibleID" Type="Int" />
                            <ext:ModelField Name="NombreEstado" Type="String" />
                            <ext:ModelField Name="CodigoEstado" Type="String" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreEstado" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosSiguientesLibres"
                OnReadData="storeCoreEstadosSiguientesLibres_Refresh"
                AutoLoad="false"
                RemoteSort="true"
                RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoSiguienteID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoSiguienteID" Type="Int" />
                            <ext:ModelField Name="NombreEstado" Type="String" />
                            <ext:ModelField Name="CodigoEstado" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreEstado" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="cargarCombo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeUsuarios"
                AutoLoad="false"
                OnReadData="storeUsuarios_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="UsuarioID">
                        <Fields>
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="EMail" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EMail" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosNotificacionesRoles"
                AutoLoad="false"
                OnReadData="storeCoreEstadosNotificacionesRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoNotificacionRolID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoNotificacionRolID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoNotificacionID" Type="Int" />
                            <ext:ModelField Name="RolID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="RolID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosNotificacionesUsuarios"
                AutoLoad="false"
                OnReadData="storeCoreEstadosNotificacionesUsuarios_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoNotificacionUsuarioID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoNotificacionUsuarioID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoNotificacionID" Type="Int" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="UsuarioID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosNotificaciones"
                AutoLoad="false"
                OnReadData="storeCoreEstadosNotificaciones_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoNotificacionID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoNotificacionID" Type="Int" />
                            <ext:ModelField Name="CoreEstadoID" Type="Int" />
                            <ext:ModelField Name="Contenido" Type="String" />
                            <ext:ModelField Name="Asunto" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="CoreEstadoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreObjetosNegocioTipos"
                AutoLoad="false"
                OnReadData="storeCoreObjetosNegocioTipos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreObjetoNegocioTipoID">
                        <Fields>
                            <ext:ModelField Name="CoreObjetoNegocioTipoID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreObjeto" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreWorkflowsInformaciones"
                AutoLoad="false"
                OnReadData="storeCoreWorkflowsInformaciones_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ID">
                        <Fields>
                            <ext:ModelField Name="ID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreTiposInformacionesAcciones"
                AutoLoad="false"
                OnReadData="storeCoreTiposInformacionesAcciones_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreTareaAccionID">
                        <Fields>
                            <ext:ModelField Name="CoreTareaAccionID" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClaveRecurso" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreEstadosTareas"
                AutoLoad="false"
                OnReadData="storeCoreEstadosTareas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoTareaID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoTareaID" Type="Int" />
                            <ext:ModelField Name="Informacion" Type="String" />
                            <ext:ModelField Name="Accion" Type="String" />
                            <ext:ModelField Name="Obligatorio" Type="Boolean" />
                            <ext:ModelField Name="Descripcion" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Informacion" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%-- FIN STORES --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window ID="winAddTabFilter"
                runat="server"
                Title="Add Tab"
                Height="195"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout">

                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>

                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar7" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="backNewUsers" Cls="btn-secondary " MinWidth="110" Text="Cancel" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="nextNewUsers" Cls="btn-ppal " Text="Save Tab" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="txtNewTabName" LabelAlign="Top" FieldLabel="Tab Name" WidthSpec="90%"></ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window ID="winSaveQF"
                runat="server"
                Title="Save Quick Filter"
                Height="195"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout">

                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>

                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar3" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="Button3" Cls="btn-secondary " MinWidth="110" Text="Cancel" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="Button4" Cls="btn-ppal " Text="Save Quick Filter" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="TextField1" LabelAlign="Top" FieldLabel="Filter Name" WidthSpec="90%"></ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window ID="winDuplicarWorkflow"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                AutoHeight="true"
                Modal="true"
                Resizable="false"
                Hidden="true">
                <Items>
                    <ext:FormPanel ID="formDuplicarWorkflow"
                        runat="server"
                        BodyStyle="padding:20px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox
                                ID="cmbDuplicateWorkflow"
                                runat="server"
                                StoreID="storeCoreWorkflows"
                                Mode="Local"
                                DisplayField="Codigo"
                                ValueField="CoreWorkFlowID"
                                ValidationGroup="FORM"
                                EmptyCls="cmbDepartament"
                                FieldBodyCls="cmbDepartament"
                                CausesValidation="true"
                                FieldLabel="<%$ Resources:Comun, strTipologias %>"
                                AllowBlank="false"
                                QueryMode="Local"
                                Anchor="95%">
                                <Listeners>
                                    <Select Fn="SeleccionarWorkflo" />
                                    <TriggerClick Fn="RecargarWorkflo" />
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
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoDuplicarWorkflow(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnGuardarDuplicarWorkflowCerrar"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winDuplicarWorkflow}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnGuardarDuplicarWorkflow"
                        runat="server"
                        Cls="btn-accept"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="DuplicarWorkFlow();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winDuplicarWorkflow}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                runat="server"
                ID="WinEstadosSiguientesDetails"
                Width="320"
                Height="300"
                MinHeight="220"
                MaxHeight="300"
                Hidden="true"
                Title="<%$ Resources:Comun, strEstadoSiguiente %>"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                Layout="FitLayout"
                OverflowY="Auto">
                <Listeners>
                    <FocusLeave Fn="HideWinEstadosSiguientes" />
                </Listeners>
                <Items>
                    <ext:GridPanel
                        ID="GridEstadosSiguientesAsig"
                        runat="server"
                        ForceFit="true"
                        Border="false"
                        EnableColumnHide="false"
                        Header="false"
                        Cls="gridPanel ExtraHeightGrid"
                        OverflowY="Auto">
                        <Store>
                            <ext:Store
                                ID="storeCoreEstadosAsignados"
                                runat="server"
                                AutoLoad="false">
                                <Model>
                                    <ext:Model runat="server" IDProperty="CoreEstadoID">
                                        <Fields>
                                            <ext:ModelField Name="Codigo" />
                                            <ext:ModelField Name="Nombre" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                                </Sorters>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    DataIndex="Codigo"
                                    Flex="1">
                                    <Filter>
                                        <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                                    </Filter>
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    DataIndex="Nombre"
                                    Flex="1">
                                    <Filter>
                                        <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                                    </Filter>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView
                                runat="server"
                                EnableTextSelection="true" />
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <ext:Window
                runat="server"
                ID="WinEstadosGlobalesDetails"
                Width="320"
                Height="300"
                MinHeight="220"
                MaxHeight="300"
                Hidden="true"
                Title="<%$ Resources:Comun, strEstadoGlobalTitulo %>"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                Layout="FitLayout"
                OverflowY="Auto">
                <Listeners>
                    <FocusLeave Fn="HideWinEstadosGlobales" />
                </Listeners>
                <Items>
                    <ext:GridPanel
                        ID="GridEstadosGlobalesAsig"
                        runat="server"
                        ForceFit="true"
                        Border="false"
                        EnableColumnHide="false"
                        Header="false"
                        Cls="gridPanel ExtraHeightGrid"
                        OverflowY="Auto">
                        <Store>
                            <ext:Store
                                ID="storeCoreEstadosGlobalesAsig"
                                runat="server"
                                AutoLoad="false">
                                <Model>
                                    <ext:Model runat="server" IDProperty="CoreEstadoGlobalID">
                                        <Fields>
                                            <ext:ModelField Name="Estado" />
                                            <ext:ModelField Name="Codigo" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter Property="Estado" Direction="ASC" />
                                </Sorters>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>

                                <ext:Column
                                    runat="server"
                                    Text="<%$ Resources:Comun, strObjeto %>"
                                    DataIndex="Codigo"
                                    Flex="1">
                                    <Filter>
                                        <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                                    </Filter>
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    Text="<%$ Resources:Comun, strEstado %>"
                                    DataIndex="Estado"
                                    Flex="1">
                                    <Filter>
                                        <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                                    </Filter>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView
                                runat="server"
                                EnableTextSelection="true" />
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <ext:Window runat="server"
                ID="winGestionEstados"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                MaxWidth="750"
                HeightSpec="80vh"
                MaxHeight="600"
                Hidden="true"
                OverflowY="Auto"
                Layout="FitLayout"
                Cls="winForm-resp">
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="tbVistasFormEstados"
                        Cls="pnNavVistas pnVistasForm"
                        Dock="Top"
                        Padding="20"
                        OverflowHandler="Scroller">
                        <Items>
                            <ext:Container runat="server"
                                ID="cntNavVistasFormEstados"
                                Cls="nav-vistas"
                                ActiveIndex="2"
                                ActiveItem="2">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormState"
                                        Value="3"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strEstados %>">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormGlobalSTS"
                                        Value="4"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="OPERAT STS">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <%--<ext:HyperlinkButton runat="server"
                                        ID="lnkFormSubprocess"
                                        Cls="lnk-navView lnk-noLine"
                                        Value="5"
                                        Text="SUBPR">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>--%>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormNextSTS"
                                        Value="6"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="NEXT STS">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <%--<ext:HyperlinkButton runat="server"
                                        ID="lnkFormLinks"
                                        Value="7"
                                        Cls="lnk-navView  lnk-noLine"
                                        Text="LINKS">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>--%>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormDocs"
                                        Value="8"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="TASK">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormProfile"
                                        Value="9"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="ROLS">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFormNotification"
                                        Cls="lnk-navView  lnk-noLine"
                                        Value="10"
                                        Text="Notifications">
                                        <Listeners>
                                            <Click Fn="showFormsEstados"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="containerFormEstados"
                        Cls="formGris formResp"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="formState"
                                Hidden="false"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2 navActivo">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Cls="item-form"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <Listeners>
                                            <Change Fn="FormularioValido" />
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
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextArea
                                        ID="txtDescripcion"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        LabelAlign="Top"
                                        Width="220"
                                        Mode="local"
                                        MaxLength="250"
                                        Scrollable="Vertical"
                                        RawText=""
                                        AllowBlank="true"
                                        WidthSpec="90%"
                                        Cls="txtMensajeStatus"
                                        Editable="true">
                                        <Listeners>
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                    </ext:TextArea>
                                    <ext:ComboBox runat="server"
                                        ID="cmbDepartamento"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strDepartamento %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbDepartament"
                                        FieldBodyCls="cmbDepartament"
                                        DisplayField="Departamento"
                                        ValueField="DepartamentoID"
                                        AllowBlank="false"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeDepartamentos"
                                                AutoLoad="false"
                                                OnReadData="storeDepartamentos_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="DepartamentoID">
                                                        <Fields>
                                                            <ext:ModelField Name="DepartamentoID" Type="Int" />
                                                            <ext:ModelField Name="Departamento" Type="String" />
                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Departamento" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarDepartamento" />
                                            <TriggerClick Fn="RecargarDepartamento" />
                                            <Change Fn="FormularioValido" />
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
                                        ID="cmbGrupos"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strGrupo %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbGroup"
                                        AllowBlank="false"
                                        FieldBodyCls="cmbGroup"
                                        DisplayField="Codigo"
                                        ValueField="EstadoAgrupacionID"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeEstadosAgrupaciones"
                                                AutoLoad="false"
                                                OnReadData="storeEstadosAgrupaciones_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="EstadoAgrupacionID">
                                                        <Fields>
                                                            <ext:ModelField Name="EstadoAgrupacionID" Type="Int" />
                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                            <ext:ModelField Name="Nombre" Type="String" />
                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                            <ext:ModelField Name="Defecto" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarAgrupacion" />
                                            <TriggerClick Fn="RecargarAgrupacion" />
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:FieldContainer runat="server"
                                        ID="chkCompleto"
                                        Cls="chkCompleto"
                                        Height="100"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Checkbox runat="server"
                                                ID="chkCompletado"
                                                BoxLabel="State as complete"
                                                Cls="chk-form"
                                                Width="200" />
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:Container runat="server"
                                        ID="ctProgress"
                                        Cls="ctForm-mini-col4">
                                        <Items>
                                            <ext:NumberField runat="server"
                                                ID="txtProgress"
                                                FieldLabel="<%$ Resources:Comun, strPorcentaje %>"
                                                MinValue="0"
                                                MaxValue="100"
                                                AllowBlank="false"
                                                MaxLength="3"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                LabelAlign="Top"
                                                Cls="item-form numEstados"
                                                MaxWidth="65"
                                                ValidationGroup="FORM">
                                                <Listeners>
                                                    <Change Handler="ProgressBarForm();" />
                                                </Listeners>
                                            </ext:NumberField>
                                            <ext:ProgressBar runat="server"
                                                ID="progressBar"
                                                Text=""
                                                Value="0.5"
                                                MinWidth="95"
                                                MinHeight="20"
                                                Cls="item-form progressBar">
                                            </ext:ProgressBar>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formGlobalSTS"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbObject"
                                        Cls=""
                                        FieldLabel="Object"
                                        LabelAlign="Top"
                                        EmptyCls="cmbObject"
                                        FieldBodyCls="cmbObject"
                                        QueryMode="Local"
                                        DisplayField="Codigo"
                                        StoreID="storeCoreObjetosNegocioTipos"
                                        ValueField="CoreObjetoNegocioTipoID"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
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
                                        SelectionMemory="false"
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        MinHeight="200"
                                        MaxHeight="200"
                                        OverflowY="Auto"
                                        StoreID="storeCoreEstadosGlobales"
                                        Cls="grdFormElAdded columGridFinal">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colObjetoNegocio"
                                                    Text="Object"
                                                    DataIndex="Tabla"
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colEstadoGlobal"
                                                    Text="<%$ Resources:Comun, strEstadoGlobal %>"
                                                    DataIndex="Estado"
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
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <%--<ext:Container runat="server"
                                ID="formSubprocess"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbSubprocesos"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strSubproceso %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbSubprocess"
                                        FieldBodyCls="cmbSubprocess"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarSubproceso" />
                                            <TriggerClick Fn="RecargarSubproceso" />
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
                                        ID="cmbWorkFlow"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strVerWorkflow %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbWorkflow"
                                        FieldBodyCls="cmbWorkflow"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarWorkflow" />
                                            <TriggerClick Fn="RecargarWorkflow" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="btnAnadirSubProceso"
                                        Width="100"
                                        Height="32"
                                        Text="Add"
                                        IconCls="ico-addBtn"
                                        Cls="btn-mini-ppal ultimaColumna">
                                        <Listeners>
                                            <Click Handler="" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridSubprocesos"
                                        MinHeight="200"
                                        MaxHeight="200"
                                        StoreID=""
                                        OverflowY="Auto"
                                        Scrollable="Vertical"
                                        Cls="grdFormElAdded columGridFinal">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colSubproceso"
                                                    Text="<%$ Resources:Comun, strSubproceso %>"
                                                    DataIndex=""
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colWorkflow"
                                                    Text="<%$ Resources:Comun, strVerWorkflow %>"
                                                    DataIndex=""
                                                    Flex="4">
                                                </ext:Column>
                                                <ext:CommandColumn runat="server" Width="50" Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Eliminar" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>--%>
                            <ext:Container runat="server"
                                ID="formNextSTS"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbName"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strNombreCodigo %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbName"
                                        DisplayField="NombreEstado"
                                        ValueField="CoreEstadoSiguienteID"
                                        StoreID="storeCoreEstadosSiguientesLibres"
                                        FieldBodyCls="cmbName"
                                        ValidationGroup="FORM"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <ListConfig>
                                            <ItemTpl runat="server">
                                                <Html>
                                                    <div class="item-form">
                                                        <p>{NombreEstado}</p>
                                                    </div>
                                                </Html>
                                            </ItemTpl>
                                        </ListConfig>
                                        <Listeners>
                                            <Select Fn="SeleccionarEstadoSiguiente" />
                                            <TriggerClick Fn="RecargarEstadoSiguiente" />
                                            <Change Fn="FormularioValidoEstadoSiguiente" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="btnAnadirEstadosSiguientes"
                                        Width="100"
                                        Disabled="true"
                                        Height="32"
                                        Text="<%$ Resources:Comun, winGestion.Title %>"
                                        IconCls="ico-addBtn"
                                        Cls="btn-mini-ppal btnAnadirEstadosSiguientes">
                                        <Listeners>
                                            <Click Fn="btnAgregarEstadoSiguiente" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridEstadosSiguientes"
                                        MinHeight="200"
                                        MaxHeight="200"
                                        SelectionMemory="false"
                                        EnableColumnHide="false"
                                        Scrollable="Vertical"
                                        OverflowY="Auto"
                                        StoreID="storeCoreEstadosSiguientes"
                                        Cls="grdFormElAdded columGridFinal">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colName"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    DataIndex="NombreEstado"
                                                    Flex="3">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    DataIndex="Defecto"
                                                    Align="Center"
                                                    Text="<%$ Resources:Comun, strDefecto %>"
                                                    ID="colDefault"
                                                    Flex="2">
                                                    <Renderer Fn="DefaultRender" />
                                                </ext:Column>
                                                <ext:CommandColumn runat="server"
                                                    MinWidth="50"
                                                    Flex="1"
                                                    Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Defecto"
                                                            IconCls="btn-edit-item">
                                                        </ext:GridCommand>
                                                        <ext:GridCommand CommandName="Eliminar"
                                                            IconCls="ico-close">
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="CambiarEstadoSiguiente" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectEstadosSiguientes"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectEstadosGlobales" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFiltersEstadosSiguientes"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <%--<ext:Container runat="server"
                                ID="formLinks"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtWorkflowProyecto"
                                        FieldLabel="Workflow / Proyecto"
                                        LabelAlign="Top"
                                        Cls="dosColumnasInicio" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbEstado"
                                        meta:resourceKey="cmbEstado"
                                        Cls=""
                                        FieldLabel="<%$ Resources:Comun, strEstado %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbEstado"
                                        FieldBodyCls="cmbEstado"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="btnAddLink"
                                        Width="100"
                                        Height="32"
                                        Text="Add"
                                        IconCls="ico-addBtn"
                                        Cls="btn-mini-ppal ultimaColumna">
                                        <Listeners>
                                            <Click Handler="" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridLinks"
                                        MinHeight="200"
                                        MaxHeight="200"
                                        StoreID=""
                                        Scrollable="Vertical"
                                        OverflowY="Auto"
                                        Cls="grdFormElAdded tresColumnas">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colWorkflowProject"
                                                    Text="Workflow / Proyecto"
                                                    DataIndex=""
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colEstado"
                                                    Text="<%$ Resources:Comun, strEstado %>"
                                                    DataIndex=""
                                                    Flex="3">
                                                </ext:Column>
                                                <ext:CommandColumn runat="server" Width="50" Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Eliminar" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>--%>
                            <ext:Container runat="server"
                                ID="formDocs"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbInformaciones"
                                        DisplayField="Nombre"
                                        ValueField="Codigo"
                                        FieldLabel="<%$ Resources:Comun, jsInfo %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbObject"
                                        FieldBodyCls="cmbObject"
                                        QueryMode="Local"
                                        StoreID="storeCoreWorkflowsInformaciones"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarInfo" />
                                            <TriggerClick Fn="RecargarInfo" />
                                            <Change Fn="FormularioValidoTareas" />
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
                                        runat="server"
                                        ID="cmbTareasAcciones"
                                        DisplayField="ClaveRecurso"
                                        ValueField="CoreTareaAccionID"
                                        FieldLabel="<%$ Resources:Comun, strAccion %>"
                                        LabelAlign="Top"
                                        EmptyCls="cmbAcciones"
                                        FieldBodyCls="cmbAcciones"
                                        QueryMode="Local"
                                        Disabled="true"
                                        StoreID="storeCoreTiposInformacionesAcciones"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="seleccionarAccion" />
                                            <TriggerClick Fn="RecargarAcciones" />
                                            <Change Fn="FormularioValidoTareas" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:TextArea runat="server"
                                        ID="txtDescripcionTarea"
                                        LabelAlign="Top"
                                        Cls="dosFilas txtAreaTask"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        MaxLength="150"
                                        AllowBlank="true">
                                        <Listeners>
                                            <Change Fn="FormularioValidoTareas" />
                                        </Listeners>
                                    </ext:TextArea>
                                    <ext:FieldContainer runat="server"
                                        ID="chkMandatory"
                                        Cls="chkCompleto"
                                        Height="100"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Checkbox runat="server"
                                                ID="chkMandat"
                                                BoxLabel="<%$ Resources:Comun, strObligatorio %>"
                                                Cls="chk-form"
                                                Width="200" >
                                                <Listeners>
                                                    <Change Fn="FormularioValidoTareas" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:Button runat="server"
                                        ID="btnAnadirObjeto"
                                        Width="100"
                                        Height="32"
                                        Disabled="true"
                                        Text="<%$ Resources:Comun, winGestion.Title %>"
                                        IconCls="ico-addBtn"
                                        Cls="btn-mini-ppal btnAnadirEstadosSiguientes">
                                        <Listeners>
                                            <Click Fn="btnAgregarObjeto" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridObjetos"
                                        MinHeight="200"
                                        MaxHeight="200"
                                        StoreID="storeCoreEstadosTareas"
                                        Cls="grdFormElAdded tresColumnas"
                                        SelectionMemory="false"
                                        Scrollable="Vertical"
                                        OverflowY="Auto"
                                        EnableColumnHide="false">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colInfo"
                                                    Text="<%$ Resources:Comun, jsInfo %>"
                                                    DataIndex="Informacion"
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colAccion"
                                                    Text="<%$ Resources:Comun, strAccion %>"
                                                    DataIndex="Accion"
                                                    Flex="5">
                                                </ext:Column>
                                                <ext:TemplateColumn runat="server"
                                                    DataIndex=""
                                                    MenuDisabled="true"
                                                    Align="Center"
                                                    Text="<%$ Resources:Comun, strObligatorio %>"
                                                    Flex="4">
                                                    <Template runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                                <div style="float: left;">
                                                                    <p style="line-height: 10px;" class="titlespan3">
                                                                        {Obligatorio}
                                                                    </p>
                                                            </tpl>
                                                        </Html>
                                                    </Template>
                                                    <Renderer Fn="RequiereRender" />
                                                </ext:TemplateColumn>
                                                <ext:CommandColumn runat="server"
                                                    Width="50"
                                                    Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="Eliminar"
                                                            IconCls="ico-close">
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarObjeto" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectObjetos"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectObjetos" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFiltersObjetos"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formProfile"
                                Hidden="true"
                                OverflowY="Hidden"
                                PaddingSpec="0 24px"
                                Cls="winGestion-panel">
                                <Items>
                                    <%--<ext:ComboBox runat="server"
                                        ID="cmbRoles"
                                        Cls="dosColumnasInicio"
                                        FieldLabel="Rols"
                                        LabelAlign="Top"
                                        DisplayField="Codigo"
                                        ValueField="RolID"
                                        StoreID="storeRolesLibres"
                                        EmptyCls="cmbModulo"
                                        FieldBodyCls="cmbModulo"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarRol" />
                                            <TriggerClick Fn="RecargarRol" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>--%>

                                    <ext:Container runat="server"
                                        Cls="dispFlex"
                                        WidthSpec="100%">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblRolesTareasEstados"
                                                Cls="lblHeadForm"
                                                MarginSpec="0 29"
                                                Text="<%$ Resources:Comun, strAsignarRolesTareasEstados %>">
                                            </ext:Label>
                                            <ext:Container runat="server"
                                                Cls="ctToggle ctRolesWF">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="labelPrivado"
                                                        Cls="lbLabel"
                                                        Text="<%$ Resources:Comun, jsPrivado %>" />
                                                    <ext:Button runat="server"
                                                        EnableToggle="true"
                                                        ID="btnWorkFlowPublicRolEscritura"
                                                        Cls="btnActivarDesactivarV2"
                                                        PressedCls="btnActivarDesactivarV2Pressed"
                                                        MinWidth="50"
                                                        Pressed="true"
                                                        Focusable="false"
                                                        OverCls="none">
                                                        <Listeners>
                                                            <Click Fn="tglRestWorkFlowEscritura" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Label runat="server"
                                                        ID="labelPublico"
                                                        Cls="lbLabel"
                                                        PaddingSpec="5 0 0 7"
                                                        Disabled="true"
                                                        Text="<%$ Resources:Comun, jsPublico %>" />
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server"
                                        ID="cnRolesTareasEstados"
                                        Cls="txtUsers"
                                        Padding="12"
                                        Height="70">
                                        <Content>
                                            <div class="form-group" id="pnRolestagsEscritura">
                                                <input type="text"
                                                    class="form-control input-control-form overflowNotificaciones"
                                                    name="rolestareasestados">
                                            </div>
                                        </Content>
                                    </ext:Container>
                                    <ext:Container runat="server"
                                        Cls="dispFlex"
                                        WidthSpec="100%">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblRolesEstadosSeguimientos"
                                                Cls="lblHeadForm"
                                                WidthSpec="60%"
                                                MarginSpec="0 29"
                                                Text="<%$ Resources:Comun, strRolesSeguimientoEstados %>">
                                            </ext:Label>
                                            <ext:Container runat="server"
                                                WidthSpec="40%"                                                
                                                Cls="ctToggle ctRolesWF">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lbPrivate"
                                                        Cls="lbLabel"
                                                        Text="<%$ Resources:Comun, jsPrivado %>" />
                                                    <ext:Button runat="server"
                                                        EnableToggle="true"
                                                        ID="btnWorkFlowPublicRol"
                                                        Cls="btnActivarDesactivarV2"
                                                        PressedCls="btnActivarDesactivarV2Pressed"
                                                        MinWidth="50"
                                                        Pressed="true"
                                                        Focusable="false"
                                                        OverCls="none">
                                                        <Listeners>
                                                            <Click Fn="tglRestWorkFlow" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Label runat="server"
                                                        ID="lbPublic"
                                                        Cls="lbLabel"
                                                        PaddingSpec="5 0 0 7"
                                                        Disabled="true"
                                                        Text="<%$ Resources:Comun, jsPublico %>" />
                                                </Items>
                                            </ext:Container>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server"
                                        ID="cnRolesEstadosSeguimientos"
                                        Cls="txtUsers"
                                        Padding="12"
                                        Height="70">
                                        <Content>
                                            <div class="form-group" id="pnRolestags">
                                                <input type="text"
                                                    class="form-control input-control-form overflowNotificaciones"
                                                    name="rolesestadosseguimientos">
                                            </div>
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formNotification"
                                Hidden="true"
                                PaddingSpec="0 48px"
                                Cls="winGestion-panel ctForm-resp unaColumna">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctAddNotificacion">
                                        <Items>
                                            <ext:Container runat="server"
                                                Cls="bordes">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lblUsuarios"
                                                        Cls="lblHeadForm"
                                                        Text="<%$ Resources:Comun, strUsuarios %>">
                                                    </ext:Label>
                                                    <ext:Container runat="server"
                                                        ID="cnUsuarios"
                                                        Cls="txtUsers"
                                                        Padding="12"
                                                        Height="70">
                                                        <Content>
                                                            <div class="form-group">
                                                                <input type="text"
                                                                    class="form-control input-control-form overflowNotificaciones"
                                                                    name="usuarios">
                                                            </div>
                                                        </Content>
                                                    </ext:Container>
                                                    <ext:Label runat="server"
                                                        ID="lblRol"
                                                        Cls="lblHeadForm"
                                                        Text="<%$ Resources:Comun, strRoles %>">
                                                    </ext:Label>
                                                    <ext:Container runat="server"
                                                        ID="cnRoles"
                                                        Cls="txtUsers"
                                                        Padding="12"
                                                        Height="70">
                                                        <Content>
                                                            <div class="form-group">
                                                                <input type="text"
                                                                    class="form-control input-control-form overflowNotificaciones"
                                                                    name="roles">
                                                            </div>
                                                        </Content>
                                                    </ext:Container>
                                                    <%--<ext:TextField runat="server"
                                                ID="txtUsers"
                                                FieldLabel="<%$ Resources:Comun, strUsuarios %>"
                                                MaxLength="100"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                LabelAlign="Top"
                                                Cls="dosColumnasInicio txtUsers"
                                                ValidationGroup="FORM">
                                                <Listeners>
                                                    <Change Fn="FormularioValido" />
                                                </Listeners>
                                            </ext:TextField>--%>
                                                    <%--<ext:Checkbox runat="server"
                                                ID="chkSuppliers"
                                                BoxLabel="<%$ Resources:Comun, strProveedores %>"
                                                Cls="chk-form chkNotification"
                                                Width="200" />
                                            <ext:Checkbox runat="server"
                                                ID="chkDepartament"
                                                BoxLabel="<%$ Resources:Comun, strDepartamento %>"
                                                Cls="chk-form chkNotification"
                                                Width="200" />--%>
                                                    <%--<ext:TextArea
                                                ID="txtaSetting"
                                                runat="server"
                                                FieldLabel="<%$ Resources:Comun, strPerfiles %>"
                                                LabelAlign="Top"
                                                Width="220"
                                                Mode="local"
                                                Cls="dosColumnasInicio textAreaGridFinal"
                                                Editable="true">
                                            </ext:TextArea>--%>
                                                    <ext:TextArea
                                                        ID="txtMensaje"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strMensaje %>"
                                                        LabelAlign="Top"
                                                        Width="220"
                                                        Mode="local"
                                                        MaxLength="250"
                                                        Scrollable="Vertical"
                                                        RawText=""
                                                        WidthSpec="90%"
                                                        Cls="ico-exclamacion-10px-grey txtMensajeStatus"
                                                        Editable="true">
                                                        <Listeners>
                                                            <Change Fn="FormularioValidoNotificacion" />
                                                            <FocusLeave Fn="FormularioValidoNotificacion" />
                                                        </Listeners>
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Container>
                                            <ext:Button
                                                ID="btnAnadirNotificacion"
                                                runat="server"
                                                Width="130"
                                                Height="32"
                                                TextAlign="Center"
                                                Disabled="true"
                                                Text="<%$ Resources:Comun, winGestion.Title %>"
                                                Cls="btn-ppal-winForm btnAgregarNotification"
                                                IconCls="ico-addBtn">
                                                <Listeners>
                                                    <Click Fn="btnAgregarNotificacion" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server"
                                        ID="cntNotificaciones">
                                    </ext:Container>
                                </Items>
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
                                ID="btnPrevEstado"
                                Cls="btn-secondary-winForm"
                                Text="<%$ Resources:Comun, jsCerrar %>">
                                <Listeners>
                                    <Click Fn="cerrarWindow" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnNextEstado"
                                Cls="btn-ppal-winForm"
                                Disabled="true"
                                Text="<%$ Resources:Comun, strGuardar %>">
                                <Listeners>
                                    <Click Fn="guardarCambios" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
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
                                <Content>
                                    <ext:GridPanel
                                        Region="Center"
                                        Hidden="false"
                                        Title="Grid Principal"
                                        runat="server"
                                        ForceFit="true"
                                        Header="false"
                                        ID="gridMain1"
                                        StoreID="storeCoreEstados"
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        SelectionMemory="false"
                                        Cls="gridPanel grdNoHeader "
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <Listeners>
                                            <AfterRender Handler="GridColHandlerDinamicoV2(this);"></AfterRender>
                                            <Resize Handler="GridColHandlerDinamicoV2(this);"></Resize>
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
                                                        Handler="ExportarDatosSinCliente('WorkFlowsEstados', #{gridMain1}, hdDescargaExcel.value, '', #{cmbWorkflows}.value);" />
                                                    <ext:Button runat="server"
                                                        ID="btnDefecto"
                                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                                        Cls="btnDefecto"
                                                        Disabled="true"
                                                        Handler="Defecto();">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnActivar"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Cls="btn-Activar"
                                                        Handler="Activar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDuplicar"
                                                        ToolTip="<%$ Resources:Comun, btnDuplicar.ToolTip %>"
                                                        Cls="btnDuplicar"
                                                        Disabled="true"
                                                        Handler="btnDuplicar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDuplicarWorkflow"
                                                        ToolTip="<%$ Resources:Comun, strDuplicarWorkflow %>"
                                                        Cls="btnDuplicarTipologia"
                                                        Handler="btnDuplicarWorkflow();">
                                                    </ext:Button>
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
                                                    <%--<ext:Button runat="server"
                                                        ID="btnMostrarFiltros"
                                                        ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                        Cls="btnFiltros"
                                                        Handler="hideAsideR('panelFiltros')" />--%>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        ID="btnVerWorkFlow"
                                                        ToolTip="btnVerWorkFlow"
                                                        Cls="btnVerFlowWork"
                                                        Hidden="true"
                                                        Handler="" />
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbWorkflows"
                                                        Cls="cmbTipologias"
                                                        FieldBodyCls="cmbWorkflows"
                                                        DisplayField="Codigo"
                                                        ValueField="CoreWorkFlowID"
                                                        AllowBlank="true"
                                                        StoreID="storeCoreWorkflows"
                                                        Hidden="false"
                                                        QueryMode="Local"
                                                        EmptyText="<%$ Resources:Comun, strTipologias %>"
                                                        LabelAlign="Right"
                                                        Width="250">
                                                        <Listeners>
                                                            <Select Fn="SeleccionarWorkflow" />
                                                            <TriggerClick Fn="RecargarWorkflow" />
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
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Cls="col-activo"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    MinWidth="40"
                                                    MaxWidth="60">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Cls="col-default"
                                                    DataIndex="Defecto"
                                                    ToolTip="<%$ Resources:Comun, strDefecto %>"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    Align="Center"
                                                    ID="ColDefecto"
                                                    Flex="2">
                                                    <Renderer Fn="DefaultRender" />
                                                </ext:Column>
                                                <ext:ProgressBarColumn runat="server"
                                                    DataIndex="Porcentaje"
                                                    Text="<%$ Resources:Comun, strPorcentaje %>"
                                                    MinWidth="60"
                                                    Align="Center"
                                                    ID="BarraProgreso"
                                                    Flex="6">
                                                    <Renderer Fn="barGridEstados"></Renderer>
                                                </ext:ProgressBarColumn>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="120"
                                                    DataIndex="NombreEstado"
                                                    Flex="8"
                                                    ID="ColNombre">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strCodigo %>"
                                                    MinWidth="120"
                                                    DataIndex="Codigo"
                                                    Flex="8"
                                                    ID="ColObjeto">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strDescripcion %>"
                                                    MinWidth="120"
                                                    DataIndex="Descripcion"
                                                    Flex="8"
                                                    ID="colDescripcion">
                                                </ext:Column>
                                                <ext:ComponentColumn runat="server"
                                                    ID="colEstadoSiguienteBoton"
                                                    Align="Start"
                                                    Flex="8"
                                                    Sortable="false"
                                                    Text="<%$ Resources:Comun, strEstadoSiguiente %>"
                                                    DataIndex="EstadosSiguientes"
                                                    MinWidth="120">
                                                    <Component>
                                                        <ext:Button runat="server"
                                                            ID="btnSiguientes"
                                                            OverCls="none"
                                                            PressedCls="none"
                                                            Border="false"
                                                            FocusCls="none"
                                                            Flat="true"
                                                            TextAlign="Left">
                                                            <Listeners>
                                                                <Click Fn="EstadosSiguientes" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Component>
                                                    <Listeners>
                                                        <Bind Handler="cmp.setText(record.get('EstadosSiguientes'))" />
                                                    </Listeners>
                                                    <Renderer Fn="columnEstadosSiguientes" />
                                                </ext:ComponentColumn>
                                                <ext:Column runat="server"
                                                    MinWidth="120"
                                                    Text="<%$ Resources:Comun, strDepartamento %>"
                                                    DataIndex="Departamento"
                                                    Flex="8"
                                                    ID="ColDepartment" />
                                                <ext:Column runat="server"
                                                    MinWidth="120"
                                                    Text="<%$ Resources:Comun, strGrupos %>"
                                                    DataIndex="NombreAgrupacion"
                                                    Flex="8"
                                                    ID="ColGrupo" />
                                                <ext:ComponentColumn runat="server"
                                                    ID="colEstadosGlobales"
                                                    Align="Start"
                                                    Flex="8"
                                                    Sortable="false"
                                                    Text="<%$ Resources:Comun, strEstadoGlobalTitulo %>"
                                                    DataIndex="EstadosGlobales"
                                                    MinWidth="120">
                                                    <Component>
                                                        <ext:Button runat="server"
                                                            ID="btnStateGlobales"
                                                            OverCls="none"
                                                            PressedCls="none"
                                                            FocusCls="none"
                                                            Border="false"
                                                            Flat="true"
                                                            TextAlign="Left">
                                                            <Listeners>
                                                                <Click Fn="EstadosGlobales" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Component>
                                                    <Listeners>
                                                        <Bind Handler="cmp.setText(record.get('EstadosGlobales'))" />
                                                    </Listeners>
                                                    <Renderer Fn="columnEstadosGlobales" />
                                                </ext:ComponentColumn>
                                                <ext:Column runat="server"
                                                    Cls="col-completo"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="Completado"
                                                    Align="Center"
                                                    ToolTip="<%$ Resources:Comun, strCompletado %>"
                                                    ID="colCompleto"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="CompletadoRender"></Renderer>
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Cls="col-lectura"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="PublicoLectura"
                                                    Align="Center"
                                                    ToolTip="Reading Public"
                                                    ID="colPublicoLectura"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="LecturaRender"></Renderer>
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Cls="col-escritura"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="PublicoEscritura"
                                                    Align="Center"
                                                    ToolTip="Writing Public"
                                                    ID="colPublicoEscritura"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="EscrituraRender"></Renderer>
                                                </ext:Column>
                                                <%--<ext:Column runat="server"
                                                    Cls="col-parallel"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="TieneSubProceso"
                                                    Align="Center"
                                                    ToolTip="<%$ Resources:Comun, strSubproceso %>"
                                                    ID="ColSubProcesoGrid"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="SubprocessRender"></Renderer>
                                                </ext:Column>--%>
                                                <%--<ext:Column runat="server"
                                                    Cls="col-link"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="TieneLink"
                                                    ToolTip="Link"
                                                    Align="Center"
                                                    ID="ColLink"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="LinkRender"></Renderer>
                                                </ext:Column>--%>
                                                <ext:Column runat="server"
                                                    Cls="col-notification"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    DataIndex="TieneNotificacion"
                                                    ToolTip="<%$ Resources:Comun, strNotificacion %>"
                                                    Align="Center"
                                                    ID="ColNotificacion"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="NotificacionRender"></Renderer>
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Cls="col-functionalities"
                                                    MinWidth="40"
                                                    MaxWidth="60"
                                                    ToolTip="Rol"
                                                    DataIndex="TieneRol"
                                                    Align="Center"
                                                    ID="ColFuncionalidad"
                                                    Flex="2"
                                                    TdCls="column">
                                                    <Renderer Fn="RolRender"></Renderer>
                                                </ext:Column>
                                                <ext:WidgetColumn ID="ColMore"
                                                    runat="server"
                                                    Cls="NoOcultar col-More"
                                                    DataIndex=""
                                                    Align="Center"
                                                    Hidden="false"
                                                    Text="<%$ Resources:Comun, strVerMas %>"
                                                    MinWidth="60"
                                                    MaxWidth="90"
                                                    Flex="99999">
                                                    <Widget>
                                                        <ext:Button runat="server"
                                                            Width="60"
                                                            OverCls="Over-btnMore"
                                                            PressedCls="Pressed-none"
                                                            FocusCls="Focus-none"
                                                            Cls="btnColMore"
                                                            Handler="cargarPanelMoreInfo('WrapEstados', 'lblEstados')" />
                                                    </Widget>
                                                </ext:WidgetColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectEstados"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectEstados" />
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
                                                StoreID="storeCoreEstados"
                                                Cls="PgToolBMainGrid"
                                                ID="PagingToolbar2"
                                                MaintainFlex="true"
                                                Flex="8"
                                                HideRefresh="true"
                                                DisplayInfo="false"
                                                OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        Cls="comboGrid"
                                                        ID="ComboBox9"
                                                        MaxWidth="80"
                                                        Flex="2">
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
                                                            <Select Fn="pageSelect" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Content>
                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
