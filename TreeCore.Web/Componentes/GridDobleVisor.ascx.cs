using CapaNegocio;
using Ext.Net;
using System;
using System.Collections.Generic;
using log4net;
using TreeCore.Data;
using System.Reflection;
using TreeCore.Page;

namespace TreeCore.Componentes
{
    public partial class GridDobleVisor : BaseUserControl
    {
   
        #region GESTION PAGINA

        protected void Page_Load(object sender, EventArgs e)
        {

            this.gridMain1.Store.Primary.DataSource = this.DataSamplegrid;
            //this.GridPanel1.Store.Primary.DataSource = this.DataSamplegrid;

        }

        private void Page_Init(object sender, EventArgs e)
        {
       

        }


        private object[] DataSamplegrid
        {
            get
            {
                return new object[]
                {
                new object[] { "", 0.5, "License denied","Licencia_denegada", "", 3, 5, "License denied", "20/20/2020", "20/20/2020", "20/20/2020",  "", "", "", "" },
                new object[] { "",  0.3, "Canceled by client", "Cancelado_cliente", "", 4, 8, "Canceled", "Client", "Blocked", "",  "", "icon", "", "icon" },
                new object[] { "",  0.3, "Provisionally denied…", "Licencia_denegada_prov…", "", 1, 2, "Blocked", "RF", "Blocked", "icon",  "", "", "icon", "" },
                new object[] { "", 0.4, "Eliminated", "Eliminado", "", 4, 7, "Eliminated", "RF",  "Blocked", "",  "", "", "icon", "" },
                new object[] { "icon", 0.1,  "Kick-off", "Carga inicial", "Launched", 5, 7, "Launched", "Infra", "On Air", "",  "icon", "", "", "" },
                new object[] { "", 1, "Pending request doc…", "Pendiente_solicitar_doc...l", "Pending documentation", 6, 8, "On going", "Infra", "On Air", "icon",  "", "icon", "icon", "" }


                };
            }
        }



        #endregion


    }
}