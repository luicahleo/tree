using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using TreeCore.Data;
using TreeCore.Clases;
using Button = Ext.Net.Button;

namespace TreeCore.ModWorkFlow
{
    public partial class WorkFlowsEstados : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, System.EventArgs e)
        {
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                if (Request.Params["ProyectoTipo"] != null)
                {
                    hdProyectoParametro.Value = Request.Params["ProyectoTipo"];
                }

                #region FILTROS

                List<string> listaIgnore = new List<string>() { "EstadosSiguientes", "EstadosGlobales" };
                List<string> listaIgnoreEstadosSiguientes = new List<string>() { };
                List<string> listaIgnoreTareas = new List<string>() { };
                List<string> listaIgnoreEstadosGlobales = new List<string>() { };
                List<string> listaIgnoreRoles = new List<string>() { };

                Comun.CreateGridFilters(gridFilters, storeCoreEstados, gridMain1.ColumnModel, listaIgnore, _Locale);
                Comun.CreateGridFilters(gridFiltersObjetos, storeCoreEstadosTareas, gridObjetos.ColumnModel, listaIgnoreTareas, _Locale);
                Comun.CreateGridFilters(gridFiltersEstadosSiguientes, storeCoreEstadosSiguientes, gridEstadosSiguientes.ColumnModel, listaIgnoreEstadosSiguientes, _Locale);
                Comun.CreateGridFilters(gridFiltersEstadosGlobales, storeCoreEstadosGlobales, gridEstadosGlobales.ColumnModel, listaIgnoreEstadosGlobales, _Locale);
                log.Info(GetGlobalResource(Comun.LogFiltrosPerfilesCreados));

                #endregion

                #region SELECCION COLUMNAS

                Comun.Seleccionable(gridMain1, storeCoreEstados, gridMain1.ColumnModel, listaIgnore, _Locale);
                log.Info(GetGlobalResource(Comun.LogSeleccionElementoGrilla));

                #endregion

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (Request.Params["ProyectoTipo"] != null)
                {
                    hdProyectoParametro.Value = Request.Params["ProyectoTipo"];
                }

                if (!ClienteID.HasValue)
                {
                    hdCliID.Value = 0;
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
                        EstadosController cEstados = new EstadosController();
                        CoreEstadosGlobalesController cGlobales = new CoreEstadosGlobalesController();
                        List<Data.Vw_CoreEstados> listaDatos = null;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        long? lWorkflowID = long.Parse(Request.QueryString["aux3"].ToString());
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro, lWorkflowID);

                        foreach (Data.Vw_CoreEstados oValor in listaDatos)
                        {
                            if (oValor.EstadosSiguientes != null && oValor.EstadosSiguientes != "")
                            {
                                long lEstadoID = long.Parse(oValor.EstadosSiguientes.Split('(')[0]);
                                string sEstado = cEstados.getNombreEstado(lEstadoID);
                                string sCuenta = oValor.EstadosSiguientes.Split('(')[1];

                                if (sCuenta.Split(')')[0] != "1")
                                {
                                    oValor.EstadosSiguientes = sEstado + " (" + sCuenta;
                                }
                                else
                                {
                                    oValor.EstadosSiguientes = sEstado;
                                }
                            }

                            if (oValor.EstadosGlobales != null && oValor.EstadosGlobales != "")
                            {
                                long lObjetoEstadoID = long.Parse(oValor.EstadosGlobales.Split(',')[0]);
                                string sNegocio = oValor.EstadosGlobales.Split(',')[1];
                                long lNegocioID = long.Parse(sNegocio.Split('(')[0]);
                                string sCuenta = sNegocio.Split('(')[1];

                                Data.CoreObjetosNegocioTipos oNegocioTipo;
                                DataTable oEstado;
                                string sNombreTabla = "";
                                List<Object> listaEstadosGlob = new List<object>();

                                CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                                TablasModeloDatosController cTablas = new TablasModeloDatosController();

                                oNegocioTipo = cObjetos.GetItem(lNegocioID);

                                if (oNegocioTipo != null)
                                {
                                    sNombreTabla = cTablas.getClaveByID(oNegocioTipo.TablaModeloDatoID);
                                    oEstado = cObjetos.getEstadoByID(lNegocioID, lObjetoEstadoID);

                                    foreach (DataRow item in oEstado.Rows)
                                    {
                                        if (sCuenta.Split(')')[0] != "1")
                                        {
                                            oValor.EstadosGlobales = item[0].ToString() + " (" + sCuenta;
                                        }
                                        else
                                        {
                                            oValor.EstadosGlobales = item[0].ToString();
                                        }
                                    }
                                }
                            }
                        }
                        //}

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(gridMain1.ColumnModel, listaDatos, Response, "", GetGlobalResource("strEstados").ToString(), _Locale);
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
        }

        #endregion

        #region STORES

        #region PRINCIPAL

        protected void storeCoreEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    string sSort, sDir = null;
                    sSort = e.Sort[0].Property.ToString();
                    sDir = e.Sort[0].Direction.ToString();
                    int iCount = 0;
                    List<long> listaProyectos = new List<long>();
                    string sFiltro = e.Parameters["gridFilters"];
                    long? lWorkflowID = null;
                    EstadosController cEstados = new EstadosController();
                    CoreEstadosGlobalesController cGlobales = new CoreEstadosGlobalesController();

                    if (cmbWorkflows.SelectedItem.Value != "" && cmbWorkflows.SelectedItem.Value != null)
                    {
                        btnAnadir.Disabled = false;
                        btnAnadir.ToolTip = (GetGlobalResource("btnAnadir.ToolTip"));
                        btnRefrescar.Disabled = false;
                        btnRefrescar.ToolTip = (GetGlobalResource("btnRefrescar.ToolTip"));
                        btnDescargar.Disabled = false;
                        btnDescargar.ToolTip = (GetGlobalResource("btnDescargar.ToolTip"));
                        btnDuplicarWorkflow.Disabled = false;
                        btnDuplicarWorkflow.ToolTip = (GetGlobalResource("strDuplicarWorkflow"));
                        btnExport.Disabled = false;
                        btnExport.ToolTip = (GetGlobalResource("strExportar"));
                        FileUploadImportar.Disabled = false;
                        lWorkflowID = long.Parse(cmbWorkflows.SelectedItem.Value);
                    }
                    else
                    {
                        btnAnadir.Disabled = true;
                        btnRefrescar.Disabled = true;
                        btnDescargar.Disabled = true;
                        btnDuplicarWorkflow.Disabled = true;
                        btnExport.Disabled = true;
                        FileUploadImportar.Disabled = true;
                    }

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro, lWorkflowID);
                    if (lista != null)
                    {
                        storeCoreEstados.DataSource = lista;

                        foreach (Data.Vw_CoreEstados oDato in lista)
                        {
                            if (oDato.EstadosSiguientes != null && oDato.EstadosSiguientes != "")
                            {
                                long lEstadoID = long.Parse(oDato.EstadosSiguientes.Split('(')[0]);
                                string sEstado = cEstados.getNombreEstado(lEstadoID);
                                string sCuenta = oDato.EstadosSiguientes.Split('(')[1];

                                if (sCuenta.Split(')')[0] != "1")
                                {
                                    oDato.EstadosSiguientes = sEstado + " (" + sCuenta;
                                }
                                else
                                {
                                    oDato.EstadosSiguientes = sEstado;
                                }
                            }

                            if (oDato.EstadosGlobales != null && oDato.EstadosGlobales != "")
                            {
                                long lObjetoEstadoID = long.Parse(oDato.EstadosGlobales.Split(',')[0]);
                                string sNegocio = oDato.EstadosGlobales.Split(',')[1];
                                long lNegocioID = long.Parse(sNegocio.Split('(')[0]);
                                string sCuenta = sNegocio.Split('(')[1];

                                Data.CoreObjetosNegocioTipos oNegocioTipo;
                                DataTable oEstado;
                                string sNombreTabla = "";
                                List<Object> listaEstadosGlob = new List<object>();

                                CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                                TablasModeloDatosController cTablas = new TablasModeloDatosController();

                                oNegocioTipo = cObjetos.GetItem(lNegocioID);

                                if (oNegocioTipo != null)
                                {
                                    sNombreTabla = cTablas.getClaveByID(oNegocioTipo.TablaModeloDatoID);
                                    oEstado = cObjetos.getEstadoByID(lNegocioID, lObjetoEstadoID);

                                    foreach (DataRow item in oEstado.Rows)
                                    {
                                        if (sCuenta.Split(')')[0] != "1")
                                        {
                                            oDato.EstadosGlobales = item[0].ToString() + " (" + sCuenta;
                                        }
                                        else
                                        {
                                            oDato.EstadosGlobales = item[0].ToString();
                                        }
                                    }
                                }
                            }
                        }

                        PageProxy temp = (PageProxy)storeCoreEstados.Proxy[0];
                        temp.Total = iCount;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        private List<Data.Vw_CoreEstados> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro, long? lWorkflowID)
        {
            List<Data.Vw_CoreEstados> listaDatos = new List<Data.Vw_CoreEstados>();
            EstadosController cEstados = new EstadosController();

            try
            {
                if (lWorkflowID != null)
                {
                    listaDatos = cEstados.GetCoreEstadosFromWorkflowID(lWorkflowID);
                    listaDatos = Clases.LinqEngine.PagingItemsListWithExtNetFilter(listaDatos, sFiltro, "", sSort, sDir, iStart, iLimit, ref iCount);
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

        #region DEPARTAMENTOS

        protected void storeDepartamentos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Departamentos> listaDepartamentos;

                    listaDepartamentos = ListaDepartamentos();

                    if (listaDepartamentos != null)
                    {
                        storeDepartamentos.DataSource = listaDepartamentos;
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

        private List<Data.Departamentos> ListaDepartamentos()
        {
            List<Data.Departamentos> listaDatos;
            DepartamentosController cDepartament = new DepartamentosController();

            try
            {
                listaDatos = cDepartament.GetActivos(long.Parse(hdCliID.Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region CORE ESTADOS GLOBALES

        protected void storeCoreEstadosGlobales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosGlobales> listaEst;
                    Data.CoreObjetosNegocioTipos oNegocioTipo;
                    Object oEstadoGlob;
                    DataTable oEstado;
                    int i = 1;
                    List<Object> listaEstadosGlob = new List<object>();

                    CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();
                    CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                    TablasModeloDatosController cTablas = new TablasModeloDatosController();

                    listaEst = cEstados.getCoreEstadosGlobales(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaEst != null)
                    {
                        foreach (Data.CoreEstadosGlobales oEstGlobal in listaEst)
                        {
                            oNegocioTipo = cObjetos.GetItem((long)oEstGlobal.CoreObjetoNegocioTipoID);

                            if (oNegocioTipo != null)
                            {
                                oEstado = cObjetos.getEstadoByID(oEstGlobal.CoreObjetoNegocioTipoID, oEstGlobal.ObjetoEstadoID);

                                foreach (DataRow item in oEstado.Rows)
                                {
                                    oEstadoGlob = new
                                    {
                                        ID = i,
                                        CoreEstadoGlobalID = oEstGlobal.CoreEstadoGlobalID,
                                        Tabla = oNegocioTipo.Codigo,
                                        Estado = item[0].ToString(),
                                    };

                                    listaEstadosGlob.Add(oEstadoGlob);
                                }
                            }
                            i++;
                        }


                        storeCoreEstadosGlobales.DataSource = listaEstadosGlob;
                        storeCoreEstadosGlobales.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ESTADOS GLOBALES

        protected void storeEstadosGlobales_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    Object oEstadoGlob;
                    int i = 1;
                    DataTable listaEstados;
                    List<long?> listaEstadosGlobales;
                    CoreEstadosGlobalesController cEstadosGlobales = new CoreEstadosGlobalesController();
                    CoreObjetosNegocioTiposController cNegociosTipo = new CoreObjetosNegocioTiposController();
                    List<Object> listaEstadosGlob = new List<object>();

                    if (cmbObject != null && cmbObject.SelectedItem.Value != "" && cmbObject.SelectedItem.Value != null)
                    {
                        long? lObjetoTipoID = long.Parse(cmbObject.SelectedItem.Value);
                        listaEstadosGlobales = cEstadosGlobales.getObjetosByEstadoID(long.Parse(hdEstadoID.Value.ToString()));
                        listaEstados = cNegociosTipo.getEstadosByID(lObjetoTipoID);

                        foreach (DataRow item in listaEstados.Rows)
                        {
                            if (!listaEstadosGlobales.Contains(long.Parse(item[0].ToString())))
                            {
                                oEstadoGlob = new
                                {
                                    ID = item[0].ToString(),
                                    Nombre = item[1].ToString(),
                                };

                                i++;
                                listaEstadosGlob.Add(oEstadoGlob);
                            }
                        }
                    }

                    storeEstadosGlobales.DataSource = listaEstadosGlob;
                    storeEstadosGlobales.DataBind();
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

        #endregion

        #region INVENTARIOS ELEMENTOS ATRIBUTOS ESTADOS

        protected void storeInventarioElementosAtributosEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    /*List<Data.InventarioElementosAtributosEstados> listaEstados;
                    Object oEstadoGlob;
                    List<Object> listaEstadosGlob = new List<object>();

                    listaEstados = ListaEstados();

                    if (listaEstados != null)
                    {
                        foreach (Data.InventarioElementosAtributosEstados oDato in listaEstados)
                        {
                            oEstadoGlob = new
                            {
                                ID = oDato.InventarioElementoAtributoEstadoID,
                                Nombre = oDato.Codigo,
                            };

                            listaEstadosGlob.Add(oEstadoGlob);
                        }

                        storeInventarioElementosAtributosEstados.DataSource = listaEstadosGlob;
                        storeInventarioElementosAtributosEstados.DataBind();
                    }

                    */
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        /*private List<Data.InventarioElementosAtributosEstados> ListaEstados()
        {
            List<Data.InventarioElementosAtributosEstados> listaDatos;
            InventarioElementosAtributosEstadosController cEstados = new InventarioElementosAtributosEstadosController();

            try
            {
                listaDatos = cEstados.GetActivosLibres(long.Parse(hdEstadoID.Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        */

        #endregion

        #region DOCUMENTOS ESTADOS

        protected void storeDocumentosEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.DocumentosEstados> listaEstados;
                    Object oEstadoGlob;
                    List<Object> listaEstadosGlob = new List<object>();

                    /*listaEstados = ListaDocumentosEstados();

                    if (listaEstados != null)
                    {
                        foreach (Data.DocumentosEstados oDato in listaEstados)
                        {
                            oEstadoGlob = new
                            {
                                ID = oDato.DocumentoEstadoID,
                                Nombre = oDato.Codigo,
                            };

                            listaEstadosGlob.Add(oEstadoGlob);
                        }

                        storeDocumentosEstados.DataSource = listaEstadosGlob;
                        storeDocumentosEstados.DataBind();
                    }
                    */
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        /*private List<Data.DocumentosEstados> ListaDocumentosEstados()
        {
            List<Data.DocumentosEstados> listaDatos;
            DocumentosEstadosController cEstados = new DocumentosEstadosController();

            try
            {
                listaDatos = cEstados.GetActivosLibres(long.Parse(hdEstadoID.Value.ToString()));

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }
        */

        #endregion

        #region ESTADOS SIGUIENTES

        protected void storeCoreEstadosSiguientes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosSiguientes> listaEst;
                    Object oEstadoSig;
                    List<Object> listaEstadosSig = new List<object>();
                    EstadosController cEstados = new EstadosController();

                    listaEst = cEstados.getEstadosSiguientes(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaEst != null && listaEst.Count > 0)
                    {
                        foreach (Data.CoreEstadosSiguientes oDato in listaEst)
                        {
                            Data.CoreEstados oValor = cEstados.GetItem(oDato.CoreEstadoPosibleID);
                            oEstadoSig = new
                            {
                                CoreEstadoSiguienteID = oDato.CoreEstadoSiguienteID,
                                CoreEstadoPosibleID = oDato.CoreEstadoPosibleID,
                                Defecto = oDato.Defecto,
                                NombreEstado = oValor.Nombre,
                                CodigoEstado = oValor.Codigo,
                            };

                            listaEstadosSig.Add(oEstadoSig);
                        }

                        storeCoreEstadosSiguientes.DataSource = listaEstadosSig;
                        storeCoreEstadosSiguientes.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ESTADOS SIGUIENTES LIBRES

        protected void storeCoreEstadosSiguientesLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreEstados> listaEst;
                    List<Data.CoreEstados> listaEstados;
                    List<Data.CoreEstadosSiguientes> listaEstadosAsignados;
                    Object oEstadoSig;
                    List<Object> listaEstadosSig = new List<object>();
                    EstadosController cEstados = new EstadosController();

                    listaEst = cEstados.getEstadosSiguientesLibres(long.Parse(hdEstadoID.Value.ToString()), long.Parse(cmbWorkflows.SelectedItem.Value));
                    listaEstadosAsignados = cEstados.getEstadosSiguientes(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaEst != null)
                    {
                        if (listaEst.Count == 0 && listaEstadosAsignados.Count == 0)
                        {
                            listaEstados = cEstados.getAllEstados(long.Parse(hdEstadoID.Value.ToString()), long.Parse(cmbWorkflows.SelectedItem.Value));

                            foreach (Data.CoreEstados oDato in listaEstados)
                            {
                                oEstadoSig = new
                                {
                                    CoreEstadoSiguienteID = oDato.CoreEstadoID,
                                    NombreEstado = oDato.Nombre,
                                    CodigoEstado = oDato.Codigo,
                                };

                                listaEstadosSig.Add(oEstadoSig);
                            }
                        }
                        else
                        {
                            foreach (Data.Vw_CoreEstados oDato in listaEst)
                            {
                                oEstadoSig = new
                                {
                                    CoreEstadoSiguienteID = oDato.CoreEstadoID,
                                    NombreEstado = oDato.NombreEstado,
                                    CodigoEstado = oDato.Codigo,
                                };

                                listaEstadosSig.Add(oEstadoSig);
                            }
                        }

                        storeCoreEstadosSiguientesLibres.DataSource = listaEstadosSig;
                        storeCoreEstadosSiguientesLibres.DataBind();
                    }


                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region TAREAS ESTADOS ASIGNADOS

        protected void storeTareasEstadosAsignados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosRolesEscrituras> listaDatos;

                    listaDatos = ListaTareasEstadosAsignados();

                    if (listaDatos != null)
                    {

                        storeTareasEstadosAsignados.DataSource = listaDatos;
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

        private List<Data.CoreEstadosRolesEscrituras> ListaTareasEstadosAsignados()
        {
            List<Data.CoreEstadosRolesEscrituras> listaDatos;
            CoreEstadosRolesEscriturasController cPerfiles = new CoreEstadosRolesEscriturasController();
            long rolID = long.Parse(hdEstadoID.Value.ToString());
            try
            {
                listaDatos = cPerfiles.getRolByEstadoID(rolID);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region ROLES ESTADOS SEGUIMIENTO

        protected void storeRolesEstadosSeguimiento_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosRolesLectura> listaDatos;

                    listaDatos = ListaRolesEstadosSeguimiento();

                    if (listaDatos != null)
                    {

                        storeRolesEstadosSeguimiento.DataSource = listaDatos;
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

        private List<Data.CoreEstadosRolesLectura> ListaRolesEstadosSeguimiento()
        {
            List<Data.CoreEstadosRolesLectura> listaDatos;
            CoreEstadosRolesLecturaController cControl = new CoreEstadosRolesLecturaController();
            long rolID = long.Parse(hdEstadoID.Value.ToString());
            try
            {
                listaDatos = cControl.getRolByEstadoID(rolID);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region CORE ESTADOS ROLES

        protected void storeCoreEstadosRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreEstadosRoles> listaEst;
                    CoreEstadosRolesController cEstados = new CoreEstadosRolesController();

                    listaEst = cEstados.getRolesByEstadoID(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaEst != null)
                    {
                        storeCoreEstadosRoles.DataSource = listaEst;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ROLES LIBRES

        protected void storeRolesLibres_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Roles> listaEstados;
                    Object oRol;
                    List<Object> listaRoles = new List<object>();
                    RolesController cRoles = new RolesController();

                    listaEstados = cRoles.GetRolesLibres(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaEstados != null)
                    {
                        foreach (Data.Roles oDato in listaEstados)
                        {
                            oRol = new
                            {
                                RolID = oDato.RolID,
                                Codigo = oDato.Codigo,
                                Nombre = oDato.Nombre,
                                Activo = oDato.Activo,
                            };

                            listaRoles.Add(oRol);
                        }

                        storeRolesLibres.DataSource = listaRoles;
                        storeRolesLibres.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ESTADOS AGRUPACIONES

        protected void storeEstadosAgrupaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.EstadosAgrupaciones> listaAgrupaciones;

                    listaAgrupaciones = ListaAgrupaciones();

                    if (listaAgrupaciones != null)
                    {
                        storeEstadosAgrupaciones.DataSource = listaAgrupaciones;
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

        private List<Data.EstadosAgrupaciones> ListaAgrupaciones()
        {
            List<Data.EstadosAgrupaciones> listaDatos;
            AgrupacionEstadosController cAgrupaciones = new AgrupacionEstadosController();

            try
            {
                listaDatos = cAgrupaciones.GetActivos(long.Parse(hdCliID.Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region CORE WORKFLOWS
        protected void storeCoreWorkflows_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreWorkflows> lista = new List<Data.CoreWorkflows>();
                    lista = ListaCoreWorkflows();

                    storeCoreWorkflows.DataSource = lista;
                    storeCoreWorkflows.DataBind();
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                    MensajeBox(codTit, Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);
                }
            }

        }

        private List<Data.CoreWorkflows> ListaCoreWorkflows()
        {
            CoreWorkflowsController cWorkflows = new CoreWorkflowsController();
            List<Data.CoreWorkflows> listaDatos;

            try
            {
                listaDatos = cWorkflows.getAllWorkflowsActivas();
            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }


            return listaDatos;
        }

        #endregion

        #region USUARIOS

        protected void storeUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Usuarios> listaUsuarios;
                    UsuariosController cUser = new UsuariosController();

                    listaUsuarios = cUser.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaUsuarios != null)
                    {
                        storeUsuarios.DataSource = listaUsuarios;
                        storeUsuarios.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ROLES

        protected void storeRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Roles> listaRoles;
                    RolesController cRoles = new RolesController();

                    listaRoles = cRoles.GetActivos(long.Parse(hdCliID.Value.ToString()));

                    if (listaRoles != null)
                    {
                        storeRoles.DataSource = listaRoles;
                        storeRoles.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region NOTIFICACIONES ROLES

        protected void storeCoreEstadosNotificacionesRoles_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosNotificacionesRoles> listaEst = null;
                    //CoreEstadosNotificacionesRolesController cEstados = new CoreEstadosNotificacionesRolesController();

                    //listaEst = cEstados.getRoles(long.Parse(hdEstadoNotificacion.Value.ToString()));

                    if (listaEst != null && listaEst.Count > 0)
                    {
                        storeCoreEstadosNotificacionesRoles.DataSource = listaEst;
                        storeCoreEstadosNotificacionesRoles.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region NOTIFICACIONES USUARIOS

        protected void storeCoreEstadosNotificacionesUsuarios_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosNotificacionesUsuarios> listaEst = null;
                    //CoreEstadosNotificacionesRolesController cEstados = new CoreEstadosNotificacionesRolesController();

                    //listaEst = cEstados.getRoles(long.Parse(hdEstadoNotificacion.Value.ToString()));

                    if (listaEst != null && listaEst.Count > 0)
                    {
                        storeCoreEstadosNotificacionesUsuarios.DataSource = listaEst;
                        storeCoreEstadosNotificacionesUsuarios.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region NOTIFICACIONES 

        protected void storeCoreEstadosNotificaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    Container contenedor;
                    Button btnEliminarNotificacion;
                    TextArea txtUser;
                    TextArea txtRol;
                    TextArea txtMensajeDin;
                    List<Data.CoreEstadosNotificaciones> listaNot = null;
                    CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();
                    CoreEstadosNotificacionesRolesController cRoles = new CoreEstadosNotificacionesRolesController();
                    CoreEstadosNotificacionesUsuariosController cUsers = new CoreEstadosNotificacionesUsuariosController();
                    UsuariosController cUsuarios = new UsuariosController();
                    RolesController cRol = new RolesController();

                    listaNot = cNot.getNotificacionesByEstado(long.Parse(hdEstadoID.Value.ToString()));

                    if (listaNot != null && listaNot.Count > 0)
                    {
                        foreach (Data.CoreEstadosNotificaciones not in listaNot)
                        {
                            contenedor = new Container();
                            contenedor.Cls = "bordes";

                            List<long> listaUser = cUsers.getUserByNotificacionID(not.CoreEstadoNotificacionID);
                            txtUser = new TextArea();
                            txtUser.FieldLabel = GetGlobalResource("strUsuarios");
                            txtUser.LabelAlign = LabelAlign.Top;
                            txtUser.Scrollable = ScrollableOption.Vertical;
                            txtUser.Width = 110;
                            txtUser.RawText = "";
                            txtUser.Cls = "txtMensajeStatus";
                            txtUser.Editable = false;

                            foreach (long lUser in listaUser)
                            {
                                string sUsuarios = cUsuarios.GetItem(lUser).EMail;

                                if (txtUser.Text == "")
                                {
                                    txtUser.Text = sUsuarios;
                                }
                                else
                                {
                                    txtUser.Text = txtUser.Text + ", " + sUsuarios;
                                }
                            }
                            contenedor.Items.Add(txtUser);

                            List<long> listaRoles = cRoles.getRolByNotificacionID(not.CoreEstadoNotificacionID);
                            txtRol = new TextArea();
                            txtRol.FieldLabel = "Rol";
                            txtRol.Editable = false;
                            txtRol.Scrollable = ScrollableOption.Vertical;
                            txtRol.Width = 110;
                            txtRol.Cls = "txtMensajeStatus";
                            txtRol.RawText = "";
                            txtRol.LabelAlign = LabelAlign.Top;

                            foreach (long lRol in listaRoles)
                            {
                                string sRol = cRol.GetItem(lRol).Codigo;

                                if (txtRol.Text == "")
                                {
                                    txtRol.Text = sRol;
                                }
                                else
                                {
                                    txtRol.Text = txtRol.Text + ", " + sRol;
                                }
                            }
                            contenedor.Items.Add(txtRol);

                            txtMensajeDin = new TextArea();
                            txtMensajeDin.FieldLabel = GetGlobalResource("strMensaje");
                            txtMensajeDin.LabelAlign = LabelAlign.Top;
                            txtMensajeDin.Width = 220;
                            txtMensajeDin.Scrollable = ScrollableOption.Vertical;
                            txtMensajeDin.WidthSpec = "90%";
                            txtMensajeDin.RawText = "";
                            txtMensajeDin.Cls = "txtMensajeStatus";
                            txtMensajeDin.Editable = false;
                            txtMensajeDin.Text = not.Contenido;
                            contenedor.Items.Add(txtMensajeDin);

                            btnEliminarNotificacion = new Button();
                            btnEliminarNotificacion.Hidden = false;
                            btnEliminarNotificacion.Cls = "btnBasura-ctNotification FloatR btn-trans";
                            btnEliminarNotificacion.MarginSpec = "0 0 8px 0";
                            btnEliminarNotificacion.Listeners.Click.Handler = "EliminarNotificacion(" + not.CoreEstadoNotificacionID + ")";
                            contenedor.Items.Add(btnEliminarNotificacion);

                            cntNotificaciones.Items.Add(contenedor);
                        }
                    }

                    cntNotificaciones.UpdateContent();
                    storeCoreEstadosNotificaciones.DataSource = listaNot;
                    storeCoreEstadosNotificaciones.DataBind();
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region ESTADOS TAREAS

        protected void storeCoreEstadosTareas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.CoreEstadosTareas> listaEstadosTareas;
                    CoreTiposInformacionesAccionesController cTipoAccionesController = new CoreTiposInformacionesAccionesController();
                    CoreCustomFieldsController cAtributosController = new CoreCustomFieldsController();
                    CoreTiposInformacionesController cTipoController = new CoreTiposInformacionesController();
                    CoreTareasAccionesController cAccionesController = new CoreTareasAccionesController();
                    Object oTarea;
                    string sClaveInfo = "";
                    List<Object> listaTareas = new List<object>();

                    listaEstadosTareas = ListaTareasEstados();

                    if (listaEstadosTareas != null)
                    {
                        foreach (Data.CoreEstadosTareas estadoTarea in listaEstadosTareas)
                        {
                            CoreTiposInformacionesAcciones oTipoAccion = cTipoAccionesController.GetItem(estadoTarea.CoreTipoInformacionAccionID);
                            CoreTiposInformaciones oTipoInfo = cTipoController.GetItem(oTipoAccion.CoreTipoInformacionID);

                            if (oTipoInfo != null)
                            {
                                if (oTipoInfo.Codigo == "CUSTOMFIELD")
                                {
                                    CoreAtributosConfiguraciones oAtributo = cAtributosController.getAtributoByCodigo((long)estadoTarea.CoreWorkflowsInformaciones.CoreCustomFieldID);
                                    if (oAtributo != null)
                                    {
                                        sClaveInfo = oAtributo.Codigo;
                                    }
                                }
                                else
                                {
                                    sClaveInfo = GetGlobalResource(oTipoInfo.ClaveRecurso);
                                }
                                CoreTareasAcciones oTareaAccion = cAccionesController.GetItem(oTipoAccion.CoreTareaAccionID);

                                if (oTareaAccion != null)
                                {
                                    string sClaveAccion = GetGlobalResource(oTareaAccion.ClaveRecurso);

                                    oTarea = new
                                    {
                                        CoreEstadoTareaID = estadoTarea.CoreEstadoTareaID,
                                        Informacion = sClaveInfo,
                                        Accion = sClaveAccion,
                                        Obligatorio = estadoTarea.Obligatorio,
                                        Descripcion = estadoTarea.Descripcion
                                    };

                                    listaTareas.Add(oTarea);
                                }
                            }
                        }

                        storeCoreEstadosTareas.DataSource = listaTareas;
                        storeCoreEstadosTareas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        private List<Data.CoreEstadosTareas> ListaTareasEstados()
        {
            List<Data.CoreEstadosTareas> listaDatos;
            CoreEstadosTareasController cEstados = new CoreEstadosTareasController();

            try
            {
                listaDatos = cEstados.GetByEstado(long.Parse(hdEstadoID.Value.ToString()));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        #endregion

        #region CORE OBJETOS NEGOCIOS TIPOS

        protected void storeCoreObjetosNegocioTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    List<Data.Vw_CoreObjetosNegocioTipos> listaObjetos;
                    List<JsonObject> listaFinal = new List<JsonObject>();
                    CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();

                    listaObjetos = cObjetos.GetItemsList<Data.Vw_CoreObjetosNegocioTipos>();
                    listaObjetos.Sort((x, y) => x.Codigo.CompareTo(y.Codigo));

                    foreach (Data.Vw_CoreObjetosNegocioTipos oValor in listaObjetos)
                    {
                        string sNombre = "";

                        if (oValor.NombreObjeto != null && oValor.NombreObjeto.Contains(" "))
                        {
                            sNombre = oValor.NombreObjeto.Split(' ')[1];
                        }
                        else
                        {
                            sNombre = GetGlobalResource(oValor.NombreObjeto);
                        }

                        JsonObject oJson = new JsonObject();

                        oJson.Add("Codigo", oValor.Codigo);
                        oJson.Add("Nombre", sNombre);
                        oJson.Add("CoreObjetoNegocioTipoID", oValor.CoreObjetoNegocioTipoID);

                        listaFinal.Add(oJson);
                    }

                    if (listaFinal != null)
                    {
                        storeCoreObjetosNegocioTipos.DataSource = listaFinal;
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    string codTit = Util.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region INFORMACIONES TIPOS
        protected void storeCoreWorkflowsInformaciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreWorkflowsInformacionesController cCoreWorkflowsInformaciones = new CoreWorkflowsInformacionesController();

                try
                {
                    int i = 1;
                    List<JsonObject> listaFinal = new List<JsonObject>();
                    long lWorkflowID = long.Parse(hdWorkflows.Value.ToString());
                    long lEstadoID = long.Parse(hdEstadoID.Value.ToString());
                    List<CoreAtributosConfiguraciones> listaAtributos = cCoreWorkflowsInformaciones.GetatributosVinculados(lWorkflowID, lEstadoID);
                    List<CoreTiposInformaciones> listaTiposInfo = cCoreWorkflowsInformaciones.GetTiposInfoVinculados(lWorkflowID, lEstadoID);

                    foreach (CoreAtributosConfiguraciones oAtr in listaAtributos)
                    {
                        JsonObject oJson = new JsonObject();

                        oJson.Add("Nombre", oAtr.Codigo);
                        oJson.Add("Codigo", oAtr.Codigo);
                        oJson.Add("ID", i);

                        listaFinal.Add(oJson);
                        i++;
                    }

                    foreach (CoreTiposInformaciones oInfo in listaTiposInfo)
                    {
                        JsonObject oJson = new JsonObject();

                        oJson.Add("Nombre", GetGlobalResource(oInfo.ClaveRecurso));
                        oJson.Add("Codigo", oInfo.Codigo);
                        oJson.Add("ID", i);

                        listaFinal.Add(oJson);
                        i++;
                    }

                    if (listaFinal != null)
                    {
                        storeCoreWorkflowsInformaciones.DataSource = listaFinal;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #region TAREAS ACCIONES
        protected void storeCoreTiposInformacionesAcciones_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                CoreAtributosConfiguracionesController cAtributosController = new CoreAtributosConfiguracionesController();
                CoreTiposInformacionesController cTiposController = new CoreTiposInformacionesController();
                List<JsonObject> listaFinal = new List<JsonObject>();
                List<CoreTareasAcciones> lista = new List<CoreTareasAcciones>();
                CoreEstadosTareasController cEstados = new CoreEstadosTareasController();
                CoreTiposInformacionesAccionesController cCoreTareasAcciones = new CoreTiposInformacionesAccionesController();

                cTiposController.SetDataContext(cAtributosController.Context);
                cEstados.SetDataContext(cAtributosController.Context);
                cCoreTareasAcciones.SetDataContext(cAtributosController.Context);

                try
                {
                    if (cmbInformaciones.SelectedItem.Value != null)
                    {
                        string sInfo = cmbInformaciones.SelectedItem.Value.ToString();
                        CoreAtributosConfiguraciones oAtr = cAtributosController.GetAtributoByCodigo(sInfo);

                        if (oAtr == null)
                        {
                            CoreTiposInformaciones oTipo = cTiposController.GetInformacion(sInfo);

                            if (oTipo != null)
                            {
                                lista = cCoreTareasAcciones.getLista(oTipo.CoreTipoInformacionID);
                            }
                        }
                        else
                        {
                            // CustomField equivale a CoreTipoInformacionID = 1
                            lista = cCoreTareasAcciones.getLista(1);
                        }

                        if (lista != null)
                        {
                            foreach (CoreTareasAcciones oTarea in lista)
                            {
                                JsonObject oJson = new JsonObject();

                                oJson.Add("CoreTareaAccionID", oTarea.CoreTareaAccionID);
                                oJson.Add("Activo", oTarea.Activo);
                                oJson.Add("ClaveRecurso", GetGlobalResource(oTarea.ClaveRecurso));
                                oJson.Add("Codigo", oTarea.Codigo);

                                listaFinal.Add(oJson);
                            }

                            storeCoreTiposInformacionesAcciones.DataSource = listaFinal;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }
        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar, bool bDuplicar)
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstados = new EstadosController();
            InfoResponse oResponse;
            Data.CoreEstados oDato = new Data.CoreEstados();

            try
            {
                if (hdEstadoID.Value != null && hdEstadoID.Value.ToString() != "")
                {
                    oDato = cEstados.GetItem(long.Parse(hdEstadoID.Value.ToString()));

                    if (oDato.Codigo == txtCodigo.Text && oDato.Nombre == txtNombre.Text)
                    {
                        bAgregar = false;
                    }
                }

                if (!bAgregar)
                {
                    #region EDITAR

                    if (hdEstadoID.Value.ToString() == "")
                    {
                        long lS = long.Parse(GridRowSelectEstados.SelectedRecordID);
                        oDato = cEstados.GetItem(lS);
                    }

                    if (oDato != null)
                    {
                        oDato.Nombre = txtNombre.Text;
                        oDato.Codigo = txtCodigo.Text;
                        oDato.Activo = true;
                        oDato.Descripcion = txtDescripcion.Text;
                        oDato.Porcentaje = Convert.ToDouble(txtProgress.Text);

                        if (cmbWorkflows.SelectedItem != null && cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value != "")
                        {
                            oDato.CoreWorkFlowID = long.Parse(cmbWorkflows.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.CoreWorkFlowID = null;
                        }

                        if (cmbDepartamento.SelectedItem != null && cmbDepartamento.SelectedItem.Value != null && cmbDepartamento.SelectedItem.Value != "")
                        {
                            oDato.DepartamentoID = long.Parse(cmbDepartamento.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.DepartamentoID = 0;
                        }

                        if (cmbGrupos.SelectedItem != null && cmbGrupos.SelectedItem.Value != null && cmbGrupos.SelectedItem.Value != "")
                        {
                            oDato.EstadoAgrupacionID = long.Parse(cmbGrupos.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            oDato.EstadoAgrupacionID = 0;
                        }

                        if (chkCompletado.Checked)
                        {
                            oDato.Completado = true;
                        }
                        else
                        {
                            oDato.Completado = false;
                        }

                        if (btnWorkFlowPublicRol.Pressed)
                        {
                            oDato.PublicoLectura = false;
                        }
                        else
                        {
                            oDato.PublicoLectura = true;
                        }

                        if (btnWorkFlowPublicRolEscritura.Pressed)
                        {
                            oDato.PublicoEscritura = false;
                        }
                        else
                        {
                            oDato.PublicoEscritura = true;
                        }

                        oResponse = cEstados.Update(oDato);
                        if (oResponse.Result)
                        {
                            log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                            oResponse = cEstados.SubmitChanges();
                            storeCoreEstados.DataBind();

                            if (oResponse.Result)
                            {
                                direct.Success = true;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                            else
                            {
                                cEstados.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            cEstados.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region AGREGAR

                    oDato = new Data.CoreEstados();
                    oDato.Nombre = txtNombre.Text;
                    oDato.Codigo = txtCodigo.Text;
                    oDato.Activo = true;
                    oDato.Descripcion = txtDescripcion.Text;
                    oDato.Porcentaje = Convert.ToDouble(txtProgress.Text);

                    if (cmbWorkflows.SelectedItem != null && cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value != "")
                    {
                        oDato.CoreWorkFlowID = long.Parse(cmbWorkflows.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        oDato.CoreWorkFlowID = null;
                    }

                    if (cmbDepartamento.SelectedItem != null && cmbDepartamento.SelectedItem.Value != null && cmbDepartamento.SelectedItem.Value != "")
                    {
                        oDato.DepartamentoID = long.Parse(cmbDepartamento.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        oDato.DepartamentoID = 0;
                    }

                    if (cmbGrupos.SelectedItem != null && cmbGrupos.SelectedItem.Value != null && cmbGrupos.SelectedItem.Value != "")
                    {
                        oDato.EstadoAgrupacionID = long.Parse(cmbGrupos.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        oDato.EstadoAgrupacionID = 0;
                    }

                    if (chkCompletado.Checked)
                    {
                        oDato.Completado = true;
                    }
                    else
                    {
                        oDato.Completado = false;
                    }

                    oResponse = cEstados.Add(oDato);

                    if (oResponse.Result)
                    {
                        oResponse = cEstados.SubmitChanges();

                        if (oResponse.Result)
                        {
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);

                            log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                            hdEstadoID.Value = oDato.CoreEstadoID;

                            if (bDuplicar)
                            {
                                long lS = long.Parse(GridRowSelectEstados.SelectedRecordID);
                                ComprobarTabs(lS);

                                storeCoreEstados.Reload();
                            }
                        }
                        else
                        {
                            cEstados.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        return direct;
                    }

                    #endregion
                }
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
        public DirectResponse MostrarEditar()
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstados = new EstadosController();

            DepartamentosController cDepart = new DepartamentosController();
            AgrupacionEstadosController cAgrup = new AgrupacionEstadosController();
            cDepart.SetDataContext(cEstados.Context);
            cAgrup.SetDataContext(cEstados.Context);

            try
            {
                long lS = long.Parse(GridRowSelectEstados.SelectedRecordID);

                Data.Vw_CoreEstados oDato;
                oDato = cEstados.GetItem<Data.Vw_CoreEstados>(lS);

                Data.CoreEstados oEstado = cEstados.GetItem(lS);

                if (oDato != null && oEstado != null)
                {
                    txtNombre.Text = oDato.NombreEstado;
                    txtCodigo.Text = oDato.Codigo;
                    txtDescripcion.Text = oEstado.Descripcion;
                    txtProgress.Text = oDato.Porcentaje.ToString();
                    progressBar.Value = (float)(oDato.Porcentaje * 0.01);

                    if (oDato.Completado)
                    {
                        chkCompletado.Checked = true;
                    }
                    else
                    {
                        chkCompletado.Checked = false;
                    }

                    if (!oEstado.PublicoLectura)
                    {
                        btnWorkFlowPublicRol.Pressed = true;
                    }
                    else
                    {
                        btnWorkFlowPublicRol.Pressed = false;
                    }

                    if (!oEstado.PublicoEscritura)
                    {
                        btnWorkFlowPublicRolEscritura.Pressed = true;
                    }
                    else
                    {
                        btnWorkFlowPublicRolEscritura.Pressed = false;
                    }

                    long lDepartamentoID = cDepart.GetDepartamentoID(oDato.Departamento);
                    cmbDepartamento.SetValue(lDepartamentoID);

                    long lAgrupacionID = cAgrup.getNombreAgrupacion(oDato.NombreAgrupacion);
                    cmbGrupos.SetValue(lAgrupacionID);

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
            EstadosController cEstados = new EstadosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelectEstados.SelectedRecordID);
                Data.CoreEstados oDato = cEstados.GetItem(lID);
                oResponse = cEstados.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse ComprobarTabs(long lEstadoID)
        {
            DirectResponse direct = new DirectResponse();

            CoreEstadosGlobalesController cCoreEstadosGlobales = new CoreEstadosGlobalesController();
            Data.CoreEstadosGlobales oEstadoGlobal;
            List<Data.CoreEstadosGlobales> listaEstadoGlobal;

            CoreEstadosRolesController cCoreEstadosRoles = new CoreEstadosRolesController();
            Data.CoreEstadosRoles oEstadoRol;
            List<Data.CoreEstadosRoles> listaEstadoRol;

            EstadosSiguientesController cCoreEstadosSiguientes = new EstadosSiguientesController();
            Data.CoreEstadosSiguientes oEstadoSiguiente;
            List<Data.CoreEstadosSiguientes> listaEstadoSiguiente;

            CoreEstadosNotificacionesController cCoreEstadosNotificaciones = new CoreEstadosNotificacionesController();
            Data.CoreEstadosNotificaciones oEstadoNotificacion;
            List<Data.CoreEstadosNotificaciones> listaEstadoNotificacion;

            try
            {
                #region ESTADOS GLOBALES

                listaEstadoGlobal = cCoreEstadosGlobales.getCoreEstadosGlobales(lEstadoID);

                foreach (Data.CoreEstadosGlobales oGlobal in listaEstadoGlobal)
                {
                    oEstadoGlobal = new Data.CoreEstadosGlobales();
                    oEstadoGlobal.ObjetoEstadoID = oGlobal.ObjetoEstadoID;
                    oEstadoGlobal.CoreObjetoNegocioTipoID = oGlobal.CoreObjetoNegocioTipoID;
                    oEstadoGlobal.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());

                    cCoreEstadosGlobales.AddItem(oEstadoGlobal);
                }


                #endregion

                #region ROLES

                listaEstadoRol = cCoreEstadosRoles.getTablaRolesByEstadoID(lEstadoID);

                foreach (Data.CoreEstadosRoles oRol in listaEstadoRol)
                {
                    oEstadoRol = new Data.CoreEstadosRoles();
                    oEstadoRol.RolID = oRol.RolID;
                    oEstadoRol.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());

                    cCoreEstadosRoles.AddItem(oEstadoRol);
                }

                #endregion

                #region ESTADOS SIGUIENTES

                listaEstadoSiguiente = cCoreEstadosSiguientes.getEstadosSiguientesByEstadoID(lEstadoID);

                foreach (Data.CoreEstadosSiguientes oSiguiente in listaEstadoSiguiente)
                {
                    oEstadoSiguiente = new Data.CoreEstadosSiguientes();
                    oEstadoSiguiente.CoreEstadoPosibleID = oSiguiente.CoreEstadoPosibleID;
                    oEstadoSiguiente.Defecto = oSiguiente.Defecto;
                    oEstadoSiguiente.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());

                    cCoreEstadosSiguientes.AddItem(oEstadoSiguiente);
                }

                #endregion

                //#region DOCUMENTOS TIPOS

                //listaEstadoDocumento = cCoreEstadosDocumentos.getDocumentosByEstadoID(lEstadoID);

                //foreach (Data.CoreEstadosDocumentosTipos oDoc in listaEstadoDocumento)
                //{
                //    oEstadoDocumento = new Data.CoreEstadosDocumentosTipos();
                //    oEstadoDocumento.DocumentoTipoID = oDoc.DocumentoTipoID;
                //    oEstadoDocumento.Obligatorio = oDoc.Obligatorio;
                //    oEstadoDocumento.RequiereValidacion = oDoc.RequiereValidacion;
                //    oEstadoDocumento.RequiereFirma = oDoc.RequiereFirma;
                //    oEstadoDocumento.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());

                //    cCoreEstadosDocumentos.AddItem(oEstadoDocumento);
                //}

                //#endregion

                #region NOTIFICACIONES

                listaEstadoNotificacion = cCoreEstadosNotificaciones.getNotificacionesByEstado(lEstadoID);

                foreach (Data.CoreEstadosNotificaciones oNot in listaEstadoNotificacion)
                {
                    oEstadoNotificacion = new Data.CoreEstadosNotificaciones();
                    oEstadoNotificacion.Contenido = oNot.Contenido;
                    oEstadoNotificacion.Asunto = oNot.Asunto;
                    oEstadoNotificacion.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());

                    cCoreEstadosNotificaciones.AddItem(oEstadoNotificacion);

                    CoreEstadosNotificacionesRolesController cRoles = new CoreEstadosNotificacionesRolesController();
                    Data.CoreEstadosNotificacionesRoles oNotRol;
                    List<long> listaNotRoles = cRoles.getRolByNotificacionID(oNot.CoreEstadoNotificacionID);

                    foreach (long lRolID in listaNotRoles)
                    {
                        oNotRol = new Data.CoreEstadosNotificacionesRoles();
                        oNotRol.CoreEstadoNotificacionID = oEstadoNotificacion.CoreEstadoNotificacionID;
                        oNotRol.RolID = lRolID;

                        cRoles.AddItem(oNotRol);
                    }

                    CoreEstadosNotificacionesUsuariosController cUsuarios = new CoreEstadosNotificacionesUsuariosController();
                    Data.CoreEstadosNotificacionesUsuarios oNotUser;
                    List<long> listaNotUsers = cUsuarios.getUserByNotificacionID(oNot.CoreEstadoNotificacionID);

                    foreach (long lUserID in listaNotUsers)
                    {
                        oNotUser = new Data.CoreEstadosNotificacionesUsuarios();
                        oNotUser.CoreEstadoNotificacionID = oEstadoNotificacion.CoreEstadoNotificacionID;
                        oNotUser.UsuarioID = lUserID;

                        cUsuarios.AddItem(oNotUser);
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
        public DirectResponse AsignarPorDefecto()
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstados = new EstadosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelectEstados.SelectedRecordID);
                Data.CoreEstados oDato = cEstados.GetItem(lID);
                oResponse = cEstados.SetDefecto(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse Activar()
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cController = new EstadosController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(GridRowSelectEstados.SelectedRecordID);
                Data.CoreEstados oDato = cController.GetItem(lID);
                oResponse = cController.ModificarActivar(oDato);

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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

        [DirectMethod]
        public DirectResponse DuplicarWorkflow()
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstados = new EstadosController();
            EstadosController cEstadosDupli = new EstadosController();
            cEstadosDupli.SetDataContext(cEstados.Context);
            List<Data.CoreEstados> lEstados;
            List<Data.CoreEstados> lEstadosDupli;
            InfoResponse oResponse;

            try
            {
                lEstadosDupli = cEstadosDupli.GetCoreEstadosFromWorkflow(Convert.ToInt32(cmbWorkflows.SelectedItem.Value));

                if (lEstadosDupli.Count > 0)
                {
                    log.Error(GetGlobalResource("strDuplicarWorkflow"));
                }
                else
                {
                    Data.CoreEstados estDupli = new Data.CoreEstados();

                    lEstados = cEstados.GetCoreEstadosFromWorkflow(Convert.ToInt32(cmbDuplicateWorkflow.SelectedItem.Value));

                    foreach (Data.CoreEstados est in lEstados)
                    {
                        estDupli = new Data.CoreEstados();

                        estDupli.Codigo = est.Codigo;
                        estDupli.Nombre = est.Nombre;
                        estDupli.Porcentaje = est.Porcentaje;
                        estDupli.CoreEstadoID = est.CoreEstadoID;
                        estDupli.CoreWorkFlowID = long.Parse(cmbWorkflows.SelectedItem.Value);
                        estDupli.Defecto = est.Defecto;
                        estDupli.Completado = est.Completado;
                        estDupli.EstadoAgrupacionID = est.EstadoAgrupacionID;
                        estDupli.DepartamentoID = est.DepartamentoID;
                        estDupli.Descripcion = est.Descripcion;
                        estDupli.PublicoEscritura = est.PublicoEscritura;
                        estDupli.PublicoLectura = est.PublicoLectura;
                        estDupli.Activo = est.Activo;

                        oResponse = cEstadosDupli.Add(estDupli);

                        if (oResponse.Result)
                        {
                            oResponse = cEstados.SubmitChanges();

                            if (oResponse.Result)
                            {
                                direct.Success = true;
                                direct.Result = GetGlobalResource(oResponse.Description);

                                log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                                hdEstadoID.Value = estDupli.CoreEstadoID;
                                ComprobarTabs(est.CoreEstadoID);
                            }
                            else
                            {
                                cEstados.DiscardChanges();
                                direct.Success = false;
                                direct.Result = GetGlobalResource(oResponse.Description);
                            }
                        }
                        else
                        {
                            cEstados.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }
                    }
                }

                direct.Success = true;
                direct.Result = "";
            }
            catch (Exception ex)
            {
                log.Error(GetGlobalResource("strDuplicarWorkflow"));
                string codTit = Util.ExceptionHandler(ex);
                direct.Success = true;
            }

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ExportarFlujo()
        {
            DirectResponse direct = new DirectResponse();
            try
            {
                #region CONVERSION KEYS

                string sEstados = Comun.ConvertKeyXML(GetGlobalResource("strEstados"));
                string sCodigo = Comun.ConvertKeyXML(GetGlobalResource("strCodigo"));
                string sNombre = Comun.ConvertKeyXML(GetGlobalResource("strNombre"));
                string sPorcentaje = Comun.ConvertKeyXML(GetGlobalResource("strPorcentaje"));
                string sDefecto = Comun.ConvertKeyXML(GetGlobalResource("strDefecto"));
                string sActivo = Comun.ConvertKeyXML(GetGlobalResource("strActivo"));
                string sCompletado = Comun.ConvertKeyXML(GetGlobalResource("strCompletado"));
                string sGrupos = Comun.ConvertKeyXML(GetGlobalResource("strGrupos"));
                string sDepartamento = Comun.ConvertKeyXML(GetGlobalResource("strDepartamento"));
                string sEstadosSiguientes = Comun.ConvertKeyXML(GetGlobalResource("strEstadosSiguientes"));
                string sEstado = Comun.ConvertKeyXML(GetGlobalResource("strEstado"));
                string sTabla = Comun.ConvertKeyXML(GetGlobalResource("strTabla"));
                string sEstadoSiguiente = Comun.ConvertKeyXML(GetGlobalResource("strEstadoSiguiente"));
                string sTiposDocumentos = Comun.ConvertKeyXML(GetGlobalResource("strTiposDocumentos"));
                string sDocumentoTipo = Comun.ConvertKeyXML(GetGlobalResource("strDocumentoTipo"));
                string sRequerido = Comun.ConvertKeyXML(GetGlobalResource("strRequerido"));
                string sValidado = Comun.ConvertKeyXML(GetGlobalResource("strValidado"));
                string sObligatorio = Comun.ConvertKeyXML(GetGlobalResource("strObligatorio"));
                string sEstadosGlobales = Comun.ConvertKeyXML(GetGlobalResource("strEstadosGlobales"));
                string sEstadoGlobal = Comun.ConvertKeyXML(GetGlobalResource("strEstadoGlobal"));
                string sInventarioElemento = Comun.ConvertKeyXML(GetGlobalResource("strInventarioElemento"));
                string sDocumento = Comun.ConvertKeyXML(GetGlobalResource("strDocumento"));
                string sRoles = Comun.ConvertKeyXML(GetGlobalResource("strRoles"));
                string sMensaje = Comun.ConvertKeyXML(GetGlobalResource("strMensaje"));
                string sUsuario = Comun.ConvertKeyXML(GetGlobalResource("strUsuario"));
                string sNotificaciones = Comun.ConvertKeyXML(GetGlobalResource("strNotificaciones"));
                string sDescripcion = Comun.ConvertKeyXML(GetGlobalResource("strDescripcion"));
                string sPublicoLectura = Comun.ConvertKeyXML(GetGlobalResource("strPublicoLectura"));
                string sPublicoEscritura = Comun.ConvertKeyXML(GetGlobalResource("strPublicoEscritura"));
                string sInformacion = Comun.ConvertKeyXML(GetGlobalResource("strInfoDoc"));
                string sAccion = Comun.ConvertKeyXML(GetGlobalResource("strAccion"));
                string sTarea = Comun.ConvertKeyXML(GetGlobalResource("strTarea"));
                string sWorkflow = Comun.ConvertKeyXML(GetGlobalResource("strVerWorkflow"));

                #endregion

                CoreWorkflowsController cWorkflows = new CoreWorkflowsController();
                string sNombreWorkflow = "";
                long lWorkflowID = 0;

                if (cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value.ToString() != "")
                {
                    lWorkflowID = Convert.ToInt32(cmbWorkflows.SelectedItem.Value);
                    sNombreWorkflow = cWorkflows.getCodigoByID(lWorkflowID);
                }

                log.Info(GetGlobalResource("strExportarFlujo"));
                log.Info(GetGlobalResource("strComienzoExportacion"));

                List<Data.Vw_CoreEstados> listaEstados = new List<Data.Vw_CoreEstados>();
                EstadosController cEstados = new EstadosController();
                listaEstados = cEstados.GetCoreEstadosFromWorkflowID(lWorkflowID);

                //
                // Descargo la tabla de BBDD sobre un DataTale
                //
                DataTable EstadosDt = new DataTable();
                EstadosDt.TableName = sNombreWorkflow;

                EstadosDt.Columns.Add(sCodigo);
                EstadosDt.Columns.Add(sNombre);
                EstadosDt.Columns.Add(sPorcentaje);
                EstadosDt.Columns.Add(sDefecto);
                EstadosDt.Columns.Add(sActivo);
                EstadosDt.Columns.Add(sCompletado);
                EstadosDt.Columns.Add(sGrupos);
                EstadosDt.Columns.Add(sDepartamento);
                EstadosDt.Columns.Add(sDescripcion);
                EstadosDt.Columns.Add(sPublicoLectura);
                EstadosDt.Columns.Add(sPublicoEscritura);

                foreach (var item in listaEstados)
                {
                    EstadosDt.Rows.Add();

                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sCodigo] = item.Codigo;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sPorcentaje] = item.Porcentaje;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sNombre] = item.NombreEstado;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sCompletado] = item.Completado;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sDefecto] = item.Defecto;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sActivo] = item.Activo;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sGrupos] = item.NombreAgrupacion;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sDepartamento] = item.Departamento;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sDescripcion] = item.Descripcion;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sPublicoLectura] = item.PublicoLectura;
                    EstadosDt.Rows[EstadosDt.Rows.Count - 1][sPublicoEscritura] = item.PublicoEscritura;
                }

                #region ESTADOS SIGUIENTES 

                List<Data.CoreEstadosSiguientes> listaEstadosSig = new List<Data.CoreEstadosSiguientes>();
                EstadosSiguientesController cEstadosSiguientes = new EstadosSiguientesController();
                EstadosSiguientesController cEstSig = new EstadosSiguientesController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosSiguientes> aux;
                    aux = cEstadosSiguientes.getEstadosSiguientesByEstadoID(item.CoreEstadoID);
                    listaEstadosSig.AddRange(aux);
                }

                #region DATA TABLE

                DataTable dt1 = new DataTable();
                dt1.TableName = sEstadosSiguientes;

                dt1.Columns.Add(sEstado);
                dt1.Columns.Add(sEstadoSiguiente);
                dt1.Columns.Add(sDefecto);

                #endregion

                #region ESTADOS

                foreach (var item in listaEstadosSig)
                {
                    dt1.Rows.Add();

                    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt1.Rows[dt1.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    if (item.CoreEstadoPosibleID != 0 && item.CoreEstadoPosibleID != null && !DBNull.Value.Equals(item.CoreEstadoPosibleID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoPosibleID);
                        dt1.Rows[dt1.Rows.Count - 1][sEstadoSiguiente] = oEst.Codigo;
                    }

                    dt1.Rows[dt1.Rows.Count - 1][sDefecto] = item.Defecto;
                }

                #endregion

                #endregion

                #region ESTADOS GLOBALES

                List<Data.CoreEstadosGlobales> listaEstadosGlobales = new List<Data.CoreEstadosGlobales>();
                CoreEstadosGlobalesController cEstadosGlobales = new CoreEstadosGlobalesController();
                CoreEstadosGlobalesController cEstGlob = new CoreEstadosGlobalesController();
                CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                TablasModeloDatosController cTablas = new TablasModeloDatosController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosGlobales> aux = new List<Data.CoreEstadosGlobales>();
                    aux = cEstadosGlobales.getCoreEstadosGlobales(item.CoreEstadoID);
                    listaEstadosGlobales.AddRange(aux);
                }

                #region DATA TABLE

                DataTable dt3 = new DataTable();
                dt3.TableName = sEstadosGlobales;

                dt3.Columns.Add(sEstado);
                dt3.Columns.Add(sEstadoGlobal);
                dt3.Columns.Add(sTabla);
                dt3.Columns.Add(sCodigo);

                #endregion

                #region ESTADOS

                foreach (var item in listaEstadosGlobales)
                {
                    dt3.Rows.Add();

                    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt3.Rows[dt3.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    if (item.ObjetoEstadoID != 0 && item.ObjetoEstadoID != null && !DBNull.Value.Equals(item.ObjetoEstadoID))
                    {
                        if (item.CoreObjetoNegocioTipoID != 0 && item.CoreObjetoNegocioTipoID != null && !DBNull.Value.Equals(item.CoreObjetoNegocioTipoID))
                        {
                            Data.CoreObjetosNegocioTipos oNegocioTipo = cObjetos.GetItem((long)item.CoreObjetoNegocioTipoID);

                            if (oNegocioTipo != null)
                            {
                                DataTable oEstado;
                                string sNombreTabla = "";

                                sNombreTabla = cTablas.getClaveByID(oNegocioTipo.TablaModeloDatoID);
                                oEstado = cObjetos.getEstadoByID(item.CoreObjetoNegocioTipoID, item.ObjetoEstadoID);

                                foreach (DataRow oValor in oEstado.Rows)
                                {
                                    dt3.Rows[dt3.Rows.Count - 1][sEstadoGlobal] = oValor[0].ToString();
                                    dt3.Rows[dt3.Rows.Count - 1][sTabla] = GetGlobalResource(sNombreTabla);
                                    dt3.Rows[dt3.Rows.Count - 1][sCodigo] = oNegocioTipo.Codigo;
                                }
                            }
                        }
                    }
                }

                #endregion

                #endregion

                #region ROLES LECTURA

                List<Data.CoreEstadosRolesLectura> listaEstadosRolesLectura = new List<Data.CoreEstadosRolesLectura>();
                CoreEstadosRolesLecturaController cEstadosRolesLec = new CoreEstadosRolesLecturaController();
                CoreEstadosRolesController cRoles = new CoreEstadosRolesController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosRolesLectura> auxLec = new List<Data.CoreEstadosRolesLectura>();
                    auxLec = cEstadosRolesLec.getTablaRolesByEstadoID(item.CoreEstadoID);
                    listaEstadosRolesLectura.AddRange(auxLec);
                }

                #region DATA TABLE

                DataTable dt4 = new DataTable();
                dt4.TableName = "Reading Rols";

                dt4.Columns.Add(sEstado);
                dt4.Columns.Add("Rol");

                #endregion

                #region ESTADOS

                foreach (var item in listaEstadosRolesLectura)
                {
                    dt4.Rows.Add();

                    if (item.CoreEstadoID != 0 && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt4.Rows[dt4.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    if (item.RolID != 0 && !DBNull.Value.Equals(item.RolID))
                    {
                        Data.Roles oEst = new Data.Roles();
                        RolesController cRol = new RolesController();

                        oEst = cRol.GetItem((long)item.RolID);
                        dt4.Rows[dt4.Rows.Count - 1]["Rol"] = oEst.Codigo;
                    }
                }

                #endregion

                #endregion

                #region ROLES ESCRITURA

                List<Data.CoreEstadosRolesEscrituras> listaEstadosRolesEscrituras = new List<Data.CoreEstadosRolesEscrituras>();
                CoreEstadosRolesEscriturasController cEstadosRolesEsc = new CoreEstadosRolesEscriturasController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosRolesEscrituras> auxEsc = new List<Data.CoreEstadosRolesEscrituras>();
                    auxEsc = cEstadosRolesEsc.getTablaRolesByEstadoID(item.CoreEstadoID);
                    listaEstadosRolesEscrituras.AddRange(auxEsc);
                }

                #region DATA TABLE

                DataTable dt5 = new DataTable();
                dt5.TableName = "Writing Rols";

                dt5.Columns.Add(sEstado);
                dt5.Columns.Add("Rol");

                #endregion

                #region ESTADOS

                foreach (var item in listaEstadosRolesEscrituras)
                {
                    dt5.Rows.Add();

                    if (item.CoreEstadoID != 0 && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt5.Rows[dt5.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    if (item.RolID != 0 && !DBNull.Value.Equals(item.RolID))
                    {
                        Data.Roles oEst = new Data.Roles();
                        RolesController cRol = new RolesController();

                        oEst = cRol.GetItem((long)item.RolID);
                        dt5.Rows[dt5.Rows.Count - 1]["Rol"] = oEst.Codigo;
                    }
                }

                #endregion

                #endregion

                #region TAREAS

                List<Data.CoreEstadosTareas> listaEstadosTareas = new List<Data.CoreEstadosTareas>();
                CoreEstadosTareasController cEstadosTareas = new CoreEstadosTareasController();
                CoreWorkflowsInformacionesController cWorkflowsInfos = new CoreWorkflowsInformacionesController();
                CoreTiposInformacionesController cTiposInfo = new CoreTiposInformacionesController();
                CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                CoreTiposInformacionesAccionesController cAcciones = new CoreTiposInformacionesAccionesController();
                CoreTareasAccionesController cTarAcciones = new CoreTareasAccionesController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosTareas> aux = new List<Data.CoreEstadosTareas>();
                    aux = cEstadosTareas.GetByEstado(item.CoreEstadoID);
                    listaEstadosTareas.AddRange(aux);
                }

                #region DATA TABLE

                DataTable dt6 = new DataTable();
                dt6.TableName = sTarea;

                dt6.Columns.Add(sEstado);
                dt6.Columns.Add(sWorkflow);
                dt6.Columns.Add(sInformacion);
                dt6.Columns.Add(sAccion);
                dt6.Columns.Add(sDescripcion);
                dt6.Columns.Add(sObligatorio);

                #endregion

                #region ESTADOS

                foreach (var item in listaEstadosTareas)
                {
                    dt6.Rows.Add();

                    if (item.CoreEstadoID != 0 && item.CoreEstadoID != null && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt6.Rows[dt6.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    dt6.Rows[dt6.Rows.Count - 1][sObligatorio] = item.Obligatorio;
                    dt6.Rows[dt6.Rows.Count - 1][sDescripcion] = item.Descripcion;

                    if (item.CoreWorkflowInformacionID != 0 && item.CoreWorkflowInformacionID != null && !DBNull.Value.Equals(item.CoreWorkflowInformacionID))
                    {
                        CoreWorkflowsInformaciones oWorkflowsInfo = cWorkflowsInfos.GetItem(item.CoreWorkflowInformacionID);

                        if (oWorkflowsInfo != null)
                        {
                            if (oWorkflowsInfo.CoreCustomFieldID == null)
                            {
                                CoreTiposInformaciones oTipoInfo = cTiposInfo.GetItem(oWorkflowsInfo.CoreTipoInformacionID);

                                if (oTipoInfo != null)
                                {
                                    dt6.Rows[dt6.Rows.Count - 1][sInformacion] = oTipoInfo.Codigo;
                                }
                            }
                            else
                            {
                                CoreAtributosConfiguraciones oAtributo = cAtributos.getAtributoByID(oWorkflowsInfo.CoreCustomFields.CoreAtributoConfiguracionID);

                                if (oAtributo != null)
                                {
                                    dt6.Rows[dt6.Rows.Count - 1][sInformacion] = oAtributo.Codigo;
                                }
                            }

                            dt6.Rows[dt6.Rows.Count - 1][sWorkflow] = oWorkflowsInfo.CoreWorkflows.Nombre;
                        }
                    }

                    if (item.CoreTipoInformacionAccionID != 0 && item.CoreTipoInformacionAccionID != null && !DBNull.Value.Equals(item.CoreTipoInformacionAccionID))
                    {
                        CoreTiposInformacionesAcciones oAccion = cAcciones.GetItem(item.CoreTipoInformacionAccionID);

                        if (oAccion != null)
                        {
                            CoreTareasAcciones oTareaAccion = cTarAcciones.GetItem(oAccion.CoreTareaAccionID);

                            if (oTareaAccion != null)
                            {
                                dt6.Rows[dt6.Rows.Count - 1][sAccion] = oTareaAccion.Codigo;
                            }
                        }
                    }
                }

                #endregion

                #endregion

                #region NOTIFICACIONES

                List<Data.CoreEstadosNotificaciones> listaNot = new List<Data.CoreEstadosNotificaciones>();
                CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();

                foreach (var item in listaEstados)
                {
                    List<Data.CoreEstadosNotificaciones> aux = new List<Data.CoreEstadosNotificaciones>();
                    aux = cNot.getNotificacionesByEstado(item.CoreEstadoID);
                    listaNot.AddRange(aux);
                }

                #region DATA TABLE

                DataTable dt7 = new DataTable();
                dt7.TableName = sNotificaciones;

                dt7.Columns.Add(sEstado);
                dt7.Columns.Add(sUsuario);
                dt7.Columns.Add("Rol");
                dt7.Columns.Add(sMensaje);

                #endregion

                #region ESTADOS

                foreach (var item in listaNot)
                {
                    dt7.Rows.Add();

                    dt7.Rows[dt7.Rows.Count - 1][sMensaje] = item.Contenido;

                    if (item.CoreEstadoID != 0 && !DBNull.Value.Equals(item.CoreEstadoID))
                    {
                        Data.CoreEstados oEst = new Data.CoreEstados();

                        oEst = cEstados.GetItem((long)item.CoreEstadoID);
                        dt7.Rows[dt7.Rows.Count - 1][sEstado] = oEst.Codigo;
                    }

                    CoreEstadosNotificacionesRolesController cNotificacionesRoles = new CoreEstadosNotificacionesRolesController();
                    List<long> listaRoles = cNotificacionesRoles.getRolByNotificacionID(item.CoreEstadoNotificacionID);

                    foreach (var rol in listaRoles)
                    {
                        Data.Roles oRol = new Data.Roles();
                        RolesController cRolesCont = new RolesController();

                        oRol = cRolesCont.GetItem(rol);

                        if (oRol != null)
                        {
                            dt7.Rows[dt7.Rows.Count - 1]["Rol"] = oRol.Codigo;
                        }
                    }

                    CoreEstadosNotificacionesUsuariosController cUsuarios = new CoreEstadosNotificacionesUsuariosController();
                    List<long> listaUsuarios = cUsuarios.getUserByNotificacionID(item.CoreEstadoNotificacionID);

                    foreach (var user in listaUsuarios)
                    {
                        Data.Usuarios oUser = new Data.Usuarios();
                        UsuariosController cUserCont = new UsuariosController();

                        oUser = cUserCont.GetItem(user);

                        if (oUser != null)
                        {
                            dt7.Rows[dt7.Rows.Count - 1][sUsuario] = oUser.EMail;
                        }
                    }
                }

                #endregion

                #endregion

                string Filepath = "";
                Filepath = TreeCore.DirectoryMapping.GetDocumentDirectory();

                DateTime fecha = DateTime.Today;
                string fileName = "";

                if (sNombreWorkflow != null)
                {
                    fileName = sEstados + " - " + sNombreWorkflow + " - " + fecha.Day + fecha.Month + fecha.Year + ".xml";
                    fileName = fileName.Replace(" ", "");
                }
                else
                {
                    fileName = sEstados + " - " + fecha.Day + fecha.Month + fecha.Year + ".xml";
                }

                string fullpath = Path.Combine(Filepath, fileName);

                if (!System.IO.Directory.Exists(Filepath))
                { System.IO.Directory.CreateDirectory(Filepath); }

                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }

                DataTable[] tablas = { EstadosDt, dt1, dt3, dt4, dt5, dt6, dt7 };

                XmlSerializer serializer = new XmlSerializer(typeof(DataTable[]));
                TextWriter writer = new StreamWriter(fullpath);
                serializer.Serialize(writer, tablas);
                writer.Close();

            }
            catch (Exception ex)
            {
                log.Error(GetGlobalResource("strExportarFlujo"));
                log.Error(ex.Message);

                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            direct.Success = true;
            direct.Result = GetGlobalResource("strDescargaCorrecta");

            return direct;
        }

        [DirectMethod()]
        public DirectResponse ImportarFlujo()
        {
            DirectResponse direct = new DirectResponse();
            string sEstadosRepetidos = "";
            List<Object> listaMensajes = new List<Object>();
            Object oDato;
            var lista = "";

            #region CONVERSION KEYS

            string sEstados = Comun.ConvertKeyXML(GetGlobalResource("strEstados"));
            string sCodigo = Comun.ConvertKeyXML(GetGlobalResource("strCodigo"));
            string sNombre = Comun.ConvertKeyXML(GetGlobalResource("strNombre"));
            string sPorcentaje = Comun.ConvertKeyXML(GetGlobalResource("strPorcentaje"));
            string sDefecto = Comun.ConvertKeyXML(GetGlobalResource("strDefecto"));
            string sActivo = Comun.ConvertKeyXML(GetGlobalResource("strActivo"));
            string sCompletado = Comun.ConvertKeyXML(GetGlobalResource("strCompletado"));
            string sGrupos = Comun.ConvertKeyXML(GetGlobalResource("strGrupos"));
            string sDepartamento = Comun.ConvertKeyXML(GetGlobalResource("strDepartamento"));
            string sEstadosSiguientes = Comun.ConvertKeyXML(GetGlobalResource("strEstadosSiguientes"));
            string sEstado = Comun.ConvertKeyXML(GetGlobalResource("strEstado"));
            string sTabla = Comun.ConvertKeyXML(GetGlobalResource("strTabla"));
            string sEstadoSiguiente = Comun.ConvertKeyXML(GetGlobalResource("strEstadoSiguiente"));
            string sTiposDocumentos = Comun.ConvertKeyXML(GetGlobalResource("strTiposDocumentos"));
            string sDocumentoTipo = Comun.ConvertKeyXML(GetGlobalResource("strDocumentoTipo"));
            string sRequerido = Comun.ConvertKeyXML(GetGlobalResource("strRequerido"));
            string sValidado = Comun.ConvertKeyXML(GetGlobalResource("strValidado"));
            string sObligatorio = Comun.ConvertKeyXML(GetGlobalResource("strObligatorio"));
            string sEstadosGlobales = Comun.ConvertKeyXML(GetGlobalResource("strEstadosGlobales"));
            string sEstadoGlobal = Comun.ConvertKeyXML(GetGlobalResource("strEstadoGlobal"));
            string sInventarioElemento = Comun.ConvertKeyXML(GetGlobalResource("strInventarioElemento"));
            string sDocumento = Comun.ConvertKeyXML(GetGlobalResource("strDocumento"));
            string sRoles = Comun.ConvertKeyXML(GetGlobalResource("strRoles"));
            string sMensaje = Comun.ConvertKeyXML(GetGlobalResource("strMensaje"));
            string sUsuario = Comun.ConvertKeyXML(GetGlobalResource("strUsuario"));
            string sNotificaciones = Comun.ConvertKeyXML(GetGlobalResource("strNotificaciones"));
            string sDescripcion = Comun.ConvertKeyXML(GetGlobalResource("strDescripcion"));
            string sPublicoLectura = Comun.ConvertKeyXML(GetGlobalResource("strPublicoLectura"));
            string sPublicoEscritura = Comun.ConvertKeyXML(GetGlobalResource("strPublicoEscritura"));
            string sInformacion = Comun.ConvertKeyXML(GetGlobalResource("strInfoDoc"));
            string sAccion = Comun.ConvertKeyXML(GetGlobalResource("strAccion"));
            string sTarea = Comun.ConvertKeyXML(GetGlobalResource("strTarea"));
            string sWorkflow = Comun.ConvertKeyXML(GetGlobalResource("strVerWorkflow"));

            #endregion

            try
            {
                CoreWorkflowsController cWorkflows = new CoreWorkflowsController();
                string sNombreWorkflow = "";
                long lWorkflowID = 0;


                if (cmbWorkflows.SelectedItem.Value != null && cmbWorkflows.SelectedItem.Value.ToString() != "")
                {
                    lWorkflowID = Convert.ToInt32(cmbWorkflows.SelectedItem.Value);
                    sNombreWorkflow = cWorkflows.getCodigoByID(lWorkflowID);
                }

                string fileName = Path.GetFileName(FileUploadImportar.PostedFile.FileName);
                string ext = Path.GetExtension(FileUploadImportar.PostedFile.FileName);

                if (ext != ".xml")
                {
                    log.Error(GetGlobalResource("strFormatoIncorrecto"));

                    oDato = cargarMensajes(GetGlobalResource("strFormatoIncorrecto"), "", false, "General");
                    listaMensajes.Add(oDato);
                }
                else
                {
                    string fileDirectory = "";
                    fileDirectory = TreeCore.DirectoryMapping.GetDocumentDirectory();

                    if (!System.IO.Directory.Exists(fileDirectory))
                    { System.IO.Directory.CreateDirectory(fileDirectory); }

                    string fullpath = Path.Combine(fileDirectory, fileName);

                    if (!System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }

                    if (FileUploadImportar.HasFile)
                    {
                        FileUploadImportar.PostedFile.SaveAs(fullpath);

                        // Deserializo y paso a formato Datatable el documento xml, guardo el fichero provisional
                        XmlSerializer serializer = new XmlSerializer(typeof(DataTable[]));
                        FileStream loadStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                        DataTable[] tablas = (DataTable[])serializer.Deserialize(loadStream);
                        loadStream.Close();
                        int iEstadoAñadido = 0;
                        int iEstadoSiguienteAñadido = 0;
                        int iEstadoGlobalAñadido = 0;
                        int iRolAñadido = 0;
                        int iTareaAñadida = 0;
                        int iNotificacionAñadido = 0;

                        EstadosController cEstados = new EstadosController();

                        for (int k = 0; k < tablas[0].Rows.Count; k++)
                        {
                            Data.CoreEstados oEstado = new Data.CoreEstados();
                            DataRow row = tablas[0].Rows[k];

                            oEstado.Codigo = Convert.ToString(row[sCodigo]);
                            oEstado.Porcentaje = Convert.ToInt32(row[sPorcentaje]);
                            oEstado.Nombre = Convert.ToString(row[sNombre]);
                            oEstado.Completado = Convert.ToBoolean(row[sCompletado]);
                            oEstado.Defecto = Convert.ToBoolean(row[sDefecto]);
                            oEstado.Activo = Convert.ToBoolean(row[sActivo]);
                            oEstado.Descripcion = Convert.ToString(row[sDescripcion]);
                            oEstado.PublicoLectura = Convert.ToBoolean(row[sPublicoLectura]);
                            oEstado.PublicoEscritura = Convert.ToBoolean(row[sPublicoEscritura]);
                            oEstado.CoreWorkFlowID = lWorkflowID;

                            if (row[sGrupos] != null && !DBNull.Value.Equals(row[sGrupos]))
                            {
                                AgrupacionEstadosController cGrup = new AgrupacionEstadosController();
                                oEstado.EstadoAgrupacionID = cGrup.GetEstadosAgrupacionesByNombre(row[sGrupos].ToString()).EstadoAgrupacionID;
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sGrupos, false, oEstado.Codigo);
                                listaMensajes.Add(oDato);
                            }

                            if (row[sDepartamento] != null && !DBNull.Value.Equals(row[sDepartamento]))
                            {
                                DepartamentosController cDep = new DepartamentosController();
                                oEstado.DepartamentoID = cDep.GetDepartamentoID(row[sDepartamento].ToString());
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sDepartamento, false, oEstado.Codigo);
                                listaMensajes.Add(oDato);
                            }

                            if (!cEstados.ExisteEstadoByWorkflow((long)oEstado.CoreWorkFlowID, oEstado.Codigo, oEstado.Nombre))
                            {
                                oEstado = cEstados.AddItem(oEstado);
                                iEstadoAñadido++;

                                oDato = cargarMensajes(GetGlobalResource("strEstadoAñadido"), oEstado.Codigo, true, oEstado.Codigo);
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                if (sEstadosRepetidos.Equals(""))
                                {
                                    sEstadosRepetidos = Convert.ToString(row[sCodigo]);
                                }
                                else
                                {
                                    sEstadosRepetidos += (", " + Convert.ToString(row[sCodigo]));
                                }
                            }
                        }

                        #region ESTADOS SIGUIENTES

                        EstadosSiguientesController cEstadosSiguientes = new EstadosSiguientesController();

                        for (int k = 0; k < tablas[1].Rows.Count; k++)
                        {
                            Data.CoreEstadosSiguientes oEstSig = new Data.CoreEstadosSiguientes();
                            DataRow row = tablas[1].Rows[k];

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstSig.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row[sEstadoSiguiente] != null && !DBNull.Value.Equals(row[sEstadoSiguiente]))
                            {
                                oEstSig.CoreEstadoPosibleID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstadoSiguiente].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstadoSiguiente, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            oEstSig.Defecto = Convert.ToBoolean(row[sDefecto]);

                            if (!cEstadosSiguientes.RegistroDuplicado(oEstSig))
                            {
                                cEstadosSiguientes.AddItem(oEstSig);
                                iEstadoSiguienteAñadido++;

                                oDato = cargarMensajes(GetGlobalResource("strEstadoSiguienteAñadido"), Convert.ToString(row[sEstadoSiguiente]), true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strEstadoSiguienteDuplicado"), Convert.ToString(row[sEstadoSiguiente]), false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        #region ESTADOS GLOBALES 

                        CoreEstadosGlobalesController cEstadosGlobales = new CoreEstadosGlobalesController();
                        CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                        TablasModeloDatosController cTablas = new TablasModeloDatosController();

                        for (int k = 0; k < tablas[2].Rows.Count; k++)
                        {
                            Data.CoreEstadosGlobales oEstGlobal = new Data.CoreEstadosGlobales();
                            DataRow row = tablas[2].Rows[k];
                            string sEstGlo = "";

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstGlobal.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row[sEstadoGlobal] == null && DBNull.Value.Equals(row[sEstadoGlobal])
                                && row[sTabla] == null && DBNull.Value.Equals(row[sTabla]))
                            {
                                sEstGlo = Convert.ToString(row[sEstadoGlobal]);
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstadoGlobal, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                CoreObjetosNegocioTiposController cObjetosNegocios = new CoreObjetosNegocioTiposController();
                                Data.CoreObjetosNegocioTipos oNegocioTipo = cObjetosNegocios.getItemByCodigo(row[sCodigo].ToString());

                                if (oNegocioTipo != null)
                                {
                                    sEstGlo = Convert.ToString(row[sEstadoGlobal]);
                                    oEstGlobal.CoreObjetoNegocioTipoID = oNegocioTipo.CoreObjetoNegocioTipoID;
                                    DataTable oTabla = cObjetosNegocios.getIDByEstadoNegocio(oNegocioTipo.CoreObjetoNegocioTipoID, row[sEstadoGlobal].ToString());
                                    oEstGlobal.ObjetoEstadoID = long.Parse(oTabla.Rows[0][0].ToString());
                                }
                            }

                            if (!cEstadosGlobales.RegistroDuplicado(oEstGlobal))
                            {
                                cEstadosGlobales.AddItem(oEstGlobal);
                                iEstadoGlobalAñadido++;

                                oDato = cargarMensajes(GetGlobalResource("strEstadoGlobalAñadido"), sEstGlo, true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strEstadoGlobalDuplicado"), sEstGlo, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        #region ROLES LECTURA

                        CoreEstadosRolesLecturaController cEstadosRolesLectura = new CoreEstadosRolesLecturaController();
                        RolesController cRoles = new RolesController();

                        for (int k = 0; k < tablas[3].Rows.Count; k++)
                        {
                            Data.CoreEstadosRolesLectura oEstRolLectura = new Data.CoreEstadosRolesLectura();
                            DataRow row = tablas[3].Rows[k];

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstRolLectura.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row["Rol"] != null && !DBNull.Value.Equals(row["Rol"]))
                            {
                                oEstRolLectura.RolID = cRoles.getIDByCodigo(row["Rol"].ToString());
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), "Rol", false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (!cEstadosRolesLectura.RegistroDuplicado2((long)oEstRolLectura.CoreEstadoID, (long)oEstRolLectura.RolID))
                            {
                                cEstadosRolesLectura.AddItem(oEstRolLectura);
                                iRolAñadido++;

                                oDato = cargarMensajes(GetGlobalResource("strRolAñadido"), Convert.ToString(row["Rol"]), true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strRolDuplicado"), Convert.ToString(row["Rol"]), false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        #region ROLES ESCRITURA

                        CoreEstadosRolesEscriturasController cEstadosRolesEscrituras = new CoreEstadosRolesEscriturasController();

                        for (int k = 0; k < tablas[4].Rows.Count; k++)
                        {
                            Data.CoreEstadosRolesEscrituras oEstRolEsc = new Data.CoreEstadosRolesEscrituras();
                            DataRow row = tablas[4].Rows[k];

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstRolEsc.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row["Rol"] != null && !DBNull.Value.Equals(row["Rol"]))
                            {
                                oEstRolEsc.RolID = cRoles.getIDByCodigo(row["Rol"].ToString());
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), "Rol", false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (!cEstadosRolesEscrituras.RegistroDuplicado2((long)oEstRolEsc.CoreEstadoID, (long)oEstRolEsc.RolID))
                            {
                                cEstadosRolesEscrituras.AddItem(oEstRolEsc);
                                iRolAñadido++;

                                oDato = cargarMensajes(GetGlobalResource("strRolAñadido"), Convert.ToString(row["Rol"]), true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strRolDuplicado"), Convert.ToString(row["Rol"]), false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        #region TAREAS

                        CoreEstadosTareasController cEstadosTareas = new CoreEstadosTareasController();
                        CoreTareasAccionesController cAcciones = new CoreTareasAccionesController();
                        CoreTiposInformacionesAccionesController cTiposAcciones = new CoreTiposInformacionesAccionesController();
                        CoreCustomFieldsController cCustomFields = new CoreCustomFieldsController();
                        CoreTiposInformacionesController cTiposInfo = new CoreTiposInformacionesController();
                        CoreAtributosConfiguracionesController cAtributos = new CoreAtributosConfiguracionesController();
                        CoreWorkflowsInformacionesController cWorkflowsInfo = new CoreWorkflowsInformacionesController();

                        for (int k = 0; k < tablas[5].Rows.Count; k++)
                        {
                            Data.CoreEstadosTareas oEstTarea = new Data.CoreEstadosTareas();
                            DataRow row = tablas[5].Rows[k];
                            string sInfo = "";

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstTarea.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            oEstTarea.Obligatorio = Convert.ToBoolean(row[sObligatorio]);
                            oEstTarea.Descripcion = Convert.ToString(row[sDescripcion]);
                            sInfo = Convert.ToString(row[sInformacion]);

                            if (row[sInformacion] == null && DBNull.Value.Equals(row[sInformacion]) &&
                                row[sWorkflow] == null && DBNull.Value.Equals(row[sWorkflow]))
                            {
                                sWorkflow = Convert.ToString(row[sWorkflow]);
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sInfo, false, Convert.ToString(row[sEstado]));
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sWorkflow, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                CoreTiposInformaciones oTipoInfo = cTiposInfo.GetInformacion(Convert.ToString(row[sInformacion]));

                                if (oTipoInfo != null)
                                {
                                    long lWflowID = cWorkflows.getObjetoByName(Convert.ToString(row[sWorkflow]));
                                    CoreWorkflowsInformaciones oWorkflowInfo = cWorkflowsInfo.getObjetoByID(oTipoInfo.CoreTipoInformacionID, lWflowID);

                                    if (oWorkflowInfo != null)
                                    {
                                        oEstTarea.CoreWorkflowInformacionID = oWorkflowInfo.CoreWorkflowInformacionID;
                                    }
                                }
                                else
                                {
                                    CoreAtributosConfiguraciones oAtributo = cAtributos.GetAtributoByCodigo(Convert.ToString(row[sInformacion]));

                                    if (oAtributo != null)
                                    {
                                        CoreCustomFields oCustom = cCustomFields.getCustomByAtributo(oAtributo.CoreAtributoConfiguracionID);

                                        if (oCustom != null)
                                        {
                                            CoreWorkflowsInformaciones oWorkInfo = cWorkflowsInfo.GetRelacion(lWorkflowID, oCustom.CoreCustomFieldID);

                                            if (oWorkInfo != null)
                                            {
                                                oEstTarea.CoreWorkflowInformacionID = oWorkInfo.CoreWorkflowInformacionID;
                                            }
                                        }
                                    }
                                }
                            }

                            if (row[sAccion] == null && DBNull.Value.Equals(row[sAccion]))
                            {
                                sAccion = Convert.ToString(row[sAccion]);
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sAccion, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                CoreTareasAcciones oAccion = cAcciones.getObjetoByCodigo(Convert.ToString(row[sAccion]));

                                if (oAccion != null)
                                {
                                    long lTipoID = cWorkflowsInfo.GetItem(oEstTarea.CoreWorkflowInformacionID).CoreTipoInformacionID;
                                    CoreTiposInformacionesAcciones oTipoAccion = cTiposAcciones.getIDByParametros(oAccion.CoreTareaAccionID, lTipoID);

                                    if (oTipoAccion != null)
                                    {
                                        oEstTarea.CoreTipoInformacionAccionID = oTipoAccion.CoreTipoInformacionAccionID;
                                    }
                                }
                            }

                            if (!cEstadosTareas.RegistroDuplicado(oEstTarea))
                            {
                                cEstadosTareas.AddItem(oEstTarea);
                                iTareaAñadida++;

                                oDato = cargarMensajes(GetGlobalResource("strTareaAñadida"), sInfo, true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strTareaDuplicada"), sInfo, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        #region NOTIFICACIONES 

                        CoreEstadosNotificacionesController cEstadosNotificaciones = new CoreEstadosNotificacionesController();
                        CoreEstadosNotificacionesRolesController cNotificacionesRoles = new CoreEstadosNotificacionesRolesController();
                        CoreEstadosNotificacionesUsuariosController cNotificacionesUsuarios = new CoreEstadosNotificacionesUsuariosController();
                        UsuariosController cUsuarios = new UsuariosController();

                        for (int k = 0; k < tablas[6].Rows.Count; k++)
                        {
                            Data.CoreEstadosNotificaciones oEstNot = new Data.CoreEstadosNotificaciones();
                            Data.CoreEstadosNotificacionesRoles oEstNotRol = new Data.CoreEstadosNotificacionesRoles();
                            Data.CoreEstadosNotificacionesUsuarios oEstNotUser = new Data.CoreEstadosNotificacionesUsuarios();

                            DataRow row = tablas[6].Rows[k];

                            if (row[sEstado] != null && !DBNull.Value.Equals(row[sEstado]))
                            {
                                oEstNot.CoreEstadoID = (long)cEstados.GetEstadoIDByWorkflow(row[sEstado].ToString(), lWorkflowID);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sEstado, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row["Rol"] != null && !DBNull.Value.Equals(row["Rol"]))
                            {
                                oEstNotRol.RolID = cRoles.getIDByCodigo(row["Rol"].ToString());
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), "Rol", false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            if (row[sUsuario] != null && !DBNull.Value.Equals(row[sUsuario]))
                            {
                                oEstNotUser.UsuarioID = cUsuarios.getUsuarioByEmail(row[sUsuario].ToString()).UsuarioID;
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNoEncuentra"), sUsuario, false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }

                            oEstNot.Contenido = row[sMensaje].ToString();
                            oEstNot.Asunto = "";

                            if (!cEstadosNotificaciones.RegistroDuplicado((long)oEstNot.CoreEstadoID, oEstNot.Contenido))
                            {
                                cEstadosNotificaciones.AddItem(oEstNot);
                                iNotificacionAñadido++;

                                oEstNotRol.CoreEstadoNotificacionID = oEstNot.CoreEstadoNotificacionID;
                                cNotificacionesRoles.AddItem(oEstNotRol);

                                oEstNotUser.CoreEstadoNotificacionID = oEstNot.CoreEstadoNotificacionID;
                                cNotificacionesUsuarios.AddItem(oEstNotUser);

                                oDato = cargarMensajes(GetGlobalResource("strNotificacionAñadida"), Convert.ToString(row[sMensaje]), true, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                            else
                            {
                                oDato = cargarMensajes(GetGlobalResource("strNotificacionDuplicada"), Convert.ToString(row[sMensaje]), false, Convert.ToString(row[sEstado]));
                                listaMensajes.Add(oDato);
                            }
                        }

                        #endregion

                        if (sEstadosRepetidos.Equals(""))
                        {
                            log.Info(GetGlobalResource("strSubidaCorrecta"));
                        }
                        else
                        {
                            log.Error(GetGlobalResource("strSubidaFallo"));

                            oDato = cargarMensajes(GetGlobalResource("strEstadosDuplicados"), sEstadosRepetidos, false, "Resumen");
                            listaMensajes.Add(oDato);
                        }

                        lista = Newtonsoft.Json.JsonConvert.SerializeObject(listaMensajes);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(GetGlobalResource("strImportarFlujo"));
                log.Error(GetGlobalResource("strErrorImportacion"));

                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
            }

            storeCoreEstados.DataBind();
            FileUploadImportar.PostedFile.InputStream.Dispose();
            FileUploadImportar.Clear();

            direct.Success = true;
            direct.Result = lista;

            return direct;
        }

        #endregion

        #region DIRECT METHOD ESTADOS GLOBALES

        [DirectMethod()]
        public DirectResponse AgregarEstadoGlobal()
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse oResponse;
            CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();

            try
            {
                Data.CoreEstadosGlobales oDato = new Data.CoreEstadosGlobales();

                if (hdEstadoID.Value.ToString() != "")
                {
                    oDato.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());
                }
                else
                {
                    oDato.CoreEstadoID = 0;
                }

                if (cmbObject.SelectedItem != null && cmbObject.SelectedItem.Value != null && cmbObject.SelectedItem.Value != "")
                {
                    if (cmbEstadosGlobales.SelectedItem != null && cmbEstadosGlobales.SelectedItem.Value != null && cmbEstadosGlobales.SelectedItem.Value != "")
                    {
                        oDato.ObjetoEstadoID = long.Parse(cmbEstadosGlobales.SelectedItem.Value);
                        oDato.CoreObjetoNegocioTipoID = long.Parse(cmbObject.SelectedItem.Value);
                    }
                }
                else
                {
                    oDato.ObjetoEstadoID = null;
                    oDato.CoreObjetoNegocioTipoID = null;
                }

                oResponse = cEstados.Add(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse EliminarEstadoGlobal(long lEstadoGlobalID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();
            InfoResponse oResponse;

            try
            {
                Data.CoreEstadosGlobales oDato = cEstados.GetItem(lEstadoGlobalID);
                oResponse = cEstados.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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

        [DirectMethod]
        public DirectResponse MostrarEstadosGlobalesAsignados(long lEstadoID)
        {
            DirectResponse direct = new DirectResponse();

            try
            {
                Data.CoreObjetosNegocioTipos oNegocioTipo;
                Object oEstadoGlob;
                DataTable oEstado;
                int i = 1;
                List<Object> listaEstadosGlob = new List<object>();

                CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();

                CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                TablasModeloDatosController cTablas = new TablasModeloDatosController();
                cObjetos.SetDataContext(cEstados.Context);
                cTablas.SetDataContext(cEstados.Context);

                Store store = this.GridEstadosGlobalesAsig.GetStore();

                if (store != null)
                {
                    List<Data.CoreEstadosGlobales> lista = cEstados.getCoreEstadosGlobales(lEstadoID);

                    if (lista != null)
                    {
                        foreach (Data.CoreEstadosGlobales oEstGlobal in lista)
                        {
                            oNegocioTipo = cObjetos.GetItem((long)oEstGlobal.CoreObjetoNegocioTipoID);

                            if (oNegocioTipo != null)
                            {
                                oEstado = cObjetos.getEstadoByID(oEstGlobal.CoreObjetoNegocioTipoID, oEstGlobal.ObjetoEstadoID);

                                foreach (DataRow item in oEstado.Rows)
                                {
                                    oEstadoGlob = new
                                    {
                                        ID = i,
                                        CoreEstadoGlobalID = oEstGlobal.CoreEstadoGlobalID,
                                        Codigo = oNegocioTipo.Codigo,
                                        Estado = item[0].ToString(),
                                    };

                                    listaEstadosGlob.Add(oEstadoGlob);
                                }
                            }
                            i++;
                        }

                        store.DataSource = listaEstadosGlob;
                        store.DataBind();
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

        #region DIRECT METHOD ESTADOS SIGUIENTES

        [DirectMethod()]
        public DirectResponse AgregarEstadoSiguiente()
        {
            DirectResponse direct = new DirectResponse();
            EstadosSiguientesController cEstados = new EstadosSiguientesController();
            InfoResponse oResponse;

            try
            {
                List<Data.CoreEstadosSiguientes> listaEstSig = cEstados.getEstadosSiguientesByEstadoID(long.Parse(hdEstadoID.Value.ToString()));
                Data.CoreEstadosSiguientes oDato = new Data.CoreEstadosSiguientes();

                if (hdEstadoID.Value.ToString() != "")
                {
                    oDato.CoreEstadoID = long.Parse(hdEstadoID.Value.ToString());
                }
                else
                {
                    oDato.CoreEstadoID = 0;
                }

                if (cmbName.SelectedItem != null && cmbName.SelectedItem.Value != null && cmbName.SelectedItem.Value != "")
                {
                    oDato.CoreEstadoPosibleID = long.Parse(cmbName.SelectedItem.Value.ToString());
                }
                else
                {
                    oDato.CoreEstadoPosibleID = 0;
                }

                if (listaEstSig.Count == 0)
                {
                    oDato.Defecto = true;
                }
                else
                {
                    oDato.Defecto = false;
                }

                oResponse = cEstados.Add(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse AsignarDefectoEstadoSiguiente(long lEstadoSiguienteID)
        {
            DirectResponse direct = new DirectResponse();
            EstadosSiguientesController cEstados = new EstadosSiguientesController();
            EstadosController cEst = new EstadosController();
            cEst.SetDataContext(cEstados.Context);
            InfoResponse oResponse;

            try
            {
                Data.CoreEstadosSiguientes oDato = cEstados.GetEstadoSiguiente(long.Parse(hdEstadoID.Value.ToString()), lEstadoSiguienteID);
                Data.CoreEstados oEstado = cEst.GetItem(long.Parse(hdEstadoID.Value.ToString()));
                oResponse = cEstados.SetDefecto(oEstado, oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse EliminarEstadoSiguiente(long lEstadoID)
        {
            DirectResponse direct = new DirectResponse();
            EstadosSiguientesController cEstados = new EstadosSiguientesController();
            InfoResponse oResponse;

            try
            {
                Data.CoreEstadosSiguientes oDato = cEstados.GetItem(lEstadoID);
                oResponse = cEstados.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cEstados.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cEstados.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cEstados.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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

        [DirectMethod]
        public DirectResponse MostrarEstadosSiguientesAsignados(long lEstadoID)
        {
            DirectResponse direct = new DirectResponse();
            EstadosSiguientesController cController = new EstadosSiguientesController();

            try
            {
                Store store = this.GridEstadosSiguientesAsig.GetStore();
                if (store != null)
                {
                    List<Data.CoreEstados> lista = cController.GetByEstadoID(lEstadoID);

                    if (lista != null)
                    {
                        store.DataSource = lista;
                        store.DataBind();
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

        #region DIRECT METHOD ROLES

        [DirectMethod()]
        public DirectResponse AgregarTareasEstadosAsignados(long rolID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosRolesEscriturasController cController = new CoreEstadosRolesEscriturasController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(hdEstadoID.Value.ToString());

                Data.CoreEstadosRolesEscrituras dato = new Data.CoreEstadosRolesEscrituras();
                dato.CoreEstadoID = lID;
                dato.RolID = rolID;
                oResponse = cController.Add(dato);
                storeTareasEstadosAsignados.Reload();

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storeTareasEstadosAsignados.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                //else
                //{
                //    cController.DiscardChanges();
                //    direct.Success = false;
                //    direct.Result = GetGlobalResource(oResponse.Description);
                //}
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
        public DirectResponse EliminarTareasEstadosAsignados(long rolID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosRolesEscriturasController cController = new CoreEstadosRolesEscriturasController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(hdEstadoID.Value.ToString());

                Data.CoreEstadosRolesEscrituras dato = cController.GetBuscado(lID, rolID);

                oResponse = cController.Delete(dato);
                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeTareasEstadosAsignados.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
                storeTareasEstadosAsignados.Reload();

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
        public DirectResponse AgregarTareasEstadosSeguimiento(long rolID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosRolesLecturaController cController = new CoreEstadosRolesLecturaController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(hdEstadoID.Value.ToString());


                Data.CoreEstadosRolesLectura dato = new Data.CoreEstadosRolesLectura();
                dato.CoreEstadoID = lID;
                dato.RolID = rolID;
                oResponse = cController.Add(dato);

                storeRolesEstadosSeguimiento.Reload();

                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storeRolesEstadosSeguimiento.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                //else
                //{
                //    cController.DiscardChanges();
                //    direct.Success = false;
                //    direct.Result = GetGlobalResource(oResponse.Description);
                //}

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
        public DirectResponse EliminarTareasEstadosSeguimiento(long rolID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosRolesLecturaController cController = new CoreEstadosRolesLecturaController();
            InfoResponse oResponse;

            try
            {
                long lID = long.Parse(hdEstadoID.Value.ToString());

                Data.CoreEstadosRolesLectura dato = cController.GetBuscado(lID, rolID);

                oResponse = cController.Delete(dato);
                if (oResponse.Result)
                {
                    oResponse = cController.SubmitChanges();
                    if (oResponse.Result)
                    {
                        log.Warn(GetGlobalResource(Comun.LogEliminacionRealizada));
                        storeRolesEstadosSeguimiento.DataBind();

                        direct.Success = true;
                        direct.Result = "";
                    }
                    else
                    {
                        cController.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cController.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
                storeRolesEstadosSeguimiento.Reload();

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
        public DirectResponse CambiarRolPublicoPrivadoEscritura(string Valor)
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstado = new EstadosController();

            Data.CoreEstados oDato = new Data.CoreEstados();

            oDato = cEstado.GetItem(long.Parse(hdEstadoID.Value.ToString()));

            if (Valor == "Publico")
            {
                oDato.PublicoEscritura = true;
            }
            else
            {
                oDato.PublicoEscritura = false;
            }

            try
            {
                if (cEstado.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

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
        public DirectResponse CambiarRolPublicoPrivadoLectura(string Valor)
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstado = new EstadosController();

            Data.CoreEstados oDato = new Data.CoreEstados();

            oDato = cEstado.GetItem(long.Parse(hdEstadoID.Value.ToString()));

            if (Valor == "Publico")
            {
                oDato.PublicoLectura = true;
            }
            else
            {
                oDato.PublicoLectura = false;
            }

            try
            {
                if (cEstado.UpdateItem(oDato))
                {
                    log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    direct.Success = true;
                    direct.Result = "";
                }

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

        #endregion

        #region DIRECT METHOD TAREAS

        [DirectMethod()]
        public DirectResponse AgregarEstadoTarea()
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse oResponse;
            CoreEstadosTareasController cTareas = new CoreEstadosTareasController();

            try
            {
                long lEstadoID = long.Parse(hdEstadoID.Value.ToString());
                long lWorkflowID = long.Parse(cmbWorkflows.SelectedItem.Value.ToString());
                long lAccionID = long.Parse(cmbTareasAcciones.SelectedItem.Value.ToString());
                string sInfo = cmbInformaciones.SelectedItem.Value.ToString();

                oResponse = cTareas.AddEstadoTarea(lEstadoID, lWorkflowID, lAccionID, sInfo, chkMandat.Checked, txtDescripcionTarea.Text);

                if (oResponse.Result)
                {
                    oResponse = cTareas.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        cTareas.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cTareas.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse EliminarEstadoTarea(long lEstadoTareaID)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosTareasController cTareas = new CoreEstadosTareasController();
            InfoResponse oResponse;

            try
            {
                Data.CoreEstadosTareas oDato = cTareas.GetItem(lEstadoTareaID);
                oResponse = cTareas.Delete(oDato);

                if (oResponse.Result)
                {
                    oResponse = cTareas.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                    else
                    {
                        cTareas.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cTareas.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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

        #endregion

        #region DIRECT METHOD NOTIFICACIONES

        [DirectMethod()]
        public DirectResponse AgregarNotificacion(List<string> listaRolesIDs, List<string> listaUsuariosIDs)
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse oResponse;
            CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();

            try
            {
                long lID = long.Parse(hdEstadoID.Value.ToString());
                Data.CoreEstadosNotificaciones oDato = new Data.CoreEstadosNotificaciones();

                if (hdEstadoID.Value.ToString() != "")
                {
                    oDato.CoreEstadoID = lID;
                }
                else
                {
                    oDato.CoreEstadoID = 0;
                }

                oDato.Contenido = txtMensaje.Text;
                oDato.Asunto = "";

                oResponse = cNot.AddEstadoNotificacion(oDato, listaRolesIDs, listaUsuariosIDs);

                if (oResponse.Result)
                {
                    oResponse = cNot.SubmitChanges();

                    if (oResponse.Result)
                    {
                        direct.Success = true;
                        direct.Result = GetGlobalResource(oResponse.Description);
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        cNot.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
                    }
                }
                else
                {
                    cNot.DiscardChanges();
                    direct.Success = false;
                    direct.Result = GetGlobalResource(oResponse.Description);
                }
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
        public DirectResponse EliminarNotificacion(long lNotificacionID)
        {
            DirectResponse direct = new DirectResponse();
            InfoResponse oResponse;
            CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();

            try
            {
                CoreEstadosNotificaciones oNotificacion = cNot.GetItem(lNotificacionID);

                if (oNotificacion != null)
                {
                    oResponse = cNot.Delete(oNotificacion);

                    if (oResponse.Result)
                    {
                        oResponse = cNot.SubmitChanges();

                        if (oResponse.Result)
                        {
                            direct.Success = true;
                            direct.Result = GetGlobalResource(oResponse.Description);
                            log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                            storeCoreEstadosNotificaciones.Reload();
                        }
                        else
                        {
                            cNot.DiscardChanges();
                            direct.Success = false;
                            direct.Result = GetGlobalResource(oResponse.Description);
                        }

                    }
                    else
                    {
                        cNot.DiscardChanges();
                        direct.Success = false;
                        direct.Result = GetGlobalResource(oResponse.Description);
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

            return direct;
        }

        #endregion

        #region FUNCTIONS

        [DirectMethod()]
        public DirectResponse RecargarPanelLateral(string sPanel)
        {
            DirectResponse direct = new DirectResponse();
            CoreEstadosGlobalesController cEstados = new CoreEstadosGlobalesController();
            CoreEstadosRolesController cRoles = new CoreEstadosRolesController();
            EstadosController cEst = new EstadosController();
            RolesController cRols = new RolesController();
            UsuariosController cUsers = new UsuariosController();
            CoreEstadosNotificacionesController cNot = new CoreEstadosNotificacionesController();
            CoreCustomFieldsController cAtributosController = new CoreCustomFieldsController();
            CoreTiposInformacionesController cTipoController = new CoreTiposInformacionesController();
            CoreTareasAccionesController cAccionesController = new CoreTareasAccionesController();
            CoreEstadosNotificacionesRolesController cNotRoles = new CoreEstadosNotificacionesRolesController();
            CoreEstadosNotificacionesUsuariosController cNotUsers = new CoreEstadosNotificacionesUsuariosController();
            CoreTiposInformacionesAccionesController cTipoAccionesController = new CoreTiposInformacionesAccionesController();
            var lista = "";
            List<Object> listaObjetos = new List<Object>();
            Object oJson;

            try
            {
                switch (sPanel)
                {
                    case "pnEstadosGlobales":

                        List<Data.CoreEstadosGlobales> listaDatos;
                        Data.CoreObjetosNegocioTipos oNegocioTipo;
                        Object oEstadoGlob;
                        DataTable oEstado;
                        int i = 1;
                        string sNombreTabla = "";

                        CoreObjetosNegocioTiposController cObjetos = new CoreObjetosNegocioTiposController();
                        TablasModeloDatosController cTablas = new TablasModeloDatosController();

                        listaDatos = cEstados.getCoreEstadosGlobales(long.Parse(hdEstadoID.Value.ToString()));

                        if (listaDatos != null)
                        {
                            foreach (Data.CoreEstadosGlobales oDato in listaDatos)
                            {
                                oNegocioTipo = cObjetos.GetItem((long)oDato.CoreObjetoNegocioTipoID);

                                if (oNegocioTipo != null)
                                {
                                    sNombreTabla = cTablas.getClaveByID(oNegocioTipo.TablaModeloDatoID);
                                    oEstado = cObjetos.getEstadoByID(oDato.CoreObjetoNegocioTipoID, oDato.ObjetoEstadoID);

                                    foreach (DataRow item in oEstado.Rows)
                                    {
                                        oEstadoGlob = new
                                        {
                                            ID = i,
                                            CoreEstadoGlobalID = oDato.CoreEstadoGlobalID,
                                            Tabla = GetGlobalResource(sNombreTabla),
                                            Estado = item[0].ToString(),
                                        };

                                        listaObjetos.Add(oEstadoGlob);
                                    }
                                }
                                i++;
                            }
                        }

                        break;

                    case "pnEstadosSiguientes":

                        List<Data.CoreEstadosSiguientes> listaEst;

                        listaEst = cEst.getEstadosSiguientes(long.Parse(hdEstadoID.Value.ToString()));

                        if (listaEst != null)
                        {
                            foreach (Data.CoreEstadosSiguientes oDato in listaEst)
                            {
                                Data.CoreEstados oValor = cEst.GetItem(oDato.CoreEstadoPosibleID);

                                oJson = new
                                {
                                    NombreEstado = oValor.Nombre,
                                    CodigoEstado = oValor.Codigo,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnTareas":

                        List<Data.CoreEstadosTareas> listaEstadosTareas;
                        Object oTarea;
                        string sClaveInfo = "";

                        listaEstadosTareas = ListaTareasEstados();

                        if (listaEstadosTareas != null)
                        {
                            foreach (Data.CoreEstadosTareas estadoTarea in listaEstadosTareas)
                            {
                                CoreTiposInformacionesAcciones oTipoAccion = cTipoAccionesController.GetItem(estadoTarea.CoreTipoInformacionAccionID);
                                CoreTiposInformaciones oTipoInfo = cTipoController.GetItem(oTipoAccion.CoreTipoInformacionID);

                                if (oTipoInfo != null)
                                {
                                    if (oTipoInfo.Codigo == "CUSTOMFIELD")
                                    {
                                        CoreAtributosConfiguraciones oAtributo = cAtributosController.getAtributoByCodigo((long)estadoTarea.CoreWorkflowsInformaciones.CoreCustomFieldID);
                                        if (oAtributo != null)
                                        {
                                            sClaveInfo = oAtributo.Codigo;
                                        }
                                    }
                                    else
                                    {
                                        sClaveInfo = GetGlobalResource(oTipoInfo.ClaveRecurso);
                                    }
                                    CoreTareasAcciones oTareaAccion = cAccionesController.GetItem(oTipoAccion.CoreTareaAccionID);

                                    if (oTareaAccion != null)
                                    {
                                        string sClaveAccion = GetGlobalResource(oTareaAccion.ClaveRecurso);

                                        oTarea = new
                                        {
                                            CoreEstadoTareaID = estadoTarea.CoreEstadoTareaID,
                                            Informacion = sClaveInfo,
                                            Accion = sClaveAccion,
                                            Obligatorio = estadoTarea.Obligatorio,
                                            Descripcion = estadoTarea.Descripcion
                                        };

                                        listaObjetos.Add(oTarea);
                                    }
                                }
                            }
                        }

                        break;
                    case "pnRoles":

                        List<Data.Vw_CoreEstadosRoles> listaRoles;

                        listaRoles = cRoles.getRolesByEstadoID(long.Parse(hdEstadoID.Value.ToString()));

                        if (listaRoles != null)
                        {
                            foreach (Data.Vw_CoreEstadosRoles oDato in listaRoles)
                            {
                                oJson = new
                                {
                                    NombreRol = oDato.CodigoRol,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;
                    case "pnNotificaciones":

                        List<Data.CoreEstadosNotificaciones> listaNot;

                        listaNot = cNot.getNotificacionesByEstado(long.Parse(hdEstadoID.Value.ToString()));

                        if (listaNot != null)
                        {
                            foreach (Data.CoreEstadosNotificaciones oNot in listaNot)
                            {
                                List<long> listaUser = cNotUsers.getUserByNotificacionID(oNot.CoreEstadoNotificacionID);
                                string sUser = "";

                                foreach (long lUser in listaUser)
                                {
                                    string sUsuarios = cUsers.GetItem(lUser).EMail;

                                    if (sUser == "")
                                    {
                                        sUser = sUsuarios;
                                    }
                                    else
                                    {
                                        sUser = sUser + ", " + sUsuarios;
                                    }
                                }

                                List<long> listaRols = cNotRoles.getRolByNotificacionID(oNot.CoreEstadoNotificacionID);
                                string sRole = "";

                                foreach (long lRol in listaRols)
                                {
                                    string sRol = cRols.GetItem(lRol).Codigo;

                                    if (sRole == "")
                                    {
                                        sRole = sRol;
                                    }
                                    else
                                    {
                                        sRole = sRole + ", " + sRol;
                                    }
                                }

                                oJson = new
                                {
                                    User = sUser,
                                    Rol = sRole,
                                    Contenido = oNot.Contenido,
                                };

                                listaObjetos.Add(oJson);
                            }
                        }

                        break;

                }

                lista = Newtonsoft.Json.JsonConvert.SerializeObject(listaObjetos);
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = lista;

            return direct;
        }

        [DirectMethod()]
        public Object cargarMensajes(string sMensaje, string sValor, bool bCorrecto, string sEstado)
        {
            Object oJSON;

            oJSON = new
            {
                Estado = sEstado,
                Mensaje = sMensaje,
                Valor = sValor,
                Correcto = bCorrecto
            };

            return oJSON;
        }

        [DirectMethod()]
        public DirectResponse ComprobarEstadoExiste()
        {
            DirectResponse direct = new DirectResponse();
            EstadosController cEstados = new EstadosController();

            try
            {
                if (hdEstadoID.Value != null && hdEstadoID.Value.ToString() != "")
                {
                    Data.CoreEstados oEst = cEstados.GetItem(long.Parse(hdEstadoID.Value.ToString()));

                    if (oEst != null)
                    {
                        string sCodigo = oEst.Codigo;
                        string sNombre = oEst.Nombre;
                        long? lWorkflow = oEst.CoreWorkFlowID;

                        if (sCodigo == txtCodigo.Text && sNombre == txtNombre.Text && lWorkflow == long.Parse(cmbWorkflows.SelectedItem.Value))
                        {
                            direct.Success = false;
                            direct.Result = "Editado";
                        }
                        else
                        {
                            if (cEstados.RegistroDuplicadoForm(txtCodigo.Text, txtNombre.Text, long.Parse(cmbWorkflows.SelectedItem.Value)))
                            {
                                direct.Success = false;
                                direct.Result = "Codigo";
                                return direct;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = "";
                log.Error(ex.Message);
                return direct;
            }

            return direct;
        }

        #endregion
    }
}
