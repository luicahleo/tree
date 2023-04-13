using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CondicionesPagosController : GeneralBaseController<CondicionesPagos, TreeCoreContext>
    {
        public CondicionesPagosController()
            : base()
        { }

        public bool RegistroVinculado(long CondicionPagoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string CondicionPago, string sCodigo, long clienteID)
        {
            bool isExiste = false;
            List<CondicionesPagos> datos = new List<CondicionesPagos>();


            datos = (from c in Context.CondicionesPagos where (c.CondicionPago == CondicionPago || c.Codigo == sCodigo) 
                     && c.ClienteID == clienteID select c).ToList<CondicionesPagos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long CondicionPagoID)
        {
            CondicionesPagos dato = new CondicionesPagos();
            CondicionesPagosController cController = new CondicionesPagosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && CondicionPagoID == " + CondicionPagoID.ToString());

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

        public CondicionesPagos GetDefault(long clienteID)
        {
            CondicionesPagos oCondiciones;
            try
            {
                oCondiciones = (from c in Context.CondicionesPagos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oCondiciones = null;
            }

            return oCondiciones;
        }

        public List<CondicionesPagos> GetCondicionesPagosByCliente(long clienteID)
        {
            List<CondicionesPagos> datos = new List<CondicionesPagos>();

            datos = (from c in Context.CondicionesPagos where (c.ClienteID == clienteID) orderby c.CondicionPago select c).ToList<CondicionesPagos>();

            return datos;
        }

        public CondicionesPagos GetCondicionPagoByNombre(string sNombre)
        {
            List<CondicionesPagos> lista = null;
            CondicionesPagos dato = null;

            try
            {

                lista = (from c in Context.CondicionesPagos where c.CondicionPago == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}