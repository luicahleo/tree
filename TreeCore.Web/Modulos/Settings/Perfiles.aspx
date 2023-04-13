<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="TreeCore.ModGlobal.pages.Perfiles" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>
</head>
<body>
    <link href="css/stylePerfiles.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.amsify.suggestags.js"></script>
    <form id="form1" runat="server">
        <div>
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdProyectoTipoSeleccionado" runat="server" />
            <ext:Hidden ID="hdPerfilSeleccionado" runat="server" />
            <ext:Hidden ID="hdFuncionalidadModuloSeleccionado" runat="server" />
            <ext:Hidden ID="hdPaginaSeleccionada" runat="server" />
            <ext:Hidden ID="hdRolSeleccionado" runat="server" />

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Store runat="server" ID="storeModules" AutoLoad="true" OnReadData="storeModules_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="CargarTreePerfiles" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server" ID="storeUserFuntionalityTypes" AutoLoad="true" OnReadData="storeUserFuntionalityTypes_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeUserInterfaces" AutoLoad="false" OnReadData="storeUserInterfaces_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Functionalities" Type="Object" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeRoles" AutoLoad="false" OnReadData="storeRoles_Refresh" RemoteSort="false"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="RolID,Codigo,Descripcion">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Descripcion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                    <BeforeLoad Fn="DeseleccionarGrillaRoles" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server" ID="storePerfiles" AutoLoad="false" OnReadData="storePerfiles_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="PerfilID">
                        <Fields>
                            <ext:ModelField Name="PerfilID" Type="Int" />
                            <ext:ModelField Name="Perfil_esES" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Perfil_esES" Direction="ASC" />
                </Sorters>
                <Listeners>
                </Listeners>
            </ext:Store>
            <ext:Store runat="server" ID="storePerfilesAsignados" AutoLoad="false" OnReadData="storePerfilesAsignados_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="PerfilID">
                        <Fields>
                            <ext:ModelField Name="PerfilID" Type="Int" />
                            <ext:ModelField Name="Perfil_esES" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Perfil_esES" Direction="ASC" />
                </Sorters>
                <Listeners>
                </Listeners>
            </ext:Store>
            <ext:Store runat="server" ID="storePermisosRoles" AutoLoad="false" OnReadData="storePermisosRoles_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ObjetoID">
                        <Fields>
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="ListaItem" Type="Object" />

                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
                <Listeners>
                </Listeners>
            </ext:Store>

            <ext:Window ID="winAddProfile"
                runat="server"
                Title="Add Profile"
                Height="500"
                Width="340"
                BodyCls=""
                BodyPaddingSummary="8px 32px 0"
                Modal="true"
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
                    <ext:Toolbar runat="server" ID="Toolbar10" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarPerfil"
                                Cls="btn-secondary"
                                Icon="Cancel"
                                MinWidth="110"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="App.winAddProfile.hide()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarPerfil"
                                Cls="btn-ppal"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Icon="Accept"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="BtnGuardarPerfil" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server"
                        ID="txtNombrePerfil"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioPerfil" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextArea runat="server"
                        ID="txtaDescripcionPerfil"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioPerfil" />
                        </Listeners>
                    </ext:TextArea>
                    <ext:ComboBox runat="server"
                        ID="cmbPerfilesFuncionalidadesTipos"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                        ValueField="Code"
                        DisplayField="Name"
                        StoreID="storeUserFuntionalityTypes"
                        QueryMode="Local"
                        Mode="Local"
                        Hidden="true"
                        AllowBlank="true">
                        <Triggers>
                            <ext:FieldTrigger IconCls="ico-reload"
                                Hidden="true"
                                Weight="-1"
                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                        </Triggers>
                        <Listeners>
                            <Select Fn="SeleccionarCombo" />
                            <TriggerClick Fn="RecargarCombo" />
                            <ValidityChange Fn="ValidarFormularioPerfil" />
                        </Listeners>
                    </ext:ComboBox>
                </Items>
            </ext:Window>

            <ext:Window ID="winAddFuncionalidad"
                runat="server"
                Title="Add Functionality"
                Height="515"
                Width="340"
                BodyCls=""
                BodyPaddingSummary="8px 32px 0"
                Modal="true"
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
                    <ext:Toolbar runat="server" ID="Toolbar5" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarFuncionalidad"
                                Cls="btn-secondary"
                                MinWidth="110"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="App.winAddFuncionalidad.hide()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarFuncionalidad"
                                Cls="btn-ppal"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="BtnGuardarFuncionalidad" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server"
                        ID="txtNombreFuncionalidad"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioFuncionalidad" />
                        </Listeners>
                    </ext:TextField>
                    <ext:NumberField runat="server"
                        ID="nbCodigoFuncionalidad"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                        WidthSpec="100%"
                        MinValue="0"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioFuncionalidad" />
                        </Listeners>
                    </ext:NumberField>
                    <ext:TextField runat="server"
                        ID="txtAliasFuncionalidad"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strKeyTraduccion %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioFuncionalidad" />
                        </Listeners>
                    </ext:TextField>
                    <ext:ComboBox runat="server"
                        ID="cmbTipoFuncionalidadFuncionalidad"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                        ValueField="Code"
                        DisplayField="Name"
                        StoreID="storeUserFuntionalityTypes"
                        QueryMode="Local"
                        Mode="Local"
                        AllowBlank="false">
                        <Triggers>
                            <ext:FieldTrigger IconCls="ico-reload"
                                Hidden="true"
                                Weight="-1"
                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                        </Triggers>
                        <Listeners>
                            <Select Fn="SeleccionarTipoFuncionalidadFuncionalidad" />
                            <TriggerClick Fn="RecargarCombo" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:TextArea runat="server"
                        ID="txtaDescripcionFuncionalidad"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioFuncionalidad" />
                        </Listeners>
                    </ext:TextArea>
                </Items>
            </ext:Window>

            <ext:Window ID="winAddModulo"
                runat="server"
                Title="Add Modulo"
                Height="450"
                Width="340"
                BodyCls=""
                BodyPaddingSummary="8px 32px 0"
                Modal="true"
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
                    <ext:Toolbar runat="server" ID="ToolbarModulo" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarModulo"
                                Cls="btn-secondary"
                                MinWidth="110"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="App.winAddModulo.hide()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarModulo"
                                Cls="btn-ppal"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="BtnGuardarModulo" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server"
                        ID="txtNombreModulo"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioModulo" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField runat="server"
                        ID="txtPaginaModulo"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strPagina %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioModulo" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField runat="server"
                        ID="txtAliasModulo"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strKeyTraduccion %>"
                        WidthSpec="100%"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioModulo" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextArea runat="server"
                        ID="txtaDescripcionModulo"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                        AllowBlank="false">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioModulo" />
                        </Listeners>
                    </ext:TextArea>

                    <ext:Container runat="server" Cls="ctForm-mini-col2">
                        <Items>
                            <ext:Checkbox runat="server" ID="CheckSuperUser" LabelAlign="Right" BoxLabel="<%$ Resources:Comun, strSuper %>" Cls="chkboxLabelGrid" PaddingSpec="0 20 0 20"></ext:Checkbox>

                            <ext:Checkbox runat="server" ID="CheckProduccion" LabelAlign="Right" BoxLabel="<%$ Resources:Comun, strProduccion %>" Cls="chkboxLabelGrid" PaddingSpec="0 20 0 20"></ext:Checkbox>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Window>

            <ext:Window ID="winGestionRoles"
                runat="server"
                Title="Add ROL"
                Height="400"
                Width="340"
                BodyCls=""
                Modal="true"
                BodyPaddingSummary="8px 32px 0"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="FitLayout">
                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar3" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarRol"
                                Cls="btn-secondary"
                                MinWidth="110"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="App.winGestionRoles.hide()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarRol"
                                Cls="btn-ppal"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="ajaxAgregarEditar" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server" ID="formRoles" Cls="formGris">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctFormRol"
                                Cls="winGestion-panel ctForm-resp">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCodigo"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        AllowBlank="false"
                                        WidthSpec="100%"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        AllowBlank="false"
                                        WidthSpec="100%"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                    </ext:TextField>
                                    <ext:TextArea runat="server"
                                        ID="txtaDescripcion"
                                        LabelAlign="Top"
                                        WidthSpec="100%"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        ValidationGroup="FORM"
                                        AllowBlank="true">
                                    </ext:TextArea>
                                    <%--<ext:TextField runat="server"
                                        ID="txtDescripcion"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        CausesValidation="true"
                                        ValidationGroup="FORM"
                                        AllowBlank="true">
                                    </ext:TextField>--%>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="validarFormulario(valid);"></ValidityChange>
                        </Listeners>
                    </ext:FormPanel>


                </Items>
            </ext:Window>

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                    <AfterLayout Handler=""></AfterLayout>


                </Listeners>
                <Items>
                    <%-----------------------Panel con menu visor elementos gráficos---------------------%>
                    <ext:Panel ID="pnComboGrdVisor"
                        runat="server"
                        Header="false"
                        Cls="col-pdng"
                        Layout="BorderLayout">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="TbNavegacionTabs" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkRoles"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="Roles">
                                        <Listeners>
                                            <Click Handler="NavegacionTabs(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkPerfiles"
                                        Cls="lnk-navView lnk-noLine"
                                        meta:resourceKey="lnkPerfiles"
                                        Text="PROFILES">
                                        <Listeners>
                                            <Click Handler="NavegacionTabs(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkFuncionalidades"
                                        Cls="lnk-navView lnk-noLine "
                                        meta:resourceKey="lnkFuncionalidades"
                                        Text="FUNCTIONALITIES">
                                        <Listeners>
                                            <Click Handler="NavegacionTabs(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                Hidden="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn"
                                BodyCls="pnCentralWrap-body">
                                <Listeners>
                                    <AfterLayout Handler="ControlPanelesProfile(this)"></AfterLayout>
                                    <Resize Handler="ControlPanelesProfile(this)"></Resize>
                                </Listeners>
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="ToolbarSpacer" Dock="Top" Hidden="true" MinHeight="36" Cls="tbGrey">
                                        <Items>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tbFiltrosYSlidersProfile"
                                        Dock="Top"
                                        Cls="tbGrey tbNoborder "
                                        Hidden="true"
                                        Layout="HBoxLayout"
                                        Flex="1">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Toolbar runat="server"
                                                ID="tbSlidersProfile"
                                                Dock="Top"
                                                Hidden="false"
                                                MinHeight="36"
                                                Cls="tbGrey tbNoborder">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnPrevP"
                                                        IconCls="ico-prev-w"
                                                        Cls="btnMainSldr SliderBtn"
                                                        Handler="SliderMoveProfile('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNextP"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="SliderMoveProfile('Next');"
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="ctMain1" Flex="2" MaxWidth="440" Layout="FitLayout" Cls="col colCt1" Hidden="false" MinWidth="260">
                                        <Items>
                                            <ext:TreePanel
                                                ForceFit="true"
                                                Hidden="false"
                                                Header="true"
                                                runat="server"
                                                Cls="gridPanel  TreePnl"
                                                Title="PROFILES"
                                                ID="TreePanelPerfiles"
                                                UseArrows="true"
                                                RootVisible="false"
                                                MultiSelect="true"
                                                SingleExpand="true"
                                                OverflowX="Hidden"
                                                meta:resourceKey="TreePanelPerfiles"
                                                OverflowY="Auto">

                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbAdministracionPerfiles"
                                                        Dock="Top"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server" Disabled="true" ID="btnAnadirPerfiles" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnAgregarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" Disabled="true" ID="btnEditarPerfiles" Cls="btnEditar" AriaLabel="Editar" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnEditarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" Disabled="true" ID="btnEliminarPerfiles" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnEliminarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescarPerfiles" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnRefrescarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivarPerfiles"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                meta:resourceKey="btnActivar"
                                                                Disabled="true"
                                                                Cls="btnActivar">
                                                                <Listeners>
                                                                    <Click Fn="BtnActivarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnSoloActivosPerfiles"
                                                                Width="41"
                                                                EnableToggle="true"
                                                                Cls="btn-toggleGrid"
                                                                Hidden="false"
                                                                Pressed="true"
                                                                AriaLabel=""
                                                                ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnRefrescarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnDescargarPerfiles" Cls="btnDescargar" AriaLabel="Descargar" Hidden="true" ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="BtnDescargarPerfil" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-next" Cls="btnMainSldr SliderBtn" Handler="moveCtSldr(this);" Disabled="false"></ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server" ID="tlbFiltrosPerfiles" Cls="" Layout="VBoxLayout" Dock="Top">
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                        </LayoutConfig>
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtFiltroPerfiles"
                                                                Cls="txtSearchD"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                MarginSpec="0 8 20 0"
                                                                EnableKeyEvents="true"
                                                                Flex="1">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <KeyUp Fn="FiltrarPerfiles" Buffer="250" />
                                                                    <TriggerClick Fn="LimpiarFiltroPerfiles" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                            <ext:ComboBox
                                                                ID="cmbTipoProyecto"
                                                                Cls="txtSearchD"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                FieldLabel="<%$ Resources:Comun, strModulo %>"
                                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                                ValueField="Code"
                                                                DisplayField="Name"
                                                                StoreID="storeModules"
                                                                QueryMode="Local"
                                                                Mode="Local"
                                                                AllowBlank="true"
                                                                Flex="1">
                                                                <Triggers>
                                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        Weight="-1"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarProyectosTipos" />
                                                                    <TriggerClick Fn="RecargarProyectosTipos" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TreeColumn
                                                            ID="ColNombre"
                                                            runat="server"
                                                            Filterable="false"
                                                            Flex="1"
                                                            Text="<%$ Resources:Comun, strPerfil %>"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="Nombre">
                                                            <Filter>
                                                                <ext:StringFilter />
                                                            </Filter>
                                                        </ext:TreeColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server" />
                                                </Plugins>
                                                <SelectionModel>
                                                    <ext:TreeSelectionModel runat="server"
                                                        ID="GridPerfilesRowSelect"
                                                        Mode="Single">

                                                        <Listeners>
                                                            <Select Fn="GridPerfiles_Row_Select" />
                                                        </Listeners>
                                                    </ext:TreeSelectionModel>
                                                </SelectionModel>
                                            </ext:TreePanel>
                                        </Items>
                                    </ext:Panel>


                                    <ext:Panel runat="server" ID="ctMain2" Flex="6" Layout="FitLayout" Cls="pnHeaderGr" Hidden="false" MarginSpec="0 12 0 8" MinWidth="350" Header="true" Title="FUNCTIONALITIES" Scrollable="Vertical"
                                        meta:resourceKey="ctMain2">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tblConfiguracionPerfiles" MarginSpec="15 0 0 0" Height="35">
                                                <Items>
                                                    <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-prev" Cls="SliderBtn" Handler="moveCtSldr(this);" Disabled="false" Hidden="true"></ext:Button>
                                                    <ext:Container runat="server" WidthSpec="100%" Cls="gridLblTitleProfile">
                                                        <Items>
                                                            <ext:Label ID="lbNombrePerfil" runat="server" Cls="PanelFuncTitle"></ext:Label>
                                                            
                                                            <ext:Label ID="lbDescripcionPerfil" Hidden="true" runat="server" Cls="" PaddingSpec="0 20 20 18"></ext:Label>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" ID="Toolbar2" Dock="Bottom" MarginSpec="15 10 5 0">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        ID="BtnConfirmarCambiosPerfiles"
                                                        Cls="btn-ppal-winForm"
                                                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                        Focusable="false"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="BtnGuardarFuncionalidades" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:DataView
                                                runat="server"
                                                ID="DTUserInterfaces"
                                                SingleSelect="true"
                                                ItemSelector="div.interface"
                                                StoreID="storeUserInterfaces">
                                                <Tpl runat="server">
                                                    <Html>
                                                        <div id="items-ct">
                                                            <tpl for=".">
                                                                <div class="interface" code="{Code}">
                                                                    <h4 class="interfaceTitle spanLbl">{Name}</h4>
                                                                    <div class="contFunctionalities">
                                                                        <tpl for="Functionalities">
                                                                            <div class="contFuntionality">
                                                                                <input type="checkbox" id="chkDone{Code}" class="chkDone" code="{Code}" {Asignada} />
                                                                                <label class="lblNombre spanLbl">
                                                                                    {Name}
                                                                                </label>
                                                                            </div>
                                                                        </tpl>
                                                                    </div>
                                                                </div>
                                                            </tpl>
                                                        </div>
                                                    </Html>
                                                </Tpl>
                                            </ext:DataView>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <ext:Panel ID="pnFuncionalidades"
                                Visible="true"
                                Hidden="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn ">
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="ToolbarSpacer2" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey">
                                        <Items>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:TreePanel
                                        ForceFit="true"
                                        Hidden="false"
                                        Header="false"
                                        runat="server"
                                        Cls="gridPanel grdNoHeader TreePnl"
                                        ID="TreePanelFuncionalidades"
                                        UseArrows="true"
                                        RootVisible="false"
                                        MultiSelect="true"
                                        SingleExpand="true"
                                        FolderSort="true"
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar
                                                runat="server"
                                                ID="tlbAdministracionTreePanelFuncionalidades"
                                                Dock="Top"
                                                Cls="tlbGrid c-Grid">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnAnadirFuncionalidades"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="BtnAgregarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditarFuncionalidades"
                                                        Cls="btnEditar"
                                                        AriaLabel="Editar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="BtnEditarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminarFuncionalidades"
                                                        Cls="btnEliminar"
                                                        AriaLabel="Eliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="BtnEliminarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivarFuncionalidades"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        meta:resourceKey="btnActivar"
                                                        Cls="btnActivar"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="BtnActivarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnSoloActivosFuncionalidades"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid"
                                                        Hidden="false"
                                                        Pressed="true"
                                                        AriaLabel=""
                                                        ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="BtnRefrescarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnRefrescarFuncionalidades" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="BtnRefrescarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnDescargarFuncionalidades" Cls="btnDescargar" AriaLabel="Descargar" Hidden="true" ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="BtnDescargarFuncionalidad" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server"
                                                ID="tlbFiltrosTreePanelFuncionalidades"
                                                Cls="tlbGrid"
                                                Layout="ColumnLayout"
                                                Dock="Top">
                                                <Items>
                                                    <ext:TextField
                                                        ID="txtFiltroFuncionalidades"
                                                        Cls="txtSearchC"
                                                        runat="server"
                                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                        LabelWidth="50"
                                                        EnableKeyEvents="true"
                                                        Width="250">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <KeyUp Fn="FiltrarFuncionaliades" Buffer="250" />
                                                            <TriggerClick Fn="LimpiarFiltroFuncionalidades" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:TreeColumn
                                                    ID="ColNombreFuncionalidades"
                                                    runat="server"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    Flex="3"
                                                    Sortable="true"
                                                    Hidden="false"
                                                    DataIndex="Nombre" />
                                                <ext:Column
                                                    ID="ColInfoFuncionalidades"
                                                    runat="server"
                                                    Text="<%$ Resources:Comun, strPagina %>"
                                                    Flex="2"
                                                    Sortable="true"
                                                    DataIndex="Info"
                                                    Hidden="false">
                                                </ext:Column>
                                                <ext:Column
                                                    ID="ColTipoFuncionalidades"
                                                    runat="server"
                                                    Text="<%$ Resources:Comun, strTipoFuncionalidad %>"
                                                    Flex="2"
                                                    Editable="true"
                                                    Hidden="false"
                                                    DataIndex="TipoFuncionalidad" />
                                                <ext:Column runat="server"
                                                    ID="ColDescripcionFuncionalidades"
                                                    Text="<%$ Resources:Comun, strDescripcion %>"
                                                    Flex="4"
                                                    DataIndex="Descripcion"
                                                    MenuDisabled="true"
                                                    Hidden="false">
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colSuperUser"
                                                    DataIndex="SuperUser"
                                                    Align="Center"
                                                    Text="<%$ Resources:Comun, strSuper %>"
                                                    meta:resourceKey="colSuperUser"
                                                    Flex="1"
                                                    MinWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colProduccion"
                                                    DataIndex="Produccion"
                                                    Align="Center"
                                                    Text="<%$ Resources:Comun, strProduccion %>"
                                                    meta:resourceKey="colProduccion"
                                                    Flex="1"
                                                    MinWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:TreeSelectionModel runat="server"
                                                ID="TreeFuncionalidadesSelectionModel"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="GridFuncionalidades_Row_Select" />
                                                </Listeners>
                                            </ext:TreeSelectionModel>
                                        </SelectionModel>
                                    </ext:TreePanel>
                                </Items>
                            </ext:Panel>

                            <ext:Panel ID="wrapComponenteCentral"
                                Visible="true"
                                Hidden="false"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn"
                                BodyCls="pnCentralWrap-body">
                                <Listeners>
                                    <AfterLayout Handler="ControlPaneles(this)"></AfterLayout>
                                    <Resize Handler="ControlPaneles(this)"></Resize>
                                </Listeners>
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tbFiltrosYSliders"
                                        Dock="Top"
                                        Cls="tbGrey tbNoborder "
                                        Hidden="true"
                                        Layout="HBoxLayout"
                                        Flex="1">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Toolbar runat="server"
                                                ID="tbSliders"
                                                Dock="Top"
                                                Hidden="false"
                                                MinHeight="36"
                                                Cls="tbGrey tbNoborder">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnPrev"
                                                        IconCls="ico-prev-w"
                                                        Cls="btnMainSldr SliderBtn"
                                                        Handler="SliderMove('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
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

                                    <ext:Panel runat="server" ID="pnRoles" Flex="2" Layout="FitLayout" Cls="col colCt1 TreePL" Hidden="false" MinWidth="260">
                                        <Items>
                                            <ext:GridPanel
                                                Hidden="false"
                                                Header="true"
                                                HideHeaders="true"
                                                runat="server"
                                                Cls="gridPanel "
                                                Title="<%$ Resources:Comun, strRoles %>"
                                                ID="gridRoles"
                                                StoreID="storeRoles"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">

                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnAnadirRol" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="btnAnadirRoles" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" Disabled="true" ID="btnEditarRol" Cls="btnEditar" AriaLabel="Editar" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="btnEditarRoles" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" Disabled="true" ID="btnEliminarRol" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="btnEliminarRoles" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescarRol" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="btnRefrescarRoles" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivarRol"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                meta:resourceKey="btnActivar"
                                                                Disabled="true"
                                                                Cls="btnActivar">
                                                                <Listeners>
                                                                    <Click Fn="ajaxActivarRol" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivosRol"
                                                                Width="41"
                                                                EnableToggle="true"
                                                                Cls="btn-toggleGrid"
                                                                Hidden="false"
                                                                Pressed="true"
                                                                AriaLabel=""
                                                                ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                                Handler="btnRefrescarRoles();">
                                                            </ext:Button>

                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                        <Content>

                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storeRoles"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                Grid="gridRoles"
                                                                MostrarBusqueda="false"
                                                                QuitarFiltros="true" />
                                                        </Content>
                                                    </ext:Container>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column
                                                            ID="colNombreRol"
                                                            runat="server"
                                                            Flex="1"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            DataIndex="Nombre" />
                                                    </Columns>
                                                </ColumnModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server" />
                                                </Plugins>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server" ID="GridRowSelectRoles" PruneRemoved="false" Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelectRoles" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>


                                    <ext:Panel runat="server" ID="pnDetalle" Flex="6" Cls="pnHeaderGr" Hidden="false" MarginSpec="0 12 0 8" Header="true" OverflowY="Auto" Title="<%$ Resources:Comun, strVistaDetalle %>">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar4" MarginSpec="10 0 0 0" Height="50">
                                                <Items>

                                                    <ext:Label ID="lbCodigoRol" runat="server" Cls="PanelFuncTitle"></ext:Label>
                                                    <ext:Label ID="lbTituloRol" runat="server" Cls="PanelFuncTitle"></ext:Label>

                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" ID="Toolbar6" MarginSpec="0 20 0 18" Height="30">
                                                <Items>
                                                    <ext:Label ID="lbDescripcionRol" runat="server" Cls=""></ext:Label>
                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>
                                        <Items>

                                            <ext:Container runat="server" ID="cnPerfilesRol" Cls="ContenedorCombo" Hidden="true" Height="100">

                                                <Content>
                                                    <div class="form-group">
                                                        <input type="text" class="form-control" name="perfiles">
                                                    </div>

                                                </Content>
                                                <Listeners>
                                                </Listeners>
                                            </ext:Container>
                                            <ext:Panel runat="server" ID="pnPermisosRoles" Cls="pnPermisosRoles" Hidden="true">
                                                <Items>
                                                    <ext:Container runat="server" ID="ctCombosPermisos" Cls="ctCombosPermisos winGestion-paneles ctForm-resp-col4">
                                                        <Items>

                                                            <ext:ComboBox runat="server"
                                                                ID="cmbFiltroPermisos"
                                                                ValueField="Alls"
                                                                Cls="comboPermisos"
                                                                EmptyText="<%$ Resources:Comun, strTipo %>"
                                                                DisplayField="Alls"
                                                                Flex="8">
                                                                <Items>
                                                                    <ext:ListItem Value="Documentos" Text="<%$ Resources:Comun, jsDocumentoTipo %>"></ext:ListItem>
                                                                    <ext:ListItem Value="Emplazamientos" Text="<%$ Resources:Comun, jsEmplazamientoAtributos %>" />
                                                                    <ext:ListItem Value="Inventario" Text="<%$ Resources:Comun, jsInventarioElemento %>" />
                                                                    <ext:ListItem Value="Accesos" Text="<%$ Resources:Comun, jsGruposAccesosWeb %>" />
                                                                </Items>
                                                                <Listeners>
                                                                    <Select Handler="refrescarPermisos()" />
                                                                    <TriggerClick Fn="limpiarComboPermisos" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear" />
                                                                </Triggers>
                                                            </ext:ComboBox>

                                                            <ext:TextField
                                                                ID="txtBusquedaPermisos"
                                                                Cls="comboPermisos"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                EnableKeyEvents="true"
                                                                Flex="1">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <KeyUp Fn="FiltrarPermisos" Buffer="250" />
                                                                    <TriggerClick Fn="LimpiarFiltroPermisos" />
                                                                </Listeners>
                                                            </ext:TextField>

                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container runat="server">
                                                        <Items>

                                                            <ext:DataView runat="server" ID="gridPermisos" Cls="gridPermisos" EmptyText="No items to display" ItemSelector="div.group-header" StoreID="storePermisosRoles">
                                                                <Tpl runat="server">
                                                                    <Html>
                                                                        <div id="items-ct" class="dvPermisos">
                                                                        </div>
                                                                    </Html>
                                                                </Tpl>

                                                                <Listeners>
                                                                    <%--<Render Fn="CrearGrid" />--%>
                                                                </Listeners>
                                                            </ext:DataView>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                            </ext:Panel>
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
