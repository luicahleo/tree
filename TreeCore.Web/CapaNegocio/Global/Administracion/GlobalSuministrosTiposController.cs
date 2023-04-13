using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalSuministrosTiposController : GeneralBaseController<GlobalSuministrosTipos, TreeCoreContext>, IBasica<GlobalSuministrosTipos>
    {
        public GlobalSuministrosTiposController()
            : base()
        { }

        public bool RegistroVinculado(long SuministroTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SuministroTipo, long clienteID)
        {
            bool isExiste = false;
            List<GlobalSuministrosTipos> datos = new List<GlobalSuministrosTipos>();


            datos = (from c in Context.GlobalSuministrosTipos where (c.SuministroTipo == SuministroTipo && c.ClienteID == clienteID) select c).ToList<GlobalSuministrosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SuministroTipoID)
        {
            GlobalSuministrosTipos dato = new GlobalSuministrosTipos();
            GlobalSuministrosTiposController cController = new GlobalSuministrosTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SuministroTipoID == " + SuministroTipoID.ToString());

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