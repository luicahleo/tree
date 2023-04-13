<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportacionDatosPlantillas.aspx.cs" Inherits="TreeCore.ModExportarImportar.ExportacionDatosPlantillas" %>

<%@ Register Src="~/Componentes/ProgramadorTareasCron.ascx" TagName="Programador" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleExportacionDatosPlantillas.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue.min.js"></script>
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue-i18n.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdTablaseleccionadaID" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdTablaModeloDatoForm" runat="server" />
            <ext:Hidden ID="hdColumnaSeleccionadaID" runat="server" />
            <ext:Hidden ID="hdCeldaAddTransformation" runat="server" />
            <ext:Hidden ID="ReglaRequiereValor" runat="server" />
            <ext:Hidden ID="hdIDPlantillaPreview" runat="server" />
            <ext:Hidden ID="hdQuery" runat="server" />
            <ext:Hidden ID="hdLocale" runat="server" />
            <ext:Hidden ID="hdCeldaID" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoCeldaID" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoRuta" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoCategoria" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoTipo" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoCategoriaOriginal" runat="server" />
            <ext:Hidden ID="hdPermiteEdicion" runat="server" />
            <ext:Hidden ID="hdCeldaIDTextoFijo" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO STORES --%>
            <ext:Store
                runat="server"
                ID="storeExportacionDatosPlantillas"
                AutoLoad="true"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                OnReadData="storeExportacionDatosPlantillas_Refresh"
                shearchBox="txtBuscador"
                listNotPredictive="CronFormat,NombreServicioFrecuencia,ColumnaModeloDatoID,TablaModeloDatosID,UnaVez,Activo,CoreExportacionDatoPlantillaID">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatoPlantillaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatoPlantillaID" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="UnaVez" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="TablaModeloDatosID" />
                            <ext:ModelField Name="ColumnaModeloDatoID" />
                            <ext:ModelField Name="NombreServicioFrecuencia" />
                            <ext:ModelField Name="CronFormat" />
                            <ext:ModelField Name="Filtro" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClaveRecurso" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeTablasModeloDatos"
                AutoLoad="false"
                OnReadData="storeTablasModeloDatos_Refresh"
                RemoteFilter="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="TablaModeloDatosID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="TablaModeloDatosID" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="NombreTabla" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="Controlador" />
                            <ext:ModelField Name="Indice" />
                            <ext:ModelField Name="ModuloID" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClaveRecurso" />
                </Sorters>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeColumnasModeloDatos"
                AutoLoad="false"
                OnReadData="storeColumnasModeloDatos_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ColumnaModeloDatosID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="ID" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Dynamic" Type="Boolean" />
                            <ext:ModelField Name="TypeDynamicID" />
                            <ext:ModelField Name="DataType" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" />
                </Sorters>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeColumnasModeloDatosForm"
                AutoLoad="false"
                OnReadData="storeColumnasModeloDatosForm_Refresh"
                RemoteFilter="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ColumnaModeloDatosID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="ColumnaModeloDatosID" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="NombreColumna" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="TablaModeloDatosID" />
                            <ext:ModelField Name="TipoDatoID" />
                            <ext:ModelField Name="ForeignKeyID" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClaveRecurso" />
                </Sorters>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeFrecuencias"
                AutoLoad="false"
                OnReadData="storeFrecuencias_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreServicioFrecuenciaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreServicioFrecuenciaID" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="CronFormat" />
                            <ext:ModelField Name="FechaInicio" />
                            <ext:ModelField Name="FechaFin" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeReglaTransformacion"
                AutoLoad="false"
                OnReadData="storeReglaTransformacion_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillasReglaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillasReglaID" />
                            <ext:ModelField Name="TipoDatoOperadorID" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="RequiereValor" />
                            <ext:ModelField Name="TipoDato" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeColumnasModelo"
                AutoLoad="false"
                OnReadData="storeColumnasModelo_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillaColumnaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillaColumnaID" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeFilasModelo"
                AutoLoad="false"
                OnReadData="storeFilasModelo_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillaFilaID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillaFilaID" />

                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeCeldas"
                AutoLoad="false"
                OnReadData="storeCeldas_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillasCeldasID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillasCeldasID" />
                            <ext:ModelField Name="ColumnasModeloDatoID" />
                            <ext:ModelField Name="CoreAtributosConfiguracionID" />
                            <ext:ModelField Name="CoreExportacionDatosPlantillaFilaID" />
                            <ext:ModelField Name="CoreExportacionDatosPlantillaColumnaID" />
                            <ext:ModelField Name="TextoFijo" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeColumnaCategoria"
                AutoLoad="false"
                OnReadData="storeColumnaCategoria_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="Id"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Id" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeSelectCamposVinculados"
                AutoLoad="false"
                OnReadData="storeSelectCamposVinculados_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="ID" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="TypeDynamicID" />
                            <ext:ModelField Name="Dynamic" />
                            <ext:ModelField Name="DataType" />
                            <ext:ModelField Name="esCarpeta" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="esCarpeta" Direction="DESC" />
                    <ext:DataSorter Property="Name" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreExportacionDatosPlantillasReglasCeldas"
                AutoLoad="false"
                OnReadData="storeCoreExportacionDatosPlantillasReglasCeldas_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillasReglaCeldaID" runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillasReglaCeldaID" />
                            <ext:ModelField Name="CoreExportacionDatosPlantillasReglaID" />
                            <ext:ModelField Name="ValorIf" />
                            <ext:ModelField Name="ValorCelda" />
                            <ext:ModelField Name="CheckValorDefecto" />
                            <ext:ModelField Name="ValorDefecto" />
                            <ext:ModelField Name="CoreExportacionDatosPlantillasCeldaID" />
                            <%--<ext:ModelField Name="Orden" />--%>
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCoreExportacionDatosPlantillasHistoricos"
                AutoLoad="false"
                OnReadData="storeCoreExportacionDatosPlantillasHistoricos_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="CoreExportacionDatosPlantillaHistoricoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="CoreExportacionDatosPlantillaHistoricoID" />
                            <ext:ModelField Name="CoreExportacionDatosPlantillaID" />
                            <ext:ModelField Name="Archivo" />
                            <ext:ModelField Name="FechaEjecucion" />
                            <ext:ModelField Name="Version" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Version" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <%--FIN STORES --%>

            <%-- CONTEXT MENU --%>
            <ext:Menu
                ID="contextMenuPlantillas"
                runat="server">
                <Items>
                    <ext:MenuItem runat="server"
                        ID="btnMnHistorico"
                        Hidden="false"
                        IconCls="btnTime top0"
                        ToolTip="<%$ Resources:Comun, strHistorico %>"
                        Text="<%$ Resources:Comun, strHistorico %>">
                        <Listeners>
                            <Click Fn="VerHistorico" />
                        </Listeners>
                    </ext:MenuItem>
                </Items>
            </ext:Menu>
            <%-- FIN CONTEXT MENU --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <%--<WindowResize Handler="GridResizer();" />--%>
                    <%--<WindowResize Handler="ColOverrideControl();" />--%>
                </Listeners>
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winAddTemplateDataExport"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                HeightSpec="80vh"
                MaxHeight="600"
                Closable="false"
                Resizable="false"
                Hidden="true"
                Layout="FitLayout"
                Cls="winForm-resp winGestion">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formTemplate"
                        WidthSpec="100%"
                        OverflowY="Auto"
                        Cls="formGris formResp"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="ctForm"
                                MonitorPoll="500"
                                MonitorValid="true"
                                ActiveIndex="2"
                                Border="false"
                                Cls="pnNavVistas pnVistasForm"
                                Height="50"
                                AriaRole="navigation">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="cntNavVistasFormExportar"
                                        Cls="nav-vistas nav-vistasObligatorio ctForm-tab-resp-col3"
                                        ActiveIndex="1"
                                        Height="50"
                                        ActiveItem="1">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormExport"
                                                meta:resourceKey="lnkExport"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="<%$ Resources:Comun, strExportar %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormExportar" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormFrecuencia"
                                                meta:resourceKey="lnkFrecuencia"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="<%$ Resources:Comun, strFrecuencia %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormExportar" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formExport"
                                Hidden="false"
                                HeightSpec="80%"
                                Scrollable="Vertical"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        MaxWidth="350"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>">
                                        <Listeners>
                                            <ValidityChange Fn="ValidarFormulario" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTablasModelosDatos"
                                        StoreID="storeTablasModeloDatos"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        AllowBlank="false"
                                        DisplayField="ClaveRecurso"
                                        ValueField="TablaModeloDatosID"
                                        FieldLabel="<%$ Resources:Comun, strObjeto %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true"
                                        Mode="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarTablaModeloDato" />
                                            <TriggerClick Fn="RecargarTablaModeloDato" />
                                            <ValidityChange Fn="ValidarFormulario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbColumnasModeloDatosForm"
                                        StoreID="storeColumnasModeloDatosForm"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        AllowBlank="true"
                                        DisplayField="ClaveRecurso"
                                        ValueField="ColumnaModeloDatosID"
                                        FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true"
                                        Mode="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarColumnasModeloDatosForm" />
                                            <TriggerClick Fn="RecargarColumnasModeloDatosForm" />
                                            <ValidityChange Fn="ValidarFormulario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTipoFichero"
                                        AllowBlank="false"
                                        FieldLabel="<%$ Resources:Comun, strLabelTipoArchivo %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        Editable="true"
                                        Mode="Local"
                                        QueryMode="Local">
                                        <Items>
                                            <ext:ListItem Text="xlsx" Value="xlsx" />
                                            <ext:ListItem Text="csv" Value="csv" />
                                        </Items>
                                        <Listeners>
                                            <ValidityChange Fn="ValidarFormulario" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <%-- <ext:ComboBox runat="server"
                                        ID="cmbFrecuencias"
                                        StoreID="storeFrecuencias"
                                        LabelAlign="Top"
                                        MaxWidth="350"
                                        AllowBlank="false"
                                        DisplayField="Nombre"
                                        ValueField="CoreServicioFrecuenciaID"
                                        FieldLabel="<%$ Resources:Comun, strFrecuencia %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        WidthSpec="90%"
                                        Editable="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarstoreFrecuencias" />
                                            <TriggerClick Fn="RecargarstoreFrecuencias" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>--%>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="FormFrecuencia"
                                Hidden="true"
                                BodyStyle="padding:10px;"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp">
                                <Items>
                                    <ext:FieldContainer runat="server"
                                        ID="contenedorCategorias"
                                        Cls="containerAttributes ctForm-resp ctForm-content-resp-col3">
                                        <Content>
                                            <local:Programador ID="ProgramadorExportar"
                                                runat="server" />
                                        </Content>
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
                                        ID="btnCancel"
                                        Cls="btn-secondary "
                                        MinWidth="110"
                                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="#{winAddTemplateDataExport}.hide();" />
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
                                            <Click Handler="winAddTemplateDataExportGuardar();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>

            </ext:Window>

            <ext:Window ID="winAddTransformationRule"
                runat="server"
                Title="<%$ Resources:Comun, strElegirCampoVinculado %>"
                Height="500"
                Width="460"
                BodyPaddingSummary="16px 53px"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout"
                Modal="true">
                <Defaults>
                    <ext:Parameter Name="margin"
                        Value="0 0 5 0"
                        Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);">
                    </Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar2"
                        Cls=" greytb"
                        Dock="Bottom">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarRegla"
                                Cls="btn-secondary "
                                MinWidth="110"
                                Text="<%$ Resources:Comun, strCancelar %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winAddTransformationRule}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnAceptarRegla"
                                Cls="btn-ppal "
                                Text="<%$ Resources:Comun, strAceptar %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="winAddTransformationRuleGuardar();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:Container runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="btnValuePorDefecto"
                                Width="41"
                                EnableToggle="true"
                                Cls="btn-toggleGrid FloatR"
                                Hidden="false"
                                Pressed="false"
                                AriaLabel=""
                                ToolTip="<%$ Resources:Comun, jsValorPorDefectoRegla %>"
                                Handler="chkValuePorDefecto();">
                            </ext:Button>
                            <ext:Label runat="server"
                                ID="labelBtnValuePorDefecto"
                                Cls="lblBtnActivo FloatR"
                                PaddingSpec="5px 10px 0 0"
                                Text="<%$ Resources:Comun, jsValorPorDefectoRegla %>">
                            </ext:Label>
                        </Items>
                    </ext:Container>
                    <ext:ComboBox runat="server"
                        ID="cmbRule"
                        StoreID="storeReglaTransformacion"
                        LabelAlign="Top"
                        MaxWidth="350"
                        AllowBlank="false"
                        DisplayField="Nombre"
                        ValueField="CoreExportacionDatosPlantillasReglaID"
                        FieldLabel="<%$ Resources:Comun, strRegla %>"
                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                        WidthSpec="90%"
                        Editable="true"
                        QueryMode="Local">
                        <Listeners>
                            <Select Fn="SeleccionarReglaTransformacion" />
                            <TriggerClick Fn="RecargarReglaTransformacion" />
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                        <Triggers>
                            <ext:FieldTrigger
                                IconCls="ico-reload"
                                Hidden="true"
                                Weight="-1"
                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                        </Triggers>
                    </ext:ComboBox>
                    <ext:TextField runat="server"
                        MarginSpec="10 0 0 0"
                        ID="txtValorRegla"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        FieldLabel="<%$ Resources:Comun, strValor %>"
                        WidthSpec="90%">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:TextField>
                    <ext:DatePicker runat="server"
                        MarginSpec="10 0 0 0"
                        ID="dateValorRegla"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        FieldLabel="<%$ Resources:Comun, strValor %>"
                        WidthSpec="90%"
                        Hidden="true">
                    </ext:DatePicker>
                    <ext:NumberField
                        runat="server"
                        ID="numberValorRegla"
                        MarginSpec="10 0 0 0"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        FieldLabel="<%$ Resources:Comun, strValor %>"
                        WidthSpec="90%"
                        Hidden="true">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:NumberField>
                    <ext:Checkbox runat="server"
                        ID="checkboxValorRegla"
                        MarginSpec="10 0 0 0"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        BoxLabel="<%$ Resources:Comun, strValor %>"
                        WidthSpec="90%"
                        Hidden="true">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:Checkbox>
                    <ext:MultiCombo runat="server"
                        ID="cmbValorRegla"
                        DisplayField="Name"
                        ValueField="ID"
                        FieldLabel="<%$ Resources:Comun, strRegla %>"
                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                        MarginSpec="10 0 0 0"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        WidthSpec="90%"
                        Hidden="true">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                        <Store>
                            <ext:Store
                                ID="storeValorRegla"
                                runat="server"
                                OnReadData="storeValorRegla_Refresh"
                                AutoLoad="false">
                                <Model>
                                    <ext:Model runat="server" IDProperty="ID">
                                        <Fields>
                                            <ext:ModelField Name="Name" />
                                            <ext:ModelField Name="ID" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter Property="Name" />
                                </Sorters>
                                <Listeners>
                                    <DataChanged Fn="cmbValorRegla" />
                                </Listeners>
                            </ext:Store>
                        </Store>
                    </ext:MultiCombo>
                    <ext:TextField runat="server"
                        MarginSpec="10 0 0 0"
                        ID="txtCellValueIs"
                        LabelAlign="Top"
                        AllowBlank="false"
                        MaxWidth="350"
                        FieldLabel="<%$ Resources:Comun, strValorCeldaEs %>"
                        WidthSpec="90%">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField runat="server"
                        MarginSpec="10 0 0 0"
                        ID="txtElseCellValue"
                        LabelAlign="Top"
                        AllowBlank="true"
                        MaxWidth="350"
                        Disabled="true"
                        FieldLabel="<%$ Resources:Comun, strElseValorCeldaEs %>"
                        WidthSpec="90%">
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField runat="server"
                        MarginSpec="10 0 0 0"
                        ID="txtFormatoFecha"
                        Text="dd/MM/yyyy"
                        Hidden="true"
                        LabelAlign="Top"
                        AllowBlank="true"
                        MaxWidth="350"
                        FieldLabel="<%$ Resources:Comun, strFormatoFecha %>"
                        WidthSpec="90%">
                        <ToolTips>
                            <ext:ToolTip runat="server"
                                ID="toltiptxtFormatoFecha"
                                TrackMouse="true"
                                Html="<%$ Resources:Comun, strToolTipFormatoFecha %>">
                            </ext:ToolTip>
                        </ToolTips>
                        <Listeners>
                            <ValidityChange Fn="ValidarFormularioReglasTransformacion" />
                        </Listeners>
                    </ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window runat="server"
                ID="winSelectCampoVinculado"
                Title="<%$ Resources:Comun, strElegirCampoVinculado %>"
                Height="400"
                Width="300"
                Cls="gridPanel winSelectCampoVinculado"
                Hidden="true"
                Resizable="false"
                Layout="VBoxLayout"
                Modal="true">
                <Defaults>
                    <ext:Parameter Name="margin"
                        Value="0 0 5 0"
                        Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);" />
                    <Close Fn="closeWinSelectCampoVinculado" />
                </Listeners>
                <Items>
                    <ext:GridPanel runat="server"
                        ID="gridCamposVinculados"
                        StoreID="storeSelectCamposVinculados"
                        Flex="1"
                        Layout="FitLayout"
                        Cls="gridPanel noBorder"
                        Header="false"
                        Hidden="false"
                        Width="300">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbRuta"
                                Cls="tlbGrid"
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
                            <ext:Toolbar
                                runat="server"
                                ID="tlbCampoVinculadoBuscador"
                                Cls="tlbGrid"
                                Dock="Top">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCampoVinculadoBuscador"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        WidthSpec="100%">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear"
                                                Hidden="true"
                                                Handler="limpiarCampoVinculadoBuscador();" />
                                            <ext:FieldTrigger Icon="Search" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Fn="buscador" />
                                            <Change Fn="FiltrarColumnasCampoVinculadoBuscador" />
                                        </Listeners>
                                    </ext:TextField>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server"
                                    ID="icon"
                                    DataIndex="DataType"
                                    Flex="1"
                                    MaxWidth="50"
                                    Align="Center"
                                    Sortable="false">
                                    <Renderer Fn="RendererIconLinkedField" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colLinkedName"
                                    DataIndex="Name"
                                    Flex="3"
                                    Align="Start"
                                    Sortable="false">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <Listeners>
                            <RowDblClick Fn="LinkedGridDoubleClick" />
                        </Listeners>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters">
                            </ext:GridFilters>
                        </Plugins>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="GridRowSelect"
                                Mode="Multi">
                                <Listeners>
                                    <Select Fn="gridCamposVinculados_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <ext:Window
                runat="server"
                ID="WinTextoFijo"
                Height="100"
                Width="350"
                Header="false"
                Cls="gridPanel winTextoFijo"
                Hidden="true"
                Resizable="false"
                Layout="VBoxLayout"
                Modal="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="FormPanel1"
                        WidthSpec="100%"
                        OverflowY="Auto"
                        Cls="formGris formResp"
                        PaddingSpec="8 16"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtTextoFijo"
                                MarginSpec="10 0 0 0"
                                EmptyText="Type your text here"
                                LabelAlign="Top"
                                AllowBlank="false"
                                MaxWidth="350"
                                WidthSpec="90%">
                                <Listeners>
                                    <ValidityChange Fn="ValidarFormularioTextoFijo" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar3"
                                Cls=" greytb"
                                WidthSpec="100%"
                                Dock="Bottom">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCancelarTextoFijo"
                                        Cls="btn-secondary "
                                        MinWidth="110"
                                        Text="<%$ Resources:Comun, jsCerrar %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="#{WinTextoFijo}.hide();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGuardarTextoFijo"
                                        Cls="btn-ppal "
                                        Text="<%$ Resources:Comun, jsGuardar %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Disabled="true"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="WinTextoFijoGuardar();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Listeners>
                    <AfterRender Handler="winFormResize();"></AfterRender>
                    <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                    <Resize Handler="showPanelsByWindowSize(); resizeCenterPanel();" Buffer="50"></Resize>
                </Listeners>
                <Items>
                    <ext:Panel ID="pnComboGrdVisor"
                        runat="server"
                        Header="false"
                        Cls="col-pdng pnDatosPlantilla"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                BodyCls="bgGrey"
                                Cls="visorInsidePn heigPanel">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Container
                                        runat="server"
                                        ID="WrapAlturaCabecera"
                                        Dock="Top"
                                        Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar
                                                runat="server"
                                                ID="tbTitulo"
                                                Dock="Top"
                                                Cls="tbGrey tbTitleAlignBot tbNoborder"
                                                Hidden="false"
                                                Layout="ColumnLayout"
                                                Flex="1"
                                                MarginSpec="10 0 10 0 ">
                                                <Items>
                                                    <ext:Label
                                                        runat="server"
                                                        ID="lbltituloPrincipal"
                                                        Cls="TituloCabecera"
                                                        Text="<%$ Resources:Comun, strExportacionDatosPlantillas %>" Height="25" />
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                    <ext:Toolbar runat="server"
                                        ID="tbCollapseAsR"
                                        Cls="tbGrey"
                                        Dock="Top"
                                        Hidden="false"
                                        MinHeight="20">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button runat="server"
                                                ID="btnCollapseAsR"
                                                Cls="btn-trans btnCollapseAsRClosedv2"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                                                <Listeners>
                                                    <Click Fn="hidePnFilters" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="true" Layout="HBoxLayout" Flex="1">
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
                                                        Handler="loadPanelByBtns('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="loadPanelByBtns('Next');"
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="ctMain1"
                                        Flex="2"
                                        Layout="FitLayout"
                                        Cls="col colCt1 gridPanel"
                                        Padding="0"
                                        Hidden="false"
                                        MinWidth="330"
                                        MaxWidth="400"
                                        Title="<%$ Resources:Comun, strExportacionDatosPlantillas %>">
                                        <Items>
                                            <ext:GridPanel
                                                Hidden="false"
                                                Cls="gridPanelSimple"
                                                ID="gridTemplates"
                                                runat="server"
                                                Flex="1"
                                                StoreID="storeExportacionDatosPlantillas"
                                                Scrollable="Vertical"
                                                OverflowX="Hidden"
                                                OverflowY="Hidden"
                                                MinWidth="320"
                                                EnableColumnHide="false"
                                                ContextMenuID="contextMenuPlantillas">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        ID="Toolbar1"
                                                        Hidden="false"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                AriaLabel="Añadir"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                Handler="AgregarEditar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="MostrarEditar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                Disabled="true"
                                                                Cls="btnActivar"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                Handler="Activar()">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                Disabled="true"
                                                                Cls="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Handler="Eliminar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnHistorico"
                                                                Disabled="true"
                                                                Hidden="true"
                                                                Cls="btnTime"
                                                                ToolTip="<%$ Resources:Comun, strHistorico %>">
                                                                <Listeners>
                                                                    <Click Fn="VerHistorico" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Handler="Refrescar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Hidden="true"
                                                                Handler="ExportarDatos('ExportacionDatosPlantillas', hdCliID.value, #{gridTemplates}, App.btnActivos.pressed, 'EXPORTAR', App.hdStringBuscador.value);">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnActivos"
                                                                Width="41"
                                                                EnableToggle="true"
                                                                Cls="btn-toggleGrid"
                                                                Hidden="false"
                                                                Pressed="true"
                                                                AriaLabel=""
                                                                ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                                Handler="btnActivos();">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        ID="tlbFiltros"
                                                        Hidden="false"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtBuscador"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                WidthSpec="100%">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Clear"
                                                                        Hidden="true"
                                                                        Handler="limpiar();" />
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Fn="buscador" />
                                                                    <Change Fn="FiltrarColumnas" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            DataIndex="Activo"
                                                            ID="colActivo"
                                                            Hidden="true"
                                                            Width="40"
                                                            Text="<%$ Resources:Comun, strActivo %>">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colNombre"
                                                            DataIndex="Nombre"
                                                            Flex="1"
                                                            Text="<%$ Resources:Comun, strPlantilla %>">
                                                            <Renderer Fn="PlantillaTemplateColumn" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colClaveRecurso"
                                                            DataIndex="ClaveRecurso"
                                                            Text="<%$ Resources:Comun, strObjetoNegocio %>"
                                                            Hidden="true"
                                                            Width="40">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectTemplate"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFiltersTemplate"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        StoreID="storeExportacionDatosPlantillas"
                                                        ID="PagingToolbar"
                                                        MaintainFlex="true"
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

                                    <ext:Panel runat="server"
                                        ID="ctMain2"
                                        Flex="4"
                                        Layout="FitLayout"
                                        Cls="col colCt2 grdNoHeader"
                                        Hidden="false">
                                        <Items>
                                            <ext:Panel ID="PanelVisorMain"
                                                runat="server"
                                                Header="false"
                                                Border="false"
                                                Layout="FitLayout"
                                                Cls="visorInsidePn">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbGrid"
                                                        Dock="Top"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                ToolTip="">
                                                                <Listeners>
                                                                    <Click Fn="showOnlySecundary" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lblTabs"
                                                                Cls="HeaderLblVisor"
                                                                Text="">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server"
                                                                ID="btnShowFilter"
                                                                IconCls="btnFiltros"
                                                                Cls="btnFiltros"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, jsFiltros %>">
                                                                <Listeners>
                                                                    <Click Fn="btnShowFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPreview"
                                                                Cls="btnVisual"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, jsVistaPrevia %>">
                                                                <Listeners>
                                                                    <Click Fn="btnPreview" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:Panel runat="server" ID="tagsContainerTemplate" Layout="ColumnLayout" MaxHeight="35"
                                                                Padding="5">
                                                                <Items>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnConfiguration"
                                                        MarginSpec="0 5"
                                                        OverflowY="Auto"
                                                        OverflowX="Auto">
                                                        <Content>
                                                            <div id="containerConfigTemplate" class="containerConfigTemplate">
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnPreview"
                                                        Cls="pnPreviewIE"
                                                        MarginSpec="0 5"
                                                        Hidden="true">
                                                        <Items>
                                                            <ext:GridPanel
                                                                runat="server"
                                                                ID="grdPreview"
                                                                Cls="gridPreviewIE"
                                                                EnableColumnHide="false"
                                                                EnableColumnMove="false"
                                                                OverflowY="Auto"
                                                                OverflowX="Auto"
                                                                Scrollable="Vertical">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storegrdPreview"
                                                                        AutoLoad="false"
                                                                        RemotePaging="false"
                                                                        RemoteFilter="false"
                                                                        RemoteSort="false"
                                                                        PageSize="20">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="PreviewID"></ext:Model>
                                                                        </Model>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel runat="server"></ColumnModel>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Header="false"
                                Border="false"
                                Width="380"
                                Hidden="false">
                                <Listeners>
                                    <AfterRender Handler="ResizerAside(this)"></AfterRender>
                                </Listeners>
                                <DockedItems>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameR"
                                        runat="server"
                                        IconCls="ico-head-filters"
                                        Cls="lblHeadAsideDock"
                                        Text="<%$ Resources:Comun, strFiltros %>">
                                    </ext:Label>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameInfo"
                                        runat="server"
                                        IconCls="ico-head-info"
                                        Cls="lblHeadAsideDock"
                                        Hidden="true"
                                        Text="<%$ Resources:Comun, strAtributos %>">
                                    </ext:Label>
                                </DockedItems>
                                <Items>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideR"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyF2"
                                                        ID="btnMyF2"
                                                        Cls="btnFiltersPlus-asR"
                                                        ToolTip="<%$ Resources:Comun, strCrearFiltro %>"
                                                        Handler="displayMenuInventary('pnCFilters')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANELS--%>

                                            <ext:Panel
                                                ID="pnGridsAside"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Layout="AnchorLayout">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <%--CREATE FILTERS PANEL--%>
                                                    <ext:Panel
                                                        AnchorVertical="100%"
                                                        AnchorHorizontal="100%"
                                                        Hidden="true"
                                                        runat="server"
                                                        PaddingSpec="15 20 20 15"
                                                        Layout="VBoxLayout"
                                                        Cls="Whitebg"
                                                        ID="pnCFilters">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 0"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="lblGrid"
                                                                runat="server"
                                                                IconCls="btn-CFilter"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Stretch" />
                                                        </LayoutConfig>
                                                        <Items>
                                                            <ext:Container runat="server" ID="fila2F" MarginSpec="10 0 0 0">
                                                                <Items>
                                                                    <ext:ComboBox runat="server"
                                                                        meta:resourcekey="cmbField"
                                                                        MarginSpec="0 6 0 0"
                                                                        ID="cmbField"
                                                                        FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                        Flex="1"
                                                                        WidthSpec="100%"
                                                                        Cls="pnForm fieldFilter"
                                                                        ValueField="Campo">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeCampos"
                                                                                runat="server"
                                                                                OnReadData="storeCampos_Refresh"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Campo">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Campo" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                            <ext:ModelField Name="TypeData" />
                                                                                            <ext:ModelField Name="TipoCampo" />
                                                                                            <ext:ModelField Name="QueryValores" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Sorters>
                                                                                    <ext:DataSorter Property="Name" />
                                                                                </Sorters>
                                                                                <Listeners>
                                                                                    <DataChanged Fn="beforeLoadCmbField" />
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Select Fn="selectField" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:TextField runat="server"
                                                                        Flex="1"
                                                                        WidthSpec="100%"
                                                                        meta:resourcekey="pnSearch"
                                                                        ID="textInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                        Cls="pnForm"
                                                                        Hidden="false" />
                                                                    <ext:DateField
                                                                        runat="server"
                                                                        ID="dateInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        WidthSpec="100%"
                                                                        Hidden="true"
                                                                        Format="dd/MM/yyyy">
                                                                    </ext:DateField>
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="numberInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        WidthSpec="100%"
                                                                        Hidden="true">
                                                                    </ext:NumberField>
                                                                    <ext:ComboBox
                                                                        ID="cmbOperatorField"
                                                                        runat="server"
                                                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                        Cls="pnForm"
                                                                        Flex="2"
                                                                        WidthSpec="70%"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        QueryMode="Local">
                                                                        <Items>
                                                                            <ext:ListItem Text="=" Value="IGUAL" />
                                                                            <ext:ListItem Text="<" Value="MENOR" />
                                                                            <ext:ListItem Text=">" Value="MAYOR" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                    <ext:MultiCombo runat="server"
                                                                        Hidden="true"
                                                                        ID="cmbTiposDinamicos"
                                                                        FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        Flex="1"
                                                                        WidthSpec="100%"
                                                                        Cls="pnForm"
                                                                        ValueField="ID"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeTiposDinamicos"
                                                                                runat="server"
                                                                                AutoLoad="false"
                                                                                OnReadData="storeTiposDinamicos_Refresh">
                                                                                <Proxy>
                                                                                    <ext:PageProxy />
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ID" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Sorters>
                                                                                    <ext:DataSorter Property="Name" />
                                                                                </Sorters>
                                                                                <Listeners>
                                                                                    <%--<DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:MultiCombo>
                                                                    <ext:Checkbox runat="server"
                                                                        ID="chkTiposDinamicos"
                                                                        FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        Hidden="true">
                                                                    </ext:Checkbox>
                                                                    <ext:Component runat="server" Flex="1"></ext:Component>
                                                                    <ext:Button
                                                                        runat="server" meta:resourcekey="ColMas"
                                                                        ID="btnAdd"
                                                                        MarginSpec="6 0 0 0"
                                                                        Flex="1"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        Text="<%$ Resources:Comun, jsAgregar %>">
                                                                        <Listeners>
                                                                            <Click Fn="addElementFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:GridPanel
                                                                ID="GridCrearFiltros"
                                                                runat="server"
                                                                Header="false"
                                                                Border="false"
                                                                Cls="GridCreateFilters gridFilter"
                                                                Scrollable="Vertical"
                                                                Height="180">
                                                                <DockedItems>
                                                                    <ext:Toolbar runat="server" ID="tbGridCrearFiltros" Dock="Bottom" MarginSpec="0 -8 0 0">
                                                                        <Items>
                                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                                            <ext:Button
                                                                                runat="server"
                                                                                meta:resourcekey="btnSaveFilter"
                                                                                ID="btnSaveFilter"
                                                                                Cls="btn-end"
                                                                                Enabled="false"
                                                                                Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                                <Listeners>
                                                                                    <Click Fn="saveFilter" />
                                                                                </Listeners>
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </DockedItems>
                                                                <Store>
                                                                    <ext:Store
                                                                        runat="server"
                                                                        PageSize="10"
                                                                        ID="storeFiltros"
                                                                        AutoLoad="false">
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="Campo">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="Name" />
                                                                                    <ext:ModelField Name="Campo" />
                                                                                    <ext:ModelField Name="Value" />
                                                                                    <ext:ModelField Name="DisplayValue" />
                                                                                    <ext:ModelField Name="TypeData" />
                                                                                    <ext:ModelField Name="Operador" />
                                                                                    <ext:ModelField Name="TipoCampo" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel runat="server">
                                                                    <Columns>
                                                                        <ext:Column runat="server"
                                                                            Sortable="true"
                                                                            DataIndex="Name"
                                                                            TdCls="firstColumnFilter"
                                                                            Text="Field"
                                                                            Flex="1"
                                                                            Align="Start">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            Sortable="true"
                                                                            Text="Search"
                                                                            TdCls="secondColumnFilter"
                                                                            Flex="1"
                                                                            DataIndex="DisplayValue"
                                                                            Align="Start">
                                                                        </ext:Column>

                                                                        <ext:CommandColumn ID="colEliminarFiltro" TdCls="closeColumnFilter" runat="server" Width="50" Align="Center">
                                                                            <Commands>
                                                                                <ext:GridCommand CommandName="EliminarFiltro" IconCls="ico-close">
                                                                                </ext:GridCommand>
                                                                            </Commands>
                                                                            <Listeners>
                                                                                <Command Fn="EliminarFiltro" />
                                                                            </Listeners>
                                                                        </ext:CommandColumn>
                                                                    </Columns>
                                                                </ColumnModel>
                                                                <View>
                                                                    <ext:GridView runat="server" LoadMask="false" />
                                                                </View>
                                                                <Plugins>
                                                                    <ext:GridFilters runat="server" />
                                                                </Plugins>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>

                                        </Items>
                                    </ext:Panel>

                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="pnAsideR2"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Header="false"
                                Border="false"
                                Width="380"
                                Hidden="false">
                                <Listeners>
                                    <AfterRender Handler="ResizerAside(this)"></AfterRender>
                                </Listeners>
                                <DockedItems>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameR2"
                                        runat="server"
                                        IconCls="btnDataGreen"
                                        Cls="lblHeadAsideDock"
                                        Text="DATA EXPORT TEMPLATE">
                                    </ext:Label>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameInfo2"
                                        runat="server"
                                        IconCls="ico-head-info"
                                        Cls="lblHeadAsideDock"
                                        Hidden="true"
                                        Text="<%$ Resources:Comun, strAtributos %>">
                                    </ext:Label>
                                </DockedItems>
                                <Items>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideR2"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel
                                                ID="mnAsideR2"
                                                runat="server"
                                                Border="false"
                                                Header="false"
                                                AnchorVertical="100%"
                                                AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyF2"
                                                        ID="Button1"
                                                        Cls="btnHistorial-asR"
                                                        ToolTip="<%$ Resources:Comun, strHistorico %>"
                                                        Handler="displayMenuInventary('pnCFilters')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANELS--%>

                                            <ext:Panel
                                                ID="pnGridsAside2"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Layout="AnchorLayout">
                                                <Listeners>
                                                </Listeners>
                                                <Items>

                                                    <%-- HISTORICO --%>
                                                    <ext:Panel runat="server"
                                                        AnchorVertical="100%"
                                                        AnchorHorizontal="100%"
                                                        Hidden="true"
                                                        BodyPaddingSummary="5 20 20 15"
                                                        Layout="VBoxLayout"
                                                        Cls="Whitebg"
                                                        OverflowY="Auto"
                                                        ID="pnHistorico">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 0"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="lblGrid2"
                                                                runat="server"
                                                                IconCls="btnHistorygreen"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strHistorico %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Stretch" />
                                                        </LayoutConfig>
                                                        <Items>
                                                            <ext:GridPanel runat="server"
                                                                ID="grdHistorico"
                                                                Cls="gridHistoricoIE"
                                                                StoreID="storeCoreExportacionDatosPlantillasHistoricos">
                                                                <ColumnModel>
                                                                    <Columns>
                                                                        <ext:Column
                                                                            runat="server"
                                                                            ID="colFechaEjecucion"
                                                                            DataIndex=""
                                                                            MaxWidth="193"
                                                                            Width="193"
                                                                            FocusCls="none"
                                                                            Flex="6">
                                                                            <Renderer Fn="HistoricoTemplate" />
                                                                        </ext:Column>
                                                                        <ext:ComponentColumn
                                                                            runat="server"
                                                                            ID="colbtnDownload"
                                                                            Cls=""
                                                                            MaxWidth="50"
                                                                            Flex="1">
                                                                            <Component>
                                                                                <ext:Button runat="server"
                                                                                    ID="btnDownloadHistory"
                                                                                    Cls="btnDescargar noBorder-background"
                                                                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                                                                                    <Listeners>
                                                                                        <Click Fn="DownloadHistory" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Component>
                                                                        </ext:ComponentColumn>
                                                                        <ext:ComponentColumn
                                                                            runat="server"
                                                                            ID="colbtnDesactivar"
                                                                            MaxWidth="50"
                                                                            Flex="1">
                                                                            <Component>
                                                                                <ext:Button
                                                                                    runat="server"
                                                                                    ID="btnDesactivarHistory"
                                                                                    Cls="btnNoDoc16p noBorder-background"
                                                                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>">
                                                                                    <Listeners>
                                                                                        <Click Fn="DeactivateHistory" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Component>
                                                                        </ext:ComponentColumn>
                                                                    </Columns>
                                                                </ColumnModel>
                                                            </ext:GridPanel>
                                                        </Items>
                                                        <Listeners>
                                                            <AfterLayout Fn="renderHistorico" />
                                                        </Listeners>
                                                    </ext:Panel>
                                                    <%-- END HISTORICO --%>
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
        <ext:ToolTip runat="server"
            Target="={#{gridTemplates}.getView().el}"
            Delegate=".x-grid-cell"
            TrackMouse="true">
            <Listeners>
                <Show Handler="onShow(this, #{gridTemplates});" />
            </Listeners>
        </ext:ToolTip>
    </form>
</body>
</html>
