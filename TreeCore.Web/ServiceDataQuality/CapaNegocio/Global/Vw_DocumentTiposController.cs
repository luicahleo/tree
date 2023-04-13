using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;
using Ext.Net;

namespace CapaNegocio
{
    public class Vw_DocumentTiposController : GeneralBaseController<Vw_DocumentTipos, TreeCoreContext>
    {
        public Vw_DocumentTiposController()
            : base()
        { }

        public List<Vw_AccessEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAccess(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AccessEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_AccessEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.AccessEmplazamientosEstadosTiposDocumentos where c.AccessEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AccessEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AccessEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AccessEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_AdquisicionesEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAdquisiciones(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AdquisicionesEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AdquisicionesEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AdquisicionesEmplazamientosEstadosTiposDocumentos where c.AdquisicionEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AdquisicionesEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AdquisicionEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AdquisicionesEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAdquisicionesSARF(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AdquisicionesSARFEmplazamientosEstadosTiposDocumentos where c.AdquisicionSARFEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AdquisicionSARFEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AdquisicionesSARFEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AmbientalEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAmbiental(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AmbientalEstadosDocumentos> listaResult = new List<Vw_AmbientalEstadosDocumentos>();

            listaTipos = (from c in Context.AmbientalEstadosTiposDocumentos where c.AmbientalEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AmbientalEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.AmbientalEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AmbientalEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAmpliaciones(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.AmpliacionesEmplazamientosEstadosTiposDocumentos where c.AmpliacionEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AmpliacionEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AmpliacionesEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAssetsPurchase(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AssetsPurchaseEmplazamientosEstadosTiposDocumentos where c.AssetPurchaseEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AssetsPurchaseEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.AssetPurchaseEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AssetsPurchaseEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_AuditEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosAudit(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_AuditEmplazamientosEstadosDocumentos> listaResult = new List<Vw_AuditEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.AuditEmplazamientosEstadosTiposDocumentos where c.AuditEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_AuditEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.AuditEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_AuditEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_CityEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosCity(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_CityEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_CityEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.CityEmplazamientosEstadosTiposDocumentos where c.CityEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_CityEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.CityEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_CityEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_DesplieguesEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosDespliegues(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_DesplieguesEmplazamientosEstadosDocumentos> listaResult = new List<Vw_DesplieguesEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.DesplieguesEmplazamientosEstadosTiposDocumentos where c.DespliegueEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_DesplieguesEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.DespliegueEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_DesplieguesEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_EnergyEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosEnergy(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_EnergyEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_EnergyEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.EnergyEmplazamientosEstadosTiposDocumentos where c.EnergyEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_EnergyEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.EnergyEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_EnergyEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_FinancieroAlquileresEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosFinanciero(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_FinancieroAlquileresEstadosDocumentos> listaResult = new List<Vw_FinancieroAlquileresEstadosDocumentos>();

            listaTipos = (from c in Context.FinancieroAlquileresEstadosTiposDocumentos where c.FinancieroAlquilerEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_FinancieroAlquileresEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.FinancieroAlquilerEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_FinancieroAlquileresEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_FirmaDigitalEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosFirmaDigital(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_FirmaDigitalEstadosDocumentos> listaResult = new List<Vw_FirmaDigitalEstadosDocumentos>();

            listaTipos = (from c in Context.FirmaDigitalEstadosTiposDocumentos where c.FirmaDigitalEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_FirmaDigitalEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.FirmaDigitalEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_FirmaDigitalEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_IndoorEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosIndoor(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_IndoorEmplazamientosEstadosDocumentos> listaResult = new List<Vw_IndoorEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.IndoorEmplazamientosEstadosTiposDocumentos where c.IndoorEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_IndoorEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.IndoorEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_IndoorEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosInstallObraCivil(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos> listaResult = new List<Vw_InstallObraCivilEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.InstallObraCivilEmplazamientosEstadosTiposDocumentos where c.InstallObraCivilEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_InstallObraCivilEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.InstallObraCivilEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_InstallObraCivilEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosInstallTecnica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_InstallTecnicaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.InstallTecnicaEmplazamientosEstadosTiposDocumentos where c.InstallTecnicaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_InstallTecnicaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.InstallTecnicaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_InstallTecnicaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_LegalEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosLegal(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_LegalEmplazamientosEstadosDocumentos> listaResult = new List<Vw_LegalEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.LegalEmplazamientosEstadosTiposDocumentos where c.LegalEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_LegalEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.LegalEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_LegalEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_MantenimientoEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosMantenimiento(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_MantenimientoEmplazamientosEstadosDocumentos> listaResult = new List<Vw_MantenimientoEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.MantenimientoEmplazamientosEstadosTiposDocumentos where c.MantenimientoEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_MantenimientoEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.MantenimientoEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_MantenimientoEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_PlanningPlanificacionesEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosPlanning(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_PlanningPlanificacionesEstadosTiposDocumentos> listaResult = new List<Vw_PlanningPlanificacionesEstadosTiposDocumentos>();

            listaTipos = (from c in Context.PlanningPlanificacionesEstadosTiposDocumentos where c.PlanningPlanificacionEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_PlanningPlanificacionesEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.PlanningPlanificacionEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_PlanningPlanificacionesEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_SavingEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSaving(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SavingEmplazamientosEstadosDocumentos> listaResult = new List<Vw_SavingEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.SavingEmplazamientosEstadosTiposDocumentos where c.SavingEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SavingEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SavingEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SavingEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_SharingEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSharing(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SharingEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_SharingEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.SharingEmplazamientosEstadosTipoDocumentos where c.SharingEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SharingEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SharingEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SharingEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_SpaceEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSpace(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SpaceEmplazamientosEstadosDocumentos> listaResult = new List<Vw_SpaceEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.SpaceEmplazamientosEstadosTiposDocumentos where c.SpaceEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SpaceEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.SpaceEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SpaceEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_SSRREmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosSSRR(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_SSRREmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_SSRREmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.SSRREmplazamientosEstadosTiposDocumentos where c.SSRREmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_SSRREmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.SSRREmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_SSRREmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosTorrerosSARF(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos> listaResult = new List<Vw_TorrerosSARFEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.TorrerosSARFEmplazamientosEstadosTiposDocumentos where c.TorreroSARFEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_TorrerosSARFEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.TorreroSARFEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_TorrerosSARFEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_TorrerosEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosTorreros(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_TorrerosEmplazamientosEstadosDocumentos> listaResult = new List<Vw_TorrerosEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.TorrerosEmplazamientosEstadosTiposDocumentos where c.TorreroEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_TorrerosEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.TorreroEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_TorrerosEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallAdminEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallAdmin(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallAdminEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallAdminEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallAdminEmplazamientosEstadosTiposDocumentos where c.UninstallAdminEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallAdminEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallAdminEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallAdminEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallTecnica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallTecnicaEmplazamientosEstadosTiposDocumentos where c.UninstallTecnicaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallTecnicaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallTecnicaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallTecnicaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosUninstallElectrica(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos> listaResult = new List<Vw_UninstallElectricaEmplazamientosEstadosDocumentos>();

            listaTipos = (from c in Context.UninstallElectricaEmplazamientosEstadosTiposDocumentos where c.UninstallElectricaEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_UninstallElectricaEmplazamientosEstadosDocumentos where (listaTiposNuevo.Contains(c.DocumentoTipoID) && c.UninstallElectricaEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_UninstallElectricaEmplazamientosEstadosDocumentos>();

            return listaResult;
        }

        public List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos> GetDocumentosTiposAsignadosEstadosEmplazamientosVandalismo(long lEstadoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos> listaResult = new List<Vw_VandalismoEmplazamientosEstadosTiposDocumentos>();

            listaTipos = (from c in Context.VandalismoEmplazamientosEstadosTiposDocumentos where c.VandalismoEmplazamientoEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            listaResult = (from c in Context.Vw_VandalismoEmplazamientosEstadosTiposDocumentos where (listaTiposNuevo.Contains(c.DocumentTipoID) && c.VandalismoEmplazamientoEstadoID == lEstadoID) select c).OrderBy("DocumentTipo").ToList<Vw_VandalismoEmplazamientosEstadosTiposDocumentos>();

            return listaResult;
        }

        public List<Vw_ProyectosTiposDocumentosTipos> GetDocumentosTiposNoAsignadosEstados(long lEstadoID, long lProyectoTipoID)
        {
            List<long> listaTipos = null;
            List<long?> listaTiposNuevo = null;
            List<Vw_ProyectosTiposDocumentosTipos> lista = null;

            listaTipos = (from c in Context.AdquisicionesSARFEstadosTiposDocumentos where c.AdquisicionSARFEstadoID == lEstadoID select c.DocumentoTipoID).ToList<long>();

            if (listaTipos != null && listaTipos.Count > 0)
            {
                listaTiposNuevo = listaTipos.Cast<long?>().ToList();
            }
            else
            {
                listaTiposNuevo = new List<long?>();
            }

            lista = (from c in Context.Vw_ProyectosTiposDocumentosTipos where !(listaTiposNuevo.Contains(c.DocumentTipoID)) && c.ProyectoTipoID == lProyectoTipoID select c).OrderBy("DocumentTipo").ToList<Vw_ProyectosTiposDocumentosTipos>();

            return lista;
        }

        public long GetDocTipo(string sDocTipo)
        {
            long lTipos = new long();

            lTipos = (from c in Context.Vw_DocumentTipos where c.DocumentTipo == sDocTipo select c.DocumentTipoID).First();

            return lTipos;
        }

        public DocumentosPerfiles CompruebaPermisoDocumentoTipo(long DocumentoTipoID, long UsuarioID)
        {
            UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();
            List<long> lPerfilesIDs = new List<long>();
            lPerfilesIDs = cUsuariosPerfiles.perfilesAsignadosIDs(UsuarioID);

            if (lPerfilesIDs != null && lPerfilesIDs.Count > 0)
            {
                List<DocumentosPerfiles> listaPermisos = (from c in Context.DocumentosPerfiles where c.PerfilID != null & c.Activo && c.TipoDocumentoID == DocumentoTipoID && lPerfilesIDs.Contains((long) c.PerfilID) select c).ToList();

                if (listaPermisos == null || listaPermisos.Count == 0)
                {
                    return null;
                }
                else if (listaPermisos.Count == 1)
                {
                    return listaPermisos[0];
                }
                else
                {
                    DocumentosPerfiles permisoMultiPerfil = new DocumentosPerfiles();
                    permisoMultiPerfil.PermisoLectura = false;
                    permisoMultiPerfil.PermisoEscritura = false;
                    permisoMultiPerfil.PermisoDescarga = false;

                    foreach (DocumentosPerfiles permiso in listaPermisos)
                    {
                        if(permiso.PermisoLectura)
                        {
                            permisoMultiPerfil.PermisoLectura = true;
                        }
                        if (permiso.PermisoEscritura)
                        {
                            permisoMultiPerfil.PermisoEscritura = true;
                        }
                        if(permiso.PermisoDescarga)
                        {
                            permisoMultiPerfil.PermisoDescarga = true;
                        }
                    }

                    return permisoMultiPerfil;
                }

            }

            return null;
        }

        public List<Vw_DocumentTipos> GetDocumentosTiposByEmplazamiento_PerfilesUsuario(long UsuarioID, long EmplazamientoID, long ProyectoTipoID)
        {
            List<long> lDocumentosTiposIDs = new List<long>();
            List<long> lPerfilesIDs = new List<long>();
            UsuariosPerfilesController cUsuariosPerfiles = new UsuariosPerfilesController();

            lPerfilesIDs = cUsuariosPerfiles.perfilesAsignadosIDs(UsuarioID);

            lDocumentosTiposIDs = (from c in Context.Vw_DocumentTipos where c.Activo && c.EsCarpeta select c.DocumentTipoID).ToList<long>();

            if (lPerfilesIDs != null && lPerfilesIDs.Count > 0)
            {
                foreach (long PerfilID in lPerfilesIDs)
                {
                    List<long> aux = new List<long>();
                    aux = (from c in Context.DocumentosPerfiles where c.PerfilID == PerfilID && c.Activo select c.TipoDocumentoID).Distinct().ToList<long>();
                    foreach (long DocumentoTipoID in aux)
                    {
                        lDocumentosTiposIDs.Add(DocumentoTipoID);
                    }
                }
            }

            List<long> lDocumentosTiposIDsAux = new List<long>();
            DocumentosController cDocumentos = new DocumentosController();
            lDocumentosTiposIDsAux = (from c in Context.ProyectosTiposDocumentosTipos where c.ProyectoTipoID == ProyectoTipoID select c.DocumentTipoID).ToList<long>();
            //lDocumentosTiposIDsAux = (from c in Context.Documentos where c.EmplazamientoID == EmplazamientoID && c.Activo && c.DocumentTipoID != null select c.DocumentTipoID).Distinct().ToList<long?>();
            List<Vw_DocumentTipos> lDocumentosTiposIDsFinal = new List<Vw_DocumentTipos>();

            foreach (long ID in lDocumentosTiposIDs)
            {
                Vw_DocumentTipos docTipo = new Vw_DocumentTipos();
                docTipo = GetItem(ID);
                if (docTipo != null && docTipo.EsCarpeta)
                {
                    lDocumentosTiposIDsFinal.Add(docTipo);
                }
            }

            foreach (long ID in lDocumentosTiposIDsAux)
            {
                if (lDocumentosTiposIDs.Contains(ID))
                {
                    Vw_DocumentTipos docTipo = new Vw_DocumentTipos();
                    docTipo = GetItem(ID);
                    lDocumentosTiposIDsFinal.Add(docTipo);
                }
            }

            return lDocumentosTiposIDsFinal;
        }

        public List<Vw_DocumentTipos> GetActivos(long ClienteID)
        {
            return (from c in Context.Vw_DocumentTipos 
                    where 
                        c.Activo && 
                        c.EsCarpeta == false &&
                        c.ClienteID == ClienteID 
                    select c).ToList();
        }

        public List<Vw_DocumentTipos> GetSortedActivos()
        {
            return (from c in Context.Vw_DocumentTipos where c.Activo == true orderby c.DocumentTipo select c).ToList();
        }

        public List<Vw_DocumentTipos> GetDocumentosTiposByProyectoTipoID(long ProyectoTipoID)
        {
            return (from c in Context.Vw_DocumentTipos where c.Activo == true && c.EsCarpeta == false && c.ProyectoTipoID == ProyectoTipoID select c).ToList();
        }

        public List<long> GetDocTipoIDByProyTipoID(long proyTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.ProyectoTipoID == proyTipoID select c.DocumentTipoID).ToList();
        }

        public List<Vw_DocumentosPerfiles> GetDocPerfilesByTipoDocumentoID(long tipoDocumentoID)
        {
            return (from c in Context.Vw_DocumentosPerfiles where c.TipoDocumentoID == tipoDocumentoID select c).ToList();
        }

        public List<long> GetProyectosTiposByClienteID(long clienteID)
        {
            return (from c in Context.ClientesProyectosTipos where c.ClienteID == clienteID select c.ProyectoTipoID).ToList();
        }

        public List<DocumentosExtensiones> GetDocExtensionesByClienteID(long clienteID)
        {
            return (from c in Context.DocumentosExtensiones where c.ClienteID == clienteID select c).ToList();
        }

        public List<long> GetDocExtIDByTipoDocID(long tipoDocID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == tipoDocID select c.DocumentoExtensionID).ToList();
        }

        public DocumentosExtensiones GetDocExtensionesByClienteIDYExtension(long clienteID, string extension)
        {
            return (from c in Context.DocumentosExtensiones where c.ClienteID == clienteID && c.Extension.Equals(extension) select c).FirstOrDefault();
        }

        public List<long> GetDocExtByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == docTipoID select c.DocumentoExtensionID).ToList();
        }

        public List<long> GetDoctiposExtByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == docTipoID select c.DocumentoTipoExtensionID).ToList();
        }

        public long GetProyectosTipoIDByDocTipoIDAndProyID(long docTipoID, long ProyID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID && c.ProyectoTipoID == ProyID select c.ProyectoTipoDocumentoTipoID).FirstOrDefault();
        }

        public List<ProyectosTiposDocumentosTipos> GetProyectosTipoByDocTipoID(long docTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID select c).ToList();
        }

        public List<long> GetProyectosTipoIDByDocTipoID(long docTipoID)
        {
            return (from c in Context.ProyectosTiposDocumentosTipos where c.DocumentTipoID == docTipoID select c.ProyectoTipoID).ToList();
        }

        public long GetDocExtIDByDocTipoIDAndProyID(long docTipoID, long docExtID)
        {
            return (from c in Context.DocumentosTiposExtensiones where c.DocumentTipoID == docTipoID && c.DocumentoExtensionID == docExtID select c.DocumentoTipoExtensionID).FirstOrDefault();
        }

        public List<long> GetDocPerfilesIDByDocTipoID(long docTipoID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == docTipoID select c.DocumentoPerfilID).ToList();
        }

        public long GetDocPerfilIDByDocTipoIDAndProyID(long docTipoID, long perfilID)
        {
            return (from c in Context.DocumentosPerfiles where c.TipoDocumentoID == docTipoID && c.PerfilID == perfilID select c.DocumentoPerfilID).FirstOrDefault();
        }

        public List<Perfiles> GetPerfilesByProyTipoIDandClienteID(long tipoProyID, long clienteID)
        {
            return (from c in Context.Perfiles where c.TipoProyectoID == tipoProyID && c.ClienteID == clienteID select c).ToList();
        }

        public List<long?> GetPerfilesByTipoDocID(long tipoDocID)
        {
            return (from c in Context.DocumentosPerfiles where c.PerfilID != null && c.TipoDocumentoID == tipoDocID select c.PerfilID).ToList();
        }

        public Vw_DocumentTipos CheckExisteDocTipoMismoNombre(string nombre)
        {
            return (from c in Context.Vw_DocumentTipos where c.DocumentTipo == nombre select c).FirstOrDefault();
        }

    }
}