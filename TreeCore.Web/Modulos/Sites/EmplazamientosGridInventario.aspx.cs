using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Data;
using TreeCore.Page;

namespace TreeCore.ModGlobal.pages
{
    public partial class EmplazamientosGridInventario : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        Data.Usuarios oUser;

        #region GESTION PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                Data.Usuarios oUsuario = (TreeCore.Data.Usuarios)this.Session["USUARIO"];
                UsuariosController cUsuarios = new UsuariosController();

                if (oUsuario != null)
                {
                    oUser = cUsuarios.GetItem(oUsuario.UsuarioID);
                }

                #region FILTROS

                List<string> listaIgnore = new List<string>();

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grdInventarioElementosEmplazamientos, grdInventarioElementosEmplazamientos.GetStore(), grdInventarioElementosEmplazamientos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region EXCEL
                if (Request.QueryString["opcion"] != null)
                {
                    string sOpcion = Request.QueryString["opcion"];

                    if (sOpcion == "EXPORTAR")
                    {
                        try
                        {
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sCliente = Request.QueryString["cliente"];
                            string sFiltro = Request.QueryString["filtro"];
                            string sFiltro2 = Request.QueryString["aux3"];
                            string sTextoBuscado = Request.QueryString["aux4"];
                            string sIdBuscado = Request.QueryString["aux5"];
                            sResultadoKPIid = Request.QueryString["aux6"];

                            hdStringBuscador.Value = (!string.IsNullOrEmpty(sTextoBuscado)) ? sTextoBuscado : "";
                            hdIDEmplazamientoBuscador.Value = (!string.IsNullOrEmpty(sIdBuscado)) ? Convert.ToInt64(sIdBuscado) : new System.Nullable<long>();

                            List<JsonObject> lista;
                            List<string> listaVacia = new List<string>();
                            EmplazamientosController cEmplazamientos = new EmplazamientosController();
                            string sVariablesExcluidas = "";
                            int total = 0;
                            lista = cEmplazamientos.AplicarFiltroInterno(true, sFiltro2, -1, -1, out total, null, sFiltro, sCliente, hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, true, grdInventarioElementosEmplazamientos.ColumnModel, sVariablesExcluidas);
                            Comun.ExportacionDesdeListaNombreTask(grdInventarioElementosEmplazamientos.ColumnModel, lista, Response, sVariablesExcluidas, GetGlobalResource(Comun.strInventario).ToString(), Comun.DefaultLocale);

                            #region ESTADISTICAS
                            try
                            {
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(oUser.UsuarioID, oUser.ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, Comun.DefaultLocale);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            Response.Write("ERROR: " + ex.Message);
                        }

                        Response.End();
                    }
                }
                #endregion

                storeInventarioElementosEmplazamientos.Reload();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "EmplazamientosContenedor.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };

        }

        #endregion

        #region STORES
        protected void storeInventarioElementosEmplazamientos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int pageSize = Convert.ToInt32(cmbNumRegistros.Value);
                    int curPage = e.Page - 1;
                    int total;

                    string s = e.Parameters["filter"];

                    List<JsonObject> lista;
                    List<string> listaVacia = new List<string>();
                    EmplazamientosController cEmplazamientos = new EmplazamientosController();

                    #region KPI
                    if (hdnameIndiceID.Value != null && hdidsResultados.Value != null)
                    {
                        nameIndiceID = hdnameIndiceID.Value.ToString();
                        sResultadoKPIid = hdidsResultados.Value.ToString();
                    }
                    hdResultadoKPIid.SetValue(sResultadoKPIid);
                    #endregion

                    lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), pageSize, curPage, out total, e.Sort, s, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, grdInventarioElementosEmplazamientos.ColumnModel, "");
                    if (storeInventarioElementosEmplazamientos != null && lista != null)
                    {
                        e.Total = total;
                        hdTotalCountGrid.SetValue(total);
                        storeInventarioElementosEmplazamientos.DataSource = lista;
                        storeInventarioElementosEmplazamientos.DataBind();

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }


        #endregion

        #region DIRECT METHODS

        [DirectMethod]
        public DirectResponse GetDatosBuscador()
        {
            DirectResponse direct = new DirectResponse();
            EmplazamientosController cEmplazamientos = new EmplazamientosController();

            try
            {
                int total;
                List<JsonObject> lista;
                List<string> listaVacia = new List<string>();
                lista = cEmplazamientos.AplicarFiltroInterno(true, hdFiltrosAplicados.Value.ToString(), -1, -1, out total, null, null, hdCliID.Value.ToString(), hdStringBuscador, hdIDEmplazamientoBuscador, sResultadoKPIid, false, grdInventarioElementosEmplazamientos.ColumnModel, "");
                direct.Result = lista;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;

            return direct;
        }

        #endregion
    }
}