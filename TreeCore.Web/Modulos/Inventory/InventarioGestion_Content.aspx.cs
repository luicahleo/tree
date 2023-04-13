using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using TreeCore.Data;

namespace TreeCore.ModInventario
{
    public partial class InventarioGestion_Content : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        List<Container> contenedores;
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTIÓN DE PÁGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                hdHideMenuClick.Value = "false";
                if (!(listaFuncionalidades.Contains(500512) || listaFuncionalidades.Contains(500513)))
                {
                    hdHideMenuClick.Value = "true";
                }

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                #region NIVEL MAXIMO PERMITIDO
                ParametrosController cParametros = new ParametrosController();
                string sNivel = cParametros.GetItemValor(Comun.MENU_NIVEL_MAXIMO);
                if (!string.IsNullOrEmpty(sNivel))
                {
                    hdNivelMaxPermitido.Value = sNivel;
                }
                else
                {
                    hdNivelMaxPermitido.Value = 3;
                }

                #endregion

                if (Request["EmplazamientoID"] != null && Request["EmplazamientoID"] != "")
                {
                    hdEmplazamientoID.SetValue(Request["EmplazamientoID"]);
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();
                    lbRutaEmplazamiento.Text = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString())).NombreSitio;
                }
                else
                {
                    hdEmplazamientoID.SetValue(0);
                }

                hdElementoPadre.SetValue(0);

                //this.formGestion.ProyectoTipo = ((long)Comun.Modulos.GLOBAL);

                storePrincipal.Reload();
            }
            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.Vw_InventarioElementosReducida> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        //listaDatos = ListaPrincipal(0, 0, "", "", ref iCount, "", CliID);

                        //Clases.LinqEngine.PagingItemsListWithExtNetFilter(listaDatos, sFiltro, "", sOrden, sDir, 0, 0, ref iCount);
                        #region ESTADISTICAS
                        try
                        {
                            EmplazamientosController cEmplazamientos = new EmplazamientosController();
                            Emplazamientos emplazamiento = cEmplazamientos.GetItem(CliID);
                            //Comun.ExportacionDesdeListaNombre(TreePanelV1.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString() + emplazamiento.NombreSitio, _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };

        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
                List<JsonObject> listaJson;
                JsonObject oJson;

                try
                {
                    List<Vw_InventarioElementosVinculaciones> lista;

                    long? ElemID = long.Parse(hdElementoPadre.Value.ToString());
                    if (ElemID == 0)
                    {
                        ElemID = null;
                    }
                    if (cmbTiposVinculaciones.Value != null && cmbTiposVinculaciones.Value.ToString() != "")
                    {
                        List<long> TiposVinculacionesID = new List<long>();
                        foreach (var item in cmbTiposVinculaciones.SelectedItems)
                        {
                            TiposVinculacionesID.Add(long.Parse(item.Value));
                        }
                        lista = cVinculaciones.GetVwVinculacionesFromEmplazamiento(long.Parse(hdEmplazamientoID.Value.ToString()), ElemID, TiposVinculacionesID);
                    }
                    else
                    {
                        lista = cVinculaciones.GetVwVinculacionesFromEmplazamiento(long.Parse(hdEmplazamientoID.Value.ToString()), ElemID);
                    }

                    if (lista != null)
                    {
                        listaJson = new List<JsonObject>();
                        foreach (var item in lista)
                        {
                            oJson = new JsonObject();
                            oJson.Add("InventarioElementoVinculacionID", item.InventarioElementoVinculacionID);
                            oJson.Add("InventarioCategoriaVinculacionID", item.InventarioCategoriaVinculacionID);
                            oJson.Add("InventarioElementoPadreID", item.InventarioElementoPadreID);
                            oJson.Add("InventarioElementoID", item.InventarioElementoID);
                            oJson.Add("Nombre", item.NombreElemento);
                            oJson.Add("NumeroInventario", item.NumeroInventario);
                            oJson.Add("Operador", item.NombreEntidad);
                            oJson.Add("FechaCreacion", item.FechaCreacion);
                            oJson.Add("InventarioCategoria", item.InventarioCategoria);
                            oJson.Add("EstadoInventarioElemento", item.NombreEstado);
                            oJson.Add("Icono", Comun.rutaIconoWebInventario(item.Icono));
                            oJson.Add("Padres", GetPadres(item.InventarioElementoID).Result);
                            oJson.Add("Vinculaciones", GetVinculaciones(item.InventarioCategoriaVinculacionID).Result);
                            oJson.Add("CreadorID", item.CreadorID);
                            oJson.Add("FechaAlta", item.FechaCreacion);
                            oJson.Add("FechaMod", item.UltimaModificacionFecha);
                            oJson.Add("OperadorID", item.OperadorID);
                            oJson.Add("EstadoID", item.InventarioElementoAtributoEstadoID);
                            listaJson.Add(oJson);
                        }
                        storePrincipal.DataSource = listaJson;
                        storePrincipal.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ELEMENTOS

        #region ALL ELEMENTOS

        protected void storeElementos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosController cElementos = new InventarioElementosController();
                try
                {

                    var lista = cElementos.GetElementosByEmplazamientoID(long.Parse(hdEmplazamientoID.Value.ToString()));

                    if (lista != null)
                    {
                        storeElementos.DataSource = lista;
                        storeElementos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ELEMENTOS FORM

        protected void storeElementosForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosController cElementos = new InventarioElementosController();
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                Data.Emplazamientos oEmplazamiento;
                Data.InventarioElementos oElemento;
                List<JsonObject> listaDatos;
                JsonObject jsonAux;
                try
                {
                    oEmplazamiento = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                    List<InventarioElementos> lista;
                    if (long.Parse(hdElementoPadre.Value.ToString()) == 0)
                    {
                        lista = cElementos.GetElementosNoAsignadosByEmplazamientoIDDisponible(null, oEmplazamiento.EmplazamientoID, null, oEmplazamiento.EmplazamientoTipoID);
                    }
                    else
                    {
                        oElemento = cElementos.GetItem(long.Parse(hdElementoPadre.Value.ToString()));
                        lista = cElementos.GetElementosNoAsignadosByEmplazamientoIDDisponible(oElemento.InventarioElementoID, oEmplazamiento.EmplazamientoID, oElemento.InventarioCategoriaID, oEmplazamiento.EmplazamientoTipoID);
                    }

                    if (lista != null)
                    {
                        listaDatos = new List<JsonObject>();
                        foreach (var item in lista)
                        {
                            jsonAux = new JsonObject();
                            jsonAux.Add("InventarioElementoID", item.InventarioElementoID);
                            jsonAux.Add("Nombre", item.Nombre + ", " + item.NumeroInventario + " (" + item.InventarioCategorias.InventarioCategoria + ")");
                            listaDatos.Add(jsonAux);
                        }
                        storeElementosForm.DataSource = listaDatos;
                        storeElementosForm.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region ELEMENTOS PADRES

        [DirectMethod(Timeout = 120000)]
        public DirectResponse GetPadres(long lCatID)
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listaPadres = new JsonObject();
            JsonObject aux;

            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
            List<Data.InventarioElementos> listaVin;

            try
            {
                listaVin = cVinculaciones.GetElementosPadres(lCatID);
                foreach (var item in listaVin)
                {
                    aux = new JsonObject();
                    aux.Add("InventarioElementoID", item.InventarioElementoID);
                    aux.Add("NumeroInventario", item.NumeroInventario);
                    listaPadres.Add(item.InventarioElementoID.ToString(), aux);
                }

                direct.Success = true;
                direct.Result = listaPadres.ToJsonString();
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

        #region TIPOS VINCULACIONES

        protected void storeTiposVinculaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                EmplazamientosController cEmplazamientos = new EmplazamientosController();
                Data.Emplazamientos oEmpl;
                InventarioTiposVinculacionesController cVinculaciones = new InventarioTiposVinculacionesController();
                try
                {
                    oEmpl = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                    var lista = cVinculaciones.GetActivos(oEmpl.ClienteID);

                    if (lista != null)
                    {
                        storeTiposVinculaciones.DataSource = lista;
                        storeTiposVinculaciones.DataBind();
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

        #region VINCULACION

        [DirectMethod(Timeout = 120000)]
        public DirectResponse AgregarEditarVinculacion(bool Agregar)
        {
            DirectResponse direct = new DirectResponse();

            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();
            Data.InventarioElementosVinculaciones oDato;
            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.Emplazamientos oEmplazamientos;
            InventarioCategoriasVinculacionesController cCateVinculaciones = new InventarioCategoriasVinculacionesController();
            Data.InventarioCategoriasVinculaciones oCatVin;
            InventarioElementosController cElementos = new InventarioElementosController();
            Data.InventarioElementos oElementos, oElementoPadre = null;
            Clases.ResponseCreateController reponse;

            try
            {
                oEmplazamientos = cEmplazamientos.GetItem(long.Parse(hdEmplazamientoID.Value.ToString()));
                oElementos = cElementos.GetItem(long.Parse(cmbElementos.Value.ToString()));
                if (oEmplazamientos != null && oElementos != null)
                {
                    long? CatPadre;
                    long? ElePadre = long.Parse(hdElementoPadre.Value.ToString());
                    if (ElePadre == 0)
                    {
                        ElePadre = null;
                    }
                    if (ElePadre != null)
                    {
                        oElementoPadre = cElementos.GetItem((long)ElePadre);
                        CatPadre = oElementoPadre.InventarioCategoriaID;
                    }
                    else
                    {
                        CatPadre = null;
                    }
                    reponse = cVinculaciones.SaveUpdateVinculacion(oElementoPadre, oElementos, oEmplazamientos);
                    if (!reponse.infoResponse.Result)
                    {
                        direct.Success = false;
                        direct.Result = reponse.infoResponse.Description;
                        return direct;
                    }
                    else
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
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

        [DirectMethod(Timeout = 120000)]
        public DirectResponse EliminarVinculacion()
        {
            DirectResponse direct = new DirectResponse();
            long lVinculacionID = long.Parse(GridRowSelect.SelectedRecordID);
            InventarioElementosVinculacionesController cVinculaciones = new InventarioElementosVinculacionesController();

            try
            {
                if (cVinculaciones.DeleteItem(lVinculacionID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

        [DirectMethod(Timeout = 120000)]
        public DirectResponse GetVinculaciones(long lVinID)
        {
            DirectResponse direct = new DirectResponse();
            JsonObject listaPadres = new JsonObject();
            JsonObject aux;

            InventarioTiposVinculacionesController cVinculaciones = new InventarioTiposVinculacionesController();
            List<Data.InventarioTiposVinculaciones> listaVin;

            try
            {
                listaVin = cVinculaciones.GetTiposFromVinculacion(lVinID);
                foreach (var item in listaVin)
                {
                    aux = new JsonObject();
                    aux.Add("InventarioTipoVinculacionID", item.InventarioTipoVinculacionID);
                    aux.Add("Nombre", item.Nombre);
                    listaPadres.Add(item.InventarioTipoVinculacionID.ToString(), aux);
                }

                direct.Success = true;
                direct.Result = listaPadres.ToJsonString();
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

    }
}