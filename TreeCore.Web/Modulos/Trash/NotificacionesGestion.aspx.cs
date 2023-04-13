using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using TreeCore.Data;

namespace TreeCore.ModGlobal
{
    public partial class NotificacionesGestion : TreeCore.Page.BasePageExtNet
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

                if (!ClienteID.HasValue)
                {
                    cmbClientes.Hidden = false;
                }

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
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        string sModuloID = Request.QueryString["aux"].ToString();
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.Vw_Notificaciones> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

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
                            List<Data.Vw_NotificacionesCorreos> ListaDatosDetalle = null;

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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_NOTIFICACIONES_GESTION))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_NOTIFICACIONES_GESTION))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = false;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
            }
        }

        #endregion

        #region STORES

        #region MAESTRO

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

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, long.Parse(hdCliID.Value.ToString()));

                    if (vLista != null)
                    {
                        storePrincipal.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storePrincipal.Proxy[0];
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

        private List<Data.Vw_Notificaciones> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_Notificaciones> ListaDatos;
            NotificacionesController CNotificaciones = new NotificacionesController();

            try
            {
                if (ClienteID.HasValue)
                {
                    ListaDatos = CNotificaciones.GetItemsWithExtNetFilterList<Data.Vw_Notificaciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
                }
                else
                {
                    ListaDatos = null;
                }
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

                    if (!hdModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(hdModuloID.Value);
                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lMaestroID);
                    if (vLista != null)
                    {
                        storeDetalle.DataSource = vLista;

                        PageProxy temp;
                        temp = (PageProxy)storeDetalle.Proxy[0];
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

        private List<Data.Vw_NotificacionesCorreos> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_NotificacionesCorreos> ListaDatos;

            try
            {
                NotificacionesCorreosController CNotificacionesCorreos = new NotificacionesCorreosController();

                ListaDatos = CNotificacionesCorreos.GetItemsWithExtNetFilterList<Data.Vw_NotificacionesCorreos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "NotificacionID == " + lMaestroID);

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                ListaDatos = null;
            }

            return ListaDatos;
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

        #region PERFILES
        protected void storePerfiles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir;
                    //Recupera los parámetros para obtener los datos de la grilla
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFilters"];

                    //Recupera los datos y los establece
                    var ls = ListaPerfiles(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);
                    if (ls != null)
                    {
                        storePerfiles.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storePerfiles.Proxy[0];
                        temp.Total = iCount;
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



        /// <summary>
        /// Método que se usa para cargar los perfiles
        /// </summary>
        /// <param name="iStart">Índice del primer elemento de la grilla</param>
        /// <param name="iLimit">Número de elementos a recuperar</param>
        /// <param name="sSort">Columna por la que se desean ordenar los datos</param>
        /// <param name="sDir">Dirección de la ordenación de los datos</param>
        /// <param name="iCount">Número de datos que se recuperan</param>
        /// <param name="sFiltro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Data.Perfiles> ListaPerfiles(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Perfiles> listaDatos = null;
            try
            {
                PerfilesController cPerfiles = new PerfilesController();
                if (cmbProyectosTipos.SelectedItem.Value != null && cmbProyectosTipos.SelectedItem.Value != "")
                {
                    listaDatos = cPerfiles.GetAllPerfilesByTipoProyectoID(Convert.ToInt32(cmbProyectosTipos.SelectedItem.Value.ToString()));
                }
                else
                {
                    listaDatos = new List<TreeCore.Data.Perfiles>();
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                listaDatos = null;
            }
            return listaDatos;
        }

        private void RefreshStorePerfiles()
        {
            storePerfiles.DataBind();
        }

        #endregion

        #region PROYECTOS TIPOS
        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

                try
                {
                    List<Data.ProyectosTipos> listaProyectosTipos = cProyectosTipos.GetListaOrderByProyectoTipo();
                    if (listaProyectosTipos != null)
                    {
                        storeProyectosTipos.DataSource = listaProyectosTipos;
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

        /// <summary>
        /// Método que se usa para cargar los proyectos tipos
        /// </summary>
        /// <param name="iStart">Índice del primer elemento de la grilla</param>
        /// <param name="iLimit">Número de elementos a recuperar</param>
        /// <param name="sSort">Columna por la que se desean ordenar los datos</param>
        /// <param name="sDir">Dirección de la ordenación de los datos</param>
        /// <param name="iCount">Número de datos que se recuperan</param>
        /// <param name="sFiltro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Data.ProyectosTipos> ListaProyectosTipos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.ProyectosTipos> lista = null;
            ProyectosTiposController cTipo = new ProyectosTiposController();

            try
            {
                lista = cTipo.GetItemList();
            }
            catch (Exception ex)
            {
                lista = new List<TreeCore.Data.ProyectosTipos>();
                log.Error(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }
            return lista;
        }

        private void RefreshStoreCamposAsociados()
        {
            storeProyectosTipos.DataBind();
        }

        #endregion

        #region NOTIFICACIONES CADENCIAS
        protected void storeNotificacionesCadencias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    NotificacionesCadenciasController cNotificacionesCadencias = new NotificacionesCadenciasController();

                    var listaNotificacionesCadencias = cNotificacionesCadencias.GetItemList();
                    if (listaNotificacionesCadencias != null)
                    {
                        storeNotificacionesCadencias.DataSource = listaNotificacionesCadencias;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    Response.Write("ERROR: " + codTit + "<br>" + Comun.ERRORAJAXGENERICO);
                }
            }
        }
        #endregion

        #region GRUPOS
        protected void storeNotificacionesGruposCriterios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    NotificacionesGruposCriteriosController cNotificacionesGruposCriterios = new NotificacionesGruposCriteriosController();

                    var listaNotificacionesGruposCriterios = cNotificacionesGruposCriterios.GetItemList();
                    if (listaNotificacionesGruposCriterios != null)
                        storeNotificacionesGruposCriterios.DataSource = listaNotificacionesGruposCriterios;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    Response.Write("ERROR: " + codTit + "<br>" + Comun.ERRORAJAXGENERICO);
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
            NotificacionesController cNotificaciones = new NotificacionesController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    Data.Notificaciones oDato = null;

                    oDato = cNotificaciones.GetItem(lID);


                    if (oDato.Notificacion == txtNotificacion.Text)
                    {
                        oDato.Notificacion = txtNotificacion.Text;
                        oDato.Asunto = txtAsunto.Text;
                        oDato.Contenido = txaCuerpo.Text;
                        oDato.NotificacionCadenciaID = Convert.ToInt32(cmbCadencias.SelectedItem.Value);
                        oDato.NotificacionGrupoID = Convert.ToInt32(cmbGrupo.SelectedItem.Value);
                        oDato.FechaActivacion = txtFechaActivacion.SelectedDate;
                        if (txtFechaDesactivacion.SelectedDate != DateTime.MinValue)
                        {
                            oDato.FechaDesactivacion = txtFechaDesactivacion.SelectedDate;
                        }
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());
                        if (cNotificaciones.RegistroDuplicado(txtNotificacion.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Notificacion = txtNotificacion.Text;
                            oDato.Asunto = txtAsunto.Text;
                            oDato.Contenido = txaCuerpo.Text;
                            oDato.NotificacionCadenciaID = Convert.ToInt32(cmbCadencias.SelectedItem.Value);
                            oDato.NotificacionGrupoID = Convert.ToInt32(cmbGrupo.SelectedItem.Value);
                            oDato.FechaActivacion = txtFechaActivacion.SelectedDate;
                            if (txtFechaDesactivacion.SelectedDate != DateTime.MinValue)
                            {
                                oDato.FechaDesactivacion = txtFechaDesactivacion.SelectedDate;
                            }
                        }
                    }

                    if (cNotificaciones.UpdateItem(oDato))
                    {
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cNotificaciones.RegistroDuplicado(txtNotificacion.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.Notificaciones oDato = new Data.Notificaciones
                        {
                            Notificacion = txtNotificacion.Text,
                            Asunto = txtAsunto.Text,
                            Contenido = txaCuerpo.Text,
                            NotificacionCadenciaID = Convert.ToInt32(cmbCadencias.SelectedItem.Value),
                            NotificacionGrupoID = Convert.ToInt32(cmbGrupo.SelectedItem.Value),
                            FechaActivacion = txtFechaActivacion.SelectedDate,
                            FechaCreacion = DateTime.Now,
                            CreadorID = Usuario.UsuarioID,
                            Activo = true
                        };
                        if (txtFechaDesactivacion.SelectedDate != DateTime.MinValue)
                        {
                            oDato.FechaDesactivacion = txtFechaDesactivacion.SelectedDate;
                        }

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = lCliID;
                        }

                        if (cNotificaciones.AddItem(oDato) != null)
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
                long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.Notificaciones oDato;
                NotificacionesController cNotificaciones = new NotificacionesController();
                oDato = cNotificaciones.GetItem(lID);
                txtNotificacion.Text = oDato.Notificacion;
                cmbCadencias.SetValue(oDato.NotificacionCadenciaID.ToString());
                cmbGrupo.SetValue(oDato.NotificacionGrupoID.ToString());
                txtAsunto.Text = oDato.Asunto;
                txaCuerpo.Text = oDato.Contenido;
                txtFechaActivacion.SelectedDate = oDato.FechaActivacion;
                if (oDato.FechaDesactivacion != null)
                {
                    txtFechaDesactivacion.SelectedDate = oDato.FechaDesactivacion.Value;
                }

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
            NotificacionesController cNotificaciones = new NotificacionesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cNotificaciones.DeleteItem(lID))
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
            NotificacionesController cNotificaciones = new NotificacionesController();
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.Notificaciones oDato;
                oDato = cNotificaciones.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cNotificaciones.UpdateItem(oDato))
                {
                    storePrincipal.DataBind();
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
        #region ENVIAR NOTIFICACIONES

        /// <summary>
        /// TEST DE ENVÍO DE NOTIFICACIONES
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public DirectResponse EnviarNotificaciones()
        {
            DirectResponse ajax = new DirectResponse();
            NotificacionesController cNotificaciones = new NotificacionesController();
            NotificacionesCorreosController cNotificacionesCorreos = new NotificacionesCorreosController();
            string sListaDirecciones = null;
            string sResultado = null;
            ajax.Result = "";
            ajax.Success = true;

            try
            {
                List<Vw_Notificaciones> lista = null;
                lista = cNotificaciones.GetListaNotificacionesAEnviar(DateTime.Now);
                long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.Notificaciones oDato;
                Data.Vw_Notificaciones oNotifica;

                oDato = cNotificaciones.GetItem(lID);
                oNotifica = cNotificaciones.GetItem<Data.Vw_Notificaciones>(lID);
                // Sends the email for each notification
                if (lista != null)
                {
                    sListaDirecciones = cNotificacionesCorreos.GetCadenaDireccionesCorreosByNotificacion(lID);
                    sResultado = GetConsulta(oNotifica);
                    string sRespuestaNotificacion = "";
                    //sRespuestaNotificacion = Comun.EnviarNotificacion("", sListaDirecciones, oDato.Asunto, oNotifica.Contenido + sResultado);
                    log.Info(sRespuestaNotificacion);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
            }
            return ajax;
        }

        #endregion


        #endregion

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long lModuloID)
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };

            try
            {
                storeDetalle.DataBind();
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
        public DirectResponse AgregarEditarDetalle(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            long lCliID = 0;
            try
            {
                long lNotificacionID = Int64.Parse(GridRowSelect.SelectedRecordID);
                NotificacionesCorreosController cNotificacionesCorreos = new NotificacionesCorreosController();
                if (!bAgregar)
                {
                    long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.NotificacionesCorreos oDato;


                    oDato = cNotificacionesCorreos.GetItem(ID);

                    if (oDato.Correo == txtCorreo.Text)
                    {
                        oDato.Correo = txtCorreo.Text;
                        oDato.PerfilID = Int64.Parse(cmbPerfiles.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());
                        if (cNotificacionesCorreos.RegistroDuplicado(txtCorreo.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Correo = txtCorreo.Text;
                            oDato.PerfilID = Int64.Parse(cmbPerfiles.SelectedItem.Value.ToString());
                        }
                    }
                    if (cNotificacionesCorreos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cNotificacionesCorreos.RegistroDuplicado(txtCorreo.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.NotificacionesCorreos dato = new Data.NotificacionesCorreos
                        {
                            Correo = txtCorreo.Text,
                            PerfilID = Int64.Parse(cmbPerfiles.SelectedItem.Value.ToString()),
                            Activo = true,
                            NotificacionID = lNotificacionID
                        };
                        if (cNotificacionesCorreos.AddItem(dato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));

                        }

                    }

                }
                storeDetalle.DataBind();
            }
            catch (Exception ex)
            {
                ajax.Success = false;
                ajax.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return ajax;
            }

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.NotificacionesCorreos oDato;
                NotificacionesCorreosController cnotificacionesCorreos = new NotificacionesCorreosController();
                oDato = cnotificacionesCorreos.GetItem(lID);
                txtCorreo.Text = oDato.Correo;
                if (oDato.PerfilID != null)
                {
                    PerfilesController cPerfiles = new PerfilesController();
                    Data.Perfiles perfilLocal = cPerfiles.GetItem((long)oDato.PerfilID);
                    ProyectosTiposController cTiposProyectos = new ProyectosTiposController();
                    if (perfilLocal.TipoProyectoID != null)
                    {
                        Data.ProyectosTipos proTipo = cTiposProyectos.GetItem((long)perfilLocal.TipoProyectoID);
                        cmbProyectosTipos.SetValue(proTipo.ProyectoTipoID.ToString());
                    }
                    if (perfilLocal.PerfilID.ToString() != null)
                    {
                        cmbPerfiles.Value = perfilLocal.Perfil_esES;
                    }
                }

                winGestionDetalle.Show();
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };

            NotificacionesCorreosController cNotificacionesCorreos = new NotificacionesCorreosController();


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (cNotificacionesCorreos.DeleteItem(lID))
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
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesCorreosController cNotificacionesCorreos = new NotificacionesCorreosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.NotificacionesCorreos oDato;
                oDato = cNotificacionesCorreos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cNotificacionesCorreos.UpdateItem(oDato))
                {
                    storeDetalle.DataBind();
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
        ///
        ///
        /// Obtains the result from invoking the consult
        ///
        ///
        public string GetConsulta(Vw_Notificaciones oNotifica)
        {
            // Local variables
            string sResultado = null;
            string sFiltro = null;
            NotificacionesGruposCriteriosController cGrupo = new NotificacionesGruposCriteriosController();
            NotificacionesGruposCriterios oGrupo = null;
            NotificacionesCriteriosController cCriterio = new NotificacionesCriteriosController();
            List<Data.NotificacionesCriterios> listaCriterios = null;
            Data.TiposDatos oTipoDato = null;
            TiposDatosController cTipodato = null;
            string sValor = null;

            try
            {
                if (oNotifica.NotificacionGrupoID != null)
                {
                    oGrupo = cGrupo.GetItem((long)oNotifica.NotificacionGrupoID);
                    listaCriterios = cCriterio.GetAllActiveCriteria(oGrupo.NotificacionGrupoCriterioID);

                    // Searches for the data type
                    cTipodato = new TiposDatosController();
                    oTipoDato = cTipodato.GetTipoDatosByNombre(Comun.TiposDatos.Fecha);
                    // Creates the filter
                    sFiltro = "";
                    foreach (Data.NotificacionesCriterios oCriterio in listaCriterios)
                    {

                        sFiltro = sFiltro + oCriterio.Campo + "!= null && ";
                        if (oCriterio.TipoDatoID == oTipoDato.TipoDatoID)
                        {
                            sValor = DateTime.Now.AddDays(Convert.ToInt32(oCriterio.Valor)).Ticks.ToString();
                            sValor = "Datetime(" + sValor + ")";
                        }
                        else
                        {
                            sValor = oCriterio.Valor;
                        }
                        sFiltro = sFiltro + oCriterio.Campo + oCriterio.Operador + sValor;
                        sFiltro = sFiltro + " && ";

                    }
                }
                sFiltro = sFiltro.Substring(0, sFiltro.Length - 3);


                string sControlador = oNotifica.Tabla.Substring(4) + "Controller";
                string sTabla = oNotifica.Tabla.Substring(4);
                // Realizar el tratamiento para las tablas de InfraestructurasGLOBAL --->> Emplazamientos
                // Las clases de Infraestructuras se llaman "EmplazamientosAntenas"
                // Sin embargo el controlador se llama "GlobalAntenasController"

                if (sTabla.Substring(0, 6) == "Global")
                {
                    sTabla = sTabla.Substring(6, sTabla.Length - 6);
                    sTabla = "Emplazamientos" + sTabla;

                }

                Type[] types = Assembly.GetExecutingAssembly().GetTypes();

                Type clase = null;
                Type controller = null;

                foreach (Type t in types)
                {
                    if (t.Name == sTabla)
                    {
                        clase = t;
                    }
                    else if (t.Name == sControlador)
                    {
                        controller = t;
                    }

                }

                ConstructorInfo constuctorController = controller.GetConstructor(Type.EmptyTypes);
                object objetoConstructor = constuctorController.Invoke(new object[] { });

                ConstructorInfo constuctorClase = clase.GetConstructor(Type.EmptyTypes);
                object objetoClase = constuctorClase.Invoke(new object[] { });
                string sVisualiza = oNotifica.CamposVisualizar;

                MethodInfo m = controller.GetMethod("GetListaByFiltro");
                Object invocacion = null;
                if (m != null)
                {
                    invocacion = m.Invoke(objetoConstructor, new object[] { sFiltro, sVisualiza });
                }
                else
                {
                    invocacion = "";
                }
                sResultado = (string)invocacion;
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                sResultado = "";
            }

            // Returns the result
            return sResultado;
        }
        #endregion
    }
}


