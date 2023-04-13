using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.Page;

namespace TreeCore.PaginasComunes
{
	public partial class Proyectos : TreeCore.Page.BasePageExtNet
	{
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseUserControl currentUC;

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                


                //             #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                //             #endregion


                
                //}

                //#region EXCEL
                //if (Request.QueryString["opcion"] != null)
                //{
                //	string sOpcion = Request.QueryString["opcion"];

                //	if (sOpcion == "EXPORTAR")
                //	{
                //		try
                //		{
                //			List<Data.Bancos> listaDatos;
                //			string sOrden = Request.QueryString["orden"];
                //			string sDir = Request.QueryString["dir"];
                //			string sFiltro = Request.QueryString["filtro"];
                //			long CliID = long.Parse(Request.QueryString["cliente"]);
                //			int iCount = 0;

                //			listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                //			#region ESTADISTICAS
                //			try
                //			{
                //				Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                //				log.Info(GetGlobalResource(Comun.LogExcelExportado));
                //				EstadisticasController cEstadisticas = new EstadisticasController();
                //				cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                //			}
                //			catch (Exception ex)
                //			{
                //				log.Error(ex.Message);
                //			}
                //			#endregion
                //		}
                //		catch (Exception ex)
                //		{
                //			log.Error(ex.Message);
                //			Response.Write("ERROR: " + ex.Message);
                //		}

                //		Response.End();
                //	}

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            //#endregion
        }
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

		public static List<object> DataPhases
		{
			get
			{
				List<object> goods = new List<object>
			{
				new
				{
					Id = 1,
					Phases = "Legal Compliance",
				},



			};

				return goods;
			}
		}
	}
}