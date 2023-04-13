using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace TreeCore.ModGlobal
{
    public partial class Limites : TreeCore.Page.BasePageExtNet
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
                    hdCliID.Value = 0;
                    cmbClientes.Hidden = false;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                #region FILTROS

                List<string> ListaIgnore = new List<string>()
                { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);

                List<string> ListaIgnoreDetalle = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

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
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.Vw_GlobalLimites> ListaDatos = null;
                            ListaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, CliID);

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
                            List<Data.Vw_GlobalLimitesCondiciones> ListaDatosDetalle = null;

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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_LIMITES))
            {
                btnAnadir.Hidden = true;
                btnEditar.Hidden = true;
                btnEliminar.Hidden = true;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = true;
                btnActivar.Hidden = true;
                btnDefecto.Hidden = true;

                btnAnadirDetalle.Hidden = true;
                btnEditarDetalle.Hidden = true;
                btnEliminarDetalle.Hidden = true;
                btnActivarDetalle.Hidden = true;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = true;
                btnDefectoDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_LIMITES))
            {
                btnAnadir.Hidden = false;
                btnEditar.Hidden = false;
                btnEliminar.Hidden = false;
                btnRefrescar.Hidden = false;
                btnDescargar.Hidden = false;
                btnActivar.Hidden = false;
                btnDefecto.Hidden = false;

                btnAnadirDetalle.Hidden = false;
                btnEditarDetalle.Hidden = false;
                btnEliminarDetalle.Hidden = false;
                btnActivarDetalle.Hidden = false;
                btnRefrescarDetalle.Hidden = false;
                btnDescargarDetalle.Hidden = false;
                btnDefectoDetalle.Hidden = false;
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
                    long lCliID = long.Parse(hdCliID.Value.ToString());

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lCliID);

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

        private List<Data.Vw_GlobalLimites> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lClienteID)
        {
            List<Data.Vw_GlobalLimites> ListaDatos;
            GlobalLimitesController CGlobalLimites = new GlobalLimitesController();

            try
            {
                if (lClienteID != 0)
                {
                    ListaDatos = CGlobalLimites.GetItemsWithExtNetFilterList<Data.Vw_GlobalLimites>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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
                    string sFiltro = e.Parameters["gridFiltersDetalle"];

                    if (!ModuloID.Value.Equals(""))
                    {
                        lMaestroID = Convert.ToInt64(ModuloID.Value);
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

        private List<Data.Vw_GlobalLimitesCondiciones> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_GlobalLimitesCondiciones> ListaDatos;

            try
            {
                GlobalLimitesCondicionesController CGlobalLimitesCondiciones = new GlobalLimitesCondicionesController();

                ListaDatos = CGlobalLimitesCondiciones.GetItemsWithExtNetFilterList<Data.Vw_GlobalLimitesCondiciones>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "GlobalLimiteID == " + lMaestroID);

            }

            catch (Exception ex)
            {
                ListaDatos = null;
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

        #region STORE TIPOS DATOS

        protected void storeTiposDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los datos y los establece

                    long lCliID = long.Parse(hdCliID.Value.ToString());
                    var ls = ListaTiposDatos(lCliID);
                    if (ls != null)
                    {
                        storeTiposDatos.DataSource = ls;
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



        /// <summary>
        /// Método que se usa para cargar los datos de la grilla
        /// </summary>
        /// <param name="start">Índice del primer elemento de la grilla</param>
        /// <param name="limit">Número de elementos a recuperar</param>
        /// <param name="sort">Columna por la que se desean ordenar los datos</param>
        /// <param name="dir">Dirección de la ordenación de los datos</param>
        /// <param name="count">Número de datos que se recuperan</param>
        /// <param name="filtro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Data.TiposDatos> ListaTiposDatos(long clienteID)
        {
            List<Data.TiposDatos> listadatos;
            try
            {
                TiposDatosController mControl = new TiposDatosController();
                listadatos = mControl.GetActivos(clienteID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        #endregion

        #region STORE TIPO CAMPO ASOCIADO

        protected void storeTipoCampoAsociado_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Object objeto = null;

                ParametrosController cParametros = new ParametrosController();
                List<Comun.Parametroslist> listaParamAux = new List<Comun.Parametroslist>();

                List<Object> listaParam = new List<object>();


                try
                {

                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(Data.TreeCoreContext));


                    listaParamAux = cParametros.ObtenerListaParametro("VISTA_LIMITES");

                    foreach (Comun.Parametroslist parametro in listaParamAux)
                    {
                        objeto = new { TipoCampoAsociadoID = parametro.Valor.ToString(), TipoCampoAsociado = parametro.Valor.ToString() };
                        listaParam.Add(objeto);
                    }

                    if (listaParam != null)
                    {
                        storeTipoCampoAsociado.DataSource = listaParam;
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

        #region STORE TIPO CAMPO ASOCIADO

        protected void storeTipoCampoAsociadoCondiciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                Object objeto = null;


                ParametrosController cParametros = new ParametrosController();
                List<Comun.Parametroslist> listaParamAux;

                List<Object> listaParam = new List<object>();

                try
                {

                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(Data.TreeCoreContext));

                    listaParamAux = cParametros.ObtenerListaParametro("VISTA_LIMITES");

                    foreach (Comun.Parametroslist parametro in listaParamAux)
                    {
                        objeto = new { TipoCampoAsociadoID = parametro.Valor.ToString(), TipoCampoAsociado = parametro.Valor.ToString() };
                        listaParam.Add(objeto);
                    }

                    if (listaParam != null)
                    {
                        storeTipoCampoAsociadoCondiciones.DataSource = listaParam;
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

        #region STORE CAMPOS ASOCIADOS


        protected void storeCamposAsociados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los datos y los establece
                    var ls = ListaCamposAsociados();
                    if (ls != null)
                    {
                        storeCamposAsociados.DataSource = ls;
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



        /// <summary>
        /// Método que se usa para cargar los Campos Asociados de la grilla
        /// </summary>
        /// <param name="start">Índice del primer elemento de la grilla</param>
        /// <param name="limit">Número de elementos a recuperar</param>
        /// <param name="sort">Columna por la que se desean ordenar los datos</param>
        /// <param name="dir">Dirección de la ordenación de los datos</param>
        /// <param name="count">Número de datos que se recuperan</param>
        /// <param name="filtro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Object> ListaCamposAsociados()
        {
            List<Object> lista = new List<object>();
            Object objeto = null;
            string sNombreTabla = null;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            int i = 1;
            GlobalLimitesController cLimite = new GlobalLimitesController();
            Data.GlobalLimites limiteLocal = null;
            bool bTabla = true;
            try
            {
                limiteLocal = cLimite.GetItem(Int64.Parse(GridRowSelect.SelectedRecordID));
                if (limiteLocal.Vista != null || !limiteLocal.Vista.Equals(""))
                {
                    sNombreTabla = limiteLocal.Vista;
                }
                else
                {
                    sNombreTabla = cmbTipoCampoAsociadoCondiciones.SelectedItem.Text;
                }

                // Modifies the table name to use the view name
                if (!sNombreTabla.Contains("vw_") && !sNombreTabla.Contains("Vw_"))
                {
                    sNombreTablaMayuscula = sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    sNombreTablaMinuscula = sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                    bTabla = true;
                }
                else
                {
                    sNombreTablaMayuscula = sNombreTabla.Replace("vw_", "Vw_");
                    sNombreTablaMinuscula = sNombreTabla.Replace("Vw_", "vw_");
                    bTabla = false;
                }
                AttributeMappingSource modelo = new AttributeMappingSource();
                var model = modelo.GetModel(typeof(Data.TreeCoreContext));
                if (bTabla)
                {
                    foreach (var mt in model.GetTables())
                    {
                        if (sNombreTablaMinuscula.Equals(mt.TableName) || sNombreTablaMayuscula.Equals(mt.TableName))
                        {
                            int ini = 0;
                            string termino = ""; // filtro para que no muestre los ID
                            string fk = "";  //filtro para que no muestre los foreign key

                            foreach (var dm in mt.RowType.DataMembers)
                            {
                                ini = dm.MappedName.Length - 2;
                                termino = dm.MappedName.Substring(ini, 2);

                                if (termino != "ID")
                                {
                                    if (dm.MappedName.Length > 2)
                                    {
                                        fk = dm.MappedName.Substring(0, 3);
                                    }
                                    else
                                    {
                                        fk = dm.MappedName.Substring(0, 2);
                                    }

                                    if (fk != "FK_")
                                    {
                                        objeto = new { CampoAsociadoID = i.ToString(), CampoAsociado = dm.MappedName.ToString() };
                                        lista.Add(objeto);
                                    }
                                }

                                i = i + 1;
                            }

                        }
                    }
                }
                else
                {
                    foreach (var mt in model.GetTables())
                    {
                        if (sNombreTablaMinuscula.Equals(mt.TableName) || sNombreTablaMayuscula.Equals(mt.TableName))
                        {
                            int ini = 0;
                            string termino = ""; // filtro para que no muestre los ID
                            string fk = "";  //filtro para que no muestre los foreign key

                            foreach (var dm in mt.RowType.DataMembers)
                            {
                                ini = dm.MappedName.Length - 2;
                                termino = dm.MappedName.Substring(ini, 2);

                                if (termino != "ID")
                                {
                                    if (dm.MappedName.Length > 2)
                                    {
                                        fk = dm.MappedName.Substring(0, 3);
                                    }
                                    else
                                    {
                                        fk = dm.MappedName.Substring(0, 2);
                                    }

                                    if (fk != "FK_")
                                    {
                                        objeto = new { CampoAsociadoID = i.ToString(), CampoAsociado = dm.MappedName.ToString() };
                                        lista.Add(objeto);
                                    }
                                }

                                i = i + 1;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        private void RefreshStoreCamposAsociados()
        {
            storeCamposAsociados.DataBind();
        }

        #endregion

        #region STORE OPERACIONES
        protected void storeOperaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Object> lista = new List<object>();
                    Object objeto = null;
                    int i = 0;

                    objeto = new { OperacionID = i.ToString(), Operacion = ">" };
                    lista.Add(objeto);
                    i = i + 1;
                    objeto = new { OperacionID = i.ToString(), Operacion = "<" };
                    lista.Add(objeto);
                    i = i + 1;
                    objeto = new { OperacionID = i.ToString(), Operacion = "=" };
                    lista.Add(objeto);
                    i = i + 1;
                    objeto = new { OperacionID = i.ToString(), Operacion = "> (%)" };
                    lista.Add(objeto);
                    i = i + 1;
                    objeto = new { OperacionID = i.ToString(), Operacion = "< (%)" };
                    lista.Add(objeto);
                    i = i + 1;

                    if (lista != null)
                        storeOperaciones.DataSource = lista;
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

        #region STORE TIPOS DATOS


        protected void storeProyectosTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los datos y los establece
                    var ls = ListaProyectosTipos();
                    if (ls != null)
                    {
                        storeProyectosTipos.DataSource = ls;
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



        /// <summary>
        /// Método que se usa para cargar los datos de la grilla
        /// </summary>
        /// <param name="start">Índice del primer elemento de la grilla</param>
        /// <param name="limit">Número de elementos a recuperar</param>
        /// <param name="sort">Columna por la que se desean ordenar los datos</param>
        /// <param name="dir">Dirección de la ordenación de los datos</param>
        /// <param name="count">Número de datos que se recuperan</param>
        /// <param name="filtro">Filtrado para los datos</param>
        /// <returns>Lista con los datos a mostrar en la grilla</returns>
        private List<Data.ProyectosTipos> ListaProyectosTipos()
        {
            List<Data.ProyectosTipos> listadatos;
            try
            {
                ProyectosTiposController mControl = new ProyectosTiposController();
                long lCliID = long.Parse(hdCliID.Value.ToString());
                listadatos = mControl.GetAllProyectosTipos();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        #endregion

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse ajax = new DirectResponse();
            GlobalLimitesController mController = new GlobalLimitesController();

            try
            {

                long lCliID = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                    Data.GlobalLimites oDato = null;
                    oDato = mController.GetItem(ID);

                    if (oDato.NombreLimite == txtLimite.Text)
                    {
                        oDato.NombreLimite = txtLimite.Text;
                        oDato.Vista = cmbTipoCampoAsociado.SelectedItem.Text;
                        oDato.ProyectoTipoID = Convert.ToInt32(cmbProyectosTipos.SelectedItem.Value);
                    }
                    else
                    {
                        if (mController.RegistroDuplicado(txtLimite.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NombreLimite = txtLimite.Text;
                            oDato.Vista = cmbTipoCampoAsociado.SelectedItem.Text;
                            oDato.ProyectoTipoID = Convert.ToInt32(cmbProyectosTipos.SelectedItem.Value);
                        }
                    }

                    if (mController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                    mController = null;
                }
                else
                {
                    if (mController.RegistroDuplicado(txtLimite.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalLimites oDato = new Data.GlobalLimites();
                        oDato.NombreLimite = txtLimite.Text;
                        oDato.FechaCreacion = DateTime.Now;
                        oDato.CreadorID = Usuario.UsuarioID;
                        oDato.Activo = true;
                        oDato.Vista = cmbTipoCampoAsociado.SelectedItem.Text;
                        oDato.ProyectoTipoID = Convert.ToInt32(cmbProyectosTipos.SelectedItem.Value);
                        oDato.ClienteID = lCliID;
                        oDato = mController.AddItem(oDato);
                        if (oDato != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storePrincipal.DataBind();
                        }
                        storePrincipal.DataBind();
                        mController = null;
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
                long ID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalLimites oDato;
                GlobalLimitesController mController = new GlobalLimitesController();
                oDato = mController.GetItem(ID);
                txtLimite.Text = oDato.NombreLimite;
                cmbTipoCampoAsociado.SetValue(oDato.Vista);
                cmbProyectosTipos.SetValue(oDato.ProyectoTipoID.ToString());
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
            GlobalLimitesController cLimites = new GlobalLimitesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.GlobalLimites oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cLimites.GetDefault(long.Parse(hdCliID.Value.ToString()));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cLimites.UpdateItem(oDato);
                    }

                    oDato = cLimites.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cLimites.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cLimites.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cLimites.UpdateItem(oDato);
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
            GlobalLimitesController CGlobalLimites = new GlobalLimitesController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CGlobalLimites.DeleteItem(lID))
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
            GlobalLimitesController cController = new GlobalLimitesController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.GlobalLimites oDato = cController.GetItem(lID);
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

        #region DIRECT METHOD DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle(long moduloID)
        {
            DirectResponse direct = new DirectResponse();
            direct.Result = "";
            direct.Success = true;

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
        public DirectResponse AgregarEditarDetalle(bool agregar)
        {
            DirectResponse ajax = new DirectResponse();

            GlobalLimitesCondicionesController fController = new GlobalLimitesCondicionesController();

            try
            {
                long limiteLocalID = Int64.Parse(GridRowSelect.SelectedRecordID);
                GlobalLimitesController cLimites = new GlobalLimitesController();
                Data.GlobalLimites limite = cLimites.GetItem(limiteLocalID);
                if (!agregar)
                {
                    long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.GlobalLimitesCondiciones oDato = null;
                    oDato = fController.GetItem(ID);

                    if (oDato.NombreCondicion == txtCriterio.Text)
                    {
                        oDato.NombreCondicion = txtCriterio.Text;
                        oDato.Operador = cmbOperaciones.SelectedItem.Text;
                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.Valor = txtValor.Text;
                        if (txtPorcentajeAdicional.Text != "")
                        {
                            oDato.IncrementoPorcentaje = txtPorcentajeAdicional.Number;
                        }
                        else
                        {
                            oDato.IncrementoPorcentaje = null;
                        }
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        oDato.Modificado = ckModificado.Checked;
                    }
                    else
                    {
                        if (fController.RegistroDuplicadoDetalle(txtCriterio.Text, limite.GlobalLimiteID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NombreCondicion = txtCriterio.Text;
                            oDato.Operador = cmbOperaciones.SelectedItem.Text;
                            oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                            oDato.Valor = txtValor.Text;
                            if (txtPorcentajeAdicional.Text != "")
                            {
                                oDato.IncrementoPorcentaje = txtPorcentajeAdicional.Number;
                            }
                            else
                            {
                                oDato.IncrementoPorcentaje = null;
                            }
                            oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                            oDato.Modificado = ckModificado.Checked;
                        }
                    }
                    if (fController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeDetalle.DataBind();
                    }
                }
                else
                {
                    if (fController.RegistroDuplicadoDetalle(txtCriterio.Text, limite.GlobalLimiteID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.GlobalLimitesCondiciones oDato = new Data.GlobalLimitesCondiciones();
                        oDato.NombreCondicion = txtCriterio.Text;
                        oDato.Operador = cmbOperaciones.SelectedItem.Text;
                        if (txtPorcentajeAdicional.Text != "")
                        {
                            oDato.IncrementoPorcentaje = txtPorcentajeAdicional.Number;
                        }

                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.Valor = txtValor.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        oDato.Activo = true;
                        oDato.Modificado = ckModificado.Checked;
                        oDato.CreadorID = Usuario.UsuarioID;
                        oDato.FechaModificacion = DateTime.Now;
                        oDato.GlobalLimiteID = Int64.Parse(GridRowSelect.SelectedRecordID);

                        if (fController.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDetalle.DataBind();
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

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.GlobalLimitesCondiciones oDato;
                GlobalLimitesCondicionesController fController = new GlobalLimitesCondicionesController();
                oDato = fController.GetItem(ID);
                txtCriterio.Value = oDato.NombreCondicion;
                cmbTiposDatos.SetValue(oDato.TipoDatoID.ToString());
                txtValor.Value = oDato.Valor;
                cmbOperaciones.SetValue(oDato.Operador.ToString());
                cmbCamposAsociados.SetValue(oDato.Campo.ToString());
                ckModificado.Checked = oDato.Modificado;
                if (oDato.IncrementoPorcentaje != null)
                {
                    txtPorcentajeAdicional.Number = (double)oDato.IncrementoPorcentaje;
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
        public DirectResponse AsignarPorDefectoDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalLimitesCondicionesController cLimites = new GlobalLimitesCondicionesController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.GlobalLimitesCondiciones oDato;

                // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
                oDato = cLimites.GetDefault(long.Parse(GridRowSelect.SelectedRecordID));

                // SI HAY Y ES DISTINTO AL SELECCIONADO
                if (oDato != null)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        cLimites.UpdateItem(oDato);
                    }

                    oDato = cLimites.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cLimites.UpdateItem(oDato);
                }
                // SI NO HAY ELEMENTO POR DEFECTO
                else
                {
                    oDato = cLimites.GetItem(lID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    cLimites.UpdateItem(oDato);
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalLimitesCondicionesController CGlobalLimitesCondiciones = new GlobalLimitesCondicionesController();

            direct.Result = "";
            direct.Success = true;


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (CGlobalLimitesCondiciones.DeleteItem(lID))
                {
                    log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

                CGlobalLimitesCondiciones = null;
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
            GlobalLimitesCondicionesController CGlobalLimitesCondiciones = new GlobalLimitesCondicionesController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.GlobalLimitesCondiciones oDato = CGlobalLimitesCondiciones.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CGlobalLimitesCondiciones.UpdateItem(oDato))
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

            CGlobalLimitesCondiciones = null;
            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        #endregion

    }
}