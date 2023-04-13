<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GestionCategoriasAtributos.ascx.cs" Inherits="TreeCore.Componentes.GestionCategoriasAtributos" %>

<script type="text/javascript" src="../../Componentes/js/GestionCategoriasAtributos.js"></script>

<%--Stores--%>



<%--Componente--%>

<ext:FieldSet
    runat="server"
    ID="containerAttributes"
    Padding="0"
    Cls="containerAttributes">
    <Items>
        <ext:Hidden runat="server" ID="hdEsPlantilla" />
        <ext:Toolbar runat="server">
            <Items>
                <ext:Label runat="server"
                    ID="lbNombreCategoria"
                    Cls="btnCategory"
                    Height="41" />
                <ext:ComboBox runat="server"
                    ID="cmbPlantilla"
                    meta:resourceKey="cmbPlantilla"
                    DisplayField="Nombre"
                    ValueField="CoreInventarioPlantillaAtributoCategoriaID"
                    Cls="comboGrid pos-boxGrid"
                    QueryMode="Local"
                    Width="200"
                    Hidden="true"
                    LabelAlign="Top"
                    FieldLabel="<%$ Resources:Comun, strPlantilla %>">
                    <Store>
                        <ext:Store runat="server"
                            ID="storePlantillas"
                            RemotePaging="false"
                            AutoLoad="false"
                            OnReadData="storePlantillas_Refresh"
                            RemoteSort="false"
                            PageSize="20"
                            RemoteFilter="false">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Model>
                                <ext:Model runat="server" IDProperty="CoreInventarioPlantillaAtributoCategoriaID">
                                    <Fields>
                                        <ext:ModelField Name="CoreInventarioPlantillaAtributoCategoriaID" Type="Int" />
                                        <ext:ModelField Name="Nombre" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Listeners>
                        <Select Fn="SeleccionarPlantillaSubCat" />
                        <TriggerClick Fn="LimpiarCategoria" />
                        <Change Fn="SetTriggerCmbPlantilla" />
                        <AfterRender Fn="CargarStorePlantilla" />
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
        </ext:Toolbar>
        <ext:Container
            runat="server"
            ID="flexContainer"
            Cls="">
            <Items>
            </Items>
        </ext:Container>
    </Items>
</ext:FieldSet>
