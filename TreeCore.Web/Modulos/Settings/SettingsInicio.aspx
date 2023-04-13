<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettingsInicio.aspx.cs" Inherits="TreeCore.Modulos.Settings.SettingsInicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/CSS/HomeStyle.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>
            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">
                <Items>

                    <ext:Container ID="ctMain1"
                        runat="server"
                        Hidden="false"
                        Cls="col-pdng"
                        Scrollable="Vertical">

                        <Items>
                            <ext:Panel runat="server" ID="SettingsHeader" Cls="HeaderInicio">
                                <Items>
                                    <ext:Image
                                        runat="server"
                                        Src="../../ima/modulos/modSwapping.svg"
                                        Height="80"
                                        Width="80"
                                        MarginSpec="0 5 0 0"
                                        Cls="image-title"
                                        Alt="Third Party" />
                                    <ext:Label ID="ExpMaintitle"
                                        meta:resourceKey="lblExpMaintitle"
                                        runat="server"
                                        Cls="big-lbl-title LblElipsis TituloCabecera"
                                        Text="Settings"
                                        Flex="8">
                                    </ext:Label>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
