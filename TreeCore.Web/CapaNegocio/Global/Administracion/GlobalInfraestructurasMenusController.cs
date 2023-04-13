using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalInfraestructurasMenusController : GeneralBaseController<GlobalInfraestructurasMenus, TreeCoreContext>, IBasica<GlobalInfraestructurasMenus>
    {
        public GlobalInfraestructurasMenusController()
            : base()
        { }

        public bool RegistroVinculado(long GlobalInfraestructuraMenuID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Alias, long clienteID)
        {
            bool isExiste = false;
            List<GlobalInfraestructurasMenus> datos = new List<GlobalInfraestructurasMenus>();


            datos = (from c in Context.GlobalInfraestructurasMenus where (c.Alias == Alias && c.ClienteID == clienteID) select c).ToList<GlobalInfraestructurasMenus>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalInfraestructuraMenuID)
        {
            GlobalInfraestructurasMenus dato = new GlobalInfraestructurasMenus();
            GlobalInfraestructurasMenusController cController = new GlobalInfraestructurasMenusController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalInfraestructuraMenuID == " + GlobalInfraestructuraMenuID.ToString());

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