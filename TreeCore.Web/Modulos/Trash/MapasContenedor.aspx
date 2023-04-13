<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapasContenedor.aspx.cs" Inherits="TreeCore.PaginasComunes.MapasContenedor" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleMapasContenedor.css" rel="stylesheet" type="text/css" />
    <script src="js/MapasContenedor.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">


                <Listeners>
                    <WindowResize Handler="GridResizer()" />
                </Listeners>

            </ext:ResourceManager>
            <ext:Viewport ID="vwResp" runat="server" Cls="vwContenedor">
                <Items>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();"></ext:Button>
                    <ext:Panel runat="server"
                        ID="pnNavVistas"
                        Cls="pnNavVistas"
                        AnchorVertical="15%"
                        AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server"
                                ID="conNavVistas"
                                Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSites"
                                        Handler="showForms(this);"
                                        meta:resourceKey="lnkSites"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="MAPS">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkLocation"
                                        Handler="showForms(this);"
                                        meta:resourceKey="lnkLocation"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 2">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkAdditional"
                                        Handler="showForms(this);"
                                        meta:resourceKey="lnkAdditional"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 3">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkDashboard"
                                        Handler="showForms(this);"
                                        meta:resourceKey="lnkDashboard"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 4">
                                    </ext:HyperlinkButton>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>

                    <ext:Container ID="hugeCt" runat="server" Layout="FitLayout" Cls="hugeMainCt" AnchorVertical="50%">
                        <Loader ID="Loader1"
                            Height="400"
                            runat="server"
                            Url="../PaginasComunes/Mapas.aspx"
                            Mode="Frame">
                            <LoadMask ShowMask="true" />
                        </Loader>

                    </ext:Container>


                    <ext:Panel runat="server" ID="pnAsideR" Hidden="false"
                        Header="false" Border="false" Width="360">
                        <Items>
                            <ext:Label ID="lblAsideNameR" runat="server" IconCls="ico-head-filters" Cls="lblHeadAside" Text="Filters"></ext:Label>
                            <ext:Panel ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                <Items>
                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false">
                                        <Items>
                                            <ext:Button runat="server" ID="btnMyF2" Cls="btnFiltersPlus-asR" Handler="displayMenu('pnCFilters')"></ext:Button>
                                            <ext:Button runat="server" ID="btnMyFilters" Cls="btnMyFilters-asR" Handler="displayMenu('pnGridsAsideMyFilters')"></ext:Button>
                                            <ext:Button runat="server" ID="btnMapFilters" Cls="btnFiltrosMapas-asR" Handler="displayMenu('pnMapFilters')"></ext:Button>
                                        </Items>
                                    </ext:Panel>



                                    <%--CREATE FILTERS PANEL--%>
                                    <ext:Panel runat="server" ID="pnCFilters" Hidden="false">
                                        <Items>
                                            <ext:Label ID="Label2" runat="server" IconCls="btn-CFilter" Cls="lblHeadAside" Text="Create Filter"></ext:Label>
                                            <ext:Panel runat="server" ID="pnCFiltersContainer">
                                                <Items>
                                                    <ext:TextField runat="server"
                                                        ID="pnNewFilter"
                                                        FieldLabel=""
                                                        LabelAlign="Top"
                                                        AllowBlank="false"
                                                        ValidationGroup="FORM"
                                                        CausesValidation="true"
                                                        EmptyText="Filter Name" />
                                                    <ext:Button runat="server" ID="btnFilter" Cls="btn-add" Text="New Filter"></ext:Button>
                                                    <ext:ComboBox runat="server"
                                                        ID="pnField"
                                                        FieldLabel="Field"
                                                        LabelAlign="Top"
                                                        EmptyText="Field"
                                                        Flex="1"
                                                        Cls="pnForm">
                                                        <Items>
                                                            <ext:ListItem runat="server" Text="Example 1"></ext:ListItem>
                                                            <ext:ListItem runat="server" Text="Example 2"></ext:ListItem>
                                                            <ext:ListItem runat="server" Text="Example 3"></ext:ListItem>
                                                        </Items>
                                                    </ext:ComboBox>
                                                    <ext:TextField runat="server"
                                                        ID="pnSearch"
                                                        FieldLabel="Search"
                                                        LabelAlign="Top"
                                                        AllowBlank="false"
                                                        ValidationGroup="FORM"
                                                        CausesValidation="true"
                                                        EmptyText="Depends of field"
                                                        Cls="pnForm" />
                                                    <ext:Button runat="server" ID="btnAdd" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
                                                    <ext:Panel runat="server" ID="tagsHeader">
                                                        <Items>
                                                            <ext:Label runat="server" Cls="tabsLabels" Text="Field"></ext:Label>
                                                            <ext:Label runat="server" Cls="tabsLabels" Text="Search"></ext:Label>
                                                        </Items>
                                                    </ext:Panel>
                                                    <ext:Panel runat="server" ID="pnTagContainer">
                                                        <Items>
                                                            <ext:Panel runat="server" Cls="pntag" ID="pnTags" Hidden="false">
                                                                <Items>
                                                                    <ext:Label runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                    <ext:Label runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                    <ext:Button runat="server" ID="ButtonClose" Cls="btnCloseTag" FocusCls="none"></ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel runat="server" Cls="pntag">
                                                                <Items>
                                                                    <ext:Label runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                    <ext:Label runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                    <ext:Button runat="server" ID="Button2" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel runat="server" Cls="pntag">
                                                                <Items>
                                                                    <ext:Label runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                    <ext:Label runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                    <ext:Button runat="server" ID="Button3" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel runat="server" Cls="pntag">
                                                                <Items>
                                                                    <ext:Label runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                    <ext:Label runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                    <ext:Button runat="server" ID="Button4" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Panel>
                                                    <ext:Button runat="server" ID="Button5" Cls="btn-end" Text="Apply"></ext:Button>
                                                    <ext:Button runat="server" ID="Button6" Cls="btn-save" Text="Save"></ext:Button>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <%--MY FILTERS PANEL--%>
                                    <ext:Panel ID="pnGridsAsideMyFilters" runat="server" Border="false" Header="false" Scrollable="Vertical" OverflowY="Scroll" Hidden="true" Cls="">
                                        <Items>


                                            <ext:Label ID="Label1" runat="server" IconCls="ico-head-my-filters" Cls="lblHeadAside" Text="My Filters"></ext:Label>
                                            <ext:Panel runat="server" ID="pnMFiltersContainer">
                                                <Items>
                                                    <ext:GridPanel
                                                        ID="GridMyFilters"
                                                        runat="server"
                                                        Header="false"
                                                        Border="false"
                                                        Cls="GridMyFilters">
                                                        <Store>
                                                            <ext:Store ID="Store1" runat="server" PageSize="10" AutoLoad="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="ID" />
                                                                            <ext:ModelField Name="Name" />
                                                                            <ext:ModelField Name="Start" Type="Date" />
                                                                            <ext:ModelField Name="End" Type="Date" />
                                                                            <ext:ModelField Name="Completed" Type="Boolean" />
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
                                                                    MinWidth="110"
                                                                    Align="Start">
                                                                </ext:Column>

                                                                <ext:WidgetColumn meta:resourcekey="ColMas" ID="ColMas" runat="server" Width="15" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="btnColMore" Width="16" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="BtnDeleteChk" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                                    </Widget>
                                                                </ext:WidgetColumn>

                                                                <ext:WidgetColumn meta:resourcekey="ColMas" ID="WidgetColumn1" runat="server" Width="18" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button1" Width="18" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="BtnEditChk" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                                    </Widget>
                                                                </ext:WidgetColumn>

                                                                <ext:WidgetColumn meta:resourcekey="ColMas" ID="WidgetColumn2" runat="server" Width="18" Cls="col-Chk" DataIndex="" Align="Center" Hidden="false" MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Checkbox runat="server"
                                                                            ID="Checkbox1"
                                                                            Cls="chkNot chkNewAlign">
                                                                        </ext:Checkbox>
                                                                    </Widget>
                                                                </ext:WidgetColumn>




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


                                    <%--MAP FILTERS PANEL--%>
                                    <ext:Panel ID="pnMapFilters" runat="server" Border="false" Header="false" Scrollable="Vertical" OverflowY="Scroll" Hidden="true" Cls="">
                                        <Items>


                                            <ext:Label ID="Label3" runat="server" IconCls="ico-head-my-filters" Cls="lblHeadAside" Text="Map Filters"></ext:Label>
                                            <ext:FormPanel ID="pnFormFiltersMap" runat="server" Hidden="false" Layout="VBoxLayout">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Center"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Defaults>
                                                    <ext:Parameter Name="width" Value="80%" Mode="Auto" />
                                                </Defaults>
                                                <Items>
                                                    <ext:NumberField runat="server"
                                                        ID="numRadio"
                                                        WidthSpec="80%"
                                                        meta:resourceKey="numRadio"
                                                        FieldLabel="Radio (Km)"
                                                        LabelAlign="Top"
                                                        AllowDecimals="false"
                                                        Number="10"
                                                        MaxValue="25" />
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClusters"
                                                        meta:resourceKey="cmbClusters"
                                                        FieldLabel="Cluster"
                                                        LabelAlign="Top"
                                                        Editable="false">
                                                    </ext:ComboBox>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClientes"
                                                        meta:resourceKey="cmbClientes"
                                                        FieldLabel="Clientes"
                                                        LabelAlign="Top"
                                                        Editable="false">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiOperadores"
                                                        meta:resourceKey="multiOperadores"
                                                        FieldLabel="Operadores"
                                                        LabelAlign="Top"
                                                        QueryMode="Remote"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEstadosGlobales"
                                                        meta:resourceKey="multiEstadosGlobales"
                                                        FieldLabel="Estados Globales"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiCategoriasSitios"
                                                        meta:resourceKey="multiCategoriasSitios"
                                                        FieldLabel="Categorias Sitios"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEmplazamientosTipos"
                                                        meta:resourceKey="multiEmplazamientosTipos"
                                                        FieldLabel="Emplazamientos Tipos"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEmplazamientosTamanos"
                                                        meta:resourceKey="multiEmplazamientosTamanos"
                                                        FieldLabel="Emplazamientos Tamaños"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:Button runat="server"
                                                        ID="btnAplicar"
                                                        Cls="btn-end btnApplyFiltersMap"
                                                        meta:resourceKey="btnAplicar"
                                                        Text="Aplicar">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:FormPanel>


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
