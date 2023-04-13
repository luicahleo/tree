using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosRiesgosController : GeneralBaseController<EmplazamientosRiesgos, TreeCoreContext>, IBasica<EmplazamientosRiesgos>
    {
        public EmplazamientosRiesgosController()
            : base()
        { }

        public bool RegistroVinculado(long EmplazamientoRiesgoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string EmplazamientoRiesgo, long clienteID)
        {
            bool isExiste = false;
            List<EmplazamientosRiesgos> datos = new List<EmplazamientosRiesgos>();


            datos = (from c in Context.EmplazamientosRiesgos where (c.EmplazamientoRiesgo == EmplazamientoRiesgo && c.ClienteID == clienteID) select c).ToList<EmplazamientosRiesgos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoRiesgoID)
        {
            EmplazamientosRiesgos dato = new EmplazamientosRiesgos();
            EmplazamientosRiesgosController cController = new EmplazamientosRiesgosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoRiesgoID == " + EmplazamientoRiesgoID.ToString());

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

        public EmplazamientosRiesgos GetDefault(long clienteID) {
            EmplazamientosRiesgos oEmplazamientosRiesgos;
            try
            {
                oEmplazamientosRiesgos = (from c in Context.EmplazamientosRiesgos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEmplazamientosRiesgos = null;
            }
            return oEmplazamientosRiesgos;
        }

        public long GetEmplazamientosRiesgoIDByNombre(string sNombre)
        {
            // Local variable            
            List<EmplazamientosRiesgos> lista = null;
            long dato = -1;

            try
            {
                // Gets the information
                lista = ((from c in Context.EmplazamientosRiesgos where c.EmplazamientoRiesgo == sNombre select c).ToList());

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0).EmplazamientoRiesgoID;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = -1;
            }


            // returns the result
            return dato;
        }
    }
}