using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.Data.SqlClient;
using log4net;
using TreeCore.Clases;
using System.Reflection;


namespace TreeCore.ModCalidad
{
    public partial class CalidadDQSemaforos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA
        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                List<string> pathsOfScripts = new List<string>();
                pathsOfScripts.Add(Comun.BundleConfigPaths.CONTENT_JS_SEMAFORO);

                List<string> pathsOfCss = new List<string>();
                pathsOfCss.Add(Comun.BundleConfigPaths.CONTENT_CSS_SEMAFORO);
                
                ResourceManagerOperaciones(ResourceManagerTreeCore, pathsOfScripts, pathsOfCss);
                

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridSemaforos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridSemaforos, storePrincipal, gridSemaforos.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        List<Data.DQSemaforos> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        bool bEstado = Request.QueryString["aux"] == "true";
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, bEstado);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridSemaforos.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strCalidadKPI).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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

                ResourceManagerTreeCore.RegisterIcon(Icon.CogGo);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { btnDescargar }},
            { "Post", new List<ComponentBase> { btnAnadir }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
            { "Delete", new List<ComponentBase> { btnEliminar }}
        };
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storePrincipal_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];
                    bool bEstado = btnActivo.Pressed;

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, bEstado);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        PageProxy temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.DQSemaforos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, bool bEstado)
        {
            List<Data.DQSemaforos> listaDatos;
            DQSemaforosController cCalidad = new DQSemaforosController();

            try
            {
                if (bEstado)
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.DQSemaforos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
                }
                else
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.DQSemaforos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse oResponse;
            DQSemaforosController cCalidad = new DQSemaforosController();

            try
            {
                if (!agregar)
                {
                    long lIdSelect = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.DQSemaforos oDato = cCalidad.GetItem(lIdSelect);

                    if (oDato.DQSemaforo == txtName.Text)
                    {
                        oDato.DQSemaforo = txtName.Text;
                        oDato.IntervaloVerde = Convert.ToInt32(txtMedio.Text.Split('%')[0]);
                        oDato.IntervaloRojo = Convert.ToInt32(txtMedio2.Text.Split('%')[0]);
                    }
                    else
                    {
                        oDato.DQSemaforo = txtName.Text;
                        oDato.IntervaloVerde = Convert.ToInt32(txtMedio.Text.Split('%')[0]);
                        oDato.IntervaloRojo = Convert.ToInt32(txtMedio2.Text.Split('%')[0]);
                    }

                    oResponse = cCalidad.Update(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cCalidad.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cCalidad.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cCalidad.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    Data.DQSemaforos oDato = new Data.DQSemaforos();
                    oDato.DQSemaforo = txtName.Text;
                    oDato.Activo = true;
                    oDato.IntervaloVerde = Convert.ToInt32(txtMedio.Text.Split('%')[0]);
                    oDato.IntervaloRojo = Convert.ToInt32(txtMedio2.Text.Split('%')[0]);

                    oResponse = cCalidad.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cCalidad.SubmitChanges();

                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();

                            direct.Success = true;
                            direct.Result = "";
                        }
                        else
                        {
                            cCalidad.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cCalidad.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            DQSemaforosController cSemaforos = new DQSemaforosController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DQSemaforos oDato;
                oDato = cSemaforos.GetItem(S);
                txtName.Text = oDato.DQSemaforo;
                txtMedio.Text = oDato.IntervaloVerde.ToString() + "%";
                txtMedio2.Text = oDato.IntervaloRojo.ToString() + "%";

                winSaveSemaforo.Show();

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            DQSemaforosController cCalidad = new DQSemaforosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.DQSemaforos oDato = cCalidad.GetItem(lID);
                oResponse = cCalidad.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cCalidad.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cCalidad.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cCalidad.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            DQSemaforosController cController = new DQSemaforosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.DQSemaforos oDato = cController.GetItem(lID);
                oResponse = cController.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storePrincipal.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion

        #region FUNCTIONS

        #endregion

    }
}