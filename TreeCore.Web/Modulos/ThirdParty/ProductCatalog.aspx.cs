using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.Page;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.PaginasComunes
{
    public partial class ProductCatalog : TreeCore.Page.BasePageExtNet
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;

        #region PAGE MANAGEMENT
        private void Page_Init(object sender, EventArgs e)
        {

            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridCatalogos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                Comun.CreateGridFilters(gridFiltersServicios, storeServiciosAsignados, gridServiciosCatalogos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            //long CliID = long.Parse(Request.QueryString["cliente"]);
                            bool bActivo = Request.QueryString["aux"] == "true";
                            int iCount = 0;
                            string sGrid = Request.QueryString["aux3"];
                            if (sGrid == "1")
                            {
                                string sOrden1 = Request.QueryString["orden"];
                                string sDir1 = Request.QueryString["dir"];
                                string sCliente = Request.QueryString["cliente"];
                                string sFiltro1 = Request.QueryString["filtro"];
                                string sTextoBuscado = Request.QueryString["aux4"];
                                string sIdBuscado = Request.QueryString["aux5"];
                                string sEsPack = Request.QueryString["aux6"];
                                long sMaestroID = long.Parse(Request.QueryString["aux"].ToString());
                                bool bDescarga = true;
                                string sVariablesExcluidas = "CoreProductCatalogID, CoreProductCatalogServicioAsignadoID, CoreProductCatalogServicioTipoID, CoreProductCatalogServicioID, ";
                                int total = 0;

                                hdStringBuscador2.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                                hdIDCatalogoBuscador2.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                                List<JsonObject> lista = new List<JsonObject>();
                                Vw_CoreProductCatalogs oCat = new Vw_CoreProductCatalogs();
                                List<CoreProductCatalogPacksAsignados> listaPackAsign = new List<CoreProductCatalogPacksAsignados>();
                                CoreProductCatalogsPacksController cPack = new CoreProductCatalogsPacksController();
                                CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
                                CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();
                                CoreProductCatalogPacksAsignadosController cPacks = new CoreProductCatalogPacksAsignadosController();
                                CoreProductCatalogServiciosAsignadosController cServAsign = new CoreProductCatalogServiciosAsignadosController();
                                List<Vw_CoreProductCatalogPacksAsignados> listaPacksAsign = new List<Vw_CoreProductCatalogPacksAsignados>();

                                lista = cServicios.AplicarFiltroInternoByCatalogoID(-1, -1, out total, null, sFiltro1, hdStringBuscador2, hdIDCatalogoBuscador2, bDescarga, gridServiciosCatalogos.ColumnModel, sVariablesExcluidas, sMaestroID);

                                oCat = cCatalogos.getVistaByID(sMaestroID);
                                listaPacksAsign = cPacks.GetItemsList<Vw_CoreProductCatalogPacksAsignados>();

                                if (oCat != null)
                                {
                                    foreach (Vw_CoreProductCatalogPacksAsignados oPack in listaPacksAsign)
                                    {
                                        listaPackAsign = cPacks.getPackByCatalogoID(sMaestroID);

                                        foreach (CoreProductCatalogPacksAsignados oPackAsign in listaPackAsign)
                                        {
                                            if (oPackAsign != null && oPackAsign.CoreProductCatalogPackAsignadoID == oPack.CoreProductCatalogPackAsignadoID
                                            && (hdIDCatalogoBuscador2.Value == null || (sEsPack != null && sEsPack == "true")))
                                            {
                                                if (hdStringBuscador2.Value.ToString() == "" || (oPack.Nombre.IndexOf(hdStringBuscador2.Value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0))
                                                {
                                                    string sSimbolo = oPack.Moneda + " / " + oPack.Identificador;
                                                    string sPrecio = oPack.Precio + " " + sSimbolo;

                                                    CoreProductCatalogPacks oP = cPack.GetItem(oPackAsign.CoreProductCatalogPackID);

                                                    if (oP != null)
                                                    {
                                                        JsonObject oDato = new JsonObject();
                                                        oDato.Add("CoreProductCatalogID", oCat.CoreProductCatalogID);
                                                        oDato.Add("CodigoProductCatalog", oCat.Codigo);
                                                        oDato.Add("NombreProductCatalog", oCat.NombreProductCatalog);
                                                        oDato.Add("NombreCatalogServicio", oPack.Nombre);
                                                        oDato.Add("CantidadCatalogServicio", "");
                                                        oDato.Add("Precio", sPrecio);
                                                        oDato.Add("Moneda", oPack.Moneda);
                                                        oDato.Add("Simbolo", sSimbolo);
                                                        oDato.Add("Nombre", oP.Nombre);
                                                        oDato.Add("Codigo", oP.Codigo);
                                                        oDato.Add("FechaModificacion", oP.FechaModificacion.ToString(Comun.FORMATO_FECHA));
                                                        oDato.Add("CoreProductCatalogPackID", oPackAsign.CoreProductCatalogPackID);
                                                        oDato.Add("CoreProductCatalogServicioTipoID", "");
                                                        oDato.Add("CoreProductCatalogServicioID", "");
                                                        oDato.Add("NombreCatalogServicioTipo", "");
                                                        oDato.Add("Tarea", false);
                                                        oDato.Add("CodigoProductCatalogServicio", "");
                                                        oDato.Add("Identificador", oPack.Identificador);
                                                        oDato.Add("esPack", true);

                                                        lista.Add(oDato);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                int i = 1;
                                foreach (JsonObject oValor in lista)
                                {
                                    oValor.Add("CoreProductCatalogServiceID", i);

                                    if (!oValor["Precio"].ToString().Contains("/"))
                                    {
                                        Vw_CoreProductCatalogServiciosAsignados oServicio = cServAsign.getItemByNombreServicio(oValor["NombreCatalogServicio"].ToString());

                                        if (oServicio != null)
                                        {
                                            //string sPrecio = oValor["Precio"] + " " + oServicio.Simbolo + " / " + oServicio.Identificador;

                                            oValor.Remove("Precio");
                                            //oValor.Add("Precio", sPrecio);
                                        }
                                    }

                                    i++;
                                }

                                if (sEsPack == "true")
                                {
                                    JsonObject oResultado = new JsonObject();
                                    object oResult;
                                    bool bResult = false;

                                    oResultado = lista.Find(x =>
                                    {
                                        bResult = ((x.TryGetValue("CoreProductCatalogPackID", out oResult)) && (oResult.ToString() == hdIDCatalogoBuscador2.Value.ToString()));

                                        return bResult;
                                    });

                                    if (bResult)
                                    {
                                        lista.Clear();
                                        lista.Add(oResultado);
                                    }
                                }

                                #region ESTADISTICAS
                                try
                                {
                                    Comun.ExportacionDesdeListaNombreTemplate(gridServiciosCatalogos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource("strServicio").ToString(), Comun.DefaultLocale);

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
                                string sFiltro1 = Request.QueryString["filtro"];
                                //string sTextoBuscado = Request.QueryString["aux4"];
                                //string sIdBuscado = Request.QueryString["aux5"];
                                //bool bDescarga = true;
                                string sVariablesExcluidas = "CoreProductCatalogID, EntidadID, ClienteID, CoreProductCatalogTipoID, CoreReajustePrecioID, ";
                                //int total = 0;
                                int pageSize = Convert.ToInt32(cmbNumRegistros.Value);

                                QueryDTO qFilter = QueryWeb.ParseFilterDTO(sFiltro1, sOrden, sDir1, null, pageSize, -1);

                                BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);
                                var lista = APIClient.GetList(qFilter).Result;

                                //hdStringBuscador.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                                //hdIDCatalogoBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                                //List<JsonObject> lista;
                                //CoreProductCatalogsController cProductos = new CoreProductCatalogsController();

                                //lista = cProductos.AplicarFiltroInterno(-1, -1, out total, null, sFiltro1, hdStringBuscador, hdIDCatalogoBuscador, bDescarga, gridCatalogos.ColumnModel, sVariablesExcluidas);

                                #region ESTADISTICAS
                                try
                                {
                                    Comun.ExportacionDesdeListaNombreTemplate(gridCatalogos.ColumnModel, lista.Value, Response, sVariablesExcluidas, GetGlobalResource("strCatalogos").ToString(), Comun.DefaultLocale);

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
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar}},
            { "Delete", new List<ComponentBase> { btnEliminar }}
            };
        }

        #endregion

        #region STORES

        #region CATALOG
        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    string sSort, sDir = null;
                    int curPage = e.Page;
                    int pageSize = Convert.ToInt32(cmbNumRegistros.Value);
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();

                    QueryDTO qFilter = QueryWeb.ParseFilterDTO(e.Parameters["filter"], sSort, sDir, null, pageSize, curPage);

                    BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);
                    var lista = APIClient.GetList(qFilter).Result;

                    if (lista != null)
                    {
                        e.Total = lista.TotalItems;
                        hdTotalCountGrid.SetValue(lista.TotalItems);

                        hdStore.SetValue(JsonConvert.SerializeObject(lista));
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

        #endregion

        #region CURRENCY

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CurrencyDTO> APIClient = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
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

        #endregion

        #region LIFECYCLE STATUS

        protected void storeEstadosGlobales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CatalogLifecycleStatusDTO> APIClient = new BaseAPIClient<CatalogLifecycleStatusDTO>(TOKEN_API);
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
                    BaseAPIClient<CatalogDTO> APIClientCatalog = new BaseAPIClient<CatalogDTO>(TOKEN_API);
                    BaseAPIClient<CatalogAssignedProductsDTO> APIClientCatalogAssign = new BaseAPIClient<CatalogAssignedProductsDTO>(TOKEN_API);
                    var listaServicios = APIClient.GetList(qFilter).Result;

                    CatalogDTO oCat = new CatalogDTO();
                    List<JsonObject> lista = new List<JsonObject>();

                    oCat = APIClientCatalog.GetByCode(hdProductCatalogID.Value.ToString()).Result.Value;

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
                            oDato.Add("Currency", oCat.CurrencyCode);

                            if (oCat.LinkedProducts != null)
                            {
                                foreach (CatalogAssignedProductsDTO link in oCat.LinkedProducts)
                                {
                                    if (link.ProductCode == oServ.Code)
                                    {
                                        oDato.Add("Price", link.Price);
                                        bAsign = true;
                                    }
                                }
                            }
                            else
                            {
                                oDato.Add("Price", 0);
                            }

                            oDato.Add("EsAsignado", bAsign);
                            lista.Add(oDato);
                        }
                    }
                    else
                    {
                        foreach (ProductDTO oServ in listaServicios.Value)
                        {
                            bool bAsign = false;

                            JsonObject oDato = new JsonObject();
                            oDato.Add("CodeProduct", oServ.Code);
                            oDato.Add("Name", oServ.Name);
                            oDato.Add("ProductTypeCode", oServ.ProductTypeCode);
                            oDato.Add("CodeCatalog", "");
                            oDato.Add("Currency", cmbMonedas.Value.ToString());
                            oDato.Add("Price", 0);
                            oDato.Add("EsAsignado", bAsign);
                            lista.Add(oDato);
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

        #region SERVICIOS ASIGNADOS

        protected void storeServiciosAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);
                    BaseAPIClient<ProductDTO> APIClientProduct = new BaseAPIClient<ProductDTO>(TOKEN_API);
                    List<Filter> listFilters = new List<Filter>();
                    List<JsonObject> listaFinal = new List<JsonObject>();

                    string sSort, sDir = null;
                    int pageSize = Convert.ToInt32(cmbNumRegistros2.Value);
                    int curPage = e.Page - 1;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    string sFiltro = e.Parameters["filter"];

                    CatalogDTO cat = APIClient.GetByCode(hdProductCatalogID.Value.ToString()).Result.Value; 

                    if (cat != null && cat.LinkedProducts != null && cat.LinkedProducts.Count > 0)
                    {
                        foreach(CatalogAssignedProductsDTO prod in cat.LinkedProducts)
                        {
                            Filter fDTO = new Filter(nameof(ProductDTO.Code), nameof(Operators.eq), prod.ProductCode, Filter.Types.OR, null);
                            listFilters.Add(fDTO);
                        }

                        QueryDTO qFilter = QueryWeb.ParseFilterDTO(sFiltro, sSort, sDir, listFilters, pageSize, curPage);
                        var lista = APIClientProduct.GetList(qFilter).Result;

                        int i = 0;
                        foreach (ProductDTO oServ in lista.Value)
                        {
                            JsonObject oDato = new JsonObject();
                            oDato.Add("CodeProduct", oServ.Code);
                            oDato.Add("Price", cat.LinkedProducts[i].Price);
                            oDato.Add("Currency", cat.CurrencyCode);
                            listaFinal.Add(oDato);
                            i++;
                        }

                        if (listaFinal != null)
                        {
                            e.Total = listaFinal.Count();
                            hdTotalCountGrid2.SetValue(listaFinal.Count());

                            store.DataSource = listaFinal;
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

        #region TYPE

        protected void storeProductCatalogTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Store store = (Store)sender;
                try
                {
                    BaseAPIClient<CatalogTypeDTO> APIClient = new BaseAPIClient<CatalogTypeDTO>(TOKEN_API);
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

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);
            CatalogDTO oDato;

            try
            {
                if (!bAgregar)
                {
                    string sCode = GridRowSelect.SelectedRecordID;
                    oDato = APIClient.GetByCode(sCode).Result.Value;

                    var originalCode = oDato.Code;

                    oDato.Name = txtNombre.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Description = txtDescripcion.Text;

                    oDato.CatalogTypeCode = cmbProductCatalogTipo.Value.ToString();
                    oDato.CurrencyCode = cmbMonedas.Value.ToString();
                    oDato.LifecycleStatusCode = cmbEstadosGlobales.Value.ToString();

                    if (txtFechaFin.SelectedValue != null)
                    {
                        oDato.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                    }

                    oDato.PricesReadjustment = new Shared.DTO.ValueObject.PriceReadjustmentDTO();

                    if (oDato.PricesReadjustment != null)
                    {
                        switch (cmpReajustes.Tipo)
                        {
                            case 1:
                                cmpReajustes.HdRadio = 1;
                                oDato.PricesReadjustment.Type = PReadjustment.sWithoutIncrements;
                                break;
                            case 2:
                                oDato.PricesReadjustment.Type = PReadjustment.sPCI;
                                oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;

                                if (cmpReajustes.FechaProxima.Ticks > 0)
                                {
                                    oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                                }
                                else
                                {
                                    oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                                }
                                if (cmpReajustes.ControlFechaFin)
                                {
                                    oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                                else
                                {
                                    oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                                }

                                oDato.PricesReadjustment.CodeInflation = cmpReajustes.Inflacion;

                                cmpReajustes.HdRadio = 2;
                                break;
                            case 3:
                                oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;
                                oDato.PricesReadjustment.Type = PReadjustment.sFixedAmount;

                                if (cmpReajustes.ControlFechaFin)
                                {
                                    if (txtFechaFin.SelectedValue != null)
                                    {
                                        oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                                    }
                                }
                                else
                                {
                                    oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                                }

                                if (cmpReajustes.FechaProxima.Ticks > 0)
                                {
                                    oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                                }
                                else
                                {
                                    oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                                }

                                oDato.PricesReadjustment.Frequency = cmpReajustes.Periodicidad;
                                oDato.PricesReadjustment.FixedAmount = Convert.ToSingle(cmpReajustes.CantidadFija);

                                cmpReajustes.HdRadio = 3;

                                break;
                            case 4:
                                oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;
                                oDato.PricesReadjustment.Type = PReadjustment.sFixedPercentege;

                                if (cmpReajustes.ControlFechaFin)
                                {
                                    if (txtFechaFin.SelectedValue != null)
                                    {
                                        oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                                    }
                                }
                                else
                                {
                                    oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                                }

                                if (cmpReajustes.FechaProxima.Ticks > 0)
                                {
                                    oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                                }
                                else
                                {
                                    oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                                }

                                oDato.PricesReadjustment.Frequency = cmpReajustes.Periodicidad;
                                oDato.PricesReadjustment.FixedPercentage = Convert.ToSingle(cmpReajustes.PorcentajeFijo);

                                cmpReajustes.HdRadio = 4;

                                break;
                        }
                    }

                    if (hdServicioAsignadoID.Value.ToString() != "" && hdServicioAsignadoID.Value != null)
                    {
                        oDato.LinkedProducts = new List<CatalogAssignedProductsDTO>();

                        foreach (string sObj in hdServicioAsignadoID.Value.ToString().Split(','))
                        {
                            if (sObj != "")
                            {
                                string[] sVariables = sObj.Split(':');

                                CatalogAssignedProductsDTO assigned = new CatalogAssignedProductsDTO
                                {
                                    ProductCode = sVariables[0],
                                    Price = Convert.ToSingle(sVariables[1])
                                };

                                oDato.LinkedProducts.Add(assigned);
                            }
                        }
                    }

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
                    oDato = new CatalogDTO();

                    oDato.Name = txtNombre.Text;
                    oDato.Code = txtCodigo.Text;
                    oDato.Description = txtDescripcion.Text;

                    oDato.CatalogTypeCode = cmbProductCatalogTipo.Value.ToString();
                    oDato.CurrencyCode = cmbMonedas.Value.ToString();
                    oDato.LifecycleStatusCode = cmbEstadosGlobales.Value.ToString();

                    oDato.StartDate = DateTime.Parse(txtFechaInicio.Value.ToString());
                    if (txtFechaFin.SelectedValue != null)
                    {
                        oDato.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                    }

                    oDato.PricesReadjustment = new Shared.DTO.ValueObject.PriceReadjustmentDTO();

                    switch (cmpReajustes.Tipo)
                    {
                        case 1:
                            cmpReajustes.HdRadio = 1;
                            oDato.PricesReadjustment.Type = PReadjustment.sWithoutIncrements;
                            break;
                        case 2:
                            oDato.PricesReadjustment.Type = PReadjustment.sPCI;
                            oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;

                            if (cmpReajustes.FechaProxima.Ticks > 0)
                            {
                                oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                            }
                            else
                            {
                                oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                            }
                            if (cmpReajustes.ControlFechaFin)
                            {
                                oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                            }
                            else
                            {
                                oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                            }

                            oDato.PricesReadjustment.CodeInflation = cmpReajustes.Inflacion;

                            cmpReajustes.HdRadio = 2;
                            break;
                        case 3:
                            oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;
                            oDato.PricesReadjustment.Type = PReadjustment.sFixedAmount;

                            if (cmpReajustes.ControlFechaFin)
                            {
                                if (txtFechaFin.SelectedValue != null)
                                {
                                    oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                            }
                            else
                            {
                                oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                            }

                            if (cmpReajustes.FechaProxima.Ticks > 0)
                            {
                                oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                            }
                            else
                            {
                                oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                            }

                            oDato.PricesReadjustment.Frequency = cmpReajustes.Periodicidad;
                            oDato.PricesReadjustment.FixedAmount = Convert.ToSingle(cmpReajustes.CantidadFija);

                            cmpReajustes.HdRadio = 3;

                            break;
                        case 4:
                            oDato.PricesReadjustment.StartDate = cmpReajustes.FechaInicio;
                            oDato.PricesReadjustment.Type = PReadjustment.sFixedPercentege;

                            if (cmpReajustes.ControlFechaFin)
                            {
                                if (txtFechaFin.SelectedValue != null)
                                {
                                    oDato.PricesReadjustment.EndDate = DateTime.Parse(txtFechaFin.Value.ToString());
                                }
                            }
                            else
                            {
                                oDato.PricesReadjustment.EndDate = cmpReajustes.FechaFin;
                            }

                            if (cmpReajustes.FechaProxima.Ticks > 0)
                            {
                                oDato.PricesReadjustment.NextDate = cmpReajustes.FechaProxima;
                            }
                            else
                            {
                                oDato.PricesReadjustment.NextDate = oDato.PricesReadjustment.StartDate;
                            }

                            oDato.PricesReadjustment.Frequency = cmpReajustes.Periodicidad;
                            oDato.PricesReadjustment.FixedPercentage = Convert.ToSingle(cmpReajustes.PorcentajeFijo);

                            cmpReajustes.HdRadio = 4;

                            break;
                    }

                    if (hdServicioAsignadoID.Value.ToString() != "" && hdServicioAsignadoID.Value != null)
                    {
                        oDato.LinkedProducts = new List<CatalogAssignedProductsDTO>();

                        foreach (string sObj in hdServicioAsignadoID.Value.ToString().Split(','))
                        {
                            if (sObj != "")
                            {
                                string[] sVariables = sObj.Split(':');

                                CatalogAssignedProductsDTO assigned = new CatalogAssignedProductsDTO
                                {
                                    ProductCode = sVariables[0],
                                    Price = Convert.ToSingle(sVariables[1])
                                };

                                oDato.LinkedProducts.Add(assigned);
                            }
                        }
                    }

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

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                string sCode = GridRowSelect.SelectedRecordID;
                BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);

                var oDato = APIClient.GetByCode(sCode).Result.Value;
                txtNombre.Text = oDato.Name;
                txtCodigo.Text = oDato.Code;
                txtDescripcion.Text = oDato.Description;

                cmbProductCatalogTipo.SetValue(oDato.CatalogTypeCode);
                cmbMonedas.SetValue(oDato.CurrencyCode);
                cmbEstadosGlobales.SetValue(oDato.LifecycleStatusCode);

                txtFechaInicio.SetValue(oDato.StartDate);
                //cmpReajustes.FechaInicio = oDato.FechaInicioVigencia.Value;

                if (oDato.EndDate != null)
                {
                    txtFechaFin.SetValue(oDato.EndDate);
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<CatalogDTO> APIClient = new BaseAPIClient<CatalogDTO>(TOKEN_API);

            try
            {
                var lID = GridRowSelect.SelectedRecordID;
                var Result = APIClient.DeleteEntity(lID).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
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

        [DirectMethod]
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();
            CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();

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

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();

                Vw_CoreProductCatalogs oCat = new Vw_CoreProductCatalogs();
                CoreProductCatalogsPacksController cPack = new CoreProductCatalogsPacksController();
                List<CoreProductCatalogPacksAsignados> listaPackAsign = new List<CoreProductCatalogPacksAsignados>();
                CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
                CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();
                CoreProductCatalogsPacksController cPacks = new CoreProductCatalogsPacksController();
                CoreProductCatalogPacksAsignadosController cPacksAsign = new CoreProductCatalogPacksAsignadosController();
                List<Vw_CoreProductCatalogPacks> listaPacksAsign = new List<Vw_CoreProductCatalogPacks>();

                lista = cServicios.AplicarFiltroInternoByCatalogoID(-1, -1, out total, null, null, hdStringBuscador2, hdIDCatalogoBuscador2, false, gridServiciosCatalogos.ColumnModel, "", long.Parse(hdProductCatalogID.Value.ToString()));

                oCat = cCatalogos.getVistaByID(long.Parse(hdProductCatalogID.Value.ToString()));
                listaPacksAsign = cPacks.GetItemsList<Vw_CoreProductCatalogPacks>();

                if (oCat != null)
                {
                    foreach (Vw_CoreProductCatalogPacks oPack in listaPacksAsign)
                    {
                        listaPackAsign = cPacksAsign.getPackByCatalogoID(long.Parse(hdProductCatalogID.Value.ToString()));

                        foreach (CoreProductCatalogPacksAsignados oPackAsign in listaPackAsign)
                        {
                            if (oPackAsign != null && oPackAsign.CoreProductCatalogPackID == oPack.CoreProductCatalogPackID)
                            {
                                string sSimbolo = oCat.Moneda + " / " + oPack.Identificador;

                                CoreProductCatalogPacks oP = cPack.GetItem(oPackAsign.CoreProductCatalogPackID);

                                if (oP != null)
                                {
                                    JsonObject oDato = new JsonObject();
                                    oDato.Add("CoreProductCatalogID", oCat.CoreProductCatalogID);
                                    oDato.Add("CodigoProductCatalog", oCat.Codigo);
                                    oDato.Add("NombreProductCatalog", oCat.NombreProductCatalog);
                                    oDato.Add("NombreCatalogServicio", oPack.Nombre);
                                    oDato.Add("CantidadCatalogServicio", "");
                                    oDato.Add("Precio", oPackAsign.Precio);
                                    oDato.Add("Moneda", oCat.Moneda);
                                    oDato.Add("Simbolo", sSimbolo);
                                    oDato.Add("Nombre", oP.Nombre);
                                    oDato.Add("Codigo", oP.Codigo);
                                    oDato.Add("FechaModificacion", oP.FechaModificacion.ToString(Comun.FORMATO_FECHA));
                                    oDato.Add("CoreProductCatalogPackID", oPackAsign.CoreProductCatalogPackID);
                                    oDato.Add("CoreProductCatalogServicioTipoID", "");
                                    oDato.Add("CoreProductCatalogServicioID", "");
                                    oDato.Add("NombreCatalogServicioTipo", "");
                                    oDato.Add("Tarea", false);
                                    oDato.Add("CodigoProductCatalogServicio", "");
                                    oDato.Add("Identificador", oPack.Identificador);
                                    oDato.Add("esPack", true);

                                    lista.Add(oDato);
                                }
                            }
                        }
                    }
                }

                int i = 1;
                foreach (JsonObject oValor in lista)
                {
                    oValor.Add("CoreProductCatalogServiceID", i);
                    i++;
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
        [DirectMethod]
        public DirectResponse GetDatosBuscador3()
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                int pageSize = 100;
                int total = 0;

                Data.Vw_CoreProductCatalogs oCat = new Vw_CoreProductCatalogs();
                CoreProductCatalogServiciosAsignados oAsign = new CoreProductCatalogServiciosAsignados();
                List<Data.Vw_CoreProductCatalogServicios> listaServicios = new List<Vw_CoreProductCatalogServicios>();

                CoreProductCatalogsController cCatg = new CoreProductCatalogsController();
                CoreProductCatalogServiciosAsignadosController cServiciosAsign = new CoreProductCatalogServiciosAsignadosController();
                CoreProductCatalogServiciosController cServicios = new CoreProductCatalogServiciosController();

                long lCatalogoID = 0;
                string sSimbolo = "";
                string sNombreServicio = "";
                string sIdentificador = "";
                List<JsonObject> lista = new List<JsonObject>();

                listaServicios = cServicios.AplicarFiltroInternoCatalogo(pageSize, 0, out total, null, "", hdStringBuscador3, hdIDCatalogoBuscador3, false, gridWinServicios.ColumnModel, "");

                foreach (Data.Vw_CoreProductCatalogServicios oServ in listaServicios)
                {
                    int iPrecio = 0;
                    bool bAsign = false;
                    string sMoneda = "";
                    sNombreServicio = oServ.Nombre;

                    oCat = cCatg.getVistaByID(long.Parse(hdProductCatalogID.Value.ToString()));

                    if (oCat != null)
                    {
                        //sIdentificador = oServ.Identificador;
                        sMoneda = oCat.Moneda;
                        sSimbolo = sMoneda + " / " + sIdentificador;

                        long lServicioAsignID = cServiciosAsign.getValorByValoresID(oServ.CoreProductCatalogServicioID, oCat.CoreProductCatalogID);

                        if (lServicioAsignID != 0)
                        {
                            oAsign = cServiciosAsign.GetItem(lServicioAsignID);

                            if (oAsign != null)
                            {
                                bAsign = true;
                                iPrecio = Convert.ToInt32(oAsign.Precio);
                                lCatalogoID = long.Parse(hdProductCatalogID.Value.ToString());
                            }

                            JsonObject oDato = new JsonObject();
                            oDato.Add("CoreProductCatalogServicioID", oAsign.CoreProductCatalogServicioID);
                            oDato.Add("NombreCatalogServicio", sNombreServicio);
                            oDato.Add("Precio", iPrecio);
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
                            oDato.Add("Precio", iPrecio);
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
        public DirectResponse ComprobarCodigoCatalogoDuplicado()
        {
            DirectResponse direct = new DirectResponse();
            direct.Success = true;
            direct.Result = "";

            //EmplazamientosController cEmplazamientos = new EmplazamientosController();
            //Data.GlobalCondicionesReglas aplicarRegla;
            //List<Data.GlobalCondicionesReglasConfiguraciones> configuraciones;

            try
            {
                //#region COMPROBAR CODIGO
                //if (cEmplazamientos.CodigoDuplicadoGeneradorCodigos(hdCodigoCatalogoDuplicado.Value.ToString()))
                //{

                //    hdCodigoCatalogoDuplicado.Value = "Duplicado";
                //}
                //else
                //{
                //    hdCodigoCatalogoDuplicado.Value = "No_Duplicado";
                //}
                //#endregion

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