using System;
using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SqlClient;

namespace TreeCore.ModCalidad.pages
{
    public partial class CalidadDQDataSources : TreeCore.Page.BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Data.Vw_Funcionalidades> listaFuncionalidades = new List<Data.Vw_Funcionalidades>();


        #region GESTIÓN DE PÁGINA

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
                        List<JsonObject> listaDatos;
                        string sOrden = Request.QueryString["orden"];
                        string sDir = Request.QueryString["dir"];
                        string sFiltro = Request.QueryString["filtro"];
                        int iCount = 0;

                        listaDatos = ListaPrincipal(0, 0, sOrden, sDir, ref iCount, sFiltro);

                        #region ESTADISTICAS
                        try
                        {
                            Comun.ExportacionDesdeListaNombre(grid.ColumnModel, listaDatos, Response, "", GetGlobalResource(Comun.jsFuenteDatos).ToString(), _Locale);
                            log.Info(GetGlobalResource(Comun.LogExcelExportado));
                            EstadisticasController cEstadisticas = new EstadisticasController();
                            //cEstadisticas.registrarDescargaExcel(Usuario.UsuarioID, Convert.ToInt32(Comun.Modulos.GLOBAL), Request.Url.Segments[Request.Url.Segments.Length - 1], Comun.EXCEL, _Locale);
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
            { "Put", new List<ComponentBase> { btnEditar, btnActivar }},
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

                    var lista = ListaPrincipal(e.Start, e.Limit, sSort, sDir, ref iCount, sFiltro);


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

        private List<JsonObject> ListaPrincipal(int iStart, int iLimit, string sSort, string sDir, ref int iCount, string sFiltro)
        {
            List<Data.Vw_DQTablasPaginas> listaDatos;
            DQTablasPaginasController CDQTablasPaginas = new DQTablasPaginasController();
            JsonObject obj;
            List<JsonObject> Principal = new List<JsonObject>();

            try
            {

                listaDatos = CDQTablasPaginas.GetItemsWithExtNetFilterList<Data.Vw_DQTablasPaginas>(sFiltro, sSort, sDir, iStart, iLimit, ref iCount);
                if (listaDatos != null)
                {
                    foreach (var item in listaDatos)
                    {
                        obj = new JsonObject();
                        obj.Add("DQTablaPaginaID", item.DQTablaPaginaID);
                        obj.Add("NombreTabla", item.NombreTabla);
                        obj.Add("ClaveRecurso", item.ClaveRecurso);
                        obj.Add("Alias", item.Alias);
                        obj.Add("Activo", item.Activo);
                        obj.Add("ClaveRecursoTabla", (GetGlobalResource(item.ClaveRecurso) == "") ? item.ClaveRecurso : GetGlobalResource(item.ClaveRecurso));

                        Principal.Add(obj);

                    }
                }

            }
            catch (Exception ex)
            {
                listaDatos = null;
                log.Error(ex.Message);
            }

            return Principal;
        }

        #endregion

        #region TABLAS

        protected void storeTablas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    JsonObject obj;
                    List<JsonObject> Tablas = new List<JsonObject>();
                    var lista = ListaTablas();

                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            obj = new JsonObject();
                            obj.Add("TablaModeloDatosID", item.TablaModeloDatosID);
                            obj.Add("Activo", item.Activo);
                            obj.Add("NombreTabla", item.NombreTabla);
                            obj.Add("ClaveRecurso", item.ClaveRecurso);
                            obj.Add("Controlador", item.Controlador);
                            obj.Add("Indice", item.Indice);
                            obj.Add("ModuloID", item.ModuloID);
                            obj.Add("NombreTablaTraducido", (GetGlobalResource(item.ClaveRecurso) == "") ? item.ClaveRecurso : GetGlobalResource(item.ClaveRecurso));


                            Tablas.Add(obj);
                        }
                        if (Tablas != null)
                        {
                            storeTablas.DataSource = Tablas;
                            storeTablas.DataBind();
                        }

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

        private List<Data.TablasModeloDatos> ListaTablas()
        {
            List<Data.TablasModeloDatos> listaDatos;
            TablasModeloDatosController CDQTablasPaginas = new TablasModeloDatosController();

            try
            {

                listaDatos = CDQTablasPaginas.GetTablasModulos();

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

        #region DIRECT METHODS



        [DirectMethod()]
        public DirectResponse AgregarEditar(bool bAgregar)
        {
            DirectResponse direct = new DirectResponse();

            DQTablasPaginasController cControl = new DQTablasPaginasController();

            bool duplicado = false;

            try
            {
                if (!bAgregar)
                {
                    long lS = long.Parse(GridRowSelect.SelectedRecordID);
                    Data.DQTablasPaginas oDato;
                    oDato = cControl.GetItem(lS);

                    if (oDato.Alias == txtAlias.Text && oDato.TablaModeloDatosID == long.Parse(cmbTablas.Value.ToString()))
                    {
                        if (txtAlias.Text != "")
                        {
                            if (cControl.RegistroDuplicado(txtAlias.Text))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                duplicado = true;
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                return direct;
                            }


                            if (cControl.RegistroDuplicadoTablas(long.Parse(cmbTablas.Value.ToString())))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                duplicado = true;
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                return direct;
                            }

                        }
                        else if (duplicado == false)
                        {
                            if (txtAlias.IsEmpty)
                            {

                                oDato.Alias = "";
                            }
                            else
                            {

                                oDato.Alias = txtAlias.Text;
                            }


                            if (cControl.RegistroDuplicadoTablas(long.Parse(cmbTablas.Value.ToString())))
                            {
                                log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                duplicado = true;
                                direct.Success = false;
                                direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                return direct;

                            }

                            oDato.TablaModeloDatosID = long.Parse(cmbTablas.Value.ToString());
                            oDato.Activo = true;
                        }
                    }
                    else
                    {
                        if (txtAlias.Text != "")
                        {
                            if (oDato.Alias != txtAlias.Text)
                            {
                                if (cControl.RegistroDuplicado(txtAlias.Text))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                    duplicado = true;
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                    return direct;
                                }
                            }

                            
                        }

                        if (duplicado == false)
                        {
                            if (txtAlias.IsEmpty)
                            {

                                oDato.Alias = "";
                            }
                            else
                            {

                                oDato.Alias = txtAlias.Text;
                            }

                            if (oDato.TablaModeloDatosID != long.Parse(cmbTablas.Value.ToString()))
                            {
                                if (cControl.RegistroDuplicadoTablas(long.Parse(cmbTablas.Value.ToString())))
                                {
                                    log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                                    MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                                    duplicado = true;
                                    direct.Success = false;
                                    direct.Result = GetGlobalResource(Comun.jsYaExiste);
                                    return direct;
                                }
                            }

                            oDato.TablaModeloDatosID = long.Parse(cmbTablas.Value.ToString());
                            oDato.Activo = true;
                        }
                    }
                    if (cControl.UpdateItem(oDato))
                    {
                        log.Warn(GetGlobalResource(Comun.LogActualizacionRealizada));
                        storePrincipal.DataBind();
                    }
                }
                else
                {

                    Data.DQTablasPaginas dato = new Data.DQTablasPaginas();

                    if (txtAlias.Text != "")
                    {
                        if (cControl.RegistroDuplicado(txtAlias.Text))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            duplicado = true;
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            return direct;
                        }

                        if (cControl.RegistroDuplicadoTablas(long.Parse(cmbTablas.Value.ToString())))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            duplicado = true;
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            return direct;
                        }

                    }
                    else if (duplicado == false)
                    {
                        if (txtAlias.IsEmpty)
                        {

                            dato.Alias = "";
                        }
                        else
                        {

                            dato.Alias = txtAlias.Text;
                        }

                        if (cControl.RegistroDuplicadoTablas(long.Parse(cmbTablas.Value.ToString())))
                        {
                            log.Warn(GetGlobalResource(Comun.LogRegistroExistente));
                            MensajeBox(GetGlobalResource(Comun.jsInfo), GetGlobalResource(Comun.jsYaExiste), Ext.Net.MessageBox.Icon.INFO, null);
                            duplicado = true;
                            direct.Success = false;
                            direct.Result = GetGlobalResource(Comun.jsYaExiste);
                            return direct;
                        }

                        
                    }

                    dato.TablaModeloDatosID = long.Parse(cmbTablas.Value.ToString());
                    dato.Activo = true;
                    dato.Alias = txtAlias.Text;

                    if (cControl.AddItem(dato) != null)
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                        storePrincipal.DataBind();
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

            DQTablasPaginasController cControl = new DQTablasPaginasController();

            try
            {
                long lS = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DQTablasPaginas dato;
                dato = cControl.GetItem<Data.DQTablasPaginas>(lS);

                if (dato.TablaModeloDatosID != 0)
                {
                    cmbTablas.SetValue(dato.TablaModeloDatosID);
                }
                txtAlias.Text = dato.Alias;

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
        public DirectResponse Eliminar()
        {
            DirectResponse direct = new DirectResponse();
            DQTablasPaginasController cController = new DQTablasPaginasController();

            long lID = long.Parse(GridRowSelect.SelectedRecordID);

            try
            {
                if (cController.DeleteItem(lID))
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
            DQTablasPaginasController cController = new DQTablasPaginasController();

            try
            {
                long lID = long.Parse(GridRowSelect.SelectedRecordID);

                Data.DQTablasPaginas oDato;
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
    }
}