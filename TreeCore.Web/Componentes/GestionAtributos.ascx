<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GestionAtributos.ascx.cs" Inherits="TreeCore.Componentes.GestionAtributos" %>

<script type="text/javascript" src="../../Componentes/js/GestionAtributos.js"></script>
<!--<script type="text/javascript" src="/JS/common.js"></script>-->

<%--Stores--%>



<%--Componente--%>

<ext:Container runat="server" Cls="mainContainer" ID="mainConstainer">
    <Items>
        <ext:Container runat="server" Cls="headerContainerFlex containerHeight">
            <Items>
                <ext:Label runat="server" 
                    Cls="headerAligne txtLblAttribute ico-exclamacion-10px-grey textStyleUnification" 
                    ID="lbNombreAtr" 
                    Border="false" >
                </ext:Label>
            </Items>
        </ext:Container>
        <ext:Container runat="server" Cls="flexItems">
            <Items>
                <ext:Container
                    runat="server"
                    ID="contenedorControl">
                    <Items>
                    </Items>
                    <Listeners>
                        <AfterRender Fn="AnadirListener"/>
                    </Listeners>
                </ext:Container>
            </Items>
        </ext:Container>
    </Items>
    <Listeners>
        <Render Fn="IsHidden"/>
        <Hide Fn="EsconderPadre"/>
    </Listeners>
</ext:Container>
