using System;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosAgenciasController : GeneralBaseController<MantenimientoEmplazamientosAgencias, TreeCoreContext>
    {
        public MantenimientoEmplazamientosAgenciasController()
            : base()
        { }

        public MantenimientoEmplazamientosAgencias GetByNombreYProyecto(string Nombre, long Proyecto)
        {

            MantenimientoEmplazamientosAgencias agencia;
            try
            {
                agencia = (from c 
                           in Context.MantenimientoEmplazamientosAgencias 
                           where c.Agencia.Equals(Nombre) && 
                                c.ProyectoID.Equals(Proyecto) &&
                                c.Activo
                           select c).First();
            }
            catch (Exception ex)
            {
                agencia = null;
                log.Error(ex.Message);
            }
            return agencia;
        }
    }
}