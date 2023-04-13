using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace TreeAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

            //Log4Net configure
            log4net.Config.XmlConfigurator.Configure();

            TreeCore.DirectoryMapping.ChangeLogFileName("InfoAp", TreeCore.DirectoryMapping.GetAPILog4NetDirectoryINFO());
            TreeCore.DirectoryMapping.ChangeLogFileName("ErrorAp", TreeCore.DirectoryMapping.GetAPILog4NetDirectoryERROR());

            GlobalConfiguration.Configure(WebApiConfig.Register);

            
        }
    }
}