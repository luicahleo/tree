using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTiposGasesController : GeneralBaseController<GlobalTiposGases, TreeCoreContext>, IBasica<GlobalTiposGases>
    {
        public GlobalTiposGasesController()
            : base()
        { }

        public bool RegistroVinculado(long TipoGasID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string TipoGas, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTiposGases> datos = new List<GlobalTiposGases>();


            datos = (from c in Context.GlobalTiposGases where (c.TipoGas == TipoGas && c.ClienteID == clienteID) select c).ToList<GlobalTiposGases>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipoGasID)
        {
            GlobalTiposGases dato = new GlobalTiposGases();
            GlobalTiposGasesController cController = new GlobalTiposGasesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipoGasID == " + TipoGasID.ToString());

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