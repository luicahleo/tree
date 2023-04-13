<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigInicialForm.aspx.cs" Inherits="TreeCore.ModGlobal.ConfigInicialForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <script type="text/javascript" src="js/ConfigInicialForm.js"></script>
            <%--INICIO HIDDEN --%>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <ext:Hidden ID="hdEntidadID" runat="server" />
            <%--FIN HIDDEN --%>


            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store ID="storeIdiomas" runat="server" AutoLoad="false" OnReadData="storeIdiomas_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="IdiomaID" runat="server">
                        <Fields>
                            <ext:ModelField Name="IdiomaID" />
                            <ext:ModelField Name="Idioma" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Idioma" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store ID="storePais" runat="server" AutoLoad="false" OnReadData="storePais_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="PaisID" runat="server">
                        <Fields>
                            <ext:ModelField Name="PaisID" />
                            <ext:ModelField Name="Pais" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Pais" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeMunicipios" runat="server" AutoLoad="false" OnReadData="storeMunicipios_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="MunicipioID" runat="server">
                        <Fields>
                            <ext:ModelField Name="MunicipioID" />
                            <ext:ModelField Name="Municipio" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Municipios" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeMonedas" runat="server" AutoLoad="false" OnReadData="storeMonedas_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="MonedaID" runat="server">
                        <Fields>
                            <ext:ModelField Name="MonedaID" />
                            <ext:ModelField Name="Moneda" />
                            <ext:ModelField Name="Simbolo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Moneda" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>
            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>

                    <ext:FormPanel ID="pnFormProductCatalog" Cls="formGris formResp" runat="server" Hidden="false">
                        <Items>
                            <ext:Container runat="server" ID="ctUsuario" Hidden="false">
                                <Items>
                                    <ext:FormPanel runat="server" ID="pnUsuarios" Cls="winGestion-panel ctForm-resp ctForm-col2-resp">
                                        <Items>
                                            <ext:TextField runat="server"
                                                ID="txtNombre"
                                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true" />
                                            <ext:TextField runat="server"
                                                ID="txtApellidos"
                                                FieldLabel="<%$ Resources:Comun, strApellidos %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true" />

                                            <ext:TextField runat="server"
                                                meta:resourcekey="EMail"
                                                ID="txtEMail"
                                                FieldLabel="<%$ Resources:Comun, strEmail %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                Vtype="email"
                                                ValidationGroup="FORM"
                                                CausesValidation="true" />

                                            <ext:TextField runat="server"
                                                meta:resourcekey="clave"
                                                ID="txtClave"
                                                FieldLabel="<%$ Resources:Comun, strContraseña %>"
                                                EmptyText="<%$ Resources:Comun, strPasswordStrengthEmptyText %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true"
                                                EnableKeyEvents="true"
                                                InputType="Password"
                                                Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                                                RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                                                <Listeners>
                                                    <Change Handler="ClaveIgual()" Buffer="100" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server"
                                                meta:resourcekey="clave"
                                                ID="txtClaveRepite"
                                                FieldLabel="<%$ Resources:Comun, strConfirmarContraseña %>"
                                                EmptyText="<%$ Resources:Comun, strPasswordStrengthEmptyText %>"
                                                LabelAlign="Top"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true"
                                                EnableKeyEvents="true"
                                                InputType="Password"
                                                Regex="<%$ Resources:Comun, strPasswordStrengthRegExp %>"
                                                RegexText="<%$ Resources:Comun, strPasswordStrengthText %>">
                                                <Listeners>
                                                    <Change Handler="ClaveIgual()" Buffer="100" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                        <Listeners>
                                            <ValidityChange Handler="FormularioValidoUsuarios(valid)" />
                                        </Listeners>
                                    </ext:FormPanel>

                                </Items>
                                <Listeners>
                                </Listeners>
                            </ext:Container>

                            <ext:Container runat="server" ID="ctIdioma" Cls="winGestion-panel ctForm-resp ctForm-col2-resp" Hidden="true">
                                <Items>

                                    <ext:ComboBox runat="server"
                                        meta:resourcekey="cmbPais"
                                        ID="cmbPais"
                                        Mode="Local"
                                        FieldLabel="<%$ Resources:Comun, strPaises %>"
                                        DisplayField="Pais"
                                        ValueField="PaisID"
                                        StoreID="storePais"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        Editable="true"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<TriggerClick Fn="RecargarMonedas" />
                                                            <Select Fn="SeleccionarMoneda" />--%>
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        meta:resourcekey="cmbIdioma"
                                        ID="cmbIdiomas"
                                        Mode="Local"
                                        FieldLabel="<%$ Resources:Comun, strIdioma %>"
                                        DisplayField="Idioma"
                                        ValueField="IdiomaID"
                                        StoreID="storeIdiomas"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        Editable="true"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<TriggerClick Fn="RecargarMonedas" />
                                                            <Select Fn="SeleccionarMoneda" />--%>
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                </Items>

                            </ext:Container>

                            <ext:Container runat="server" ID="ctEntidad" Cls="winGestion-panel ctForm-resp ctForm-col2-resp" Hidden="true">
                                <Items>

                                    <ext:TextField runat="server"
                                        ID="txtNombreEntidad"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCodigo"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />

                                    <ext:TextField runat="server"
                                        meta:resourcekey="direccion"
                                        ID="txtDireccion"
                                        FieldLabel="<%$ Resources:Comun, strDireccion %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />

                                    <ext:ComboBox runat="server"
                                        meta:resourcekey="cmbMunicipio"
                                        ID="cmbMunicipio"
                                        Mode="Local"
                                        FieldLabel="<%$ Resources:Comun, strMunicipio %>"
                                        DisplayField="Municipio"
                                        ValueField="MunicipioID"
                                        StoreID="storeMunicipios"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        Editable="true"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<TriggerClick Fn="RecargarMonedas" />
                                                            <Select Fn="SeleccionarMoneda" />--%>
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                    <ext:FileUploadField
                                        ID="FileImagen"
                                        runat="server"
                                        meta:resourceKey="FileImagen"
                                        AllowBlank="true"
                                        EmptyText="Seleccione una Imagen"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strImagen %>"
                                        ButtonText=""
                                        IconCls="ico-image">
                                        <Listeners>
                                        </Listeners>
                                    </ext:FileUploadField>

                                </Items>

                            </ext:Container>

                            <ext:Container runat="server" ID="ctMonedas" Cls="winGestion-panel ctForm-resp ctForm-col2-resp" Hidden="true">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        meta:resourcekey="cmbMonedas"
                                        ID="cmbMonedas"
                                        Mode="Local"
                                        FieldLabel="<%$ Resources:Comun, strMonedas %>"
                                        DisplayField="Moneda"
                                        ValueField="MonedaID"
                                        StoreID="storeMonedas"
                                        QueryMode="Local"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        Editable="true"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<TriggerClick Fn="RecargarMonedas" />
                                                            <Select Fn="SeleccionarMoneda" />--%>
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:FieldContainer runat="server"
                                        ID="chkRequired"
                                        Cls="chkCompleto noPadding"
                                        Height="100"
                                        Layout="HBoxLayout">
                                        <Items>
                                            <ext:Checkbox runat="server" ID="chkControlMoneda" Checked="false" BoxLabel="<%$ Resources:Comun, strAnadir %>" AllowBlank="true" Hidden="false">
                                                <Listeners>
                                                    <Change Fn="AnadirMoneda" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </Items>
                                    </ext:FieldContainer>
                                    <ext:Container runat="server" ID="ctNuevaMoneda" Hidden="true" Cls="dosColumnasInicio noPadding">
                                        <Items>
                                            <ext:TextField runat="server"
                                                ID="txtMoneda"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                MaxLength="50"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true" />
                                            <ext:TextField runat="server"
                                                ID="txtSimbolo"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strSimbolo %>"
                                                MaxLength="10"
                                                ValidationGroup="FORM"
                                                AllowBlank="false"
                                                CausesValidation="true" />
                                            <ext:NumberField runat="server"
                                                ID="txtDolar"
                                                MaxLength="20"
                                                ValidationGroup="FORM"
                                                AllowBlank="false"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strCambioDolar %>"
                                                CausesValidation="true"
                                                DecimalPrecision="4"
                                                meta:resourceKey="txtDolar" />
                                            <ext:NumberField runat="server"
                                                ID="txtEuro"
                                                MaxLength="20"
                                                ValidationGroup="FORM"
                                                AllowBlank="false"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strCambioEuro %>"
                                                CausesValidation="true"
                                                DecimalPrecision="4"
                                                meta:resourceKey="txtEuro" />
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>

                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="TbNavegacionTabs" Dock="Top"
                                Padding="20" Cls="tbGrey" Hidden="false" MinHeight="36">
                                <Items>

                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkUsuario"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strAdmin %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkIdioma"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strIdioma %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkEntidades"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strEntidad %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkMonedas"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="<%$ Resources:Comun, strMonedas %>">
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server" ID="Toolbar10" Cls="greytb" Dock="Bottom" Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server" ID="btnPrev" Cls="btn-secondary" MinWidth="110" Text="<%$ Resources:Comun, strAnterior %>" Focusable="false" PressedCls="none" Hidden="false">
                                        <Listeners>
                                            <Click Fn="btnPrev_Click" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnNext" Cls="btn-ppal" Text="<%$ Resources:Comun, strSiguiente %>" Focusable="false" PressedCls="none" Hidden="false" Disabled="false">
                                        <Listeners>
                                            <Click Fn="btnNext_Click" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnAgregar" Cls="btn-ppal" Text="<%$ Resources:Comun, strGuardar %>" Focusable="false" PressedCls="none" Hidden="true">
                                        <Listeners>
                                            <Click Handler="winGestionGuardar()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>

                    </ext:FormPanel>

                </Items>

            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
