using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using TreeCore.Clases;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.ModInventario
{
    public partial class InvInicio : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        const int TOP_CATEGORIAS = 8;
        const int TOP_OPERADORES = 8;
        const int TOP_ESTADOS_OPERACIONALES = 8;
        const string CREACION = "CREACION";
        const string MODIFICACION = "MODIFICACION";
        const string TOTALES_CATEGORIA = "TOTALES_CATEGORIA";

        List<long> operadoresID = new List<long>(/*new long[] { 10065, 10066, 10071}*/);
        List<long> categoriasID = new List<long>(/*new long[] { 10121, 10123, 10125 }*/);
        List<long> estadosOperacionalesID = new List<long>(/*new long[] { 1, 3 }*/);

        #region EVENTOS DE PAGINA
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ClienteID.HasValue)
            {
                hdCliID.Value = 0;
            }
            else
            {
                hdCliID.Value = ClienteID;
            }

            #region EXPORTAR 



            #endregion

            #region Configuracion Home
            hdTOP_CATEGORIAS.SetValue(TOP_CATEGORIAS);
            hdTOP_OPERADORES.SetValue(TOP_OPERADORES);
            hdTOP_ESTADOS_OPERACIONALES.SetValue(TOP_ESTADOS_OPERACIONALES);

            GetConfiguracionHome();
            #endregion
        }

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                List<string> pathsOfScripts = new List<string>();
                pathsOfScripts.Add(Comun.BundleConfigPaths.CONTENT_JS_CHART);

                ResourceManagerOperaciones(ResourceManagerTreeCore, pathsOfScripts, new List<string>());

                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                if (listaFuncionalidades.Contains((long)Comun.ModFun.INV_InventarioInicio_Total))
                {
                    btnConfig.Hidden = false;
                }
                else
                {
                    btnConfig.Hidden = true;
                }
            }
        }

        #endregion

        #region STORE

        #region storeInventarioElementosCategorias
        protected void storeInventarioElementosCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreInventarioHistoricosController cCoreInventarioHistoricos = new CoreInventarioHistoricosController();

                try
                {
                    DateTime fechaIncio = GetDateInitCharts();
                    List<InventarioHistorico> lista = cCoreInventarioHistoricos.GetHistoricoByTopCategories(TOP_CATEGORIAS, categoriasID, fechaIncio);

                    if (lista != null)
                    {
                        storeInventarioElementosCategorias.DataSource = lista;
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

        #region storeInventarioElementosOperadores
        protected void storeInventarioElementosOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosController cInventarioElementos = new InventarioElementosController();
                try
                {
                    List<ItemInvInicio> lista = cInventarioElementos.GetElementosByTopOperadores(TOP_OPERADORES, operadoresID);

                    if (lista != null)
                    {
                        storeInventarioElementosOperadores.DataSource = lista;
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

        #region storeInventarioElementosEstadosOperacionales
        protected void storeInventarioElementosEstadosOperacionales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosController cInventarioElementos = new InventarioElementosController();
                try
                {
                    List<ItemInvInicio> lista = cInventarioElementos.GetElementosByTopEstadosOperacionales(TOP_ESTADOS_OPERACIONALES, estadosOperacionalesID);

                    if (lista != null)
                    {
                        storeInventarioElementosEstadosOperacionales.DataSource = lista;
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

        #region storeInventarioElementosTotales
        protected void storeInventarioElementosTotales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreInventarioHistoricosController cCoreInventarioHistoricos = new CoreInventarioHistoricosController();

                try
                {
                    DateTime fechaIncio = GetDateInitCharts();
                    List<InventarioHistorico> lista = cCoreInventarioHistoricos.GetHistoricoTotal(fechaIncio);
                    
                    if (lista != null)
                    {
                        storeInventarioElementosTotales.DataSource = lista;
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

        #region WinConfigCharts

        #region storeOperadoresForm
        protected void storeOperadoresForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                OperadoresController cOperadores = new OperadoresController();

                try
                {
                    List<Operadores> lista = cOperadores.GetAllOperadores(ClienteID.Value);

                    if (lista != null)
                    {
                        storeOperadoresForm.DataSource = lista;
                        storeOperadoresForm.DataBind();

                        cmbOperadores.SetValue(operadoresID.ToArray());
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

        #region storeCategoriasForm
        protected void storeCategoriasForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioCategoriasController cInventarioCategorias = new InventarioCategoriasController();
                try
                {
                    List<Data.InventarioCategorias> lista = cInventarioCategorias.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeCategoriasForm.DataSource = lista;
                        storeCategoriasForm.DataBind();
                        
                        cmbCategorias.SetValue(categoriasID.ToArray());
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

        #region storeEstadosOperacionalesForm
        protected void storeEstadosOperacionalesForm_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                InventarioElementosAtributosEstadosController cInventarioElementosAtributosEstados = new InventarioElementosAtributosEstadosController();

                try
                {
                    List<Data.InventarioElementosAtributosEstados> lista = cInventarioElementosAtributosEstados.GetActivos(ClienteID.Value);

                    if (lista != null)
                    {
                        storeEstadosOperacionalesForm.DataSource = lista;
                        storeEstadosOperacionalesForm.DataBind();

                        cmbEstadosOperacionales.SetValue(estadosOperacionalesID.ToArray());
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

        #endregion

        #region DIRECT METHODS

        #region InventarioCategoriasKPI
        [DirectMethod()]
        public DirectResponse InventarioCategoriasKPI(long categoryID)
        {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            CoreInventarioHistoricosController cCoreInventarioHistoricos = new CoreInventarioHistoricosController();
            #endregion

            try
            {
                DateTime fechaIncio = GetDateInitCharts();
                List<InventarioHistorico> items = cCoreInventarioHistoricos.GetHistoricoByCategory(fechaIncio, categoryID);



                direct.Success = true;
                direct.Result = items;
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

        #region GetTopCategories
        [DirectMethod()]
        public DirectResponse GetTopCategories(int top)
        {
            DirectResponse direct = new DirectResponse();

            #region Controllers
            InventarioElementosController cInventarioElementos = new InventarioElementosController();
            #endregion

            try
            {
                List<long> ids = cInventarioElementos.GetTopCategories(top, categoriasID);

                direct.Result = ids;
                direct.Success = true;
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

        #region GuardarConfiguracionHome
        [DirectMethod()]
        public DirectResponse GuardarConfiguracionHome()
        {
            DirectResponse direct = new DirectResponse();

            CoreConfiguracionHomeModulosController cCoreConfiguracionHomeModulos = new CoreConfiguracionHomeModulosController();
            ModulosController cModulos = new ModulosController();

            try
            {
                string pagina = getThisPage();
                
                categoriasID = (string.IsNullOrEmpty((string)cmbCategorias.Value)) ? new List<long>() : ListItemCollectionToListLong(cmbCategorias.SelectedItems);
                operadoresID = (string.IsNullOrEmpty((string)cmbOperadores.Value)) ? new List<long>() : ListItemCollectionToListLong(cmbOperadores.SelectedItems);
                estadosOperacionalesID = (string.IsNullOrEmpty((string)cmbEstadosOperacionales.Value)) ? new List<long>() : ListItemCollectionToListLong(cmbEstadosOperacionales.SelectedItems);

                ConfiguracionHomeModulo configHome = new ConfiguracionHomeModulo()
                {
                    categoriasIDs = categoriasID,
                    operadoresIDs = operadoresID,
                    estadosOperacionalesIDs = estadosOperacionalesID
                };

                CoreConfiguracionHomeModulos objConfig = cCoreConfiguracionHomeModulos.GetConfiguracionByModulo(ClienteID.Value, pagina);
                Data.Modulos modulo = cModulos.getModuloByPagina(pagina);

                string configString = JsonConvert.SerializeObject(configHome);

                if (objConfig != null)
                {
                    objConfig.Configuracion = configString;

                    cCoreConfiguracionHomeModulos.UpdateItem(objConfig);
                }
                else
                {

                    objConfig = new CoreConfiguracionHomeModulos()
                    {
                        Activo = true,
                        ClienteID = ClienteID.Value,
                        ModuloID = modulo.ModuloID,
                        Configuracion = configString
                    };

                    cCoreConfiguracionHomeModulos.AddItem(objConfig);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            return direct;
        }
        #endregion

        #endregion

        #region FUNCTIONS

        #region GetDateInitCharts
        private DateTime GetDateInitCharts()
        {
            DateTime hoy = DateTime.Now;
            DateTime fechaInicio = DateTime.Today;
            int valor = int.Parse(cmbRangoTiempo.SelectedItem.Value.ToString());

            fechaInicio = hoy.AddMonths(valor);

            return fechaInicio;
        }
        #endregion

        #region GetConfiguracionHome
        public void GetConfiguracionHome()
        {
            CoreConfiguracionHomeModulosController cCoreConfiguracionHomeModulos = new CoreConfiguracionHomeModulosController();

            try
            {
                CoreConfiguracionHomeModulos configuracionHome = cCoreConfiguracionHomeModulos.GetConfiguracionByModulo(ClienteID.Value, getThisPage());

                if (configuracionHome != null)
                {
                    ConfiguracionHomeModulo config = JsonConvert.DeserializeObject<ConfiguracionHomeModulo>(configuracionHome.Configuracion);

                    if (config != null)
                    {
                        if (config.categoriasIDs != null)
                        {
                            categoriasID = config.categoriasIDs;
                        }
                        if (config.operadoresIDs != null)
                        {
                            operadoresID = config.operadoresIDs;
                        }
                        if (config.estadosOperacionalesIDs != null)
                        {
                            estadosOperacionalesID = config.estadosOperacionalesIDs;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region ListItemCollectionToListLong
        private List<long> ListItemCollectionToListLong(ListItemCollection sLista)
        {
            List<long> lista = new List<long>();

            try
            {
                foreach(var a in sLista)
                {
                    lista.Add(long.Parse(a.Value));   
                }
            }
            catch(Exception ex)
            {
                lista = null;
                log.Error(ex.Message);
            }

            return lista;
        }
        #endregion

        #endregion
    }

    #region CLASSES
    class kpiItem
    {
        public long InventarioCategoriaID { get; set; }
        public long id { get; set; }
        public string title { get; set; }
        public long value { get; set; }
        public List<kpiData> data { get; set; }
    }

    class kpiData {
        public long y { get; set; }
        public DateTime x { get; set; }
    }
    #endregion

}