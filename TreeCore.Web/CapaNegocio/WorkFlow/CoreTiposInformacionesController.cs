using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreTiposInformacionesController : GeneralBaseController<CoreTiposInformaciones, TreeCoreContext>
    {
        public CoreTiposInformacionesController()
            : base()
        { }

        public CoreTiposInformaciones GetInformacion(string sCodigo) {
            CoreTiposInformaciones oDato;
            try
            {
                oDato = (from c in Context.CoreTiposInformaciones where c.Codigo == sCodigo select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

    }
}