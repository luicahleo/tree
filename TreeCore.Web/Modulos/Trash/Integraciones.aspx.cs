using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using System.Data.SqlClient;
using log4net;
using TreeCore.Data;
using System.Linq;
namespace TreeCore.ModGlobal
{
    public partial class Integraciones : TreeCore.Page.BasePageExtNet
    {
        public ILog log = LogManager.GetLogger("");
        public List<Data.Vw_Funcionalidades> ListaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        long lMaestroID = 0;
        bool agregarDetalle = true;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
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
                        long lCliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.ToolIntegraciones> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lCliID);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaDatos, Response, "", paginaJS, _Locale);
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
                            List<Data.Vw_ToolIntegracionesServiciosMetodos> ListaDatosDetalle = null;
                            ListaDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sModuloID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaDatosDetalle, Response, "", paginaJS, _Locale);
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

        private List<Data.ToolIntegraciones> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.ToolIntegraciones> ListaDatos;
            ToolIntegracionesController CToolIntegraciones = new ToolIntegracionesController();

            try
            {
                if (ClienteID.HasValue)
                {
                    ListaDatos = CToolIntegraciones.GetItemsWithExtNetFilterList<Data.ToolIntegraciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        private List<Vw_ToolIntegracionesServiciosMetodos> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_ToolIntegracionesServiciosMetodos> ListaDatos = new List<Vw_ToolIntegracionesServiciosMetodos>();
            ToolIntegracionesServiciosMetodosController CToolIntegracionesServiciosMetodos = new ToolIntegracionesServiciosMetodosController();

            try
            {
                ListaDatos = CToolIntegracionesServiciosMetodos.GetItemsWithExtNetFilterList<Data.Vw_ToolIntegracionesServiciosMetodos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "IntegracionID == " + lMaestroID);
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)

        {
            DirectResponse direct = new DirectResponse();

            IntegracionesController cIntegraciones = new IntegracionesController();

            long cliID = 0;

            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.ToolIntegraciones dato;
                    dato = cIntegraciones.GetItem(S);

                    if (dato.Codigo == txtCodigo.Text)
                    {
                        dato.Codigo = txtCodigo.Text;
                        dato.Integracion = txtIntegracion.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cIntegraciones.RegistroDuplicado(txtCodigo.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            dato.Codigo = txtCodigo.Text;
                            dato.Integracion = txtIntegracion.Text;
                        }
                    }

                    if (cIntegraciones.UpdateItem(dato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cIntegraciones.RegistroDuplicado(txtCodigo.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ToolIntegraciones dato = new Data.ToolIntegraciones();
                        dato.Codigo = txtCodigo.Text;
                        dato.Integracion = txtIntegracion.Text;
                        dato.Activo = true;
                        dato.ClienteID = cliID;

                        if (cIntegraciones.AddItem(dato) != null)
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

            IntegracionesController cIntegraciones = new IntegracionesController();


            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolIntegraciones dato = new Data.ToolIntegraciones();
                dato = cIntegraciones.GetItem(S);
                txtIntegracion.Text = dato.Integracion;
                txtCodigo.Text = dato.Codigo;

                winGestion.Show();
                cIntegraciones = null;

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                direct.Result = "";
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
            ToolIntegracionesController CToolIntegraciones = new ToolIntegracionesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CToolIntegraciones.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CToolIntegraciones = null;
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

        #region GESTION DETALLE

        [DirectMethod()]
        public DirectResponse AgregarEditarDetalle(bool agregar)
        {
            DirectResponse ajax = new DirectResponse();


            try

            {
                long S = Int64.Parse(GridRowSelect.SelectedRecordID);
                ToolIntegracionesServiciosMetodosController cMetPara = new ToolIntegracionesServiciosMetodosController();
                Data.ToolIntegracionesServiciosMetodos cMetodoPara = cMetPara.GetItem(S);
                if (!agregar)
                {
                    long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.ToolIntegracionesServiciosMetodos dato = null;
                    ToolIntegracionesServiciosMetodosController fController = new ToolIntegracionesServiciosMetodosController();
                    dato = fController.GetItem(ID);
                    dato.IntegracionID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                    dato.ServicioID = Convert.ToInt32(cmbServicio.SelectedItem.Value);
                    dato.MetodoID = Convert.ToInt32(cmbMetodo.SelectedItem.Value);

                    if (!fController.existeRegistro(dato.IntegracionID, dato.ServicioID, dato.MetodoID))
                    {
                        if (fController.UpdateItem(dato))
                        {
                            //storePrincipal.DataBind();
                            storeDetalle.DataBind();
                        }
                    }
                    else
                    {
                        /* Obtener traducciones de recursos definidos en Comun.resx */
                        string mensaje_recursoTraducido = Resources.Comun.ResourceManager.GetString("strExiste", new System.Globalization.CultureInfo(_Locale)); //la variable _Locale tiene el país actual
                        string titulo_recursoTraducido = Resources.Comun.ResourceManager.GetString("strTituloAtencion", new System.Globalization.CultureInfo(_Locale));
                        MensajeBox(titulo_recursoTraducido, mensaje_recursoTraducido, MessageBox.Icon.WARNING, null);
                    }

                    fController = null;
                }
                else
                {
                    Data.ToolIntegracionesServiciosMetodos dato = new Data.ToolIntegracionesServiciosMetodos();
                    ToolIntegracionesServiciosMetodosController fController = new ToolIntegracionesServiciosMetodosController();
                    IntegracionesController mController = new IntegracionesController();
                    ToolIntegraciones mDatos = new ToolIntegraciones();
                    dato.IntegracionID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                    dato.ServicioID = Convert.ToInt32(cmbServicio.SelectedItem.Value);
                    dato.MetodoID = Convert.ToInt32(cmbMetodo.SelectedItem.Value);
                    dato.Activo = true;

                    if (!fController.existeRegistro(dato.IntegracionID, dato.ServicioID, dato.MetodoID))
                    {
                        fController.AddItem(dato);
                        storeDetalle.DataBind();
                    }
                    else
                    {
                        /* Obtener traducciones de recursos definidos en Comun.resx */
                        string mensaje_recursoTraducido = Resources.Comun.ResourceManager.GetString("strExiste", new System.Globalization.CultureInfo(_Locale)); //la variable _Locale tiene el país actual
                        string titulo_recursoTraducido = Resources.Comun.ResourceManager.GetString("strTituloAtencion", new System.Globalization.CultureInfo(_Locale));
                        MensajeBox(titulo_recursoTraducido, mensaje_recursoTraducido, MessageBox.Icon.WARNING, null);
                    }
                    fController = null;

                }
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
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.ToolIntegracionesServiciosMetodos dato = new Data.ToolIntegracionesServiciosMetodos();

                ToolIntegracionesServiciosMetodosController fController = new ToolIntegracionesServiciosMetodosController();
                dato = fController.GetItem(ID);
                cmbServicio.SetValue(dato.ServicioID.ToString());
                if (cmbServicio.Value != null)
                {
                    storeMetodos.Reload();
                }
                string metodo = fController.getMetodo(dato.MetodoID);
                cmbMetodo.SetValue(metodo);
                //   storeServicios.DataBind();
                //cmbCamposAsociados.Value = dato.Campo.ToString();
                winGestionDetalle.Show();
                fController = null;
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
            DirectResponse direct = new DirectResponse();
            ToolIntegracionesServiciosMetodosController CToolIntegracionesServiciosMetodos = new ToolIntegracionesServiciosMetodosController();

            direct.Result = "";
            direct.Success = true;


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (CToolIntegracionesServiciosMetodos.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CToolIntegracionesServiciosMetodos = null;
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
            IntegracionesController cController = new IntegracionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolIntegraciones oDato;
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



        [DirectMethod()]
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            ToolIntegracionesServiciosMetodosController CToolIntegracionesServiciosMetodos = new ToolIntegracionesServiciosMetodosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.ToolIntegracionesServiciosMetodos oDato = CToolIntegracionesServiciosMetodos.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CToolIntegracionesServiciosMetodos.UpdateItem(oDato))
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

        protected void storeServicios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    sort = e.Sort[0].Property.ToString();
                    dir = e.Sort[0].Direction.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];



                    ToolIntegracionesServiciosMetodos inteServMet = new ToolIntegracionesServiciosMetodos();
                    ToolIntegracionesServiciosMetodosController cInteServMet = new ToolIntegracionesServiciosMetodosController();

                    ToolServiciosController mControl = new ToolServiciosController();
                    List<Data.ToolServicios> datos;

                    if (hdAgregar.Value != null && hdAgregar.Value.ToString() != "")
                        agregarDetalle = bool.Parse(hdAgregar.Value.ToString());

                    //Recupera los datos y los establece
                    if (GridRowSelectDetalle.SelectedRecordID != "")
                        inteServMet = cInteServMet.GetItem(Int64.Parse(GridRowSelectDetalle.SelectedRecordID));

                    if (cmbClientes.SelectedItem.Value != "" && cmbClientes.SelectedItem.Value != null)
                    {

                        datos = mControl.GetItemsWithExtNetFilterList<Data.ToolServicios>(filtro, sort, dir, e.Start, e.Limit, ref count, "Activo == True && ClienteID == " + cmbClientes.SelectedItem.Value);
                    }
                    else
                    {

                        //ListaServiciosLibres(Convert.ToInt64(GridRowSelect.SelectedRecordID), inteServMet.ServicioID, agregarDetalle);
                        datos = mControl.GetItemsWithExtNetFilterList<Data.ToolServicios>(filtro, sort, dir, e.Start, e.Limit, ref count, "Activo == True && ClienteID == " + Usuario.ClienteID);
                    }


                    if (datos != null)
                        storeServicios.DataSource = datos;

                }
                catch (Exception ex)
                {
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        protected void storeMetodos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    //Recupera los parámetros para obtener los datos de la grilla
                    sort = e.Sort[0].Property.ToString();
                    dir = e.Sort[0].Direction.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters"];

                    //Recupera los datos y los establece
                    var ls = ListaMetodos(e.Start, e.Limit, sort, dir, ref count, filtro);
                    if (ls != null)
                    {
                        storeMetodos.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeServicios.Proxy[0];
                        temp.Total = count;
                    }
                }
                catch (Exception ex)
                {
                    string codTit = "";
                    codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }
        private List<Data.ToolMetodos> ListaMetodos(int start, int limit, string sort, string dir, ref int count, string filtro)
        {
            List<Data.ToolMetodos> lista_Metodos = new List<ToolMetodos>();
            List<Data.ToolMetodos> lista_MetodosAux = new List<ToolMetodos>();
            try
            {
                if (cmbServicio.Value != null)
                {
                    //Obtener la lista de Métodos
                    ToolMetodosController cToolMetodos = new ToolMetodosController();
                    lista_MetodosAux = cToolMetodos.GetItemsWithExtNetFilterList<Data.ToolMetodos>(filtro, sort, dir, start, limit, ref count, "Activo == True");

                    //Quitar de la lista de Metodos obtenida anteriormente los que ya han sido agregados para el servicio seleccionado
                    long pIntegracionID = long.Parse(GridRowSelect.SelectedRecordID);
                    List<ToolIntegracionesServiciosMetodos> lista_IntegracionesServiciosMetodos = new List<ToolIntegracionesServiciosMetodos>();
                    ToolIntegracionesServiciosMetodosController cToolIntegracionesServiciosMetodos = new ToolIntegracionesServiciosMetodosController();
                    lista_IntegracionesServiciosMetodos = cToolIntegracionesServiciosMetodos.GetItemsList("Activo == True && IntegracionID ==" + pIntegracionID.ToString() + " && ServicioID ==" + cmbServicio.SelectedItem.Value.ToString());
                    foreach (var item_servicio in lista_IntegracionesServiciosMetodos)
                    {
                        for (int i = 0; i < lista_MetodosAux.Count; i++)
                        {
                            if (item_servicio.MetodoID == lista_MetodosAux[i].MetodoID)
                            {
                                lista_MetodosAux.RemoveAt(i);
                                i--;
                            }
                        }
                    }

                    lista_Metodos = lista_MetodosAux;
                }
            }
            catch (Exception ex)
            {
                string codTit = "";
                codTit = Util.ExceptionHandler(ex);
                MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                lista_Metodos = null;
            }

            return lista_Metodos;
        }


        #endregion

    }
}