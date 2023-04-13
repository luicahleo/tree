using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;

using System.Linq;

namespace CapaNegocio
{
    public sealed class InstallTecnicaTareasController : GeneralBaseController<InstallTecnicaTareas, TreeCoreContext>
    {
        public InstallTecnicaTareasController()
            : base()
        {

        }

        /// <summary>
        /// Devuelve el precio de la tareaID
        /// </summary>
        public double devolverPrecioTarea(long pInstallTecnicaTareaID)
        {
            double precio = 0;

            precio = (from c in Context.InstallTecnicaTareas where c.InstallTecnicaTareaID == pInstallTecnicaTareaID select c).First().Precio;

            return precio;
        }

        


    }
}