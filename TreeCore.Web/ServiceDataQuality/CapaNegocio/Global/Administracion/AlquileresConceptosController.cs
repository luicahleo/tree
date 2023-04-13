using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class AlquileresConceptosController : GeneralBaseController<AlquileresConceptos, TreeCoreContext>, IBasica<AlquileresConceptos>
    {
        public AlquileresConceptosController()
            : base()
        { }

        public bool RegistroVinculado(long AlquilerConceptoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string AlquilerConcepto, long clienteID)
        {
            bool isExiste = false;
            List<AlquileresConceptos> datos = new List<AlquileresConceptos>();


            datos = (from c in Context.AlquileresConceptos where (c.AlquilerConcepto == AlquilerConcepto && c.ClienteID == clienteID) select c).ToList<AlquileresConceptos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long AlquilerConceptoID)
        {
            AlquileresConceptos dato = new AlquileresConceptos();
            AlquileresConceptosController cController = new AlquileresConceptosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && AlquilerConceptoID == " + AlquilerConceptoID.ToString());

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

        public AlquileresConceptos GetDefault(long clienteID) {
            AlquileresConceptos oAlquileresConceptos;
            try
            {
                oAlquileresConceptos = (from c in Context.AlquileresConceptos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oAlquileresConceptos = null;
            }
            return oAlquileresConceptos;
        }

        public AlquileresConceptos GetConceptosByNombre(string sNombre)
        {
            // Local variables
            List<AlquileresConceptos> lista = null;
            AlquileresConceptos dato = null;

            try
            {

                lista = (from c in Context.AlquileresConceptos where c.AlquilerConcepto == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<AlquileresConceptos>();
                return dato;
            }

            return dato;
        }

        public AlquileresConceptos GetConceptosByCodigo(string sNombre)
        {
            // Local variables
            List<AlquileresConceptos> lista = null;
            AlquileresConceptos dato = null;

            try
            {

                lista = (from c in Context.AlquileresConceptos where c.Codigo == sNombre select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<AlquileresConceptos>();
                return dato;
            }

            return dato;
        }
    }
}