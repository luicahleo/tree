using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CalidadController : GeneralBaseController<Calidad, TreeCoreContext>, IBasica<Calidad>
    {
        public CalidadController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string NombreCampo, long clienteID)
        {
            bool isExiste = false;
            List<Calidad> datos;


            datos = (from c in Context.Calidad where (c.NombreCampo == NombreCampo && c.ClienteID == clienteID) select c).ToList<Calidad>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicado(string Descripcion, string Operador, long TipoDatoID, long clienteID)
        {
            bool isExiste = false;
            List<Calidad> datos;


            datos = (from c 
                     in Context.Calidad 
                     where (c.Descripcion == Descripcion && c.ClienteID == clienteID) ||
                            (c.ClienteID == clienteID && c.Operador == Operador && c.TipoDatoID == TipoDatoID) 
                     select c).ToList<Calidad>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long CalidadID)
        {
            Calidad dato = new Calidad();
            CalidadController cController = new CalidadController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && CalidadID == " + CalidadID.ToString());

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

        public Calidad GetDefault(long clienteID)
        {
            Calidad calidad;
            try
            {
                calidad = (from c 
                           in Context.Calidad 
                           where c.Defecto &&
                                c.ClienteID == clienteID
                           select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                calidad = null;
            }
            return calidad;
        }
    }
}