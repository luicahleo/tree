using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ProyectosFasesController : GeneralBaseController<ProyectosFases, TreeCoreContext>
    {
        public ProyectosFasesController()
            : base()
        { }

        public bool controlDuplicadoFasesByProyecto(string Nombre, long proyectoID)
        {
            bool control = false;
            List<ProyectosFases> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.ProyectosFases where c.Fase.Equals(Nombre) && c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (listaProyectos.Count > 0)
            {
                control = true;
            }
            return control;
        }

        public List<ProyectosFases> getFasesByProyectoID(long proyectoID)
        {
            List<ProyectosFases> listaProyectos = null;

            try
            {
                listaProyectos = (from c in Context.ProyectosFases where c.ProyectoID == proyectoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaProyectos;
        }

    }
}