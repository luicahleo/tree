using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using TreeCore.Page;

namespace TreeCore.Modulos.Monitoring
{
    public partial class MonitoringInicio : BasePageExtNet
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                List<string> pathsOfScripts = new List<string>();
                pathsOfScripts.Add(Comun.BundleConfigPaths.CONTENT_JS_CHART);

                ResourceManagerOperaciones(ResourceManagerTreeCore, pathsOfScripts, new List<string>());

            }
        }
    }
}