using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;


namespace TreeCore.ModGlobal
{
    public partial class NotificacionesCadencias : TreeCore.Page.BasePageExtNet
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
                        List<Data.NotificacionesCadencias> listaDatos;
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
            if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_NOTIFICACIONES_CADENCIAS))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;
            }
            else if (listaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_NOTIFICACIONES_CADENCIAS))
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
                }
            }
        }

        private List<Data.NotificacionesCadencias> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.NotificacionesCadencias> listaDatos;
            NotificacionesCadenciasController cNotificacionesCadencias = new NotificacionesCadenciasController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = cNotificacionesCadencias.GetItemsWithExtNetFilterList<Data.NotificacionesCadencias>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            NotificacionesCadenciasController cController = new NotificacionesCadenciasController();
            long lCliID = 0;

            try
            {
                //En caso de tratarse de una edición
                if (!bAgregar)
                {
                    //Recuperación del elemento a editar
                    long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    Data.NotificacionesCadencias oDato;

                    oDato = cController.GetItem(lID);
                    //Se editan los valores del elemento a editar

                    if (oDato.NotificacionCadencia == txtNotificacionCadencia.Text)
                    {
                        oDato.NotificacionCadencia = txtNotificacionCadencia.Text;

                        oDato.Lunes = chkLunes.Checked;
                        oDato.Martes = chkMartes.Checked;
                        oDato.Miercoles = chkMiercoles.Checked;
                        oDato.Jueves = chkJueves.Checked;
                        oDato.Viernes = chkViernes.Checked;
                        oDato.Sabado = chkSabado.Checked;
                        oDato.Domingo = chkDomingo.Checked;

                        if (txtDiaMes.Text.Equals(""))
                        {
                            oDato.DiaMes = null;
                        }
                        else
                        {
                            oDato.DiaMes = Convert.ToInt32(txtDiaMes.Text);
                        }

                        if (txtFechaEnvio.Text.Equals(""))
                        {
                            oDato.FechaEnvio = null;
                        }
                        else
                        {
                            oDato.FechaEnvio = txtFechaEnvio.SelectedDate;
                        }
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());
                        if (cController.RegistroDuplicado(txtNotificacionCadencia.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NotificacionCadencia = txtNotificacionCadencia.Text;

                            oDato.Lunes = chkLunes.Checked;
                            oDato.Martes = chkMartes.Checked;
                            oDato.Miercoles = chkMiercoles.Checked;
                            oDato.Jueves = chkJueves.Checked;
                            oDato.Viernes = chkViernes.Checked;
                            oDato.Sabado = chkSabado.Checked;
                            oDato.Domingo = chkDomingo.Checked;

                            if (txtDiaMes.Text.Equals(""))
                            {
                                oDato.DiaMes = null;
                            }
                            else
                            {
                                oDato.DiaMes = Convert.ToInt32(txtDiaMes.Text);
                            }

                            if (txtFechaEnvio.Text.Equals(""))
                            {
                                oDato.FechaEnvio = null;
                            }
                            else
                            {
                                oDato.FechaEnvio = txtFechaEnvio.SelectedDate;
                            }
                        }

                        //Actualización del elemento en base de datos
                        if (cController.UpdateItem(oDato))
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                        }
                    }
                }
                //En caso de tratarse de un nuevo elemento
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cController.RegistroDuplicado(txtNotificacionCadencia.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        //Se establecen los datos del nuevo elemento y se almacena
                        Data.NotificacionesCadencias oDato = new Data.NotificacionesCadencias();
                        //Se editan los valores del elemento a editar
                        oDato.NotificacionCadencia = txtNotificacionCadencia.Text;

                        oDato.Lunes = chkLunes.Checked;
                        oDato.Martes = chkMartes.Checked;
                        oDato.Miercoles = chkMiercoles.Checked;
                        oDato.Jueves = chkJueves.Checked;
                        oDato.Viernes = chkViernes.Checked;
                        oDato.Sabado = chkSabado.Checked;
                        oDato.Domingo = chkDomingo.Checked;

                        if (txtDiaMes.Text.Equals(""))
                        {
                            oDato.DiaMes = null;
                        }
                        else
                        {
                            oDato.DiaMes = Convert.ToInt32(txtDiaMes.Text);
                        }

                        if (!txtFechaEnvio.Text.Equals("") && txtDiaMes.Text != null && Convert.ToDateTime(txtFechaEnvio.Text) != DateTime.MinValue)
                        {
                            oDato.FechaEnvio = txtFechaEnvio.SelectedDate;
                        }
                        else
                        {
                            oDato.FechaEnvio = null;
                        }

                        oDato.Activo = true;

                        oDato.ClienteID = lCliID;

                        if (cController.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                    }
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
                //Se recupera el elemento a mostrar
                long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.NotificacionesCadencias oDato;
                NotificacionesCadenciasController cController = new NotificacionesCadenciasController();

                oDato = cController.GetItem(lID);
                //Se recuperan los valores del elemento
                txtNotificacionCadencia.Text = oDato.NotificacionCadencia;
                chkLunes.Checked = oDato.Lunes;
                chkMartes.Checked = oDato.Martes;
                chkMiercoles.Checked = oDato.Miercoles;
                chkJueves.Checked = oDato.Jueves;
                chkViernes.Checked = oDato.Viernes;
                chkSabado.Checked = oDato.Sabado;
                chkDomingo.Checked = oDato.Domingo;

                if (oDato.FechaEnvio != null)
                {
                    txtFechaEnvio.SelectedDate = (DateTime)oDato.FechaEnvio;
                }
                if (oDato.DiaMes != null)
                {
                    txtDiaMes.Text = oDato.DiaMes.ToString();
                }

                //Se muestra la ventana con los datos
                winGestion.Show();
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesCadenciasController cNotificacionesCadencias = new NotificacionesCadenciasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.NotificacionesCadencias oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cNotificacionesCadencias.GetDefault(Convert.ToInt32(ClienteID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.NotificacionCadenciaID != lID)
                    {
                        if (oDato.Defecto)
                        {
                            oDato.Defecto = !oDato.Defecto;
                            cNotificacionesCadencias.UpdateItem(oDato);
                        }

                        oDato = cNotificacionesCadencias.GetItem(lID);
                        oDato.Defecto = true;
                        oDato.Activo = true;
                        cNotificacionesCadencias.UpdateItem(oDato);
                    }
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cNotificacionesCadencias.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cNotificacionesCadencias.UpdateItem(oDato);
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
            NotificacionesCadenciasController cNotificacionesCadencias = new NotificacionesCadenciasController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cNotificacionesCadencias.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cNotificacionesCadencias.DeleteItem(lID))
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
            NotificacionesCadenciasController cController = new NotificacionesCadenciasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.NotificacionesCadencias oDato;
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