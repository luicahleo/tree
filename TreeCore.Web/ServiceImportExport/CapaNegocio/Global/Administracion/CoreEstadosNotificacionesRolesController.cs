using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreEstadosNotificacionesRolesController : GeneralBaseController<CoreEstadosNotificacionesRoles, TreeCoreContext>
    {
        public CoreEstadosNotificacionesRolesController()
               : base()
        { }

        public CoreEstadosNotificacionesRoles getNotificacionRoles(long lNotificacion, long lRolID)
        {
            CoreEstadosNotificacionesRoles oDato;

            try
            {
                oDato = (from c in Context.CoreEstadosNotificacionesRoles where c.RolID == lRolID && c.CoreEstadoNotificacionID == lNotificacion select c).First();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public List<long> getRolByNotificacionID(long lNotificacionID)
        {
            List<long> listaRolID;

            try
            {
                listaRolID = (from c in Context.CoreEstadosNotificacionesRoles where c.CoreEstadoNotificacionID == lNotificacionID select c.RolID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaRolID = null;
            }

            return listaRolID;
        }

    }
}