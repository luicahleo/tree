using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public sealed class AppDireccionesController : GeneralBaseController<AppDirecciones, TreeCoreContext>
    {
        public AppDireccionesController()
        {

        }

        public AppDirecciones GetDireccionByCodigo(string codigo)
        {
            return (from c in Context.AppDirecciones where c.CodigoCliente == codigo select c).SingleOrDefault();
        }

    }
}