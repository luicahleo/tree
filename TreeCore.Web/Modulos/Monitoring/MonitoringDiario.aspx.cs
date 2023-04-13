using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Data.SqlClient;
using TreeCore.Clases;
using TreeCore.Data;
using Tree.Linq.GenericExtensions;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Globalization;

namespace TreeCore.ModMonitoring
{
    public partial class MonitoringDiario : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);
                string Fechainicio = DateTime.Today.AddDays(-1).ToShortDateString();
                string FechaFin = DateTime.Today.ToShortDateString();

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
                string sAux = Request.QueryString["aux"];
                string sAux3 = Request.QueryString["aux3"];
                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<EmplazamientosSeguimientosGenerico> listaDatos;
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

        #endregion

        private List<EmplazamientosSeguimientosGenerico> ListaPrincipal(int start, int limit, string sort, string dir, ref int count, string filtro, long? lClienteID, string filtroFecha = "")
        {
            #region  Controladores Seguimientos (Genérico y resto)

            //List<Data.Vw_Estadisticas> datos = new List<Data.Vw_Estadisticas>();
            List<EmplazamientosSeguimientosGenerico> lista = new List<EmplazamientosSeguimientosGenerico>();

            #endregion
            string fecha;
            if (filtroFecha == "")
            {
                fecha = cmpFiltro.FiltrarFecha;
            }
            else
            {
                fecha = cmpFiltro.FechaFiltrada(filtroFecha);
            }
            try
            {
                //string filter = "";

                long CliID = 0;

                //string aux = Request.QueryString["aux"];
                if (ClienteID != null)
                {
                    CliID = ClienteID.Value;
                    //filter = "ClienteID == " + ClienteID.Value;
                }
                else
                {
                    if (lClienteID.Value != 0)
                    {
                        CliID = Convert.ToInt64(lClienteID);
                    }


                    if (hdCliID.Value != null && hdCliID.Value.ToString() != "" && lClienteID.Value == 0)
                    {

                        CliID = cmpFiltro.ClienteID;

                    }
                }



                if (CliID > 0)
                {
                    long? proyectotipov = null;
                    long? usuariov = null;
                    string ProyectoTipo = null;

                    CultureInfo culture = new CultureInfo("es-ES");
                    DateTime FInicio = Convert.ToDateTime(fecha, culture);
                    DateTime FFin = Convert.ToDateTime(DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), culture);

                    if (FInicio != null && FFin != null && FInicio != DateTime.MinValue && FFin != DateTime.MinValue)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                //throw ex;
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                lista = null;
            }

            string[] listaP = fecha.Split('/');
            string queryFecha = "FechaHoraInicio > DateTime(" + listaP[2] + "," + listaP[1] + ", " + listaP[0] + ")";

            return Clases.LinqEngine.PagingItemsListWithExtNetFilter(lista, null, "", sort, dir, start, limit, ref count);


        }

        #endregion




    }
}