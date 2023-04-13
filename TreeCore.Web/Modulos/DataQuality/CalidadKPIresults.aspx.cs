using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.Data.SqlClient;
using log4net;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace TreeCore.ModCalidad
{
    public partial class CalidadKPIresults : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region GESTION DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS
                List<string> listaIgnore = new List<string>() { "Colour" };
                Comun.CreateGridFilters(gridFilters, storeDQKpisMonitoring, gridMain1.ColumnModel, listaIgnore, _Locale);
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
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<object> listaDatos = null;
                        DQCategoriasController cCategorias = new DQCategoriasController();
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        int iCount = 0;
                        string sFiltro = Request.QueryString["filtro"];
                        string sCategoria = Request.QueryString["aux"];

                        if (sCategoria != "")
                        {
                            long lCategoriaID = cCategorias.getIDByName(sCategoria);
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCategoriaID);
                        }

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridMain1.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strCalidadKPIResults).ToString(), _Locale);
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
            sPagina = "CalidadKPI.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargarGrid }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { btnDetalleKPI, btnTime }},
            { "Delete", new List<ComponentBase> { }}
        };

        }


        #endregion

        #region STORES

        #region CATEGORIAS

        protected void storeDQCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Vw_DQKpis> listaKpis = null;
                List<DQCategorias> listaCategorias = null;
                List<Object> listaCat = new List<object>();
                Object oCat;
                DQKpisController cKpis = new DQKpisController();
                DQCategoriasController cCategorias = new DQCategoriasController();

                try
                {
                    listaCategorias = cCategorias.GetAllActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaCategorias != null)
                    {
                        oCat = new
                        {
                            ID = "1",
                            Categoria = GetGlobalResource("strTodasCategorias"),
                            Valores = "",
                        };

                        listaCat.Add(oCat);

                        foreach (DQCategorias oDato in listaCategorias)
                        {
                            listaKpis = cKpis.getKPIsActivos(long.Parse(hdCliID.Value.ToString()));
                            int iKpi = cKpis.getKpisByCategoriaID((long)oDato.DQCategoriaID);

                            oCat = new
                            {
                                ID = oDato.DQCategoriaID.ToString(),
                                Categoria = oDato.DQCategoria,
                                Valores = iKpi,
                            };

                            listaCat.Add(oCat);
                        }

                        storeDQCategorias.DataSource = listaCat;
                        storeDQCategorias.DataBind();
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

        #region KPI MONITORING

        protected void storeDQKpisMonitoring_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    DQCategoriasController cCategorias = new DQCategoriasController();
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    if (hdCategoria.Value.ToString() != "" && hdCategoria.Value.ToString() != "1")
                    {
                        long lCategoriaID = cCategorias.getIDByName(hdCategoria.Value.ToString());

                        var listaDatos = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lCategoriaID);

                        if (listaDatos != null)
                        {
                            storeDQKpisMonitoring.DataSource = listaDatos;
                            storeDQKpisMonitoring.DataBind();
                        }

                    }
                    else
                    {
                        lblTituloGrid.Text = GetGlobalResource("strTodasCategorias");
                        var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, 0);

                        if (lista != null)
                        {
                            storeDQKpisMonitoring.DataSource = lista;
                            storeDQKpisMonitoring.DataBind();
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<object> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lCategoriaID)
        {
            DQKpisMonitoringController cMonitoring = new DQKpisMonitoringController();
            List<object> listaDatos = new List<object>();
            string sBeforeLast = "";
            string sCurrent = "";
            string sLast = "";
            int versionUltima = 0;
            string sColor = "";
            int? iCuentaCurrent = 0;
            List<Vw_DQKpisMonitoring> lista = null;

            try
            {
                if (lCategoriaID != 0)
                {
                    lista = cMonitoring.getListaKPI(lCategoriaID);
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, 0, 0, ref iCount);
                }
                else
                {
                    lista = cMonitoring.getAll();
                    lista = Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, sFiltro, "", sSort, sDir, 0, 0, ref iCount);
                }

                if (lista != null)
                {
                    lista.FindAll(x => x.Ultima == true).ForEach(oDato =>
                    {

                        lista.FindAll(y => y.DQKpiID == oDato.DQKpiID).ForEach(oValor =>
                        {
                            if (oValor.Ultima)
                            {
                                if (oValor.NumeroElementos > 0)
                                {
                                    sCurrent = oValor.PorcentajeError;
                                }
                                else if (oValor.NumeroElementos == -1)
                                {
                                    sCurrent = GetGlobalResource("jsError");
                                }
                                else
                                {
                                    sCurrent = null;
                                }
                                if (sCurrent != null)
                                {
                                    if (oValor.NumeroElementos > 0)
                                    {
                                        iCuentaCurrent = ((oValor.NumeroElementos * 100) / oValor.Total);
                                    }
                                    else if (oValor.NumeroElementos == -1)
                                    {
                                        iCuentaCurrent = null;
                                    }

                                    if (iCuentaCurrent < oValor.IntervaloVerde)
                                    {
                                        sColor = "green";
                                    }
                                    else if (iCuentaCurrent > oValor.IntervaloRojo)
                                    {
                                        sColor = "red";
                                    }
                                    else if (iCuentaCurrent == null)
                                    {
                                        sColor = "error";
                                    }
                                    else
                                    {
                                        sColor = "yellow";
                                    }
                                }

                                versionUltima = oValor.Version;
                            }

                            if (versionUltima != 0 && oValor.Version == (versionUltima - 1))
                            {
                                if (oValor.NumeroElementos > 0)
                                {
                                    sLast = oValor.PorcentajeError;
                                }
                                else if (oValor.NumeroElementos == -1)
                                {
                                    sLast = GetGlobalResource("jsError");
                                }
                                else
                                {
                                    sLast = null;
                                }
                            }

                            else if (versionUltima != 0 && oValor.Version == (versionUltima - 2))
                            {
                                if (oValor.NumeroElementos > 0)
                                {
                                    sBeforeLast = oValor.PorcentajeError;
                                }
                                else if (oValor.NumeroElementos == -1)
                                {
                                    sBeforeLast = GetGlobalResource("jsError");
                                }
                                else
                                {
                                    sBeforeLast = null;
                                }
                            }

                            if (versionUltima != 0 && oValor.Version != versionUltima && lista.Count() != 1)
                            {
                                if (sCurrent == "" || sCurrent == null)
                                {
                                    sCurrent = "0%";
                                    sColor = "green";
                                }
                                if (sBeforeLast == "" || sBeforeLast == null)
                                {
                                    sBeforeLast = "0%";
                                }
                                if (sLast == "" || sLast == null)
                                {
                                    sLast = "0%";
                                }
                            }
                        });


                        Object oMonitoring = new
                        {
                            DQKpiMonitoringID = oDato.DQKpiMonitoringID,
                            DQKpi = oDato.DQKpi,
                            DQKpiID = oDato.DQKpiID,
                            BeforeLast = sBeforeLast,
                            Last = sLast,
                            Current = sCurrent,
                            Colour = sColor
                        };

                        listaDatos.Add(oMonitoring);
                    });


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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse ejecutarKPI()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisController cKPI = new DQKpisController();
            DQKpisGroupsController cGroups = new DQKpisGroupsController();

            try
            {
                string sNombre = GridRowSelectGrilla.SelectedRecordID;

                DQKpis oKPI = cKPI.getKPIByName(sNombre);
                List<long> listaDQKpisGroupsID = cGroups.GetGroupsActivos();

                if (oKPI != null && listaDQKpisGroupsID != null && Usuario.UsuarioID != 0)
                {
                    cKPI.EjecutarKPI(oKPI, listaDQKpisGroupsID, Usuario.UsuarioID);
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

        #endregion

    }
}