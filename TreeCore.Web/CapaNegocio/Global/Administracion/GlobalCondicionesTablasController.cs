using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{

    public class GlobalCondicionesTablasController : GeneralBaseController<GlobalCondicionesTablas, TreeCoreContext> //, IBasica<GlobalCondicionesTablas>
    {
        public GlobalCondicionesTablasController()
            : base()
        {

        }

        public List<Vw_GlobalCondicionesTablas> GetAllGlobalCondicionesTablasByCampoDestino(String lCampoDestino)
        {
            List<Vw_GlobalCondicionesTablas> lCondiciones;

            try
            {
                lCondiciones = (from c in Context.Vw_GlobalCondicionesTablas where c.CampoDestino == lCampoDestino select c).ToList();
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