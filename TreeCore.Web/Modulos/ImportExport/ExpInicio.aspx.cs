using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using TreeCore.Data;


namespace TreeCore.ModExportarImportar.pages
{
    public partial class ExpInicio : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

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

            if ((Request.QueryString["TipoProyecto"] != null))
            {
                string sProyectoTipo = Request.QueryString["TipoProyecto"];
                ExportarConfiguraciones(sProyectoTipo);
            }

            if ((Request.QueryString["DescargarPlantilla"] != null))
            {
                string sPath = Request.QueryString["Ruta"];

                DescargarPlantilla(sPath);
            }

            #endregion
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);
            }
        }
        #endregion

        #region STORE

        #region PROYECTOS TIPOS

        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_ClientesProyectosTipos> listaProyectos;

                    listaProyectos = ListaProyectos();

                    if (listaProyectos != null)
                    {
                       
                        storeProyectosTipos.DataSource = listaProyectos;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ClientesProyectosTipos> ListaProyectos()
        {
            List<Data.Vw_ClientesProyectosTipos> listaDatos;
            ProyectosTiposController cTipos = new ProyectosTiposController();

            try
            {
                listaDatos = cTipos.getProyectosByClienteIDMigrador(long.Parse(hdCliID.Value.ToString()));
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

        #region FUNCTIONS

        //Exportar Logs
        [DirectMethod()]
        public DirectResponse ExportarLogMigrador()
        {
            DirectResponse direct = new DirectResponse();

            string sLineaActual = string.Empty;
            string path = "";

            string sDirectorio = TreeCore.DirectoryMapping.GetSettingMigrationDirectory();

            if (!Directory.Exists(sDirectorio))
                Directory.CreateDirectory(sDirectorio);

            string sRandom = System.IO.Path.GetRandomFileName().Replace(".", "");
            try
            {
                MigradorController cController = new MigradorController();

                    string nombreArchivoRes = "ExportLogs_" + DateTime.Now.ToString("MM-dd-yyyy") + "_" + sRandom + ".txt";
                    path = sDirectorio + "\\" + nombreArchivoRes;
                    

                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(TextArea.Text);

                            }
                        DescargarPlantilla(path);
                        }
                        else
                        {
                            ProblemaLabel.Text = "Error";
                            ProblemaLabel.IconCls = "btnAlertaRed";
                            TextArea.Text += GetGlobalResource(Comun.strArchivoInexistente);
                            btnDescargarPlantilla.Hidden = true;
                        }

            }
            catch (Exception ex)
            {
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }

            direct.Success = true;
            direct.Result = path;

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ExportarConfiguraciones(string sProyectoTipo)
        {
            DirectResponse direct = new DirectResponse();

            string sLineaActual = string.Empty;
            string path = "";
            //string sDirectorio = TreeCore.Properties.Settings.Default.RutaDocumentos;
            string sDirectorio = DirectoryMapping.GetSettingMigrationDirectory();
            if (!Directory.Exists(sDirectorio))
                Directory.CreateDirectory(sDirectorio);
            string sRandom = System.IO.Path.GetRandomFileName().Replace(".", "");
            try
            {

                string resultado = "";

                TextArea.Text = "";
                MigradorController cController = new MigradorController();

                // Datos del archivo resultante

                bool Configuracion = true;
                bool Datos = false;
                ProyectosTiposController cProyectos = new ProyectosTiposController();
                string[] separador = { "," };
                string[] vProyectosTipos = new string[MCModulos.SelectedItems.Count];
                TextArea.Text = "";
                for (int i = 0; i < MCModulos.SelectedItems.Count; i++) // Se irá guardando los datos por cada fila
                {
                    vProyectosTipos[i] = MCModulos.SelectedItems[i].Text;
                }



                foreach (string a in vProyectosTipos)
                {

                    string nombreArchivoRes = a + "_Export_" + DateTime.Now.ToString("MM-dd-yyyy") + "_"+ sRandom + ".json";
                    path = sDirectorio + "\\" + nombreArchivoRes;
                    bool Activos = true;
                    if (a != null)
                    {
                        //Carga del archivo
                        long proyectoTipoID = cProyectos.GetProyectosTiposIDByAlias(a);
                        long registros = 0;
                        List<MigradorTablas> migradorTablas = new List<MigradorTablas>();
                        migradorTablas = cController.GetItemTablas(Configuracion, Datos, proyectoTipoID, Activos);
                        foreach (MigradorTablas mt in migradorTablas)
                        {
                            if (mt.TablaVistaMigracion != null && mt.TablaVistaMigracion != "" && mt.TablaVistaMigracion != "NULL")
                            {
                                resultado = resultado + cController.JsonTabla(mt.TablaVistaMigracion);
                                registros = cController.RegistrosTabla(mt.Tabla);
                            }
                            else
                            {
                                resultado = resultado + cController.JsonTabla(mt.Tabla);
                                registros = cController.RegistrosTabla(mt.Tabla);
                            }
                            log.Info(mt.Tabla.ToString() + " " + GetGlobalResource(Comun.LogTablaMigrada) + " " +
                                    GetGlobalResource(Comun.LogTablaMigradaWith) + " " + registros.ToString() + " " + GetGlobalResource(Comun.LogTablaMigradaRows));
                            /*TextArea.Text += mt.Tabla.ToString() + " " + GetGlobalResource(Comun.LogTablaMigrada) + " " +
                                    GetGlobalResource(Comun.LogTablaMigradaWith) + " " + registros.ToString() + " " + GetGlobalResource(Comun.LogTablaMigradaRows) + System.Environment.NewLine;
                            */
                        }

                        if (resultado != "")
                        {
                            string c = resultado.Substring(resultado.Length - 1);

                            if (c.Equals("/n"))
                                resultado = resultado.Substring(0, resultado.Length - 1);
                        }
                        
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(resultado);

                            }
                            ProblemaLabel.Text = "Success";
                            ProblemaLabel.IconCls = "btnDescargar";
                            TextArea.Text += " File Created " + System.Environment.NewLine;
                            TextArea.Text += "You must click download to save the file" + System.Environment.NewLine;
                        }
                        else
                        {
                            ProblemaLabel.Text = "Error";
                            ProblemaLabel.IconCls = "btnAlertaRed";
                            TextArea.Text += GetGlobalResource(Comun.strArchivoInexistente);
                            btnDescargarPlantilla.Hidden = true;
                        }

                        resultado = "";
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                direct.Success = true;
                return direct;
            }

            direct.Success = true;
            direct.Result = path;

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ImportarConfiguraciones()
        {
            DirectResponse direct = new DirectResponse();
            string fileDirectory = "";
            ProyectosTiposController cProyectos = new ProyectosTiposController();
            string datosresultado = "";

            try
            {
                TextArea.Text = "";
                fileDirectory = TreeCore.DirectoryMapping.GetSettingMigrationDirectory();

                if (UploadF.HasFile)
                {
                    string archivo = UploadF.FileName;
                    string fullpath = Path.Combine(fileDirectory, archivo);
                    
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }
                    
                    UploadF.PostedFile.SaveAs(fullpath);
                    System.IO.StreamReader datos = new System.IO.StreamReader(fullpath);
                    
                    string tablas = datos.ReadToEnd();
                    
                    datos.Close();
                    
                    if (tablas.Substring(tablas.Length - 1).Equals("\n"))
                    {
                        tablas = tablas.Substring(0, tablas.Length - 1);
                    }
                    string[] separador = { "[]", "][" };
                    string[] datosTablaMigrador = tablas.Split(separador, StringSplitOptions.None);
                    int j = 0;
                    string con = TreeCore.Properties.Settings.Default.Conexion;

                    //Sacamos los datos de la tabla MigradorTablas
                    bool Configuracion = true;
                    bool Datos = false;
                    long proyectoTipoID = cProyectos.GetProyectosTiposIDByAlias(cmbTipe.Value.ToString());
                    bool Activos = true;
                    int i = 0;
                    

                    List<MigradorTablas> migradorTablas = new List<MigradorTablas>();
                    MigradorController cMigrador = new MigradorController();
                    string json = "";
                    migradorTablas = cMigrador.GetItemTablas(Configuracion, Datos, proyectoTipoID, Activos);
                    if (migradorTablas.Count > 0)
                    {
                        foreach (string a in datosTablaMigrador)
                        {
                            if (a != null && !a.Equals("") && !a.Equals("]"))
                            {
                                string b = a.Substring(0, 1);
                                string c = a.Substring(a.Length - 1);
                                if (a.Substring(0, 1).Equals("[") && a.Substring(a.Length - 1).Equals("]"))
                                {
                                    json = a;
                                }
                                else if (a.Substring(0, 1).Equals("["))
                                {
                                    if (c.Equals("\n"))
                                        json = a.Substring(0, a.Length - 1);
                                    else
                                        json = a + "]";
                                }

                                else if ((a.Substring(a.Length - 1).Equals("]") && !(a.Substring(0, 1).Equals("["))) || (a.Substring(a.Length - 1).Equals("\r") && !(a.Substring(0, 1).Equals("["))))
                                {
                                    json = "[" + a;
                                }
                                else
                                {
                                    json = a.Substring(a.Length - 1);
                                    json = "[" + a + "]";
                                }
                                DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));


                                if (!json.Equals("[]"))
                                {
                                    if (migradorTablas[i].TablaVistaMigracion == null || migradorTablas[i].TablaVistaMigracion.Equals("") || migradorTablas[i].TablaVistaMigracion.Equals("NULL"))
                                    {
                                        
                                        datosresultado = cMigrador.InsertarTabla(dt, migradorTablas[i].Tabla, migradorTablas[i].CamposClave, TextArea.Text);
                                        TextArea.Text += datosresultado;
                                        log.Info(datosresultado);

                                    }
                                    else
                                    {
                                        if (migradorTablas[i].EsPadre)
                                        {
                                            datosresultado = cMigrador.InsertarTablaVista(proyectoTipoID, cMigrador.DTabla(migradorTablas[i].Tabla), migradorTablas[i].Tabla, dt, migradorTablas[i].TablaVistaMigracion, migradorTablas[i].BuscarTabla, migradorTablas[i].CamposClave, TextArea.Text);
                                        }
                                        datosresultado = cMigrador.InsertarTablaVista(proyectoTipoID, cMigrador.DTabla(migradorTablas[i].Tabla), migradorTablas[i].Tabla, dt, migradorTablas[i].TablaVistaMigracion, migradorTablas[i].BuscarTabla, migradorTablas[i].CamposClave, TextArea.Text);
                                        TextArea.Text += datosresultado;
                                        log.Info(datosresultado);
                                         

                                    }
                                }
                                else
                                {
                                    TextArea.Text += migradorTablas[i].Tabla.ToString() + " was successfully upload " + System.Environment.NewLine;
                                    log.Info(migradorTablas[i].Tabla.ToString() + " was successfully upload " + System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                TextArea.Text += migradorTablas[i].Tabla.ToString() + " was successfully upload " + System.Environment.NewLine;
                                log.Info(migradorTablas[i].Tabla.ToString() + " was successfully upload " + System.Environment.NewLine);
                            }
                            i++;
                           
                        }
                    }
                }

                ProblemaLabel.Text = "                Success";
                ProblemaLabel.IconCls = "btnDescargar";
                winExportImportProgress.Title = "Import";


            }
            catch (Exception ex)
            {
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                TextArea.Text = "There was a problem with the file you are trying to upload";
                ProblemaLabel.Text = "                There was a problem!";
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public bool ExistePlantilla(string sPath)
        {
            ProyectosTiposController cProyectos = new ProyectosTiposController();
            DocumentosCargasPlantillasController cPlantillas = new DocumentosCargasPlantillasController();
            bool bExiste = false;

            try
            {
                if (sPath != null)
                {
                    if (File.Exists(sPath))
                    {
                        bExiste = true;
                    }
                    else
                    {
                        bExiste = false;
                        MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
                    }
                }
                else
                {
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
        public DirectResponse DescargarPlantilla(string sPath)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                if (File.Exists(sPath))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(sPath);

                    Response.Clear();
                    Response.ContentType = Comun.GetMimeType(file.Extension);

                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.TransmitFile(sPath);
                    Response.Flush();

                    Response.SuppressContent = true;
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                else
                {
                    MensajeBox(GetGlobalResource(Comun.strExportarLog), GetGlobalResource(Comun.strArchivoInexistente), Ext.Net.MessageBox.Icon.WARNING, null);
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