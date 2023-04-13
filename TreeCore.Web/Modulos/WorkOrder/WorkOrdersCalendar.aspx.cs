using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;

namespace TreeCore.ModWorkOrders
{
    public partial class WorkOrdersCalendar : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}