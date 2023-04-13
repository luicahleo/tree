using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioCategoriasListasColumnasAdicionalesController : GeneralBaseController<InventarioCategoriasListasColumnasAdicionales, TreeCoreContext>
    {
        public InventarioCategoriasListasColumnasAdicionalesController()
            : base()
        { }

        public bool DeleteColumnasFromCategoria(long lCategoriaID) {
            List<InventarioCategoriasListasColumnasAdicionales> listaColumnas;
            bool valido = true;
            try
            {
                listaColumnas = (from c in Context.InventarioCategoriasListasColumnasAdicionales where c.InventarioCategoriaID == lCategoriaID select c).ToList();
                foreach (var item in listaColumnas)
                {
                    if (!DeleteItem(item.InventarioCategoriaListaColumnaAdicionalID))
                    {
                        valido = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valido = false;
            }
            return valido;
        }

    }
}