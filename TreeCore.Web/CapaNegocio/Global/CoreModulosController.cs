using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreModulosController : GeneralBaseController<CoreModulos, TreeCoreContext>
    {
        public CoreModulosController()
            : base()
        { }

        public CoreModulos GetModulo(string CodigoModulo) {
            CoreModulos oDato;
            try
            {
                oDato = (from c in Context.CoreModulos where c.Codigo == CodigoModulo select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }
            return oDato;
        }

    }
}