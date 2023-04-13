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
    public class EmplazamientosTiposEdificiosController : GeneralBaseController<EmplazamientosTiposEdificios, TreeCoreContext>, IGestionBasica<EmplazamientosTiposEdificios>
    {
        public EmplazamientosTiposEdificiosController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTiposEdificios oEntidad)
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

        public InfoResponse Update(EmplazamientosTiposEdificios oEntidad)
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

        public InfoResponse Delete(EmplazamientosTiposEdificios oEntidad)
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

        public InfoResponse ModificarActivar(EmplazamientosTiposEdificios oEntidad)
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

        public InfoResponse SetDefecto(EmplazamientosTiposEdificios oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosTiposEdificios oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoTipoEdificioID != oEntidad.EmplazamientoTipoEdificioID)
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

        public bool RegistroDuplicado(EmplazamientosTiposEdificios oEntidad)
        {
            bool isExiste = false;
            List<EmplazamientosTiposEdificios> datos;

            datos = (from c in Context.EmplazamientosTiposEdificios where (c.TipoEdificio == oEntidad.TipoEdificio && c.ClienteID == oEntidad.ClienteID && c.EmplazamientoTipoEdificioID != oEntidad.EmplazamientoTipoEdificioID) select c).ToList<EmplazamientosTiposEdificios>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public EmplazamientosTiposEdificios GetDefault(long ClienteID)
        {
            EmplazamientosTiposEdificios oTipo;
            try
            {
                oTipo = (from c
                         in Context.EmplazamientosTiposEdificios
                         where c.Defecto &&
                                c.ClienteID == ClienteID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipo = null;
            }
            return oTipo;
        }

        public bool RegistroVinculado(long EmplazamientoTipoEdificioID)
        {
            bool existe = true;


            return existe;
        }

        public bool HasActiveTipoEdificio(string tipoEdificio, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.EmplazamientosTiposEdificios
                          where c.Activo &&
                                  c.TipoEdificio == tipoEdificio &&
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

        public EmplazamientosTiposEdificios GetActivoTipoEdificio(string tipoEdificio, long clienteID)
        {
            EmplazamientosTiposEdificios result;

            try
            {
                result = (from c in Context.EmplazamientosTiposEdificios
                          where c.Activo &&
                                  c.TipoEdificio == tipoEdificio &&
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

        public long GetTipoEdificioByNombreAll(string Nombre)
        {

            long tipoedificioID = 0;
            try
            {

                tipoedificioID = (from c in Context.EmplazamientosTiposEdificios where c.TipoEdificio.Equals(Nombre) select c.EmplazamientoTipoEdificioID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                tipoedificioID = -1;

            }
            return tipoedificioID;
        }

        public List<EmplazamientosTiposEdificios> GetActivos(long lClienteID)
        {
            List<EmplazamientosTiposEdificios> listaTipos;

            try
            {
                listaTipos = (from c
                              in Context.EmplazamientosTiposEdificios
                              where c.Activo &&
                                    c.ClienteID == lClienteID
                              orderby c.TipoEdificio
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaTipos = null;
            }

            return listaTipos;
        }

        public List<string> GetEdificiosNombre(long ClienteID)
        {
            List<string> lista;

            try
            {
                lista = (from c
                         in Context.EmplazamientosTiposEdificios
                         where c.Activo && c.ClienteID == ClienteID
                         orderby c.TipoEdificio
                         select c.TipoEdificio).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }

            return lista;
        }

        public long GetTipoEdificioByNombreAndTipoEmplazamiento(long EmplazamientoTipoID, string Nombre)
        {
            long tipoedificioID = 0;
            try
            {
                tipoedificioID = (from c in Context.EmplazamientosTiposEdificios where c.TipoEdificio.Equals(Nombre) select c.EmplazamientoTipoEdificioID).First();

                //if (tipoedificioID != 0)
                //{
                //    tipoedificioID = (from c in Context.EmplazamientosTiposEmplazamientosTiposEdificios where c.EmplazamientoTipoEdificioID == tipoedificioID && c.EmplazamientoTipoID == EmplazamientoTipoID select c.EmplazamientoTipoEdificioID).First();
                //}

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                tipoedificioID = -1;
            }

            return tipoedificioID;
        }

    }
}