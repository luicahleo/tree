using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ProveedoresCuentasCorrientesController : GeneralBaseController<ProveedoresCuentasCorrientes, TreeCoreContext>, IBasica<ProveedoresCuentasCorrientes>
    {
        public ProveedoresCuentasCorrientesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string IBAN, long clienteID)
        {
            bool isExiste = false;
            List<ProveedoresCuentasCorrientes> datos = new List<ProveedoresCuentasCorrientes>();


            datos = (from c in Context.ProveedoresCuentasCorrientes where (c.IBAN == IBAN /*&& c.ClienteID == clienteID*/) select c).ToList<ProveedoresCuentasCorrientes>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProveedorCuentaCorrienteID)
        {
            ProveedoresCuentasCorrientes dato = new ProveedoresCuentasCorrientes();
            ProveedoresCuentasCorrientesController cController = new ProveedoresCuentasCorrientesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProveedorCuentaCorrienteID == " + ProveedorCuentaCorrienteID.ToString());

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

        public ProveedoresCuentasCorrientes GetCuentaPorDefectoByProveedorID(long ProveedorID)
        {

            List<ProveedoresCuentasCorrientes> lista = null;
            ProveedoresCuentasCorrientes dato = null;

            try
            {
                lista = (from c in Context.ProveedoresCuentasCorrientes where c.ProveedorID == ProveedorID && c.Defecto == true select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }
            return dato;
        }

        public ProveedoresCuentasCorrientes CuentaCorrientePorDefecto()
        {
            List<ProveedoresCuentasCorrientes> datos = null;
            datos = GetItemsList("Defecto");

            if (datos.Count > 0)
            {
                return datos[0];
            }
            else
            {
                return null;
            }

        }

        public ProveedoresCuentasCorrientes CuentaCorrientePorDefecto(long CuentaID)
        {
            List<ProveedoresCuentasCorrientes> datos = null;

            datos = (from c in Context.ProveedoresCuentasCorrientes where c.ProveedorCuentaCorrienteID == CuentaID && c.Defecto == true select c).ToList();

            if (datos.Count > 0)
            {
                return datos[0];
            }
            else
            {
                return null;
            }

        }

        public ProveedoresCuentasCorrientes GetCuentaByProveedorIBAN(long proveedorID, string sIBAN)
        {
            ProveedoresCuentasCorrientes dato = null;
            List<ProveedoresCuentasCorrientes> lista = null;

            try
            {

                lista = (from c in Context.ProveedoresCuentasCorrientes where c.ProveedorID == proveedorID && c.IBAN == sIBAN select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

        public ProveedoresCuentasCorrientes GetCuentaByProveedorDefecto(long proveedorID)
        {
            ProveedoresCuentasCorrientes dato = null;
            List<ProveedoresCuentasCorrientes> lista = null;

            try
            {

                lista = (from c in Context.ProveedoresCuentasCorrientes where c.ProveedorID == proveedorID && c.Defecto == true select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

        public ProveedoresCuentasCorrientes GetCuentaByProveedorBanco(long proveedorID, long bancoID)
        {
            ProveedoresCuentasCorrientes dato = null;
            List<ProveedoresCuentasCorrientes> lista = null;

            try
            {

                lista = (from c in Context.ProveedoresCuentasCorrientes where c.ProveedorID == proveedorID && c.BancoID == bancoID select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dato;
        }

    }
}