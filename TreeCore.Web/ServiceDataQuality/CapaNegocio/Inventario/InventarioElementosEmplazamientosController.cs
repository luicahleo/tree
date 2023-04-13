using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using CapaNegocio;

namespace CapaNegocio
{
    public sealed class InventarioElementosEmplazamientosController : GeneralBaseController<Vw_InventarioElementosEmplazamientos, TreeCoreContext>
    {
        public InventarioElementosEmplazamientosController()
            : base()
        {

        }

    }
}