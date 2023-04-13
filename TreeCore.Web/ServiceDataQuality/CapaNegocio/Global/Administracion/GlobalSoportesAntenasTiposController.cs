using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalSoportesAntenasTiposController : GeneralBaseController<GlobalSoportesAntenasTipos, TreeCoreContext>, IBasica<GlobalSoportesAntenasTipos>
    {
        public GlobalSoportesAntenasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long SoporteAntenaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SoporteAntenaTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalSoportesAntenasTipos> datos = new List<GlobalSoportesAntenasTipos>();


            datos = (from c in Context.GlobalSoportesAntenasTipos where (c.SoporteAntenaTipo == SoporteAntenaTipo && c.ClienteID == clienteID) select c).ToList<GlobalSoportesAntenasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SoporteAntenaTipoID)
        {
            GlobalSoportesAntenasTipos dato = new GlobalSoportesAntenasTipos();
            GlobalSoportesAntenasTiposController cController = new GlobalSoportesAntenasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SoporteAntenaTipoID == " + SoporteAntenaTipoID.ToString());

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