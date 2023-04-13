using CapaNegocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;
using TreeCore.Data;


namespace ServiceImportExport
{
    public partial class ServiceImportExport : ServiceBase
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Global variables
        private System.Timers.Timer temporizador = null;
        string NombreServicio = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        DateTime ahora = DateTime.Now;

        CoreServiceSettings coreServiceSettings = new CoreServiceSettings();
        int hora = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Hora"]);
        int minuto = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Minuto"]);
        int segundo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Segundo"]);
        double intervaloInicialWebConfig = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Intervalo"]);
        string directorioLogs = TreeCore.DirectoryMapping.GetImportLogDirectory();
        string Version = System.Configuration.ConfigurationManager.AppSettings["Version"];


        //Inicializamos CADENCIA del servicio
        private int cadencia = 0;

        #region GESTION SERVICIO

        public ServiceImportExport()
        {
            //Convertimos el parametro del webconfig que esta en milisecons a HORAS(numero entero) y lo pasamos como cadencia mas abajo
            cadencia = (int)TimeSpan.FromHours(intervaloInicialWebConfig).TotalHours;
            InitializeComponent();

#if SERVICE_AS_PROGRAM
            // call the event 10 seconds after this constructor
            DateTime dt = DateTime.Now.AddSeconds(10);
            //hora = dt.Hour;
            //minuto = dt.Minute;
            //segundo = dt.Second;
#endif

            this.ServiceName = NombreServicio;
        }
#if SERVICE_AS_PROGRAM
        // this will call the event on a keystroke
        public void CallEvent()
        {
            ElapsedEventArgs e = new EventArgs() as ElapsedEventArgs;

            EjecutarServicioImport(this, e);
        }
#endif
        public void Start()
        {
            OnStart(new string[0]);
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("OnStart");
            // Local variables
            ServiceSettingsfunc();
            DateTime fechaReferencia = DateTime.Now;
            TimeSpan retraso = TimeSpan.MinValue;
            double intervaloReinicio = intervaloInicialWebConfig;


            // Launches the timer
            if (temporizador == null)
            {
                temporizador = new Timer();
            }
            // Gets the interval
            ahora = DateTime.Now;

            // Checks the current time for Analytics
            if (ahora.Hour > hora)
            {

                fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo).AddDays(1);
                retraso = fechaReferencia.Subtract(ahora);
                intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;

            }
            else
            {

                if (ahora.Hour == hora)
                {

                    if (ahora.Minute >= minuto)
                    {
                        fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo).AddDays(1);
                        retraso = fechaReferencia.Subtract(ahora);
                        intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                    }
                    else
                    {
                        fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo);
                        retraso = fechaReferencia.Subtract(ahora);
                        intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                    }
                }
                else
                {

                    fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo);
                    retraso = fechaReferencia.Subtract(ahora);
                    intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                }

            }
            #region REGISTRO LOG DEL SERVICIO Start
            //VARIABLES PARA EL STOP
            CapaNegocio.MonitoringServiciosTreeController cServiciosTREE = new MonitoringServiciosTreeController();

            DateTime fechaInicio = DateTime.Now;
            DateTime? fechaFin = DateTime.Now;
            bool activo = true;
            bool desplegado = true;

            //Se registra el START del servicio a la tabla MonitoringServiciosTREE
            ParametrosController cParametro = new ParametrosController();
            //comprobamos si el registro a añadir existe previamente o no ya
            MonitoringServiciosTREE ItemPadre = cServiciosTREE.getRegistroPadre(ComunServicios.SERVICIO_IMPORT_EXPORT);

            long? padreID = null;
            if (ItemPadre != null)
            {
                padreID = ItemPadre.MonitoringServicioTREEID;
            }


            string Version = (from c in cParametro.Context.Parametros where c.Parametro == "BBDDVersion" select c.Valor).FirstOrDefault();
            cServiciosTREE.AgregarRegistro(ComunServicios.SERVICIO_IMPORT_EXPORT, Version, fechaInicio, fechaFin, activo, desplegado, padreID, cadencia);

            // check para coherencia con el item padre (Solo si ya existe anteriormente)

            if (ItemPadre != null)
            {
                ItemPadre.FechaInicio = DateTime.Now;
                ItemPadre.FechaParada = DateTime.Now;
                ItemPadre.Activo = true;
                ItemPadre.Cadencias = cadencia;
                cServiciosTREE.UpdateItem(ItemPadre);
            }

            #endregion
            
            // Starts the timer
            temporizador.Elapsed += new System.Timers.ElapsedEventHandler(EjecutarServicioImport);
            temporizador.Elapsed += new System.Timers.ElapsedEventHandler(EjecutarServicioExport);
            temporizador.Interval = intervaloReinicio;
            temporizador.Start();
            Log.Info("OnStart Hour: " + hora.ToString() + " Minute: " + minuto.ToString() + " Interval: " + intervaloReinicio.ToString());
            Log.Info("End_OnStart");
        }

        protected override void OnStop()
        {
            // si no tienes permiso en la proxima linea, ejecutar visual studio como administrador (System.Security.SecurityException)

            // o HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog\Application

            try
            {
                Log.Info("OnStop");
                EventLog.WriteEntry(NombreServicio, "STOP service");
               
            }
            catch (Exception)
            {

            }

            #region REGISTRO LOG DEL SERVICIO STOP
            //VARIABLES PARA EL STOP
            MonitoringServiciosTreeController cServiciosTREE = new MonitoringServiciosTreeController();

            DateTime ultimoregistrofechaini = cServiciosTREE.getUltimoRegistroFechaIni(ComunServicios.SERVICIO_IMPORT_EXPORT);

            DateTime fechaInicio = DateTime.MinValue;
            if (ultimoregistrofechaini != DateTime.MinValue)
            {
                fechaInicio = ultimoregistrofechaini;
            }

            DateTime? fechaFin = DateTime.Now;
            bool activo = false;
            bool desplegado = true;

            //Se registra el START del servicio a la tabla MonitoringServiciosTREE
            ParametrosController cParametro = new ParametrosController();
            //comprobamos si el registro a añadir existe previamente o no ya
            MonitoringServiciosTREE ItemPadre = cServiciosTREE.getRegistroPadre(ComunServicios.SERVICIO_IMPORT_EXPORT);

            long? padreID = null;
            if (ItemPadre != null)
            {
                padreID = ItemPadre.MonitoringServicioTREEID;
            }


            string Version = (from c in cParametro.Context.Parametros where c.Parametro == "BBDDVersion" select c.Valor).FirstOrDefault();
            cServiciosTREE.AgregarRegistro(ComunServicios.SERVICIO_IMPORT_EXPORT, Version, fechaInicio, fechaFin, activo, desplegado, padreID, cadencia);

            // check para coherencia con el item padre (Solo si ya existe anteriormente)

            if (ItemPadre != null)
            {
                ItemPadre.FechaInicio = ultimoregistrofechaini;
                ItemPadre.FechaParada = DateTime.Now;
                ItemPadre.Activo = false;
                ItemPadre.Cadencias = cadencia;
                cServiciosTREE.UpdateItem(ItemPadre);
            }

            #endregion

            temporizador.AutoReset = false;
            temporizador.Enabled = false;
            Log.Info("END_OnStop");
        }

        #endregion

        #region EJECUCION SERVICIO

        #region IMPORT
        private void EjecutarServicioImport(object sender, System.Timers.ElapsedEventArgs e)
        {
            EscritorLogs.SetRuta(directorioLogs);
            Log.Info("Service Version: " + Version);
            Log.Info("BEGIN_EjecutarServicioImport");
            // Stops the timer
            this.temporizador.Stop();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            temporizador.Interval = TimeSpan.FromHours(intervaloInicialWebConfig).TotalMilliseconds;

            DocumentosCargasController cDocumentosCarga = new DocumentosCargasController();
            DocumentosCargasPlantillasController cDocCargasPlantillas = new DocumentosCargasPlantillasController();
            DocumentosCargasPlantillasTabsController cDocCagasPlanTabs = new DocumentosCargasPlantillasTabsController();
            ClientesController cCliente = new ClientesController();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin;
            Clientes clienteLocal = null;

            try
            {
                List<DocumentosCargas> listaDocCargasAnterior = new List<DocumentosCargas>();
                List<Vw_DocumentosCargas> listaDocCargasVwAnterior = new List<Vw_DocumentosCargas>();
                List<DocumentosCargas> listaDocCargas = new List<DocumentosCargas>();
                List<Vw_DocumentosCargas> listaDocCargasVw = new List<Vw_DocumentosCargas>();
                DocumentosCargasPlantillas oDocPlantilla = new DocumentosCargasPlantillas();
                string log = "";

                clienteLocal = cCliente.GetDefault(0);
                listaDocCargasAnterior = cDocumentosCarga.GetAllDocumentosNOProcesados();
                listaDocCargasVwAnterior = cDocumentosCarga.GetAllDocumentosNOProcesadosVw();

                listaDocCargas = cDocumentosCarga.GetAllDocumentosNOProcesadosNuevo();
                listaDocCargasVw = cDocumentosCarga.GetAllDocumentosNOProcesadosVw();

                if (listaDocCargas.Count > 0)
                {
                    foreach (DocumentosCargas docCarga in listaDocCargas)
                    {
                        Vw_DocumentosCargas docCargaVw = listaDocCargasVw.Find(dc => dc.DocumentoCargaID == docCarga.DocumentoCargaID);

                        if (docCargaVw.FechaInicio.HasValue && TreeCore.Cron.SiguienteFechaEsHoy(docCargaVw.CronFormat, docCargaVw.FechaInicio.Value, docCargaVw.FechaFin))
                        {
                            #region Ejecución carga
                            //Comprobamos las condiciones de configuracion de las plantillas para hacer la carga
                            EscritorLogs.SeleccionarArchivo(docCarga.DocumentoCarga);
                            DocumentosCargasController cDocumentosCargasController = new DocumentosCargasController();
                            DocumentosCargas odocumentocarga = new DocumentosCargas();
                            DocumentosCargasPlantillasTabsController cDocumentosCargasPlantillas = new DocumentosCargasPlantillasTabsController();
                            oDocPlantilla = cDocCargasPlantillas.GetPlantillaByID(docCarga.DocumentoCargaPlantillaID, true);

                            odocumentocarga = cDocumentosCargasController.GetItem(docCarga.DocumentoCargaID);
                            odocumentocarga.RutaLog = docCarga.DocumentoCarga + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") + ".log";

                            if (docCarga.ClienteID != null || docCarga.OperadorID != null)
                            {
                                try
                                {
                                    string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
                                    sRuta = Path.Combine(sRuta, docCarga.RutaDocumento);

                                    System.IO.StreamReader datos = new System.IO.StreamReader(sRuta);

                                    switch (oDocPlantilla.DocumentoCargaPlantilla)
                                    {

                                        case ComunServicios.SERVICIO_CARGA_DISTRIBUCION_REGIONAL:

                                            log = TreeCore.Clases.ImportExport.CargarPaisRegionProvinciaMunicipio(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_SITES:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaV23SITES(datos.BaseStream, docCarga);

                                            break;
                                        case ComunServicios.SERVICIO_CARGA_CONTACTOS:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaContactos(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_DOCUMENTOS:

                                            log = TreeCore.Clases.ImportExport.CargarDocumentacion(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_INVENTARIO:

                                            log = TreeCore.Clases.ImportExport.CargaMasivaInventario(datos.BaseStream, docCarga);
                                            break;
                                        case ComunServicios.SERVICIO_CARGA_ENTIDADES:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaEntidades(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_VINCULACIONES_ELEMENTOS:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaElementosVinculaciones(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_FORM_SECTIONS:

                                            log = TreeCore.Clases.ImportExport.CargaMasivaSubcategorias(datos.BaseStream, docCarga);
                                            break;

                                        case ComunServicios.SERVICIO_CARGA_USUARIOS:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaUsuarios(datos.BaseStream, docCarga);
                                            break;

										/*case ComunServicios.SERVICIO_CARGA_PRODUCT_CATALOG_SERVICIOS:

											log = TreeCore.Clases.ImportExport.CargarPlantillaProductCatalogServicios(datos.BaseStream, docCarga);
                                        	break;

                                        case ComunServicios.SERVICIO_CARGA_PRODUCT_CATALOGS:

                                            log = TreeCore.Clases.ImportExport.CargarPlantillaProductCatalogs(datos.BaseStream, docCarga);
                                            break;*/

                                        #region Plantillas Anteriores

                                        //case ComunServicios.SERVICIO_CARGA_PLANTILLA_V23:

                                        //   if(docCarga.ClienteID!=null /*&& docCarga.OperadorID != null*/)
                                        //    {

                                        //        log = TreeCore.Clases.ImportExport.CargarPlantillaV23(datos.BaseStream, docCarga);
                                        //        odocumentocarga.RutaLog = directorioLogs + "/" + "SERVICEIMPORTEXPORTV23_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("d2") + "-" + DateTime.Now.Day.ToString("d2") + ".log";
                                        //    }
                                        //    else
                                        //    {
                                        //       log.Info(docCarga.DocumentoCarga + " The upload was successful, check the logs");
                                        //        odocumentocarga.Resultado = "The upload was successful";
                                        //        odocumentocarga.Procesado = true;
                                        //        odocumentocarga.FechaCarga = DateTime.Now;
                                        //        odocumentocarga.FechaEstimadaSubida = DateTime.Now;
                                        //        odocumentocarga.Exito = true;

                                        //        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                        //        continue;
                                        //    }

                                        //    break;
                                        //case ComunServicios.SERVICIO_CARGA_PRIPIETARIOS:

                                        //    if (docCarga.ClienteID != null /*&& docCarga.OperadorID != null*/)
                                        //    {

                                        //        log = TreeCore.Clases.ImportExport.CargarPlantillaV23Propietarios(datos.BaseStream, docCarga);
                                        //        odocumentocarga.RutaLog = directorioLogs + "/" + "SERVICEIMPORTEXPORTOWNERS_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("d2") + "-" + DateTime.Now.Day.ToString("d2") + ".log";
                                        //    }
                                        //    else
                                        //    {
                                        //        Log.Info(docCarga.DocumentoCarga + " The upload was successful, check the logs");
                                        //        odocumentocarga.Resultado = "The upload was successful";
                                        //        odocumentocarga.Procesado = true;
                                        //        odocumentocarga.FechaCarga = DateTime.Now;
                                        //        odocumentocarga.FechaEstimadaSubida = DateTime.Now;
                                        //        odocumentocarga.Exito = true;

                                        //        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                        //        continue;
                                        //    }

                                        //    break;
                                        //case ComunServicios.SERVICIO_CARGA_PROVEEDORES:

                                        //    if (docCarga.ClienteID != null /*&& docCarga.OperadorID != null*/)
                                        //    {

                                        //        log = TreeCore.Clases.ImportExport.CargarPlantillaV23Proveedores(datos.BaseStream, docCarga);
                                        //        odocumentocarga.RutaLog = directorioLogs + "/" + "SERVICEIMPORTEXPORTV23PROVIDERS_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("d2") + "-" + DateTime.Now.Day.ToString("d2") + ".log";
                                        //    }
                                        //    else
                                        //    {
                                        //        Log.Info(docCarga.DocumentoCarga + " The upload was successful, check the logs");
                                        //        odocumentocarga.Resultado = "The upload was successful";
                                        //        odocumentocarga.Procesado = true;
                                        //        odocumentocarga.FechaCarga = DateTime.Now;
                                        //        odocumentocarga.FechaEstimadaSubida = DateTime.Now;
                                        //        odocumentocarga.Exito = true;

                                        //        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                        //        continue;
                                        //    }

                                        //    break;
                                        //case ComunServicios.SERVICIO_CARGA_EMPLAZAMIENTOS:

                                        //    if (docCarga.ClienteID != null /*&& docCarga.OperadorID != null*/)
                                        //    {
                                        //        log = TreeCore.Clases.ImportExport.CargarPlantillaV23Emplazamientos(datos.BaseStream, docCarga);
                                        //        odocumentocarga.RutaLog = directorioLogs + "/" + "SERVICEIMPORTEXPORTV23SITES_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("d2") + "-" + DateTime.Now.Day.ToString("d2") + ".log";

                                        //    }
                                        //    else
                                        //    {
                                        //       log.Info(docCarga.DocumentoCarga + " The upload was successful, check the logs");
                                        //        odocumentocarga.Resultado = "The upload was successful";
                                        //        odocumentocarga.Procesado = true;
                                        //        odocumentocarga.FechaCarga = DateTime.Now;
                                        //        odocumentocarga.FechaEstimadaSubida = DateTime.Now;
                                        //        odocumentocarga.Exito = true;

                                        //        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                        //        continue;
                                        //    }

                                        //    break;

                                        #endregion

                                        default:
                                            log = "File not found";
                                            break;
                                    }

                                    if (log != "")
                                    {
                                        Log.Info(docCarga.DocumentoCarga + "Error during import. Please check ERROR log");
                                        odocumentocarga.Resultado = "Error during import. Please check ERROR log";
                                        odocumentocarga.Procesado = false;
                                        odocumentocarga.FechaCarga = DateTime.Now;
                                        odocumentocarga.Exito = false;

                                        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                    }
                                    else
                                    {
                                        Log.Info(docCarga.DocumentoCarga + " The upload was successful, check the logs");
                                        odocumentocarga.Resultado = "The upload was successful";
                                        odocumentocarga.Procesado = true;
                                        odocumentocarga.FechaCarga = DateTime.Now;
                                        odocumentocarga.FechaEstimadaSubida = DateTime.Now;
                                        odocumentocarga.Exito = true;

                                        cDocumentosCargasController.UpdateItem(odocumentocarga);
                                    }

                                    datos.Close();
                                    datos = null;
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                    odocumentocarga.Resultado = "The upload has not been done correctly, check the logs";
                                    odocumentocarga.Procesado = false;
                                    odocumentocarga.Exito = false;
                                    odocumentocarga.FechaCarga = DateTime.Now;

                                    cDocumentosCargasController.UpdateItem(odocumentocarga);
                                }
                            }
                            else
                            {
                                Log.Info(docCarga.DocumentoCarga + " Customer and operator fields are required for upload");
                                odocumentocarga = cDocumentosCargasController.GetItem(docCarga.DocumentoCargaID);
                                odocumentocarga.Resultado = "The upload has not been done correctly, check the logs";
                                odocumentocarga.Procesado = false;
                                odocumentocarga.Exito = false;
                                odocumentocarga.FechaCarga = DateTime.Now;

                                cDocumentosCargasController.UpdateItem(odocumentocarga);
                            }
                            GC.Collect();
                            #endregion
                        }
                    }
                }
                else
                {
                    // Monitoring information
                    MonitoringServiciosWindowsController cMonitoringService = new MonitoringServiciosWindowsController();
                    string myIP = "";
                    Log.Info("There are no documents to process");
                    try
                    {
                        string hostName = Dns.GetHostName();
                        myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    }
                    catch (Exception exNET)
                    {
                        myIP = "";
                        Log.Error("Failed to read IP: " + exNET.Message);
                    }

                    fechaFin = DateTime.Now;
                    cMonitoringService.AgregarRegistro(ComunServicios.SERVICIO_IMPORT_EXPORT, myIP, "No files to upload", fechaInicio, fechaFin, clienteLocal.ClienteID, true);
                }
                Log.Info("END_EjecutarServicioImport");

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MonitoringServiciosWindowsController wsController = new MonitoringServiciosWindowsController();
                string myIP = "";

                try
                {
                    string hostName = Dns.GetHostName();
                    myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                }
                catch (Exception exNET)
                {
                    myIP = "";
                    Log.Error("Error al leer la IP del equipo: " + exNET.Message);
                }
                fechaFin = DateTime.Now;
                wsController.AgregarRegistro(NombreServicio, myIP, "Error al crear ficheros: " + " Exception Code 100", fechaInicio, fechaFin, clienteLocal.ClienteID, false);
            }
            ServiceSettingsfunc();
            SetIntervalo();
            this.temporizador.Start();
            Log.Info("END_TemporizadorStart");
            Log.Info("OnStart Hour: " + hora.ToString() + " Minute: " + minuto.ToString() + " Interval: " + temporizador.Interval.ToString());
        }
        #endregion

        #region EXPORT
        private void EjecutarServicioExport(object sender, System.Timers.ElapsedEventArgs e)
        {

            // Stops the timer
            Log.Info("Service Version: " + Version);
            Log.Info("BEGIN_EjecutarServicioExport");
            this.temporizador.Stop();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            temporizador.Interval = TimeSpan.FromHours(intervaloInicialWebConfig).TotalMilliseconds;

            CoreExportacionDatosPlantillasController cCoreExportacionDatosPlantillas = new CoreExportacionDatosPlantillasController();
            ColumnasModeloDatosController cColumnasModeloDatos = new ColumnasModeloDatosController();
            TiposDatosController cTiposDatos = new TiposDatosController();
            ClientesController cCliente = new ClientesController();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin;
            Clientes clienteLocal = null;

            try
            {
                string log = "";
                clienteLocal = cCliente.GetDefault(0);


                List<CoreExportacionDatosPlantillas> plantillas = cCoreExportacionDatosPlantillas.GetActivos();
                List<Vw_CoreExportacionDatosPlantillas> plantillasVw = cCoreExportacionDatosPlantillas.GetActivosVista();
                List<ColumnasModeloDatos> columnasModeloDatos = cColumnasModeloDatos.GetItemList();
                List<TiposDatos> lTiposDatos = cTiposDatos.GetItemList();

                Dictionary<long, TiposDatos> dicTiposDatos = new Dictionary<long, TiposDatos>();

                lTiposDatos.ForEach(tp => {
                    if (!dicTiposDatos.ContainsKey(tp.TipoDatoID))
                    {
                       dicTiposDatos.Add(tp.TipoDatoID, tp);
                    }
                });


                if (plantillas != null)
                {
                    foreach (CoreExportacionDatosPlantillas plantilla in plantillas)
                    {
                        Vw_CoreExportacionDatosPlantillas plantillaVw = plantillasVw.Find(p => p.CoreExportacionDatoPlantillaID == plantilla.CoreExportacionDatoPlantillaID);

                        if (plantillaVw.FechaInicio.HasValue && TreeCore.Cron.SiguienteFechaEsHoy(plantillaVw.CronFormat, plantillaVw.FechaInicio.Value, plantillaVw.FechaFin) ||
                            plantillaVw.FechaInicio.HasValue && plantillaVw.UnaVez && plantillaVw.FechaInicio == DateTime.Today)
                        {
                            #region Ejecución de la exportación
                            Log.Info("Init template: " + plantilla.Nombre);
                            try
                            {
                                log += TreeCore.Clases.ImportExport.ExportTemplateData(plantilla, columnasModeloDatos, dicTiposDatos);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex.Message);
                            }

                            Log.Info("End template: " + plantilla.Nombre);
                            #endregion
                        }
                    }

                    if (log != "")
                    {
                        //Error en exportación
                        Log.Info(log);
                    }
                    else
                    {
                        //Exportación success
                        Log.Info(log);
                    }
                }
                else
                {
                    log = "Templates not available";
                }
                Log.Info("END_EjecutarServicioExport");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MonitoringServiciosWindowsController wsController = new MonitoringServiciosWindowsController();
                string myIP = "";

                try
                {
                    string hostName = Dns.GetHostName();
                    myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                }
                catch (Exception exNET)
                {
                    myIP = "";
                }

                fechaFin = DateTime.Now;
                wsController.AgregarRegistro(NombreServicio, myIP, "Error al exportar datos: " + " Exception Code 100", fechaInicio, fechaFin, clienteLocal.ClienteID, false);
            }
            ServiceSettingsfunc();
            SetIntervalo();
            this.temporizador.Start();
            Log.Info("BEGIN_TemporizadorStart");
            Log.Info("OnStart Hour: " + hora.ToString() + " Minute: " + minuto.ToString() + " Interval: " + temporizador.Interval.ToString());
        }
        #endregion

        public static string GetLanguage()
        {
            IdiomasController cIdiomas = new IdiomasController();
            return cIdiomas.GetCodigoDefecto();
        }
        #endregion

        public void ServiceSettingsfunc()
        {
            CoreServiceSettingsController ccoreServiceSettings = new CoreServiceSettingsController();

            try
            {
                Log.Info("Service Version: " + Version);
                Log.Info("BEGIN_ServiceSettingsfunc");
                coreServiceSettings = ccoreServiceSettings.GetCoreServiceSettings(601);
                hora = coreServiceSettings.HoraEjecucion;
                minuto = coreServiceSettings.MinutoEjecucion;
                segundo = 0;
                Log.Info("END_ServiceSettingsfunc");
            }
            catch (Exception)
            {
            }
            //return coreServiceSettings;
        }


        public void SetIntervalo()
        {
            Log.Info("Service Version: " + Version);
            Log.Info("BEGIN_SetIntervalo");
            // Gets the interval
            ahora = DateTime.Now;
            DateTime fechaReferencia = DateTime.Now;
            TimeSpan retraso = TimeSpan.MinValue;
            double intervaloReinicio = intervaloInicialWebConfig;

            // Checks the current time for Analytics
            if (ahora.Hour > hora)
            {

                fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo).AddDays(1);
                retraso = fechaReferencia.Subtract(ahora);
                intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;

            }
            else
            {

                if (ahora.Hour == hora)
                {

                    if (ahora.Minute >= minuto)
                    {
                        fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo).AddDays(1);
                        retraso = fechaReferencia.Subtract(ahora);
                        intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                    }
                    else
                    {
                        fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo);
                        retraso = fechaReferencia.Subtract(ahora);
                        intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                    }
                }
                else
                {

                    fechaReferencia = new DateTime(ahora.Year, ahora.Month, ahora.Day, hora, minuto, segundo);
                    retraso = fechaReferencia.Subtract(ahora);
                    intervaloReinicio = (retraso.Hours * 3600 + retraso.Minutes * 60 + retraso.Seconds) * 1000;
                }
                Log.Info("END_SetIntervalo");

            }
        }
    }
}
