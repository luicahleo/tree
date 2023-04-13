using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalColumnasTablasController : GeneralBaseController<ColumnasTablas, TreeCoreContext>
    {
        public GlobalColumnasTablasController()
            : base()
        { }

        public ColumnasTablas GetColumnasByNombre(string sNombre)
        {
            List<ColumnasTablas> lista = null;
            ColumnasTablas dato = null;
            lista = (from c in Context.ColumnasTablas where (c.ColumnaNombre == sNombre) select c).ToList();
            if (lista != null && lista.Count > 0)
            {
                dato = lista.ElementAt(0);
            }

            return dato;
        }
    }
}