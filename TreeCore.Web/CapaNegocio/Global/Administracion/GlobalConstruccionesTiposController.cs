using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalConstruccionesTiposController : GeneralBaseController<GlobalConstruccionesTipos, TreeCoreContext>, IBasica<GlobalConstruccionesTipos>
    {
        public GlobalConstruccionesTiposController()
            : base()
        { }

        public bool RegistroVinculado(long ConstruccionTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string ConstruccionTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalConstruccionesTipos> datos = new List<GlobalConstruccionesTipos>();


            datos = (from c in Context.GlobalConstruccionesTipos where (c.ConstruccionTipo == ConstruccionTipo && c.ClienteID == clienteID) select c).ToList<GlobalConstruccionesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ConstruccionTipoID)
        {
            GlobalConstruccionesTipos dato = new GlobalConstruccionesTipos();
            GlobalConstruccionesTiposController cController = new GlobalConstruccionesTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ConstruccionTipoID == " + ConstruccionTipoID.ToString());

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