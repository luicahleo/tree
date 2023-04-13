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
using static Comun;

namespace TreeCore.ModGlobal
{
    public partial class Metodos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> ListaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        long lMaestroID = 0;

        #region EVENTOS PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
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
                        //long? cliID = null;
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.ToolMetodos> ListaDatos = null;
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
                            List<Data.Vw_ToolMetodosParametros> ListaDatosDetalle = null;

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

        private List<Data.ToolMetodos> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.ToolMetodos> ListaDatos;
            ToolMetodosController CToolMetodos = new ToolMetodosController();

            try
            {
                if (ClienteID != 0)
                {
                    ListaDatos = CToolMetodos.GetItemsWithExtNetFilterList<Data.ToolMetodos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

        private List<Data.Vw_ToolMetodosParametros> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_ToolMetodosParametros> ListaDatos;

            try
            {
                ToolMetodosParametrosController CToolMetodosParametros = new ToolMetodosParametrosController();

                ListaDatos = CToolMetodosParametros.GetItemsWithExtNetFilterList<Data.Vw_ToolMetodosParametros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "MetodoID == " + lMaestroID);

                CToolMetodosParametros = null;
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
                    string sort, dir;
                    //Recupera los parámetros para obtener los datos de la grilla
                    sort = e.Sort[0].Property.ToString();
                    dir = e.Sort[0].Direction.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters2"];

                    //Recupera los datos y los establece
                    var ls = ListaTiposDatos(e.Start, e.Limit, sort, dir, ref count, filtro);
                    if (ls != null)
                    {
                        storeTiposDatos.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeTiposDatos.Proxy[0];
                        temp.Total = count;
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
        private List<Data.TiposDatos> ListaTiposDatos(int start, int limit, string sort, string dir, ref int count, string filtro)
        {
            List<Data.TiposDatos> datos;
            try
            {
                TiposDatosController mControl = new TiposDatosController();
                datos = mControl.GetItemsWithExtNetFilterList<Data.TiposDatos>(filtro, sort, dir, start, limit, ref count);
                mControl = null;


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }

        #endregion

        #region STORE CAMPO ASOCIADO


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
            int i = 1;
            try
            {


                if (cmbTipoCampoAsociado.Text == "")
                {
                    string ID = GridRowSelectDetalle.SelectedRecordID;
                    if (ID == "_Page")
                    {
                        sNombreTabla = cmbTipoCampoAsociado.Text;
                    }
                    else
                    {

                        Data.ToolMetodosParametros oDato;
                        ToolMetodosParametrosController fController = new ToolMetodosParametrosController();
                        oDato = fController.GetItem(long.Parse(ID));
                        sNombreTabla = oDato.Tabla.ToString();

                    }
                }
                else
                {
                    sNombreTabla = cmbTipoCampoAsociado.Text;
                }



                //sNombreTabla = cmbTipoCampoAsociado.Text;
                AttributeMappingSource modelo = new AttributeMappingSource();
                var model = modelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));
                foreach (var mt in model.GetTables())
                {
                    if (sNombreTabla.Equals(mt.TableName))
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
                                fk = dm.MappedName.Substring(0, 3);

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
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        protected void storeCamposAsociados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sort, dir;
                    //Recupera los parámetros para obtener los datos de la grilla
                    sort = e.Sort.ToString();
                    dir = e.Sort.ToString();
                    int count = 0;
                    string filtro = e.Parameters["gridFilters2"];

                    //Recupera los datos y los establece
                    var ls = ListaCamposAsociados();
                    if (ls != null)
                    {
                        storeCamposAsociados.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeCamposAsociados.Proxy[0];
                        temp.Total = count;
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
        protected void storeTipoCampoAsociado_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                List<Object> lista = new List<object>();
                Object objeto = null;
                int i = 1;

                try
                {

                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));

                    string filtro = "";
                    foreach (var mt in model.GetTables())
                    {
                        filtro = mt.TableName.Substring(4, 3);

                        if (filtro != "vw_" && filtro != "Vw_")
                        {
                            objeto = new { TipoCampoAsociadoID = mt.TableName.ToString(), TipoCampoAsociado = mt.TableName.ToString() };
                            lista.Add(objeto);
                        }

                        i = i + 1;
                    }


                    if (lista != null)
                        storeTipoCampoAsociado.DataSource = lista;//listaFiltrada;

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

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse ajax = new DirectResponse();
            ToolMetodosController mController = new ToolMetodosController();

            try

            {
                long cliID = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.ToolMetodos oDato = mController.GetItem(S);
                    if (oDato.Metodo == txtMetodo.Text)
                    {
                        oDato.Metodo = txtMetodo.Text;
                        oDato.NombreClase = txtNombreClase.Text;
                        oDato.ExtensionURL = txtExtensionURL.Text;
                    }
                    else
                    {
                        if (mController.RegistroDuplicado(txtMetodo.Text, cliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Metodo = txtMetodo.Text;
                            oDato.NombreClase = txtNombreClase.Text;
                            oDato.ExtensionURL = txtExtensionURL.Text;
                        }
                    }
                    if (mController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }


                }

                else
                {
                    if (mController.RegistroDuplicado(txtMetodo.Text, cliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ToolMetodos dato = new Data.ToolMetodos();
                        dato.Metodo = txtMetodo.Text;
                        dato.Activo = true;
                        dato.NumParametros = 0;
                        dato.NombreClase = txtNombreClase.Text;
                        dato.ExtensionURL = txtExtensionURL.Text;
                        dato.ClienteID = cliID;

                        if (mController.AddItem(dato) != null)
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
            DirectResponse direct = new DirectResponse();
            ToolMetodosController cMetodos = new ToolMetodosController();
            try

            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);
                Data.ToolMetodos dato = cMetodos.GetItem(S);
                txtMetodo.Text = dato.Metodo;
                txtNombreClase.Text = dato.NombreClase;
                txtExtensionURL.Text = dato.ExtensionURL;
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            ToolMetodosController cController = new ToolMetodosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.ToolMetodos oDato = cController.GetItem(lID);
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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            ToolMetodosController CToolMetodos = new ToolMetodosController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (CToolMetodos.DeleteItem(lID))
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

        #region DIRECT METHOD DETALLE

        [DirectMethod()]
        public DirectResponse mostrarDetalle()
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
            ToolMetodosParametrosController cToolMetodosParametros = new ToolMetodosParametrosController();
            try
            {
                long S = Int64.Parse(GridRowSelect.SelectedRecordID);
                long cliID = long.Parse(hdCliID.Value.ToString());
                if (!agregar)
                {
                    long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.ToolMetodosParametros oDato = cToolMetodosParametros.GetItem(ID);

                    if (oDato.Orden == int.Parse(txtOrden.Value.ToString()))
                    {
                        oDato.Orden = int.Parse(txtOrden.Value.ToString());
                        oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;
                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        oDato.Controlador = txtControlador.Text;
                        oDato.CampoIntegracion = txtCampoIntegracion.Text;
                    }
                    else
                    {
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (cToolMetodosParametros.RegistroDuplicado(int.Parse(txtOrden.Value.ToString()), Int64.Parse(GridRowSelect.SelectedRecordID)))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.Orden = int.Parse(txtOrden.Value.ToString());
                            oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;
                            oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                            oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                            oDato.Controlador = txtControlador.Text;
                            oDato.CampoIntegracion = txtCampoIntegracion.Text;
                        }
                    }

                    if (cToolMetodosParametros.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                    }
                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());

                    if (cToolMetodosParametros.RegistroDuplicado(int.Parse(txtOrden.Value.ToString()), Int64.Parse(GridRowSelect.SelectedRecordID)))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ToolMetodosParametros oDato = new Data.ToolMetodosParametros();
                        ToolMetodosParametrosController fController = new ToolMetodosParametrosController();
                        ToolMetodosController mController = new ToolMetodosController();
                        ToolMetodos mDatos;


                        oDato.Orden = int.Parse(txtOrden.Value.ToString());
                        oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;
                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        oDato.MetodoID = S;
                        oDato.Controlador = txtControlador.Text;
                        oDato.CampoIntegracion = txtCampoIntegracion.Text;
                        oDato.Activo = true;
                        mDatos = mController.GetItem(Int64.Parse(GridRowSelect.SelectedRecordID));

                        oDato.ClienteID = cliID;

                        if (fController.AddItem(oDato) != null)
                        {
                            mDatos.NumParametros = (int)fController.GetNumeroParametros(S);
                            mController.UpdateItem(mDatos);
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
                long ID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.ToolMetodosParametros oDato;
                ToolMetodosParametrosController fController = new ToolMetodosParametrosController();
                oDato = fController.GetItem(ID);
                cmbTipoCampoAsociado.SetValue(oDato.Tabla.ToString());
                cmbCamposAsociados.SetValue(oDato.Campo.ToString());
                cmbTiposDatos.SetValue(oDato.TipoDatoID.ToString());
                txtOrden.Value = oDato.Orden.ToString();
                txtCampoIntegracion.Text = oDato.CampoIntegracion;
                txtControlador.Text = oDato.Controlador;
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
            DirectResponse direct = new DirectResponse();
            ToolMetodosParametrosController cToolMetodosParametros = new ToolMetodosParametrosController();
            ToolMetodosController cToolMetodos = new ToolMetodosController();
            ToolMetodosParametros oToolMetodosParametros;
            ToolMetodos oToolMetodos;
            direct.Result = "";
            direct.Success = true;


            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
            oToolMetodosParametros = cToolMetodosParametros.GetItem(lID);

            try
            {
                if (cToolMetodosParametros.DeleteItem(lID))
                {
                    oToolMetodos = cToolMetodos.GetItem(oToolMetodosParametros.MetodoID);
                    oToolMetodos.NumParametros = (int)cToolMetodosParametros.GetNumeroParametros(oToolMetodos.MetodoID);
                    cToolMetodos.UpdateItem(oToolMetodos);

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
            ToolMetodosParametrosController CToolMetodosParametros = new ToolMetodosParametrosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.ToolMetodosParametros oDato = CToolMetodosParametros.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CToolMetodosParametros.UpdateItem(oDato))
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

        #endregion
    }
}