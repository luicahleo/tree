using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class FacturasPrevisionesDetallesController : GeneralBaseController<FacturasPrevisionesDetalles, TreeCoreContext>, IBasica<FacturasPrevisionesDetalles>
    {
        public FacturasPrevisionesDetallesController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string CodigoUnico, long clienteID)
        {
            bool isExiste = false;
            List<FacturasPrevisionesDetalles> datos = new List<FacturasPrevisionesDetalles>();


            datos = (from c in Context.FacturasPrevisionesDetalles where (c.CodigoUnico == CodigoUnico /*&& c.ClienteID == clienteID*/) select c).ToList<FacturasPrevisionesDetalles>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerDetalleID)
        {
            FacturasPrevisionesDetalles dato = new FacturasPrevisionesDetalles();
            FacturasPrevisionesDetallesController cController = new FacturasPrevisionesDetallesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerDetalleID == " + AlquilerDetalleID.ToString());

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

        public bool SospenderPagosByProveedor(long provID, bool Sospendido)
        {
            List<long> lista = new List<long>();
            List<FacturasPrevisionesDetalles> Pagos = new List<FacturasPrevisionesDetalles>();
            bool Resultado = false;


            try
            {
                lista = (from c in Context.Vw_AlquileresDetallesPrevisionesPagos where c.ProveedorID == provID && Convert.ToDateTime(c.FechaPrevistaPagoCobro) >= DateTime.Now select c.FacturaPrevisionDetalleID).ToList();
                Pagos = (from c in Context.FacturasPrevisionesDetalles where lista.Contains((long)c.FacturaPrevisionDetalleID) select c).ToList();
                foreach (FacturasPrevisionesDetalles p in Pagos)
                {
                    p.Suspendido = Sospendido;
                    UpdateItem(p);
                    Resultado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<long>();
                Resultado = false;

            }

            return Resultado;
        }

        public bool SupeditarPagosByProveedor(long provID, bool Supeditado)
        {
            List<long> lista = new List<long>();
            List<FacturasPrevisionesDetalles> Pagos = new List<FacturasPrevisionesDetalles>();
            bool Resultado = false;


            try
            {
                lista = (from c in Context.Vw_AlquileresDetallesPrevisionesPagos where c.ProveedorID == provID && Convert.ToDateTime(c.FechaPrevistaPagoCobro) >= DateTime.Now select c.FacturaPrevisionDetalleID).ToList();
                Pagos = (from c in Context.FacturasPrevisionesDetalles where lista.Contains((long)c.FacturaPrevisionDetalleID) select c).ToList();
                foreach (FacturasPrevisionesDetalles p in Pagos)
                {
                    p.Supeditado = Supeditado;
                    UpdateItem(p);
                    Resultado = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<long>();
                Resultado = false;

            }

            return Resultado;
        }
    }
}