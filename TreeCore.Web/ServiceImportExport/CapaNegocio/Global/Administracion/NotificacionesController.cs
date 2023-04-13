using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class NotificacionesController : GeneralBaseController<Notificaciones, TreeCoreContext>
    {
        public NotificacionesController()
            : base()
        { }

        public bool RegistroDuplicado(string Notificacion, long clienteID)
        {
            bool isExiste = false;
            List<Notificaciones> datos = new List<Notificaciones>();


            datos = (from c in Context.Notificaciones where (c.Notificacion == Notificacion && c.ClienteID == clienteID) select c).ToList<Notificaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public List<Vw_Notificaciones> GetListaNotificacionesAEnviar(DateTime hoy)
        {
            // Local variables
            List<Vw_Notificaciones> lista = null;
            List<long> listaCadencias = null;

            // Takes the cadence
            try
            {
                NotificacionesCadenciasController cCadencia = new NotificacionesCadenciasController();
                listaCadencias = cCadencia.GetCadenciasIDAEnviar(DateTime.Now);

                if (listaCadencias != null && listaCadencias.Count > 0)
                {
                    // Gets the notifications for the given date.
                    lista = (from c in Context.Vw_Notificaciones where (c.NotificacionCadenciaID != null && listaCadencias.Contains((long)c.NotificacionCadenciaID)) && c.Activo select c).ToList();


                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return lista;
        }

        public List<Vw_Notificaciones> GetAll(long lClienteID)
        {
            List<Vw_Notificaciones> listaNotificaciones;

            try
            {
                listaNotificaciones = (from c in Context.Vw_Notificaciones where c.ClienteID == lClienteID select c).ToList();
            }
            catch (Exception)
            {
                listaNotificaciones = null;
            }

            return listaNotificaciones;
        }
    }

}