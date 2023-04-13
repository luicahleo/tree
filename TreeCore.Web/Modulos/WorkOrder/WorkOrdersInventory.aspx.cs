using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModWorkOrders.pages
{
    public partial class WorkOrdersInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.gridWOInventory.Store.Primary.DataSource = this.DataSamplegridSites;
            this.gridWOInventoryDetails.Store.Primary.DataSource = this.DataSamplegrid;
        }

        private object[] DataSamplegridSites
        {
            get
            {
                return new object[]
                {
                new object[] {"TR00088", "COD-123456", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.", "1", "1", "1" }
                };
            }
        }

        private object[] DataSamplegrid
        {
            get
            {
                return new object[]
                {
                new object[] {"Antenna TSX8989", "An89389", "ATRS-93838", "02/11/2021", "", "Proyect 1", "15% StatusName"},
                new object[] {"Antenna TSX8989", "An89389", "ATRS-93838", "02/11/2021", "", "Proyect 1", "30% StatusName"},
                new object[] {"Antenna TSX8989", "An89389", "ATRS-93838", "02/11/2021", "31/12/2021", "Proyect 1", "95% StatusName"}
                };
            }
        }
    }
}