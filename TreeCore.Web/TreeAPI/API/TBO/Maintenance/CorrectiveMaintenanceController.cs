using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TreeAPI.API.TBO.Interfaces;
using TreeAPI.DTO.Interfaces;
using TreeAPI.DTO.Salida.Maintenance;
using TreeCore.Data;
using System.Reflection;
using log4net;

namespace TreeAPI.Controllers
{
    [RoutePrefix("api/CorrectiveMaintenance")]
    public class CorrectiveMaintenanceController : ApiBaseController, ICollective
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /*[HttpGet]
        [Route("")]
        public string GetCorrectiveMaintenance(string s)
        {
            return "Hola CorrectiveMaintenanceController";
        }*/

        #region COLLECTIVE INTERFACE

        #region _CI_Create
        public TBOResponse _CI_Create(TreeDataObject Element)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_Disable
        public TBOResponse _CI_Disable(long lIdentifier)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_Enable
        public TBOResponse _CI_Enable(long lIdentifier)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_FindAll
        public List<TreeDataObject> _CI_FindAll(bool bActive, int iMax)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_FindByPrimaryCode
        public TBOResponse _CI_FindByPrimaryCode(string sIdentifier)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_FindByPrimaryKey
        public TreeDataObject _CI_FindByPrimaryKey(long lIdentifier)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_Remove
        public TBOResponse _CI_Remove(long lIdentifier)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _CI_Update
        public TBOResponse _CI_Update(TreeDataObject Element)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
        
        #region INTERNAL METHODS

        #region ConvertToSiteCorrectiveMaintenance
        public static SiteCorrectiveMaintenance ConvertToSiteCorrectiveMaintenance(MantenimientoEmplazamientoCorrectivo emplazamientoCorrectivo)
        {
            SiteCorrectiveMaintenance result = new SiteCorrectiveMaintenance();

            EmplazamientosController cEmplazamientos = new EmplazamientosController();
            MantenimientoEmplazamientosConflictividadController cMantenimientoEmplazamientosConflictividad = new MantenimientoEmplazamientosConflictividadController();
            ProyectosController cProyectos = new ProyectosController();
            MantenimientoIncidenciasController cMantenimientoIncidencias = new MantenimientoIncidenciasController();
            MantenimientoEmplazamientosAgenciasController cMantenimientoEmplazamientosAgencias = new MantenimientoEmplazamientosAgenciasController();
            UsuariosController cUsuarios = new UsuariosController();

            try
            {
                if (emplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoID > 0)
                {
                    result.lIdentifierLong = emplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoID;
                }

                Emplazamientos emplazamiento = cEmplazamientos.GetItem(emplazamientoCorrectivo.EmplazamientoID);
                if (emplazamiento != null)
                {
                    result.sSiteCode = emplazamiento.Codigo;
                }

                MantenimientoEmplazamientosConflictividad mantenimientoEmplazamientosConflictividad = cMantenimientoEmplazamientosConflictividad.GetItem(emplazamientoCorrectivo.MantenimientoEmplazamientoConflictividadID);
                if (mantenimientoEmplazamientosConflictividad != null)
                {
                    result.sConflictLevel = mantenimientoEmplazamientosConflictividad.Conflictividad_esES;
                }

                Proyectos proyecto = cProyectos.GetItem(emplazamientoCorrectivo.ProyectoID);
                if (proyecto != null)
                {
                    result.sProject = proyecto.Proyecto;
                }

                if (emplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoSeguimientoID.HasValue)
                {
                    MantenimientoEmplazamientosCorrectivosSeguimientosController cMantenimientoEmplazamientosCorrectivosSeguimientos = new MantenimientoEmplazamientosCorrectivosSeguimientosController();
                    MantenimientoEmplazamientosCorrectivosSeguimientos seguimiento = cMantenimientoEmplazamientosCorrectivosSeguimientos.GetItem(emplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoSeguimientoID.Value);

                    if (seguimiento != null)
                    {
                        MantenimientoEmplazamientosCorrectivosEstadosController cMantenimientoEmplazamientosEstados = new MantenimientoEmplazamientosCorrectivosEstadosController();

                        MantenimientoEmplazamientosCorrectivosEstados estado = cMantenimientoEmplazamientosEstados.GetItem(seguimiento.MantenimientoEmplazamientoCorrectivoEstadoID);
                        if (estado != null)
                        {
                            if (estado.MantenimientoTipologiaID.HasValue)
                            {
                                MantenimientoTipologiasController cMantenimientoTipologias = new MantenimientoTipologiasController();
                                MantenimientoTipologias tipologia = cMantenimientoTipologias.GetItem(estado.MantenimientoTipologiaID.Value);

                                if (tipologia != null)
                                {
                                    result.sWorkflowType = tipologia.MantenimientoTipologia;
                                }
                                else
                                {
                                    result.sWorkflowType = string.Empty;
                                }
                            }
                            else
                            {
                                result.sWorkflowType = string.Empty;
                            }
                        }
                        else
                        {
                            result.sWorkflowType = string.Empty;
                        }
                    }
                    else
                    {
                        result.sWorkflowType = string.Empty;
                    }
                }
                else
                {
                    result.sWorkflowType = string.Empty;
                }

                result.bActive = emplazamientoCorrectivo.Activo;

                if (emplazamientoCorrectivo.MantenimientoIncidenciaID != null)
                {
                    MantenimientoIncidencias incidencia = cMantenimientoIncidencias.GetItem(Convert.ToInt64(emplazamientoCorrectivo.MantenimientoIncidenciaID));
                    result.sMaintenanceIncident = incidencia.MantenimientoIncidencia;
                }

                if (emplazamientoCorrectivo.MantenimientoEmplazamientoAgenciaID != null)
                {
                    MantenimientoEmplazamientosAgencias agencia = cMantenimientoEmplazamientosAgencias.GetItem(Convert.ToInt64(emplazamientoCorrectivo.MantenimientoEmplazamientoAgenciaID));
                    if (agencia != null)
                    {
                        result.sAgency = agencia.Agencia;
                    }
                }

                if (emplazamientoCorrectivo.UsuarioID != null)
                {
                    Usuarios usuario = cUsuarios.GetItem(Convert.ToInt64(emplazamientoCorrectivo.UsuarioID));
                    if (usuario != null)
                    {
                        result.sUserEmail = usuario.EMail;
                    }
                }

                if (emplazamientoCorrectivo.MantenimientoCriticidadID != null)
                {
                    MantenimientoCriticidadesController cMantenimientoCriticidades = new MantenimientoCriticidadesController();
                    MantenimientoCriticidades criticidad = cMantenimientoCriticidades.GetItem(Convert.ToInt64(emplazamientoCorrectivo.MantenimientoCriticidadID));
                    if (criticidad != null)
                    {
                        result.sSeverity = criticidad.Criticidad;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }
        #endregion

        #endregion
    }
}