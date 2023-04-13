using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class EstadosGlobalesController : GeneralBaseController<EstadosGlobales, TreeCoreContext>, IGestionBasica<EstadosGlobales>
    {
        public EstadosGlobalesController()
            : base()
        { }

        public InfoResponse Add(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oEntidad);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (RegistroDuplicado(oEntidad))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oEntidad);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                if (oEntidad.Defecto)
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.jsPorDefecto,
                        Data = null
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEntidad);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse ModificarActivar(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oEntidad.Activo = !oEntidad.Activo;
                oResponse = Update(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse SetDefecto(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EstadosGlobales oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EstadoGlobalID != oEntidad.EstadoGlobalID)
                    {
                        if (oDefault.Defecto)
                        {
                            oDefault.Defecto = false;
                            oResponse = Update(oDefault);
                        }
                        oEntidad.Defecto = true;
                        oEntidad.Activo = true;
                        oResponse = Update(oEntidad);
                    }
                    else
                    {
                        oResponse = new InfoResponse
                        {
                            Result = true,
                            Code = "",
                            Description = "",
                            Data = oEntidad
                        };
                    }

                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oEntidad.Defecto = true;
                    oEntidad.Activo = true;
                    oResponse = Update(oEntidad);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public bool RegistroDuplicado(EstadosGlobales oEntidad)
        {
            bool isExiste = false;
            List<EstadosGlobales> datos;

            datos = (from c in Context.EstadosGlobales where (c.EstadoGlobal == oEntidad.EstadoGlobal && c.ClienteID == oEntidad.ClienteID && c.EstadoGlobalID != oEntidad.EstadoGlobalID) select c).ToList<EstadosGlobales>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public EstadosGlobales GetDefault(long ClienteID)
        {
            EstadosGlobales oEntidad;
            try
            {
                oEntidad = (from c
                         in Context.EstadosGlobales
                            where c.Defecto &&
                                   c.ClienteID == ClienteID
                            select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEntidad = null;
            }
            return oEntidad;
        }

        public InfoResponse AddEstado(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = ComprobarEstado(oEntidad);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
                oResponse = Add(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse UpdateEstado(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = ComprobarEstado(oEntidad);
                if (!oResponse.Result)
                {
                    return oResponse;
                }
                oResponse = Update(oEntidad);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse ComprobarEstado(EstadosGlobales oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                EstadosGlobales estado = GetDesactivo((long)oEntidad.ClienteID);
                EstadosGlobales estadoDesinstalado = GetDesinstalado((long)oEntidad.ClienteID);
                if (oEntidad.Desactivo)
                {
                    if (estado != null)
                    {
                        if (estado.Desactivo)
                        {
                            if (long.Parse(estado.EstadoGlobalID.ToString()) != oEntidad.EstadoGlobalID)
                            {
                                estado.Desactivo = !estado.Desactivo;
                                oResponse = Update(estado);

                                if (!oResponse.Result)
                                {
                                    return oResponse;
                                }
                            }
                        }

                    }
                }

                if (oEntidad.Desinstalado)
                {
                    if (estadoDesinstalado != null)
                    {
                        if (estadoDesinstalado.Desinstalado)
                        {
                            if (long.Parse(estadoDesinstalado.EstadoGlobalID.ToString()) != oEntidad.EstadoGlobalID)
                            {
                                estadoDesinstalado.Desinstalado = !estadoDesinstalado.Desinstalado;
                                oResponse = Update(estadoDesinstalado);

                                if (!oResponse.Result)
                                {
                                    return oResponse;
                                }
                            }
                        }

                    }
                }

                oResponse = new InfoResponse
                {
                    Result = true
                };
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public bool RegistroVinculado(long EstadoGlobalID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveEstadoGlobal(string estadoGlobal, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EstadosGlobales
                          where c.Activo == true &&
                                c.EstadoGlobal == estadoGlobal &&
                                c.ClienteID == clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public EstadosGlobales GetActivoEstadoGlobal(string estadoGlobal, long clienteID)
        {
            EstadosGlobales result;

            try
            {
                result = (from c in Context.EstadosGlobales
                          where c.Activo == true &&
                                c.EstadoGlobal == estadoGlobal &&
                                c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public long GetGlobalID(string sNombre)
        {
            long lDato = new long();

            try
            {
                lDato = (from c in Context.EstadosGlobales where c.EstadoGlobal == sNombre select c.EstadoGlobalID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lDato = new long();
            }

            return lDato;
        }

        public List<EstadosGlobales> GetEstadosGlobalesActivos(long ClienteID)
        {
            List<EstadosGlobales> lista = null;

            try
            {
                lista = (from c in Context.EstadosGlobales where c.ClienteID == ClienteID && c.Activo == true orderby c.EstadoGlobal select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EstadosGlobales>();
            }

            return lista;
        }

        public List<EstadosGlobales> GetAllEstados(bool bActivo)
        {
            // Local variables
            List<EstadosGlobales> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.EstadosGlobales where c.Activo == true select c).ToList<EstadosGlobales>();
                }
                else
                {
                    lista = (from c in Context.EstadosGlobales select c).ToList<EstadosGlobales>();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }
            return lista;
        }

        public EstadosGlobales GetDesactivo(long clienteID)
        {
            EstadosGlobales oEstadosGlobales;
            try
            {
                oEstadosGlobales = (from c in Context.EstadosGlobales where c.Desactivo && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstadosGlobales = null;
            }
            return oEstadosGlobales;
        }

        public EstadosGlobales GetDesinstalado(long clienteID)
        {
            EstadosGlobales oEstadosGlobales;
            try
            {
                oEstadosGlobales = (from c in Context.EstadosGlobales where c.Desinstalado && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEstadosGlobales = null;
            }
            return oEstadosGlobales;
        }

        public List<EstadosGlobales> GetActivos(long clienteID)
        {
            List<EstadosGlobales> lista;

            try
            {
                lista = (from c
                         in Context.EstadosGlobales
                         where c.Activo == true &&
                                c.ClienteID == clienteID
                         orderby c.EstadoGlobal
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetEstadosGlobalesNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EstadosGlobales
                         where c.Activo == true && c.ClienteID == ClienteID
                         orderby c.EstadoGlobal
                         select c.EstadoGlobal).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetEstadoGlobalByNombre(string Nombre)
        {

            long estGLobalID = 0;
            try
            {

                estGLobalID = (from c in Context.EstadosGlobales where c.EstadoGlobal.Equals(Nombre) select c.EstadoGlobalID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                estGLobalID = -1;

            }
            return estGLobalID;
        }

        /* public List<EstadosGlobales> GetActivosLibres(long lEstadoID)
         {
             List<EstadosGlobales> listaDatos;
             List<long?> listaIDs;

             try
             {
                // listaIDs = (from c in Context.CoreEstadosGlobales where c.CoreEstadoID == lEstadoID select c.EstadoGlobalID).ToList();
                 //listaDatos = (from c in Context.EstadosGlobales where c.Activo == true && !listaIDs.Contains(c.EstadoGlobalID) select c).ToList();
             }
             catch (Exception ex)
             {
                 log.Error(ex.Message);
                 listaDatos = null;
             }

             return listaDatos;
         }
         */

        public string getNombreByID(long lEstadoGlobalID)
        {
            string sNombre;

            try
            {
                sNombre = (from c in Context.EstadosGlobales where c.EstadoGlobalID == lEstadoGlobalID select c.EstadoGlobal).First();
            }
            catch (Exception)
            {
                sNombre = null;
            }

            return sNombre;
        }
    }
}