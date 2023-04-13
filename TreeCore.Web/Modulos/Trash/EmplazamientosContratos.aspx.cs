using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosContratos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Store store = this.grdTask.GetStore();
            store.DataSource = new object[]
            {
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },
                new object[] { "1223123_30", "Access Easement", "New site", "Pending to terminate", "10/02/2020", "20/02/2020", "Area 1", "CAND_SE_D001155M"  },


            };


            this.GridContractsD.Store.Primary.DataSource = new object[]
            {
                new object[] { "Docu1", "Form" },
                new object[] { "Docu2","Contract" },
                new object[] { "Docu2","Contract" },
                new object[] { "Docu2","Contract" },
                new object[] { "Docu2","Contract" },
                new object[] { "Docu2","Contract" },

            };
            this.GridContractsD.Store.Primary.DataBind();

            this.GridProjectsD.Store.Primary.DataSource = new object[]
{
                new object[] { "Project1", "Form" },
                new object[] { "Project12", "Contract" },

};
            this.GridProjectsD.Store.Primary.DataBind();
        }



        [DirectMethod]
        public void ColumnHider(string HideShow, string Res)
        {

            if (HideShow == "Hide" && Res == "1024")
            {

                this.grdTask.ColumnModel.Columns[4].Hidden = true;
                this.grdTask.ColumnModel.Columns[5].Hidden = true;
                this.grdTask.ColumnModel.Columns[6].Hidden = true;
                this.grdTask.ColumnModel.Columns[7].Hidden = true;

                this.grdTask.ColumnModel.Columns[8].Hidden = false;


                this.grdTask.ColumnModel.Columns[1].Hidden = false;
                this.grdTask.ColumnModel.Columns[2].Hidden = false;

            }

            else if (HideShow == "Hide" && Res == "480")
            {


                this.grdTask.ColumnModel.Columns[4].Hidden = true;
                this.grdTask.ColumnModel.Columns[5].Hidden = true;
                this.grdTask.ColumnModel.Columns[6].Hidden = true;
                this.grdTask.ColumnModel.Columns[7].Hidden = true;


                this.grdTask.ColumnModel.Columns[1].Hidden = true;
                this.grdTask.ColumnModel.Columns[2].Hidden = true;

                this.grdTask.ColumnModel.Columns[8].Hidden = false;

            }

            else
            {
                this.grdTask.ColumnModel.Columns[1].Hidden = false;
                this.grdTask.ColumnModel.Columns[2].Hidden = false;
                this.grdTask.ColumnModel.Columns[4].Hidden = false;
                this.grdTask.ColumnModel.Columns[5].Hidden = false;

                this.grdTask.ColumnModel.Columns[5].Hidden = false;
                this.grdTask.ColumnModel.Columns[6].Hidden = false;
                this.grdTask.ColumnModel.Columns[7].Hidden = false;

                this.grdTask.ColumnModel.Columns[8].Hidden = true;

            }
        }

    }





}