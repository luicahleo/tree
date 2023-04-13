<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadosConfigVisualFlujos.aspx.cs" Inherits="TreeCore.PaginasComunes.EstadosConfigVisualFlujos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
	<!--<script src="../../JS/common.js"></script>-->
	<link href="css/EstadosConfigVisualFlujo.css" rel="stylesheet" type="text/css" />
	<script src="js/EstadosConfigVisualFlujo.js"></script>
	<script>
		
	</script>
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<ext:ResourceManager runat="server"
				ID="ResourceManagerTreeCore"
				DirectMethodNamespace="TreeCore">
			</ext:ResourceManager>
			<ext:Viewport ID="vwResp" runat="server">
				<Listeners>
					<AfterRender Fn="DisplayBtnsSliders()"></AfterRender>
				</Listeners>
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
										meta:resourceKey="lnkSites"
										Cls="lnk-navView lnk-noLine navActivo"
										Text="STATES">
									</ext:HyperlinkButton>


								</Items>
							</ext:Container>
						</Items>
					</ext:Panel>
					<ext:Container runat="server" ID="ctBtnSldr">
						<Items>
							<ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev" Cls="btnMainSldr" Handler="moveCtSldr(this);"></ext:Button>
							<ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next" Handler="moveCtSldr(this);" Disabled="true"></ext:Button>
						</Items>
					</ext:Container>
					<ext:Container ID="ctHuge" Cls="ctHuge col-total" runat="server" HeightSpec="100%">

						<Items>
							<ext:Container ID="ctMain1" Cls="col colCt1 col-total" runat="server" Hidden="false" HeightSpec="100%">
								<Items>
									<ext:Panel
										runat="server"
										ID="pnFlujoConfig"
										Header="false"
										Cls="grdNoHeader"
										Height="700">
										<DockedItems>
											<ext:Toolbar runat="server" ID="tlbTop" Cls="tlbGrid" Dock="Top">
												<Items>
													<ext:Button runat="server" ID="btnAddState" Text="State" Cls="btnAddState"></ext:Button>
													<ext:Button runat="server" ID="btnAddTransition" Text="Transition" Cls="btnAddTransition"></ext:Button>

													<ext:Container runat="server" Layout="HBoxLayout" Cls="btnFloatRInGrid">
														<Items>
															<ext:Button runat="server" ID="btnDisplayDetails" Cls="btnStateDetails"></ext:Button>
													<ext:ComboBox runat="server" ID="cmbZoom" Cls="cmbZoom" Text="100%">
														<Items>
															<ext:ListItem runat="server" Text="250%"></ext:ListItem>
															<ext:ListItem runat="server" Text="150%"></ext:ListItem>
															<ext:ListItem runat="server" Text="100%"></ext:ListItem>
															<ext:ListItem runat="server" Text="75%"></ext:ListItem>
															<ext:ListItem runat="server" Text="50%"></ext:ListItem>
															<ext:ListItem runat="server" Text="25%"></ext:ListItem>
														</Items>
													</ext:ComboBox>
													<ext:Button runat="server" ID="btnVistaGrid" Cls="btnLista "></ext:Button>
														</Items>
													</ext:Container>
													

												</Items>
											</ext:Toolbar>
											<ext:Toolbar runat="server" ID="tlbDown" Cls="tblGrid">
												<Items>
													<ext:Label runat="server" ID="lblTypologyName" Text="Typology Name" Cls="lblTypologyName"></ext:Label>
													<ext:Button runat="server" ID="btnAddDepartment" Text="Department" Cls="btn-ppal btnAdd btnAddDepartment btnFloatRInGrid">
														<Menu>
															<ext:Menu runat="server" ID="mnDepartment">
																<Items>
																	<ext:MenuItem runat="server" Text="Department Name 1"></ext:MenuItem>
																	<ext:MenuItem runat="server" Text="Department Two"></ext:MenuItem>
																</Items>
															</ext:Menu>
														</Menu>
													</ext:Button>

												</Items>
											</ext:Toolbar>
										</DockedItems>
										<Items>
											<ext:Container runat="server" ID="ctDrawingFlow" Cls="ctDrawingFlow">
												<Items>
													<ext:Button runat="server" ID="cmbDepartmentBlock" Text="Selected Department Name">
														<Menu>
															<ext:Menu runat="server" ID="mnDepartmentBlk_1" Width="180">
																<Items>
																	<ext:MenuItem runat="server" Text="Department Name 1"></ext:MenuItem>
																	<ext:MenuItem runat="server" Text="Department Two"></ext:MenuItem>
																</Items>
															</ext:Menu>
														</Menu>
													</ext:Button>
																				
												</Items>
												<Content>
													<canvas id="myCanvas" class="canvasWorkFlow">
														<%------------------------Pintar el workflow aquí, añadir estados y transiciones aquí----------------------%>
																											
															

													</canvas>
												</Content>
											</ext:Container>

										</Items>
										<DockedItems>
											<ext:Toolbar runat="server" ID="tlbFooter" Dock="Bottom">
												<Items>
													<ext:Button runat="server" ID="btnSave" Text="Save" Cls="btn-ppal btn-add btnFloatRInGrid"></ext:Button>
												</Items>
											</ext:Toolbar>
										</DockedItems>
									</ext:Panel>
								</Items>
							</ext:Container>


						</Items>
					</ext:Container>
					<ext:Panel ID="pnAsideR" runat="server" Hidden="false">
						<Items>
							<ext:Label runat="server" IconCls="ico-head-Notif" Text="Head Label Aside" Cls="lblHeadAside"></ext:Label>
						</Items>
					</ext:Panel>
				</Items>
			</ext:Viewport>
		</div>
	</form>
</body>
</html>
