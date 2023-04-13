using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreAtributosConfiguracionListasColumnasAdicionalesController : GeneralBaseController<CoreAtributosConfiguracionListasColumnasAdicionales, TreeCoreContext>
    {
        public CoreAtributosConfiguracionListasColumnasAdicionalesController()
            : base()
        { }

        public List<CoreAtributosConfiguracionListasColumnasAdicionales> GetColumnasAtributos(long lAtributoID)
        {
            List<CoreAtributosConfiguracionListasColumnasAdicionales> listaDatos;
            try
            {
                listaDatos = (from c in Context.CoreAtributosConfiguracionListasColumnasAdicionales where c.CoreAtributoConfiguracionID == lAtributoID orderby c.Orden select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool EliminarColumnasAtributos(long lAtributoID) {
            bool correcto = true;
            try
            {
                var listaCols = GetColumnasAtributos(lAtributoID);
                if (listaCols != null && listaCols.Count > 0)
                {
                    foreach (var oCol in listaCols)
                    {
                        if (!DeleteItem(oCol.CoreAtributoConfiguracionListaColumnaAdicionalID))
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

        public List<ColumnasModeloDatos> GetItemColumnasAtributos(long lAtributoID)
        {
            List<ColumnasModeloDatos> listaDatos;
            try
            {
                listaDatos = (from c in Context.ColumnasModeloDatos join colAtr in Context.CoreAtributosConfiguracionListasColumnasAdicionales on c.ColumnaModeloDatosID equals colAtr.ColumnaModeloDatoID where colAtr.CoreAtributoConfiguracionID == lAtributoID select c).ToList();
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