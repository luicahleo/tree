using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EmplazamientosProyectosController : GeneralBaseController<EmplazamientosProyectos, TreeCoreContext>
    {
        public EmplazamientosProyectosController()
            : base()
        { }

        public List<EmplazamientosProyectos> GetActivos(long lClienteID)
        {
            List<EmplazamientosProyectos> listaProyectos;

            try
            {
                listaProyectos = (from c in Context.EmplazamientosProyectos where c.Activo /*&& c.ClienteID == lClienteID orderby c.pro */select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaProyectos = null;
            }

            return listaProyectos;
        }

        public List<Vw_EmplazamientosProyectos> GetVwByEmplazamientoID(long emplazamientoID)
        {
            List<Vw_EmplazamientosProyectos> lista;
            try
            {
                lista = (from c
                         in Context.Vw_EmplazamientosProyectos
                         where /*c.Activo && */
                               c.EmplazamientoID == emplazamientoID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public List<Vw_EmplazamientosProyectosFiltrados> GetViewByEmplazamientoID(long emplazamientoID)
        {
            List<Vw_EmplazamientosProyectosFiltrados> lista;
            try
            {
                lista = (from c
                         in Context.Vw_EmplazamientosProyectosFiltrados
                         where /*c.Activo && */
                               c.EmplazamientoID == emplazamientoID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }
    }
}