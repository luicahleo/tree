<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadosV4.aspx.cs" Inherits="TreeCore.PaginasComunes.estados" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!X.IsAjaxRequest)
		{
			this.Store1.DataSource = this.Data;
			this.Store2.DataSource = this.Data2;
			
		}
	}

	private object[] Data
	{
		get
		{
			return new object[]
			{
				new object[] { "", 0.5, "License denied","Licencia_denegada", "", 3, 5, "License denied", "RF", "Blocked", "",  "", "", "", "" },
				new object[] { "",  0.3, "Canceled by client", "Cancelado_cliente", "", 4, 8, "Canceled", "Client", "Blocked", "",  "", "icon", "", "icon" },
				new object[] { "",  0.3, "Provisionally denied…", "Licencia_denegada_prov…", "", 1, 2, "Blocked", "RF", "Blocked", "icon",  "", "", "icon", "" },
				new object[] { "", 0.4, "Eliminated", "Eliminado", "", 4, 7, "Eliminated", "RF",  "Blocked", "",  "", "", "icon", "" },
				new object[] { "icon", 0.1,  "Kick-off", "Carga inicial", "Launched", 5, 7, "Launched", "Infra", "On Air", "",  "icon", "", "", "" },
				new object[] { "", 1, "Pending request doc…", "Pendiente_solicitar_doc...l", "Pending documentation", 6, 8, "On going", "Infra", "On Air", "icon",  "", "icon", "icon", "" }


			};
		}
	}
	private object[] Data2
	{
		get
		{
			return new object[]
			{
				new object[] { "Access Acceso","Acceso_A_Access_Modulo" },
				new object[] { "Access Conflictividad","Acceso total a la conflictividad modulo…" },
				new object[] { "Access Emplazamiento","Acceso Usuarios Emplazamientos Access" },
				new object[] { "Access Emplazamiento","Acceso_restringido_access_estado" },
				new object[] { "Access Acceso","Acceso_restringido_access_estado" },
				new object[] { "Access Acceso","Acceso_A_Access_Modulo" },
				new object[] { "Access Conflictividad","Acceso total a la conflictividad modulo…" },
				new object[] { "Access Emplazamiento","Acceso Usuarios Emplazamientos Access" },
				new object[] { "Access Emplazamiento","Acceso_restringido_access_estado" },
				new object[] { "Access Acceso","Acceso_restringido_access_estado" }


			};
		}
	}

	
</script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
	<link href="css/styleStates.css" rel="stylesheet" type="text/css" />
	<link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet">
	<script type="text/javascript" src="js/Estados.js"></script>
	<script>


		

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<%--INICIO HIDDEN --%>
			<%--FIN HIDDEN --%>
			<%--INICIO  RESOURCEMANAGER --%>
			<ext:ResourceManager ID="ResourceManagerTreeCore" runat="server" DirectMethodNamespace="TreeCore">
			</ext:ResourceManager>
			<%--FIN  RESOURCEMANAGER --%>
			<%--INICIO  STORES --%>
			<%--FIN  STORES --%>
			<%--INICIO  WINDOWS --%>
			<ext:Window ID="winGestion"
				runat="server"
				Title="Add State"
				Resizable="false"
				Modal="true"
				Width="680"
				Height="650"
				Hidden="true">
				<Items>
					<ext:Panel runat="server" ID="pnVistasForm" Cls="pnNavVistas pnVistasForm" AriaRole="navigation">
						<Items>
							<ext:Container runat="server" ID="cntNavVistasForm" Cls="nav-vistas">
								<Items>
									<ext:HyperlinkButton runat="server" ID="lnkState" Cls="lnk-navView  lnk-noLine navActivo" Text="STATE" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkSubprocess" Cls="lnk-navView  lnk-noLine" Text="SUBPROCESS" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkNext" Cls="lnk-navView  lnk-noLine" Text="NEXT STATE" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkLinks" Cls="lnk-navView  lnk-noLine" Text="LINKS" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkNots" Cls="lnk-navView  lnk-noLine" Text="NOTIFICATIONS" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkDocs" Cls="lnk-navView  lnk-noLine" Text="DOCUMENTS" Handler="showForms(this);"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkFunc" Cls="lnk-navView  lnk-noLine" Text="FUNCTIONALITIES" Handler="showForms(this);"></ext:HyperlinkButton>

								</Items>
							</ext:Container>
						</Items>
					</ext:Panel>
					<ext:Panel runat="server" ID="pnContent" Header="false">
						<Items>
							<ext:Panel runat="server" ID="pnDiagramFlow0" Cls="pnDiagramFlow" Hidden="false">
								<Content>
									<div class="dvTypoForm">
										<span>Typology</span>
										<ext:Label runat="server" Cls="lblTypoForm" Text="Global"></ext:Label>
									</div>
							
								</Content>
							</ext:Panel>
							<ext:Panel runat="server" ID="pnDiagramFlow" Cls="pnDiagramFlow" Hidden="true">
								<Content>
									<div class="dvTypoForm">
										<span>Typology</span>
										<ext:Label runat="server" Cls="lblTypoForm" Text="Global"></ext:Label>
									</div>
									<div id="dvCntStatesForm" class="dvAllStates">
										<div id="prevCntStatesForm" class="dvCntState">
											<div id="prevState2Form" class="dvState">
												<ext:Label runat="server" ID="lblPrevStForm2" Text="Previous State 2" Cls="lblStatesFlow"></ext:Label>
												<span class="dotFlow"></span>
											</div>
											<div id="prevState1Form" class="dvState">
												<ext:Label runat="server" ID="lblPrevStForm1" Text="Previous State 1" Cls="lblStatesFlow"></ext:Label>
												<span class="dotFlow ancla lineFlow"></span>
											</div>
											<div id="prevState3Form" class="dvState">
												<ext:Label runat="server" ID="lblPrevStForm3" Text="Previous State 3" Cls="lblStatesFlow"></ext:Label>
												<span class="dotFlow"></span>
											</div>
										</div>
										<div id="currentStateForm" class="dvCntState">
											<span class="dotCurrent">
												<span id="dotFlowCurrentForm" class="dotFlow ancla lineFlow"></span>
											</span>
											<ext:Label runat="server" ID="lblCurrentStForm" Text="Mouseover State" Cls="lblStatesFlow"></ext:Label>
										</div>
										<div id="nextCntStateForm" class="dvCntState">
											<div id="nextState2Form" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStForm2" Text="Next State 2 " Cls="lblStatesFlow"></ext:Label>
											</div>
											<div id="nextState1Form" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStForm1" Text="Next State 1" Cls="lblStatesFlow"></ext:Label>
											</div>
											<div id="nextState3Form" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStForm3" Text="Next State 3" Cls="lblStatesFlow"></ext:Label>
											</div>
										</div>
									</div>
								</Content>
							</ext:Panel>
							<ext:Panel runat="server" ID="pnDiagramFlowLnk" Cls="pnDiagramFlow" Hidden="true">
								<Content>
									<div id="dvTypoLnkF" class="dvTypoForm">
										<span>Typology</span>
										<ext:Label runat="server" Cls="lblTypoForm" Text="Global"></ext:Label>
										<ext:Label runat="server" Cls="lblLnkModulo" Text="Acquisitions: Building Permit"></ext:Label>
									</div>
									<div id="dvCntStatesLnksF" class="dvAllStates">
										
										<div id="currentStateLnkF" class="dvCntState">
											<span id="dotCurrentLnkF" class="dotCurrent">
												<span id="dotFlowCurrentLnkF" class="dotFlow ancla lineFlow"></span>
											</span>
											<ext:Label runat="server" ID="lblCurrentLnkF" Text="Current State" Cls="lblStatesFlow"></ext:Label>
										</div>
										<div id="nextCntStateLnkF" class="dvCntState">
											<div id="nextState2LnkF" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStLnkF" Text="Next State 2 " Cls="lblStatesFlow"></ext:Label>
											</div>
											<div id="nextState1LnkF" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStLnkF1" Text="Next State 1" Cls="lblStatesFlow"></ext:Label>
											</div>
											<div id="nextState3LnkF" class="dvState">
												<span class="dotFlow"></span>
												<ext:Label runat="server" ID="lblNextStLnkF3" Text="Next State 3" Cls="lblStatesFlow"></ext:Label>
											</div>
										</div>
									</div>
								</Content>
							</ext:Panel>
							<ext:FormPanel runat="server" ID="formState" Cls="formWorkflow formGris" Hidden="false">
								<Items>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:TextField runat="server"
												ID="txtName"
												FieldLabel="Name"
												LabelAlign="Top"
												Width="268"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />

											<ext:TextField runat="server"
												ID="txtCode"
												FieldLabel="Code"
												LabelAlign="Top"
												Width="268"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />
										</Items>
									</ext:FieldContainer>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:TextField runat="server"
												ID="txtGroup"
												FieldLabel="Group"
												LabelAlign="Top"
												Width="268"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />
											<ext:ComboBox runat="server"
												ID="cmbGlobalState"
												FieldLabel="Global State"
												LabelAlign="Top"
												Width="268">
												<Items>
													<ext:ListItem Text="Global State 1" />
													<ext:ListItem Text="Global State 2" />
													<ext:ListItem Text="Global State 3" />
													<ext:ListItem Text="Global State 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
										</Items>
									</ext:FieldContainer>
									<ext:FieldContainer runat="server" Layout="HBoxLayout">
										<Items>
											<ext:ComboBox runat="server"
												ID="cmbDepartment"
												FieldLabel="Department"
												LabelAlign="Top"
												Width="268">
												<Items>
													<ext:ListItem Text="Departament 1" />
													<ext:ListItem Text="Departament 2" />
													<ext:ListItem Text="Departament 3" />
													<ext:ListItem Text="Departament 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
											<ext:NumberField runat="server"
												FieldLabel="Red"
												LabelAlign="Top"
												ID="txtRed"
												Width="72" />
											<ext:NumberField runat="server"
												FieldLabel="Yellow"
												LabelAlign="Top"
												ID="txtAmarillo"
												Width="72" />
											<ext:NumberField runat="server"
												FieldLabel="Average"
												LabelAlign="Top"
												ID="txtAverage"
												Width="74" />
										</Items>
									</ext:FieldContainer>
									<ext:FieldContainer runat="server" Layout="HBoxLayout">
										<Items>
											<ext:Checkbox runat="server"
												ID="chkComplete"
												BoxLabel="State as complete"
												Cls="chk-form"
												Width="200" />
										</Items>
									</ext:FieldContainer>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel ID="formSubP" runat="server" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:TextField runat="server"
												ID="txtSubprocess"
												FieldLabel="Subprocess"
												LabelAlign="Top"
												Width="268"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />

											<ext:ComboBox runat="server"
												ID="cmbTypology"
												FieldLabel="Typology"
												LabelAlign="Top"
												Width="268">
												<Items>
													<ext:ListItem Text="Typology 1" />
													<ext:ListItem Text="Typology 2" />
													<ext:ListItem Text="Typology 3" />
													<ext:ListItem Text="Typology 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
										</Items>
									</ext:FieldContainer>
									<ext:Button runat="server" ID="btnAddTypo" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
									<ext:Panel runat="server" ID="pnTypoList" Cls="pnListF" Header="false" Border="false">
										<Content>
											<div id="dvTypoLbls" class="dvLbls-listF">
												<span class="spanLbl flx-2 ">Subprocess</span>
												<span class="spanLbl flx-4">Code</span>
												<span class="spanLbl flx-3">Typology</span>
											</div>
											<div id="dvCntTypoList" class="cntListF">
												<ul id="ulTypo" class="ulListF">
													<li id="liTypo1" class="liListF">
														<ext:Label runat="server" ID="lblSubp1" Cls="lblBox" Width="220" Text="Legal Compliance"></ext:Label>
														<ext:Label runat="server" ID="lblCode1" Cls="lblBox" Width="90" Text="33234T"></ext:Label>
														<ext:Label runat="server" ID="LblTypo1" Cls="lblBox" Width="190" Text="Legal II"></ext:Label>
														<ext:Button runat="server" Cls="btn-cleanBox"></ext:Button>
													</li>
												</ul>
											</div>

										</Content>
									</ext:Panel>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel ID="formNext" runat="server" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:TextField runat="server"
												ID="txtNameNext"
												FieldLabel="Name"
												LabelAlign="Top"
												Width="267"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />

											<ext:TextField runat="server"
												ID="txtCodeNext"
												FieldLabel="Code"
												LabelAlign="Top"
												Width="267"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />
										</Items>
									</ext:FieldContainer>
									<ext:Button runat="server" ID="btnAddNext" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
									<ext:Panel runat="server" ID="pnNextList" Cls="pnListF" Header="false" Border="false">
										<Content>
											<div id="dvNextLbls" class="dvLbls-listF">
												<span class="spanLbl flx-1">Name</span>
												<span class="spanLbl flx-2">Code</span>

											</div>
											<div id="dvCntNextList" class="cntListF">
												<ul id="ulNext" class="ulListF">
													<li id="liNext1" class="liListF">
														<ext:Label runat="server" ID="lblNextName1" Cls="lblBox" Width="280" Text="Pending Correction"></ext:Label>
														<ext:Label runat="server" ID="lblNextCode1" Cls="lblBox" Width="220" Text="Pending_Correction"></ext:Label>
														<ext:Button runat="server" Cls="btn-cleanBox"></ext:Button>
													</li>
												</ul>
											</div>

										</Content>
									</ext:Panel>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel ID="formLinks" runat="server" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:ComboBox runat="server"
												ID="cmbProjectLnk"
												FieldLabel="Project Type"
												LabelAlign="Top"
												Width="168">
												<Items>
													<ext:ListItem Text="Project Type 1" />
													<ext:ListItem Text="Project Type 2" />
													<ext:ListItem Text="Project Type 3" />
													<ext:ListItem Text="Project Type 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>

											<ext:ComboBox runat="server"
												ID="cmbTypologyLnk"
												FieldLabel="Project Type"
												LabelAlign="Top"
												Width="168">
												<Items>
													<ext:ListItem Text="Typology 1" />
													<ext:ListItem Text="Typology 2" />
													<ext:ListItem Text="Typology 3" />
													<ext:ListItem Text="Typology 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
											<ext:ComboBox runat="server"
												ID="cmbStateLnk"
												FieldLabel="Project Type"
												LabelAlign="Top"
												Width="168">
												<Items>
													<ext:ListItem Text="State 1" />
													<ext:ListItem Text="State 2" />
													<ext:ListItem Text="State 3" />
													<ext:ListItem Text="State 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
										</Items>
									</ext:FieldContainer>
									<ext:Button runat="server" ID="btnAddLnk" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
									<ext:Panel runat="server" ID="pnLnksList" Cls="pnListF" Header="false" Border="false">
										<Content>
											<div id="dvLnksLbls" class="dvLbls-listF">
												<span class="spanLbl flx-3">Project Type</span>
												<span class="spanLbl flx-3">Typology</span>
												<span class="spanLbl flx-3">State</span>

											</div>
											<div id="dvCntLnksList" class="cntListF">
												<ul id="ulLnks" class="ulListF">
													<li id="liLnks1" class="liListF">
														<ext:Label runat="server" ID="lblLnksProj1" Cls="lblBox" Width="160" Text="Pending Correction"></ext:Label>
														<ext:Label runat="server" ID="lblLnksTypo1" Cls="lblBox" Width="170" Text="Pending_Correction"></ext:Label>
														<ext:Label runat="server" ID="lblLnksState1" Cls="lblBox" Width="160" Text="Pending_Correction"></ext:Label>
														<ext:Button runat="server" Cls="btn-cleanBox"></ext:Button>
													</li>
												</ul>
											</div>

										</Content>
									</ext:Panel>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel runat="server" ID="formNots" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server">
										<Items>
											<ext:TextField runat="server"
												ID="txtRecipients"
												FieldLabel="Recipients"
												LabelAlign="Top"
												Width="570"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />

											<ext:TextField runat="server"
												ID="txtProfiles"
												FieldLabel="Profiles"
												LabelAlign="Top"
												Width="570"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />

											<ext:TextArea runat="server"
												ID="txaMessage"
												FieldLabel="Profiles"
												LabelAlign="Top"
												Width="570"
												AllowBlank="false"
												ValidationGroup="FORM"
												CausesValidation="true" />
										</Items>
									</ext:FieldContainer>

									<ext:FieldContainer runat="server" ID="flCntChkNots" Layout="HBoxLayout">
										<Items>
											<ext:Checkbox runat="server"
												ID="chkNotAgency"
												BoxLabel="Notify to Agency"
												Cls="chk-form"
												Width="150" />
											<ext:CheckboxGroup runat="server" ID="chkGroupSend" Cls="chk-form" Width="390">
												<Items>
													<ext:Checkbox ID="chkSendRecip" runat="server" BoxLabel="Send to Recipients" />
													<ext:Checkbox ID="chkSendProf" runat="server" BoxLabel="Send to Profiles" />
													<ext:Checkbox ID="chkNoSend" runat="server" BoxLabel="No Send" />

												</Items>
											</ext:CheckboxGroup>



										</Items>
									</ext:FieldContainer>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel ID="formDocs" runat="server" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server" Layout="HBox">
										<Items>
											<ext:ComboBox runat="server"
												ID="cmbDocType"
												FieldLabel="Document Type"
												LabelAlign="Top"
												Width="168">
												<Items>
													<ext:ListItem Text="Doc Type 1" />
													<ext:ListItem Text="Document Type 2" />
													<ext:ListItem Text="Doc Type 3" />
													<ext:ListItem Text="Document Type 4" />
												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>

											<ext:ComboBox runat="server"
												ID="cmbRequiredDoc"
												FieldLabel="Required"
												LabelAlign="Top"
												Width="168">
												<Items>
													<ext:ListItem Text="Optional" />
													<ext:ListItem Text="Required" />

												</Items>
												<Triggers>
													<ext:FieldTrigger meta:resourceKey="LimpiarLista"
														Icon="Clear"
														QTip="Limpiar Lista" />
													<ext:FieldTrigger meta:resourceKey="RecargarLista"
														IconCls="ico-reload"
														QTip="Recargar Lista" />
												</Triggers>
											</ext:ComboBox>
											<ext:Checkbox ID="chkValidated"
												runat="server"
												Cls="chk-form"
												BoxLabel="Validated"
												Width="168">
											</ext:Checkbox>
										</Items>
									</ext:FieldContainer>
									<ext:Button runat="server" ID="btnAddDoc" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
									<ext:Panel runat="server" ID="pnDocsList" Cls="pnListF" Header="false" Border="false">
										<Content>
											<div id="dvDocsLbls" class="dvLbls-listF">
												<span class="spanLbl flx-3">Document Type</span>
												<span class="spanLbl flx-3">Required</span>
												<span class="spanLbl flx-3">Validated</span>

											</div>
											<div id="dvCntDocsList" class="cntListF">
												<ul id="ulDocs" class="ulListF">
													<li id="liDoc1" class="liListF">
														<ext:Label runat="server" ID="lblDocType1" Cls="lblBox" Width="160" Text="Administrative Licence"></ext:Label>
														<ext:Label runat="server" ID="lblDocRequired1" Cls="lblBox" Width="170" Text="Optional"></ext:Label>
														<ext:Label runat="server" ID="lblDocValidated1" Cls="lblBox" Width="160" IconCls="ico-accept"></ext:Label>
														<ext:Button runat="server" Cls="btn-cleanBox"></ext:Button>
													</li>
												</ul>
											</div>

										</Content>
									</ext:Panel>
								</Items>
							</ext:FormPanel>
							<ext:FormPanel ID="formFuncs" runat="server" Cls="formWorkflow formGris" Hidden="true">
								<Items>
									<ext:FieldContainer runat="server" ID="flLlbFuncs" Layout="HBoxLayout">
										<Items>
											<ext:Label runat="server" id="lblModuleFuncs" Text="Module"></ext:Label>
											<ext:Label runat="server" ID="lblFuncsFuncs" Text="Functionalities"></ext:Label>
										</Items>
									</ext:FieldContainer>
									<ext:GridPanel
										ID="gridFuncs"
										runat="server"
										Width="568"
										Height="150"
										Cls="gridPanel">
										<Store>
											<ext:Store ID="Store2" runat="server" PageSize="10">
												<Model>
													<ext:Model runat="server">
														<Fields>
															<ext:ModelField Name="Module" />
															<ext:ModelField Name="Func" />
															
														</Fields>
													</ext:Model>
												</Model>
											</ext:Store>
										</Store>
										<ColumnModel runat="server">
											<Columns>
												<ext:Column runat="server" Text="Module" Width="245" DataIndex="Module" />
												<ext:Column runat="server" Text="Functionality" Width="245" DataIndex="Func" />
											</Columns>
										</ColumnModel>
										<SelectionModel>
											<ext:CheckboxSelectionModel runat="server" Mode="Multi" />
										</SelectionModel>
									</ext:GridPanel>
									<ext:Button runat="server" ID="btnAddFunc" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd" Text="Add"></ext:Button>
									<ext:Panel runat="server" ID="pnFuncList" Cls="pnListF" Header="false" Border="false">
										<Content>
											<div id="dvFuncsLbls" class="dvLbls-listF">
												<span class="spanLbl flx-3">Document Type</span>
												<span class="spanLbl flx-3">Required</span>
												<span class="spanLbl flx-3">Validated</span>

											</div>
											<div id="dvCntFuncsList" class="cntListF">
												<ul id="ulFuncs" class="ulListF">
													<li id="liFunc1" class="liListF">
														<ext:Label runat="server" ID="lblFuncType1" Cls="lblBox" Width="160" Text="Administrative Licence"></ext:Label>
														<ext:Label runat="server" ID="lblFuncRequired1" Cls="lblBox" Width="170" Text="Optional"></ext:Label>
														<ext:Label runat="server" ID="lblFuncValidated1" Cls="lblBox" Width="160" IconCls="ico-accept"></ext:Label>
														<ext:Button runat="server" Cls="btn-cleanBox"></ext:Button>
													</li>
												</ul>
											</div>

										</Content>
									</ext:Panel>
								</Items>
							</ext:FormPanel>
							<ext:Container runat="server" ID="cntBtnForm">
								<Items>
									<ext:Button runat="server" ID="btnPrev" Cls="btn-secondary" Text="Previous"></ext:Button>
									<ext:Button runat="server" ID="btnNext" Cls="btn-ppal" Text="Next"></ext:Button>
								</Items>
							</ext:Container>
						</Items>

					</ext:Panel>
				</Items>

			</ext:Window>






			<%--FIN  WINDOWS --%>

			<%--INICIO  VIEWPORT --%>
			<ext:Viewport runat="server" ID="vwContenedor" Cls="vwContenedor" Layout="Anchor">

				<Items>
					<ext:Panel runat="server" ID="pnShowWorkflow" IconCls="ico-showFlow" Cls="floatingPn" Collapsed="false"
						Collapsible="true" Title="Workflow" Width="600" Height="250" Hidden="true" Draggable="true" >
						<Content>
							<div id="dvTypo">
								<span>Typology</span>
								<ext:Label runat="server" ID="lblTypo" Text="Global"></ext:Label>
							</div>
							<div id="dvCntStates" class="dvAllStates">
								<div id="prevCntStates" class="dvCntState">
									<div id="prevState2" class="dvState">
										<ext:Label runat="server" ID="lblPrevState2" Text="Previous State 2" Cls="lblStatesFlow"></ext:Label>
										<span class="dotFlow"></span>
									</div>
									<div id="prevState1" class="dvState">
										<ext:Label runat="server" ID="lblPrevState1" Text="Previous State 1" Cls="lblStatesFlow"></ext:Label>
										<span class="dotFlow ancla lineFlow"></span>
									</div>
									<div id="prevState3" class="dvState">
										<ext:Label runat="server" ID="lblPrevState3" Text="Previous State 3" Cls="lblStatesFlow"></ext:Label>
										<span class="dotFlow"></span>
									</div>
								</div>
								<div id="currentState" class="dvCntState">
									<span class="dotCurrent">
										<span id="dotFlowCurrent" class="dotFlow ancla lineFlow"></span>
									</span>
									<ext:Label runat="server" ID="lblCurrState" Text="Mouseover State" Cls="lblStatesFlow"></ext:Label>
								</div>
								<div id="nextCntState" class="dvCntState">
									<div id="nextState2" class="dvState">
										<span class="dotFlow"></span>
										<ext:Label runat="server" ID="lblNextState2" Text="Next State 2 " Cls="lblStatesFlow"></ext:Label>
									</div>
									<div id="nextState1" class="dvState">
										<span class="dotFlow"></span>
										<ext:Label runat="server" ID="lblNextState1" Text="Next State 1" Cls="lblStatesFlow"></ext:Label>
									</div>
									<div id="nextState3" class="dvState">
										<span class="dotFlow"></span>
										<ext:Label runat="server" ID="lblNextState3" Text="Next State 3" Cls="lblStatesFlow"></ext:Label>
									</div>
								</div>
							</div>
						</Content>
					</ext:Panel>

					<ext:Panel runat="server" ID="pnNavVistas" Cls="pnNavVistas" AnchorVertical="15%" AriaRole="navigation">
						<Items>
							<ext:Label runat="server" ID="lblNombrePagina" Cls="h4" Text="States"></ext:Label>
							<ext:Container runat="server" ID="conNavVistas" Cls="nav-vistas">
								<Items>
									<ext:HyperlinkButton runat="server" ID="lnkStates" Cls="lnk-navView lnk-noLine navActivo" Text="STATES"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkSubProc1" Cls="lnk-navView lnk-noLine" Text="SUBPROCCESS 1"></ext:HyperlinkButton>
									<ext:HyperlinkButton runat="server" ID="lnkSubProc2" Cls="lnk-navView lnk-noLine" Text="SUBPROCCESS 2"></ext:HyperlinkButton>

								</Items>
							</ext:Container>
						</Items>
					</ext:Panel>

					<ext:GridPanel
						runat="server"
						ID="gridGruposAutorizaciones"
						Cls="gridPanel grdNoHeader"
						AnchorHorizontal="-100" AnchorVertical="85%"
						AriaRole="main">

						<DockedItems>
							<ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid">
								<Items>
									<ext:Button runat="server" ID="btnAnadir" Cls="btn-trans" AriaLabel="Añadir" ToolTip="Añadir" Handler="anadir();"></ext:Button>
									<ext:Button runat="server" ID="btnEditar" Cls="btn-trans" AriaLabel="Editar" ToolTip="Editar" Handler="editar();"></ext:Button>
									<ext:Button runat="server" ID="btnEliminar" Cls="btn-trans" AriaLabel="Eliminar" ToolTip="Eliminar" Handler="eliminar();"></ext:Button>
									<ext:Button runat="server" ID="btnRefrescar" Cls="btn-trans" AriaLabel="Refrescar" ToolTip="Refrescar" Handler="refrescar();"></ext:Button>
									<ext:Button runat="server" ID="btnDescargar" Cls="btn-trans" AriaLabel="Descargar" ToolTip="Descargar" Handler="this.up('grid').print();"></ext:Button>
									<ext:Button runat="server" ID="btnDefecto" Cls="btn-trans" AriaLabel="Categoría por defecto" ToolTip="Categoría por defecto"></ext:Button>
									<ext:Button runat="server" ID="btnDuplicar" Cls="btn-trans" AriaLabel="Duplicar Registro" ToolTip="Duplicar Registro"></ext:Button>
									<ext:Button runat="server" ID="btnDuplicarTipologia" Cls="btn-trans" AriaLabel="Duplicar Tipología" ToolTip="Duplicar Tipología"></ext:Button>
									<ext:Button runat="server" ID="btnImportarExportar" Cls="btn-trans" AriaLabel="Importar Exportar" ToolTip="Importar Exportar" Handler="showPnAttributes();"></ext:Button>
									<ext:Button runat="server" ID="btnVerWorkFlow" Cls="btn-trans" AriaLabel="Ver Workflow" ToolTip="Ver Workflow" Handler="ShowWorkFlow();"></ext:Button>

								</Items>
							</ext:Toolbar>
							<ext:Toolbar runat="server" ID="tlbFiltros" Dock="Top">
								<Items>

									<ext:ComboBox runat="server" ID="cmbEmplazamientos" Cls="comboGrid pos-boxGrid" EmptyText="Tipologías">
										<Items>
											<ext:ListItem Text="Tipología 1" />
											<ext:ListItem Text="Tipología 2" />
											<ext:ListItem Text="Tipología 3" />
											<ext:ListItem Text="Tipología 4" />
										</Items>
									</ext:ComboBox>
								</Items>
							</ext:Toolbar>
						</DockedItems>
						<Store>
							<ext:Store ID="Store1" runat="server">
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
											<ext:ModelField Name="Notifications" />
											<ext:ModelField Name="Documents" />
											<ext:ModelField Name="Functionalities" />

										</Fields>
									</ext:Model>
								</Model>
							</ext:Store>
						</Store>
						<ColumnModel>
							<Columns>
								<ext:Column runat="server" Cls="col-default" DataIndex="Default" Width="40">
									<Renderer Fn="DefectoRender" />
								</ext:Column>
								<ext:ProgressBarColumn runat="server" DataIndex="Average" Text="Average" Width="64" Align="Center">
									<Renderer Fn="barGrid"></Renderer>
								</ext:ProgressBarColumn>

								<ext:Column runat="server" Text="Name" DataIndex="Name" Width="200" Flex="1">
								</ext:Column>
								<ext:HyperlinkColumn runat="server" Text="Code" DataIndex="Code" Width="200">
									
								</ext:HyperlinkColumn>
								
								
								<ext:Column runat="server" Text="Next Step" DataIndex="Next" Width="200">
									
								</ext:Column>
								<ext:Column runat="server" Cls="col-yellow" DataIndex="Amarillo" Width="40" Align="Center">
									<Renderer Fn="amarilloRender"></Renderer>
								</ext:Column>
								<ext:Column runat="server" Cls="col-red" DataIndex="Rojo" Width="40" Align="Center">
									<Renderer Fn="rojoRender"></Renderer>
								</ext:Column>

								<ext:Column runat="server" Text="Group" DataIndex="Group" Width="200" />
								<ext:Column runat="server" Text="Department" DataIndex="Department" Width="150" />
								<ext:Column runat="server" Text="Global State" DataIndex="GlobalState" Width="150" />
								<ext:Column runat="server" Cls="col-parallel" DataIndex="Parallel" Width="40">
									<Renderer Fn="SubprocessRender"></Renderer>
								</ext:Column>
								<ext:Column runat="server" Cls="col-link" DataIndex="Link" Width="40">
									<Renderer Fn="LinkRender"></Renderer>
								</ext:Column>
								<ext:Column runat="server" Cls="col-notification" DataIndex="Notifications" Width="40">
									<Renderer Fn="NotifRender"></Renderer>
								</ext:Column>
								<ext:Column runat="server" Cls="col-doc" DataIndex="Documents" Width="40">
									<Renderer Fn="DocsRender"></Renderer>
								</ext:Column>
								<ext:Column runat="server" Cls="col-functionalities" DataIndex="Functionalities" Width="40">
									<Renderer Fn="FunctRender"></Renderer>
								</ext:Column>
							</Columns>
						</ColumnModel>
						<SelectionModel>
							<ext:RowSelectionModel runat="server" />
						</SelectionModel>

						<BottomBar>
							<ext:PagingToolbar runat="server" StoreID="Store1">
								<Items>
									<ext:ComboBox runat="server" Cls="comboGrid">
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
					<ext:Panel ID="pnAsideRight" runat="server" Header="false" Width="400" Hidden="true">
						<Content>
							<ext:Button runat="server" ID="btnMoveRight" Handler="hidePnAttributes();"></ext:Button>
							<ext:Label runat="server" ID="lblRegSelect" Text="Pending delivery in Administration"></ext:Label>
							<ext:Label runat="server" ID="lblGroup" Text="Group: on going"></ext:Label>

							<div id="SliderLinks" class="slider">
								<h5>Linked Projects</h5>
								<ul id="ulLinks">
									<li id="liLnk_registro1">
										<span class="labelSlide">Project Type</span>
										<ext:Label ID="lblProjectType_r1" runat="server" Text="Adquisitions"></ext:Label>
										<span class="labelSlide">Typology</span>
										<ext:Label ID="lblTypology_r1" runat="server" Text="New Site"></ext:Label>
										<span class="labelSlide">State</span>
										<ext:Label ID="lblState_r1" runat="server" Text="Pending to star negotiation"></ext:Label>
									</li>
									<li id="liLnk_registro2">
										<span class="labelSlide">Project Type</span>
										<ext:Label ID="lblProjectType_r2" runat="server" Text="Towers"></ext:Label>
										<span class="labelSlide">Typology</span>
										<ext:Label ID="lblTypology_r2" runat="server" Text="Global"></ext:Label>
										<span class="labelSlide">State</span>
										<ext:Label ID="lblState_r2" runat="server" Text="Kick off"></ext:Label>
									</li>
								</ul>
								<nav class="navDot">
									<ext:Button runat="server" ID="btnDotLinks1" Cls="btnDot" Handler="slider(this);"></ext:Button>
									<ext:Button runat="server" ID="btnDotLinks2" Cls="btnDot" Handler="slider(this);"></ext:Button>
								</nav>

							</div>
							<div id="SliderNotifications" class="slider">
								<h5>Notifications</h5>
								<ul id="ulNotifications">
									<li id="liNot" class="ancla">
										<span class="labelSlide">Recipients</span>
										<ext:Label ID="lblRecipients" runat="server" Text="manuel.ferreira@atrebo.com, marcia.Oliveira@atrebo.com, sofia.sanchez@atrebo.com, alberto.miranda@atrebo.com"></ext:Label>
										<ext:Button ID="btnMoreRecipients" runat="server" Cls="btnMasInfo" >
											<Listeners>
												<MouseOver Handler="allRecipients();"></MouseOver>
											</Listeners>
										</ext:Button>
										<div id="dvAllRecipients" runat="server" class="d-none" >
											
										</div>
										<span class="labelSlide">Profiles</span>
										<ext:Label ID="lblProfiles" runat="server" Text="Admin_Adquisitions"></ext:Label>
										<span class="labelSlide">Message</span>
										<ext:Label ID="lblMessage" runat="server" Text="The documentation is ok. It’s time to
delivery to the administration. Maximum term: 5 days. This procedure is very important. Don't forget.">
										</ext:Label>
										<ext:Button ID="btnMoreMessage" runat="server" Cls="btnMasInfo" >
											<Listeners>
												<MouseOver Handler="entireMessage();"></MouseOver>
											</Listeners>
										</ext:Button>
										<div id="dvEntireMessage" runat="server"  class="d-none">
											
										</div>
										<span class="labelSlide"></span>
										<ext:Label ID="lblSendTo" runat="server" Text="Send to: Agency, Profiles, Recipients"></ext:Label>
									</li>

								</ul>

							</div>
							<div id="SliderDocuments" class="slider">
								<h5>Documents</h5>
								<ul id="ulDocuments">
									<li id="liDocs_registro1">
										<span class="labelSlide">Documento</span>
										<ext:Label ID="lblDocumento" Cls="d-blk lblDocumento" runat="server" Text="Administrative License"></ext:Label>
										<ext:Label ID="lblRequired" runat="server" IconCls="ico-accept" Cls="lblAttribDoc" Text="Required"></ext:Label>
										<ext:Label ID="lblValidated" runat="server" IconCls="ico-cancel"  Cls="lblAttribDoc" Text="Validated"></ext:Label>
									</li>
									<li id="liDocs_registro2">
										<span class="labelSlide">Documento</span>
										<ext:Label ID="lblDocumento_r2" Cls="d-blk lblDocumento" runat="server" Text="Contractual compliance"></ext:Label>
										<ext:Label ID="lblRequired_r2" runat="server" IconCls="ico-cancel" Cls="lblAttribDoc" Text="Casual"></ext:Label>
										<ext:Label ID="lblValidated_r2" runat="server" IconCls="ico-accept" Cls="lblAttribDoc" Text="Validated"></ext:Label>
									</li>
								</ul>
								<nav class="navDot">
									<ext:Button runat="server" ID="btnDotDocs1" Cls="btnDot" Handler="slider(this);"></ext:Button>
									<ext:Button runat="server" ID="btnDotDocs2" Cls="btnDot" Handler="slider(this);"></ext:Button>
								</nav>
							</div>
							<div id="SliderFunctionalities" class="slider">
								<h5>Functionalities</h5>
								<ul id="ulFunctionalities">
									<li id="liFunc_registro1">
										<span class="labelSlide">Module</span>
										<ext:Label ID="lblModule" runat="server" Text="Legal User"></ext:Label>
										<span class="labelSlide">Functionality</span>
										<ext:Label ID="Label1" runat="server" Text="Acceso_Cliente_A_Legalizaciones"></ext:Label>
									</li>
								</ul>
							</div>


						</Content>
					</ext:Panel>
				</Items>
			</ext:Viewport>
			<%--FIN  VIEWPORT --%>
		</div>
	</form>
</body>
</html>
