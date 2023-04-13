using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using TreeCore;
namespace CapaNegocio
{
    public sealed class MonitoringServiciosWindowsController : GeneralBaseController<MonitoringServiciosWindows, TreeCoreContext>
    {
        public MonitoringServiciosWindowsController()
            : base()
        {

        }

        #region GESTION BASICA
        /// <summary>
        /// Muestra los servicios para una determinada integración
        /// </summary>
        public List<MonitoringServiciosWindows> GetAllByServicio(string servicio)
        {
            // Local variables
            List<MonitoringServiciosWindows> lista = null;

            try
            {
                lista = (from c in Context.MonitoringServiciosWindows where c.MonitoringServicioWindows == servicio select c).ToList();

            }
            catch (Exception ex)
            {
                lista = new List<MonitoringServiciosWindows>();
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<MonitoringServiciosWindows> GetAllByCliente(long ClienteID)
        {
            // local variables
            List<MonitoringServiciosWindows> lista = null;

            try
            {
                lista = (from c in Context.MonitoringServiciosWindows where c.ClienteID == ClienteID select c).ToList();

            }
            catch (Exception ex)
            {
                lista = new List<MonitoringServiciosWindows>();
                log.Error(ex.Message);
            }

            return lista;
        }

        #endregion

        #region AGREGAR

        public bool AgregarRegistro(string servicio, string IP, string comentarios, DateTime fechaInicio, DateTime fechaFin, long? clienteID, bool bExito)
        {
            // Local variables
            bool bResultado = false;
            MonitoringServiciosWindows ws = null;


            // Performs the operation
            try
            {
                // Prepares the object
                ws = new MonitoringServiciosWindows();

                // Takes the project type
                ws.MonitoringServicioWindows = servicio;
                ws.FechaInicio = fechaInicio;
                ws.FechaFin = fechaFin;
                ws.Comentarios = comentarios;
                ws.IP = IP;
                if (clienteID > 0)
                {
                    ws.ClienteID = clienteID;
                }
                else
                {
                    ws.ClienteID = null;
                }
                ws.Exito = bExito;


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

        #endregion

        #region POR FECHAS

        public List<MonitoringServiciosWindows> GetAllByFechas(long? ClienteID, DateTime FechaInicio, DateTime FechaFin)
        {
            List<MonitoringServiciosWindows> lista = new List<MonitoringServiciosWindows>();

            if (ClienteID != 0)
            {
                lista = (from c in Context.MonitoringServiciosWindows where c.ClienteID == ClienteID && c.FechaInicio >= FechaInicio && c.FechaFin <= FechaFin select c).ToList<MonitoringServiciosWindows>();
            }
            else
            {
                lista = (from c in Context.MonitoringServiciosWindows where c.ClienteID == null && c.FechaInicio >= FechaInicio && c.FechaFin <= FechaFin select c).ToList<MonitoringServiciosWindows>();
            }

            return lista;
        }

        #endregion
    }
}