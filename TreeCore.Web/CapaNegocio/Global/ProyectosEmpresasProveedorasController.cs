using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosEmpresasProveedorasController : GeneralBaseController<ProyectosEmpresasProveedoras, TreeCoreContext>
    {
        public ProyectosEmpresasProveedorasController()
            : base()
        { }

        public List<Vw_ProyectosEmpresasProveedoras> getVWProyectosEmpresasProveedoras(long proyectoID)
        {
            List<Vw_ProyectosEmpresasProveedoras> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.Vw_ProyectosEmpresasProveedoras where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

        public List<ProyectosEmpresasProveedoras> getEmpresasProveedorasByProyectoID(long proyectoID)
        {
            List<ProyectosEmpresasProveedoras> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.ProyectosEmpresasProveedoras where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

    }
}