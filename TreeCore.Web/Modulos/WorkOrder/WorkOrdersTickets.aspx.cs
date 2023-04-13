using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.ModWorkOrders.pages
{
    public partial class WorkOrdersTickets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataView1.Store.Primary.DataSource = this.DataSamplegrid;

        }

        private object[] DataSamplegrid
        {
            get
            {
                return new object[]
                {
                new object[] { "Brandon Sanderson", "mario.lopez@telcocom.com", "Reinvestment","21/10/2021", "Incident Name", "Description Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.", "98989898", "TREE0088", "T8989 - Site", "StatusName", "/_temp/imgUsuarios/81.jpg"},
                new object[] { "Brandon Sanderson", "mario.lopez@telcocom.com", "","21/10/2021", "Incident Name", "Lorem iptum dolor", "98989898", "TREE0088", "T8989 - Site", "StatusName", "/_temp/imgUsuarios/81.jpg"},
                new object[] { "Brandon Sanderson", "mario.lopez@telcocom.com", "Reinvestment","21/10/2021", "Incident Name", "Lorem iptum dolor", "98989898", "TREE0088", "T8989 - Site", "StatusName", "/_temp/imgUsuarios/81.jpg"},
                new object[] { "Brandon Sanderson", "mario.lopez@telcocom.com", "","21/10/2021", "Incident Name", "Lorem iptum dolor", "98989898", "TREE0088", "T8989 - Site", "StatusName", "/_temp/imgUsuarios/81.jpg"},
                new object[] { "Brandon Sanderson", "mario.lopez@telcocom.com", "Reinvestment","21/10/2021", "Incident Name", "Lorem iptum dolor", "98989898", "TREE0088", "T8989 - Site", "StatusName", "/_temp/imgUsuarios/81.jpg"}
                };
            }
        }

    }
}