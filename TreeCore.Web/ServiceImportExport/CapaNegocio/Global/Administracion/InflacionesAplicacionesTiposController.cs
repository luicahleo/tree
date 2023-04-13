using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InflacionesAplicacionesTiposController : GeneralBaseController<InflacionesAplicacionesTipos, TreeCoreContext>
    {
        public InflacionesAplicacionesTiposController()
            : base()
        { }

        public bool RegistroVinculado(long InflacionAplicacionTipoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string nombre, string sCodigo, long clienteID)
        {
            bool isExiste = false;
            List<InflacionesAplicacionesTipos> datos = new List<InflacionesAplicacionesTipos>();


            datos = (from c in Context.InflacionesAplicacionesTipos where 
                     (c.Nombre == nombre || c.Codigo == sCodigo) && c.ClienteID == clienteID
                     select c).ToList<InflacionesAplicacionesTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long InflacionAplicacionTipoID)
        {
            InflacionesAplicacionesTipos dato = new InflacionesAplicacionesTipos();
            InflacionesAplicacionesTiposController cController = new InflacionesAplicacionesTiposController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && InflacionAplicacionTipoID == " + InflacionAplicacionTipoID.ToString());

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

        public InflacionesAplicacionesTipos GetDefault(long clienteID)
        {
            InflacionesAplicacionesTipos oTipo;

            try
            {
                oTipo = (from c in Context.InflacionesAplicacionesTipos where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception)
            {
                oTipo = null;
            }

            return oTipo;
        }
    }
}