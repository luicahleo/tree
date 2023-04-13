using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using Tree.Linq.GenericExtensions;

namespace TreeCore.ModInventario
{
    public partial class InventarioGestionContenedor : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                if (Request["VistaPlantilla"] != null && Request["VistaPlantilla"] != "")
                {
                    hdVistaPlantilla.SetValue(Request["VistaPlantilla"]);
                }
                if (Request["EmplazamientoID"] != null && Request["EmplazamientoID"] != "")
                {
                    hdEmplazamientoID.SetValue(Request["EmplazamientoID"]);
                }
                else
                {
                    hdEmplazamientoID.SetValue(0);
                    HyperlinkButton4.Hide();
                }

                if (Request.QueryString[Comun.PARAM_IDS_RESULTADOS] != null && Request.QueryString[Comun.PARAM_NAME_INDICE_ID] != null)
                {
                    //Asignación de parametros de resultados de KPI para las paginas contenidas
                    string idsResult = Request[Comun.PARAM_IDS_RESULTADOS];
                    string nameIndice = Request[Comun.PARAM_NAME_INDICE_ID];

                    InventarioGestion_Content.Loader.Params.Add(new { idsResultados = idsResult });
                    InventarioGestion_Content.Loader.Params.Add(new { nameIndiceID = nameIndice });
                    hugeCt.Loader.Params.Add(new { idsResultados = idsResult });
                    hugeCt.Loader.Params.Add(new { nameIndiceID = nameIndice });
                }

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
                hdCategoriaActiva.Value = 0;
                hdFiltroID.Value = 0;
                //hdEmplazamientoID.SetValue(26596);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };
        }

        #region STORES

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

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EntidadesController cOperadores = new EntidadesController();
                    var listaOperadores = cOperadores.getEntidadesOperadores(long.Parse(hdCliID.Value.ToString()));
                    if (listaOperadores != null)
                    {
                        storeOperadores.DataSource = listaOperadores;
                        storeOperadores.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region ESTADOS
        protected void storeEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();
                    var listaEstados = cEstados.GetActivos(long.Parse(hdCliID.Value.ToString()));
                    if (listaEstados != null)
                    {
                        storeEstados.DataSource = listaEstados;
                        storeEstados.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region CATEGORIAS
        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    List<Data.InventarioCategorias> listaCategorias = null;
                    if (long.Parse(hdEmplazamientoID.Value.ToString()) != 0)
                    {
                        Data.Emplazamientos oEmplazamientos = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                        listaCategorias = cCategorias.GetInventarioCategoriasByTipoEmplazamiento2(oEmplazamientos.EmplazamientoTipoID, oEmplazamientos.ClienteID);
                    }
                    if (listaCategorias != null)
                    {
                        storeCategorias.DataSource = listaCategorias;
                        storeCategorias.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region USUARIOS
        protected void storeUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    UsuariosController cUsuarios = new UsuariosController();
                    var listaUsuarios = cUsuarios.getUsuariosConInventario(long.Parse(hdCliID.Value.ToString()));
                    if (listaUsuarios != null)
                    {
                        storeUsuarios.DataSource = listaUsuarios;
                        storeUsuarios.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region CAMPOS
        protected void storeCampos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var listaCampos = TreeCore.Data.CamposFiltroInventario.GetCamposFiltrosInventario(long.Parse(hdCliID.Value.ToString()), long.Parse(hdCategoriaActiva.Value.ToString()), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                    if (listaCampos != null)
                    {
                        foreach (var item in listaCampos)
                        {
                            if (item.Name.StartsWith("str"))
                            {
                                item.Name = GetGlobalResource(item.Name);
                            }
                        }
                        storeCampos.DataSource = listaCampos;
                        storeCampos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region VIEWS
        protected void storeViews_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            ColumnasJsonController cColumnas = new ColumnasJsonController();
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    CoreGestionVistasController cVistas = new CoreGestionVistasController();
                    var listaDatos = cVistas.GetVistas("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaActiva.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                    if (listaDatos != null && listaDatos.Count > 0)
                    {
                        foreach (var item in listaDatos)
                        {
                            item.JsonColumnas = cColumnas.Serializacion((from c in cColumnas.Deserializacion(item.JsonColumnas) where (from col in cColumnas.Deserializacion(hdColumnas.Value.ToString()) select col.Columna).ToList().Contains(c.Columna) select c).ToList());                            
                            item.JsonColumnas = cColumnas.Serializacion(cColumnas.Deserializacion(item.JsonColumnas).Concat(cColumnas.Deserializacion(hdColumnas.Value.ToString())).Distinct(new ColumnasJsonControllerComparer()).ToList());
                        }
                        btnOpciones.Show();
                        storeViews.DataSource = listaDatos;
                        storeViews.DataBind();
                    }
                    else
                    {
                        btnOpciones.Hide();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region FILTROS
        protected void storeMyFilters_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosController cController = new EmplazamientosController();
                    DataTable tablaValores = cController.EjecutarQuery("select GestionFiltroID, UsuarioID, NombreFiltro, JsonItemsFiltro, Pagina from GestionFiltros where Pagina = 'InventarioGestionContenedor.aspx' and UsuarioID = " + ((Data.Usuarios)Session["USUARIO"]).UsuarioID + " and Json_Value(JsonItemsFiltro, '$.\"InventarioCategoriaID\"') = " + hdCategoriaActiva.Value.ToString());
                    JsonObject oDato;
                    List<JsonObject> listaDatos = new List<JsonObject>();
                    if (tablaValores != null && tablaValores.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in tablaValores.Rows)
                        {
                            oDato = new JsonObject();
                            oDato.Add("GestionFiltroID", oRow[0].ToString());
                            oDato.Add("UsuarioID", oRow[1].ToString());
                            oDato.Add("NombreFiltro", oRow[2].ToString());
                            oDato.Add("JsonItemsFiltro", oRow[3].ToString());
                            oDato.Add("Pagina", oRow[4].ToString());
                            listaDatos.Add(oDato);
                        }
                        storeMyFilters.DataSource = listaDatos;
                        storeMyFilters.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region Dinamicos
        protected void storeTiposDinamicos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosController cController = new EmplazamientosController();
                    DataTable tablaValores = cController.EjecutarQuery(hdQuery.Value.ToString());
                    JsonObject oDato;
                    List<JsonObject> listaDatos = new List<JsonObject>();
                    if (tablaValores.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in tablaValores.Rows)
                        {
                            oDato = new JsonObject();
                            oDato.Add("Name", oRow[1].ToString());
                            oDato.Add("ID", oRow[0].ToString());
                            listaDatos.Add(oDato);
                        }
                        storeTiposDinamicos.DataSource = listaDatos;
                        storeTiposDinamicos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public DirectResponse ExportarModeloDatos(long[] listaCatIDs)
        {
            DirectResponse direct = new DirectResponse();
            InventarioElementosController cController = new InventarioElementosController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            List<Data.InventarioCategorias> listaCategorias = new List<Data.InventarioCategorias>();
            Data.Emplazamientos oEmplazamiento;

            try
            {
                direct.Success = true;
                direct.Result = "";
                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = "";
                if (long.Parse(hdEmplazamientoID.Value.ToString()) != 0)
                {
                    Data.Emplazamientos oEmplazamientos = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));

                    fileName = GetGlobalResource("strElementosInventario") + "_" + oEmplazamientos.NombreSitio.Replace("/", "") + "_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                }
                else
                {
                    fileName = GetGlobalResource("strElementosInventario") + "_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                }
                string saveAs = directorio + fileName;
                oEmplazamiento = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));

                string sOperadores = "";
                string sEstados = "";
                string sUsuarios = "";
                string sdatMinDateCrea = "";
                string sdatMaxDateCrea = "";
                string sdatMinDateMod = "";
                string sdatMaxDateMod = "";

                foreach (var ope in cmbOperadores.SelectedItems)
                {
                    sOperadores += ((sOperadores == "") ? "" : ",") + ope.Value;
                }

                foreach (var est in cmbEstados.SelectedItems)
                {
                    sEstados += ((sEstados == "") ? "" : ",") + est.Value;
                }

                foreach (var usu in cmbUsuarios.SelectedItems)
                {
                    sUsuarios += ((sUsuarios == "") ? "" : ",") + usu.Value;
                }

                if (datMinDateCrea.RawValue != null)
                {
                    sdatMinDateCrea = DateTime.Parse(datMinDateCrea.Value.ToString()).ToString(Comun.FORMATO_FECHA);
                }
                if (datMaxDateCrea.RawValue != null)
                {
                    sdatMaxDateCrea = DateTime.Parse(datMaxDateCrea.Value.ToString()).ToString(Comun.FORMATO_FECHA);
                }
                if (datMinDateMod.RawValue != null)
                {
                    sdatMinDateMod = DateTime.Parse(datMinDateMod.Value.ToString()).ToString(Comun.FORMATO_FECHA);
                }
                if (datMaxDateMod.RawValue != null)
                {
                    sdatMaxDateMod = DateTime.Parse(datMaxDateMod.Value.ToString()).ToString(Comun.FORMATO_FECHA);
                }

                JsonObject jsonTraducciones = new JsonObject();
                jsonTraducciones.Add("Codigo", GetGlobalResource("strCodigoElemento"));
                jsonTraducciones.Add("Nombre", GetGlobalResource("strNombreElemento"));
                jsonTraducciones.Add("Creador", GetGlobalResource("strCreador"));
                jsonTraducciones.Add("FechaCreacion", GetGlobalResource("strFechaCreacion"));
                jsonTraducciones.Add("Modificador", GetGlobalResource("strModificado"));
                jsonTraducciones.Add("FechaUltimaModificacion", GetGlobalResource("strFechaModificacion"));
                jsonTraducciones.Add("Operador", GetGlobalResource("strOperador"));
                jsonTraducciones.Add("Estado", GetGlobalResource("strEstado"));
                jsonTraducciones.Add("Plantilla", GetGlobalResource("strPlantilla"));
                if (oEmplazamiento != null)
                {
                    cController.ExportarInventarioEmplazamiento(oEmplazamiento.EmplazamientoID, saveAs, Usuario.UsuarioID, listaCatIDs.ToList(), jsonTraducciones,
                        sOperadores,
                        sEstados,
                        sUsuarios,
                        sdatMinDateCrea,
                        sdatMaxDateCrea,
                        sdatMinDateMod,
                        sdatMaxDateMod,
                        hdFiltros.Value.ToString());
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, oEmplazamiento.ClienteID, Convert.ToInt32(Comun.Modulos.INVENTARIO), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                    Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }


            return direct;
        }

        [DirectMethod]
        public DirectResponse CargarDatosElementos(long ElementoID, string sTab)
        {
            DirectResponse direct = new DirectResponse();
            InventarioElementosController cElementos = new InventarioElementosController();
            Data.Vw_InventarioElementosReducida oElemento;
            Data.InventarioElementos oElementoObj;
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones oCatConf;
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreInventarioElementosAtributosController cAtributos = new CoreInventarioElementosAtributosController();
            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            EmplazamientosController cEmp = new EmplazamientosController();
            EmplazamientosAtributosConfiguracionController cConfig = new EmplazamientosAtributosConfiguracionController();
            List<long> listaPlaIds;
            string sValor = "";
            JsonObject oDato = new JsonObject();
            JsonObject jsonAux;

            try
            {
                if (sTab == "Inventario")
                {
                    oElemento = cElementos.GetElementoVistaReducida(ElementoID);
                    oElementoObj = cElementos.GetItem(ElementoID);
                    oDato.Add(GetGlobalResource("strCodigo"), oElemento.NumeroInventario);
                    oDato.Add(GetGlobalResource("strNombre"), oElemento.Nombre);
                    if (!oElemento.Plantilla)
                    {
                        oDato.Add(GetGlobalResource("strEmplazamiento"), oElemento.Codigo);
                        oDato.Add(GetGlobalResource("strOperador"), oElemento.NombreEntidad);
                        int ContCodigo = 1;
                        oDato.Add(GetGlobalResource("strEstado"), oElemento.EstadoInventarioElemento);
                    }

                    CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();

                    #region Cargas Listas

                    CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();

                    JsonObject listasItems = new JsonObject();
                    JsonObject listaItems = new JsonObject();
                    JsonObject auxJson;

                    var listaAtrVis = cCategoriasVin.GetAtributosVisiblesByInventarioCategoriaID(oElementoObj.InventarioCategoriaID, Usuario.UsuarioID);

                    foreach (var oAtr in cCategoriasVin.GetAtributosByInventarioCategoriaID(oElementoObj.InventarioCategoriaID))
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

                    foreach (var oAtr in cAtrJson.Deserializacion(oElementoObj.JsonAtributosDinamicos))
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
                            }
                            else if (oAtr.TipoDato != null && oAtr.TipoDato == Comun.TIPODATO_CODIGO_FECHA)
                            {
                                if (oAtr.Valor != "")
                                {
                                    sValor = DateTime.Parse(oAtr.Valor).ToString(Comun.FORMATO_FECHA);
                                }
                                else
                                {
                                    sValor = oAtr.Valor;
                                }
                            }
                            else
                            {
                                sValor = oAtr.Valor;
                            }

                            if (listaAtrVis.Select(c => c.CoreAtributoConfiguracionID).Contains(oAtr.AtributoID))
                            {
                                oDato.Add(listaAtrVis.Where(c => c.CoreAtributoConfiguracionID == oAtr.AtributoID).First().Codigo, sValor);
                            }
                        }
                        catch (Exception)
                        {
                            //jsonAux.Add(oAtr.AtributoID, valor.Valor);
                        }
                    }
                    listaPlaIds = new List<long>();
                    foreach (var oPla in cPlaJson.Deserializacion(oElementoObj.JsonPlantillas))
                    {
                        if (oPla.NombreCategoria != "" && oPla.NombreCategoria != null)
                        {
                            oDato.Add(oPla.NombreCategoria, oPla.NombrePlantilla);
                        }
                        else
                        {
                            oCatConf = cCatConf.GetItem(oPla.InvCatConfID);
                            oDato.Add(oCatConf.InventarioAtributosCategorias.InventarioAtributoCategoria, oPla.NombrePlantilla);
                        }
                        listaPlaIds.Add(oPla.PlantillaID);
                    }
                    direct.Success = true;
                    direct.Result = oDato;
                }
                else if (sTab == "Sites")
                {
                    Data.Emplazamientos oEmp = cEmp.GetItem(ElementoID);

                    #region General

                    jsonAux = new JsonObject();

                    jsonAux.Add(GetGlobalResource("strNombre"), oEmp.NombreSitio);
                    jsonAux.Add(GetGlobalResource("strCodigo"), oEmp.Codigo);
                    jsonAux.Add(GetGlobalResource("strOperador"), oEmp.Operadores.Operador);
                    jsonAux.Add(GetGlobalResource("strEstadoGlobal"), oEmp.EstadosGlobales.EstadoGlobal);
                    jsonAux.Add(GetGlobalResource("strMoneda"), oEmp.Monedas.Moneda);
                    jsonAux.Add(GetGlobalResource("strCategoria"), oEmp.EmplazamientosCategoriasSitios.CategoriaSitio);
                    jsonAux.Add(GetGlobalResource("strTipo"), oEmp.EmplazamientosTipos.Tipo);
                    jsonAux.Add(GetGlobalResource("strTipoEdificio"), oEmp.EmplazamientosTiposEdificios.TipoEdificio);
                    jsonAux.Add(GetGlobalResource("strFechaActivacion"), oEmp.FechaActivacion.Value.ToString(Comun.FORMATO_FECHA));

                    if (oEmp.FechaDesactivacion != null)
                    {
                        jsonAux.Add(GetGlobalResource("strFechaDesactivacion"), oEmp.FechaDesactivacion.Value.ToString(Comun.FORMATO_FECHA));
                    }
                    else
                    {
                        jsonAux.Add(GetGlobalResource("strFechaDesactivacion"), "");
                    }

                    if (oEmp.EmplazamientoTipoEstructuraID != null)
                    {
                        jsonAux.Add(GetGlobalResource("strTipoEstructura"), oEmp.EmplazamientosTiposEstructuras.TipoEstructura);
                    }
                    else
                    {
                        jsonAux.Add(GetGlobalResource("strTipoEstructura"), "");
                    }
                    if (oEmp.EmplazamientoTamanoID != null)
                    {
                        jsonAux.Add(GetGlobalResource("strTamano"), oEmp.EmplazamientosTamanos.Tamano);
                    }
                    else
                    {
                        jsonAux.Add(GetGlobalResource("strTamano"), "");
                    }

                    oDato.Add("General", jsonAux);

                    #endregion

                    #region Location

                    jsonAux = new JsonObject();

                    jsonAux.Add(GetGlobalResource("strDireccion"), oEmp.Direccion);
                    jsonAux.Add(GetGlobalResource("strMunicipiosProvincias"), oEmp.Municipio);
                    jsonAux.Add(GetGlobalResource("strBarrio"), oEmp.Barrio);
                    jsonAux.Add(GetGlobalResource("strCodigoPostal"), oEmp.CodigoPostal);
                    jsonAux.Add(GetGlobalResource("strLatitud"), oEmp.Latitud);
                    jsonAux.Add(GetGlobalResource("strLongitud"), oEmp.Longitud);

                    oDato.Add("Localizacion", jsonAux);

                    #endregion

                    #region Adicional

                    jsonAux = new JsonObject();
                    var listaAtrVisibles = cConfig.GetAtributosVisibles(long.Parse(hdCliID.Value.ToString()), Usuario.UsuarioID);

                    if (oEmp.JsonAtributosDinamicos != null && (oEmp.JsonAtributosDinamicos != ""))
                    {
                        foreach (var valor in cAtrJson.Deserializacion(oEmp.JsonAtributosDinamicos))
                        {
                            if (listaAtrVisibles.Select(c => c.EmplazamientoAtributoConfiguracionID).ToList().Contains(valor.AtributoID))
                            {
                                try
                                {
                                    var atr = cConfig.GetItem(long.Parse(valor.AtributoID.ToString()));

                                    if (valor.TipoDato != null && (valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA || valor.TipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE))
                                    {
                                        string sValorAtributos = "";
                                        JsonObject listaItems = new JsonObject();
                                        JsonObject auxJson;
                                        dynamic auxDina;
                                        if (atr.TablaModeloDatoID != null)
                                        {
                                            listaItems = cConfig.GetJsonItemsComboBoxByColumnaModeloDatosID(atr.EmplazamientoAtributoConfiguracionID);
                                        }
                                        else if (atr.FuncionControlador != null && atr.FuncionControlador != "")
                                        {
                                            listaItems = cConfig.GetJsonItemsComboBoxByFuncion(atr.FuncionControlador, null, null, atr.EmplazamientoAtributoConfiguracionID);
                                        }
                                        else if (atr.ValoresPosibles != null && atr.ValoresPosibles != "")
                                        {
                                            listaItems = new JsonObject();
                                            foreach (var val in atr.ValoresPosibles.Split(';'))
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
                                        if (listaItems != null)
                                        {
                                            if (valor.Valor.ToString() != "")
                                            {
                                                foreach (var val in valor.Valor.ToString().Split(','))
                                                {
                                                    if (listaItems.TryGetValue(val, out auxDina))
                                                    {
                                                        foreach (var aux in auxDina)
                                                        {
                                                            if (aux.Key == "Text")
                                                            {
                                                                sValorAtributos += ", " + aux.Value;
                                                            }
                                                        }
                                                    }
                                                }
                                                sValorAtributos = sValorAtributos.Remove(0, 2);
                                            }
                                            else
                                            {
                                                sValorAtributos = valor.Valor.ToString();
                                            }
                                        }
                                        jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, sValorAtributos);
                                    }
                                    else
                                    {
                                        if (valor.NombreAtributo != null)
                                        {
                                            if (atr != null)
                                            {
                                                jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                                            }
                                            else
                                            {
                                                jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    jsonAux.Add(listaAtrVisibles.Where(c => c.EmplazamientoAtributoConfiguracionID == valor.AtributoID).First().NombreAtributo, valor.Valor.ToString());
                                }
                            }
                        }
                    }

                    oDato.Add("Adicional", jsonAux);

                    #endregion

                    direct.Result = oDato;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }


            return direct;

        }

        [DirectMethod]
        public DirectResponse CargarVinculacionesElementos(long ElementoID)
        {
            DirectResponse direct = new DirectResponse();
            InventarioElementosController cElementos = new InventarioElementosController();
            Data.Vw_InventarioElementosReducida oElemento;
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            CoreInventarioElementosAtributosController cAtributos = new CoreInventarioElementosAtributosController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
            List<Data.InventarioElementos> listaElementosVinculados;
            string sValor = "";
            JsonObject oDatoFinal = new JsonObject();
            JsonObject oDato;
            JsonObject oDatoAux;
            string vin;
            try
            {
                oDato = new JsonObject();
                listaElementosVinculados = cVinculaciones.GetElementosPadres(ElementoID);
                foreach (var item in listaElementosVinculados)
                {
                    oDatoAux = new JsonObject();
                    oDatoAux.Add("Categoria", GetGlobalResource("strCategoria") + ": " + item.InventarioCategorias.InventarioCategoria);
                    oDatoAux.Add("Codigo", GetGlobalResource("strCodigo") + ": " + item.NumeroInventario);
                    oDatoAux.Add("Nombre", GetGlobalResource("strNombre") + ": " + item.Nombre);
                    oDatoAux.Add("Estado", GetGlobalResource("strEstado") + ": " + item.InventarioElementosAtributosEstados.Nombre);
                    oDatoAux.Add("Operador", GetGlobalResource("strOperador") + ": " + item.Operadores.Operador);
                    vin = "";
                    foreach (var oVin in cVinculaciones.GetTiposVinculacionFromElementos(ElementoID, item.InventarioElementoID))
                    {
                        if (vin == "")
                        {
                            vin += oVin.Nombre;
                        }
                        else
                        {
                            vin += ", " + oVin.Nombre;
                        }
                    }
                    oDatoAux.Add("Vinculaciones", GetGlobalResource("strVinculacion") + ": " + vin);
                    oDato.Add(item.InventarioElementoID.ToString(), oDatoAux);
                }
                oDatoFinal.Add("Padres", oDato);

                oDato = new JsonObject();
                listaElementosVinculados = cVinculaciones.GetElementosHijos(ElementoID);
                foreach (var item in listaElementosVinculados)
                {
                    oDatoAux = new JsonObject();
                    oDatoAux.Add("Categoria", GetGlobalResource("strCategoria") + ": " + item.InventarioCategorias.InventarioCategoria);
                    oDatoAux.Add("Codigo", GetGlobalResource("strCodigo") + ": " + item.NumeroInventario);
                    oDatoAux.Add("Nombre", GetGlobalResource("strNombre") + ": " + item.Nombre);
                    oDatoAux.Add("Estado", GetGlobalResource("strEstado") + ": " + item.InventarioElementosAtributosEstados.Nombre);
                    oDatoAux.Add("Operador", GetGlobalResource("strOperador") + ": " + item.Operadores.Operador);
                    vin = "";
                    foreach (var oVin in cVinculaciones.GetTiposVinculacionFromElementos(item.InventarioElementoID, ElementoID))
                    {
                        if (vin == "")
                        {
                            vin += oVin.Nombre;
                        }
                        else
                        {
                            vin += ", " + oVin.Nombre;
                        }
                    }
                    oDatoAux.Add("Vinculaciones", GetGlobalResource("strVinculacion") + ": " + vin);
                    oDato.Add(item.InventarioElementoID.ToString(), oDatoAux);
                }
                oDatoFinal.Add("Hijas", oDato);
                direct.Success = true;
                direct.Result = oDatoFinal;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }


            return direct;

        }

        [DirectMethod]
        public DirectResponse GuardarFiltro(string filtro, string NombreFiltro)
        {
            TreeCore.Data.GestionFiltros oFiltro;
            GestionFiltrosController cFiltros = new GestionFiltrosController();
            FiltroInventarioElementosController cJsonFiltro = new FiltroInventarioElementosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                if (long.Parse(hdFiltroID.Value.ToString()) == 0)
                {
                    if (cFiltros.DuplicidadFiltroInventario(NombreFiltro, "InventarioGestionContenedor.aspx", ((Data.Usuarios)Session["USUARIO"]).UsuarioID, long.Parse(hdCategoriaActiva.Value.ToString())))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        return direct;
                    }
                    oFiltro = new TreeCore.Data.GestionFiltros
                    {
                        UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                        NombreFiltro = NombreFiltro,
                        JsonItemsFiltro = filtro,
                        Pagina = "InventarioGestionContenedor.aspx"
                    };
                    if (cFiltros.AddItem(oFiltro) != null)
                    {
                        log.Info(Comun.LogAgregacionRealizada);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    }
                }
                else
                {
                    oFiltro = cFiltros.GetItem(long.Parse(hdFiltroID.Value.ToString()));
                    if (oFiltro.NombreFiltro != NombreFiltro && cFiltros.DuplicidadFiltro(NombreFiltro, "InventarioGestionContenedor.aspx", ((Data.Usuarios)Session["USUARIO"]).UsuarioID))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsYaExiste);
                        return direct;
                    }
                    oFiltro.NombreFiltro = NombreFiltro;
                    oFiltro.JsonItemsFiltro = filtro;
                    if (cFiltros.UpdateItem(oFiltro))
                    {
                        log.Info(Comun.LogActivacionRealizada);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse EliminarFiltro()
        {
            GestionFiltrosController cFiltros = new GestionFiltrosController();
            DirectResponse direct = new DirectResponse();
            try
            {
                direct.Success = true;
                direct.Result = "";
                if (cFiltros.DeleteItem(long.Parse(hdFiltroID.Value.ToString())))
                {
                    log.Info(Comun.LogEliminacionRealizada);
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                }
            }
            catch (Exception ex)
            {
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
            return direct;
        }

        #endregion

        [DirectMethod]
        public DirectResponse GuardarView(string JsonColumnas, string JsonFiltros)
        {
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            Data.CoreGestionVistas oVista = null;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                oVista = cVistas.GetItem(long.Parse(cmbViews.Value.ToString()));
                if (oVista != null)
                {
                    oVista.JsonColumnas = JsonColumnas;
                    if (btnFiltosActivos.Pressed)
                    {
                        oVista.JsonFiltros = JsonFiltros;
                    }
                    else
                    {
                        oVista.JsonFiltros = "";
                    }
                    if (!cVistas.UpdateItem(oVista))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                }
                else
                {
                    oVista = new Data.CoreGestionVistas
                    {
                        Nombre = GetGlobalResource("jsDefecto"),
                        UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                        JsonColumnas = JsonColumnas,
                        JsonFiltros = "",
                        Pagina = "InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaActiva.Value.ToString(),
                        Defecto = false
                    };
                    if (btnFiltosActivos.Pressed)
                    {
                        oVista.JsonFiltros = JsonFiltros;
                    }
                    if ((oVista = cVistas.AddItem(oVista)) == null)
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse GuardarNuevaView(string sNombre, string JsonColumnas, string JsonFiltros)
        {
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            Data.CoreGestionVistas oVista;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                oVista = new Data.CoreGestionVistas
                {
                    Nombre = sNombre,
                    UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                    JsonColumnas = JsonColumnas,
                    JsonFiltros = "",
                    Pagina = "InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaActiva.Value.ToString(),
                    Defecto = false
                };
                if (btnFiltosActivos.Pressed)
                {
                    oVista.JsonFiltros = JsonFiltros;
                }
                if ((oVista = cVistas.AddItem(oVista)) == null)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }
                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                direct.Result = oVista.CoreGestionVistaID;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse RenameView(string sNombre)
        {
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            Data.CoreGestionVistas oVista = null;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (cmbViews.SelectedItem != null)
                {
                    oVista = cVistas.GetItem(long.Parse(cmbViews.Value.ToString()));
                }

                if (oVista != null)
                {
                    oVista.Nombre = sNombre;
                    if (!cVistas.UpdateItem(oVista))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }
                log.Info(GetGlobalResource(Comun.LogActualizacionNombre));
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        [DirectMethod]
        public DirectResponse SetDefaultView()
        {
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            Data.CoreGestionVistas oVista = null;
            Data.CoreGestionVistas oVistaDef = null;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (cmbViews.SelectedItem != null)
                {
                    oVista = cVistas.GetItem(long.Parse(cmbViews.Value.ToString()));
                    oVistaDef = cVistas.GetDefault("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaActiva.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                }

                if (oVista != null)
                {
                    oVista.Defecto = true;
                    oVistaDef.Defecto = false;
                    if (!cVistas.UpdateItem(oVista) || !cVistas.UpdateItem(oVistaDef))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }
                log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }


        [DirectMethod]
        public DirectResponse DeleteView()
        {
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            Data.CoreGestionVistas oVista = null;
            Data.CoreGestionVistas oVistaDef = null;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (cmbViews.SelectedItem != null)
                {
                    oVista = cVistas.GetItem(long.Parse(cmbViews.Value.ToString()));
                    oVistaDef = cVistas.GetDefault("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaActiva.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                }

                if (oVista != null)
                {
                    if (oVista.Defecto)
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                        return direct;
                    }
                    if (!cVistas.DeleteItem(oVista.CoreGestionVistaID))
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }
                }
                log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                direct.Result = oVistaDef.CoreGestionVistaID;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

    }
}