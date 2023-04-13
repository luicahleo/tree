using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ContactosGlobalesEntidadesController : GeneralBaseController<ContactosGlobalesEntidades, TreeCoreContext>
    {
        public ContactosGlobalesEntidadesController()
            : base()
        { }

        public List<Vw_ContactosGlobalesEntidades> GetContactosGlobalesByEntidad(long entidadID,long clienteID)
        {
            List<Vw_ContactosGlobalesEntidades> datos = new List<Vw_ContactosGlobalesEntidades>();

            datos = (from c in Context.Vw_ContactosGlobalesEntidades where (c.EntidadID== entidadID) && (c.ClienteID == clienteID) select c).ToList<Vw_ContactosGlobalesEntidades>();

            return datos;
        }
        public ContactosGlobalesEntidades GetContactoGlobalEntidadByIDs(long lEntidadID, long lContactoGlobalID)
        {
            ContactosGlobalesEntidades oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEntidades where c.ContactoGlobalID == lContactoGlobalID && c.EntidadID == lEntidadID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public ContactosGlobalesEntidades GetContactosByID(long lEntidadID, long lContactoGlobalID)
        {
            ContactosGlobalesEntidades oDato;

            try
            {
                oDato = (from c in Context.ContactosGlobalesEntidades where c.ContactoGlobalID == lContactoGlobalID && c.EntidadID == lEntidadID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;

        }

        public bool AddUpdateRelacion(long lEntidadID, long lContactoGlobalID)
        {
            List<ContactosGlobalesEntidades> listaDatos;
            ContactosGlobalesEntidades oDato;
            bool bAdd = false;

            try
            {
                listaDatos = (from c in Context.ContactosGlobalesEntidades where c.EntidadID == lEntidadID && c.ContactoGlobalID == lContactoGlobalID select c).ToList();
                if (listaDatos != null && listaDatos.Count > 0)
                {
                    bAdd = true;
                }
                else
                {
                    oDato = this.AddItem(new ContactosGlobalesEntidades { EntidadID = lEntidadID, ContactoGlobalID = lContactoGlobalID });
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