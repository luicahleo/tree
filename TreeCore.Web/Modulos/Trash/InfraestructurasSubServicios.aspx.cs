using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;


namespace TreeCore.ModGlobal
{
    public partial class InfraestructurasSubServicios : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));
                

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
                        List<Data.Vw_GlobalSubElementosMarcos> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, ClienteID);

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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_GLOBAL_INFRAESTRUCTURA_SUBSERVICIOS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_GLOBAL_INFRAESTRUCTURA_SUBSERVICIOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
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
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<Data.Vw_GlobalSubElementosMarcos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_GlobalSubElementosMarcos> listaDatos;
            InfraestructurasSubserviciosController cTipos = new InfraestructurasSubserviciosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cTipos.GetItemsWithExtNetFilterList<Data.Vw_GlobalSubElementosMarcos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, ""/*"ClienteID == " + lClienteID*/);
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

        #region TIPOS DATOS

        protected void storeTipoDato_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                TiposDatosController cTipos = new TiposDatosController();

                try
                {
                    List<Data.TiposDatos> listaTipos;

                    listaTipos = cTipos.GetAllTipoDatos();

                    if (listaTipos != null)
                    {
                        storeTipoDato.DataSource = listaTipos;
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

        #endregion

        #region MONEDA

        protected void storeMoneda_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                MonedasController cMoneda = new MonedasController();
                try
                {
                    List<Data.Monedas> listaMonedas = new List<Data.Monedas>();

                    listaMonedas = cMoneda.GetAllMonedas();

                    if (listaMonedas != null) 
                    {
                        storeMoneda.DataSource = listaMonedas; 
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

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            InfraestructurasSubserviciosController cIntraestructurasSub = new InfraestructurasSubserviciosController();
            long lCliID = 0;
            
            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.GlobalSubElementosMarcos oDato;
                    oDato = cIntraestructurasSub.GetItem(lS);

                    if (oDato.GlobalSubElemento == txtSubElementos.Text)
                    {
                        oDato.GlobalSubElemento = txtSubElementos.Text;
                        oDato.Cantidad = numCantidad.Number;
                        oDato.Unidad = txtUnidad.Text;
                        oDato.Valor = NumValor.Number;
                        oDato.FactorComuna = NumFactorComuna.Number;
                        oDato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                        oDato.FechaModificacion = DateTime.Now;
                        oDato.TipoDatoID = long.Parse(cmbTipoDato.SelectedItem.Value.ToString());
                        oDato.UsuarioID = Usuario.UsuarioID;
                            
                        oDato.EsCantidad = chkEsCantidad.Checked;
                        oDato.TieneRedondeo = chkTieneRedondeo.Checked;
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cIntraestructurasSub.RegistroDuplicado(txtSubElementos.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.GlobalSubElemento = txtSubElementos.Text;
                            oDato.Cantidad = numCantidad.Number;
                            oDato.Unidad = txtUnidad.Text;
                            oDato.Valor = NumValor.Number;
                            oDato.FactorComuna = NumFactorComuna.Number;
                            oDato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                            oDato.FechaModificacion = DateTime.Now;
                            oDato.TipoDatoID = long.Parse(cmbTipoDato.SelectedItem.Value.ToString());
                            oDato.UsuarioID = Usuario.UsuarioID;

                            oDato.EsCantidad = chkEsCantidad.Checked;
                            oDato.TieneRedondeo = chkTieneRedondeo.Checked;
                        }
                    }

                    if (cIntraestructurasSub.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cIntraestructurasSub.RegistroDuplicado(txtSubElementos.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalSubElementosMarcos oDato = new Data.GlobalSubElementosMarcos();
                        oDato.GlobalSubElemento = txtSubElementos.Text;
                        oDato.Cantidad = numCantidad.Number;
                        oDato.Unidad = txtUnidad.Text;
                        oDato.Valor = NumValor.Number;
                        oDato.FactorComuna = NumFactorComuna.Number;
                        oDato.MonedaID = long.Parse(cmbMoneda.Value.ToString());
                        oDato.FechaCreacion = DateTime.Now;
                        oDato.TipoDatoID = long.Parse(cmbTipoDato.SelectedItem.Value.ToString());
                        oDato.UsuarioID = Usuario.UsuarioID;

                        oDato.EsCantidad = chkEsCantidad.Checked;
                        oDato.TieneRedondeo = chkTieneRedondeo.Checked;

                        oDato.Activo = true;

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            //oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            //oDato.ClienteID = lCliID;
                        }

                        if (cIntraestructurasSub.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
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

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();           
            InfraestructurasSubserviciosController cIntraestructurasSub = new InfraestructurasSubserviciosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalSubElementosMarcos oDato;
                oDato = cIntraestructurasSub.GetItem(lS);

                if (oDato.Cantidad != null)
                {
                    numCantidad.Number = oDato.Cantidad.Value;
                }

                txtUnidad.Text = oDato.Unidad;

                if (oDato.Valor != null)
                {
                    NumValor.Number = oDato.Valor.Value;
                }
                if (oDato.GlobalSubElementoMarcoID != 0)
                {
                    cmbMoneda.SetValue(oDato.MonedaID);
                    cmbTipoDato.SetValue(oDato.TipoDatoID);

                }

                NumFactorComuna.Text = Convert.ToString(oDato.FactorComuna);
                cmbMoneda.SelectedItem.Value = oDato.MonedaID.ToString();                
                txtSubElementos.Text = oDato.GlobalSubElemento;
                cmbTipoDato.SelectedItem.Value = oDato.TipoDatoID.ToString();

                chkEsCantidad.Checked = oDato.EsCantidad;
                chkTieneRedondeo.Checked = oDato.TieneRedondeo;

                winGestion.Show();
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            InfraestructurasSubserviciosController cIntraestructurasSub = new InfraestructurasSubserviciosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                long lCliID = 0;
                Data.GlobalSubElementosMarcos oDato;

                if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                {
                    lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                }
                else if (ClienteID.HasValue)
                {
                    lCliID = Convert.ToInt32(ClienteID);
                }

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cIntraestructurasSub.GetDefault(lCliID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.GlobalSubElementoMarcoID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cIntraestructurasSub.UpdateItem(oDato);
                        }

                        oDato = cIntraestructurasSub.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cIntraestructurasSub.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cIntraestructurasSub.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cIntraestructurasSub.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

                cIntraestructurasSub = null;
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
            InfraestructurasSubserviciosController cIntraestructurasSub = new InfraestructurasSubserviciosController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cIntraestructurasSub.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cIntraestructurasSub.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                cIntraestructurasSub = null;
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
            InfraestructurasSubserviciosController cController = new InfraestructurasSubserviciosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalSubElementosMarcos oDato;
                oDato = cController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

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

        #endregion
    }
}