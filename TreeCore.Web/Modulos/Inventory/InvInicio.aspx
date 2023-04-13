<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvInicio.aspx.cs" Inherits="TreeCore.ModInventario.InvInicio" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="../Inventory/css/styleInvInicio.css" rel="stylesheet" type="text/css" />
    <!--<script type="text/javascript" src="../../Scripts/Chartjs/chart.js"></script>
    <script type="text/javascript" src="../../Scripts/Chartjs/luxon.js"></script>
    <script type="text/javascript" src="../../Scripts/Chartjs/chartjs-adapter-luxon.js"></script>-->
        



    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdTOP_CATEGORIAS" runat="server" />
            <ext:Hidden ID="hdTOP_OPERADORES" runat="server" />
            <ext:Hidden ID="hdTOP_ESTADOS_OPERACIONALES" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                    <DocumentReady Fn="bindParams" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO STORES --%>

            <ext:Store
                runat="server"
                ID="storeInventarioElementosOperadores"
                AutoLoad="false"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                OnReadData="storeInventarioElementosOperadores_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="OperadorID">
                        <Fields>
                            <ext:ModelField Name="OperadorID" Type="Int" />
                            <ext:ModelField Name="Operador" />
                            <ext:ModelField Name="ocurrencies" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioElementoID" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="loadChartOperadores" />
                </Listeners>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeInventarioElementosCategorias"
                AutoLoad="false"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                OnReadData="storeInventarioElementosCategorias_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="CoreInventarioHistoricoID">
                        <Fields>
                            <ext:ModelField Name="CoreInventarioHistoricoID" Type="Int" />
                            <ext:ModelField Name="fecha" />
                            <ext:ModelField Name="InventarioCategoriaID" />
                            <ext:ModelField Name="InventarioCategoria" />
                            <ext:ModelField Name="informacion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioElementoID" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="loadChartCategoria" />
                </Listeners>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeInventarioElementosEstadosOperacionales"
                AutoLoad="false"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                OnReadData="storeInventarioElementosEstadosOperacionales_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="InventarioElementoAtributoEstado">
                        <Fields>
                            <ext:ModelField Name="InventarioElementoAtributoEstado" Type="Int" />
                            <ext:ModelField Name="NombreAtributoEstado" />
                            <ext:ModelField Name="CodigoAtributoEstado" />
                            <ext:ModelField Name="ocurrencies" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioElementoID" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="loadChartEstadosOperacionales" />
                </Listeners>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeInventarioElementosTotales"
                AutoLoad="false"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                OnReadData="storeInventarioElementosTotales_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        runat="server"
                        IDProperty="CoreInventarioHistoricoID">
                        <Fields>
                            <ext:ModelField Name="CoreInventarioHistoricoID" Type="Int" />
                            <ext:ModelField Name="fecha" />
                            <ext:ModelField Name="InventarioCategoriaID" />
                            <ext:ModelField Name="informacion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioElementoID" />
                </Sorters>
                <Listeners>
                    <DataChanged Fn="loadChartTotales" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeOperadoresForm"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeOperadoresForm_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="OperadorID">
                        <Fields>
                            <ext:ModelField Name="OperadorID" />
                            <ext:ModelField Name="Operador" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeCategoriasForm"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCategoriasForm_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" />
                            <ext:ModelField Name="InventarioCategoria" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeEstadosOperacionalesForm"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeEstadosOperacionalesForm_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy Timeout="120000" />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioElementoAtributoEstadoID">
                        <Fields>
                            <ext:ModelField Name="InventarioElementoAtributoEstadoID" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>

            <%--FIN STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="WinConfigCharts"
                Width="300"
                Hidden="true"
                Modal="true">
                <Items>
                    <ext:FormPanel runat="server"
                        BodyPaddingSummary="16 24">
                        <Items>
                            <ext:MultiCombo runat="server"
                                ID="cmbOperadores"
                                LabelAlign="Top"
                                DisplayField="Operador"
                                ValueField="OperadorID"
                                FieldLabel="<%$ Resources:Comun, strOperadores %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                WidthSpec="100%"
                                QueryMode="Local"
                                ValidationGroup="FORM"
                                StoreID="storeOperadoresForm">
                                <Triggers>
                                    <ext:FieldTrigger />
                                </Triggers>
                                <Listeners>
                                    <Change Fn="validateCombosForm" />
                                </Listeners>
                            </ext:MultiCombo>
                            <ext:MultiCombo runat="server"
                                ID="cmbCategorias"
                                LabelAlign="Top"
                                DisplayField="InventarioCategoria"
                                ValueField="InventarioCategoriaID"
                                FieldLabel="<%$ Resources:Comun, strCategorias %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                WidthSpec="100%"
                                QueryMode="Local"
                                ValidationGroup="FORM"
                                StoreID="storeCategoriasForm">
                                <Triggers>
                                    <ext:FieldTrigger />
                                </Triggers>
                                <Listeners>
                                    <Change Fn="validateCombosForm" />
                                </Listeners>
                            </ext:MultiCombo>
                            <ext:MultiCombo runat="server"
                                ID="cmbEstadosOperacionales"
                                LabelAlign="Top"
                                DisplayField="Nombre"
                                ValueField="InventarioElementoAtributoEstadoID"
                                FieldLabel="<%$ Resources:Comun, strEstados %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                AllowBlank="true"
                                WidthSpec="100%"
                                QueryMode="Local"
                                ValidationGroup="FORM"
                                StoreID="storeEstadosOperacionalesForm">
                                <Triggers>
                                    <ext:FieldTrigger />
                                </Triggers>
                                <Listeners>
                                    <Change Fn="validateCombosForm" />
                                </Listeners>
                            </ext:MultiCombo>
                        </Items>
                        <Buttons>
                            <ext:Button runat="server"
                                ID="btnCancelar"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                IconCls="ico-cancel"
                                Cls="btn-cancel">
                                <Listeners>
                                    <Click Handler="#{WinConfigCharts}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardar"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                IconCls="ico-accept"
                                Disabled="false"
                                Cls="btn-accept">
                                <Listeners>
                                    <Click Fn="WinConfigChartsBotonGuardar" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                </Items>
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
                        Scrollable="Vertical">

                        <Items>

                            <ext:Panel runat="server" ID="InvHeader">
                                <Items>
                                    <ext:Image
                                        runat="server"
                                        Src="../../ima/modulos/modInventario.svg"
                                        Height="80"
                                        Width="80"
                                        MarginSpec="0 5 0 0"
                                        Cls="image-title"
                                        Alt="<%$ Resources:Comun, strInventario %>" />
                                    <ext:Label ID="ExpMaintitle"
                                        meta:resourceKey="lblExpMaintitle"
                                        runat="server"
                                        Cls="big-lbl-title LblElipsis TituloCabecera"
                                        Text="<%$ Resources:Comun, strInventario %>"
                                        Flex="8">
                                    </ext:Label>
                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="controles" Cls="alturaPn" BodyCls="alturaPn">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnConfig"
                                        Hidden="true"
                                        MarginSpec="16 0 8"
                                        Cls="cmb-config btnChartsConfig"
                                        IconCls="ico-head-cog-gr btnChartsConfigI">
                                        <Listeners>
                                            <Click Fn="openConfigCharts" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ComboBox
                                        runat="server"
                                        ID="cmbRangoTiempo"
                                        MarginSpec="16 32 8 8"
                                        Cls="cmb-config cmbCharts">
                                        <Items>
                                            <ext:ListItem Text="<%$ Resources:Comun, strUltimoAño %>" Value="-12"></ext:ListItem>
                                            <ext:ListItem Text="<%$ Resources:Comun, strUltimosSeisMeses %>" Value="-6"></ext:ListItem>
                                            <ext:ListItem Text="<%$ Resources:Comun, strUltimoTrimestre %>" Value="-3"></ext:ListItem>
                                            <ext:ListItem Text="<%$ Resources:Comun, strUltimoMes %>" Value="-1"></ext:ListItem>
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Value="-1" />
                                        </SelectedItems>
                                        <Listeners>
                                            <Select Fn="changeCmbRangoTiempo" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox
                                        runat="server"
                                        ID="cmbValoresQueMostrar"
                                        MarginSpec="16 0 8"
                                        Cls="cmb-config cmbCharts">
                                        <Items>
                                        	<ext:ListItem Text="<%$ Resources:Comun, strCreacion %>" Value="CREACION" />
                                        	<ext:ListItem Text="<%$ Resources:Comun, strModificacion %>" Value="MODIFICACION" />
                                        	<ext:ListItem Text="<%$ Resources:Comun, strTotales %>" Value="TOTALES_CATEGORIA" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Value="CREACION" />
                                        </SelectedItems>
                                        <Listeners>
                                            <Select Fn="changeCmbRangoTiempo" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Panel>

                            <ext:Container runat="server"
                                Cls="displayCharts">
                                <Items>
                                    <ext:Panel
                                        runat="server"
                                        ID="pnGraficos1"
                                        Cls="chart-container"
                                        WidthSpec="100%"
                                        Height="330"
                                        BodyCls="color-bg">
                                        <Content>
                                            <div class="">
                                                <%--<asp:Literal runat="server" Text="ASDF" />--%>
                                            </div>
                                            <div id="grid-kpi" class="charts-kpi"></div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel
                                        runat="server"
                                        ID="pnGraficos2"
                                        Cls="chart-container"
                                        WidthSpec="100%"
                                        Height="330">
                                        <Content>
                                            <div class="title">
                                                <asp:Literal runat="server" Text="<%$ Resources:Comun, strElementosPorOperador %>" />
                                            </div>
                                            <div class="chart">
                                                <canvas id="tarta-operadores" class="padCharts" />
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel
                                        runat="server"
                                        ID="pnGraficos3"
                                        Cls="chart-container"
                                        WidthSpec="100%"
                                        Height="330">
                                        <Content>
                                            <div class="title">
                                                <asp:Literal runat="server" Text="<%$ Resources:Comun, strElementosCreados %>" />
                                            </div>
                                            <div class="chart">
                                                <canvas id="lineal-sites" class="padChartsLine" />
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel
                                        runat="server"
                                        ID="pnGraficos4"
                                        Cls="chart-container"
                                        WidthSpec="100%"
                                        Height="330">
                                        <Content>
                                            <div class="title">
                                                <asp:Literal runat="server" Text="<%$ Resources:Comun, strElementosPorEstadoOperacional %>" />
                                            </div>
                                            <div class="chart">
                                                <canvas id="donut-estados-operacionales" class="padCharts" />
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                </Items>
                            </ext:Container>


                            <%--<ext:Panel runat="server"
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
                                                    <Click Handler="#{WinConfirmExport}.show();" />
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
                                    </ext:Panel>--%>
                        </Items>
                    </ext:Container>
                    <%--END CONTENT--%>
                </Items>
            </ext:Viewport>
        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
