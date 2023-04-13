using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreProductCatalogUnidadesController : GeneralBaseController<CoreUnidades, TreeCoreContext>
    {
        public CoreProductCatalogUnidadesController()
            : base()
        { }

        public List<CoreUnidades> getTiposActivos()
        {
            List<CoreUnidades> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreUnidades where c.Activo select c).ToList();
            }
            catch (Exception)
            {
                listaTipos = null;
            }

            return listaTipos;
        }

        public bool RegistroDuplicado(string lNombre)
        {
            bool isExiste = false;
            List<CoreUnidades> datos = new List<CoreUnidades>();


            datos = (from c in Context.CoreUnidades where (c.Nombre == lNombre) select c).ToList<CoreUnidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string lCodigo)
        {
            bool isExiste = false;
            List<CoreUnidades> datos = new List<CoreUnidades>();


            datos = (from c in Context.CoreUnidades where (c.Codigo == lCodigo) select c).ToList<CoreUnidades>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }

}