using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CoreEstadosNotificacionesController : GeneralBaseController<CoreEstadosNotificaciones, TreeCoreContext>
    {
        public CoreEstadosNotificacionesController()
               : base()
        { }

        public List<CoreEstadosNotificaciones> getNotificacionesByEstado (long lEstadoID)
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
    }
}