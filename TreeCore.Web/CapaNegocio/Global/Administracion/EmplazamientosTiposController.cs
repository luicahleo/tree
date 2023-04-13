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
    public class EmplazamientosTiposController : GeneralBaseController<EmplazamientosTipos, TreeCoreContext>, IGestionBasica<EmplazamientosTipos>
    {
        public EmplazamientosTiposController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTipos oEntidad)
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

        public InfoResponse Update(EmplazamientosTipos oEntidad)
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

        public InfoResponse Delete(EmplazamientosTipos oEntidad)
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

        public InfoResponse ModificarActivar(EmplazamientosTipos oEntidad)
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

        public InfoResponse SetDefecto(EmplazamientosTipos oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosTipos oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoTipoID != oEntidad.EmplazamientoTipoID)
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

        public bool RegistroDuplicado(EmplazamientosTipos oEntidad)
        {
            bool isExiste = false;
            List<EmplazamientosTipos> datos;

            datos = (from c in Context.EmplazamientosTipos where (c.Tipo == oEntidad.Tipo && c.ClienteID == oEntidad.ClienteID && c.EmplazamientoTipoID != oEntidad.EmplazamientoTipoID) select c).ToList<EmplazamientosTipos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public EmplazamientosTipos GetDefault(long ClienteID)
        {
            EmplazamientosTipos oEmplazamientosTipos;
            try
            {
                oEmplazamientosTipos = (from c
                         in Context.EmplazamientosTipos
                                        where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oEmplazamientosTipos = null;
            }
            return oEmplazamientosTipos;
        }

        public List<EmplazamientosTipos> GetEmplazamientosTiposActivos(long ClienteID)
        {
            List<EmplazamientosTipos> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosTipos where c.Activo && c.ClienteID == ClienteID orderby c.Tipo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = new List<EmplazamientosTipos>();
            }

            return lista;
        }
        public bool RegistroVinculado(long EmplazamientoTipoID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveEmplazamientoTipo(string tipo, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTipos
                          where c.Activo && 
                                c.Tipo==tipo && 
                                c.ClienteID==clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }

        public EmplazamientosTipos GetActivoEmplazamientoTipo(string tipo, long clienteID)
        {
            EmplazamientosTipos result;

            try
            {
                result = (from c in Context.EmplazamientosTipos
                          where c.Activo &&
                                c.Tipo == tipo &&
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

        public List<EmplazamientosTipos> GetActivos(long clienteID)
        {
            List<EmplazamientosTipos> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTipos
                         where c.Activo && 
                                c.ClienteID == clienteID
                         orderby c.Tipo
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public List<string> GetTiposNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTipos
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.Tipo
                         select c.Tipo).ToList();
            }
            catch (Exception)
            {
                lista = null;
            }

            return lista;
        }

        public long GetTipoByNombreAll(string Nombre)
        {

            long tipoID = 0;
            try
            {

                tipoID = (from c in Context.EmplazamientosTipos where c.Tipo.Equals(Nombre) select c.EmplazamientoTipoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoID = -1;

            }
            return tipoID;
        }

    }
}