using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoPreventivoEmplazamientosProcesosController : GeneralBaseController<MantenimientoPreventivosEmplazamientosProcesos, TreeCoreContext>
    {
        public MantenimientoPreventivoEmplazamientosProcesosController()
            : base()
        { }
    }
}