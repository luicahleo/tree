using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Data.Linq.Mapping;

namespace TreeCore.ModGlobal
{
    public partial class TablasModeloDatos : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> ListaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        public static List<Object> listaTablas;
        public static List<Object> listaControladores;
        long MaestroID = 0;

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

                List<string> ListaIgnore = new List<string>() { };
                Comun.CreateGridFilters(gridFilters, storePrincipal, gridMaestro.ColumnModel, ListaIgnore, _Locale);
                List<string> ListaIgnoreDetalle = new List<string>() { };
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
                        string sMaestroID = Request.QueryString["aux"].ToString();
                        long CliID = long.Parse(Request.QueryString["cliente"]);
                        int iCount = 0;
                        string[] param = Request.QueryString["aux"].ToString().Split(';');
                        string sGrid = Request.QueryString["aux3"];
                        RegionesPaisesController cRegionesPaises = new RegionesPaisesController();


                        #region MAESTRO

                        if (sGrid == "-1")
                        {

                            List<JsonObject> ListaoDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(gridMaestro.ColumnModel, ListaoDatos, Response, "", GetGlobalResource(Comun.jsTablasModeloDatos), _Locale);
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
                            List<JsonObject> ListaoDatosDetalle = ListaDetalle(0, 0, sOrden, sDir, ref iCount, sFiltro, long.Parse(sMaestroID));

                            try
                            {
                                Comun.ExportacionDesdeListaNombre(GridDetalle.ColumnModel, ListaoDatosDetalle, Response, "", GetGlobalResource(Comun.jsTablasModeloDatos), _Locale);
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
            sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            funtionalities = new System.Collections.Hashtable() {
                { "Read", new List<ComponentBase> { } },
                { "Download", new List<ComponentBase> { btnDescargar, btnDescargarDetalle }},
                { "Post", new List<ComponentBase> { btnAnadir, btnAnadirDetalle, btnEditarDetalle, btnActivarDetalle }},
                { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
                { "Delete", new List<ComponentBase> { btnEliminar, btnEliminarDetalle }}
            };
        }

        #endregion

        #region STORES

        #region CONTROLADORES

        protected void storeControladores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();



                Object objeto;
                Type[] types;
                int i = 1;



                try
                {
                    types = Assembly.GetExecutingAssembly().GetTypes();
                    if (listaControladores != null)
                    {
                        lista = listaControladores;
                    }
                    else
                    {
                        foreach (Type t in types)
                        {
                            if (t.Name.EndsWith("Controller"))
                            {
                                objeto = new { ControladorID = i.ToString(), ControladorNombre = t.Name };
                                lista.Add(objeto);
                                i = i + 1;
                            }



                        }
                        listaControladores = lista;
                    }
                    if (lista.Count > 0)
                    {
                        storeControladores.DataSource = lista;
                        storeControladores.DataBind();
                        if (hdController.Value != null && hdController.Value != "")
                        {
                            cmbControlador.SetValue(hdController.Value);
                            cmbControlador.Triggers[0].Hidden = false;
                            hdController.SetValue("");
                        }
                    }
                }
                catch (ReflectionTypeLoadException excep)
                {
                    log.Error(excep.Message);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region MODULOS

        protected void storeModulos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaModulos();

                    if (vLista != null)
                    {
                        storeModulos.DataSource = vLista;

                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Modulos> ListaModulos()
        {
            List<Data.Modulos> ListaDatos;
            ModulosController CModulos = new ModulosController();

            try
            {
                ListaDatos = CModulos.getModulosActivos();
            }

            catch (Exception ex)
            {
                ListaDatos = null;
                log.Error(ex.Message);
            }

            return ListaDatos;
        }


        #endregion

        #region TABLAS

        protected void storeTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                #region REFLEXION
                try
                {
                    List<Object> lista = new List<object>();
                    Object objeto = null;
                    int i = 1;

                    if (listaTablas != null)
                    {
                        lista = listaTablas;
                    }
                    else
                    {
                        AttributeMappingSource oModelo = new AttributeMappingSource();
                        var vModel = oModelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));

                        foreach (var mt in vModel.GetTables())
                        {
                            objeto = new { TablaID = i.ToString(), TablaNombre = mt.TableName.ToString() };
                            lista.Add(objeto);
                            i++;
                        }
                    }

                    if (lista.Count > 0)
                    {
                        storeTablas.DataSource = lista;
                        storeTablas.DataBind();
                        if (hdDatabase.Value != null && hdDatabase.Value != "")
                        {
                            cmbTable.Triggers[0].Hidden = false;
                            hdDatabase.SetValue("");
                        }
                        listaTablas = lista;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                #endregion
            }

        }

        #endregion

        #region COLUMNAS
        protected void storeColumnas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Object> lista = new List<object>();
                Object objeto;
                int i = 1;

                try
                {
                    /*TablasBD tabla = new TablasBD();
                    if (hdDatabase.Value != null && hdDatabase.Value != "")
                    {
                        tabla.TablaNombre = hdDatabase.Value.ToString();
                    }
                    if (tabla.TablaNombre != null)
                    {
                        AttributeMappingSource oModelo = new AttributeMappingSource();
                        var vModel = oModelo.GetModel(typeof(TreeCore.Data.TreeCoreContext));

                        foreach (var mt in vModel.GetTables())
                        {
                            if (tabla.TablaNombre.Equals(mt.TableName))
                            {
                                foreach (var dm in mt.RowType.DataMembers)
                                {
                                    objeto = new { ColumnaTablaID = i.ToString(), ColumnaNombre = dm.MappedName.ToString() };
                                    lista.Add(objeto);
                                    i = i + 1;
                                }
                            }
                        }

                        if (lista.Count > 0)
                        {
                            storeColumnas.DataSource = lista;
                            storeColumnas.DataBind();
                            if (hdValue.Value != null || hdValue.Value != "")
                            {
                                cmbValue.SetValue(hdValue.Value);
                                cmbValue.Triggers[0].Hidden = false;
                                hdValue.SetValue("");
                            }
                        }
                    }
                    else
                    {
                        storeColumnas.DataSource = null;
                    }*/
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

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


                    //Recupera los oDatos y los establece
                    var ls = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);
                    if (ls != null)
                    {
                        storePrincipal.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storePrincipal.Proxy[0];
                        temp.Total = iCount;


                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    //string codTit = Util.ExceptionHandler(ex);
                    //MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }
        }

        private List<JsonObject> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_TablasModelosDatos> ListaoDatos;
            GlobalTablasModeloDatosController CTablasModeloDatos = new GlobalTablasModeloDatosController();
            JsonObject obj;
            List<JsonObject> ListGlobalTablasModeloDatos = new List<JsonObject>();

            try
            {
                if (sSort == "ClaveRecursoTabla")
                {
                    sSort = "ClaveRecurso";
                }

                ListaoDatos = CTablasModeloDatos.GetItemsWithExtNetFilterList<Data.Vw_TablasModelosDatos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);

                if (ListaoDatos != null)
                {
                    foreach (var item in ListaoDatos)
                    {
                        obj = new JsonObject();
                        obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                        obj.Add("Activo", item.Activo);
                        obj.Add("NombreTabla", item.NombreTabla);
                        obj.Add("ClaveRecurso", item.ClaveRecurso);
                        obj.Add("Controlador", item.Controlador);
                        obj.Add("Indice", item.Indice);
                        obj.Add("ModuloID", item.ModuloID);
                        obj.Add("Modulo", item.Modulo);

                        obj.Add("ClaveRecursoTabla", (GetGlobalResource(item.ClaveRecurso) == "") ? item.ClaveRecurso : GetGlobalResource(item.ClaveRecurso));

                        ListGlobalTablasModeloDatos.Add(obj);
                    }


                }
            }
            catch (Exception ex)
            {
                ListGlobalTablasModeloDatos = null;
                log.Error(ex.Message);
            }

            return ListGlobalTablasModeloDatos;
        }


        #endregion

        #region TABLAS MODELOS DATOS

        protected void storeTablasModelosDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    //Recupera los oDatos y los establece
                    var ls = ListaTablasModelosDatos();
                    if (ls != null)
                    {
                        storeTablasModelosDatos.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeTablasModelosDatos.Proxy[0];

                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<JsonObject> ListaTablasModelosDatos()
        {
            List<Data.TablasModeloDatos> ListaoDatos;
            GlobalTablasModeloDatosController CTablasModeloDatos = new GlobalTablasModeloDatosController();
            JsonObject obj;
            List<JsonObject> ListGlobalTablasModeloDatos = new List<JsonObject>();

            try
            {
                long lTabla = long.Parse(hdDatabaseID.Value.ToString());
                ListaoDatos = CTablasModeloDatos.GetAllTablaModeloDatosActivos(lTabla);

                if (ListaoDatos != null)
                {
                    foreach (var item in ListaoDatos)
                    {
                        obj = new JsonObject();
                        obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                        obj.Add("Activo", item.Activo);
                        obj.Add("NombreTabla", item.NombreTabla);
                        obj.Add("ClaveRecurso", item.ClaveRecurso);

                        obj.Add("ClaveRecursoTabla", (GetGlobalResource(item.ClaveRecurso) == "") ? item.ClaveRecurso : GetGlobalResource(item.ClaveRecurso));

                        ListGlobalTablasModeloDatos.Add(obj);
                    }

                }
            }
            catch (Exception ex)
            {
                ListGlobalTablasModeloDatos = null;
                log.Error(ex.Message);
            }

            return ListGlobalTablasModeloDatos;
        }


        #endregion

        #region COLUMNAS FK
        protected void storeColumnasFK_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {

            if (RequestManager.IsAjaxRequest)
            {

                try
                {
                    //Recupera los oDatos y los establece
                    var ls = ListaColumnasFK();
                    if (ls != null)
                    {
                        storeColumnasFK.DataSource = ls;

                        PageProxy temp;
                        temp = (PageProxy)storeColumnasFK.Proxy[0];

                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_ColumnasModeloDatos> ListaColumnasFK()
        {
            List<Data.Vw_ColumnasModeloDatos> ListaoDatos;
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();

            try
            {
                long lTabla = long.Parse(cmbDataSource.Value.ToString());
                ListaoDatos = cColumnas.GetColumnasTablasVista(lTabla);


            }
            catch (Exception ex)
            {
                ListaoDatos = null;
                log.Error(ex.Message);
            }

            return ListaoDatos;
        }

        #endregion

        #region DETALLE

        protected void storeDetalle_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> ListGlobalColumnasModeloDatos = new List<JsonObject>();

                    string sSort, sDir;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersDetalle"];


                    long MaestroID;

                    if (hdMaestroID.Value.ToString() != "0")
                    {
                        if (hdDatabaseID.Value.ToString() != "0")
                        {
                            MaestroID = Convert.ToInt64(hdDatabaseID.Value.ToString());
                        }
                        else
                        {
                            MaestroID = 0;
                        }
                    }
                    else
                    {
                        MaestroID = 0;
                    }

                    var vLista = ListaDetalle(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, MaestroID);


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

        private List<JsonObject> ListaDetalle(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long MaestroID)
        {
            List<Data.Vw_ColumnasModeloDatos> ListaoDatos;
            GlobalColumnasModeloDatosController CColumnasModeloDatos = new GlobalColumnasModeloDatosController();
            JsonObject obj;
            List<JsonObject> ListDetalleJ = new List<JsonObject>();



            try

            {
                if (MaestroID != 0)
                {
                    if (sSort == "ClaveRecurso")
                    {
                        sSort = "ClaveRecursoColumna";
                    }

                    ListaoDatos = CColumnasModeloDatos.GetItemsWithExtNetFilterList<Data.Vw_ColumnasModeloDatos>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "TablaModeloDatosID == " + MaestroID.ToString());

                    if (ListaoDatos != null)
                    {
                        foreach (var item in ListaoDatos)
                        {
                            obj = new JsonObject();
                            obj.Add("ColumnaModeloDatosID", item.ColumnaModeloDatosID);
                            obj.Add("Activo", item.ActivoColumna);
                            obj.Add("NombreColumna", item.NombreColumna);
                            obj.Add("ClaveRecurso", item.ClaveRecursoColumna);
                            obj.Add("TipoDato", item.TipoDato);
                            obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("ForeignKeyNombre", item.ForeignKeyNombre);
                            obj.Add("ForeignKeyID", item.ForeignKeyID);

                            obj.Add("ClaveRecursoColumna", (GetGlobalResource(item.ClaveRecursoColumna) == "") ? item.ClaveRecursoColumna : GetGlobalResource(item.ClaveRecursoColumna));
                            obj.Add("ClaveRecursoTabla", (GetGlobalResource(item.ClaveRecursoTabla) == "") ? item.ClaveRecursoTabla : GetGlobalResource(item.ClaveRecursoTabla));

                            ListDetalleJ.Add(obj);
                        }
                    }
                }
                else
                {
                    ListDetalleJ = null;
                }
            }

            catch (Exception ex)
            {
                ListaoDatos = null;
                log.Error(ex.Message);
            }

            return ListDetalleJ;
        }


        #endregion


        #region TIPO DATO

        protected void storeTipoDato_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var vLista = ListaTipoDato();

                    if (vLista != null)
                    {
                        storeTipoDato.DataSource = vLista;
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

        private List<Data.TiposDatos> ListaTipoDato()
        {
            List<Data.TiposDatos> ListaoDatos;
            TiposDatosController CTiposDatos = new TiposDatosController();

            try
            {
                String compaID = cmbValue.Value.ToString();

                int tam_var = compaID.Length;
                String Var_Sub = compaID.Substring((tam_var - 2), 2);

                if (Var_Sub == "ID")
                {
                    ListaoDatos = CTiposDatos.GetListasTiposDatos();
                }
                else
                {
                    ListaoDatos = CTiposDatos.GetAllTiposDatosNoListas();
                }

            }

            catch (Exception ex)
            {
                ListaoDatos = null;
                log.Error(ex.Message);
            }

            return ListaoDatos;
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
            List<Data.Clientes> listaoDatos;
            ClientesController cClientes = new ClientesController();

            try
            {
                listaoDatos = cClientes.GetActivos();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaoDatos = null;
            }

            return listaoDatos;
        }

        #endregion


        #endregion

        #region DIRECT METHOD

        #region DIRECT METHOD MAESTRO

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();

            GlobalTablasModeloDatosController cTablasModeloDatos = new GlobalTablasModeloDatosController();

            #endregion

            #region INTEGRACION_COMARCH
            try
            {
                if (!agregar)
                {
                    long S = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.TablasModeloDatos oDato = cTablasModeloDatos.GetItem(S);

                    oDato.NombreTabla = cmbTable.Text;
                    oDato.Activo = true;
                    oDato.ClaveRecurso = txtClaveRecurso.Text;
                    oDato.Controlador = cmbControlador.Text;
                    if (cmbModulo.Value != "")
                    {
                        oDato.ModuloID = Convert.ToInt32(cmbModulo.Value.ToString());
                    }
                    else
                    {
                        oDato.ModuloID = null;
                    }

                    if (cmbIndice.Value != "")
                    {
                        oDato.Indice = cmbIndice.Text;
                    }
                    else
                    {
                        oDato.Indice = null;
                    }


                    if (cTablasModeloDatos.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }

                }
                else
                {

                    if (cTablasModeloDatos.RegistroDuplicadoByNombre(cmbTable.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.TablasModeloDatos oDato = new Data.TablasModeloDatos();
                        oDato.NombreTabla = cmbTable.Text;
                        oDato.Activo = true;
                        oDato.ClaveRecurso = txtClaveRecurso.Text;
                        oDato.Controlador = cmbControlador.Text;
                        if (cmbModulo.Value != "")
                        {
                            oDato.ModuloID = Convert.ToInt32(cmbModulo.Value.ToString());
                        }
                        else
                        {
                            oDato.ModuloID = null;
                        }

                        if (cmbIndice.Value != "")
                        {
                            oDato.Indice = cmbIndice.Text;
                        }
                        else
                        {
                            oDato.Indice = null;
                        }




                        if (cTablasModeloDatos.AddItem(oDato) != null)
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
            #endregion
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();

            GlobalTablasModeloDatosController cTablasModeloDatos = new GlobalTablasModeloDatosController();


            try

            {
                long S = long.Parse(GridRowSelect.SelectedRows[0].RecordID);

                Data.TablasModeloDatos oDato = cTablasModeloDatos.GetItem(S);

                cmbTable.SetValue(oDato.NombreTabla);
                txtClaveRecurso.Text = oDato.ClaveRecurso;
                cmbControlador.SetValue(oDato.Controlador);
                cmbModulo.SetValue(oDato.ModuloID);
                cmbIndice.SetValue(oDato.Indice);
                winGestion.Show();
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
            GlobalTablasModeloDatosController CProvincias = new GlobalTablasModeloDatosController();
            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {

                Data.TablasModeloDatos oDato;
                oDato = CProvincias.GetItem(lID);

                if (CProvincias.DeleteItem(lID))
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
        public DirectResponse mostrarDetalle(long MaestroID)
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

            GlobalColumnasModeloDatosController CGlobalColumnasModeloDatosController = new GlobalColumnasModeloDatosController();
            long municipalidadID = long.Parse(GridRowSelect.SelectedRecordID);

            long cliID = 0;
            try
            {
                if (!agregar)
                {
                    long S = Int64.Parse(GridRowSelectDetalle.SelectedRows[0].RecordID);
                    Data.ColumnasModeloDatos oDato = CGlobalColumnasModeloDatosController.GetItem(S);

                    if (oDato.NombreColumna == cmbValue.Text)
                    {
                        oDato.NombreColumna = cmbValue.Text;
                        oDato.ClaveRecurso = txtClaveRecursoDetalle.Text;


                        if (cmbTipoDato.SelectedItem != null && cmbTipoDato.SelectedItem.Text.ToString() != null && cmbTipoDato.SelectedItem.Text.ToString() != "")
                        {
                            TiposDatosController cTipo = new TiposDatosController();
                            Data.TiposDatos oTipo = cTipo.GetTipoDatosByNombre(cmbTipoDato.SelectedItem.Text.ToString());

                            oDato.TipoDatoID = long.Parse(oTipo.TipoDatoID.ToString());
                        }
                        else
                        {
                            oDato.TipoDatoID = null;
                        }


                        if (cmbForeignKey.SelectedItem.Text != hdForeignKey.Value.ToString())
                        {
                            if (cmbForeignKey.SelectedItem != null && cmbForeignKey.SelectedItem.Text != null && cmbForeignKey.SelectedItem.Text != "")
                            {

                                oDato.ForeignKeyID = long.Parse(cmbForeignKey.Text.ToString());
                            }
                            else
                            {
                                oDato.ForeignKeyID = null;
                            }
                        }
                        else
                        {
                            oDato.ForeignKeyID = oDato.ForeignKeyID;
                        }

                    }
                    else
                    {
                        long MaestroID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);
                        cliID = long.Parse(hdCliID.Value.ToString());
                        if (CGlobalColumnasModeloDatosController.RegistroDuplicadoByNombre(cmbValue.Text, MaestroID))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            oDato.NombreColumna = cmbValue.Text;
                            oDato.ClaveRecurso = txtClaveRecursoDetalle.Text;


                            if (cmbTipoDato.SelectedItem != null && cmbTipoDato.SelectedItem.Value != null && cmbTipoDato.SelectedItem.Value != "")
                            {
                                oDato.TipoDatoID = long.Parse(cmbTipoDato.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.TipoDatoID = null;
                            }

                            if (cmbForeignKey.SelectedItem != null && cmbForeignKey.SelectedItem.Value != null && cmbForeignKey.SelectedItem.Value != "")
                            {
                                oDato.ForeignKeyID = long.Parse(cmbForeignKey.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.ForeignKeyID = null;
                            }
                        }
                    }

                    if (CGlobalColumnasModeloDatosController.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));

                    }

                }
                else
                {
                    cliID = long.Parse(hdCliID.Value.ToString());
                    long MaestroID = Int64.Parse(GridRowSelect.SelectedRows[0].RecordID);

                    if (CGlobalColumnasModeloDatosController.RegistroDuplicadoByNombre(cmbValue.Text, MaestroID))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.ColumnasModeloDatos oDato = new Data.ColumnasModeloDatos();
                        oDato.NombreColumna = cmbValue.Text;
                        oDato.ClaveRecurso = txtClaveRecursoDetalle.Text;
                        oDato.Activo = true;
                        oDato.TablaModeloDatosID = Convert.ToInt64(hdDatabaseID.Value.ToString());

                        if (cmbTipoDato.SelectedItem != null && cmbTipoDato.SelectedItem.Value != null && cmbTipoDato.SelectedItem.Value != "")
                        {
                            oDato.TipoDatoID = long.Parse(cmbTipoDato.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.TipoDatoID = null;
                        }

                        if (cmbForeignKey.SelectedItem != null && cmbForeignKey.SelectedItem.Value != null && cmbForeignKey.SelectedItem.Value != "")
                        {
                            oDato.ForeignKeyID = long.Parse(cmbForeignKey.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ForeignKeyID = null;
                        }

                        if (CGlobalColumnasModeloDatosController.AddItem(oDato) != null)
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

                GlobalColumnasModeloDatosController CGlobalColumnasModeloDatosController = new GlobalColumnasModeloDatosController();
                Data.Vw_ColumnasModeloDatos oDato = CGlobalColumnasModeloDatosController.GetItem<Data.Vw_ColumnasModeloDatos>(Convert.ToInt64(hdColumnaID.Value.ToString()));

                cmbValue.SetValue(oDato.NombreColumna);
                txtClaveRecursoDetalle.Text = oDato.ClaveRecursoColumna;
                cmbTipoDato.Enable();
                cmbTipoDato.SetValue(oDato.TipoDato);

                if (oDato.TipoDato.ToString() == "Lista" || oDato.TipoDato.ToString() == "Lista Multiple")
                {
                    storeColumnasFK.Reload();

                    cmbForeignKey.Show();
                    cmbForeignKey.AllowBlank = false;
                    cmbForeignKey.Enable();

                    storeTablasModelosDatos.Reload();

                    cmbDataSource.Show();
                    cmbDataSource.AllowBlank = false;
                    cmbDataSource.Enable();

                    cmbDataSource.SetValue(GetGlobalResource(oDato.ClaveRecursoColumna));


                    if (oDato.ForeignKeyID.ToString() != "")
                    {
                        cmbForeignKey.SetValue(oDato.ForeignKeyNombre);
                        hdForeignKey.SetValue(oDato.ForeignKeyNombre);
                    }

                }
                else
                {
                    cmbDataSource.Hide();
                    cmbDataSource.AllowBlank = true;
                    cmbForeignKey.Hide();
                    cmbForeignKey.AllowBlank = true;
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
        public DirectResponse EliminarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalColumnasModeloDatosController CGlobalColumnasModeloDatosController = new GlobalColumnasModeloDatosController();

            direct.Result = "";
            direct.Success = true;

            long lID = Int64.Parse(GridRowSelectDetalle.SelectedRecordID);

            try
            {
                Data.ColumnasModeloDatos oDato;
                oDato = CGlobalColumnasModeloDatosController.GetItem(lID);

                if (CGlobalColumnasModeloDatosController.DeleteItem(lID))
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
            GlobalTablasModeloDatosController cProvincias = new GlobalTablasModeloDatosController();
            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);
                Data.TablasModeloDatos oDato = cProvincias.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (cProvincias.UpdateItem(oDato))
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

        [DirectMethod()]
        public DirectResponse ActivarDetalle()
        {
            DirectResponse direct = new DirectResponse();
            GlobalColumnasModeloDatosController CGlobalColumnasModeloDatosController = new GlobalColumnasModeloDatosController();

            try
            {
                long lID = long.Parse(GridRowSelectDetalle.SelectedRecordID);

                Data.ColumnasModeloDatos oDato = CGlobalColumnasModeloDatosController.GetItem(lID);
                oDato.Activo = !oDato.Activo;

                if (CGlobalColumnasModeloDatosController.UpdateItem(oDato))
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

    }

}