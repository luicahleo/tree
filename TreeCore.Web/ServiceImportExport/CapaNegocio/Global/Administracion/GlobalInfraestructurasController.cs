using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalInfraestructurasController : GeneralBaseController<GlobalInfraestructuras, TreeCoreContext>, IBasica<GlobalInfraestructuras>
    {
        public GlobalInfraestructurasController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalInfraestructuraID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string nombreInfraestructura, long clienteID)
        {
            bool isExiste = false;
            List<GlobalInfraestructuras> datos = new List<GlobalInfraestructuras>();


            datos = (from c in Context.GlobalInfraestructuras where (c.NombreInfraestructura == nombreInfraestructura && c.ClienteID == clienteID) select c).ToList<GlobalInfraestructuras>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalInfraestructuraID)
        {
            GlobalInfraestructuras dato = new GlobalInfraestructuras();
            GlobalInfraestructurasController cController = new GlobalInfraestructurasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalInfraestructuraID == " + GlobalInfraestructuraID.ToString());

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
    }
}