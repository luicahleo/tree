using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosCorrectivosEstadosController : GeneralBaseController<MantenimientoEmplazamientosCorrectivosEstados, TreeCoreContext>
    {
        public MantenimientoEmplazamientosCorrectivosEstadosController()
            : base()
        { }
        public MantenimientoEmplazamientosCorrectivosEstados EstadoCorrectivoPorDefectoByTipologia(long? ID)
        {
            List<MantenimientoEmplazamientosCorrectivosEstados> datos = null;
            if (ID == null)
            {
                datos = (from c in Context.MantenimientoEmplazamientosCorrectivosEstados
                         where c.MantenimientoTipologiaID == null && 
                                c.Defecto
                         select c).ToList();
                if (datos.Count > 0)
                {
                    return datos[0];
                }
            }

            datos = (from c in Context.MantenimientoEmplazamientosCorrectivosEstados
                     where c.MantenimientoTipologiaID == ID &&
                            c.Defecto
                     select c).ToList();
            if (datos.Count > 0)
            {
                return datos[0];
            }
            else
            {
                datos = (from c in Context.MantenimientoEmplazamientosCorrectivosEstados
                         where c.MantenimientoTipologiaID == ID && 
                                c.Defecto
                         orderby c.MantenimientoEmplazamientoCorrectivoEstadoID
                         select c).ToList();
                if (datos.Count > 0)
                {
                    return datos[0];
                }
                else
                {
                }
            }

            return null;
        }
    }
}