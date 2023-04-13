<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiagramaEstados.aspx.cs" Inherits="TreeCore.PaginasComunes.DiagramaEstados" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Import Namespace="System.Collections.Generic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleStates.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server"
            ID="ResourceManagerTreeCore"
            DirectMethodNamespace="TreeCore"/>
            <div>
                <ext:Panel runat="server"
                    ID="pnShowWorkflow"
                    Hidden="true">
                    <Content runat="server">
                        <div runat="server" class="dvTypo">
                            <ext:Label runat="server"
                                ID="lblTypoView"
                                meta:resourceKey="lblTypoView"
                                Text="Tipologia " />
                            <ext:Label runat="server"
                                ID="lblTipology"
                                meta:resourceKey="lblTypoView"
                                Text=""
                                Cls="lblTypoForm" />
                        </div>
                        <div runat="server" id="dvCntStates" class="dvAllStates">
                            <div runat="server" id="prevCntStates" class="dvCntState">
                                <span class="dotFlow ancla lineFlow"></span>
                            </div>
                            <div id="currentState" class="dvCntState">
                                <span class="dotCurrent">
                                    <span id="dotFlowCurrent"
                                        class="dotFlow ancla lineFlow"></span>
                                </span>
                                <ext:Label runat="server"
                                    ID="lblCurrState"
                                    meta:resourceKey="lblTypoView"
                                    Text=""
                                    Cls="lblTypoForm" />
                            </div>
                            <div runat="server" id="nextCntState" class="dvCntState">
                                <div id="nextState1" class="dvState">
                                    <span class="dotFlow"></span>
                                </div>
                            </div>
                        </div>
                    </Content>
                </ext:Panel>
            </div>
    </form>
</body>
</html>
