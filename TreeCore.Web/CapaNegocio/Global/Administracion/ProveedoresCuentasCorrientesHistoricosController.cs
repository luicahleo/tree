using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ProveedoresCuentasCorrientesHistoricosController : GeneralBaseController<ProveedoresCuentasCorrientesHistoricos, TreeCoreContext>, IBasica<ProveedoresCuentasCorrientesHistoricos>
    {
        public ProveedoresCuentasCorrientesHistoricosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Proveedor, long clienteID)
        {
            bool isExiste = false;
            List<ProveedoresCuentasCorrientesHistoricos> datos = new List<ProveedoresCuentasCorrientesHistoricos>();


            datos = (from c in Context.ProveedoresCuentasCorrientesHistoricos where (c.Proveedor == Proveedor /*&& c.ClienteID == clienteID*/) select c).ToList<ProveedoresCuentasCorrientesHistoricos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProveedorCuentaCorrienteHistoricoID)
        {
            ProveedoresCuentasCorrientesHistoricos dato = new ProveedoresCuentasCorrientesHistoricos();
            ProveedoresCuentasCorrientesHistoricosController cController = new ProveedoresCuentasCorrientesHistoricosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProveedorCuentaCorrienteHistoricoID == " + ProveedorCuentaCorrienteHistoricoID.ToString());

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