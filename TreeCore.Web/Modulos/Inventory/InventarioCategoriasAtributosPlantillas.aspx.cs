using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Transactions;
using System.Linq;
using System.Globalization;


namespace TreeCore.ModInventario
{
    public partial class InventarioCategoriasAtributosPlantillas : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        List<Componentes.GestionCategoriasAtributos> listaCategorias;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                //Comun.CreateGridFilters(gridFilter, storePrincipal, GridPanelCategorias.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //#endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(GridPanelCategorias, storePrincipal, GridPanelCategorias.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                hdVistaPlantilla.Value = true;
                hdCatSelect.Value = 0;
                hdCatConfSelect.Value = 0;
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<JsonObject> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(CliID, true, true);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(GridPanelCategorias.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        Response.Write("ERROR: " + ex.Message);
                    }

                    Response.End();
                }
                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "InventarioCategoriasAtributosPlantillas.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { btnAnadirPlantilla }},
            { "Put", new List<ComponentBase> { btnEditarPlantilla }},
            { "Delete", new List<ComponentBase> { btnEliminarPlantilla }}
        };
            PintarCategorias(false);
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaPrincipal(long.Parse(hdCliID.Value.ToString()), btnActivos.Pressed, true);

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<JsonObject> ListaPrincipal(long lClienteID, bool Activo, bool Plantillas)
        {
            List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaDatos;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();

            List<JsonObject> listaJson = new List<JsonObject>();
            JsonObject jsonAux;

            try
            {
                listaDatos = cCategoriasConf.GetPlantillas(lClienteID, Activo);
                foreach (var item in listaDatos)
                {
                    jsonAux = new JsonObject();
                    jsonAux.Add("InventarioAtributoCategoriaID", item.InventarioAtributoCategoriaID);
                    jsonAux.Add("InventarioAtributoCategoria", item.InventarioAtributosCategorias.InventarioAtributoCategoria);
                    jsonAux.Add("Activo", item.InventarioAtributosCategorias.Activo);
                    jsonAux.Add("CoreInventarioCategoriaAtributoCategoriaConfiguracionID", item.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
                    listaJson.Add(jsonAux);
                }
            }
            catch (Exception ex)
            {
                listaJson = null;
                log.Error(ex.Message);
            }

            return listaJson;
        }

        #endregion

        #region PLANTILLAS

        protected void storePlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
                CoreInventarioPlantillasAtributosCategoriasAtributosController cPlantillasAtr = new CoreInventarioPlantillasAtributosCategoriasAtributosController();
                InventarioPlantillasAtributosJsonController cPlantillaJson = new InventarioPlantillasAtributosJsonController();
                Data.CoreAtributosConfiguraciones atr;
                CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
                CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
                string sValor;

                List<JsonObject> listaJson = new List<JsonObject>();
                JsonObject jsonAux;

                try
                {
                    long lCatConfID = long.Parse(hdCatConfSelect.Value.ToString());

                    var vLista = cPlantillas.GetPlantillasConf(lCatConfID);

                    #region Cargas Listas

                    JsonObject listasItems = new JsonObject();
                    JsonObject listaItems = new JsonObject();
                    JsonObject auxJson;

                    foreach (var oAtr in cCategoriasConf.GetListaAtributos(lCatConfID))
                        {
                        if (oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA || oAtr.TiposDatos.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                        {
                            if (oAtr.TablaModeloDatoID != null)
                            {
                                listaItems = cAtributosConf.GetJsonItemsComboBox((long)oAtr.CoreAtributoConfiguracionID);

                            }
                            else if (oAtr.ValoresPosibles != null && oAtr.ValoresPosibles != "")
                            {
                                listaItems = new JsonObject();
                                foreach (var val in oAtr.ValoresPosibles.Split(';'))
                                {
                                    try
                                    {
                                        auxJson = new JsonObject();
                                        auxJson.Add("Value", val);
                                        auxJson.Add("Text", val);
                                        listaItems.Add(val, auxJson);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            listasItems.Add(oAtr.CoreAtributoConfiguracionID.ToString(), listaItems);
                        }
                    }

                    #endregion

                    foreach (var Plan in vLista)
                    {
                        jsonAux = new JsonObject();
                        jsonAux.Add("CoreInventarioPlantillaAtributoCategoriaID", Plan.CoreInventarioPlantillaAtributoCategoriaID);
                        jsonAux.Add("Nombre", Plan.Nombre);
                                                
                        var listAtr = cPlantillaJson.Deserializacion(Plan.JsonAtributosDinamicos);
                        foreach (var oAtr in listAtr)
                        {
                            try
                            {
                                if (oAtr.TipoDato != null && (oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA || oAtr.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                {
                                    sValor = "";
                                    dynamic auxDina;
                                    if (listasItems.TryGetValue(oAtr.AtributoID.ToString(), out auxDina))
                                    {
                                        listaItems = (JsonObject)auxDina;
                                        string Auxstr;
                                        if (oAtr.Valor.ToString() != "")
                                        {
                                            foreach (var val in oAtr.Valor.ToString().Split(','))
                                            {
                                                if (listaItems.TryGetValue(val, out auxDina))
                                                {
                                                    foreach (var aux in auxDina)
                                                    {
                                                        if (aux.Key == "Text")
                                                        {
                                                            sValor += ", " + aux.Value;
                                                        }
                                                    }
                                                }
                                            }
                                            sValor = sValor.Remove(0, 2);
                                        }
                                        else
                                        {
                                            sValor = oAtr.Valor.ToString();
                                        }
                                    }
                                    else
                                    {
                                        sValor = oAtr.Valor.ToString();
                                    }
                                    jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), sValor);
                                }
                                else if (oAtr.TipoDato != null && oAtr.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                                {
                                    if (oAtr.NombreAtributo != null)
                                    {
                                        if (oAtr.Valor.ToString() != "")
                                        {
                                            try
                                            {
                                                jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), DateTime.Parse(oAtr.Valor.ToString(), CultureInfo.InvariantCulture));
                                            }
                                            catch (Exception ex)
                                            {
                                                jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.Valor.ToString());
                                            }
                                        }
                                        else
                                        {
                                            jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), "");
                                        }
                                    }
                                }
                                else
                                {
                                    if (oAtr.NombreAtributo != null)
                                    {
                                        jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.Valor.ToString());
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                //jsonAux.Add(oAtr.AtributoID, valor.Valor);
                            }
                        }
                        listaJson.Add(jsonAux);
                    }

                    if (vLista != null)
                    {
                        storePlantillas.DataSource = listaJson;
                        storePlantillas.DataBind();
                    }
                    else
                    {
                        storePlantillas.DataSource = new List<Data.CoreInventarioPlantillasAtributosCategorias>();
                        storePlantillas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    storePlantillas.DataSource = new List<Data.CoreInventarioPlantillasAtributosCategorias>();
                    storePlantillas.DataBind();
                }
            }
        }

        #endregion

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes;

                    listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }
                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            List<Object> listaAtributos = new List<object>();
            Data.CoreInventarioPlantillasAtributosCategorias oDato;
            Clases.ResponseCreateController cResponse;

            try
            {
                long lCatConfID = long.Parse(GridRowSelect.SelectedRow.RecordID);
                foreach (var item in listaCategorias)
                {
                    if (!item.GuardarValor(listaAtributos, cPlantillas.Context))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }

                if (bAgregar)
                {
                    cResponse = cPlantillas.CreateUpdatePlantillaCategorias(txtNombrePlantilla.Text,
                        txtNombrePlantilla.Text,
                        ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                        listaAtributos,
                        lCatConfID,
                        null
                        );
                }
                else
                {
                    oDato = cPlantillas.GetItem(long.Parse(GridRowSelectPlantillas.SelectedRow.RecordID));
                    cResponse = cPlantillas.CreateUpdatePlantillaCategorias(txtNombrePlantilla.Text,
                        txtNombrePlantilla.Text,
                        ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                        listaAtributos,
                        lCatConfID,
                        oDato.CoreInventarioPlantillaAtributoCategoriaID
                        );
                }

                cPlantillas.Context.SubmitChanges();

                if (cResponse.infoResponse.Result)
                {
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            CoreInventarioPlantillasAtributosCategoriasAtributosController cPlantillaAtributos = new CoreInventarioPlantillasAtributosCategoriasAtributosController();
            InventarioPlantillasAtributosJsonController cPlantillaJson = new InventarioPlantillasAtributosJsonController();
            cPlantillaAtributos.SetDataContext(cPlantillas.Context);
            List<Data.InventarioPlantillasAtributosJson> listaAtributos;
            Data.CoreInventarioPlantillasAtributosCategorias oDato;

            List<object> listaValoresAtributos = new List<object>();
            JsonObject jsDatos = new JsonObject();

            try
            {
                long lPlantillaID = long.Parse(GridRowSelectPlantillas.SelectedRow.RecordID);
                oDato = cPlantillas.GetItem(lPlantillaID);

                listaAtributos = cPlantillaJson.Deserializacion(oDato.JsonAtributosDinamicos);

                if (oDato != null)
                {
                    txtNombrePlantilla.SetValue(oDato.Nombre);

                    foreach (var item in listaAtributos)
                    {
                        listaValoresAtributos.Add(new
                        {
                            AtributoID = item.AtributoID,
                            Valor = item.Valor
                        });
                    }
                    if (listaAtributos.Count > 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            item.MostrarEditar(listaValoresAtributos, jsDatos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = jsDatos;

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            CoreInventarioPlantillasAtributosCategoriasAtributosController cPlantillaAtributos = new CoreInventarioPlantillasAtributosCategoriasAtributosController();
            cPlantillaAtributos.SetDataContext(cPlantillas.Context);
            List<Data.CoreInventarioPlantillasAtributosCategoriasAtributos> listaAtributos;

            long lPlantillaID = long.Parse(GridRowSelectPlantillas.SelectedRow.RecordID);
            long lCatConf = long.Parse(GridRowSelect.SelectedRow.RecordID);

            if (cPlantillas.PlantillaAplicada(lPlantillaID, lCatConf))
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                return direct;
            }
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    listaAtributos = cPlantillaAtributos.GetAtributosValoresFromPlantilla(lPlantillaID);
                    foreach (var item in listaAtributos)
                    {
                        if (!cPlantillaAtributos.DeleteItem(item.CoreInventarioPlantillaAtributoCategoriaAtributoID))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                    if (cPlantillas.DeleteItem(lPlantillaID))
                    {
                        trans.Complete();
                        direct.Result = GetGlobalResource(Comun.LogEliminacionRealizada);
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    if (ex is SqlException Sql)
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                        log.Error(Sql.Message);
                        return direct;
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        log.Error(ex.Message);
                        return direct;
                    }
                }
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }


        [DirectMethod()]
        public DirectResponse PintarCategorias(bool Update)
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            Componentes.GestionCategoriasAtributos oComponente;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (Update && contenedorCategorias != null && contenedorCategorias.ContentControls.Count > 0)
                {
                    listaCategorias = new List<Componentes.GestionCategoriasAtributos>();
                    contenedorCategorias.ContentControls.Clear();
                }
                if (hdCatSelect.Value != null && hdCatSelect.Value.ToString() != "" && long.Parse(hdCatSelect.Value.ToString()) != 0)
                {
                    if (listaCategorias == null || listaCategorias.Count == 0)
                    {
                        listaCategorias = new List<Componentes.GestionCategoriasAtributos>();
                        long lCatID = long.Parse(hdCatSelect.Value.ToString());
                        Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCatConf = cCategoriasConf.GetPlantilla(lCatID);
                        oComponente = (Componentes.GestionCategoriasAtributos)this.LoadControl("../../Componentes/GestionCategoriasAtributos.ascx");
                        oComponente.ID = "CAT" + oCatConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                        oComponente.CategoriaAtributoID = oCatConf.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                        oComponente.Nombre = oCatConf.InventarioAtributosCategorias.InventarioAtributoCategoria;
                        oComponente.Orden = 0;
                        oComponente.Modulo = (long)Comun.Modulos.INVENTARIO;
                        oComponente.EsPlantilla = false;
                        oComponente.MostrarPlantillas = false;
                        listaCategorias.Add(oComponente);
                    }
                    listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                    foreach (var item in listaCategorias)
                    {
                        contenedorCategorias.ContentControls.Add(item);
                    }
                }
                if (Update)
                {
                    //contenedorCategorias.Render();
                    if (listaCategorias != null && listaCategorias.Count != 0)
                    {
                        foreach (var item in listaCategorias)
                        {
                            item.PintarAtributos(true);
                        }
                    }
                    contenedorCategorias.UpdateContent();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse GenerarGridCat()
        {
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCategoriasConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();
            TiposDatosController cTipoDatos = new TiposDatosController();
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                long lCatID = long.Parse(hdCatConfSelect.Value.ToString());
                List<Data.Vw_CoreInventarioAtributos> oCatConf = cCategoriasConf.GetListaVwAtributos(lCatID);
                foreach (var atr in oCatConf)
                {
                    ModelField modelField = new ModelField
                    {
                        Name = "Atr" + atr.CoreAtributoConfiguracionID
                    };
                    Data.TiposDatos oTipoDato;
                    List<Data.Vw_CoreAtributosConfiguracionRolesRestringidos> listaRestriccionRoles = cRestriccionRoles.GetVwRolesFromAtributoNoDefecto(atr.CoreAtributoConfiguracionID);
                    oTipoDato = cTipoDatos.GetItem((long)atr.TipoDatoID);
                    switch (oTipoDato.Codigo)
                    {
                        case "TEXTO":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "NUMERICO":
                            modelField.Type = ModelFieldType.Int;
                            break;
                        case "FECHA":
                            modelField.Type = ModelFieldType.Date;
                            break;
                        case "LISTA":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "LISTAMULTIPLE":
                            modelField.Type = ModelFieldType.String;
                            break;
                        case "BOOLEANO":
                            modelField.Type = ModelFieldType.Boolean;
                            break;
                        //case "ENTERO":

                        //    break;
                        //case "FLOTANTE":

                        //    break;
                        //case "MONEADA":

                        //    break;
                        //case "GEOPOSICION":

                        //    break;
                        case "TEXTAREA":
                            modelField.Type = ModelFieldType.String;
                            break;
                        default:
                            modelField.Type = ModelFieldType.String;
                            break;
                    }
                    storePlantillas.ModelInstance.Fields.Add(modelField);
                    if (oTipoDato.Codigo == "FECHA")
                    {
                        DateColumn colDate = new DateColumn
                        {
                            ID = "col" + atr.CoreAtributoConfiguracionID,
                            Text = atr.CodigoCoreAtributoConfg,
                            Format = "dd/m/Y",
                            DataIndex = "Atr" + atr.CoreAtributoConfiguracionID,
                            Flex = 1,
                        };
                        //grid.InsertColumn(grid.ColumnModel.Columns.Count, colDate);
                        if (listaRestriccionRoles != null && listaRestriccionRoles.Count > 0)
                        {
                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            List<long> listaRolesIDs = new List<long>();
                            foreach (var item in listaRoles) { listaRolesIDs.Add(item.RolID); }
                            foreach (var oRestriccionRol in listaRestriccionRoles)
                            {
                                if (listaRolesIDs.Contains((long)oRestriccionRol.RolID) && oRestriccionRol.Restriccion == (long)Comun.RestriccionesAtributoDefecto.HIDDEN)
                                {
                                    colDate.Hidden = true;
                                    colDate.Fixed = true;
                                }
                            }
                        }
                        else
                        {
                            var oRestriccionRol = cRestriccionRoles.GetDefault(atr.CoreAtributoConfiguracionID);
                            if (oRestriccionRol != null && oRestriccionRol.Restriccion == (long)Comun.RestriccionesAtributoDefecto.HIDDEN)
                            {
                                colDate.Hidden = true;
                                colDate.Fixed = true;
                            }
                        }
                        if (!colDate.Fixed)
                        {
                            grid.InsertColumn(grid.ColumnModel.Columns.Count, colDate);
                        }
                    }
                    else
                    {
                        Column col = new Column
                        {
                            ID = "col" + atr.CoreAtributoConfiguracionID,
                            Text = atr.CodigoCoreAtributoConfg,
                            DataIndex = "Atr" + atr.CoreAtributoConfiguracionID,
                            Flex = 1,
                        };
                        //grid.InsertColumn(grid.ColumnModel.Columns.Count, col);
                        if (listaRestriccionRoles != null && listaRestriccionRoles.Count > 0)
                        {
                            RolesController cRoles = new RolesController();
                            List<Data.Roles> listaRoles = cRoles.GetRolesFromUsuario(((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            List<long> listaRolesIDs = new List<long>();
                            foreach (var item in listaRoles) { listaRolesIDs.Add(item.RolID); }
                            foreach (var oRestriccionRol in listaRestriccionRoles)
                            {
                                if (listaRolesIDs.Contains((long)oRestriccionRol.RolID) && oRestriccionRol.Restriccion == (long)Comun.RestriccionesAtributoDefecto.HIDDEN)
                                {
                                    col.Hidden = true;
                                    col.Fixed = true;
                                }
                            }
                        }
                        else
                        {
                            var oRestriccionRol = cRestriccionRoles.GetDefault(atr.CoreAtributoConfiguracionID);
                            if (oRestriccionRol != null && oRestriccionRol.Restriccion == (long)Comun.RestriccionesAtributoDefecto.HIDDEN)
                            {
                                col.Hidden = true;
                                col.Fixed = true;
                            }
                        }
                        if (!col.Fixed)
                        {
                            grid.InsertColumn(grid.ColumnModel.Columns.Count, col);
                        }
                    }
                }

                #region FILTROS

                List<string> listaIgnore = new List<string>();

                Comun.CreateGridFilters(gridFilters, grid.GetStore(), grid.ColumnModel, listaIgnore, _Locale);

                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                grid.UpdateLayout();
                grid.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        #endregion

    }
}