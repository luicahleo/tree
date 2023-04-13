<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectosUsuarios.aspx.cs" Inherits="TreeCore.PaginasComunes.ProyectosUsuarios" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarfiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>

    <!--<script src="../../JS/common.js"></script>-->
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <script src="js/ProyectosUsuarios.js"></script>
    <form id="form1" runat="server">

        <div>
            <ext:Hidden ID="hdClienteID" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <ext:Hidden ID="hdProyectoSeleccionadoID" runat="server" />
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <ext:Store runat="server"
                ID="storeUsuarios"
                AutoLoad="false"
                OnReadData="storeUsuarios_Refresh"
                shearchBox="cmpFiltro_txtSearch"
                RemotePaging="false"
                PageSize="20"
                RemoteSort="TRUE"
                listNotPredictive="UsuariosProyectosID">
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="UsuariosProyectosID">
                        <Fields>
                            <ext:ModelField Name="UsuariosProyectosID" />
                            <ext:ModelField Name="UsuarioID" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="EMail" />
                            <ext:ModelField Name="Perfil" />
                            <ext:ModelField Name="NombreEntidad" />
                            <ext:ModelField Name="Proyecto" />

                        </Fields>
                    </ext:Model>
                </Model>
                <Parameters>
                    <ext:StoreParameter Value="cargarProyectoID()" Name="proyectoIDParametro" Mode="Raw" />
                </Parameters>
                <Sorters>
                    <ext:DataSorter Property="UsuariosProyectosID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeUsuariosLibres"
                AutoLoad="false"
                RemoteSort="false"
                RemoteFilter="false"
                RemotePaging="true"
                OnReadData="storeUsuariosLibres_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuariosProyectosLibresID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Tipo" Type="String" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Window ID="winGestionUsuarios"
                runat="server"
                meta:resourceKey="winGestionUsuarios"
                Width="450"
                Height="150"
                Title="Vincular/Desvincular Usuarios"
                MinHeight="220"
                Modal="true"
                Cls="winForm-respSimple formularioProyectos winGestion-panel-1-Field"
                Hidden="true"
                Border="false"
                Resizable="false"
                Closable="true"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionContactos"
                        Cls="formGris formResp"
                        Layout="AnchorLayout">
                        <Items>

                            <ext:ComboBox runat="server"
                                ID="cmbUsuarios"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strMiembros %>"
                                Cls="cmbUsers"
                                StoreID="storeUsuariosLibres"
                                DisplayField="Nombre"
                                ValueField="UsuarioID"
                                Model="Local"
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Fn="RecargarComboUsuarios" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger meta:resourceKey="RecargarLista" Hidden="false"
                                        IconCls="ico-reload"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:Button runat="server" ID="btnAnadirUsuarios" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd btnAnadirUsuarios">
                                <Listeners>
                                    <Click Fn="winAgregarUsuarios" />
                                </Listeners>
                            </ext:Button>

                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Listeners>
                    <Show Fn="cargarProyectoID" />
                </Listeners>
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">

                        <Items>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">

                                <Items>
                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
                                        <Items>

                                            <ext:GridPanel
                                                Region="Center"
                                                Hidden="false"
                                                Title="Grid Principal"
                                                runat="server"
                                                StoreID="storeUsuarios"
                                                SelectionMemory="false"
                                                Header="false"
                                                ID="gridPrincipal"
                                                EnableColumnHide="false"
                                                Cls="gridPanel grdNoHeader "
                                                OverflowX="Hidden"
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
                                                            <ext:Button runat="server"
                                                                ID="btnGestionUsuarios"
                                                                ToolTip="<%$ Resources:Comun, strAnadir %>"
                                                                Cls="btnAnadir"
                                                                Handler="GestionarUsuarios();" />
                                                            <ext:Button runat="server" ID="btnEliminar" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="cambiarAsignacion" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescar" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="Refrescar()" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Handler="ExportarDatosSinCliente('ProyectosUsuarios', #{gridPrincipal}, '','', App.cmbProyectos.value,'');">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>


                                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                        <Content>

                                                            <local:toolbarfiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storeUsuarios"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                Grid="gridPrincipal"
                                                                MostrarBusqueda="false" />

                                                        </Content>
                                                    </ext:Container>


                                                </DockedItems>

                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server" DataIndex="Proyecto" Text="<%$ Resources:Comun, strProyecto %>" MinWidth="120" Flex="1" ID="ColProyecto">
                                                        </ext:Column>
                                                        <ext:Column runat="server" DataIndex="NombreCompleto" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" Flex="1" ID="ColName">
                                                        </ext:Column>
                                                        <ext:Column runat="server" DataIndex="EMail" Text="<%$ Resources:Comun, strEmail %>" MinWidth="120" Flex="1" ID="ColEmail">
                                                        </ext:Column>
                                                        <ext:Column runat="server" DataIndex="NombreEntidad" Text="<%$ Resources:Comun, strEntidad %>" MinWidth="120" Flex="1" ID="ColCompany">
                                                        </ext:Column>
                                                        <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MaxWidth="90" MinWidth="60" Flex="99999">
                                                            <Widget>
                                                                <ext:Button ID="btnMore" runat="server" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Fn="VerMas" />
                                                                    </Listeners>
                                                                </ext:Button>

                                                            </Widget>
                                                        </ext:WidgetColumn>


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
                                                    <ext:PagingToolbar runat="server" StoreID="storeUsuarios" Cls="PgToolBMainGrid" ID="PGToolBarGrid" OverflowHandler="Scroller" MaintainFlex="true" Flex="3" HideRefresh="true">
                                                        <Items>
                                                            <ext:ComboBox runat="server" Cls="comboGrid" ID="cmbTbRegistros" Width="80">
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
