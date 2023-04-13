using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposAccesosController : GeneralBaseController<GlobalTiposAccesos, TreeCoreContext>, IBasica<GlobalTiposAccesos>
    {
        public GlobalTiposAccesosController()
            : base()
        { }

        public bool RegistroVinculado(long TipoAccesoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoAcceso, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTiposAccesos> datos = new List<GlobalTiposAccesos>();


            datos = (from c in Context.GlobalTiposAccesos where (c.TipoAcceso == TipoAcceso && c.ClienteID == clienteID) select c).ToList<GlobalTiposAccesos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoAccesoID)
        {
            GlobalTiposAccesos dato = new GlobalTiposAccesos();
            GlobalTiposAccesosController cController = new GlobalTiposAccesosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoAccesoID == " + TipoAccesoID.ToString());

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