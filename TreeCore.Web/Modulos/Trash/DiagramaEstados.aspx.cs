using CapaNegocio;
using Ext.Net;
using Ext.Net.Utilities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Web.UI.HtmlControls;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Reflection;
using Newtonsoft.Json;

namespace TreeCore.PaginasComunes
{
    public partial class DiagramaEstados : TreeCore.Page.BasePageExtNet
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static long lProyectoTipoID;
        public static long lID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (Request.Params["ProyectoTipoID"] != null)
                {
                    lProyectoTipoID = long.Parse(Request.Params["ProyectoTipoID"]);
                }

                if (Request.Params["EstadoID"] != null)
                {
                    lID = long.Parse(Request.Params["EstadoID"]);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }
      
        }

    }
}