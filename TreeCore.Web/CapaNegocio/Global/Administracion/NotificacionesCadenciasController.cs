using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class NotificacionesCadenciasController : GeneralBaseController<NotificacionesCadencias, TreeCoreContext>, IBasica<NotificacionesCadencias>
    {
        public NotificacionesCadenciasController()
            : base()
        { }

        public bool RegistroVinculado(long NotificacionCadenciaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string NotificacionCadencia, long clienteID)
        {
            bool isExiste = false;
            List<NotificacionesCadencias> datos = new List<NotificacionesCadencias>();


            datos = (from c in Context.NotificacionesCadencias where (c.NotificacionCadencia == NotificacionCadencia && c.ClienteID == clienteID) select c).ToList<NotificacionesCadencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long NotificacionCadenciaID)
        {
            NotificacionesCadencias dato = new NotificacionesCadencias();
            NotificacionesCadenciasController cController = new NotificacionesCadenciasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && NotificacionCadenciaID == " + NotificacionCadenciaID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }
        public List<long> GetCadenciasIDAEnviar(DateTime hoy)
        {
            // Local variables                        
            List<long> lista = null;

            try
            {
                // Gets the date information
                string dia = hoy.ToString("dddd", new CultureInfo("en-US"));

                switch (dia.ToLower())
                {
                    case "monday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "tuesday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "wednesday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "thursday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "friday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "saturday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                    case "sunday":
                        // Verifies if there is any record linked to the one we wanna remove.
                        lista = (from c in Context.NotificacionesCadencias where (c.Lunes || c.FechaEnvio == hoy.Date || c.DiaMes == hoy.Day) && c.Activo select c.NotificacionCadenciaID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }



            // Returns the result
            return lista;
        }

        public NotificacionesCadencias GetDefault(long clienteID)
        {
            NotificacionesCadencias oNotificacion;
            try
            {
                oNotificacion = (from c 
                                 in Context.NotificacionesCadencias 
                                 where c.Defecto &&
                                        c.ClienteID == clienteID
                                 select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oNotificacion = null;
            }

            return oNotificacion;
        }
    }
}