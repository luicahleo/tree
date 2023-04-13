using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController : GeneralBaseController<EmplazamientosAtributosConfiguracionListasColumnasAdicionales, TreeCoreContext>
    {
        public EmplazamientosAtributosConfiguracionListasColumnasAdicionalesController()
            : base()
        { }

        public List<EmplazamientosAtributosConfiguracionListasColumnasAdicionales> GetColumnasFromAtributo(long AtributoID)
        {
            List<EmplazamientosAtributosConfiguracionListasColumnasAdicionales> listaColumnas = new List<EmplazamientosAtributosConfiguracionListasColumnasAdicionales>();
            try
            {
                listaColumnas = (from c in Context.EmplazamientosAtributosConfiguracionListasColumnasAdicionales
                                 where c.EmplazamientosAtributoConfiguracionID == AtributoID
                                 orderby c.Orden
                                 select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaColumnas = null;
            }
            return listaColumnas;
        }

        public bool EliminarColumnasAtributos(long lAtributoID)
        {
            bool correcto = true;
            try
            {
                var listaCols = GetColumnasFromAtributo(lAtributoID);
                if (listaCols != null && listaCols.Count > 0)
                {
                    foreach (var oCol in listaCols)
                    {
                        if (!DeleteItem(oCol.EmplazamientoAtributoConfiguracionListaColumnaAdicionalID))
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                correcto = false;
            }
            return correcto;
        }

    }
}