using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalLineasVidasTiposController : GeneralBaseController<GlobalLineasVidasTipos, TreeCoreContext>, IBasica<GlobalLineasVidasTipos>
    {
        public GlobalLineasVidasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long LineaVidaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string LineaVidaTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalLineasVidasTipos> datos = new List<GlobalLineasVidasTipos>();


            datos = (from c in Context.GlobalLineasVidasTipos where (c.LineaVidaTipo == LineaVidaTipo && c.ClienteID == clienteID) select c).ToList<GlobalLineasVidasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long LineaVidaTipoID)
        {
            GlobalLineasVidasTipos dato = new GlobalLineasVidasTipos();
            GlobalLineasVidasTiposController cController = new GlobalLineasVidasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && LineaVidaTipoID == " + LineaVidaTipoID.ToString());

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