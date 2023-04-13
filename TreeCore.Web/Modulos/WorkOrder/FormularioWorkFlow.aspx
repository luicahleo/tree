<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormularioWorkFlow.aspx.cs" Inherits="TreeCore.ModWorkOrders.pages.FormularioWorkFlow" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleFormularioWorkFlow.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery.amsify.suggestags.js"></script>
    <form id="form1" runat="server">
        <div>

            <ext:Hidden runat="server" ID="hdObjeto" />
            <ext:Hidden runat="server" ID="hdWFCode" />
            <ext:Hidden runat="server" ID="hdWFStatusCode" />

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>

            <ext:Menu runat="server"
                ID="mnOpciones">
                <Items>
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsEditar %>"
                        ID="mnoEditar"
                        Handler="ShowConfStatus(hdWFStatusCode.value)" />
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsActivar %>"
                        ID="mnoActivar"
                        Handler="ActivarStatus(hdWFStatusCode)" />
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsEliminar %>"
                        ID="mnoEliminar"
                        Cls="spanRedV2"
                        Handler="DeleteStatus(hdWFStatusCode)" />
                </Items>
            </ext:Menu>

            <ext:Store ID="storeRoles" runat="server" AutoLoad="false" OnReadData="storeRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Code" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeStatusGroup" runat="server" AutoLoad="false" OnReadData="storeStatusGroup_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
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

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnWorkFlow"
                                Cls="pnLateralForm"
                                Collapsed="false"
                                CollapseDirection="Right"
                                CollapseMode="Header"
                                Collapsible="true"
                                Header="false"
                                Hidden="false"
                                Layout="FitLayout"
                                PaddingSpec="15 25"
                                Region="West"
                                Width="350">
                                <DockedItems>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:Label runat="server" Text="<%$ Resources:Comun, strInfoWorkflow %>" Cls="panelTitle"></ext:Label>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="pnConfWorkFlow" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowConfWorkFlow" />
                                            <Hide Fn="HideConfWorkFlow" />
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Bottom">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        ID="btnAddWF"
                                                        Cls="btnSecundary"
                                                        Disabled="true"
                                                        Width="100"
                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                        <Listeners>
                                                            <Click Fn="ApplyRolWF" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server"
                                                ID="pnFormWF"
                                                Cls="seccion"
                                                Title="<%$ Resources:Comun, strGeneral %>">
                                                <Items>
                                                    <ext:TextField runat="server"
                                                        ID="txtNameWF"
                                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                        AllowBlank="false"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="WorkFlowValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        AllowBlank="false"
                                                        ID="txtCodeWF"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="WorkFlowValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextArea runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                        AllowBlank="false"
                                                        ID="txtDescriptionWF"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="WorkFlowValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextArea>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="seccion content100" Title="<%$ Resources:Comun, strRoles %>">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server" ID="btnPublicWF" Text="<%$ Resources:Comun, jsPublico %>" Cls="btnPublic" EnableToggle="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWF" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnPrivateWF" Text="<%$ Resources:Comun, jsPrivado %>" Cls="btnPrivate" EnableToggle="true" Pressed="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWF" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <ext:Label runat="server"
                                                        ID="lblPublicRole"
                                                        Text="#Add who is restricted from reading"
                                                        Cls="lbTitleRoles" />
                                                    <ext:Label runat="server"
                                                        ID="lblPrivateRole"
                                                        Text="#Add who can read"
                                                        Cls="lbTitleRoles" />
                                                    <div id="contRolesWF">
                                                        <input type="text" class="form-control" name="inputRolesWF">
                                                    </div>
                                                </Content>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="pnSumWorkFlow" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowSumWorkFlow" />
                                            <Hide Fn="HideSumWorkFlow" />
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tblEditWF" Dock="Bottom" Hidden="true">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnSecundary"
                                                        Width="100"
                                                        Text="<%$ Resources:Comun, jsEditar %>">
                                                        <Listeners>
                                                            <Click Fn="EditWF" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" ID="tblSubmitWF" Dock="Bottom" Hidden="true">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        Cls="btnSecundary"
                                                        IconCls="btnNext-blue"
                                                        Width="34">
                                                        <Listeners>
                                                            <Click Fn="EditWF" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnSave me-2"
                                                        Width="120"
                                                        Text="<%$ Resources:Comun, strSubmit %>">
                                                        <Listeners>
                                                            <Click Fn="SubmitWF" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server" Cls="seccion" Title="<%$ Resources:Comun, jsResumen %>">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" Dock="Top" Layout="FitLayout" Height="200">
                                                        <Items>
                                                            <ext:Container runat="server">
                                                                <Content>
                                                                    <div id="dtvTarjeta" class=""></div>
                                                                </Content>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Label ID="lbTitleWF" Cls="lbTitleWF" runat="server"></ext:Label>
                                                    <ext:Label ID="lbDescriptionWF" Cls="lbDescriptionWF" runat="server"></ext:Label>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" BodyCls="bgColor" Region="Center" Layout="FitLayout">
                                <Items>
                                    <ext:Panel ID="pnDiagram" runat="server" BodyCls="viewCardsCont">
                                        <Listeners>
                                            <AfterRender Fn="ShowCardsStatus" />
                                        </Listeners>
                                        <Content>
                                            <div id="dtvTarjetasStatus" class="styleContainerGrid"></div>
                                        </Content>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                ID="pnStatus"
                                Cls="pnLateralForm"
                                Collapsed="true"
                                CollapseDirection="Left"
                                CollapseMode="Header"
                                Collapsible="true"
                                Header="false"
                                Layout="FitLayout"
                                PaddingSpec="15 25"
                                Region="East"
                                Width="350">
                                <DockedItems>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:Label runat="server" Text="<%$ Resources:Comun, strInfoEstado %>" Cls="panelTitle"></ext:Label>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="pnConfStatus" Hidden="true">
                                        <Listeners>
                                            <%--<Show Handler="ShowConfStatus();" />--%>
                                            <Hide Fn="HideConfStatus" />
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Bottom">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnSecundary"
                                                        Width="100"
                                                        Text="<%$ Resources:Comun, strSiguiente %>">
                                                        <Listeners>
                                                            <Click Fn="EditRoles" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server" Cls="seccion" Title="<%$ Resources:Comun, strGeneral %>">
                                                <Items>
                                                    <ext:TextField runat="server"
                                                        ID="txtStatusName"
                                                        FieldLabel="<%$ Resources:Comun, strEstadoNombre %>"
                                                        AllowBlank="false"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField runat="server"
                                                        ID="txtStatusCode"
                                                        FieldLabel="<%$ Resources:Comun, strEstadoCodigo %>"
                                                        AllowBlank="false"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbStatusGroup"
                                                        FieldLabel="<%$ Resources:Comun, strGrupoEstado %>"
                                                        AllowBlank="false"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        StoreID="storeStatusGroup"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <Listeners>
                                                            <Select Fn="" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <%--<ext:TextField runat="server"
                                                        ID=""
                                                        
                                                        AllowBlank="false"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>--%>
                                                    <ext:TextArea runat="server"
                                                        ID="txtStatusDescription"
                                                        FieldLabel="<%$ Resources:Comun, strEstadoDescripcion %>"
                                                        LabelAlign="Top"
                                                        WidthSpec="100%">
                                                    </ext:TextArea>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbTime"
                                                        FieldLabel=""
                                                        LabelAlign="Top"
                                                        WidthSpec="100%">
                                                        <Items>
                                                            <ext:ListItem Text="<%$ Resources:Comun, jsHoras %>" Value="horas" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strDias %>" Value="dias" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, jsSemanas %>" Value="semanas" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="horas" />
                                                        </SelectedItems>
                                                    </ext:ComboBox>
                                                    <ext:NumberField runat="server"
                                                        ID="txtTimeDuration"
                                                        FieldLabel="<%$ Resources:Comun, strDuracionEstimada %>"
                                                        LabelAlign="Top"
                                                        AllowBlank="false"
                                                        WidthSpec="100%"
                                                        MinValue="0">
                                                    </ext:NumberField>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="pnConfRoles" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowConfRoles" />
                                            <Hide Fn="HideConfRoles" />
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tblEditRoles" Dock="Bottom">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        Cls="btnSecundary"
                                                        IconCls="btnNext-blue"
                                                        Width="34">
                                                        <Listeners>
                                                            <Click Fn="EditStatus" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnSecundary"
                                                        Width="100"
                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                        <Listeners>
                                                            <Click Fn="AddStatus" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server" Cls="seccion content100" Title="<%$ Resources:Comun, strRoles %>" MarginSpec="0 0 2rem">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server" ID="btnPublicRead" Text="<%$ Resources:Comun, jsPublico %>" Cls="btnPublic" EnableToggle="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWFRead" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnPrivateRead" Text="<%$ Resources:Comun, jsPrivado %>" Cls="btnPrivate" EnableToggle="true" Pressed="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWFRead" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <ext:Label runat="server" Text="#Add  who is restricted from reading" Cls="lbTitleRoles" />
                                                    <div id="contRolesWFRead">
                                                        <input type="text" class="form-control" name="inputRolesWFRead">
                                                    </div>
                                                </Content>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="seccion content100">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server" ID="btnPublicWrite" Text="<%$ Resources:Comun, jsPublico %>" Cls="btnPublic" EnableToggle="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWFWrite" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnPrivateWrite" Text="<%$ Resources:Comun, jsPrivado %>" Cls="btnPrivate" EnableToggle="true" Pressed="true">
                                                                <Listeners>
                                                                    <Click Fn="ControlRolWFWrite" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <ext:Label runat="server" Text="#Add  who is restricted from Editing" Cls="lbTitleRoles" />
                                                    <div id="contRolesWFWrite">
                                                        <input type="text" class="form-control" name="inputRolesWFWrite">
                                                    </div>
                                                </Content>
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
