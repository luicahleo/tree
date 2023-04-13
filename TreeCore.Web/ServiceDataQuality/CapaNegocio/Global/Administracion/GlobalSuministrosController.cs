using System;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalSuministrosController : GeneralBaseController<GlobalSuministros, TreeCoreContext>
    {
        public GlobalSuministrosController()
            : base()
        {

        }

        public bool HasSuministrosPropios(long EmplazamientoID)
        {
            bool HasSuministroPropio = false;

            try
            {
                HasSuministroPropio = (from c in Context.GlobalSuministros
                                       where c.EmplazamientoID == EmplazamientoID
                                       select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                HasSuministroPropio = false;
            }

            return HasSuministroPropio;
        }

    }
}