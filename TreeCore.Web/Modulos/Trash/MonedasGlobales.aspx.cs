using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using System.Reflection;

namespace TreeCore.ModGlobal
{
    public partial class MonedasGlobales : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> ListaFuncionalidades = new List<long>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {

                ListaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> ListaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);

                List<string> ListaIgnoreDetalle = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters2, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));
                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridMaestro, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                Comun.Seleccionable(GridDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);
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
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sModuloID = Request.QueryString["aux"].ToString();
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.Vw_MonedasGlobales> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                        }
                        #endregion

                        #region DETALLE
                        else
                        {
                            List<Data.Vw_MonedasGlobales> ListaDatosDetalle = null;
                            ListaDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaDatosDetalle, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
                                EstadisticasController cEstadisticas = new EstadisticasController();
                                cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, ClienteID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
                                log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            }
                            catch (Exception ex)
                            {
                                LogController lController = new LogController();
                                lController.EscribeLog(Ip, Usuario.UsuarioID, ex.Message);
                                log.Error(ex.Message);
                            }
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
                ResourceManagerTreeCore.RegisterIcon(Icon.ChartCurve);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_MONEDAS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;

                btnEliminarDetalle.Hidden = false;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
            }
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
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;

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

        private List<Data.Vw_MonedasGlobales> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_MonedasGlobales> ListaDatos;
            MonedasGlobalesController cMonedasGlobales = new MonedasGlobalesController();

            try
            {
                ListaDatos = cMonedasGlobales.GetItemsWithExtNetFilterList<Data.Vw_MonedasGlobales>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "FechaFin == null");
            }
            catch (Exception ex)
            {
                ListaDatos = null;
                log.Error(ex.Message);
            }

            return ListaDatos;
        }


        #endregion

        #region DETALLE

        protected void storeDetalle_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters2"];

                    if (!ModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(ModuloID.Value);
                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);

                    if (vLista != null)
                    {
                        storeDetalle.DataSource = vLista;

                        PageProxy temp = (PageProxy)storeDetalle.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_MonedasGlobales> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_MonedasGlobales> ListaDatos = new List<Data.Vw_MonedasGlobales>();
            MonedasGlobalesController CMonedasGlobales = new MonedasGlobalesController();

            try
            {
                ListaDatos = CMonedasGlobales.GetItemsWithExtNetFilterList<Data.Vw_MonedasGlobales>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "MonedaGlobalID == " + lMaestroID);
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return ListaDatos;
        }


        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh (object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                MonedasController cMonedas = new MonedasController();

                try
                {
                    var vLista = cMonedas.GetAllActivos();

                    if (vLista != null)
                    {
                        storeMonedas.DataSource = vLista;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            bool bValido = true;


            try
            {
                if (!bAgregar)
                {
                    long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    Data.MonedasGlobales oDato;
                    MonedasGlobalesController cController = new MonedasGlobalesController();
                    oDato = cController.GetItem(ID);

                    if (txtFechaInicio.Text.Equals(DateTime.MinValue.ToString()))
                    {
                        oDato.FechaFin = DateTime.Now.AddDays(-1);

                    }
                    else
                    {
                        oDato.FechaFin = Convert.ToDateTime(txtFechaInicio.Text).AddDays(-1);
                    }

                    if (cmbMonedas.SelectedItem.Text != "" && cmbMonedas.SelectedItem.Text != null)
                    {
                        oDato.MonedaID = long.Parse(cmbMonedas.SelectedItem.Value);
                    }

                    if (oDato.FechaFin <= oDato.FechaInicio)
                    {
                        bValido = false;
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsFechaIncorrecta), Ext.Net.MessageBox.Icon.INFO, null);
                    }

                    if (bValido && cController.UpdateItem(oDato))
                    {
                        Data.MonedasGlobales oMoneda = new Data.MonedasGlobales();
                        oMoneda.MonedaID = oDato.MonedaID;
                        oMoneda.CambioDolar = Comun.ConvertCambioMoneda(txtDolar.Text);
                        oMoneda.CambioEuro = Comun.ConvertCambioMoneda(txtEuro.Text);

                        if (txtFechaInicio.Text.Equals(DateTime.MinValue.ToString()))
                        {
                            oMoneda.FechaInicio = DateTime.Now;
                        }
                        else
                        {
                            oMoneda.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                        }

                        cController.AddItem(oMoneda);
                        storePrincipal.DataBind();
                        storeDetalle.DataBind();
                    }
                    cmbMonedas.Enabled = true;
                }
                else
                {
                    Data.MonedasGlobales oDato = new Data.MonedasGlobales();
                    MonedasGlobalesController cController = new MonedasGlobalesController();

                    oDato.MonedaID = Convert.ToInt32(cmbMonedas.SelectedItem.Value.ToString());
                    oDato.CambioDolar = Comun.ConvertCambioMoneda(txtDolar.Text);
                    oDato.CambioEuro = Comun.ConvertCambioMoneda(txtEuro.Text);

                    if (txtFechaInicio.Text.Equals(DateTime.MinValue.ToString()))
                    {
                        oDato.FechaInicio = DateTime.Now;
                    }
                    else
                    {
                        oDato.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
                    }

                    if (cController.GetMonedasGlobalesVigentesByMoneda(oDato.MonedaID).Count > 0)
                    {
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        oDato = cController.AddItem(oDato);
                    }

                    storePrincipal.DataBind();
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse ajax = new DirectResponse();

            try

            {
                long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.MonedasGlobales oDato = new Data.MonedasGlobales();
                MonedasGlobalesController cController = new MonedasGlobalesController();

                oDato = cController.GetItem(ID);

                txtDolar.Text = oDato.CambioDolar.ToString();
                txtEuro.Text = oDato.CambioEuro.ToString();
                txtFechaInicio.Text = oDato.FechaInicio.ToString();
                cmbMonedas.SetValue(oDato.MonedaID);

                cmbMonedas.Enabled = false;
                winGestion.Show();
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                ajax.Result = "";
                log.Error(ex.Message);
                return ajax;
            }

            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            MonedasGlobalesController cMonedasGlobales = new MonedasGlobalesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            long lMonedaLocalID = 0;
            List<Data.MonedasGlobales> listaDatos;
            Data.MonedasGlobales oMoneda;

            try
            {
                oMoneda = cMonedasGlobales.GetItem(lID);
                lMonedaLocalID = oMoneda.MonedaID;

                listaDatos = cMonedasGlobales.GetMonedasGlobalesByMoneda(lMonedaLocalID);

                foreach (Data.MonedasGlobales oReg in listaDatos)
                {
                    if (cMonedasGlobales.DeleteItem(oReg.MonedaGlobalID))
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                        direct.Success = true;
                        direct.Result = "";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            return direct;
        }

        #endregion

        #region DIRECT METHOD DETALLE

        [DirectMethod()]
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            MonedasGlobalesController cMonedasGlobales = new MonedasGlobalesController();

            direct.Result = "";
            direct.Success = true;

            Data.MonedasGlobales oMoneda;
            DateTime dFecha = DateTime.MinValue;
            long lMonedaLocalID = 0;
            List<Data.MonedasGlobales> listaDatos;
            long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                oMoneda = cMonedasGlobales.GetItem(lID);
                dFecha = oMoneda.FechaInicio;
                lMonedaLocalID = oMoneda.MonedaID;

                listaDatos = cMonedasGlobales.GetMonedasGlobalesByMonedaFecha(lMonedaLocalID, dFecha);

                if (listaDatos.Count > 0)
                {
                    oMoneda = listaDatos[0];
                    oMoneda.FechaInicio = dFecha;
                    cMonedasGlobales.UpdateItem(oMoneda);
                }

                if (cMonedasGlobales.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException Sql)
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.jsTieneRegistros);
                    log.Error(Sql.Message);
                    return direct;
                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    log.Error(ex.Message);
                    return direct;
                }
            }

            return direct;
        }

        #endregion

    }
}