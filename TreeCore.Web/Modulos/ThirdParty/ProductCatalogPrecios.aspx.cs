using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class ProductCatalogPrecios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        public string sListaCat = null;

        #region GESTION PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));


                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>() { };

                Comun.CreateGridFilters(gridFiltersPrecio, storeCoreProductCatalog, gridPrecios.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                DirectResponse dResponse = new DirectResponse();
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<JsonObject> listaDatos = null;
                        JObject oValor = new JObject();
                        List<Object> listaResults = new List<Object>();
                        List<JObject> listaFinal = new List<JObject>();
                        List<Vw_CoreProductCatalogs> listaCatalogos = null;
                        CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sCliente = Request.QueryString["cliente"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sTextoBuscado = Request.QueryString["aux4"];
                        string sIdBuscado = Request.QueryString["aux5"];
                        sListaCat = Request.QueryString["aux3"];
                        bool bDescarga = true;
                        int iCount = 0;
                        string sVariablesExcluidas = "";
                        int total = 0;

                        hdStringBuscadorTraduccion.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                        hdIDTraduccionBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                        listaDatos = cServicios.AplicarFiltroInterno(-1, -1, out total, null, sFiltro, hdStringBuscadorTraduccion, hdIDTraduccionBuscador, bDescarga, pnColumnaTraducciones.ColumnModel, sVariablesExcluidas);
                        listaCatalogos = ListaCatalogos(0, 0, sOrden, sDir, ref iCount, sFiltro);
                        listaResults = (List<Object>)GetEstadoActualPrecioJSON(false, listaDatos).Result;

                        if (listaResults != null)
                        {
                            if (listaResults.Count > 0)
                            {
                                foreach (JObject oResult in listaResults)
                                {
                                    oValor = JSON.Deserialize<JObject>(oResult.ToString());
                                    listaFinal.Add(oValor);
                                }

                                ExportarModeloDatos(listaFinal, listaDatos, listaCatalogos);
                            }
                        }
                        else
                        {
                            Comun.ExportacionDesdeListaNombre(pnColumnaTraducciones.ColumnModel, listaDatos, Response, "", GetGlobalResource("strPrecios").ToString(), _Locale);
                        }

                        #region ESTADISTICAS
                        try
                        {

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

            sPagina = "ProductCatalogServiciosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargarPrecio }},
            { "Post", new List<ComponentBase> {  }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> {  }}
            };
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storeCoreProductCatalog_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {

            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    int pageSize = Convert.ToInt32(cmbNumRegistrosPrecios.Value);
                    int curPage = e.Page - 1;
                    int total;
                    string sFiltro = e.Parameters["gridFiltersPrecio"];

                    List<JsonObject> lista;
                    CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();

                    lista = cCatalogos.AplicarFiltroInternoByEntidad(pageSize, curPage, out total, e.Sort, sFiltro, hdStringBuscadorPrecios, hdIDPrecioBuscador, false, gridPrecios.ColumnModel, "");

                    if (storeCoreProductCatalog != null && lista != null)
                    {
                        e.Total = total;
                        hdTotalCountGridPrecios.SetValue(total);

                        storeCoreProductCatalog.DataSource = lista;
                        storeCoreProductCatalog.DataBind();
                    }
                }
                catch (Exception ex)
                {

                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_CoreProductCatalogs> ListaCatalogos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {

            List<Data.Vw_CoreProductCatalogs> listaDatos = new List<Vw_CoreProductCatalogs>();
            CoreProductCatalogsController cServicios = new CoreProductCatalogsController();

            try
            {
                if (sListaCat != "" && sListaCat != null)
                {
                    foreach (string sID in sListaCat.Split(','))
                    {
                        Data.Vw_CoreProductCatalogs oDato = cServicios.getVistaByID(long.Parse(sID));

                        if (oDato != null)
                        {
                            listaDatos.Add(oDato);
                        }
                    }
                }
                else
                {
                    listaDatos = cServicios.GetItemsWithExtNetFilterList<Vw_CoreProductCatalogs>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }
            }
            catch (Exception ex)
            {

                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #region ENTIDADES

        protected void storeEntidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Entidades> listaEntidades;
                    EntidadesController cEntity = new EntidadesController();

                    listaEntidades = cEntity.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaEntidades != null)
                    {
                        storeEntidades.DataSource = listaEntidades;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region TRADUCCIONES 

        protected void storeTraducciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<JsonObject> listaDatos = new List<JsonObject>();
                    List<JsonObject> lista = new List<JsonObject>();
                    CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

                    JsonObject oCodigo = new JsonObject();
                    oCodigo.Add("Codigo", GetGlobalResource("strCodigo"));
                    oCodigo.Add("Nombre", GetGlobalResource("strServicio"));
                    listaDatos.Add(oCodigo);

                    int pageSize = Convert.ToInt32(cmbNumRegistrosTraduccion.Value);
                    int curPage = e.Page - 1;
                    int total;
                    string sFiltro = e.Parameters["filter"];

                    lista = cServicios.AplicarFiltroInterno(pageSize, curPage, out total, e.Sort, sFiltro, hdStringBuscadorTraduccion, hdIDTraduccionBuscador, false, pnColumnaTraducciones.ColumnModel, "");

                    if ((hdIDTraduccionBuscador.Value != null && hdIDTraduccionBuscador.Value != "")
                        || (hdStringBuscadorTraduccion.Value != null && hdStringBuscadorTraduccion.Value != ""))
                    {
                        DirectResponse direct = new DirectResponse();
                        direct = GetEstadoActualPrecioJSON(true, lista);
                        X.Js.Call("agregarColumnaDinamicaPrecio", null, null, null, direct.Result);
                    }

                    foreach (JsonObject oDato in lista)
                    {
                        listaDatos.Add(oDato);
                    }

                    if (storeTraducciones != null && listaDatos != null)
                    {
                        e.Total = total;
                        hdTotalCountGridTraduccion.SetValue(total);

                        storeTraducciones.DataSource = listaDatos;
                        storeTraducciones.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<JsonObject> ListaPrecios()
        {
            List<JsonObject> listaDatos = new List<JsonObject>();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

            try
            {
                List<CoreProductCatalogServicios> listaServicios = cServicios.GetItemsList<CoreProductCatalogServicios>();

                foreach (CoreProductCatalogServicios oDato in listaServicios)
                {
                    JsonObject temp = new JsonObject();
                    string sCodigo = oDato.Codigo;
                    string sServicio = oDato.Nombre;

                    temp.Add("Codigo", sCodigo);
                    temp.Add("Nombre", sServicio);
                    listaDatos.Add(temp);
                }
            }
            catch (Exception ex)
            {

                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod]
        public DirectResponse GetEstadoActualPrecioJSON(bool bFiltro, List<JsonObject> listaFiltrada)
        {
            DirectResponse direct = new DirectResponse();
            List<Object> listaJsonOrdenado = new List<Object>();
            CoreProductCatalogServiciosAsignadosController cAsignados = new CoreProductCatalogServiciosAsignadosController();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

            try
            {
                if (hdServicioPrecioID.Value != null && hdServicioPrecioID.Value.ToString() != null && hdServicioPrecioID.Value.ToString() != "")
                {
                    long lCategoriaID = long.Parse(hdServicioPrecioID.Value.ToString());

                    CoreProductCatalogServiciosAsignados vwServiciosAsignados = cAsignados.getItemByCatalogoID(lCategoriaID);

                    if (vwServiciosAsignados != null)
                    {
                        JObject oJsonServicio = JObject.Parse(Comun.ObjectToJSON(vwServiciosAsignados, "CoreProductCatalogServicios").ToString());

                        if (bFiltro && (hdIDTraduccionBuscador.Value != null && hdIDTraduccionBuscador.Value != "")
                        || (hdStringBuscadorTraduccion.Value != null && hdStringBuscadorTraduccion.Value != ""))
                        {
                            JObject oJsonServicioOrdenado = new JObject();
                            List<Vw_CoreProductCatalogServiciosAsignados> servAsign = cAsignados.getItemsByCatalogoID(lCategoriaID);

                            foreach (Vw_CoreProductCatalogServiciosAsignados oServicio in servAsign)
                            {
                                foreach (string sCampoValido in Comun.CamposValidosProductCatalog)
                                {
                                    if (listaFiltrada != null)
                                    {
                                        foreach (JsonObject oServ in listaFiltrada)
                                        {
                                            string sValor = "";

                                            if ((long.Parse(oServ["CoreProductCatalogServicioID"].ToString())) == oServicio.CoreProductCatalogServicioID)
                                            {
                                                string sNombre = sCampoValido + oServ["CoreProductCatalogServicioID"].ToString();
                                                string sMoneda = cAsignados.getMonedaByCatalogoID(lCategoriaID);
                                                //string sIdentificador = cServicios.getIdentificador(long.Parse(oServ["CoreProductCatalogServicioID"].ToString()));

                                                if (long.Parse(oServ["Cantidad"].ToString()) != 1)
                                                {
                                                    //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + oServ["Cantidad"].ToString() + sIdentificador;
                                                }
                                                else
                                                {
                                                    //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + sIdentificador;
                                                }

                                                oJsonServicioOrdenado.Add(sNombre, sValor);
                                            }
                                        }
                                    }
                                }
                            }

                            direct.Success = true;
                            direct.Result = oJsonServicioOrdenado;

                            return direct;
                        }
                        else
                        {
                            List<CoreProductCatalogServicios> listaServicios = cServicios.GetItemsList<CoreProductCatalogServicios>();

                            JObject oJsonServicioOrdenado = new JObject();
                            foreach (string sCampoValido in Comun.CamposValidosProductCatalog)
                            {
                                foreach (CoreProductCatalogServicios oServ in listaServicios)
                                {
                                    string sValor = "";

                                    if (oServ.CoreProductCatalogServicioID == vwServiciosAsignados.CoreProductCatalogServicioID)
                                    {
                                        string sNombre = sCampoValido + oServ.CoreProductCatalogServicioID;
                                        string sMoneda = cAsignados.getMonedaByCatalogoID(lCategoriaID);
                                        //string sIdentificador = cServicios.getIdentificador(oServ.CoreProductCatalogServicioID);

                                        if (oServ.Cantidad != 1)
                                        {
                                            //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + oServ.Cantidad + sIdentificador;
                                        }
                                        else
                                        {
                                            //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + sIdentificador;
                                        }

                                        oJsonServicioOrdenado.Add(sNombre, sValor);
                                    }
                                    else
                                    {
                                        string sNombreColumn = sCampoValido + oServ.CoreProductCatalogServicioID;
                                        oJsonServicioOrdenado.Add(sNombreColumn, "");
                                    }
                                }
                            }

                            direct.Success = true;
                            direct.Result = oJsonServicioOrdenado;

                            return direct;
                        }
                    }
                }
                else if (sListaCat != null && sListaCat != "")
                {
                    foreach (string sID in sListaCat.Split(','))
                    {
                        CoreProductCatalogServiciosAsignados vwServiciosAsignados = cAsignados.getItemByCatalogoID(long.Parse(sID));
                        List<Data.Vw_CoreProductCatalogServicios> listaServicios = new List<Data.Vw_CoreProductCatalogServicios>();

                        if (vwServiciosAsignados != null)
                        {
                            JObject oJsonServicio = JObject.Parse(Comun.ObjectToJSON(vwServiciosAsignados, "CoreProductCatalogServicios").ToString());

                            if (hdStringBuscadorTraduccion.Value != null && hdStringBuscadorTraduccion.Value != "")
                            {
                                listaServicios = cServicios.getListaByFiltro(hdIDTraduccionBuscador.Value.ToString());
                            }
                            else if (hdIDTraduccionBuscador.Value != null && hdIDTraduccionBuscador.Value != "")
                            {
                                Vw_CoreProductCatalogServicios oServicio = cServicios.GetItem<Vw_CoreProductCatalogServicios>("CoreProductCatalogServicioID ==" + long.Parse(hdIDTraduccionBuscador.Value.ToString()));
                                listaServicios.Add(oServicio);
                            }
                            else
                            {
                                listaServicios = cServicios.GetItemsList<Vw_CoreProductCatalogServicios>();
                            }

                            JObject oJsonServicioOrdenado = new JObject();

                            foreach (string sCampoValido in Comun.CamposValidosProductCatalog)
                            {
                                foreach (Vw_CoreProductCatalogServicios oServ in listaServicios)
                                {
                                    string sValor = "";

                                    if (oServ.CoreProductCatalogServicioID == vwServiciosAsignados.CoreProductCatalogServicioID)
                                    {
                                        string sNombre = sCampoValido + oServ.CoreProductCatalogServicioID;
                                        string sMoneda = cAsignados.getMonedaByCatalogoID(long.Parse(sID));
                                        //string sIdentificador = cServicios.getIdentificador(oServ.CoreProductCatalogServicioID);

                                        if (oServ.Cantidad != 1)
                                        {
                                            //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + oServ.Cantidad + sIdentificador;
                                        }
                                        else
                                        {
                                            //sValor = oJsonServicio[sCampoValido] + sMoneda + " / " + sIdentificador;
                                        }

                                        oJsonServicioOrdenado.Add(sNombre, sValor);
                                    }
                                    else
                                    {
                                        string sNombreColumn = sCampoValido + oServ.CoreProductCatalogServicioID;
                                        oJsonServicioOrdenado.Add(sNombreColumn, "");
                                    }
                                }
                            }
                            listaJsonOrdenado.Add(oJsonServicioOrdenado);
                        }
                    }

                    direct.Success = true;
                    direct.Result = listaJsonOrdenado;

                    return direct;
                }
            }
            catch (Exception ex)
            {

                direct.Success = false;
                direct.Result = null;
                log.Error(ex.Message);
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse ExportarModeloDatos(List<JObject> listaResult, List<JsonObject> listaDatos, List<Vw_CoreProductCatalogs> listaCatalogos)
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosAsignadosController cController = new CoreProductCatalogServiciosAsignadosController();

            try
            {
                direct.Success = true;
                direct.Result = "";

                int i = 0;
                int j = 0;
                int k = 0;
                int l = 0;
                List<string> listaCabecera = new List<string>();
                List<string> listaKeys = new List<string>();
                List<JsonObject> listaColumnas = new List<JsonObject>();
                List<JsonObject> listaFinal = new List<JsonObject>();
                List<string> lColumnas = new List<string>();
                List<string> lValores = new List<string>();

                string directorio = DirectoryMapping.GetTemplatesTempDirectory();
                string fileName = GetGlobalResource("strPrecios") + ".xls";

                string saveAs = directorio + fileName;

                foreach (Vw_CoreProductCatalogs oDato in listaCatalogos)
                {
                    string sCabecera = oDato.Codigo;
                    listaCabecera.Add(sCabecera);
                }

                JsonObject oColumn = new JsonObject();
                oColumn.Add("Codigo", GetGlobalResource("strCodigo"));
                oColumn.Add("Nombre", GetGlobalResource("strServicio"));

                foreach (string sCabecera in listaCabecera)
                {
                    oColumn.Add("Creador" + k, sCabecera);
                    k++;
                }

                listaColumnas.Add(oColumn);

                foreach (JObject oJson in listaResult)
                {
                    foreach (string sKey in oJson.Properties())
                    {
                        listaKeys.Add(sKey);
                    }

                    foreach (JsonObject jDato in listaDatos)
                    {
                        if (listaFinal.Count <= listaDatos.Count - 1)
                        {
                            JsonObject oDato = new JsonObject();
                            oDato.Add("Codigo", jDato["Codigo"]);
                            oDato.Add("Servicio", jDato["Nombre"]);

                            foreach (JObject oDinam in listaResult)
                            {
                                foreach (string sKey in oDinam.Properties())
                                {
                                    if (i == j)
                                    {
                                        if (sKey == "")
                                        {
                                            oDato.Add("Creador" + l, "");
                                        }
                                        else
                                        {
                                            oDato.Add("Creador" + l, sKey);
                                        }
                                    }
                                    i++;
                                }
                                l++;
                                i = 0;
                            }

                            if (!listaFinal.Contains(oDato))
                            {
                                listaFinal.Add(oDato);
                                j++;
                            }
                        }
                    }
                }

                foreach (JsonObject oValor in listaFinal)
                {
                    foreach (string sCont in oValor.Values)
                    {
                        lValores.Add(sCont);
                    }
                }

                foreach (JsonObject oCabecera in listaColumnas)
                {
                    foreach (string sCab in oCabecera.Values)
                    {
                        lColumnas.Add(sCab);
                    }
                }

                cController.ExportarProductCatalogPrecios(saveAs, GetGlobalResource("strPrecios"), lValores, lColumnas);
                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                Tree.Web.MiniExt.Location(ResourceManagerTreeCore, DirectoryMapping.GetFileTemplatesTempDirectoryRelative(fileName), false);

                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

                FileInfo file = new FileInfo(saveAs);

                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                HttpContext.Current.Response.TransmitFile(saveAs);
                HttpContext.Current.Response.Flush();
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
        public DirectResponse GetDatosBuscadorPrecios()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cCatalogos.AplicarFiltroInternoByEntidad(-1, -1, out total, null, null, hdStringBuscadorPrecios, hdIDPrecioBuscador, false, gridPrecios.ColumnModel, "");
                direct.Result = lista;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;

            return direct;
        }

        [DirectMethod]
        public DirectResponse GetDatosBuscadorTraduccion()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cServicios.AplicarFiltroInterno(-1, -1, out total, null, null, hdStringBuscadorTraduccion, hdIDTraduccionBuscador, false, pnColumnaTraducciones.ColumnModel, "");
                direct.Result = lista;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;

            return direct;
        }

        #endregion
    }
}