<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoContenedor.aspx.cs" Inherits="TreeCore.PaginasComunes.SeguimientoContenedor" %>

<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!X.IsAjaxRequest)
		{

			this.grAsR1.Store.Primary.DataSource = TreeCore.PaginasComunes.SeguimientoContenedor.DataGridAsR;

		}
	}
</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleSeguimientoContenedor.css" rel="stylesheet" type="text/css" />
    <script src="js/SeguimientoContenedor.js"></script>

    <script type="text/javascript">
        var nm = {
            Add: function () {
                var panel1 = Ext.getCmp('hugeCt');
                var child = {
                    xtype: 'panel',
                    layout: 'fit',
                    closable: false,
                    title: 'TESTTTTTT',
                    autoLoad: {
                        showMask: true,
                        scripts: true,
                        renderer: 'frame',
                        url: '../PaginasComunes/SeguimientoChecklist.aspx'
                    }
                };
                panel1.add(child);
            }
        };
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <ext:Viewport ID="vwResp" runat="server" Cls="vwContenedor">
                <Items>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();" ></ext:Button>
              <%--      <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="nm.Add();" ></ext:Button>--%>
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
                                        Text="CURRENT VIEW">
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

                    <ext:Container ID="hugeCt" runat="server" Layout="FitLayout" Cls="hugeMainCt">
                                <Loader ID="Loader1"
                                    Height="900"
                                    runat="server"
                                    Url="../PaginasComunes/Seguimiento.aspx"
                                    Mode="Frame">
                                    <LoadMask ShowMask="true" />
                                </Loader>
                
                    </ext:Container>


       	<ext:Panel runat="server" ID="pnAsideR" Hidden="false"
						Header="false" Border="false" Width="360">
						<Items>
							<ext:Label ID="lblAsideNameR" runat="server" IconCls="ico-head-Notif" Cls="lblHeadAside" Text="Panel Name"></ext:Label>
							<ext:Panel ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
								<Items>
									<ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false">
										<Items>
											<ext:Button runat="server" ID="btnCandidatos" Cls="btnCandidatos-asR"></ext:Button>
											<ext:Button runat="server" ID="btnActivos" Cls="btnActivos-asR"></ext:Button>
										</Items>
									</ext:Panel>
									<ext:Panel ID="pnGridsAsideR" runat="server" Border="false" Header="false">
										<Items>
											<ext:Label ID="lblGrid" runat="server" IconCls="ico-head-Notif" Cls="lblHeadAside" Text="Grid Name"></ext:Label>
											<ext:Toolbar ID="tblGrdAsR" runat="server" Cls="tlbGrid tlbGrdAsR">
												<Items>
													<ext:Button runat="server" ID="btnAnadirGrdAsR" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="Añadir"></ext:Button>
													<ext:Button runat="server" ID="btnEditarGrdAsR" Cls="btnEditar" AriaLabel="Editar" ToolTip="Editar"></ext:Button>
													<ext:Button runat="server" ID="btnEliminarGrdAsR" Cls="btnEliminar" ArialLabel="Eliminar" ToolTip="Eliminar"></ext:Button>
													<ext:Button runat="server" ID="btnRefrescarGrdAsR" Cls="btnRefrescar" ArialLabel="Refrescar" ToolTip="Refrescar"></ext:Button>
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
												<ColumnModel runat="server" >
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
