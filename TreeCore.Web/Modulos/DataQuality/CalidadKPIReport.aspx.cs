using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModCalidad.pages
{


    public partial class CalidadKPIReport : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, GridPanelSideL.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(GridPanelSideL, storePrincipal, GridPanelSideL.ColumnModel, listaIgnore, _Locale);
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
                    //cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "CalidadKPI.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { btnSemaforo, btnKPI, btnDetalleKPI }},
            { "Delete", new List<ComponentBase> { }}
        };

        }


        #region Stores
        private List<object> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lCategoriaID)
        {
            DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();
            DQKpisController cDQKpis = new DQKpisController();
            List<object> listaDatos = new List<object>();

            try
            {
                List<Vw_DQKpisMonitoring> lista = cDQKpisMonitoring.GetUltimosKPIs(lCategoriaID);
                lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, 0, 0, ref iCount);

                lista.ForEach(dqkpiMonitoring =>
                {
                    string rutaPagina = cDQKpis.GetRutaPagina(dqkpiMonitoring.DQKpiID);
                    listaDatos.Add(new
                    {
                        RutaPagina = rutaPagina,
                        DQKpiMonitoringID = dqkpiMonitoring.DQKpiMonitoringID,
                        DQKpi = dqkpiMonitoring.DQKpi,
                        DQKpiID = dqkpiMonitoring.DQKpiID,
                        Version = dqkpiMonitoring.Version,
                        Ultima = dqkpiMonitoring.Ultima,
                        Activa = dqkpiMonitoring.Activa,
                        FechaEjecucion = dqkpiMonitoring.FechaEjecucion,
                        DQCategoriaID = dqkpiMonitoring.DQCategoriaID,
                        NumeroElementos = dqkpiMonitoring.NumeroElementos,
                        Total = dqkpiMonitoring.Total
                    });
                });

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

            return listaDatos;
        }

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


                    long? categoryID = null;
                    if (cmbDQCategorias.SelectedItem.Value != null)
                    {
                        categoryID = long.Parse(cmbDQCategorias.SelectedItem.Value);
                    }
                    
                    List<object> lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, categoryID);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;
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

        protected void storeDQCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    DQCategoriasController cDQCategorias = new DQCategoriasController();
                    List<DQCategorias> categorias = cDQCategorias.GetAllActivos(long.Parse(hdCliID.Value.ToString()));


                    if (categorias != null)
                    {
                        storeDQCategorias.DataSource = categorias;
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

        public void storeGrupos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            DQGroupsMonitoringController cDQGroupsMonitoring = new DQGroupsMonitoringController();

            try
            {
                long DQKpiID = long.Parse(hdDQKpiID.Value.ToString());

                List<object> lista = cDQGroupsMonitoring.GetUltimosByKPI(DQKpiID, 3);

                if (lista != null)
                {
                    storeGrupos.DataSource = lista;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }
        }

        public void storeKpisMonitoring_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();

            try
            {
                long DQKpiID = long.Parse(hdDQKpiID.Value.ToString());

                List<Vw_DQKpisMonitoring> lista = cDQKpisMonitoring.getByKPIActivos(DQKpiID);

                if (lista != null)
                {
                    storeKpisMonitoring.DataSource = lista;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

        }

        public void storeUltimosKpisMonitoring_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            DQKpisMonitoringController cDQKpisMonitoring = new DQKpisMonitoringController();

            try
            {
                long DQKpiID = long.Parse(hdDQKpiID.Value.ToString());

                List<Vw_DQKpisMonitoring> lista = cDQKpisMonitoring.GetUltimosKPIs(DQKpiID);

                if (lista != null)
                {
                    storeUltimosKpisMonitoring.DataSource = lista;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }

        }
        #endregion

        #region DIRECT METHOD

        #endregion

    }

}