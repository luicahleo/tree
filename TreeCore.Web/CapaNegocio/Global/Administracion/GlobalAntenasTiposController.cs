using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalAntenasTiposController : GeneralBaseController<GlobalAntenasTipos, TreeCoreContext>, IBasica<GlobalAntenasTipos>
    {
        public GlobalAntenasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long AntenaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AntenaTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalAntenasTipos> datos = new List<GlobalAntenasTipos>();


            datos = (from c in Context.GlobalAntenasTipos where (c.AntenaTipo == AntenaTipo && c.ClienteID == clienteID) select c).ToList<GlobalAntenasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AntenaTipoID)
        {
            GlobalAntenasTipos dato = new GlobalAntenasTipos();
            GlobalAntenasTiposController cController = new GlobalAntenasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AntenaTipoID == " + AntenaTipoID.ToString());

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