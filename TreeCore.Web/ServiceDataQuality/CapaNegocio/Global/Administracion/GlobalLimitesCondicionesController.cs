using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalLimitesCondicionesController : GeneralBaseController<GlobalLimitesCondiciones, TreeCoreContext>
    {
        public GlobalLimitesCondicionesController()
            : base()
        { }

        public bool RegistroDuplicadoDetalle(string NombreCondicion)
        {
            bool isExiste = false;
            List<GlobalLimitesCondiciones> datos = new List<GlobalLimitesCondiciones>();

            datos = (from c in Context.GlobalLimitesCondiciones where (c.NombreCondicion == NombreCondicion) select c).ToList<GlobalLimitesCondiciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoDetalle(string NombreCondicion, long limiteID)
        {
            bool isExiste = false;
            List<GlobalLimitesCondiciones> datos = new List<GlobalLimitesCondiciones>();

            datos = (from c in Context.GlobalLimitesCondiciones where (c.NombreCondicion == NombreCondicion) && c.GlobalLimiteID == limiteID select c).ToList<GlobalLimitesCondiciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }


        public GlobalLimitesCondiciones GetDefault(long limiteID)
        {
            GlobalLimitesCondiciones oGlobalLimites;
            try
            {
                oGlobalLimites = (from c in Context.GlobalLimitesCondiciones where c.Defecto && c.GlobalLimiteID == limiteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGlobalLimites = null;
            }
            return oGlobalLimites;
        }
    }
}