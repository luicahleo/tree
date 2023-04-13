using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreAtributosConfiguracionTiposDatosPropiedadesController : GeneralBaseController<CoreAtributosConfiguracionTiposDatosPropiedades, TreeCoreContext>
    {
        public CoreAtributosConfiguracionTiposDatosPropiedadesController()
            : base()
        { }

        public List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades> GetVwPropiedadesFromAtributo(long lAtributoID) {
            List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionID == lAtributoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<TiposDatosPropiedades> GetPropiedadesLibresAtributo(long AtributoID, long TipoDatoID)
        {
            List<TiposDatosPropiedades> listaDatos;
            try
            {
                List<long> asignados = (from c in Context.CoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionID == AtributoID select c.TipoDatoPropiedadID).ToList();
                listaDatos = (from c in Context.TiposDatosPropiedades where c.TipoDatoID == TipoDatoID && !(asignados.Contains(c.TipoDatoPropiedadID)) select c).ToList();
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