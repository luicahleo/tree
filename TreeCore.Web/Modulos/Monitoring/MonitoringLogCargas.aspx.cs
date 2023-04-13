using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Data.SqlClient;
using System.Reflection;

namespace TreeCore.ModMonitoring
{
    public partial class MonitoringLogCargas : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        static DateTime dateFechaInicio = DateTime.MinValue;
        static DateTime dateFechaFin = DateTime.MinValue;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                
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

            #region DESCARGA FICHERO

            if ((Request.QueryString["DescargarLog"] != null))
            {

                string pathEntera = Request.QueryString["DescargarLog"];

                DescargaXLS(pathEntera);
            }

            #endregion

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.Vw_MonitoringCargasMasivas> listaDatos;
                        string sAux = Request.QueryString["aux"];
                        string sAux3 = Request.QueryString["aux3"];

                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        if (sAux3 == "null")
                        {
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, sAux);
                        }
                        else
                        {
                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, sAux3);
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
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                dateFechaInicio = DateTime.MinValue;
                dateFechaFin = DateTime.MinValue;
            }
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_MonitoringCargasMasivas> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, string sFecha = "" )
        {
            List<Data.Vw_MonitoringCargasMasivas> listaDatos;
            MonitoringCargasMasivasController cMonitoringCargasMasivas = new MonitoringCargasMasivasController();

            try
            {
                string sFechaFiltro = "";
                if (sFecha == "")
                {
                    sFechaFiltro = cmpFiltro.FiltrarFecha;
                }
                else
                {
                    sFechaFiltro = cmpFiltro.FechaFiltrada(sFecha);
                }

                string[] sLista = sFechaFiltro.Split('/');
                string sQueryFecha = "FechaCarga > DateTime(" + sLista[2] + "," + sLista[1] + ", " + sLista[0] + ")";

                listaDatos = cMonitoringCargasMasivas.GetItemsWithExtNetFilterList<Data.Vw_MonitoringCargasMasivas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, sQueryFecha);
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            MonitoringCargasMasivasController CMonitoringCargasMasivas = new MonitoringCargasMasivasController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CMonitoringCargasMasivas.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CMonitoringCargasMasivas = null;
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
        public DirectResponse DescargaXLS(string sRutaOrigen)
        {
            DirectResponse direct = new DirectResponse();
            MonitoringLogCargas cFlujos = new MonitoringLogCargas();

            try
            {
                if (File.Exists(sRutaOrigen))
                {
                    string Filepath = "";
                    string nombrearchivo = sRutaOrigen.Substring(sRutaOrigen.LastIndexOf('\\') + 1);

                    Filepath = TreeCore.DirectoryMapping.GetDocumentDirectory();

                    string sRutaDest = Path.Combine(Filepath, nombrearchivo);

                    File.Copy(sRutaOrigen, sRutaDest, true);

                    if (File.Exists(sRutaDest))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(sRutaDest);

                        Response.Clear();
                        Response.ContentType = Comun.GetMimeType(file.Extension);

                        Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.TransmitFile(sRutaDest);
                        Response.Flush();
                    }

                    Response.SuppressContent = true;

                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                    if (System.IO.File.Exists(sRutaDest))
                    {
                        File.Delete(sRutaDest);
                    }
                }

                else
                {
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
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

        #endregion
    }
}