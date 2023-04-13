using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Reflection;
using log4net;
using System.Data.SqlClient;


namespace TreeCore.ModGlobal
{
    public partial class Licencias : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(grid, storePrincipal, grid.ColumnModel, listaIgnore, _Locale);
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
                    cmbClientes.Hidden = false;
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
                        List<Data.Vw_Licencias> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetLocalResourceObject(Comun.jsTituloModulo).ToString(), _Locale);
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.Vw_Licencias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Licencias> listaDatos;
            LicenciasController cLicencias = new LicenciasController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cLicencias.GetItemsWithExtNetFilterList<Data.Vw_Licencias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    listaDatos = null;
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

        #region CLIENTES

        protected void storeClientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Clientes> listaClientes;

                    listaClientes = ListaClientes();

                    if (listaClientes != null)
                    {
                        storeClientes.DataSource = listaClientes;
                    }

                    if (ClienteID.HasValue)
                    {
                        cmbClientes.SelectedItem.Value = ClienteID.Value.ToString();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Clientes> ListaClientes()
        {
            List<Data.Clientes> listaDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region LICENCIAS TIPO

        protected void storeLicenciasTipo_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                LicenciasTiposController cTipos = new LicenciasTiposController();

                try
                {
                    List<Data.LicenciasTipos> lista;
                    lista = cTipos.GetActivos();

                    if (lista != null)
                    {
                        storeLicenciasTipo.DataSource = lista;
                    }

                    cTipos = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse Agregar()
        {
            DirectResponse ajax = new DirectResponse();
            LicenciasController cLicencias = new LicenciasController();
            Data.Licencias listaLicencias;
            long lCliID = 0;

            try
            {
                lCliID = long.Parse(hdCliID.Value.ToString());

                if (cLicencias.RegistroDuplicado(txtCodigoLicencia.Text, lCliID))
                {
                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                }
                else
                {
                    Data.Licencias oDato = new Data.Licencias();
                    listaLicencias = cLicencias.ExtractLicenciaFromCodigo(txtCodigoLicencia.Text);
                    oDato.Activa = true;

                    if (listaLicencias != null)
                    {
                        oDato.CodigoLicencia = listaLicencias.CodigoLicencia;
                        oDato.FechaActivacion = listaLicencias.FechaActivacion;
                        oDato.FechaCaducidad = listaLicencias.FechaCaducidad;
                        oDato.Disponibles = oDato.Disponibles + listaLicencias.NumeroLicencias - oDato.NumeroLicencias;
                        oDato.NumeroLicencias = listaLicencias.NumeroLicencias;
                        oDato.LicenciaTipoID = listaLicencias.LicenciaTipoID;

                    }
                    oDato.ClienteID = lCliID;

                    if (cLicencias.AddItem(oDato) != null)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetLocalResourceObject("strMensanjeGenerico").ToString();
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
            LicenciasController cLicencias = new LicenciasController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cLicencias.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                cLicencias = null;
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            LicenciasController cController = new LicenciasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.Licencias oDato;
                oDato = cController.GetItem(lID);
                oDato.Activa = !oDato.Activa;

                if (cController.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
                    log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
                }
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

        #endregion

        #region FUNCTIONS

        [DirectMethod]
        public DirectResponse GenerarCodigo()
        {
            DirectResponse ajax = new DirectResponse();
            ajax.Result = "";
            ajax.Success = true;

            LicenciasController cLicencias = new LicenciasController();
            Data.Licencias oLicencia = new Data.Licencias();
            string sCodigo = null;

            try
            {
                oLicencia.Activa = true;
                oLicencia.FechaActivacion = txtFechaActivacion.SelectedDate;
                oLicencia.FechaCaducidad = txtFechaCaducidad.SelectedDate;
                oLicencia.NumeroLicencias = int.Parse(txtNumeroLicencias.Text);
                oLicencia.ClienteID = (ClienteID.Value);
                oLicencia.LicenciaTipoID = long.Parse(cmbTiposLicencias.SelectedItem.Value);
                sCodigo = cLicencias.CreateCodeFromLicencia(oLicencia);

                MensajeBox(GetLocalResourceObject("jsTituloModulo").ToString(), sCodigo, MessageBox.Icon.INFO, null);
                winGenerar.Hide();

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

        #endregion

    }
}