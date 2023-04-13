using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeCore.PaginasComunes
{
	public partial class ProyectosSLA : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		public static List<object> DataSLA
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					SLAName = "Technical Uninstall",
					Typology = "Uninstall workflow",
					Default = true,
					Stop = true,
					Inactive = true,
					


				},
					new
				{
					Id = 2,
					SLAName = "Onboarding",
					Typology = "Onboarding flow",
					Default = false,
					Stop = false,
					Inactive = false,

					},
					new
				{
					Id = 3,
					SLAName = "Real State Agency SLA",
					Typology = "Typology name",
					Default = false,
					Stop = false,
					Inactive = true,

				}					




			};

				return goods;
			}
		}

		public static List<object> DataTime
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					TimeLimit = 15,
					ToOk = 70,
					ToWarning = 40,

				}
						
			};

				return goods;
			}
		}

		public static List<object> DataSuppliers
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					Supplier = "Landaria",
					Contact = "info@landaria.com",
					Report = false,
					Contacts = true,
					Active = false,

				},
					new
				{
					Id = 2,
					Supplier = "Bestplace",
					Contact = "a.gonzalez@bestplace.com",
					Report = true,
					Contacts = false,
					Active = true,

				},
					new
				{
					Id = 3,
					Supplier = "Goldencorner",
					Contact = "mail@goldencorner.com",
					Report = true,
					Contacts = false,
					Active = true,

				},



			};

				return goods;
			}
		}

		public static List<object> DataPenalties
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					Name = "3 days late",
					Amount = 3.280,
					Currency = "Euros",
					Comments = "Just in case they exceed the time limit in 3 days",
					
				}
					

			};

				return goods;
			}
		}

		public static List<object> DataNotifications
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris",
					Recipients = "emydickinson@telcocom.com",
					Enviar = true,

				},
					new
				{
					Id = 2,
					Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.",
					Recipients = "jeanpsartre@pensalia.com",
					Enviar = false,

				},
					new
				{
					Id = 3,
					Message = "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim",
					Recipients = "pamandjim@summerlove.com",
					Enviar = false,

				},
					new
				{
					Id = 4,
					Message = "Consectetur adipiscing elit, sed do eiusmod tempor incididunt.",
					Recipients = "admin_sharing",
					Enviar = false,

				},





			};

				return goods;
			}
		}

	}

}