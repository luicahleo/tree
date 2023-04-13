using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;


namespace TreeCore.ModMonitoring
{
    public partial class MonitoringServiciosWeb : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        static DateTime statfechaini = DateTime.MinValue;
        static DateTime statfechafin = DateTime.MinValue;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

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
                string sOpcion = Request.QueryString["opcion"];
                string sAux = Request.QueryString["aux"];
                string sAux3 = Request.QueryString["aux3"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.Vw_MonitoringWSRegistros> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        if (sAux3 == "null")
                        {
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID, sAux);
                        }
                        else
                        {
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, Convert.ToInt64(sAux3), sAux);
                        }


                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", paginaJS, _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.MONITORING), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };
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
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    if (hdCliID.Value.Equals("0"))
                    {
                        var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, cmpFiltro.ClienteID);
                        if (lista != null)
                        {
                            storePrincipal.DataSource = lista;

                            PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                            temp.Total = iCount;
                        }
                    }

                    else
                    {
                        var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));
                        if (lista != null)
                        {
                            storePrincipal.DataSource = lista;

                            PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_MonitoringWSRegistros> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID, string filtroFecha = "")
        {
            List<Data.Vw_MonitoringWSRegistros> listaDatos;
            MonitoringWSRegistrosController cMonitoringWSRegistros = new MonitoringWSRegistrosController();

            string fecha;
            if (filtroFecha == "")
            {
                fecha = cmpFiltro.FiltrarFecha;
            }
            else
            {
                fecha = cmpFiltro.FechaFiltrada(filtroFecha);
            }

            string[] listaP = fecha.Split('/');
            string queryFecha = " && " + "FechaCreacion > DateTime(" + listaP[2] + "," + listaP[1] + ", " + listaP[0] + ")";

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cMonitoringWSRegistros.GetItemsWithExtNetFilterList<Data.Vw_MonitoringWSRegistros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID ==" + lClienteID + queryFecha);
                }
                else
                {
                    listaDatos = null;
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

        #region SERVICIOS

        protected void storeServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ToolConexionesController cConexiones = new ToolConexionesController();
                try
                {

                    List<Data.Vw_ToolConexionesIntegraciones> lista = null;
                    lista = cConexiones.GetConexionesPorClienteID(Usuario.ClienteID);
                    if (lista != null)
                    {
                        storeServicios.DataSource = lista;
                    }

                    cConexiones = null;

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

        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse MostrarParametros(bool entrada, long servicioID)
        {
            MonitoringWSRegistrosController cControler = new MonitoringWSRegistrosController();
            DirectResponse direct = new DirectResponse();

            try
            {
                TreeCore.Data.MonitoringWSRegistros servicio = cControler.GetItem(servicioID);

                if (servicio != null)
                {
                    if (entrada)
                    {
                        if (servicio != null && servicio.ParametrosEntrada != null)
                        {
                            txaParametro.Text = servicio.ParametrosEntrada;
                        }
                        else
                        {
                            txaParametro.Text = "";
                        }
                    }
                    else
                    {
                        if (servicio != null && servicio.ParametrosSalida != null)
                        {
                            txaParametro.Text = servicio.ParametrosSalida;
                        }
                        else
                        {
                            txaParametro.Text = "";
                        }
                    }
                }

                FormPanelParametro.ValidateRequestMode = System.Web.UI.ValidateRequestMode.Disabled;
                winParametros.ValidateRequestMode = System.Web.UI.ValidateRequestMode.Disabled;
                winParametros.Show();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }

            direct.Success = true;
            direct.Result = "";
            return direct;

        }

        #endregion

    }
}