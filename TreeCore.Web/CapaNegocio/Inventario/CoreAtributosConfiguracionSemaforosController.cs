using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreAtributosConfiguracionSemaforosController : GeneralBaseController<CoreAtributosConfiguracionSemaforos, TreeCoreContext>
    {

        public CoreAtributosConfiguracionSemaforosController()
           : base()
        { }

        public List<CoreAtributosConfiguracionSemaforos> ExisteSemaforoByConfiguracion(long AtributoID)
        {
            List<CoreAtributosConfiguracionSemaforos> lista = new List<CoreAtributosConfiguracionSemaforos>();
            try
            {
                lista = (from c in Context.CoreAtributosConfiguracionSemaforos where c.CoreAtributoConfiguracionID == AtributoID select c).ToList().GroupBy(p => new { p.CoreSemaforoID }).Select(cl => cl.First()).ToList();
                    
            }
            catch
            {
                lista = null;
            }
            return lista;
        }
    }
}