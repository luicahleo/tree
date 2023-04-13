using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public sealed class ContactosContratosMarcosController : GeneralBaseController<ContactosContratosMarcos, TreeCoreContext>
    {
        public ContactosContratosMarcosController()
            : base()
        {

        }
        
        public bool ExisteContactbyNombreContacto(string contacto, long contratoMarcoID)
        {

            bool exite = false;
            List<ContactosContratosMarcos> datos = new List<ContactosContratosMarcos>();

            datos = (from c in Context.ContactosContratosMarcos where c.ContactoNombre == contacto && c.ContratoMarcoID == contratoMarcoID select c).ToList();
            if (datos.Count > 0)
            {
                exite = true;
            }


            return exite;


        }

        public ContactosContratosMarcos ContactoByNombreAlquiler(string contacto, long contratoMarcoID)
        {

            ContactosContratosMarcos datos = new ContactosContratosMarcos();
            try
            {
                datos = (from c in Context.ContactosContratosMarcos where c.ContactoNombre == contacto && c.ContratoMarcoID == contratoMarcoID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }


        public ContactosContratosMarcos ContactoByContratoMarco(long contratoMarcoID)
        {
            ContactosContratosMarcos contacto = new ContactosContratosMarcos();
            try
            {
                contacto = (from c in Context.ContactosContratosMarcos where c.ContratoMarcoID == contratoMarcoID select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                contacto = null;

            }
            return contacto;

        }

        


        public ContactosContratosMarcos ContactoPorDefectoByContratoMarco(long contratoMarcoID)
        {

            List<ContactosContratosMarcos> contactos = new List<ContactosContratosMarcos>();

            try
            {
                contactos = (from c in Context.ContactosContratosMarcos where c.ContratoMarcoID == contratoMarcoID && c.Activo == true && c.Defecto == true select c).ToList();

                if (contactos.Count == 0)
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                contactos = null;
            }

            return contactos[0];

        }

        #region CALIDAD

        public int GetCalidad(string sFiltro)
        {
            // Local variables
            int iResultado = 0;
            List<ContactosContratosMarcos> lista = null;

            try
            {
                lista = GetItemsList(sFiltro);
                if (lista != null)
                {
                    iResultado = lista.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return iResultado;
        }

        #endregion
    }
}