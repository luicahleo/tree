using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CoreSemaforosColoresController : GeneralBaseController<CoreSemaforosColores, TreeCoreContext>
    {
        public CoreSemaforosColoresController()
            : base()
        { }

       public List<CoreSemaforosColores> GetColoresByClienteID(long ClienteID)
        {
            List<CoreSemaforosColores> lista = new List<CoreSemaforosColores>();
            try
            {
                lista = GetItemsList("ClienteID ==" + ClienteID + "&& Activo");
            }
            catch
            {
                lista = new List<CoreSemaforosColores>();
            }
            return lista;
        }

        public string GetColorDefecto(long ClienteID)
        {
            string Color;
            try
            {
                Color = (from c in Context.CoreSemaforosColores where c.Defecto == true && c.ClienteID == ClienteID select c.Nombre).FirstOrDefault();
            }
            catch
            {
                Color = null;
            }
            return Color;
        }

        public long GetColorDefectoID(long ClienteID)
        {
            long Color;
            try
            {
                Color = (from c in Context.CoreSemaforosColores where c.Defecto == true && c.ClienteID == ClienteID select c.CoreSemaforoColorID).FirstOrDefault();
            }
            catch
            {
                Color = 0;
            }
            return Color;
        }
    }
}