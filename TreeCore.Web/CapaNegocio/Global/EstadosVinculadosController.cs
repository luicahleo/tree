using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class EstadosVinculadosController : GeneralBaseController<EstadosVinculados, TreeCoreContext>
    {
        public EstadosVinculadosController()
            : base()
        { }

        public List<Vw_EstadosVinculados> GetEstadosVinculadosByEstadoID(long lEstadoID, string sTipoProyecto)
        {
            List<Vw_EstadosVinculados> lista = null;

            try
            {
                lista = (from c in Context.Vw_EstadosVinculados where (c.EstadoOrigenID == lEstadoID && c.ProyectoTipoOrigen == sTipoProyecto) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return lista;
        }

        public List<EstadosVinculados> GetEstadosVinculadosByEstadoTipologia (long lEstadoID, long? lTipologiaID, string sTabla, bool bColocalizacion)
        {
            List<EstadosVinculados> listaDatos = new List<EstadosVinculados>();

            switch (sTabla)
            {
                case "AdquisicionesEstadosSARF":
                    AdquisicionesSARFEstados oDatoSARF = (from c in Context.AdquisicionesSARFEstados where c.AdquisicionSARFEstadoID == lEstadoID && c.AdquisicionTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oDatoSARF != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oDatoSARF.AdquisicionSARFEstadoID select c).ToList();
                    }

                    break;

                case "AdquisicionesSARFEmplazamientosEstados":
                    AdquisicionesSARFEmplazamientosEstados oDato = (from c in Context.AdquisicionesSARFEmplazamientosEstados where c.AdquisicionSARFEmplazamientoEstadoID == lEstadoID && c.AdquisicionTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oDato != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oDato.AdquisicionSARFEmplazamientoEstadoID select c).ToList();
                    }

                    break;

                case "AdquisicionesEmplazamientosEstados":
                    AdquisicionesEmplazamientosEstados oEstado = (from c in Context.AdquisicionesEmplazamientosEstados where c.AdquisicionEmplazamientoEstadoID == lEstadoID && c.AdquisicionTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oEstado != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oEstado.AdquisicionEmplazamientoEstadoID select c).ToList();
                    }

                    break;

                case "TorrerosSARFEstados":
                    TorrerosSARFEstados oSARF = (from c in Context.TorrerosSARFEstados where c.TorreroSARFEstadoID == lEstadoID && c.TorreroTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oSARF != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oSARF.TorreroSARFEstadoID select c).ToList();
                    }

                    break;

                case "TorrerosSARFEmplazamientosEstados":
                    TorrerosSARFEmplazamientosEstados oSARFTower = (from c in Context.TorrerosSARFEmplazamientosEstados where c.TorreroSARFEmplazamientoEstadoID == lEstadoID && c.TorreroTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oSARFTower != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oSARFTower.TorreroSARFEmplazamientoEstadoID select c).ToList();
                    }

                    break;

                case "TorrerosEmplazamientosEstados":
                    TorrerosEmplazamientosEstados oTower = (from c in Context.TorrerosEmplazamientosEstados where c.TorreroEmplazamientoEstadoID == lEstadoID && c.TorreroTipologiaID == lTipologiaID select c).FirstOrDefault();

                    if (oTower != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oTower.TorreroEmplazamientoEstadoID select c).ToList();
                    }

                    break;

                case "SavingEmplazamientosEstados":
                    SavingEmplazamientosEstados oSaving = null;

                    if (lTipologiaID != null)
                    {
                        oSaving = (from c in Context.SavingEmplazamientosEstados where c.SavingEmplazamientoEstadoID == lEstadoID && c.SavingTipologiaID == lTipologiaID select c).FirstOrDefault();
                    }
                    else
                    {
                        oSaving = (from c in Context.SavingEmplazamientosEstados where c.SavingEmplazamientoEstadoID == lEstadoID && c.SavingTipologiaID == null select c).FirstOrDefault();
                    }

                    if (oSaving != null)
                    {
                        listaDatos = (from c in Context.EstadosVinculados where c.EstadoOrigenID == oSaving.SavingEmplazamientoEstadoID select c).ToList();
                    }

                    break;
            }

            return listaDatos;
        }

        public bool HasEstadosVinculadosByEstadoID (long lEstadoOrigenID, long lTipoProyectoOrigenID, long lEstadoDestinoID, long lTipoProyectoDestinoID)
        {
            List<Vw_EstadosVinculados> lista = null;
            bool bRes = false;

            try
            {
                lista = (from c in Context.Vw_EstadosVinculados where (c.EstadoOrigenID == lEstadoOrigenID && c.ProyectoTipoOrigenID == lTipoProyectoOrigenID && c.EstadoDestinoID == lEstadoDestinoID && c.ProyectoTipoDestinoID == lTipoProyectoDestinoID) select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return bRes;
        }
    }
}