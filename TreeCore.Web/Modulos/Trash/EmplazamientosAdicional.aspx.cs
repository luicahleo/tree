using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosAdicional : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Store store = this.grdTask.GetStore();
            store.DataSource = new object[]
            {
                new object[] { 2, "Area 1", "CAND_Region_TMP2918", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Distribution", "Technical", "Large", "", "", 0  },
                new object[] { 1, "Espartinas City council", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Core", "Mixed", "Medium", "", "", 0  },
                new object[] { 2, "Deployment Seville", "CAND_SE_D001155M", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Access", "Mixed", "Micro", "" , "", 0 },
                new object[] { 3, "Area 82", "CAND_Region_90324Z", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Office", "Technical", "Large", "Rooftop", "Zone A", 1  },
                new object[] { 1, "Palmar", "CAND_SE_D001155M", "TREE Telecom", "TREE Telecom", "TREE Telecom", "Blocked", "Distribution", "Technical", "Medium", "Rooftop" , "Zone A", 0 },
                new object[] { 2, "Verdes", "CAND_Region_TMP2918", "TREE Telecom", "TREE Telecom", "TREE Telecom", "Blocked", "Core", "Technical", "Medium", "Monopost", "Cluster North", 1 },
                new object[] { 4, "Area 1", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Access", "Mixed", "Medium" , "" , "Cluster North", 0 },
                new object[] { 1, "Espartinas City council", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Office", "Mixed", "Micro" , "", 0 },
                new object[] { 2, "Deployment Seville", "CAND_Region_90324Z", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Distribution", "Mixed", "Micro" , "", 0 },
                new object[] { 2, "Area 82", "CAND_Region_TMP2918", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Core", "Technical", "Large", "Rooftop", "Zone A", 0 },
                new object[] { 2, "Palmar", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Access", "Technical", "Large", "Monopost", "Zone A", 1 },
                new object[] { 2, "Verdes", "CAND_Region_90324Z", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Office", "Mixed" , "Micro", "Monopost", "", 0 },
                new object[] { 3, "Deployment Seville", "CAND_Region_TMP2918", "TREE Telecom", "TREE Telecom", "TREE Telecom", "Blocked" , "Distribution", "Mixed", "Micro", "Rooftop", "", 0 },
                new object[] { 1, "Espartinas City council", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Core" , "Technical", "Micro" , "" , "Zone A", 0 },
                new object[] { 2, "Area 1", "CAND_Region_90324Z", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air", "Access" , "Mixed", "Large" , "", "Zone A", 0},
                new object[] { 3, "Palmar", "CAND_Region_S852KM", "TREE Telecom", "TREE Telecom", "TREE Telecom", "On Air" , "Office", "Mixed", "Large" , "" , "Cluster North", 0}
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

            if (HideShow == "Hide")
            {

                this.grdTask.ColumnModel.Columns[5].Hidden = true;
                this.grdTask.ColumnModel.Columns[6].Hidden = true;
                this.grdTask.ColumnModel.Columns[7].Hidden = true;
                this.grdTask.ColumnModel.Columns[8].Hidden = true;
                this.grdTask.ColumnModel.Columns[9].Hidden = true;
                this.grdTask.ColumnModel.Columns[10].Hidden = true;
                this.grdTask.ColumnModel.Columns[11].Hidden = true;
                this.grdTask.ColumnModel.Columns[12].Hidden = true;
                this.grdTask.ColumnModel.Columns[13].Hidden = true;

                this.grdTask.ColumnModel.Columns[14].Hidden = false;

            }

            else
            {
                this.grdTask.ColumnModel.Columns[5].Hidden = false;
                this.grdTask.ColumnModel.Columns[6].Hidden = false;
                this.grdTask.ColumnModel.Columns[7].Hidden = false;
                this.grdTask.ColumnModel.Columns[8].Hidden = false;
                this.grdTask.ColumnModel.Columns[9].Hidden = false;
                this.grdTask.ColumnModel.Columns[10].Hidden = false;
                this.grdTask.ColumnModel.Columns[11].Hidden = false;
                this.grdTask.ColumnModel.Columns[12].Hidden = false;
                this.grdTask.ColumnModel.Columns[13].Hidden = false;

                this.grdTask.ColumnModel.Columns[14].Hidden = true;

            }
        }

    }





}