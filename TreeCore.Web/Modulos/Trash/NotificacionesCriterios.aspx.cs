using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.Data.SqlClient;
using log4net;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace TreeCore.ModGlobal
{
    public partial class NotificacionesCriterios : TreeCore.Page.BasePageExtNet
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

                Comun.CreateGridFilters(gridFiltersDetalle, storeDetalle, GridDetalle.ColumnModel, ListaIgnoreDetalle, _Locale);

                List<string> ListaIgnoreCampo = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersCampos, storeCampos, gridCampos.ColumnModel, ListaIgnoreCampo, _Locale);

                List<string> ListaIgnoreCamposLibres = new List<string>()
                { };

                Comun.CreateGridFilters(gridFiltersCamposLibres, storeCamposLibres, gridCamposLibres.ColumnModel, ListaIgnoreCamposLibres, _Locale);

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
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;

                        #region MAESTRO
                        if (sModuloID == null || sModuloID == "" || sModuloID == "-1")
                        {

                            List<Data.Vw_NotificacionesGruposCriterios> ListaDatos;
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
                            List<Data.Vw_NotificacionesCriterios> ListaDatosDetalle;
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
            if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDO_NOTIFICACIONES_CRITERIOS))
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
                btnRefrescarDetalle.Hidden = true;
                btnDescargarDetalle.Hidden = true;
            }
            else if (ListaFuncionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_NOTIFICACIONES_CRITERIOS))
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

                    var vLista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro,  long.Parse(hdCliID.Value.ToString()));

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
                }
            }
        }

        private List<Data.Vw_NotificacionesGruposCriterios> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lClienteID)
        {
            List<Data.Vw_NotificacionesGruposCriterios> ListaDatos;
            NotificacionesGruposCriteriosController CNotificacionesGruposCriterios = new NotificacionesGruposCriteriosController();

            try
            {
                if (ClienteID.HasValue)
                {
                    ListaDatos = CNotificacionesGruposCriterios.GetItemsWithExtNetFilterList<Data.Vw_NotificacionesGruposCriterios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "ClienteID == " + lClienteID);
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

                    if (!GridRowSelect.SelectedRecordID.Equals(""))
                    {
                        lMaestroID = long.Parse(GridRowSelect.SelectedRecordID);
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
                }
            }
        }

        private List<Data.Vw_NotificacionesCriterios> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lMaestroID)
        {
            List<Data.Vw_NotificacionesCriterios> listaDatos = new List<Data.Vw_NotificacionesCriterios>();
            NotificacionesCriteriosController cNotificacionesCriterios = new NotificacionesCriteriosController();

            try
            {
                listaDatos = cNotificacionesCriterios.GetItemsWithExtNetFilterList<Data.Vw_NotificacionesCriterios>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "NotificacionGrupoCriterioID == " + lMaestroID);
            }

            catch (Exception ex)
            {
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
                    string sCodTit = Util.ExceptionHandler(ex);
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


        protected void storeTiposDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaTiposDatos();

                    if (ls != null)
                    {
                        storeTiposDatos.DataSource = ls;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.TiposDatos> ListaTiposDatos()
        {
            List<Data.TiposDatos> listaDatos = null;
            TiposDatosController cControl = new TiposDatosController();

            try
            {
                listaDatos = cControl.GetItemsList("");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return listaDatos;
        }

        #endregion

        #region TIPO CAMPO ASOCIADO

        protected void storeTipoCampoAsociado_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {

                List<Object> lista = new List<object>();
                Object objeto = null;

                ParametrosController cParametros = new ParametrosController();
                List<Comun.Parametroslist> listaParamAux = new List<Comun.Parametroslist>();

                List<Object> listaParam = new List<object>();
                List<Object> listaFiltrada = new List<object>();

                try
                {

                    AttributeMappingSource modelo = new AttributeMappingSource();
                    var model = modelo.GetModel(typeof(Data.TreeCoreContext));
                    listaParamAux = cParametros.ObtenerListaParametro("TABLAS_AUDITABLES");

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
                    MensajeErrorGenerico(ex);
                }
            }
        }
        #endregion

        #region CAMPOS ASOCIADOS
        protected void storeCamposAsociados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var ls = ListaCamposAsociados();

                    if (ls != null)
                    {
                        storeCamposAsociados.DataSource = ls;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Object> ListaCamposAsociados()
        {
            List<Object> lista = new List<object>();
            Object objeto = null;
            string sNombreTabla = null;
            string sNombreTablaMinuscula = null;
            string sNombreTablaMayuscula = null;
            int i = 1;
            NotificacionesGruposCriteriosController cGrupo = new NotificacionesGruposCriteriosController();
            Data.NotificacionesGruposCriterios oGrupo = null;
            bool bTabla = true;

            try
            {
                oGrupo = cGrupo.GetItem(long.Parse(GridRowSelect.SelectedRecordID));
                sNombreTabla = oGrupo.Tabla;

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
            }
            return lista;
        }

        #endregion

        #region OPERACIONES
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

                    objeto = new { OperacionID = i.ToString(), Operacion = "==" };
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
                    MensajeErrorGenerico(ex);
                }
            }
        }
        #endregion

        #region CAMPOS

        protected void storeCampos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersCampos"];

                    var ls = ListaCampos(e.Start, e.Limit, "", "", ref iCount, sFiltro);

                    if (ls != null)
                    {
                        storeCampos.DataSource = ls;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        public List<Object> ListaCampos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Object> lista = null;
            int i = 1;
            Object objeto = null;

            try
            {
                long lGrupoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                NotificacionesGruposCriteriosController cGrupo = new NotificacionesGruposCriteriosController();

                NotificacionesGruposCriterios oGrupo = cGrupo.GetItem(lGrupoID);
                lista = new List<object>();

                if (oGrupo.CamposVisualizar != null && !oGrupo.CamposVisualizar.Equals(""))
                {
                    string[] aCampo = oGrupo.CamposVisualizar.Split(';');

                    foreach (string visor in aCampo)
                    {
                        objeto = new { CampoID = i.ToString(), Campo = visor };
                        lista.Add(objeto);
                        i = i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
            }

            return lista;
        }


        #endregion

        #region CAMPOS LIBRES

        protected void storeCamposLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                UsuariosPerfilesController cUsuairosPerfiles = new UsuariosPerfilesController();

                try
                {
                    var ls = ListaCamposLibres();

                    if (ls != null)
                    {
                        storeCamposLibres.DataSource = ls;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        public List<Object> ListaCamposLibres()
        {
            List<Object> lista = null;
            int i = 1;
            Object objeto = null;
            List<string> listaCampos = null;

            try
            {
                long lGrupoID = Convert.ToInt32(GridRowSelect.SelectedRecordID);
                NotificacionesGruposCriteriosController cGrupo = new NotificacionesGruposCriteriosController();
                NotificacionesGruposCriterios oGrupo = cGrupo.GetItem(lGrupoID);

                if (oGrupo.CamposVisualizar != null && !oGrupo.CamposVisualizar.Equals(""))
                {
                    listaCampos = oGrupo.CamposVisualizar.Split(';').ToList();
                }
                else
                {
                    listaCampos = new List<string>();
                }

                oGrupo = cGrupo.GetItem(Int64.Parse(GridRowSelect.SelectedRecordID));
                string sNombreTabla = oGrupo.Tabla;
                string sNombreTablaMinuscula = "";
                string sNombreTablaMayuscula = "";

                AttributeMappingSource modelo = new AttributeMappingSource();
                var model = modelo.GetModel(typeof(Data.TreeCoreContext));
                sNombreTablaMayuscula = sNombreTabla.Substring(0, 3) + "." + "Vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);
                sNombreTablaMinuscula = sNombreTabla.Substring(0, 3) + "." + "vw_" + sNombreTabla.Substring(4, sNombreTabla.Length - 4);

                foreach (var mt in model.GetTables())
                {
                    if (sNombreTablaMinuscula.Equals(mt.TableName) || sNombreTablaMayuscula.Equals(mt.TableName))
                    {
                        int ini = 0;
                        string termino = ""; // filtro para que no muestre los ID
                        string fk = "";  //filtro para que no muestre los foreign key
                        lista = new List<object>();

                        foreach (var dm in mt.RowType.DataMembers)
                        {
                            ini = dm.MappedName.Length - 2;
                            termino = dm.MappedName.Substring(ini, 2);

                            if (termino != "ID")
                            {
                                if (dm.MappedName.Length > 3)
                                {
                                    fk = dm.MappedName.Substring(0, 3);
                                }
                                else
                                {
                                    fk = "";
                                }

                                if (fk != "FK_")
                                {
                                    if (!listaCampos.Contains(dm.MappedName.ToString()))
                                    {
                                        objeto = new { CampoID = dm.MappedName.ToString(), Campo = dm.MappedName.ToString() };
                                        lista.Add(objeto);
                                        i = i + 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);
            }

            return lista;
        }

        #endregion

        #endregion

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse ajax = new DirectResponse();
            NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();
            long lCliID = 0;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.NotificacionesGruposCriterios oDato = cController.GetItem(lS);

                    if (oDato.NotificacionGrupoCriterio == txtGrupo.Text)
                    {
                        oDato.NotificacionGrupoCriterio = txtGrupo.Text;
                        oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;
                    }
                    else
                    {
                        lCliID = long.Parse(hdCliID.Value.ToString());

                        if (cController.RegistroDuplicado(txtGrupo.Text, lCliID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NotificacionGrupoCriterio = txtGrupo.Text;
                            oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;
                        }
                    }
                    if (cController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {
                    lCliID = long.Parse(hdCliID.Value.ToString());

                    if (cController.RegistroDuplicado(txtGrupo.Text, lCliID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.NotificacionesGruposCriterios oDato = new Data.NotificacionesGruposCriterios();
                        oDato.NotificacionGrupoCriterio = txtGrupo.Text;
                        oDato.FechaCreacion = DateTime.Now;
                        oDato.CreadorID = Usuario.UsuarioID;
                        oDato.Activo = true;
                        oDato.Tabla = cmbTipoCampoAsociado.SelectedItem.Text;

                        if (cmbClientes.SelectedItem.Value != null && cmbClientes.SelectedItem.Value != "")
                        {
                            oDato.ClienteID = long.Parse(cmbClientes.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ClienteID = lCliID;
                        }

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
                long lID = Int64.Parse(GridRowSelect.SelectedRecordID);
                Data.NotificacionesGruposCriterios oDato = new Data.NotificacionesGruposCriterios();
                NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();

                oDato = cController.GetItem(lID);
                txtGrupo.Text = oDato.NotificacionGrupoCriterio;

                if (oDato.Tabla != null)
                {
                    cmbTipoCampoAsociado.SetValue(oDato.Tabla);
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
            NotificacionesGruposCriteriosController cNotificacionesGruposCriterios = new NotificacionesGruposCriteriosController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cNotificacionesGruposCriterios.DeleteItem(lID))
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
            NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.NotificacionesGruposCriterios oDato = cController.GetItem(lID);
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
                storeDetalle.Reload();
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

            try
            {
                long lGrupoID = Int64.Parse(GridRowSelect.SelectedRecordID);
                NotificacionesGruposCriteriosController cGrupos = new NotificacionesGruposCriteriosController();
                NotificacionesCriteriosController cController = new NotificacionesCriteriosController();
                Data.NotificacionesGruposCriterios oGrupo = cGrupos.GetItem(lGrupoID);

                if (!bAgregar)
                {
                    long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                    Data.NotificacionesCriterios oDato = cController.GetItem(lID);

                    if (oDato.NotificacionCriterio == txtCriterio.Text)
                    {
                        oDato.NotificacionCriterio = txtCriterio.Text;
                        oDato.Operador = cmbOperaciones.SelectedItem.Text;
                        oDato.Tabla = oGrupo.Tabla;
                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.Valor = txtValor.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                    }
                    else
                    {
                        if (cController.RegistroDuplicado(txtCriterio.Text, lGrupoID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NotificacionCriterio = txtCriterio.Text;
                            oDato.Operador = cmbOperaciones.SelectedItem.Text;
                            oDato.Tabla = oGrupo.Tabla;
                            oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                            oDato.Valor = txtValor.Text;
                            oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        }
                    }

                    if (cController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storeDetalle.DataBind();
                    }
                }
                else
                {
                    if (cController.RegistroDuplicado(txtCriterio.Text, lGrupoID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.NotificacionesCriterios oDato = new Data.NotificacionesCriterios();
                        oDato.NotificacionCriterio = txtCriterio.Text;
                        oDato.Operador = cmbOperaciones.SelectedItem.Text;
                        oDato.Tabla = oGrupo.Tabla;
                        oDato.Campo = cmbCamposAsociados.SelectedItem.Text;
                        oDato.Valor = txtValor.Text;
                        oDato.TipoDatoID = Convert.ToInt32(cmbTiposDatos.SelectedItem.Value);
                        oDato.Activo = true;
                        oDato.CreadorID = Usuario.UsuarioID;
                        oDato.FechaCreacion = DateTime.Now;
                        oDato.NotificacionGrupoCriterioID = Int64.Parse(GridRowSelect.SelectedRecordID);
                        
                        if (cController.AddItem(oDato) != null)
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

            ajax.Success = true;
            ajax.Result = "";

            return ajax;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditarDetalle()
        {
            DirectResponse ajax = new DirectResponse();

            try
            {
                long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);
                Data.NotificacionesCriterios oDato = new Data.NotificacionesCriterios();
                NotificacionesCriteriosController cController = new NotificacionesCriteriosController();

                oDato = cController.GetItem(lID);
                txtCriterio.Value = oDato.NotificacionCriterio;

                if (oDato.TipoDatoID != 0)
                {
                    cmbTiposDatos.SetValue(oDato.TipoDatoID.ToString());
                }

                txtValor.Value = oDato.Valor;

                if (oDato.Operador != null)
                {
                    cmbOperaciones.SetValue(oDato.Operador);
                }
                if (oDato.Campo != null)
                {
                    cmbCamposAsociados.SetValue(oDato.Campo);
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
            DirectResponse direct = new DirectResponse();
            NotificacionesCriteriosController cNotificacionesCriterios = new NotificacionesCriteriosController();
            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                if (cNotificacionesCriterios.DeleteItem(lID))
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
            NotificacionesCriteriosController cNotificacionesCriterios = new NotificacionesCriteriosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.NotificacionesCriterios oDato = cNotificacionesCriterios.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cNotificacionesCriterios.UpdateItem(oDato))
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

        #region NOTIFICACIONES

        [DirectMethod]
        public DirectResponse EnviarNotificaciones()
        {
            DirectResponse ajax = new DirectResponse();
            NotificacionesGruposCriteriosController cGrupos = new NotificacionesGruposCriteriosController();
            NotificacionesCriteriosController cController = new NotificacionesCriteriosController();

            storeCampos.Reload();
            winCampos.Show();

            return ajax;
        }

        #endregion

        #region ASIGNAR CAMPOS

        [DirectMethod]
        public DirectResponse AgregarCampos()
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();
            List<string> lista = null;
            string sCampos = null;

            try
            {
                lista = new List<string>();
                NotificacionesGruposCriterios oGrupo = cController.GetItem(Convert.ToInt32(GridRowSelect.SelectedRecordID));

                if (oGrupo.CamposVisualizar != null && !oGrupo.CamposVisualizar.Equals(""))
                {
                    sCampos = oGrupo.CamposVisualizar + ";";
                }
                else
                {
                    sCampos = "";
                }

                foreach (SelectedRow selec in GridRowSelectCampoLibre.SelectedRows)
                {
                    if (!sCampos.Contains(selec.RecordID + ";"))
                    {
                        sCampos = sCampos + selec.RecordID;
                        sCampos = sCampos + ";";
                    }
                }

                oGrupo.CamposVisualizar = sCampos.Substring(0, sCampos.Length - 1);
                cController.UpdateItem(oGrupo);

                storeCampos.DataBind();
                storeCamposLibres.DataSource = new List<Object>();

                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);

                direct.Success = true;
            }

            return direct;
        }

        [DirectMethod]
        public DirectResponse QuitarCampos()
        {
            DirectResponse direct = new DirectResponse();
            NotificacionesGruposCriteriosController cController = new NotificacionesGruposCriteriosController();
            List<string> lista = null;
            string sCampos = null;
            int i = 0;

            try
            {
                lista = new List<string>();
                NotificacionesGruposCriterios oGrupo = cController.GetItem(Convert.ToInt32(GridRowSelect.SelectedRecordID));

                if (oGrupo.CamposVisualizar != null && !oGrupo.CamposVisualizar.Equals(""))
                {
                    lista = oGrupo.CamposVisualizar.Split(';').ToList();
                }
                else
                {
                    lista = new List<string>();
                }

                foreach (SelectedRow selec in GridRowSelectCampos.SelectedRows)
                {
                    i = selec.RowIndex;
                    lista.RemoveAt(i);
                }

                sCampos = "";

                foreach (string nuevoCampo in lista)
                {
                    if (nuevoCampo != null && nuevoCampo != "")
                    {
                        sCampos = sCampos + nuevoCampo + ";";
                    }
                }

                if (sCampos != "")
                {
                    oGrupo.CamposVisualizar = sCampos.Substring(0, sCampos.Length - 1);
                }
                else
                {
                    oGrupo.CamposVisualizar = sCampos.Substring(0, sCampos.Length);
                }

                cController.UpdateItem(oGrupo);
                storeCampos.DataBind();

                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                string codTit = Util.ExceptionHandler(ex);

                direct.Success = true;
            }

            return direct;
        }

        #endregion

        #endregion
    }
}