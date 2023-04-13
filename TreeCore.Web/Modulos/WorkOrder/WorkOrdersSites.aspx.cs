using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModWorkOrders.pages
{
    public partial class WorkOrdersSites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.gridWOSites.Store.Primary.DataSource = this.DataSamplegridSites;
            this.gridWODetails.Store.Primary.DataSource = this.DataSamplegrid;
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
                new object[] {"Entire Site", "Maintenance", "TR00088 - Site", "1", "15% StatusName", "Proyect 1"}
                };
            }
        }
    }
}