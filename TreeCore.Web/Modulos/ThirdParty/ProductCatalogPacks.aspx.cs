using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.APIClient;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.Page;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using ListItemCollection = Ext.Net.ListItemCollection;

namespace TreeCore.PaginasComunes
{
    public partial class ProductCatalogPacks : TreeCore.Page.BasePageExtNet
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;
        private void Page_Init(object sender, EventArgs e)
        {

            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                //             //#region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridCatalogos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                Comun.CreateGridFilters(gridFiltersServicios, storeServiciosAsignados, gridServiciosCatalogos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //             //#endregion

                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion


                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                //}

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            List<ProductDTO> listaDatos = null;

                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            long CliID = long.Parse(Request.QueryString["cliente"]);
                            bool bActivo = Request.QueryString["aux"] == "true";
                            int iCount = 0;
                            string sGrid = Request.QueryString["aux3"];

                            QueryDTO queryDTO = QueryWeb.ParseFilterDTO(sFiltro, sOrden, sDir, null);

                            BaseAPIClient<ProductDTO> aPIClient = new BaseAPIClient<ProductDTO>(TOKEN_API);

                            listaDatos = aPIClient.GetList(queryDTO).Result.Value;

                            if (sGrid == "1")
                            {

                                string sOrden1 = Request.QueryString["orden"];
                                string sDir1 = Request.QueryString["dir"];
                                string sCliente = Request.QueryString["cliente"];
                                string sFiltro1 = Request.QueryString["filtro"];
                                string sTextoBuscado = Request.QueryString["aux4"];
                                string sIdBuscado = Request.QueryString["aux5"];
                                bool bDescarga = true;
                                string sVariablesExcluidas = "CoreProductCatalogID, CoreProductCatalogServicioPackAsignadoID, CoreProductCatalogServicioTipoID, CoreProductCatalogServicioID, ";
                                int total = 0;

                                hdStringBuscador2.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                                hdIDCatalogoBuscador2.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                                List<JsonObject> lista;
                                CoreProductCatalogServiciosController mControl = new CoreProductCatalogServiciosController();
                                long sMaestroID = long.Parse(Request.QueryString["aux"].ToString());
                                lista = mControl.AplicarFiltroInternoByPackID(-1, -1, out total, null, sFiltro1, hdStringBuscador2, hdIDCatalogoBuscador2, bDescarga, gridServiciosCatalogos.ColumnModel, sVariablesExcluidas, sMaestroID);


                                #region ESTADISTICAS
                                try
                                {
                                    Comun.ExportacionDesdeListaNombreTask(gridServiciosCatalogos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource("strServicio").ToString(), Comun.DefaultLocale);

                                    log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }
                                #endregion
                            }
                            else
                            {

                                string sOrden1 = Request.QueryString["orden"];
                                string sDir1 = Request.QueryString["dir"];
                                string sCliente = Request.QueryString["cliente"];
                                string sFiltro1 = Request.QueryString["filtro"];
                                string sTextoBuscado = Request.QueryString["aux4"];
                                string sIdBuscado = Request.QueryString["aux5"];
                                bool bDescarga = true;
                                string sVariablesExcluidas = "CoreProductCatalogPackID, CoreProductCatalogServicioID, CoreProductCatalogServicioTipoID, CoreProductCatalogServicioPackAsignadoID, ";
                                int total = 0;

                                hdStringBuscador.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                                hdIDCatalogoBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                                List<JsonObject> lista;
                                CoreProductCatalogsPacksController cProductos = new CoreProductCatalogsPacksController();

                                lista = cProductos.AplicarFiltroInterno(-1, -1, out total, null, sFiltro1, hdStringBuscador, hdIDCatalogoBuscador, bDescarga, gridCatalogos.ColumnModel, sVariablesExcluidas);

                                //listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                                #region ESTADISTICAS
                                try
                                {
                                    //Comun.ExportacionDesdeListaNombreTemplate(gridCatalogos.ColumnModel, lista, Response, "", GetGlobalResource("strCatalogos").ToString(), _Locale);
                                    Comun.ExportacionDesdeListaNombreTask(gridCatalogos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource("strPaquetes").ToString(), Comun.DefaultLocale);

                                    log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                    cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                }

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
                storePrincipal.Reload();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "ProductCatalogServiciosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir, btnAnadirClausulas }},
            { "Put", new List<ComponentBase> { btnEditar}},
            { "Delete", new List<ComponentBase> { btnEliminar, btnEliminaClausulas }}
            };
        }

        #region STORES
        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)

        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProductDTO> APIClientProduct = new BaseAPIClient<ProductDTO>(TOKEN_API);
                    List<Filter> listFilters = new List<Filter>();

                    string sSort, sDir = null;
                    int pageSize = Convert.ToInt32(cmbNumRegistros.Value);
                    int curPage = e.Page - 1;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    string sFiltro = e.Parameters["filter"];

                    Filter fDTO = new Filter(nameof(ProductDTO.IsPack), nameof(Operators.eq), true, Filter.Types.AND, null);
                    listFilters.Add(fDTO);

                    QueryDTO qFilter = QueryWeb.ParseFilterDTO(sFiltro, sSort, sDir, listFilters, pageSize, curPage);
                    var lista = APIClientProduct.GetList(qFilter).Result;

                    if (lista != null)
                    {
                        e.Total = lista.TotalItems;
                        hdTotalCountGrid.SetValue(lista.TotalItems);

                        store.DataSource = lista.Value;
                        store.DataBind();
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


        #region TYPE

        protected void storeProductCatalogTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProductTypeDTO> APIClient = new BaseAPIClient<ProductTypeDTO>(TOKEN_API);
                    var lista = APIClient.GetList().Result;
                    if (lista != null)
                    {
                        store.DataSource = lista.Value;
                        store.DataBind();
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

        #endregion#region ENTIDADES

        protected void storeEntidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CompanyDTO> ApiClient = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    List <CompanyDTO> listaFinal = new List<CompanyDTO>();
                    if (lista != null)
                    {
                        foreach (CompanyDTO comp in lista.Value)
                        {
                            if (comp.Supplier == true)
                            {
                                listaFinal.Add(comp);
                            }
                        }

                        store.DataSource = listaFinal;
                        store.DataBind();
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

        #endregion

        #region SERVICIOS

        protected void storeProductCatalogServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {

                    string sSort, sDir = null;
                    int pageSize = 100;
                    int curPage = e.Page - 1;
                    string sFiltro = e.Parameters["filter"];
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();

                    QueryDTO qFilter = QueryWeb.ParseFilterDTO(sFiltro, sSort, sDir, null, pageSize, curPage);

                    BaseAPIClient<ProductDTO> APIClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
                    var listaServicios = APIClient.GetList(qFilter).Result;

                    ProductDTO oCat = new ProductDTO();
                    List<JsonObject> lista = new List<JsonObject>();

                    oCat = APIClient.GetByCode(hdProductCatalogID.Value.ToString()).Result.Value;


                    if (oCat != null)
                    {
                        foreach (ProductDTO oServ in listaServicios.Value)
                        {
                            bool bAsign = false;

                            JsonObject oDato = new JsonObject();
                            oDato.Add("CodeProduct", oServ.Code);
                            oDato.Add("Name", oServ.Name);
                            oDato.Add("ProductTypeCode", oServ.ProductTypeCode);
                            oDato.Add("CodeCatalog", oCat.Code);
                            oDato.Add("IsPack", oCat.IsPack);
                            oDato.Add("Public", oCat.Public);
                            oDato.Add("SupplierCompany", oCat.SupplierCompany);

                            lista.Add(oDato);
                        }
                    }
                    else
                    {
                        foreach (ProductDTO oServ in listaServicios.Value)
                        {
                            if (oServ.IsPack != true)
                            {
                                if (oServ.Public == true)
                                {
                                    bool bAsign = false;

                                    JsonObject oDato = new JsonObject();
                                    oDato.Add("CodeProduct", oServ.Code);
                                    oDato.Add("Name", oServ.Name);
                                    oDato.Add("ProductTypeCode", oServ.ProductTypeCode);
                                    oDato.Add("CodeCatalog", "");
                                    oDato.Add("Price", 0);
                                    oDato.Add("EsAsignado", bAsign);
                                    lista.Add(oDato);
                                }
                                else if (oServ.SupplierCompany == oCat.SupplierCompany)
                                {
                                    bool bAsign = false;

                                    JsonObject oDato = new JsonObject();
                                    oDato.Add("CodeProduct", oServ.Code);
                                    oDato.Add("Name", oServ.Name);
                                    oDato.Add("ProductTypeCode", oServ.ProductTypeCode);
                                    oDato.Add("CodeCatalog", "");
                                    oDato.Add("Price", 0);
                                    oDato.Add("EsAsignado", bAsign);
                                    lista.Add(oDato);
                                }
                            }                            
                        }
                    }

                    lista = LinqEngine.SortJson(lista, e.Sort);

                    if (lista != null)
                    {
                        e.Total = lista.Count();

                        store.DataSource = lista;
                        store.DataBind();
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

        #endregion

        #region STORE Servicios Asignados

        protected void storeServiciosAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<ProductDTO> APIClientProduct = new BaseAPIClient<ProductDTO>(TOKEN_API);
                    List<Filter> listFilters = new List<Filter>();
                    List<JsonObject> listaFinal = new List<JsonObject>();
                    List<ProductDTO> listaProducts = new List<ProductDTO>();

                    string sSort, sDir = null;
                    int pageSize = Convert.ToInt32(cmbNumRegistros2.Value);
                    int curPage = e.Page - 1;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    string sFiltro = e.Parameters["filter"];

                    ProductDTO pro = APIClientProduct.GetByCode(hdProductCatalogID.Value.ToString()).Result.Value;

                    if (pro != null && pro.LinkedProducts != null && pro.LinkedProducts.Count > 0)
                    {
                        foreach (string prod in pro.LinkedProducts)
                        {
                            Filter fDTO = new Filter(nameof(ProductDTO.Code), nameof(Operators.eq), prod, Filter.Types.OR, null);
                            listFilters.Add(fDTO);

                            ProductDTO product = APIClientProduct.GetByCode(prod).Result.Value;

                            listaProducts.Add(product);
                        }

                        //QueryDTO qFilter = QueryWeb.ParseFilterDTO(sFiltro, sSort, sDir, listFilters, pageSize, curPage);
                        //var lista = APIClientProduct.GetList(qFilter).Result;

                        //int i = 0;
                        //foreach (ProductDTO oServ in lista.Value)
                        //{
                        //    JsonObject oDato = new JsonObject();
                        //    oDato.Add("CodeProduct", oServ.Code);
                        //    oDato.Add("Name", pro.Name);
                        //    listaFinal.Add(oDato);
                        //    i++;
                        //}

                        if (listaProducts != null)
                        {
                            e.Total = listaProducts.Count();
                            hdTotalCountGrid2.SetValue(listaProducts.Count());

                            store.DataSource = listaProducts;
                            store.DataBind();
                        }
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

        #endregion

        //#region SERVICIOS

        //protected void storeProductCatalogServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {

        //            int pageSize = 100;
        //            int curPage = e.Page - 1;
        //            int total;
        //            string sFiltro = e.Parameters["filter"];
        //            long cliID = long.Parse(hdCliID.Value.ToString());

        //            Data.Vw_CoreProductCatalogPacks oCat = new Vw_CoreProductCatalogPacks();
        //            CoreProductCatalogServiciosPacksAsignados oAsign = new CoreProductCatalogServiciosPacksAsignados();
        //            List<Data.Vw_CoreProductCatalogServicios> listaServicios = new List<Vw_CoreProductCatalogServicios>();

        //            CoreProductCatalogsPacksController cCatg = new CoreProductCatalogsPacksController();
        //            CoreProductCatalogServiciosPacksAsignadosController cServAsign = new CoreProductCatalogServiciosPacksAsignadosController();
        //            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

        //            long lCatalogoID = 0;
        //            string sSimbolo = "";
        //            string sNombreServicio = "";
        //            string sIdentificador = "";
        //            List<JsonObject> lista = new List<JsonObject>();

        //            listaServicios = cServicios.AplicarFiltroInternoCatalogo(pageSize, curPage, out total, e.Sort, sFiltro, hdStringBuscador3, hdIDCatalogoBuscador3, false, gridWinServicios.ColumnModel, "");

        //            foreach (Data.Vw_CoreProductCatalogServicios oServ in listaServicios)
        //            {
        //                bool bAsign = false;
        //                sNombreServicio = oServ.Nombre;

        //                oCat = cCatg.getVistaByID(long.Parse(hdProductCatalogID.Value.ToString()));

        //                if (oCat != null)
        //                {
        //                    //sIdentificador = oServ.Identificador;
        //                    sSimbolo = sIdentificador;

        //                    long lServAsignID = cServAsign.getValorByValoresID(oServ.CoreProductCatalogServicioID, oCat.CoreProductCatalogPackID);

        //                    if (lServAsignID != 0)
        //                    {
        //                        oAsign = cServAsign.GetItem(lServAsignID);

        //                        if (oAsign != null)
        //                        {
        //                            bAsign = true;
        //                            lCatalogoID = long.Parse(hdProductCatalogID.Value.ToString());
        //                        }

        //                        JsonObject oDato = new JsonObject();
        //                        oDato.Add("CoreProductCatalogServicioID", oAsign.CoreProductCatalogServicioID);
        //                        oDato.Add("CoreProductCatalogServicioPackAsignadoID", oAsign.CoreProductCatalogServicioPackAsignadoID);
        //                        oDato.Add("NombreCatalogServicio", sNombreServicio);
        //                        oDato.Add("NombreCatalogServicioTipo", oServ.CodigoTipo);
        //                        oDato.Add("CoreProductCatalogID", lCatalogoID);
        //                        oDato.Add("Simbolo", sSimbolo);
        //                        oDato.Add("EsAsignado", bAsign);
        //                        lista.Add(oDato);
        //                    }
        //                    else
        //                    {
        //                        JsonObject oDato = new JsonObject();
        //                        oDato.Add("CoreProductCatalogServicioID", oServ.CoreProductCatalogServicioID);
        //                        oDato.Add("CoreProductCatalogServicioPackAsignadoID", "");
        //                        oDato.Add("NombreCatalogServicio", sNombreServicio);
        //                        oDato.Add("NombreCatalogServicioTipo", oServ.CodigoTipo);
        //                        oDato.Add("CoreProductCatalogID", lCatalogoID);
        //                        oDato.Add("Simbolo", sSimbolo);
        //                        oDato.Add("EsAsignado", bAsign);
        //                        lista.Add(oDato);
        //                    }

        //                }
        //            }

        //            if (storeServiciosAsignados != null && lista != null)
        //            {
        //                e.Total = total;

        //                storeProductCatalogServicios.DataSource = lista;
        //                storeProductCatalogServicios.DataBind();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //            MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
        //        }
        //    }
        //}

        //private List<Data.Vw_CoreProductCatalogServiciosAsignados> ListaServicios()
        //{
        //    List<Data.Vw_CoreProductCatalogServiciosAsignados> listadatos;
        //    try
        //    {
        //        CoreProductCatalogServiciosAsignadosController mControl = new CoreProductCatalogServiciosAsignadosController();
        //        long lCliID = long.Parse(hdCliID.Value.ToString());
        //        long catalogoID = long.Parse(hdProductCatalogID.Value.ToString());
        //        listadatos = mControl.GetAllServiciosAsigadosByCatalogoID(lCliID, catalogoID);
        //        listadatos = listadatos.OrderBy(c => c.NombreCatalogServicio).OrderBy(c => c.NombreCatalogServicioTipo).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        listadatos = null;
        //    }
        //    return listadatos;
        //}
        //#endregion

        //#region STORE Servicios Asignados

        //protected void storeServiciosAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {

        //            int pageSize = Convert.ToInt32(cmbNumRegistros2.Value);
        //            int curPage = e.Page - 1;
        //            int total;
        //            string sFiltro = e.Parameters["filter"];

        //            List<JsonObject> lista;
        //            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

        //            lista = cServicios.AplicarFiltroInternoByPackID(pageSize, curPage, out total, e.Sort, sFiltro, hdStringBuscador2, hdIDCatalogoBuscador2, false, gridServiciosCatalogos.ColumnModel, "", long.Parse(hdProductCatalogID.Value.ToString()));

        //            if (storeServiciosAsignados != null && lista != null)
        //            {
        //                e.Total = total;
        //                hdTotalCountGrid2.SetValue(total);

        //                storeServiciosAsignados.DataSource = lista;
        //                storeServiciosAsignados.DataBind();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //            MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
        //        }
        //    }
        //}

        //#endregion


        //#region CORE FRECUENCIAS

        //protected void storeCoreFrecuencias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            List<Data.CoreFrecuencias> listaFrecuencias;
        //            CoreFrecuenciasController cFrecuencias = new CoreFrecuenciasController();

        //            listaFrecuencias = cFrecuencias.getFrecuenciasActivas();

        //            if (listaFrecuencias != null)
        //            {
        //                storeCoreFrecuencias.DataSource = listaFrecuencias;
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion

        //#region CORE UNIDADES

        //protected void storeCoreUnidades_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        //{
        //    if (RequestManager.IsAjaxRequest)
        //    {
        //        try
        //        {
        //            List<Data.CoreUnidades> listaUnidades;
        //            CoreUnidadesController cUnits = new CoreUnidadesController();

        //            listaUnidades = cUnits.getUnidadesActivas();

        //            if (listaUnidades != null)
        //            {
        //                storeCoreUnidades.DataSource = listaUnidades;
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message);
        //            string codTit = Util.ExceptionHandler(ex);
        //        }
        //    }
        //}

        //#endregion


        #region DIRECTMETHOD

        #region PRODUCTCATALOG
        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ProductDTO> APIClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
            //BaseAPIClient<ProductTypeDTO> APIClientType = new BaseAPIClient<ProductTypeDTO>(TOKEN_API);
            ProductDTO oDato;
            //ProductTypeDTO oDatoType;


            try
            {
                if (!bAgregar)
                {
                    string sCode = hdProductCatalogID.Value.ToString();
                    oDato = APIClient.GetByCode(sCode).Result.Value;
                    oDato.SupplierCompany = new List<string>();

                    var originalCode = oDato.Code;

                    oDato.Name = txtNombre.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Description = txtDescripcion.Text;

                    foreach (var code in cmbEntidad.SelectedItems)
                    {
                        oDato.SupplierCompany.Add(code.Value.ToString());
                    }

                    oDato.IsPack = true;

                    oDato.ProductTypeCode = cmbProductCatalogTipo.Value.ToString();

                    oDato.Unit = txtunidad.Text;

                    var Result = APIClient.UpdateEntity(originalCode, oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                        hdProductCatalogID.SetValue(oDato.Code);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
                    }
                }
                else
                {
                    oDato = new ProductDTO();

                    oDato.SupplierCompany = new List<string>();

                    oDato.Name = txtNombre.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Description = txtDescripcion.Text;

                    foreach (var code in cmbEntidad.SelectedItems)
                    {
                        oDato.SupplierCompany.Add(code.Value);
                    }

                    oDato.ProductTypeCode = cmbProductCatalogTipo.Value.ToString();
                    oDato.IsPack = true;

                    oDato.Unit = txtunidad.Text;
                    var Result = APIClient.AddEntity(oDato).Result;

                    if (Result.Success)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                        hdProductCatalogID.SetValue(oDato.Code);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = Result.Errors[0].Message;
                        return direct;
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
            direct.Result = "";

            return direct;
        }

        //[DirectMethod()]
        //public DirectResponse AgregarEditar(bool bAgregar)
        //{
        //    DirectResponse direct = new DirectResponse();
        //    CoreProductCatalogsPacksController cProduct = new CoreProductCatalogsPacksController();
        //    long lCliID = long.Parse(hdCliID.Value.ToString());
        //    InfoResponse oResponse;

        //    try
        //    {
        //        if (!bAgregar)
        //        {
        //            long lS = long.Parse(hdProductCatalogID.Value.ToString());
        //            Data.CoreProductCatalogPacks oDato;
        //            oDato = cProduct.GetItem(lS);

        //            oDato.Nombre = txtNombre.Text;
        //            oDato.Codigo = txtCodigo.Text;
        //            oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
        //            oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
        //            oDato.FechaModificacion = DateTime.Now;
        //            oDato.UsuarioID = Usuario.UsuarioID;
        //            oResponse = cProduct.Update(oDato);

        //            if (oResponse.Result)
        //            {
        //                oResponse = cProduct.SubmitChanges();
        //                if (oResponse.Result)
        //                {
        //                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //                    storePrincipal.DataBind();
        //                }
        //                else
        //                {
        //                    cProduct.DiscardChanges();
        //                    direct.Success = false;
        //                    direct.Result = GetGlobalResource(oResponse.Description);
        //                    return direct;
        //                }

        //            }
        //            else
        //            {
        //                cProduct.DiscardChanges();
        //                direct.Success = false;
        //                direct.Result = GetGlobalResource(oResponse.Description);
        //                return direct;
        //            }
        //            hdProductCatalogID.SetValue(oDato.CoreProductCatalogPackID);
        //        }
        //        else
        //        {

        //            CoreProductCatalogPacks oDato = new CoreProductCatalogPacks();

        //            oDato.Nombre = txtNombre.Text;
        //            oDato.Codigo = txtCodigo.Text;
        //            oDato.EntidadID = long.Parse(cmbEntidad.Value.ToString());
        //            oDato.FechaModificacion = DateTime.Now;
        //            oDato.UsuarioID = Usuario.UsuarioID;

        //            //oDato.FechaInicioVigencia = DateTime.Parse(txtFechaInicio.Value.ToString());
        //            //if (txtFechaFin.SelectedValue != null)
        //            //{
        //            //    oDato.FechaFinVigencia = DateTime.Parse(txtFechaFin.Value.ToString());
        //            //}
        //            oResponse = cProduct.Add(oDato);
        //            if (oResponse.Result)
        //            {
        //                oResponse = cProduct.SubmitChanges();
        //                if (oResponse.Result)
        //                {
        //                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
        //                    storePrincipal.DataBind();
        //                }
        //                else
        //                {
        //                    cProduct.DiscardChanges();
        //                    direct.Success = false;
        //                    direct.Result = GetGlobalResource(oResponse.Description);
        //                    return direct;
        //                }

        //            }
        //            else
        //            {
        //                cProduct.DiscardChanges();
        //                direct.Success = false;
        //                direct.Result = GetGlobalResource(oResponse.Description);
        //                return direct;
        //            }

        //            hdProductCatalogID.SetValue(oDato.CoreProductCatalogPackID);

        //            //if (oDato != null)
        //            //{
        //            //    hdProductCatalogID.SetValue(oDato.CoreProductCatalogPackID);
        //            //    log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

        //            //    if (hdCondicionCatalogoReglaID.Value.ToString() != "")
        //            //    {
        //            //        GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
        //            //        if (!cCondicionesConfiguraciones.ActualizarUltimoCodigoByReglaID(long.Parse(hdCondicionCatalogoReglaID.Value.ToString()), txtCodigo.Text))
        //            //        {
        //            //            direct.Success = false;
        //            //            direct.Result = GetLocalResourceObject("strErrorActualizarCodigoAutomatico").ToString();
        //            //            return direct;
        //            //        }
        //            //    }
        //            //}


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        direct.Success = false;
        //        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
        //        log.Error(ex.Message);
        //        return direct;
        //    }

        //    direct.Success = true;
        //    direct.Result = "";

        //    return direct;
        //}

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                string oCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<ProductDTO> ApiClient = new BaseAPIClient<ProductDTO>(TOKEN_API);
                var oDato = ApiClient.GetByCode(oCode).Result;
                txtNombre.Text = oDato.Value.Name;
                txtCodigo.Text = oDato.Value.Code;
                txtDescripcion.Text = oDato.Value.Description;
                txtunidad.Text = oDato.Value.Unit;
                winGestion.Show();
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
        public DirectResponse Eliminar()
        {

            DirectResponse direct = new DirectResponse();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            CoreProductCatalogsPacksController cProduct = new CoreProductCatalogsPacksController();
            CoreProductCatalogServiciosPacksAsignadosController cAsignados = new CoreProductCatalogServiciosPacksAsignadosController();

            List<CoreProductCatalogServiciosPacksAsignados> lista = new List<CoreProductCatalogServiciosPacksAsignados>();
            lista = cAsignados.GetAllServiciosByPack(lID);

            if (lista != null)
            {
                foreach (CoreProductCatalogServiciosPacksAsignados oDato in lista)
                {
                    cAsignados.Delete(oDato);
                }
            }

            try
            {
                if (cProduct.DeleteItem(lID))
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

            return direct;
        }


        [DirectMethod()]
        public DirectResponse AgregarServicio(bool check)
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosPacksAsignadosController cProduct = new CoreProductCatalogServiciosPacksAsignadosController();
            long lCliID = 0;
            long servicio;
            CoreProductCatalogServiciosPacksAsignados servicioAsignado;
            long producCatalog;
            double precio;


            try
            {
                InfoResponse oResponse = null;
                lCliID = long.Parse(hdCliID.Value.ToString());
                servicio = long.Parse(hdServicioID.Value.ToString());
                producCatalog = long.Parse(hdProductCatalogID.Value.ToString());
                servicioAsignado = cProduct.getServicioAsignadoIDByCatalogoIDYServicioID(producCatalog, servicio);

                CoreProductCatalogServiciosPacksAsignados dato;
                if (check)
                {
                    if (servicioAsignado == null)
                    {
                        dato = new CoreProductCatalogServiciosPacksAsignados();
                        dato.CoreProductCatalogServicioID = servicio;
                        dato.CoreProductCatalogPackID = producCatalog;
                        oResponse = cProduct.Add(dato);
                    }
                    //else
                    //{
                    //    dato = cProduct.GetItem(servicioAsignado);
                    //    cProduct.Update(dato);

                    //}
                }
                else
                {
                    oResponse = cProduct.Delete(servicioAsignado);
                }

                if (oResponse.Result)
                {
                    oResponse = cProduct.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                    else
                    {
                        cProduct.DiscardChanges();
                    }

                }
                else
                {
                    cProduct.DiscardChanges();
                    direct.Success = false;
                    direct.Result = oResponse.Description;
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

        [DirectMethod]
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsPacksController cCatalogos = new CoreProductCatalogsPacksController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cCatalogos.AplicarFiltroInterno(-1, -1, out total, null, null, hdStringBuscador, hdIDCatalogoBuscador, false, gridCatalogos.ColumnModel, "");
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
        public DirectResponse GetDatosBuscador2()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cServicios.AplicarFiltroInternoByCatalogoID(-1, -1, out total, null, null, hdStringBuscador2, hdIDCatalogoBuscador2, false, gridServiciosCatalogos.ColumnModel, "", long.Parse(hdProductCatalogID.Value.ToString()));

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
        public DirectResponse GetDatosBuscador3()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                int pageSize = 100;
                int total = 0;

                Data.Vw_CoreProductCatalogPacks oCat = new Vw_CoreProductCatalogPacks();
                CoreProductCatalogServiciosPacksAsignados oAsign = new CoreProductCatalogServiciosPacksAsignados();
                List<Data.Vw_CoreProductCatalogServicios> listaServicios = new List<Vw_CoreProductCatalogServicios>();

                CoreProductCatalogsPacksController cCatg = new CoreProductCatalogsPacksController();
                CoreProductCatalogServiciosPacksAsignadosController cServiciosAsign = new CoreProductCatalogServiciosPacksAsignadosController();
                CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

                long lCatalogoID = 0;
                string sSimbolo = "";
                string sNombreServicio = "";
                string sIdentificador = "";
                List<JsonObject> lista = new List<JsonObject>();

                listaServicios = cServicios.AplicarFiltroInternoPack(pageSize, 0, out total, null, "", hdStringBuscador3, hdIDCatalogoBuscador3, false, gridWinServicios.ColumnModel, "");

                foreach (Data.Vw_CoreProductCatalogServicios oServ in listaServicios)
                {
                    bool bAsign = false;
                    sNombreServicio = oServ.Nombre;

                    oCat = cCatg.getVistaByID(long.Parse(hdProductCatalogID.Value.ToString()));

                    if (oCat != null)
                    {
                        //sIdentificador = oServ.Identificador;
                        sSimbolo = sIdentificador;

                        long lServicioAsignID = cServiciosAsign.getValorByValoresID(oServ.CoreProductCatalogServicioID, oCat.CoreProductCatalogPackID);

                        if (lServicioAsignID != 0)
                        {
                            oAsign = cServiciosAsign.GetItem(lServicioAsignID);

                            if (oAsign != null)
                            {
                                bAsign = true;
                                lCatalogoID = long.Parse(hdProductCatalogID.Value.ToString());
                            }

                            JsonObject oDato = new JsonObject();
                            oDato.Add("CoreProductCatalogServicioID", oAsign.CoreProductCatalogServicioID);
                            oDato.Add("NombreCatalogServicio", sNombreServicio);
                            oDato.Add("CoreProductCatalogID", lCatalogoID);
                            oDato.Add("NombreCatalogServicioTipo", oServ.CodigoTipo);
                            oDato.Add("Simbolo", sSimbolo);
                            oDato.Add("EsAsignado", bAsign);
                            lista.Add(oDato);
                        }
                        else
                        {
                            JsonObject oDato = new JsonObject();
                            oDato.Add("CoreProductCatalogServicioID", oServ.CoreProductCatalogServicioID);
                            oDato.Add("NombreCatalogServicio", sNombreServicio);
                            oDato.Add("CoreProductCatalogID", lCatalogoID);
                            oDato.Add("NombreCatalogServicioTipo", oServ.CodigoTipo);
                            oDato.Add("Simbolo", sSimbolo);
                            oDato.Add("EsAsignado", bAsign);
                            lista.Add(oDato);
                        }

                    }
                }
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


        [DirectMethod()]
        public DirectResponse GenerarCodigoCatalogo()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            GlobalCondicionesReglasController cCondicionesReglasController = new GlobalCondicionesReglasController();
            GlobalCondicionesReglasConfiguracionesController cCondicionesConfiguraciones = new GlobalCondicionesReglasConfiguracionesController();
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            long lCliID = 0;

            try
            {
                lCliID = long.Parse(((Hidden)X.GetCmp("hdCliID")).Value.ToString());

                Data.GlobalCondicionesReglas aplicarRegla = cCondicionesReglasController.GetReglaByCampoDestino("CODIGO_CATALOG", (long)Comun.Modulos.GLOBAL);

                if (aplicarRegla != null)
                {
                    configuraciones = cCondicionesConfiguraciones.GlobalCondicionesReglasConfiguracionesBySeleccionadoID(aplicarRegla.GlobalCondicionReglaID);

                    if (configuraciones != null && configuraciones.Count > 0)
                    {
                        string siguienteCodigo;
                        siguienteCodigo = cCondicionesConfiguraciones.GetSiguienteByListaCondicionesReglasConfiguraciones(configuraciones, aplicarRegla.UltimoGenerado, aplicarRegla.Modificada, lCliID);

                        txtCodigo.SetValue(siguienteCodigo);

                        hdCodigoCatalogoAutogenerado.SetValue(siguienteCodigo);
                        hdCondicionCatalogoReglaID.SetValue(aplicarRegla.GlobalCondicionReglaID);

                        JsonObject listaIDs = new JsonObject();
                        direct.Result = cCondicionesReglasController.getConfiguracionRegla(aplicarRegla.GlobalCondicionReglaID, listaIDs);
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                        return direct;
                    }

                }
                //else
                //{
                //    direct.Success = false;
                //    direct.Result = GetGlobalResource(Comun.strGeneracionSinRegla);
                //    return direct;
                //}
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strGeneracionCodigoFallida);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ComprobarCatalogoExiste()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsController cCatalogo = new CoreProductCatalogsController();
            long cliID = long.Parse(hdCliID.Value.ToString());
            try
            {
                CoreProductCatalogs oDato = cCatalogo.GetItem(long.Parse(hdProductCatalogID.Value.ToString()));

                if (oDato != null)
                {
                    string sCodigo = oDato.Codigo;

                    if (sCodigo == txtCodigo.Text)
                    {
                        direct.Success = false;
                        direct.Result = "Editado";
                    }
                    else
                    {
                        CoreProductCatalogs oValor = cCatalogo.GetItem("Codigo ==" + sCodigo);

                        if (oValor != null)
                        {
                            hdCodigoCatalogoDuplicado.SetValue("Duplicado");
                            direct.Success = false;
                            direct.Result = "Codigo";
                            return direct;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ComprobarCodigoCatalogoDuplicado()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            Data.GlobalCondicionesReglas aplicarRegla;
            List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            try
            {
                #region COMPROBAR CODIGO
                if (cEmplazamientos.CodigoDuplicadoGeneradorCodigos(hdCodigoCatalogoDuplicado.Value.ToString()))
                {

                    hdCodigoCatalogoDuplicado.Value = "Duplicado";
                }
                else
                {
                    hdCodigoCatalogoDuplicado.Value = "No_Duplicado";
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
        #endregion


        #endregion



    }


}