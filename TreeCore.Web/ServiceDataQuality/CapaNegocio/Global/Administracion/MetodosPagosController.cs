using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MetodosPagosController : GeneralBaseController<MetodosPagos, TreeCoreContext>
    {
        public MetodosPagosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string CodigoMetodoPago, string sNombre, long clienteID)
        {
            bool isExiste = false;
            List<MetodosPagos> datos = new List<MetodosPagos>();


            datos = (from c in Context.MetodosPagos where (c.CodigoMetodoPago == CodigoMetodoPago || c.MetodoPago == sNombre)
                     && c.ClienteID == clienteID select c).ToList<MetodosPagos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long MetodoPagoID)
        {
            MetodosPagos dato = new MetodosPagos();
            MetodosPagosController cController = new MetodosPagosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && MetodoPagoID == " + MetodoPagoID.ToString());

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
        public List<MetodosPagos> GetMetodosPagoByCliente(long clienteID)
        {
            List<MetodosPagos> datos = new List<MetodosPagos>();

            datos = (from c in Context.MetodosPagos where (c.ClienteID == clienteID) orderby c.MetodoPago select c).ToList<MetodosPagos>();

            return datos;
        }

        public MetodosPagos GetDefault(long clienteID)
        {
            MetodosPagos oMetodo;
            try
            {
                oMetodo = (from c 
                           in Context.MetodosPagos 
                           where c.Defecto &&
                                c.ClienteID == clienteID
                           select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMetodo = null;
            }

            return oMetodo;
        }

        public long GetMetodoByCodigoAll(string Codigo)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.MetodosPagos where c.CodigoMetodoPago.Equals(Codigo) select c.MetodoPagoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }
    }
}