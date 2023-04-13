using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace TreeCore.ModGlobal
{
    public partial class Conexiones : TreeCore.Page.BasePageExtNet
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
                        List<Data.Vw_ToolConexionesIntegraciones> listaDatos;
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar }},
                { "Post", new List<ComponentBase> { btnAnadir }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnDefecto }},
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

        private List<Data.Vw_ToolConexionesIntegraciones> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_ToolConexionesIntegraciones> listaDatos;
            ToolConexionesController CToolConexionesIntegraciones = new ToolConexionesController();

            try
            {
                if (lClienteID.HasValue)
                {
                    listaDatos = CToolConexionesIntegraciones.GetItemsWithExtNetFilterList<Data.Vw_ToolConexionesIntegraciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        #region INTEGRACIONES
        protected void storeIntegracion_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                IntegracionesController cInte = new IntegracionesController();

                try
                {
                    List<Data.ToolIntegraciones> lista;
                    lista = cInte.GetActivos();

                    if (lista != null)
                    {
                        storeIntegracion.DataSource = lista;
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

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();
            ToolConexionesController cConex = new ToolConexionesController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);

                    Data.ToolConexiones oDato;
                    oDato = cConex.GetItem(lS);

                    if (oDato.IntegracionID == Convert.ToInt32(cmbIntegracion.SelectedItem.Value))
                    {
                        oDato.Servidor = txtServidor.Text;
                        oDato.Usuario = txtUsuario.Text;
                        oDato.Clave = Util.EncryptKey(txtClave.Text);
                        oDato.IntegracionID = long.Parse(cmbIntegracion.SelectedItem.Value);
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cConex.RegistroDuplicado(txtServidor.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }

                        oDato.Servidor = txtServidor.Text;
                        oDato.Usuario = txtUsuario.Text;
                        oDato.Clave = Util.EncryptKey(txtClave.Text);
                        oDato.IntegracionID = Convert.ToInt32(cmbIntegracion.SelectedItem.Value);

                    }
                    if (cConex.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cConex.RegistroDuplicado(txtServidor.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ToolConexiones oDato = new Data.ToolConexiones();

                        oDato.ClienteID = Convert.ToInt32(cmbClientes.SelectedItem.Value);
                        oDato.Servidor = txtServidor.Text;
                        oDato.Usuario = txtUsuario.Text;
                        oDato.Clave = Util.EncryptKey(txtClave.Text);
                        oDato.IntegracionID = long.Parse(cmbIntegracion.SelectedItem.Value);
                        oDato.Activo = true;

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = lCliID;
                        }

                        if (cConex.AddItem(oDato) != null)
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
            ToolConexionesController cConex = new ToolConexionesController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolConexiones oDato;
                oDato = cConex.GetItem(lS);

                txtServidor.Text = oDato.Servidor;
                txtUsuario.Text = oDato.Usuario;
                txtClave.Text = Util.DecryptKey(oDato.Clave);
                cmbIntegracion.SetValue(oDato.IntegracionID);

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
            ToolConexionesController cToolConexiones = new ToolConexionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.ToolConexiones oDato;
                long lCliID = 0;

                if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                {
                    lCliID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                }
                else if (ClienteID.HasValue)
                {
                    lCliID = Convert.ToInt32(ClienteID);
                }

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cToolConexiones.GetDefault(lCliID);

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cToolConexiones.UpdateItem(oDato);
                    }

                    oDato = cToolConexiones.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cToolConexiones.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cToolConexiones.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cToolConexiones.UpdateItem(oDato);
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
            ToolConexionesController cToolConexionesIntegraciones = new ToolConexionesController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cToolConexionesIntegraciones.RegistroDefecto(lID))
                {
                    direct.Result = GetGlobalResource(Comun.jsPorDefecto);
                    direct.Success = false;
                }
                else if (cToolConexionesIntegraciones.DeleteItem(lID))
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
            ToolConexionesController cController = new ToolConexionesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.ToolConexiones oDato = cController.GetItem(lID);
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
        #region ESTRUCTURA

        [DirectMethod(Timeout = 90000)]
        public DirectResponse GenerarEstructura()
        {
            DirectResponse direct = new DirectResponse();

            ToolConexionesController cController = new ToolConexionesController();
            IntegracionesController cIntegra = new IntegracionesController();
            Data.ToolConexiones dato = null;
            Data.ToolIntegraciones integra = null;
            string sEstructura = null;

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                dato = new Data.ToolConexiones();
                dato = cController.GetItem(S);
                integra = cIntegra.GetItem(dato.IntegracionID);

                direct.Success = true;
                direct.Result = "";

                switch (integra.Codigo)
                {
                    case Comun.REPORTING_SOURCE_TREE:
                        sEstructura = GeneraTree(dato);
                        if (sEstructura != null)
                        {
                            dato.Estructura = sEstructura;
                            if (cController.UpdateItem(dato))
                            {
                                log.Info(GetGlobalResource(Comun.LogEstructuraActualizada));
                                direct.Result = GetGlobalResource(Comun.LogEstructuraActualizada);
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        }
                        break;
                    case Comun.REPORTING_SOURCE_SQL_SERVER:
                        sEstructura = GeneraSQLServer(dato);
                        if (sEstructura != null)
                        {
                            dato.Estructura = sEstructura;
                            if (cController.UpdateItem(dato))
                            {
                                log.Info(GetGlobalResource(Comun.LogEstructuraActualizada));
                                direct.Result = GetGlobalResource(Comun.LogEstructuraActualizada);
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        }
                        break;
                    case Comun.REPORTING_SOURCE_EXCEL:
                        sEstructura = GeneraExcel(dato);
                        if (sEstructura != null)
                        {
                            dato.Estructura = sEstructura;
                            if (cController.UpdateItem(dato))
                            {
                                log.Info(GetGlobalResource(Comun.LogEstructuraActualizada));
                                direct.Result = GetGlobalResource(Comun.LogEstructuraActualizada);
                            }
                            else
                            {
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            }
                        }
                        else
                        {
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        }
                        break;
                    case Comun.REPORTING_SOURCE_MYSQL:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                    case Comun.REPORTING_SOURCE_ORACLE:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                    case Comun.REPORTING_SOURCE_DB2:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                    case Comun.REPORTING_SOURCE_POSTGRE:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                    case Comun.REPORTING_SOURCE_CSV:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                    default:
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        break;
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            cController = null;
            return direct;
        }

        #region TREE
        private string GeneraTree(Data.ToolConexiones conexion)
        {
            // Local variables
            string sResultado = null;
            try
            {
                string sConexion = Properties.Settings.Default.Conexion;
                sConexion = ServerConnectionChooser.getServerString(sConexion);
                SQLServerModelController cController = new SQLServerModelController(sConexion);
                sResultado = cController.GetItemsArbolString(null, conexion.NombreConexion);
                cController = null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sResultado = "";
            }
            return sResultado;
        }

        #endregion

        #region SQL SERVER

        private string GeneraSQLServer(Data.ToolConexiones conexion)
        {
            // Local variables
            string sResultado = null;
            try
            {
                string sConexion = "";

                //Data Source = 192.168.21.209; Initial Catalog = Sites; Persist Security Info = True; User ID = ahp; Password = Sql.2021

                //sConexion = Properties.Settings.Default.Conexion;
                //sConexion = ServerConnectionChooser.getServerString(sConexion);
                sConexion = "Data Source =" + conexion.Servidor + ";Initial Catalog = " + conexion.NombreBBDD + ";Persist Security Info = True; User ID =" + conexion.Usuario + ";Password = " + Util.DecryptKey(conexion.Clave);
                SQLServerModelController cController = new SQLServerModelController(sConexion);
                sResultado = cController.GetItemsArbolString(null, conexion.NombreConexion);
                cController = null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sResultado = "";
            }
            return sResultado;
        }

        #endregion

        #region EXCEL

        private string GeneraExcel(Data.ToolConexiones conexion)
        {
            // Local variables
            string sResultado = null;
            try
            {

                ExcelController cController = new ExcelController(conexion.RutaFichero, conexion.Extension);
                sResultado = cController.GetItemsArbolString(null);
                cController = null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sResultado = "";
            }
            return sResultado;
        }


        #endregion

        #endregion

        #endregion

        #region FUNCTIONS

        #endregion
    }
}