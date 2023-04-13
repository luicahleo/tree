using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosTiposController : GeneralBaseController<CoreProductCatalogServiciosTipos, TreeCoreContext>
    {
        public CoreProductCatalogServiciosTiposController()
            : base()
        { }

        public List<CoreProductCatalogServiciosTipos> getTiposActivos()
        {
            List<CoreProductCatalogServiciosTipos> listaTipos;

            try
            {
                listaTipos = (from c in Context.CoreProductCatalogServiciosTipos where c.Activo select c).ToList();
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
            List<CoreProductCatalogServiciosTipos> datos = new List<CoreProductCatalogServiciosTipos>();


            datos = (from c in Context.CoreProductCatalogServiciosTipos where (c.Nombre == lNombre ) select c).ToList<CoreProductCatalogServiciosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool RegistroDuplicadoCodigo(string lCodigo)
        {
            bool isExiste = false;
            List<CoreProductCatalogServiciosTipos> datos = new List<CoreProductCatalogServiciosTipos>();


            datos = (from c in Context.CoreProductCatalogServiciosTipos where (c.Codigo == lCodigo) select c).ToList<CoreProductCatalogServiciosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public CoreProductCatalogServiciosTipos GetDefault()
        {
            CoreProductCatalogServiciosTipos oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosTipos where c.Defecto select c).First();
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
            CoreProductCatalogServiciosTipos dato = new CoreProductCatalogServiciosTipos();
            CoreProductCatalogServiciosTiposController CCoreProductCatalogServiciosTipos = new CoreProductCatalogServiciosTiposController();
            bool defecto = false;

            dato = CCoreProductCatalogServiciosTipos.GetItem("Defecto == true && CoreProductCatalogServicioTipoID == " + lID.ToString());

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