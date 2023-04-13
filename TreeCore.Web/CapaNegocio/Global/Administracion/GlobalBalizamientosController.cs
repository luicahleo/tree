using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalBalizamientosController : GeneralBaseController<GlobalBalizamientos, TreeCoreContext>, IBasica<GlobalBalizamientos>
    {
        public GlobalBalizamientosController()
            : base()
        { }

        public bool RegistroVinculado(long BalizamientoID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Balizamiento, long clienteID)
        {
            bool isExiste = false;
            List<GlobalBalizamientos> datos = new List<GlobalBalizamientos>();


            datos = (from c in Context.GlobalBalizamientos where (c.Balizamiento == Balizamiento && c.ClienteID == clienteID) select c).ToList<GlobalBalizamientos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long BalizamientoID)
        {
            GlobalBalizamientos dato = new GlobalBalizamientos();
            GlobalBalizamientosController cController = new GlobalBalizamientosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && BalizamientoID == " + BalizamientoID.ToString());

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