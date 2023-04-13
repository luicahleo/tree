using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.ModExportarImportar
{
    public partial class DataUploadGrid : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridPrincipal.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
                hdLocale.Value = _Locale.ToString();
            }

            #region DESCARGA PLANTILLA

            if ((Request.QueryString["DescargarPlantilla"] != null))
            {
                string sPathEntera = Request.QueryString["DescargarPlantilla"];

                try
                {
                    DescargaXLS(sPathEntera);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    Response.Write("ERROR: " + ex.Message);
                }

                Response.End();
            }

            #endregion

            #region DESCARGA LOG

            if (Request.QueryString["DescargarLog"] != null)
            {
                string sNombreArchivo = Request.QueryString["DescargarLog"];
                string sRuta = TreeCore.DirectoryMapping.GetImportLogDirectory();
                sRuta = Path.Combine(sRuta, sNombreArchivo);
                if (File.Exists(sRuta))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(sRuta);
                    Response.Clear();
                    Response.ContentType = Comun.GetMimeType(file.Extension);
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.TransmitFile(sRuta);
                    Response.Flush();
                }

                Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                Response.End();
            }

            #endregion

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];
                string sProyecto = Request.QueryString["aux"].ToString();

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        List<Data.Vw_DocumentosCargas> listaDatos;
                        listaDatos = ListaCargas(0, 0, sOrden, sDir, ref iCount, sFiltro, sProyecto);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridPrincipal.ColumnModel, listaDatos, Response, "", "Documentos Cargas", _Locale);
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar, btnDescargarLog, btnDescargarTemplate, ShowLog, ShowTemplate }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };
        }

        #endregion

        #region STORES

        #region DOCUMENTOS CARGAS

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ImportTaskDTO> baseAPI = new BaseAPIClient<ImportTaskDTO>(TOKEN_API);
                    var lista = baseAPI.GetList().Result;

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista.Value;
                        storePrincipal.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_DocumentosCargas> ListaCargas(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, string sProyecto)
        {
            List<Data.Vw_DocumentosCargas> listaDatos = null;
            DocumentosCargasController cCargas = new DocumentosCargasController();
            ProyectosController cProyectos = new ProyectosController();
            UsuariosPerfilesController cUser = new UsuariosPerfilesController();

            try
            {
                if (sProyecto != "" && sProyecto != "null")
                {
                    long lProyectoID = (long)cProyectos.GetProyectoID(sProyecto);
                    listaDatos = cCargas.GetItemsWithExtNetFilterList<Data.Vw_DocumentosCargas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ProyectoID == " + lProyectoID + "&& ClienteID == " + long.Parse(hdCliID.Value.ToString()));
                }
                else if (Usuario.ClienteID == null)
                {
                    listaDatos = cCargas.GetItemsWithExtNetFilterList<Data.Vw_DocumentosCargas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }
                else
                {
                    listaDatos = cCargas.GetItemsWithExtNetFilterList<Data.Vw_DocumentosCargas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + long.Parse(hdCliID.Value.ToString()));
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

        #region PROYECTOS

        protected void storeProyectos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Proyectos> listaProyectos;

                    listaProyectos = ListaProyectos();

                    if (listaProyectos != null)
                    {
                        storeProyectos.DataSource = listaProyectos;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Proyectos> ListaProyectos()
        {
            List<Data.Proyectos> listaDatos;
            ProyectosController cTipos = new ProyectosController();

            try
            {
                listaDatos = cTipos.GetAllProyectos();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #region PLANTILLAS

        protected void storePrincipalPlantillas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.DocumentosCargasPlantillas> listaPlantillas;

                    listaPlantillas = ListaPlantillas();

                    if (listaPlantillas != null)
                    {
                        storePrincipalPlantillas.DataSource = listaPlantillas;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.DocumentosCargasPlantillas> ListaPlantillas()
        {
            List<Data.DocumentosCargasPlantillas> listaDatos = null;
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();
            ProyectosController cProyectos = new ProyectosController();

            try
            {
                //if (cmbProyectosUpload.SelectedItem.Value != null && cmbProyectosUpload.SelectedItem.Value != "")
                //{
                //    long lProyectoTipoID = cProyectos.getProyectoTipoByNombre(cmbProyectosUpload.SelectedItem.Value);
                //    listaDatos = cPlantillas.getPlantillasByProyectoID(lProyectoTipoID);
                //}
                //else
                //{
                listaDatos = cPlantillas.GetAllPlantillas(true);
                //}
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #region OPERADORES

        protected void storeOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Operadores> listaOperadores;

                    listaOperadores = ListaOperadores();

                    if (listaOperadores != null)
                    {
                        storeOperadores.DataSource = listaOperadores;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Operadores> ListaOperadores()
        {
            List<Data.Operadores> listaDatos;
            OperadoresController cOperadores = new OperadoresController();

            try
            {
                listaDatos = cOperadores.GetActivos(long.Parse(hdCliID.Value.ToString()));
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
        public DirectResponse Agregar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            DocumentosCargasController cController = new DocumentosCargasController();
            ProyectosController cProyectos = new ProyectosController();
            cProyectos.SetDataContext(cController.Context);
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();
            cPlantillas.SetDataContext(cController.Context);
            ParametrosController cParametros = new ParametrosController();
            cParametros.SetDataContext(cController.Context);
            CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            cCoreServiciosFrecuencias.SetDataContext(cController.Context);

            List<string> listaCargasAPI = new List<string>() {
                "PRODUCT CATALOG SERVICES",
                "ENTITIES",
                "CONTRACTS",
                "SITES",
                "INVENTORY"
            };

            if (listaCargasAPI.Contains(cmbPlantillas.Text))
            {
                try
                {
                    direct.Success = true;
                    direct.Result = "";

                    BaseAPIClient<ImportTaskDTO> cImportTask = new BaseAPIClient<ImportTaskDTO>(TOKEN_API);
                    CronDTO oCron = new CronDTO
                    {
                        Cron = $" {tmHoraCarga.SelectedTime.Minutes} {tmHoraCarga.SelectedTime.Hours} * * *",
                        StartDate = txtFechaInicio.SelectedDate.Date,
                        EndDate = txtFechaInicio.SelectedDate.Date
                    };
                    FileDTO oFile = new FileDTO
                    {
                        Document = UploadF.FileName,
                        DocumentData = UploadF.FileBytes
                    };
                  
                    if (bAgregar)
                    {
                        if (cmbPlantillas.Text.Equals("SITES") || cmbPlantillas.Text.Equals("INVENTORY"))
                        {
                            if (cController.RegistroDuplicado(txtNombre.Text))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            }
                            else
                            {

                                Data.CoreServiciosFrecuencias oFrecuencia = new Data.CoreServiciosFrecuencias();
                                //if (ProgramadorUpload.Frecuencias == "NoSeRepite")
                                //{
                                oFrecuencia.Nombre = "Cron_" + txtNombre.Text;
                                oFrecuencia.Activo = true;

                                oFrecuencia.FechaInicio = (DateTime)txtFechaInicio.Value;
                                oFrecuencia.FechaFin = null;

                                oFrecuencia.TipoFrecuencia = "NoSeRepite";
                                oFrecuencia.CronFormat = "_";

                                oFrecuencia = cCoreServiciosFrecuencias.AddItem(oFrecuencia);

                                if (oFrecuencia != null)
                                {
                                    log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                                }


                                Data.DocumentosCargas oDato = new Data.DocumentosCargas();
                                long lPlantillaID = 0;
                                string sRuta = "";

                                oDato.DocumentoCarga = txtNombre.Text;
                                oDato.Activo = true;
                                oDato.Procesado = false;
                                oDato.Exito = false;
                                oDato.CoreServicioFrecuenciaID = oFrecuencia.CoreServicioFrecuenciaID;
                                oDato.FechaSubida = DateTime.Now;
                                oDato.UsuarioID = Usuario.UsuarioID;

                                if (Usuario.ClienteID.HasValue)
                                {
                                    oDato.ClienteID = Usuario.ClienteID;
                                }
                                else
                                {
                                    oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                                }

                                string sValor = cParametros.GetItemValor("CADENCIA_SERVICIO_IMPORT_EXPORT");
                                DateTime dtSubida = oDato.FechaSubida;
                                oDato.FechaEstimadaSubida = dtSubida.AddDays(Convert.ToInt32(sValor));

                                if (cmbPlantillas.SelectedItem.Value != null && cmbPlantillas.SelectedItem.Value != "")
                                {
                                    lPlantillaID = cPlantillas.getIDByName(cmbPlantillas.SelectedItem.Value);
                                    oDato.DocumentoCargaPlantillaID = lPlantillaID;
                                }
                                else
                                {
                                    oDato.DocumentoCargaPlantillaID = 0;
                                }

                                if (!string.IsNullOrEmpty(UploadF.FileName))
                                {
                                    sRuta = GuardarPlantilla(UploadF);

                                    if (sRuta != "")
                                    {
                                        oDato.RutaDocumento = UploadF.FileName;
                                        oDato = cController.AddItem(oDato);

                                        if (oDato != null)
                                        {
                                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                            storePrincipal.DataBind();
                                        }
                                    }


                                }
                                else
                                {
                                    oDato = cController.AddItem(oDato);

                                    if (oDato != null)
                                    {
                                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                        storePrincipal.DataBind();

                                    }
                                    else
                                    {

                                        direct.Success = false;
                                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                        return direct;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ImportTaskDTO oTask = new ImportTaskDTO
                            {
                                Code = txtNombre.Text.ToString(),
                                UploadDate = DateTime.Now,
                                ImportDate = txtFechaInicio.SelectedDate.Add(tmHoraCarga.SelectedTime),
                                Type = cmbPlantillas.Value.ToString(),
                                Document = oFile
                            };
                            var Result = cImportTask.AddEntity(oTask).Result;
                            if (Result.Success)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = Result.Errors[0].Message;
                                return direct;
                            }
                        }
                    }


                    else
                    {
                        ImportTaskDTO oTask = new ImportTaskDTO
                        {
                            Code = txtNombre.Text.ToString(),
                            UploadDate = DateTime.Now,
                            ImportDate = txtFechaInicio.SelectedDate.Add(tmHoraCarga.SelectedTime),
                            Type = cmbPlantillas.Value.ToString(),
                            Document = oFile
                        };
                        var Result = cImportTask.UpdateEntity(GridRowSelect.SelectedRecordID, oTask).Result;
                        if (Result.Success)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
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
                }

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
            BaseAPIClient<ImportTaskDTO> APIClient = new BaseAPIClient<ImportTaskDTO>(TOKEN_API);

            var lID = GridRowSelect.SelectedRecordID;

            try
            {
                var Result = APIClient.DeleteEntity(lID).Result;

                if (Result.Success)
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                }
                else
                {
                    direct.Success = false;
                    direct.Result = Result.Errors[0].Message;
                    return direct;
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
        public string GuardarPlantilla(FileUploadField file)
        {
            string sRutaFinal;
            bool bExiste = false;

            string sFn = Path.GetFileName(file.PostedFile.FileName);
            string sExt = Path.GetExtension(file.PostedFile.FileName).ToLower();
            string sArchivo = Path.GetFileNameWithoutExtension(file.PostedFile.FileName);

            string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
            sRutaFinal = Path.Combine(sRuta, sFn);

            if (sExt.StartsWith("."))
            {
                sExt = sExt.Substring(1, sExt.Length - 1);
            }

            String[] sExtensionPermitido = "pdf,doc,docx,xls,xlsx,txt,ppt,jpg,png,jpeg,kmz,msg,dwg,zip,pptx,xlsm,pst,ost,eml".Split(',');

            foreach (string sExtension in sExtensionPermitido)
            {
                if (sExtension == sExt)
                {
                    bExiste = true;
                }
            }

            if (!bExiste)
            {
                throw new Exception(GetGlobalResource(Comun.strTipoArchivo));
            }

            try
            {
                if (File.Exists(sRutaFinal))
                {
                    File.Delete(sRutaFinal);
                }

                file.PostedFile.SaveAs(sRutaFinal);
            }
            catch (Exception ex)
            {
                string codTit = Util.ExceptionHandler(ex);
                throw new Exception(codTit);
            }

            return sRutaFinal;
        }

        [DirectMethod()]
        public DirectResponse DescargaXLS(string sNombreArchivo)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
                sRuta = Path.Combine(sRuta, sNombreArchivo);

                if (File.Exists(sRuta))
                {
                    if (File.Exists(sRuta))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(sRuta);

                        Response.Clear();
                        Response.ContentType = Comun.GetMimeType(file.Extension);

                        Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.TransmitFile(sRuta);
                        Response.Flush();
                    }

                    Response.SuppressContent = true;

                    HttpContext.Current.ApplicationInstance.CompleteRequest();

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

        [DirectMethod()]
        public bool ExistePlantilla(string sRutaOrigen)
        {
            bool bExiste = false;

            try
            {
                string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
                sRuta = Path.Combine(sRuta, sRutaOrigen);

                if (File.Exists(sRuta))
                {
                    bExiste = true;
                }

                else
                {
                    bExiste = false;
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return bExiste;
        }
        [DirectMethod()]
        public bool ExisteLog(string sRutaOrigen)
        {
            bool bExiste = false;

            try
            {
                string sRuta = TreeCore.DirectoryMapping.GetImportLogDirectory();
                sRuta = Path.Combine(sRuta, sRutaOrigen);

                if (File.Exists(sRuta))
                {
                    bExiste = true;
                }

                else
                {
                    bExiste = false;
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return bExiste;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            BaseAPIClient<ImportTaskDTO> APIClient = new BaseAPIClient<ImportTaskDTO>(TOKEN_API);

            string lS = GridRowSelect.SelectedRecordID;

            var oDato = APIClient.GetByCode(lS).Result;

            try
            {
                txtFechaInicio.Value = oDato.Value.ImportDate;
                txtNombre.Value = oDato.Value.Code;
                cmbPlantillas.Value = oDato.Value.Type;
                tmHoraCarga.Value = oDato.Value.ImportDate.TimeOfDay;
                WinConfirmExport.Show();
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

        #region FUNCTIONS

        #region DISEÑO

        #region Direct & Methods LAYOUT


        [DirectMethod]
        public void VwUpdater()
        {

        }





        #endregion

        protected void ShowHidePnAsideR(object sender, DirectEventArgs e)
        {

            //pnAsideR.AnimCollapse = true;
            //pnAsideR.ToggleCollapse();

        }

        protected void ShowHidePnAsideRColumnas(object sender, DirectEventArgs e)
        {


            // X.Call("hidePnLiteDirect");

            //WrapMainContent1.Hide();
            //WrapGestionColumnas.Show();

            //pnAsideR.AnimCollapse = true;
            //pnAsideR.Expand();

        }

        [DirectMethod]
        public void DirectShowHidePnAsideR()
        {


            //  X.Call("hidePnLiteDirect");

            //WrapGestionColumnas.Hide();
            //WrapMainContent1.Show();


            //pnAsideR.AnimCollapse = true;
            //pnAsideR.Expand();

        }

        [DirectMethod]
        public void ColumnHider(string HideShow, string Res)
        {
            // gridMain1.ColumnModel.Columns[2].Hide();

            if (HideShow == "Hide" && Res == "1024")
            {


                // this.TreePanelV1.ColumnModel.Columns[5].Hidden = false;

            }

            else if (HideShow == "Hide" && Res == "480")
            {

                // this.TreePanelV1.ColumnModel.Columns[5].Hidden = false;

            }

            else
            {
                // this.TreePanelV1.ColumnModel.Columns[5].Hidden = true;


            }
        }

        #endregion

        #endregion

    }
}