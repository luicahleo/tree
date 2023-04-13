using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreProductCatalogFrecuenciasController : GeneralBaseController<CoreFrecuencias, TreeCoreContext>
    {
        public CoreProductCatalogFrecuenciasController()
            : base()
        { }

        public List<CoreFrecuencias> getTiposActivos()
        {
            List<CoreFrecuencias> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreFrecuencias where c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public bool RegistroDuplicado(string lNombre)
        {
            bool isExiste = false;
            List<CoreFrecuencias> datos = new List<CoreFrecuencias>();


            datos = (from c in Context.CoreFrecuencias where (c.Nombre == lNombre ) select c).ToList<CoreFrecuencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string lCodigo)
        {
            bool isExiste = false;
            List<CoreFrecuencias> datos = new List<CoreFrecuencias>();


            datos = (from c in Context.CoreFrecuencias where (c.Codigo == lCodigo) select c).ToList<CoreFrecuencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

    }
}