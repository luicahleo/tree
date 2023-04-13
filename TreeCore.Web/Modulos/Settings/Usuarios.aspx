<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="TreeCore.ModGlobal.Usuarios" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarfiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdProyectoTipoConZona" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />

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
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemotePaging="false"
                RemoteSort="true"
                PageSize="20"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="UsuarioID,Activo,Clave,FechaUltimoAcceso,LDAP,Interno,Soporte,UsuarioOAMID,FechaUltimoCambio,FechaCaducidadUsuario,FechaCaducidadClave">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="UsuarioID">
                        <Fields>
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Apellidos" />
                            <ext:ModelField Name="Telefono" />
                            <ext:ModelField Name="EMail" />
                            <ext:ModelField Name="NombreEntidad" />
                            <ext:ModelField Name="Clave" />
                            <ext:ModelField Name="LDAP" Type="Boolean" />
                            <ext:ModelField Name="Interno" Type="Boolean" />
                            <ext:ModelField Name="Soporte" Type="Boolean" />
                            <ext:ModelField Name="UsuarioOAMID" Type="Boolean" />
                            <ext:ModelField Name="FechaUltimoAcceso" Type="Date" />
                            <ext:ModelField Name="FechaUltimoCambio" Type="Date" />
                            <ext:ModelField Name="FechaCaducidadUsuario" Type="Date" />
                            <ext:ModelField Name="FechaCaducidadClave" Type="Date" />
                            <ext:ModelField Name="MacDispositivo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
                <Listeners>
                    
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store ID="storeEntidades" runat="server" AutoLoad="true" OnReadData="storeEntidades_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EntidadID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EntidadID" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Entidad" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeRoles" runat="server" AutoLoad="false" OnReadData="storeRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="UsuarioRolID" runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuarioRolID" Type="Int" />
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="NombreCompleto" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeRolesLibres" runat="server" AutoLoad="false" OnReadData="storeRolesLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="RolID" runat="server">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Descripcion" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeUsuarios" runat="server" AutoLoad="false" OnReadData="storeUsuarios_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="UsuarioID" runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeProyectosAgrupaciones" runat="server" AutoLoad="false" OnReadData="storeProyectosAgrupaciones_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoAgrupacionID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoAgrupacionID" Type="Int" />
                            <ext:ModelField Name="ProyectoAgrupacion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoAgrupacion" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeProyectos" runat="server" AutoLoad="false" OnReadData="storeProyectos_Refresh"
                WarningOnDirty="false" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="Proyecto" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Proyecto" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storePermisosAgregados" runat="server" AutoLoad="false" OnReadData="storePermisosAgregados_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="UsuariosProyectosID" runat="server">
                        <Fields>
                            <ext:ModelField Name="UsuariosProyectosID" Type="Int" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoAgrupacionID" Type="Int" />
                            <ext:ModelField Name="ZonaID" Type="Int" />
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="Proyecto" />
                            <ext:ModelField Name="Cliente" />
                            <ext:ModelField Name="ProyectoAgrupacion" />
                            <ext:ModelField Name="Zona" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="ProyectoTipo" />
                            <ext:ModelField Name="Alias" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Proyecto" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeDocumentosTipos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposDocumentos_Refresh"
                WarningOnDirty="false"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="DocumentTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="DocumentTipoID" Type="Int" />
                            <ext:ModelField Name="DocumentTipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="DocumentTipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeDocumentos"
                runat="server"
                AutoLoad="false"
                OnReadData="storeDocumentos_Refresh"
                RemotePaging="false"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="DocumentoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="DocumentoID" Type="Int" />
                            <ext:ModelField Name="DocumentTipo" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="Documento" />
                            <ext:ModelField Name="Archivo" />
                            <ext:ModelField Name="Extension" />
                            <ext:ModelField Name="FechaDocumento" Type="Date" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="FechaDocumento" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <%-- Context Menu --%>

            <ext:Menu runat="server"
                ID="ContextMenu">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-Documents-ctxMnu"
                        Text="<%$ Resources:Comun, strDocumentos %>"
                        ID="irADocumentos"
                        Hidden="true">
                        <Listeners>
                            <Click Fn="irADocumentos" />
                        </Listeners>
                    </ext:MenuItem>
                </Items>
            </ext:Menu>


            <%--VENTANAS EMERGENTES --%>

            <ext:Window ID="winAnadirDocumentos"
                runat="server"
                meta:resourcekey="winAnadirDocumentos"
                Title="<%$ Resources:Comun, strDocumento %>"
                Height="670"
                Width="872"
                Modal="true"
                Centered="true"
                BodyCls="winAddDirBody"
                Cls="winForm-resp"
                Scrollable="Vertical"
                OverflowY="Scroll"
                Hidden="true">
                <Items>
                    <ext:FormPanel ID="FormPanel3"
                        Cls="formWorkflow formGris "
                        runat="server"
                        Hidden="false"
                        Layout=""
                        Padding="20">
                        <Items>
                            <ext:Container runat="server"
                                Layout="HBoxLayout"
                                MarginSpec="0 0 20 0">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbDocumentosTipos"
                                        Cls=" "
                                        MinWidth="200"
                                        Width="300"
                                        FieldLabel="<%$ Resources:Comun, strDocumentoTipo %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        DisplayField="DocumentTipo"
                                        ValueField="DocumentTipoID"
                                        StoreID="storeDocumentosTipos"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Flex="4"
                                        EnableKeyEvents="true">
                                        <Listeners>
                                            <TriggerClick Fn="RecargarTiposDocumentos" />
                                            <KeyUp Fn="ComboBoxKeyUp" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ToolbarFill Flex="1"></ext:ToolbarFill>
                                    <ext:FileUploadField runat="server"
                                        ID="uploadFieldDocumento"
                                        Flex="5"
                                        MinWidth="200"
                                        Width="300"
                                        MarginSpec="28 0 0 0"
                                        ButtonText=""
                                        Cls="uploadGeneric"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                    </ext:FileUploadField>
                                    <ext:ToolbarFill Flex="1"></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="Button4"
                                        Cls="btn-ppal btnBold "
                                        IconCls="ico-addBtn"
                                        Text="<%$ Resources:Comun, strAgregarDocumento %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false"
                                        MinWidth="100"
                                        Width="120"
                                        Flex="5"
                                        MarginSpec="28 0 0 0">
                                        <Listeners>
                                            <Click Fn="AgregarDocumento" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Container>
                            <ext:GridPanel
                                Hidden="false"
                                Header="false"
                                runat="server"
                                Height="500"
                                ForceFit="false"
                                FocusCls="none"
                                ID="gridAddRemoveDocumentos"
                                Cls="gridPanel grdNoHeader"
                                OverflowX="Hidden"
                                OverflowY="Hidden"
                                StoreID="storeDocumentos"
                                EnableColumnHide="false">
                                <ColumnModel>
                                    <Columns>
                                        <ext:CommandColumn runat="server" Width="30">
                                            <Commands>
                                                <ext:GridCommand CommandName="DescargarDocumento"
                                                    Cls="btn-trans btnDocs">
                                                    <ToolTip Text="<%$ Resources:Comun, strDocumento %>" />
                                                </ext:GridCommand>
                                            </Commands>
                                            <Listeners>
                                                <Command Handler="SeleccionarComando(command, record);" />
                                            </Listeners>
                                        </ext:CommandColumn>
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strTipo %>"
                                            DataIndex="DocumentTipo"
                                            ID="Column6"
                                            Flex="3" />
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strDocumento %>"
                                            DataIndex="Documento"
                                            ID="Column5"
                                            Flex="3" />
                                        <ext:DateColumn runat="server"
                                            Text="<%$ Resources:Comun, strFecha %>"
                                            DataIndex="FechaDocumento"
                                            ID="Column99"
                                            Flex="2"
                                            Format="<%$ Resources:Comun, FormatFecha %>" />
                                        <ext:ComponentColumn runat="server"
                                            DataIndex="Activo"
                                            Hidden="true"
                                            Flex="1">
                                            <Component>
                                                <ext:Button runat="server"
                                                    ID="btnToggle"
                                                    Width="41"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip="">
                                                    <Listeners>
                                                        <Click Fn="cambiarAsignacion" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Component>
                                            <Listeners>
                                                <Bind Fn="SetToggleValue" />
                                            </Listeners>
                                        </ext:ComponentColumn>
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:GridFilters runat="server"
                                        ID="gridFiltersDocumentos"
                                        MenuFilterText="<%$ Resources:Comun, GridFilters.MenuFilterText %>" />
                                    <ext:CellEditing runat="server"
                                        ClicksToEdit="2" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window runat="server"
                ID="winGestionUsuarios"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, strUsuarios %>"
                Modal="true"
                Width="650"
                HeightSpec="80vh"
                MaxHeight="600"
                Hidden="true"
                OverflowY="Auto"
                PaddingSpec="0 32px"
                Layout="FitLayout"
                Cls="winForm-resp">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar4" Cls="pnNavVistas pnVistasForm" Dock="Top" PaddingSpec="20 20 0 10">
                        <Items>
                            <ext:Container runat="server"
                                ID="conNavVistasNewContactos"
                                Cls="nav-vistas"
                                ActiveIndex="2"
                                ActiveItem="2">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkUsuario"
                                        meta:resourceKey="lnkUsuario"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Value="3"
                                        Text="<%$ Resources:Comun, strUsuario %>">
                                        <Listeners>
                                            <Click Fn="showFormTab" />
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkPerfiles"
                                        meta:resourceKey="lnkPerfiles"
                                        Cls="lnk-navView lnk-noLine"
                                        Value="4"
                                        Text="<%$ Resources:Comun, strRoles %>">
                                        <Listeners>
                                            <Click Fn="showFormTab" />
                                        </Listeners>
                                    </ext:HyperlinkButton>

                                    <%--TAB DE PERMISOS--%>

                                    <%--<ext:HyperlinkButton runat="server"
                                        ID="lnkPermisos"
                                        meta:resourceKey="lnkPermisos"
                                        Cls="lnk-navView lnk-noLine"
                                        Value="5"
                                        Text="<%$ Resources:Comun, strPermisos %>">
                                        <Listeners>
                                            <Click Fn="showFormTab" />
                                        </Listeners>
                                    </ext:HyperlinkButton>--%>
                                    <%--<ext:HyperlinkButton runat="server" ID="lnkPermisosContrato" meta:resourceKey="lnkPermisosContrato" Cls="lnk-navView lnk-noLine" Value="6" Text="<%$ Resources:Comun, strPermisoContrato %>" Handler="showForms(this);">
                                        <DirectEvents>
                                            <Click OnEvent="Activate_Link_Click">
                                                <ExtraParams>
                                                    <ext:Parameter Name="index" Value="6" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:HyperlinkButton>--%>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Toolbar>
                    <ext:Toolbar runat="server" ID="Toolbar1" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill />
                            <ext:Button runat="server"
                                ID="btnPrevTab"
                                Width="100px"
                                meta:resourceKey="btnPrev"
                                IconCls="ico-close"
                                Cls="btn-secondary-winForm"
                                Text="<%$ Resources:Comun, jsCerrar %>">
                                <Listeners>
                                    <Click Fn="cerrarWindow" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnNext"
                                Disabled="true"
                                Width="100px"
                                meta:resourceKey="btnNext"
                                Cls="btn-ppal-winForm"
                                Text="<%$ Resources:Comun, strGuardar %>">
                                <Listeners>
                                    <Click Fn="guardarCambios" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>

                    <%--PANEL AÑADIR USUARIO--%>

                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        Cls="formGris formResp navActivo"
                        OverflowY="Auto"
                        Layout="FitLayout">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctForm"
                                OverflowY="Auto"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        meta:resourcekey="Apellidos"
                                        ID="txtApellidos"
                                        FieldLabel="<%$ Resources:Comun, strApellidos %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:DropDownField runat="server"
                                        meta:resourcekey="Telefono"
                                        ID="txtTelefono"
                                        FieldLabel="<%$ Resources:Comun, strTelefono %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="telefono"
                                        TriggerCls="icoprefix"
                                        FocusCls="testfocus"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Regex="/^[[+]([0-9])*]? ([0-9]){5,20}$/">
                                        <Component>
                                            <ext:MenuPanel runat="server" ID="MenuPanelIco" MaxWidth="120">
                                                <Menu runat="server" 
                                                    ID="MenuPrefijos">
                                                    <Items>
                                                        <ext:MenuItem 
                                                            runat="server" 
                                                            IconCls="x-loading-indicator" 
                                                            Text="<%$ Resources:Comun, jsMensajeProcesando %>" 
                                                            Focusable="false" 
                                                            HideOnClick="false" />
                                                    </Items>
                                                    <Loader runat="server" 
                                                        Mode="Component" 
                                                        DirectMethod="#{DirectMethods}.LoadPrefijos" 
                                                        RemoveAll="true">
                                                    </Loader>
                                                    <Listeners>
                                                        <Click Handler="#{txtTelefono}.setValue(menuItem.text + ' ');" />
                                                    </Listeners>
                                                </Menu>
                                            </ext:MenuPanel>
                                        </Component>
                                    </ext:DropDownField>
                                    <ext:TextField runat="server"
                                        meta:resourcekey="EMail"
                                        ID="txtEMail"
                                        FieldLabel="<%$ Resources:Comun, strEmail %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Vtype="email"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        meta:resourcekey="cmbEntidad"
                                        ID="cmbEntidad"
                                        Mode="Local"
                                        FieldLabel="<%$ Resources:Comun, strEntidades %>"
                                        DisplayField="Nombre"
                                        ValueField="EntidadID"
                                        StoreID="storeEntidades"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        Editable="true"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarEmpresaProveedora" />
                                            <TriggerClick Fn="RecargarEmpresaProveedoraAsociada" />
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:TextField runat="server"
                                        meta:resourcekey="clave"
                                        ID="txtClave"
                                        FieldLabel="<%$ Resources:Comun, strContraseña %>"
                                        EmptyText="<%$ Resources:Comun, strPasswordStrengthEmptyText %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EnableKeyEvents="true"
                                        InputType="Password"
                                        Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                                        RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                                    </ext:TextField>
                                    <ext:DateField runat="server"
                                        meta:resourcekey="FechaCaducidadUsuario"
                                        ID="txtFechaCaducidadUsuario"
                                        FieldLabel="<%$ Resources:Comun, strFechaCaducidad %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        MinDate="<%# DateTime.Now %>"
                                        AutoDataBind="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Format="<%$ Resources:Comun, FormatFecha %>" />
                                    <%--<ext:Container runat="server" ID="EmptyCOnt">
                                        <Content>
                                        </Content>
                                    </ext:Container>--%>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>

                    <%--PANEL AÑADIR PERFILES--%>
                    <ext:FormPanel ID="FormAnadirPerfiles"
                        Cls="formGris formResp"
                        OverflowY="Auto"
                        runat="server"
                        Hidden="true">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctFormPerfiles"
                                OverflowY="Auto"
                                Cls="winGestion-panel ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbRolesLibres"
                                        Cls=""
                                        MinWidth="200"
                                        FieldLabel="<%$ Resources:Comun, strRoles %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        DisplayField="Nombre"
                                        ValueField="RolID"
                                        StoreID="storeRolesLibres"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        MultiSelect="true"
                                        QueryMode="Local"
                                        Editable="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarPerfilesLibres" />
                                            <TriggerClick Fn="RecargarPerfilesLibres" />
                                            <%--<KeyUp Fn="ComboBoxKeyUp" />--%>
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="Button5"
                                        Width="100"
                                        Height="32"
                                        Cls="btn-mini-ppal btnAnadirEstadosSiguientes"
                                        IconCls="ico-addBtn"
                                        Text="<%$ Resources:Comun, strAnadir %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="BotonGuardarPerfiles();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel
                                        Hidden="false"
                                        Header="false"
                                        Height="240"
                                        runat="server"
                                        FocusCls="none"
                                        ForceFit="false"
                                        StoreID="storeRoles"
                                        ID="gridPerfilesAgregados"
                                        Cls="grdFormElAdded columGridFinal"
                                        OverflowX="Auto"
                                        OverflowY="Auto"
                                        EnableColumnHide="false"
                                        Scrollable="Vertical">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strRoles %>"
                                                    DataIndex="Nombre"
                                                    ID="Column1"
                                                    Flex="10"
                                                    PaddingSpec="0 0 0 8" />
                                                <ext:CommandColumn ID="colEliminarPerfil" runat="server" Width="50" Align="Center">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="EliminarPerfil" IconCls="ico-close">
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarPerfil" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters_PerfilesAgregados"
                                                MenuFilterText="<%$ Resources:Comun, GridFilters.MenuFilterText %>" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>

                    <%--PANEL AÑADIR PERMISOS--%>
                    <%--<ext:FormPanel runat="server"
                        ID="WrapFormPermisosUsuariosPrincipal"
                        Cls="formGris formResp"
                        PaddingSpec="0 16px 0 16px"
                        Hidden="true">
                        <DockedItems>
                            <ext:Toolbar runat="server" Dock="Top" Cls="tbGrey ExtraMarginxGrid">
                                <Items>
                                    <ext:Container runat="server" ID="ctFormPermisos" Cls="ctForm-resp ctForm-resp-col3">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                meta:resourcekey="cmbProyectosAgrupaciones"
                                                ID="cmbProyectosAgrupaciones"
                                                FieldLabel="<%$ Resources:Comun, strGrupo %>"
                                                Mode="Local"
                                                MinWidth="200"
                                                Width="250"
                                                DisplayField="ProyectoAgrupacion"
                                                ValueField="ProyectoAgrupacionID"
                                                StoreID="storeProyectosAgrupaciones"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                AllowBlank="true"
                                                ValidationGroup="FORM"
                                                EnableKeyEvents="true">
                                                <Listeners>
                                                    <Select Fn="SeleccionarProyectosAgrupaciones" />
                                                    <TriggerClick Fn="RecargarProyectosAgrupaciones" />
                                                    <KeyUp Fn="ComboBoxKeyUp" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        Icon="Clear"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:ComboBox runat="server"
                                                MinWidth="200"
                                                Width="250" meta:resourcekey="cmbProyectos"
                                                ID="cmbProyectos"
                                                FieldLabel="<%$ Resources:Comun, strProyectos %>"
                                                Mode="Local"
                                                DisplayField="Proyecto"
                                                ValueField="ProyectoID"
                                                StoreID="storeProyectos"
                                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                                LabelAlign="Top"
                                                AllowBlank="true"
                                                ValidationGroup="FORM"
                                                QueryMode="Local"
                                                Editable="true"
                                                MultiSelect="true">
                                                <Listeners>
                                                    <Select Fn="SeleccionarProyecto" />
                                                    <TriggerClick Fn="RecargarProyecto" />
                                                    <KeyUp Fn="ComboBoxKeyUp" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        Icon="Clear"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:Button runat="server"
                                                ID="Button7"
                                                Width="100"
                                                Height="32"
                                                Cls="btn-mini-ppal btnBold btnCenterGrid"
                                                IconCls="ico-addBtn"
                                                Text="<%$ Resources:Comun, strAnadir %>"
                                                Focusable="false"
                                                PressedCls="none"
                                                Hidden="false"
                                                MinWidth="90">
                                                <Listeners>
                                                    <Click Fn="btnGuardarPermiso" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:GridPanel
                                Hidden="false"
                                Header="false"
                                runat="server"
                                ForceFit="false"
                                StoreID="storePermisosAgregados"
                                Height="300"
                                FocusCls="none"
                                SelectionMemory="false"
                                ID="gridPermisos"
                                Cls="grdFormElAdded"
                                EnableColumnHide="false"
                                OverflowX="Auto"
                                Scrollable="Vertical">
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column runat="server" Text="<%$ Resources:Comun, strGrupo %>" DataIndex="ProyectoAgrupacion" ID="Column7" Flex="10" PaddingSpec="0 0 0 8"></ext:Column>
                                        <ext:Column runat="server" Text="<%$ Resources:Comun, strProyecto %>" DataIndex="Proyecto" ID="Column9" Flex="10" PaddingSpec="0 0 0 8"></ext:Column>
                                        <ext:CommandColumn ID="colEliminarPermiso" runat="server" Width="50" Align="Center">
                                            <Commands>
                                                <ext:GridCommand CommandName="EliminarPermiso" IconCls="ico-close">
                                                </ext:GridCommand>
                                            </Commands>
                                            <Listeners>
                                                <Command Fn="EliminarPermiso" />
                                            </Listeners>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel></ext:RowSelectionModel>
                                </SelectionModel>
                                <Plugins>
                                    <ext:GridFilters runat="server"
                                        ID="gridFilters_PermisosAgregados"
                                        MenuFilterText="<%$ Resources:Comun, GridFilters.MenuFilterText %>" />
                                    <%--<ext:CellEditing runat="server"
                                        ClicksToEdit="2" />
                                </Plugins>
                            </ext:GridPanel>
                        </Items>
                    </ext:FormPanel>--%>

                    <%--PANEL AÑADIR  PERMISOS CONTRATO--%>
                    <%--<ext:FormPanel ID="FormAnadirPermisosContrato" 
                        Cls="formWorkflow formGris " runat="server" Hidden="true" Layout="" Padding="20">
                        <Items>
                            <ext:Container runat="server" Layout="HBoxLayout" MarginSpec="0 0 20 0">
                                <Items>


                                    <ext:ComboBox runat="server" ID="ComboBox2" Cls="  " Flex="8" FieldLabel="<%$ Resources:Comun, strCategoriaContratos %>" LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Filtro 1" />
                                            <ext:ListItem Text="Filtro 2" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ToolbarFill Flex="1"></ext:ToolbarFill>

                                    <ext:ComboBox runat="server" ID="ComboBox3" Cls="  " Flex="8" FieldLabel="<%$ Resources:Comun, strContratos %>" LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Filtro 1" />
                                            <ext:ListItem Text="Filtro 2" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                    </ext:ComboBox>



                                </Items>
                            </ext:Container>

                            <ext:Container runat="server" Layout="HBoxLayout" MarginSpec="0 0 20 0">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>

                                    <ext:Button runat="server" ID="Button2" Cls="btn-ppal btnBold " IconCls="ico-addBtn" Text="<%$ Resources:Comun, strAnadir %>" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>

                                </Items>
                            </ext:Container>

                            <ext:GridPanel
                                Hidden="false"
                                Header="false"
                                runat="server"
                                ForceFit="false"
                                Height="300"
                                Focusable="false"
                                DisableSelection="true"
                                FocusCls="none"
                                ID="gridAddPermisosContrato"
                                Cls="gridPanel GridSimple"
                                OverflowX="Auto">


                                <Store>
                                    <ext:Store ID="Store2" runat="server">
                                        <Model>
                                            <ext:Model runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="NombrePerfil" />
                                                    <ext:ModelField Name="NombreContrato" />

                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>



                                        <ext:Column runat="server" Text="<%$ Resources:Comun, strRol %>" DataIndex="NombrePerfil" ID="Column2" Flex="10" PaddingSpec="0 0 0 8">
                                        </ext:Column>

                                        <ext:Column runat="server" Text="<%$ Resources:Comun, strContratos %>" DataIndex="NombreContrato" ID="Column3" Flex="10">
                                        </ext:Column>


                                        <ext:WidgetColumn ID="WidgetColumn2" runat="server" Cls="col-More" DataIndex="" Filterable="false" Hidden="false" MaxWidth="70" Flex="2">
                                            <Widget>
                                                <ext:Button runat="server" Width="20" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColCerrar" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                            </Widget>
                                        </ext:WidgetColumn>

                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>


                        </Items>

                    </ext:FormPanel>--%>
                </Items>
                <Listeners>
                    <Close Fn="Refrescar" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winDates"
                runat="server"
                Title="<%$ Resources:Comun, strFechas %>"
                Width="320"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Resizable="false"
                Closable="true"
                Layout="FitLayout">
                <Listeners>
                    <FocusLeave Handler="App.winDates.hide();" />
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar3" Cls=" greytb" Dock="Bottom" Height="16">
                        <%--<Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="Button15" Cls="btn-ppal " Text="<%$ Resources:Comun, strGuardar %>" Focusable="false" PressedCls="none" Hidden="false" MarginSpec="50 10 0 0" Handler="App.winDates.hide();"></ext:Button>

                        </Items>--%>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel ID="FormDates" Cls="formWorkflow formGris FormDates  " runat="server" Hidden="false" Padding="10" BodyCls="WhiteBg">
                        <%--<LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Middle" />
                        </LayoutConfig>--%>
                        <Items>
                            <ext:Container runat="server" ID="cont1" Layout="VBoxLayout" Width="150">
                                <Items>
                                    <ext:Label runat="server" Cls="lblSpaced" Text="<%$ Resources:Comun, strUltimoAcceso %>"></ext:Label>
                                    <ext:Label runat="server" ID="colUltimoAcceso" Cls="lblSpaced txtBold" Text=""></ext:Label>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server" ID="cont2" Layout="VBoxLayout" Width="150">
                                <Items>
                                    <ext:Label runat="server" Cls="lblSpaced" Text="<%$ Resources:Comun, strUltimaModificacionClave %>"></ext:Label>
                                    <ext:Label runat="server" ID="colUltimaModificacion" Cls="lblSpaced txtBold" Text=""></ext:Label>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server" ID="cont3" Layout="VBoxLayout" Width="150">
                                <Items>
                                    <ext:Label runat="server" Cls="lblSpaced" Text="<%$ Resources:Comun, strUsuarioExpiracion %>"></ext:Label>
                                    <ext:Label runat="server" ID="colCaducidadUsuario" Cls="lblSpaced txtBold" Text=""></ext:Label>
                                </Items>
                            </ext:Container>

                            <%--CLAVE EXPIRACION--%>

                            <%--<ext:Container runat="server" ID="cont4" Layout="VBoxLayout" Width="150">
                                <Items>
                                    <ext:Label runat="server" Cls="lblSpaced" Text="<%$ Resources:Comun, strClaveExpiracion %>"></ext:Label>
                                    <ext:Label runat="server" ID="colCaducidadClave" Cls="lblSpaced txtBold" Text=""></ext:Label>
                                </Items>
                            </ext:Container>--%>
                        </Items>
                    </ext:FormPanel>
                </Items>

            </ext:Window>

            <ext:Window ID="winCambiarClave"
                runat="server"
                Title="Agregar"
                Width="400"
                AutoHeight="true"
                Modal="true"
                Resizable="false"
                ShowOnLoad="false"
                Hidden="true"
                meta:resourceKey="winCambiarClave">
                <Listeners>
                    <Resize Handler="winCambiarClave(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="formClave"
                        BodyStyle="padding:10px;"
                        Border="false"
                        runat="server">
                        <Items>
                            <ext:TextField ID="txtCambiarClave"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strContraseña %>"
                                Text="<%$ Resources:Comun, strContraseña %>"
                                InputType="Password"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM2"
                                EnableKeyEvents="true"
                                meta:resourceKey="txtCambiarClave"
                                Anchor="90%"
                                Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                                RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                            </ext:TextField>
                            <ext:TextField ID="txtCambiarClave2"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strConfirmarContraseña %>"
                                Text="<%$ Resources:Comun, strConfirmarContraseña %>"
                                InputType="Password"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM2"
                                EnableKeyEvents="true"
                                meta:resourceKey="txtCambiarClave2"
                                Anchor="90%"
                                Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                                RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                            </ext:TextField>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoCambiarClave(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancelarCodigo"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winCambiarClave}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnCambiarGuardar"
                        runat="server"
                        Cls="btn-accept"
                        Disabled="true"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>">
                        <Listeners>
                            <Click Handler="winCambiarClaveBotonCambiar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window ID="winDuplicarConfig"
                runat="server"
                Title="Agregar"
                Width="450"
                AutoHeight="true"
                Modal="true"
                Resizable="false"
                Hidden="true"
                meta:resourceKey="winDuplicarConfig">
                <%--<Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>--%>
                <Items>
                    <ext:FormPanel ID="formDuplicarConfig"
                        runat="server"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox meta:resourceKey="cmbUsuarios"
                                ID="cmbUsuarios"
                                runat="server"
                                StoreID="storeUsuarios"
                                Mode="Local"
                                DisplayField="NombreCompleto"
                                ValueField="UsuarioID"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                FieldLabel="<%$ Resources:Comun, strUsuarioDestino %>"
                                AllowBlank="false"
                                ForceSelection="true"
                                Anchor="95%"
                                EnableKeyEvents="true"
                                Cls="item-form comboForm">
                                <Listeners>
                                    <KeyUp Fn="ComboBoxKeyUp" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoDuplicarConfig(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnGuardarDuplicarConfigCerrar"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winDuplicarConfig}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnGuardarDuplicarConfig"
                        runat="server"
                        Cls="btn-accept"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winDuplicarConfigBotonGuardar();" />
                        </Listeners>
                    </ext:Button>

                </Buttons>
                <Listeners>
                    <Show Handler="#{winDuplicarConfig}.center();" />
                </Listeners>
            </ext:Window>

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
                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strUsuarios %>" Height="25" />
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
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        AriaRole="main"
                                        ContextMenuID="ContextMenu">
                                        <Listeners>
                                            <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                            <Resize Handler="GridColHandler(this)"></Resize>
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnAnadir"
                                                        meta:resourceKey="btnAnadir"
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
                                                        Disabled="true"
                                                        Handler="MostrarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        meta:resourceKey="btnEliminar"
                                                        Hidden="true"
                                                        Disabled="true"
                                                        Cls="btn-Eliminar"
                                                        Handler="Eliminar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        meta:resourceKey="btnRefrescar"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatos('Usuarios', hdCliID.value, #{grid}, App.btnActivo.pressed, '', App.cmbEntidades.value);" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        Cls="btnActivar"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Handler="Activar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnPassword"
                                                        Cls="btn-trans btnPasswordKey"
                                                        AriaLabel="<%$ Resources:Comun, strContraseña %>"
                                                        ToolTip="<%$ Resources:Comun, strContraseña %>"
                                                        Handler="BotonCambiarClave();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnAgregarDocumentos"
                                                        Cls="btnDocumentos"
                                                        Disabled="true"
                                                        Hidden="true"
                                                        AriaLabel="<%$ Resources:Comun, strDocumento %>"
                                                        ToolTip="<%$ Resources:Comun, strDocumento %>"
                                                        Handler="AbrirDocumentos();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDuplicarUsuario"
                                                        Cls="btnCopiarUser"
                                                        Disabled="true"
                                                        AriaLabel="<%$ Resources:Comun, strDuplicarUsuario %>"
                                                        ToolTip="<%$ Resources:Comun, strDuplicarUsuario %>"
                                                        meta:resourceKey="btnDuplicar"
                                                        Handler="btnDuplicarUsuarioClick();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDuplicarConfig"
                                                        Disabled="true"
                                                        Cls="btnCopiarConfig"
                                                        AriaLabel="<%$ Resources:Comun, strDuplicarConfiguracion %>"
                                                        ToolTip="<%$ Resources:Comun, strDuplicarConfiguracion %>"
                                                        meta:resourceKey="btnDuplicarConfig"
                                                        Handler="btnDuplicarConfigClick();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDesvincularDispositivoMovil"
                                                        Disabled="true"
                                                        Cls="btnLinkBroken"
                                                        AriaLabel="<%$ Resources:Comun, strDesvincularDispositivoMovil %>"
                                                        ToolTip="<%$ Resources:Comun, strDesvincularDispositivoMovil %>"
                                                        Handler="btnDesvincularDispositivoMovil();" />
                                                    <ext:Label runat="server"
                                                        ID="lblToggle"
                                                        Cls="lblBtnActivo"
                                                        PaddingSpec="6 0 25 0"
                                                        Text="<%$ Resources:Comun, strActivo %>" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivo"
                                                        Width="41"
                                                        Pressed="true"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid"
                                                        AriaLabel="<%$ Resources:Comun, strEmplazamientosCliente %>"
                                                        ToolTip="<%$ Resources:Comun, strActivo %>"
                                                        Handler="Refrescar();" />
                                                    <ext:ToolbarFill />
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbEntidades"
                                                        StoreID="storeEntidades"
                                                        MarginSpec="10 16 0 0"
                                                        QueryMode="Local"
                                                        LabelAlign="Top"
                                                        MaxWidth="250"
                                                        DisplayField="Nombre"
                                                        ValueField="EntidadID"
                                                        EmptyText="<%$ Resources:Comun, strEntidades %>"
                                                        WidthSpec="90%"
                                                        Editable="true">
                                                        <Listeners>
                                                            <Select Fn="SeleccionarEntidad" />
                                                            <TriggerClick Fn="RecargarEntidad" />
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

                                            <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                <Content>

                                                    <local:toolbarfiltros
                                                        ID="cmpFiltro"
                                                        runat="server"
                                                        Stores="storePrincipal"
                                                        MostrarComboFecha="false"
                                                        FechaDefecto="Dia"
                                                        Grid="grid"
                                                        QuitarBotonesFiltros="true"
                                                        MostrarBusqueda="false" />
                                                </Content>
                                            </ext:Container>

                                        </DockedItems>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Cls="col-activo"
                                                    Flex="1"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    meta:resourceKey="colActivo"
                                                    MinWidth="50"
                                                    MaxWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    DataIndex="Nombre"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    ID="ColName" />
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strApellidos %>"
                                                    DataIndex="Apellidos"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    ID="ColStep" />
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strTelefono %>"
                                                    DataIndex="Telefono"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    ID="ColNumTelefono" />
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strEmail %>"
                                                    DataIndex="EMail"
                                                    Flex="3"
                                                    MinWidth="200"
                                                    ID="ColEmail" />
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strEntidades %>"
                                                    DataIndex="NombreEntidad"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    ID="ColEmpresa" />
                                                <ext:DateColumn runat="server"
                                                    Text="<%$ Resources:Comun, strFechaUltimoAcceso %>"
                                                    DataIndex="FechaUltimoAcceso"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    Format="dd/MM/yyyy"
                                                    ID="colFechaUltimoAcceso" />
                                                <ext:DateColumn runat="server"
                                                    Text="<%$ Resources:Comun, strFechaCambio %>"
                                                    DataIndex="FechaUltimoCambio"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    Format="dd/MM/yyyy"
                                                    ID="colFechaUltimoCambio" />
                                                <ext:DateColumn runat="server"
                                                    Text="Expiration Date User"
                                                    DataIndex="FechaCaducidadUsuario"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    Format="dd/MM/yyyy"
                                                    ID="colFechaCaducidad" />
                                                <ext:DateColumn runat="server"
                                                    Text="Expiration Date Password"
                                                    DataIndex="FechaCaducidadClave"
                                                    Flex="2"
                                                    Format="dd/MM/yyyy"
                                                    MinWidth="120"
                                                    ID="colFechaCaducidadClave" />
                                                <%--<ext:ComponentColumn runat="server"
                                                    meta:resourceKey="colFechas"
                                                    ID="colVerFechas"
                                                    Flex="1"
                                                    MinWidth="90"
                                                    MaxWidth="90"
                                                    Text="<%$ Resources:Comun, strFechas %>"
                                                    Cls="excluirPnInfo"
                                                    Align="Center"
                                                    Sortable="false">
                                                    <Component>
                                                        <ext:Button
                                                            ID="btnFechas"
                                                            Cls="btnColMoreX"
                                                            OverCls="none"
                                                            PressedCls="none"
                                                            FocusCls="Focus-none"
                                                            Flat="true"
                                                            runat="server">
                                                            <Listeners>
                                                                <Click Fn="verFechas" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Component>
                                                </ext:ComponentColumn>--%>
                                                <%--<ext:Column runat="server"
                                                    ID="ColPassCambiada"
                                                    DataIndex="CambiarClave"
                                                    Align="Center"
                                                    Flex="2"
                                                    MinWidth="120"
                                                    Text="<%$ Resources:Comun, strCambiarContraseña %>">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="ColLPDA"
                                                    DataIndex="LDAP"
                                                    Align="Center"
                                                    Flex="1"
                                                    MinWidth="90"
                                                    Text="<%$ Resources:Comun, strLDAP %>">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="ColIDOAM"
                                                    DataIndex="UsuarioOAMID"
                                                    Align="Center"
                                                    MinWidth="120"
                                                    Text="<%$ Resources:Comun, strIDOAM %>"
                                                    Flex="2">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>--%>
                                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="NoOcultar col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MaxWidth="90" MinWidth="60" Flex="99999">
                                                    <Widget>
                                                        <ext:Button ID="btnMore" runat="server" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                                            <Listeners>
                                                                <Click Fn="hidePanelMoreInfo" />
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
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePrincipal"
                                                OverflowHandler="Scroller"
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
