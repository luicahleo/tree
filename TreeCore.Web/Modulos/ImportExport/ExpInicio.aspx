<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpInicio.aspx.cs" Inherits="TreeCore.ModExportarImportar.pages.ExpInicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Export / Import tool</title>

</head>
<body>
    <link href="css/styleExpInicio.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <WindowResize Handler="winFormResize();" />
                </Listeners>
            </ext:ResourceManager>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeProyectosTipos"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storeProyectosTipos_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="Alias" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Alias" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN STORES --%>

            <%--WINDOW FORM --%>

            <ext:Window ID="winModuloExport"
                meta:resourceKey="winModuloExport"
                runat="server"
                Title=""
                Width="350"
                Height="210"
                Modal="true"
                Centered="true"
                Cls="winForm-resp winGestion-panel"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                BodyPadding="20"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar3"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 18 40">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnExportarTemp"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, strExportar %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Disabled="true"
                                Hidden="false"
                                Handler="btnExportarConfiguracion();">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formDownload"
                        Cls="formGris">
                        <Items>
                            <ext:ComboBox
                                ID="MCModulos"
                                QueryMode="Local"
                                StoreID="storeProyectosTipos"
                                runat="server"
                                DisplayField="Alias"
                                ValueField="Alias"
                                LabelAlign="Top"
                                Hidden="false"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strProyectoTipo %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                <Listeners>
                                    <Select Fn="SeleccionarProyectoTipo" />
                                    <TriggerClick Fn="RecargarProyectoTipo" />
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
                            <ValidityChange Handler="FormularioValidoDownload(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="WinConfirmExport"
                meta:resourceKey="WinConfirmExport"
                runat="server"
                Title=""
                Width="672"
                Height="210"
                Modal="true"
                Centered="true"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar1"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 30 18 40">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="Button13"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, strExportar %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Hidden="false"
                                Handler="btnExportarConfiguracion();">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:Label IconAlign="Left"
                        IconCls="btnDescargar"
                        meta:resourceKey="LabelDescargar"
                        Cls="ExitoLbl"
                        ID="Label18"
                        runat="server"
                        Text="Are you sure you wan to export the content?"
                        MarginSpec="20 10 20 10"
                        Hidden="false">
                    </ext:Label>
                    <ext:Label IconAlign="Left"
                        IconCls=""
                        Cls=""
                        meta:resourceKey="lblDescarga"
                        ID="Label1d8"
                        runat="server"
                        Text="This action will create a file "
                        MarginSpec="10 10 20 50"
                        Hidden="false">
                    </ext:Label>
                </Items>
            </ext:Window>

            <ext:Window ID="WinConfirmImport"
                meta:resourceKey="WinConfirmImport"
                runat="server"
                Title="Import Content"
                Width="350"
                Height="350"
                Modal="true"
                Centered="true"
                Cls="winForm-resp"
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
                        PaddingSpec="0 30 18 40">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnImportarTemp"
                                Cls="btn-ppal btnBold"
                                MinWidth="160"
                                Text="<%$ Resources:Comun, strImportar %>"
                                Focusable="false"
                                PressedCls="none"
                                 Disabled="true"
                                Flex="3"
                                Handler="btnImportarConfiguracion();"
                                Hidden="false">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formImport">
                        <Items>
                            <ext:Label IconAlign="Left"
                                IconCls="ico-upload-grid"
                                Cls="formtitle"
                                ID="Label19"
                                meta:resourceKey="lblImportar"
                                runat="server"
                                Text="Are you sure you want to import the content?"
                                MarginSpec="20 10 10 10"
                                Hidden="true">
                            </ext:Label>
                            <ext:Label IconAlign="Left"
                                IconCls=""
                                Cls=""
                                ID="Label20"
                                meta:resourceKey="lblActionImport"
                                runat="server"
                                Text="This action will create or modify current data "
                                MarginSpec="0 10 10 35"
                                Hidden="true">
                            </ext:Label>
                            <ext:ComboBox runat="server"
                                ID="cmbTipe"
                                QueryMode="Local"
                                StoreID="storeProyectosTipos"
                                DisplayField="Alias"
                                ValueField="Alias"
                                Width="280"
                                AllowBlank="false"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strProyectoTipo %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Editable="true">
                                <Listeners>
                                    <Select Fn="SeleccionarProyectoTipoImp" />
                                    <TriggerClick Fn="RecargarProyectoTipoImp" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:FileUploadField runat="server"
                                ID="UploadF"
                                LabelAlign="Top"
                                Width="280"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strArchivo %>"
                                PaddingSpec="0 10 0 10"
                                ButtonText=""
                                Cls=" btnUploadFolderGrid">
                            </ext:FileUploadField>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoImport(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winExportImportProgress"
                meta:resourceKey="winExportImportProgress"
                runat="server"
                Title="Export Tool"
                Width="500"
                Height="500"
                Modal="true"
                Centered="true"
                Cls=""
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this)"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="tbWinProgress"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 30 18 40">
                        <Items>
                            <ext:Button runat="server"
                                ID="Button10"
                                Cls="btn-ppal btnBold "
                                IconCls="ico-docs-white"
                                MinWidth="150"
                                Text="<%$ Resources:Comun, strDownloadLog %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="ExportarLogMigrador();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="backNewUsers"
                                Cls="btn-secondary minW"
                                MinWidth="100"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winExportImportProgress}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnDescargarPlantilla"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="DescargarPlantilla();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:Label IconAlign="Left"
                        IconCls="btnDescargar"
                        ID="ProgresoLabel"
                        meta:resourceKey="lblProgreso"
                        runat="server"
                        Text="1/2 Creating Files"
                        MarginSpec="20 10 20 0"
                        Hidden="true">
                    </ext:Label>
                    <ext:Label IconAlign="Left"
                        IconCls="btnDescargar"
                        ID="ExitoLabel"
                        meta:resourceKey="lblExito"
                        Cls="ExitoLbl"
                        runat="server"
                        Text="Success!"
                        MarginSpec="20 10 20 0"
                        Hidden="true">
                    </ext:Label>
                    <ext:Label IconAlign="Left"
                        IconCls="btnAlertaRed"
                        ID="ProblemaLabel"
                        meta:resourceKe="lblProblema"
                        Cls="ExitoLbl"
                        runat="server"
                        Text="There was a problem..."
                        MarginSpec="20 10 20 0"
                        Hidden="false">
                    </ext:Label>
                    <ext:ProgressBar runat="server"
                        Value=".34"
                        Width="500"
                        MinHeight="20"
                        MarginSpec="0 40 0 35"
                        Hidden="true">
                    </ext:ProgressBar>
                    <ext:TextArea runat="server"
                        Editable="false"
                        Cls="outputDisable"
                        Disabled="false"
                        HeightSpec="75%"
                        MarginSpec="10 40 0 40"
                        ID="TextArea"
                        Text="">
                    </ext:TextArea>
                </Items>
            </ext:Window>

            <%--FIN WINDOW FORM --%>


            <ext:Viewport ID="vwResp" runat="server">
                <Listeners>
                    <AfterRender Fn="DisplayBtnsSliders()"></AfterRender>
                </Listeners>
                <Items>
                    <ext:Button runat="server"
                        ID="btnCollapseAsR"
                        Hidden="true"
                        Cls="btn-trans"
                        Handler="hidePn();">
                    </ext:Button>

                    <ext:Container runat="server" ID="ctBtnSldr">
                        <Items>
                            <ext:Button runat="server"
                                ID="btnPrevSldr"
                                IconCls="ico-prev"
                                Cls="btnMainSldr"
                                Handler="moveCtSldr(this);">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnNextSldr"
                                IconCls="ico-next"
                                Handler="moveCtSldr(this);"
                                Disabled="true">
                            </ext:Button>
                        </Items>
                    </ext:Container>
                    <%--CONTENT--%>
                    <ext:Container ID="ctHuge"
                        Cls="ctHuge col-total"
                        runat="server"
                        HeightSpec="100%">
                        <Items>
                            <ext:Container ID="ctMain1"
                                Cls="col colCt1 m-r col-total col-pdng"
                                runat="server"
                                Hidden="false"
                                HeightSpec="100%">
                                <Items>
                                    <ext:Panel runat="server" ID="ExpHeader">
                                        <Items>
                                            <ext:Label ID="ExpMaintitle"
                                                runat="server"
                                                Cls="big-lbl-title LblElipsis"
                                                Text="<%$ Resources:Comun, strExportarImportar %>"
                                                Flex="8">
                                            </ext:Label>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" Cls="Expbox" Height="197px">
                                        <Items>
                                            <ext:Container runat="server" Cls="ExpTitleBox">
                                                <Items>
                                                    <ext:Label MarginSpec=""
                                                        runat="server"
                                                        ID="ExportTitle"
                                                        meta:resourceKey="lblExportTitle"
                                                        Cls="ExpDescription, h4"
                                                        Text="Export Content">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Container>
                                            <ext:Component runat="server"
                                                Cls="ExtIcon ico-download-grid">
                                            </ext:Component>
                                            <ext:Label runat="server"
                                                Cls="ExpBoxDescription"
                                                meta:resourceKey="lblDescription"
                                                Text="You must select the project type in the Export Button">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                Cls="btn-ppal"
                                                Text="<%$ Resources:Comun, strExportar %>"
                                                Handler="ExpOpenExport();">
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" Cls="Expbox" Height="197px">
                                        <Items>
                                            <ext:Container runat="server" Cls="ExpTitleBox">
                                                <Items>
                                                    <ext:Label MarginSpec=""
                                                        runat="server"
                                                        ID="ImportTitle"
                                                        meta:resourceKey="lblImportTitle"
                                                        Cls="ExpDescription, h4"
                                                        Text="Import Content">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Container>
                                            <ext:Component runat="server" Cls="ExtIcon ico-upload-grid"></ext:Component>
                                            <ext:Label runat="server"
                                                Cls="ExpBoxDescription"
                                                meta:resourceKey="lblDescriptionImport"
                                                Text="You must select the File in the Import Button">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                Cls="btn-ppal"
                                                Text="<%$ Resources:Comun, strImportar %>"
                                                Handler="OpenImport();">
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <%--END CONTENT--%>

                    <%--RIGHT PANEL--%>
                    <ext:Panel ID="pnAsideR" runat="server" Hidden="true">
                        <Items>
                            <ext:Label runat="server"
                                ID="lblAsideNameR"
                                IconCls="btnHistorygreen"
                                Cls="lblHeadAside"
                                meta:resourceKey="lblAsideNameR"
                                Text="EXPORT / IMPORT HISTORICAL">
                            </ext:Label>
                            <ext:Panel runat="server"
                                ID="ctAsideR"
                                Border="false"
                                Header="false"
                                Cls="ctAsideR">
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="mnAsideR"
                                        Border="false"
                                        Header="false">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnExportar"
                                                Cls="btnExport-asR"
                                                ToolTip="<%$ Resources:Comun, strExportar %>"
                                                FocusCls="none"
                                                Handler="RpanelBTNNotes();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnImportar"
                                                Cls="btnImport-asR"
                                                ToolTip="<%$ Resources:Comun, strImportar %>"
                                                FocusCls="none"
                                                Handler="RpanelBTNAlert();">
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <%--PANELS--%>
                                    <ext:Panel runat="server"
                                        ID="pnGridsAsideR"
                                        Border="false"
                                        StoreID="storeNotificaciones"
                                        Header="false"
                                        Title="etiqgridTitle"
                                        Hidden="false"
                                        Scrollable="Vertical"
                                        OverflowY="Scroll">
                                        <Items>
                                            <%--IMPORT PANEL--%>
                                            <ext:Panel runat="server"
                                                ID="pnNotificationsFull"
                                                Hidden="true">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="Label8"
                                                        meta:resourceKey="lblImportarHistorico"
                                                        IconCls="btnImportgreen-asR"
                                                        Cls="lblHeadAside"
                                                        Text="Import Historical">
                                                    </ext:Label>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="Panel1"
                                                        Header="false"
                                                        Hidden="false"
                                                        Disabled="false">
                                                        <Items>
                                                            <ext:Container runat="server" Cls="mainContainer">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        Text="<%$ Resources:Comun, strBuscar %>"
                                                                        Cls="ExpSearchText" />
                                                                    <ext:Container runat="server">
                                                                        <Items>
                                                                            <ext:TextField
                                                                                runat="server"
                                                                                Cls="ExpSearch"
                                                                                Width="302"
                                                                                EmptyText="<%$ Resources:Comun, strBuscar %>">
                                                                                <Triggers>
                                                                                    <ext:FieldTrigger Icon="Search" />
                                                                                </Triggers>
                                                                            </ext:TextField>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                        <Content>
                                                            <ul id="ulNotif" class="ulboxes">
                                                                <li id="ImpExample" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="Label9"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label10"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label11"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button7"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="ImpExample2" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="Label12"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label13"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label14"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button6"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="ImpExample3" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="Label15"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label16"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label17"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button8"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                            </ul>
                                                        </Content>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                            <%--EXPORT PANEL--%>
                                            <ext:Panel runat="server"
                                                ID="pnNotesFull"
                                                Hidden="false">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="aLabel1"
                                                        meta:resourceKey="lblExportarHistorico"
                                                        IconCls="btnExportgreen-asR"
                                                        Cls="lblHeadAside"
                                                        Text="Export Historical">
                                                    </ext:Label>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnAlerts"
                                                        Header="false"
                                                        Hidden="false"
                                                        Disabled="false">
                                                        <Items>
                                                            <ext:Container runat="server" Cls="mainContainer">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        Text="<%$ Resources:Comun, strBuscar %>"
                                                                        Cls="ExpSearchText" />
                                                                    <ext:Container runat="server">
                                                                        <Items>
                                                                            <ext:TextField
                                                                                runat="server"
                                                                                Cls="ExpSearch"
                                                                                Width="302"
                                                                                EmptyText="<%$ Resources:Comun, strBuscar %>">
                                                                                <Triggers>
                                                                                    <ext:FieldTrigger Icon="Search" />
                                                                                </Triggers>
                                                                            </ext:TextField>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                        <Content>
                                                            <ul id="ulNotif2" class="ulboxes">
                                                                <li id="ExpExample" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel2"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel4"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label1"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button1"
                                                                        Cls="btnDownload-item">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="aButton2"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="ExpExample2" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="Label2"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label3"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label4"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button2"
                                                                        Cls="btnDownload-item">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="Button3"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="ExpExample3" class="liNot">
                                                                    <div class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="Label5"
                                                                            Cls="titNot"
                                                                            Text="User Name">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label6"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 | 17:29h">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="Label7"
                                                                            Cls="version"
                                                                            Text="v.5.1.2.45">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server" Cls="liVbox" Text="Prep. V."></ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="Button4"
                                                                        Cls="btnDownload-item">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="Button5"
                                                                        Cls="btnDelete-item">
                                                                    </ext:Button>
                                                                </li>
                                                            </ul>
                                                        </Content>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                    <%--END RIGHT PANEL--%>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
