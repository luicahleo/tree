<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuLateral.aspx.cs" Inherits="TreeCore.ModGlobal.MenuLateral" %>

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
            <ext:Hidden ID="hd_MenuSeleccionado" runat="server" />
            <ext:Hidden ID="hd_NivelMaxPermitido" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>
            <ext:Store
                ID="storeMenuModulo"
                runat="server"
                AutoLoad="true"
                OnReadData="storeMenuModulo_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="MenuModuloID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="MenuModuloID"
                                Type="Int" />
                            <ext:ModelField
                                Name="Modulo"
                                Type="string" />
                            <ext:ModelField
                                Name="RutaModulo"
                                Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeIcono"
                runat="server"
                AutoLoad="false"
                OnReadData="storeIcono_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="icono"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="iconPath" />
                            <ext:ModelField Name="icono" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeProyectosTipos"
                runat="server"
                AutoLoad="false"
                RemoteSort="false"
                Hidden="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="ProyectoTipoID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="ProyectoTipoID"
                                Type="Int" />
                            <ext:ModelField
                                Name="ProyectoTipo"
                                Type="string" />
                            <ext:ModelField
                                Name="Pagina"
                                Type="string" />
                            <ext:ModelField
                                Name="Descripcion"
                                Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Modulo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeModulo"
                runat="server"
                AutoLoad="false"
                OnReadData="storeModulo_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="ModuloID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="ModuloID"
                                Type="Int" />
                            <ext:ModelField
                                Name="Modulo"
                                Type="string" />
                            <ext:ModelField
                                Name="Pagina"
                                Type="string" />
                            <ext:ModelField
                                Name="Descripcion"
                                Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Modulo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storePaginaModulo"
                runat="server"
                AutoLoad="false"
                OnReadData="storePaginaModulo_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="MenuModuloID"
                        runat="server">
                        <Fields>
                            <ext:ModelField
                                Name="MenuModuloID"
                                Type="Int" />
                            <ext:ModelField
                                Name="Modulo"
                                Type="string" />
                            <ext:ModelField
                                Name="RutaModulo"
                                Type="string" />
                            <ext:ModelField
                                Name="Activo"
                                Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Modulo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        Cls="ctForm-resp ctForm-resp-col2"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                runat="server"
                                ID="txtNombre"
                                meta:resourceKey="txtNombre"
                                FieldLabel="<%$ Resources:Comun, strAlias %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                runat="server"
                                ID="txtKeyTraduccion"                               
                                FieldLabel="<%$ Resources:Comun, strKeyTraduccion %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:ComboBox
                                meta:resourceKey="cmbIcono"
                                runat="server"
                                ID="cmbIcono"
                                StoreID="storeIcono"
                                Mode="local"
                                ValueField="icono"
                                DisplayField="icono"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strIcono %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="false"
                                Editable="true"
                                QueryMode="Local">
                                <ListConfig>
                                    <ItemTpl runat="server">
                                        <Html>
                                            <img src="{iconPath}" alt="{icono}" /> {icono}
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <Listeners>
                                    <TriggerClick Handler="RecargarIconos();" />
                                    <Select Fn="SeleccionaIconos" />
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
                                meta:resourceKey="cmbProyectosTipos"
                                runat="server"
                                ID="cmbProyectosTipos"
                                Mode="local"
                                Hidden="true"
                                ValueField="ModuloID"
                                DisplayField="Modulo"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strProyectosTipos %>"
                                WidthSpec="100%"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                Editable="true"
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Handler="RecargarProyectosTipos();" />
                                    <Select Fn="SeleccionaProyectosTipos" />
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
                                meta:resourceKey="cmbModulo"
                                runat="server"
                                ID="cmbModulo"
                                StoreID="storeModulo"
                                Mode="local"
                                ValueField="ModuloID"
                                DisplayField="Modulo"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strPagina %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                Editable="true"
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Handler="RecargarModulos();" />
                                    <Select Fn="SeleccionaModulos" />
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
                                meta:resourceKey="cmbPaginaModulo"
                                runat="server"
                                ID="cmbPaginaModulo"
                                StoreID="storePaginaModulo"
                                Mode="local"
                                ValueField="MenuModuloID"
                                DisplayField="Modulo"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strModulo %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                Editable="true"
                                QueryMode="Local">
                                <Listeners>
                                    <TriggerClick Handler="RecargarPaginaModulo();" />
                                    <Select Fn="SeleccionaPaginaModulo" />
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
                                runat="server"
                                ID="txtParametro"
                                FieldLabel="<%$ Resources:Comun, strParametro %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Checkbox
                                runat="server"
                                ID="chkNuevo"
                                AllowBlank="true"
                                meta:resourceKey="chkNuevo"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strNuevo %>" />
                            <ext:Checkbox
                                runat="server"
                                ID="chkActualizado"
                                AllowBlank="true"
                                meta:resourceKey="chkActualizado"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strActualizado %>" />
                            <ext:Checkbox
                                runat="server"
                                ID="chkExpandido"
                                AllowBlank="true"
                                meta:resourceKey="chkExpandido"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strExpandido %>" />
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

                    <ext:TreePanel
                        ID="Tree"
                        runat="server"
                        Scrollable="Both"
                        Cls="gridPanel TreePnl"
                        FolderSort="true"
                        Scroll="Vertical"
                        AnchorVertical="100%"
                        AnchorHorizontal="0">
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
                                        Disabled="true"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="AgregarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        Cls="btnEditar"
                                        Disabled="true"
                                        Handler="MostrarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Cls="btnActivar"
                                        Disabled="true"
                                        Handler="Activar();">
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
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items> 
                                    <ext:TextField ID="txtBuscar"
                                        runat="server"
                                        EnableKeyEvents="true"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                            <ext:FieldTrigger Icon="Search" />
                                            
                                        </Triggers>
                                        <Listeners>
                                            <KeyUp Fn="filtrarTree"
                                                Buffer="100" />
                                            <TriggerClick Handler="clearFilter();" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:ComboBox
                                        meta:resourceKey="cmbMenuModulo"
                                        runat="server"
                                        ID="cmbMenuModulo"
                                        StoreID="storeMenuModulo"
                                        Mode="local"
                                        ValueField="MenuModuloID"
                                        DisplayField="Modulo"
                                        LabelAlign="Right"
                                        FieldLabel="<%$ Resources:Comun, strModulo %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        AllowBlank="false"
                                        Width="320"
                                        Editable="true"
                                        QueryMode="Local"
                                         MarginSpec="0 15 0 0">
                                        <Listeners>
                                            <TriggerClick Handler="RecargarMenuModulos();" />
                                            <Select Fn="SeleccionaMenuModulos" />
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
                                  <%--  <ext:ToolbarTextItem runat="server"
                                        Text="<%$ Resources:Comun, strFiltros %>" />--%>
                                  
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>

                        <ColumnModel>
                            <Columns>
                                <ext:Column
                                    runat="server"                                   
                                    DataIndex="Activo"
                                    Align="Center"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:TreeColumn
                                    runat="server"
                                    MinWidth="150"     
                                    Flex="1"
                                    DataIndex="text"
                                    Header="<%$ Resources:Comun, strAlias %>">
                                </ext:TreeColumn>
                                <ext:Column
                                    runat="server"
                                    ID="colKey"
                                    Flex="2"
                                    MinWidth="150"
                                    DataIndex="Alias"
                                    Hidden="true"
                                    Header="<%$ Resources:Comun, strKeyTraduccion %>" />
                                <ext:Column
                                    runat="server"
                                     MinWidth="150"
                                    ID="colPagina"
                                    Flex="1"
                                    DataIndex="Pagina"
                                    Hidden="true"
                                    Header="<%$ Resources:Comun, strPagina %>">
                                    <Filter>
                                        <ext:StringFilter />
                                    </Filter>
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    ID="colNombreModulo"
                                    DataIndex="NombreModulo"
                                    MinWidth="100"
                                    Flex="1"
                                    Hidden="true"
                                    Header="<%$ Resources:Comun, strModulo %>" />
                                <ext:Column
                                    runat="server"
                                    MinWidth="150"
                                    ID="colParametros"
                                    Flex="1"
                                    DataIndex="Parametros"
                                    Hidden="true"
                                    Header="<%$ Resources:Comun, strParametro %>" />
                                <%--<ext:Column 
                                    runat="server" 
                                    Flex="1" 
                                    DataIndex="Icono"
                                    Header="<%$ Resources:Comun, strIcono %>" >
                                    <Renderer Fn="printIconOnColum" />
                                    </ext:Column>--%>
                                <ext:Column
                                    runat="server"
                                    DataIndex="Nuevo"
                                    Header="<%$ Resources:Comun, strNuevo %>"
                                    Align="Center"
                                    Flex="1"
                                    Width="150">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    DataIndex="Actualizado"
                                    Header="<%$ Resources:Comun, strActualizado %>"
                                    Align="Center"
                                    Width="150">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    DataIndex="Expandido"
                                    Header="<%$ Resources:Comun, strExpandido %>"
                                    Align="Center"
                                    Flex="1"
                                    Width="150">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>

                        <Listeners>
                            <AfterRender Fn="AfterRender" />
                            <BeforeLoad Fn="DeseleccionarGrilla" />
                            <Select Fn="Grid_RowSelect" />
                        </Listeners>

                        <View>
                            <ext:TreeView runat="server">
                                <Plugins>
                                    <ext:TreeViewDragDrop runat="server"
                                        AppendOnly="true"
                                        ContainerScroll="true"
                                        SortOnDrop="true" />
                                </Plugins>
                                <Listeners>
                                    <BeforeDrop Fn="BeforeDropNodo" />
                                </Listeners>
                            </ext:TreeView>
                        </View>

                        <Plugins>
                            <ext:GridFilters runat="server" ID="gridFilters">
                            </ext:GridFilters>
                        </Plugins>
                    </ext:TreePanel>

                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
