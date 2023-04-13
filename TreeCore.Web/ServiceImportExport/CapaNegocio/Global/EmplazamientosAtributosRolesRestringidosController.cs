using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class EmplazamientosAtributosRolesRestringidosController : GeneralBaseController<EmplazamientosAtributosRolesRestringidos, TreeCoreContext>
    {
        public EmplazamientosAtributosRolesRestringidosController()
            : base()
        { }

        public List<Vw_EmplazamientosAtributosRolesRestringidos> GetRolesByRolID(long rolID)
        {
            return (from c in Context.Vw_EmplazamientosAtributosRolesRestringidos where c.RolID == rolID select c).ToList();
        }
    }
}