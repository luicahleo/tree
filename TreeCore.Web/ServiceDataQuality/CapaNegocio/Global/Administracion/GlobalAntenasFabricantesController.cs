using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalAntenasFabricantesController : GeneralBaseController<GlobalAntenasFabricantes, TreeCoreContext>, IBasica<GlobalAntenasFabricantes>
    {
        public GlobalAntenasFabricantesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Fabricante, long clienteID)
        {
            bool isExiste = false;
            List<GlobalAntenasFabricantes> datos = new List<GlobalAntenasFabricantes>();


            datos = (from c in Context.GlobalAntenasFabricantes where (c.Fabricante == Fabricante && c.ClienteID == clienteID) select c).ToList<GlobalAntenasFabricantes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalAntenaFabricanteID)
        {
            GlobalAntenasFabricantes dato = new GlobalAntenasFabricantes();
            GlobalAntenasFabricantesController cController = new GlobalAntenasFabricantesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalAntenaFabricanteID == " + GlobalAntenaFabricanteID.ToString());

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