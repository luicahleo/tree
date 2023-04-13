<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusquedaGlobalContenedor.aspx.cs" Inherits="TreeCore.PaginasComunes.BusquedaGlobalContenedor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!X.IsAjaxRequest)
        {
            this.StoreGridMain.DataSource = this.Data;

            this.grAsR1.Store.Primary.DataSource = TreeCore.PaginasComunes.BusquedaGlobalContenedor.DataGridAsR;
        }


        Store store = this.ComboPagBusq.GetStore();

        store.DataSource = new object[]
        {
            new object[] { "ico-columnas-yesno", "DBI"},
            new object[] { "ico-columnas-yesno", "DBI"},
            new object[] { "ico-columnas-yesno", "DBI"},
            new object[] { "ico-columnas-yesno", "DBI"},
        };

        store.DataBind();




    }

    private object[] Data
    {
        get
        {
            return new object[]
            {
                new object[] { "", 0.5, "License denied","Licencia_denegada", "", 3, 5, "License denied", "RF", "Blocked", "",  "", "Santiponce", "Atrebo", "" },
                new object[] { "",  0.3, "Canceled by client", "Cancelado_cliente", "", 4, 8, "Canceled", "Client", "Blocked", "",  "", "Santiponce", "Atrebo", "icon" },
                new object[] { "",  0.3, "Provisionally denied…", "Licencia_denegada_prov…", "", 1, 2, "Blocked", "RF", "Blocked", "icon",  "", "Santiponce", "icon", "" },
                new object[] { "", 0.4, "Eliminated", "Eliminado", "", 4, 7, "Eliminated", "RF",  "Blocked", "",  "", "Santiponce", "icon", "" },
                new object[] { "icon", 0.1,  "Kick-off", "Carga inicial", "Launched", 5, 7, "Launched", "Infra", "On Air", "",  "icon", "Santiponce", "Atrebo", "" },
                new object[] { "", 1, "Pending request doc…", "Pendiente_solicitar_doc...l", "Pending documentation", 6, 8, "On going", "Infra", "On Air", "icon",  "", "icon", "icon", "" }


            };
        }
    }

</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleBusquedaGlobalContenedor.css" rel="stylesheet" type="text/css" />
    <script src="js/BusquedaGlobalContenedor.js"></script>
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

                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server" Layout="AnchorLayout">
                                <Items>

                                    <ext:Toolbar runat="server" MinHeight="80" Cls="tbNobackground">
                                        <Items>

                                            <ext:TextField meta:resourcekey="txtBusqueda" ID="txtBusqueda" runat="server" EmptyText="Busqueda" Flex="12">

                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="SearchIcoTxtSpec" />
                                                </Triggers>

                                            </ext:TextField>


                                            <ext:ToolbarFill Width="20"></ext:ToolbarFill>
                                            <ext:ComboBox
                                                meta:resourcekey="ComboPagBusq"
                                                ID="ComboPagBusq"
                                                runat="server"
                                                Flex="4"
                                                Editable="false"
                                                DisplayField="name"
                                                ValueField="name"
                                                QueryMode="Local"
                                                TriggerAction="All"
                                                EmptyText="Select Area of Search">

                                                <Listeners>
                                                    <Select Handler="cmbAreaBusquedaAdjust()" />
                                                </Listeners>

                                                <Store>
                                                    <ext:Store runat="server">
                                                        <Model>
                                                            <ext:Model runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="iconCls" />
                                                                    <ext:ModelField Name="name" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                <ListConfig>
                                                    <ItemTpl runat="server">
                                                        <Html>
                                                            <div class="icon-combo-item {iconCls}">
                                                                    {name}
                                                                </div>
                                                        </Html>
                                                    </ItemTpl>
                                                </ListConfig>
                                                <Listeners>
                                                    <Change Handler="if(this.valueCollection.getCount() > 0) {this.setIconCls(this.valueCollection.getAt(0).get('iconCls'));}" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>


                                    <ext:Toolbar runat="server" MinHeight="80" Cls="tbNobackground">
                                        <Items>
                                            <ext:Label meta:resourcekey="lblFiltroBusq" ID="lblFiltroBusq" runat="server" Text="Santiponce" Cls="OrangBack lblFilters lblsearchCol" Height="30" IconCls="ico-close-label" IconAlign="Right"></ext:Label>

                                        </Items>
                                    </ext:Toolbar>

                                    <ext:GridPanel
                                        meta:resourcekey="GridBusqMain"
                                        Hidden="false"
                                        runat="server"
                                        ForceFit="false"
                                        ID="GridBusqMain"
                                        Height="900"
                                        Cls="gridPanel "
                                        
                                        OverflowX="Auto">

                                        <Store>
                                            <ext:Store ID="StoreGridMain" runat="server">
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="Default" />
                                                            <ext:ModelField Name="Average" Type="Float" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Next" />
                                                            <ext:ModelField Name="Amarillo" Type="Float" />
                                                            <ext:ModelField Name="Rojo" Type="Float" />
                                                            <ext:ModelField Name="Group" />
                                                            <ext:ModelField Name="Department" />
                                                            <ext:ModelField Name="GlobalState" />
                                                            <ext:ModelField Name="Parallel" />
                                                            <ext:ModelField Name="Link" />
                                                            <ext:ModelField Name="Town" />
                                                            <ext:ModelField Name="Company" />
                                                            <ext:ModelField Name="Functionalities" />

                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>

                                                <ext:ProgressBarColumn meta:resourcekey="ColPBar" runat="server" DataIndex="Average" Text="" Width="80" Align="Center" ID="ColPBar">
                                                    <Renderer Fn="barGrid"></Renderer>
                                                </ext:ProgressBarColumn>


                                                <ext:WidgetColumn meta:resourcekey="ColDocums" ID="ColDocums" runat="server" Width="60" Cls="col-notes" DataIndex="" OverCls="none" Align="Center" Text="" Hidden="false">
                                                    <Widget>
                                                        <ext:Button runat="server" Width="37" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" TextAlign="Center" Cls="col-Docs-btn" Handler="Ext.Msg.alert('Button clicked', 'BOTON DOCS! ' + this.getWidgetRecord().get('name'));" />
                                                    </Widget>
                                                </ext:WidgetColumn>

                                                <ext:Column meta:resourcekey="ColNombre" ID="ColNombre" runat="server" Text="Name" DataIndex="Name" >
                                                </ext:Column>

                                                <ext:HyperlinkColumn meta:resourcekey="ColCodigo" runat="server" Text="Code" DataIndex="Code" Width="200" ID="ColCodigo">
                                                </ext:HyperlinkColumn>

                                                <ext:Column meta:resourcekey="ColGrupo" runat="server" Text="Proyecto" DataIndex="Group" Width="200" ID="ColGrupo" />

                                                <ext:TemplateColumn meta:resourcekey="ColCoincidencias" ID="ColCoincidencias" runat="server" Flex="4" MinWidth="150" Text="Coincidencias"  DataIndex="" MenuDisabled="true">
                                                    <Template runat="server">
                                                        <Html>
                                                            <div class="coincidencias-wrap">
                                                                <span  class="OrangBack lblsearchCol">{Town}</span>
                                                                <span class="OrangBack lblsearchCol" >{Company}</span>
                                                           </div>
                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>

                                                <ext:WidgetColumn meta:resourcekey="ColMas" ID="ColMas" runat="server" Width="60" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="true" MinWidth="70">
                                                    <Widget>
                                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="btnColMore" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                    </Widget>
                                                </ext:WidgetColumn>

                                                <ext:WidgetColumn meta:resourcekey="ColMostrar" ID="ColMostrar" runat="server" Width="60" Cls="col-More" DataIndex="" Align="Start" Text="" Hidden="false">
                                                    <Widget>
                                                        <ext:Button meta:resourcekey="btnMostrar" runat="server" ID="btnMostrar" Height="40" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColLaunch" Handler="hidePn()" />
                                                    </Widget>
                                                </ext:WidgetColumn>

                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" />
                                        </SelectionModel>

                                        <BottomBar>
                                            <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PGToolBarGrid" MaintainFlex="true" Flex="8">
                                                <Items>
                                                    <ext:ComboBox meta:resourcekey="cmbTbRegistros" runat="server" Cls="comboGrid" ID="cmbTbRegistros" Flex="2">
                                                        <Items>
                                                            <ext:ListItem Text="10 Registros" />
                                                            <ext:ListItem Text="20 Registros" />
                                                            <ext:ListItem Text="30 Registros" />
                                                            <ext:ListItem Text="40 Registros" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="20 Registros" />
                                                        </SelectedItems>

                                                    </ext:ComboBox>

                                                </Items>

                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>


                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server">
                                <Items>
                                    <ext:Label runat="server" Text="ct2"></ext:Label>

                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain3" runat="server">
                                <Items>
                                    <ext:Label runat="server" Text="ct3"></ext:Label>

                                </Items>

                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Panel runat="server" ID="pnAsideR" Hidden="false"
                        Header="false" Border="false" Width="360" MarginSpec="0 -360 0 0">
                        <Items>
                            <ext:Label meta:resourcekey="lblAsideNameR" ID="lblAsideNameR" runat="server" IconCls="ico-head-Notif" Cls="lblHeadAside" Text="Panel Name"></ext:Label>
                            <ext:Panel ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                <Items>
                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false">
                                        <Items>
                                            <ext:Button meta:resourcekey="btnCandidatos" runat="server" ID="btnCandidatos" Cls="btnCandidatos-asR"></ext:Button>
                                            <ext:Button meta:resourcekey="btnActivos" runat="server" ID="btnActivos" Cls="btnActivos-asR"></ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="pnGridsAsideR" runat="server" Border="false" Header="false">
                                        <Items>
                                            <ext:Label meta:resourcekey="lblGrid" ID="lblGrid" runat="server" IconCls="ico-head-Notif" Cls="lblHeadAside" Text="Grid Name"></ext:Label>
                                            <ext:Toolbar ID="tblGrdAsR" runat="server" Cls="tlbGrid tlbGrdAsR">
                                                <Items>
                                                    <ext:Button meta:resourcekey="btnAnadirGrdAsR" runat="server" ID="btnAnadirGrdAsR" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="Añadir"></ext:Button>
                                                    <ext:Button meta:resourcekey="btnEditarGrdAsR" runat="server" ID="btnEditarGrdAsR" Cls="btnEditar" AriaLabel="Editar" ToolTip="Editar"></ext:Button>
                                                    <ext:Button meta:resourcekey="btnEliminarGrdAsR" runat="server" ID="btnEliminarGrdAsR" Cls="btnEliminar" ArialLabel="Eliminar" ToolTip="Eliminar"></ext:Button>
                                                    <ext:Button meta:resourcekey="btnRefrescarGrdAsR" runat="server" ID="btnRefrescarGrdAsR" Cls="btnRefrescar" ArialLabel="Refrescar" ToolTip="Refrescar"></ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Label ID="lblRecordName" runat="server" Cls="lblRecordName" Text="Selected Record Name"></ext:Label>
                                            <ext:GridPanel ID="grAsR1" runat="server"
                                                Width="320"
                                                Height="600"
                                                Cls="grdPnColIcons grdIntoAside"
                                                Border="false">

                                                <Store>
                                                    <ext:Store
                                                        ID="Store6"
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
                                                        <ext:TemplateColumn runat="server" DataIndex="" ID="templateColumn1" MenuDisabled="true" Text="" Flex="1">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
												                        <table class="tmpCol-table">
                                                                          <tr class="tmpCol-tr"> 
																				<td class="tmpCol-td" colspan="3"><div class="d-flx"><span class="userImg">{Ini}</span><div><span class="userName">{Name}</span><span class="d-blk userProfile">{Profile}</span></div></td>
                                                                           </tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Company</span><span class="dataGrd">{Company}</span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Email</span><span class="dataGrd">{Email}</span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="2"><span class="lblGrd">Project</span><span class="dataGrd">{Project}</span></td>
																				<td class="tmpCol-td alg-center"><span class="lblGrd">Authorized</span><span class="dataGrd"><img id="imAuthorized{Id}" onload="renderAuthorized({Authorized}, {Id})" src="../../ima/ico-accept.svg"></img></span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td alg-center col-3"><span class="lblGrd">Staff</span><span class="dataGrd"><img id="imStaff{Id}" onload="renderStaff({Staff}, {Id})" src="../../ima/ico-accept.svg"></img></spa></td>
																				<td class="tmpCol-td alg-center col-3"><span class="lblGrd">Support</span><span class="dataGrd"><img id="imSupport{Id}" onload="renderSupport({Support}, {Id})" src="../../ima/ico-accept.svg"></img></spa></td>
																				<td class="tmpCol-td alg-center col-3"><span class="lblGrd">LDAP</span><span class="dataGrd"><img id="imLDAP{Id}" onload="renderLDAP({LDAP}, {Id})" src="../../ima/ico-accept.svg"></img></spa></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">License</span><span class="dataGrd">{License}</span></td>
																				
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Key Expiration</span><span  class="dataGrd">{KeyExpiration}</span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Last Key</span><span class="dataGrd">{LastKey}</span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Last Access</span><span class="dataGrd">{LastAccess}</span></td>
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
