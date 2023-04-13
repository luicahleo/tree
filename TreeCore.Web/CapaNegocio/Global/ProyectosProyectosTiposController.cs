using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosProyectosTiposController : GeneralBaseController<ProyectosProyectosTipos, TreeCoreContext>
    {
        public ProyectosProyectosTiposController()
            : base()
        { }

        public List<Vw_ProyectosProyectosTipos> getVWProyectosProyectosTipos(long proyectoID)
        {
            List<Vw_ProyectosProyectosTipos> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.Vw_ProyectosProyectosTipos where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

        public List<ProyectosProyectosTipos> getProyectoTipoByProyectoID(long proyectoID)
        {
            List<ProyectosProyectosTipos> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.ProyectosProyectosTipos where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

    }
}