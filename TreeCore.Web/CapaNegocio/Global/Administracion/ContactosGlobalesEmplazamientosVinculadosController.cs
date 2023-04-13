using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ContactosGlobalesEmplazamientosVinculadosController : GeneralBaseController<Vw_ContactosGlobalesEmplazamientosVinculados, TreeCoreContext>
    {
        public ContactosGlobalesEmplazamientosVinculadosController()
            : base()
        { }

        public List<Vw_ContactosGlobalesEmplazamientosVinculados> GetListaContactosByEmplazamientoID(long lEmplazamientoID)
        {
            List<Vw_ContactosGlobalesEmplazamientosVinculados> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientosVinculados where c.EmplazamientoID == lEmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;

        }

        public List<Vw_ContactosGlobalesEmplazamientosVinculados> getContactosNoAsignadosByEmail(string sEmail, long EmplazamientoID)
        {
            List<Vw_ContactosGlobalesEmplazamientosVinculados> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientosVinculados
                              where (c.Email == sEmail && c.EmplazamientoID == EmplazamientoID) && c.Activo == true
                              select c).ToList();

                if (listaDatos == null || listaDatos.Count == 0)
                {
                    listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientosVinculados
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

        public List<Vw_ContactosGlobalesEmplazamientosVinculados> getContactosNoAsignadosByTelefono(string sTelefono, long EmplazamientoID)
        {
            List<Vw_ContactosGlobalesEmplazamientosVinculados> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientosVinculados
                              where c.Telefono == sTelefono && c.EmplazamientoID == EmplazamientoID && c.Activo == true
                              select c).Distinct().ToList();

                if (listaDatos == null || listaDatos.Count == 0)
                {
                    listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientosVinculados
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