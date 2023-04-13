using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class SAPTratamientosController : GeneralBaseController<SAPTratamientos, TreeCoreContext>, IBasica<SAPTratamientos>
    {
        public SAPTratamientosController()
            : base()
        { }

        public bool RegistroVinculado(long SAPTratamientoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string SAPTratamiento, long clienteID)
        {
            bool isExiste = false;
            List<SAPTratamientos> datos = new List<SAPTratamientos>();


            datos = (from c in Context.SAPTratamientos where (c.SAPTratamiento == SAPTratamiento && c.ClienteID == clienteID) select c).ToList<SAPTratamientos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long SAPTratamientoID)
        {
            SAPTratamientos dato = new SAPTratamientos();
            SAPTratamientosController cController = new SAPTratamientosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && SAPTratamientoID == " + SAPTratamientoID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public SAPTratamientos GetDefault(long lClienteID)
        {
            SAPTratamientos oTratamiento;

            try
            {
                oTratamiento = (from c in Context.SAPTratamientos where c.Defecto && c.ClienteID == lClienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTratamiento = null;
            }

            return oTratamiento;
        }
        public List<SAPTratamientos> GetSAPTratamientosByCliente(long clienteID)
        {
            List<SAPTratamientos> datos = new List<SAPTratamientos>();

            datos = (from c in Context.SAPTratamientos where (c.ClienteID == clienteID) orderby c.SAPTratamiento select c).ToList<SAPTratamientos>();

            return datos;
        }

        public SAPTratamientos GetTratamientoByNombre(string sNombre)
        {
            List<SAPTratamientos> lista = null;
            SAPTratamientos dato = null;

            try
            {

                lista = (from c in Context.SAPTratamientos where c.SAPTratamiento == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return dato;
            }

            return dato;
        }
    }
}