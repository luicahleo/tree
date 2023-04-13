using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ToolServiciosController : GeneralBaseController<ToolServicios, TreeCoreContext>, IBasica<ToolServicios>
    {
        public ToolServiciosController()
            : base()
        { }

        public bool RegistroVinculado(long ClienteID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Servicio, long clienteID)
        {
            bool isExiste = false;
            List<ToolServicios> datos = new List<ToolServicios>();


            datos = (from c in Context.ToolServicios where (c.Servicio == Servicio && c.ClienteID == clienteID) select c).ToList<ToolServicios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long lServicioID)
        {
            ToolServicios oDato = new ToolServicios();
            bool bDefecto = false;

            oDato = (from c in Context.ToolServicios where c.Defecto == true && c.ServicioID == lServicioID select c).First();

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

        public ToolServicios GetServicioByName(string servicio)
        {
            // Local variables
            List<ToolServicios> lista = null;

            // Obtains the information
            lista = (from c in Context.ToolServicios where (c.Servicio == servicio) select c).ToList();

            if (lista.Count > 0)
            {
                return lista.ElementAt(0);
            }

            return null;
        }

        public ToolServicios GetDefault(long clienteID)
        {
            ToolServicios oServicio;

            try
            {
                oServicio = (from c 
                             in Context.ToolServicios 
                             where c.Defecto &&
                                    c.ClienteID == clienteID
                             select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oServicio = null;
            }

            return oServicio;
        }
    }
}