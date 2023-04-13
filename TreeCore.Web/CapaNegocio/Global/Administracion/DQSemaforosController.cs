using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public class DQSemaforosController : GeneralBaseController<DQSemaforos, TreeCoreContext>
    {
        public DQSemaforosController()
            : base()
        {

        }

    }
}