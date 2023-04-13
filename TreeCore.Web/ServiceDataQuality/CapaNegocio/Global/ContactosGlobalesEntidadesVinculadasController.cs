using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ContactosGlobalesEntidadesVinculadasController : GeneralBaseController<Vw_ContactosGlobalesEntidadesVinculadas, TreeCoreContext>
    {
        public ContactosGlobalesEntidadesVinculadasController()
            : base()
        { }

        public List<Vw_ContactosGlobalesEntidadesVinculadas> GetContactosGlobalesVinculadosByEntidad(long entidadID)
        {
            List<Vw_ContactosGlobalesEntidadesVinculadas> datos = new List<Vw_ContactosGlobalesEntidadesVinculadas>();

            datos = (from c in Context.Vw_ContactosGlobalesEntidadesVinculadas where (c.EntidadID== entidadID) select c).ToList<Vw_ContactosGlobalesEntidadesVinculadas>();

            return datos;
        }

        public List<Vw_ContactosGlobalesEntidadesVinculadas> getContactosNoAsignadosByEmail(string sEmail, long entidadID)
        {
            List<Vw_ContactosGlobalesEntidadesVinculadas> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEntidadesVinculadas
                              where (c.Email == sEmail && c.EntidadID == entidadID) && c.Activo == true
                              select c).ToList();

                if (listaDatos == null || listaDatos.Count == 0)
                {
                    listaDatos = (from c in Context.Vw_ContactosGlobalesEntidadesVinculadas
                                  where c.Email == sEmail && c.Activo == true
                                  select c).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;

        }

        public List<Vw_ContactosGlobalesEntidadesVinculadas> getContactosNoAsignadosByTelefono(string sTelefono, long entidadID)
        {
            List<Vw_ContactosGlobalesEntidadesVinculadas> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEntidadesVinculadas
                              where c.Telefono == sTelefono && c.EntidadID == entidadID && c.Activo == true
                              select c).Distinct().ToList();

                if (listaDatos == null || listaDatos.Count == 0)
                {
                    listaDatos = (from c in Context.Vw_ContactosGlobalesEntidadesVinculadas
                                  where c.Telefono == sTelefono && c.Activo == true
                                  select c).Distinct().ToList();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;

        }

    }
}