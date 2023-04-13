using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalCerradurasTiposController : GeneralBaseController<GlobalCerradurasTipos, TreeCoreContext>, IBasica<GlobalCerradurasTipos>
    {
        public GlobalCerradurasTiposController()
            : base()
        { }

        public bool RegistroVinculado(long CerraduraTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string CerraduraTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalCerradurasTipos> datos = new List<GlobalCerradurasTipos>();


            datos = (from c in Context.GlobalCerradurasTipos where (c.CerraduraTipo == CerraduraTipo && c.ClienteID == clienteID) select c).ToList<GlobalCerradurasTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long CerraduraTipoID)
        {
            GlobalCerradurasTipos dato = new GlobalCerradurasTipos();
            GlobalCerradurasTiposController cController = new GlobalCerradurasTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && CerraduraTipoID == " + CerraduraTipoID.ToString());

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