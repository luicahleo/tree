using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposTransmisionesController : GeneralBaseController<GlobalTiposTransmisiones, TreeCoreContext>, IBasica<GlobalTiposTransmisiones>
    {
        public GlobalTiposTransmisionesController()
            : base()
        { }

        public bool RegistroVinculado(long TipoTransmisionID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoTransmision, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTiposTransmisiones> datos = new List<GlobalTiposTransmisiones>();


            datos = (from c in Context.GlobalTiposTransmisiones where (c.TipoTransmision == TipoTransmision && c.ClienteID == clienteID) select c).ToList<GlobalTiposTransmisiones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoTransmisionID)
        {
            GlobalTiposTransmisiones dato = new GlobalTiposTransmisiones();
            GlobalTiposTransmisionesController cController = new GlobalTiposTransmisionesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoTransmisionID == " + TipoTransmisionID.ToString());

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
    }
}