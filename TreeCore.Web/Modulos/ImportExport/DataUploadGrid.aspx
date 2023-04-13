<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataUploadGrid.aspx.cs" Inherits="TreeCore.ModExportarImportar.DataUploadGrid" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue.min.js"></script>
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue-i18n.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdLocale" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
                ID="storePrincipal"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false"
                PageSize="20"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="DocumentoCargaID,Activo,RutaDocumento,DocumentoCargaPlantillaID,UsuarioID,Proyecto,TabInicial,TabFinal,Exito,Procesado,ClienteID,Operador,RutaLog">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" Type="String" />
                            <ext:ModelField Name="Type" Type="String" />
                            <ext:ModelField Name="Document" Type="object" />
                            <ext:ModelField Name="UploadDate" Type="Date" />
                            <ext:ModelField Name="ImportDate" Type="Date" />
                            <ext:ModelField Name="Processed" Type="Boolean" />
                            <ext:ModelField Name="Success" Type="Boolean" />
                            <ext:ModelField Name="LogFile" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ImportDate" Direction="DESC" />
                </Sorters>

                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeProyectos"
                AutoLoad="true"
                OnReadData="storeProyectos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="Proyecto" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Alias" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePrincipalPlantillas"
                AutoLoad="true"
                OnReadData="storePrincipalPlantillas_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="DocumentoCargaPlantillaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="DocumentoCargaPlantillaID" Type="Int" />
                            <ext:ModelField Name="DocumentoCargaPlantilla" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="DocumentoCargaPlantillaID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeOperadores"
                AutoLoad="true"
                OnReadData="storeOperadores_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="OperadorID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="OperadorID" Type="Int" />
                            <ext:ModelField Name="Operador" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="OperadorID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN STORES --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%-- FIN  RESOURCEMANAGER --%>

            <%-- INICIO WINDOWS --%>

            <ext:Window runat="server"
                ID="WinConfirmExport"
                Title="Upload Template"
                Modal="true"
                Width="750"
                Height="345"
                Closable="false"
                Hidden="true"
                Layout="FitLayout"
                Cls="winForm-resp winGestion">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formUpload"
                        WidthSpec="100%"
                        OverflowY="Auto"
                        Cls="formGris formResp"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="formImport"
                                Hidden="false"
                                HeightSpec="100%"
                                Scrollable="Vertical"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        MaxWidth="350"
                                        MaxLength="40"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>">
                                    </ext:TextField>
                                    <ext:FileUploadField runat="server"
                                        ID="UploadF"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        AllowBlank="false"
                                        FieldLabel="<%$ Resources:Comun, strArchivo %>"
                                        ButtonText=""
                                        Cls=" btnUploadFolderGrid">
                                    </ext:FileUploadField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbPlantillas"
                                        StoreID="storePrincipalPlantillas"
                                        QueryMode="Local"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        AllowBlank="false"
                                        DisplayField="DocumentoCargaPlantilla"
                                        ValueField="DocumentoCargaPlantilla"
                                        FieldLabel="<%$ Resources:Comun, strPlantillas %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarPlantillas" />
                                            <TriggerClick Fn="RecargarPlantillas" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:DateField runat="server"
                                        meta:resourcekey="txtFechaInicio"
                                        ID="txtFechaInicio"
                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        MinDate="<%# DateTime.Now %>"
                                        AutoDataBind="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <Listeners>
                                            <%--<Change Fn="ValidarFecha" />--%>
                                            <%--<WriteableChange Fn="ValidarFecha" />
                                            <ValidityChange Fn="ValidarFecha" />--%>
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TimeField runat="server"
                                        meta:resourcekey="tmHoraCarga"
                                        ID="tmHoraCarga"
                                        FieldLabel="<%$ Resources:Comun, strFechaHoraInicio %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Increment="5"
                                        Format="H:mm"
                                        AutoDataBind="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<Change Fn="ValidarFecha" />--%>
                                            <%--<WriteableChange Fn="ValidarFecha" />
                                            <ValidityChange Fn="ValidarFecha" />--%>
                                        </Listeners>
                                    </ext:TimeField>
                                    <ext:FieldContainer runat="server"
                                        ID="CntBtnOffset"
                                        Hidden="true"
                                        Layout="HBoxLayout">
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Pack="End"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblbtnToggle"
                                                Text="Mantener Contenido Actual"
                                                Padding="6">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                ID="btnActivo"
                                                Width="41"
                                                Pressed="true"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid" />
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="ValidarFormulario(valid);" />
                        </Listeners>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbButtons"
                                Cls=" greytb"
                                Dock="Bottom">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCancelar"
                                        Cls="btn-secondary "
                                        MinWidth="110"
                                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="#{WinConfirmExport}.hide();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGuardar"
                                        Cls="btn-ppal "
                                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Disabled="true"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="winGestionBotonGuardar();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>

            </ext:Window>

            <ext:Window ID="WinConfirmImport"
                runat="server"
                Title="Upload Content"
                MaxWidth="572"
                Width="572"
                Height="210"
                Modal="true"
                Centered="true"
                Cls="winForm-resp winRed"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar2"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 15 18 30">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarAdd"
                                Cls="btn-secondary"
                                MinWidth="100"
                                Text="Back"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{WinConfirmImport}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnEliminarUpload"
                                Cls="btn-ppal btnBold btnRed "
                                MinWidth="160"
                                Text="Remove and Upload"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="ajaxAgregar();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:Label IconAlign="Left"
                        IconCls="btnSubir"
                        Cls="ExitoLbl"
                        ID="Label19"
                        runat="server"
                        Text="Vas a borrar todos los datos actuales"
                        MarginSpec="20 10 20 10"
                        Hidden="false">
                    </ext:Label>
                    <ext:Label IconAlign="Left"
                        IconCls=""
                        Cls=""
                        ID="Label20"
                        runat="server"
                        Text="This action will Erase everything and replace information loremp ipsum lorem"
                        MarginSpec="10 10 20 50"
                        Hidden="false">
                    </ext:Label>
                </Items>
            </ext:Window>

            <ext:Menu runat="server"
                ID="ContextMenuTreeL">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-CntxMenuExcel"
                        Text="Download Filled Template"
                        Disabled="true"
                        ID="ShowTemplate">
                        <Listeners>
                            <Click Handler="DescargarPlantilla();" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-CntxPaginaDoc"
                        Text="Download Log"
                        Disabled="true"
                        ID="ShowLog"
                        Hidden="false">
                        <Listeners>
                            <Click Handler="DescargarLog();" />
                        </Listeners>
                    </ext:MenuItem>
                </Items>
            </ext:Menu>

            <%-- FIN WINDOWS --%>

            <%--INICIO VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Cls="vwContenedor"
                Layout="Anchor">
                <Items>
                    <ext:GridPanel
                        Hidden="false"
                        Title="Grid Principal"
                        runat="server"
                        Header="false"
                        StoreID="storePrincipal"
                        ContextMenuID="ContextMenuTreeL"
                        Scrollable="Vertical"
                        ID="gridPrincipal"
                        Cls="gridPanel"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        OverflowX="Auto">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button
                                        runat="server"
                                        ID="btnAnadir"
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
                                        Hidden="true"
                                        Handler="MostrarEditar();" />
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btn-Eliminar"
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
                                        Handler="ExportarDatos('DataUploadGrid', hdCliID.value, #{gridPrincipal}, '', '', '');" />
                                    <ext:Button runat="server"
                                        ID="btnDescargarTemplate"
                                        ToolTip="<%$ Resources:Comun, btnDescargarPlantilla.ToolTip %>"
                                        Disabled="true"
                                        Cls="btnExcel"
                                        Handler="DescargarPlantilla();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargarLog"
                                        ToolTip="<%$ Resources:Comun, btnDescargarLog.ToolTip %>"
                                        Cls="btnPaginaDoc"
                                        Disabled="true"
                                        Handler="DescargarLog();" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                <Content>
                                    <local:toolbarFiltros
                                        ID="cmpFiltro"
                                        runat="server"
                                        Stores="storePrincipal"
                                        MostrarComboFecha="false"
                                        FechaDefecto="Dia"
                                        Grid="gridPrincipal"
                                        MostrarBusqueda="false" />
                                </Content>
                            </ext:Container>
                        </DockedItems>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server"
                                    ID="colProcesado"
                                    DataIndex="Processed"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, colProcesado.ToolTip %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colExito"
                                    DataIndex="Success"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, colExito.ToolTip %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    DataIndex="Code"
                                    Flex="1"
                                    ID="colNombre">
                                </ext:Column>
                                <ext:Column runat="server"
                                    Text="<%$ Resources:Comun, strPlantilla %>"
                                    DataIndex="Type"
                                    Flex="1"
                                    ID="colTipoCarga">
                                </ext:Column>
                                <ext:DateColumn runat="server"
                                    Text="<%$ Resources:Comun, strFechaSubida %>"
                                    DataIndex="UploadDate"
                                    Flex="1"
                                    Format="dd/MM/yyyy HH:mm"
                                    ID="dtFechaSubida" />
                                <ext:DateColumn runat="server"
                                    Text="<%$ Resources:Comun, strFechaCarga %>"
                                    DataIndex="ImportDate"
                                    Flex="1"
                                    Format="dd/MM/yyyy HH:mm"
                                    ID="dtFechaEstimadaSubida" />
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
                            <ext:PagingToolbar runat="server"
                                StoreID="storePrincipal"
                                Cls="PgToolBMainGrid"
                                ID="PagingToolbar2"
                                DisplayInfo="true"
                                HideRefresh="true"
                                MaintainFlex="true"
                                Flex="8">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        Cls="comboGrid"
                                        ID="ComboBox9"
                                        Width="80">
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
            </ext:Viewport>

            <%--FIN VIEWPORT --%>
        </div>
    </form>
</body>
</html>
