using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class CoreEstadosNotificacionesController : GeneralBaseController<CoreEstadosNotificaciones, TreeCoreContext>, IGestionBasica<CoreEstadosNotificaciones>
    {
        public CoreEstadosNotificacionesController()
               : base()
        { }

        public InfoResponse Add(CoreEstadosNotificaciones oNotificacion)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = AddEntity(oNotificacion);
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

        public InfoResponse Update(CoreEstadosNotificaciones oNotificacion)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oNotificacion);
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

        public InfoResponse Delete(CoreEstadosNotificaciones oNotificacion)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oNotificacion);
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

        public InfoResponse AddEstadoNotificacion(CoreEstadosNotificaciones oDato, List<string> listaRolesIDs, List<string> listaUsuariosIDs)
        {
            InfoResponse oResponse;
            CoreEstadosNotificacionesRoles oRol;            
            CoreEstadosNotificacionesUsuarios oUser;

            CoreEstadosNotificacionesRolesController cRoles = new CoreEstadosNotificacionesRolesController();
            CoreEstadosNotificacionesUsuariosController cUsers = new CoreEstadosNotificacionesUsuariosController();
            cRoles.SetDataContext(this.Context);
            cUsers.SetDataContext(this.Context);

            try
            {
                oResponse = Add(oDato);

                foreach (string sValor in listaRolesIDs)
                {
                    oRol = new CoreEstadosNotificacionesRoles();
                    oRol.CoreEstadosNotificaciones = oDato;
                    oRol.RolID = long.Parse(sValor);

                    oResponse = cRoles.Add(oRol);
                }

                foreach (string sUser in listaUsuariosIDs)
                {
                    oUser = new CoreEstadosNotificacionesUsuarios();
                    oUser.CoreEstadosNotificaciones = oDato;
                    oUser.UsuarioID = long.Parse(sUser);

                    oResponse = cUsers.Add(oUser);
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

        public InfoResponse DeleteEstadoNotificacion(CoreEstadosNotificaciones oNotificacion)
        {
            InfoResponse oResponse;

            CoreEstadosNotificacionesRolesController cRoles = new CoreEstadosNotificacionesRolesController();
            CoreEstadosNotificacionesUsuariosController cUsers = new CoreEstadosNotificacionesUsuariosController();
            cRoles.SetDataContext(this.Context);
            cUsers.SetDataContext(this.Context);

            try
            {
                List<long> listaUsuariosIDs = cUsers.getUserByNotificacionID(oNotificacion.CoreEstadoNotificacionID);
                List<long> listaRolesIDs = cRoles.getRolByNotificacionID(oNotificacion.CoreEstadoNotificacionID);

                foreach (long lValor in listaRolesIDs)
                {
                    CoreEstadosNotificacionesRoles oRol = cRoles.getNotificacionRoles(oNotificacion.CoreEstadoNotificacionID, lValor);
                    oResponse = cRoles.Delete(oRol);
                }

                foreach (long lUser in listaUsuariosIDs)
                {
                    CoreEstadosNotificacionesUsuarios oUser = cUsers.getNotificacionUsuarios(oNotificacion.CoreEstadoNotificacionID, lUser);
                    oResponse = cUsers.Delete(oUser);
                }

                oResponse = Delete(oNotificacion);
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

        public List<CoreEstadosNotificaciones> getNotificacionesByEstado(long lEstadoID)
        {
            List<CoreEstadosNotificaciones> listaDatos;

            try
            {
                listaDatos = (from c in Context.CoreEstadosNotificaciones where c.CoreEstadoID == lEstadoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public long getNotificacionIDByEstado(long lEstadoID)
        {
            long lID;

            try
            {
                lID = (from c in Context.CoreEstadosNotificaciones where c.CoreEstadoID == lEstadoID select c.CoreEstadoNotificacionID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lID = 0;
            }

            return lID;
        }

        public bool RegistroDuplicado(long lEstadoID, string sContenido)
        {
            bool isExiste = false;
            List<CoreEstadosNotificaciones> datos = new List<CoreEstadosNotificaciones>();

            datos = (from c in Context.CoreEstadosNotificaciones where (c.CoreEstadoID == lEstadoID && c.Contenido == sContenido) select c).ToList<CoreEstadosNotificaciones>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}