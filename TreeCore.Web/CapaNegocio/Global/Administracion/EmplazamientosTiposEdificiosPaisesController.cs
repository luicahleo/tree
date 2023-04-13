using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public sealed class EmplazamientosTiposEdificiosPaisesController : GeneralBaseController<EmplazamientosTiposEdificiosPaises, TreeCoreContext>, IGestionBasica<EmplazamientosTiposEdificiosPaises>
    {
        public EmplazamientosTiposEdificiosPaisesController()
            : base()
        { }

        public InfoResponse Add(EmplazamientosTiposEdificiosPaises oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = AddEntity(oEntidad);
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

        public InfoResponse Update(EmplazamientosTiposEdificiosPaises oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = UpdateEntity(oEntidad);
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

        public InfoResponse Delete(EmplazamientosTiposEdificiosPaises oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {
                oResponse = DeleteEntity(oEntidad);
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

        public InfoResponse ModificarActivar(EmplazamientosTiposEdificiosPaises oEntidad)
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


        public InfoResponse SetDefecto(EmplazamientosTiposEdificiosPaises oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            EmplazamientosTiposEdificiosPaises oDefault;
            try
            {
                oDefault = GetDefault((long)oEntidad.ClienteID);
                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDefault != null)
                {
                    if (oDefault.EmplazamientoTipoEdificioPaisID != oEntidad.EmplazamientoTipoEdificioPaisID)
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

        public EmplazamientosTiposEdificiosPaises GetDefault(long ClienteID)
        {
            EmplazamientosTiposEdificiosPaises oTipo;
            try
            {
                oTipo = (from c
                         in Context.EmplazamientosTiposEdificiosPaises
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

        public List<Vw_EmplazamientosTiposEdificiosPaises> GetTipoEdificioID(long lTipo)
        {
            List<Vw_EmplazamientosTiposEdificiosPaises> listaDatos;

            listaDatos = (from c in Context.Vw_EmplazamientosTiposEdificiosPaises where c.EmplazamientoTipoEdificioID == lTipo orderby c.Pais select c).ToList();

            return listaDatos;
        }

    }
}