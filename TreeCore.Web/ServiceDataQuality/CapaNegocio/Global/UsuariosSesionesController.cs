using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Ext.Net;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using CapaNegocio;
using System;
using TreeCore;
using Tree;
using System.Data.Linq;

namespace CapaNegocio
{
    public sealed class UsuariosSesionesController : GeneralBaseController<UsuariosSesiones, TreeCoreContext>
    {
        public UsuariosSesionesController()
            : base()
        {

        }

        public UsuariosSesiones GetSesion(string VexSesionGUID)
        {

#if SERVICESETTINGS
            int SessionTimeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TiempoSesionMinutos"]);
#elif TREEAPI
            int SessionTimeout = TreeAPI.Properties.Settings.Default.TiempoSesionMinutos;
#else
            int SessionTimeout = TreeCore.Properties.Settings.Default.TiempoSesionMinutos;
#endif

            UsuariosSesiones sesion = (from c in Context.UsuariosSesiones
                                        where c.SesionGUID == VexSesionGUID /*&&
                                        c.UltimaSolicitud != null && c.UltimaSolicitud.AddMinutes(SessionTimeout) > DateTime.Now*/
                                        select c).FirstOrDefault();

            /*if (sesion != null && sesion.UltimaSolicitud != null)
            {
                Context.Refresh(RefreshMode.KeepChanges, sesion);
                sesion.UltimaSolicitud = DateTime.Now;
                try
                {
                    //this.UpdateItem(sesion);
                }
                catch (ChangeConflictException ex)
                {
                    Comun.Log("UsuariosSesionesController -> GetSesion : " + ex.Message);
                    foreach (ObjectChangeConflict occ in Context.ChangeConflicts)
                    {
                        occ.Resolve(RefreshMode.KeepChanges);
                    }
                }
            }*/

            return sesion;
        }

        public UsuariosSesiones GetUtimaSesion(long usuarioid)
        {
            UsuariosSesiones sesion = new UsuariosSesiones();

            List<UsuariosSesiones> lUserSesion = new List<UsuariosSesiones>();

            lUserSesion = (from p in Context.UsuariosSesiones where p.UsuarioID == usuarioid select p).OrderByDescending(p => p.UltimaSolicitud).ToList();

            sesion = lUserSesion.First();

            return sesion;
        }

    }
}