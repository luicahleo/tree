using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosEstadosSiguientesController : GeneralBaseController<MantenimientoEmplazamientosEstadosSiguientes, TreeCoreContext>
    {
        public MantenimientoEmplazamientosEstadosSiguientesController()
            : base()
        { }

        public List<MantenimientoEmplazamientosEstadosSiguientes> GetMantenimientoEmplazamientosEstadosSiguientesByEstadoTipologia(long lEstadoID, long? lTipologiaID)
        {
            List<MantenimientoEmplazamientosEstadosSiguientes> listaDatos = new List<MantenimientoEmplazamientosEstadosSiguientes>();
            List<long> listaEstadosID = new List<long>();

            //if (lTipologiaID != null)
            //{
            //    listaEstadosID = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoTipologiaID == lTipologiaID select c.MantenimientoEmplazamientoEstadoID).ToList();
            //}
            //else
            //{
            //    listaEstadosID = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoTipologiaID == null select c.MantenimientoEmplazamientoEstadoID).ToList();
            //}

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == lEstadoID && listaEstadosID.Contains(c.MantenimientoEmplazamientoEstadoPosibleID) select c).ToList();

            return listaDatos;
        }

        public bool ExisteMantenimientoEmplazamientoEstadoSiguiente(long lEstadoID, long lEstadoPosibleID, long lTipologia)
        {
            bool bExiste = false;
            List<MantenimientoEmplazamientosEstadosSiguientes> listaDatos = new List<MantenimientoEmplazamientosEstadosSiguientes>();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == lEstadoID && c.MantenimientoEmplazamientoEstadoSiguienteID == lEstadoPosibleID select c).ToList();

            listaDatos = listaDatos.Where(c => c.MantenimientoEmplazamientosEstados.MantenimientoTipologiaID == lTipologia).ToList();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public int GetCount(long EstadoID)
        {
            int count = 0;
            List<MantenimientoEmplazamientosEstadosSiguientes> datos = new List<MantenimientoEmplazamientosEstadosSiguientes>();
            datos = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == EstadoID select c).ToList();

            if (datos.Count > 0)
            {
                count = datos.Count;
            }

            return count;
        }

    }
}