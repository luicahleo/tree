using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalDireccionesAccesosJornadasController : GeneralBaseController<GlobalDireccionesAccesosJornadas, TreeCoreContext>
    {
        public GlobalDireccionesAccesosJornadasController()
            : base()
        { }
        
    }
}