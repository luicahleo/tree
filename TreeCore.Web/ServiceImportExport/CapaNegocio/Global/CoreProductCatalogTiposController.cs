using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CoreProductCatalogTiposController : GeneralBaseController<CoreProductCatalogTipos, TreeCoreContext>
    {
        public CoreProductCatalogTiposController()
            : base()
        { }

        public List<CoreProductCatalogTipos> GetAllCoreProductCatalogTiposByClienteID(long CliID)
        {
            // Local variables
            List<CoreProductCatalogTipos> lista = null;
            try
            {
                lista = (from c in Context.CoreProductCatalogTipos where c.Activo && c.ClienteID == CliID select c).ToList<CoreProductCatalogTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public bool RegistroDuplicado(string lNombre)
        {
            bool isExiste = false;
            List<CoreProductCatalogTipos> datos = new List<CoreProductCatalogTipos>();


            datos = (from c in Context.CoreProductCatalogTipos where (c.Nombre == lNombre) select c).ToList<CoreProductCatalogTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string lCodigo)
        {
            bool isExiste = false;
            List<CoreProductCatalogTipos> datos = new List<CoreProductCatalogTipos>();


            datos = (from c in Context.CoreProductCatalogTipos where (c.Codigo == lCodigo) select c).ToList<CoreProductCatalogTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreProductCatalogTipos GetDefault()
        {
            CoreProductCatalogTipos oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public bool RegistroDefecto(long lID)
        {
            CoreProductCatalogTipos dato = new CoreProductCatalogTipos();
            CoreProductCatalogTiposController CCoreProductCatalogTipos = new CoreProductCatalogTiposController();
            bool defecto = false;

            dato = CCoreProductCatalogTipos.GetItem("Defecto == true && CoreProductCatalogTipoID == " + lID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }
    }
}