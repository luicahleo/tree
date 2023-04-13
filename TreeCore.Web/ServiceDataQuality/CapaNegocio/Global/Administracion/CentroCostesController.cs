using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CentroCostesController : GeneralBaseController<CentrosCostes, TreeCoreContext>, IBasica<CentrosCostes>
    {
        public CentroCostesController()
            : base()
        { }

        public bool RegistroVinculado(long CentroCosteID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string CentroCoste, long clienteID)
        {
            bool isExiste = false;
            List<CentrosCostes> datos = new List<CentrosCostes>();


            datos = (from c in Context.CentrosCostes where (c.CentroCoste == CentroCoste && c.ClienteID == clienteID) select c).ToList<CentrosCostes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long CentroCosteID)
        {
            CentrosCostes dato = new CentrosCostes();
            CentroCostesController cController = new CentroCostesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && CentroCosteID == " + CentroCosteID.ToString());

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

        public CentrosCostes GetDefault(long lClienteID)
        {
            CentrosCostes oCentro;

            try
            {
                oCentro = (from c in Context.CentrosCostes where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oCentro = null;
            }

            return oCentro;
        }
    }
}