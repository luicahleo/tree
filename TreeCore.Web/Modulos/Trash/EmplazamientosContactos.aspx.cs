using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosContactos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Store store = this.grdContactos.GetStore();
            store.DataSource = new object[]
            {
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },
                new object[] { "Pedro", "Pére", "633 11 22 33", "Pedro@hotmail.com", "Seville", "Spain", "Lawyer", "CAND_SE_D001155M"  },
                new object[] { "Marta", "Suarez", "633 22 33 44", "MartaS@hotmail.com", "Malaga", "Spain", "Doctor", "CAND_MA_D342355M"  },



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

                this.grdContactos.ColumnModel.Columns[4].Hidden = true;
                this.grdContactos.ColumnModel.Columns[5].Hidden = true;
                this.grdContactos.ColumnModel.Columns[6].Hidden = true;
                this.grdContactos.ColumnModel.Columns[7].Hidden = true;

                this.grdContactos.ColumnModel.Columns[8].Hidden = false;


                this.grdContactos.ColumnModel.Columns[1].Hidden = false;
                this.grdContactos.ColumnModel.Columns[3].Hidden = false;

            }

            else if (HideShow == "Hide" && Res == "480")
            {


                this.grdContactos.ColumnModel.Columns[4].Hidden = true;
                this.grdContactos.ColumnModel.Columns[5].Hidden = true;
                this.grdContactos.ColumnModel.Columns[6].Hidden = true;
                this.grdContactos.ColumnModel.Columns[7].Hidden = true;


                this.grdContactos.ColumnModel.Columns[1].Hidden = true;
                this.grdContactos.ColumnModel.Columns[3].Hidden = true;

                this.grdContactos.ColumnModel.Columns[8].Hidden = false;

            }

            else
            {
                this.grdContactos.ColumnModel.Columns[1].Hidden = false;
                this.grdContactos.ColumnModel.Columns[3].Hidden = false;
                this.grdContactos.ColumnModel.Columns[4].Hidden = false;
                this.grdContactos.ColumnModel.Columns[5].Hidden = false;

                this.grdContactos.ColumnModel.Columns[5].Hidden = false;
                this.grdContactos.ColumnModel.Columns[6].Hidden = false;
                this.grdContactos.ColumnModel.Columns[7].Hidden = false;

                this.grdContactos.ColumnModel.Columns[8].Hidden = true;

            }
        }

    }





}