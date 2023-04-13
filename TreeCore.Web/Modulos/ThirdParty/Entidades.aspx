<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entidades.aspx.cs" Inherits="TreeCore.ModGlobal.Entidades" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<%@ Register Src="/Componentes/GridEmplazamientosContactos.ascx" TagName="GridContactosGlobales" TagPrefix="local" %>

<%@ Register Src="/Componentes/Geoposicion.ascx" TagName="Geoposicion" TagPrefix="local" %>

<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactosEmplazamientos" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css" integrity="sha384-TX8t27EcRE3e/ihU7zmQxVncDAy5uIKz4rEkgIXeMed4M0jlfIDPvg6uqKI2xXr2" crossorigin="anonymous">--%>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/Geoposicion.css" rel="stylesheet" type="text/css" />
    <link href="css/styleEmplazamientos.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/FormEmplazamientos.css" rel="stylesheet" type="text/css" />
    <!--<script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>-->
    <!--<script type="text/javascript" src="../../Componentes/js/Localizaciones.js"></script>
    <script type="text/javascript" src="../../Componentes/js/toolbarFiltros.js"></script>
    <script type="text/javascript" src="../../Componentes/js/Geoposicion.js"></script>
    <script type="text/javascript" src="../../Componentes/js/GridEmplazamientosContactos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/FormContactos.js"></script>
    <script type="text/javascript" src="/PaginasComunes/js/Ext.ux.Map.js"></script>
    <script type="text/javascript" src="/PaginasComunes/js/Ext.ux.GMapPanel.js"></script>-->

    <form id="form1" runat="server">
        <div>
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
            <ext:Hidden ID="hdColumnasEntidades" runat="server" />

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Handler="setClienteIDComponentes();"></DocumentReady>

                </Listeners>
            </ext:ResourceManager>

            <%--STORES--%>

            <ext:Store
                ID="storePrincipal"
                runat="server"
                OnReadData="storePrincipal_Refresh"
                RemoreSort="true"
                RemotePaging="false"
                PageSize="20"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="EntidadID,ClienteID,EsPropietario,EsProveedor,EsEmpresaProveedora,EsOperador,EntidadCliente">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                    <DataChanged Fn="BuscadorPredictivo" />
                    <Load Handler="GridColHandlerDinamico(#{grid})"></Load>
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

            <ext:Window ID="WinGestion"
                runat="server"
                meta:resourceKey="WinGestionEntidad"
                Width="600"
                HeightSpec="80vh"
                MaxHeight="600"
                Hidden="true"
                Modal="true"
                Centered="true"
                Resizable="false"
                Cls="winForm-resp winGestion WinGestionEntidad"
                Title="Añadir nueva entidad"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel ID="formInicial"
                        Cls="formGris"
                        PaddingSpec="20 0 0 0"
                        runat="server"
                        Hidden="false"
                        Scrollable="Vertical">
                        <Items>
                            <ext:Label runat="server" ID="lblDataEntity" Cls="lblCompany" Text="Data Company" />
                            <ext:FormPanel ID="formGestionEntidad"
                                Cls="formGris"
                                runat="server"
                                Hidden="false"
                                Scrollable="Vertical">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctFormEntidades"
                                        Cls="winGestion-panel ctForm-resp ctForm-content-resp-col2">
                                        <Content>
                                            <ext:TextField ID="txtCodigo"
                                                runat="server"
                                                FieldLabel="<%$ Resources:Comun, strCodigo%>"
                                                LabelAlign="Top"
                                                MaxLength="50"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                AllowBlank="false"
                                                Regex="/^[^$%&|<>/\#]*$/"
                                                RegexText="<%$ Resources:Comun, regexNombreText %>"
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
                                                QueryMode="Local">
                                                <Listeners>
                                                    <Select Fn="SeleccionarComboEntidades" />
                                                    <TriggerClick Fn="RecargarCombo" />
                                                    <Change Fn="FormularioValidoEntidades" />
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
                                                Cls="telefono"
                                                CausesValidation="true"
                                                Regex="/^[[+]([0-9])*]? ([0-9])*$/">
                                                <Component>
                                                    <ext:MenuPanel runat="server" ID="MenuPanel1" MaxWidth="120">
                                                        <Menu runat="server"
                                                            ID="MenuPrefijos2">
                                                            <Items>
                                                                <ext:MenuItem 
                                                                    runat="server" 
                                                                    IconCls="x-loading-indicator" 
                                                                    Text="<%$ Resources:Comun, jsMensajeProcesando %>" 
                                                                    Focusable="false" 
                                                                    HideOnClick="false" />
                                                            </Items>
                                                            <Loader runat="server" 
                                                                Mode="Component" 
                                                                DirectMethod="#{DirectMethods}.LoadPrefijos" 
                                                                RemoveAll="true">
                                                            </Loader>
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
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:Label runat="server" ID="lblLocationCompany" Cls="lblCompany" Text="Location Company" />
                            <ext:FormPanel ID="formGeo"
                                Cls="formGris"
                                runat="server"
                                Hidden="false"
                                Scrollable="Vertical">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="cntGeo"
                                        Cls="winGestion-panel ctForm-resp ctForm-content-resp-col2">
                                        <Content>
                                            <local:Geoposicion ID="geoEntidad"
                                                runat="server"
                                                Localizacion="false"
                                                Contactos="false"
                                                Entidad="true" />
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:Label runat="server" ID="lblRolCompany" Cls="lblCompany" Text="Role Company" />
                            <ext:FormPanel ID="formToggle"
                                Cls="formGris"
                                runat="server"
                                Hidden="false"
                                Scrollable="Vertical">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="ctToggle"
                                        Cls="winGestion-panel ctForm-resp ctForm-content-resp-col2">
                                        <Content>
                                            <%--<ext:Label runat="server" Hidden="true" />--%>
                                            <content id="Content1" class="botonesInferiores">

                                                <div id="ctOwners">
                                                    <ext:Label runat="server"
                                                        Cls="lbl-lastButtons"
                                                        ID="lblTogglePropietario"
                                                        Text="<%$ Resources:Comun, strEntidadPropietario %>">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        ID="btnPropietario"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid chk-lastButtons"
                                                        AriaLabel=""
                                                        ToolTip=""
                                                        Handler="togglePropietario()" />
                                                </div>
                                            </content>
                                            <content id="Content2" class="botonesInferiores">
                                                <div id="ctOperator">

                                                    <ext:Label runat="server"
                                                        ID="lblToggleOperador"
                                                        Cls="lbl-lastButtons"
                                                        Text="<%$ Resources:Comun, strEntidadOperador %>">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        ID="btnOperador"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid chk-lastButtons"
                                                        AriaLabel=""
                                                        ToolTip=""
                                                        Handler="toogleOperador();" />
                                                    <ext:Button runat="server"
                                                        ID="btnEditarOperador"
                                                        OverCls="Over-btnMore"
                                                        Disabled="true"
                                                        PressedCls="Pressed-none"
                                                        FocusCls="Focus-none"
                                                        Cls="btnEditar btn-trans edit-lastButtons"
                                                        Handler="GestionarComoOperador();" />
                                                </div>
                                            </content>
                                            <content id="contentSupplier" class="botonesInferiores">
                                                <div id="ctSupplier">
                                                    <ext:Label runat="server"
                                                        ID="lblToggleProveedor"
                                                        Cls="lbl-lastButtons"
                                                        MinWidth="120"
                                                        Text="<%$ Resources:Comun, strEntidadProveedor %>">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        ID="btnProveedor"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid chk-lastButtons"
                                                        AriaLabel=""
                                                        ToolTip=""
                                                        Width="41"
                                                        Handler="toogleProveedor();" />
                                                    <ext:Button runat="server"
                                                        ID="btnEditarProveedor"
                                                        OverCls="Over-btnMore"
                                                        PressedCls="Pressed-none"
                                                        FocusCls="Focus-none"
                                                        Disabled="true"
                                                        Cls="btnEditar btn-trans edit-lastButtons">
                                                        <Listeners>
                                                            <Click Fn="GestionarComoProveedorMostrar" />
                                                        </Listeners>
                                                    </ext:Button>

                                                </div>
                                            </content>

                                            <content id="Container4" class="botonesInferiores">
                                                <div id="ctCompany">
                                                    <ext:Label runat="server"
                                                        ID="lblToggleCompania"
                                                        Cls="lbl-lastButtons"
                                                        Text="<%$ Resources:Comun, strEntidadEmpresaProveedora %>">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        ID="btnCompania"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid chk-lastButtons"
                                                        AriaLabel=""
                                                        ToolTip=""
                                                        Handler="tooglebEditarCompania();" />
                                                    <ext:Button runat="server"
                                                        ID="btnEditarCompania"
                                                        OverCls="Over-btnMore"
                                                        PressedCls="Pressed-none"
                                                        Disabled="true"
                                                        FocusCls="Focus-none"
                                                        Cls="btnEditar btn-trans edit-lastButtons"
                                                        Handler="GestionarComoEmpresaProveedoraMostrar();" />
                                                </div>
                                            </content>
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                        </Items>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Close Fn="Refrescar" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="WinGestionContactos"
                runat="server"
                meta:resourceKey="WinGestionContactos"
                Width="700"
                Height="500"
                MinHeight="220"
                Hidden="true"
                Title="<%$ Resources:Comun, strAgregarEliminarContactos %>"
                Cls="WinGestionContactos"
                Border="false"
                Resizable="false"
                Closable="true"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionContactos"
                        Cls="formGestion-resp formGris formGestionContactos"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:Container ID="AgregarContacto" runat="server">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregarContacto"
                                        Text="<%$ Resources:Comun, jsAgregar %>"
                                        IconCls="ico-addBtn"
                                        Cls="addContactos btnAgregarContacto">
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
                                        Cls="txtBuscarMail"
                                        EmptyText="<%$ Resources:Comun, strBusqueda %>"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" Handler="limpiar(this);" />
                                            <ext:FieldTrigger Icon="Search" Handler="buscador()" />
                                        </Triggers>
                                    </ext:TextField>
                                    <ext:DropDownField runat="server"
                                        meta:resourcekey="searchTel"
                                        ID="searchTel"
                                        Cls="telefono"
                                        AllowBlank="true"
                                        MaxHeight="32"
                                        TriggerCls="icoprefix"
                                        EmptyText="<%$ Resources:Comun, strSeleccionarPrefijo %>"
                                        FocusCls="testfocus"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Component>
                                            <ext:MenuPanel runat="server" ID="MenuPanelIco" MaxWidth="120">
                                                <Menu runat="server"
                                                    ID="MenuPrefijos">
                                                    <Items>
                                                        <ext:MenuItem 
                                                            runat="server" 
                                                            IconCls="x-loading-indicator" 
                                                            Text="<%$ Resources:Comun, jsMensajeProcesando %>" 
                                                            Focusable="false" 
                                                            HideOnClick="false" />
                                                    </Items>
                                                    <Loader runat="server" 
                                                        Mode="Component" 
                                                        DirectMethod="#{DirectMethods}.LoadPrefijos" 
                                                        RemoveAll="true">
                                                    </Loader>
                                                    <Listeners>
                                                        <Click Handler="#{searchTel}.setValue(menuItem.text);" />
                                                    </Listeners>
                                                </Menu>
                                            </ext:MenuPanel>
                                        </Component>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" Handler="limpiar(this);" />
                                            <ext:FieldTrigger Icon="Search" Handler="buscador()" />
                                        </Triggers>

                                    </ext:DropDownField>
                                </Items>
                            </ext:Container>
                            <ext:GridPanel
                                Header="false"
                                runat="server"
                                Height="300"
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
                Closable="false"
                Draggable="false"
                Hidden="true">
                <Content>
                    <local:FormContactosEmplazamientos ID="formAgregarEditarContacto"
                        runat="server" />
                </Content>
                <%--<Listeners>
                    <Close Fn="cerrarWindow" />
                </Listeners>--%>
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
                        Height="80"
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
                Height="260"
                MinHeight="240"
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
                            <ext:Container runat="server" ID="ctFormOp" Cls="ctForm-resp-col1" Margin="16">
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
                                                Text="<%$ Resources:Comun, strEsCliente %>">
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeMetodosPago"
                                        DisplayField="MetodoPago"
                                        ValueField="MetodoPagoID"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeTiposContribuyentes"
                                        DisplayField="TipoContribuyente"
                                        ValueField="TipoContribuyenteID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        EmptyText="<%$ Resources:Comun, strTipoNif %>"
                                        FieldLabel="<%$ Resources:Comun, strTipoNif %>"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPTipoNIF"
                                        DisplayField="Descripcion"
                                        ValueField="SAPTipoNIFID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPTratamientos"
                                        DisplayField="SAPTratamiento"
                                        ValueField="SAPTratamientoID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPGruposCuentas"
                                        DisplayField="SAPGrupoCuenta"
                                        ValueField="SAPGrupoCuentaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPCuentasAsociadas"
                                        DisplayField="Descripcion"
                                        ValueField="SAPCuentaAsociadaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPClavesClasificaciones"
                                        DisplayField="Descripcion"
                                        ValueField="SAPClaveClasificacionID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeSAPGruposTesorerias"
                                        DisplayField="Descripcion"
                                        ValueField="SAPGrupoTesoreriaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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
                                        AllowBlank="true"
                                        Cls="cmbFormProveedor"
                                        Hidden="false"
                                        StoreID="storeCondicionesPagos"
                                        DisplayField="CondicionPago"
                                        ValueField="CondicionPagoID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEntidadesProveedores" />
                                            <TriggerClick Fn="RecargarCombo" />
                                            <Change Fn="FormularioValidoProveedor" />
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

            <ext:Window ID="WinAsignarImagenOperador"
                runat="server"
                meta:resourceKey="winAsignarImagenOperador"
                Width="400"
                Height="200"
                MinHeight="220"
                Cls="win1Col WinAsignarImagenOperador"
                Hidden="true"
                Title="<%$ Resources:Comun, strAsignarImagen %>"
                Border="false"
                Resizable="false"
                Closable="true"
                MaxHeight="500"
                Modal="true"
                OverflowY="Auto">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formAsignarImagenOperador"
                        Cls="formGris"
                        Padding="20"
                        Border="false">
                        <Items>
                            <ext:Container runat="server"
                                ID="ctForm"
                                Cls="form1Col ctAgregarImageForm">
                                <Items>
                                    <ext:FileUploadField
                                        ID="fuImagenOperador"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strImagen %>"
                                        LabelAlign="Top"
                                        Accept="image/svg"
                                        IconCls="ico-folder-bl"
                                        Cls=""
                                        ButtonText=""
                                        AllowBlank="false"
                                        CausesValidation="true"
                                        EmptyText=".svg">
                                        <Listeners>
                                            <Change Fn="validarExtensionImagenOperador" />
                                        </Listeners>
                                    </ext:FileUploadField>
                                    <ext:Image ID="imgImagenOperador"
                                        runat="server"
                                        Alt="<%$ Resources:Comun, strImagen %>"
                                        MinHeight="100"
                                        Cls="form-logo-cliente">
                                    </ext:Image>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardarImagenOperador"
                        meta:resourceKey="btnGuardarImagenOperador"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Cls="btn-accept"
                        Enabled="false">
                        <Listeners>
                            <Click Handler="winAsignarImagenOperadorGuardar()" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>


            <ext:Viewport runat="server" ID="vwResp" OverflowY="auto" Layout="FitLayout">
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Content>
                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                        </Content>
                        <Items>

                            <%-- PANEL CENTRAL--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 0"
                                Border="false"
                                Region="Center"
                                Layout="AnchorLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <DockedItems>
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <%--<ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strEntidades %>" Height="25" MarginSpec="10 0 10 0" />--%>
                                                    <%--<ext:Label runat="server" ID="lblCodCabecera" Cls="codigoCabecera" Text="COD182371" />
                                                    <ext:Label runat="server" ID="lblsubtituloNaranjPrincipal" Cls="subtltOrange" Text="Parent Name" />--%>
                                                <%--</Items>
                                            </ext:Toolbar>--%>

                                            <%--<ext:Toolbar runat="server" ID="tbNavNAside" Dock="Top" Cls="tbGrey tbNoborder" Hidden="false" PaddingSpec="10 10 10 10" OverflowHandler="Scroller" Flex="1">
                                                <Items>



                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkEntities"
                                                        meta:resourceKey="lnkSites"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="<%$ Resources:Comun, strEntidades %>">
                                                        <Listeners>
                                                            <Click Fn="NavegacionTabs" />
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkContacts"
                                                        meta:resourceKey="lnkLocation"
                                                        Cls="lnk-navView lnk-noLine"
                                                        Hidden="true"
                                                        Text="<%$ Resources:Comun, strContactos %>">
                                                        <Listeners>
                                                            <Click Handler="NavegacionTabs(this,'../../Componentes/GridEmplazamientosContactos.ascx', 'GridEmplazamientosContactos', '../../Componentes/js/GridEmplazamientosContactos.js')" />
                                                        </Listeners>
                                                    </ext:HyperlinkButton>

                                                </Items>
                                            </ext:Toolbar>--%>

                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Items>

                                    <ext:GridPanel
                                        runat="server"
                                        ID="grid"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel"
                                        StoreID="storePrincipal"
                                        Title="etiqgridTitle"
                                        Header="false"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%"
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        AriaRole="main">
                                        <Listeners>
                                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                            <ViewReady Handler="GridColHandlerDinamicoV2(this)"></ViewReady>
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnAnadir"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="AgregarEditar" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnEditar"
                                                        Cls="btnEditar"
                                                        AriaLabel="Editar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="MostrarEditar" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnEliminar"
                                                        Cls="btnEliminar"
                                                        AriaLabel="Eliminar"
                                                        ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="Eliminar" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnRefrescar"
                                                        Cls="btnRefrescar"
                                                        AriaLabel="Refrescar"
                                                        ToolTip="<%$ Resources:Comun,  btnRefrescar.ToolTip %>">
                                                        <Listeners>
                                                            <Click Fn="Refrescar" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnDescargar"
                                                        Cls="btnDescargar"
                                                        AriaLabel="Descargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Handler="ExportarDatos('Entidades', hdCliID.value, #{grid}, '-1','EXPORTARENTIDAD', #{cmbEntidades}.value);">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnGenerarPlantilla"
                                                        Hidden="true"
                                                        IconCls="ico-CntxMenuExcel"
                                                        AriaLabel="Generar plantilla"
                                                        ToolTip="<%$ Resources:Comun, btnGenerarPlantilla.ToolTip %>"
                                                        Handler="GenerarPlantillaEntidades();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnGestionContactos"
                                                        ToolTip="<%$ Resources:Comun, strContactos %>"
                                                        Cls="btnContactos"
                                                        Handler="GestionarContacto();" />
                                                    <ext:Button runat="server"
                                                        ID="btnAsignarImagen"
                                                        ToolTip="<%$ Resources:Comun, strAsignarImagen %>"
                                                        Cls="btnImage"
                                                        Handler="AsignarImagenOperador();"
                                                        Disabled="true" />
                                                    <ext:Button runat="server"
                                                        ID="btnEntidadCliente"
                                                        ToolTip="<%$ Resources:Comun, strCliente %>"
                                                        Cls="btnDefecto"
                                                        Handler="AsignarEntidadCliente();"
                                                        Disabled="true" />
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
                                                            <Select Handler="#{grid}.store.reload(); App.pnAsideR.collapse();document.getElementById('btnCollapseAsRClosed').style.transform = 'rotate(-180deg)';" />
                                                        </Listeners>
                                                    </ext:ComboBox>


                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                <Content>

                                                    <local:toolbarFiltros
                                                        ID="cmpFiltro"
                                                        runat="server"
                                                        Stores="storePrincipal"
                                                        MostrarComboFecha="false"
                                                        FechaDefecto="Dia"
                                                        QuitarBotonesFiltros="true"
                                                        Grid="grid"
                                                        MostrarBusqueda="false" />

                                                </Content>
                                            </ext:Container>

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
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />

                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePrincipal"
                                                OverflowHandler="Scroller"
                                                DisplayInfo="true"
                                                HideRefresh="true">
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

                                    <ext:Container ID="hugeCt" runat="server" Cls="hugeMainCt" Layout="FitLayout" Hidden="true">
                                        <%--<Content>
                                            <local:GridContactosGlobales ID="gridContactos"
                                                vista="ContactosEntidades"
                                                runat="server" />
                                        </Content>--%>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>


                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Items>
                                    <%-- PANEL MORE INFO--%>


                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowHandler="Scroller" OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>

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
