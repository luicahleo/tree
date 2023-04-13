using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{

    public class GlobalCondicionesColumnasTablasController : GeneralBaseController<GlobalCondicionesColumnasTablas, TreeCoreContext> //, IBasica<GlobalCondicionesColumnasTablas>
    {
        public GlobalCondicionesColumnasTablasController()
            : base()
        {

        }

        public List<ColumnasModeloDatos> GetAllGlobalCondicionesColumnasTablasByTabla(long lTabla)
        {
            List<ColumnasModeloDatos> lCondiciones;

            try
            {
                lCondiciones = (from c in Context.ColumnasModeloDatos where c.TablaModeloDatosID == lTabla && c.ForeignKey == null select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lCondiciones = null;
            }
            return lCondiciones;
        }


    }
}