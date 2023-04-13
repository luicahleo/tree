<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentalTiposDocumentos.aspx.cs" Inherits="TreeCore.ModDocumental.pages.DocumentalTiposDocumentos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>

</head>
<body>

    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdTipoDocSeleccionado" runat="server" />
            <ext:Hidden ID="hdCarpetaID" runat="server" />
            <ext:Hidden ID="hdAnadirCarpeta" runat="server" />
            <ext:Hidden ID="hdEsCarpeta" runat="server" />
            <ext:Hidden ID="hdEditarTipoDocSeleccionado" runat="server" />
            <ext:Hidden ID="hdDocumentoPerfilID" runat="server" />
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>
            <%--INICIO STORES--%>
            <ext:Store ID="StoreRoles" runat="server" AutoLoad="false" OnReadData="StoreRoles_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoTipoRoleID">
                        <Fields>
                            <ext:ModelField Name="DocumentoTipoRoleID" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="PermisoLectura" />
                            <ext:ModelField Name="PermisoEscritura" />
                            <ext:ModelField Name="PermisoDescarga" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store ID="StoreExtensiones" runat="server" AutoLoad="false" OnReadData="StoreExtensiones_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoExtensionID">
                        <Fields>
                            <ext:ModelField Name="DocumentoExtensionID" />
                            <ext:ModelField Name="Extension" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Extension" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="StoreGridExtensiones" runat="server" AutoLoad="false" OnReadData="StoreGridExtensiones_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Extension">
                        <Fields>
                            <ext:ModelField Name="Extension" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="StoreOnlyExtensiones" runat="server" AutoLoad="false" OnReadData="StoreOnlyExtensiones_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoExtensionID">
                        <Fields>
                            <ext:ModelField Name="DocumentoExtensionID" />
                            <ext:ModelField Name="Extension" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Extension" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store ID="StoreOnlyRoles" runat="server" AutoLoad="false" OnReadData="StoreOnlyRoles_Refresh" RemoteSort="false" shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="PerfilID">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="PerfilID">
                        <Fields>
                            <ext:ModelField Name="PerfilID" />
                            <ext:ModelField Name="Perfil_esES" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Perfil_esES" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="StoreRolesLibres" runat="server" AutoLoad="false" OnReadData="StoreRolesLibres_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Perfil_esES" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <%--FIN STORES--%>
            <ext:Window ID="winNewFolder"
                runat="server"
                Title="<%$ Resources:Comun, strNuevaCarpeta %>"
                Height="190"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout"
                Closable="true"
                CloseToolText="">
                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar10" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnAnadirCarpeta"
                                Cls="btn-ppal "
                                Text="<%$ Resources:Comun, strAñadirCarpeta %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false"
                                Handler="addNewFolder()">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formCarpeta"
                        Cls="formGestion-resp formGris formGestion"
                        WidthSpec="90%"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                MarginSpec="10 0 0 0"
                                ID="txtNombreCarpeta"
                                LabelAlign="Top"
                                AllowBlank="false"
                                EmptyText="<%$ Resources:Comun, strNombreCarpeta %>"
                                FieldLabel="<%$ Resources:Comun, strNombreCarpeta %>"
                                WidthSpec="90%">
                            </ext:TextField>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <ext:Window ID="winGestionDocumento"
                runat="server"
                Title="<%$ Resources:Comun, strNuevoTipoDocumento %>"
                Height="700"
                Width="480"
                BodyCls=""
                Cls="winForm-respSimple tbGrey"
                Scrollable="Vertical"
                Hidden="true"
                Padding="12"
                Layout="FitLayout"
                CloseToolText="">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="TbNavegacionTabs" Dock="Top" Cls="tbGrey" Hidden="false" MinHeight="36">
                        <Items>

                            <ext:HyperlinkButton runat="server"
                                ID="lnkExtensions"
                                Cls="lnk-navView lnk-noLine navActivo"
                                Text="<%$ Resources:Comun, strExtensiones %>">
                                <Listeners>
                                    <Click Handler="NavegacionTabs(this)"></Click>
                                </Listeners>
                            </ext:HyperlinkButton>
                            <ext:HyperlinkButton runat="server"
                                ID="lnkPermissions"
                                Cls="lnk-navView lnk-noLine "
                                Text="<%$ Resources:Comun, strPermisos %>">
                                <Listeners>
                                    <Click Handler="NavegacionTabs(this)"></Click>
                                </Listeners>
                            </ext:HyperlinkButton>
                        </Items>
                    </ext:Toolbar>
                    <ext:Toolbar runat="server" ID="Toolbar5" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="btnPrev" Cls="btn-secondary " MinWidth="110" Text="<%$ Resources:Comun, strAnterior %>" Focusable="false" PressedCls="none" Hidden="false">
                                <Listeners>
                                    <Click Fn="btnPrev_Click" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button
                                runat="server"
                                ID="btnNext"
                                Cls="btn-ppal "
                                Text="<%$ Resources:Comun, strSiguiente %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="btnNext_Click" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnAdd" Cls="btn-ppal " Text="<%$ Resources:Comun, strGuardar %>" Focusable="false" PressedCls="none" Disabled="true" Hidden="true">
                                <Listeners>
                                    <Click Handler="winGestionAgregarEditarDocumento();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionDocumentos"
                        Cls="formGestion-resp formGris formGestion"
                        Border="false">
                        <Items>
                            <ext:Container runat="server" ID="cntWinExtensions" Layout="VBoxLayout" Padding="0" Hidden="false" Flex="1">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <Items>

                                    <ext:TextField runat="server" ID="txtNombreTipoDocumento" LabelAlign="Top" AllowBlank="false" EmptyText="<%$ Resources:Comun, strNombreTipo %>" FieldLabel="<%$ Resources:Comun, strNombreTipo %>" WidthSpec="90%">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoDocumentos" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:Label runat="server" meta:resourceKey="strExtensiones" Text="<%$ Resources:Comun, strExtensiones%>" MarginSpec="0 0 0 10" Cls="x-form-item-label-default" />
                                    <ext:Container runat="server" ID="ctExtensionEntireForm" Cls="WhiteFill ContainerSimpleBorder pdng-top" MarginSpec="0 10 10 10" Flex="1">
                                        <Items>
                                            <ext:GridPanel
                                                ID="winGestionDocumentosExtensiones"
                                                runat="server"
                                                StoreID="StoreGridExtensiones"
                                                Cls="gridPanel"
                                                Height="500"
                                                Scroll="Vertical">
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strExtensiones%>"
                                                            Width="160"
                                                            DataIndex="Extension"
                                                            Resizable="false"
                                                            MenuDisabled="true"
                                                            Flex="1" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                                                </SelectionModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>

                            <ext:Container runat="server" ID="ctnWinPermissions" Layout="VBoxLayout" Padding="0" Hidden="true">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <Items>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoLectura"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strLectura %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="50 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisos" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoDescarga"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strDescarga %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="5 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisos" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoEscritura"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strEscritura %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="5 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisos" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <ext:Window ID="winAddRoles"
                runat="server"
                Title="<%$ Resources:Comun, strRoles %>"
                Height="700"
                Width="480"
                BodyCls=""
                Cls=" tbGrey"
                Scrollable="Vertical"
                Hidden="true"
                Padding="12"
                Layout="FitLayout"
                CloseToolText="">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="TbNavegacionTabsOP" Dock="Top" Cls="tbGrey" Hidden="false" MinHeight="36">
                        <Items>
                            <ext:HyperlinkButton runat="server"
                                ID="lnkProfilesOP"
                                Cls="lnk-navView lnk-noLine navActivo "
                                Text="<%$ Resources:Comun, strRoles %>">
                                <Listeners>
                                    <Click Handler="NavegacionTabsOnlyProf(this)"></Click>
                                </Listeners>
                            </ext:HyperlinkButton>
                            <ext:HyperlinkButton runat="server"
                                ID="lnkPermissionsOP"
                                Cls="lnk-navView lnk-noLine "
                                Text="<%$ Resources:Comun, strPermisos %>">
                                <Listeners>
                                    <Click Handler="NavegacionTabsOnlyProf(this)"></Click>
                                </Listeners>
                            </ext:HyperlinkButton>
                        </Items>
                    </ext:Toolbar>
                    <ext:Toolbar runat="server" ID="Toolbar8" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="btnPrevProfiles" Cls="btn-secondary " MinWidth="110" Text="<%$ Resources:Comun, strAnterior %>" Focusable="false" PressedCls="none" Hidden="false">
                                <Listeners>
                                    <Click Fn="btnPrevProfiles_Click" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnNextProfiles" Cls="btn-ppal " Text="<%$ Resources:Comun, strSiguiente %>" Focusable="false" PressedCls="none" Hidden="false">
                                <Listeners>
                                    <Click Fn="btnNextProfiles_Click" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button
                                runat="server"
                                ID="btnAddProfiles"
                                Cls="btn-ppal "
                                Text="<%$ Resources:Comun, strAnadir %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="true">
                                <Listeners>
                                    <Click Fn="btnNextProfiles_Click" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionRoles"
                        Cls="formGestion-resp formGris formGestion"
                        Border="false">
                        <Items>
                            <ext:Container runat="server" ID="ctnOPProfiles" Layout="VBoxLayout" Padding="0" Hidden="false">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <Items>
                                    <ext:Container runat="server" ID="ctProfileForm" Cls="WhiteFill ContainerSimpleBorder pdng-top" MarginSpec="0 10 10 10" Flex="1" Scrollable="Vertical">
                                        <Items>
                                            <ext:GridPanel
                                                ID="GridOnlyRoles"
                                                runat="server"
                                                Cls="gridPanel"
                                                Height="500"
                                                StoreID="StoreRolesLibres">
                                                <DockedItems>
                                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                        <Content>

                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="StoreRolesLibres"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                Grid="GridOnlyRoles"
                                                                MostrarBusqueda="false"
                                                                QuitarFiltros="true" />

                                                        </Content>
                                                    </ext:Container>
                                                </DockedItems>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strRoles%>"
                                                            Width="160"
                                                            DataIndex="Nombre"
                                                            Resizable="false"
                                                            MenuDisabled="true"
                                                            Flex="1" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                                                </SelectionModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server" ID="ctnOPPermissions" Layout="VBoxLayout" Padding="0" Hidden="true">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <Items>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoLecturaRoles"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strLectura %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="50 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisosRoles" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoEscrituraRoles"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strEscritura %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="5 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisosRoles" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox
                                        runat="server"
                                        ID="chkPermisoDescargaRoles"
                                        WidthSpec="95%"
                                        LabelAlign="Right"
                                        BoxLabel="<%$ Resources:Comun, strDescarga %>"
                                        Cls="chkboxLabelWin"
                                        MarginSpec="5 10 5 10">
                                        <Listeners>
                                            <Change Fn="changeChkPermisosRoles" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <ext:Window ID="winOnlyExtensions"
                runat="server"
                Title="<%$ Resources:Comun, strExtensiones%>"
                Height="700"
                Width="480"
                BodyCls=""
                Cls="winForm-respSimple tbGrey"
                Scrollable="Vertical"
                Hidden="true"
                Padding="12"
                Layout="FitLayout"
                CloseToolText="">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar6" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="btnAddOnlyExtensiones" Cls="btn-ppal " Text="<%$ Resources:Comun, strAnadir%>" Focusable="false" PressedCls="none" Hidden="false" Handler="addOnlyExtensiones"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>

                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionExtensiones"
                        Cls="formGestion-resp formGris formGestion"
                        Border="false">
                        <Items>
                            <ext:Container runat="server" Layout="VBoxLayout" Padding="0" Cls="WhiteFill ContainerSimpleBorder pdng-top" Hidden="false" Flex="1" Scrollable="Vertical">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <Items>
                                    <ext:GridPanel
                                        ID="GridOnlyExtensiones"
                                        runat="server"
                                        Height="550"
                                        StoreID="StoreOnlyExtensiones">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column
                                                    runat="server"
                                                    Text="<%$ Resources:Comun, strExtensiones%>"
                                                    Width="160"
                                                    DataIndex="Extension"
                                                    Resizable="false"
                                                    MenuDisabled="true"
                                                    Flex="1" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                                        </SelectionModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
            <%--FIN  WINDOWS --%>
            <ext:Viewport runat="server" ID="MainVwP" Layout="FitLayout">
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
                        <Items>
                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>
                            <%-- PANEL CENTRAL CON SLIDERS--%>
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
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" MinHeight="60" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strTiposDocumentos %>" Height="25" />

                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>

                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="false" Layout="HBoxLayout" Flex="1">
                                        <Items>


                                            <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey tbNoborder PosUnsFloatR">
                                                <Items>

                                                    <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="SliderMove('Prev');" Disabled="true"></ext:Button>
                                                    <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next-w" Cls="SliderBtn" Handler="SliderMove('Next');" Disabled="false"></ext:Button>

                                                </Items>
                                            </ext:Toolbar>



                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>

                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey">

                                        <Listeners>
                                            <AfterRender Handler="ControlSlider(this)"></AfterRender>
                                            <Resize Handler="ControlSlider(this)"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:TreePanel
                                                HideHeaders="true"
                                                Hidden="false"
                                                Flex="3"
                                                runat="server"
                                                Cls="gridPanel  TreePnl"
                                                Title="<%$ Resources:Comun, strTiposDocumentos%>"
                                                ID="TreePanelV1"
                                                RootVisible="true"
                                                AutoScroll="true" >
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir-subM "
                                                                AriaLabel="<%$ Resources:Comun, strAnadir%>"
                                                                ToolTip="<%$ Resources:Comun, strAnadir %>"
                                                                Width="55">
                                                                <Menu>
                                                                    <ext:Menu runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem
                                                                                meta:resourceKey="mnuDwldIFRS16"
                                                                                ID="AddFolderMainPanel"
                                                                                runat="server"
                                                                                Text="<%$ Resources:Comun, strAñadirCarpeta%>"
                                                                                IconCls="btnfolder16"
                                                                                Handler="showNewFolder" />
                                                                            <ext:MenuItem
                                                                                meta:resourceKey="mnuDwldModeloReg"
                                                                                ID="AddDocuMainPanel"
                                                                                runat="server"
                                                                                Text="<%$ Resources:Comun, strAñadirDocumento%>"
                                                                                IconCls="btndocs16"                                                                             
                                                                                Handler="agregarEditarDocumento()" />
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEditarDocumento" Cls="btnEditar" AriaLabel="<%$ Resources:Comun, jsEditar%>" ToolTip="<%$ Resources:Comun, jsEditar%>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="MostrarEditarDocumento" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEliminarDocumento" Cls="btnEliminar" AriaLabel="<%$ Resources:Comun, jsEliminar%>" ToolTip="<%$ Resources:Comun, jsEliminar%>" Disabled="true" Handler="eliminarTipoDoc"></ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescarDocumento" Cls="btnRefrescar" AriaLabel="<%$ Resources:Comun, jsRefrescar%>" ToolTip="<%$ Resources:Comun, jsRefrescar%>" Handler="refreshTreePanel();"></ext:Button>
                                                            <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="<%$ Resources:Comun, jsDescargar%>" ToolTip="<%$ Resources:Comun, jsDescargar%>" Handler="" Hidden="true"></ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                Cls="btnActivar"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, jsActivar %>"
                                                                Handler="Activar();">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lblToggle"
                                                                Cls="lblBtnActivo"
                                                                PaddingSpec="0 0 25 0"
                                                                Text="<%$ Resources:Comun, strActivo %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnActivo"
                                                                Width="41"
                                                                Pressed="true"
                                                                EnableToggle="true"
                                                                TextAlign="Left"
                                                                Cls="btn-toggleGrid"
                                                                AriaLabel="<%$ Resources:Comun, strEmplazamientosCliente %>"
                                                                ToolTip="<%$ Resources:Comun, strActivo %>"
                                                                Handler="refreshTreePanel();" />
                                                            <ext:ToolbarFill />                                                            
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server" ID="Toolbar2" Cls="tlbGrid" Layout="HBoxLayout" Dock="Top">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="TextFilter"
                                                                Cls=""
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar%>"
                                                                LabelWidth="50"
                                                                Flex="1"
                                                                EnableKeyEvents="true">
                                                                <Listeners>
                                                                    <KeyUp Fn="filterTree" Buffer="250" />
                                                                    <TriggerClick Handler="LimpiarFiltroTreePanel()" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear" />
                                                                </Triggers>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TreeColumn
                                                            ID="ColNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strElements%>"
                                                            Flex="3"
                                                            Hidden="false"
                                                            DataIndex="DocumentTipo" />
                                                    </Columns>
                                                </ColumnModel>
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
                                                <SelectionModel>
                                                    <ext:TreeSelectionModel runat="server"
                                                        ID="GridRowSelect"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                        </Listeners>
                                                    </ext:TreeSelectionModel>
                                                </SelectionModel>
                                            </ext:TreePanel>

                                            <ext:Panel runat="server" ID="pnCol1" Flex="5" Layout="VBoxLayout" BodyCls="tbGrey" MarginSpec="0 0 0 10">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:GridPanel
                                                        Hidden="false"
                                                        Title="<%$ Resources:Comun, strRoles %>"
                                                        runat="server"
                                                        MaxWidth="350"
                                                        SelectionMemory="false"
                                                        MinWidth="350"
                                                        MarginSpec="0 0 5 0"
                                                        Flex="1"
                                                        ID="GridP2Bot"
                                                        Cls="gridPanel"
                                                        Region="Center"
                                                        Scrollable="Vertical"
                                                        StoreID="StoreRoles"
                                                        MultiSelect="false">
                                                        <DockedItems>
                                                            <ext:Toolbar
                                                                runat="server"
                                                                ID="Toolbar3"
                                                                Dock="Top"
                                                                Cls="tlbGrid c-Grid">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnAnadirRoles"
                                                                        Cls="btnAnadir"
                                                                        AriaLabel="<%$ Resources:Comun, strAnadir%>"
                                                                        Handler="showOnlyRoles"
                                                                        Disabled="true"
                                                                        ToolTip="<%$ Resources:Comun, strAnadir %>">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server" ID="btnEliminarRoles" Cls="btnEliminar" AriaLabel="<%$ Resources:Comun, jsEliminar%>" Disabled="true" ToolTip="<%$ Resources:Comun, jsEliminar%>" Handler="deleteProfiles"></ext:Button>
                                                                    <ext:Button runat="server" ID="btnRefrescarRoles" Cls="btnRefrescar" AriaLabel="<%$ Resources:Comun, jsRefrescar%>" ToolTip="<%$ Resources:Comun, jsRefrescar%>" Handler="App.StoreRoles.reload();"></ext:Button>
                                                                    <ext:ToolbarFill />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" DataIndex="Nombre" Flex="2" ID="Column2"></ext:Column>
                                                                <ext:Column runat="server"
                                                                    Flex="2"
                                                                    Text="<%$ Resources:Comun, strLectura %>"
                                                                    ID="colLectura"
                                                                    DataIndex="PermisoLectura"
                                                                    Align="Center"
                                                                    meta:resourceKey="colLectura">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colEscritura"
                                                                    Text="<%$ Resources:Comun, strEscritura %>"
                                                                    Flex="2"
                                                                    DataIndex="PermisoEscritura"
                                                                    Align="Center"
                                                                    meta:resourceKey="colEscritura">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colDescarga"
                                                                    Text="<%$ Resources:Comun, strDescarga %>"
                                                                    Flex="2"
                                                                    DataIndex="PermisoDescarga"
                                                                    Align="Center"
                                                                    meta:resourceKey="colDescarga">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelectRoles" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                    </ext:GridPanel>


                                                    <ext:GridPanel
                                                        Hidden="false"
                                                        Title="<%$ Resources:Comun, strExtensiones%>"
                                                        runat="server"
                                                        MaxWidth="350"
                                                        MinWidth="350"
                                                        Flex="1"
                                                        ID="GridP3Top"
                                                        Cls="gridPanel"
                                                        SelectionMemory="false"
                                                        Region="Center"
                                                        Scrollable="Vertical"
                                                        StoreID="StoreExtensiones"
                                                        MultiSelect="false">
                                                        <DockedItems>
                                                            <ext:Toolbar
                                                                runat="server"
                                                                ID="Toolbar4"
                                                                Dock="Top"
                                                                Cls="tlbGrid c-Grid">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnAnadirExtension"
                                                                        Cls="btnAnadir"
                                                                        AriaLabel="<%$ Resources:Comun, strAnadir%>"
                                                                        Disabled="true"
                                                                        ToolTip="<%$ Resources:Comun, strAnadir %>"
                                                                        Handler="showOnlyExtensiones">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server" ID="btnEliminarExtension" Cls="btnEliminar" AriaLabel="<%$ Resources:Comun, jsEliminar%>" Disabled="true" ToolTip="<%$ Resources:Comun, jsEliminar%>" Handler="deleteExtensions"></ext:Button>
                                                                    <ext:Button runat="server" ID="btnRefrescarExtension" Cls="btnRefrescar" AriaLabel="<%$ Resources:Comun, jsRefrescar%>" ToolTip="<%$ Resources:Comun, jsRefrescar%>" Handler="App.StoreExtensiones.reload();"></ext:Button>
                                                                    <ext:ToolbarFill />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre%>" DataIndex="Extension" Width="200" Flex="1" ID="Column1">
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server" />
                                                        </SelectionModel>
                                                        <Listeners>
                                                            <ItemClick Fn="showRemoveExtensions" />
                                                        </Listeners>
                                                    </ext:GridPanel>
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
