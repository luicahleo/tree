using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.PaginasComunes
{
	public partial class BusquedaGlobalContenedor : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}


		public static List<object> DataGridAsR
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					Ini = "SP",
					Name = "Sara Parker",
					Profile = "Installing Agency",
					Company = "Telcocom",
					Email = "sara.parker@telcocom",
					Project = "Sharing 2020",
					Authorized = true,
					Staff = true,
					Support = true,
					LDAP = false,
					License = "21/12/2022",
					KeyExpiration = "06/12/2021",
					LastKey = "18/10/2020",
					LastAccess = "05/11/2020",

				},



			};

				return goods;
			}
		}


		[DirectMethod]
		public void ColumnHider(string HideShow, string Res)
		{

			if (HideShow == "Hide" && Res == "1024")
			{


				this.GridBusqMain.ColumnModel.Columns[3].Hidden = true;
				this.GridBusqMain.ColumnModel.Columns[4].Hidden = true;
				this.GridBusqMain.ColumnModel.Columns[6].Hidden = false;

				this.GridBusqMain.ColumnModel.Columns[0].Hidden = false;
				this.GridBusqMain.ColumnModel.Columns[1].Hidden = false;

			}
			else if (HideShow == "Hide" && Res == "480")
			{

				this.GridBusqMain.ColumnModel.Columns[0].Hidden = true;

				this.GridBusqMain.ColumnModel.Columns[1].Hidden = true;
				this.GridBusqMain.ColumnModel.Columns[3].Hidden = true;
				this.GridBusqMain.ColumnModel.Columns[4].Hidden = true;

				this.GridBusqMain.ColumnModel.Columns[6].Hidden = false;
			}

			else
			{
				this.GridBusqMain.ColumnModel.Columns[0].Hidden = false;
				this.GridBusqMain.ColumnModel.Columns[1].Hidden = true;


				this.GridBusqMain.ColumnModel.Columns[3].Hidden = false;
				this.GridBusqMain.ColumnModel.Columns[4].Hidden = false;

				this.GridBusqMain.ColumnModel.Columns[6].Hidden = true;

			}
		}

	}
}