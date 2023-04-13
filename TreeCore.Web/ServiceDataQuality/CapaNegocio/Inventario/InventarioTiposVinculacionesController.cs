using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class InventarioTiposVinculacionesController : GeneralBaseController<InventarioTiposVinculaciones, TreeCoreContext>
    {
        public InventarioTiposVinculacionesController()
            : base()
        {

        }


        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            bool isExiste = false;
            List<InventarioTiposVinculaciones> datos;

            datos = (from c in Context.InventarioTiposVinculaciones where (c.ClienteID == clienteID) && (c.Nombre == nombre) select c).ToList<InventarioTiposVinculaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoCodigo(string codigo, long clienteID)
        {
            bool isExiste = false;
            List<InventarioTiposVinculaciones> datos;

            datos = (from c in Context.InventarioTiposVinculaciones where (c.ClienteID == clienteID) && (c.Codigo == codigo) select c).ToList<InventarioTiposVinculaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<InventarioTiposVinculaciones> GetActivos(long lClienteID) {
            List<InventarioTiposVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioTiposVinculaciones where c.ClienteID == lClienteID && c.Activo select c).ToList(); ;
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public List<InventarioTiposVinculaciones> GetTiposFromVinculacion(long lVinculacionID) {
            List<InventarioTiposVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioTiposVinculaciones join vin in Context.InventarioCategoriasVinculacionesTiposVinculaciones on c.InventarioTipoVinculacionID equals vin.InventarioTipoVinculacionID 
                              where vin.InventarioCategoriaVinculacionID == lVinculacionID select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        public long getidTipoVinculaciones(string nombre, long clienteID)
        {
            bool isExiste = false;
            long datos;
            try
            {

                datos = (from c in Context.InventarioTiposVinculaciones where (c.ClienteID == clienteID) && (c.Nombre == nombre) select c.InventarioTipoVinculacionID).First();
            }
            catch
            {
                datos = 0;
                return datos;
            }
            return datos;
        }



    }
}
