using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalLimitesController : GeneralBaseController<GlobalLimites, TreeCoreContext>
    {
        public GlobalLimitesController()
            : base()
        { }

        public bool RegistroDuplicado(string NombreLimite)
        {
            bool isExiste = false;
            List<GlobalLimites> datos = new List<GlobalLimites>();


            datos = (from c in Context.GlobalLimites where (c.NombreLimite == NombreLimite) select c).ToList<GlobalLimites>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicado(string NombreLimite, long clienteID)
        {
            bool isExiste = false;
            List<GlobalLimites> datos;
            datos = (from c in Context.GlobalLimites where (c.NombreLimite == NombreLimite) && c.ClienteID == clienteID select c).ToList<GlobalLimites>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public GlobalLimites GetDefault(long clienteid)
        {
            GlobalLimites oGlobalLimites;
            try
            {
                oGlobalLimites = (from c in Context.GlobalLimites where c.Defecto && c.ClienteID == clienteid select c).First();
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