using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class BusinessProcessTipoController : GeneralBaseController<CoreBusinessProcessTipos, TreeCoreContext>
    {
        public BusinessProcessTipoController()
            : base()
        { }

        public List<CoreBusinessProcessTipos> getAllActivos()
        {
            List<CoreBusinessProcessTipos> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreBusinessProcessTipos where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }

        public string getCodigoByID (long lBusinessProcessTiposID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.CoreBusinessProcessTipos where c.CoreBusinessProcessTipoID == lBusinessProcessTiposID select c.Codigo).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sCodigo = null;
            }

            return sCodigo;
        }

    }
}