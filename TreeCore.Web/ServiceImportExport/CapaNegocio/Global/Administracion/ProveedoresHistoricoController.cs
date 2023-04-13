using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ProveedoresHistoricoController : GeneralBaseController<ProveedoresHistorico, TreeCoreContext>, IBasica<ProveedoresHistorico>
    {
        public ProveedoresHistoricoController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string FacturacionDNICIF, long clienteID)
        {
            bool isExiste = false;
            List<ProveedoresHistorico> datos = new List<ProveedoresHistorico>();


            datos = (from c in Context.ProveedoresHistorico where (c.FacturacionDNICIF == FacturacionDNICIF/* && c.ClienteID == clienteID*/) select c).ToList<ProveedoresHistorico>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AreaFuncionalID)
        {
            ProveedoresHistorico dato = new ProveedoresHistorico();
            ProveedoresHistoricoController cController = new ProveedoresHistoricoController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AreaFuncionalID == " + AreaFuncionalID.ToString());

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