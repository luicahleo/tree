using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosGlobalZonasController : GeneralBaseController<ProyectosGlobalZonas, TreeCoreContext>
    {
        public ProyectosGlobalZonasController()
            : base()
        { }

        public List<Vw_ProyectosGlobalZonas> getVWProyectosGlobalZonas(long proyectoID)
        {
            List<Vw_ProyectosGlobalZonas> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.Vw_ProyectosGlobalZonas where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

        public List<ProyectosGlobalZonas> getGlobalZonasByProyectoID(long proyectoID)
        {
            List<ProyectosGlobalZonas> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.ProyectosGlobalZonas where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

    }
}