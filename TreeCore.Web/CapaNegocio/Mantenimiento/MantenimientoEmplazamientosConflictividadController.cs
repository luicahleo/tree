using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosConflictividadController : GeneralBaseController<MantenimientoEmplazamientosConflictividad, TreeCoreContext>, IBasica<MantenimientoEmplazamientosConflictividad>
    {
        public MantenimientoEmplazamientosConflictividadController()
            : base()
        { }

        public bool RegistroVinculado (long conflictividadID)
        {
            bool existe = false;
            List<MantenimientoEmplazamientos> datos;

            datos = (from c 
                     in Context.MantenimientoEmplazamientos 
                     where c.MantenimientoEmplazamientoConflictividadID == conflictividadID 
                     select c).ToList();

            if (datos.Count == 0)
            {
                existe = false;
            }
            else
            {
                existe = true;
            }

            return existe;
        }


        public bool RegistroDuplicado(string conflictividad, long clienteID)
        {
            bool isExiste = false;
            List<MantenimientoEmplazamientosConflictividad> datos;

          
            datos = (from c 
                     in Context.MantenimientoEmplazamientosConflictividad 
                     where (c.Conflictividad_esES == conflictividad && 
                            c.ClienteID == clienteID) 
                     select c).ToList<MantenimientoEmplazamientosConflictividad>();
            
            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long conflictividadID)
        {
            MantenimientoEmplazamientosConflictividad dato;
            MantenimientoEmplazamientosConflictividadController cEmplazamientos = new MantenimientoEmplazamientosConflictividadController();
            bool defecto = false;

            dato = (from c 
                    in Context.MantenimientoEmplazamientosConflictividad 
                    where c.Defecto && 
                            c.MantenimientoEmplazamientoConflictividadID == conflictividadID 
                    select c).First();

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        public MantenimientoEmplazamientosConflictividad GetByConflictividad(string Conflictividad)
        {
            MantenimientoEmplazamientosConflictividad result = null;
            try
            {
                result = (from c in Context.MantenimientoEmplazamientosConflictividad
                          where (c.Conflictividad_esES == Conflictividad ||
                                    c.Conflictividad_frFR == Conflictividad ||
                                    c.Conflictividad_enUS == Conflictividad) &&
                                    c.Activo
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