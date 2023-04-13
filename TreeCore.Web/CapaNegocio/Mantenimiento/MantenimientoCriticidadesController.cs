using System;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoCriticidadesController : GeneralBaseController<MantenimientoCriticidades, TreeCoreContext>
    {
        public MantenimientoCriticidadesController()
            : base()
        { }

        public MantenimientoCriticidades GetCriticidad(string sCriticidad)
        {
            MantenimientoCriticidades criticidad = null;
            try
            {
                criticidad = (from c 
                              in Context.MantenimientoCriticidades
                              where c.Criticidad == sCriticidad && 
                                    c.Activo
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                criticidad = null;
            }

            return criticidad;
        }
    }
}