<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosLocalizacion.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosLocalizacion" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="System.Collections.Generic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <link href="css/styleEmplazamientosLocalizacion.css" rel="stylesheet" type="text/css" />
    <link href="../../css/tCore.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/EmplazamientosLocalizacion.js"></script>
    <title>Sites - Global</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <WindowResize Handler="GridResizer()" />
                </Listeners>
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>
            <%--INICIO  WINDOWS --%>
            <ext:Window runat="server"
                ID="winGestion"
                meta:resourceKey="winGestion"
                Title="Añadir Sitio"
                Resizable="False"
                Modal="True"
                Width="750px"
                Height="700px"
                Hidden="True">
                <Items>
                    <ext:Panel runat="server"
                        ID="pnVistasForm"
                        MonitorPoll="500"
                        MonitorValid="true"
                        ActiveIndex="2"
                        Border="false"
                        Cls="pnNavVistas pnVistasForm"
                        AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server"
                                ID="cntNavVistasForm"
                                Cls="nav-vistas"
                                ActiveIndex="2"
                                ActiveItem="2">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSiteF"
                                        meta:resourceKey="lnkSite"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="SITE"
                                        Handler="showForms(this);">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkLocationF"
                                        meta:resourceKey="lnkLocation"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="LOCATION"
                                        Handler="showForms(this);">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkAdditionalF"
                                        meta:resourceKey="lnkAdditional"
                                        Cls="lnk-navView  lnk-noLine"
                                        Text="ADDITIONAL"
                                        Handler="showForms(this);">
                                    </ext:HyperlinkButton>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:FormPanel runat="server"
                        ID="formSite"
                        Height="500"
                        Hidden="false">
                        <Items>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtSiteNameF"
                                        meta:resourceKey="txtSiteNameF"
                                        FieldLabel="Name"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtSiteCodeF"
                                        meta:resourceKey="txtSiteCodeF"
                                        FieldLabel="Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbOperatorF"
                                        meta:resourceKey="cmbOperatorF"
                                        Editable="false"
                                        FieldLabel="Operator"
                                        LabelAlign="Top"
                                        DisplayField="Operator"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Movistar" />
                                            <ext:ListItem Text="Vodafone" />
                                            <ext:ListItem Text="Orange" />
                                            <ext:ListItem Text="Yoigo" />
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
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtLandownerF"
                                        meta:resourceKey="txtLandownerF"
                                        FieldLabel="Landowner"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtStrcOwnerF"
                                        meta:resourceKey="txtStrcOwnerF"
                                        FieldLabel="Structure Owner"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbGlobalStateF"
                                        meta:resourceKey="cmbGlobalStateF"
                                        Editable="false"
                                        FieldLabel="Global State"
                                        LabelAlign="Top"
                                        DisplayField="Global State"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="On Air" />
                                            <ext:ListItem Text="Blocked" />
                                            <ext:ListItem Text="Uninstalled" />

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
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbCategoryF"
                                        meta:resourceKey="cmbCategoryF"
                                        Editable="false"
                                        FieldLabel="Category"
                                        LabelAlign="Top"
                                        DisplayField="Category"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Category 1" />
                                            <ext:ListItem Text="Category 2" />
                                            <ext:ListItem Text="Category 3" />
                                            <ext:ListItem Text="Category 4" />
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
                                        ID="cmbTypeSiteF"
                                        meta:resourceKey="cmbTypeSiteF"
                                        Editable="false"
                                        FieldLabel="Type of Site"
                                        LabelAlign="Top"
                                        DisplayField="Type"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Tipo 1" />
                                            <ext:ListItem Text="Tipo 2" />
                                            <ext:ListItem Text="Tipo 3" />
                                            <ext:ListItem Text="Tipo 4" />
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
                                        ID="cmbSizeSiteF"
                                        meta:resourceKey="cmbSizeSiteF"
                                        Editable="false"
                                        FieldLabel="Size of Site"
                                        LabelAlign="Top"
                                        DisplayField="Size"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Small" />
                                            <ext:ListItem Text="Medium" />
                                            <ext:ListItem Text="Large" />
                                            <ext:ListItem Text="Huge" />
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
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTypeStructureF"
                                        meta:resourceKey="cmbTypeStructureF"
                                        Editable="false"
                                        FieldLabel="Type of Structure"
                                        LabelAlign="Top"
                                        DisplayField="Structure"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Estructura 1" />
                                            <ext:ListItem Text="Estructura 2" />
                                            <ext:ListItem Text="Estructura 3" />
                                            <ext:ListItem Text="Estructura 4" />
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
                                        ID="cmbTypeBuildingF"
                                        meta:resourceKey="cmbTypeBuildingF"
                                        Editable="false"
                                        FieldLabel="Type of Building"
                                        LabelAlign="Top"
                                        DisplayField="Building"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Edificio 1" />
                                            <ext:ListItem Text="Edificio 2" />
                                            <ext:ListItem Text="Edificio 3" />
                                            <ext:ListItem Text="Edificio 4" />
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
                                        ID="cmbCluster"
                                        meta:resourceKey="cmbCluster"
                                        Editable="false"
                                        FieldLabel="Cluster"
                                        LabelAlign="Top"
                                        DisplayField="Cluster"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Cluster 1" />
                                            <ext:ListItem Text="Cluster 2" />
                                            <ext:ListItem Text="Cluster 3" />
                                            <ext:ListItem Text="Cluster 4" />
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
                        </Items>
                    </ext:FormPanel>
                    <ext:FormPanel runat="server"
                        ID="formLocation"
                        Height="500"
                        Hidden="true">
                        <Items>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbZoneF"
                                        meta:resourceKey="cmbOperatorF"
                                        Editable="false"
                                        FieldLabel="Zone"
                                        LabelAlign="Top"
                                        DisplayField="Zone"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Zona 1" />
                                            <ext:ListItem Text="Zona 2" />
                                            <ext:ListItem Text="Zona 3" />
                                            <ext:ListItem Text="Zona 4" />
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
                                        ID="cmbCountryF"
                                        meta:resourceKey="cmbCountryF"
                                        Editable="false"
                                        FieldLabel="Country"
                                        LabelAlign="Top"
                                        DisplayField="Country"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="País 1" />
                                            <ext:ListItem Text="País 2" />
                                            <ext:ListItem Text="País 3" />
                                            <ext:ListItem Text="País 4" />
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
                                        ID="cmbRegionF"
                                        meta:resourceKey="cmbRegionF"
                                        Editable="false"
                                        FieldLabel="Region"
                                        LabelAlign="Top"
                                        DisplayField="Region"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Region 1" />
                                            <ext:ListItem Text="Region 2" />
                                            <ext:ListItem Text="Region 3" />
                                            <ext:ListItem Text="Region 4" />
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
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbProvinceF"
                                        meta:resourceKey="cmbProvinceF"
                                        Editable="false"
                                        FieldLabel="Province"
                                        LabelAlign="Top"
                                        DisplayField="Province"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Provincia 1" />
                                            <ext:ListItem Text="Provincia 2" />
                                            <ext:ListItem Text="Provincia 3" />
                                            <ext:ListItem Text="Provincia 4" />
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
                                        ID="cmbMunicipalityF"
                                        meta:resourceKey="cmbMunicipalityF"
                                        Editable="false"
                                        FieldLabel="Municipality"
                                        LabelAlign="Top"
                                        DisplayField="Municipality"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Minicipio 1" />
                                            <ext:ListItem Text="Minicipio 2" />
                                            <ext:ListItem Text="Minicipio 3" />
                                            <ext:ListItem Text="Minicipio 4" />
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
                                        ID="cmbCityF"
                                        meta:resourceKey="cmbCityF"
                                        Editable="false"
                                        FieldLabel="City"
                                        LabelAlign="Top"
                                        DisplayField="City"
                                        Width="202"
                                        Cls="item-form comboForm">
                                        <Items>
                                            <ext:ListItem Text="Ciudad 1" />
                                            <ext:ListItem Text="Ciudad 2" />
                                            <ext:ListItem Text="Ciudad 3" />
                                            <ext:ListItem Text="Ciudad 4" />
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
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtAddressF"
                                        meta:resourceKey="txtAddressF"
                                        FieldLabel="Address"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtZipCodeF"
                                        meta:resourceKey="txtZipCodeF"
                                        FieldLabel="Zip Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:Button ID="btnGeo" Cls="btnGeo" runat="server"></ext:Button>
                                    <ext:Label ID="lblGeo" runat="server" Text="Geolocation"></ext:Label>
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkDegreesF"
                                        BoxLabel="Degrees"
                                        Width="202"
                                        Cls="chk-form" />
                                    <ext:TextField runat="server"
                                        ID="txtLongitudeF"
                                        meta:resourceKey="txtLongitudeF"
                                        FieldLabel="Longitude"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtLatitudeF"
                                        meta:resourceKey="txtLatitudeF"
                                        FieldLabel="Latitude"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                </Items>
                            </ext:FieldContainer>
                        </Items>
                    </ext:FormPanel>
                    <ext:FormPanel runat="server"
                        ID="formAdditional"
                        Height="450"
                        Scrollable="Vertical"
                        Hidden="true">
                        <Items>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtBuyer"
                                        meta:resourceKey="txtBuyer"
                                        FieldLabel="Buyer Company"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtMnemonic"
                                        meta:resourceKey="txtMnemonic"
                                        FieldLabel="Mnemonic"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtThirdF"
                                        meta:resourceKey="txtThirdF"
                                        FieldLabel="Site selling to third party"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtTowerCodeF"
                                        meta:resourceKey="txtTowerCodeF"
                                        FieldLabel="Tower Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtTelcoCodeF"
                                        meta:resourceKey="txtTelcoCodeF"
                                        FieldLabel="Telco Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtSAPCodeF"
                                        meta:resourceKey="txtSAPCodeF"
                                        FieldLabel="SAP Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtLandRegistryF"
                                        meta:resourceKey="txtLandRegistryF"
                                        FieldLabel="Address"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false" />
                                    <ext:TextField runat="server"
                                        ID="txtPotentialSiteF"
                                        meta:resourceKey="txtPotentialSiteF"
                                        FieldLabel="Zip Code"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtCourtRecordF"
                                        meta:resourceKey="txtCourtRecordF"
                                        FieldLabel="Court Record"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtVARIncomeF"
                                        meta:resourceKey="txtVARIncomeF"
                                        FieldLabel="VAR Average Income"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtSupervisorF"
                                        meta:resourceKey="txtSupervisorF"
                                        FieldLabel="Supervisor"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtAltitudeSiteF"
                                        meta:resourceKey="txtAltitudeSiteF"
                                        FieldLabel="Altitude of Site"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="80" Layout="HBox">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtDependingSitesF"
                                        meta:resourceKey="txtDependingSitesF"
                                        FieldLabel="Numbers of Depending Sites"
                                        MaxLength="100"
                                        MaxLengthText="Ha superado el máximo número de caracteres"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form"
                                        AllowBlank="false">
                                    </ext:TextField>
                                    <ext:DateField runat="server"
                                        ID="dateActivationF"
                                        meta:resourceKey="dateActivationF"
                                        FieldLabel="Activation Date"
                                        LabelAlign="Top"
                                        LabelWidth="125"
                                        Width="202"
                                        Cls="item-form dateField ">
                                    </ext:DateField>
                                    <ext:DateField runat="server"
                                        ID="DateDeactivationF"
                                        meta:resourceKey="DateDeactivationF"
                                        FieldLabel="Deactivation Date"
                                        LabelAlign="Top"
                                        LabelWidth="125"
                                        Width="202"
                                        Cls="item-form dateField ">
                                    </ext:DateField>
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server" Height="120" Layout="HBox">
                                <Items>
                                    <ext:TextArea runat="server"
                                        ID="txaCommentBuildingF"
                                        meta:resourceKey="txaCommentBuildingF"
                                        FieldLabel="Comments about Building"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form">
                                    </ext:TextArea>
                                    <ext:TextArea runat="server"
                                        ID="txaEnSituationF"
                                        meta:resourceKey="txaEnSituationF"
                                        FieldLabel="Engineering Situation"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form">
                                    </ext:TextArea>
                                    <ext:TextArea runat="server"
                                        ID="txaCommentTeamF"
                                        meta:resourceKey="txaCommentTeamF"
                                        FieldLabel="Comments about Teams"
                                        LabelAlign="Top"
                                        Width="202"
                                        Cls="item-form">
                                    </ext:TextArea>
                                </Items>
                            </ext:FieldContainer>
                            <ext:TextArea runat="server"
                                ID="txaCommentsF"
                                meta:resourceKey="txaCommentsF"
                                FieldLabel="Comments"
                                LabelAlign="Top"
                                Width="640"
                                Cls="item-form">
                            </ext:TextArea>
                        </Items>
                    </ext:FormPanel>
                    <ext:ButtonGroup runat="server" ID="btnsForm">
                        <Items>
                            <ext:Button runat="server"
                                ID="btnPrev"
                                meta:resourceKey="btnPrev"
                                Cls="btn-secondary-winForm"
                                Text="Anterior">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnNext"
                                meta:resourceKey="btnNext"
                                Cls="btn-ppal-winForm"
                                Text="Siguiente">
                            </ext:Button>
                        </Items>
                    </ext:ButtonGroup>
                </Items>
            </ext:Window>


            <ext:Window
                runat="server" ID="WinContractDetails"
                meta:resourceKey="WinContractDetails"
                Width="480"
                MinHeight="220"
                Hidden="true"
                Title="Contracts"
                IconCls="ico-contrato-gr"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                X="200"
                Y="200"
                MaxHeight="500"
                Layout="FitLayout"
                OverflowY="Auto">
                <Items>
                    <ext:GridPanel
                        ID="GridContractsD"
                        runat="server"
                        MultiSelect="true"
                        ForceFit="true"
                        Border="false"
                        Header="false"
                        Cls="gridPanel"
                        OverflowY="Auto">
                        <Store>
                            <ext:Store ID="StoreDocs" runat="server">
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>

                                            <ext:ModelField Name="Type" />
                                            <ext:ModelField Name="Number" />

                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server" Text="Type" DataIndex="Type" Width="140" />
                                <ext:Column runat="server" Text="Number" DataIndex="Number" Width="110">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true" />
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:Window>


            <ext:Window
                runat="server" ID="WinProjectsDetails"
                meta:resourceKey="WinProjectsDetails"
                Width="480"
                MinHeight="220"
                Hidden="true"
                Title="Projects"
                IconCls="ico-folder-gr"
                Cls="WinTrackDocum WinContractD"
                Border="false"
                Resizable="false"
                Closable="true"
                X="200"
                Y="200"
                MaxHeight="500"
                Layout="FitLayout"
                OverflowY="Auto">
                <Items>
                    <ext:GridPanel
                        ID="GridProjectsD"
                        runat="server"
                        MultiSelect="true"
                        ForceFit="true"
                        Border="false"
                        Header="false"
                        Cls="gridPanel"
                        OverflowY="Auto">
                        <Store>
                            <ext:Store ID="Store1" runat="server">
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Project" />
                                            <ext:ModelField Name="Number" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server" Text="Project" DataIndex="Project" Width="140" />
                                <ext:Column runat="server" Text="Number" DataIndex="Number" Width="110">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true" />
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:Window>


            <%--FIN  WINDOWS --%>
            <%--INICIO  VIEWPORT --%>
            <ext:Viewport ID="vwSites" runat="server" Layout="FitLayout">
                <Items>

                    <ext:GridPanel
                        runat="server"
                        ID="grdTask"
                        ForceFit="true"
                        Cls="gridPanel grdNoHeader">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid c-Grid" Layout="ColumnLayout">

                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAnadir"
                                        Cls="btn-trans"
                                        AriaLabel="Añadir"
                                        ToolTip="Añadir"
                                        Handler="anadir();" />
                                    <ext:Button runat="server"
                                        ID="btnEditar"
                                        Cls="btn-trans"
                                        AriaLabel="Editar"
                                        ToolTip="Editar"
                                        Handler="editar();" />
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        Cls="btn-trans"
                                        AriaLabel="Eliminar"
                                        ToolTip="Eliminar" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        Cls="btn-trans"
                                        AriaLabel="Actualizar"
                                        ToolTip="Actualizar" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        Cls="btn-trans btnDescargar"
                                        AriaLabel="Descargar"
                                        ToolTip="Descargar" />
                                    <ext:Button runat="server"
                                        ID="btnContratos"
                                        Cls="btn-trans"
                                        AriaLabel="Contratos"
                                        ToolTip="Contratos" />
                                    <ext:Button runat="server"
                                        ID="btnProjects"
                                        Cls="btn-trans"
                                        AriaLabel="Añadir a Proyecto"
                                        ToolTip="Añadir a Proyecto" />
                                    <ext:Button runat="server"
                                        ID="btnDirecciones"
                                        Cls="btn-trans"
                                        AriaLabel="Direcciones"
                                        ToolTip="Direcciones" />
                                    <ext:Button runat="server"
                                        ID="btnContactos"
                                        Cls="btn-trans"
                                        AriaLabel="Contactos"
                                        ToolTip="Contactos" />
                                    <ext:Button runat="server"
                                        ID="btnHistorial"
                                        Cls="btn-trans"
                                        AriaLabel="Historial"
                                        ToolTip="Historial" />
                                    <ext:Button runat="server"
                                        ID="btnVandalismo"
                                        Cls="btn-trans"
                                        AriaLabel="Vandalismo"
                                        ToolTip="Vandalismo" />
                                    <ext:Button runat="server"
                                        ID="btnRenta"
                                        Cls="btn-trans"
                                        AriaLabel="Actualizar Rentas"
                                        ToolTip="Actualizar Rentas" />
                                    <ext:Button runat="server"
                                        ID="btnCompartido"
                                        Cls="btn-trans"
                                        AriaLabel="Emplazamiento Compartido"
                                        ToolTip="Emplazamiento Compartido" />
                                    <ext:Label runat="server"
                                        ID="lblToggle"
                                        Text="Client Sites">
                                    </ext:Label>
                                    <ext:Button runat="server"
                                        ID="btnCliente"
                                        Width="41"
                                        EnableToggle="true"
                                        Cls="btn-toggleGrid"
                                        AriaLabel="Emplazamientos del cliente"
                                        ToolTip="Emplazamiento del cliente" />
                                    <ext:Button runat="server"
                                        ID="btnFiltros"
                                        Cls="btn-trans"
                                        EnableToggle="true"
                                        AriaLabel="Mostrar Filtros"
                                        ToolTip="Mostrar Panel Filtros"
                                        Handler="showPnAsideR(this.id);" />
                                    <ext:Button runat="server"
                                        ID="btnBuscar"
                                        Cls="btn-trans"
                                        EnableToggle="true"
                                        AriaLabel="Buscar"
                                        ToolTip="Mostrar Panel Buscar"
                                        Handler="showPnAsideR(this.id);" />
                                    <ext:Button runat="server"
                                        ID="btnOpciones"
                                        Cls="btn-trans"
                                        AriaLabel="Opciones"
                                        ToolTip="Más opciones" />
                                </Items>

                            </ext:Toolbar>
                            <ext:Toolbar runat="server" ID="tbFiltros" Cls="tlbGrid" Layout="ColumnLayout">
                                <Items>

                                    <ext:TextField
                                        ID="txtSearch"
                                        Cls="txtSearchC"
                                        runat="server"
                                        EmptyText="Search"
                                        LabelWidth="50"
                                        Width="250">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Search" />
                                        </Triggers>
                                    </ext:TextField>

                                    <ext:FieldContainer runat="server" ID="FCCombos" Cls="FloatR FCCombos" Layout="HBoxLayout">
                                        <Defaults>
                                            <ext:Parameter Name="margin" Value="0 5 0 0" Mode="Value" />
                                        </Defaults>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Middle" />
                                        </LayoutConfig>
                                        <Items>
                                            <ext:ComboBox runat="server" ID="cmbProyectos" Cls="comboGrid  " EmptyText="Projects" Flex="1">
                                                <Items>
                                                    <ext:ListItem Text="Proyecto 1" />
                                                    <ext:ListItem Text="Proyecto 2" />
                                                </Items>
                                            </ext:ComboBox>

                                            <ext:ComboBox runat="server" ID="cmbEmplazamientos" Cls="comboGrid  " EmptyText="Tipologías" Flex="1">
                                                <Items>
                                                    <ext:ListItem Text="Tipología 1" />
                                                    <ext:ListItem Text="Tipología 2" />
                                                    <ext:ListItem Text="Tipología 3" />
                                                    <ext:ListItem Text="Tipología 4" />
                                                </Items>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:FieldContainer>




                                    <ext:FieldContainer runat="server" ID="ContButtons" Cls="FloatR ContButtons">
                                        <Items>
                                            <ext:Button runat="server" Width="30" ID="Button1" Cls="btn-trans btnColumnas" AriaLabel="Duplicar Tipología" ToolTip="Duplicar Tipología"></ext:Button>
                                            <ext:Button runat="server" Width="30" ID="btnClearFilters" Cls="btn-trans btnRemoveFilters" AriaLabel="Quitar Filtros" ToolTip="Quitar Filtros"></ext:Button>
                                            <ext:Button runat="server" Width="30" ID="Button3" Cls="btn-trans btnFiltroNegativo" AriaLabel="Ver Workflow" ToolTip="Ver Workflow" Handler="ShowWorkFlow();"></ext:Button>
                                        </Items>
                                    </ext:FieldContainer>




                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Store>
                            <ext:Store runat="server">
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Project" />
                                            <ext:ModelField Name="Name" />
                                            <ext:ModelField Name="Code" Type="String" />
                                            <ext:ModelField Name="Operator" />
                                            <ext:ModelField Name="StrOwner" />
                                            <ext:ModelField Name="Landowner" />
                                            <ext:ModelField Name="GlobalState" />
                                            <ext:ModelField Name="Category" />
                                            <ext:ModelField Name="Type" />
                                            <ext:ModelField Name="Size" />
                                            <ext:ModelField Name="Structure" />
                                            <ext:ModelField Name="Cluster" />
                                            <ext:ModelField Name="Subsites" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>


                                <ext:Column
                                    meta:resourceKey="colName"
                                    ID="colName"
                                    runat="server"
                                    Text="Name"
                                    DataIndex="Name" />


                                <ext:Column runat="server" ID="colCode" DataIndex="Code" Text="Code"
                                    meta:resourceKey="colName">
                                    <%--<Renderer Fn="linkRendererTrack"></Renderer>--%>
                                </ext:Column>


                                <ext:WidgetColumn
                                    meta:resourceKey="colProject"
                                    ID="colProject"
                                    Align="Center"
                                    runat="server"
                                    Text="Project"
                                    DataIndex="Project"
                                    MinWidth="80"
                                    MaxWidth="80">

                                    <Widget>
                                        <ext:Button ID="ProjectBtn" Cls="ico-masInfo-widget btnProjectsNContracts" Handler="ClickShowProjectsD()" OverCls="none" PressedCls="none" FocusCls="none" runat="server"></ext:Button>
                                    </Widget>
                                </ext:WidgetColumn>

                                <ext:WidgetColumn
                                    meta:resourceKey="colContracts"
                                    ID="colContracts"
                                    Align="Center"
                                    runat="server"
                                    Text="Contracts"
                                    DataIndex="Project"
                                    MinWidth="80"
                                    MaxWidth="80">
                                    <Widget>
                                        <ext:Button ID="ContractsBtn" Cls="ico-masInfo-widget btnProjectsNContracts" Handler="ClickShowContractD()" OverCls="none" PressedCls="none" FocusCls="none" runat="server"></ext:Button>
                                    </Widget>
                                </ext:WidgetColumn>

                                <ext:Column
                                    meta:resourceKey="colOperator"
                                    ID="colOperator"
                                    runat="server"
                                    Text="Operator"
                                    DataIndex="Operator" />
                                <ext:Column
                                    meta:resourceKey="colStrOwner"
                                    ID="colStrOwner"
                                    runat="server"
                                    Text="Str Owner"
                                    DataIndex="StrOwner" />
                                <ext:Column
                                    meta:resourceKey="colLandowner"
                                    ID="colLandowner"
                                    runat="server"
                                    Text="Landowner"
                                    DataIndex="Landowner" />
                                <ext:WidgetColumn
                                    meta:resourceKey="colGlobalState"
                                    ID="colGlobalState"
                                    runat="server"
                                    Text="Global State"
                                    DataIndex="GlobalState"
                                    Width="120">
                                    <Widget>
                                        <ext:ComboBox runat="server" Cls="miniCombo">
                                            <Items>
                                                <ext:ListItem Text="On Aire" />
                                                <ext:ListItem Text="Blocked" />
                                                <ext:ListItem Text="Uninstalled" />
                                            </Items>
                                        </ext:ComboBox>
                                    </Widget>
                                </ext:WidgetColumn>

                                <ext:Column
                                    meta:resourceKey="colCategory"
                                    ID="colCategory"
                                    runat="server"
                                    Text="Category"
                                    DataIndex="Category" />
                                <ext:Column
                                    meta:resourceKey="colType"
                                    ID="colType"
                                    runat="server"
                                    Text="Type"
                                    DataIndex="Type" />
                                <ext:Column
                                    meta:resourceKey="colSize"
                                    ID="colSize"
                                    runat="server"
                                    Text="Size"
                                    DataIndex="Size" />
                                <ext:Column
                                    meta:resourceKey="colStructure"
                                    ID="colStructure"
                                    runat="server"
                                    Text="Structure"
                                    DataIndex="Structure" />
                                <ext:Column
                                    meta:resourceKey="colCluster"
                                    ID="colCluster"
                                    runat="server"
                                    Text="Cluster"
                                    DataIndex="Cluster" />
                                <ext:Column
                                    meta:resourceKey="colSubsites"
                                    ID="colSubsites"
                                    runat="server"
                                    Text="Subsites"
                                    DataIndex="Subsites"
                                    MaxWidth="100" />

                                <ext:WidgetColumn meta:resourcekey="ColMas" ID="ColMas" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="true" MinWidth="70">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="btnColMore" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                            <Listeners>
                                                <Click Fn="MostrarPanelMoreInfo" />
                                            </Listeners>
                                        </ext:Button>
                                    </Widget>
                                </ext:WidgetColumn>

                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                runat="server"
                                ID="GridRowSelect"
                                Mode="Multi">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectEmplazamiento" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                    </ext:GridPanel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
