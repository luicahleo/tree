using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public sealed class ContactosController : GeneralBaseController<Contactos, TreeCoreContext>
    {
        public ContactosController()
            : base()
        {

        }



        public bool tieneRegistrosAsociado(long AlquilerID)
        {
            bool tiene = false;
            EmplazamientosController fControl = new EmplazamientosController();
            List<Emplazamientos> datos;
            datos = fControl.GetItemsList("AlquilerID == " + AlquilerID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }
            return tiene;
        }

        public bool ExisteContactbyNombreContacto(string contacto, long AlquilerID)
        {

            bool exite = false;
            List<Contactos> datos = new List<Contactos>();

            datos = (from c in Context.Contactos where c.ContactoNombre == contacto && c.AlquilerID == AlquilerID select c).ToList();
            if (datos.Count > 0)
            {
                exite = true;
            }


            return exite;


        }

        public Contactos GetContactbyNombreContactoCompleto(string nombreContacto, string apellidoContacto, long AlquilerID)
        {

            List<Contactos> datos = new List<Contactos>();
            Contactos dato = null;

            datos = (from c in Context.Contactos where c.ContactoNombre == nombreContacto && c.ContactoApellidos == apellidoContacto && c.AlquilerID == AlquilerID select c).ToList();

            if (datos.Count == 1)
            {
                dato = datos[0];
            }

            return dato;


        }


        public Contactos GetContactbyDefault(long AlquilerID)
        {

            List<Contactos> lista = new List<Contactos>();
            Contactos dato = null;

            try
            {
                lista = (from c in Context.Contactos where c.Defecto == true && c.AlquilerID == AlquilerID select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return dato;


        }

        public Contactos GetContactbyEmail(string sEmail)
        {

            List<Contactos> lista = new List<Contactos>();
            Contactos dato = null;

            try
            {
                lista = (from c in Context.Contactos where c.ContactoEmail == sEmail select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return dato;


        }



    }
}