using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreUnidadesController : GeneralBaseController<CoreUnidades, TreeCoreContext>
    {
        public CoreUnidadesController()
            : base()
        { }

        public List<CoreUnidades> getUnidadesActivas()
        {
            List<CoreUnidades> listaUnidades;

            try
            {
                listaUnidades = (from c in Context.CoreUnidades where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUnidades = null;
            }

            return listaUnidades;
        }

        public string getCodigoByID(long? lID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.CoreUnidades where c.CoreUnidadID == lID select c.Codigo).First();
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