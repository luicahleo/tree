<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataUpload.aspx.cs" Inherits="TreeCore.ModExportarImportar.DataUpload" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleDataUpload.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/DataUpload.js"></script>
    <link href="/CSS/HomeStyle.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <ext:Hidden ID="hdRuta1" runat="server" />

            <ext:Hidden ID="hdRuta2" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeDocumentosCargaPlantillas"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storeDocumentosCargaPlantillas_Refresh"
                RemoteSort="false">
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
                            <ext:ModelField Name="FechaSubida" Type="Date" />
                            <ext:ModelField Name="RutaDocumento" Type="String" />
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="Alias" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeAuxiliar"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeAuxiliar_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ElementoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ElementoID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window ID="WinConfirmExport"
                runat="server"
                Title="Download Template"
                meta:resourceKey="lblExportTitle"
                Width="400"
                Height="300"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formDownload"
                        Height="200">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbPlantillas"
                                QueryMode="Local"
                                StoreID="storeDocumentosCargaPlantillas"
                                LabelAlign="Top"
                                DisplayField="DocumentoCargaPlantilla"
                                ValueField="DocumentoCargaPlantilla"
                                Editable="true"
                                AllowBlank="false"
                                MaxWidth="350"
                                WidthSpec="90%"
                                FieldLabel="<%$ Resources:Comun, strPlantillas %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                PaddingSpec="10 0 0 0">
                                <Listeners>
                                    <Select Fn="SeleccionarPlantilla" />
                                    <TriggerClick Fn="RecargarPlantilla" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:MultiCombo runat="server"
                                ID="cmbAuxiliar"
                                QueryMode="Local"
                                StoreID="storeAuxiliar"
                                LabelAlign="Top"
                                DisplayField="Nombre"
                                ValueField="ElementoID"
                                AllowBlank="true"
                                Disabled="true"
                                MaxWidth="350"
                                WidthSpec="90%"
                                FieldLabel="<%$ Resources:Comun, strCategorias %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                PaddingSpec="10 0 0 0">
                                <CustomConfig>
                                    <ext:ConfigItem Name="MaxSelection" Value="50" />
                                </CustomConfig>
                                <Validator Fn="SeleccionarComboAuxiliar" />
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:MultiCombo>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoDownload(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Cls="btn-cancel"
                        MinWidth="120"
                        IconCls="ico-cancel"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>">
                        <Listeners>
                            <Click Handler="#{WinConfirmExport}.hide();#{formDownload}.getForm().reset();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnDescargar"
                        MinWidth="120"
                        Cls="btn-accept"
                        IconCls="ico-accept"
                        Text="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="DescargarPlantilla();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">

                <%-- <Listeners>
                    <AfterRender Fn="DisplayBtnsSliders()"></AfterRender>
                </Listeners>--%>
                <Items>
                    <%--<ext:Container runat="server" ID="ctBtnSldr">
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
                    </ext:Container>--%>
                    <%--CONTENT--%>

                    <ext:Container ID="ctMain1"
                        runat="server"
                        Hidden="false"
                        Cls="col-pdng"
                        MaxHeight="800"
                        Scrollable="Vertical">

                        <Items>
                            <ext:Panel runat="server" ID="ExpHeader">
                                <Items>
                                    <ext:Panel runat="server" ID="ImportExportHeader"
                                        Cls="HeaderInicio"
                                        PaddingSpec="0 0 16 0">
                                        <Items>
                                            <ext:Image
                                                runat="server"
                                                Src="../../ima/modulos/ic_modulos_exportImport.svg"
                                                Height="80"
                                                Width="80"
                                                MarginSpec="0 5 0 0"
                                                Cls="image-title"
                                                Alt="ImportExport" />
                                            <ext:Label ID="ExpMaintitle"
                                                meta:resourceKey="lblExpMaintitle"
                                                runat="server"
                                                Cls="big-lbl-title LblElipsis TituloCabecera"
                                                Text="Content Upload Tool"
                                                Flex="8">
                                            </ext:Label>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Toolbar runat="server"
                                        ID="ExpSubtitle"
                                        Hidden="true"
                                        Cls="ExpSubtitle">
                                        <Items>
                                            <ext:Label ID="lblSubtitle"
                                                runat="server"
                                                Cls="subtitle-code"
                                                Text="V.5.1.2.45">
                                            </ext:Label>
                                            <ext:Label ID="lblSubtitleSubcode"
                                                meta:resourceKey="lblSubtitleSubcode"
                                                runat="server"
                                                Cls="subtitle-subcode"
                                                Text="Preproduction Version"
                                                Height="25">
                                            </ext:Label>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Label runat="server"
                                        ID="lblExpDescription"
                                        meta:resourceKey="lblExpDescription"
                                        Cls="ExpDescription"
                                        Text="Procedimiento para subir una plantilla.">
                                    </ext:Label>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                Cls="Expbox"
                                MinHeight="275"
                                MarginSpec="30 20 0 0">
                                <Items>
                                    <ext:Container runat="server"
                                        Cls=""
                                        PaddingSpec="0 0 20 0">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="Label22"
                                                Cls="h4"
                                                Text="1">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>

                                    <ext:Container runat="server"
                                        Cls="ExpTitleBox">
                                        <Items>
                                            <ext:Label MarginSpec=""
                                                runat="server"
                                                ID="ExportTitle"
                                                meta:resourceKey="lblExportTitle"
                                                Cls="ExpDescription, h4"
                                                Text="Download Template">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Component runat="server"
                                        Cls="ExtIcon ico-CntxMenuFileDown">
                                    </ext:Component>
                                    <ext:Label runat="server"
                                        ID="lblExpBoxDescription"
                                        meta:resourceKey="lblExpBoxDescription"
                                        Cls="ExpBoxDescription"
                                        Text="En primer lugar, descarge una plantilla si no dispone de una propia. 
                                                Para realizar la descarga debe seleccionar un proyecto tipo y un tipo de plantilla.">
                                    </ext:Label>
                                    <ext:Button runat="server"
                                        ID="btnDescargarExport"
                                        Cls="btn-ppal btnBold "
                                        Text="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        MarginSpec="35 0 0 0"
                                        Flex="3"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="App.cmbAuxiliar.disable();#{formDownload}.getForm().reset();#{WinConfirmExport}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                Cls="Expbox"
                                MinHeight="275"
                                MarginSpec="30 20 0 0">
                                <Items>
                                    <ext:Container runat="server"
                                        Cls=""
                                        PaddingSpec="0 0 20 0">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="Label23"
                                                Cls="h4"
                                                Text="2">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Cls="ExpTitleBox">
                                        <Items>
                                            <ext:Label MarginSpec=""
                                                runat="server"
                                                ID="ImportTitle"
                                                meta:resourceKey="lblImportTitle"
                                                Cls="ExpDescription, h4"
                                                Text="Fill the Template">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Component runat="server"
                                        Cls="ExtIcon ico-CntxMenuExcelBasic">
                                    </ext:Component>
                                    <ext:Label runat="server"
                                        ID="lblFillBoxDescription"
                                        meta:resourceKey="lblFillBoxDescription"
                                        Cls="ExpBoxDescription"
                                        Text="En segundo lugar, rellene la plantilla descargada con las filas y datos que
                                                mejor se ajusten a sus necesidades.">
                                    </ext:Label>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                Cls="Expbox"
                                MinHeight="275"
                                MarginSpec="30 0 0 0">
                                <Items>
                                    <ext:Container runat="server"
                                        Cls=""
                                        PaddingSpec="0 0 20 0">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="Label24"
                                                Cls="h4"
                                                Text="3">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Cls="ExpTitleBox">
                                        <Items>
                                            <ext:Label MarginSpec=""
                                                runat="server"
                                                ID="lblImpTitle"
                                                meta:resourceKey="lblImpTitle"
                                                Cls="ExpDescription, h4"
                                                Text="Upload Content">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Component runat="server"
                                        Cls="ExtIcon ico-CntxMenuFileUp">
                                    </ext:Component>
                                    <ext:Label runat="server"
                                        ID="lblImpBoxDescription"
                                        meta:resourceKey="lblImpBoxDescription"
                                        Cls="ExpBoxDescription"
                                        Text="Una vez llegado a este punto, solo faltaría subir la plantilla generada.
                                                Para realizar la subida, no hace falta haber pasado por los pasos anteriores 
                                                si ya dispone de una plantilla propia.">
                                    </ext:Label>
                                    <ext:Button runat="server"
                                        ID="btnImportUpload"
                                        meta:resourceKey="btnImportUpload"
                                        Cls="btn-ppal btnBold "
                                        Text="Upload Content"
                                        Focusable="false"
                                        IconCls="ico-gotoPage"
                                        PressedCls="none"
                                        Flex="3"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Fn="abrirGridTemplate" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>

                        </Items>
                    </ext:Container>
                    <%--END CONTENT--%>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
