using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using log4net;
using System.Reflection;
using System.Data.SqlClient;

namespace TreeCore.ModGlobal
{
    public partial class ActivosEstados : TreeCore.Page.BasePageExtNet
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
                else {
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
                        List<Data.ActivosEstados> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ACTIVOS_ESTADOS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
                btnAlmacen.Hidden = true;
                btnBaja.Hidden = true;
                btnMantenimiento.Hidden = true;
                btnReparacion.Hidden = true;
                btnTraslado.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_ACTIVOS_ESTADOS))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;
                btnAlmacen.Hidden = false;
                btnBaja.Hidden = false;
                btnMantenimiento.Hidden = false;
                btnReparacion.Hidden = false;
                btnTraslado.Hidden = false;
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro,  long.Parse(hdCliID.Value.ToString()));

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

        private List<Data.ActivosEstados> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.ActivosEstados> listaDatos;
            ActivosEstadosController CActivosEstados = new ActivosEstadosController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CActivosEstados.GetItemsWithExtNetFilterList<Data.ActivosEstados>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
                    List<Data.Clientes> listaClientes = ListaClientes();

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
                    string codTit = Util.ExceptionHandler(ex);
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.ActivosEstados oDato = new Data.ActivosEstados();
                    oDato = cActivosEstados.GetItem(lS);

                    if (oDato.ActivoEstado == txtEstado.Text)
                    {
                        oDato.ActivoEstado = txtEstado.Text;
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cActivosEstados.RegistroDuplicado(txtEstado.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.ActivoEstado = txtEstado.Text;
                        }
                    }
                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cActivosEstados.RegistroDuplicado(txtEstado.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ActivosEstados oDato = new Data.ActivosEstados();
                        oDato.ActivoEstado = txtEstado.Text;
                        oDato.Activo = true;

                        oDato.ClienteID = lCliID;

                        if (cActivosEstados.AddItem(oDato) != null)
                        {
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
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato = cActivosEstados.GetItem(lS);
                txtEstado.Text = oDato.ActivoEstado;
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
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.ActivosEstados oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cActivosEstados.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cActivosEstados.UpdateItem(oDato);
                    }

                    oDato = cActivosEstados.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cActivosEstados.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cActivosEstados.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cActivosEstados.UpdateItem(oDato);
                }

                log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
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
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cActivosEstados.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cActivosEstados.DeleteItem(lID))
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

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cController = new ActivosEstadosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato = cController.GetItem(lID);
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

        #region TRASLADO

        [DirectMethod()]
        public DirectResponse Traslados()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato;
                Data.ActivosEstados oEstadoTraslado = cActivosEstados.GetEstadoTraslados(true);

                if (oEstadoTraslado != null)
                {
                    oEstadoTraslado.Traslado = false;

                    if (cActivosEstados.UpdateItem(oEstadoTraslado))
                    {
                        oDato = cActivosEstados.GetItem(lS);
                        oDato.Traslado = true;

                        if (cActivosEstados.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                            log.Info(GetGlobalResource(Comun.LogCambioTraslado));
                        }
                    }
                }
                else
                {
                    oDato = cActivosEstados.GetItem(lS);
                    oDato.Traslado = true;

                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogCambioTraslado));
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

        #endregion

        #region BAJA

        [DirectMethod()]
        public DirectResponse Bajas()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato;
                Data.ActivosEstados oEstadoBaja = cActivosEstados.GetEstadoBajas(true);

                if (oEstadoBaja != null)
                {
                    oEstadoBaja.Baja = false;

                    if (cActivosEstados.UpdateItem(oEstadoBaja))
                    {
                        oDato = cActivosEstados.GetItem(lS);
                        oDato.Baja = true;

                        if (cActivosEstados.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                            log.Info(GetGlobalResource(Comun.LogCambioBaja));
                        }
                    }
                }
                else
                {
                    oDato = cActivosEstados.GetItem(lS);
                    oDato.Baja = true;

                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogCambioBaja));
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

        #endregion

        #region MANTENIMIENTO

        [DirectMethod()]
        public DirectResponse Mantenimientos()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato;
                Data.ActivosEstados oEstadoMantenimiento = cActivosEstados.GetEstadoMantenimientos(true);

                if (oEstadoMantenimiento != null)
                {
                    oEstadoMantenimiento.Mantenimiento = false;

                    if (cActivosEstados.UpdateItem(oEstadoMantenimiento))
                    {
                        oDato = cActivosEstados.GetItem(lS);
                        oDato.Mantenimiento = true;

                        if (cActivosEstados.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                            log.Info(GetGlobalResource(Comun.LogCambioMantenimiento));
                        }
                    }
                }
                else
                {
                    oDato = cActivosEstados.GetItem(lS);
                    oDato.Mantenimiento = true;

                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogCambioMantenimiento));
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

        #endregion

        #region REPARACIONES

        [DirectMethod()]
        public DirectResponse Reparaciones()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato;
                Data.ActivosEstados oEstadoReparaciones = cActivosEstados.GetEstadoReparaciones(true);

                if (oEstadoReparaciones != null)
                {
                    oEstadoReparaciones.Reparacion = false;

                    if (cActivosEstados.UpdateItem(oEstadoReparaciones))
                    {
                        oDato = cActivosEstados.GetItem(lS);
                        oDato.Reparacion = true;

                        if (cActivosEstados.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                            log.Info(GetGlobalResource(Comun.LogCambioReparacion));
                        }
                    }
                }
                else
                {
                    oDato = cActivosEstados.GetItem(lS);
                    oDato.Reparacion = true;

                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogCambioReparacion));
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

        #endregion

        #region ALMACEN

        [DirectMethod()]
        public DirectResponse Almacenes()
        {
            DirectResponse direct = new DirectResponse();
            ActivosEstadosController cActivosEstados = new ActivosEstadosController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ActivosEstados oDato;
                Data.ActivosEstados oEstadoAlmacen = cActivosEstados.GetEstadoAlmacen(true);

                if (oEstadoAlmacen != null)
                {
                    oEstadoAlmacen.Almacen = false;

                    if (cActivosEstados.UpdateItem(oEstadoAlmacen))
                    {
                        oDato = cActivosEstados.GetItem(lS);
                        oDato.Almacen = true;

                        if (cActivosEstados.UpdateItem(oDato))
                        {
                            storePrincipal.DataBind();
                            log.Info(GetGlobalResource(Comun.LogCambioAlmacen));
                        }
                    }
                }
                else
                {
                    oDato = cActivosEstados.GetItem(lS);
                    oDato.Almacen = true;

                    if (cActivosEstados.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                        log.Info(GetGlobalResource(Comun.LogCambioAlmacen));
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

        #endregion

        #endregion

    }
}