using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class GlobalCondicionesCodigosEmplazamientosController : GeneralBaseController<GlobalCondicionesCodigosEmplazamientos, TreeCoreContext>
    {
        public GlobalCondicionesCodigosEmplazamientosController()
          : base()
        {

        }

        public bool ExsisteOrden(double Orden)
        {
            bool Exsiste = false;
            List<GlobalCondicionesCodigosEmplazamientos> lCondiciones = new List<GlobalCondicionesCodigosEmplazamientos>();
            lCondiciones = (from c in Context.GlobalCondicionesCodigosEmplazamientos where c.Orden == Orden select c).ToList();
            if (lCondiciones.Count > 0)
            {
                Exsiste = true;
            }

            return Exsiste;
        }

        public List<GlobalCondicionesCodigosEmplazamientos> GlobalCondicionesCodigosEmplazamientosBySeleccionadoID(long seleccionadoID)
        {
            List<GlobalCondicionesCodigosEmplazamientos> lCondiciones;

            try
            {
                lCondiciones = (from c in Context.GlobalCondicionesCodigosEmplazamientos where c.GlobalCondicionReglaID == seleccionadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lCondiciones = null;
            }
            return lCondiciones;
        }
    }
}