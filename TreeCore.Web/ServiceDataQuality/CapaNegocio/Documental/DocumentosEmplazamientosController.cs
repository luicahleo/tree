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
    public sealed class DocumentosEmplazamientosController : GeneralBaseController<Vw_DocumentosEmplazamientos, TreeCoreContext>
    {
        public DocumentosEmplazamientosController()
            : base()
        {

        }

    }
}