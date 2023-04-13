using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalColumnasModeloDatosController : GeneralBaseController<ColumnasModeloDatos, TreeCoreContext>
    {
        public GlobalColumnasModeloDatosController()
            : base()
        { }

        public bool RegistroDuplicadoByNombre(string NombreColumna, long MaestroID)
        {
            bool isExiste = false;
            List<ColumnasModeloDatos> datos = new List<ColumnasModeloDatos>();


            datos = (from c in Context.ColumnasModeloDatos where c.NombreColumna == NombreColumna && c.TablaModeloDatosID == MaestroID select c).ToList<ColumnasModeloDatos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}