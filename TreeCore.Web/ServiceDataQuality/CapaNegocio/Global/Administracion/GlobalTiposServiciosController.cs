using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposServiciosController : GeneralBaseController<GlobalTiposServicios, TreeCoreContext>, IBasica<GlobalTiposServicios>
    {
        public GlobalTiposServiciosController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalTipoServicioID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string GlobalTipoServicio, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTiposServicios> datos = new List<GlobalTiposServicios>();


            datos = (from c in Context.GlobalTiposServicios where (c.GlobalTipoServicio == GlobalTipoServicio && c.ClienteID == clienteID) select c).ToList<GlobalTiposServicios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalTipoServicioID)
        {
            GlobalTiposServicios dato = new GlobalTiposServicios();
            GlobalTiposServiciosController cController = new GlobalTiposServiciosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalTipoServicioID == " + GlobalTipoServicioID.ToString());

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

        public GlobalTiposServicios GetDefault(long clienteID)
        {
            GlobalTiposServicios oGlobalTiposServicios;
            try
            {
                oGlobalTiposServicios = (from c in Context.GlobalTiposServicios where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalTiposServicios = null;
            }
            return oGlobalTiposServicios;
        }
    }
}