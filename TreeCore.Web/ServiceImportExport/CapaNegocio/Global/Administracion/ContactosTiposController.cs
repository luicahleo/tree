using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ContactosTiposController : GeneralBaseController<ContactosTipos, TreeCoreContext>
    {
        public ContactosTiposController()
            : base()
        { }

        public List<ContactosTipos> GetAllContactosTipos()
        {
            // Local variables
            List<ContactosTipos> lista = null;
            try
            {
                lista = (from c in Context.ContactosTipos select c).ToList<ContactosTipos>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return lista;
        }

        public List<ContactosTipos> GetAllTipoDatos()
        {
            List<ContactosTipos> lista = new List<ContactosTipos>();
            lista = (from c in Context.ContactosTipos orderby c.ContactoTipo ascending select c).ToList();

            return lista;
        }

        public ContactosTipos GetDefault()
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.Defecto select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public ContactosTipos GetTipobyNombre(string sTipo, long lClienteID)
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.ClienteID == lClienteID && c.ContactoTipo == sTipo select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public ContactosTipos GetDefaultByClienteID(long clienteID)
        {
            ContactosTipos oContacto;

            try
            {
                oContacto = (from c in Context.ContactosTipos where c.Defecto && c.ClienteID==clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oContacto = null;
            }

            return oContacto;
        }

        public List<ContactosTipos> GetActivos(long clienteID)
        {
            List<ContactosTipos> listaTipos;

            try
            {
                listaTipos = (from c in Context.ContactosTipos
                              where c.Activo &&
                                    c.ClienteID == clienteID
                              orderby c.ContactoTipo
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public bool RegistroDuplicado(string contactoTipo, long clienteID)
        {
            bool isExiste = false;
            List<ContactosTipos> datos = new List<ContactosTipos>();


            datos = (from c in Context.ContactosTipos where (c.ContactoTipo == contactoTipo && c.ClienteID == clienteID) select c).ToList<ContactosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public String getTipoByID (long? lContactoTipoID)
        {
            String sDato;

            try
            {
                sDato = (from c in Context.ContactosTipos where c.ContactoTipoID == lContactoTipoID select c.ContactoTipo).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sDato = null;
            }

            return sDato;
        }

        public List<String> ListContactosTipos()
        {
            return (from c in Context.ContactosTipos select c.ContactoTipo).ToList();
        }

    }
}