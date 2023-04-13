using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class ContactosGlobalesController : GeneralBaseController<ContactosGlobales, TreeCoreContext>
    {
        public ContactosGlobalesController()
            : base()
        { }

        public List<ContactosGlobales> GetActivos()
        {
            List<ContactosGlobales> listaContactos;

            try
            {
                listaContactos = (from c in Context.ContactosGlobales where c.Activo == true select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public List<ContactosGlobales> getAll()
        {
            List<ContactosGlobales> listaContactos;

            try
            {
                listaContactos = (from c in Context.ContactosGlobales orderby c.ContactoGlobalID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public bool ContactoDuplicado(string sTelefono, string sEmail)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Telefono == sTelefono ||
                                 c.Email == sEmail)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public bool ContactoDuplicadoID(string sTelefono, string sEmail, long sContacto)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where ((c.Telefono == sTelefono || c.Email == sEmail) && c.ContactoGlobalID != sContacto)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool DuplicadoID(string sTelefono, string sEmail)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Telefono == sTelefono || c.Email == sEmail)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool ContactoDuplicadoEmail(string sEmail, long sContacto)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Email == sEmail && c.ContactoGlobalID != sContacto)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool DuplicadoEmail(string sEmail)
        {
            bool isExiste = false;
            List<ContactosGlobales> listaDatos;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where c.Email == sEmail select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public ContactosGlobales getContactoByID(long lContactoID)
        {
            ContactosGlobales oContacto;

            try
            {
                oContacto = (from c in Context.ContactosGlobales where c.ContactoGlobalID == lContactoID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public List<ContactosGlobales> getContactosByID(long lContactoID)
        {
            List<ContactosGlobales> listaContactos;

            try
            {
                listaContactos = (from c in Context.ContactosGlobales where c.ContactoGlobalID == lContactoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaContactos = null;
            }

            return listaContactos;
        }

        public bool tieneRegistrosAsociados(long lContactoGlobalID)
        {
            bool bTiene = false;
            int iRegistros = 0;

            iRegistros = (from c in Context.ContactosGlobalesEmplazamientos where c.ContactoGlobalID == lContactoGlobalID select c).Count();

            if (iRegistros > 0)
            {
                bTiene = true;
            }

            return bTiene;
        }

        public ContactosGlobales GetContactoByTelefonoEmail(string sTelefono, string sEmail)
        {
           
            List<ContactosGlobales> listaDatos;
            ContactosGlobales dato = null;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Telefono == sTelefono ||
                                 c.Email == sEmail)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                dato = listaDatos[0];
            }

            return dato;
        }

        public ContactosGlobales GetContactoByEmail(string sEmail, long lClienteID)
        {
           
            List<ContactosGlobales> listaDatos;
            ContactosGlobales dato = null;

            listaDatos = (from c
                     in Context.ContactosGlobales
                          where (c.Email == sEmail && c.ClienteID == lClienteID)
                          select c).ToList<ContactosGlobales>();

            if (listaDatos.Count > 0)
            {
                dato = listaDatos[0];
            }

            return dato;
        }

    }
}