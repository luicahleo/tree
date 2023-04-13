using System;
using System.IO;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace TreeCore.ModMonitoring
{
    public partial class MonitoringLogServicio : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades;
        public string sCarpeta;

        public class FicheroLogs
        {
            public int LogID { get; set; }
            public string nombrefichero { get; set; }
            public DateTime fechaultima { get; set; }
            public string tipo { get; set; }
            public string tipoFichero { get; set; }
            public string ruta { get; set; }
        }

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                //Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                //log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                //Registro de Estadistica
                if (!IsPostBack && !RequestManager.IsAjaxRequest)
                {
                    Data.ProyectosTipos ptip = new Data.ProyectosTipos();
                    ProyectosTiposController cProyTip = new ProyectosTiposController();
                    ptip = cProyTip.GetProyectosTiposByNombre(Comun.MODULOMONITORING);
                    Util.EscribeEstadistica(Usuario.UsuarioID, ptip.ProyectoTipoID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, "");
                }

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                hdCarpeta.Value = "DEBUG";
                if (Request.QueryString["hdCarpeta"] != null)
                {
                    sCarpeta = Request.QueryString["hdCarpeta"];
                    hdCarpeta.Value = sCarpeta;
                }
            }

            #region DESCARGAR LOG

            if (Request.QueryString["DescargarLog"] != null)
            {
                string filePath = sCarpeta + "\\" + Request.QueryString["DescargarLog"];

                if (File.Exists(filePath))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                    if (file.Length != 0)
                    {

                        Response.Clear();
                        Response.ContentType = Comun.GetMimeType(file.Extension);
                        Response.AddHeader("content-disposition", "attachment; filename=" + (file.Directory.Name + "_" + file.Name));
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.TransmitFile(filePath);
                        Response.Flush();

                        Response.SuppressContent = true;
                        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            #endregion

            #region EXCEL

            if (Request.QueryString["opcion"] != null)
            {
                if (Request.Params["aux4"] != null)
                {
                    sCarpeta = Request.Params["aux4"];
                    hdCarpeta.Value = sCarpeta.ToString();
                }

                List<FicheroLogs> listaDatos;
                string sAux = Request.QueryString["aux"];
                string sAux3 = Request.QueryString["aux3"];
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        if (sAux3 == "null")
                        {
                            listaDatos = ListaPrincipal(sAux);
                        }
                        else
                        {
                            listaDatos = ListaPrincipal(sAux3);
                        }

                        Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strLogsServicio), _Locale);
                        log.Info(GetGlobalResource(Comun.LogExcelExportado));
                        EstadisticasController cEstadisticas = new EstadisticasController();
                        cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.MONITORING), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
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
            try
            {
                var lista = ListaPrincipal();

                if (lista != null)
                {
                    storePrincipal.DataSource = lista;
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        private List<FicheroLogs> ListaPrincipal(string sFecha = "")
        {
            List<FicheroLogs> listaDatos = new List<FicheroLogs>();

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

                int i = 0;

                #region ERROR

                if (btnError.Pressed || sCarpeta == "ERROR")
                {
                    List<FicheroLogs> listaDatosError = new List<FicheroLogs>();
                    DirectoryInfo directorio = new DirectoryInfo(DirectoryMapping.GetServiceImportExportLog4NetDirectoryERROR());

                    foreach (var fi in directorio.GetFiles())
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        DateTime fecha = Convert.ToDateTime(sFechaFiltro, culture);

                        if (fi.LastWriteTime > fecha)
                        {
                            FicheroLogs fLogs = new FicheroLogs();
                            fLogs.LogID = i++;
                            fLogs.nombrefichero = fi.Name;
                            fLogs.tipo = GetGlobalResource("jsError");
                            fLogs.ruta = fi.DirectoryName;
                            fLogs.fechaultima = fi.LastWriteTime;
                            fLogs.tipoFichero = GetGlobalResource("strExportarImportar");

                            listaDatosError.Add(fLogs);
                        }
                    }

                    DirectoryInfo directorioDQ = new DirectoryInfo(DirectoryMapping.GetServiceDataQualityLog4NetDirectoryERROR());

                    foreach (var fi in directorioDQ.GetFiles())
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        DateTime fecha = Convert.ToDateTime(sFechaFiltro, culture);

                        if (fi.LastWriteTime > fecha)
                        {
                            FicheroLogs fLogs = new FicheroLogs();
                            fLogs.LogID = i++;
                            fLogs.nombrefichero = fi.Name;
                            fLogs.fechaultima = fi.LastWriteTime;
                            fLogs.tipo = GetGlobalResource("jsError");
                            fLogs.ruta = fi.DirectoryName;
                            fLogs.tipoFichero = GetGlobalResource("strCalidad");

                            listaDatosError.Add(fLogs);
                        }
                    }

                    listaDatos.AddRange(listaDatosError);
                }

                #endregion

                #region INFO

                if (btnInfo.Pressed || sCarpeta == "INFO")
                {
                    List<FicheroLogs> listaDatosInfo = new List<FicheroLogs>();
                    DirectoryInfo directorio = new DirectoryInfo(DirectoryMapping.GetServiceImportExportLog4NetDirectoryINFO());

                    foreach (var fi in directorio.GetFiles())
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        DateTime fecha = Convert.ToDateTime(sFechaFiltro, culture);

                        if (fi.LastWriteTime > fecha)
                        {
                            FicheroLogs fLogs = new FicheroLogs();
                            fLogs.LogID = i++;
                            fLogs.nombrefichero = fi.Name;
                            fLogs.fechaultima = fi.LastWriteTime;
                            fLogs.tipo = GetGlobalResource("jsInfo");
                            fLogs.ruta = fi.DirectoryName;
                            fLogs.tipoFichero = GetGlobalResource("strExportarImportar");

                            listaDatosInfo.Add(fLogs);
                        }
                    }

                    DirectoryInfo directorioDQ = new DirectoryInfo(DirectoryMapping.GetServiceDataQualityLog4NetDirectoryINFO());

                    foreach (var fi in directorioDQ.GetFiles())
                    {
                        CultureInfo culture = new CultureInfo("es-ES");
                        DateTime fecha = Convert.ToDateTime(sFechaFiltro, culture);

                        if (fi.LastWriteTime > fecha)
                        {
                            FicheroLogs fLogs = new FicheroLogs();
                            fLogs.LogID = i++;
                            fLogs.nombrefichero = fi.Name;
                            fLogs.fechaultima = fi.LastWriteTime;
                            fLogs.tipo = GetGlobalResource("jsInfo");
                            fLogs.ruta = fi.DirectoryName;
                            fLogs.tipoFichero = GetGlobalResource("strCalidad");

                            listaDatosInfo.Add(fLogs);
                        }
                    }

                    listaDatos.AddRange(listaDatosInfo);
                }

                #endregion

            }

            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            #region ORDENACION

            if (listaDatos != null && listaDatos.Count > 0)
            {
                listaDatos.Sort((x, y) => y.fechaultima.CompareTo(x.fechaultima));
            }
            #endregion

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHODS

        [DirectMethod()]
        public DirectResponse TieneContenido(string sNombreFichero, string sRuta, string sTipo)
        {
            DirectResponse direct = new DirectResponse();
            string filePath = "";
            string[] sReadTexto;
            string sTexto = "";

            try
            {
                if (sRuta != "" && sNombreFichero != "")
                {
                    filePath = sRuta + "\\" + sNombreFichero;


                    if (File.Exists(filePath))
                    {
                        sReadTexto = File.ReadAllLines(filePath, Encoding.Default);

                        if (sReadTexto.Length == 0)
                        {
                            direct.Success = true;
                            direct.Result = false;
                            return direct;
                        }
                        else
                        {
                            direct.Success = true;
                            direct.Result = true;
                            return direct;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }

            direct.Success = true;
            direct.Result = sTexto;
            return direct;

        }

        [DirectMethod()]
        public DirectResponse MostrarLog(string sNombreFichero, string sRuta, string sTipo)
        {
            DirectResponse direct = new DirectResponse();
            string filePath = "";
            string[] sReadTexto;
            string sTexto = "";

            try
            {
                if (sRuta != "" && sNombreFichero != "")
                {
                    filePath = sRuta + "\\" + sNombreFichero;


                    if (File.Exists(filePath))
                    {
                        sReadTexto = File.ReadAllLines(filePath, Encoding.Default);

                        if (sReadTexto.Length == 0)
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strLogVacio);
                            return direct;
                        }
                        else
                        {
                            foreach (string sText in sReadTexto)
                            {
                                sTexto += sText + '.' + Environment.NewLine;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }

            direct.Success = true;
            direct.Result = sTexto;
            return direct;

        }

        #endregion
    }
}