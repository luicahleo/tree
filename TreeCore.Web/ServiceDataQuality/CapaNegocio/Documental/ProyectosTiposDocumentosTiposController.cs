using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using System.IO;
using log4net;

namespace CapaNegocio
{
    public class ProyectosTiposDocumentosTiposController : GeneralBaseController<ProyectosTiposDocumentosTipos, TreeCoreContext>
    {
        public ProyectosTiposDocumentosTiposController() : base()
        { }
    }
}