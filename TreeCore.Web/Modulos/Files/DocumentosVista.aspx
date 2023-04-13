<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="DocumentosVista.aspx.cs" Inherits="TreeCore.PaginasComunes.pages.DocumentosVista" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title />
</head>
<body>
    <script src="/JS/dropzone.js"></script>
    <link href="/CSS/dropzone.css" rel="stylesheet" />
    <div id="toolTipdoDocumento" class="tooltipwhite"></div>
    <style>
        .x-menu-default {
            border-style: none;
        }
    </style>

    <form id="formDocumentosVista" runat="server">
        <div draggable="true" ondragover="allowDrop(event)">
            <ext:Hidden ID="hdObjetoID" runat="server" />
            <ext:Hidden ID="hdObjetoTipo" runat="server" />
            <ext:Hidden ID="hdClienteID" runat="server" />
            <ext:Hidden ID="hdDocumentoID" runat="server" />
            <ext:Hidden ID="hdTipoDocumentoID" runat="server" />
            <ext:Hidden ID="hdTipoDocumentoNombre" runat="server" />
            <ext:Hidden ID="hdVisor" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <ext:Hidden ID="hdModuloID" runat="server" />
            <ext:Hidden ID="hdCulture" runat="server" />
            <ext:Hidden ID="hdEditadoObjetoID" runat="server" />
            <ext:Hidden ID="hdMaxRequestLength" runat="server" />
            <ext:Hidden ID="hdTamanoMaximoDescarga" runat="server" />
            <ext:Hidden ID="hdCarpetaPadreID" runat="server" />
            <ext:Hidden ID="hdCarpetaActual" runat="server" />
            <ext:Hidden ID="hdDocumentosIDsCopiados" runat="server" />
            <ext:Hidden ID="hdFiltDocumentTipoIDs" runat="server" />
            <ext:Hidden ID="hdFiltExtensiones" runat="server" />
            <ext:Hidden ID="hdFiltEstadosIDs" runat="server" />
            <ext:Hidden ID="hdFiltCreadoresIDs" runat="server" />
            <ext:Hidden ID="hdIdDocumentBuscador" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--INICIO STORES--%>
            <ext:Store runat="server"
                ID="storeClientes"
                AutoLoad="true"
                OnReadData="storeClientes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
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
                RemotePaging="true"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true"
                RemoteFilter="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                    <DataChanged Fn="sotorePrincipalDataChanged" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="id">
                        <Fields>
                            <ext:ModelField Name="Icono" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="ObjetoTipo" />
                            <ext:ModelField Name="Extension" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="Proyecto" />
                            <ext:ModelField Name="Creador" />
                            <ext:ModelField Name="Fecha" Type="Date" />
                            <ext:ModelField Name="Estado" />
                            <ext:ModelField Name="EstadoID" />
                            <ext:ModelField Name="Version" />
                            <ext:ModelField Name="DocumentoPadreID" />
                            <ext:ModelField Name="EmplazamientoID" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Tamano" />
                            <ext:ModelField Name="EsDocumento" />
                            <ext:ModelField Name="tipoDocumento" />
                            <ext:ModelField Name="slug" />
                            <ext:ModelField Name="DocumentoTipoID" />
                            <ext:ModelField Name="DocumentoTipo" />
                            <ext:ModelField Name="EsCarpeta" />
                            <ext:ModelField Name="Lectura" />
                            <ext:ModelField Name="Escritura" />
                            <ext:ModelField Name="Descarga" />
                            <ext:ModelField Name="NombreObjeto" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeTiposDocumentos"
                AutoLoad="false"
                OnReadData="storeTiposDocumentos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentTipoID">
                        <Fields>
                            <ext:ModelField Name="DocumentTipoID" />
                            <ext:ModelField Name="DocumentTipo" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                ID="storeDocumentoLateral"
                runat="server"
                AutoLoad="false"
                Buffered="true"
                RemoteFilter="true"
                LeadingBufferZone="1000"
                PageSize="50"
                OnReadData="storeDocumentoLateral_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoID">
                        <Fields>
                            <ext:ModelField Name="DocumentoID" Type="Int" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="ObjetoTipo" />
                            <ext:ModelField Name="Fecha" Type="Date" />
                            <ext:ModelField Name="Proyecto" />
                            <ext:ModelField Name="Creador" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="Estado" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeVersiones"
                runat="server"
                AutoLoad="false"
                RemoteFilter="true"
                RemoteSort="true"
                PageSize="50"
                OnReadData="storeVersiones_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoID">
                        <Fields>
                            <ext:ModelField Name="DocumentoID" Type="Int" />
                            <ext:ModelField Name="Documento" />
                            <ext:ModelField Name="Fecha" />
                            <ext:ModelField Name="Creador" Type="String" />
                            <ext:ModelField Name="Version" Type="Int" />
                            <ext:ModelField Name="UltimaVersion" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Tamano" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Version" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeDocumentosEstados"
                runat="server"
                AutoLoad="false"
                RemoteFilter="true"
                RemoteSort="true"
                OnReadData="storeDocumentosEstados_Refresh">
                <Model>
                    <ext:Model runat="server" IDProperty="DocumentoEstadoID">
                        <Fields>
                            <ext:ModelField Name="DocumentoEstadoID" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="Defecto" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN STORES--%>
            <ext:Menu runat="server"
                ID="ContextMenuTreeL">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-Visible-ctxMnu"
                        Text="<%$ Resources:Comun, strPrevisualizar %>"
                        ID="Preview"
                        Hidden="false">
                        <Listeners>
                            <Click Fn="menuPrevisualizar" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="btnDescargarCtxMenu"
                        Text="<%$ Resources:Comun, jsDescargar %>"
                        ID="Download"
                        Hidden="false">
                        <Listeners>
                            <Click Fn="menuDescargar" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-Documents-ctxMnu"
                        Text="<%$ Resources:Comun, strVersionesPrevias %>"
                        Hidden="true"
                        ID="Versions">
                        <Listeners>
                            <Click Fn="menuVersiones" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-cut-ctxMnu"
                        Text="<%$ Resources:Comun, strCortar %>"
                        Hidden="false"
                        ID="CutFile">
                        <Listeners>
                            <Click Fn="menuCortar" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-paste-ctxMnu"
                        Text="<%$ Resources:Comun, strPegarComo %>"
                        Disabled="false"
                        Hidden="true"
                        ID="PasteFile">
                        <Menu>
                            <ext:Menu
                                runat="server"
                                Hidden="false"
                                ID="menuPegarComo">
                                <Items>
                                    <ext:MenuItem
                                        runat="server"
                                        IconCls="x-loading-indicator"
                                        Text="<%$ Resources:Comun, jsCargando %>"
                                        Focusable="false"
                                        HideOnClick="false" />
                                </Items>
                                <Listeners>
                                    <Click Fn="pegarDocumentoComo" />
                                </Listeners>
                                <Loader
                                    runat="server"
                                    Mode="Component"
                                    DirectMethod="#{DirectMethods}.LoaderMenuPegarComo"
                                    RemoveAll="true">
                                </Loader>
                            </ext:Menu>
                        </Menu>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-trash-ctxMnu"
                        Text="<%$ Resources:Comun, strEliminar %>"
                        ID="ShowContracts"
                        Hidden="true" />
                    <ext:MenuItem runat="server"
                        ID="changeFileName"
                        Text="<%$ Resources:Comun, strCambiarMetadatos %>"
                        IconCls="ico-editar-menu"
                        Hidden="false">
                        <Listeners>
                            <Click Fn="changeMetadataDoc" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        IconCls="ico-paste-ctxMnu"
                        Text="<%$ Resources:Comun, strCambiarEstadoA %>"
                        Disabled="false"
                        Hidden="true"
                        ID="mnCambiarEstadoMulti">
                        <Menu>
                            <ext:Menu
                                runat="server"
                                Hidden="false"
                                ID="menuCambiarEstadoDoc">
                                <Items>
                                    <ext:MenuItem
                                        runat="server"
                                        IconCls="x-loading-indicator"
                                        Text="<%$ Resources:Comun, jsCargando %>"
                                        Focusable="false"
                                        HideOnClick="false" />
                                </Items>
                                <Listeners>
                                    <Click Fn="cambiarEstadoDocumento" />
                                </Listeners>
                                <Loader
                                    runat="server"
                                    Mode="Component"
                                    DirectMethod="#{DirectMethods}.LoaderMenuCambiarEstado"
                                    RemoveAll="true">
                                </Loader>
                            </ext:Menu>
                        </Menu>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        ID="btnCompartir"
                        Text="<%$ Resources:Comun, strCompartir %>"
                        IconCls="ico-compartir"
                        Hidden="false">
                        <Listeners>
                            <Click Fn="shareDocument" />
                        </Listeners>
                    </ext:MenuItem>
                    <ext:MenuItem runat="server"
                        ID="btnHistorico"
                        Text="<%$ Resources:Comun, strHistorico %>"
                        IconCls="ico-historial-ctxMnu"
                        Hidden="false">
                        <Listeners>
                            <Click Fn="historyDocument" />
                        </Listeners>
                    </ext:MenuItem>
                </Items>
                <Listeners>
                    <%--  <Click Fn="OpcionSeleccionada" />--%>
                    <BeforeShow Fn="beforeShowContextMenu" />
                </Listeners>
            </ext:Menu>

            <ext:Window ID="winAddDocumentUpload"
                runat="server"
                Title="<%$ Resources:Comun, strAgregarDocumento %>"
                Width="572"
                Height="300"
                Centered="true"
                Cls="winAddDocumentUpload"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar
                        runat="server"
                        ID="tbWinProgress"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 30 18 40">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button
                                runat="server"
                                ID="btnAceptarSubida"
                                Cls="btn-ppal btnBold "
                                MinWidth="120" Text=""
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Fn="btnAceptarSubida" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:Label
                        IconAlign="Left"
                        IconCls="btnSubir"
                        ID="ProgresoLabel"
                        runat="server"
                        Text="1/2 Uploading Files"
                        MarginSpec="20 10 20 0"
                        Hidden="true">
                    </ext:Label>
                    <ext:ProgressBar
                        ID="pBarDocs"
                        runat="server"
                        Value=".34"
                        Width="500"
                        MinHeight="20"
                        MarginSpec="0 40 0 35"
                        Hidden="true"
                        Text="">
                    </ext:ProgressBar>
                    <ext:Label
                        IconAlign="Left"
                        IconCls="btnSubir"
                        ID="ExitoLabel"
                        Cls="ExitoLbl"
                        runat="server"
                        Text="Success!"
                        MarginSpec="20 10 20 30"
                        Hidden="false">
                    </ext:Label>
                    <ext:Panel
                        runat="server"
                        ID="pnProgresFiles">
                    </ext:Panel>
                </Items>
                <Listeners>
                    <Close Fn="btnAceptarSubida" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winUploadFieldDocument"
                runat="server"
                Title="<%$ Resources:Comun, strAgregarDocumento %>"
                Width="672"
                OverflowY="Hidden"
                OverflowX="Hidden"
                Centered="true"
                Cls="winForm-resp"
                Layout=""
                Hidden="true"
                meta:resourceKey="winUploadFieldDocument">

                <Content>
                    <%--<ext:Image runat="server" ID="PlaceholderIMGUPLOAD" ImageUrl="../ima/img-multipleupload.JPG" MaxHeight="287"></ext:Image>--%>
                    <%----%>
                    <ext:Container runat="server" ID="ctUploadFieldDocument" Cls="winUploadFieldDocument-panel ctForm-resp ctForm-resp-col2">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbTiposDocumentos"
                                StoreID="storeTiposDocumentos"
                                DisplayField="DocumentTipo"
                                ValueField="DocumentTipoID"
                                Cls=""
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, jsDocumentoTipo %>"
                                FieldLabel="<%$ Resources:Comun, jsDocumentoTipo %>"
                                AllowBlank="false">
                                <Listeners>
                                    <Select Fn="SeleccionarDocumentoTipo" />
                                    <TriggerClick Fn="RecargarDocumentoTipo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextArea
                                runat="server"
                                ID="txtDescripcionDocumento"
                                MaxLength="1000"
                                Cls="textAreaGridFinal txtDescripcionDocumento"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>">
                            </ext:TextArea>
                        </Items>
                    </ext:Container>
                    <ext:Panel
                        runat="server"
                        Cls="dz-preview dz-file-preview"
                        ID="paneEmpty"
                        Layout="AutoLayout"
                        Margin="15"
                        BodyCls="tbGrey"
                        Height="300">
                    </ext:Panel>

                    <ext:Action
                        runat="server"
                        ID="UploadFileDropzone"
                        Text="Subir Archivo"
                        Disabled="false"
                        Handler="SubirArchivo();" />
                    <ext:Panel
                        runat="server"
                        Cls="dropzone dz-preview dz-file-preview"
                        ID="panelCenterShow"
                        Layout="AutoLayout"
                        Margin="15"
                        BodyCls="tbGrey"
                        Height="300">
                        <LayoutConfig>
                        </LayoutConfig>
                        <Items>
                            <ext:Label
                                runat="server"
                                Text="<%$ Resources:Comun, strSoltarDocumentos %>"
                                DefaultAlign="center"
                                Cls="DropAreaText"
                                meta:resourceKet="lblDropzone" />
                        </Items>
                    </ext:Panel>
                </Content>
                <Listeners>
                    <Close Fn="HabilitarGrid" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winAddFolder"
                runat="server"
                Title="Add New Folder"
                Height="190"
                Width="380"
                Centered="true"
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
                            <ext:Button runat="server" ID="backNewUsers" Cls="btn-secondary " MinWidth="110" Text="Previous" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="nextNewUsers" Cls="btn-ppal " Text="Next" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="txtNewFolderName" LabelAlign="Top" FieldLabel="Folder Name" WidthSpec="90%"></ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window ID="winAddDocument"
                runat="server"
                Title="Add New Document"
                Height="480"
                Width="680"
                Resizable="true"
                Centered="true"
                BodyCls="testclass"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true"
                Layout=""
                Modal="true">
                <Listeners>
                    <Resize Handler="winFormResizeAddNewDocument(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar8" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="Button2" Cls="btn-secondary " MinWidth="110" Text="Previous" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="Button17" Cls="btn-ppal " Text="Next" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="TextF3ield3" MaxHeight="40" LabelAlign="Top" FieldLabel="Date"></ext:TextField>
                    <ext:TextField runat="server" ID="TextFielsd3" MaxHeight="40" LabelAlign="Top" FieldLabel="End Date"></ext:TextField>
                    <ext:TextField runat="server" ID="TextFideld3" MaxHeight="40" LabelAlign="Top" FieldLabel="Document"></ext:TextField>
                    <ext:TextArea runat="server" ID="TextField3" Cls="grid2rowArea" Height="140" LabelAlign="Top" FieldLabel="Note"></ext:TextArea>
                    <ext:TextField runat="server" ID="TextFisaeld3" MaxHeight="40" LabelAlign="Top" FieldLabel="File"></ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window
                ID="winChangeNameFile"
                runat="server"
                Title="<%$ Resources:Comun, strCambiarMetadatos %>"
                Width="450"
                Resizable="true"
                Centered="true"
                BodyCls="testclass"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true"
                Layout=""
                Modal="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formChangeNameFile"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                meta:resourceKey="txtNameFile"
                                ID="txtNameFile"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="FormChangeNameFileValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:ComboBox
                                runat="server"
                                ID="cmbDocumentosEstados"
                                FieldLabel="<%$ Resources:Comun, strEstado %>"
                                EmptyText="<%$ Resources:Comun, strEstado %>"
                                DisplayField="Nombre"
                                ValueField="DocumentoEstadoID"
                                StoreID="storeDocumentosEstados">
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextArea
                                runat="server"
                                ID="txtDescripcion"
                                MaxLength="1000"
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>">
                            </ext:TextArea>
                        </Items>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winChangeNameFile}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarNameFile"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winChangeNameFileBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window
                ID="winCompartirDocumento"
                runat="server"
                Title="<%$ Resources:Comun, strCompartir %>"
                Height="250"
                Width="600"
                Resizable="true"
                Centered="true"
                BodyCls="testclass"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true"
                Layout=""
                Modal="true">
                <Items>
                    <ext:Container runat="server"
                        ID="pnVistas"
                        MonitorPoll="500"
                        MonitorValid="true"
                        ActiveIndex="2"
                        Border="false"
                        Cls="pnNavVistas pnVistasForm"
                        Height="50"
                        AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server"
                                ID="cntNavVistasShare"
                                Cls="nav-vistas ctForm-tab-resp-col3"
                                ActiveIndex="1"
                                Height="50"
                                ActiveItem="1">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkShareShow"
                                        meta:resourceKey="lnkShareShow"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strDocumento %>">
                                        <Listeners>
                                            <Click Fn="showTabsShare" />
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkShareDownload"
                                        meta:resourceKey="lnkShareDownload"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strDescarga %>">
                                        <Listeners>
                                            <Click Fn="showTabsShare" />
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Container runat="server"
                        ID="formShareShow"
                        Hidden="false"
                        Scrollable="Vertical"
                        OverflowY="Auto"
                        PaddingSpec="0 24px"
                        Cls="winCompartirDocumento-paneles winGestion-panel">
                        <Items>
                            <ext:Label runat="server"
                                ID="lbElementName1"
                                Text="Document"
                                Cls="txtBold lblShare"
                                Visible="true">
                            </ext:Label>
                            <ext:TextField
                                meta:resourceKey="txtUrl"
                                ID="txtUrl"
                                Cls="urlcopy"
                                runat="server"
                                AllowBlank="false"
                                Editable="false"
                                Width="250">
                            </ext:TextField>
                        </Items>
                    </ext:Container>
                    <ext:Container runat="server"
                        ID="formShareDownload"
                        Hidden="true"
                        Scrollable="Vertical"
                        OverflowY="Auto"
                        PaddingSpec="0 24px"
                        Cls="winCompartirDocumento-paneles winGestion-panel">
                        <Items>
                            <ext:Label runat="server"
                                ID="lbElementName2"
                                Text="Document"
                                Cls="txtBold lblShare"
                                Visible="true">
                            </ext:Label>
                            <ext:TextField
                                meta:resourceKey="txtUrl"
                                ID="txtUrlDescarga"
                                Cls="urlcopy"
                                runat="server"
                                AllowBlank="false"
                                Editable="false"
                                Width="250">
                            </ext:TextField>
                        </Items>
                    </ext:Container>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCerrarWinCompartir"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winCompartirDocumento}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnCopiarUrlCompartir"
                        Text="<%$ Resources:Comun, strCopiar %>"
                        IconCls="ico-compartir"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winCompartirCopiarUrl();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--VIEWPORT--%>
            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>
                <Items>
                    <ext:Panel ID="pnComboGrdVisor"
                        runat="server"
                        Header="false"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                BodyPaddingSummary="0 16"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="tbSliders" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button runat="server"
                                                ID="btnPrevSldr"
                                                IconCls="ico-prev-w"
                                                Cls="btnMainSldr SliderBtn"
                                                Handler="moveCtSldr(this);"
                                                Disabled="true"
                                                Hidden="true" />
                                            <ext:Button runat="server"
                                                ID="btnNextSldr"
                                                IconCls="ico-next-w"
                                                Cls="SliderBtn"
                                                Handler="moveCtSldr(this);"
                                                Disabled="false"
                                                Hidden="true" />
                                            <ext:Button runat="server"
                                                ID="btnCollapseAsRClosed"
                                                Cls="btn-trans"
                                                Disabled="false"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="MostrarPanelLateral" />
                                                    <AfterRender Fn="AsRclosed" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="ctMain1"
                                        Flex="2"
                                        Layout="FitLayout"
                                        Cls="col colCt1 "
                                        PaddingSpec="8 0 20 8"
                                        Hidden="false">
                                        <Items>
                                            <ext:GridPanel
                                                runat="server"
                                                Hidden="false"
                                                ID="grid"
                                                meta:resourceKey="grid"
                                                SelectionMemory="false"
                                                Cls="gridPanel  TreePnl"
                                                StoreID="storePrincipal"
                                                Title="Documentos"
                                                OverflowX="Hidden"
                                                OverflowY="Auto"
                                                Header="false"
                                                EnableColumnHide="false"
                                                EnableColumnMove="false"
                                                EnableColumnResize="false"
                                                ForceFit="true"
                                                AriaRole="main"
                                                ContextMenuID="ContextMenuTreeL">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbClientes"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClientes"
                                                                StoreID="storeClientes"
                                                                DisplayField="Cliente"
                                                                ValueField="ClienteID"
                                                                Cls="comboGrid pos-boxGrid"
                                                                Hidden="true"
                                                                EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                                FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarCliente" />
                                                                    <TriggerClick Fn="RecargarClientes" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button
                                                                meta:resourceKey="btnAgregarDocumento"
                                                                ID="btnAgregarDocumento"
                                                                runat="server"
                                                                Disabled="false"
                                                                ToolTip="<%$ Resources:Comun, strAgregarDocumento %>"
                                                                Cls="btnAnadir">
                                                                <Listeners>
                                                                    <Click Fn="AgregarDocumento" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEditarDocumento"
                                                                Cls="btnEditar"
                                                                Disabled="true"
                                                                AriaLabel="Editar"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="editarDocumento();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDesactivar"
                                                                Cls="btnDesactivar"
                                                                Disabled="true"
                                                                AriaLabel="Desactivar"
                                                                ToolTip="<%$ Resources:Comun, jsDesactivar %>"
                                                                Handler="desactivarDocumento();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                AriaLabel="Refrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="refreshStorePrincipal();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Descargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="descargarDocumentos();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="Button13"
                                                                Cls="btn-trans btnFiltros"
                                                                AriaLabel="Ver Workflow"
                                                                Hidden="true"
                                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                                Handler="ShowWorkFlow();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnFiltros"
                                                                Cls="btnFiltros"
                                                                EnableToggle="true"
                                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                                Handler="hidePnQuickFilters();">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbTgDocumentsActivos"
                                                                Cls="lblBtnActivo"
                                                                PaddingSpec="0 0 25 0"
                                                                Text="<%$ Resources:Comun, strMostrarInactivos %>" />
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnTgDocumentsActivos"
                                                                Text=""
                                                                ToolTip="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                EnableToggle="true"
                                                                Pressed="false"
                                                                Focusable="false"
                                                                Cls="btn-toggleGrid"
                                                                Width="41"
                                                                AriaLabel="">
                                                                <Listeners>
                                                                    <Click Fn="btnTgDocumentsActivos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server"
                                                                ID="GridbtnvistaVisor"
                                                                Cls="btnvistaTree"
                                                                AriaLabel="Cambiar Vista Visor"
                                                                ToolTip="<%$ Resources:Comun, strCambiaModoVisor %>">
                                                                <Listeners>
                                                                    <Click Fn="VisorMode" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar2"
                                                        Cls="tlbGrid"
                                                        Layout="HBoxLayout"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="TextFilter"
                                                                Cls=""
                                                                runat="server"
                                                                MaxWidth="280"
                                                                EmptyText="<%$ Resources:Comun, strBuscar%>"
                                                                LabelWidth="50"
                                                                Flex="1"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                    <TriggerClick Fn="LimpiarFiltroBusqueda" />
                                                                    <KeyPress Fn="buscarTextoTextFilter" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbRuta"
                                                        Cls="tbGrey"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnBack"
                                                                IconCls="ico-prev"
                                                                ID="btnBack">
                                                                <Listeners>
                                                                    <Click Fn="VolverAtras" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Text="<%$ Resources:Comun, strRaiz %>"
                                                                Cls="tbNavPath btnBack"
                                                                ID="lbRutaEmplazamientoTipo">
                                                                <Listeners>
                                                                    <Click Fn="IrRutaRaiz" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btnRaizCarpeta">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                Hidden="true"
                                                                ID="btnMenuRuta"
                                                                Cls="noBorder btnMenuRuta btnBack"
                                                                IconCls="ico-nav-folder-gr-16">
                                                                <Menu>
                                                                    <ext:Menu
                                                                        runat="server"
                                                                        ID="menuRuta"
                                                                        Cls="menuRuta">
                                                                        <Items>
                                                                        </Items>
                                                                        <Listeners>
                                                                            <Click Fn="SeleccionarRuta" />
                                                                        </Listeners>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btnCarpetaActual">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbRutaCategoria"
                                                                Cls="rutaCategoria btnBack"
                                                                Height="32"
                                                                Hidden="true">
                                                            </ext:Label>
                                                            <ext:Button
                                                                runat="server"
                                                                Hidden="true"
                                                                ID="btnPadreCarpetaActucal"
                                                                Cls="noBorder btnMenuRuta btnBack"
                                                                IconCls="ico-link-vertical">
                                                                <Menu>
                                                                    <ext:Menu
                                                                        runat="server"
                                                                        ID="menuPadreCarpetaActual"
                                                                        Cls="menuRuta">
                                                                        <Items>
                                                                        </Items>
                                                                        <Listeners>
                                                                            <Click Fn="SeleccionarPadre" />
                                                                        </Listeners>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            ID="colIcono"
                                                            DataIndex="Icono"
                                                            Align="Center"
                                                            Sortable="false"
                                                            Width="30"
                                                            MinWidth="30">
                                                            <Renderer Fn="RenderIcono" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="ColNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            Flex="3"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="Nombre">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="ColDocumentTipo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strDocumentoTipo %>"
                                                            Flex="2"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="DocumentoTipo">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colDesactivado"
                                                            runat="server"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            Width="50"
                                                            Hidden="true">
                                                            <Renderer Fn="renderInactiveDocuments" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="ColExtension"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strExtension %>"
                                                            Flex="1"
                                                            Sortable="true"
                                                            DataIndex="Extension"
                                                            Hidden="false">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colEstado"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strEstado %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Estado">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colObjetoTipo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strObjetoTipo %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="ObjetoTipo">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colObjetoCodigo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCodigoObjeto %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Codigo">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colObjetoNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strNombreObjeto %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="NombreObjeto">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colCreador"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCreador %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Creador">
                                                        </ext:Column>
                                                        <ext:DateColumn runat="server"
                                                            ID="ColFecha"
                                                            Text="<%$ Resources:Comun, strFecha %>"
                                                            Flex="2"
                                                            DataIndex="Fecha"
                                                            Hidden="false"
                                                            Align="Center"
                                                            Format="dd/MM/yyyy">
                                                        </ext:DateColumn>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strVersion_ %>"
                                                            Flex="1"
                                                            DataIndex="Version"
                                                            Hidden="false"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strTamano %>"
                                                            Flex="2" 
                                                            ID="colTamano"
                                                            DataIndex="Tamano"
                                                            Hidden="false">
                                                            <Renderer Fn="rendererTamano" />
                                                        </ext:Column>
                                                        <ext:ComponentColumn ID="ColMore"
                                                            runat="server"
                                                            Width="105"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            MinWidth="70"
                                                            MaxWidth="70"
                                                            Sortable="false">
                                                            <Component>
                                                                <ext:Button runat="server"
                                                                    Width="90"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Fn="MostrarPanelLateral" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Component>
                                                            <Listeners>
                                                                <Bind Fn="HideBtnMore" />
                                                            </Listeners>
                                                        </ext:ComponentColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                </Plugins>
                                                <%--<SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectArbol"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Select Fn="CheckMultiSelect_Grid_RowSelectArbol" />
                                                            <Deselect Fn="CheckMultiSelect_Grid_RowSelectArbol" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>--%>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelect"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                    <%--<ext:CheckboxSelectionModel
                                                        runat="server"
                                                        ID="CheckboxSelectionModel1"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                            <Deselect Fn="DeseleccionarGrilla" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>--%>
                                                </SelectionModel>
                                                <Listeners>
                                                    <ItemMouseEnter Fn="tooltipPermisosTipoDocumento" />
                                                    <ItemMouseLeave Fn="tooltipPermisosTipoDocumentoLeave" />
                                                    <DoubleTap Fn="EntrarEnCarpeta" />
                                                    <AfterLayout Fn="afterArbolJerarquico" />
                                                </Listeners>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        ID="PagingToolBar"
                                                        meta:resourceKey="PagingToolBar"
                                                        StoreID="storePrincipal"
                                                        DisplayInfo="true"
                                                        HideRefresh="true">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                Width="80"
                                                                ID="cmbPagination">
                                                                <Items>
                                                                    <ext:ListItem Text="10" Value="10" />
                                                                    <ext:ListItem Text="20" Value="20" />
                                                                    <ext:ListItem Text="30" Value="30" />
                                                                    <ext:ListItem Text="40" Value="40" />
                                                                    <ext:ListItem Text="50" Value="50" />
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
                                            <%--<ext:TreePanel
                                                ForceFit="true"
                                                Hidden="false"
                                                runat="server"
                                                Cls="gridPanel  TreePnl"
                                                ID="TreePanelV1"
                                                UseArrows="false"
                                                RootVisible="false"
                                                MultiSelect="false"
                                                SingleExpand="false"
                                                FolderSort="false"
                                                EnableColumnHide="false"
                                                OverflowX="Auto"
                                                Scrollable="Vertical"
                                                ContextMenuID="ContextMenuTreeL"
                                                nameSearchBox="TextFilter"
                                                listNotPredictive="id,iconCls,text,children,DocumentoTipoID,EsCarpeta,EsDocumento,Lectura,Escritura,Descarga,parentId,index,depth,expanded,expandable,checked,leaf,cls,icon,glyph,root,isLast,isFirst,allowDrop,allowDrag,loaded,loading,href,hrefTarget,qtip,qtitle,qshowDelay,visible,dataPath,selected">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button
                                                                meta:resourceKey="btnAgregarDocumento"
                                                                ID="btnAgregarDocumento"
                                                                runat="server"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, strAgregarDocumento %>"
                                                                Cls="btnAnadir">
                                                                <Listeners>
                                                                    <Click Fn="AgregarDocumento" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEditarDocumento"
                                                                Cls="btnEditar"
                                                                Disabled="true"
                                                                AriaLabel="Editar"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="editarDocumento();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDesactivar"
                                                                Cls="btnDesactivar"
                                                                Disabled="true"
                                                                AriaLabel="Desactivar"
                                                                ToolTip="<%$ Resources:Comun, jsDesactivar %>"
                                                                Handler="desactivarDocumento();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                AriaLabel="Refrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Handler="refreshStorePrincipal();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Descargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="this.up('grid').print();">
                                                                <Listeners>
                                                                    <Click Handler="descargarDocumentos();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="Button13"
                                                                Cls="btn-trans btnFiltros"
                                                                AriaLabel="Ver Workflow"
                                                                Hidden="true"
                                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                                Handler="ShowWorkFlow();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnFiltros"
                                                                Cls="btnFiltros"
                                                                EnableToggle="true"
                                                                ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                                                Handler="hidePnQuickFilters();" >
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbTgDocumentsActivos"
                                                                Cls="lblBtnActivo"
                                                                PaddingSpec="0 0 25 0"
                                                                Text="<%$ Resources:Comun, strMostrarInactivos %>" />
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnTgDocumentsActivos"
                                                                Text=""
                                                                ToolTip="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                EnableToggle="true"
                                                                Pressed="false"
                                                                Focusable="false"
                                                                Cls="btn-toggleGrid"
                                                                Width="41"
                                                                AriaLabel="">
                                                                <Listeners>
                                                                    <Click Fn="btnTgDocumentsActivos" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server"
                                                                ID="GridbtnvistaVisor"
                                                                Cls="btnvistaTree"
                                                                AriaLabel="Cambiar Vista Visor"
                                                                ToolTip="<%$ Resources:Comun, strCambiaModoVisor %>">
                                                                <Listeners>
                                                                    <Click Fn="VisorMode" />
                                                                </Listeners>
                                                            </ext:Button>
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
                                                                EnableKeyEvents="true"
                                                                IdTreePanel="TreePanelV1">
                                                                <Listeners>
                                                                    <KeyUp Fn="filterTree" Buffer="250" />
                                                                </Listeners>
                                                            </ext:TextField>

                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Fields>
                                                    <ext:ModelField Name="text" />
                                                    <ext:ModelField Name="Extension" />
                                                    <ext:ModelField Name="Codigo" />
                                                    <ext:ModelField Name="ObjetoTipo" />
                                                    <ext:ModelField Name="Alias" />
                                                    <ext:ModelField Name="Proyecto" />
                                                    <ext:ModelField Name="Creador" />
                                                    <ext:ModelField Name="Fecha" Type="Date" DateFormat="d/m/yyyy" />
                                                    <ext:ModelField Name="Estado" />
                                                </Fields>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TreeColumn
                                                            ID="ColNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            Flex="3"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="text">
                                                        </ext:TreeColumn>
                                                        <ext:Column
                                                            ID="colDesactivado"
                                                            runat="server"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            Width="50"
                                                            Hidden="true">
                                                            <Renderer Fn="renderInactiveDocuments" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="ColExtension"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strExtension %>"
                                                            Flex="1"
                                                            Sortable="true"
                                                            DataIndex="Extension"
                                                            Hidden="false">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colEstado"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strEstado %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Estado">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colObjetoTipo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strObjetoTipo %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="ObjetoTipo">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colObjetoCodigo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Codigo">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colCreador"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCreador %>"
                                                            Flex="2"
                                                            Editable="true"
                                                            Hidden="false"
                                                            DataIndex="Creador">
                                                        </ext:Column>
                                                        <ext:DateColumn runat="server"
                                                            ID="ColFecha"
                                                            Text="<%$ Resources:Comun, strFecha %>"
                                                            Flex="2"
                                                            DataIndex="Fecha"
                                                            MenuDisabled="false"
                                                            Hidden="false"
                                                            Align="Center" 
                                                            Format="dd/MM/yyyy">
                                                        </ext:DateColumn>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strVersion_ %>"
                                                            Flex="1"
                                                            MenuDisabled="true"
                                                            DataIndex="Version"
                                                            Hidden="false"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strTamano %>"
                                                            Flex="2"
                                                            MenuDisabled="true"
                                                            DataIndex="Tamano"
                                                            Hidden="false">
                                                            <Renderer Fn="rendererTamano" />
                                                        </ext:Column>

                                                        <ext:ComponentColumn ID="ColMore"
                                                            runat="server"
                                                            Width="105"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            MinWidth="70"
                                                            MaxWidth="70">
                                                            <Component>
                                                                <ext:Button runat="server"
                                                                    Width="90"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Fn="MostrarPanelLateral" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Component>
                                                            <Listeners>
                                                                <Bind Fn="HideBtnMore" />
                                                            </Listeners>
                                                        </ext:ComponentColumn>
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
                                                <Plugins>
                                                    <ext:GridFilters runat="server" />
                                                </Plugins>
                                                <SelectionModel>
                                                    <ext:TreeSelectionModel runat="server"
                                                        ID="GridRowSelectArbol"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Select Fn="CheckMultiSelect_Grid_RowSelectArbol" />
                                                            <Deselect Fn="CheckMultiSelect_Grid_RowSelectArbol" />
                                                        </Listeners>
                                                    </ext:TreeSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                    <ItemMouseEnter Fn="tooltipPermisosTipoDocumento" />
                                                    <ItemMouseLeave Fn="tooltipPermisosTipoDocumentoLeave" />
                                                </Listeners>
                                            </ext:TreePanel>--%>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="ctMain2" Flex="4" Layout="FitLayout" Cls="col colCt2 " PaddingSpec="8 8 20 0" Hidden="true">
                                        <Items>
                                            <ext:Panel ID="PanelVisorMain"
                                                runat="server"
                                                Header="false"
                                                Border="false"
                                                BodyCls="Fixvisualizador"
                                                Layout="FitLayout"
                                                Cls="visorInsidePn pnNoHeader">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar5"
                                                        Dock="Top"
                                                        Height="50"
                                                        PaddingSpec="5px 0 8px 14px"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory">
                                                                <Listeners>
                                                                    <Click Fn="btnCloseShowVisorTreeP" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Label runat="server" ID="labelVisor" Cls="HeaderLblVisor" Text=""></ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server"
                                                                ID="GridbtnvistaGrid"
                                                                Cls="btnvistaGrid"
                                                                AriaLabel="Cambiar Vista Solo Grid"
                                                                ToolTip="<%$ Resources:Comun, strCambiaModoGrid %>">
                                                                <Listeners>
                                                                    <Click Fn="VisorMode" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server" ID="Toolbar6" Cls="tlbGrid" Layout="ColumnLayout" Dock="Top">
                                                        <Items>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnInfoVisor"
                                                                Cls="btnInfo"
                                                                AriaLabel="Añadir"
                                                                ToolTip="<%$ Resources:Comun, strInfoDoc %>"
                                                                Handler="MostrarPanelLateralInfo();">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnHistoricoVisor"
                                                                Cls="btnTiposDocs"
                                                                AriaLabel="Editar"
                                                                ToolTip="<%$ Resources:Comun, strHistorico %>"
                                                                Handler="MostrarPanelLateralVersiones();">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnDescargarVisor"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Eliminar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="descargarDocumento();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDesactivarVisor"
                                                                Cls="btnDesactivar"
                                                                AriaLabel="Desactivar"
                                                                ToolTip="<%$ Resources:Comun, strDesactivar %>"
                                                                Handler="desactivarDocumento();">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Loader runat="server" ID="loaderDoc" Url="" Mode="Frame"></Loader>
                                                <Items>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="ctMain3" Flex="1" Layout="FitLayout" Cls="col colCt3" Hidden="true">
                                        <Items>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="false"
                                CollapseMode="Header"
                                Collapsible="true"
                                Cls=""
                                Header="false" Border="false" Width="380" Hidden="false">
                                <Listeners>
                                    <Collapse Handler="ActiveResizer();"></Collapse>
                                    <Expand Handler="ActiveResizer();"></Expand>
                                </Listeners>
                                <Items>
                                    <ext:Toolbar
                                        runat="server"
                                        ID="tbHeaderDownload"
                                        Cls="tbGrey"
                                        PaddingSpec="25 34 8 20">
                                        <Items>
                                            <ext:Label
                                                meta:resourcekey="lblAsideNameR"
                                                ID="lblAsideNameR"
                                                runat="server"
                                                IconCls=""
                                                Cls="lblHeadAside lblHeadAsideBlue"
                                                Text="<%$ Resources:Comun, strInfoDoc %>">
                                            </ext:Label>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button
                                                runat="server"
                                                ID="btnTbHeaderDescargar"
                                                Text=""
                                                ToolTip="<%$ Resources:Comun, jsDescargar %>"
                                                Cls="btn-trans btnDescargar btnNoBorder"
                                                IconCls="">
                                                <Listeners>
                                                    <Click Handler="descargarDocumento();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideR"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Cls="ctAsideR">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel
                                                ID="mnAsideR"
                                                runat="server"
                                                Border="false"
                                                Header="false">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnInfoGrid"
                                                        ID="btnGridInfo"
                                                        Cls="btnInfo-asR"
                                                        ToolTip="<%$ Resources:Comun, jsInfo %>"
                                                        Handler="displayMenuDoc('pnGridInfo')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnVersions"
                                                        ID="btnInfoVersiones"
                                                        Cls="btnHistory-asR"
                                                        ToolTip="<%$ Resources:Comun, jsHistorico %>"
                                                        Handler="displayMenuDoc('pnInfoVersiones')">
                                                    </ext:Button>
                                                    <%-- <ext:Button runat="server" meta:resourcekey="btnInfoGrid" ID="btnMetadata" Cls="btnMetadata-asR" Handler="displayMenu('pnMetaData')"></ext:Button>--%>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnQuickFilters"
                                                        ID="btnQuickFilters"
                                                        Cls="btnFilters-asR"
                                                        ToolTip="<%$ Resources:Comun, jsFiltros %>"
                                                        Handler="displayMenuDoc('pnQuickFilters')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANEL INFO GRID--%>
                                            <ext:Panel ID="pnGridInfo" runat="server" Border="false" Header="false" Hidden="false" Scrollable="Vertical">
                                                <Listeners>
                                                    <AfterRender Fn="resizeGridInfo" />
                                                    <Resize Fn="resizeGridInfo" />
                                                </Listeners>
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-docs-gr" Cls="lblHeadAsideMid " Dock="Top" Text="INFO"></ext:Label>
                                                    <ext:Panel ID="grAsR1" runat="server"
                                                        Cls="grdPnColIcons grdIntoAside"
                                                        Border="false"
                                                        Scrollable="Vertical"
                                                        OverflowY="Auto">
                                                        <Content runat="server">
                                                            <div>
                                                                <table class="tmpCol-table" id="tableInfoDocumentos">
                                                                    <tbody id="bodyTablaInfoDocumentos">
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                            <%--<Columns>
                                                                <ext:TemplateColumn runat="server" DataIndex="" ID="templateColumn1" MenuDisabled="true" Text="" Flex="1">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
												                                <table class="tmpCol-table" id="tableInfoDocumentos">
                                                                                    <tbody id="bodyTablaInfoDocumentos">
                                                                                    </tbody>
																		        </table>
                               										        </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                </ext:TemplateColumn>
                                                            </Columns>--%>
                                                        </Content>
                                                        <%--<View>
                                                            <ext:GridView runat="server" TrackOver="false" EnableTextSelection="true">
                                                            </ext:GridView>
                                                        </View>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />
                                                        </SelectionModel>
                                                        <DockedItems></DockedItems>--%>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANEL VERSIONES--%>
                                            <ext:Panel runat="server"
                                                ID="pnInfoVersiones"
                                                Border="false" Header="false" Scrollable="Vertical"
                                                Hidden="true">
                                                <Items>
                                                    <ext:Label
                                                        meta:resourcekey="lblAsideNameR"
                                                        ID="lblVersiones"
                                                        runat="server"
                                                        IconCls="ico-head-tag-gr"
                                                        Cls="lblHeadAsideMid "
                                                        Dock="Top"
                                                        Text="<%$ Resources:Comun, strHistorico %>">
                                                    </ext:Label>
                                                    <ext:GridPanel
                                                        ID="GridUltimaVersion"
                                                        runat="server"
                                                        Height="600"
                                                        Border="false"
                                                        Cls="grdPnColIcons grdIntoAside"
                                                        Scrollable="Vertical"
                                                        StoreID="storeVersiones">
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:TemplateColumn
                                                                    runat="server"
                                                                    ID="TemplateColumn3"
                                                                    DataIndex=""
                                                                    MenuDisabled="true"
                                                                    Text=""
                                                                    Flex="5">
                                                                    <Template ID="historico" runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div class="customCol1" onclick="mostrarDocuVersion({DocumentoID});">
                                                                                    <p class="TopTitleCustomCol1 nameDocumentHistorical" title="{Documento}">{Documento}</p>
                                                                                    <div class="customColDiv1">
                                                                                        {Fecha}
                                                                                    </div>
                                                                                    <div class="customColDiv1">
                                                                                        {Creador}
                                                                                    </div>
                                                                                    <div class="customColDiv1">
                                                                                        v{Version} | {Tamano}
                                                                                    </div>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                    <Renderer Fn="renderTamanoHistorico" />
                                                                    <Listeners>
                                                                        <AfterRender Fn="nameDocumentHistorical" />
                                                                    </Listeners>
                                                                </ext:TemplateColumn>
                                                                <ext:ComponentColumn
                                                                    ID="WidgetColumn2"
                                                                    runat="server"
                                                                    Width="50"
                                                                    Cls="col-More"
                                                                    DataIndex="UltimaVersion"
                                                                    Align="End"
                                                                    Text=""
                                                                    Hidden="false">
                                                                    <Component>
                                                                        <ext:Button
                                                                            runat="server"
                                                                            ToolTip="<%$ Resources:Comun, strRestaurar %>"
                                                                            Width="40"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="btn-trans btnReset16p"
                                                                            Handler="restaurarDocumento(this);">
                                                                        </ext:Button>
                                                                    </Component>
                                                                    <Listeners>
                                                                        <Bind Fn="hideIfNotUltimaVersion" />
                                                                    </Listeners>
                                                                </ext:ComponentColumn>
                                                                <ext:ComponentColumn
                                                                    ID="WidgetColumn1"
                                                                    runat="server"
                                                                    Width="50"
                                                                    Cls="col-More"
                                                                    DataIndex="UltimaVersion"
                                                                    Align="End"
                                                                    Text=""
                                                                    Hidden="false">
                                                                    <Component>
                                                                        <ext:Button
                                                                            runat="server"
                                                                            ToolTip="<%$ Resources:Comun, strDesactivar %>"
                                                                            Width="40"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="btn-trans btnNoDoc16p"
                                                                            Handler="desactivarVersionDocumento(this);">
                                                                        </ext:Button>
                                                                    </Component>
                                                                    <Listeners>
                                                                        <Bind Fn="hideIfNotUltimaVersionDeactivate" />
                                                                    </Listeners>
                                                                </ext:ComponentColumn>
                                                                <ext:Column
                                                                    ID="colVersDesact"
                                                                    runat="server"
                                                                    DataIndex="Activo"
                                                                    Align="Center"
                                                                    Width="50"
                                                                    Hidden="false">
                                                                    <Renderer Fn="renderInactiveDocuments" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectVersiones"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelectVersiones" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                    </ext:GridPanel>
                                                    <%--<ext:GridPanel
                                                        ID="GridPanelVersionesOld"
                                                        Margin="8"
                                                        runat="server"
                                                        OverflowY="Auto"
                                                        OverflowX="Hidden"
                                                        Cls=" gridPanel grdPanelVersionesOld "
                                                        StoreID="storeVersiones">
                                                        <DockedItems>
                                                            <ext:Label runat="server" ID="Label4" Text="Older Versions" Cls="TopLabelGrid" Dock="Top"></ext:Label>
                                                        </DockedItems>
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="5">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div class="customCol1">
                                                                                    <p class="TopTitleCustomCol1">HOla</p> 
                                                                                    <div  class="customColDiv1">
                                                                                        HOLA
                                                                                    </div>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                </ext:TemplateColumn>
                                                                <ext:WidgetColumn ID="WidgetColumn1" runat="server" Width="105" Cls="col-More" DataIndex="" Align="Center" Text="" Hidden="false" MinWidth="70">
                                                                    <Widget>
                                                                        <ext:Button runat="server" Width="40" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btn-trans bntBasura" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                    </ext:GridPanel>--%>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANEL METADATA GRID--%>
                                            <ext:Panel ID="pnMetaData" runat="server" Border="false" Header="false" Hidden="true">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label3" runat="server" IconCls="ico-head-tag-gr" Cls="lblHeadAsideMid " Dock="Top" Text="METADATA"></ext:Label>
                                                    <ext:GridPanel ID="GridMetadata" runat="server"
                                                        Height="900"
                                                        Cls="grdPnColIcons grdIntoAside"
                                                        Border="false">
                                                        <Store>
                                                            <ext:Store
                                                                ID="Store1"
                                                                runat="server"
                                                                Buffered="true"
                                                                RemoteFilter="true"
                                                                LeadingBufferZone="1000"
                                                                PageSize="50">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Id" />
                                                                            <ext:ModelField Name="Ini" />
                                                                            <ext:ModelField Name="Name" />
                                                                            <ext:ModelField Name="Profile" />
                                                                            <ext:ModelField Name="Company" />
                                                                            <ext:ModelField Name="Email" />
                                                                            <ext:ModelField Name="Project" />
                                                                            <ext:ModelField Name="Authorized" />
                                                                            <ext:ModelField Name="Staff" />
                                                                            <ext:ModelField Name="Support" />
                                                                            <ext:ModelField Name="LDAP" />
                                                                            <ext:ModelField Name="License" />
                                                                            <ext:ModelField Name="KeyExpiration" />
                                                                            <ext:ModelField Name="LastKey" />
                                                                            <ext:ModelField Name="LastAccess" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server" DataIndex="" ID="templateColumn2" MenuDisabled="true" Text="" Flex="1">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <table class="tmpCol-table">
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">Company</span><span class="dataGrd">{Company}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3"></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">Email</span><span class="dataGrd">{Email}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">License</span><span class="dataGrd">{License}</span></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="2">
                                                                                            <span class="lblGrd">Project</span><span class="dataGrd">{Project}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3"></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">License</span><span class="dataGrd">{License}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">License</span><span class="dataGrd">{License}</span></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">Key Expiration</span><span class="dataGrd">{KeyExpiration}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3"></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">Last Key</span><span class="dataGrd">{LastKey}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3"></td>
                                                                                    </tr>
                                                                                    <tr class="tmpCol-tr">
                                                                                        <td class="tmpCol-td" colspan="3">
                                                                                            <span class="lblGrd">Last Access</span><span class="dataGrd">{LastAccess}</span></td>
                                                                                        <td class="tmpCol-td" colspan="3"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                </ext:TemplateColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <View>
                                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                                                            </ext:GridView>
                                                        </View>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />
                                                        </SelectionModel>
                                                        <DockedItems></DockedItems>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANEL QUICK FILTERS--%>
                                            <ext:Panel
                                                ID="pnQuickFilters"
                                                runat="server"
                                                PaddingSpec="10 20 20 15"
                                                Border="false"
                                                Header="false"
                                                Scrollable="Vertical"
                                                Hidden="true"
                                                Cls="Whitebg pnQuickFilters">
                                                <DockedItems>
                                                    <ext:Label
                                                        MarginSpec="5 0 8 0"
                                                        Dock="Top"
                                                        meta:resourcekey="lblGrid"
                                                        ID="Label4"
                                                        runat="server"
                                                        IconCls="ico-head-filters"
                                                        Cls="lblHeadAsideMid"
                                                        Text="<%$ Resources:Comun, strFiltrosRapidos %>">
                                                    </ext:Label>

                                                    <ext:Toolbar runat="server" ID="Toolbar4" Dock="Bottom" Padding="0" MarginSpec="8 8 0 0">
                                                        <Items>

                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnAplyFilt"
                                                                Cls="btn-ppal "
                                                                Text="<%$ Resources:Comun, strAplicarFiltro %>"
                                                                Focusable="false"
                                                                PressedCls="none"
                                                                Hidden="false"
                                                                Flex="10">
                                                                <Listeners>
                                                                    <Click Fn="aplicarFiltrosRapidos" />
                                                                </Listeners>
                                                            </ext:Button>

                                                            <ext:Button
                                                                runat="server"
                                                                ID="Button1"
                                                                Cls="btn-secondary "
                                                                Text="<%$ Resources:Comun, strLimpiarFiltro %>"
                                                                Focusable="false"
                                                                PressedCls="none"
                                                                Hidden="false"
                                                                Flex="10">
                                                                <Listeners>
                                                                    <Click Fn="LimpiarFiltrosRapidos" />
                                                                </Listeners>
                                                            </ext:Button>

                                                        </Items>
                                                    </ext:Toolbar>

                                                </DockedItems>
                                                <Items>

                                                    <ext:MultiCombo
                                                        ID="cmbfiltDocumentoTipo"
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="<%$ Resources:Comun, strDocumentoTipo %>"
                                                        DisplayField="DocumentTipo"
                                                        ValueField="DocumentTipoID"
                                                        QueryMode="Local">
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <TriggerClick Handler="RecargarFiltroDocumentoTipo();" />
                                                            <Select Fn="SeleccionaFiltroDocumentoTipo" />
                                                        </Listeners>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeFiltDocumentoTipo"
                                                                AutoLoad="false"
                                                                RemoteFilter="true"
                                                                RemoteSort="true"
                                                                PageSize="50"
                                                                OnReadData="storeFiltDocumentoTipo_Refresh">
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="DocumentTipoID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="DocumentTipoID" />
                                                                            <ext:ModelField Name="DocumentTipo" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <Listeners>
                                                            <BeforeSelect Fn="" />
                                                        </Listeners>
                                                    </ext:MultiCombo>

                                                    <ext:MultiCombo
                                                        ID="cmbfiltExtension"
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="<%$ Resources:Comun, strExtension %>"
                                                        DisplayField="Extension"
                                                        ValueField="Extension"
                                                        QueryMode="Local">
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <TriggerClick Handler="RecargarExtension();" />
                                                            <Select Fn="SeleccionaExtension" />
                                                        </Listeners>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeFiltExtension"
                                                                AutoLoad="false"
                                                                RemoteFilter="true"
                                                                RemoteSort="true"
                                                                PageSize="50"
                                                                OnReadData="storeFiltExtension_Refresh">
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
                                                        </Store>
                                                    </ext:MultiCombo>

                                                    <ext:MultiCombo
                                                        ID="cmbfiltEstado"
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="<%$ Resources:Comun, strEstado %>"
                                                        DisplayField="Nombre"
                                                        ValueField="DocumentoEstadoID"
                                                        QueryMode="Local">
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <TriggerClick Handler="RecargarEstado();" />
                                                            <Select Fn="SeleccionaEstado" />
                                                        </Listeners>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeFiltEstado"
                                                                AutoLoad="false"
                                                                RemoteFilter="true"
                                                                RemoteSort="true"
                                                                PageSize="50"
                                                                OnReadData="storeFiltEstado_Refresh">
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="DocumentoEstadoID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="DocumentoEstadoID" Type="Int" />
                                                                            <ext:ModelField Name="Codigo" />
                                                                            <ext:ModelField Name="Nombre" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:MultiCombo>

                                                    <ext:MultiCombo
                                                        ID="cmbfiltCreador"
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="<%$ Resources:Comun, strCreador %>"
                                                        DisplayField="EMail"
                                                        ValueField="UsuarioID"
                                                        QueryMode="Local">
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <TriggerClick Handler="RecargarCreador();" />
                                                            <Select Fn="SeleccionaCreador" />
                                                        </Listeners>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeFiltCreador"
                                                                AutoLoad="false"
                                                                RemoteFilter="true"
                                                                RemoteSort="true"
                                                                PageSize="50"
                                                                OnReadData="storeFiltCreador_Refresh">
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="UsuarioID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="UsuarioID" Type="Int" />
                                                                            <ext:ModelField Name="EMail" />
                                                                            <ext:ModelField Name="Nombre" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                    </ext:MultiCombo>

                                                    <ext:Panel
                                                        runat="server"
                                                        ID="wrapSelectoresFecha"
                                                        Layout="HBoxLayout"
                                                        MarginSpec="0 17 0 0">
                                                        <Items>

                                                            <ext:DateField
                                                                ID="datfiltDateStart"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                Cls=""
                                                                Flex="1"
                                                                MarginSpec="0 8 0 0"
                                                                FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                                Format="dd/MM/yyyy">
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="LimpiarDateStart();" />
                                                                    <Change Fn="SeleccionaDateStart" />
                                                                </Listeners>
                                                            </ext:DateField>

                                                            <ext:DateField
                                                                ID="datfiltDateEnd"
                                                                MarginSpec="0 0 0 8"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                Flex="1"
                                                                Cls=""
                                                                FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                                                Format="dd/MM/yyyy">
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="LimpiarDateEnd();" />
                                                                    <Change Fn="SeleccionaDateEnd" />
                                                                </Listeners>
                                                            </ext:DateField>

                                                        </Items>
                                                    </ext:Panel>

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
    <form class="dropzone" action="DocumentosVista.ashx"></form>
</body>
</html>
