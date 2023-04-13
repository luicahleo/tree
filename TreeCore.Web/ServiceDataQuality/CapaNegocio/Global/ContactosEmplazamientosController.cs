using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using CapaNegocio;

namespace CapaNegocio
{
    public sealed class ContactosEmplazamientosController : GeneralBaseController<ContactosEmplazamientos, TreeCoreContext>
    {
        public ContactosEmplazamientosController()
            : base()
        {

        }



        public bool tieneRegistrosAsociado(long EmplazamientoID)
        {
            bool tiene = false;
            EmplazamientosController fControl = new EmplazamientosController();
            List<Emplazamientos> datos;
            datos = fControl.GetItemsList("EmplazamientoID == " + EmplazamientoID.ToString());
            if (datos.Count > 0)
            {
                tiene = true;
            }
            return tiene;
        }

        public bool ExisteContactbyNombreContacto(string contacto, long EmplazamientoID)
        {

            bool exite = false;
            List<ContactosEmplazamientos> datos = new List<ContactosEmplazamientos>();

            datos = (from c in Context.ContactosEmplazamientos where c.ContactoNombre == contacto && c.EmplazamientoID == EmplazamientoID select c).ToList();
            if (datos.Count > 0)
            {
                exite = true;
            }


            return exite;


        }

        public ContactosEmplazamientos ContactoByNombreAlquiler(string contacto, long EmplazamientoID)
        {

            ContactosEmplazamientos datos = new ContactosEmplazamientos();
            try
            {
                datos = (from c in Context.ContactosEmplazamientos where c.ContactoNombre == contacto && c.EmplazamientoID == EmplazamientoID select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }


        public ContactosEmplazamientos ContactoByNumeroAlquiler(long EmplazamientoID)
        {
            ContactosEmplazamientos contacto = new ContactosEmplazamientos();
            try
            {
                contacto = (from c in Context.ContactosEmplazamientos where c.EmplazamientoID == EmplazamientoID select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                contacto = null;

            }
            return contacto;

        }

        public List<ContactosEmplazamientos> ContactoByEmplazamiento(long EmplazamientoID)
        {


            List<ContactosEmplazamientos> ContactosEmplazamientos = new List<ContactosEmplazamientos>();

            List<ContactosEmplazamientos> ContactosEmplazamientosDef = new List<ContactosEmplazamientos>();

            List<Alquileres> alquileres = new List<Alquileres>();


            try
            {

                foreach (Alquileres alquiler in alquileres)
                {
                    ContactosEmplazamientos = (from c in Context.ContactosEmplazamientos where c.EmplazamientoID == alquiler.EmplazamientoID && c.Activo == true select c).ToList();

                    ContactosEmplazamientosDef.AddRange(ContactosEmplazamientos);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ContactosEmplazamientos = null;
            }

            return ContactosEmplazamientosDef;

        }


        public ContactosEmplazamientos ContactoPorDefectoByAlquiler(long EmplazamientoID)
        {

            List<ContactosEmplazamientos> ContactosEmplazamientos = new List<ContactosEmplazamientos>();

            try
            {
                ContactosEmplazamientos = (from c in Context.ContactosEmplazamientos where c.EmplazamientoID == EmplazamientoID && c.Activo == true && c.Defecto == true select c).ToList();

                if (ContactosEmplazamientos.Count == 0)
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ContactosEmplazamientos = null;
            }

            return ContactosEmplazamientos[0];

        }

        public ContactosEmplazamientos ContactoEmplazamientoByEmailContacto(string email, long EmplazamientoID)
        {

            ContactosEmplazamientos contEmp = new ContactosEmplazamientos();
            List<ContactosEmplazamientos> datos = new List<ContactosEmplazamientos>();

            datos = (from c in Context.ContactosEmplazamientos where c.ContactoEmail == email && c.EmplazamientoID == EmplazamientoID select c).ToList();
            if (datos.Count > 0)
            {
                contEmp = datos.ElementAt(0);
            }
            else
            {
                contEmp = null;
            }


            return contEmp;


        }

        #region CALIDAD

        //public int GetCalidad(string sFiltro)
        //{
        //    // Local variables
        //    int iResultado = 0;
        //    List<ContactosEmplazamientos> lista = null;

        //    try
        //    {
        //        lista = GetItemsList(sFiltro);
        //        if (lista != null)
        //        {
        //            iResultado = lista.Count;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Comun.cLog.EscribirLog("ContactosEmplazamientosController - GetCalidad: " + ex.Message);
        //    }

        //    // Returns the result
        //    return iResultado;
        //}

        #endregion
    }
}