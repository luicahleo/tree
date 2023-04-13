using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreEstadosNotificacionesUsuariosController : GeneralBaseController<CoreEstadosNotificacionesUsuarios, TreeCoreContext>, IGestionBasica<CoreEstadosNotificacionesUsuarios>
    {
        public CoreEstadosNotificacionesUsuariosController()
               : base()
        { }

        public InfoResponse Add(CoreEstadosNotificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oUser);
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

        public InfoResponse Update(CoreEstadosNotificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oUser);
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

        public InfoResponse Delete(CoreEstadosNotificacionesUsuarios oUser)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oUser);
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

        public CoreEstadosNotificacionesUsuarios getNotificacionUsuarios(long lNotificacionID, long lUsuarioID)
        {
            CoreEstadosNotificacionesUsuarios oDato;

            try
            {
                oDato = (from c in Context.CoreEstadosNotificacionesUsuarios where c.UsuarioID == lUsuarioID && c.CoreEstadoNotificacionID == lNotificacionID select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public List<long> getUserByNotificacionID(long lNotificacionID)
        {
            List<long> listaUsuarioID;

            try
            {
                listaUsuarioID = (from c in Context.CoreEstadosNotificacionesUsuarios where c.CoreEstadoNotificacionID == lNotificacionID select c.UsuarioID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaUsuarioID = null;
            }

            return listaUsuarioID;
        }
    }
}