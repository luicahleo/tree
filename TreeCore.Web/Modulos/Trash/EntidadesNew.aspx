<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntidadesNew.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EntidadesNew" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>


<%@ Register Src="/Componentes/GridEmplazamientosContactos.ascx" TagName="GridContactosGlobales" TagPrefix="local" %>

<%@ Register Src="/Componentes/Localizaciones.ascx" TagName="Localizaciones" TagPrefix="local" %>

<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactosEmplazamientos" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>

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
                    <WindowResize Handler="ColOverrideControl();" />
                    <DocumentReady Handler="setClienteIDComponentes();"></DocumentReady>
                </Listeners>


            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <link href="css/Entidades.css" rel="stylesheet" type="text/css" />
            <!--<script src="../../JS/common.js"></script>-->
            <script type="text/javascript" src="../../Componentes/js/Localizaciones.js"></script>
            <script type="text/javascript" src="../../Componentes/js/toolbarFiltros.js"></script>
            <script type="text/javascript" src="../../Componentes/js/FormContactos.js"></script>
            <script type="text/javascript" src="../../Componentes/js/GridEmplazamientosContactosResp.js"></script>
            <script type="text/javascript" src="js/EntidadesNew.js"></script>
            <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
            <link href="css/styleEntidadesNew.css" rel="stylesheet" type="text/css" />
            <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />

            <ext:Menu runat="server"
                ID="ContextMenuTreeL">
                <Items>
                    <ext:MenuItem runat="server"
                        IconCls="ico-CntxMenuExcel"
                        Text="Download Filled Template"
                        ID="ShowMap" />
                    <ext:MenuItem runat="server"
                        IconCls="ico-CntxPaginaDoc"
                        Text="Download Log"
                        ID="ShowDashboard"
                        Hidden="false" />

                </Items>
                <Listeners>
                    <%--  <Click Fn="OpcionSeleccionada" />--%>
                </Listeners>
            </ext:Menu>
            <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden runat="server" ID="hdEntidadID" />
            <ext:Hidden runat="server" ID="hdControlProveedor" />
            <ext:Hidden runat="server" ID="hdControlOperador" />
            <ext:Hidden runat="server" ID="hdControlEmpresaProveedora" />
            <ext:Hidden runat="server" ID="hdControlFormulario" />
            <ext:Hidden ID="CurrentSet" runat="server" Text="Alls" />
            <ext:Hidden ID="CurrentControl" runat="server" />
            <ext:Hidden ID="hdControlURL" runat="server" />
            <ext:Hidden ID="hdControlName" runat="server" />

            <%--STORES--%>

            <ext:Store
                ID="storePrincipal"
                runat="server"
                OnReadData="storePrincipal_Refresh"
                RemoreSort="true"
                RemotePaging="false">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EntidadID" />
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EntidadID" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeEntidadTipo"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeEntidadTipo_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EntidadTipoID">
                        <Fields>
                            <ext:ModelField Name="EntidadTipoID" Type="Int" />
                            <ext:ModelField Name="EntidadTipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EntidadTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeMetodosPago"
                AutoLoad="false"
                OnReadData="storeMetodosPago_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="MetodoPagoID">
                        <Fields>
                            <ext:ModelField Name="MetodoPagoID" Type="Int" />
                            <ext:ModelField Name="MetodoPago" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeTiposContribuyentes"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeTiposContribuyentes_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="TipoContribuyenteID">
                        <Fields>
                            <ext:ModelField Name="TipoContribuyenteID" Type="Int" />
                            <ext:ModelField Name="TipoContribuyente" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPTipoNIF"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPTipoNIF_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPTipoNIFID">
                        <Fields>
                            <ext:ModelField Name="SAPTipoNIFID" Type="Int" />
                            <ext:ModelField Name="Descripcion" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPTratamientos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPTratamientos_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPTratamientoID">
                        <Fields>
                            <ext:ModelField Name="SAPTratamientoID" Type="Int" />
                            <ext:ModelField Name="SAPTratamiento" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPGruposCuentas"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPGruposCuentas_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPGrupoCuentaID">
                        <Fields>
                            <ext:ModelField Name="SAPGrupoCuentaID" Type="Int" />
                            <ext:ModelField Name="SAPGrupoCuenta" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPCuentasAsociadas"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPCuentasAsociadas_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPCuentaAsociadaID">
                        <Fields>
                            <ext:ModelField Name="SAPCuentaAsociadaID" Type="Int" />
                            <ext:ModelField Name="Descripcion" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPClavesClasificaciones"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPClavesClasificaciones_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPClaveClasificacionID">
                        <Fields>
                            <ext:ModelField Name="SAPClaveClasificacionID" Type="Int" />
                            <ext:ModelField Name="Descripcion" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeSAPGruposTesorerias"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeSAPGruposTesorerias_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPGrupoTesoreriaID">
                        <Fields>
                            <ext:ModelField Name="SAPGrupoTesoreriaID" Type="Int" />
                            <ext:ModelField Name="Descripcion" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeCondicionesPagos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCondicionesPagos_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CondicionPagoID">
                        <Fields>
                            <ext:ModelField Name="CondicionPagoID" Type="Int" />
                            <ext:ModelField Name="CondicionPago" />
                        </Fields>
                    </ext:Model>
                </Model>

            </ext:Store>
            <ext:Store
                runat="server"
                ID="storeClientesProyectosTipos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeClientesProyectosTipos_refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="Alias" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeContactosGlobalesEntidad" runat="server" OnReadData="storeContactosGlobalesEntidad_Refresh" AutoLoad="false" RemoteSort="true" PageSize="20" RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ContactoGlobalEntidadID">
                        <Fields>
                            <ext:ModelField Name="Email">
                            </ext:ModelField>
                            <ext:ModelField Name="Telefono">
                            </ext:ModelField>
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store ID="storeModulosEmpresaProveedora" runat="server" OnReadData="storeModulosEmpresaProveedora_Refresh" AutoLoad="false" RemoteSort="true" PageSize="20" RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>

                    <ext:Model runat="server" IDProperty="ProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID">
                            </ext:ModelField>
                            <ext:ModelField Name="Alias">
                            </ext:ModelField>
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--VENTANAS EMERGENTES--%>
            <ext:Window
                runat="server" ID="WinContactsDetails"
                meta:resourceKey="WinContactsDetails"
                Width="480"
                MinHeight="220"
                Hidden="true"
                Title="<%$ Resources:Comun, strContactos %>"
                IconCls="ico-contact"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                X="200"
                Y="200"
                MaxHeight="500"
                OverflowY="Auto">
                <Items>
                    <ext:GridPanel
                        ID="GridContactsD"
                        runat="server"
                        ForceFit="true"
                        Border="false"
                        StoreID="storeContactosGlobalesEntidad"
                        Height="140"
                        Scrollable="Vertical"
                        Header="false"
                        Cls="gridPanel gridPopup">
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server"
                                    Text="<%$ Resources:Comun, strEmail %>"
                                    DataIndex="Email" Width="140">
                                </ext:Column>
                                <ext:Column runat="server"
                                    Text="<%$ Resources:Comun, strTelefono %>"
                                    DataIndex="Telefono"
                                    Width="110">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true">
                            </ext:GridView>
                        </View>

                    </ext:GridPanel>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button runat="server"
                                ID="btnWinContactD"
                                Cls="btn-ppal"
                                Text="Edit">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>

                </Items>


            </ext:Window>

            <ext:Window
                runat="server" ID="WinModulosEmpresaProveedoras"
                meta:resourceKey="WinModulosEmpresaProveedoras"
                Width="480"
                MinHeight="220"
                Hidden="true"
                Title="<%$ Resources:Comun, strEmpresaProveedora %>"
                IconCls="ico-contact"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                X="200"
                Y="200"
                MaxHeight="500"
                OverflowY="Auto">
                <Items>
                    <ext:GridPanel
                        ID="GridPanel1"
                        runat="server"
                        ForceFit="true"
                        Border="false"
                        StoreID="storeModulosEmpresaProveedora"
                        Height="140"
                        Scrollable="Vertical"
                        Header="false"
                        Cls="gridPanel gridPopup">

                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server" Text="<%$ Resources:Comun, strModulo %>" DataIndex="Alias" Width="140">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true">
                            </ext:GridView>
                        </View>

                    </ext:GridPanel>

                </Items>


            </ext:Window>

            <ext:Window ID="WinGestionEntidad"
                runat="server"
                meta:resourceKey="WinGestionEntidad"
                Width="600"
                Height="550"
                Hidden="true"
                Modal="true"
                Centered="true"
                Resizable="false"
                Cls="winForm-resp winForm-respDockBot"
                Title="Añadir nueva entidad"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel ID="formGestionEntidad"
                        Cls="formGris formEntidad"
                        runat="server"
                        Hidden="false">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctFormEntidades"
                                Cls="ctForm-resp ctForm-content-resp-col2">
                                <Content>
                                    <ext:TextField ID="txtCodigo"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strCodigo%>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        Cls="FormularioEntidad"
                                        CausesValidation="true"
                                        meta:resourceKey="txtCodigo">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtNombre"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        Cls="FormularioEntidad"
                                        CausesValidation="true"
                                        meta:resourceKey="txtNombre">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtAlias"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strAlias %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Cls="FormularioEntidad"
                                        meta:resourceKey="txtAlias">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox meta:resourceKey="cmbTipoEntidad"
                                        runat="server"
                                        ID="cmbTipoEntidad"
                                        EmptyText="Select a Type of Entity"
                                        FieldLabel="<%$ Resources:Comun, strTipoEntidad %>"
                                        DisplayField="EntidadTipo"
                                        ValueField="EntidadTipoID"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="FormularioEntidad"
                                        Hidden="false"
                                        StoreID="storeEntidadTipo"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:DropDownField runat="server"
                                        meta:resourcekey="txtTelefono1"
                                        ID="txtTelefono1"
                                        FieldLabel="<%$ Resources:Comun, strTelefono %>"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        TriggerCls="icoprefix"
                                        FocusCls="testfocus"
                                        ValidationGroup="FORM"
                                        Cls="FormularioEntidad"
                                        CausesValidation="true"
                                        Regex="/^[[+]([0-9])*]? ([0-9])*$/">
                                        <Component>
                                            <ext:MenuPanel runat="server" ID="MenuPanel1" MaxWidth="120">
                                                <Menu runat="server">
                                                    <Items>
                                                        <ext:MenuItem runat="server" Text="+34 " Icon="FlagEs" />
                                                        <ext:MenuItem runat="server" Text="+43 " Icon="FlagFr" />
                                                        <ext:MenuItem runat="server" Text="+54 " Icon="FlagDe" />
                                                    </Items>
                                                    <Listeners>
                                                        <Click Handler="#{txtTelefono1}.setValue(menuItem.text);" />
                                                    </Listeners>
                                                </Menu>
                                            </ext:MenuPanel>
                                        </Component>
                                    </ext:DropDownField>
                                    <ext:TextField ID="txtEmail"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strEmail %>"
                                        LabelAlign="Top"
                                        MaxLength="200"
                                        ValidationGroup="FORM"
                                        AllowBlank="true"
                                        Vtype="email"
                                        CausesValidation="true"
                                        Cls="FormularioEntidad"
                                        meta:resourceKey="txtEmail">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:NumberField ID="numMaxUsuarios"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strNumMaximoUsuarios %>"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        AllowDecimals="false"
                                        AllowExponential="false"
                                        Cls="FormularioEntidad"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:NumberField>
                                    <local:Localizaciones ID="locGeografica"
                                        runat="server"
                                        Requerido="true"
                                        PaisDefecto="false"
                                        ActRegion="false" />
                                    <ext:TextField ID="txtDireccion"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strDireccion %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Cls="FormularioEntidad"
                                        meta:resourceKey="txtDireccion">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtNumero"
                                        runat="server"
                                        FieldLabel="No"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Cls="formDomicilio"
                                        WidthSpec="9.5%"
                                        meta:resourceKey="txtNumero"
                                        Visible="false" />
                                    <ext:TextField ID="txtPiso"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strPiso %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Cls="formDomicilio"
                                        WidthSpec="9.5%"
                                        meta:resourceKey="txtPiso"
                                        Visible="false" />
                                    <ext:TextField ID="txtPuerta"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strPuerta %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        Cls="formDomicilio"
                                        WidthSpec="9.5%"
                                        meta:resourceKey="txtPuerta"
                                        Visible="false" />
                                    <ext:TextField ID="txtCP"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strCodigoPostal %>"
                                        LabelAlign="Top"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        meta:resourceKey="txtCP">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoEntidades" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:Label runat="server" Hidden="true" />
                                    <ext:Container ID="Container1"
                                        Cls="botonesInferiores"
                                        runat="server"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblTogglePropietario"
                                                Text="<%$ Resources:Comun, strEntidadPropietario %>">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                ID="btnPropietario"
                                                Width="41"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid btn-check"
                                                AriaLabel=""
                                                ToolTip=""
                                                Handler="togglePropietario()" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container2"
                                        Cls="botonesInferiores"
                                        runat="server"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblToggleOperador"
                                                Text="<%$ Resources:Comun, strEntidadOperador %>">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                ID="btnOperador"
                                                Width="41"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid btn-check"
                                                AriaLabel=""
                                                ToolTip=""
                                                Handler="toogleOperador();" />
                                            <ext:Button runat="server"
                                                ID="btnEditarOperador"
                                                OverCls="Over-btnMore"
                                                Disabled="true"
                                                PressedCls="Pressed-none"
                                                FocusCls="Focus-none"
                                                Cls="btnEditar btn-trans btn-edit"
                                                Handler="GestionarComoOperador();" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container3"
                                        Cls="botonesInferiores"
                                        runat="server"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblToggleProveedor"
                                                Text="<%$ Resources:Comun, strEntidadProveedor %>">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                ID="btnProveedor"
                                                Width="41"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid btn-check"
                                                AriaLabel=""
                                                ToolTip=""
                                                Handler="toogleProveedor();" />
                                            <ext:Button runat="server"
                                                ID="btnEditarProveedor"
                                                OverCls="Over-btnMore"
                                                PressedCls="Pressed-none"
                                                FocusCls="Focus-none"
                                                Disabled="true"
                                                Cls="btnEditar btn-trans btn-edit">
                                                <Listeners>
                                                    <Click Fn="GestionarComoProveedorMostrar" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container4" Cls="botonesInferiores" runat="server" Layout="HBoxLayout">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblToggleCompania"
                                                Text="<%$ Resources:Comun, strEntidadEmpresaProveedora %>">
                                            </ext:Label>
                                            <ext:Button runat="server"
                                                ID="btnCompania"
                                                Width="41"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid btn-check"
                                                AriaLabel=""
                                                ToolTip=""
                                                Handler="tooglebEditarCompania();" />
                                            <ext:Button runat="server"
                                                ID="btnEditarCompania"
                                                OverCls="Over-btnMore"
                                                PressedCls="Pressed-none"
                                                Disabled="true"
                                                FocusCls="Focus-none"
                                                Cls="btnEditar btn-trans btn-edit"
                                                Handler="GestionarComoEmpresaProveedoraMostrar();" />
                                        </Items>
                                    </ext:Container>
                                </Content>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window ID="WinGestionContactos"
                runat="server"
                meta:resourceKey="WinGestionContactos"
                Width="700"
                Height="500"
                MinHeight="220"
                Hidden="true"
                Title="<%$ Resources:Comun, strAgregarEliminarContactos %>"
                Border="false"
                Resizable="false"
                Closable="true"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionContactos"
                        Cls="formGestion-resp"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:Container ID="AgregarContacto" runat="server">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregarContacto"
                                        Text="<%$ Resources:Comun, jsAgregar %>"
                                        IconCls="ico-addBtn"
                                        Cls="addContactos">
                                        <Listeners>
                                            <Click Fn="agregarContactoEntidad" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="Busquedas"
                                runat="server"
                                Cls="ctFormContactos-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtBuscarMail"
                                        EmptyText="<%$ Resources:Comun, strBusqueda %>"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Search" Handler="limpiar(this);" />
                                        </Triggers>
                                        <Listeners>
                                            <Change Fn="buscador" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:DropDownField runat="server"
                                        meta:resourcekey="searchTel"
                                        ID="searchTel"
                                        AllowBlank="true"
                                        TriggerCls="icoprefix"
                                        FocusCls="testfocus"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Component>
                                            <ext:MenuPanel runat="server" ID="MenuPanelIco" MaxWidth="120">
                                                <Menu runat="server">
                                                    <Items>
                                                        <ext:MenuItem runat="server" Text="+34 " Icon="FlagEs" />
                                                        <ext:MenuItem runat="server" Text="+43 " Icon="FlagFr" />
                                                        <ext:MenuItem runat="server" Text="+54 " Icon="FlagDe" />
                                                    </Items>
                                                    <Listeners>
                                                        <Click Handler="#{searchTel}.setValue(menuItem.text);" />
                                                    </Listeners>
                                                </Menu>
                                            </ext:MenuPanel>
                                        </Component>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Search" />
                                        </Triggers>
                                        <Listeners>
                                            <Change Fn="buscador" />
                                        </Listeners>
                                    </ext:DropDownField>
                                </Items>
                            </ext:Container>
                            <ext:GridPanel
                                Header="false"
                                runat="server"
                                Height="320"
                                ForceFit="false"
                                FocusCls="none"
                                ID="gridAddRemoveContactos"
                                Cls="gridPanel grdNoHeader"
                                OverflowX="Hidden"
                                OverflowY="Hidden"
                                Scrollable="Disabled">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="storeContactosGlobalesEntidadesVinculadas"
                                        AutoLoad="true"
                                        RemoteSort="false"
                                        RemoteFilter="false"
                                        RemotePaging="true"
                                        OnReadData="storeContactosGlobalesEntidadesVinculadas_Refresh">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model runat="server"
                                                IDProperty="ContactoGlobalID">
                                                <Fields>
                                                    <ext:ModelField Name="ContactoGlobalID" Type="Int" />
                                                    <ext:ModelField Name="EntidadID" Type="Int" />
                                                    <ext:ModelField Name="Nombre" Type="String" />
                                                    <ext:ModelField Name="Apellidos" Type="String" />
                                                    <ext:ModelField Name="Email" Type="String" />
                                                    <ext:ModelField Name="Telefono" Type="String" />
                                                    <ext:ModelField Name="ContactoTipoID" Type="Int" />
                                                    <ext:ModelField Name="MunicipioID" Type="Int" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <ColumnModel>
                                    <Columns>
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strNombre %>"
                                            DataIndex="Nombre"
                                            Flex="3"
                                            MinWidth="140"
                                            Width="140" />
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strApellidos %>"
                                            DataIndex="Apellidos"
                                            Flex="3"
                                            MinWidth="140"
                                            Width="140" />
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strEmail %>"
                                            DataIndex="Email"
                                            Flex="3"
                                            MinWidth="140"
                                            Width="140" />
                                        <ext:Column runat="server"
                                            Text="<%$ Resources:Comun, strTelefono %>"
                                            DataIndex="Telefono"
                                            Flex="2"
                                            MinWidth="140"
                                            Width="140" />
                                        <ext:WidgetColumn ID="wdEditar"
                                            runat="server"
                                            Text="<%$ Resources:Comun, jsEditar %>"
                                            Cls="col-More"
                                            DataIndex=""
                                            Filterable="false"
                                            Hidden="false"
                                            MinWidth="50"
                                            Flex="1">
                                            <Widget>
                                                <ext:Button runat="server"
                                                    OverCls="Over-btnMore"
                                                    PressedCls="Pressed-none"
                                                    FocusCls="Focus-none"
                                                    Cls="btnEditar btn-trans">
                                                    <Listeners>
                                                        <Click Fn="editarContactoEntidad" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Widget>
                                        </ext:WidgetColumn>
                                        <ext:ComponentColumn runat="server" DataIndex="EntidadID" Flex="1">
                                            <Component>
                                                <ext:Button runat="server"
                                                    ID="btnToggle"
                                                    Width="41"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip="">
                                                    <Listeners>
                                                        <Click Fn="cambiarAsignacion" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Component>
                                            <Listeners>
                                                <Bind Fn="SetToggleValue" />
                                            </Listeners>
                                        </ext:ComponentColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server"
                                        ID="GridRowSelectContacto"
                                        Mode="Single">
                                        <Listeners>
                                            <Select Fn="Grid_RowSelectContactos" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </Items>
                        <Listeners>
                            <AfterRender Handler="resizeWinForm();" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window runat="server"
                ID="winAgregarContacto"
                Resizable="false"
                Modal="true"
                Width="750px"
                Draggable="false"
                Hidden="true">
                <Content>
                    <local:FormContactosEmplazamientos ID="formAgregarEditarContacto"
                        runat="server" />
                </Content>
                <Listeners>
                    <%--<Close Fn="closeWindowContacto" />--%>
                </Listeners>
            </ext:Window>

            <ext:Window ID="winGestionCompaniaProveedora"
                runat="server"
                Cls="winFormCenter"
                meta:resourceKey="winGestionCompaniaProveedora"
                Width="380"
                Height="200"
                MinHeight="200"
                Hidden="true"
                Title="<%$ Resources:Comun, strEntidadEmpresaProveedora %>"
                Border="false"
                Resizable="false"
                Closable="true"
                Modal="true"
                MaxHeight="500"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel ID="formCompaniaProveedora"
                        Cls="formGris"
                        runat="server"
                        Hidden="false">
                        <Content>
                            <ext:Container runat="server" ID="ctFormComPro" Cls="ctForm-resp ctForm-resp-col1">
                                <Items>
                                    <ext:ComboBox meta:resourceKey="cmbModulos"
                                        runat="server"
                                        ID="cmbModulos"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strModulo %>"
                                        LabelAlign="Top"
                                        Cls="cmbModulos"
                                        Hidden="false"
                                        MultiSelect="true"
                                        QueryMode="Local"
                                        StoreID="storeClientesProyectosTipos"
                                        DisplayField="Alias"
                                        ValueField="ProyectoTipoID">
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Content>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoCompaniaProveedora(valid);" />
                            <%--<AfterRender Handler="redimensionar();" />--%>
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardarCompaniaProveedora"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionCompaniaProveedoraGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <AfterRender Handler="winCenter" />
                    <Close Handler="cerrarWinEmpresaProveedora()" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winGestionOperador"
                runat="server"
                meta:resourceKey="winGestionOperador"
                Width="380"
                Height="250"
                MinHeight="220"
                Hidden="true"
                Cls="winGestionOperador-resp winFormCenter"
                Title="<%$ Resources:Comun, strEntidadOperador %>"
                Border="false"
                Resizable="false"
                Closable="true"
                Modal="true"
                MaxHeight="500"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel ID="FormGestionOperator"
                        Cls="formGris formGestion-resp"
                        runat="server"
                        Hidden="false">
                        <Content>
                            <ext:Container runat="server" ID="ctFormOp" Cls="ctForm-resp-col1">
                                <Items>
                                    <ext:Container ID="ctrFriendly" runat="server" Layout="HBoxLayout" Cls="checks" Border="true">
                                        <Items>
                                            <ext:Checkbox ID="chkFriendly"
                                                runat="server">
                                            </ext:Checkbox>
                                            <ext:Label runat="server"
                                                ID="lblFriendly"
                                                Text="<%$ Resources:Comun, strFriendly %>">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="ctrTorre" runat="server" Layout="HBoxLayout" Cls="checks" Border="true">
                                        <Items>
                                            <ext:Checkbox ID="chkTorre"
                                                runat="server">
                                            </ext:Checkbox>
                                            <ext:Label runat="server"
                                                ID="lblTorre"
                                                Text="<%$ Resources:Comun, strTorreros %>">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="ctrCliente" runat="server" Layout="HBoxLayout" Cls="checks" Border="true">
                                        <Items>
                                            <ext:Checkbox ID="chkCliente"
                                                runat="server">
                                            </ext:Checkbox>
                                            <ext:Label runat="server"
                                                ID="lblCliente"
                                                Text="<%$ Resources:Comun, strCliente %>">
                                            </ext:Label>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Content>
                        <%--<Listeners>
                            <ValidityChange Handler="FormularioValidoOperador(valid);" />
                        </Listeners>--%>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardarOperador"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionOperadorGuardar()" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Close Handler="cerrarWinOperador()" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winGestionProveedor"
                runat="server"
                meta:resourceKey="winGestionProveedor"
                Width="800"
                Height="400"
                MinHeight="220"
                Hidden="true"
                Cls="WinFormProveedor-resp"
                Title="<%$ Resources:Comun, strEntidadProveedor %>"
                Border="false"
                Resizable="false"
                Closable="true"
                MaxHeight="500"
                Modal="true"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel ID="FormProveedor"
                        Cls="formGris"
                        runat="server"
                        Hidden="false">
                        <Content>
                            <ext:Container runat="server" ID="ctFormPro" Cls="ctFormProveedor-resp ctForm-resp-col3">
                                <Items>
                                    <ext:ComboBox meta:resourceKey="cmbMetodoPago"
                                        runat="server"
                                        ID="cmbMetodoPago"
                                        EmptyText="<%$ Resources:Comun, strMetodoPago %>"
                                        FieldLabel="<%$ Resources:Comun, strMetodoPago %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeMetodosPago"
                                        DisplayField="MetodoPago"
                                        ValueField="MetodoPagoID"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbTipoContribuyente"
                                        runat="server"
                                        ID="cmbTipoContribuyente"
                                        EmptyText="<%$ Resources:Comun, strContribuyente %>"
                                        FieldLabel="<%$ Resources:Comun, strContribuyente %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeTiposContribuyentes"
                                        DisplayField="TipoContribuyente"
                                        ValueField="TipoContribuyenteID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbIdentificacion"
                                        runat="server"
                                        ID="cmbIdentificacion"
                                        EmptyText="<%$ Resources:Comun, strNumIdentificacion %>"
                                        FieldLabel="<%$ Resources:Comun, strNumIdentificacion %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPTipoNIF"
                                        DisplayField="Descripcion"
                                        ValueField="SAPTipoNIFID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbTratamiento"
                                        runat="server"
                                        ID="cmbTratamiento"
                                        EmptyText="<%$ Resources:Comun, strTratamiento %>"
                                        FieldLabel="<%$ Resources:Comun, strTratamiento %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPTratamientos"
                                        DisplayField="SAPTratamiento"
                                        ValueField="SAPTratamientoID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbGrupoCuenta"
                                        runat="server"
                                        ID="cmbGrupoCuenta"
                                        EmptyText="<%$ Resources:Comun, strGrupoCuenta %>"
                                        FieldLabel="<%$ Resources:Comun, strGrupoCuenta %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPGruposCuentas"
                                        DisplayField="SAPGrupoCuenta"
                                        ValueField="SAPGrupoCuentaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbCuenta"
                                        runat="server"
                                        ID="cmbCuenta"
                                        EmptyText="<%$ Resources:Comun, strCuenta %>"
                                        FieldLabel="<%$ Resources:Comun, strCuenta %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPCuentasAsociadas"
                                        DisplayField="Descripcion"
                                        ValueField="SAPCuentaAsociadaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbClaveClasificacion"
                                        runat="server"
                                        ID="cmbClaveClasificacion"
                                        EmptyText="<%$ Resources:Comun, strClaveClasificacion %>"
                                        FieldLabel="<%$ Resources:Comun, strClaveClasificacion %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPClavesClasificaciones"
                                        DisplayField="Descripcion"
                                        ValueField="SAPClaveClasificacionID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbTesoreria"
                                        runat="server"
                                        ID="cmbTesoreria"
                                        EmptyText="<%$ Resources:Comun, strGrupoTesoreria %>"
                                        FieldLabel="<%$ Resources:Comun, strGrupoTesoreria %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPGruposTesorerias"
                                        DisplayField="Descripcion"
                                        ValueField="SAPGrupoTesoreriaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox meta:resourceKey="cmbCondicionPago"
                                        runat="server"
                                        ID="cmbCondicionPago"
                                        EmptyText="<%$ Resources:Comun, strCondicionPago %>"
                                        FieldLabel="<%$ Resources:Comun, strCondicionPago %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeCondicionesPagos"
                                        DisplayField="CondicionPago"
                                        ValueField="CondicionPagoID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarCombo" />
                                            <TriggerClick Fn="RecargarCombo" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Content>
                        <Listeners>
                            <AfterRender Handler="resizeWinForm();" />
                            <ValidityChange Handler="FormularioValidoProveedor(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardarProveedor"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionProveedorGuardar()" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Close Handler="cerrarWinProveedor()" />
                </Listeners>
            </ext:Window>


            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls=""
                        Layout="BorderLayout">

                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL CON SLIDERS--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn">

                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>

                                <DockedItems>

                                    <ext:Toolbar runat="server" ID="TbNavegacionTabs" Dock="Top" Cls="tbGrey" Hidden="false" MinHeight="36">
                                        <Items>



                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkPrimero"
                                                Hidden="false"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="ActiveTab">
                                                <Listeners>
                                                    <Click Handler="NavegacionTabs(this)"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>

                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkSegundo"
                                                Hidden="false"
                                                Cls="lnk-navView lnk-noLine "
                                                Text="ActiveTa2b">
                                                <Listeners>
                                                    <Click Handler="NavegacionTabs(this)"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>



                                            <ext:ToolbarFill></ext:ToolbarFill>

                                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv2" OnDirectClick="ShowHidePnAsideR" Handler="hidePnLite();" Disabled="false" Hidden="true"></ext:Button>
                                        </Items>
                                    </ext:Toolbar>


                                    <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey">
                                        <Items>

                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="moveCtSldr(this);" Disabled="true"></ext:Button>
                                            <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next-w" Cls="SliderBtn" Handler="moveCtSldr(this);" Disabled="false"></ext:Button>

                                        </Items>
                                    </ext:Toolbar>

                                </DockedItems>

                                <Items>



                                    <%-- PANELES PRICIPALES COLUMNMAS QUE TIENEN COMPORTAMIENTO SLIDER--%>


                                    <ext:Panel runat="server" ID="ctMain1" Flex="2" Layout="FitLayout" Cls="col" Hidden="false">
                                        <Items>


                                            <ext:GridPanel
                                                Hidden="false"
                                                Title="Grid Principal"
                                                runat="server"
                                                ForceFit="false"
                                                Header="false"
                                                ContextMenuID="ContextMenuTreeL"
                                                SelectionMemory="false"
                                                ID="gridEntities"
                                                StoreID="storePrincipal"
                                                Scrollable="Vertical"
                                                Cls="gridPanel grdNoHeader ">
                                                <Listeners>
                                                    <%--<AfterRender Handler="GridColHandler(this)"></AfterRender>--%>
                                                    <%--<Resize Handler="GridColHandler(this)"></Resize>--%>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnAnadir" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="AgregarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEditar" Cls="btnEditar" AriaLabel="Editar" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="MostrarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnEliminar" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>" Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="Eliminar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnRefrescar" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun,  btnRefrescar.ToolTip %>">
                                                                <Listeners>
                                                                    <Click Fn="Refrescar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server" ID="btnDescargar"
                                                                Cls="btnDescargar" AriaLabel="Descargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('Entidades', hdCliID.value, #{gridEntities}, '-1',undefined, #{cmbEntidades}.value);">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnGenerarPlantilla"
                                                                Hidden="true"
                                                                IconCls="ico-CntxMenuExcel" AriaLabel="Generar plantilla"
                                                                ToolTip="<%$ Resources:Comun, btnGenerarPlantilla.ToolTip %>"
                                                                Handler="GenerarPlantillaEntidades();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnGestionContactos"
                                                                ToolTip="<%$ Resources:Comun, strContactos %>"
                                                                Cls="btnContactos"
                                                                Handler="GestionarContacto();" />
                                                            <ext:ToolbarFill />
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbEntidades"
                                                                ValueField="Alls"
                                                                Cls="comboGrid"
                                                                EmptyText="<%$ Resources:Comun, strTodasEntidades %>"
                                                                DisplayField="Alls"
                                                                MaxWidth="250"
                                                                Flex="8">
                                                                <Items>
                                                                    <ext:ListItem Value="Alls" Text="<%$ Resources:Comun, strTodasEntidades %>"></ext:ListItem>
                                                                    <ext:ListItem Value="Owners" Text="<%$ Resources:Comun, strPropietario %>" />
                                                                    <ext:ListItem Value="Suppliers" Text="<%$ Resources:Comun, strProveedor %>" />
                                                                    <ext:ListItem Value="Companies" Text="<%$ Resources:Comun, strEmpresaProveedora %>" />
                                                                    <ext:ListItem Value="Operators" Text="<%$ Resources:Comun, strOperador %>" />
                                                                </Items>
                                                                <Listeners>
                                                                    <Select Handler="#{gridEntities}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>

                                                        </Items>
                                                    </ext:Toolbar>


                                                    <%-- <ext:Container ID="cntFiltro" runat="server" Header="false" Border="false">
                                                        <Content>
                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storePrincipal"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                MostrarBusqueda="true" />
                                                        </Content>
                                                    </ext:Container>--%>
                                                </DockedItems>

                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelect"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>">
                                                    </ext:GridFilters>
                                                    <%--<ext:LiveSearchGridPanel runat="server" />--%>
                                                </Plugins>

                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server" StoreID="storePrincipal" Cls="PgToolBMainGrid" ID="PGToolBarGrid" HideRefresh="true" DisplayInfo="true">
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
                                                                <Listeners>
                                                                    <Select Fn="handlePageSizeSelect" />
                                                                </Listeners>

                                                            </ext:ComboBox>
                                                            <ext:StatusBar ID="StatusBar1" runat="server" DefaultText="Nothing Found" Hidden="true">
                                                                <CustomConfig>
                                                                    <ext:ConfigItem Name="setStatus" Value="SetStatusBarText" Mode="Raw" />
                                                                </CustomConfig>
                                                            </ext:StatusBar>
                                                            <ext:Label runat="server"
                                                                ID="StatusBar"
                                                                Text="<%$ Resources:Comun, strNingunResultado %>" />
                                                        </Items>

                                                    </ext:PagingToolbar>
                                                </BottomBar>

                                            </ext:GridPanel>


                                            <ext:Panel runat="server" ID="gridMain2" Layout="FitLayout">
                                                <Content>
                                                    <local:GridContactosGlobales ID="gridContactos"
                                                        vista="ContactosEntidades"
                                                        runat="server" />
                                                </Content>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>


                                    <%--                                    <ext:Panel runat="server" ID="ctMain2" Flex="1" Layout="FitLayout" Cls="col colCt2" Hidden="true">
                                        <Items>


                                            <ext:Label runat="server" Text="ct2"></ext:Label>
                                            <ext:Label runat="server" Text="ct2"></ext:Label>
                                            <ext:Label runat="server" Text="ct2"></ext:Label>
                                            <ext:Label runat="server" Text="ct2"></ext:Label>


                                        </Items>
                                    </ext:Panel>


                                    <ext:Panel runat="server" ID="ctMain3" Flex="1" Layout="FitLayout" Cls="col colCt3" Hidden="true">
                                        <Items>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>
                                            <ext:Label runat="server" Text="ct3"></ext:Label>





                                        </Items>
                                    </ext:Panel>--%>
                                </Items>
                            </ext:Panel>


                            <%-- PANEL LATERAL DESPLEGABLE--%>


                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Header="false" Border="false" Width="380" Hidden="false">
                                <Listeners>
                                    <Collapse Handler="ActiveResizer();"></Collapse>
                                    <Expand Handler="ActiveResizer();"></Expand>
                                </Listeners>
                                <Items>



                                    <%-- PANEL PRINCIPAL (WRAP A PANELES QUE NO SON GESTION COLUMNAS)--%>


                                    <ext:Container runat="server" ID="WrapFilterControls" Hidden="true" Cls="tbGrey">
                                        <Items>


                                            <ext:Label meta:resourcekey="lblAsideNameR" ID="lblAsideNameR" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="Additional INFO" PaddingSpec="0 0 20 0"></ext:Label>


                                            <ext:Panel meta:resourcekey="ctAsideR" ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                                <Items>
                                                    <%--LEFT TABS MENU--%>
                                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" Hidden="false">
                                                        <Items>
                                                            <ext:Button runat="server" meta:resourcekey="btnInfoGrid" ID="btnCreateFilters" Cls="btnFiltersPlus-asR" Handler="displayMenu('pnCFilters')"></ext:Button>
                                                            <ext:Button runat="server" meta:resourcekey="btnVersions" ID="btnMyFilters" Cls="btnMyFilters-asR" Handler="displayMenu('pnGridsAsideMyFilters')"></ext:Button>

                                                        </Items>
                                                    </ext:Panel>





                                                    <%--CREATE FILTERS PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnCFilters"
                                                        Hidden="false">
                                                        <Items>
                                                            <ext:Label
                                                                meta:resourcekey="lblGrid"
                                                                ID="lblGrid"
                                                                runat="server"
                                                                IconCls="btn-CFilter"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                            </ext:Label>
                                                            <ext:Panel
                                                                runat="server"
                                                                ID="pnCFiltersContainer">
                                                                <Items>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnNewFilter"
                                                                        ID="pnNewFilter"
                                                                        FieldLabel=""
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strNombreFiltro %>" />
                                                                    <ext:Button
                                                                        MarginSpec="0 0 0 10"
                                                                        runat="server"
                                                                        ID="btnFilter"
                                                                        Cls="btn-add"
                                                                        Text="New Filter">
                                                                        <Listeners>
                                                                            <%--<Click Fn="newFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ComboBox runat="server"
                                                                        meta:resourcekey="cmbField"
                                                                        ID="cmbField"
                                                                        FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                        Flex="1"
                                                                        MarginSpec="0 10 0 0"
                                                                        Cls="pnForm fieldFilter"
                                                                        ValueField="Id"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeCampos"
                                                                                runat="server"
                                                                                AutoLoad="true">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                            <ext:ModelField Name="typeData" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <%--         <DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <%--        <Select Fn="selectField" />--%>
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnSearch"
                                                                        ID="textInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                        Cls="pnForm"
                                                                        Hidden="false" />
                                                                    <ext:DateField
                                                                        runat="server"
                                                                        ID="dateInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true"
                                                                        Format="dd/MM/yyyy">
                                                                    </ext:DateField>
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="numberInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true">
                                                                    </ext:NumberField>
                                                                    <ext:ComboBox
                                                                        ID="cmbOperatorField"
                                                                        runat="server"
                                                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                        Cls="pnForm"
                                                                        Flex="1"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        QueryMode="Local">
                                                                        <Items>
                                                                            <ext:ListItem Text="=" Value="IGUAL" />
                                                                            <ext:ListItem Text="<" Value="MENOR" />
                                                                            <ext:ListItem Text=">" Value="MAYOR" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                    <ext:MultiCombo
                                                                        ID="cmbTiposDinamicos"
                                                                        runat="server"
                                                                        FieldLabel="tiposDinamicos"
                                                                        DisplayField="Name"
                                                                        EmptyText="tiposDinamicos"
                                                                        Cls="pnForm"
                                                                        Flex="1"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        ValueField="Id"
                                                                        SelectionMode="Selection"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeTiposDinamicos"
                                                                                runat="server"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <%--      <DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:MultiCombo>
                                                                    <ext:Button
                                                                        runat="server" meta:resourcekey="ColMas"
                                                                        ID="btnAdd"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        Text="<%$ Resources:Comun, jsAgregar %>">
                                                                        <Listeners>
                                                                            <%--     <Click Fn="addElementFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Panel runat="server" ID="tagsHeader">
                                                                        <Items>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblCampo"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strCampo %>">
                                                                            </ext:Label>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblBuscar"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strBuscar %>">
                                                                            </ext:Label>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" ID="pnTagContainer" Scrollable="Vertical" MaxHeight="100" MarginSpec="0 190 8 0">
                                                                        <Items>
                                                                            <%-- <ext:Panel runat="server" Cls="pntag" ID="pnTags" Hidden="false">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="ButtonClose" Cls="btnCloseTag" FocusCls="none"></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button1" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button2" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button3" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>--%>
                                                                        </Items>
                                                                    </ext:Panel>

                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnAplyFilter"
                                                                        ID="btnAplyFilter"
                                                                        Cls="btn-end"
                                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                                        <Listeners>
                                                                            <%--          <Click Fn="aplyFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnSaveFilter"
                                                                        ID="btnSaveFilter"
                                                                        Cls="btn-save"
                                                                        Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                        <Listeners>
                                                                            <%--       <Click Fn="saveFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Panel>

                                                    <%--MY FILTERS PANEL--%>
                                                    <ext:Panel
                                                        ID="pnGridsAsideMyFilters"
                                                        runat="server"
                                                        Border="false"
                                                        Header="false"
                                                        Scrollable="Vertical"
                                                        OverflowY="Scroll"
                                                        Hidden="true"
                                                        Cls="">
                                                        <Items>

                                                            <ext:Label
                                                                meta:resourcekey="lblMyFilters"
                                                                ID="Label4"
                                                                runat="server"
                                                                IconCls="ico-head-my-filters"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strMisFiltros %>">
                                                            </ext:Label>
                                                            <ext:Panel
                                                                runat="server"
                                                                ID="pnMFiltersContainer"
                                                                Layout="FitLayout"
                                                                Hidden="false">
                                                                <Items>
                                                                    <ext:GridPanel
                                                                        ID="GridMyFilters"
                                                                        runat="server"
                                                                        Header="false"
                                                                        Border="false"
                                                                        Width="100"
                                                                        Cls="GridMyFilters"
                                                                        Scrollable="Vertical"
                                                                        Height="600">
                                                                        <Store>
                                                                            <ext:Store
                                                                                runat="server"
                                                                                PageSize="10"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="GestionFiltroID" />
                                                                                            <ext:ModelField Name="UsuarioID" />
                                                                                            <ext:ModelField Name="NombreFiltro" />
                                                                                            <ext:ModelField Name="JsonItemsFiltro" />
                                                                                            <ext:ModelField Name="Pagina" />
                                                                                            <ext:ModelField Name="check" Type="Boolean" DefaultValue="false" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <ColumnModel runat="server">
                                                                            <Columns>

                                                                                <ext:Column runat="server"
                                                                                    Sortable="true"
                                                                                    DataIndex="NombreFiltro"
                                                                                    Width="150"
                                                                                    Align="Start">
                                                                                </ext:Column>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="ColMas"
                                                                                    runat="server"
                                                                                    Width="15"
                                                                                    Cls="col-More"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            meta:resourcekey="btnColMore"
                                                                                            runat="server"
                                                                                            ID="btnColMore"
                                                                                            Width="16"
                                                                                            OverCls="Over-btnMore"
                                                                                            PressedCls="Pressed-none"
                                                                                            FocusCls="Focus-none"
                                                                                            Cls="BtnDeleteChk">
                                                                                            <Listeners>
                                                                                                <%--          <Click Fn="DeleteFilter" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
                                                                                    </Widget>
                                                                                </ext:WidgetColumn>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="colBtnEdit"
                                                                                    runat="server"
                                                                                    Width="18"
                                                                                    Cls="col-More"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            meta:resourcekey="btnColMore"
                                                                                            runat="server"
                                                                                            ID="btnCheck"
                                                                                            Width="18"
                                                                                            OverCls="Over-btnMore"
                                                                                            PressedCls="Pressed-none"
                                                                                            FocusCls="Focus-none"
                                                                                            Cls="BtnEditChk">
                                                                                            <Listeners>
                                                                                                <%--     <Click Fn="MostrarEditarFiltroGuardado" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
                                                                                    </Widget>
                                                                                </ext:WidgetColumn>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="colChkAplyFilter"
                                                                                    runat="server"
                                                                                    Width="18"
                                                                                    Cls="col-Chk"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            runat="server"
                                                                                            ID="chkAplyFilter"
                                                                                            Cls="btn-mini-ppal btnApply ico-tic-wh"
                                                                                            DataIndex="check">
                                                                                            <Listeners>
                                                                                                <%--     <Click Fn="AplyFilterSaved" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
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


                                                </Items>
                                            </ext:Panel>


                                        </Items>
                                    </ext:Container>



                                    <%-- PANEL GESTION COLUMNAS--%>


                                    <ext:Panel runat="server" ID="WrapGestionColumnas" Hidden="false" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch" />
                                        </LayoutConfig>

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tbTitlePanelColumnas" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label6" runat="server" IconCls="ico-head-columns-gr" Cls="lblHeadAside lblHeadAside tbGrey" Text="COLUMN SETTINGS" MarginSpec="25 15 30 15"></ext:Label>



                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>



                                        <Items>

                                            <%--   <ext:Label ID="Label5" runat="server" Cls="lblRecordName" Text="Site Name"></ext:Label>--%>
                                            <ext:ComboBox runat="server" ID="ComboBox4" Cls="comboGrid comboCustomTrigger  " EmptyText="Profiles" LabelAlign="Top" FieldLabel="Profiles" Padding="15">
                                                <Items>
                                                    <ext:ListItem Text="Profile 1" />
                                                    <ext:ListItem Text="Profile 2" />
                                                </Items>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-trigger-reload" ExtraCls="none"></ext:FieldTrigger>
                                                </Triggers>
                                            </ext:ComboBox>

                                            <ext:GridPanel
                                                ID="GridColumnas"
                                                Padding="15"
                                                BodyPadding="2"
                                                runat="server"
                                                OverflowY="Auto"
                                                OverflowX="Hidden"
                                                Cls=" gridPanel grdPanelVersionesOld ">

                                                <DockedItems>


                                                    <ext:Toolbar runat="server" ID="Toolbar9" Dock="Bottom" Padding="0" MarginSpec="8 -8 0 0">
                                                        <Items>


                                                            <ext:Button runat="server" ID="Button1" Cls="btn-ppal " Text="Save in new Tab" Focusable="false" PressedCls="none" Hidden="false" Flex="1" Handler=""></ext:Button>




                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Toolbar runat="server" ID="tbColumnasSaveApply" Dock="Bottom" Padding="0" MarginSpec="8 -8 0 0">
                                                        <Items>


                                                            <ext:Button runat="server" ID="btnApplicar" Cls="btn-ppal " Text="Apply" Focusable="false" PressedCls="none" Hidden="false" Flex="1"></ext:Button>


                                                            <ext:Button runat="server" ID="btnGuardarCols" Cls="btn-ppal " Text="Save" Focusable="false" PressedCls="none" Hidden="false" Flex="1"></ext:Button>


                                                        </Items>
                                                    </ext:Toolbar>



                                                </DockedItems>


                                                <Store>
                                                    <ext:Store runat="server">

                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="ID" />
                                                                    <ext:ModelField Name="NombreCategoria" />
                                                                    <ext:ModelField Name="NombreItemTarea" />
                                                                    <ext:ModelField Name="CosteItemUnidad" />
                                                                    <ext:ModelField Name="CosteItemTipoUnidad" />
                                                                    <ext:ModelField Name="NumItems" />
                                                                    <ext:ModelField Name="CosteTotalFila" Type="Int" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>

                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>


                                                        <ext:WidgetColumn ID="WidgetColumn4" runat="server" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MaxWidth="50">
                                                            <Widget>
                                                                <ext:Label runat="server" ID="DragBtn" IconCls="btnMoverFila" Cls=" btn-trans " OverCls="none"></ext:Label>
                                                            </Widget>
                                                        </ext:WidgetColumn>


                                                        <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="5">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                                                            <div class="customCol1">
                                                                
                                                                <div class="LabelColumnGridRow">{NombreCategoria}


                                                                 <%--   <button  name="btnSubmit" id="btnSubmit" runat="server" onserverclick="Submit_Click"/>--%>


                                                                </div> 
                                                             

                                                            </div>
                                                        </tpl>
                                                                </Html>

                                                            </Template>

                                                        </ext:TemplateColumn>


                                                        <ext:WidgetColumn ID="WidgetColumn3" runat="server" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MaxWidth="40">
                                                            <Widget>
                                                                <ext:Button runat="server" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" btn-trans btnVisible" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                            </Widget>
                                                        </ext:WidgetColumn>



                                                    </Columns>
                                                </ColumnModel>

                                                <View>
                                                    <ext:GridView runat="server">
                                                        <Plugins>
                                                            <ext:GridDragDrop runat="server" DragText="Drag To Re-order"></ext:GridDragDrop>
                                                        </Plugins>
                                                    </ext:GridView>
                                                </View>

                                            </ext:GridPanel>


                                        </Items>
                                    </ext:Panel>



                                    <%-- PANEL MORE INFO--%>



                                    <ext:Container runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey">
                                        <Items>


                                            <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" PaddingSpec="0 0 20 0"></ext:Label>


                                            <ext:Panel meta:resourcekey="ctAsideR" ID="ctAsideR2" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                                <Items>
                                                    <%--LEFT TABS MENU--%>
                                                    <ext:Panel ID="mnAsideR2" runat="server" Border="false" Header="false" Hidden="true">
                                                        <Items>
                                                            <ext:Button runat="server" meta:resourcekey="btnMoreInfo" ID="Button8" Cls="btnInfo-asR" Handler="displayMenuPnInfo('pnGridInfoSeparated')"></ext:Button>

                                                        </Items>
                                                    </ext:Panel>


                                                    <%--PANEL INFO GRID--%>

                                                    <ext:Panel ID="pnGridInfoSeparated" runat="server" Border="false" Header="false" Hidden="false">
                                                        <Items>

                                                            <ext:Label ID="Label2" runat="server" Cls="lblRecordName" Text="Site Name"></ext:Label>
                                                            <ext:GridPanel ID="grAsR1" runat="server"
                                                                Height="900"
                                                                Cls="grdPnColIcons grdIntoAside"
                                                                Border="false">

                                                                <Store>
                                                                    <ext:Store
                                                                        ID="Store1"
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
                                                                        <ext:TemplateColumn runat="server" DataIndex="" ID="templateColumn2" MenuDisabled="true" Text="" Flex="1">
                                                                            <Template runat="server">
                                                                                <Html>
                                                                                    <tpl for=".">
												                        <table class="tmpCol-table">
                                                                    
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{Company}</span></td>
																				<td class="tmpCol-td" colspan="3"></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{Email}</span></td>
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{License}</span></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="2"><span class="lblGrd">Info Label</span><span class="dataGrd">{Project}</span></td>
																				<td class="tmpCol-td" colspan="3"></td>

																			</tr>
																		
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{License}</span></td>
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{License}</span></td>
																				
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span  class="dataGrd">{KeyExpiration}</span></td>
																				<td class="tmpCol-td" colspan="3"></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{LastKey}</span></td>
																				<td class="tmpCol-td" colspan="3"></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{LastAccess}</span></td>
																				<td class="tmpCol-td" colspan="3"></td>

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
                                                                        <Plugins>
                                                                            <ext:GridDragDrop runat="server" DragText="TEST!"></ext:GridDragDrop>
                                                                        </Plugins>
                                                                    </ext:GridView>
                                                                </View>







                                                                <DockedItems></DockedItems>
                                                            </ext:GridPanel>

                                                        </Items>
                                                    </ext:Panel>



                                                </Items>
                                            </ext:Panel>


                                        </Items>
                                    </ext:Container>





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
