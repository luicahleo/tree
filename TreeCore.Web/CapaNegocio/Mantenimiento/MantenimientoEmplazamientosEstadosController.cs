using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosEstadosController : GeneralBaseController<MantenimientoEmplazamientosEstados, TreeCoreContext>
    {
        public MantenimientoEmplazamientosEstadosController()
            : base()
        { }

        public List<MantenimientoEmplazamientosEstados> Lista_EstadosByTipologia(long? lTipologiaID)
        {
            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();

            if (lTipologiaID != null)
            {
                listaDatos = (from c in Context.MantenimientoEmplazamientosEstados where (c.MantenimientoTipologiaID == lTipologiaID) select c).OrderBy("MantenimientoEmplazamientoEstado_esES").ToList();

            }
            else
            {
                listaDatos = (from c in Context.MantenimientoEmplazamientosEstados where (c.MantenimientoTipologiaID == null) select c).OrderBy("MantenimientoEmplazamientoEstado_esES").ToList();

            }


            return listaDatos;
        }

        public List<MantenimientoEmplazamientosEstados> Lista_EstadosPrincipalesByTipologia(long lTipologiaID)
        {
            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstados
                          where c.MantenimientoTipologiaID == lTipologiaID && c.EstadoPadreID == null
                          select c).OrderBy("MantenimientoEmplazamientoEstado_esES").ToList();

            return listaDatos;
        }

        public bool ExisteEstadoByTipologia(long lTipologiaid, string sCodigo, string sNombre)
        {
            bool bTiene = false;

            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstados where c.MantenimientoTipologiaID == lTipologiaid && c.Codigo == sCodigo && c.MantenimientoEmplazamientoEstado_enUS == sNombre select c).ToList();

            if (listaDatos.Count > 0)
            {
                bTiene = true;
            }

            return bTiene;
        }

        public long? GetMantenimientoEmplazamientoEstadoIDByCodigoTipologia(string sCodigo, long lTipologia)
        {
            long? lDatos = null;

            try
            {
                lDatos = (from c in Context.MantenimientoEmplazamientosEstados where c.Codigo == sCodigo && c.MantenimientoTipologiaID == lTipologia select c.MantenimientoEmplazamientoEstadoID).First();
            }
            catch (Exception ex)
            {
                lDatos = null;
                log.Error(ex.Message);
            }

            return lDatos;
        }

        public List<Vw_MantenimientoEmplazamientoEstadosSiguientes> EstadosSiguiente(long lEstadoID)
        {
            List<Vw_MantenimientoEmplazamientoEstadosSiguientes> listaDatos = new List<Vw_MantenimientoEmplazamientoEstadosSiguientes>();

            listaDatos = (from c in Context.Vw_MantenimientoEmplazamientoEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == lEstadoID select c).ToList();

            return listaDatos;
        }

        public List<MantenimientoEmplazamientosEstados> EstadosPendientes(long lEstadoID, long lTipologiaID)
        {
            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();
            List<long> listaEstadosAsignados = new List<long>();

            listaEstadosAsignados = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == lEstadoID select c.MantenimientoEmplazamientoEstadoPosibleID).ToList();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstados where (!listaEstadosAsignados.Contains(c.MantenimientoEmplazamientoEstadoID) && c.MantenimientoTipologiaID == lTipologiaID) select c).ToList();

            return listaDatos;
        }

        public List<MantenimientoEmplazamientosEstados> GetAllItemsByOrder()
        {
            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstados where (c.Orden > 0) orderby c.Orden select c).ToList();

            return listaDatos;
        }

        public List<Vw_MantenimientoEmplazamientoEstadosSiguientes> GetEstadosAnteriores(long MantenimientoEmplazamientosEstadoID)
        {
            List<Vw_MantenimientoEmplazamientoEstadosSiguientes> lEstados = new List<Vw_MantenimientoEmplazamientoEstadosSiguientes>();

            try
            {
                lEstados = (from c in Context.Vw_MantenimientoEmplazamientoEstadosSiguientes where c.MantenimientoEmplazamientoEstadoPosibleID == MantenimientoEmplazamientosEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                lEstados = new List<Vw_MantenimientoEmplazamientoEstadosSiguientes>();
                log.Error(ex.Message);
            }

            return lEstados;
        }

        public List<Vw_MantenimientoEmplazamientoEstadosSiguientes> GetEstadosSiguientes(long MantenimientoEmplazamientosEstadoID)
        {
            List<Vw_MantenimientoEmplazamientoEstadosSiguientes> lEstados = new List<Vw_MantenimientoEmplazamientoEstadosSiguientes>();

            try
            {
                lEstados = (from c in Context.Vw_MantenimientoEmplazamientoEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == MantenimientoEmplazamientosEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                lEstados = new List<Vw_MantenimientoEmplazamientoEstadosSiguientes>();
                log.Error(ex.Message);
            }

            return lEstados;
        }

        public bool TieneRegistrosAsociados(long lEstadoID)
        {
            bool bTiene = false;
            int iRegistrosRelecionados = 0;

            iRegistrosRelecionados = (from c in Context.MantenimientoEmplazamientosEstadosSiguientes where c.MantenimientoEmplazamientoEstadoID == lEstadoID || c.MantenimientoEmplazamientoEstadoPosibleID == lEstadoID select c).Count();

            if (iRegistrosRelecionados > 0)
            {
                bTiene = true;
            }

            return bTiene;
        }

        public MantenimientoEmplazamientosEstados GetAllStates()
        {
            MantenimientoEmplazamientosEstados oDatos = new MantenimientoEmplazamientosEstados();
            List<MantenimientoEmplazamientosEstados> listaDatos = new List<MantenimientoEmplazamientosEstados>();

            listaDatos = (from c in Context.MantenimientoEmplazamientosEstados select c).ToList();

            oDatos = listaDatos.Last();

            return oDatos;
        }
    }
}