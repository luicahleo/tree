<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiciosFrecuencias.aspx.cs" Inherits="TreeCore.ModGlobal.ServiciosFrecuencias" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarfiltros" TagPrefix="local" %>
<%@ Register Src="~/Componentes/ProgramadorTareasCron.ascx" TagName="Programador" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/GestionUsuarios.css" rel="stylesheet" type="text/css" />--%>
    <script src="js/ServiciosFrecuencias.js"></script>
    <!--<script src="../../JS/common.js"></script>-->
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
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
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
                    <ext:Model
                        runat="server"
                        IDProperty="CoreServicioFrecuenciaID">
                        <Fields>
                            <ext:ModelField Name="CoreServicioFrecuenciaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />                            
                            <ext:ModelField Name="CoreServicioFrecuenciaTipoID" Type="Int" />
                            <ext:ModelField Name="CronFormat" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="FechaInicio" Type="Date" />                            
                            <ext:ModelField Name="FechaFin" Type="Date" />                            
                            <ext:ModelField Name="NombreServicioFrecuenciaTipo" />                            
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="Nombre"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

<%--            <ext:Store
                runat="server"
                ID="storeFrecuenciasTipo"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storeFrecuenciasTipo_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="IDCoreServiciosFrecuenciasTipos">
                        <Fields>
                            <ext:ModelField Name="IDCoreServiciosFrecuenciasTipos" Type="Int" />
                            <ext:ModelField Name="Nombre" /> 
                            <ext:ModelField Name="Codigo" />                            
                            <ext:ModelField Name="Activo" Type="Boolean" />            
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="Nombre"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>--%>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="850"
                HeightSpec="60vh"
                Resizable="false"
                OverflowY="Auto"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Cls="winGestion-paneles ctForm-resp"
                        Border="false">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctProgramador"
                                HeightSpec="100%"
                                Cls="ctForm-resp ctForm-content-resp-col3">
                                <Content>
                                    <local:Programador
                                        ID="conProgramador"
                                        runat="server" />
                                </Content>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <%--<ValidityChange Fn="Validar"/>--%>
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        runat="server"
                        ID="btnCancelar"
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

            <%--VENTANAS EMERGENTES --%>



            <ext:Viewport runat="server" ID="vwResp" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Content>
                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                        </Content>

                        <Items>

                            <%-- PANEL CENTRAL--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <DockedItems>
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1" MarginSpec="10 0 10 0 ">
                                                <Items>
                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strServiciosFrecuencias %>" Height="25" />
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                </DockedItems>
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
                                                        Cls="btnAnadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnEditar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Cls="btnEditar"
                                                        Handler="MostrarEditar();">
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
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatos('ServiciosFrecuencias', '', #{grid}, '');" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server"
                                                ID="tlbClientes"
                                                Dock="Top">
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colNombre"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="Nombre"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    Width="150"
                                                    Flex="1" />
                                                <ext:Column
                                                    runat="server"
                                                    ID="colServicio"
                                                    meta:resourceKey="colServicio"
                                                    DataIndex="NombreServicioFrecuenciaTipo"
                                                    Text="<%$ Resources:Comun, strServicio %>"
                                                    Width="150"
                                                    Flex="1" />                                                
                                                <ext:DateColumn
                                                    runat="server"
                                                    ID="colInicio"
                                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                                    DataIndex="FechaInicio"
                                                    Text="<%$ Resources:Comun, strInicio %>"
                                                    Width="150"
                                                    Flex="1" />
                                                <ext:DateColumn
                                                    runat="server"
                                                    ID="colFinal"
                                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                                    DataIndex="FechaFin"
                                                    Text="<%$ Resources:Comun, strFinal %>"
                                                    Width="150"
                                                    Flex="1" />
                                                <ext:Column
                                                    runat="server"
                                                    ID="colFormatoCron"
                                                    meta:resourceKey="colFormatoCron"
                                                    DataIndex="CronFormat"
                                                    Text="<%$ Resources:Comun, strFormatoCron %>"
                                                    Width="150"
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
                            </ext:Panel>


                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Listeners>
                                </Listeners>
                                <Items>
                                    <%-- PANEL MORE INFO--%>


                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar2" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>

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
