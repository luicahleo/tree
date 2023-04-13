using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class PerfilesFuncionalidadesController : GeneralBaseController<PerfilesFuncionalidades, TreeCoreContext>
    {
        public PerfilesFuncionalidadesController()
            : base()
        {

        }

        public PerfilesFuncionalidades GetRelacion(long perfilID, long funcionalidadID) {
            PerfilesFuncionalidades oDato;
            try
            {
                oDato = (from c in Context.PerfilesFuncionalidades where c.PerfilID == perfilID && c.FuncionalidadID == funcionalidadID select c).First();
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
