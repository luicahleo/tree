using CapaNegocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using TreeCore.Data;

namespace ServiceDataQuality
{
    public partial class ServiceDataQuality : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger("");

        public static string _emailServiceUser = "treeservices@atrebo.com";
        public static string TREE_SERVICES_USER
        {
            get
            {
                UsuariosController cUsuarios = new UsuariosController();
                ClientesController cClientes = new ClientesController();
                long? cliID = cClientes.GetSingleClientID();

                if (cUsuarios.checkSystemUser("Service", "User", _emailServiceUser, cliID))
                {
                    return _emailServiceUser;
                }
                else
                {
                    return "";
                }
            }
        }
       

        // Global variables
        private System.Timers.Timer temporizador = null;
        string NombreServicio = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        DateTime ahora = DateTime.Now;
               
        CoreServiceSettings coreServiceSettings = new CoreServiceSettings();
        int hora = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Hora"]);
        int minuto = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Minuto"]);
        int segundo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Segundo"]);
        double intervaloInicialWebConfig = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Intervalo"]);
        string Version = System.Configuration.ConfigurationManager.AppSettings["Version"];


        //Inicializamos CADENCIA del servicio
        private int cadencia = 0;
      
       
        public ServiceDataQuality()
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
        }
#if SERVICE_AS_PROGRAM
        // this will call the event on a keystroke
        public void CallEvent()
        {
            ElapsedEventArgs e = new EventArgs() as ElapsedEventArgs;

            EjecutarServicio(this, e);
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
            MonitoringServiciosTREE ItemPadre = cServiciosTREE.getRegistroPadre(Comun.SERVICIO_DATA_QUALITY);

            long? padreID = null;
            if (ItemPadre != null)
            {
                padreID = ItemPadre.MonitoringServicioTREEID;
            }


            string Version = (from c in cParametro.Context.Parametros where c.Parametro == "BBDDVersion" select c.Valor).FirstOrDefault();
            cServiciosTREE.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, Version, fechaInicio, fechaFin, activo, desplegado, padreID, cadencia);

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
            Log.Info("OnStart Hour: " + hora.ToString() + " Minute: " + minuto.ToString() + " Interval: " + intervaloReinicio.ToString());
            // Starts the timer
            temporizador.Elapsed += new System.Timers.ElapsedEventHandler(EjecutarServicio);
            temporizador.Interval = intervaloReinicio;
            temporizador.Start();

            Log.Info("END_OnStart");
        }

        protected override void OnStop()
        {
            Log.Info("OnStop");
            // si no tienes permiso en la proxima linea, ejecutar visual studio como administrador (System.Security.SecurityException)

            // o HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog\Application

            try
            {
                EventLog.WriteEntry(NombreServicio, "STOP service");
            }
            catch (Exception)
            {

            }

            #region REGISTRO LOG DEL SERVICIO STOP
            //VARIABLES PARA EL STOP
            MonitoringServiciosTreeController cServiciosTREE = new MonitoringServiciosTreeController();

            DateTime ultimoregistrofechaini = cServiciosTREE.getUltimoRegistroFechaIni(Comun.SERVICIO_DATA_QUALITY);

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
            MonitoringServiciosTREE ItemPadre = cServiciosTREE.getRegistroPadre(Comun.SERVICIO_DATA_QUALITY);

            long? padreID = null;
            if (ItemPadre != null)
            {
                padreID = ItemPadre.MonitoringServicioTREEID;
            }


            string Version = (from c in cParametro.Context.Parametros where c.Parametro == "BBDDVersion" select c.Valor).FirstOrDefault();
            cServiciosTREE.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, Version, fechaInicio, fechaFin, activo, desplegado, padreID, cadencia);

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

        #region EJECUCION SERVICIO
        
        private void EjecutarServicio(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Stops the timer
            Log.Info("BEGIN_EjecutarServicio");
            this.temporizador.Stop();
            temporizador.Interval = TimeSpan.FromHours(intervaloInicialWebConfig).TotalMilliseconds;
            DateTime fechaInicio = DateTime.Now;
            string sComentarios = "";
            Log.Info("Service Version: " + Version);
            EjecucionServiceDataQuality(fechaInicio);

            EjecucionServiceInventarioHistoricos();
            ServiceSettingsfunc();
            SetIntervalo();
            this.temporizador.Start();
            Log.Info("BEGIN_TemporizadorStart");
            Log.Info("OnStart Hour: " + hora.ToString() + " Minute: " + minuto.ToString() + " Interval: " + temporizador.Interval.ToString());
        }

        public static string GetLanguage()
        {
            IdiomasController cIdiomas = new IdiomasController();
            return cIdiomas.GetCodigoDefecto();
        }

        private string getMyIP()
        {
            string myIP;

            try
            {
                string hostName = Dns.GetHostName();
                myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            }
            catch (Exception exNET)
            {
                myIP = "";
            }

            return myIP;
        }
        #endregion

        public void ServiceSettingsfunc()
        {
            CoreServiceSettingsController ccoreServiceSettings = new CoreServiceSettingsController();
          
            try
            {
                Log.Info("Service Version: " + Version);
                Log.Info("BEGIN_ServiceSettingsfunc");
                coreServiceSettings = ccoreServiceSettings.GetCoreServiceSettings(602);
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

        #region DataQuality
        private void EjecucionServiceDataQuality(DateTime fechaInicio)
        {
            DateTime fechaFin;

            MonitoringServiciosWindowsController cMonitoringService = new MonitoringServiciosWindowsController();
            UsuariosController cUsuarios = new UsuariosController();

            try
            {
                Log.Info(Comun.SERVICIO_DATA_QUALITY + ": " + "Init");

                string _email = TREE_SERVICES_USER;
                Usuarios usuario = cUsuarios.getUsuarioByEmail(TREE_SERVICES_USER);

                if (usuario != null)
                {
                    Log.Info("User: " + usuario);
                    DQKpisController cDQKpis = new DQKpisController();
                    DQKpisGroupsController cDQKpisGroups = new DQKpisGroupsController();
                    List<DQKpis> kpisActivos = cDQKpis.getActivosService(usuario.ClienteID);
                    List<Vw_DQKpis> kpisVwActivos = cDQKpis.getActivosServiceVw(usuario.ClienteID);
                    Log.Info("Listado de KPIs");
                    if (kpisActivos != null && kpisActivos.Count > 0)
                    {
                        Log.Info("Listado de KPIs: " + kpisActivos.Count);

                        foreach (DQKpis kpi in kpisActivos)
                        {
                            Vw_DQKpis vwKpi = kpisVwActivos.Find(k => k.DQKpiID == kpi.DQKpiID);


                            if (vwKpi.FechaInicio.HasValue && TreeCore.Cron.SiguienteFechaEsHoy(vwKpi.CronFormat, vwKpi.FechaInicio.Value, vwKpi.FechaFin))
                            {
                                #region Ejecución KPI
                                Log.Info($"{Comun.SERVICIO_DATA_QUALITY}: Execute KPI: {kpi.DQKpi}");

                                List<DQKpisGroups> kpiGroups = cDQKpisGroups.GetAllGroupsByKPI(kpi.DQKpiID, true);
                                List<long> GroupsIDs = new List<long>();
                                kpiGroups.ForEach(g =>
                                {
                                    if (!GroupsIDs.Contains(g.DQGroupID))
                                    {
                                        GroupsIDs.Add(g.DQGroupID);
                                    }
                                });

                                ResultDQKpi resultKpi = cDQKpis.EjecutarKPI(kpi, GroupsIDs, usuario.UsuarioID);

                                string messageMonitoring = $"KPI success({resultKpi.success.ToString()})({resultKpi.numeroElementos}/{resultKpi.total})";
                                string messageFinalLog = $"{Comun.SERVICIO_DATA_QUALITY}: {messageMonitoring}: {resultKpi.mensaje}";
                                Log.Info(messageFinalLog);
                                if (!resultKpi.success)
                                {
                                    Log.Warn(messageFinalLog);
                                }

                                Log.Error(messageFinalLog);
                                // Monitoring information

                                fechaFin = DateTime.Now;
                                cMonitoringService.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, getMyIP(), messageMonitoring, fechaInicio, fechaFin, usuario.ClienteID, resultKpi.success);

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        string message = "There are no active Kpis";
                        Log.Error(Comun.SERVICIO_DATA_QUALITY + ": " + message);
                        // Monitoring information


                        fechaFin = DateTime.Now;
                        cMonitoringService.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, getMyIP(), message, fechaInicio, fechaFin, usuario.ClienteID, true);
                    }
                }
                else
                {
                    string message = String.Format("The user with email '{0}' does not exist.(App.config parameter \"EmailUsuario\")", Comun.TREE_SERVICES_USER);
                    Log.Error(Comun.SERVICIO_DATA_QUALITY + ": " + message);
                    // Monitoring information

                    fechaFin = DateTime.Now;
                    cMonitoringService.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, getMyIP(), message, fechaInicio, fechaFin, usuario.ClienteID, true);
                }

            }
            catch (Exception ex)
            {
                Log.Error(Comun.SERVICIO_DATA_QUALITY + ": " + ex.Message);

                fechaFin = DateTime.Now;
                cMonitoringService.AgregarRegistro(Comun.SERVICIO_DATA_QUALITY, getMyIP(), ex.Message, fechaInicio, fechaFin, null, false);
            }

            Log.Info(Comun.SERVICIO_DATA_QUALITY + ": " + "End");
        }
        #endregion

        #region EjecucionServiceInventarioHistoricos
        private void EjecucionServiceInventarioHistoricos()
        {
            CoreInventarioHistoricosController cInventarioHistoricos = new CoreInventarioHistoricosController();

            try
            {
                DateTime hoy = DateTime.Now.Date;
                Log.Info("Init service Generate History of Inventory: " + hoy.ToString());
                DateTime FechaUltimoHistorico = cInventarioHistoricos.GetFechaUltimoHistorico();
                DateTime fechaGeneracion = FechaUltimoHistorico;

                for (int i = 0; hoy >= fechaGeneracion; i++)
                {
                    Log.Info("Int Generate History of Inventory: " + fechaGeneracion.ToString());
                    cInventarioHistoricos.GenerarHistorico(fechaGeneracion);
                    Log.Info("End generate History of Inventory: " + fechaGeneracion.ToString());
                    
                    fechaGeneracion = fechaGeneracion.AddDays(1);
                }

                Log.Info("End service Generate History of Inventory");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        #endregion
    }
}
