using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ToolIntegracionesController : GeneralBaseController<ToolIntegraciones, TreeCoreContext>
    {
        public ToolIntegracionesController()
            : base()
        { }

        public bool RegistroVinculado(long etiqIDPrincipal)
        {
            bool bExiste = true;
         

            return bExiste;
        }

        public bool RegistroDuplicado(string Codigo, long lClienteID)
        {
            bool bExiste = false;
            List<ToolIntegraciones> listaDatos;


            listaDatos = (from c in Context.ToolIntegraciones where (c.Codigo == Codigo && c.ClienteID == lClienteID) select c).ToList<ToolIntegraciones>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

    }
}