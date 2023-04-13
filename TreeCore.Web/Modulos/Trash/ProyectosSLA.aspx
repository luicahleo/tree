<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectosSLA.aspx.cs" Inherits="TreeCore.PaginasComunes.ProyectosSLA" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!X.IsAjaxRequest)
        {

            this.grdSLA.Store.Primary.DataSource = TreeCore.PaginasComunes.ProyectosSLA.DataSLA;
            this.grdTime.Store.Primary.DataSource = TreeCore.PaginasComunes.ProyectosSLA.DataTime;
            this.grdPenalties.Store.Primary.DataSource = TreeCore.PaginasComunes.ProyectosSLA.DataPenalties;
            //this.grNotifications.Store.Primary.DataSource = TreeCore.PaginasComunes.ProyectosSLA.DataNotifications;

        }
    }
</script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <%--<script src="../../JS/LayoutResponsive.js"></script>--%>
    <%--<link href="css/StyleProyectosSLA.css" rel="stylesheet" type="text/css" />--%>
    <script src="js/ProyectosSLA.js"></script>
    <!--<script src="../../JS/common.js"></script>-->

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>
            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Listeners>
                    <AfterRender Handler="ControlSlider()"></AfterRender>
                    <Resize Handler="ControlSlider()"></Resize>
                </Listeners>
                <Items>
                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1200">

                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                        </LayoutConfig>
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="false" Layout="HBoxLayout" Flex="1">
                                <Items>

                                    <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey tbNoborder PosUnsFloatR">
                                        <Items>

                                            <ext:Button runat="server" ID="btnPrevGrid" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="SliderMove('Prev');" Disabled="true"></ext:Button>
                                            <ext:Button runat="server" ID="btnNextGrid" IconCls="ico-next-w" Cls="SliderBtn" Handler="SliderMove('Next');" Disabled="false"></ext:Button>

                                        </Items>
                                    </ext:Toolbar>



                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:GridPanel ID="grdSLA"
                                runat="server"
                                Header="false"
                                Flex="3"
                                MarginSpec="0 10 5 10"
                                HideHeaders="true"
                                Region="Center"
                                Hidden="false"
                                Cls="gridPanel grdNoHeader grdPnColIcons "
                                OverflowX="Hidden"
                                OverflowY="Auto">
                                <Store>
                                    <ext:Store
                                        ID="Store4"
                                        runat="server"
                                        PageSize="50">

                                        <Model>
                                            <ext:Model runat="server" IDProperty="Id">
                                                <Fields>
                                                    <ext:ModelField Name="Id" />
                                                    <ext:ModelField Name="SLAName" />
                                                    <ext:ModelField Name="Typology" />
                                                    <ext:ModelField Name="Default" />
                                                    <ext:ModelField Name="Stop" />
                                                    <ext:ModelField Name="Inactive" />

                                                </Fields>
                                            </ext:Model>
                                        </Model>

                                    </ext:Store>

                                </Store>

                                <ColumnModel runat="server">
                                    <Columns>

                                        <ext:TemplateColumn runat="server" DataIndex="" ID="tempCol1" Text="" Flex="1">

                                            <Template runat="server">
                                                <Html>
                                                    <tpl for=".">
												       <div class="d-flx">
                                                           <img src="../../ima/ico-drag-vertical.svg"></img>
                                                            <ul class="ulGrid"> 
                                                                <li><h5>{SLAName}</h5></li>
                                                                <li><span>Typology:{Typology}</span></li>
                                                            </ul>
                                                            <ul class="ulGrid ulGridCol2"> 
																<li>
																	<div class="ctIcons">
																		<span class="icoGridItem" title="Default"><img id="imDefault{Id}" onload="renderDefault({Default}, {Id})" src="../../ima/ico-default.svg"></img></span>
																		<span class="icoGridItem" title="Stoping"><img id="imStop{Id}" onload="renderStop({Stop}, {Id})" src="../../ima/ico-stop.svg"></img></span>
																		<span class="icoGridItem" title="Inactive"><img id="imInactive{Id}" onload="renderInactive({Inactive}, {Id})" src="../../ima/ico-cancel.svg"></img></span>
																    </div>
														        </li>            
                                                            </ul>
                                                         </div>
												    </tpl>
                                                </Html>
                                            </Template>
                                        </ext:TemplateColumn>
                                    </Columns>
                                </ColumnModel>

                                <View>
                                    <ext:GridView ID="ListaReordenada" runat="server">
                                        <Plugins>
                                            <ext:GridDragDrop runat="server" />
                                        </Plugins>
                                    </ext:GridView>
                                </View>
                                <SelectionModel>
                                    <%--<ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />--%>
                                </SelectionModel>
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid">
                                        <Items>
                                            <ext:Button runat="server" ID="btnAnadir" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="Añadir"></ext:Button>
                                            <ext:Button runat="server" ID="btnEditar" Cls="btnEditar" AriaLabel="Editar" ToolTip="Editar"></ext:Button>
                                            <ext:Button runat="server" ID="btnEliminar" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="Eliminar"></ext:Button>
                                            <ext:Button runat="server" ID="btnRefrescar" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="Refrescar"></ext:Button>
                                            <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="Descargar"></ext:Button>
                                            <ext:Button runat="server" ID="btnDuplicar" Cls="btnDuplicar" AriaLabel="Duplicar Registro" ToolTip="Duplicar Registro"></ext:Button>

                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server" ID="tlbFiltros" Dock="Top">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtSeachBox" Cls="" EmptyText="Search">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search"></ext:FieldTrigger>
                                                    <ext:FieldTrigger Icon="Clear"></ext:FieldTrigger>
                                                </Triggers>
                                            </ext:TextField>

                                            <ext:ComboBox runat="server" ID="cmbEmplazamientos" Cls="comboGrid pos-boxGrid" EmptyText="My Filters">
                                                <Items>
                                                    <ext:ListItem Text="Filtro 1" />
                                                    <ext:ListItem Text="Filtro 2" />
                                                    <ext:ListItem Text="Filtro 3" />
                                                    <ext:ListItem Text="Filtro 4" />
                                                </Items>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                            </ext:GridPanel>

                            <ext:Panel runat="server" ID="pnCol1" Flex="2" Layout="VBoxLayout" BodyCls="tbGrey">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                </LayoutConfig>
                                <Items>
                                    <ext:GridPanel ID="grdTime"
                                        runat="server"
                                        Title="Resolution Time"
                                        Cls="grJustOne gridPanel"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        Hidden="false"
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <Store>
                                            <ext:Store
                                                ID="Store5"
                                                runat="server"
                                                Buffered="true"
                                                RemoteFilter="true"
                                                LeadingBufferZone="1000"
                                                PageSize="50">

                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Id">
                                                        <Fields>
                                                            <ext:ModelField Name="Id" />
                                                            <ext:ModelField Name="TimeLimit" />
                                                            <ext:ModelField Name="ToOk" />
                                                            <ext:ModelField Name="ToWarning" />

                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>

                                        </Store>

                                        <ColumnModel runat="server">
                                            <Columns>

                                                <ext:TemplateColumn runat="server" DataIndex="" ID="TemplateColumn2" MenuDisabled="true" Text="" Flex="1">

                                                    <Template runat="server">
                                                        <Html>

                                                            <tpl for=".">
												                                 <div class="d-flx">
                                                                                      <ul class="ul100w"> 
                                                                                          <li id="liTime" class="d-flx"><span>Time Limit:</span><h5>{TimeLimit}</h5><span id="spDays">days</span></li>
                                                                                         <li id="liToOk" class="d-flx fGreen"><span>Average to Ok</span><h5>{ToOk}%</h5></li>
																						  <li id="liToWarning" class="d-flx fYellow"><span>Average to Warning</span><h5>{ToWarning}%</h5></li>
                                                                                          
                                                                                      </ul>
                                                                                   
                                                                                     
                                                                                   </div>
                               
														</tpl>

                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>
                                            </Columns>
                                        </ColumnModel>

                                        <View>
                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>">
                                            </ext:GridView>
                                        </View>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />
                                        </SelectionModel>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBaseTime" Dock="Top" Cls="tlbGrid">
                                                <Items>
                                                    <ext:RadioGroup runat="server" ID="ctRadioTime" Cls="pos-boxGrid">
                                                        <Items>
                                                            <ext:Radio runat="server" ID="radMonths" BoxLabel="Months" Cls="radioGr"></ext:Radio>
                                                            <ext:Radio runat="server" ID="radDays" BoxLabel="Days" Cls="radioGr"></ext:Radio>
                                                        </Items>
                                                    </ext:RadioGroup>

                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>
                                    </ext:GridPanel>
                                    <ext:GridPanel ID="grdPenalties"
                                        runat="server"
                                        Title="Penalties"
                                        Cls="grJustOne gridPanel"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        Hidden="false"
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <Store>
                                            <ext:Store
                                                ID="Store2"
                                                runat="server"
                                                Buffered="true"
                                                RemoteFilter="true"
                                                LeadingBufferZone="1000"
                                                PageSize="50">

                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Id">
                                                        <Fields>
                                                            <ext:ModelField Name="Id" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Amount" />
                                                            <ext:ModelField Name="Currency" />
                                                            <ext:ModelField Name="Comments" />

                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>

                                        </Store>

                                        <ColumnModel runat="server">
                                            <Columns>

                                                <ext:TemplateColumn runat="server" DataIndex="" ID="TemplateColumn1" MenuDisabled="true" Text="" Flex="1">

                                                    <Template runat="server">
                                                        <Html>

                                                            <tpl for=".">
												                                 <div class="d-flx">
                                                                                      <ul class="ul100w"> 
                                                                                          <li><h5 class="fGrey">{Name}</h5>
																							  <span class="autoH">Amount:{Amount}€</span>
																							  <span class="autoH">Currency:{Currency}</span>
                                                                                          </li>
                                                                                          <li><h5 class="fGrey">Comments</h5>
																							  <p>{Comments}</p>
																							  
                                                                                          </li>
                                                                                          
                                                                                      </ul>
                                                                                   
                                                                                     
                                                                                   </div>
                               
														</tpl>

                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>
                                            </Columns>
                                        </ColumnModel>

                                        <View>
                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>">
                                            </ext:GridView>
                                        </View>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />
                                        </SelectionModel>
                                        <DockedItems>
                                        </DockedItems>
                                    </ext:GridPanel>

                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="pnCol2" Flex="2" Layout="VBoxLayout" BodyCls="tbGrey">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                </LayoutConfig>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="pnNotifications"
                                        Title="Notificacions"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        Hidden="false"
                                        Cls="gridPanel"
                                        OverflowX="Hidden"
                                        OverflowY="Auto">

                                        <Items>
                                            <ext:Toolbar runat="server" ID="tlbGridSlider" Dock="Top">

                                                <Items>
                                                    <ext:ToolbarFill></ext:ToolbarFill>

                                                    <ext:Label runat="server"
                                                        ID="lblToggle"
                                                        Text="Enviar"
                                                        Cls="lblToogle-gr">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        ID="btnEnviar"
                                                        Width="42"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Cls="btn-toggleGrid"
                                                        AriaLabel="Enviar Mensajes"
                                                        ToolTip="Enviar este mesaje" />


                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Panel
                                                ID="pnCardSlider"
                                                runat="server"
                                                Header="false"
                                                Height="250"
                                                Layout="card"
                                                ActiveIndex="0"
                                                Border="false"
                                                Cls="basicPn pnCardSlider">
                                                <Items>
                                                    <ext:Container
                                                        runat="server">
                                                        <Items>
                                                            <ext:Label runat="server" ID="LabelMessage1" Text="Message" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtMessage1" Cls="txtCardSlider" Text="This is the text we’re going to send you. You can image the text is as long as you want or you need."></ext:Label>
                                                            <ext:Button ID="btnEntireMessage" runat="server" Cls="btnMasInfo"></ext:Button>
                                                            <%--Debe aparecer si no se puede mostrar todo el mensaje--%>
                                                            <ext:Label runat="server" ID="LabelRecipients1" Text="Recipients" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtRecipients1" Cls="txtRecipientSlider" Text="pedro.gonzalez@atrebo.com; "></ext:Label>
                                                            <ext:Button ID="btnMoreRecipients" runat="server" Cls="btnMasInfo"></ext:Button>
                                                            <%--Debe aparecer si hay más de un destinatario--%>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container
                                                        runat="server">
                                                        <Items>
                                                            <ext:Label runat="server" ID="LabelMessage2" Text="Message" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtMessage2" Cls="txtCardSlider" Text="This is the text we’re going to send you. You can image the text is as long as you want or you need."></ext:Label>
                                                            <ext:Label runat="server" ID="LabelRecipients2" Text="Recipients" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtRecipients2" Text="pedro.gonzalez@atrebo.com; "></ext:Label>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container
                                                        runat="server">
                                                        <Items>
                                                            <ext:Label runat="server" ID="LabelMessage3" Text="Message" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtMessage3" Cls="txtCardSlider" Text="This is the text we’re going to send you. You can image the text is as long as you want or you need."></ext:Label>
                                                            <ext:Label runat="server" ID="LabelRecipients3" Text="Recipients" Cls="lblCardSlider"></ext:Label>
                                                            <ext:Label runat="server" ID="txtRecipients3" Text="pedro.gonzalez@atrebo.com; "></ext:Label>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                                <Buttons>
                                                    <ext:Button runat="server" ID="btnNxtCardSldr" IconCls="ico-next" Cls="btnNext btnNvCardSldr">
                                                        <DirectEvents>
                                                            <%--<Click OnEvent="Next_Click">
										<ExtraParams>
											<ext:Parameter Name="index" Value="#{WizardPanel}.items.indexOf(#{WizardPanel}.layout.activeItem)" Mode="Raw" />
										</ExtraParams>
									</Click>--%>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnPrvCardSldr" IconCls="ico-prev" Cls="btnPrev btnNvCardSldr" Disabled="true">
                                                        <DirectEvents>
                                                            <%--<Click OnEvent="Prev_Click">
										<ExtraParams>
											<ext:Parameter Name="index" Value="#{WizardPanel}.items.indexOf(#{WizardPanel}.layout.activeItem)" Mode="Raw" />
										</ExtraParams>
									</Click>--%>
                                                        </DirectEvents>
                                                    </ext:Button>

                                                </Buttons>
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
