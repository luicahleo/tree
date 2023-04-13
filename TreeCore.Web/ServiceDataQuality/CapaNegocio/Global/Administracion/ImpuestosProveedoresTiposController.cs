using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ImpuestosProveedoresTiposController : GeneralBaseController<ImpuestosProveedoresTipos, TreeCoreContext>
    {
        public ImpuestosProveedoresTiposController()
            : base()
        { }

        public long getImpuestoProveedorTipoID(long pImpuestoID, long pImpuestoProveedorTipoID)
        {
            List<long> datos = new List<long>();

            datos = (from c in Context.ImpuestosProveedoresTipos where c.ImpuestoID == pImpuestoID && c.ImpuestoProveedorTipoID == pImpuestoProveedorTipoID select c.ImpuestoProveedorTipoID).ToList();

            if (datos.Count > 0)
            {
                return datos.First();
            }
            else
            {
                return 0;
            }
        }
    }
}