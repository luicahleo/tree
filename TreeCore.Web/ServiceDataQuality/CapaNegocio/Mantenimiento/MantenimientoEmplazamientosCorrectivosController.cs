using System;
using System.Linq;
using System.Transactions;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoEmplazamientosCorrectivosController : GeneralBaseController<MantenimientoEmplazamientoCorrectivo, TreeCoreContext>
    {
        public MantenimientoEmplazamientosCorrectivosController()
            : base()
        { }

        public ResponseCreateController CrearElemento(long EmplazamientoID, long proyectoID, long conflictividadID, long incidenciaID, long agenciaID,
            long criticidadID, double? latitud, double? longitud, long usuarioID, long tipologiaID, MantenimientoIncidencias incidencia)
        {


            MantenimientoEmplazamientoCorrectivo obj;
            ResponseCreateController result;
            InfoResponse response;

            MantenimientoEmplazamientosCorrectivosEstadosController cMantenimientoEmplazamientosCorrectivosEstados = new MantenimientoEmplazamientosCorrectivosEstadosController();
            cMantenimientoEmplazamientosCorrectivosEstados.SetDataContext(this.Context);
            MantenimientoEmplazamientosCorrectivosSeguimientosController cMantenimientoEmplazamientosCorrectivosSeguimientos = new MantenimientoEmplazamientosCorrectivosSeguimientosController();
            cMantenimientoEmplazamientosCorrectivosSeguimientos.SetDataContext(this.Context);
            MantenimientoIncidenciasController cMantenimientoIncidencias = new MantenimientoIncidenciasController();
            cMantenimientoIncidencias.SetDataContext(this.Context);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    incidencia = cMantenimientoIncidencias.GetItem(incidencia.MantenimientoIncidenciaID);

                    MantenimientoEmplazamientoCorrectivo mantenimientoEmplazamientoCorrectivo = new MantenimientoEmplazamientoCorrectivo
                    {
                        EmplazamientoID = EmplazamientoID,
                        MantenimientoEmplazamientoZonaID = null,
                        ProyectoID = proyectoID,
                        MantenimientoEmplazamientoConflictividadID = conflictividadID,
                        MantenimientoFechaPlanificacion = null,
                        MantenimientoFechaValidacion = null,
                        MantenimientoFechaAprobacionPlanificacion = null,
                        MantenimientoCorrectivoAsignadoID = null,
                        MantenimientoIncidenciaID = incidenciaID,
                        CostoCorrectivo = null,
                        FechaEjecucionPrevista = null,
                        MantenimientoEmplazamientoAgenciaID = agenciaID,
                        Finalizado = false,
                        MantenimientoCriticidadID = criticidadID,
                        Latitud = latitud,
                        Longitud = longitud,
                        Activo = true
                    };

                    mantenimientoEmplazamientoCorrectivo = this.AddItem(mantenimientoEmplazamientoCorrectivo);

                    if (mantenimientoEmplazamientoCorrectivo != null)
                    {
                        MantenimientoEmplazamientosCorrectivosEstados estado;
                        estado = cMantenimientoEmplazamientosCorrectivosEstados.EstadoCorrectivoPorDefectoByTipologia(tipologiaID);
                        MantenimientoEmplazamientosCorrectivosSeguimientos seguimiento = new MantenimientoEmplazamientosCorrectivosSeguimientos
                        {
                            Activo = true,
                            Cambios = "",
                            Fecha = DateTime.Now,
                            MantenimientoEmplazamientoCorrectivoEstadoID = estado.MantenimientoEmplazamientoCorrectivoEstadoID,
                            MantenimientoEmplazamientoCorrectivoEstadoInicialID = estado.MantenimientoEmplazamientoCorrectivoEstadoID,
                            MantenimientoEmplazamientoCorrectivoID = mantenimientoEmplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoID,
                            Nota = estado.MantenimientoEmplazamientoCorrectivoEstado_esES,
                            UsuarioID = usuarioID
                        };

                        seguimiento = cMantenimientoEmplazamientosCorrectivosSeguimientos.AddItem(seguimiento);

                        if (seguimiento != null)
                        {
                            mantenimientoEmplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoSeguimientoID = seguimiento.MantenimientoEmplazamientoCorrectivoSeguimientoID;

                            this.UpdateItem(mantenimientoEmplazamientoCorrectivo);

                            #region SLA
                            MantenimientoCorrectivosEstadosSLAController mantenimientoCorrectivosEstadosSLA = new MantenimientoCorrectivosEstadosSLAController();
                            mantenimientoCorrectivosEstadosSLA.SetDataContext(this.Context);
                            MantenimientoCorrectivosEstadosSLA estadoSLA;
                            estadoSLA = mantenimientoCorrectivosEstadosSLA.GetDefault();

                            MantenimientoCorrectivosEmplazamientosEstadosSLAController cMantenimientoCorrectivosEmplazamientosEstadosSLA = new MantenimientoCorrectivosEmplazamientosEstadosSLAController();
                            cMantenimientoCorrectivosEmplazamientosEstadosSLA.SetDataContext(this.Context);
                            MantenimientoCorrectivosEmplazamientosEstadosSLA mantenimientoCorrectivosEmplazamientosEstadosSLA = new MantenimientoCorrectivosEmplazamientosEstadosSLA()
                            {
                                Activo = true,
                                Fecha = DateTime.Now,
                                EstadoDias = 0,
                                MantenimientoCorrectivoEstadoSLAID = estadoSLA.MantenimientoCorrectivoEstadoSLAID,
                                MantenimientoCorrectivoEmplazamientoEstadoID = seguimiento.MantenimientoEmplazamientoCorrectivoEstadoID,
                                MantenimientoCorrectivoEmplazamientoID = mantenimientoEmplazamientoCorrectivo.MantenimientoEmplazamientoCorrectivoID,
                                MotivoParada = ""
                            };

                            cMantenimientoCorrectivosEmplazamientosEstadosSLA.AddItem(mantenimientoCorrectivosEmplazamientosEstadosSLA);

                            #endregion

                            incidencia = cMantenimientoIncidencias.GetItem(incidencia.MantenimientoIncidenciaID);
                            incidencia.EnTramite = true;
                            incidencia.Cancelada = false;


                            cMantenimientoIncidencias.UpdateItem(incidencia);

                            obj = mantenimientoEmplazamientoCorrectivo;

                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.GENERIC.COD_TBO_SUCCESS_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_SUCCESS_DESCRIPTION,
                                Data = obj
                            };
                            result = new ResponseCreateController(response, obj);
                        }
                        else
                        {
                            obj = null;
                            response = new InfoResponse
                            {
                                Result = true,
                                Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
                                Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
                            };
                            result = new ResponseCreateController(response, obj);
                        }
                    }
                    else
                    {
                        obj = null;
                        response = new InfoResponse
                        {
                            Result = true,
                            Code = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_CODE,
                            Description = ServicesCodes.GENERIC.COD_TBO_OBJECT_NOT_CREATION_DESCRIPTION
                        };
                        result = new ResponseCreateController(response, obj);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                obj = null;
                response = new InfoResponse
                {
                    Result = false,
                    Code = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_CODE,
                    Description = ServicesCodes.GENERIC.COD_TBO_EXCEPTION_DESCRIPTION + " " + ex.Message
                };
                result = new ResponseCreateController(response, obj);
            }
            return result;
        }

        public MantenimientoEmplazamientoCorrectivo GetByIncidenciaID(long MantenimientoIncidenciaID)
        {
            MantenimientoEmplazamientoCorrectivo result;
            try
            {
                result = (from c
                          in Context.MantenimientoEmplazamientoCorrectivo
                          where c.MantenimientoIncidenciaID == MantenimientoIncidenciaID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }
            return result;
        }
    }
}