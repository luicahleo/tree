using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalTipologiasController : GeneralBaseController<GlobalTipologias, TreeCoreContext>, IBasica<GlobalTipologias>
    {
        public GlobalTipologiasController()
            : base()
        { }

        public bool RegistroVinculado(long TipologiaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Tipologia, long clienteID)
        {
            bool isExiste = false;
            List<GlobalTipologias> datos = new List<GlobalTipologias>();


            datos = (from c in Context.GlobalTipologias where (c.Tipologia == Tipologia && c.ClienteID == clienteID) select c).ToList<GlobalTipologias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long TipologiaID)
        {
            GlobalTipologias dato = new GlobalTipologias();
            GlobalTipologiasController cController = new GlobalTipologiasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && TipologiaID == " + TipologiaID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }
    }
}