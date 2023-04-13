using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MonitoringServiciosTreeController : GeneralBaseController<MonitoringServiciosTREE, TreeCoreContext>
    {
        public MonitoringServiciosTreeController()
            : base()
        { }

        public bool RegistroVinculado(long ClienteID)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        

        public bool RegistroDefecto(long ClienteID)
        {
            MonitoringServiciosTREE oDato;
            MonitoringServiciosTreeController cController = new MonitoringServiciosTreeController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && ClienteID == " + ClienteID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        #region AGREGAR

        public bool AgregarRegistro(string servicio, string Version, DateTime fechaInicio, DateTime? fechaFin, bool activo, bool desplegado, long? PadreID, int cadencia)
        {
            // Local variables
            bool bResultado = false;
            MonitoringServiciosTREE ws = null;


            // Performs the operation
            try
            {
                // Prepares the object
                ws = new MonitoringServiciosTREE();

                // Takes the project type
                ws.MonitoringServicioTREE = servicio;
                if (fechaInicio != DateTime.MinValue)
                {
                    ws.FechaInicio = fechaInicio;

                }
                if (fechaFin != null)
                {
                    ws.FechaParada = Convert.ToDateTime(fechaFin);
                }

                ws.Version = Version;
                ws.Activo = activo;
                ws.Desplegado = desplegado;
                ws.MonitoringServicioTREEPadreID = PadreID;
                ws.Cadencias = cadencia;

                if (AddItem(ws) != null)
                {
                    bResultado = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            // Returns the result
            return bResultado;

        }

        public DateTime getUltimoRegistroFechaIni(string Servicio)
        {
            DateTime fechainicio = DateTime.MinValue;
            try
            {

                fechainicio = (from c in Context.MonitoringServiciosTREE where c.MonitoringServicioTREE == Servicio orderby c.FechaInicio descending select c.FechaInicio).FirstOrDefault();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return fechainicio;
        }


        public MonitoringServiciosTREE getRegistroPadre(string Servicio)
        {
            MonitoringServiciosTREE itempadre = new MonitoringServiciosTREE();
            try
            {
                itempadre = (from c in Context.MonitoringServiciosTREE where c.MonitoringServicioTREEPadreID == null && c.MonitoringServicioTREE == Servicio select c).FirstOrDefault();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return itempadre;
        }
        #endregion

    }
}