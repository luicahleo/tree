using CapaNegocio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Caching;
using Microsoft.Extensions.Configuration;
using TreeCore.APIClient;

namespace TreeCore
{
    public class Global : System.Web.HttpApplication
    {
        #region GESTION DE SESIONES

        public static IConfiguration Configuration { get; private set; }
        protected void Application_Start(object sender, EventArgs e)
        {
            //  Se desencadena al iniciar la aplicación

            //Log4Net configure
            log4net.Config.XmlConfigurator.Configure();

            DirectoryMapping.ChangeLogFileName("InfoAp", DirectoryMapping.GetWebLog4NetDirectoryINFO());
            DirectoryMapping.ChangeLogFileName("ErrorAp", DirectoryMapping.GetWebLog4NetDirectoryERROR());

            //Registro de rutas virtuales
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var sharedFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "TreeCore.Shared.Utilities");

            Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("MenuSetting.json", optional: false, reloadOnChange: true)
            .AddJsonFile("SharedSetting.json", optional: false, reloadOnChange: true)
            .Build();

            APIObjects.rutaAPI = Configuration.GetSection("APIRoute").Value;

            //Copiar templates a carpeta externa de recursos
            DirectoryMapping.CopyTemplates();
        }

        /// <summary>
        /// Aqui es donde Se crea la variable de sesion y se le asigna el SesionID
        /// Esto se hara uso de esta variable en login y en basePage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["SESIONID"] = Session.SessionID;
        }

        private object session(string p)
        {
            throw new NotImplementedException();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Se desencadena al comienzo de cada solicitud
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //Se desencadena al intentar autenticar el uso
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Se desencadena cuando se produce un error
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Se desencadena cuando finaliza la sesión
            try
            {
                Data.Usuarios usuario = (Data.Usuarios)Session["USUARIO"];
                if (usuario != null)
                {
                    UsuariosAccesosController cUsuariosAccesos = new UsuariosAccesosController();
                    cUsuariosAccesos.limpiarSesion(usuario.UsuarioID);
                }
            }
            catch (Exception)
            {
                Session.RemoveAll();
                Session.Abandon();
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                Data.Usuarios usuario = (Data.Usuarios)Session["USUARIO"];
                if (usuario != null)
                {
                    UsuariosAccesosController cUsuariosAccesos = new UsuariosAccesosController();
                    Data.UsuariosAccesos acceso = cUsuariosAccesos.GetItem("UsuarioID =" + usuario.UsuarioID);
                    acceso.IP = "";
                    acceso.SesionID = "";
                    cUsuariosAccesos.UpdateItem(acceso);
                }
            }
            catch (Exception)
            {
                Session.RemoveAll();
                Session.Abandon();
            }

        }

        #endregion


    }
}