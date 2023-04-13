using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ProveedoresController : GeneralBaseController<Proveedores, TreeCoreContext>, IBasica<Proveedores>
    {
        public ProveedoresController()
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
            List<Proveedores> datos = new List<Proveedores>();


            datos = (from c in Context.Proveedores where (c.FacturacionDNICIF == FacturacionDNICIF && c.ClienteID == clienteID) select c).ToList<Proveedores>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long ProveedorID)
        {
            Proveedores dato = new Proveedores();
            ProveedoresController cController = new ProveedoresController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && ProveedorID == " + ProveedorID.ToString());

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

        public Proveedores getProveedorByDNI(string dni)
        {
            Proveedores dato = null;
            List<Proveedores> lista = null;

            try
            {
                lista = (from c in Context.Proveedores where c.FacturacionDNICIF == dni && c.Activo == true select c).ToList();
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

        public List<Proveedores> GetActivos()
        {
            return (from c in Context.Proveedores where c.Activo == true select c).ToList();
        }

        public Proveedores GetProveedorByCIFSAP(string cif, string sCodigoSAP)
        {
            Proveedores dato = null;
            List<Proveedores> lista = null;

            try
            {

                lista = (from c in Context.Proveedores where c.FacturacionDNICIF == cif && c.CodigoSAP == sCodigoSAP select c).ToList();
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
        public bool eliminaProveedor(long proveedorID)
        {
            bool existe = true;
            Proveedores dato;
            dato = (from c in Context.Proveedores where c.ProveedorID == proveedorID select c).First();
            try
            {
                Context.Proveedores.DeleteOnSubmit(dato);
                Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }
            return existe;
        }
    }
}