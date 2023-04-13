using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CoreProductCatalogsController : GeneralBaseController<CoreProductCatalogs, TreeCoreContext>
    {
        public CoreProductCatalogsController()
            : base()
        { }

        public bool RegistroDuplicado(string Nombre,string Codigo, long clienteID)
        {
            bool isExiste = false;
            List<CoreProductCatalogs> datos;

            datos = (from c in Context.CoreProductCatalogs where (c.Codigo == Codigo && c.Nombre == Nombre) && c.ClienteID == clienteID select c).ToList<CoreProductCatalogs>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public string getEntidad()
        {
            string datos = "";
            try
            {
               // datos = (from c in Context.Vw_CoreProductCatalogs select c.NombreEntidad).ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = "";
            }



            return datos;
        }

        public Vw_CoreProductCatalogs getVistaByID (long lID)
        {
            Vw_CoreProductCatalogs oDato;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogs where c.CoreProductCatalogID == lID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

    }
}