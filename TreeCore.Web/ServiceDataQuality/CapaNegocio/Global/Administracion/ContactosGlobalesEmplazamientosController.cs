using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ContactosGlobalesEmplazamientosController : GeneralBaseController<ContactosGlobalesEmplazamientos, TreeCoreContext>
    {
        public ContactosGlobalesEmplazamientosController()
            : base()
        { }

        public List<Vw_ContactosGlobalesEmplazamientos> GetContactosByEmplazamientoID()
        {
            List<Vw_ContactosGlobalesEmplazamientos> listaContactos;

            try
            {
                listaContactos = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.Activo == true orderby c.EmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public ContactosGlobalesEmplazamientos GetContactosByID(long lEmplazamientoID, long lContactoGlobalID)
        {
            ContactosGlobalesEmplazamientos oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEmplazamientos where c.ContactoGlobalID == lContactoGlobalID && c.EmplazamientoID == lEmplazamientoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public Vw_ContactosGlobalesEmplazamientos GetVistaByID(long lContactoGlobalEmplazamientoID)
        {
            Vw_ContactosGlobalesEmplazamientos oDato;

            try
            {
                oDato = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.ContactoGlobalEmplazamientoID == lContactoGlobalEmplazamientoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public List<Vw_ContactosGlobalesEmplazamientos> GetListaContactosByID(long lEmplazamientoID)
        {
            List<Vw_ContactosGlobalesEmplazamientos> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_ContactosGlobalesEmplazamientos where c.EmplazamientoID == lEmplazamientoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;

        }
        
        public bool AddUpdateRelacion(long lEmplazamientoID, long lContactoGlobalID)
        {
            List<ContactosGlobalesEmplazamientos> listaDatos;
            ContactosGlobalesEmplazamientos oDato;
            bool bAdd = false;

            try
            {
                listaDatos = (from c in Context.ContactosGlobalesEmplazamientos where c.EmplazamientoID == lEmplazamientoID && c.ContactoGlobalID == lContactoGlobalID select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    bAdd = true;
                }
                else
                {
                    oDato = this.AddItem(new ContactosGlobalesEmplazamientos { EmplazamientoID = lEmplazamientoID, ContactoGlobalID = lContactoGlobalID });
                    bAdd = (oDato != null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                bAdd = false;
            }

            return bAdd;

        }
    }
}