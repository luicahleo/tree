using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosController : GeneralBaseController<CoreProductCatalogServicios, TreeCoreContext>
    {
        public CoreProductCatalogServiciosController()
            : base()
        { }


        public bool RegistroDuplicado(string sNombre, string sCodigo)
        {
            bool isExiste = false;
            List<CoreProductCatalogServicios> listaDatos = new List<CoreProductCatalogServicios>();

            listaDatos = (from c in Context.CoreProductCatalogServicios where (c.Nombre == sNombre && c.Codigo == sCodigo) select c).ToList<CoreProductCatalogServicios>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public List<Vw_CoreProductCatalogServicios> ListaServicio()
        {
            List<Vw_CoreProductCatalogServicios> listaDatos = null;
            try
            {
                listaDatos = (from c
                              in Context.Vw_CoreProductCatalogServicios
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public string getIdentificador (long lServicioID)
        {
            string sIdentificador = null;

            try
            {
              //  sIdentificador = (from c in Context.Vw_CoreProductCatalogServicios where c.CoreProductCatalogServicioID == lServicioID select c.Identificador).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sIdentificador = null;
            }

            return sIdentificador;
        }
    }
}
