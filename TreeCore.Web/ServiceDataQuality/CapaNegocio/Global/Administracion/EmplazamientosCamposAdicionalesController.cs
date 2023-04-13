
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class EmplazamientosCamposAdicionalesController : GeneralBaseController<EmplazamientosCamposAdicionales, TreeCoreContext>, IBasica<EmplazamientosCamposAdicionales>
    {
        public EmplazamientosCamposAdicionalesController()
            : base()
        { }
        //MHERBA;REVISAR LAS FUNCIONES DEL CONTROLADOR
        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string CodigoAdicional, long EmplazamientoID)
        {
            bool isExiste = false;
            List<EmplazamientosCamposAdicionales> datos = new List<EmplazamientosCamposAdicionales>();


            datos = (from c in Context.EmplazamientosCamposAdicionales where (c.CodigoAdicional == CodigoAdicional && c.EmplazamientoID == EmplazamientoID) select c).ToList<EmplazamientosCamposAdicionales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long EmplazamientoID)
        {
            EmplazamientosCamposAdicionales dato = new EmplazamientosCamposAdicionales();
            EmplazamientosCamposAdicionalesController cController = new EmplazamientosCamposAdicionalesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoID == " + EmplazamientoID.ToString());

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

        public ActivosClases GetDefault(long clienteID)
        {
            ActivosClases oActivosClases;
            try
            {
                oActivosClases = (from c in Context.ActivosClases where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oActivosClases = null;
            }
            return oActivosClases;
        }
    }
}