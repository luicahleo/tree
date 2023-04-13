using System;
using System.Collections.Generic;
using Ext.Net;
using CapaNegocio;
using TreeCore.Data;
using System.Data.SqlClient;
using log4net;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Transactions;
using System.Linq;

namespace TreeCore.ModCalidad
{
    public partial class CalidadKPI : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();
        public static List<Object> listaTablas = null;
        public static long kpi = 0;
        private const string SEPARADOR_VALORES = ", ";

        #region GESTION DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                ResourceManagerTreeCore.RegisterClientScriptBlock("Comun", Comun.cargarVariablesSTRJavascript("Comun", _Locale, false));
                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region FILTROS

                List<string> listaIgnore = new List<string>() { };

                Comun.CreateGridFilters(gridFilters, storePrincipal, grdmain.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
                }
                else
                {
                    hdCliID.Value = ClienteID;
                }

                hdLocale.Value = _Locale.ToString();
            }

            #region EXCEL
            if (Request.QueryString["opcion"] != null)
            {
                string sOpcion = Request.QueryString["opcion"];
                string sValor = Request.QueryString["aux3"];

                if (sOpcion == "EXPORTAR")
                {
                    try
                    {
                        #region KPI

                        if (sValor != "-1")
                        {
                            List<Data.Vw_DQKpis> listaDatos;
                            string sOrden = Request.QueryString["orden"];
                            string sDir = Request.QueryString["dir"];
                            string sFiltro = Request.QueryString["filtro"];
                            bool bActivo = Request.QueryString["aux"] == "true";
                            int iCount = 0;

                            listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, bActivo);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombre(grdmain.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.strCalidadKPI).ToString(), _Locale);
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

                        #endregion

                        #region PANEL LATERAL
                        else if (sValor == "-1")
                        {
                            List<object> listaVersiones;
                            string sKPI = Request.QueryString["aux"];
                            bool bActivo = Request.QueryString["aux4"] == "true";

                            listaVersiones = ListaMonitoring(sKPI, bActivo);

                            #region ESTADISTICAS
                            try
                            {
                                Comun.ExportacionDesdeListaNombreTemplate(gridVersiones.ColumnModel, listaVersiones, Response, "", GetGlobalResource(Comun.strCalidadKPI).ToString(), _Locale);
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
            { "Download", new List<ComponentBase> { btnDescargar, btnDescargarFiltro }},
            { "Post", new List<ComponentBase> { btnAnadir, btnAnadirCondition, btnAnadirFiltros, btnAnadirRule }},
            { "Put", new List<ComponentBase> { btnEditar, btnActivar, btnEditarCondition, btnActivarCondition, btnEditarFiltros, btnActivarFiltros, btnEditarRule }},
            { "Delete", new List<ComponentBase> { btnEliminar, btnEliminarCondition, btnEliminarFiltros, btnEliminarRule }}
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
                    bool bActivo = btnActivos.Pressed;
                    string sFiltro = e.Parameters["gridFilters"];

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, bActivo);

                    if (lista != null)
                    {
                        storePrincipal.DataSource = lista;

                        foreach (Vw_DQKpis oDato in lista)
                        {
                            if (oDato.ClaveRecurso != null)
                            {
                                oDato.ClaveRecurso = GetGlobalResource(oDato.ClaveRecurso);
                                oDato.AliasModulo = GetGlobalResource(oDato.AliasModulo);
                            }
                        }

                        //lista.Sort((x, y) => x.ClaveRecurso.CompareTo(y.ClaveRecurso));

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

        private List<Data.Vw_DQKpis> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, bool bActivo)
        {
            List<Data.Vw_DQKpis> listaDatos;
            CalidadKPIController cCalidad = new CalidadKPIController();
            DQKpisController cKpis = new DQKpisController();

            try
            {
                if (bActivo)
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpis>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                }
                else
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpis>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
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

        #region COLUMNAS MODELOS DATOS

        protected void storeColumnasModelosDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Vw_ColumnasModeloDatos> lista = null;
                ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
                TablasModeloDatosController cTablas = new TablasModeloDatosController();
                DQKpisController cKpis = new DQKpisController();

                try
                {
                    if (hdKPISeleccionado.Value.ToString() != "" && cmbTablasPaginasReglas.SelectedItem.Value == null && hdRuleTablaModeloDatoID.Value.ToString() == "")
                    {
                        long? lTablaID = cKpis.getTablaIDByKpi(long.Parse(hdKPISeleccionado.Value.ToString()));
                        string sNombre = cTablas.getNombreByID((long)lTablaID);
                        lista = cColumnas.GetColumnasVistaTablas(sNombre);
                    }

                    else if (hdRuleTablaModeloDatoID.Value.ToString() != "" && hdKPISeleccionado.Value.ToString() == "")
                    {
                        string sNombreTabla = cTablas.getNombreByID(long.Parse(hdRuleTablaModeloDatoID.Value.ToString()));
                        lista = cColumnas.GetColumnasVistaTablas(sNombreTabla);
                    }

                    else if (cmbTablasPaginasReglas.SelectedItem != null && cmbTablasPaginasReglas.SelectedItem.Value != null && cmbTablasPaginasReglas.SelectedItem.Value != "")
                    {
                        string sNombreTabla = cTablas.getNombreByID(long.Parse(cmbTablasPaginasReglas.SelectedItem.Value));
                        lista = cColumnas.GetColumnasVistaTablas(sNombreTabla);
                    }

                    if (lista != null)
                    {
                        storeColumnasModelosDatos.DataSource = lista;

                        foreach (Vw_ColumnasModeloDatos oDato in lista)
                        {
                            if (oDato.ClaveRecursoColumna != null)
                            {
                                oDato.ClaveRecursoColumna = GetGlobalResource(oDato.ClaveRecursoColumna);
                            }
                        }

                        lista.Sort((x, y) => x.ClaveRecursoColumna.CompareTo(y.ClaveRecursoColumna));
                    }

                    storeColumnasModelosDatos.DataBind();
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

        #region CATEGORIAS

        protected void storeCategorias_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<DQCategorias> lista = null;
                DQCategoriasController cCategorias = new DQCategoriasController();

                try
                {
                    lista = cCategorias.GetAllActivos(long.Parse(hdCliID.Value.ToString()));

                    if (lista != null)
                    {
                        storeCategorias.DataSource = lista;
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

        #region SEMAFOROS

        protected void storeSemaforos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<DQSemaforos> lista = null;
                DQSemaforosController cSemaforos = new DQSemaforosController();

                try
                {
                    lista = cSemaforos.GetAllSemaforosActivos(true);

                    if (lista != null)
                    {
                        storeSemaforos.DataSource = lista;
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

        #region TIPOS DATOS OPERADORES

        protected void storeTiposDatosOperadores_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Vw_TiposDatosOperadores> lista = null;
                ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
                TiposDatosOperadoresController cOperadores = new TiposDatosOperadoresController();
                long lTipoDatoID = 0;

                try
                {
                    if (cmbColumnasFiltro.SelectedItem != null && cmbColumnasFiltro.SelectedItem.Value != null && cmbColumnasFiltro.SelectedItem.Value != "")
                    {
                        lTipoDatoID = (long)cColumnas.getTipoDatoByColumna(long.Parse(cmbColumnasFiltro.SelectedItem.Value));
                        lista = cOperadores.getOperadorByTipoDato(lTipoDatoID);
                    }
                    else if (cmbColumnasReglas.SelectedItem != null && cmbColumnasReglas.SelectedItem.Value != null && cmbColumnasReglas.SelectedItem.Value != "")
                    {
                        lTipoDatoID = (long)cColumnas.getTipoDatoByColumna(long.Parse(cmbColumnasReglas.SelectedItem.Value));
                        lista = cOperadores.getOperadorByTipoDato(lTipoDatoID);
                    }
                    else
                    {
                        lista = cOperadores.getAllOperadoresActivos();
                    }

                    if (lista != null)
                    {
                        lista.Sort((x, y) => x.Nombre.CompareTo(y.Nombre));
                        storeTiposDatosOperadores.DataSource = lista;
                        storeTiposDatosOperadores.DataBind();
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

        #region GRUPOS
        protected void storeDQGroups_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = ListaGrupos();

                    if (lista != null)
                    {
                        storeDQGroups.DataSource = lista;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_DQGroups> ListaGrupos()
        {
            List<Data.Vw_DQGroups> listaDatos;
            DQGroupsController cCalidad = new DQGroupsController();

            try
            {
                listaDatos = cCalidad.getAllGroupActivos();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return listaDatos;
        }
        #endregion

        #region KPI GROUP

        protected void storeDQKpiGroups_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersCondition"];

                    if (hdKPISeleccionado.Value.ToString() != "")
                    {
                        var lista = ListaKPIGrupos(e.Start, e.Limit, "", "", ref iCount, sFiltro, long.Parse(hdKPISeleccionado.Value.ToString()));

                        if (lista != null)
                        {
                            storeDQKpiGroups.DataSource = lista;

                            PageProxy temp = (PageProxy)storeDQKpiGroups.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_DQKpisGroups> ListaKPIGrupos(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lKPI)
        {
            List<Data.Vw_DQKpisGroups> listaDatos;
            DQKpisGroupsController cCalidad = new DQKpisGroupsController();

            try
            {
                if (btnActivosCondition.Pressed)
                {
                    if (lKPI != 0)
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroups>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "DQKpiID == " + lKPI);
                    }
                    else
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroups>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                    }
                }
                else
                {
                    if (lKPI != 0)
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroups>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && DQKpiID == " + lKPI);
                    }
                    else
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroups>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
                    }

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

        #region KPI GROUP RULES

        protected void storeDQKpisGroupsRules_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersRule"];

                    if (hdGroupSeleccionado.Value.ToString() != "")
                    {
                        var lista = ListaKPIRules(e.Start, e.Limit, "", "", ref iCount, sFiltro, long.Parse(hdGroupSeleccionado.Value.ToString()));

                        if (lista != null)
                        {
                            storeDQKpisGroupsRules.DataSource = lista;

                            PageProxy temp = (PageProxy)storeDQKpisGroupsRules.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_DQKpisGroupsRules> ListaKPIRules(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lGroupID)
        {
            List<Data.Vw_DQKpisGroupsRules> listaDatos;
            DQKpisGroupsRulesController cCalidad = new DQKpisGroupsRulesController();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            DQGroupsController cGrupos = new DQGroupsController();

            try
            {

                if (lGroupID != 0)
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroupsRules>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && DQKpiGroupID == " + lGroupID);
                }
                else
                {
                    listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisGroupsRules>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
                }

                if (listaDatos != null)
                {
                    foreach (Data.Vw_DQKpisGroupsRules oDato in listaDatos)
                    {
                        if (oDato.Codigo == "LISTA" || oDato.Codigo == "LISTAMULTIPLE")
                        {
                            TiposDatosOperadoresController cTiposDatosOperadores = new TiposDatosOperadoresController();
                            TiposDatosOperadores tipOperador = cTiposDatosOperadores.GetItem(oDato.TipoDatoOperadorID);

                            if (tipOperador != null && tipOperador.RequiereValor)
                            {
                                string sNombreTabla = cColumnas.getDataSourceTablaColumna(oDato.ColumnaModeloDatosID);
                                string sControlador = cColumnas.getControllerColumna(oDato.ColumnaModeloDatosID);
                                string sDisplay = cColumnas.getDisplay(oDato);

                                Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                                Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                                if (tipo == null)
                                {
                                    tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                                }

                                if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                                {
                                    tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                                }

                                var instance = Activator.CreateInstance(tipoControlador);
                                MethodInfo[] methodos = tipoControlador.GetMethods();

                                foreach (MethodInfo met in methodos)
                                {
                                    if (met.Name == "GetItem" && met.ReturnType == tipo)
                                    {
                                        var Params = met.GetParameters();

                                        if (Params[0].ParameterType.Name == "Int64")
                                        {
                                            if (oDato.Codigo == "LISTAMULTIPLE")
                                            {
                                                if (oDato.IsDataTable)
                                                {
                                                    sNombreTabla = cTablas.getClaveByID(long.Parse(oDato.Valor));
                                                    oDato.Valor = GetGlobalResource(sNombreTabla);
                                                }
                                                else
                                                {
                                                    string[] listaID = oDato.Valor.Split(',');
                                                    string sNombre = "";

                                                    foreach (string sItem in listaID)
                                                    {
                                                        object result = met.Invoke(instance, new object[] { long.Parse(sItem) });
                                                        PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                        if (propName != null && result != null)
                                                        {
                                                            string sName = (string)propName.GetValue(result, null);

                                                            if (sNombre != "")
                                                            {
                                                                sNombre += ", " + sName;
                                                            }
                                                            else
                                                            {
                                                                sNombre = sName;
                                                            }
                                                        }

                                                        oDato.Valor = sNombre;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                object result = met.Invoke(instance, new object[] { long.Parse(oDato.Valor) });
                                                PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                if (propName != null)
                                                {
                                                    string sName = (string)propName.GetValue(result, null);
                                                    oDato.Valor = sName;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                oDato.Valor = "";
                            }
                        }

                        oDato.NombreTabla = cColumnas.getClaveRecursoTabla(oDato.ColumnaModeloDatosID);
                    }


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

        #region TABLAS PAGINAS

        protected void storeDQTablasPaginas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                List<Data.Vw_DQTablasPaginas> listaDatos;
                DQTablasPaginasController cCalidad = new DQTablasPaginasController();

                try
                {
                    listaDatos = cCalidad.getTablasActivas();

                    if (listaDatos != null)
                    {
                        storeDQTablasPaginas.DataSource = listaDatos;

                        foreach (Vw_DQTablasPaginas oDato in listaDatos)
                        {
                            if (oDato.ClaveRecurso != null)
                            {
                                oDato.ClaveRecurso = GetGlobalResource(oDato.ClaveRecurso);
                            }
                            else
                            {
                                oDato.ClaveRecurso = oDato.Alias;
                            }
                        }
                        listaDatos.Sort((x, y) => x.ClaveRecurso.CompareTo(y.ClaveRecurso));
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TABLAS MODELOS DATOS

        protected void storeTablasModelosDatos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
                TablasModeloDatosController cTablas = new TablasModeloDatosController();
                List<TablasModeloDatos> lista = null;
                long? lColumnaID;
                long lTablaID;

                try
                {
                    if (cmbColumnasFiltro.SelectedItem != null && cmbColumnasFiltro.SelectedItem.Value != null && cmbColumnasFiltro.SelectedItem.Value != "")
                    {
                        lColumnaID = cColumnas.getForeignKeyID(long.Parse(cmbColumnasFiltro.SelectedItem.Value));
                        lTablaID = cColumnas.getTablaByColumna(long.Parse(cmbColumnasFiltro.SelectedItem.Value));
                        lista = cTablas.getTablasByColumnaID(lColumnaID, lTablaID);
                    }
                    else if (cmbColumnasReglas.SelectedItem != null && cmbColumnasReglas.SelectedItem.Value != null && cmbColumnasReglas.SelectedItem.Value != "")
                    {
                        lColumnaID = cColumnas.getForeignKeyID(long.Parse(cmbColumnasReglas.SelectedItem.Value));
                        lTablaID = cColumnas.getTablaByColumna(long.Parse(cmbColumnasReglas.SelectedItem.Value));
                        lista = cTablas.getTablasByColumnaID(lColumnaID, lTablaID);
                    }

                    if (lista != null)
                    {
                        storeTablasModelosDatos.DataSource = lista;

                        foreach (TablasModeloDatos oDato in lista)
                        {
                            if (oDato.ClaveRecurso != null)
                            {
                                oDato.ClaveRecurso = GetGlobalResource(oDato.ClaveRecurso);
                            }
                        }
                    }

                    storeTablasModelosDatos.DataBind();
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

        #region KPI FILTROS

        protected void storeDQKpisFiltros_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    int iCount = 0;
                    string sFiltro = e.Parameters["gridFiltersFiltro"];

                    if (hdKPISeleccionado.Value.ToString() != "")
                    {
                        var lista = ListaKPIFiltros(e.Start, e.Limit, "", "", ref iCount, sFiltro, long.Parse(hdKPISeleccionado.Value.ToString()));

                        if (lista != null)
                        {
                            storeDQKpisFiltros.DataSource = lista;

                            PageProxy temp = (PageProxy)storeDQKpisFiltros.Proxy[0];
                            temp.Total = iCount;
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_DQKpisFiltros> ListaKPIFiltros(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long lKPI)
        {
            List<Data.Vw_DQKpisFiltros> listaDatos;
            DQKpisFiltrosController cCalidad = new DQKpisFiltrosController();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();

            try
            {
                if (btnActivosFiltros.Pressed)
                {
                    if (lKPI != 0)
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisFiltros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "DQKpiID == " + lKPI);
                    }
                    else
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisFiltros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                    }
                }
                else
                {
                    if (lKPI != 0)
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisFiltros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true && DQKpiID == " + lKPI);
                    }
                    else
                    {
                        listaDatos = cCalidad.GetItemsWithExtNetFilterList<Data.Vw_DQKpisFiltros>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount, "Activo == true");
                    }

                }

                if (listaDatos != null)
                {
                    foreach (Data.Vw_DQKpisFiltros oDato in listaDatos)
                    {
                        if (oDato.Codigo == "LISTA" || oDato.Codigo == "LISTAMULTIPLE")
                        {
                            TiposDatosOperadoresController cTiposDatosOperadores = new TiposDatosOperadoresController();
                            TiposDatosOperadores tipOperador = cTiposDatosOperadores.GetItem(oDato.TipoDatoOperadorID);

                            if (tipOperador != null && tipOperador.RequiereValor)
                            {
                                string sNombreTabla = cColumnas.getDataSourceTablaColumna(oDato.ColumnaModeloDatoID);
                                string sControlador = cColumnas.getControllerColumna(oDato.ColumnaModeloDatoID);
                                string sDisplay = cColumnas.getDisplayFiltro(oDato);

                                Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                                Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                                if (tipo == null)
                                {
                                    tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                                }

                                if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                                {
                                    tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                                }

                                var instance = Activator.CreateInstance(tipoControlador);
                                MethodInfo[] methodos = tipoControlador.GetMethods();

                                foreach (MethodInfo met in methodos)
                                {
                                    if (met.Name == "GetItem" && met.ReturnType == tipo)
                                    {
                                        var Params = met.GetParameters();

                                        if (Params[0].ParameterType.Name == "Int64")
                                        {
                                            if (oDato.Codigo == "LISTAMULTIPLE")
                                            {
                                                if (oDato.IsDataTable)
                                                {
                                                    sNombreTabla = cTablas.getClaveByID(long.Parse(oDato.Valor));
                                                    oDato.Valor = GetGlobalResource(sNombreTabla);
                                                }
                                                else
                                                {
                                                    string[] listaID = oDato.Valor.Split(',');
                                                    string sNombre = "";

                                                    foreach (string sItem in listaID)
                                                    {
                                                        object result = met.Invoke(instance, new object[] { long.Parse(sItem) });
                                                        PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                        if (propName != null && result != null)
                                                        {
                                                            string sName = (string)propName.GetValue(result, null);

                                                            if (sNombre != "")
                                                            {
                                                                sNombre += ", " + sName;
                                                            }
                                                            else
                                                            {
                                                                sNombre = sName;
                                                            }
                                                        }

                                                        oDato.Valor = sNombre;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                object result = met.Invoke(instance, new object[] { long.Parse(oDato.Valor) });
                                                PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                if (propName != null)
                                                {
                                                    string sName = (string)propName.GetValue(result, null);
                                                    oDato.Valor = sName;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                oDato.Valor = "";
                            }
                        }
                    }
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

        #region TIPOS DINAMICOS

        protected void storeTiposDinamicos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            List<TipoDinamico> tiposDinamicos;
            string sTipoDato;

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    sTipoDato = cColumnas.getTipoDatoNombreByColumna(long.Parse(cmbColumnasReglas.SelectedItem.Value));
                    long lSeleccion = long.Parse(cmbColumnasReglas.SelectedItem.Value);
                    string sNombreTabla = cColumnas.getDataSourceTablaColumna(lSeleccion);
                    string sControlador = cColumnas.getControllerColumna(lSeleccion);
                    string sIndice = cColumnas.getIndiceColumna(lSeleccion);
                    string sColumna = cColumnas.getNombreColumnaByTabla(lSeleccion);
                    string sTraduccion = cColumnas.getColumnaByTabla(lSeleccion);
                    string sTexto = GetGlobalResource(sTraduccion);

                    cmbTiposDinamicosReglas.FieldLabel = sTexto;
                    cmbTiposDinamicosReglas.SetEmptyText(sTexto);

                    Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                    Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                    if (tipo == null)
                    {
                        tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                    }

                    if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                    {
                        tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                    }

                    var instance = Activator.CreateInstance(tipoControlador);
                    Type[] tipos = new Type[1];
                    tipos[0] = typeof(long);
                    MethodInfo method = tipoControlador.GetMethod("GetActivos", tipos);
                    dynamic list = method.Invoke(instance, new object[] { ClienteID });
                    tiposDinamicos = new List<TipoDinamico>();

                    foreach (Object obj in list)
                    {
                        if (sColumna != null && sIndice != null)
                        {
                            PropertyInfo propID = tipo.GetProperty(sIndice);
                            PropertyInfo propName = tipo.GetProperty(sColumna);

                            if (propID != null && propName != null)
                            {
                                long lID = (long)propID.GetValue(obj, null);
                                string sName = (string)propName.GetValue(obj, null);

                                tiposDinamicos.Add(new TipoDinamico(sName, lID));
                            }
                        }
                    }

                    tiposDinamicos.Sort((x, y) => x.Name.CompareTo(y.Name));

                    if (sTipoDato == Comun.TIPODATO_CODIGO_LISTA)
                    {
                        Store store = cmbTiposDinamicosReglas.GetStore();
                        cmbTiposDinamicosReglas.FieldLabel = sTexto;
                        cmbTiposDinamicosReglas.SetEmptyText(sTexto);

                        store.DataSource = tiposDinamicos;
                        store.DataBind();
                    }
                    else if (sTipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        Store store = cmbMultiTiposDinamicosReglas.GetStore();
                        cmbMultiTiposDinamicosReglas.FieldLabel = sTexto;
                        cmbMultiTiposDinamicosReglas.SetEmptyText(sTexto);

                        store.DataSource = tiposDinamicos;
                        store.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS DINAMICOS FILTROS

        protected void storeTiposDinamicosFiltros_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            List<TipoDinamico> tiposDinamicos;
            string sTipoDato;

            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    sTipoDato = cColumnas.getTipoDatoNombreByColumna(long.Parse(cmbColumnasFiltro.SelectedItem.Value));
                    long lSeleccion = long.Parse(cmbColumnasFiltro.SelectedItem.Value);
                    string sNombreTabla = cColumnas.getDataSourceTablaColumna(lSeleccion);
                    string sControlador = cColumnas.getControllerColumna(lSeleccion);
                    string sIndice = cColumnas.getIndiceColumna(lSeleccion);
                    string sColumna = cColumnas.getNombreColumnaByTabla(lSeleccion);
                    string sTraduccion = cColumnas.getColumnaByTabla(lSeleccion);
                    string sTexto = GetGlobalResource(sTraduccion);

                    Type tipo = Type.GetType("TreeCore.Data." + sNombreTabla.Split('.')[1]);
                    Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                    if (tipo == null)
                    {
                        tipo = Type.GetType("TreeCore.Data.Vw_" + sNombreTabla.Split('_')[1]);
                    }

                    if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                    {
                        tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                    }

                    var instance = Activator.CreateInstance(tipoControlador);
                    Type[] tipos = new Type[1];
                    tipos[0] = typeof(long);
                    MethodInfo method = tipoControlador.GetMethod("GetActivos", tipos);
                    dynamic list = method.Invoke(instance, new object[] { ClienteID });
                    tiposDinamicos = new List<TipoDinamico>();

                    foreach (Object obj in list)
                    {
                        if (sColumna != null && sIndice != null)
                        {
                            PropertyInfo propID = tipo.GetProperty(sIndice);
                            PropertyInfo propName = tipo.GetProperty(sColumna);

                            if (propID != null && propName != null)
                            {
                                long lID = (long)propID.GetValue(obj, null);
                                string sName = (string)propName.GetValue(obj, null);

                                tiposDinamicos.Add(new TipoDinamico(sName, lID));
                            }
                        }
                    }

                    tiposDinamicos.Sort((x, y) => x.Name.CompareTo(y.Name));

                    if (sTipoDato == Comun.TIPODATO_CODIGO_LISTA)
                    {
                        Store store = cmbTiposDinamicosFiltro.GetStore();
                        cmbTiposDinamicosFiltro.FieldLabel = sTexto;
                        cmbTiposDinamicosFiltro.SetEmptyText(sTexto);

                        store.DataSource = tiposDinamicos;
                        store.DataBind();
                    }
                    else if (sTipoDato == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        Store store = cmbMultiTiposDinamicosFiltro.GetStore();
                        cmbMultiTiposDinamicosFiltro.FieldLabel = sTexto;
                        cmbMultiTiposDinamicosFiltro.SetEmptyText(sTexto);

                        store.DataSource = tiposDinamicos;
                        store.DataBind();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region KPIS MONITORING

        protected void storeDQKpisMonitoring_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<object> listaSem = new List<object>();
                    bool bActivo = btnActivosPanel.Pressed;
                    string sCurrent = "";
                    string sColor = "";
                    int? iCuentaCurrent;

                    if (hdKPISeleccionado.Value.ToString() != "")
                    {
                        var lista = ListaMonitoring(hdKPISeleccionado.Value.ToString(), bActivo);

                        if (lista != null)
                        {
                            storeDQKpisMonitoring.DataSource = lista;
                            storeDQKpisMonitoring.DataBind();

                            foreach (Vw_DQKpisMonitoring oDat in lista)
                            {
                                if (oDat.Ultima)
                                {
                                    if (oDat.NumeroElementos > 0)
                                    {
                                        sCurrent = oDat.PorcentajeError;
                                    }
                                    else if (oDat.NumeroElementos == -1)
                                    {
                                        sCurrent = GetGlobalResource("jsError");
                                    }
                                    else
                                    {
                                        sCurrent = "0%";
                                    }

                                    if (sCurrent != null && sCurrent != "0%")
                                    {
                                        if (oDat.NumeroElementos == -1)
                                        {
                                            iCuentaCurrent = null;
                                        }
                                        else
                                        {
                                            iCuentaCurrent = ((oDat.NumeroElementos * 100) / oDat.Total);
                                        }

                                        if (iCuentaCurrent < oDat.IntervaloVerde)
                                        {
                                            sColor = "green";
                                        }
                                        else if (iCuentaCurrent > oDat.IntervaloRojo)
                                        {
                                            sColor = "red";
                                        }
                                        else if (iCuentaCurrent == null)
                                        {
                                            sColor = "error";
                                        }
                                        else
                                        {
                                            sColor = "yellow";
                                        }
                                    }

                                    Object oSem = new
                                    {
                                        DQKpiMonitoringID = oDat.DQKpiMonitoringID,
                                        DQKpi = oDat.DQKpi,
                                        Current = sCurrent,
                                        IntervaloVerde = oDat.IntervaloVerde,
                                        IntervaloRojo = oDat.IntervaloRojo,
                                        Colour = sColor,
                                    };

                                    listaSem.Add(oSem);
                                }
                            }

                            storeDQKpisMonitoring.DataSource = listaSem;
                            storeDQKpisMonitoring.DataBind();
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Object> ListaMonitoring(string sKPI, bool bActivo)
        {
            List<Data.Vw_DQKpisMonitoring> lista;
            List<object> listaDatos = new List<object>();
            DQKpisMonitoringController cCalidad = new DQKpisMonitoringController();
            string sCurrent = "";
            string sColor = "";
            int? iCuentaCurrent;

            try
            {
                if (sKPI != null)
                {
                    if (!bActivo)
                    {
                        lista = cCalidad.getByKPIActivos(long.Parse(sKPI));
                    }
                    else
                    {
                        lista = cCalidad.getByKPI(long.Parse(sKPI));
                    }

                    if (lista.Count != 0)
                    {
                        foreach (Vw_DQKpisMonitoring oDato in lista)
                        {
                            if (oDato.NumeroElementos > 0)
                            {
                                sCurrent = oDato.PorcentajeError;
                            }
                            else if (oDato.NumeroElementos == -1)
                            {
                                sCurrent = GetGlobalResource("jsError");
                            }
                            else
                            {
                                sCurrent = "0%";
                            }

                            if (sCurrent != null && sCurrent != "0%")
                            {
                                if (oDato.NumeroElementos == -1)
                                {
                                    iCuentaCurrent = null;
                                }
                                else
                                {
                                    iCuentaCurrent = ((oDato.NumeroElementos * 100) / oDato.Total);
                                }

                                if (iCuentaCurrent < oDato.IntervaloVerde)
                                {
                                    sColor = "green";
                                }
                                else if (iCuentaCurrent > oDato.IntervaloRojo)
                                {
                                    sColor = "red";
                                }
                                else if (iCuentaCurrent == null)
                                {
                                    sColor = "error";
                                }
                                else
                                {
                                    sColor = "yellow";
                                }
                            }

                            Object oMonitoring = new
                            {
                                DQKpiMonitoringID = oDato.DQKpiMonitoringID,
                                DQKpi = oDato.DQKpi,
                                Current = sCurrent,
                                Colour = sColor,
                                FechaEjecucion = oDato.FechaEjecucion,
                                Activa = oDato.Activa,
                                NombreCompleto = oDato.NombreCompleto,
                                Version = oDato.Version,
                                Ultima = oDato.Ultima
                            };

                            listaDatos.Add(oMonitoring);
                        }
                    }
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

        #region KPIS SEMAFOROS PANEL

        protected void storeKpiSemaforos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    if (hdKPISeleccionado.Value.ToString() != "")
                    {
                        var lista = ListaSemaforos(hdKPISeleccionado.Value.ToString());

                        if (lista != null)
                        {
                            storeKpiSemaforos.DataSource = lista;
                            storeKpiSemaforos.DataBind();
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Object> ListaSemaforos(string sKPI)
        {
            List<Data.Vw_DQKpisMonitoring> lista;
            List<object> listaDatos = new List<object>();
            List<string> listaSemaforos = new List<string>() { "0% - ", "", " - 100%" };
            DQKpisMonitoringController cCalidad = new DQKpisMonitoringController();
            string sColor = "";
            string sPorcentaje = "";
            int i = 1;
            int? iCuentaCurrent = 0;

            try
            {
                if (sKPI != null)
                {
                    lista = cCalidad.getByKPI(long.Parse(sKPI));

                    if (lista.Count != 0)
                    {
                        foreach (Vw_DQKpisMonitoring oDat in lista)
                        {
                            if (oDat.Ultima)
                            {
                                foreach (string sDato in listaSemaforos)
                                {
                                    if (oDat.NumeroElementos > 0)
                                    {
                                        iCuentaCurrent = ((oDat.NumeroElementos * 100) / oDat.Total);
                                    }
                                    else if (oDat.NumeroElementos == -1)
                                    {
                                        iCuentaCurrent = null;
                                    }

                                    if (sDato == "0% - ")
                                    {
                                        sPorcentaje = sDato + oDat.IntervaloVerde + "%";
                                        sColor = "green-empty";

                                        if (iCuentaCurrent < oDat.IntervaloVerde)
                                        {
                                            sColor = "green";
                                        }
                                        else if (iCuentaCurrent == null)
                                        {
                                            sColor = "error";
                                        }
                                    }
                                    else if (sDato == "")
                                    {
                                        sPorcentaje = oDat.IntervaloVerde + "% - " + oDat.IntervaloRojo + "%";
                                        sColor = "yellow-empty";

                                        if (iCuentaCurrent > oDat.IntervaloVerde && iCuentaCurrent < oDat.IntervaloRojo)
                                        {
                                            sColor = "yellow";
                                        }
                                        else if (iCuentaCurrent == null)
                                        {
                                            sColor = "error";
                                        }
                                    }
                                    else if (sDato == " - 100%")
                                    {
                                        sPorcentaje = oDat.IntervaloRojo + "%" + sDato;
                                        sColor = "red-empty";

                                        if (iCuentaCurrent > oDat.IntervaloRojo)
                                        {
                                            sColor = "red";
                                        }
                                        else if (iCuentaCurrent == null)
                                        {
                                            sColor = "error";
                                        }
                                    }

                                    Object oSem = new
                                    {
                                        DQKpiID = i,
                                        DQKpi = oDat.DQKpi,
                                        Porcentaje = sPorcentaje,
                                        Colour = sColor
                                    };

                                    listaDatos.Add(oSem);
                                    i++;
                                }
                            }
                        }
                    }
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

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            CalidadKPIController cCalidad = new CalidadKPIController();
            DQTablasPaginasController cTablas = new DQTablasPaginasController();
            CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            cCoreServiciosFrecuencias.SetDataContext(cCalidad.Context);

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try

                {
                    if (!agregar)
                    {
                        bool actuKPI;
                        bool actuFrecu;
                        long lIdSelect = long.Parse(GridRowSelect.SelectedRecordID);

                        Data.DQKpis oDato;
                        oDato = cCalidad.GetItem(lIdSelect);

                        CoreServiciosFrecuencias fDato;
                        fDato = cCoreServiciosFrecuencias.GetItem(long.Parse(oDato.CoreServicioFrecuenciaID.ToString()));

                        if (oDato.DQKpi == txtName.Text)
                        {
                            oDato.DQKpi = txtName.Text;
                            oDato.CreadorID = Usuario.UsuarioID;
                            oDato.FechaModificacion = DateTime.Now;
                            oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                            oDato.FechaUltimaModificacion = DateTime.Now;

                            if (cmbCategory.SelectedItem != null && cmbCategory.SelectedItem.Value != null && cmbCategory.SelectedItem.Value != "")
                            {
                                oDato.DQCategoriaID = long.Parse(cmbCategory.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.DQCategoriaID = null;
                            }

                            if (cmbTraffic.SelectedItem != null && cmbTraffic.SelectedItem.Value != null && cmbTraffic.SelectedItem.Value != "")
                            {
                                oDato.DQSemaforoID = long.Parse(cmbTraffic.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.DQSemaforoID = null;
                            }

                            if (cmbTablasPaginas.SelectedItem != null && cmbTablasPaginas.SelectedItem.Value != null && cmbTablasPaginas.SelectedItem.Value != "")
                            {
                                oDato.DQTablaPaginaID = long.Parse(cmbTablasPaginas.SelectedItem.Value);
                            }
                            else
                            {
                                oDato.DQTablaPaginaID = null;
                            }
                        }
                        else
                        {
                            if (cCalidad.RegistroDuplicado(txtName.Text))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            }
                            else
                            {
                                oDato.DQKpi = txtName.Text;
                                oDato.CreadorID = Usuario.UsuarioID;
                                oDato.Activo = true;
                                oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                                oDato.FechaModificacion = DateTime.Now;
                                oDato.FechaUltimaModificacion = DateTime.Now;

                                if (cmbCategory.SelectedItem != null && cmbCategory.SelectedItem.Value != null && cmbCategory.SelectedItem.Value != "")
                                {
                                    oDato.DQCategoriaID = long.Parse(cmbCategory.SelectedItem.Value.ToString());
                                }
                                else
                                {
                                    oDato.DQCategoriaID = null;
                                }

                                if (cmbTraffic.SelectedItem != null && cmbTraffic.SelectedItem.Value != null && cmbTraffic.SelectedItem.Value != "")
                                {
                                    oDato.DQSemaforoID = long.Parse(cmbTraffic.SelectedItem.Value.ToString());
                                }
                                else
                                {
                                    oDato.DQSemaforoID = null;
                                }

                                if (cmbTablasPaginas.SelectedItem != null && cmbTablasPaginas.SelectedItem.Value != null && cmbTablasPaginas.SelectedItem.Value != "")
                                {
                                    oDato.DQTablaPaginaID = long.Parse(cmbTablasPaginas.SelectedItem.Value);
                                }
                                else
                                {
                                    oDato.DQTablaPaginaID = null;
                                }
                            }
                        }

                        actuKPI = cCalidad.UpdateItem(oDato);

                        fDato.Nombre = fDato.Nombre;
                        fDato.FechaInicio = ProgramadorKPI.FechaInicio;
                        if (ProgramadorKPI.Frecuencias == "NoSeRepite")
                        {
                            fDato.FechaFin = null;
                        }
                        else
                        {
                            if (fDato.FechaFin != null)
                            {
                                fDato.FechaFin = ProgramadorKPI.FechaFin;
                            }
                        }

                        fDato.TipoFrecuencia = ProgramadorKPI.Frecuencias;
                        fDato.CronFormat = ProgramadorKPI.CronFormat;

                        actuFrecu = cCoreServiciosFrecuencias.UpdateItem(fDato);

                        if (actuFrecu != null && actuKPI != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                            trans.Complete();
                        }
                        else if (actuFrecu != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                            trans.Complete();
                        }
                        else if (actuKPI != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storePrincipal.DataBind();
                            trans.Complete();
                        }
                        else
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                    else
                    {
                        if (cCalidad.RegistroDuplicado(txtName.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                        }
                        else
                        {
                            Data.CoreServiciosFrecuencias oFrecuencia = new Data.CoreServiciosFrecuencias();

                            oFrecuencia.Nombre = "Cron_" + txtName.Text;
                            oFrecuencia.Activo = true;
                            oFrecuencia.FechaInicio = ProgramadorKPI.FechaInicio;
                            oFrecuencia.CronFormat = ProgramadorKPI.CronFormat;
                            oFrecuencia.TipoFrecuencia = ProgramadorKPI.Frecuencias;

                            if (ProgramadorKPI.FechaFin != DateTime.MinValue)
                            {
                                oFrecuencia.FechaFin = ProgramadorKPI.FechaFin;
                            }
                            oFrecuencia = cCoreServiciosFrecuencias.AddItem(oFrecuencia);

                            if (oFrecuencia != null)
                            {
                                log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            }


                            Data.DQKpis oDato = new Data.DQKpis();
                            oDato.DQKpi = txtName.Text;
                            oDato.Activo = true;
                            oDato.ClienteID = long.Parse(hdCliID.Value.ToString());
                            oDato.CreadorID = Usuario.UsuarioID;
                            oDato.CoreServicioFrecuenciaID = oFrecuencia.CoreServicioFrecuenciaID;
                            oDato.FechaModificacion = DateTime.Now;
                            oDato.FechaUltimaModificacion = DateTime.Now;

                            if (cmbCategory.SelectedItem.Value != null && cmbCategory.SelectedItem.Value != "")
                            {
                                oDato.DQCategoriaID = long.Parse(cmbCategory.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.DQCategoriaID = null;
                            }

                            if (cmbTraffic.SelectedItem.Value != null && cmbTraffic.SelectedItem.Value != "")
                            {
                                oDato.DQSemaforoID = long.Parse(cmbTraffic.SelectedItem.Value.ToString());
                            }
                            else
                            {
                                oDato.DQSemaforoID = null;
                            }

                            if (cmbTablasPaginas.SelectedItem != null && cmbTablasPaginas.SelectedItem.Value != null && cmbTablasPaginas.SelectedItem.Value != "")
                            {
                                oDato.DQTablaPaginaID = long.Parse(cmbTablasPaginas.SelectedItem.Value);
                            }
                            else
                            {
                                oDato.DQTablaPaginaID = null;
                            }

                            if (cCalidad.AddItem(oDato) != null)
                            {
                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                storePrincipal.DataBind();
                                trans.Complete();
                            }
                            else
                            {
                                trans.Dispose();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                                return direct;
                            }

                        }
                    }
                }

                catch (Exception ex)
                {
                    trans.Dispose();
                    if (ex is SqlException Sql)
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
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
            }

            direct.Success = true;
            direct.Result = "";

            return direct;
        }

        [DirectMethod()]
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            CalidadKPIController cKPI = new CalidadKPIController();
            CoreServiciosFrecuenciasController cServiciosFrecuencias = new CoreServiciosFrecuenciasController();

            try
            {
                long S = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DQKpis oDato;
                oDato = cKPI.GetItem(S);
                txtName.Text = oDato.DQKpi;
                cmbCategory.SetValue(oDato.DQCategoriaID);
                cmbTraffic.SetValue(oDato.DQSemaforoID);
                cmbTablasPaginas.SetValue(oDato.DQTablaPaginaID);


                #region Frecuencia
                //FRECUENCIAS
                if (oDato.CoreServicioFrecuenciaID.HasValue)
                {
                    Data.CoreServiciosFrecuencias oFrecuencias = cServiciosFrecuencias.GetItem(oDato.CoreServicioFrecuenciaID.Value);

                    ProgramadorKPI.Frecuencias = oFrecuencias.TipoFrecuencia;
                    ProgramadorKPI.FechaInicio = oFrecuencias.FechaInicio;

                    if (oFrecuencias.FechaFin != null)
                    {
                        ProgramadorKPI.FechaFin = (DateTime)oFrecuencias.FechaFin;
                    }

                    if (oFrecuencias.TipoFrecuencia == "SemanalCustom")
                    {

                        string[] cron = oFrecuencias.CronFormat.Split(' ');
                        string diasSemana = cron.Last();
                        string[] Dias = diasSemana.Split(',');
                        //string diasFormateado = "";
                        //foreach (var dia in Dias)
                        //{
                        //    diasFormateado = (diasFormateado + "'"  + dia + "', ");
                        //}
                        //diasFormateado = diasFormateado.Remove((diasFormateado.Length - 1), 1);
                        //diasFormateado = diasFormateado.Remove((diasFormateado.Length - 1), 1);

                        ProgramadorKPI.Dias = Dias.ToList();

                        //foreach (var item in ProgramadorKPI.Dias)
                        //{
                        //}
                    }

                    if (oFrecuencias.TipoFrecuencia == "MensualCustom")
                    {

                        string[] cron = oFrecuencias.CronFormat.Split(' ');
                        string diaMes = cron[2];
                        ProgramadorKPI.DiaCadaMes = long.Parse(diaMes);
                        string meses = cron[3];

                        if (meses.Contains("/"))
                        {
                            //long numeroMes = 0;

                            string[] separador = meses.Split('/');
                            //if (separador[0] == "January" || separador[0] == "1")
                            //{
                            //    numeroMes = 1;
                            //}
                            //else if (separador[0] == "February" || separador[0] == "2")
                            //{
                            //    numeroMes = 2;
                            //}
                            //else if (separador[0] == "March" || separador[0] == "3")
                            //{
                            //    numeroMes = 3;
                            //}
                            //else if (separador[0] == "April" || separador[0] == "4")
                            //{
                            //    numeroMes = 4;
                            //}
                            //else if (separador[0] == "May" || separador[0] == "5")
                            //{
                            //    numeroMes = 5;
                            //}
                            //else if (separador[0] == "June" || separador[0] == "6")
                            //{
                            //    numeroMes = 6;
                            //}
                            //else if (separador[0] == "July" || separador[0] == "7")
                            //{
                            //    numeroMes = 7;
                            //}
                            //else if (separador[0] == "August" || separador[0] == "8")
                            //{
                            //    numeroMes = 8;
                            //}
                            //else if (separador[0] == "September" || separador[0] == "9")
                            //{
                            //    numeroMes = 9;
                            //}
                            //else if (separador[0] == "October" || separador[0] == "10")
                            //{
                            //    numeroMes = 10;
                            //}
                            //else if (separador[0] == "November" || separador[0] == "11")
                            //{
                            //    numeroMes = 11;
                            //}
                            //else if (separador[0] == "December" || separador[0] == "12")
                            //{
                            //    numeroMes = 12;
                            //}

                            ProgramadorKPI.TipoFrecuencia = '/' + separador[1];
                            //ProgramadorKPI.MesInicio = numeroMes;
                        }
                        else
                        {
                            string[] MesesArray = meses.Split(',');
                            ProgramadorKPI.Meses = MesesArray.ToList();
                        }
                    }
                }
                #endregion

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
            CalidadKPIController cCalidad = new CalidadKPIController();
            CoreServiciosFrecuenciasController cCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            cCoreServiciosFrecuencias.SetDataContext(cCalidad.Context);

            long lID = long.Parse(GridRowSelect.SelectedRecordID);
            DQKpis cali = cCalidad.GetItem(lID);
            long idFrecu = long.Parse(cali.CoreServicioFrecuenciaID.ToString());

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    if (cCalidad.DeleteItem(lID))
                    {
                        if (cCoreServiciosFrecuencias.DeleteItem(idFrecu))
                        {
                            trans.Complete();
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            direct.Success = true;
                            direct.Result = "";

                        }
                        else
                        {
                            trans.Dispose();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                            return direct;
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Dispose();
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
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            CalidadKPIController cController = new CalidadKPIController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DQKpis oDato;
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
        public DirectResponse cambiarConsultaKPI()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisController cKpis = new DQKpisController();
            Data.DQKpis oDato = null;

            try
            {
                if (btnToggleCondition.Pressed)
                {
                    if (hdKPISeleccionado.Value != null)
                    {
                        oDato = cKpis.getKPIByID(long.Parse(hdKPISeleccionado.Value.ToString()));
                        oDato.IsAnd = true;
                    }
                }
                else
                {
                    if (hdKPISeleccionado.Value != null)
                    {
                        oDato = cKpis.getKPIByID(long.Parse(hdKPISeleccionado.Value.ToString()));
                        oDato.IsAnd = false;
                    }
                }

                if (cKpis.UpdateItem(oDato))
                {
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
        public DirectResponse ejecutarKPI()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisController cKPI = new DQKpisController();
            DQKpisGroupsController cGroups = new DQKpisGroupsController();
            DQGroupsController cDQGroups = new DQGroupsController();
            ResultDQKpi oResult = null;

            try
            {
                long lNombre = long.Parse(GridRowSelect.SelectedRecordID);

                DQKpis oKPI = cKPI.getKPIByID(lNombre);
                List<long> listaDQGroupsID = cDQGroups.GetGroupsActivos();

                if (oKPI != null && listaDQGroupsID != null && Usuario.UsuarioID != 0)
                {
                    oResult = cKPI.EjecutarKPI(oKPI, listaDQGroupsID, Usuario.UsuarioID);
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            if (oResult != null)
            {
                direct.Success = oResult.success;
                direct.Result = "";
            }
            else
            {
                direct.Success = true;
                direct.Result = "";
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse RecargarPanelLateral(long lKPI)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisController cKPI = new DQKpisController();
            JsonObject oDato = new JsonObject();
            Vw_DQKpis oVista;

            try
            {
                oVista = cKPI.GetItem<Vw_DQKpis>(lKPI);

                if (oVista != null)
                {
                    lblKPIName.Text = oVista.DQKpi;
                    storeDQKpisMonitoring.Reload();
                    storeKpiSemaforos.Reload();
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
        public DirectResponse GetRutaPagina(long DQKpiID)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisController cDQKpis = new DQKpisController();
            DQGroupsController cDQGroups = new DQGroupsController();

            try
            {
                string rutaPagina = cDQKpis.GetRutaPagina(DQKpiID);

                #region PARAMETROS
                string param = "";
                DQKpis DQKpi = cDQKpis.GetItem(DQKpiID);
                string indiceTabla = cDQKpis.GetIndiceTabla(DQKpiID);
                List<long> DQGroupIDs = cDQGroups.GetGroupsActivos();

                if (DQGroupIDs != null && rutaPagina != "")
                {
                    //EJECUTAR KPI Y DEVOLVER IDs

                    ResultDQKpi resultKpi = cDQKpis.EjecutarKPI(DQKpi, DQGroupIDs, this.Usuario.UsuarioID);

                    if (resultKpi != null && resultKpi.success && resultKpi.ids != null)
                    {
                        /*if (resultKpi.ids.Count > 0)
                        {
                            foreach (long id in resultKpi.ids)
                            {
                                param += (string.IsNullOrEmpty(param) ? "?" + Comun.PARAM_IDS_RESULTADOS + "=" : ",") + "" + id;
                            }
                        }
                        else
                        {
                            param = "?" + Comun.PARAM_IDS_RESULTADOS + "=0";
                        }*/
                        param = "?" + Comun.PARAM_IDS_RESULTADOS + "=" + resultKpi.DQKpiMonitoring.DQKpiMonitoringID;

                        param += "&" + Comun.PARAM_NAME_INDICE_ID + "=" + indiceTabla;

                        rutaPagina += param;
                    }
                    else
                    {
                        rutaPagina = "";
                    }
                }
                #endregion

                direct.Success = true;
                direct.Result = rutaPagina;
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }
            return direct;
        }

        [DirectMethod()]
        public DirectResponse desactivarVersion(long lKpiMonitoringID)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisMonitoringController cController = new DQKpisMonitoringController();

            try
            {
                if (lKpiMonitoringID != 0)
                {
                    Data.DQKpisMonitoring oDato;
                    oDato = cController.GetItem(lKpiMonitoringID);

                    oDato.Activa = !oDato.Activa;

                    if (cController.UpdateItem(oDato))
                    {
                        storeDQKpisMonitoring.DataBind();
                        log.Info(GetGlobalResource(Comun.LogActivacionRealizada));
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

        #region DIRECT METHOD GROUPS

        [DirectMethod()]
        public DirectResponse AgregarEditarCondition(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsController cCalidad = new DQKpisGroupsController();
            DQGroupsController cGrupos = new DQGroupsController();

            try
            {
                if (!agregar)
                {
                    if (cCalidad.RegistroDuplicado(long.Parse(hdKPISeleccionado.Value.ToString()), cmbGrupos.SelectedItem.Text, txtNameCondition.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        long lIdSelect = long.Parse(GridRowSelectCondition.SelectedRecordID);

                        Data.DQKpisGroups oDato;
                        oDato = cCalidad.GetItem(lIdSelect);
                        oDato.NombreCondicion = txtNameCondition.Text;
                        oDato.DQKpiID = long.Parse(hdKPISeleccionado.Value.ToString());

                        if (cmbGrupos.SelectedItem != null && cmbGrupos.SelectedItem.Text != null && cmbGrupos.SelectedItem.Text != "")
                        {
                            oDato.DQGroupID = cGrupos.getIDByName(cmbGrupos.SelectedItem.Text);
                        }
                        else
                        {
                            oDato.DQGroupID = 0;
                        }

                        if (cCalidad.UpdateItem(oDato))
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeDQKpiGroups.DataBind();
                        }
                    }
                }
                else
                {
                    if (cCalidad.RegistroDuplicado(long.Parse(hdKPISeleccionado.Value.ToString()), cmbGrupos.SelectedItem.Text, txtNameCondition.Text))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.DQKpisGroups oDato = new Data.DQKpisGroups();
                        oDato.Activo = true;
                        oDato.NombreCondicion = txtNameCondition.Text;
                        oDato.DQKpiID = long.Parse(hdKPISeleccionado.Value.ToString());

                        if (cmbGrupos.SelectedItem.Text != null && cmbGrupos.SelectedItem.Text != "")
                        {
                            oDato.DQGroupID = cGrupos.getIDByName(cmbGrupos.SelectedItem.Text);
                        }
                        else
                        {
                            oDato.DQGroupID = 0;
                        }

                        if (cCalidad.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDQKpiGroups.DataBind();
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
        public DirectResponse MostrarEditarCondition()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsController cKPI = new DQKpisGroupsController();

            try
            {
                long S = long.Parse(GridRowSelectCondition.SelectedRecordID);

                Data.Vw_DQKpisGroups oDato;
                oDato = cKPI.GetItem<Data.Vw_DQKpisGroups>(S);

                txtNameCondition.SetText(oDato.NombreCondicion);
                cmbGrupos.SetValue(oDato.DQGroup);
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
        public DirectResponse EliminarCondition()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsController cCalidad = new DQKpisGroupsController();

            long lID = long.Parse(GridRowSelectCondition.SelectedRecordID);

            try
            {
                if (cCalidad.DeleteItem(lID))
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
        public DirectResponse ActivarCondition()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsController cController = new DQKpisGroupsController();

            try
            {
                long lID = long.Parse(GridRowSelectCondition.SelectedRecordID);

                Data.DQKpisGroups oDato;
                oDato = cController.GetItem(lID);

                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    storeDQKpiGroups.DataBind();
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
        public DirectResponse cambiarConsultaCondicion()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsController cConditions = new DQKpisGroupsController();
            Data.DQKpisGroups oDato = null;

            try
            {
                if (btnToggleRule.Pressed)
                {
                    if (hdGroupSeleccionado.Value != null)
                    {
                        oDato = cConditions.GetItem(long.Parse(hdGroupSeleccionado.Value.ToString()));
                        oDato.IsAnd = true;
                    }
                }
                else
                {
                    if (hdGroupSeleccionado.Value != null)
                    {
                        oDato = cConditions.GetItem(long.Parse(hdGroupSeleccionado.Value.ToString()));
                        oDato.IsAnd = false;
                    }
                }

                if (cConditions.UpdateItem(oDato))
                {
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

        #region DIRECT METHOD RULES

        [DirectMethod()]
        public DirectResponse AgregarEditarRule(bool agregar, long lGrupoID)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsRulesController cCalidad = new DQKpisGroupsRulesController();
            DQGroupsController cGrupos = new DQGroupsController();

            try

            {
                if (!agregar)
                {
                    if (cCalidad.RegistroDuplicado(lGrupoID, long.Parse(cmbColumnasReglas.SelectedItem.Value.ToString()), long.Parse(cmbOperadorReglas.SelectedItem.Value.ToString()), txtValorRule.Text,
                        dateValorRule.SelectedDate.ToString(Comun.FORMATO_FECHA), numberValorRule.Text, chkValorRule.Pressed, cmbTiposDinamicosReglas.SelectedItem.Value))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        long lIdSelect = long.Parse(GridRowSelectRule.SelectedRecordID);

                        Data.DQKpisGroupsRules oDato;
                        oDato = cCalidad.GetItem(lIdSelect);

                        oDato.DQKpiGroupID = lGrupoID;

                        if (cmbColumnasReglas.SelectedItem != null && cmbColumnasReglas.SelectedItem.Value != null && cmbColumnasReglas.SelectedItem.Value != "")
                        {
                            oDato.ColumnaModeloDatosID = long.Parse(cmbColumnasReglas.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ColumnaModeloDatosID = 0;
                        }

                        if (cmbOperadorReglas.SelectedItem != null && cmbOperadorReglas.SelectedItem.Value != null && cmbOperadorReglas.SelectedItem.Value != "")
                        {
                            oDato.TipoDatoOperadorID = long.Parse(cmbOperadorReglas.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.TipoDatoOperadorID = 0;
                        }

                        if (txtValorRule.Text != "")
                        {
                            oDato.Valor = txtValorRule.Text;
                        }
                        else if (dateValorRule.SelectedValue != null)
                        {
                            oDato.Valor = dateValorRule.SelectedDate.ToString(Comun.FORMATO_FECHA);
                        }
                        else if (numberValorRule.Text != "")
                        {
                            oDato.Valor = numberValorRule.Text;
                        }
                        else if (cmbTiposDinamicosReglas.SelectedItem != null && cmbTiposDinamicosReglas.SelectedItem.Value != null && cmbTiposDinamicosReglas.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTiposDinamicosReglas.SelectedItem.Value;
                        }
                        else if (cmbMultiTiposDinamicosReglas.SelectedItems.Count > 0)
                        {
                            string sValor = "";
                            foreach (ListItem item in cmbMultiTiposDinamicosReglas.SelectedItems)
                            {
                                if (item.Value != null)
                                {
                                    if (sValor != "")
                                    {
                                        sValor += SEPARADOR_VALORES + item.Value;
                                    }
                                    else
                                    {
                                        sValor = item.Value;
                                    }
                                }
                            }

                            oDato.Valor = sValor;
                            oDato.IsDataTable = false;
                        }
                        else if (cmbTablasPaginasAsociacionRule.SelectedItem != null && cmbTablasPaginasAsociacionRule.SelectedItem.Value != null && cmbTablasPaginasAsociacionRule.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTablasPaginasAsociacionRule.SelectedItem.Value;
                            oDato.IsDataTable = true;
                        }
                        else if (chkValorRule.Pressed)
                        {
                            oDato.Valor = "1";
                        }
                        else
                        {
                            oDato.Valor = "0";
                        }

                        if (cCalidad.UpdateItem(oDato))
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeDQKpisGroupsRules.DataBind();
                        }
                    }
                }
                else
                {
                    if (cCalidad.RegistroDuplicado(lGrupoID, long.Parse(cmbColumnasReglas.SelectedItem.Value.ToString()), long.Parse(cmbOperadorReglas.SelectedItem.Value.ToString()), txtValorRule.Text,
                        dateValorRule.SelectedDate.ToString(Comun.FORMATO_FECHA), numberValorRule.Text, chkValorRule.Pressed, cmbTiposDinamicosReglas.SelectedItem.Value))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.DQKpisGroupsRules oDato = new Data.DQKpisGroupsRules();
                        oDato.Activo = true;
                        oDato.DQKpiGroupID = lGrupoID;

                        if (cmbColumnasReglas.SelectedItem != null && cmbColumnasReglas.SelectedItem.Value != null && cmbColumnasReglas.SelectedItem.Value != "")
                        {
                            oDato.ColumnaModeloDatosID = long.Parse(cmbColumnasReglas.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ColumnaModeloDatosID = 0;
                        }

                        if (cmbOperadorReglas.SelectedItem != null && cmbOperadorReglas.SelectedItem.Value != null && cmbOperadorReglas.SelectedItem.Value != "")
                        {
                            oDato.TipoDatoOperadorID = long.Parse(cmbOperadorReglas.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.TipoDatoOperadorID = 0;
                        }

                        if (txtValorRule.Text != "")
                        {
                            oDato.Valor = txtValorRule.Text;
                        }
                        else if (dateValorRule.SelectedValue != null)
                        {
                            oDato.Valor = dateValorRule.SelectedDate.ToString(Comun.FORMATO_FECHA);
                        }
                        else if (numberValorRule.Text != "")
                        {
                            oDato.Valor = numberValorRule.Text;
                        }
                        else if (cmbTiposDinamicosReglas.SelectedItem != null && cmbTiposDinamicosReglas.SelectedItem.Value != null && cmbTiposDinamicosReglas.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTiposDinamicosReglas.SelectedItem.Value;
                        }
                        else if (cmbMultiTiposDinamicosReglas.SelectedItems.Count > 0)
                        {
                            string sValor = "";
                            foreach (ListItem item in cmbMultiTiposDinamicosReglas.SelectedItems)
                            {
                                if (item.Value != null)
                                {
                                    if (sValor != "")
                                    {
                                        sValor += SEPARADOR_VALORES + item.Value;
                                    }
                                    else
                                    {
                                        sValor = item.Value;
                                    }
                                }
                            }

                            oDato.Valor = sValor;
                            oDato.IsDataTable = false;
                        }
                        else if (cmbTablasPaginasAsociacionRule.SelectedItem != null && cmbTablasPaginasAsociacionRule.SelectedItem.Value != null && cmbTablasPaginasAsociacionRule.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTablasPaginasAsociacionRule.SelectedItem.Value;
                            oDato.IsDataTable = true;
                        }
                        else if (chkValorRule.Pressed)
                        {
                            oDato.Valor = "1";
                        }
                        else
                        {
                            oDato.Valor = "0";
                        }

                        if (cCalidad.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDQKpisGroupsRules.DataBind();
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
        public DirectResponse MostrarEditarRule()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsRulesController cKPI = new DQKpisGroupsRulesController();
            TiposDatosOperadoresController cTiposOperadores = new TiposDatosOperadoresController();
            TiposDatosController cTipos = new TiposDatosController();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            DQTablasPaginasController cDQTablas = new DQTablasPaginasController();

            try
            {
                long S = long.Parse(GridRowSelectRule.SelectedRecordID);

                Data.Vw_DQKpisGroupsRules oDato;
                oDato = cKPI.GetItem<Data.Vw_DQKpisGroupsRules>(S);

                cmbColumnasReglas.SetValue(oDato.ColumnaModeloDatosID);
                hdColumnaReglaID.Value = oDato.ColumnaModeloDatosID;

                cmbOperadorReglas.SetValue(oDato.TipoDatoOperadorID);
                hdOperadorReglaID.Value = oDato.TipoDatoOperadorID;

                string sNombreTabla = cTablas.getTablaByColumna(oDato.ColumnaModeloDatosID);
                hdRuleTablaModeloDatoID.Value = cDQTablas.GetDQTablaIDByTablaModeloDatoID(cColumnas.GetItem(oDato.ColumnaModeloDatosID).TablaModeloDatosID).ToString();

                cmbTablasPaginasReglas.SetValue(hdRuleTablaModeloDatoID.Value);

                TiposDatosOperadores oTipoOperador = cTiposOperadores.GetItem(oDato.TipoDatoOperadorID);
                TiposDatos oTipoDato = cTipos.GetItem(oTipoOperador.TipoDatoID);

                if (oTipoOperador.RequiereValor)
                {
                    string valor = "";
                    if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_NUMERICO)
                    {
                        numberValorRule.Show();
                        numberValorRule.Text = oDato.Valor;
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_TEXTO)
                    {
                        txtValorRule.Show();
                        txtValorRule.Text = oDato.Valor;
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                    {
                        dateValorRule.Show();
                        dateValorRule.SelectedValue = DateTime.ParseExact(oDato.Valor, Comun.FORMATO_FECHA, null);
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA || oTipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        string sNombre = cColumnas.getDataSourceTablaColumna(oDato.ColumnaModeloDatosID);
                        string sControlador = cColumnas.getControllerColumna(oDato.ColumnaModeloDatosID);
                        string sDisplay = cColumnas.getDisplay(oDato);

                        Type tipo = Type.GetType("TreeCore.Data." + sNombre.Split('.')[1]);
                        Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                        if (tipo == null)
                        {
                            tipo = Type.GetType("TreeCore.Data.Vw_" + sNombre.Split('_')[1]);
                        }

                        if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                        {
                            tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                        }

                        var instance = Activator.CreateInstance(tipoControlador);
                        MethodInfo[] methodos = tipoControlador.GetMethods();

                        foreach (MethodInfo met in methodos)
                        {
                            if (met.Name == "GetItem" && met.ReturnType == tipo)
                            {
                                var Params = met.GetParameters();

                                if (Params[0].ParameterType.Name == "Int64")
                                {
                                    if (oDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                                    {
                                        if (oDato.IsDataTable)
                                        {
                                            sNombre = cTablas.getClaveByID(long.Parse(oDato.Valor));
                                            valor = GetGlobalResource(sNombre);
                                        }
                                        else
                                        {
                                            string[] listaID = oDato.Valor.Split(',');
                                            string sNombreCombo = "";

                                            foreach (string sItem in listaID)
                                            {
                                                object result = met.Invoke(instance, new object[] { long.Parse(sItem) });
                                                PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                if (propName != null && result != null)
                                                {
                                                    string sName = (string)propName.GetValue(result, null);

                                                    if (sNombreCombo != "")
                                                    {
                                                        sNombreCombo += SEPARADOR_VALORES + sName;
                                                    }
                                                    else
                                                    {
                                                        sNombreCombo = sName;
                                                    }
                                                }

                                                valor = sNombreCombo;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        object result = met.Invoke(instance, new object[] { long.Parse(oDato.Valor) });
                                        PropertyInfo propName = tipo.GetProperty(sDisplay);

                                        if (propName != null)
                                        {
                                            string sName = (string)propName.GetValue(result, null);
                                            valor = sName;
                                        }
                                    }
                                }

                            }
                        }

                        if (oDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE && oDato.IsDataTable == false)
                        {
                            cmbMultiTiposDinamicosReglas.Show();
                            cmbMultiTiposDinamicosReglas.FieldLabel = GetGlobalResource(oDato.ClaveRecursoColumna);
                            //cmbMultiTiposDinamicosReglas.SetValue(oDato.Valor);

                            string[] a = { SEPARADOR_VALORES };
                            string[] valores = oDato.Valor.Split(a, StringSplitOptions.None);

                            cmbMultiTiposDinamicosReglas.SetValue(valores);

                            btnToggleTabRule.Show();
                            btnToggleTabRule.SetPressed(false);
                        }
                        else if (oDato.IsDataTable)
                        {
                            cmbTablasPaginasAsociacionRule.Show();
                            cmbTablasPaginasAsociacionRule.FieldLabel = GetGlobalResource("strTabla");
                            cmbTablasPaginasAsociacionRule.SetValue(valor);

                            btnToggleTabRule.Show();
                            btnToggleTabRule.SetPressed(true);
                        }
                        else
                        {
                            cmbTiposDinamicosReglas.Show();
                            cmbTiposDinamicosReglas.FieldLabel = GetGlobalResource(oDato.ClaveRecursoColumna);
                            cmbTiposDinamicosReglas.SetValue(valor);
                        }
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN)
                    {
                        if (valor == "1")
                        {
                            chkValorRule.Show();
                            chkValorRule.Pressed = true;
                        }
                        else
                        {
                            chkValorRule.Show();
                            chkValorRule.Pressed = false;
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
        public DirectResponse EliminarRule()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisGroupsRulesController cCalidad = new DQKpisGroupsRulesController();

            long lID = long.Parse(GridRowSelectRule.SelectedRecordID);

            try
            {
                if (cCalidad.DeleteItem(lID))
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

        #region DIRECT METHOD FILTROS

        [DirectMethod()]
        public DirectResponse AgregarEditarFiltro(bool agregar)
        {
            DirectResponse direct = new DirectResponse();
            DQKpisFiltrosController cCalidad = new DQKpisFiltrosController();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            TiposDatosOperadoresController cOperadores = new TiposDatosOperadoresController();

            try

            {
                if (!agregar)
                {
                    if (cCalidad.RegistroDuplicado(long.Parse(hdKPISeleccionado.Value.ToString()), long.Parse(cmbColumnasFiltro.SelectedItem.Value.ToString()), long.Parse(cmbOperadorFiltro.SelectedItem.Value.ToString()),
                        txtValorFiltro.Text, dateValorFiltro.SelectedDate.ToString(Comun.FORMATO_FECHA), numberValorFiltro.Text, chkValorFiltro.Pressed, cmbTiposDinamicosFiltro.SelectedItem.Value))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        long lIdSelect = long.Parse(GridRowSelectFiltro.SelectedRecordID);

                        Data.DQKpisFiltros oDato;
                        oDato = cCalidad.GetItem(lIdSelect);

                        if (cmbColumnasFiltro.SelectedItem != null && cmbColumnasFiltro.SelectedItem.Value != null && cmbColumnasFiltro.SelectedItem.Value != "")
                        {
                            oDato.ColumnaModeloDatoID = long.Parse(cmbColumnasFiltro.SelectedItem.Value);
                        }
                        else
                        {
                            oDato.ColumnaModeloDatoID = 0;
                        }

                        if (cmbOperadorFiltro.SelectedItem != null && cmbOperadorFiltro.SelectedItem.Value != null && cmbOperadorFiltro.SelectedItem.Value != "")
                        {
                            oDato.TipoDatoOperadorID = long.Parse(cmbOperadorFiltro.SelectedItem.Value);
                        }
                        else
                        {
                            oDato.TipoDatoOperadorID = 0;
                        }

                        if (txtValorFiltro.Text != "")
                        {
                            oDato.Valor = txtValorFiltro.Text;
                        }
                        else if (dateValorFiltro.SelectedValue != null)
                        {
                            oDato.Valor = dateValorFiltro.SelectedDate.ToString(Comun.FORMATO_FECHA);
                        }
                        else if (numberValorFiltro.Text != "")
                        {
                            oDato.Valor = numberValorFiltro.Text;
                        }
                        else if (cmbTiposDinamicosFiltro.SelectedItem != null && cmbTiposDinamicosFiltro.SelectedItem.Value != null && cmbTiposDinamicosFiltro.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTiposDinamicosFiltro.SelectedItem.Value;
                        }
                        else if (cmbMultiTiposDinamicosFiltro.SelectedItems.Count > 0)
                        {
                            string sValor = "";
                            foreach (ListItem item in cmbMultiTiposDinamicosFiltro.SelectedItems)
                            {
                                if (item.Value != null)
                                {
                                    if (sValor != "")
                                    {
                                        sValor += ", " + item.Value;
                                    }
                                    else
                                    {
                                        sValor = item.Value;
                                    }
                                }
                            }

                            oDato.Valor = sValor;
                            oDato.IsDataTable = false;
                        }
                        else if (cmbTablasPaginasAsociacionFiltro.SelectedItem != null && cmbTablasPaginasAsociacionFiltro.SelectedItem.Value != null && cmbTablasPaginasAsociacionFiltro.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTablasPaginasAsociacionFiltro.SelectedItem.Value;
                            oDato.IsDataTable = true;
                        }
                        else if (chkValorFiltro.Pressed)
                        {
                            oDato.Valor = "1";
                        }
                        else
                        {
                            oDato.Valor = "0";
                        }

                        if (cCalidad.UpdateItem(oDato))
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            storeDQKpisFiltros.DataBind();
                        }
                    }
                }
                else
                {
                    if (cCalidad.RegistroDuplicado(long.Parse(hdKPISeleccionado.Value.ToString()), long.Parse(cmbColumnasFiltro.SelectedItem.Value.ToString()), long.Parse(cmbOperadorFiltro.SelectedItem.Value.ToString()),
                        txtValorFiltro.Text, dateValorFiltro.SelectedDate.ToString(Comun.FORMATO_FECHA), numberValorFiltro.Text, chkValorFiltro.Pressed, cmbTiposDinamicosFiltro.SelectedItem.Value))
                    {
                        log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                        MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                    }
                    else
                    {
                        Data.DQKpisFiltros oDato = new Data.DQKpisFiltros();
                        oDato.DQKpiID = long.Parse(hdKPISeleccionado.Value.ToString());
                        oDato.Activo = true;

                        if (cmbColumnasFiltro.SelectedItem != null && cmbColumnasFiltro.SelectedItem.Value != null && cmbColumnasFiltro.SelectedItem.Value != "")
                        {
                            oDato.ColumnaModeloDatoID = long.Parse(cmbColumnasFiltro.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.ColumnaModeloDatoID = 0;
                        }

                        if (cmbOperadorFiltro.SelectedItem != null && cmbOperadorFiltro.SelectedItem.Value != null && cmbOperadorFiltro.SelectedItem.Value != "")
                        {
                            oDato.TipoDatoOperadorID = long.Parse(cmbOperadorFiltro.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.TipoDatoOperadorID = 0;
                        }

                        if (txtValorFiltro.Text != "")
                        {
                            oDato.Valor = txtValorFiltro.Text;
                        }
                        else if (dateValorFiltro.SelectedValue != null)
                        {
                            oDato.Valor = dateValorFiltro.SelectedDate.ToString(Comun.FORMATO_FECHA);
                        }
                        else if (numberValorFiltro.Text != "")
                        {
                            oDato.Valor = numberValorFiltro.Text;
                        }
                        else if (cmbTiposDinamicosFiltro.SelectedItem != null && cmbTiposDinamicosFiltro.SelectedItem.Value != null && cmbTiposDinamicosFiltro.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTiposDinamicosFiltro.SelectedItem.Value;
                        }
                        else if (cmbMultiTiposDinamicosFiltro.SelectedItems.Count > 0)
                        {
                            string sValor = "";
                            foreach (ListItem item in cmbMultiTiposDinamicosFiltro.SelectedItems)
                            {
                                if (item.Value != null)
                                {
                                    if (sValor != "")
                                    {
                                        sValor += ", " + item.Value;
                                    }
                                    else
                                    {
                                        sValor = item.Value;
                                    }
                                }
                            }

                            oDato.Valor = sValor;
                            oDato.IsDataTable = false;
                        }
                        else if (cmbTablasPaginasAsociacionFiltro.SelectedItem != null && cmbTablasPaginasAsociacionFiltro.SelectedItem.Value != null && cmbTablasPaginasAsociacionFiltro.SelectedItem.Value != "")
                        {
                            oDato.Valor = cmbTablasPaginasAsociacionFiltro.SelectedItem.Value;
                            oDato.IsDataTable = true;
                        }
                        else if (chkValorFiltro.Pressed)
                        {
                            oDato.Valor = "1";
                        }
                        else
                        {
                            oDato.Valor = "0";
                        }

                        if (cCalidad.AddItem(oDato) != null)
                        {
                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            storeDQKpisFiltros.DataBind();
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
        public DirectResponse MostrarEditarFiltro()
        {
            DirectResponse direct = new DirectResponse();
            TiposDatosOperadoresController cTiposOperadores = new TiposDatosOperadoresController();
            TiposDatosController cTipos = new TiposDatosController();
            TablasModeloDatosController cTablas = new TablasModeloDatosController();
            ColumnasModeloDatosController cColumnas = new ColumnasModeloDatosController();
            DQKpisFiltrosController cKPI = new DQKpisFiltrosController();

            try
            {
                long S = long.Parse(GridRowSelectFiltro.SelectedRecordID);

                Data.Vw_DQKpisFiltros oDato;
                oDato = cKPI.GetItem<Data.Vw_DQKpisFiltros>(S);

                cmbColumnasFiltro.SetValue(oDato.ColumnaModeloDatoID);
                hdColumnaFiltroID.Value = oDato.ColumnaModeloDatoID;

                cmbOperadorFiltro.SetValue(oDato.TipoDatoOperadorID);
                hdOperadorFiltroID.Value = oDato.TipoDatoOperadorID;

                TiposDatosOperadores oTipoOperador = cTiposOperadores.GetItem(oDato.TipoDatoOperadorID);
                TiposDatos oTipoDato = cTipos.GetItem(oTipoOperador.TipoDatoID);

                if (oTipoOperador.RequiereValor)
                {
                    if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_NUMERICO)
                    {
                        numberValorFiltro.Show();
                        numberValorFiltro.Text = oDato.Valor;
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_TEXTO)
                    {
                        txtValorFiltro.Show();
                        txtValorFiltro.Text = oDato.Valor;
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_FECHA)
                    {
                        dateValorFiltro.Show();
                        dateValorFiltro.SelectedValue = DateTime.ParseExact(oDato.Valor, Comun.FORMATO_FECHA, null);
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA || oTipoDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                    {
                        string sNombre = cColumnas.getDataSourceTablaColumna(oDato.ColumnaModeloDatoID);
                        string sControlador = cColumnas.getControllerColumna(oDato.ColumnaModeloDatoID);
                        string sDisplay = cColumnas.getDisplayFiltro(oDato);

                        Type tipo = Type.GetType("TreeCore.Data." + sNombre.Split('.')[1]);
                        Type tipoControlador = Type.GetType("CapaNegocio." + sControlador);

                        if (tipo == null)
                        {
                            tipo = Type.GetType("TreeCore.Data.Vw_" + sNombre.Split('_')[1]);
                        }

                        if (tipoControlador.BaseType.GenericTypeArguments[0].FullName != tipo.FullName)
                        {
                            tipo = Type.GetType(tipoControlador.BaseType.GenericTypeArguments[0].FullName);
                        }

                        var instance = Activator.CreateInstance(tipoControlador);
                        MethodInfo[] methodos = tipoControlador.GetMethods();

                        foreach (MethodInfo met in methodos)
                        {
                            if (met.Name == "GetItem" && met.ReturnType == tipo)
                            {
                                var Params = met.GetParameters();

                                if (Params[0].ParameterType.Name == "Int64")
                                {
                                    if (oDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE)
                                    {
                                        if (oDato.IsDataTable)
                                        {
                                            sNombre = cTablas.getClaveByID(long.Parse(oDato.Valor));
                                            oDato.Valor = GetGlobalResource(sNombre);
                                        }
                                        else
                                        {
                                            string[] listaID = oDato.Valor.Split(',');
                                            string sNombreCombo = "";

                                            foreach (string sItem in listaID)
                                            {
                                                object result = met.Invoke(instance, new object[] { long.Parse(sItem) });
                                                PropertyInfo propName = tipo.GetProperty(sDisplay);

                                                if (propName != null && result != null)
                                                {
                                                    string sName = (string)propName.GetValue(result, null);

                                                    if (sNombreCombo != "")
                                                    {
                                                        sNombreCombo += ", " + sName;
                                                    }
                                                    else
                                                    {
                                                        sNombreCombo = sName;
                                                    }
                                                }

                                                oDato.Valor = sNombreCombo;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        object result = met.Invoke(instance, new object[] { long.Parse(oDato.Valor) });
                                        PropertyInfo propName = tipo.GetProperty(sDisplay);

                                        if (propName != null)
                                        {
                                            string sName = (string)propName.GetValue(result, null);
                                            oDato.Valor = sName;
                                        }
                                    }
                                }
                            }
                        }

                        if (oDato.Codigo == Comun.TIPODATO_CODIGO_LISTA_MULTIPLE && oDato.IsDataTable == false)
                        {
                            cmbMultiTiposDinamicosFiltro.Show();
                            cmbMultiTiposDinamicosFiltro.FieldLabel = GetGlobalResource(oDato.ClaveRecursoColumna);
                            cmbMultiTiposDinamicosFiltro.SetValue(oDato.Valor);

                            btnToggleFiltro.Show();
                            btnToggleFiltro.SetPressed(false);
                        }
                        else if (oDato.IsDataTable)
                        {
                            cmbTablasPaginasAsociacionFiltro.Show();
                            cmbTablasPaginasAsociacionFiltro.FieldLabel = GetGlobalResource("strTabla"); ;
                            cmbTablasPaginasAsociacionFiltro.SetValue(oDato.Valor);

                            btnToggleFiltro.Show();
                            btnToggleFiltro.SetPressed(true);
                        }
                        else
                        {
                            cmbTiposDinamicosFiltro.Show();
                            cmbTiposDinamicosFiltro.FieldLabel = GetGlobalResource(oDato.ClaveRecursoColumna);
                            cmbTiposDinamicosFiltro.SetValue(oDato.Valor);
                        }
                    }
                    else if (oTipoDato.Codigo == Comun.TIPODATO_CODIGO_BOOLEAN)
                    {
                        if (oDato.Valor == "1")
                        {
                            chkValorFiltro.Show();
                            chkValorFiltro.Pressed = true;
                        }
                        else
                        {
                            chkValorFiltro.Show();
                            chkValorFiltro.Pressed = false;
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
        public DirectResponse EliminarFiltro()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisFiltrosController cCalidad = new DQKpisFiltrosController();

            long lID = long.Parse(GridRowSelectFiltro.SelectedRecordID);

            try
            {
                if (cCalidad.DeleteItem(lID))
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
        public DirectResponse ActivarFiltro()
        {
            DirectResponse direct = new DirectResponse();
            DQKpisFiltrosController cController = new DQKpisFiltrosController();

            try
            {
                long lID = long.Parse(GridRowSelectFiltro.SelectedRecordID);

                Data.DQKpisFiltros oDato;
                oDato = cController.GetItem(lID);

                oDato.Activo = !oDato.Activo;

                if (cController.UpdateItem(oDato))
                {
                    storeDQKpisFiltros.DataBind();
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

        #region DISEÑO
        [DirectMethod]
        public void VwUpdater()
        {
            this.CenterPanelMain.Update();
        }

        protected void ShowHidePnAsideR(object sender, DirectEventArgs e)
        {
            pnAsideR.AnimCollapse = true;
            pnAsideR.ToggleCollapse();
        }

        protected void ShowHidePnAsideRColumnas(object sender, DirectEventArgs e)
        {
            btnCollapseAsRClosed.Show();
        }

        [DirectMethod]
        public void DirectShowHidePnAsideR()
        {
            btnCollapseAsRClosed.Show();
        }
        #endregion

        #endregion

    }

    public class TipoDinamico
    {
        public string Name { get; set; }
        public long Id { get; set; }

        public TipoDinamico(string name, long id)
        {
            this.Name = name;
            this.Id = id;
        }
    }

}