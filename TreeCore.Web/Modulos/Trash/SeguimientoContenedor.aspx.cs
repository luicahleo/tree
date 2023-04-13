using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.PaginasComunes
{
	public partial class SeguimientoContenedor : System.Web.UI.Page
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

	}
}