using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Transactions;

namespace CapaNegocio
{
    public class CoreTareasAccionesController : GeneralBaseController<CoreTareasAcciones, TreeCoreContext>
    {
        public CoreTareasAccionesController()
            : base()
        { }

        public CoreTareasAcciones getObjetoByCodigo (string sCodigo)
        {
            CoreTareasAcciones oAccion;

            try
            {
                oAccion = (from c in Context.CoreTareasAcciones where c.Codigo == sCodigo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oAccion = null;
            }

            return oAccion;
        }
    }
}