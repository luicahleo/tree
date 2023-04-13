using System;
using System.Collections.Generic;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MantenimientoIncidenciasController : GeneralBaseController<MantenimientoIncidencias, TreeCoreContext>, IBasica<MantenimientoIncidencias>
    {
        public MantenimientoIncidenciasController()
            : base()
        { }

        public ResponseCreateController CrearElemento(
            string mantenimientoIncidencia, bool emergencia, Usuarios usuario, long emplazamientoID, bool EnTramite,  
            string MantenimientoDescripcionIncidencia, MantenimientoIncidenciasTipos tipo, InventarioElementos elementoInventario)
        {
            MantenimientoIncidencias obj;
            ResponseCreateController result;
            InfoResponse response;

            try
            {
                if (!ExisteDuplicado(mantenimientoIncidencia))
                {

                    obj = new MantenimientoIncidencias() { 
                        MantenimientoIncidencia= mantenimientoIncidencia,
                        Emergencia=emergencia,
                        UsuarioID = usuario.UsuarioID,
                        EmplazamientoID = emplazamientoID,
                        EnTramite = EnTramite,
                        Fecha = DateTime.Now,
                        MantenimientoDescripcionIncidencia = MantenimientoDescripcionIncidencia
                    };

                    if (tipo != null)
                    {
                        obj.MantenimientoIncidenciaTipoID = tipo.MantenimientoIncidenciaTipoID;
                    }

                    if (elementoInventario != null)
                    {
                        obj.InventarioElementoID = elementoInventario.InventarioElementoID;
                    }


                    #region VALORES POR DEFECTO
                    if (obj != null)
                    {
                        obj.Cancelada = false;
                    }
                    #endregion


                    obj = this.AddItem(obj);

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
                    response = new InfoResponse
                    {
                        Result = false,
                        Code = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_NAME_INCIDENCE_EXISTS_CODE,
                        Description = ServicesCodes.MAINTENANCE_INCIDENT.COD_TBO_NAME_INCIDENCE_EXISTS_DESCRIPTION
                    };
                    result = new ResponseCreateController(response, null);
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

        public bool RegistroVinculado(long MantenimientoIncidenciaID)
        {
            bool bExiste = true;


            return bExiste;
        }

        public bool RegistroDuplicado(string MantenimientoIncidencia, long lClienteID)
        {
            bool bExiste = false;
            List<Vw_MantenimientoIncidencias> listaDatos;


            listaDatos = (from c 
                          in Context.Vw_MantenimientoIncidencias 
                          where (c.MantenimientoIncidencia == MantenimientoIncidencia && 
                          c.ClienteID == lClienteID) select c).ToList<Vw_MantenimientoIncidencias>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDefecto(long MantenimientoIncidenciaID)
        {
            MantenimientoIncidencias oDato;
            MantenimientoIncidenciasController cController = new MantenimientoIncidenciasController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && MantenimientoIncidenciaID == " + MantenimientoIncidenciaID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public string FiltrarIncidencias(string estado)
        {
            string query = "";

            if (estado == "PendienteAnalisis") { query = "EnTramite == false && Cancelada == false "; }
            if (estado == "Analizadas") { query = "EnTramite == true && Cancelada == false "; }
            if (estado == "Canceladas") { query = "EnTramite == true && Cancelada == true "; }
            if (estado == "Finalizadas") { query = "EnTramite == false && Cancelada == true "; }

            return query;
        }

        public bool ExisteDuplicado(string MantenimientoIncidencia)
        {
            MantenimientoIncidencias incidencia;
            bool response = false;

            try
            {
                incidencia = (from c 
                              in Context.MantenimientoIncidencias 
                              where c.MantenimientoIncidencia.Equals(MantenimientoIncidencia) 
                              select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                incidencia = null;
            }

            if (incidencia != null)
            {
                response = true;
            }

            return response;
        }
    }
}