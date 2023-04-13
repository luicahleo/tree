using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using System.Web.Script.Serialization;
using TreeAPI.API.TBO.Interfaces;
using TreeAPI.DTO.Interfaces;
using TreeCore.Clases;
using TreeCore.Data;
using log4net;
using System.Reflection;


namespace TreeAPI.Controllers
{
    [RoutePrefix("api/MaintenanceIncidents")]
    public class MaintenanceIncidentsController : ApiBaseController/*, ICollective*/
    {
        #region CONSTANTES
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string language = GetLanguage();
        private const string NAME_CLASS = "MaintenanceIncidents";
        private const string MANTENIMIENTOS_CORRECTIVOS = "MANTENIMIENTOS CORRECTIVOS";
        private const string CANCELADO = "CANCELADO";
        private const string BLOQUEADO = "BLOQUEADO";
        private readonly string NOTA_SEGUIMIENTO_CANCELADO = GetGlobalResource(Comun.TBO.TAG_RECURSO.NotaEstadoCancelado, language);
        private readonly string NOTA_SEGUIMIENTO_BLOQUEADO = GetGlobalResource(Comun.TBO.TAG_RECURSO.NotaEstadoBloqueado, language);
        #endregion

        #region COMUN INTERFACE

        #region _CI_Create
        /// <summary>
        /// Create incidence
        /// </summary>
        /// <param name="Element">Data of incidence</param>
        /// <param name="sUser">User email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        //[ActionName("_CI_Create")]
        public TBOResponse _CI_Create(DTO.Entrada.Maintenance.MaintenanceIncidence Element, string sUser)
        {
            // Local variables
            TBOResponse response = null;
            DTO.Entrada.Maintenance.MaintenanceIncidence nuevo = null;
            MantenimientoIncidencias incidencia = null;
            Emplazamientos emplazamiento = null;
            MantenimientoIncidenciasTipos categoria = null;
            InventarioElementos elementoInventario = null;
            bool exitoCamposVacios = true;
            bool exitoLongitudCampos = true;
            bool fechasValidas = true;
            string sMissing = "";
            string cambosExcedidos = "";
            string fechasNoValidas = "";

            // Controllers
            MantenimientoIncidenciasController cIncidencias = null;
            EmplazamientosController cEmplazamientos = null;
            MantenimientoIncidenciasTiposController cMantenimientoIncidenciasTipos = null;
            InventarioElementosController cInventarioElementos = null;
            long? emplazamientoID = null;

            #region MONITORING_INICIAL
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sTipoProyecto = Comun.MODULOMANTENIMIENTO;
            string sServicio = Comun.INTEGRACION_SERVICIO_API;
            string sMetodo = Comun.TBO.Metodo.TBO_METODO_MAINTENANCE_INCIDENTS_CI_CREATE;
            string sComentarios = "Invocacion del método _CI_Create del TBO de MaintenanceIncidents";

            bool bPropio = true;
            string UsuarioEmail = (Element != null) ? sUser : string.Empty;
            string sParametroEntrada = GetInputParameter(sMetodo, UsuarioEmail, Element, "", "", "", "", "");
            string sParametroSalida = "";
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;
            long? monitoringUsuarioID = null;
            long? monitoringClienteID = null;
            #endregion


            try
            {
                nuevo = Element;

                if (nuevo != null)
                {
                    cIncidencias = new MantenimientoIncidenciasController();
                    cMantenimientoIncidenciasTipos = new MantenimientoIncidenciasTiposController();
                    cInventarioElementos = new InventarioElementosController();
                    cEmplazamientos = new EmplazamientosController();

                    #region COMPROBAR CAMPOS VACIOS
                    if (string.IsNullOrEmpty(sUser))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sUser) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sMaintenanceIncident))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sMaintenanceIncident) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sCategory))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sCategory) + " ";
                    }

                    if (string.IsNullOrEmpty(nuevo.sElementInventory) && string.IsNullOrEmpty(nuevo.sSiteCode))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(nuevo.sSiteCode) + " || " + nameof(nuevo.sElementInventory) + " ";
                    }

                    if (nuevo.bInProcess)
                    {
                        #region CAMPOS OBLIGATORIOS IN PROCESS
                        if (string.IsNullOrEmpty(nuevo.sProject))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(nuevo.sProject) + " ";
                        }
                        if (string.IsNullOrEmpty(nuevo.sAgency))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(nuevo.sAgency) + " ";
                        }
                        if (string.IsNullOrEmpty(nuevo.sSeverity))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(nuevo.sSeverity) + " ";
                        }
                        if (string.IsNullOrEmpty(nuevo.sWorkflowType))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(nuevo.sWorkflowType) + " ";
                        }
                        if (string.IsNullOrEmpty(nuevo.sConflictLevel))
                        {
                            exitoCamposVacios = false;
                            sMissing = sMissing + " - " + nameof(nuevo.sConflictLevel) + " ";
                        }
                        #endregion
                    }
                    #endregion

                    if (exitoCamposVacios) {

                        

                        if (fechasValidas)
                        {
                            #region COMPROBAR LONGITUD CAMPOS
                            if (sUser != null && sUser.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(sUser) + "("+ Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario + ") ";
                            }
                            if (nuevo.sMaintenanceIncident != null && nuevo.sMaintenanceIncident.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.nombreIncidencia)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sMaintenanceIncident) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.nombreIncidencia + ") ";
                            }
                            if (nuevo.sMaintenanceDescriptionIncident != null && nuevo.sMaintenanceDescriptionIncident.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.descripcionIncidencia)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sMaintenanceDescriptionIncident) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.descripcionIncidencia + ") ";
                            }
                            if (nuevo.sSiteCode != null && nuevo.sSiteCode.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sSiteCode) + "("+ Comun.TBO.LENGTH_CAMPOS.GENERAL.codigoEmplazamiento + ") ";
                            }
                            if (nuevo.sCategory != null && nuevo.sCategory.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sCategory) + "("+ Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.mantenimientoIncidenciasTipo + ") ";
                            }
                            if (nuevo.sElementInventory != null && nuevo.sElementInventory.Length > Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sElementInventory) + "("+ Comun.TBO.LENGTH_CAMPOS.INVENTORY.numeroInventario + ") ";
                            }
                            if (nuevo.sProject != null && nuevo.sProject.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.proyecto)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sProject) + "("+ Comun.TBO.LENGTH_CAMPOS.GENERAL.proyecto + ") ";
                            }
                            if (nuevo.sAgency != null && nuevo.sAgency.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.agencia)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sAgency) + "("+ Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.agencia + ") ";
                            }
                            if (nuevo.sSeverity != null && nuevo.sSeverity.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.criticidad)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sSeverity) + "("+ Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.criticidad + ") ";
                            }
                            if (nuevo.sWorkflowType != null && nuevo.sWorkflowType.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.tipologia)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sWorkflowType) + "("+ Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.tipologia + ") ";
                            }
                            if (nuevo.sConflictLevel != null && nuevo.sConflictLevel.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.conflictividad)
                            {
                                exitoLongitudCampos = false;
                                cambosExcedidos += " - " + nameof(nuevo.sConflictLevel) + "("+ Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.conflictividad + ") ";
                            }
                            #endregion

                            if (exitoLongitudCampos)
                            {
                                this.SetUsuario(sUser);

                                if (user != null)
                                {
                                    categoria = cMantenimientoIncidenciasTipos.GetByNombreAndClienteID(nuevo.sCategory, user.ClienteID.Value);

                                    if (categoria != null)
                                    {
                                        if (string.IsNullOrEmpty(nuevo.sElementInventory))
                                        {
                                            emplazamiento = cEmplazamientos.GetEmplazamientoByCodigo(nuevo.sSiteCode, user.ClienteID.Value);
                                            if (emplazamiento == null)
                                            {
                                                exitoCamposVacios = false;
                                                response = new TBOResponse()
                                                {
                                                    Result = false,
                                                    Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                                                    Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                                                };
                                            }
                                        }
                                        else
                                        {
                                            elementoInventario = cInventarioElementos.GetByNumberAndClienteID(nuevo.sElementInventory, user.ClienteID.Value);
                                            if (elementoInventario != null && elementoInventario.EmplazamientoID != null)
                                            {
                                                emplazamiento = cEmplazamientos.GetItem(elementoInventario.EmplazamientoID.Value);
                                            }
                                            else
                                            {
                                                exitoCamposVacios = false;
                                                response = new TBOResponse()
                                                {
                                                    Result = false,
                                                    Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_ELEMENT_INVENTORY_NOT_FOUND_CODE,
                                                    Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_ELEMENT_INVENTORY_NOT_FOUND_DESCRIPTION
                                                };
                                            }
                                        }

                                        #region MONITORING
                                        monitoringUsuarioID = (user == null) ? null : (long?)user.UsuarioID;
                                        monitoringAlquilerID = null;
                                        monitoringEmplazamientoID = emplazamientoID;
                                        #endregion

                                        if (exitoCamposVacios)
                                        {
                                            ResponseCreateController responseCreate = cIncidencias.CrearElemento(nuevo.sMaintenanceIncident, nuevo.bEmergency, user, emplazamiento.EmplazamientoID, false,
                                                                                    nuevo.sMaintenanceDescriptionIncident, categoria, elementoInventario);
                                            if (responseCreate.Data != null)
                                            {
                                                incidencia = (MantenimientoIncidencias)responseCreate.Data;

                                                if (incidencia != null)
                                                {
                                                    if (nuevo.bInProcess)
                                                    {
                                                        response = _SI_Accept(incidencia.MantenimientoIncidenciaID, nuevo.sProject, nuevo.sAgency, nuevo.sSeverity, nuevo.sWorkflowType, sUser, nuevo.sConflictLevel);

                                                        if (response.Result)
                                                        {
                                                            incidencia.EnTramite = nuevo.bInProcess;

                                                            DTO.Salida.Maintenance.MaintenanceIncidence maintenanceIncidences = ConvertToIncidence(incidencia);
                                                            DTO.Salida.Maintenance.SiteCorrectiveMaintenance siteCorrective = (DTO.Salida.Maintenance.SiteCorrectiveMaintenance)response.Data;
                                                            maintenanceIncidences.lIdentifierCorrectiveMaintenance = siteCorrective.lIdentifier;
                                                            maintenanceIncidences.sSiteCode = siteCorrective.sSiteCode;
                                                            maintenanceIncidences.sProject = siteCorrective.sProject;
                                                            maintenanceIncidences.sAgency = siteCorrective.sAgency;
                                                            maintenanceIncidences.sSeverity = siteCorrective.sSeverity;
                                                            maintenanceIncidences.sWorkflowType = siteCorrective.sWorkflowType;
                                                            maintenanceIncidences.sConflictLevel = siteCorrective.sConflictLevel;

                                                            response = new TBOResponse
                                                            {
                                                                Result = true,
                                                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                                Data = maintenanceIncidences
                                                            };
                                                        }
                                                        else
                                                        {
                                                            cIncidencias.DeleteItem(incidencia.MantenimientoIncidenciaID);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = true,
                                                            Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                            Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                            Data = ConvertToIncidence(incidencia)
                                                        };
                                                    }
                                                }
                                                else
                                                {
                                                    response = new TBOResponse
                                                    {
                                                        Result = false,
                                                        Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
                                                        Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                response = new TBOResponse()
                                                {
                                                    Result = responseCreate.infoResponse.Result,
                                                    Code = responseCreate.infoResponse.Code,
                                                    Description = responseCreate.infoResponse.Description
                                                };
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response = new TBOResponse
                                        {
                                            Result = false,
                                            Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_CODE,
                                            Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_CATEGORY_NOT_FOUND_DESCRIPTION
                                        };
                                    }
                                }
                                else
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                                    };
                                }
                            }
                            else
                            {
                                if (response == null)
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_CODE,
                                        Description = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_DESCRIPTION + cambosExcedidos
                                    };
                                }
                            }

                        }
                        else
                        {
                            response = new TBOResponse
                            {
                                Result = false,
                                Code = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_INCORRECT_DATE_FORMAT_DESCRIPTION + fechasNoValidas
                            };
                        }
                    }
                    else
                    {
                        response = new TBOResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                        };
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_CODE,
                        Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
            }

            #region MONITORING_FINAL
            sParametroSalida = GetOutputParameter(response);
            sComentarios = response.Description;
            cMonitoring.AgregarRegistro(sTipoProyecto, monitoringUsuarioID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, monitoringClienteID, response.Result, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
            #endregion

            // Returns the result
            return response;
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

        #region SINGULAR INTERFACE

        #region _SI_Accept
        /// <summary>
        /// Accept incidence
        /// </summary>
        /// <param name="lIncidentID">ID incidence</param>
        /// <param name="sProject">Name proyect</param>
        /// <param name="sAgency">Name agency</param>
        /// <param name="sSeverity">Name severity</param>
        /// <param name="sWorkflowType">Name typology</param>
        /// <param name="sUserEmail">User email</param>
        /// <param name="sConflictLevel">Conflict level</param>
        /// <returns></returns>

        [HttpPost]
        [Route("Accept")]
        public TBOResponse _SI_Accept(
            long lIncidentID,
            string sProject,
            string sAgency,
            string sSeverity,
            string sWorkflowType,
            string sUserEmail,
            string sConflictLevel)
        {
            //Local variables
            TBOResponse response;
            bool exitoCamposVacios = true; 
            bool exitoLongitudCampos = true;
            string sMissing = "";
            string cambosExcedidos = "";

            #region MONITORING_INICIAL
            MonitoringWSRegistrosController cMonitoring = new MonitoringWSRegistrosController();
            string sTipoProyecto = Comun.MODULOMANTENIMIENTO;
            string sServicio = Comun.INTEGRACION_SERVICIO_API;
            string sMetodo = Comun.TBO.Metodo.TBO_METODO_MAINTENANCE_INCIDENTS_SI_ACCEPT;
            string sComentarios = "Invocacion del método _SI_Accept del TBO de MaintenanceIncidents";

            bool bPropio = true;
            string UsuarioEmail = sUserEmail;
            string sParametroEntrada = GetInputParameter(sMetodo, UsuarioEmail, null, sProject, sAgency, sSeverity, sWorkflowType, sConflictLevel);
            string sParametroSalida = "";
            long? monitoringAlquilerID = null;
            long? monitoringEmplazamientoID = null;
            long? monitoringUsuarioID = null;
            long? monitoringClienteID = null;
            #endregion

            //Controllers 
            MantenimientoIncidenciasController cMantenimientoIncidencias = new MantenimientoIncidenciasController();
            ProyectosController cProyectos = new ProyectosController();
            ProyectosTiposController cProyectosTipos = new ProyectosTiposController();
            ClientesController cClientes = new ClientesController();
            MantenimientoTipologiasController cMantenimientoTipologias = new MantenimientoTipologiasController();
            MantenimientoCriticidadesController cMantenimientoCriticidades = new MantenimientoCriticidadesController();
            MantenimientoEmplazamientosAgenciasController cMantenimientoEmplazamientosAgencias = new MantenimientoEmplazamientosAgenciasController();
            MantenimientoEmplazamientosCorrectivosController cMantenimientoEmplazamientosCorrectivos = new MantenimientoEmplazamientosCorrectivosController();
            MantenimientoEmplazamientosConflictividadController cMantenimientoEmplazamientosConflictividad = new MantenimientoEmplazamientosConflictividadController();

            try
            {
                if (lIncidentID > 0)
                {
                    #region COMPROBAR CAMPOS VACIOS
                    if (string.IsNullOrEmpty(sProject))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sProject) + " ";
                    }
                    if (string.IsNullOrEmpty(sAgency))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sAgency) + " ";
                    }
                    if (string.IsNullOrEmpty(sSeverity))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sSeverity) + " ";
                    }
                    if (string.IsNullOrEmpty(sWorkflowType))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sWorkflowType) + " ";
                    }
                    if (string.IsNullOrEmpty(sUserEmail))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sUserEmail) + " ";
                    }
                    if (string.IsNullOrEmpty(sConflictLevel))
                    {
                        exitoCamposVacios = false;
                        sMissing = sMissing + " - " + nameof(sConflictLevel) + " ";
                    }
                    #endregion

                    if (exitoCamposVacios)
                    {
                        #region COMPROBAR LONGITUD CAMPOS
                        if (sUserEmail != null && sUserEmail.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sUserEmail) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.emailUsuario + ") ";
                        }
                        if (sProject != null && sProject.Length > Comun.TBO.LENGTH_CAMPOS.GENERAL.proyecto)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sProject) + "(" + Comun.TBO.LENGTH_CAMPOS.GENERAL.proyecto + ") ";
                        }
                        if (sAgency != null && sAgency.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.agencia)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sAgency) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.agencia + ") ";
                        }
                        if (sSeverity != null && sSeverity.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.criticidad)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sSeverity) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.criticidad + ") ";
                        }
                        if (sWorkflowType != null && sWorkflowType.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.tipologia)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sWorkflowType) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.tipologia + ") ";
                        }
                        if (sConflictLevel != null && sConflictLevel.Length > Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.conflictividad)
                        {
                            exitoLongitudCampos = false;
                            cambosExcedidos += " - " + nameof(sConflictLevel) + "(" + Comun.TBO.LENGTH_CAMPOS.MAINTENANCE.conflictividad + ") ";
                        }
                        #endregion

                        if (exitoLongitudCampos)
                        {
                            this.SetUsuario(sUserEmail);

                            if (base.user != null)
                            {
                                #region MONITORING
                                monitoringClienteID = user.ClienteID;
                                #endregion

                                MantenimientoIncidencias incidencia = cMantenimientoIncidencias.GetItem(lIncidentID);

                                if (incidencia != null)
                                {
                                    #region MONITORING
                                    monitoringClienteID = user.ClienteID;
                                    monitoringUsuarioID = base.user.UsuarioID;
                                    monitoringAlquilerID = null;
                                    monitoringEmplazamientoID = incidencia.EmplazamientoID;
                                    #endregion

                                    if (!incidencia.Cancelada.HasValue || !incidencia.Cancelada.Value)
                                    {
                                        if (!incidencia.EnTramite.HasValue || !incidencia.EnTramite.Value)
                                        {
                                            if (user != null)
                                            {
                                                if (user.ClienteID != null)
                                                {
                                                    Clientes cliente = cClientes.GetItem(user.ClienteID.Value);
                                                    if (cliente != null)
                                                    {
                                                        ProyectosTipos proyectoTipo = cProyectosTipos.GetProyectosTiposByNombre(MANTENIMIENTOS_CORRECTIVOS);
                                                        Proyectos proyecto = cProyectos.GetProyectoByNombreCliente(sProject, cliente.ClienteID, proyectoTipo.ProyectoTipoID);

                                                        if (proyecto != null)
                                                        {
                                                            MantenimientoEmplazamientosAgencias agencia = cMantenimientoEmplazamientosAgencias.GetByNombreYProyecto(sAgency, proyecto.ProyectoID);
                                                            if (agencia != null)
                                                            {
                                                                MantenimientoCriticidades criticidad = cMantenimientoCriticidades.GetCriticidad(sSeverity);
                                                                MantenimientoTipologias tipologia = cMantenimientoTipologias.GetByMantenimientoTipologia(sWorkflowType);
                                                                MantenimientoEmplazamientosConflictividad conflictividad = cMantenimientoEmplazamientosConflictividad.GetByConflictividad(sConflictLevel);

                                                                if (criticidad != null && tipologia != null && conflictividad != null)
                                                                {
                                                                    if (incidencia.EmplazamientoID.HasValue)
                                                                    {
                                                                        ResponseCreateController responseCreate = cMantenimientoEmplazamientosCorrectivos.CrearElemento(
                                                                            incidencia.EmplazamientoID.Value, proyecto.ProyectoID, conflictividad.MantenimientoEmplazamientoConflictividadID, incidencia.MantenimientoIncidenciaID,
                                                                            agencia.MantenimientoEmplazamientoAgenciaID, criticidad.MantenimientoCriticidadID, incidencia.Latitud, incidencia.Longitud, user.UsuarioID,
                                                                            tipologia.MantenimientoTipologiaID, incidencia);

                                                                        if (responseCreate.Data != null)
                                                                        {
                                                                            MantenimientoEmplazamientoCorrectivo mantenimientoEmplazamientoCorrectivo = (MantenimientoEmplazamientoCorrectivo)responseCreate.Data;
                                                                            if (mantenimientoEmplazamientoCorrectivo != null)
                                                                            {
                                                                                response = new TBOResponse
                                                                                {
                                                                                    Result = true,
                                                                                    Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                                                                    Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                                                                    Data = CorrectiveMaintenanceController.ConvertToSiteCorrectiveMaintenance(mantenimientoEmplazamientoCorrectivo)
                                                                                };
                                                                            }
                                                                            else
                                                                            {
                                                                                response = new TBOResponse
                                                                                {
                                                                                    Result = false,
                                                                                    Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
                                                                                    Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
                                                                                };
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            response = new TBOResponse()
                                                                            {
                                                                                Result = responseCreate.infoResponse.Result,
                                                                                Code = responseCreate.infoResponse.Code,
                                                                                Description = responseCreate.infoResponse.Description,
                                                                                Data = responseCreate.infoResponse.Data
                                                                            };
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        response = new TBOResponse
                                                                        {
                                                                            Result = false,
                                                                            Code = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_CODE,
                                                                            Description = ServicesCodes.GENERIC.COD_TBO_SITE_NOT_FOUND_DESCRIPTION
                                                                        };
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (criticidad == null)
                                                                    {
                                                                        response = new TBOResponse
                                                                        {
                                                                            Result = false,
                                                                            Code = ServicesCodes.GENERIC.COD_TBO_SEVERITY_NOT_FOUND_CODE,
                                                                            Description = ServicesCodes.GENERIC.COD_TBO_SEVERITY_NOT_FOUND_DESCRIPTION
                                                                        };
                                                                    }
                                                                    else if (tipologia == null)
                                                                    {
                                                                        response = new TBOResponse
                                                                        {
                                                                            Result = false,
                                                                            Code = ServicesCodes.GENERIC.COD_TBO_TYPOLOGY_NOT_FOUND_CODE,
                                                                            Description = ServicesCodes.GENERIC.COD_TBO_WORKFLOW_TYPE_NOT_FOUND_DESCRIPTION
                                                                        };
                                                                    }
                                                                    else if (conflictividad == null)
                                                                    {
                                                                        response = new TBOResponse
                                                                        {
                                                                            Result = false,
                                                                            Code = ServicesCodes.GENERIC.COD_TBO_CONFLICT_LEVEL_NOT_FOUND_CODE,
                                                                            Description = ServicesCodes.GENERIC.COD_TBO_CONFLICT_LEVEL_NOT_FOUND_DESCRIPTION
                                                                        };
                                                                    }
                                                                    else
                                                                    {
                                                                        response = new TBOResponse
                                                                        {
                                                                            Result = false,
                                                                            Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_FOUND_CODE,
                                                                            Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_FOUND_DESCRIPTION
                                                                        };
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                response = new TBOResponse
                                                                {
                                                                    Result = false,
                                                                    Code = ServicesCodes.GENERIC.COD_TBO_AGENCY_NOT_FOUND_CODE,
                                                                    Description = ServicesCodes.GENERIC.COD_TBO_AGENCY_NOT_FOUND_DESCRIPTION
                                                                };
                                                            }
                                                        }
                                                        else
                                                        {
                                                            response = new TBOResponse
                                                            {
                                                                Result = false,
                                                                Code = ServicesCodes.GENERIC.COD_TBO_PROYECT_NOT_FOUND_CODE,
                                                                Description = ServicesCodes.GENERIC.COD_TBO_PROYECT_NOT_FOUND_DESCRIPTION
                                                            };
                                                        }
                                                    }
                                                    else
                                                    {
                                                        response = new TBOResponse
                                                        {
                                                            Result = false,
                                                            Code = ServicesCodes.GENERIC.COD_TBO_CLIENT_NOT_FOUND_CODE,
                                                            Description = ServicesCodes.GENERIC.COD_TBO_CLIENT_NOT_FOUND_DESCRIPTION
                                                        };
                                                    }
                                                }
                                                else
                                                {
                                                    response = new TBOResponse
                                                    {
                                                        Result = false,
                                                        Code = ServicesCodes.GENERIC.COD_TBO_CLIENT_NOT_FOUND_CODE,
                                                        Description = ServicesCodes.GENERIC.COD_TBO_CLIENT_NOT_FOUND_DESCRIPTION
                                                        // REVIEW - En realidad deberia ser que no tiene el cliente el usuari pero lo modificamos despues
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                response = new TBOResponse
                                                {
                                                    Result = false,
                                                    Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                                                    Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                                                    // REVIEW - Lo comprueba después también, así qu una vez que se comprueba aquí, debería no comprobarse despuést
                                                };
                                            }

                                        }
                                        else
                                        {
                                            response = new TBOResponse
                                            {
                                                Result = false,
                                                Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_IS_IN_PROCESS_CODE,
                                                Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_IS_IN_PROCESS_DESCRIPTION
                                            };
                                        }
                                    }
                                    else
                                    {
                                        response = new TBOResponse
                                        {
                                            Result = false,
                                            Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_CANCEL_CODE,
                                            Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_CANCEL_DESCRIPTION
                                        };
                                    }
                                }
                                else
                                {
                                    response = new TBOResponse
                                    {
                                        Result = false,
                                        Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_NOT_FOUND_CODE,
                                        Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_NOT_FOUND_DESCRIPTION
                                    };
                                }
                            }
                            else
                            {
                                response = new TBOResponse
                                {
                                    Result = false,
                                    Code = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_CODE,
                                    Description = ServicesCodes.GENERIC.COD_TBO_USER_NOT_FOUND_DESCRIPTION
                                };
                            }
                        }
                        else
                        {
                            response = new TBOResponse
                            {
                                Result = false,
                                Code = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_LENGTH_DATA_EXCEEDS_DESCRIPTION + cambosExcedidos
                            };
                        }
                    }
                    else
                    {
                        response = new TBOResponse
                        {
                            Result = false,
                            Code = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION,
                            Description = ServicesCodes.GENERIC.COD_TBO_MISSING_DATA_DESCRIPTION + sMissing
                        };
                    }
                }
                else
                {
                    response = new TBOResponse
                    {
                        Result = false,
                        Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_NOT_FOUND_CODE,
                        Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_INCIDENCE_NOT_FOUND_DESCRIPTION
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                response = new TBOResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
            }

            #region MONITORING_FINAL
            sParametroSalida = GetOutputParameter(response);
            sComentarios = response.Description;
            cMonitoring.AgregarRegistro(sTipoProyecto, monitoringUsuarioID, sParametroEntrada, sParametroSalida, sServicio, sMetodo, sComentarios, monitoringClienteID, response.Result, bPropio, monitoringAlquilerID, monitoringEmplazamientoID);
            #endregion

            return response;
        }
        #endregion

        #endregion

        #region INTERNAL METHODS

        #region ConvertToIncidencia
        private MantenimientoIncidencias ConvertToIncidencia(DTO.Entrada.Maintenance.MaintenanceIncidence nuevo)
        {
            // Local variables
            MantenimientoIncidencias incidencia = null;
            Clientes cliente = null;
            Emplazamientos emplazamiento = null;
            Usuarios user = null;
            bool bExito = true;

            // Controllers
            //ClientesController cClientes = null;
            EmplazamientosController cEmplazamientos = null;
            UsuariosController cUsuarios = null;

            try
            {

                if (nuevo != null)
                {
                    incidencia = new MantenimientoIncidencias();
                    /*incidencia.Cancelada = nuevo.bCancelled;*/
                    incidencia.Emergencia = nuevo.bEmergency;
                    incidencia.MantenimientoIncidencia = nuevo.sMaintenanceIncident;
                    incidencia.MantenimientoDescripcionIncidencia = nuevo.sMaintenanceDescriptionIncident;

                    // Client
                    /*if (nuevo.sClient != null)
                    {
                        cClientes = new ClientesController();
                        cliente = cClientes.GetClienteByCIF(nuevo.sClient);
                        if (cliente != null)
                        {
                            nuevo.sClient = cliente.CIF;
                        }
                    }*/

                    // Emplazamientos
                    if (nuevo.sSiteCode != null && !nuevo.sSiteCode.Equals(""))
                    {
                        cEmplazamientos = new EmplazamientosController();
                        emplazamiento = cEmplazamientos.GetEmplazamientoByCodigo(nuevo.sSiteCode, cliente.ClienteID);
                        if (emplazamiento != null)
                        {
                            incidencia.EmplazamientoID = emplazamiento.EmplazamientoID;
                        }
                        else
                        {
                            bExito = false;
                        }
                    }
                    incidencia.EnTramite = nuevo.bInProcess;
                    //incidencia.Fecha = nuevo.dDateDT;
                    incidencia.Latitud = nuevo.dLatitudeFloat;
                    incidencia.Longitud = nuevo.dLongitudeFloat;

                    if (nuevo.sEmail != null && !nuevo.sEmail.Equals(""))
                    {
                        cUsuarios = new UsuariosController();
                        user = cUsuarios.getUsuarioByEmail(nuevo.sEmail);
                        if (user != null)
                        {
                            incidencia.UsuarioID = user.UsuarioID;
                        }
                    }
                    else
                    {
                        bExito = false;
                    }


                    if (!bExito)
                    {
                        incidencia = null;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                incidencia = null;
            }

            // Returns the result
            return incidencia;
        }
        #endregion

        #region ConvertToIncidence
        private DTO.Salida.Maintenance.MaintenanceIncidence ConvertToIncidence(MantenimientoIncidencias incidencia)
        {
            // Local variables
            DTO.Salida.Maintenance.MaintenanceIncidence respuesta = null;
            Usuarios user = null;


            // Controllers            
            EmplazamientosController cEmplazamientos = null;
            UsuariosController cUsuarios = null;


            try
            {
                respuesta = new DTO.Salida.Maintenance.MaintenanceIncidence();

                if (incidencia != null)
                {
                    if (incidencia.MantenimientoIncidenciaID > 0)
                    {
                        respuesta.lIncidenceID = incidencia.MantenimientoIncidenciaID;
                    }

                    respuesta.dDateDT = incidencia.Fecha;
                    // Cliente
                    cEmplazamientos = new EmplazamientosController();

                    respuesta.sMaintenanceDescriptionIncidence = incidencia.MantenimientoDescripcionIncidencia;
                    if (incidencia.UsuarioID != null)
                    {
                        cUsuarios = new UsuariosController();
                        user = cUsuarios.GetItem((long)incidencia.UsuarioID);
                        if (user != null)
                        {
                            respuesta.sEmail = user.EMail;
                        }
                    }
                    if (incidencia.Emergencia != null)
                    {
                        respuesta.bEmergency = (bool)incidencia.Emergencia;
                    }
                    else
                    {
                        respuesta.bEmergency = false;
                    }

                    if (incidencia.InventarioElementoID.HasValue)
                    {
                        InventarioElementosController cInventarioElementos = new InventarioElementosController();
                        InventarioElementos inventarioElemento = cInventarioElementos.GetItem(incidencia.InventarioElementoID.Value);
                        if (inventarioElemento != null)
                        {
                            respuesta.sElementInventory = inventarioElemento.NumeroInventario;
                        }
                    }

                    if (incidencia.MantenimientoIncidenciaTipoID.HasValue)
                    {
                        MantenimientoIncidenciasTiposController cMantenimientoIncidencias = new MantenimientoIncidenciasTiposController();
                        MantenimientoIncidenciasTipos categoria =  cMantenimientoIncidencias.GetItem(incidencia.MantenimientoIncidenciaTipoID.Value);
                        respuesta.sCategory = categoria.MantenimientoIncidenciaTipo;
                    }

                    respuesta.sMaintenanceIncidence = incidencia.MantenimientoIncidencia;
                    if (incidencia.EnTramite != null)
                    {
                        respuesta.bInProcess = (bool)incidencia.EnTramite;
                    }
                    else
                    {
                        respuesta.bInProcess = false;
                    }
                    if (incidencia.Latitud != null)
                    {
                        respuesta.dLatitudeFloat = (float)incidencia.Latitud;
                    }

                    if (incidencia.Longitud != null)
                    {
                        respuesta.dLongitudeFloat = (float)incidencia.Longitud;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                respuesta = null;
            }

            // returns the result
            return respuesta;
        }
        #endregion

        #region UpdateIncidencia
        private MantenimientoIncidencias UpdateIncidencia(MantenimientoIncidencias incidenciaOld, MantenimientoIncidencias incidenciaInput)
        {
            incidenciaOld.MantenimientoIncidencia = incidenciaInput.MantenimientoIncidencia;
            incidenciaOld.Fecha = incidenciaInput.Fecha;
            incidenciaOld.MantenimientoDescripcionIncidencia = incidenciaInput.MantenimientoDescripcionIncidencia;
            incidenciaOld.UsuarioID = incidenciaInput.UsuarioID;
            incidenciaOld.EmplazamientoID = incidenciaInput.EmplazamientoID;
            incidenciaOld.Emergencia = incidenciaInput.Emergencia;
            incidenciaOld.EnTramite = incidenciaInput.EnTramite;
            incidenciaOld.Cancelada = incidenciaInput.Cancelada;
            incidenciaOld.Latitud = incidenciaInput.Latitud;
            incidenciaOld.Longitud = incidenciaInput.Longitud;

            return incidenciaOld;
        }
        #endregion

        #endregion

        #region MONITORING

        #region GetInputParameter
        private string GetInputParameter(string Metodo, string sUser, DTO.Entrada.Maintenance.MaintenanceIncidence element, string sProject, string sAgency, string sSeverity, string sWorkflowType, string sConflictLevel)
        {
            string result = string.Empty;

            string url = this.Request.RequestUri.ToString(); //url con parametros
            string method = this.Request.Method.Method; //GET | POST | ...
            //string MediaType = this.Request.Content.Headers.ContentType.MediaType; //json:aplication

            result += method + Comun.NuevaLinea;
            result += url + Comun.NuevaLinea;


            //string lObjectType = (element != null) ? element.ToString() : string.Empty;
            //string Date = (element != null && element.dCreationDate != null) ? element.GetCreationDate().ToString() : string.Empty;

            switch (Metodo)
            {
                #region TBO_METODO_MAINTENANCE_INCIDENTS_CI_CREATE
                case Comun.TBO.Metodo.TBO_METODO_MAINTENANCE_INCIDENTS_CI_CREATE:
                    
                    result += new JavaScriptSerializer().Serialize(element);

                    break;
                #endregion

                #region TBO_METODO_MAINTENANCE_INCIDENTS_SI_ACCEPT
                case Comun.TBO.Metodo.TBO_METODO_MAINTENANCE_INCIDENTS_SI_ACCEPT:
                #endregion
                default:
                    break;
            }

            return result;
        }
        #endregion

        #region GetOutputParameter
        private string GetOutputParameter(TBOResponse response)
        {
            string result = new JavaScriptSerializer().Serialize(response);
            

            return result;
        }
        #endregion

        #endregion



    }
}
