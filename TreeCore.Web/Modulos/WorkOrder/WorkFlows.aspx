<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkFlows.aspx.cs" Inherits="TreeCore.ModWorkFlow.WorkFlows" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="/JS/common.js"></script>
</head>
<body>
    <link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/mainStyle.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />

            <%--FIN HIDDEN --%>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeWorkFlow"
                AutoLoad="true"
                OnReadData="storeWorkFlow_Refresh"
                RemoteFilter="true"
                RemotePaging="true"
                PageSize="20"
                RemoteSort="true">
                <Listeners>
                    <DataChanged Fn="ShowCardsWFBP" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="Active" />
                            <ext:ModelField Name="Public" />
                            <ext:ModelField Name="LinkedStatus" Type="Object" />
                            <ext:ModelField Name="LinkedRoles" Type="Object" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN STORES --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
            </ext:ResourceManager>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server" Region="Center" Layout="FitLayout">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tlbFilter"
                                        Cls="tlbGrid"
                                        Dock="Top">
                                        <Items>
                                            <ext:ToolbarFill />
                                            <ext:TextField
                                                ID="txtFiltroWorkflows"
                                                Cls="txtSearchD"
                                                runat="server"
                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                Width="300"
                                                EnableKeyEvents="true">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search" />
                                                    <ext:FieldTrigger Handler="ClearfilterCompany();" Hidden="true" Icon="clear" />
                                                </Triggers>
                                                <Listeners>
                                                    <Change Handler="App.storeWorkFlow.reload();" Buffer="250" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <BottomBar>
                                    <ext:PagingToolbar runat="server"
                                        ID="PagingToolBar"
                                        meta:resourceKey="PagingToolBar"
                                        StoreID="storeWorkFlow"
                                        OverflowHandler="Scroller"
                                        Cls="bottomBarSites bottomline"
                                        DisplayInfo="true"
                                        HideRefresh="true">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                Cls="comboGrid"
                                                ID="cmbNumRegistros"
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
                                <Items>
                                    <ext:Panel ID="pnViews" runat="server" BodyCls="viewCardsCont">
                                        <Content>
                                            <div id="dtvTarjetas" class="styleContainerGrid"></div>
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
