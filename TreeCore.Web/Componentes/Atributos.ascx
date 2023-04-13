<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Atributos.ascx.cs" Inherits="TreeCore.Componentes.Atributos" %>

<%--<script data-require="jquery@*" data-semver="2.1.1" src="../../Scripts/Semaforo/jquery.min.js"></script>--%>
<link data-require="jqueryui@*" data-semver="1.10.0" rel="stylesheet" href="../../Scripts/Semaforo/jquery-ui-1.10.0.custom.min.css" />
<link data-require="bootstrap@*" data-semver="3.2.0" rel="stylesheet" href="../../Scripts/Semaforo/bootstrap.css" />
<script data-require="bootstrap@*" data-semver="3.2.0" src="../../Scripts/Semaforo/bootstrap.js"></script>
<script data-require="jqueryui@*" data-semver="1.10.0" src="../../Scripts/Semaforo/jquery-ui.js"></script>

<link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
<link href="../../Scripts/Semaforo/Style.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/Semaforo/slider.js"></script>

<%--STORES--%>
<ext:Store
    ID="storeColores"
    runat="server"
    AutoLoad="false"
    OnReadData="storeColores_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server" IDProperty="CoreSemaforoColorID">
            <Fields>
                <ext:ModelField Name="CoreSemaforoColorID" Type="Int" />
                <ext:ModelField Name="Nombre" />
                <ext:ModelField Name="Color" />
                <ext:ModelField Name="Activo" Type="Boolean" />
                <ext:ModelField Name="Defecto" />
            </Fields>
        </ext:Model>
    </Model>
</ext:Store>

<%--WINDOWS--%>
<%--<ext:Window ID="winSaveSemaforo"
    runat="server"
    Title="<%$ Resources:Comun, winGestion.Title %>"
    Height="350"
    Width="500"
    BodyCls=""
    Scrollable="Vertical"
    Hidden="true">
    <Listeners>
        <Resize Handler="winFormCenterSimple(this);"></Resize>

    </Listeners>
    <DockedItems>
        <ext:Toolbar runat="server"
            ID="Toolbar6"
            Cls=" greytb"
            Dock="Bottom"
            Padding="20">
            <Items>
                <ext:ToolbarFill></ext:ToolbarFill>
                <ext:Button runat="server"
                    ID="btnCancelarSemaforo"
                    Cls="btn-secondary "
                    MinWidth="110"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    Focusable="false"
                    PressedCls="none"
                    Hidden="false">
                    <Listeners>
                        <Click Handler="#{winSaveSemaforo}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarSemaforo"
                    Cls="btn-ppal "
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Focusable="false"
                    PressedCls="none"
                    Hidden="false">
                    <Listeners>
                        <Click Fn="winSaveSemaforoBotonGuardar" />
                    </Listeners>
                </ext:Button>
            </Items>
        </ext:Toolbar>
    </DockedItems>
    <Items>
        <ext:FormPanel
            runat="server"
            ID="formGestn"
            BodyCls="tbGrey"
            Border="false"
            Heigth="300">
            <Items>

                <ext:Panel runat="server"
                    PaddingSpec="15 30 15 30"
                    Cls="tbGrey overflowSlider"
                    BodyCls="tbGrey border1px"
                    BodyPadding="35"
                    ID="contCurrently"
                    Flex="1">
                    <Content>
                        <div id="<%=nombreComponente%>slider" class="slider2 sleep"></div>

                        <div class="col-xs-12 padding-0">
                            <div class="slider-controller">
                                <form role="form" class="form form-inline">
                                    <ext:Container runat="server" Cls="ctForm-content-resp-col3">
                                        <Items>
                                            <ext:ComboBox
                                                ID="cmbColores"
                                                runat="server"
                                                Width="120"
                                                StoreID="storeColores"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strColor %>"
                                                ValueField="CoreSemaforoColorID"
                                                DisplayField="Nombre"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                <Listeners>
                                                    <Select Fn="SeleccionarColor" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:Button runat="server" ID="btnAnadirRango" Disabled="true" Width="120" Height="32" Cls="btn btn-success add btn-ppal marginR" Text="Add Range">
                                                <Listeners>
                                                    <Click Handler="AnadirRango();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnBorrarRango" Disabled="true" Width="120" Height="32" Cls="btn btn-warning remove btn-ppal" Text="Remove Range">
                                                <Listeners>
                                                    <Click Handler="BorrarRango();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </form>
                            </div>
                        </div>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:FormPanel>
    </Items>
</ext:Window>--%>
<%--Componente--%>

<ext:Container runat="server" Cls="mainContainer" ID="mainContainerAtributos">
    <Items>

        <%-- Variables Hidden --%>
        <ext:Hidden ID="hdAtributoID" runat="server" />
        <ext:Hidden ID="hdCategoriaAtributoID" runat="server" />
        <ext:Hidden ID="hdTipoComponente" runat="server" />
        <ext:Hidden ID="hdUltimoValorRango" runat="server" />

        <ext:Container
            runat="server"
            Cls="sideButtons">
            <Items>
                <ext:Button ID="btnMoverAtributo"
                    runat="server"
                    OverCls="none"
                    PressedCls="none"
                    Cls="ico-drag-horizontal btnMoverAtributo">
                </ext:Button>
            </Items>
        </ext:Container>
        <ext:Container runat="server" Cls="headerContainerFlex">
            <Items>
                <ext:TextField runat="server" Cls="headerAligne txtLblAttribute" ID="lbNombreAtr" Border="false" Regex="/^[a-zA-Z0-9 ()À-ÿ/,]{1,25}$/">
                    <Listeners>
                        <FocusLeave Fn="CambiarNombreAtributo" />
                        <Change Fn="ModificarNombreAtributo" />
                    </Listeners>
                    <ToolTips>
                        <ext:ToolTip runat="server" Html="<%$ Resources:Comun, strMensajeNombreAtributo %>" />
                    </ToolTips>
                </ext:TextField>
                <ext:Button runat="server" IconCls="ico-more-info" Cls="btnMoreAligned" ID="btnOpciones">
                    <Listeners>
                        <Click Handler="MostrarItems(this, #{hdTipoComponente}.getValue())" />
                    </Listeners>
                    <Menu>
                        <ext:Menu runat="server">
                            <Items>
                                <ext:MenuItem Disabled="true" runat="server" ID="menuItemConfiguracionDatos" IconCls="ico-gestionV2" Text="<%$ Resources:Comun, strConfiguracionDatos %>" Cls="borderTopV1">
                                    <Listeners>
                                        <Click Fn="AbrirVentanaDataSetting" />
                                        <%--<BeforeRender Fn="MostrarDataSetting" />--%>
                                    </Listeners>
                                </ext:MenuItem>
                                <ext:MenuItem runat="server" ID="menuItemFormato" IconCls="ico-checkedV2" Text="<%$ Resources:Comun, strFormato %>">
                                    <Listeners>
                                        <Click Fn="AbrirVentanaFormatos" />
                                    </Listeners>
                                </ext:MenuItem>
                                <ext:MenuItem Hidden="true" runat="server" ID="menuItemCondicion" IconCls="ico-functionality" Text="<%$ Resources:Comun, strCondicion %>" Cls="borderTopV1">
                                    <Listeners>
                                        <Click Fn="GridAddCondition" />
                                    </Listeners>
                                </ext:MenuItem>
                                <ext:MenuItem runat="server" ID="menuItemTrafficLight" IconCls="ico-criticidad" Text="<%$ Resources:Comun, strTrafficLight %>" Cls="borderTopV1" Hidden="true">
                                    <Listeners>
                                        <Click Fn="WinTrafficLight" />
                                    </Listeners>
                                </ext:MenuItem>
                                <ext:MenuItem runat="server" ID="menuItemPerfiles" IconCls="ico-group" Text="<%$ Resources:Comun, strRoles %>" Cls="borderTopV1">
                                    <Listeners>
                                        <Click Fn="AbrirVentanaRestricciones" />
                                    </Listeners>
                                </ext:MenuItem>
                                <ext:MenuItem runat="server" ID="menuItemEliminar" IconCls="ico-trash" Text="<%$ Resources:Comun, btnEliminar.ToolTip %>" Cls="borderTopV1 spanRed">
                                    <Listeners>
                                        <Click Fn="EliminarAtributo" />
                                    </Listeners>
                                </ext:MenuItem>
                            </Items>
                        </ext:Menu>
                    </Menu>
                </ext:Button>
            </Items>
        </ext:Container>
        <ext:Container runat="server" Hidden="true">
            <Items>
                <ext:Menu runat="server">
                    <Items>
                        <ext:Label runat="server" Text="<%$ Resources:Comun, strPlantillas %>" Width="210" Cls="labelContent" Height="30" />
                        <ext:MenuItem runat="server" Text="Critic in Site" Cls="borderTop" />
                        <ext:MenuItem runat="server" Text="Impact in Site" Cls="borderTop" />
                    </Items>
                </ext:Menu>
            </Items>
        </ext:Container>
        <ext:Container runat="server" Cls="flexItems">
            <Items>
                <ext:Container
                    runat="server"
                    ID="contenedorControl">
                    <Items>
                    </Items>
                </ext:Container>
                <ext:Container
                    runat="server"
                    Cls="sideButtons"
                    Hidden="true">
                    <Items>
                        <ext:Button ID="btnMasOrden"
                            runat="server"
                            IconCls="ico-expand-combo"
                            Hidden="true"
                            ToolTip="<%$ Resources:Comun, strBeforeText %>"
                            Cls="positionFirstButton">
                            <Listeners>
                                <Click Fn="OrdenMasAtributo"></Click>
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenosOrden"
                            runat="server"
                            IconCls="ico-expand-combo"
                            Hidden="true"
                            ToolTip="<%$ Resources:Comun, strAfterText %>"
                            Cls="positionSecondButton">
                            <Listeners>
                                <Click Fn="OrdenMenosAtributo"></Click>
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Container>
            </Items>
        </ext:Container>
    </Items>
</ext:Container>
