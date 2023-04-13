using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoIncidenciasTiposController : GeneralBaseController<MantenimientoIncidenciasTipos, TreeCoreContext>
    {
        public MantenimientoIncidenciasTiposController() : base()
        { }

        public bool RegistroDuplicado(string incidenciaTipo, long clienteID)
        {
            MantenimientoIncidenciasTipos dato = new MantenimientoIncidenciasTipos();

            dato = (from c in Context.MantenimientoIncidenciasTipos
                    where (c.MantenimientoIncidenciaTipo == incidenciaTipo && c.ClienteID == clienteID)
                    select c).FirstOrDefault();

            bool bExiste = true;
            if (dato == null) { bExiste = false; }

            return bExiste;
        }

        public MantenimientoIncidenciasTipos GetDefault(long clienteID)
        {
            MantenimientoIncidenciasTipos oIncidenciasTipo;

            oIncidenciasTipo = (from c in Context.MantenimientoIncidenciasTipos
                                where c.Defecto && c.ClienteID == clienteID
                                select c).FirstOrDefault();

            return oIncidenciasTipo;
        }

        public MantenimientoIncidenciasTipos GetByNombreAndClienteID(string nombre, long clienteID)
        {
            MantenimientoIncidenciasTipos result;

            try
            {
                result = (from c 
                          in Context.MantenimientoIncidenciasTipos 
                          where c.MantenimientoIncidenciaTipo == nombre && 
                                c.Activo &&
                                c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                result = null;
                log.Error(ex.Message);
            }

            return result;
        }
    }
}