<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioGraphicView.aspx.cs" Inherits="TreeCore.ModInventario.pages.InventarioGraphicView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleInventarioGraphicView.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <script type="text/javascript" src="js/InventarioGraphicView.js"></script>
    <script type="text/javascript">


</script>

</head>
<body>



    <form id="form1" runat="server">
        <div>
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">

                <Listeners>
                    <WindowResize Handler="GridResizer();" />

                </Listeners>

            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <ext:ToolTip
                ID="ContentAnchorTip"
                runat="server"
                Target="btnTestPopup"
                Title="<a href='#'>Rich Content Tooltip</a>"
                Anchor="top"
                Width="200"
                AutoHide="true"
                Closable="true">
                <Content>
                    <div id="content-tip">
                        <ul>
                            <li>5 bedrooms</li>
                            <li>2 bathrooms</li>
                            <li>Large backyard</li>
                            <li>Close to transport</li>
                        </ul>
                        <div class="x-clear"></div>
                    </div>
                </Content>
                <Listeners>
                    <Render Handler="this.header.on('click', function (e) { Ext.EventObject.stopEvent(); Ext.Msg.alert('Link', 'Link to something interesting.');App.ContentAnchorTip.hide(); }, this, {delegate:'a'});" />
                </Listeners>
            </ext:ToolTip>


            <ext:Viewport runat="server" ID="MainVwP" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Items>
                    <%-----------------------Panel con menu visor elementos gráficos---------------------%>
                    <ext:Panel ID="pnComboGrdVisor"
                        runat="server"
                        Header="false"
                        Cls="pnNoHeader"
                        Layout="HBoxLayout">

                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                        </LayoutConfig>
                        <Items>

                            <ext:TreePanel
                                Hidden="true"
                                Cls="gridPanel TreePnl"
                                ID="TreePanel2"
                                runat="server"
                                Width="240"
                                Scroll="None"
                                RootVisible="false"
                                Title="INVENTORY">


                                <DockedItems>

                                    <ext:Toolbar runat="server" Dock="Top" ID="toolbar4" Hidden="false" Cls="tlbGrid">
                                        <Items>

                                            <ext:Button runat="server" ID="Button18" Text="" Cls="btn-trans btnAnadir" IconCls="">
                                                <Listeners>
                                                    <Click Fn="ShowPanelAddMeds" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="Button21" Text="" Cls="btn-trans btnEditar" IconCls="">
                                            </ext:Button>
                                            <ext:Button runat="server" ID="Button22" Text="" Cls="btn-trans btnEliminar" IconCls="">
                                                <Listeners>
                                                    <Click Fn="HidePanelAddMeds" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="Button23" Text="" Cls="btn-trans btnRefrescar" IconCls="">
                                            </ext:Button>
                                            <ext:Button runat="server" ID="Button24" Text="" Cls="btn-trans btnDescargar" IconCls="">
                                            </ext:Button>
                                            <ext:ToolbarFill />
                                        </Items>

                                    </ext:Toolbar>

                                    <ext:Toolbar runat="server" Dock="Top" Cls="tlbGrid">
                                        <Items>
                                            <ext:TextField
                                                runat="server"
                                                Width="220"
                                                EmptyText="Search"
                                                LabelWidth="50">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search" />
                                                </Triggers>
                                            </ext:TextField>
                                            <ext:ToolbarFill />
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>


                                <Root>
                                    <ext:Node Expanded="true">
                                        <Children>
                                            <ext:Node Text="Types" />
                                            <ext:Node Text="Station BBS" />
                                            <ext:Node Text="Radio Base XM900" />
                                            <ext:Node Text="Radio Sat" />
                                        </Children>
                                    </ext:Node>
                                </Root>

                            </ext:TreePanel>
                            <ext:Panel ID="visorInsidePn"
                                runat="server"
                                Header="false"
                                Border="false"
                                Cls="visorInsidePn"
                                MinHeight="400"
                                Flex="1">

                                <Items>
                                    <ext:Toolbar ID="tlbVisor" Cls="tlbGrid tlbVisor" runat="server">
                                        <Items>
                                            <%--<ext:Button ID="btnShowPn" runat="server" IconCls="ico-menu" Width="44" Height="44" Border="false" Handler="displayGrd();"></ext:Button>--%>
                                            <%--  <ext:ComboBox ID="cmbLevels" runat="server" Cls="cmbIndicador" Width="120" Text="Building 1">
                                                <Items>
                                                    <ext:ListItem Text="Building 1" Value="1"></ext:ListItem>
                                                    <ext:ListItem Text="Building 2" Value="2"></ext:ListItem>
                                                    <ext:ListItem Text="Building 3" Value="3"></ext:ListItem>
                                                    <ext:ListItem Text="Building 4" Value="4"></ext:ListItem>
                                                </Items>
                                            </ext:ComboBox>--%>
                                            <ext:Button runat="server" ID="btnSbCategory" Text="Antennas" IconCls="ico-menu-gr" Cls="btnSbCategory" Handler="displayGrd();"></ext:Button>
                                            <ext:ToolbarFill />
                                            <ext:Button runat="server" ID="btnLista" Cls="btnLista" AriaLabel="Cambiar a Lista" ToolTip="Cambiar a Lista" Handler=""></ext:Button>
                                            <ext:Button runat="server" ID="Button1" Cls="btnFlow" AriaLabel="Cambiar a Flujo" ToolTip="Cambiar a Flujo" Handler=""></ext:Button>
                                        </Items>

                                    </ext:Toolbar>
                                    <ext:Panel ID="pnVisor"
                                        runat="server"
                                        Cls="pnVisorInsidePn">


                                        <Items>
                                            <%-- -----CONTENIDO AQUÍ-------%>
                                            <ext:Image
                                                ID="imgPlano"
                                                runat="server"
                                                ImageUrl="../../ima/planoexample.svg">
                                            </ext:Image>




                                            <ext:Button runat="server" ID="btnTestPopup" Cls="btnTestPopup" OverCls="none" Focusable="false" PressedCls="none" Handler="ShowPopup()" Text="">
                                            </ext:Button>
                                        </Items>

                                        <Content >
                                            <div id="dvInfoSiteMap" class="PopUpInventarioGV" style="position: absolute !important;" draggable="true">

                                                <div id="wrapClose" class="wrapClose" >
                                                    <span id='close' class="btnClose" onclick="document.getElementById('dvInfoSiteMap').style.display='none'" style="float: right; cursor: pointer;"></span>

                                                </div>



                                                <div id="dvSiteInformation" class="">
                                                    <span id="spCode" class=" lblMap d-inBlk">Code: <a href="#" class="lnkMap">812631732RK  </a></span></br>
                                                <span class="lblMap d-inBlk">Und  :  </span><span id="spUnd" class="d-inBlk lblMap">30 u  </span></br>
                                                <span class="lblMap d-inBlk">Model  :  </span><span id="spModel" class="d-inBlk lblMap">Cisco 8123  </span></br>
                                                <span class="lblMap d-inBlk">Client  :   </span><span id="spClient" class="d-inBlk ">Movistar  </span></br >
                                                    <span class="lblMap d-inBlk">Occupancy  :  </span><span id="spOcupacion" class="d-inBlk"><span class="">5%  </span></span></br >
                                                        <span class="lblMap d-inBlk">Energy  :  </span><span id="spEnergy" class="d-inBlk">22.5MW / year  </span></br >

                                                </div>

                                                <div id="BtnsFooter">

                                                    <button id="PieCh" class="BasebtnCard ico-trash "></button>
                                                    <button id="Boxes" class="BasebtnCard ico-info-dark"></button>
                                                    <button id="Notes" class="BasebtnCard ico-editar"></button>


                                                </div>

                                                <img id="picoPopup" class="picoPopUp" src="../../ima/ico-pico-popup.svg">
                                            </div>
                                        </Content>


                                    </ext:Panel>

                                </Items>
                                <Listeners>
                                    <AfterRender Handler="visorResizer();"></AfterRender>
                                </Listeners>
                            </ext:Panel>


                        </Items>
                    </ext:Panel>
                    <%-----------------------Fin Panel con visor elementos gráficos---------------------%>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
