<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inflaciones.aspx.cs" Inherits="TreeCore.PaginasComunes.Inflaciones" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Inflaciones</title>
	<link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
	<link href="css/styleGlobal.css" rel="stylesheet" type="text/css" />
	<link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
	<script type="text/javascript" src="js/Inflaciones.js"></script>
	<!--<script type="text/javascript" src="js/common.js"></script>-->
</head>
<body>
	<form id="form1" runat="server">

		<%-- INICIO HIDDEN --%>

		<ext:Hidden runat="server" ID="hdLocale" />
		<ext:Hidden runat="server" ID="hdCliID" />
		<ext:Hidden runat="server" ID="hdProAgID" />
		<ext:Hidden runat="server" ID="InflacionID" />
		<ext:Hidden runat="server" ID="ModuloID" />

		<%-- FIN HIDDEN --%>

		<ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
		</ext:ResourceManager>

		<%-- INICIO STORES --%>

		<ext:Store runat="server" ID="storePrincipal" AutoLoad="true" OnReadData="storePrincipal_Refresh" RemoteSort="true">
			<Listeners>
				<BeforeLoad Fn="DeseleccionarGrilla" />
			</Listeners>
			<Proxy>
				<ext:PageProxy />
			</Proxy>
			<Model>
				<ext:Model runat="server" IDProperty="InflacionID">
					<Fields>
						<ext:ModelField Name="InflacionID" Type="Int" />
						<ext:ModelField Name="Inflacion" />
						<ext:ModelField Name="Pais" />
						<ext:ModelField Name="PaisID" Type="Int" />
						<ext:ModelField Name="Estandar" Type="Boolean" />
						<ext:ModelField Name="Descripcion" />
					</Fields>
				</ext:Model>
			</Model>
			<Sorters>
				<ext:DataSorter Property="Inflacion" Direction="ASC" />
			</Sorters>
		</ext:Store>

		<ext:Store runat="server" ID="storeDetalle" AutoLoad="false" OnReadData="storeDetalle_Refresh" RemoteSort="true">
			<Listeners>
				<BeforeLoad Fn="DeseleccionarGrillaDetalle" />
			</Listeners>
			<Proxy>
				<ext:PageProxy />
			</Proxy>
			<Model>
				<ext:Model runat="server" IDProperty="InflacionDetalleID">
					<Fields>
						<ext:ModelField Name="InflacionDetalleID" Type="Int" />
						<ext:ModelField Name="InflacionID" Type="Int" />
						<ext:ModelField Name="Mes" Type="Int" />
						<ext:ModelField Name="Anualidad" Type="Int" />
						<ext:ModelField Name="Valor" Type="Float" />
						<ext:ModelField Name="Activo" Type="Boolean" />
						<ext:ModelField Name="FechaDesde" Type="Date" />
						<ext:ModelField Name="FechaHasta" Type="Date" />
						<ext:ModelField Name="Anual" Type="Float" />
						<ext:ModelField Name="Interanual" Type="Float" />
						<ext:ModelField Name="Trimestral" Type="Float" />
						<ext:ModelField Name="Semestral" Type="Float" />
						<ext:ModelField Name="Cuatrimestral" Type="Float" />
						<ext:ModelField Name="Acumulado" Type="Float" />
					</Fields>
				</ext:Model>
			</Model>
			<Sorters>
				<ext:DataSorter Property="InflacionDetalleID" Direction="ASC" />
			</Sorters>
		</ext:Store>

		<ext:Store runat="server" ID="storePaises" AutoLoad="true" OnReadData="storePaises_Refresh" RemoteSort="false">
			<Proxy>
				<ext:PageProxy />
			</Proxy>
			<Model>
				<ext:Model runat="server" IDProperty="PaisID">
					<Fields>
						<ext:ModelField Name="PaisID" Type="Int" />
						<ext:ModelField Name="Pais" />
					</Fields>
				</ext:Model>
			</Model>
			<Sorters>
				<ext:DataSorter Property="Pais" Direction="ASC" />
			</Sorters>
		</ext:Store>

		<ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientes_Refresh" RemoteSort="false">
			<Proxy>
				<ext:PageProxy />
			</Proxy>
			<Model>
				<ext:Model runat="server" IDProperty="ClienteID">
					<Fields>
						<ext:ModelField Name="ClienteID" Type="Int" />
						<ext:ModelField Name="Cliente" />
					</Fields>
				</ext:Model>
			</Model>
			<Sorters>
				<ext:DataSorter Property="Cliente" Direction="ASC" />
			</Sorters>
			<Listeners>
				<Load Handler="CargarStores();" />
			</Listeners>
		</ext:Store>

		<%-- FIN STORES --%>

		<%-- INICIO WINDOWS --%>

		<ext:Window runat="server" 
			ID="winGestion"
			meta:resourceKey="winGestion"
			Title="Agregar" 
			Width="500" 
			Modal="true"
			Hidden="true" >
			<Items>
				<ext:FormPanel runat="server" 
					ID="formGestion" 
					Cls="form-gestion" 
					BodyStyle="padding:10px;"
					Border="false">
					<Items>
						<ext:TextField runat="server" 
							ID="txtInflacion"
							meta:resourceKey="txtInflacion" 
							FieldLabel="Inflacion" 
							Text="" 
							MaxLength="50"
							MaxLengthText="Ha superado el máximo número de caracteres" 
							ValidationGroup="FORM"
							CausesValidation="true" />
						<ext:ComboBox runat="server" 
							ID="cmbPais" 
							meta:resourceKey="cmbPais" 
							StoreID="storePaises" 
							Width="350"
							DisplayField="Pais" 
							ValueField="PaisID" 
							EmptyText="Seleccione un pais" 
							Editable="false" 
							FieldLabel="Paises">
							<Triggers>
								<ext:FieldTrigger meta:resourceKey="LimpiarLista" 
									Icon="Clear" 
									QTip="Limpiar Lista" />
								<ext:FieldTrigger meta:resourceKey="RecargarLista" 
									IconCls="ico-reload" 
									QTip="Recargar Lista" />
							</Triggers>
							<Listeners>
								<TriggerClick Fn="TriggerPaises" />
							</Listeners>
						</ext:ComboBox>
						<ext:Checkbox runat="server" 
							ID="ckEstandar" 
							meta:resourceKey="ckEstandar" 
							FieldLabel="Estándar" 
							ValidationGroup="FORM"
							CausesValidation="true" />
						<ext:TextField runat="server" 
							ID="txtDescripcion" 
							meta:resourceKey="txtDescripcion" 
							FieldLabel="Descripción" 
							Text="" 
							MaxLength="200"
							MaxLengthText="Ha superado el máximo número de caracteres" 
							ValidationGroup="FORM"
							CausesValidation="true" />
					</Items>
					<Listeners>
						<ValidityChange Handler="FormularioValido(valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Buttons>
				<ext:Button runat="server" 
					ID="btnCancelar" 
					meta:resourceKey="btnCancelar"
					Text="Cancelar" 
					IconCls="ico-cancel" 
					Cls="btn-cancel" >
					<Listeners>
						<Click Handler="#{winGestion}.hide();" />
					</Listeners>
				</ext:Button>
				<ext:Button runat="server" 
					ID="btnGuardar" 
					meta:resourceKey="btnGuardar"
					Text="Guardar" 
					IconCls="ico-accept" 
					Cls="btn-accept" >
					<Listeners>
						<Click Handler="winGestionBotonGuardar();" />
					</Listeners>
				</ext:Button>

			</Buttons>
		</ext:Window>

		<ext:Window runat="server" 
			ID="winGestionDetalle"
			meta:resourceKey="winGestionDetalle"
			Width="400" 
			Resizable="false" 
			Modal="true"
			Hidden="true" >

			<Items>
				<ext:FormPanel runat="server" 
					ID="formGestionDetalle" 
					Cls="form-detalle" 
					BodyStyle="padding:10px;"
					Border="false">
			<FieldDefaults LabelAlign="Top" ></FieldDefaults>
					<Items>
						<ext:FieldContainer runat="server" Layout="HBox">
							<Items>
								<ext:NumberField runat="server" 
									ID="txtMes" 
									meta:resourceKey="txtMes" 
									FieldLabel="Mes" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" />
								<ext:NumberField runat="server" 
									ID="txtAnualidad" 
									meta:resourceKey="txtAnualidad" 
									FieldLabel="Anualidad" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server" Layout="HBox">
							<Items>
								<ext:DateField runat="server" 
									ID="dateDesde" 
									meta:resourceKey="dateDesde" 
									FieldLabel="Fecha Desde" 
									Hidden="false"
									ValidationGroup="FORM" 
									CausesValidation="true" />
								<ext:DateField runat="server" 
									ID="dateHasta" 
									meta:resourceKey="dateHasta" 
									FieldLabel="Fecha Hasta" 
									Hidden="false"
									ValidationGroup="FORM" 
									CausesValidation="true" />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server" Layout="HBox">
							<Items>
								<ext:NumberField runat="server" 
									ID="txtValor" 
									meta:resourceKey="txtValor" 
									FieldLabel="Valor Mensual" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
								<ext:NumberField runat="server" 
									ID="txtValorAnual" 
									meta:resourceKey="txtValorAnual" 
									FieldLabel="Valor Anual" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server" Layout="HBox">
							<Items>
								<ext:NumberField runat="server" 
									ID="txtValorInteranual" 
									meta:resourceKey="txtValorInteranual" 
									FieldLabel="Valor Interanual" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
								<ext:NumberField runat="server" 
									ID="txtValorTrimestral" 
									meta:resourceKey="txtValorTrimestral" 
									FieldLabel="Valor Trimestral" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
							</Items>
						</ext:FieldContainer>
						<ext:FieldContainer runat="server" Layout="HBox">
							<Items>
								<ext:NumberField runat="server" 
									ID="txtValorCuatrimestral" 
									meta:resourceKey="txtValorCuatrimestral" 
									FieldLabel="Valor Cuatrimestral" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
								<ext:NumberField runat="server" 
									ID="txtValorSemestral" 
									meta:resourceKey="txtValorSemestral" 
									FieldLabel="Valor Semestral" 
									Text="" 
									MaxLength="500"
									MaxLengthText="Ha superado el máximo número de caracteres" 
									ValidationGroup="FORM"
									CausesValidation="true" 
									DecimalPrecision="4" 
									DecimalSeparator="," />
							</Items>
						</ext:FieldContainer>
						<ext:NumberField runat="server" 
							ID="txtValorAcumulado" 
							meta:resourceKey="txtValorAcumulado" 
							FieldLabel="Valor Acumulado" 
							Text="" 
							MaxLength="500"
							MaxLengthText="Ha superado el máximo número de caracteres" 
							ValidationGroup="FORM"
							CausesValidation="true" 
							DecimalPrecision="4" 
							DecimalSeparator="," />
					</Items>
					<Listeners>
						<ValidityChange Handler="FormularioValidoDetalle(valid);" />
					</Listeners>
				</ext:FormPanel>
			</Items>
			<Buttons>
				<ext:Button runat="server" 
					ID="btnCancelarDetalle" 
					meta:resourceKey="btnCancelar"
					IconCls="ico-cancel" 
					Cls="btn-cancel" >
					<Listeners>
						<Click Handler="#{winGestionDetalle}.hide();" />
					</Listeners>
				</ext:Button>
				<ext:Button runat="server" 
					ID="btnGuardarDetalle"
					meta:resourceKey="btnGuardar"
					IconCls="ico-accept" 
					Cls="btn-accept" >
					<Listeners>
						<Click Handler="winGestionBotonGuardarDetalle();" />
					</Listeners>
				</ext:Button>

			</Buttons>
			<Listeners>
				<Show Handler="#{winGestionDetalle}.center();" />
			</Listeners>
		</ext:Window>

		<%-- FIN WINDOWS --%>

		<%-- INICIO VIEWPORT --%>

		<ext:Viewport runat="server" ID="vwContenedor" Cls="vwContenedor" Layout="Anchor">
			<Items>
				<ext:GridPanel
					runat="server"
					ID="gridMaestro"
					StoreID="storePrincipal"
					Cls="gridPanel"
					Title="Inflaciones"
					EnableColumnHide="false"
					SelectionMemory="false"
					AnchorHorizontal="-100" 
					AnchorVertical="58%"
					AriaRole="main">

					<DockedItems>
						<ext:Toolbar runat="server" 
							ID="tlbBase" 
							Dock="Top" 
							Cls="tlbGrid">
							<Items>
								<ext:Button runat="server" 
									ID="btnAnadir" 
									Cls="btn-trans" 
									AriaLabel="Añadir" 
									ToolTip="Añadir" 
									Handler="AgregarEditar();" />
								<ext:Button runat="server" 
									ID="btnEditar" 
									Cls="btn-trans" 
									AriaLabel="Editar" 
									ToolTip="Editar" 
									Handler="MostrarEditar();" />
								<ext:Button runat="server" 
									ID="btnEliminar" 
									Cls="btn-trans" 
									AriaLabel="Eliminar" 
									ToolTip="Eliminar" 
									Handler="Eliminar();" />
								<ext:Button runat="server" 
									ID="btnRefrescar" 
									Cls="btn-trans" 
									AriaLabel="Refrescar" 
									ToolTip="Refrescar" 
									Handler="refrescar();" />
								<ext:Button runat="server" 
									ID="btnDescargar" 
									Cls="btn-trans" 
									AriaLabel="Descargar" 
									ToolTip="Descargar" 
									Handler="ExportarDatos('Inflaciones', #{gridMaestro}, '-1');" />
							</Items>
						</ext:Toolbar>
						<ext:Toolbar runat="server" 
							ID="tlbClientes" 
							Dock="Top">
							<Items>
								<ext:ComboBox runat="server" 
									ID="cmbClientes" 
									StoreID="storeClientes" 
									Hidden="true" 
									ValueField="ClienteID" 
									Cls="comboGrid pos-boxGrid" 
									EmptyText="Clientes"
									DisplayField="Cliente" 
									FieldLabel="Cliente">
									<Listeners>
										<Select Handler="SeleccionarCliente();" />
										<TriggerClick Handler="RecargarClientes();" />
									</Listeners>
									<Triggers>
										<ext:FieldTrigger meta:resourceKey="RecargarLista" 
											IconCls="gen_TriggerReload"
											QTip="Recargar Lista" />
									</Triggers>
								</ext:ComboBox>
							</Items>
						</ext:Toolbar>
					</DockedItems>
					<ColumnModel>
						<Columns>
							<ext:Column runat="server" 
								ID="Estandar" 
								meta:resourceKey="colEstandar"
								DataIndex="Estandar" 
								Width="100" 
								Align="Center" 
								Flex="1" >
								<Renderer Fn="ActivoRender" />
							</ext:Column>
							<ext:Column runat="server" 
								ID="Inflacion" 
								meta:resourceKey="InflacionColumn" 
								DataIndex="Inflacion" 
								Align="Start" 
								Hideable="false" 
								Width="100" />
							<ext:Column runat="server" 
								ID="Pais" 
								meta:resourceKey="PaisColumn" 
								DataIndex="Pais" 
								Align="Start" 
								Hideable="false" 
								Width="200" />
							<ext:Column runat="server" 
								ID="Descripcion" 
								meta:resourceKey="columnDescripcion" 
								DataIndex="Descripcion" 
								Align="Start" 
								Hideable="false" 
								Width="400" />
						</Columns>
					</ColumnModel>
					<SelectionModel>
						<ext:RowSelectionModel runat="server" 
							ID="GridRowSelect" 
							EnableViewState="true">
							<Listeners>
								<Select Fn="Grid_RowSelect" />
							</Listeners>
						</ext:RowSelectionModel>
					</SelectionModel>
					<Plugins>
						<ext:GridFilters runat="server" 
							ID="gridFilters" />
					</Plugins>
					<BottomBar>
						<ext:PagingToolbar runat="server" 
							ID="PagingToolBar1" 
							meta:resourceKey="PagingToolBar1"
							StoreID="storePrincipal"
							HideRefresh="true" >
						<Items>
							<ext:ComboBox runat="server" 
								Cls="comboGrid" 
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
				</ext:GridPanel>
				<ext:GridPanel
					runat="server"
					ID="GridDetalle"
					Cls="gridPanel gridDetalle"
					SelectionMemory="false"
					EnableColumnHide="false"
					StoreID="storeDetalle"
					AnchorHorizontal="-100" 
					AnchorVertical="42%"
					AriaRole="main">

					<DockedItems>
						<ext:Toolbar runat="server" 
							ID="tlbDetalle" 
							Dock="Top" 
							Cls="tlbGrid">
							<Items>
								<ext:Button runat="server" 
									ID="btnAnadirDetalle" 
									Cls="btn-trans" 
									AriaLabel="Añadir" 
									ToolTip="Añadir" 
									Handler="AgregarEditarDetalle()" />
								<ext:Button runat="server" 
									ID="btnEditarDetalle"
									Cls="btn-trans" 
									AriaLabel="Editar" 
									ToolTip="Editar" 
									Handler="MostrarEditarDetalle()" />
								<ext:Button runat="server" 
									ID="btnEliminarDetalle"
									Cls="btn-trans" 
									AriaLabel="Eliminar" 
									ToolTip="Eliminar" 
									Handler="EliminarDetalle()" />
								<ext:Button runat="server" 
									ID="btnActivarDetalle" 
									Cls="btn-trans" 
									AriaLabel="Activar" 
									ToolTip="Activar" 
									Handler="ActivarDetalle()" />
								<ext:Button runat="server" 
									ID="btnRefrescarDetalle" 
									Cls="btn-trans" 
									AriaLabel="Refrescar" 
									ToolTip="Refrescar" 
									Handler="refrescarDetalle()" />
								<ext:Button runat="server" 
									ID="btnDescargarDetalle" 
									Cls="btn-trans" 
									AriaLabel="Descargar" 
									ToolTip="Descargar" 
									Handler="ExportarDatos('Inflaciones', #{GridDetalle}, #{ModuloID}.value);" />
								<%--<ext:ButtonGroup runat="server" ID="btnActivarDetalle" Cls="btn-trans" Columns="1" AriaLabel="" >
									<Items>
										<ext:Button runat="server" ID="btnActivo" AriaLabel="Activar" />
										<ext:Button runat="server" ID="btnDesactivado" AriaLabel="Desactivar" Checked="true" />
									</Items>
								</ext:ButtonGroup>--%>
							</Items>
						</ext:Toolbar>
					</DockedItems>
					<ColumnModel>
						<Columns>
							<ext:Column runat="server" 
								ID="Activo" 
								meta:resourceKey="columnActivo"
								DataIndex="Activo" 
								Width="60" 
								Align="Center" >
								<Renderer Fn="ActivoRender" />
							</ext:Column>
							<ext:DateColumn runat="server" 
								ID="FechaDesde" 
								meta:resourceKey="columFechaDesde" 
								DataIndex="FechaDesde" 
								Width="100" 
								Format="dd/MM/yyyy" 
								Align="Center" />
							<ext:DateColumn runat="server" 
								ID="FechaHasta" 
								meta:resourceKey="columFechaHasta" 
								DataIndex="FechaHasta" 
								Width="100" 
								Format="dd/MM/yyyy" 
								Align="Center" />
							<ext:Column runat="server" 
								ID="Mes"
								meta:resourceKey="columnMes" 
								DataIndex="Mes" 
								Width="80" 
								Align="Start" />
							<ext:Column runat="server" 
								ID="Anualidad" 
								meta:resourceKey="columnAnualidad" 
								DataIndex="Anualidad" 
								Width="80" 
								Align="Start" />
							<ext:NumberColumn runat="server" 
								ID="Valor"
								meta:resourceKey="columnValor" 
								DataIndex="Valor" 
								Align="End" 
								Width="70" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Anual" 
								meta:resourceKey="columnAnual" 
								DataIndex="Anual"
								Align="End" 
								Width="100" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Interanual" 
								meta:resourceKey="columnInteranual" 
								DataIndex="Interanual" 
								Align="End" 
								Width="100" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Trimestral" 
								meta:resourceKey="columnTrimestral" 
								DataIndex="Trimestral" 
								Align="End" 
								Width="100" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Cuatrimestral"
								meta:resourceKey="columnCuatrimestral" 
								DataIndex="Cuatrimestral" 
								Align="End" 
								Width="100" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Semestral"
								meta:resourceKey="columnSemestral" 
								DataIndex="Semestral"
								Align="End" 
								Width="100" 
								Format="0.00" />
							<ext:NumberColumn runat="server" 
								ID="Acumulado" 
								meta:resourceKey="columnAcumulado" 
								DataIndex="Acumulado" 
								Align="End" 
								Width="100" 
								Format="0.00" />
						</Columns>
					</ColumnModel>
					<SelectionModel>
						<ext:RowSelectionModel runat="server" 
							ID="GridRowSelectDetalle" 
							EnableViewState="true">
							<Listeners>
								<Select Fn="Grid_RowSelect_Detalle" />
								<Deselect Fn="DeseleccionarGrillaDetalle" />
							</Listeners>
						</ext:RowSelectionModel>
					</SelectionModel>
					<Plugins>
						<ext:GridFilters runat="server" 
							ID="gridFilters2" />
					</Plugins>
					<BottomBar>
						<ext:PagingToolbar runat="server" 
							ID="PagingToolBar2" 
							meta:resourceKey="PagingToolBar1"
							StoreID="storeDetalle" 
							HideRefresh="true" >
						<Items>
							<ext:ComboBox runat="server"
								Cls="comboGrid" 
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
									<Select Fn="handlePageSizeSelectDetalle" />
								</Listeners>
							</ext:ComboBox>
						</Items>
						</ext:PagingToolbar>
					</BottomBar>
				</ext:GridPanel>
			</Items>
		</ext:Viewport>

		<%-- FIN VIEWPORT --%>
		<div>
		</div>

	</form>
</body>
</html>
