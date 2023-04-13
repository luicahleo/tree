using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Transactions;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using TreeCore.Componentes;
using System.Globalization;
using System.Web;
using Newtonsoft.Json;

namespace TreeCore.ModInventario.pages
{
    public partial class InventarioCategoryViewVistaCategoria : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        List<Componentes.GestionCategoriasAtributos> listaCategorias;
        List<ColumnBase> listaColumnasGrid;

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);


                List<string> listaIgnore = new List<string>()
                { };

                #region FILTROS

                List<string> listaIgnoreEmp = new List<string>()
                { "colSelEmplazamientos" };

                Comun.CreateGridFilters(gridEmplazamientosFilters, storeEmplazamientos, gridEmplazamientos.ColumnModel, listaIgnoreEmp, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);

                if (Request.QueryString["EmplazamientoID"] != null && Request.QueryString["EmplazamientoID"] != "")
                {
                    hdEmplazamientoID.SetValue(Request.QueryString["EmplazamientoID"]);
                }
            }

            hdNumeroMaximoCol.SetValue(4);

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    DescargaGrid();
                    Response.End();
                }
                else if (sOpcion == "EXPORTARNOCOLUMNMODEL")
                {
                    DescargaGridNoColumnModels();
                    Response.End();
                }

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
                ResourceManagerTreeCore.RegisterIcon(Icon.ChartCurve);
            }

            #endregion

            if (Request.QueryString["EmplazamientoID"] != null && Request.QueryString["EmplazamientoID"] != "")
            {
                hdEmplazamientoID.SetValue(Request.QueryString["EmplazamientoID"]);
                if (long.Parse(hdEmplazamientoID.Value.ToString()) == 0)
                {
                    //btnDescargarTodo.Hidden = true;
                }
            }
            else
            {
                hdEmplazamientoID.SetValue(0);
                //btnDescargarTodo.Hidden = true;
            }

            if (Request.QueryString["CategoriaID"] != null && Request.QueryString["CategoriaID"] != "")
            {
                hdCategoriaID.SetValue(Request.QueryString["CategoriaID"]);
                InventarioCategoriasController categoriasController = new InventarioCategoriasController();
                TreeCore.Data.InventarioCategorias oDato = categoriasController.GetItem(long.Parse(hdCategoriaID.Value.ToString()));
                hdCliID.SetValue(oDato.ClienteID);
                cmbCategoriaElemento.RawValue = oDato.InventarioCategoria;
            }
            else
            {
                hdCategoriaID.SetValue(0);
                hdCliID.SetValue(Usuario.ClienteID);
            }

            if (Request["VistaPlantilla"] != null && Request["VistaPlantilla"] != "")
            {
                hdVistaPlantilla.SetValue(Request["VistaPlantilla"]);
                cmbOperador.Hidden = true;
                cmbEstado.Hidden = true;
                cmbPlantilla.Hidden = true;
                //btnDescargarTodo.Hidden = true;
            }

            if (Request.QueryString[Comun.PARAM_IDS_RESULTADOS] != null && Request.QueryString[Comun.PARAM_NAME_INDICE_ID] != null)
            {
                string idsResult = Request[Comun.PARAM_IDS_RESULTADOS];
                string nameIndice = Request[Comun.PARAM_NAME_INDICE_ID];

                string copysIdsResultadosKPI = sResultadoKPIid;
                string copyIdsResultadosKPI = IdsResultadosKPI;
                List<long> copylistIdsResultadosKPI = listIdsResultadosKPI;
                string copynameIndiceID = nameIndiceID;

            }

            //this.formGestion.ProyectoTipo = ((long)Comun.Modulos.GLOBAL);
            //hdEmplazamientoID.SetValue(26596);
            //hdCategoriaID.SetValue(2);
        }

        #region Exportaciones

        protected void DescargaGrid()
        {
            try
            {
                string sOrden = Request.QueryString["orden"];

                string sDir = Request.QueryString["dir"];

                DataSorter[] ord = new DataSorter[1];
                ord[0] = new DataSorter
                {
                    Property = sOrden,
                    Direction = ((sDir == "ASC") ? SortDirection.ASC : SortDirection.DESC)
                };

                string sFiltro = Request.QueryString["filtro"];
                string sModuloID = Request.QueryString["aux"].ToString();
                hdEmplazamientoID.SetValue(Request.QueryString["EmplazamientoID"]);
                hdCategoriaID.SetValue(Request.QueryString["CategoriaID"]);
                if (Request["VistaPlantilla"] != null && Request["VistaPlantilla"] != "")
                {
                    hdVistaPlantilla.SetValue(Request["VistaPlantilla"]);
                    //btnDescargarTodo.Hidden = true;
                }

                int iCount = 0;
                HttpCookie GridCookie = Cookies.GetCookie("DescargaInventarioGrid");
                if ((GridCookie != null) && !string.IsNullOrEmpty(GridCookie.Value))
                {
                    string sGrid = HttpUtility.UrlDecode(GridCookie.Value);
                    GenerarGrid(sGrid, true);
                }
                else
                {
                    GenerarGrid("", true);
                }

                string sCategoria = Request.QueryString["CategoriaID"];
                string sEmplazamientoID = Request.QueryString["EmplazamientoID"];
                string sOperadores = Request.QueryString["Operadores"];
                string sEstados = Request.QueryString["Estados"];
                string sUsuarios = Request.QueryString["Usuarios"];
                string sFechaCreacionMinima = Request.QueryString["FechaCreacionMinima"];
                string sFechaCreacionMaxima = Request.QueryString["FechaCreacionMaxima"];
                string sFechaModificacionMinima = Request.QueryString["FechaModificacionMinima"];
                string sFechaModificacionMaxima = Request.QueryString["FechaModificacionMaxima"];
                string sFiltros;
                HttpCookie FiltrosCookie = Cookies.GetCookie("DescargaInventarioFiltros");
                if ((FiltrosCookie != null) && !string.IsNullOrEmpty(FiltrosCookie.Value))
                {
                    sFiltros = HttpUtility.UrlDecode(FiltrosCookie.Value);
                }
                else
                {
                    sFiltros = "[]";
                }

                List<JsonObject> listaDatos = ListaPrincipal(sCategoria, sEmplazamientoID, 0, 0, sFiltro, out int numTotal,
                    sOperadores, sEstados, sUsuarios, sFechaCreacionMinima, sFechaCreacionMaxima, sFechaModificacionMinima, sFechaModificacionMaxima, sFiltros, grid.ColumnModel, ord);

                try
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    Data.Emplazamientos oEmplazamiento = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                    Data.InventarioCategorias oCategoria = cCategorias.GetItem(long.Parse(hdCategoriaID.Value.ToString()));
                    foreach (var col in grid.ColumnModel.Columns) { col.Hidden = false; }
                    if (oEmplazamiento != null)
                    {
                        if (oCategoria != null)
                        {
                            Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oEmplazamiento.NombreSitio + '_' + oCategoria.InventarioCategoria, _Locale);
                        }
                        else
                        {
                            Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oEmplazamiento.NombreSitio, _Locale);
                        }
                    }
                    else
                    {
                        Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oCategoria.InventarioCategoria, _Locale);
                    }
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                    log.Info(GetGlobalResource(Comun.LogExcelExportado));
                }
                catch (Exception ex)
                {
                    LogController lController = new LogController();
                    lController.EscribeLog(Ip, Usuario.UsuarioID, ex.Message);
                    log.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Response.Write("ERROR: " + ex.Message);
            }
        }
        protected void DescargaGridNoColumnModels()
        {
            try
            {
                string sOrden = Request.QueryString["orden"];
                string sDir = Request.QueryString["dir"];

                DataSorter[] ord = new DataSorter[1];
                ord[0] = new DataSorter
                {
                    Property = sOrden,
                    Direction = ((sDir == "ASC") ? SortDirection.ASC : SortDirection.DESC)
                };

                string sFiltro = Request.QueryString["filtro"];
                string sModuloID = Request.QueryString["aux"].ToString();
                hdEmplazamientoID.SetValue(Request.QueryString["EmplazamientoID"]);
                hdCategoriaID.SetValue(Request.QueryString["CategoriaID"]);
                if (Request["VistaPlantilla"] != null && Request["VistaPlantilla"] != "")
                {
                    hdVistaPlantilla.SetValue(Request["VistaPlantilla"]);
                    //btnDescargarTodo.Hidden = true;
                }

                int iCount = 0;
                //string sGrid = Request.QueryString["Grid"];
                GenerarGrid("", true, true);

                string sCategoria = Request.QueryString["CategoriaID"];
                string sEmplazamientoID = Request.QueryString["EmplazamientoID"];
                string sOperadores = Request.QueryString["Operadores"];
                string sEstados = Request.QueryString["Estados"];
                string sUsuarios = Request.QueryString["Usuarios"];
                string sFechaCreacionMinima = Request.QueryString["FechaCreacionMinima"];
                string sFechaCreacionMaxima = Request.QueryString["FechaCreacionMaxima"];
                string sFechaModificacionMinima = Request.QueryString["FechaModificacionMinima"];
                string sFechaModificacionMaxima = Request.QueryString["FechaModificacionMaxima"];
                string sFiltros;
                HttpCookie FiltrosCookie = Cookies.GetCookie("DescargaInventarioFiltros");
                if ((FiltrosCookie != null) && !string.IsNullOrEmpty(FiltrosCookie.Value))
                {
                    sFiltros = HttpUtility.UrlDecode(FiltrosCookie.Value);
                }
                else
                {
                    sFiltros = "[]";
                }

                List<JsonObject> listaDatos = ListaPrincipalAtributosPlantillas(sCategoria, sEmplazamientoID, 0, 0, sFiltro, out int numTotal,
                    sOperadores, sEstados, sUsuarios, sFechaCreacionMinima, sFechaCreacionMaxima, sFechaModificacionMinima, sFechaModificacionMaxima, sFiltros, grid.ColumnModel, ord);

                try
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    Data.Emplazamientos oEmplazamiento = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                    Data.InventarioCategorias oCategoria = cCategorias.GetItem(long.Parse(hdCategoriaID.Value.ToString()));
                    foreach (var col in grid.ColumnModel.Columns) { col.Hidden = false; }
                    if (oEmplazamiento != null)
                    {
                        if (oCategoria != null)
                        {
                            Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oEmplazamiento.NombreSitio + '_' + oCategoria.InventarioCategoria, _Locale);
                        }
                        else
                        {
                            Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oEmplazamiento.NombreSitio, _Locale);
                        }
                    }
                    else
                    {
                        Comun.ExportacionDesdeListaNombreTask(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource("strElementosInventario") + '_' + oCategoria.InventarioCategoria, _Locale);
                    }
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                    log.Info(GetGlobalResource(Comun.LogExcelExportado));
                }
                catch (Exception ex)
                {
                    LogController lController = new LogController();
                    lController.EscribeLog(Ip, Usuario.UsuarioID, ex.Message);
                    log.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Response.Write("ERROR: " + ex.Message);
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PintarCategorias(false);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                sPagina = "InventarioGestionContenedor.aspx";
                funtionalities = new System.Collections.Hashtable() {
                    { "Read", new List<ComponentBase> { } },
                    { "Download", new List<ComponentBase> { btnDescargar,  }},
                    { "Post", new List<ComponentBase> { btnAnadir }},
                    { "Put", new List<ComponentBase> { btnEditar, btnMover, btnClonar, btnDescargarTodo}},
                    { "Delete", new List<ComponentBase> { btnEliminar }}
                };
                MontarGridDinamico(true);
                if (hdCategoriaActiva.Value == "NoActiva" || hdCategoriaActiva.Value == null)
                {
                    btnAnadir.Disabled = true;
                    btnEditar.Hidden = true;
                    btnCopiar.Hidden = true;
                    btnMover.Hidden = true;
                    btnClonar.Hidden = true;
                }

                if (hdEmplazamientoID.Value.ToString() == "0")
                {
                    btnAnadir.Disabled = true;
                    btnCopiar.Hidden = true;
                    btnMover.Hidden = true;
                    btnClonar.Hidden = true;
                }
                btnClonar.Hidden = true;
                hdErrorCarga.Value = false;
            }
        }

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int curPage = e.Page - 1;
                    int pageSize = Convert.ToInt32(cmbPagination.Value);
                    DataSorter[] sSort = e.Sort;
                    int total = 0;

                    string filter = e.Parameters["filter"];


                    List<JsonObject> listaObjetos;
                    listaObjetos = ListaPrincipal(hdCategoriaID.Value.ToString(), hdEmplazamientoID.Value.ToString(), curPage, pageSize, filter, out total,
                        hdOperador.Value.ToString(),
                        hdEstado.Value.ToString(),
                        hdUsuario.Value.ToString(),
                        hdFechaMinCrea.Value.ToString(),
                        hdFechaMaxCrea.Value.ToString(),
                        hdFechaMinMod.Value.ToString(),
                        hdFechaMaxMod.Value.ToString(),
                        hdFiltros.Value.ToString(),
                        null,
                        sSort
                        );
                    e.Total = total;
                    if (listaObjetos != null)
                    {
                        storePrincipal.DataSource = listaObjetos;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        protected List<JsonObject> ListaPrincipal(string CategoriaID, string EmplazamientoID, int numPag, int tmnPag, string filtros, out int numTotal,
            string listaIDsOperadores,
            string listaIDsEstados,
            string listaIDsUsuarios,
            string sFechaCreacionMin,
            string sFechaCreacionMax,
            string sFechaModMin,
            string sFechaModMax,
            string sFiltros,
            GridHeaderContainer columnModel = null,
            DataSorter[] sOrdenacion = null)
        {
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            System.Data.DataTable listaObjetos;
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            List<JsonObject> json = new List<JsonObject>();
            List<long> listaPlaIds;
            JsonObject jsonAux;
            string sValor;
            try
            {
                numTotal = 0;
                if (sOrdenacion != null && sOrdenacion.Length > 0)
                {
                    string TipoDato = "";
                    if (sOrdenacion[0].Property.StartsWith("Atr"))
                    {
                        var oAtrConf = cAtributosConf.GetItem(long.Parse(sOrdenacion[0].Property.Replace("Atr", "")));
                        TipoDato = oAtrConf.TiposDatos.Codigo;
                    }
                    listaObjetos = cInventarioElementos.GetGridDinamicoInventariov4(long.Parse(CategoriaID), long.Parse(EmplazamientoID), numPag, tmnPag,
                        listaIDsOperadores,
                        listaIDsEstados,
                        listaIDsUsuarios,
                        sFechaCreacionMin,
                        sFechaCreacionMax,
                        sFechaModMin,
                        sFechaModMax,
                        sFiltros,
                        sOrdenacion[0].Property,
                        sOrdenacion[0].Direction == SortDirection.ASC,
                        TipoDato);
                }
                else
                {
                    listaObjetos = cInventarioElementos.GetGridDinamicoInventariov4(long.Parse(CategoriaID), long.Parse(EmplazamientoID), numPag, tmnPag,
                        listaIDsOperadores,
                        listaIDsEstados,
                        listaIDsUsuarios,
                        sFechaCreacionMin,
                        sFechaCreacionMax,
                        sFechaModMin,
                        sFechaModMax,
                        sFiltros);
                }

                #region Cargas Listas

                CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();

                JsonObject listasItems = new JsonObject();
                JsonObject listaItems = new JsonObject();
                JsonObject auxJson;

                if (listaObjetos != null && listaObjetos.Rows.Count > 0)
                {
                    foreach (var oAtr in cCategoriasVin.GetAtributosByInventarioCategoriaID(long.Parse(CategoriaID)))
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

                    foreach (DataRow item in listaObjetos.Rows)
                    {
                        jsonAux = new JsonObject();
                        if (!bool.Parse(hdVistaInventario.Value.ToString()))
                        {
                            if (item[15] != null && item[15].ToString() != "")
                            {
                                foreach (var oAtr in cAtrJson.Deserializacion(item[15].ToString()))
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

                                                    if (!sValor.Equals(""))
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
                                                        jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.TextLista);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.TextLista.ToString());
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
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        //jsonAux.Add(oAtr.AtributoID.ToString(), oAtr.Valor);
                                    }
                                }
                            }
                            else
                            {
                                jsonAux = new JsonObject();
                            }
                            if (item[16] != null && item[16].ToString() != "")
                            {
                                listaPlaIds = new List<long>();
                                try
                                {
                                    foreach (dynamic oPla in cPlaJson.Deserializacion(item[16].ToString()))
                                    {
                                        jsonAux.Add("Pla" + oPla.InvCatConfID, oPla.NombrePlantilla);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }
                            }
                            /*if (item[17] != null && item[17].ToString() != "" && item[17].ToString().StartsWith("{"))
                            {
                                foreach (var oAtr in cPlaAtrJson.Deserializacion(item[17].ToString()))
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
                                        else
                                        {
                                            if (oAtr.AtributoID != null)
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
                            }*/
                        }
                        jsonAux.Add("InventarioElementoID", item[0].ToString());
                        jsonAux.Add("NumeroInventario", item[1].ToString());
                        jsonAux.Add("Nombre", item[2].ToString());
                        jsonAux.Add("InventarioCategoriaID", item[3].ToString());
                        jsonAux.Add("InventarioCategoria", item[4].ToString());
                        jsonAux.Add("EmplazamientoID", item[5].ToString());
                        jsonAux.Add("Codigo", item[6].ToString());
                        jsonAux.Add("OperadorID", item[7].ToString());
                        jsonAux.Add("Operador", item[8].ToString());
                        jsonAux.Add("EstadoID", item[9].ToString());
                        jsonAux.Add("NombreAtributoEstado", item[10].ToString());
                        jsonAux.Add("CreadorID", item[11].ToString());
                        jsonAux.Add("NombreCreador", item[12].ToString());
                        jsonAux.Add("FechaCreacion", item[13].ToString());
                        jsonAux.Add("FechaMod", item[14].ToString());
                        if (columnModel == null)
                        {
                            json.Add(jsonAux);
                        }
                        else
                        {
                            object oAux;
                            JsonObject jsonDescarga = new JsonObject();
                            columnModel.Columns.ForEach(col =>
                            {
                                if (col.DataIndex != null)
                                {
                                    if (jsonAux.TryGetValue(col.DataIndex, out oAux))
                                    {
                                        jsonDescarga.Add(col.DataIndex, oAux.ToString());
                                    }
                                    else
                                    {
                                        jsonDescarga.Add(col.DataIndex, "");
                                    }
                                }
                            });
                            json.Add(jsonDescarga);
                        }
                    }
                    if (listaObjetos.Rows[0][17].ToString().StartsWith("{"))
                    {
                        numTotal = int.Parse(listaObjetos.Rows[0][18].ToString());
                    }
                    else
                    {
                        numTotal = int.Parse(listaObjetos.Rows[0][17].ToString());
                    }
                }
                else
                {
                    numTotal = 0;
                }
            }
            catch (Exception ex)
            {
                numTotal = 0;
                json = null;
                log.Error(ex);
            }
            return json;
        }
        protected List<JsonObject> ListaPrincipalAtributosPlantillas(string CategoriaID, string EmplazamientoID, int numPag, int tmnPag, string filtros, out int numTotal,
            string listaIDsOperadores,
            string listaIDsEstados,
            string listaIDsUsuarios,
            string sFechaCreacionMin,
            string sFechaCreacionMax,
            string sFechaModMin,
            string sFechaModMax,
            string sFiltros,
            GridHeaderContainer columnModel = null,
            DataSorter[] sOrdenacion = null)
        {
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            CoreAtributosConfiguracionesController cAtributosConf = new CoreAtributosConfiguracionesController();
            System.Data.DataTable listaObjetos;
            CoreInventarioPlantillasAtributosCategoriasController cPlantillas = new CoreInventarioPlantillasAtributosCategoriasController();
            List<JsonObject> json = new List<JsonObject>();
            List<long> listaPlaIds;
            JsonObject jsonAux;
            string sValor;
            try
            {
                numTotal = 0;

                if (sOrdenacion != null && sOrdenacion.Length > 0)
                {
                    string TipoDato = "";
                    if (sOrdenacion[0].Property.StartsWith("Atr"))
                    {
                        var oAtrConf = cAtributosConf.GetItem(long.Parse(sOrdenacion[0].Property.Replace("Atr", "")));
                        TipoDato = oAtrConf.TiposDatos.Codigo;
                    }
                    listaObjetos = cInventarioElementos.GetGridDinamicoInventariov4AtributosPlantillas(long.Parse(CategoriaID), long.Parse(EmplazamientoID), numPag, tmnPag,
                        listaIDsOperadores,
                        listaIDsEstados,
                        listaIDsUsuarios,
                        sFechaCreacionMin,
                        sFechaCreacionMax,
                        sFechaModMin,
                        sFechaModMax,
                        sFiltros,
                        sOrdenacion[0].Property,
                        sOrdenacion[0].Direction == SortDirection.ASC,
                        TipoDato);
                }
                else
                {
                    listaObjetos = cInventarioElementos.GetGridDinamicoInventariov4AtributosPlantillas(long.Parse(CategoriaID), long.Parse(EmplazamientoID), numPag, tmnPag,
                        listaIDsOperadores,
                        listaIDsEstados,
                        listaIDsUsuarios,
                        sFechaCreacionMin,
                        sFechaCreacionMax,
                        sFechaModMin,
                        sFechaModMax,
                        sFiltros);
                }

                #region Cargas Listas

                CoreInventarioCategoriasAtributosCategoriasController cCategoriasVin = new CoreInventarioCategoriasAtributosCategoriasController();

                JsonObject listasItems = new JsonObject();
                JsonObject listaItems = new JsonObject();
                JsonObject auxJson;

                if (listaObjetos.Rows.Count > 0)
                {
                    foreach (var oAtr in cCategoriasVin.GetAtributosByInventarioCategoriaID(long.Parse(CategoriaID)))
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

                    foreach (DataRow item in listaObjetos.Rows)
                    {
                        jsonAux = new JsonObject();
                        if (!bool.Parse(hdVistaInventario.Value.ToString()))
                        {
                            if (item[15] != null && item[15].ToString() != "")
                            {
                                foreach (var oAtr in cAtrJson.Deserializacion(item[15].ToString()))
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
                                                        jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.TextLista);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        jsonAux.Add("Atr" + oAtr.AtributoID.ToString(), oAtr.TextLista.ToString());
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
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        //jsonAux.Add(oAtr.AtributoID.ToString(), oAtr.Valor);
                                    }
                                }
                            }
                            else
                            {
                                jsonAux = new JsonObject();
                            }
                            if (item[16] != null && item[16].ToString() != "")
                            {
                                listaPlaIds = new List<long>();
                                try
                                {
                                    foreach (dynamic oPla in cPlaJson.Deserializacion(item[16].ToString()))
                                    {
                                        jsonAux.Add("Pla" + oPla.InvCatConfID, oPla.NombrePlantilla);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }
                            }
                        }
                        jsonAux.Add("InventarioElementoID", item[0].ToString());
                        jsonAux.Add("NumeroInventario", item[1].ToString());
                        jsonAux.Add("Nombre", item[2].ToString());
                        jsonAux.Add("InventarioCategoriaID", item[3].ToString());
                        jsonAux.Add("InventarioCategoria", item[4].ToString());
                        jsonAux.Add("EmplazamientoID", item[5].ToString());
                        jsonAux.Add("Codigo", item[6].ToString());
                        jsonAux.Add("OperadorID", item[7].ToString());
                        jsonAux.Add("Operador", item[8].ToString());
                        jsonAux.Add("EstadoID", item[9].ToString());
                        jsonAux.Add("NombreAtributoEstado", item[10].ToString());
                        jsonAux.Add("CreadorID", item[11].ToString());
                        jsonAux.Add("NombreCreador", item[12].ToString());
                        jsonAux.Add("FechaCreacion", item[13].ToString());
                        jsonAux.Add("FechaMod", item[14].ToString());
                        if (columnModel == null)
                        {
                            json.Add(jsonAux);
                        }
                        else
                        {
                            object oAux;
                            JsonObject jsonDescarga = new JsonObject();
                            columnModel.Columns.ForEach(col =>
                            {
                                if (col.DataIndex != null)
                                {
                                    if (jsonAux.TryGetValue(col.DataIndex, out oAux))
                                    {
                                        jsonDescarga.Add(col.DataIndex, oAux.ToString());
                                    }
                                    else
                                    {
                                        jsonDescarga.Add(col.DataIndex, "");
                                    }
                                }
                            });
                            json.Add(jsonDescarga);
                        }
                    }
                    if (listaObjetos.Rows[0][17].ToString().StartsWith("{"))
                    {
                        numTotal = int.Parse(listaObjetos.Rows[0][18].ToString());
                    }
                    else
                    {
                        numTotal = int.Parse(listaObjetos.Rows[0][17].ToString());
                    }
                }
                else
                {
                    numTotal = 0;
                }
            }
            catch (Exception ex)
            {
                numTotal = 0;
                json = null;
                log.Error(ex);
            }
            return json;
        }

        #endregion

        #region FORM

        //#region CATEGORIAS

        //protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        InventarioCategoriasController cCategorias = new InventarioCategoriasController();
        //        try
        //        {
        //            var listaDatos = new List<Data.InventarioCategorias>();
        //            listaDatos.Add(cCategorias.GetItem(long.Parse(hdCategoriaID.Value.ToString())));
        //            storeCategoriasElementos.DataSource = listaDatos;
        //            storeCategoriasElementos.DataBind();
        //            if (listaDatos == null || listaDatos.Count == 0)
        //            {
        //                cmbCategoriaElemento.EmptyText = GetGlobalResource("strNoCategorias");
        //            }
        //            cmbCategoriaElemento.SetValue(long.Parse(hdCategoriaID.Value.ToString()));
        //            cmbCategoriaElemento.Disable();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //        }
        //    }

        //}

        //#endregion

        #region ESTADOS

        protected void storeEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();
                InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                List<Data.InventarioElementosAtributosEstados> listaDatos;
                Data.InventarioCategorias CategoriaActiva;
                try
                {

                    long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                    CategoriaActiva = cCategorias.GetItem(CategoriaID);
                    if (CategoriaActiva != null)
                    {
                        listaDatos = cEstados.GetActivos((long)CategoriaActiva.ClienteID);
                        var oDato = cEstados.GetDefault((long)CategoriaActiva.ClienteID);
                        storeEstados.DataSource = listaDatos;
                        storeEstados.DataBind();

                        if (oDato != null && (cmbEstado.Value == null || cmbEstado.Value.ToString() == ""))
                        {
                            cmbEstado.SetValue(oDato.InventarioElementoAtributoEstadoID);
                            cmbEstado.Triggers[0].Hidden = false;
                            cmbEstado.ResetOriginalValue();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Entidades> listaOperadores;
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    EntidadesController cEntidades = new EntidadesController();
                    Data.InventarioCategorias CategoriaActiva;
                    long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                    CategoriaActiva = cCategorias.GetItem(CategoriaID);
                    if (CategoriaActiva != null)
                    {
                        listaOperadores = cEntidades.getEntidadesOperadores((long)CategoriaActiva.ClienteID);
                        storeOperadores.DataSource = listaOperadores;
                        storeOperadores.DataBind();

                        if (cmbOperador.Value == null || cmbOperador.Value.ToString() == "")
                        {
                            CategoriaActiva = cCategorias.GetItem(CategoriaID);
                            this.cmbOperador.SetValue(cEntidades.getOperadorCliente((long)CategoriaActiva.ClienteID));
                            this.cmbOperador.ResetOriginalValue();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region PLANTILLAS

        protected void storePlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    InventarioCategoriasController cCategorias = new InventarioCategoriasController();
                    long CategoriaID = long.Parse(((Hidden)X.GetCmp("hdCategoriaID")).Value.ToString());
                    InventarioElementosController cElementos = new InventarioElementosController();
                    List<Data.InventarioElementos> listaDatos = new List<Data.InventarioElementos>();
                    Data.InventarioCategorias CategoriaActiva;
                    CategoriaActiva = cCategorias.GetItem(CategoriaID);
                    if (CategoriaActiva != null)
                    {
                        //listaDatos = cElementos.GetPlantillasCategoria(CategoriaID, (long)CategoriaActiva.ClienteID);
                        storePlantillas.DataSource = listaDatos;
                        storePlantillas.DataBind();
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

        #region VIEWS
        protected void storeViews_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    CoreGestionVistasController cVistas = new CoreGestionVistasController();
                    var listaDatos = cVistas.GetVistas("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaID.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                    if (listaDatos != null && listaDatos.Count > 0)
                    {
                        storeViews.DataSource = listaDatos;
                        storeViews.DataBind();

                        if (cmbViews.Value == null || cmbViews.Value.ToString() == "")
                        {
                            var oDefault = cVistas.GetDefault("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaID.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                            cmbViews.SetValue(oDefault.CoreGestionVistaID);
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        #endregion

        #region EMPLAZAMIENTOS
        protected void storeEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    var listaDatos = cEmplazamientos.GetActivos(long.Parse(hdCliID.Value.ToString()));
                    if (listaDatos != null && listaDatos.Count > 0)
                    {
                        storeEmplazamientos.DataSource = listaDatos;
                        storeEmplazamientos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

        }

        protected class EmplazamientosJson
        {
            public long EmplazamientoID { get; set; }
            public string NombreSitio { get; set; }
            public string Codigo { get; set; }
            public bool Seleccionado { get; set; }
        }

        [DirectMethod()]
        public DirectResponse CargarEmplazamientos()
        {
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                List<EmplazamientosJson> listaEmplazamientos = new List<EmplazamientosJson>();
                if (long.Parse(hdCategoriaID.Value.ToString()) != 0)
                {
                    string query = "SELECT Emp.EmplazamientoID, Emp.Codigo, Emp.NombreSitio FROM Emplazamientos Emp";
                    var oCat = cCategorias.GetItem(long.Parse(hdCategoriaID.Value.ToString()));
                    if (oCat.EmplazamientoTipoID.HasValue)
                    {
                        query += " WHERE Emp.EmplazamientoTipoID =" + oCat.EmplazamientoTipoID;
                    }
                    var oTabla = cEmplazamientos.EjecutarQuery(query);
                    foreach (DataRow item in oTabla.Rows)
                    {
                        listaEmplazamientos.Add(new EmplazamientosJson
                        {
                            EmplazamientoID = long.Parse(item.ItemArray[0].ToString()),
                            NombreSitio = item.ItemArray[1].ToString(),
                            Codigo = item.ItemArray[2].ToString(),
                            Seleccionado = false
                        });
                    }
                }
                direct.Result = JsonConvert.SerializeObject(listaEmplazamientos);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        #region FORM

        private class objCategoriaAtributo
        {
            public string ID;
            public long CategoriaAtributoID;
            public string Nombre;
            public long Orden;
            public long Modulo;
            public bool EsSubCategoria;
        }

        private void CargarListaCategorias()
        {
            try
            {
                CoreInventarioCategoriasAtributosCategoriasController cCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
                Componentes.GestionCategoriasAtributos oComponente;
                listaCategorias = new List<Componentes.GestionCategoriasAtributos>();
                long lCatID = long.Parse(hdCategoriaID.Value.ToString());
                List<Data.CoreInventarioCategoriasAtributosCategorias> listaInventarioAtributosCategorias;
                listaInventarioAtributosCategorias = cCategorias.GetCategoriasAtributosVinculadas(lCatID).ToList();
                foreach (var idCate in listaInventarioAtributosCategorias)
                {
                    oComponente = (Componentes.GestionCategoriasAtributos)this.LoadControl("../../Componentes/GestionCategoriasAtributos.ascx");
                    oComponente.ID = "CAT" + idCate.CoreInventarioCategoriaAtributoCategoriaID;
                    oComponente.CategoriaAtributoID = idCate.CoreInventarioCategoriaAtributoCategoriaConfiguracionID;
                    oComponente.Nombre = idCate.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.InventarioAtributoCategoria;
                    oComponente.Orden = idCate.Orden;
                    oComponente.Modulo = (long)Comun.Modulos.INVENTARIO;
                    if (idCate.CoreInventarioCategoriasAtributosCategoriasConfiguraciones.InventarioAtributosCategorias.EsPlantilla)
                    {
                        oComponente.EsSubCategoria = true;
                    }
                    listaCategorias.Add(oComponente);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        [DirectMethod(Timeout = 120000)]
        public DirectResponse PintarCategorias(bool Update)
        {
            CoreInventarioCategoriasAtributosCategoriasController cCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
            //Componentes.GestionCategoriasAtributos oComponente;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (Update && contenedorCategorias != null && contenedorCategorias.ContentControls.Count > 0)
                {
                    listaCategorias = new List<Componentes.GestionCategoriasAtributos>();

                }
                if (hdCategoriaID.Value != null && hdCategoriaID.Value.ToString() != "" && long.Parse(hdCategoriaID.Value.ToString()) != 0)
                {
                    if (listaCategorias == null || listaCategorias.Count == 0)
                    {
                        #region No hay hd
                        if (X.GetCmp("hdPageLoad") == null)
                        {
                            CargarListaCategorias();
                        }
                        #endregion
                        else
                        {
                            Hidden hdPL = (Hidden)X.GetCmp("hdPageLoad");
                            hdPageLoadController hdController = new hdPageLoadController(hdPL);
                            string oValor = hdController.GetValor(System.IO.Path.GetFileName(Request.Url.AbsolutePath));
                            if (oValor == "")
                            {
                                CargarListaCategorias();
                                List<objCategoriaAtributo> listaObjCategorias = new List<objCategoriaAtributo>();
                                foreach (var oCat in listaCategorias)
                                {
                                    listaObjCategorias.Add(new objCategoriaAtributo
                                    {
                                        ID = oCat.ID,
                                        CategoriaAtributoID = oCat.CategoriaAtributoID,
                                        Nombre = oCat.Nombre,
                                        Orden = oCat.Orden,
                                        Modulo = oCat.Modulo,
                                        EsSubCategoria = oCat.EsSubCategoria
                                    });
                                }
                                oValor = JsonConvert.SerializeObject(listaObjCategorias);
                                hdController.SetValor(System.IO.Path.GetFileName(Request.Url.AbsolutePath), oValor);
                            }
                            else
                            {
                                var settings = new JsonSerializerSettings
                                {
                                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                    MissingMemberHandling = MissingMemberHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                    NullValueHandling = NullValueHandling.Ignore,
                                    DefaultValueHandling = DefaultValueHandling.Ignore,
                                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                                };

                                contenedorCategorias.ContentControls.Clear();
                                contenedorCategorias.Dispose();

                                var listaObjCategorias = JsonConvert.DeserializeObject<List<objCategoriaAtributo>>(oValor, settings).AsReadOnly();
                                listaCategorias = new List<Componentes.GestionCategoriasAtributos>();
                                //foreach (var oCat in listaObjCategorias)
                                for (int i = 0; i < listaObjCategorias.Count; i++)
                                {
                                    var oComponente = (Componentes.GestionCategoriasAtributos)this.LoadControl("../../Componentes/GestionCategoriasAtributos.ascx");
                                    oComponente.ID = listaObjCategorias[i].ID;
                                    oComponente.CategoriaAtributoID = listaObjCategorias[i].CategoriaAtributoID;
                                    oComponente.Nombre = listaObjCategorias[i].Nombre;
                                    oComponente.Orden = listaObjCategorias[i].Orden;
                                    oComponente.Modulo = listaObjCategorias[i].Modulo;
                                    oComponente.EsSubCategoria = listaObjCategorias[i].EsSubCategoria;
                                    listaCategorias.Add(oComponente);
                                }
                                GC.KeepAlive(listaObjCategorias);
                            }
                            listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                            foreach (var item in listaCategorias)
                            {
                                contenedorCategorias.ContentControls.Add(item);
                            }
                        }
                    }
                    listaCategorias = listaCategorias.OrderBy(it => it.Orden).ToList();
                    foreach (var item in listaCategorias)
                    {
                        contenedorCategorias.ContentControls.Add(item);
                    }
                }
                if (Update)
                {
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
                hdErrorCarga.Value = true;
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        #region GENERACION AUTOMÁTICA CÓDIGO


        [DirectMethod(Timeout = 120000)]
        public DirectResponse ComprobarCodigoInventarioDuplicado()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;


            try
            {
                #region COMPROBAR CODIGO
                if (cCategorias.CodigoDuplicadoGeneradorCodigos(hdCodigoAutogenerado.Value.ToString(), long.Parse(hdEmplazamientoID.Value.ToString())))
                {

                    hdCodigoDuplicado.Value = "Duplicado";
                }
                else
                {
                    hdCodigoDuplicado.Value = "No_Duplicado";
                }
                #endregion

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }


        [DirectMethod(Timeout = 120000)]
        public DirectResponse GenerarCamposInventario(bool Codigo, bool Nombre)
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;
            Data.GlobalCondicionesReglas aplicarRegla = new GlobalCondicionesReglas();
            JsonObject[] reglas = new JsonObject[2];

            try
            {
                if (Codigo)
                {
                    #region CODIGO

                    aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("CODIGO_INVENTARIO", (long)Comun.Modulos.GLOBAL);

                    if (aplicarRegla != null)
                    {
                        configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                        if (configuraciones != null && configuraciones.Count > 0)
                        {
                            if (!(hdVistaPlantilla.Value != null && hdVistaPlantilla.Value.ToString() != ""))
                            {
                                JsonObject listaIDs = new JsonObject();
                                listaIDs.Add("Emplazamientos", long.Parse(hdEmplazamientoID.Value.ToString()));
                                listaIDs.Add("InventarioCategorias", long.Parse(hdCategoriaID.Value.ToString()));
                                reglas[0] = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);

                                if (reglas[0] == null)
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                                    return direct;
                                }
                            }

                            hdCondicionCodigoReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                            return direct;
                        }
                    }

                    #endregion
                }

                if (Nombre)
                {
                    #region NOMBRE

                    aplicarRegla = new GlobalCondicionesReglas();
                    aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("NOMBRE_INVENTARIO", (long)Comun.Modulos.GLOBAL);

                    if (aplicarRegla != null)
                    {
                        configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                        if (configuraciones != null && configuraciones.Count > 0)
                        {
                            if (!(hdVistaPlantilla.Value != null && hdVistaPlantilla.Value.ToString() != ""))
                            {
                                JsonObject listaIDs = new JsonObject();
                                listaIDs.Add("Emplazamientos", long.Parse(hdEmplazamientoID.Value.ToString()));
                                listaIDs.Add("InventarioCategorias", long.Parse(hdCategoriaID.Value.ToString()));
                                reglas[1] = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);

                                if (reglas[1] == null)
                                {
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.strGeneracionNombreFallida);
                                    return direct;
                                }
                            }

                            hdCondicionNombreReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);
                        }

                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                log.Error(ex.Message);
                return direct;
            }

            direct.Result = reglas;
            return direct;
        }

        #endregion

        #endregion

        [DirectMethod(Timeout = 120000)]
        public DirectResponse GuardarValor(bool bAgregar, bool SobrescribirEdicion)
        {
            InventarioElementosController cElementos = new InventarioElementosController();
            CoreInventarioElementosAtributosController cAtributos = new CoreInventarioElementosAtributosController();
            cAtributos.SetDataContext(cElementos.Context);
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            cEmplazamientos.SetDataContext(cElementos.Context);
            CoreInventarioElementosPlantillasAtributosCategoriasAtributosController cPlantillas = new CoreInventarioElementosPlantillasAtributosCategoriasAtributosController();
            cPlantillas.SetDataContext(cElementos.Context);
            InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
            cHistorico.SetDataContext(cElementos.Context);
            Data.InventarioElementosHistoricos DatoHistorico;
            InfoResponse cResponse;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (hdErrorCarga.Value == "True" || hdErrorCarga.Value == "true")
                {
                    direct.Success = false;
                    direct.Result = "ErrorPageLoad";
                    return direct;
                }
                if (!bAgregar)
                {
                    DatoHistorico = cHistorico.getHistoricoByID(long.Parse(GridRowSelect.SelectedRows[0].RecordID));
                }
                else
                {
                    DatoHistorico = null;
                }
                if (bAgregar || (SobrescribirEdicion || (DatoHistorico == null && hdHistoricoInventario.Value.ToString() == "") || (hdHistoricoInventario.Value.ToString() != "" && long.Parse(hdHistoricoInventario.Value.ToString()) == DatoHistorico.InventarioElementoHistoricoID)))
                {
                    long CategoriaID = long.Parse(hdCategoriaID.Value.ToString());

                    long EmplazamientoID = long.Parse(hdEmplazamientoID.Value.ToString());
                    List<object> ListaAtributos = new List<object>();
                    List<object> ListaPlantillas = new List<object>();
                    foreach (var item in listaCategorias)
                    {
                        if (!item.GuardarValor(ListaAtributos, cElementos.Context, ListaPlantillas))
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                    if (bAgregar)
                    {

                        GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
                        cCondicionesConfiguraciones.SetDataContext(cElementos.Context);

                        if (hdCondicionCodigoReglaID.Value.ToString() != "" && hdCodigoAutogenerado.Value.ToString() == txtCodigoElemento.Text.Trim())
                        {
                            if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionCodigoReglaID.Value.ToString()), txtCodigoElemento.Text))
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource("strErrorActualizarCodigoAutomatico");
                                return direct;
                            }
                        }

                        if (hdCondicionNombreReglaID.Value.ToString() != "" && hdNombreAutogenerado.Value.ToString() == txtNombreElemento.Text.Trim())
                        {
                            if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionNombreReglaID.Value.ToString()), txtNombreElemento.Text))
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource("strErrorActualizarCodigoAutomatico");
                                return direct;
                            }
                        }

                        var oElemento = new InventarioElementos
                        {
                            Activo = true,
                            Plantilla = false,
                            Nombre = txtNombreElemento.Value.ToString(),
                            NumeroInventario = txtCodigoElemento.Value.ToString(),
                            InventarioCategoriaID = CategoriaID,
                            InventarioElementoAtributoEstadoID = long.Parse(cmbEstado.Value.ToString()),
                            Emplazamientos = cEmplazamientos.GetItem(EmplazamientoID),
                            FechaCreacion = DateTime.Now,
                            FechaAlta = DateTime.Now,
                            UltimaModificacionFecha = DateTime.Now,
                            UltimaModificacionUsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                            CreadorID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                            EntidadID = long.Parse(cmbOperador.Value.ToString())
                        };

                        cResponse = cElementos.AddUpdateInventarioElemento(oElemento,
                            ListaAtributos,
                            ListaPlantillas);
                    }
                    else
                    {
                        var oElemento = cElementos.GetItem(long.Parse(GridRowSelect.SelectedRows[0].RecordID));
                        oElemento.Nombre = txtNombreElemento.Value.ToString();
                        oElemento.NumeroInventario = txtCodigoElemento.Value.ToString();
                        oElemento.EntidadID = long.Parse(cmbOperador.Value.ToString());
                        oElemento.InventarioElementoAtributoEstadoID = long.Parse(cmbEstado.Value.ToString());
                        oElemento.UltimaModificacionFecha = DateTime.Now;
                        oElemento.UltimaModificacionUsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID;
                        cResponse = cElementos.AddUpdateInventarioElemento(oElemento,
                            ListaAtributos,
                            ListaPlantillas);
                    }
                    if (cResponse.Result)
                    {
                        cResponse = cElementos.SubmitChanges();
                        if (cResponse.Result)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = cResponse.Description;
                            return direct;
                        }
                    }
                    else
                    {
                        if (cResponse.Code == ServicesCodes.INVENTORY_ELEMENT.COD_TBO_INVENTORY_ELEMENT_CODE_DUPLICATE)
                        {
                            hdCodigoDuplicado.SetValue("Duplicado");
                        }
                        direct.Success = false;
                        direct.Result = cResponse.Description;
                        return direct;
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = "Editado";
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
            return direct;
        }

        [DirectMethod(Timeout = 120000)]
        public DirectResponse MostrarEditar()
        {
            List<object> listaValoresAtributos = new List<object>();
            List<object> listaValoresPlantillas = new List<object>();
            Data.InventarioElementos oInventarioElementos;
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            InventarioElementosAtributosJsonController cAtrJson = new InventarioElementosAtributosJsonController();
            InventarioElementosPlantillasJsonController cPlaJson = new InventarioElementosPlantillasJsonController();
            InventarioPlantillasAtributosJsonController cPlaAtrJson = new InventarioPlantillasAtributosJsonController();
            CoreInventarioCategoriasAtributosCategoriasController cAtr = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreInventarioCategoriasAtributosCategoriasController cCategorias = new CoreInventarioCategoriasAtributosCategoriasController();
            JsonObject jsDatos = new JsonObject();

            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            try
            {
                long ElementoID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);
                long CatID = long.Parse(hdCategoriaID.Value.ToString());

                oInventarioElementos = cInventarioElementos.GetItem(ElementoID);

                var listaAtr = cAtr.GetAtributosByInventarioCategoriaID(CatID);
                var listaCat = cCategorias.GetCategoriasAtributosVinculadas(CatID).ToList();

                InventarioElementosHistoricosController cHistorico = new InventarioElementosHistoricosController();
                Data.InventarioElementosHistoricos DatoHistorico;

                DatoHistorico = cHistorico.getHistoricoByID(ElementoID);
                if (DatoHistorico != null)
                {
                    hdHistoricoInventario.Value = DatoHistorico.InventarioElementoHistoricoID.ToString();
                }
                else
                {
                    hdHistoricoInventario.Value = "";
                }

                foreach (var item in cAtrJson.Deserializacion(oInventarioElementos.JsonAtributosDinamicos))
                {
                    CoreAtributosConfiguraciones Atr = listaAtr.FindAll(c => c.CoreAtributoConfiguracionID == item.AtributoID).FirstOrDefault();
                    if (Atr != null)
                    {
                        object sValor = item.Valor;
                        if (item.Valor != "")
                        {
                            switch (Atr.TiposDatos.Codigo)
                            {
                                case Comun.TIPODATO_CODIGO_FECHA:
                                    sValor = DateTime.Parse(item.Valor).ToString(Comun.FORMATO_FECHA);
                                    break;
                                case Comun.TIPODATO_CODIGO_LISTA_MULTIPLE:
                                    sValor = item.Valor.Split(',').ToList();
                                    break;
                                case Comun.TIPODATO_CODIGO_BOOLEAN:
                                    sValor = bool.Parse(item.Valor);
                                    break;
                                default:
                                    break;
                            }
                        }

                        listaValoresAtributos.Add(new
                        {
                            ControlID = "CAT" + Atr.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos[0].CoreInventarioCategoriasAtributosCategoriasConfiguraciones.CoreInventarioCategoriasAtributosCategorias.ToList().FindAll(c => c.InventarioCategoriaID == CatID).First().CoreInventarioCategoriaAtributoCategoriaID + "_ATR" + Atr.CoreAtributoConfiguracionID + "_Control" + Atr.CoreAtributoConfiguracionID,
                            Valor = sValor
                        });
                    }
                }

                List<long> listaPlaIds = new List<long>();
                foreach (var item in cPlaJson.Deserializacion(oInventarioElementos.JsonPlantillas))
                {
                    CoreInventarioCategoriasAtributosCategorias oCat = listaCat.FindAll(c => c.CoreInventarioCategoriaAtributoCategoriaConfiguracionID == item.InvCatConfID).FirstOrDefault();
                    listaValoresAtributos.Add(new
                    {
                        ControlID = "CAT" + oCat.CoreInventarioCategoriaAtributoCategoriaID + "_cmbPlantilla",
                        Valor = item.PlantillaID
                    });
                    listaPlaIds.Add(item.PlantillaID);
                }

                direct.Result = new
                {
                    Nombre = oInventarioElementos.Nombre,
                    NumeroInventario = oInventarioElementos.NumeroInventario,
                    EstadoID = oInventarioElementos.InventarioElementoAtributoEstadoID,
                    OperadorID = oInventarioElementos.EntidadID,
                    JsonAtributosDinamicos = listaValoresAtributos
                };
            }
            catch (Exception ex)
            {
                hdErrorCarga.Value = true;
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }
            return direct;
        }

        [DirectMethod(Timeout = 120000)]
        public DirectResponse EliminarElemento()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                long ElementoID = long.Parse(GridRowSelect.SelectedRows[0].RecordID);
                InventarioElementosController cInventarioElementos = new InventarioElementosController();
                InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
                List<Data.InventarioElementos> listaVinculaciones = new List<Data.InventarioElementos>();
                listaVinculaciones.AddRange(cVinculaciones.GetElementosHijos(ElementoID));
                listaVinculaciones.AddRange(cVinculaciones.GetElementosPadres(ElementoID));
                InfoResponse cResponse;
                direct.Success = true;
                direct.Result = "";
                if (listaVinculaciones.Count == 0)
                {
                    var oDato = cInventarioElementos.GetItem(ElementoID);
                    cResponse = cInventarioElementos.Delete(oDato);
                    if (cResponse.Result)
                    {
                        cResponse = cInventarioElementos.SubmitChanges();
                        if (cResponse.Result)
                        {
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = cResponse.Description;
                            return direct;
                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                        return direct;
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource("jsTieneVinculaciones");
                    return direct;
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

        [DirectMethod(Timeout = 120000)]
        public DirectResponse MoverElementos(long EmplazamientoID)
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse cResponse;
            try
            {
                InventarioElementosController cElementos = new InventarioElementosController();
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                cEmplazamientos.SetDataContext(cElementos.Context);
                var oEmplazamiento = cEmplazamientos.GetItem(EmplazamientoID);
                List<InventarioElementos> listaElementos = new List<InventarioElementos>();
                foreach (var Inv in GridRowSelect.SelectedRows)
                {
                    listaElementos.Add(cElementos.GetItem(long.Parse(Inv.RecordID)));
                }
                cResponse = cElementos.MoverElementos(listaElementos, oEmplazamiento);
                if (cResponse.Result)
                {
                    cResponse = cElementos.SubmitChanges();
                    if (cResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = cResponse.Description;
                        return direct;
                    }
                }
                else
                {
                    direct.Success = false;
                    direct.Result = cResponse.Description;
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
            return direct;
        }

        public DirectResponse ClonarElementos(long ElementoID, long[] EmplazamientoID)
        {
            DirectResponse direct = new DirectResponse();
            try
            {

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        #endregion

        #region FUNCTIONS
        [DirectMethod(Timeout = 120000)]
        public DirectResponse MontarGridDinamico(bool isRender, bool AtrPla = false)
        {
            DirectResponse direct = new DirectResponse();
            CoreInventarioCategoriasAtributosCategoriasController cCatVin = new CoreInventarioCategoriasAtributosCategoriasController();
            CoreInventarioCategoriasAtributosCategoriasConfiguracionesController cCatConf = new CoreInventarioCategoriasAtributosCategoriasConfiguracionesController();
            CoreAtributosConfiguracionRolesRestringidosController cRestriccionRoles = new CoreAtributosConfiguracionRolesRestringidosController();
            CoreGestionVistasController cVistas = new CoreGestionVistasController();
            CoreGestionVistas oVista;
            InventarioCategoriasController cCategorias = new InventarioCategoriasController();
            TiposDatosController cTipoDatos = new TiposDatosController();
            Data.InventarioCategorias cCategoria;
            int numerocol = 5;
            ColumnasJsonController cColumnas = new ColumnasJsonController();
            List<ColumnasJson> listaColumnas = new List<ColumnasJson>();
            Column col;
            try
            {
                listaColumnasGrid = new List<ColumnBase>();
                direct.Success = true;
                direct.Result = "";

                cCategoria = cCategorias.GetItem(long.Parse(hdCategoriaID.Value.ToString()));

                col = new Column
                {
                    ID = "colNumeroInventario",
                    Text = GetGlobalResource("strCodigo"),
                    DataIndex = "NumeroInventario",
                    MinWidth = 100,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                listaColumnasGrid.Add(col);
                listaColumnas.Add(new ColumnasJson
                {
                    NombreColumna = GetGlobalResource("strCodigo"),
                    Columna = "NumeroInventario",
                    Orden = 0,
                    Visible = true
                });

                col = new Column
                {
                    ID = "colNombre",
                    Text = GetGlobalResource("strNombre"),
                    DataIndex = "Nombre",
                    MinWidth = 100,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                listaColumnasGrid.Add(col);
                listaColumnas.Add(new ColumnasJson
                {
                    NombreColumna = GetGlobalResource("strNombre"),
                    Columna = "Nombre",
                    Orden = 1,
                    Visible = true
                });

                col = new Column
                {
                    ID = "colEmplazamiento",
                    Text = GetGlobalResource("strEmplazamiento"),
                    DataIndex = "Codigo",
                    MinWidth = 100,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                listaColumnasGrid.Add(col);
                listaColumnas.Add(new ColumnasJson
                {
                    NombreColumna = GetGlobalResource("strEmplazamiento"),
                    Columna = "Codigo",
                    Orden = 2,
                    Visible = true
                });

                col = new Column
                {
                    ID = "colOperador",
                    Text = GetGlobalResource("strOperador"),
                    DataIndex = "Operador",
                    MinWidth = 120,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                listaColumnasGrid.Add(col);
                listaColumnas.Add(new ColumnasJson
                {
                    NombreColumna = GetGlobalResource("strOperador"),
                    Columna = "Operador",
                    Orden = 3,
                    Visible = true
                });

                col = new Column
                {
                    ID = "colEstado",
                    Text = GetGlobalResource("strEstado"),
                    DataIndex = "NombreAtributoEstado",
                    MinWidth = 120,
                    Sortable = true,
                    Filterable = false,
                    Resizable = true,
                    Flex = 1
                };
                listaColumnasGrid.Add(col);
                listaColumnas.Add(new ColumnasJson
                {
                    NombreColumna = GetGlobalResource("strEstado"),
                    Columna = "NombreAtributoEstado",
                    Orden = 4,
                    Visible = true
                });

                if (Request["VistaInventario"] != null && Request["VistaInventario"] != "" && Request["VistaInventario"] != "false")
                {
                    hdVistaInventario.Value = true;
                    btnAnadir.Hidden = true;
                    btnEditar.Hidden = true;
                    col = new Column
                    {
                        ID = "colCategoria",
                        Text = GetGlobalResource("strCategoria"),
                        DataIndex = "InventarioCategoria",
                        MinWidth = 150,
                        Flex = 1
                    };
                    listaColumnas.Add(new ColumnasJson
                    {
                        NombreColumna = col.Text,
                        Columna = col.DataIndex,
                        Orden = numerocol,
                        Visible = true
                    });
                    numerocol++;
                    listaColumnasGrid.Add(col);
                }
                else
                {
                    hdVistaInventario.SetValue(false);
                }

                if (!bool.Parse(hdVistaInventario.Value.ToString()))
                {
                    if (cCategorias != null && cCategoria.Activo)
                    {
                        hdCategoriaActiva.Value = "Activa";
                    }
                    else
                    {
                        hdCategoriaActiva.Value = "NoActiva";
                        btnAnadir.Hidden = true;
                        btnEditar.Hidden = true;
                    }

                    List<Data.CoreInventarioCategoriasAtributosCategoriasConfiguraciones> listaPlantillas = cCatConf.GetPlantillasCategoriaID(long.Parse(hdCategoriaID.Value.ToString()));
                    foreach (var oPla in listaPlantillas)
                    {
                        ModelField modelField = new ModelField
                        {
                            Name = "Pla" + oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                            Type = ModelFieldType.String
                        };
                        storePrincipal.ModelInstance.Fields.Add(modelField);
                        col = new Column
                        {
                            ID = "colPla" + oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                            Text = oPla.InventarioAtributosCategorias.InventarioAtributoCategoria,
                            DataIndex = "Pla" + oPla.CoreInventarioCategoriaAtributoCategoriaConfiguracionID,
                            MinWidth = 100,
                            Sortable = true,
                            Filterable = false,
                            Flex = 1
                        };
                        listaColumnas.Add(new ColumnasJson
                        {
                            NombreColumna = col.Text,
                            Columna = col.DataIndex,
                            Orden = numerocol,
                            Visible = true
                        });
                        numerocol++;
                        listaColumnasGrid.Add(col);
                    }

                    List<Data.CoreAtributosConfiguraciones> listaAtributos;

                    listaAtributos = cCatVin.GetAtributosVisiblesByInventarioCategoriaID(long.Parse(hdCategoriaID.Value.ToString()), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);

                    foreach (var atr in listaAtributos)
                    {
                        ModelField modelField = new ModelField
                        {
                            Name = "Atr" + atr.CoreAtributoConfiguracionID
                        };
                        Data.TiposDatos oTipoDato;
                        oTipoDato = cTipoDatos.GetItem(atr.TipoDatoID);
                        switch (oTipoDato.TipoDato)
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
                        storePrincipal.ModelInstance.Fields.Add(modelField);
                        if (oTipoDato.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                        {
                            Column colDate = new Column
                            {
                                ID = "col" + atr.CoreAtributoConfiguracionID,
                                Text = atr.Codigo,
                                DataIndex = "Atr" + atr.CoreAtributoConfiguracionID,
                                MinWidth = 100,
                                Sortable = true,
                                Filterable = false,
                                Flex = 1,
                            };
                            listaColumnas.Add(new ColumnasJson
                            {
                                NombreColumna = colDate.Text,
                                Columna = colDate.DataIndex,
                                Orden = numerocol,
                                Visible = true
                            });
                            numerocol++;
                            listaColumnasGrid.Add(colDate);
                        }
                        else
                        {
                            col = new Column
                            {
                                ID = "col" + atr.CoreAtributoConfiguracionID,
                                Text = atr.Codigo,
                                DataIndex = "Atr" + atr.CoreAtributoConfiguracionID,
                                MinWidth = 100,
                                Sortable = true,
                                Filterable = false,
                                Flex = 1
                            };
                            listaColumnas.Add(new ColumnasJson
                            {
                                NombreColumna = col.Text,
                                Columna = col.DataIndex,
                                Orden = numerocol,
                                Visible = true
                            });
                            numerocol++;
                            listaColumnasGrid.Add(col);
                        }
                    }

                }

                WidgetColumn colMore = new WidgetColumn
                {
                    ID = "colVerMas",
                    Text = GetGlobalResource("strMas"),
                    Cls = "NoOcultar col-More",
                    Align = ColumnAlign.Center,
                    Sortable = true,
                    Filterable = false,
                    Resizable = false,
                    MinWidth = 50
                };

                Button btnMore = new Button
                {
                    OverCls = "Over-btnMore",
                    PressedCls = "Pressed-none",
                    FocusCls = "Focus-none",
                    Cls = "btnColMore"
                };

                btnMore.OnClientClick = "VerMas(this);";

                colMore.Widget.Add(btnMore);

                listaColumnasGrid.Add(colMore);

                if (isRender)
                {
                    oVista = cVistas.GetDefault("InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaID.Value.ToString(), ((Data.Usuarios)Session["USUARIO"]).UsuarioID);
                    if (oVista != null)
                    {
                        GenerarGrid(oVista.JsonColumnas, isRender);
                        oVista.JsonColumnas = cColumnas.Serializacion((from c in cColumnas.Deserializacion(oVista.JsonColumnas) where (from oCol in listaColumnas select oCol.Columna).ToList().Contains(c.Columna) select c).ToList());
                        hdColumas.Value = cColumnas.Serializacion(cColumnas.Deserializacion(oVista.JsonColumnas).Concat(listaColumnas).Distinct(new ColumnasJsonControllerComparer()).ToList());
                        hdViewJson.Value = hdColumas.Value;
                        hdFiltros.Value = oVista.JsonFiltros;
                    }
                    else
                    {
                        GenerarGrid("", isRender);
                        oVista = new Data.CoreGestionVistas
                        {
                            Nombre = GetGlobalResource("jsDefecto"),
                            UsuarioID = ((Data.Usuarios)Session["USUARIO"]).UsuarioID,
                            JsonColumnas = cColumnas.Serializacion(listaColumnas),
                            JsonFiltros = "",
                            Pagina = "InventarioCategoryViewVistaCategoria.aspx&CatID=" + hdCategoriaID.Value.ToString(),
                            Defecto = true
                        };
                        if ((oVista = cVistas.AddItem(oVista)) == null)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        hdColumas.Value = cColumnas.Serializacion(listaColumnas);
                        hdViewJson.Value = cColumnas.Serializacion(listaColumnas);
                    }
                }

                //hdColumas.Value = cColumnas.Serializacion(listaColumnas);

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        [DirectMethod(Timeout = 120000)]
        public DirectResponse GenerarGrid(string JsonColumnas, bool isRender, bool AtrPla = false)
        {
            ColumnasJsonController cColumnas = new ColumnasJsonController();
            List<ColumnasJson> listaColumnas;
            List<ColumnBase> listaCols = new List<ColumnBase>();
            ColumnBase colAux;
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";
            try
            {
                if (listaColumnasGrid == null)
                {
                    MontarGridDinamico(false, AtrPla);
                }
                if (!isRender)
                {
                    grid.RemoveAllColumns();
                }
                if (JsonColumnas != "")
                {
                    listaColumnas = cColumnas.Deserializacion(JsonColumnas);
                    if (listaColumnas != null && listaColumnas.Count > 0)
                    {
                        foreach (var oCol in listaColumnas)
                        {
                            if (oCol.Visible)
                            {
                                colAux = (from c in listaColumnasGrid where c.DataIndex == oCol.Columna select c).FirstOrDefault();
                                if (colAux != null)
                                {
                                    if (oCol.NombreColumna == "Site")
                                    {
                                        WidgetColumn oColumna = new WidgetColumn
                                        {
                                            ID = "col" + oCol.Columna,
                                            Text = oCol.NombreColumna,
                                            Align = ColumnAlign.Start,
                                            Sortable = true,
                                            MinWidth = 100,
                                            Flex = 1,
                                            DataIndex = oCol.Columna
                                        };

                                        Button oLink = new Button
                                        {
                                            PressedCls = "Pressed-none",
                                            FocusCls = "Focus-none",
                                            Cls = "underlineColumn"
                                        };

                                        oLink.OnClientClick = "sitesInfo(this);";
                                        oColumna.Widget.Add(oLink);
                                        listaCols.Add(oColumna);
                                    }
                                    else
                                    {
                                        listaCols.Add(colAux);
                                    }
                                }
                            }
                        }
                        colAux = (from c in listaColumnasGrid where c.ID == "colVerMas" select c).FirstOrDefault();
                        if (colAux != null)
                        {
                            listaCols.Add(colAux);
                        }
                    }
                }
                else
                {
                    grid.ColumnModel.Columns.AddRange(listaColumnasGrid);
                }
                //grid.UpdateLayout();
                //grid.Refresh();
                if (!isRender)
                {
                    foreach (var col in listaCols)
                    {
                        grid.AddColumn(col);
                    }
                }
                else
                {
                    grid.ColumnModel.Columns.AddRange(listaCols);
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

    }
}