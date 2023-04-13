using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioCategoriasVinculacionesTiposVinculacionesController : GeneralBaseController<InventarioCategoriasVinculacionesTiposVinculaciones, TreeCoreContext>
    {
        public InventarioCategoriasVinculacionesTiposVinculacionesController()
            : base()
        { }

        public bool DeleteTiposVinculacionesFromVinculacion(long lVinculacionID) {
            bool bCorrecto = true;
            try
            {
                var listaDatos = (from c in Context.InventarioCategoriasVinculacionesTiposVinculaciones where c.InventarioCategoriaVinculacionID == lVinculacionID select c).ToList();
                foreach (var item in listaDatos)
                {
                    if (!DeleteItem(item.InventarioCategoriaVinculacionTipoVinculacionID))
                    {
                        bCorrecto = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bCorrecto = false;
            }
            return bCorrecto;
        }

        public List<InventarioCategoriasVinculacionesTiposVinculaciones> GetTiposFromVinculaciones(long lVinculacionID) {
            List<InventarioCategoriasVinculacionesTiposVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculacionesTiposVinculaciones where c.InventarioCategoriaVinculacionID == lVinculacionID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

    }
}