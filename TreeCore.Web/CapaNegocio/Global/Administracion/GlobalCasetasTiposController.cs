using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalCasetasTiposController : GeneralBaseController<GlobalCasetasTipos, TreeCoreContext>, IBasica<GlobalCasetasTipos>
    {
        public GlobalCasetasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long CasetaTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string CasetaTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalCasetasTipos> datos = new List<GlobalCasetasTipos>();


            datos = (from c in Context.GlobalCasetasTipos where (c.CasetaTipo == CasetaTipo && c.ClienteID == clienteID) select c).ToList<GlobalCasetasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long CasetaTipoID)
        {
            GlobalCasetasTipos dato = new GlobalCasetasTipos();
            GlobalCasetasTiposController cController = new GlobalCasetasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && CasetaTipoID == " + CasetaTipoID.ToString());

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