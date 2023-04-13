using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreFrecuenciasController : GeneralBaseController<CoreFrecuencias, TreeCoreContext>
    {
        public CoreFrecuenciasController()
            : base()
        { }

        public List<CoreFrecuencias> getFrecuenciasActivas()
        {
            List<CoreFrecuencias> listaFrecuencias;

            try
            {
                listaFrecuencias = (from c in Context.CoreFrecuencias where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaFrecuencias = null;
            }

            return listaFrecuencias;
        }

        public string getCodigoByID(long? lID)
        {
            string sCodigo;

            try
            {
                sCodigo = (from c in Context.CoreFrecuencias where c.CoreFrecuenciaID == lID select c.Codigo).First();
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