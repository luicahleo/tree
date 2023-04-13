<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormGestionElementos.ascx.cs" Inherits="TreeCore.Componentes.FormGestionElementos" %>

<script type="text/javascript" src="../../Componentes/js/FormGestionElementos.js"></script>
<!--<script type="text/javascript" src="/JS/common.js"></script>-->


<%--Stores--%>



<%--Componente--%>

<ext:Container ID="ctMain2" runat="server" Layout="FitLayout"
    HeightSpec="100%">
    <Items>
        <ext:Hidden runat="server" ID="hdCatID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdCatPadreID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdElementoPadreID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdElementoID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdOperadorID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdPlantillaID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdCodigoAutogenerado"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdCondicionCodigoReglaID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdNombreAutogenerado"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdCondicionNombreReglaID"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdProyectoTipo"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdHistoricoInventario"></ext:Hidden>

        <ext:FormPanel
            Hidden="false"
            runat="server"
            ForceFit="true"
            ID="pnConfigurador"
            Layout=""
            HeightSpec="100%"
            OverflowX="Auto"
            OverflowY="Auto">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" />
            </LayoutConfig>
            <DockedItems>
                <ext:Toolbar runat="server" Dock="Bottom" Cls="tbGrey">
                    <Items>
                        <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button runat="server"
                            ID="btnCancelarAgregarEditar"
                            Text="<%$ Resources:Comun, btnCancelar.Text %>"
                            Cls=" btn-secondary">
                            <Listeners>
                                <Click Fn="btnCancelarFormElementos" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            ID="btnGuardarAgregarEditar"
                            Text="<%$ Resources:Comun, btnGuardar.Text %>"
                            Cls="btn-ppal"
                            CausesValidation="true"
                            Focusable="false"
                            Disabled="true"
                            ValidationGroup="FORM">
                            <Listeners>
                                <Click Fn="btnGuardarFormElementos" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>

            </DockedItems>

            <Items>
                <ext:Container runat="server"
                    Cls="gridFormBasic" ID="flexContainer">

                    <Items>
                        <ext:TextField runat="server"
                            ID="txtNombreElemento"
                            meta:resourceKey="txtNombreElemento"
                            FieldLabel="<%$ Resources:Comun, strNombre %>"
                            LabelAlign="Top"
                            Text=""
                            MaxLength="40"
                            Cls="item-form"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM">
                            <Listeners>
                                <ValidityChange Fn="FormularioValidoInventario" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField runat="server"
                            ID="txtCodigoElemento"
                            meta:resourceKey="txtCodigoElemento"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            LabelAlign="Top"
                            Text=""
                            MaxLength="40"
                            Cls="item-form"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM">
                            <Listeners>
                                <ValidityChange Fn="FormularioValidoInventario" />
                            </Listeners>
                        </ext:TextField>
                        <ext:ComboBox runat="server"
                            ID="cmbEstado"
                            DisplayField="Nombre"
                            Mode="Local"
                            QueryMode="Local"
                            LabelAlign="Top"
                            ValueField="InventarioElementoAtributoEstadoID"
                            Cls="item-form"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            FieldLabel="<%$ Resources:Comun, strEstado %>"
                            AllowBlank="false"
                            ValidationGroup="FORM">
                            <CustomConfig>
                                <ext:ConfigItem Name="Tabla" Value="InventarioElementosAtributosEstados" />
                            </CustomConfig>
                            <Store>
                                <ext:Store runat="server"
                                    ID="storeEstados"
                                    AutoLoad="false"
                                    OnReadData="storeEstados_Refresh"
                                    RemoteSort="false">
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Model>
                                        <ext:Model runat="server"
                                            IDProperty="InventarioElementoAtributoEstadoID">
                                            <Fields>
                                                <ext:ModelField Name="InventarioElementoAtributoEstadoID" Type="Int" />
                                                <ext:ModelField Name="Nombre" Type="String" />
                                                <ext:ModelField Name="Codigo" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Fn="SeleccionarComboInventario" />
                                <TriggerClick Fn="RecargarComboInventario" />
                                <Change Fn="FormularioValidoInventario" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>"
                                    Weight="-1" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
                <ext:Container runat="server"
                    Cls="gridFormBasic" ID="Container1">

                    <Items>
                        <ext:ComboBox runat="server"
                            ID="cmbCategoriaElemento"
                            DisplayField="InventarioCategoria"
                            LabelAlign="Top"
                            Mode="Local"
                            QueryMode="Local"
                            ValueField="InventarioCategoriaID"
                            Cls="item-form"
                            AllowBlank="false"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            FieldLabel="<%$ Resources:Comun, strCategoria %>"
                            ValidationGroup="FORM">
                            <CustomConfig>
                                <ext:ConfigItem Name="Tabla" Value="InventarioCategorias" />
                            </CustomConfig>
                            <Store>
                                <ext:Store runat="server"
                                    ID="storeCategoriasElementos"
                                    AutoLoad="false"
                                    OnReadData="storeCategorias_Refresh"
                                    RemoteSort="false">
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Model>
                                        <ext:Model runat="server"
                                            IDProperty="CategoriaID">
                                            <Fields>
                                                <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                                                <ext:ModelField Name="InventarioCategoria" Type="String" />
                                                <ext:ModelField Name="Codigo" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Fn="SeleccionarCategoria" />
                                <TriggerClick Fn="RecargarCategoria" />
                                <Change Fn="FormularioValidoInventario" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>"
                                    Weight="-1" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="cmbOperador"
                            DisplayField="Nombre"
                            LabelAlign="Top"
                            ValueField="OperadorID"
                            AllowBlank="false"
                            Cls="item-form"
                            Mode="Local"
                            QueryMode="Local"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            FieldLabel="<%$ Resources:Comun, strOperador %>">
                            <CustomConfig>
                                <ext:ConfigItem Name="Tabla" Value="vw_EntidadesOperadores" />
                            </CustomConfig>
                            <Store>
                                <ext:Store runat="server"
                                    ID="storeOperadores"
                                    AutoLoad="false"
                                    OnReadData="storeOperadores_Refresh"
                                    RemoteSort="false">
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Model>
                                        <ext:Model runat="server"
                                            IDProperty="OperadorID">
                                            <Fields>
                                                <ext:ModelField Name="OperadorID" Type="Int" />
                                                <ext:ModelField Name="Nombre" Type="String" />
                                                <ext:ModelField Name="Codigo" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Fn="SeleccionarComboInventario" />
                                <TriggerClick Fn="RecargarComboInventario" />
                                <Change Fn="FormularioValidoInventario" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>"
                                    Weight="-1" />
                            </Triggers>
                        </ext:ComboBox>
                        <ext:ComboBox runat="server"
                            ID="cmbPlantilla"
                            DisplayField="Nombre"
                            LabelAlign="Top"
                            ValueField="InventarioElementoID"
                            Cls="item-form"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                            FieldLabel="<%$ Resources:Comun, strPlantilla %>">
                            <Store>
                                <ext:Store runat="server"
                                    ID="storePlantillas"
                                    AutoLoad="false"
                                    OnReadData="storePlantillas_Refresh"
                                    RemoteSort="false">
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                    <Model>
                                        <ext:Model runat="server"
                                            IDProperty="InventarioElementoID">
                                            <Fields>
                                                <ext:ModelField Name="InventarioElementoID" Type="Int" />
                                                <ext:ModelField Name="Nombre" Type="String" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Fn="SeleccionarPlantilla" />
                                <TriggerClick Fn="RecargarPlantilla" />
                                <Change Fn="FormularioValidoInventario" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Hidden="true"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>"
                                    Weight="-1" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" ID="contenedorCategorias" Layout="AutoLayout" Scrollable="Vertical">
                    <Items>
                    </Items>
                </ext:Container>
            </Items>
            <Listeners>
                <ValidityChange Fn="FormularioValidoInventario" />
            </Listeners>
        </ext:FormPanel>

    </Items>
</ext:Container>
